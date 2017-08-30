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

public partial class DashboardV2 : System.Web.UI.Page
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
        ddlEnvironments.DataSource = reportProvicer.GetAllEnvironments();
        ddlEnvironments.DataBind();
    }

  
    [WebMethod]
    public static CountryCategoryList GetCountriesAndCategories()
    {
        CountryCategoryList response = new CountryCategoryList();
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            List<string> lstCountries = reportProvicer.GetAllCountries();
            List<string> lstCategories = reportProvicer.GetAllCategories();
            response = new CountryCategoryList { Categories = lstCategories, Countries = lstCountries };
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetLatestResult(string environment, string country, string category)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetLatestPieData(country, environment, category);            
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetLatestResultDetail(string environment, string country, string category, string result)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetLatestDetailByWhere(country, environment, category, result);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetTestHistory(string test_name, string country, string environment)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetTestHistory(test_name, country, environment);
        }
        catch { }
        return response;
    }

    [WebMethod]
    public static object GetTestDetails(string id)
    {
        object response = null;
        try
        {
            ReportSummaryProvider reportProvicer = new ReportSummaryProvider();
            response = reportProvicer.GetTestDetails(id);
        }
        catch { }
        return response;
    }
}