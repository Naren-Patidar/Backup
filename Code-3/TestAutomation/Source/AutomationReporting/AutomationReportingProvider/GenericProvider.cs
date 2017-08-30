using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.RestClient;

namespace AutomationReportingProvider
{
    public class GenericProvider
    {
        RestProxies proxy = new RestProxies();

        public T GetDistinct<T>(string key) where T : new()
        { 
            T obj = new T();
            string url = string.Format("{0}/distinct?q={1}", ConfigurationManager.AppSettings["apiUrl"], key);
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        public T GetLatestTestResult<T>(Dictionary<string,string> query) where T : new()
        {
            T obj = new T();
            string url = string.Format("{0}/where/latest/testresult{1}", ConfigurationManager.AppSettings["apiUrl"], Utility.GetQueryString(query));
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        public T GetLatestPieData<T>(Dictionary<string, string> query) where T : new()
        {
            T obj = new T();
            string url = string.Format("{0}/where/latest/piedata{1}", ConfigurationManager.AppSettings["apiUrl"], Utility.GetQueryString(query));
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        public T GetTests<T>(Dictionary<string, string> query) where T : new()
        {
            T obj = new T();
            string url = string.Format("{0}/where/test{1}", ConfigurationManager.AppSettings["apiUrl"], Utility.GetQueryString(query));
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        public T GetHistory<T>(Dictionary<string, string> query) where T : new()
        {
            T obj = new T();
            string url = string.Format("{0}/history{1}", ConfigurationManager.AppSettings["apiUrl"], Utility.GetQueryString(query));
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        public T TriggerRun<T>(string env, string country, string cat) where T : new()
        {
            T obj = new T();
            string config = string.Format("{0}_{1}_API", env.ToUpper(), country.ToUpper());
            string url = string.Format("{0}/firetest/batch/{1}/{2}/{3}", ConfigurationManager.AppSettings[config], env, country, cat);
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

        /// <summary>
        /// Method to get the batch (Automation Job) status (if its ready to Trigger or already running)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="env">string Environment</param>
        /// <param name="country">string Country</param>
        /// <param name="cat">string Category</param>
        /// <returns>object status</returns>
        public T GetBatchStatus<T>(string env, string country, string cat) where T : new()
        {
            T obj = new T();
            string url = string.Format("{0}/firetest/getbatchstatus/{1}/{2}/{3}", ConfigurationManager.AppSettings["apiUrl"], env, country, cat);
            obj = proxy.RestGet<T>(url, null);
            return obj;
        }

    }
}
