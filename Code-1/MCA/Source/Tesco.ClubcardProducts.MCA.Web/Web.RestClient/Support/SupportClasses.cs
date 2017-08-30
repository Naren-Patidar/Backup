namespace Tesco.ClubcardProducts.MCA.Web.RestClient.Support
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;

    // The delegates use for asychronous calls
    public delegate void RestCallBack<T>(Exception ex, RestAPIResponse<T> result);

    public delegate void RestCallBackNonQuery(Exception ex);

    // Defines an adapter interface for the serializers
    public interface ISerializerAdapter
    {
        string Serialize(object obj);

        T Deserialize<T>(string input);
    }

    public class CookiedHttpWebRequestFactory
    {
        private static Object synchLock = new Object();
        ////This dictionary keeps the cookie container for each domain.
        private static Dictionary<string, CookieContainer> containers
            = new Dictionary<string, CookieContainer>();

        public static HttpWebRequest Create(string url)
        {
            // Create a HttpWebRequest object
            var request = (HttpWebRequest)WebRequest.Create(url);

            // this get the domain part of from the url
            string domain = (new Uri(url)).GetLeftPart(UriPartial.Authority);

            // try to get a container from the dictionary, if it is in the
            // dictionary, use it. Otherwise, create a new one and put it
            // into the dictionary and use it.
            CookieContainer container;
            lock (synchLock)
            {
                if (!containers.TryGetValue(domain, out container))
                {
                    container = new CookieContainer();
                    containers[domain] = container;
                }
            }

            // Assign the cookie container to the HttpWebRequest object
            request.CookieContainer = container;

            return request;
        }
    }

    // An implementation of ISerializerAdapter based on the JavaScriptSerializer
    public class JavaScriptSerializerAdapter : ISerializerAdapter
    {
        private JavaScriptSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the JavaScriptSerializerAdapter class.
        /// </summary>
        public JavaScriptSerializerAdapter()
        {
            serializer = new JavaScriptSerializer();
        }

        public string Serialize(object obj)
        {
            return serializer.Serialize(obj);
        }

        public T Deserialize<T>(string input)
        {
            return serializer.Deserialize<T>(input);
        }
    }

    public class JsonNetSerializer : ISerializerAdapter
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string input)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                DefaultValueHandling = DefaultValueHandling.Populate
            };

            return JsonConvert.DeserializeObject<T>(input, jsonSettings);
        }
    }

    // The configuration class defines how the rest call is made.
    public class ClientConfiguration
    {
        public string ContentType { get; set; }

        public string Accept { get; set; }

        public bool RequrieSession { get; set; }

        public ISerializerAdapter OutBoundSerializerAdapter { get; set; }

        public ISerializerAdapter InBoundSerializerAdapter { get; set; }
    }
}
