using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutomationReportingProvider;
using System.Web.Services;
using System.Web.Script.Serialization;
using AutomationReportingProvider.Entities;

public partial class History : System.Web.UI.Page
{
    ReportSummaryProvider reportProvicer = new ReportSummaryProvider();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDashBoard();            
        }
    }

    private void BindDashBoard()
    {
        BindEnvironments();        
    }

    private void BindEnvironments()
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
    
}