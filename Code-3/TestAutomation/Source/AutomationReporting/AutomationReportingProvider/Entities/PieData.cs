using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationReportingProvider.Entities
{
    public class PieData
    {
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public int Inconclusive { get; set; }
        public string Server { get; set; }
        public string ExecutionTime { get; set; }
        public string Browser { get; set; }
    }
}