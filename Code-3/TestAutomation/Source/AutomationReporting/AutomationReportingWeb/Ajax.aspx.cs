using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutomationReportingProvider;
using System.Web.Services;

public partial class Ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod]
    public static object GetDistinct()
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            string query = HttpContext.Current.Request.QueryString["q"];
            response = reportProvicer.GetDistinctData(query);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetGraphData(string Environment, string Category, string Country, string FromDate)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetGraphData(Environment, Category, Country, FromDate);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetBuildData(string BuildInfo)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetBuildData(BuildInfo);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object Trigger(string environment, string country, string category)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            string query = HttpContext.Current.Request.QueryString["q"];
            response = reportProvicer.TriggerBatch(environment, country, category);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetStatus(string environment, string country, string category)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            string query = HttpContext.Current.Request.QueryString["q"];
            response = reportProvicer.GetBatchStatus(environment, country, category);
        }
        catch { }
        return response;
    }
}