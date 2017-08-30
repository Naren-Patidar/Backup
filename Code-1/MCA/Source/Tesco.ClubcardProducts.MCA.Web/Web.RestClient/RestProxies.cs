namespace Tesco.ClubcardProducts.MCA.Web.RestClient
{
    using System.IO;
    using System.Net;
    using System.Text;
    using Tesco.ClubcardProducts.MCA.Web.RestClient.Support;
    using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    /// <summary>
    /// Synchronous proxy implementation
    /// </summary>
    public partial class RestProxies : IRestProxies
    {
        /// <summary>
        /// Initializes a new instance of the RestProxies class.
        /// </summary>
        public RestProxies()
        {
            // Initiate the default configuration
            DefaultConfiguration = new ClientConfiguration();
            DefaultConfiguration.ContentType = "application/json";
            DefaultConfiguration.Accept = "application/json";
            DefaultConfiguration.RequrieSession = false;
            DefaultConfiguration.OutBoundSerializerAdapter = new JsonNetSerializer();
            DefaultConfiguration.InBoundSerializerAdapter = new JsonNetSerializer();
        }

        /// <summary>
        /// Gets or sets the default configuration.
        /// </summary>
        /// <value>The default configuration.</value>
        public static ClientConfiguration DefaultConfiguration { get; set; }

        /// <summary>
        /// Create a request object according to the configuration
        /// </summary>
        private static HttpWebRequest CreateRequest(string url, ClientConfiguration clientConfig)
        {
            return clientConfig.RequrieSession ? CookiedHttpWebRequestFactory.Create(url) :
                (HttpWebRequest)WebRequest.Create(url);
        }

        /// <summary>
        /// Post data to the service
        /// </summary>
        private static void PostData<T>(HttpWebRequest request, ClientConfiguration clientConfig, T data)
        {
            var jsonRequestString = clientConfig.OutBoundSerializerAdapter.Serialize(data);
            var bytes = Encoding.UTF8.GetBytes(jsonRequestString);

            using (var postStream = request.GetRequestStream())
            {
                postStream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        ///  Receive data from the service
        /// </summary>
        private static RestAPIResponse<T> ReceiveData<T>(HttpWebRequest request, ClientConfiguration clientConfig)
        {
            string jsonResponseString;
            RestAPIResponse<T> result = new RestAPIResponse<T>();
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    if (stream == null)
                    {
                        return default(RestAPIResponse<T>);
                    }

                    using (var streamReader = new StreamReader(stream))
                    {
                        jsonResponseString = streamReader.ReadToEnd();
                    }

                    result.CharacterSet = response.CharacterSet;
                    result.ContentEncoding = response.ContentEncoding;
                    result.ContentLength = response.ContentLength;
                    result.ContentType = response.ContentType;
                    response.Headers.AllKeys.ToList().ForEach(h => result.Headers.Add(h, response.GetResponseHeader(h)));
                    result.LastModified = response.LastModified;
                    result.Method = response.Method;
                    result.ProtocolVersion = response.ProtocolVersion;
                    result.ResponseUri = response.ResponseUri;
                    result.Server = response.Server;
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;

                    jsonResponseString = jsonResponseString.Replace("\\", "").Trim('"');

                    result.Body = clientConfig.InBoundSerializerAdapter.Deserialize<T>(jsonResponseString);

                }

            }
            catch(Exception ex)
            {
                Exception exBase = ex.GetBaseException();
                if (exBase != null && exBase is WebException)
                {
                    System.Net.WebException realException = exBase as System.Net.WebException;
                    realException.Response.Headers.AllKeys.ToList().ForEach(h => result.Headers.Add(h, realException.Response.Headers[h]));

                    var httpWR = ((System.Net.HttpWebResponse)((realException).Response));

                    result.CharacterSet = httpWR.CharacterSet;
                    result.ContentEncoding = httpWR.ContentEncoding;
                    result.ContentLength = httpWR.ContentLength;
                    result.ContentType = httpWR.ContentType;
                    result.LastModified = httpWR.LastModified;
                    result.Method = httpWR.Method;
                    result.ProtocolVersion = httpWR.ProtocolVersion;
                    result.ResponseUri = httpWR.ResponseUri;
                    result.Server = httpWR.Server;
                    result.StatusCode = httpWR.StatusCode;
                    result.StatusDescription = httpWR.StatusDescription;
                }
                else
                {
                    throw ex;
                }
            }

            return result;
        }
    }
}
