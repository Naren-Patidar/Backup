using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for ReportSummary
/// </summary>
public class ReportSummary
{
    List<ReportDetail> reportDetails = new List<ReportDetail>();
    DateTime summaryDate = DateTime.MinValue;      

    public List<ReportDetail> ReportDetails
    {
        get { return reportDetails; }
        set { reportDetails = value; }
    }

    public DateTime SummaryDate
    {
        get { return summaryDate; }
        set { summaryDate = value; }
    }
}

public class ReportDetail
{
    string serverName = string.Empty;
    string countryCode = string.Empty;
    string absoluteDirectoryPath = string.Empty;
    string relativeDirectoryPath = string.Empty;
    List<Report> reports = new List<Report>();
    DateTime reportDate = DateTime.MinValue;
    string passPercentage = string.Empty;

    public string ServerName
    {
        get { return serverName; }
        set { serverName = value; }
    }

    public string CountryCode
    {
        get { return countryCode; }
        set { countryCode = value; }
    }

    public string AbsoluteDirectoryPath
    {
        get { return absoluteDirectoryPath; }
        set { absoluteDirectoryPath = value; }
    }

    public string RelativeDirectoryPath
    {
        get { return relativeDirectoryPath; }
        set { relativeDirectoryPath = value; }
    }

    public List<Report> Reports
    {
        get { return reports; }
        set { reports = value; }
    }

    public DateTime ReportDate
    {
        get { return reportDate; }
        set { reportDate = value; }
    }

    public string ExecutionTime
    {
        get {
            string time = "00:00:00";
            try
            {
                List<string> times = this.reports.Select(r => r.Duration).ToList();
                int hours = 0, minutes = 0, seconds = 0;
                times.ForEach(t => hours += Convert.ToInt32(t.Split(':')[0]));
                times.ForEach(t => minutes += Convert.ToInt32(t.Split(':')[1]));
                times.ForEach(t => seconds += Convert.ToInt32(t.Split(':')[2]));
                int extraMins = seconds / 60;
                seconds = seconds % 60;
                minutes += extraMins;
                int extraHours = minutes / 60;
                minutes = minutes % 60;
                hours += extraHours;
                time = string.Join(":", new object[] { hours.ToString("00"), minutes.ToString("00"), seconds.ToString("00") });
            }
            catch { }
            return time;
        }
    }

    public string PassPercentage
    {
        get
        {            
            try
            {
                if (string.IsNullOrEmpty(passPercentage))
                {
                    passPercentage = "00.00";
                    Int32 failed = 0;
                    this.reports.ForEach(r => failed += r.FailedTests);
                    Int32 total = 0;
                    this.reports.ForEach(r => total += r.TotalTests);
                    passPercentage = ((decimal)(total - failed) * 100 / total).ToString("00.00");
                }
            }
            catch { }
            return passPercentage;
        }
    }

    public decimal PassPercentageValue
    {
        get {
            decimal value = 0;
            decimal.TryParse(PassPercentage, out value);
            return value;
        }
    }

    public bool IsGreen
    {
        get
        {
            bool status = false;
            int threshold1 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold1"], out threshold1);
            status = PassPercentageValue >= threshold1;
            return status;
        }
    }

    public bool IsSuccess
    {
        get
        {
            bool status = false;
            int threshold1 = 0, threshold2 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold1"], out threshold1);
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold2"], out threshold2);
            status = threshold2 <= PassPercentageValue && PassPercentageValue < threshold1;
            return status;
        }
    }

    public bool IsAmber
    {
        get
        {
            bool status = false;
            int threshold2 = 0, threshold3 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold2"], out threshold2);
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold3"], out threshold3);
            status = threshold3 <= PassPercentageValue && PassPercentageValue < threshold2;
            return status;
        }
    }
}

public class Report
{
    string id = string.Empty;
    string countryCode = string.Empty;
    string categoryName = string.Empty;
    string browser = string.Empty;
    string environment = string.Empty;
    string server = string.Empty;
    string displayName = string.Empty;
    string resultPercentage = string.Empty;
    string detailLink = string.Empty;
    int totalTests = 0;    
    int passedTests = 0;
    int inconclusiveTests = 0;
    int failedTests = 0;
    DateTime reportDate = DateTime.MinValue;
    string duration = string.Empty;


    public string ID
    {
        get { return id; }
        set { id = value; }
    }

    public string CountryCode
    {
        get { return countryCode; }
        set { countryCode = value; }
    }

    public string CategoryName
    {
        get { return categoryName; }
        set { categoryName = value; }
    }

    public string DisplayName
    {
        get { return displayName; }
        set { displayName = value; }
    }

    public string Browser
    {
        get { return browser; }
        set { browser = value; }
    }

    public string Environment
    {
        get { return environment; }
        set { environment = value; }
    }

    public string Server
    {
        get { return server; }
        set { server = value; }
    }

    public string ResultPercentage
    {
        get { return resultPercentage; }
        set { resultPercentage = value; }
    }

    public string DetailLink
    {
        get { return detailLink; }
        set { detailLink = value; }
    }

    public int TotalTests
    {
        get { return totalTests; }
        set { totalTests = value; }
    }

    public int PassedTests
    {
        get { return passedTests; }
        set { passedTests = value; }
    }

    public int InconclusiveTests
    {
        get { return inconclusiveTests; }
        set { inconclusiveTests = value; }
    }

    public int FailedTests
    {
        get { return failedTests; }
        set { failedTests = value; }
    }

    public DateTime ReportDate
    {
        get { return reportDate; }
        set { reportDate = value; }
    }

    public string Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    public string ReportDateString
    {
        get {
            string strDate = DateTime.MinValue.ToString("dd-MMM");
            if (reportDate != null)
            {
                strDate = reportDate.ToString(ConfigurationManager.AppSettings["DateFormat"]);
            }
            return strDate; 
        }
    }

    public string ReportTimeString
    {
        get
        {
            string strTime = DateTime.MinValue.ToString("hh:mm");
            if (reportDate != null)
            {
                strTime = reportDate.ToString(ConfigurationManager.AppSettings["TimeFormat"]);
            }
            return strTime;
        }
    }

    public decimal PassPercentage
    {
        get
        {
            string tmp = ResultPercentage;
            decimal value = 0;
            tmp = tmp.Replace("%", "");
            decimal.TryParse(tmp.Trim(), out value);
            return value;
        }
    }

    public bool IsGreen
    {
        get
        {
            bool status = false;
            int threshold1 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold1"], out threshold1);
            status = PassPercentage >= threshold1;
            return status;
        }
    }

    public bool IsSuccess
    {
        get
        {
            bool status = false;
            int threshold1 = 0, threshold2 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold1"], out threshold1);
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold2"], out threshold2);
            status = threshold2 <= PassPercentage && PassPercentage < threshold1;
            return status;
        }
    }

    public bool IsAmber
    {
        get
        {
            bool status = false;
            int threshold2 = 0, threshold3 = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold2"], out threshold2);
            Int32.TryParse(ConfigurationManager.AppSettings["PassThreshold3"], out threshold3);
            status = threshold3 <= PassPercentage && PassPercentage < threshold2;
            return status;
        }
    }
}