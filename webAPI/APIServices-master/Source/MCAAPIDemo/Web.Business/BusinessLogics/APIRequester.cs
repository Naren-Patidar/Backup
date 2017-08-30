using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Tesco.ClubcardProducts.WebAPI.ServiceManager;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class APIRequester
    {
        public APIRequester(string url)
        {
            this._url = url;
        }

        public string MakeRequest(string postData)
        {
            string consumerKey = "34A50496-C9FA-464A-87B2-7E8705387830";
            string timestamp = DateTime.Now.ToLongTimeString();
            string nonce = Guid.NewGuid().ToString("N");
            string normalizedUrl = String.Empty;
            string normalizedRequestParameters = String.Empty; ;

            OAuthBase oauth = new OAuthBase();
            string hash = oauth.GenerateSignature(
                        new Uri(this._url),
                        consumerKey,
                        "C8A93057-3A9C-4938-BA71-A2518BF9FC48",
                        null, // totken
                        null, //token secret
                        "POST",
                        timestamp,
                        nonce,
                        out normalizedUrl,
                        out normalizedRequestParameters
                      );

            string outhToken =  HttpContext.Current.Request.Cookies[ParameterNames.OAUTH_TOKEN] != null ?
                                HttpContext.Current.Request.Cookies[ParameterNames.OAUTH_TOKEN].Value : String.Empty;

            Dictionary<string, string> headers = new Dictionary<string, string>() 
                {
                    {"oauth.accesstoken", outhToken},
                    {"publickey",  consumerKey},
                    {"nonce", nonce },
                    {"oauth_timestamp", timestamp },
                    {"signature", hash }
                };

            var request = (HttpWebRequest)WebRequest.Create(_url);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Accept = "application/json";

            foreach (var hv in headers)
            {
                request.Headers[hv.Key] = hv.Value;
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            responseString = responseString.Trim('"').Replace("\\", "").Replace("\\", "");
            return responseString;
        }

        public string _url { get; set; }
    }
}
