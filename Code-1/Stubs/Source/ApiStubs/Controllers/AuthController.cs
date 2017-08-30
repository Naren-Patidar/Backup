using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IdentityApiStub.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace IdentityApiStub.Controllers
{
    public class AuthController : BaseApiController
    {
        [HttpGet]
        public object ValidateToken(string access_token)
        {
            object response = null;
            string responseFile = string.Empty;
            Type returnType = null;
            if (!string.IsNullOrEmpty(access_token))
            {
                responseFile = GetDataFileName(new List<string> { string.Format("{0}.{1}", "ValidateToken", access_token) });
                returnType = typeof(ValidationDetails);
            }
            if (File.Exists(responseFile))
            {
                string strAddresses = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strAddresses, returnType);
            }
            return response;
        }

        [HttpPost]
        public object oauth(string v2, string token)
        {
            object response = null;
            string responseFile = string.Empty;
            Type returnType = null;
            TokenDetails tokenDetails = new TokenDetails();
            HttpContent requestContent = Request.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(jsonContent))
            {
                tokenDetails = JsonConvert.DeserializeObject<TokenDetails>(jsonContent);
            }

            if (!string.IsNullOrEmpty(tokenDetails.client_id) && !string.IsNullOrEmpty(tokenDetails.grant_type) && !string.IsNullOrEmpty(tokenDetails.clubcard))
            {
                responseFile = GetDataFileName(new List<string> { string.Format("{0}.{1}", "OAuth", tokenDetails.clubcard)});
                returnType = typeof(OAuthTokenDetails);
            }
            if (File.Exists(responseFile))
            {
                string strClaims = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strClaims, returnType);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}
