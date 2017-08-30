namespace Tesco.ClubcardProducts.MCA.Web.RestClient
{
    using Tesco.ClubcardProducts.MCA.Web.RestClient.Support;
    using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;

    /// <summary>
    /// Class RestProxies.
    /// </summary>
    public partial class RestProxies : IRestProxies
    {
        // ********************** Synchronous GET *************************
        public RestAPIResponse<TR> RestGet<TR>(string url, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.Accept = clientConfig.Accept;
            request.Method = "GET";
            request.Headers.Add("Authorization", headerValue);

            return ReceiveData<TR>(request, clientConfig);
        }

        // Overload method
        public RestAPIResponse<TR> RestGet<TR>(string url, string headerValue)
        {
            return RestGet<TR>(url, DefaultConfiguration, headerValue);
        }

        // ******** Synchronous GET, no response expected *******
        public void RestGetNonQuery(string url, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.Method = "GET";
            request.Headers.Add("Authorization", headerValue);
            request.GetResponse().Close();
        }

        // Overload method
        public void RestGetNonQuery(string url, string headerValue)
        {
            RestGetNonQuery(url, DefaultConfiguration, headerValue);
        }

        // ***************** Synchronous POST ********************
        public RestAPIResponse<TR> RestPost<TR, TI>(string url, TI data, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "POST";
            request.Headers.Add("Authorization", headerValue);

            PostData(request, clientConfig, data);
            return ReceiveData<TR>(request, clientConfig);
        }

        // Overload method
        public RestAPIResponse<TR> RestPost<TR, TI>(string url, TI data, string headerValue)
        {
            return RestPost<TR, TI>(url, data, DefaultConfiguration, headerValue);
        }

        // ****** Synchronous POST, no respons expected ******
        public void RestPostNonQuery<TI>(string url, TI data, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "POST";
            request.Headers.Add("Authorization", headerValue);

            PostData(request, clientConfig, data);
            request.GetResponse().Close();
        }

        // Overload method
        public void RestPostNonQuery<TI>(string url, TI data, string headerValue)
        {
            RestPostNonQuery(url, data, DefaultConfiguration, headerValue);
        }

        // ***************** Synchronous PUT ********************
        public RestAPIResponse<TR> RestPut<TR, TI>(string url, TI data, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "PUT";
            request.Headers.Add("Authorization", headerValue);

            PostData(request, clientConfig, data);
            return ReceiveData<TR>(request, clientConfig);
        }

        // Overload method
        public RestAPIResponse<TR> RestPut<TR, TI>(string url, TI data, string headerValue)
        {
            return RestPut<TR, TI>(url, data, DefaultConfiguration, headerValue);
        }

        // ****** Synchronous PUT, no respons expected ******
        public void RestPutNonQuery<TI>(string url, TI data, ClientConfiguration configuration, string headerValue)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "PUT";
            request.Headers.Add("Authorization", headerValue);

            PostData(request, clientConfig, data);
            request.GetResponse().Close();
        }

        // Overload method
        public void RestPutNonQuery<TI>(string url, TI data, string headerValue)
        {
            RestPutNonQuery(url, data, DefaultConfiguration, headerValue);
        }
    }
}
