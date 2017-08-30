using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.IO;

public partial class Aggregate : System.Web.UI.Page
{
    List<string> XKeysList = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" , "l", "m" , "n" , "o" , "p", "q" , "r", "s" , "t" , "u" , "v" , "w" , "x" , "y" , "z" };
    List<string> YKeysList = new List<string>() { "y" };
    public string CountryXKeys = string.Empty;
    public string CountryYKeys = string.Empty;
    public string CountryWiseData = string.Empty;
    public string CategoryXKeys = string.Empty;
    public string CategoryYKeys = string.Empty;
    public string CategoryWiseData = string.Empty;
    public string Dates = string.Empty;
    public string Categories = string.Empty;
    public string Countries = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCountries();
            BindAllCategories();
            BindAllCountries();
            BindCategories();
            BindCountryWiseGraph(ddlCountries.SelectedValue, ddlAllCategories.SelectedValue);
            BindCategoryyWiseGraph(ddlAllCountries.SelectedValue, ddlCategories.SelectedValue);
        }
    }


    private void BindCountryWiseGraph(string countryCode, string category)
    {
        StringBuilder sbData = new StringBuilder();
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        reports = reports.FindAll(r => r.CountryCode == countryCode);
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        List<string> DateList = reports.Select(c => c.ReportDateString).Distinct().ToList();
        Dates = jsonSerializer.Serialize(DateList);
        List<string> CategoriesList = reports.Select(c => c.CategoryName).Distinct().ToList();
        if (!string.IsNullOrEmpty(category) && category.ToUpper() != "ALL")
        {
            CategoriesList = CategoriesList.FindAll(c => c == category);
        }
        Categories = jsonSerializer.Serialize(CategoriesList);
        CountryXKeys = jsonSerializer.Serialize(XKeysList.Take(CategoriesList.Count));
        CountryYKeys = jsonSerializer.Serialize(YKeysList);
        int index0 = 0;
        sbData.Append("[");
        foreach (string date in DateList)
        {
            sbData.Append("{ y:\"" + string.Format("{0}", date) + "\" , ");
            int index2 = 0;
            foreach (string cat in CategoriesList)
            {
                sbData.Append(XKeysList[index2] + ": ");
                Report report = reports.Find(r => r.ReportDateString == date && r.CategoryName == cat);//&& r.CountryCode == country);
                if (report != null)
                {
                    sbData.Append(report.PassPercentage);
                }
                else
                {
                    sbData.Append("0");
                }
                if (index2 + 1 < CategoriesList.Count)
                {
                    sbData.Append(", ");
                }
                index2++;
            }
            sbData.Append("}");
            if (index0 + 1 < DateList.Count) //* CountryList.Count)
            {
                sbData.Append(", ");
            }
            index0++;
        }

        sbData.Append("]");
        CountryWiseData = sbData.ToString();
    }

    private void BindCategoryyWiseGraph(string countryCode, string category)
    {
        StringBuilder sbData = new StringBuilder();
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        reports = reports.FindAll(r => r.CategoryName == category);
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        List<string> DateList = reports.Select(c => c.ReportDateString).Distinct().ToList();
        Dates = jsonSerializer.Serialize(DateList);
        List<string> CountryList = reports.Select(c => c.CountryCode).Distinct().ToList();
        if (!string.IsNullOrEmpty(countryCode) && countryCode.ToUpper() != "ALL")
        {
            CountryList = CountryList.FindAll(c => c == countryCode);
        }
        Countries = jsonSerializer.Serialize(CountryList);
        CategoryXKeys = jsonSerializer.Serialize(XKeysList.Take(CountryList.Count));
        CategoryYKeys = jsonSerializer.Serialize(YKeysList);
        int index0 = 0;
        sbData.Append("[");
        foreach (string date in DateList)
        {
            sbData.Append("{ y:\"" + string.Format("{0}", date) + "\" , ");
            int index2 = 0;
            foreach (string country in CountryList)
            {
                sbData.Append(XKeysList[index2] + ": ");
                Report report = reports.Find(r => r.ReportDateString == date && r.CountryCode == country);//&& r.CountryCode == country);
                if (report != null)
                {
                    FileInfo rFi = new FileInfo(Server.MapPath(report.DetailLink));
                    if (File.Exists(rFi.FullName))
                    {
                        sbData.Append(GenericHelper.GetReportSection(rFi, "percentage").Replace("%", "").Trim());
                    }
                    else
                    {
                        sbData.Append("0");
                    }
                }
                else
                {
                    sbData.Append("0");
                }
                if (index2 + 1 < CountryList.Count)
                {
                    sbData.Append(", ");
                }
                index2++;
            }
            sbData.Append("}");
            if (index0 + 1 < DateList.Count) //* CountryList.Count)
            {
                sbData.Append(", ");
            }
            index0++;
        }

        sbData.Append("]");
        CategoryWiseData = sbData.ToString();
    }

    private void BindCountries()
    {
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        List<string> countries = reports.Select(c => c.CountryCode).Distinct().ToList();
        //countries.Insert(0, "All");
        ddlCountries.DataSource = countries;
        ddlCountries.DataBind();
    }


    private void BindAllCategories()
    {
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        List<string> categories = reports.Select(c => c.CategoryName).Distinct().ToList();
        categories.Insert(0, "All");
        ddlAllCategories.DataSource = categories;
        ddlAllCategories.DataBind();
    }

    private void BindAllCountries()
    {
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        List<string> countries = reports.Select(c => c.CountryCode).Distinct().ToList();
        countries.Insert(0, "All");
        ddlAllCountries.DataSource = countries;
        ddlAllCountries.DataBind();
    }


    private void BindCategories()
    {
        List<Report> reports = GenericHelper.GetArchievedReportSummary(string.Empty);
        List<string> categories = reports.Select(c => c.CategoryName).Distinct().ToList();        
        ddlCategories.DataSource = categories;
        ddlCategories.DataBind();
    }

    protected void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCountryWiseGraph(ddlCountries.SelectedValue, ddlAllCategories.SelectedValue);
        BindCategoryyWiseGraph(ddlAllCountries.SelectedValue, ddlCategories.SelectedValue);
    }

    protected void ddlAllCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCountryWiseGraph(ddlCountries.SelectedValue, ddlAllCategories.SelectedValue);
        BindCategoryyWiseGraph(ddlAllCountries.SelectedValue, ddlCategories.SelectedValue);
    }

    protected void ddlAllCountries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCountryWiseGraph(ddlCountries.SelectedValue, ddlAllCategories.SelectedValue);
        BindCategoryyWiseGraph(ddlAllCountries.SelectedValue, ddlCategories.SelectedValue);
    }

    protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCountryWiseGraph(ddlCountries.SelectedValue, ddlAllCategories.SelectedValue);
        BindCategoryyWiseGraph(ddlAllCountries.SelectedValue, ddlCategories.SelectedValue);
    }

}