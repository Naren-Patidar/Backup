using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.Common.Reporting.Entities
{
    public class ResultSummary
    {
        [XmlAttribute("outcome")]
        public string Outcome { get; set; }

        [XmlElement("Counters")]
        public Counters ExecutionCounters { get; set; }
    }

    public class Counters
    {
        [XmlAttribute("pending")]
        public string PendingTestCases { get; set; }

        [XmlAttribute("inProgress")]
        public string InProgressTestCases { get; set; }

        [XmlAttribute("completed")]
        public string CompletedTestCases { get; set; }

        [XmlAttribute("warning")]
        public string Warnings { get; set; }

        [XmlAttribute("disconnected")]
        public string Disconnected { get; set; }

        [XmlAttribute("notExecuted")]
        public string NotExecuted { get; set; }

        [XmlAttribute("notRunnable")]
        public string NotRunnable { get; set; }

        [XmlAttribute("passedButRunAborted")]
        public string PassedButRunAborted { get; set; }
        
        [XmlAttribute("inconclusive")]
        public string Inconclusive { get; set; }

        [XmlAttribute("aborted")]
        public string Aborted { get; set; }
                
        [XmlAttribute("timeout")]
        public string Timeout { get; set; }

        [XmlAttribute("failed")]
        public string Failed { get; set; }

        [XmlAttribute("error")]
        public string Error { get; set; }

        [XmlAttribute("passed")]
        public string Passed { get; set; }

        [XmlAttribute("executed")]
        public string Executed { get; set; }

        [XmlAttribute("total")]
        public string Total { get; set; }
    }
}
