using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for GenericHelper
/// </summary>
public class GenericHelper
{
	public GenericHelper()
	{
		
	}

    public static List<string> GetCountries(string dir)
    {
        List<string> countries = new List<string>();
        try
        {
            if (!string.IsNullOrEmpty(dir))
            {
                List<DirectoryInfo> countriesDI = new DirectoryInfo(dir).GetDirectories().ToList();
                countriesDI = countriesDI.FindAll(di => di.Name.Length == 2);
                countriesDI.ForEach(di => countries.Add(di.Name));
            }
        }
        catch { }
        return countries;
    }

    public static List<string> GetCategories(string dir, string country)
    {
        List<string> categories = new List<string>();
        try
        {
            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(country))
            {
                List<DirectoryInfo> countriesDI = new DirectoryInfo(Path.Combine(dir, country)).GetDirectories().ToList();                
                countriesDI.ForEach(di => categories.Add(string.Format("{0}|{1}",country, di.Name)));
            }
        }
        catch { }
        return categories;
    }

    public static List<string> GetReports(string dir, string category)
    {
        List<string> reports = new List<string>();
        string country = category.Split('|').ToList().FirstOrDefault();
        category = category.Split('|').ToList().Skip(1).FirstOrDefault();
        try
        {
            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(category))
            {
                List<FileInfo> countriesDI = new DirectoryInfo(Path.Combine(dir, country, category)).GetFiles().ToList();
                countriesDI.ForEach(f => reports.Add(MakeRelative(f.FullName, 4)));
            }
        }
        catch { }
        return reports;
    }

    public static string MakeRelative(string filePath, int depth)
    {
        List<string> parts = filePath.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        parts.Reverse();
        parts = parts.Take(depth).ToList();
        parts.Reverse();
        return string.Join("/", parts);
    }

    public static string GetReportDisplayName(string fullName)
    {
        string displayName = string.Empty;
        try
        {
            displayName = Path.GetFileName(fullName).Split('_').ToList().LastOrDefault();
            displayName = new string(displayName.Where(c => !new char[] { '0', '1', '2' ,'3', '4', '5', '6', '7', '8', '9' }.Contains(c)).ToArray());
        }
        catch
        {
            displayName = "Error";
        }
        return displayName;
    }

    public static ReportSummary GetReportSummary()
    {
        ReportSummary summary = new ReportSummary();
        try 
        {
            string reportDir = ConfigurationManager.AppSettings["ReportsDir"];
            reportDir = HttpContext.Current.Server.MapPath(reportDir);
            List<DirectoryInfo> countries = new List<DirectoryInfo>();
            DirectoryInfo sourceDir = new DirectoryInfo(reportDir);
            if (sourceDir.Exists)
            {
                countries = sourceDir.GetDirectories().ToList();
                countries.RemoveAll(c => c.Name.StartsWith("Archived"));
                foreach (DirectoryInfo country in countries)
                {
                    ReportDetail detail = new ReportDetail();
                    detail.CountryCode = country.Name;
                    detail.AbsoluteDirectoryPath = country.FullName;
                    List<DirectoryInfo> categories = new List<DirectoryInfo>();
                    categories = country.GetDirectories().ToList();
                    int index = 0;
                    foreach (DirectoryInfo category in categories)
                    {
                        Report report = new Report();
                        report.ID = string.Format("{0}_{1}_{2}", detail.CountryCode, category.Name, index++);
                        report.CountryCode = country.Name;
                        report.CategoryName = category.Name;
                        try
                        {
                            List<FileInfo> reports = category.GetFiles().ToList();
                            FileInfo reportFi = (from f in reports
                                                 orderby f.CreationTime descending
                                                 select f).ToList().FirstOrDefault();
                            report.ReportDate = reportFi.LastWriteTime;
                            report.ResultPercentage = GetReportNumberData(reportFi, "percentage");
                            report.TotalTests = Convert.ToInt32(GetReportNumberData(reportFi, "total").Trim());
                            report.PassedTests = Convert.ToInt32(GetReportNumberData(reportFi, "pass").Trim());
                            report.InconclusiveTests = Convert.ToInt32(GetReportNumberData(reportFi, "inconclusive").Trim());
                            report.FailedTests = Convert.ToInt32(GetReportNumberData(reportFi, "failed").Trim());
                            report.Duration = GetReportSection(reportFi, "time");
                            detail.Reports.Add(report);
                        }
                        catch { }
                    }
                    summary.ReportDetails.Add(detail);
                }
            }
        }
        catch { }
        return summary;
    }

    public static ReportSummary GetReportSummary(string environment)
    {
        ReportSummary summary = new ReportSummary();
        try
        {
            string reportDir = ConfigurationManager.AppSettings["ReportsDir"];
            reportDir = HttpContext.Current.Server.MapPath(reportDir);
            List<DirectoryInfo> countries = new List<DirectoryInfo>();
            DirectoryInfo sourceDir = new DirectoryInfo(reportDir);
            if (sourceDir.Exists)
            {
                countries = sourceDir.GetDirectories().ToList();
                countries.RemoveAll(c => c.Name.StartsWith("Archived"));
                countries.RemoveAll(c => !c.Name.ToUpper().EndsWith(string.Format("_{0}", environment.ToUpper())));
                foreach (DirectoryInfo country in countries)
                {
                    ReportDetail detail = new ReportDetail();
                    detail.CountryCode = country.Name;
                    detail.AbsoluteDirectoryPath = country.FullName;
                    List<DirectoryInfo> categories = new List<DirectoryInfo>();
                    categories = country.GetDirectories().ToList();
                    int index = 0;
                    foreach (DirectoryInfo category in categories)
                    {
                        Report report = new Report();
                        report.ID = string.Format("{0}_{1}_{2}", detail.CountryCode, category.Name, index++);
                        report.CountryCode = country.Name;
                        report.CategoryName = category.Name;                        
                        try
                        {
                            List<FileInfo> reports = category.GetFiles().ToList();
                            FileInfo reportFi = (from f in reports
                                                 orderby f.CreationTime descending
                                                 select f).ToList().FirstOrDefault();
                            report.Browser = GetBrowserName(reportFi.Name);
                            report.Environment = GetEnvironmentName(reportFi.Name);
                            report.Server = GetServer(reportFi.Name);
                            report.ReportDate = reportFi.LastWriteTime;
                            report.ResultPercentage = GetReportNumberData(reportFi, "percentage");
                            report.TotalTests = Convert.ToInt32(GetReportNumberData(reportFi, "total").Trim());
                            report.PassedTests = Convert.ToInt32(GetReportNumberData(reportFi, "pass").Trim());
                            report.InconclusiveTests = Convert.ToInt32(GetReportNumberData(reportFi, "inconclusive").Trim());
                            report.FailedTests = Convert.ToInt32(GetReportNumberData(reportFi, "failed").Trim());
                            report.Duration = GetReportSection(reportFi, "time");
                            report.DetailLink = MakeRelative(reportFi.FullName, 4);
                            detail.Reports.Add(report);
                        }
                        catch { }
                    }
                    summary.ReportDetails.Add(detail);
                }
            }
        }
        catch { }
        return summary;
    }

    public static List<string> GetEnvironments()
    {
        List<string> environments = new List<string>();
        string reportDir = ConfigurationManager.AppSettings["ReportsDir"];
        reportDir = HttpContext.Current.Server.MapPath(reportDir);
        List<DirectoryInfo> countries = new List<DirectoryInfo>();
        DirectoryInfo sourceDir = new DirectoryInfo(reportDir);
        if (sourceDir.Exists)
        {
            countries = sourceDir.GetDirectories().ToList();
            environments = (from t in countries
                            where t.Name.Contains("_")
                            select t.Name.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[1]).Distinct().ToList();
        }
        return environments;
    }

    public static List<Report> GetArchievedReportSummary(string date)
    {
        int reportDays = 7;
        if(ConfigurationManager.AppSettings.AllKeys.Contains("DaysRange"))
        {
            Int32.TryParse(ConfigurationManager.AppSettings["DaysRange"], out reportDays);
        }
        
        List<Report> summary = new List<Report>();
        try
        {
            string reportDir = ConfigurationManager.AppSettings["ArchievedDir"];
            string dateFormat = ConfigurationManager.AppSettings["DateFormat"];
            reportDir = HttpContext.Current.Server.MapPath(reportDir);
            DirectoryInfo sourceDir = new DirectoryInfo(reportDir);
            if (sourceDir.Exists)
            {
                List<FileInfo> reportsFis = sourceDir.GetFiles("*.html", SearchOption.AllDirectories).ToList();
                if (!string.IsNullOrEmpty(date))
                {
                    reportsFis = (from f in reportsFis
                                  where f.LastWriteTime.ToString(dateFormat) == date
                                  select f).ToList();
                }
                else
                {

                    reportsFis = (from f in reportsFis
                                  where f.LastWriteTime > DateTime.Now.AddDays(0 - reportDays)
                                  orderby f.LastWriteTime
                                  select f).ToList();
                }
                foreach (FileInfo r in reportsFis)
                {
                    Report rep = new Report();
                    rep.CategoryName = GetCategory(r.FullName);
                    rep.CountryCode = GetCountry(r.FullName);
                    rep.DetailLink = MakeRelative(r.FullName, 5);
                    rep.ResultPercentage = GetReportSection(r, "percentage");
                    rep.PassedTests = Convert.ToInt32(GetReportNumberData(r, "pass"));
                    rep.TotalTests = Convert.ToInt32(GetReportNumberData(r, "total"));
                    rep.InconclusiveTests = Convert.ToInt32(GetReportNumberData(r, "inconclusive"));
                    rep.FailedTests = Convert.ToInt32(GetReportNumberData(r, "failed"));
                    rep.Duration = GetReportSection(r, "time");
                    rep.ReportDate = r.LastWriteTime;
                    rep.DisplayName = r.Name;
                    summary.Add(rep);
                }
            }
        }
        catch { }
        return summary;
    }    

    public static string GetReportNumberData(FileInfo report, string sectionName)
    {
        string data = string.Empty;
        try
        {
            data = GetReportSection(report, sectionName).Trim();
            if (string.IsNullOrEmpty(data))
            {
                data = "0";
            }
            else
            {
 
            }
        }
        catch { }
        return data;
    }

    public static string GetReportSection(FileInfo report, string sectionName)
    {
        string section = string.Empty;
        string pattern = string.Empty;
        MatchCollection matches;
        if (report != null)
        {
            string input = File.ReadAllText(report.FullName);
            switch (sectionName)
            {
                case "percentage":
                    pattern = "<td nowrap=\"nowrap\" width=\"(.*?)\" align=\"right\"><b>(.*?)<\\/b><\\/td>";

                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[2].Value;
                    }
                    break;
                case "total":
                    pattern = "<td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td>";
                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[1].Value;
                    }
                    break;
                case "pass":
                    pattern = "<td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td>";
                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[2].Value;
                    }
                    break;
                case "inconclusive":
                    pattern = "<td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td>";
                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[3].Value;
                    }
                    break;
                case "failed":
                    pattern = "<td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td><td>(.*?)<\\/td>";
                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[4].Value;
                    }
                    break;
                case "time":
                    pattern = "<td>(.*?)<\\/td><\\/tr>";
                    matches = Regex.Matches(input, pattern);
                    if (matches.Count > 0)
                    {
                        section = matches[0].Groups[1].Value;
                    }
                    break;
            }
        }
        return section;
    }

    private static string GetCountry(string p)
    {
        string country = string.Empty;
        List<string> parts = Path.GetDirectoryName(p).Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        parts.Reverse();
        country = parts.Skip(1).Take(1).FirstOrDefault();
        return country;
    }

    private static string GetCategory(string p)
    {
        string category = string.Empty;
        List<string> parts = Path.GetDirectoryName(p).Split(new string[] { @"\" } , StringSplitOptions.RemoveEmptyEntries).ToList();
        category = parts.LastOrDefault();
        return category;
    }

    public static Report GetReport(FileInfo r)
    {
        Report report = new Report();
        report.CategoryName = GetCategory(r.FullName);
        report.CountryCode = GetCountry(r.FullName);
        report.DetailLink = MakeRelative(r.FullName, 5);
        report.ResultPercentage = GetReportSection(r, "percentage");
        report.PassedTests = Convert.ToInt32(GetReportNumberData(r, "pass"));
        report.TotalTests = Convert.ToInt32(GetReportNumberData(r, "total"));
        report.InconclusiveTests = Convert.ToInt32(GetReportNumberData(r, "inconclusive"));
        report.FailedTests = Convert.ToInt32(GetReportNumberData(r, "failed"));
        report.Duration = GetReportSection(r, "time");
        report.ReportDate = r.LastWriteTime;
        report.DisplayName = r.Name;
        return report;
    }

    public static string GetBrowserName(string report)
    {
        string browser = string.Empty;
        List<string> parts = report.Split(new string[] { "_" }, StringSplitOptions.None).ToList();
        browser = parts.Skip(1).Take(1).FirstOrDefault();
        return browser;
    }

    
    public static string GetEnvironmentName(string report)
    {
        string env = string.Empty;
        List<string> parts = report.Split(new string[] { "_" }, StringSplitOptions.None).ToList();
        env = parts.Skip(2).Take(1).FirstOrDefault();
        return env;
    }

    //GetServer
    public static string GetServer(string report)
    {
        string server = string.Empty;
        List<string> parts = report.Split(new string[] { "_" }, StringSplitOptions.None).ToList();
        server = parts.Skip(6).Take(1).FirstOrDefault();
        return server;
    }
}