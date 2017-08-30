using System;
using System.Runtime.Remoting.Messaging;
using Tesco.ClubcardProducts.MCA.Web.RestClient.Support;
using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;

namespace Tesco.ClubcardProducts.MCA.Web.RestClient
{
    // Asynchronous proxy implementationp
    public partial class RestProxies : IRestAsyncProxies
    {
        private delegate RestAPIResponse<TR> GetDelegate<TR>(string url,
            ClientConfiguration configuration, string headerValue);
        private delegate void GetNonQueryDelegate(string url,
            ClientConfiguration configuration, string headerValue);
        private delegate RestAPIResponse<TR> PostDelegate<TR, TI>(string url, TI data,
            ClientConfiguration configuration, string headerValue);
        private delegate void PostNonQueryDelegate<TI>(string url, TI data,
            ClientConfiguration configuration, string headerValue);
        private delegate RestAPIResponse<TR> PutDelegate<TR, TI>(string url, TI data,
            ClientConfiguration configuration, string headerValue);
        private delegate void PutNonQueryDelegate<TI>(string url, TI data,
            ClientConfiguration configuration, string headerValue);

        // *************** Asynchronous Get ***************************
        #region Asynchronous Get
        public void RestGetAsync<TR>(string url, RestCallBack<TR> callback,
            ClientConfiguration configuration, string header)
        {
            var get = new GetDelegate<TR>(RestGet<TR>);
            get.BeginInvoke(url, configuration, header,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (GetDelegate<TR>)result.AsyncDelegate;
                var value = default(RestAPIResponse<TR>);
                Exception e = null;

                try { value = del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e, value); }

            }, null);
        }

        // Overload method
        public void RestGetAsync<TR>(string url, RestCallBack<TR> callback, string header)
        {
            RestGetAsync<TR>(url, callback, DefaultConfiguration, header);
        }

        // *********** Asynchronous Get, no response expected *************
        public void RestGetNonQueryAsync(string url,
            RestCallBackNonQuery callback, ClientConfiguration configuration, string headerValue)
        {
            var get = new GetNonQueryDelegate(RestGetNonQuery);
            get.BeginInvoke(url, configuration, headerValue,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (GetNonQueryDelegate)result.AsyncDelegate;
                Exception e = null;

                try { del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e); }

            }, null);

        }

        // Overload method
        public void RestGetNonQueryAsync(string url,
            RestCallBackNonQuery callback, string headerValue)
        {
            RestGetNonQueryAsync(url, callback, DefaultConfiguration, headerValue);
        } 
        #endregion
        // *************** Asynchronous Post *********************
        #region Asynchronous Post
        public void RestPostAsync<TR, TI>(string url, TI data,
            RestCallBack<TR> callback, ClientConfiguration configuration, string header)
        {
            var post = new PostDelegate<TR, TI>(RestPost<TR, TI>);
            post.BeginInvoke(url, data, configuration, "",
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PostDelegate<TR, TI>)result.AsyncDelegate;
                var value = default(RestAPIResponse<TR>);
                Exception e = null;

                try { value = del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e, value); }

            }, null);
        }

        // Overload method
        public void RestPostAsync<TR, TI>(string url, TI data,
            RestCallBack<TR> callback, string header)
        {
            RestPostAsync<TR, TI>(url, data, callback, DefaultConfiguration, header);
        }


        // ********* Asynchronous Post, no response expected *********
        public void RestPostNonQueryAsync<TI>(string url, TI data,
            RestCallBackNonQuery callback, ClientConfiguration configuration, string headerValue)
        {
            var post = new PostNonQueryDelegate<TI>(RestPostNonQuery);
            post.BeginInvoke(url, data, configuration, headerValue,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PostNonQueryDelegate<TI>)result.AsyncDelegate;
                Exception e = null;

                try { del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e); }

            }, null);
        }

        // Overload method
        public void RestPostNonQueryAsync<TI>(string url, TI data,
            RestCallBackNonQuery callback, string headerValue)
        {
            RestPostNonQueryAsync(url, data, callback, DefaultConfiguration, headerValue);
        }
        #endregion
        // *************** Asynchronous Post *********************
        #region Asynchronous Put
        public void RestPutAsync<TR, TI>(string url, TI data,
            RestCallBack<TR> callback, ClientConfiguration configuration, string header)
        {
            var post = new PutDelegate<TR, TI>(RestPut<TR, TI>);
            post.BeginInvoke(url, data, configuration, "",
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PutDelegate<TR, TI>)result.AsyncDelegate;
                var value = default(RestAPIResponse<TR>);
                Exception e = null;

                try { value = del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e, value); }

            }, null);
        }

        // Overload method
        public void RestPutAsync<TR, TI>(string url, TI data,
            RestCallBack<TR> callback, string header)
        {
            RestPutAsync<TR, TI>(url, data, callback, DefaultConfiguration, header);
        }

        // ********* Asynchronous Put, no response expected *********
        public void RestPutNonQueryAsync<TI>(string url, TI data,
            RestCallBackNonQuery callback, ClientConfiguration configuration, string headerValue)
        {
            var post = new PutNonQueryDelegate<TI>(RestPutNonQuery);
            post.BeginInvoke(url, data, configuration, headerValue,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PutNonQueryDelegate<TI>)result.AsyncDelegate;
                Exception e = null;

                try { del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e); }

            }, null);
        }
        // Overload method
        public void RestPutNonQueryAsync<TI>(string url, TI data,
            RestCallBackNonQuery callback, string headerValue)
        {
            RestPutNonQueryAsync(url, data, callback, DefaultConfiguration, headerValue);
        } 
        #endregion
    }
}
