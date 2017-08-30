using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.API.ServiceManager;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Models;
using System.Net;
using System.Configuration;
using System.Text;
using System.IO;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Globalization;
using System.Web.Helpers;

namespace Tesco.ClubcardProducts.MCA.API.Controllers
{
    public class MCAController : BaseController
    {
        string _uuid        = String.Empty;
        string _dotcomid    = String.Empty;

        [HttpPost]
        public HttpResponseMessage APIGateway()
        {
            DateTime dtStart = DateTime.UtcNow;
            APIRequest apiRequest = new APIRequest();
            APIResponse response = new APIResponse();
            ServiceUtility sUtil = new ServiceUtility();
            double dTotalTime = 0;

            try
            {
                response.clienttransactionid = base._LogData._clientTransactionID = base._AuditLogData._clientTransactionID = this.GetClientTransactionID();
                response.servedby = sUtil.GetServerIdentifier();
                this.AuthenticateRequest();

                base._LogData.RecordStep("data read started");
                APIRequest request = Request.Content.ReadAsAsync<APIRequest>().Result;
                base._LogData.RecordStep("data read completed");

                if (request == null)
                {
                    throw new APIBadRequestException(APIErrors.E_400_1);
                }

                apiRequest.service = request.service;

                if (String.IsNullOrWhiteSpace(apiRequest.service))
                {
                    throw new APIBadRequestException(APIErrors.E_400_2);
                }

                apiRequest.operation = request.operation;
                if (String.IsNullOrWhiteSpace(apiRequest.operation))
                {
                    throw new APIBadRequestException(APIErrors.E_400_3);
                }

                base._AuditLogData._serviceRequested = apiRequest.service;
                base._AuditLogData._operationRequested = apiRequest.operation;

                response.operationname = String.Format("{0}_{1}", apiRequest.service, apiRequest.operation);

                base._LogData.RecordStep("parameter conversion started");
                apiRequest.parameters = request.parameters;
                base._LogData.RecordStep("parameter conversion completed");

                var valResult = sUtil.ValidateRequest(apiRequest, ref base._LogData);

                base._LogData.RecordStep("Starting Request Execution");

                valResult.Item1.custInfo.dotcomid = this._dotcomid;
                valResult.Item1.custInfo.uuid = this._uuid;
                valResult.Item1.culture = GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.Culture);

                var internalResp = sUtil.ExecuteRequest(valResult.Item2, valResult.Item1);

                response.data = internalResp.data;
                response.servicestats = internalResp.servicestats;
                response.status = internalResp.errors.Count < 1;
                response.errors = internalResp.errors;

                if (response.errors != null && response.errors.Count > 0)
                {
                    base._LogData._overrideLevel = true;
                    if (response.errors[0].Value.ToLower().Contains("activation error denied access"))
                    {
                        throw new APIUnAuthorizedException(APIErrors.E_401_13, response.errors[0].Value);
                    }
                    base._LogData.RecordStep(response.errors.JsonText(), true);
                }

                response.httpstatuscode = HttpStatusCode.OK;

                base._LogData.RecordStep("Finished Request Execution");
            }
            catch (APIException ex)
            {
                response.status = false;
                response.errors.Add(new KeyValuePair<string, string>("ERR-INTERNAL-APIX", ex.Response.ReasonPhrase));
                response.httpstatuscode = ex.Response.StatusCode;

                String logDetails = String.Empty;
                if (ex.Data != null && ex.Data[APIException.KEY_ERR_DATA] != null)
                {
                    logDetails = ex.Data[APIException.KEY_ERR_DATA].ToString();
                }
                base._LogData.RecordStep(String.Format("Error: {0}, StackTrace: {1}", logDetails, ex.ToString()), ex.Response.ReasonPhrase, true);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.errors.Add(new KeyValuePair<string, string>("ERR-INTERNAL", "Internal Server Error"));
                response.httpstatuscode = HttpStatusCode.InternalServerError;
                base._LogData.RecordStep(ex.ToString(), "generic", true);
            }
            finally
            {
                response.identifier = base._LogData._identifier;
                response.receivedat = dtStart.GetEpochTime().ToString();
                dTotalTime = DateTime.UtcNow.Subtract(dtStart).TotalMilliseconds;
                response.overallduration = dTotalTime.ToString();

                base._AuditLogData._overallduration = base._LogData._overallduration = response.overallduration;

                if (dTotalTime > 800)
                {
                    base._LogData._overrideLevel = true;
                }

                base._AuditLogData.RecordStep(String.Format("{0}|{1}|{2}",
                                                response.status,
                                                String.IsNullOrWhiteSpace(response.servicestats) ? String.Empty : response.servicestats.TrimStart('|').TrimEnd('|').Replace("|", "#"),
                                                response.errors.Count > 0 ? JsonConvert.SerializeObject(response.errors.Select(e => e.Key).ToList<string>()) : String.Empty).TrimEnd('|')
                                            );
            }

