using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AutomationReportingProvider.Entities
{
    [XmlRoot(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public class UnitTestResult
    {
        string outcome = string.Empty;
        string serverName = string.Empty;
        string startTime = string.Empty;
        string endTime = string.Empty;
        string duration = string.Empty;
        string testName = string.Empty;
        Output output = new Output();
        string environment = string.Empty;
        string browser = string.Empty;
        string category = string.Empty;
        string country = string.Empty;
        public int Id { get; set; }
        public int BatchId { get; set; }

        [XmlAttribute("outcome")]
        public string Result
        {
            get { return outcome; }
            set { outcome = value; }
        }

        [XmlAttribute("computerName")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        [XmlAttribute("startTime")]
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        [XmlAttribute("endTime")]
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        [XmlAttribute("duration")]
        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [XmlAttribute("testName")]
        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        [XmlElementAttribute("Output")]
        public Output Output
        {
            get { return output; }
            set { output = value; }
        }

        public string Environment
        {
            get { return environment; }
            set { environment = value; }
        }

        public string Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }
    }

    public class Output
    {
        string debugTrace = string.Empty;
        ErrorInfo error = new ErrorInfo();

        [XmlElementAttribute("DebugTrace")]
        public string DebugTrace
        {
            get { return debugTrace; }
            set { debugTrace = value; }
        }

        [XmlElementAttribute("ErrorInfo")]
        public ErrorInfo Error
        {
            get { return error; }
            set { error = value; }
        }
    }

    public class ErrorInfo
    {
        string message = string.Empty;
        string stackTrace = string.Empty;

        [XmlElementAttribute("Message")]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [XmlElementAttribute("StackTrace")]
        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }
    }
}
