using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationReportingProvider
{
    public class Utility
    {
        public static string GetQueryString(Dictionary<string, string> query)
        {
            string qs= string.Empty;
            int index = 0;
            foreach (KeyValuePair<string, string> entry in query)
            {
                qs += (index++ == 0) ? "?" : "&";
                qs += string.Format("{0}={1}", entry.Key, entry.Value);
            }
            return qs;
        }
    }
}