            string responseText = JsonConvert.SerializeObject(response, Formatting.None,
                                                                new JsonSerializerSettings
                                                                {
                                                                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                                                                });

            base._LogData.CaptureData("api_request_header", this.Request.Headers);
            base._LogData.CaptureData("api_request_body", apiRequest);
            base._LogData.CaptureData("api_response", responseText);

            BaseController._GeneralLoggingService.Submit(base._LogData);
            BaseController._AuditLoggingService.Submit(base._AuditLogData);

            if (response.httpstatuscode == HttpStatusCode.OK && !response.status && response.errors != null && response.errors.Count > 0)
            {
                var errors = new List<KeyValuePair<string, string>>();
                response.errors.ForEach(err => errors.Add(new KeyValuePair<string, string>(err.Key, String.Empty)));
                response.errors = errors;
            }
            return this.Request.CreateResponse(response.httpstatuscode, response);
        }

        [HttpGet]
        [ActionName("APIGateway")]
        public string APIGatewayGet(string appkey)
        {
            ServiceUtility sUtil = new ServiceUtility();
            string sReturn = String.Empty;
            try
            {
                //ServiceUtility.LoadServices();

                if (!String.IsNullOrWhiteSpace(appkey))
                {
                    ClientAuth clientAuth = this.GetClientAuthDetails(appkey);
                    if (clientAuth != null && !String.IsNullOrWhiteSpace(clientAuth.publickey))
                    {
                        return sUtil.GetMetadata(appkey);
                    }
                }

                return sUtil.HealthCheck();
            }
            catch (Exception ex)
            {
                sReturn = ex.ToString();
            }
            return sReturn;
        }

        private void AuthenticateRequest()
        {
            base._LogData.RecordStep("Running all authentication routines");

            this.ValidateApplicationkey();
            this.ValidateOAuthHeader();
            this.ValidateWithEnterprise();

            base._LogData.RecordStep("Finished authentication routines");
        }

        private void ValidateApplicationkey()
        {
            if (GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.ValidateAppKey) != "1")
            {
                base._LogData.RecordStep("Application Key Validation skipped as its disabled.");
                return;
            }

            var headers = this.Request.Headers;

