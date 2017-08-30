using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace TestWebAPIWebClient.Models
{
    public class Helper
    {
        #region Properties

        public string _apigatewayUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["apigatewayUrl"];
            }
        }

        private bool _identityEnabled
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["identityenabled"] == "1";
            }
        }

        private string _identityrooturl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["identityrooturl"];
            }
        }

        private string _identityclientid
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["identityclientid"];
            }
        }

        private string _identityusername
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["identityusername"];
            }
        }

        private string _identitypassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["identitypassword"];
            }
        }

        public string _publicKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["publickey"];
            }
        }

        public string _secretKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["secretkey"];
            }
        }

        private string _timestamp
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["timestamp"];
            }
        }

        private string _nonce
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["nonce"];
            }
        }

        private string _testusers 
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["testusers"];
            }
        }

        private string ComponseIdentityRequest(string username, string password)
        {
            return String.Format("{0}/v3/api/auth/oauth/v2/token?grant_type=password&client_id={1}&password={2}&username={3}",
                    this._identityrooturl, this._identityclientid, password, username); 
        }

        #endregion Properties

        public string MakePostRequest(string url, string postData, Dictionary<string, string> headers)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Accept = "application/json";

            if (headers != null && headers.Count > 0)
            {
                foreach (var hv in headers)
                {
                    request.Headers[hv.Key] = hv.Value;
                }
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            StringBuilder sHeaders = new StringBuilder();
            string responseString = String.Empty;

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                response.Headers.AllKeys.ToList().ForEach(h => sHeaders.AppendLine(String.Format("{0}: {1}", h, response.GetResponseHeader(h))));
                sHeaders.AppendLine(String.Format("status: {0}", response.StatusCode));
                sHeaders.AppendLine(String.Format("status-description: {0}", response.StatusDescription));

            }
            catch (Exception ex)
            {
                Exception exBase = ex.GetBaseException();
                if (exBase != null)
                {
                    System.Net.WebException realException = exBase as System.Net.WebException;
                    realException.Response.Headers.AllKeys.ToList().ForEach(h => sHeaders.AppendLine(String.Format("{0}: {1}", h, realException.Response.Headers[h])));
                    sHeaders.AppendLine(String.Format("status: {0}", ((System.Net.HttpWebResponse)((realException).Response)).StatusCode));
                    sHeaders.AppendLine(String.Format("StatusDescription: {0}", ((System.Net.HttpWebResponse)((realException).Response)).StatusDescription));                    
                }
            }

            return String.Format(@"
{0} 
-------------------------------------------------------------
{1}", sHeaders.ToString(), responseString);
        }

        public string MakeGetRequest(string url)
        {
            string responseData = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseData = reader.ReadToEnd();
                    }
                }
            }

            return responseData;
        }
        
        public double GetEpochTime(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public HomeData LoadControlDefaults()
        {
            HomeData hd = new HomeData();

            hd.APIGatewayURL = this._apigatewayUrl;
            hd.UserName = this._identityusername;
            hd.Password = this._identitypassword;

            string dataFilePath = System.Web.HttpContext.Current.Server.MapPath("postdata.json");

            if (File.Exists(dataFilePath))
            {
                hd.RequestBody = File.ReadAllText(dataFilePath);
            }

            string metadata = this.MakeGetRequest(this._apigatewayUrl);
            metadata = metadata.Replace("\\", "").TrimStart('"').TrimEnd('"');

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            hd.gatewayMetadata = JsonConvert.DeserializeObject<GatewayMetadata>(metadata, jsonSettings);

            return hd;
        }

        public LoginClaim LoginToIdentity(string username, string password)
        {
            /* Mock Data
            var testLoginData = new LoginClaim();
            testLoginData = new LoginClaim();
            testLoginData.access_token = "test";
            testLoginData.Claims = new List<Claim>();
            testLoginData.Claims.Add(new Claim()
            {
                claimType = "http://schemas.tesco.com/ws/2011/12/identity/claims/userkey",
                value = "trn:tesco:uid:uuid:e12da3b3-bc5e-42a7-a8cf-71209e5d160b"
            });

            testLoginData = new LoginClaim();
            testLoginData.error = "invalid_grant";
            testLoginData.error_description = "Resource Owner authentication failed";

            string responseData = JsonConvert.SerializeObject(testLoginData);
            */
            string responseData = this.MakeGetRequest(this.ComponseIdentityRequest(username, password));

            responseData = responseData.Replace("\\", "").TrimStart('"').TrimEnd('"');

            if (String.IsNullOrWhiteSpace(responseData))
            {
                throw new Exception("Identity authorization did not yield any response.");
            }
            else
            {
                var loginDetails = JsonConvert.DeserializeObject<LoginClaim>(responseData);
                if (loginDetails == null)
                {
                    throw new Exception("Identity authorization failed to generate a valid login object.");
                }
                else if (String.IsNullOrWhiteSpace(loginDetails.error))
                {
                    if (loginDetails.Claims != null && loginDetails.Claims.Count > 0)
                    {
                        var uuidClaim = loginDetails.Claims.Where(c => c.claimType.ToLower().Equals("http://schemas.tesco.com/ws/2011/12/identity/claims/userkey")).FirstOrDefault();
                        if (uuidClaim != null)
                        {
                            loginDetails.uuid = uuidClaim.value.Replace("trn:tesco:uid:uuid:", String.Empty);
                            return loginDetails;
                        }
                        else
                        {
                            throw new Exception("uuid is null.");
                        }
                    }
                    else
                    {
                        throw new Exception("uuid is null.");
                    }
                }
                else if (String.IsNullOrWhiteSpace(loginDetails.error_description))
                {
                    throw new Exception(String.Format("Unknown error reported by identity auth token generation process. Error code: {0}", loginDetails.error));
                }
                else
                {
                    throw new Exception(String.Format("{0}. Error code: {1}", loginDetails.error, loginDetails.error_description));
                }
            }
        }

        public Tuple<string, string> InvokeAPIGateway(string accesstoken, string uuid, string requestBody, string apigatewayURL)
        {
            string publicKey = this._publicKey;
            string secretKey = this._secretKey;

            string timestamp = this._timestamp;
            if (String.IsNullOrWhiteSpace(this._timestamp))
            {
                timestamp = GetEpochTime(DateTime.UtcNow).ToString();
            }

            string nonce = this._nonce;
            if (String.IsNullOrWhiteSpace(nonce))
            {
                nonce = Guid.NewGuid().ToString();
            }

            string normalizedUrl = String.Empty;
            string normalizedRequestParameters = String.Empty;

            OAuthBase oauth = new OAuthBase();

            string hash = oauth.GenerateSignature(
                        new Uri(this._apigatewayUrl),
                        publicKey,
                        secretKey,
                        null, // totken
                        null, //token secret
                        "POST",
                        timestamp,
                        nonce,
                        out normalizedUrl,
                        out normalizedRequestParameters
                      );

            Dictionary<string, string> headers = new Dictionary<string, string>() 
                {
                    {"appkey",  publicKey},
                    {"nonce", nonce },
                    {"timestamp", timestamp },
                    {"signature", hash },
                    {"X-TransactionID", Guid.NewGuid().ToString()}
                };


            headers.Add("accesstoken", accesstoken);
            headers.Add("uuid", uuid);

            return new Tuple<string, string>(this.MakePostRequest(apigatewayURL, requestBody, headers), hash);
        }

        public TestUser GetTestUser(string custid)
        {
            TestUser tu = new TestUser() { custid = "50014000", username = this._identityusername, password = this._identitypassword };

            if (String.IsNullOrWhiteSpace(custid))
            {
                return tu;
            }

            var testUsers = JsonConvert.DeserializeObject<List<TestUser>>(this._testusers);

            if (testUsers == null)
            {
                return tu;
            }

            tu = testUsers.Where(t => t.custid.ToLower().Equals(custid.ToLower())).FirstOrDefault<TestUser>();

            if (tu == null)
            {
                tu = new TestUser() { custid = "50014000", username = "testing", password = "testing" };
            }

            return tu;
        }
    }

    public class TestUser
    {
        public string custid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}