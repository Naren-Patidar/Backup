using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using CCODundeeApplication.NGCReportingService;

namespace CCODundeeApplication.Reports
{
    public partial class ExportToExcel : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Xml DownLoadXSL;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Initialize the culture


        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture"));
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("Default.aspx", false);
        }
        #endregion

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            try
            {
                InitializeComponent();
                base.OnInit(e);
                Response.ContentType = "application/ms-excel";
                XmlDocument reportXml = new XmlDocument();
                //Changed for Development of NGCv3.6

                string reportName = "";
                string startDate = null;
                string endDate = null;
                int week = 0;
                string period = "";

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportName")))
                {
                    reportName = Helper.GetTripleDESEncryptedCookieValue("ReportName");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("StartDate")))
                {

                    startDate = Helper.GetTripleDESEncryptedCookieValue("StartDate").ToString();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EndDate")))
                {

                    endDate = Helper.GetTripleDESEncryptedCookieValue("EndDate").ToString();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Week")))
                {
                    week = Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("Week").ToString());
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Period")))
                {
                    period = Helper.GetTripleDESEncryptedCookieValue("Period").ToString();
                }
                if (reportName == "Promotional Code Report")
                {
                    reportXml = new XmlDocument();
                    NGCReportingServiceClient objReportingService = new NGCReportingServiceClient();
                    DataSet ds = new DataSet();
                    ds = objReportingService.GetPromotionalReport(startDate, endDate, week, period);
                    reportXml.LoadXml(ds.GetXml());
                }
               else if (reportName == "CustomerLoadReport")
                {
                    reportXml = new XmlDocument();
                    NGCReportingServiceClient objReportingService = new NGCReportingServiceClient();
                    DataSet ds = new DataSet();
                    ds = objReportingService.GetCustomerLoadReport(startDate, endDate, week, period);
                    reportXml.LoadXml(ds.GetXml());
                }
                 //Added to format the data before downloading to Excel
                //XmlNodeList customer_accounts = reportXml.SelectNodes("//JoinRoute");
                XmlNodeList clubcardNode = reportXml.SelectNodes("//ClubcardAccountNumber");
                foreach (XmlNode accountNode in clubcardNode)
                {
                    accountNode.InnerText = "'" + accountNode.InnerText;
                }
                XmlNodeList primaryclubcardNode = reportXml.SelectNodes("//PrimaryCardAccountNumber");
                foreach (XmlNode accountNode in primaryclubcardNode)
                {
                    accountNode.InnerText = "'" + accountNode.InnerText;
                }

               //Added to download the report to Excel instaed of csv
                //Application excelApplication = new Application();
                Workbook xlWB = null;
                Worksheet xlWS = null;


                xlWB = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Missing.Value);
                //xlWB = excelApplication.Workbooks.Add(Type.Missing);
                //xlWB.Application.Visible = true;
                xlWS = (Microsoft.Office.Interop.Excel.Worksheet)xlWB.ActiveSheet;

                //Microsoft.Office.Interop.Excel.Application app = null;
                //Microsoft.Office.Interop.Excel.Workbook xlWB = null;
                //Microsoft.Office.Interop.Excel.Worksheet xlWS = null;
                //app = new Microsoft.Office.Interop.Excel.Application();
                //app.Visible = true;
                //xlWB = app.Workbooks.Add(1);
                //xlWS = (Microsoft.Office.Interop.Excel.Worksheet)xlWB.Sheets[1];


                DataSet dsReportData = new DataSet();
                dsReportData.ReadXml(new XmlNodeReader(reportXml));

                int i = 1, j = 1;
                i = 1; j = 1;
                int count = dsReportData.Tables[0].Columns.Count;

                //Get the Country Name
                string DefaultCulture;
                DefaultCulture = ConfigurationSettings.AppSettings["CultureDefault"];
                RegionInfo country = new RegionInfo(new CultureInfo((string)DefaultCulture, false).LCID);
                string Country = country.DisplayName.ToString();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string reportHeader = "";//Localization.GetLocalizedAttributeString("NGCMarketing.Report" + reportName);
                DateTime Date = DateTime.Now;
                string CurrentDate = Date.ToString("dd_MM_yyyy");
                string reportRunDate = "";// Localization.GetLocalizedAttributeString("NGCMarketing.Report.InputCriteria.ReportRunDate");


                xlWS.Cells[i, j] = country.DisplayName.ToString();
                i = i + 1;
                xlWS.Cells[i, j] = GetLocalResourceObject("CSC.CustomerLoadReport.Report." + reportName).ToString();
                i = i + 2;

                xlWS.get_Range("$A1", "$E1").Font.Bold = true;
                xlWS.get_Range("$A1", "$E1").Font.Size = 14;
                xlWS.get_Range("$A1", "$E1").HorizontalAlignment = XlHAlign.xlHAlignCenter;
                xlWS.get_Range(xlWS.Cells[1, 1], xlWS.Cells[1, 4]).Merge(Type.Missing);

                xlWS.get_Range("$A2", "$E2").Font.Bold = true;
                xlWS.get_Range("$A2", "$E2").Font.Size = 14;
                xlWS.get_Range("$A2", "$E2").HorizontalAlignment = XlHAlign.xlHAlignCenter;
                xlWS.get_Range(xlWS.Cells[2, 1], xlWS.Cells[2, 4]).Merge(Type.Missing);

                //Write the input criterias to xml
                string inputXml = "";
                //Changed for NGCv32 Req.No:015

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportHeader")))
                {
                    inputXml = Helper.GetTripleDESEncryptedCookieValue("ReportHeader");
                }
                else
                {
                    inputXml = "";
                }
                //inputXml = this.Session["InputCriteria"].ToString();

                //Chnage completed
                Hashtable htInputCriteria = new Hashtable();
                htInputCriteria = XMLToHashTable(inputXml, "InputData");
                string headerXml;
                Hashtable htHeader = new Hashtable();

                //headerXml = this.Session["Headers"].ToString();
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportHeader")))
                {
                    headerXml = Helper.GetTripleDESEncryptedCookieValue("ReportHeader");
                }
                else
                {
                    headerXml = "";
                }

                htHeader = XMLToHashTable(headerXml, "Headers");



                int inputCount = htInputCriteria.Count;
                for (int z = 0; z < inputCount; z++)
                {
                    xlWS.Cells[i, j] = htInputCriteria["Input" + z].ToString();
                    z = z + 1; j = j + 1;
                    xlWS.Cells[i, j] = htInputCriteria["Input" + z].ToString();
                    if (j == 2)
                    {
                        j = j + 1;
                    }
                    else
                    {
                        i = i + 1; j = 1;
                    }
                }
                if (j == 2)
                {
                    j = j + 1;
                    //xlWS.Cells[i, j] = "Report Run Date";
                    xlWS.Cells[i, j]=GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName.ReportRunDate").ToString();
                    j = j + 1;
                    xlWS.Cells[i, j] = "'" + date;
                }
                else
                {
                    //xlWS.Cells[i, j] = "Report Run Date";
                    xlWS.Cells[i, j] = GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName.ReportRunDate").ToString();
                    j = j + 1;
                    xlWS.Cells[i, j] = date;
                }

                i = i + 2; ; j = 1;
                for (int m = 0; m <= count; m++)
                {
                    if (m == count)
                    {
                        xlWS.Cells[i, j] = "Total";
                        break;
                    }
                    string colHeader = dsReportData.Tables[0].Columns[m].ColumnName;
                    string localizedHeader = "";
                    if (reportName == "Clubcard Registration Report" && m > 0)
                    {

                        localizedHeader = colHeader;
                    }
                    if (reportName == "Promotional Code Report")
                    {
                        localizedHeader = colHeader;
                    }
                    else
                    {
                        localizedHeader = GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName." + colHeader).ToString();
                    }
                    
                    xlWS.Cells[i, j] = localizedHeader;
                    j++;
                    
                   
                }
                i = i + 1;

                int tablesCount = dsReportData.Tables.Count;

                if (reportName == "Clubcard Registration Report")
                {

                    for (int m = 0; m < tablesCount; m++)
                    {
                        count = dsReportData.Tables[m].Columns.Count;
                        foreach (DataRow dr in dsReportData.Tables[m].Rows)
                        {
                            j = 1;
                            int total = 0;
                            for (int k = 0; k <= count; k++)
                            {
                                if (k < count)
                                {
                                    xlWS.Cells[i, j] = dr[k];
                                    if (k > 0)
                                        total = total + Convert.ToInt32(dr[k]);
                                    j++;
                                }
                                else if (k == count)
                                {
                                    xlWS.Cells[i, j] = total.ToString();
                                }
                            }

                            i++;
                        }
                    }
                    j = 2;
                    count = dsReportData.Tables[0].Columns.Count;
                    int rows = dsReportData.Tables[0].Rows.Count;
                    int sum = 0;
                    for (int h = 0; h < count; h++)
                    {
                        if (h > 0)
                        {
                            sum = 0;
                            for (int g = 0; g < rows; g++)
                            {
                                sum = sum + Convert.ToInt32(dsReportData.Tables[0].Rows[g][h].ToString());
                            }
                            xlWS.Cells[i + 1, j-1] = sum;
                            j++;
                        }
                        else
                        {
                            xlWS.Cells[i + 1, j-1] = "Total";
                            j++;
                        }
                    }
                    
                }
                else if (reportName == "Promotional Code Report")
                {

                    for (int m = 0; m < tablesCount; m++)
                    {
                        count = dsReportData.Tables[m].Columns.Count;
                        foreach (DataRow dr in dsReportData.Tables[m].Rows)
                        {
                            j = 1;
                            int total = 0;
                            for (int k = 0; k <= count; k++)
                            {
                                if (k < count)
                                {
                                    xlWS.Cells[i, j] = dr[k];
                                    if (k > 1)
                                        total = total + Convert.ToInt32(dr[k]);
                                    j++;
                                }
                                else if (k == count)
                                {
                                    xlWS.Cells[i, j] = total.ToString();
                                }
                            }

                            i++;
                        }
                    }


                }
                else
                {
                    for (int m = 0; m < tablesCount; m++)
                    {
                        count = dsReportData.Tables[m].Columns.Count;
                        foreach (DataRow dr in dsReportData.Tables[m].Rows)
                        {
                            j = 1;

                            for (int k = 0; k < count; k++)
                            {
                                xlWS.Cells[i, j] = dr[k];
                                j++;
                            }
                            i++;
                        }
                    }
                }
                
              
                xlWS.Columns.AutoFit();

                string userName = "";//this.Session[WebSessionIndexes.UserId()].ToString();

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    userName = Helper.GetTripleDESEncryptedCookieValue("UserID");
                }
                else
                {
                    userName = "";
                }


                if (reportName == "Clubcard Registration Report")
                {
                    Range range;
                    string str = "";
                    range = xlWS.UsedRange;
                    int totalRows = range.Rows.Count;
                    int totalColumn=range.Columns.Count;
                    int totalCount = 0;
                    //for (int rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    //{
                        for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                        {
                            str =Convert.ToString(((range.Cells[totalRows, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2));
                            if (str != "" && str !="Total")
                            {
                                totalCount = Convert.ToInt16(str) + totalCount;
                            }
                        }
                        
                                          
                        xlWS.Cells[totalRows-1, totalColumn] = totalCount.ToString();
                    //}
                }

                string folder = ConfigurationSettings.AppSettings["NGCDownloadReportsFolder"];
                string sFile = folder + "MyExcel" + userName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //string sFile = "C:\\MyExcel_" + userName + ".xls";

                //Delete if the file exists
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }

                xlWB.Saved = true;
                //xlWB.SaveAs(sFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlWB.SaveCopyAs(sFile);
                xlWB.Close(null, null, null);
                GC.Collect();
                //excelApplication.Quit();            

                Response.Clear();
                //Get the Cardtype column into the xml document
                string CardTypeDesc = "";
                if (htInputCriteria["ClubcardType"] != null)
                    CardTypeDesc = htInputCriteria["ClubcardType"].ToString();
                string csvFileName = "UK" + "." + reportName + CardTypeDesc + "." + CurrentDate;
                Response.AddHeader("Content-Disposition", "Attachment; filename=" + csvFileName + ".xls");

                Response.ContentType = "application/vnd.ms-excel";
                Response.WriteFile(sFile);
                Response.Flush();
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }
            }
            catch (Exception ex)
            { }
        }

        private void InitializeComponent()
        {

        }

        public static Hashtable XMLToHashTable(string sXml, string nodeNametoSearch)
        {
            Hashtable ht = new Hashtable();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);

                XmlNodeList nodes = doc.SelectNodes(nodeNametoSearch);
                foreach (XmlNode node in nodes)
                {
                    for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                        {
                            if (node.ChildNodes.Item(i).ChildNodes.Count > 1) //&& node.ChildNodes.Item(i).NodeType != XmlNodeType.Document )
                            {
                                HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                            }
                            else
                            {
                                ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return ht;
        }

        private static void HandleChildNodes(Hashtable ht, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                    {
                        if (node.ChildNodes.Item(i).ChildNodes.Count > 1)
                        {
                            HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                        }
                        else
                        {
                            ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                        }
                    }
                }
            }
        }


    }
}