            if (headers.Contains(HeaderKeys.AppKey))
            {
                string publicKey = headers.GetValues(HeaderKeys.AppKey).First().ToUpper();
                ClientAuth clientAuth = this.GetClientAuthDetails(publicKey);

                if (clientAuth == null || String.IsNullOrWhiteSpace(clientAuth.customerid))
                {
                    throw new APIUnAuthorizedException(APIErrors.E_401_2, String.Format("Incorrect Application Key - {0}", publicKey));
                }
                base._AuditLogData._clientID = base._LogData._clientID = clientAuth.customerid;
                base._LogData.RecordStep("Application Key Validation cleared.");
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_1);
            }
        }

        private void ValidateOAuthHeader()
        {
            if (GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.ValidateOAuth) != "1")
            {
                base._LogData.RecordStep("OAuth Validation skipped as its disabled.");
                return;
            }

            base._LogData.RecordStep("Running OAuth Validations..");
            string publicKey = String.Empty, nonce = String.Empty, oauth_timestamp = String.Empty, signature = String.Empty;
            string normalizedUrl = String.Empty;
            string normalizedRequestParameters = String.Empty;

            var headers = this.Request.Headers;

            if (headers.Contains(HeaderKeys.AppKey))
            {
                publicKey = headers.GetValues(HeaderKeys.AppKey).First();
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_1);
            }

            if (headers.Contains(HeaderKeys.Nonce))
            {
                nonce = headers.GetValues(HeaderKeys.Nonce).First();
                
                Guid guidOutput = Guid.NewGuid();
                if (!Guid.TryParse(nonce, out guidOutput))
                {
                    throw new APIUnAuthorizedException(APIErrors.E_401_4, String.Format("Invalid nonce provided - {0}", nonce));
                }
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_3);
            }

            if (headers.Contains(HeaderKeys.TimeStamp))
            {
                oauth_timestamp = headers.GetValues(HeaderKeys.TimeStamp).First();
                this.ValidateOAuthTimeStamp(oauth_timestamp);
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_5);
            }

            if (headers.Contains(HeaderKeys.Signature))
            {
                signature = headers.GetValues(HeaderKeys.Signature).First();
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_6);
            }

            ClientAuth clientAuth = this.GetClientAuthDetails(publicKey);

            if (clientAuth == null || String.IsNullOrWhiteSpace(clientAuth.secretkey))
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_7, String.Format("Unable to locate a registered client with key - {0}", publicKey));
            }

            base._AuditLogData._clientID = base._LogData._clientID = clientAuth.customerid;

            OAuthBase oauth = new OAuthBase();
            string hash = oauth.GenerateSignature(
                            new Uri(this.Request.RequestUri.ToString()),
                            publicKey,
                            clientAuth.secretkey,
                            null, // token
                            null, //token secret
                            "POST",
                            oauth_timestamp,
                            nonce,
                            out normalizedUrl,
                            out normalizedRequestParameters
                          );

            if (hash != signature)
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_8, String.Format("publickey-{0}, timestamp-{1}, nonce-{2}", publicKey, oauth_timestamp, nonce));
            }
            else
            {
                base._LogData.RecordStep("OAuth Validation cleared.");
            }
        }

        private void ValidateWithEnterprise()
        {
            //if (GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.ValidateWithEnterprise) != "1")
            //{
            //    base._LogData.RecordStep("Enterprise Validation skipped as its disabled.");
            //    return;
            //}

            base._LogData.RecordStep("Running Enterprise Validations..");

            var headers = this.Request.Headers;

            string publicKey = String.Empty, identityToken = String.Empty;
            var responseData = String.Empty;

            if (headers.Contains(HeaderKeys.AppKey))
            {
                publicKey = headers.GetValues(HeaderKeys.AppKey).First();
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_1);
            }

            if (headers.Contains(HeaderKeys.IdentityAccessToken))
            {
                identityToken = headers.GetValues(HeaderKeys.IdentityAccessToken).First();
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_10);
            }

            if (headers.Contains(HeaderKeys.IdentityUUID))
            {
                this._uuid = headers.GetValues(HeaderKeys.IdentityUUID).First();
            }
            else
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_11);
            }

            ClientAuth clientAuth = this.GetClientAuthDetails(publicKey);

            if (clientAuth == null || String.IsNullOrWhiteSpace(clientAuth.customerid))
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_2, String.Format("Incorrect Application Key - {0}", publicKey));
            }

            string url = String.Format("{0}?access_token={1}&client_id={2}", 
                GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.IdentityURL), identityToken, clientAuth.clientid);

            try
            {
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
            }
            catch(WebException eWebEx)
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_12, eWebEx.ToString());
            }

            if (!String.IsNullOrWhiteSpace(responseData))
            {
                responseData = responseData.Replace("\\", "").TrimStart('"').TrimEnd('"');

                ValidationDetails validationDetails = new ValidationDetails();

                try
                {
                    validationDetails = JsonConvert.DeserializeObject<ValidationDetails>(responseData);
                }
                catch
                {
                    throw new APIUnAuthorizedException(APIErrors.E_401_14);
                }

                base._LogData.RecordStep("Enterprise Validation Completed.");

                if (!String.Equals(validationDetails.Status, "VALID", StringComparison.InvariantCultureIgnoreCase)) 
                {
                    throw new APIUnAuthorizedException(APIErrors.E_401_15);
                }
                
                if (!String.IsNullOrWhiteSpace(validationDetails.UserId)
                        && validationDetails.UserId.Replace("trn:tesco:uid:uuid:", String.Empty).Equals(this._uuid, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (validationDetails.Claims != null && validationDetails.Claims.Count > 0)
                    {
                        var dxshClaim = validationDetails.Claims.FirstOrDefault(c => c.claimType.ToLower()
                                                                        .Equals("http://schemas.tesco.com/ws/2011/12/identity/claims/dxsh_customer_id")
                                                                       );

                        if (dxshClaim != null)
                        {
                            this._dotcomid = dxshClaim.value;
                        }
                        else
                        {
                            throw new APIUnAuthorizedException(APIErrors.E_401_19);
                        }
                    }
                    else
                    {
                        throw new APIUnAuthorizedException(APIErrors.E_401_18);
                    }
                    base._LogData.RecordStep("Enterprise Validation Cleared.");
                }
                else
                {
                    throw new APIUnAuthorizedException(APIErrors.E_401_16);
                }
            }
            else
            {
                base._LogData.RecordStep("Enterprise Validation Failed. No data received from Enterprise identity service.");
                throw new APIUnAuthorizedException(APIErrors.E_401_17);
            }
        }

        private ClientAuth GetClientAuthDetails(string publicKey)
        {
            OAuthBase oAuthBase = new OAuthBase();

            var clients = GlobalCachingProvider.Instance.GetItem(ServiceUtility.CLIENT_PREFIX);

            if (clients != null && clients is List<ClientAuth>)
            {
                List<ClientAuth> registeredClients = clients as List<ClientAuth>;
                var requestingClient = registeredClients.Where(c => c.publickey.Equals(publicKey, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                return requestingClient;
            }

            return new ClientAuth();
        }

        private void ValidateOAuthTimeStamp(string timestamp)
        {
            if (String.IsNullOrWhiteSpace(timestamp))
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_5);
            }
            double dTimeLimit = 30;
            DateTime dtTimeStamp = DateTime.UtcNow;
            string sTimeLimit = GlobalCachingProvider.Instance.GetAppSetting("oauthrequestnotolderthan");
            double.TryParse(sTimeLimit, out dTimeLimit);

            double dRequestAge = DateTime.UtcNow.Subtract(timestamp.ConvertFromUnixTimestamp()).TotalSeconds;

            if (dRequestAge < 0 || dRequestAge >= dTimeLimit)
            {
                throw new APIUnAuthorizedException(APIErrors.E_401_9, String.Format("Current Request signature is older than threshold. Difference in seconds - {0}", dRequestAge));
            }
        }

        private string GetClientTransactionID()
        {
            var headers = this.Request.Headers;

            if (headers.Contains(HeaderKeys.ClientTransactionID))
            {
                string tid = headers.GetValues(HeaderKeys.ClientTransactionID).First();
                if (!String.IsNullOrWhiteSpace(tid))
                {
                    if (tid.Length > 16)
                    {
                        tid = tid.Substring(0, 16);
                    }
                    return tid.Replace("|", "#");
                }
            }
            return String.Empty;
        }
    }
}