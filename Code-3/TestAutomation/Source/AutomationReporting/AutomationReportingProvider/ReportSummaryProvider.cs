using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationReportingProvider
{
    public class ReportSummaryProvider
    {
        GenericProvider provider = new GenericProvider();

        public List<string> GetAllCountries()
        {
            List<string> response = new List<string>();
            response = provider.GetDistinct<List<string>>("Country");
            return response;
        }

        //GetAllCategories
        public List<string> GetAllCategories()
        {
            List<string> response = new List<string>();
            response = provider.GetDistinct<List<string>>("Category");
            return response;
        }

        //GetAllEnvironments
        public List<string> GetAllEnvironments()
        {
            List<string> response = new List<string>();
            response = provider.GetDistinct<List<string>>("Environment");
            return response;
        }

        public List<string> GetDistinctData(string query)
        {
            List<string> response = new List<string>();
            response = provider.GetDistinct<List<string>>(query);
            return response;
        }

        //GetLatestPieData
        public object GetLatestPieData(string country, string environment, string category)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("Country", country);
            query.Add("Environment", environment);
            query.Add("Category", category);
            response = provider.GetLatestPieData<object>(query);
            return response;
        }

        //GetLatestDetailByWhere
        public object GetLatestDetailByWhere(string country, string environment, string category, string result)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("Country", country);
            query.Add("Environment", environment);
            query.Add("Category", category);
            query.Add("Result", result);
            response = provider.GetLatestTestResult<object>(query);
            return response;
        }

        //GetTestHistory
        public object GetTestHistory(string test_name, string country, string environment)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("TestResults.TestName", test_name);
            query.Add("Country", country);
            query.Add("Environment", environment);
            response = provider.GetTests<object>(query);
            return response;
        }

        //GetTestDetails 
        public object GetTestDetails(string id)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("TestResults._id", id);
            response = provider.GetTests<object>(query);
            return response;
        }

        //
        public object GetGraphData(string environment, string category, string country, string fromDate)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("Country", country);
            query.Add("Environment", environment);
            query.Add("Category", category);
            query.Add("FromDate", fromDate);
            response = provider.GetHistory<object>(query);
            return response;
        }

        public object GetBuildData(string build)
        {
            object response = null;
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("BuildInfo.number", build);
            response = provider.GetHistory<object>(query);
            return response;
        }

        public object TriggerBatch(string env, string country, string cat)
        {
            object response = null;
            response = provider.TriggerRun<object>(env, country, cat);
            return response;
        }

        //GetBatchStatus
        public object GetBatchStatus(string env, string country, string cat)
        {
            object response = null;
            response = provider.GetBatchStatus<object>(env, country, cat);
            return response;
        }
    }

}
