using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;

namespace APITestClient.Helper
{
    public class APIRequester
    {
        string url = ConfigurationManager.AppSettings["APIURL"];
        public string MakeRequest(string postData, Dictionary<string, string> headers)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

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
    }
}