using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ArchieveSummary
/// </summary>
public class ArchieveSummary
{    
    List<ArchieveDate> archieveDates = new List<ArchieveDate>();    
 
    public List<ArchieveDate> ArchieveDates
    {
        get { return archieveDates; }
        set { archieveDates = value; }
    }

    public void Load(List<Report> reports)
    {
        List<String> dates = reports.OrderBy(f => f.ReportDate).ToList().Select(r => r.ReportDateString).Distinct().ToList();
        foreach (string d in dates)
        {
            ArchieveDate date = new ArchieveDate();
            date.ReportDate = d;            
            List<string> countries = reports.FindAll(r => r.ReportDateString == d).Select(r => r.CountryCode).Distinct().ToList();
            foreach (string c in countries)
            {
                Country country = new Country();
                country.Name = c;
                List<string> categories = reports.FindAll(r => r.CountryCode == c && r.ReportDateString == d).Select(r => r.CategoryName).Distinct().ToList();
                foreach (string ca in categories)
                {
                    Category category = new Category();
                    category.Name = ca;
                    category.Reports = reports.FindAll(r => r.CategoryName == ca && r.CountryCode == c && r.ReportDateString == d).Distinct().ToList();
                    country.Categories.Add(category);
                }
                date.Countries.Add(country);
            }
            ArchieveDates.Add(date);
        }
    }
}

public class ArchieveDate
{
    string reportDate = string.Empty;
    List<Country> countries = new List<Country>();

    public string ReportDate
    {
        get { return reportDate; }
        set { reportDate = value; }
    }

    public List<Country> Countries
    {
        get { return countries; }
        set { countries = value; }
    }
}

public class Country
{
    string name = string.Empty;
    List<Category> categories = new List<Category>();      

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<Category> Categories
    {
        get { return categories; }
        set { categories = value; }
    }
}

public class Category
{
    string name = string.Empty;
    List<Report> reports = new List<Report>();     

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<Report> Reports
    {
        get { return reports; }
        set { reports = value; }
    }
}