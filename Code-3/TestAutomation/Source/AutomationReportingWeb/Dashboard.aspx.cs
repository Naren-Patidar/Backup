using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Web.Services;
using System.Net;
using System.Diagnostics;
using System.Web.Script.Serialization;

public partial class _Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["q"] == null)
            {
                BindDashBoard();
            }
            else
            {
                BindSearchResult(Request.QueryString["q"]);
            }
        }
    }

    private void BindSearchResult(string query)
    {
        rpSearchReport.DataSource = GenericHelper.GetArchievedReportSummary(query);
        rpSearchReport.DataBind();
    }

    private void BindDashBoard()
    {
        BindEnvironments();
        BindReport();
    }

    private void BindEnvironments()
    {
        ddlEnvironments.DataSource = GenericHelper.GetEnvironments();
        ddlEnvironments.DataBind();
    }

    private void BindReport()
    {
        ReportSummary summary = GenericHelper.GetReportSummary(ddlEnvironments.SelectedValue);
        rSumary.DataSource = summary.ReportDetails;
        rSumary.DataBind();
    }

    

    protected void rSumary_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rReports = e.Item.FindControl("rReports") as Repeater;
        if (rReports != null)
        {
            ReportDetail data = e.Item.DataItem as ReportDetail;
            rReports.DataSource = data.Reports;
            rReports.DataBind();
        }
    }

    [WebMethod]
    public static string GetStatus(string country, string category, string browser)
    {
        string response = string.Empty;
        try
        {
            string statusFolder = ConfigurationManager.AppSettings["StatusFolder"];
            string statusFile = Path.Combine(statusFolder, string.Format("{0}.{1}.{2}.status", country, category, browser));
            if (File.Exists(statusFile))
            {
                response = "Running";
            }
            else
            {
                response = "Ready";
            }
        }
        catch { response = "error"; }
        return response;
    }

    [WebMethod]
    public static string Run(string country, string category, string browser)
    {
        string response = string.Empty;
        try
        {
            string batchFolder = ConfigurationManager.AppSettings["BatchFolder"];
            string batchFile = Path.Combine(batchFolder, country, category, string.Format("{0}.{1}.bat", category, browser));
            if (File.Exists(batchFile))
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo { FileName = batchFile };
                p.Start();
            }
        }
        catch { response = "error"; }
        return response;
    }

    [WebMethod]
    public static Report GetUpdatedInformation(string country, string category, string browser)
    {
        Report response = new Report();
        try
        {
            string reportFolder = ConfigurationManager.AppSettings["ReportFolder"];
            reportFolder = Path.Combine(reportFolder, country, category);
            FileInfo file = (from f in new DirectoryInfo(reportFolder).GetFiles("*.html")
                           orderby f.LastWriteTimeUtc descending
                           select f).Take(1).FirstOrDefault();
            response = GenericHelper.GetReport(file);            
        }
        catch {  }
        return response;
    }

    protected void ddlEnvironments_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindReport();
    }
}
