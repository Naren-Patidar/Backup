using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtQuery.Attributes.Add("placeholder", ConfigurationManager.AppSettings["DateFormat"]);
        BindLeftNavigation();
        BindArchieved();
    }

    private void BindArchieved()
    {
        ArchieveSummary aSummary = new ArchieveSummary();
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        aSummary.Load(reports);
        rArchived.DataSource = aSummary.ArchieveDates;
        rArchived.DataBind();
    }

    void BindLeftNavigation()
    {
        string reportsDir = ConfigurationManager.AppSettings["ReportsDir"];
        List<string> countries = GenericHelper.GetCountries(Server.MapPath(reportsDir));
        rCountriesLinks.DataSource = countries;
        rCountriesLinks.DataBind();
    }

    protected void rCountriesLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rCategories = e.Item.FindControl("rCategoriesLinks") as Repeater;
        if (rCategories != null)
        {
            string reportsDir = ConfigurationManager.AppSettings["ReportsDir"];
            List<string> categories = GenericHelper.GetCategories(Server.MapPath(reportsDir), e.Item.DataItem.ToString());
            rCategories.DataSource = categories;
            rCategories.DataBind();
        }
    }

    protected void rCategoriesLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rCategories = e.Item.FindControl("rReportLinks") as Repeater;
        if (rCategories != null)
        {
            string reportsDir = ConfigurationManager.AppSettings["ReportsDir"];
            List<string> reports = GenericHelper.GetReports(Server.MapPath(reportsDir), e.Item.DataItem.ToString());
            rCategories.DataSource = reports;
            rCategories.DataBind();
        }
    }

    protected void rArchived_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rp = e.Item.FindControl("rArchivedCountries") as Repeater;
        if (rp != null)
        {
            ArchieveDate ad = e.Item.DataItem as ArchieveDate;
            rp.DataSource = ad.Countries;
            rp.DataBind();
        }
    }

    protected void rArchivedCountries_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rp = e.Item.FindControl("rArchivedCategories") as Repeater;
        if (rp != null)
        {
            Country c = e.Item.DataItem as Country;
            rp.DataSource = c.Categories;
            rp.DataBind();
        }
    }

    protected void rArchivedCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rp = e.Item.FindControl("rArchivedReport") as Repeater;
        if (rp != null)
        {
            Category c = e.Item.DataItem as Category;
            rp.DataSource = c.Reports;
            rp.DataBind();
        }
    }

    protected void rArchivedReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Repeater rp = e.Item.FindControl("rArchivedCategories") as Repeater;
        //if (rp != null)
        //{
        //    Category c = e.Item.DataItem as Category;
        //    rp.DataSource = c.Reports;
        //    rp.DataBind();
        //}
    }

    
    

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("Default.aspx?q={0}", txtQuery.Text));
    }
}
