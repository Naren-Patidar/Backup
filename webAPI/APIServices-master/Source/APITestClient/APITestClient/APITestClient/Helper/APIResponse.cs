using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Http;
using System.Net.Http;

using System.Reflection;
using System.Net;
using System.Text;
using System.IO;

namespace APITestClient.Helper
{
    public class APIResponse
    {
        public object data { get; set; }
        public string duration { get; set; }
        public List<KeyValuePair<string, string>> errors { get; set; }
        public string operationname { get; set; }
        public string receivedat { get; set; }
        public bool status { get; set; }
    }

    public class APIResponseHelper
    {
        public Dictionary<string, string> GetHeaders()
        {
            string publicKey = "8D011A65-641C-428F-A2AE-CEF944FD164E";
            string accessToken = "82001E56-3D43-4632-87EA-7C26780E9DAE";
            string timestamp = DateTime.Now.ToLongTimeString();
            string nonce = Guid.NewGuid().ToString("N");
            string normalizedUrl = String.Empty;
            string normalizedRequestParameters = String.Empty;
            
            OAuthBase OAuth = new OAuthBase();
            string hash = OAuth.GenerateSignature(
                            new Uri(ConfigurationManager.AppSettings["APIURL"]),
                            publicKey,
                            accessToken,
                            null, // token
                            null, //token secret
                            "POST",
                            timestamp,
                            nonce,
                            out normalizedUrl,
                            out normalizedRequestParameters
                          );

            Dictionary<string, string> headers = new Dictionary<string, string>() 
                {
                    {"publickey",  publicKey.ToString()},
                    {"oauth.accessToken", accessToken.ToString()},
                    {"nonce", nonce },
                    {"oauth_timestamp", timestamp },
                    {"signature", hash }
                };
            return headers;
        }

        public APIResponse GetAPIResponse(string data, Dictionary<string, string> headers)
        {
            APIRequester apiRequester = new APIRequester();

            var apiResponse = apiRequester.MakeRequest(data, headers);
            APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });
            return apiResponseObj;
        }
    }
}