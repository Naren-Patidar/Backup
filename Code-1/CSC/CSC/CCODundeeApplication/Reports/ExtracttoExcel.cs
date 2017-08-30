using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using CCODundeeApplication.NGCReportingService;
using System.Globalization;
using NGCTrace;


namespace CCODundeeApplication
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public static class ExtracttoExcel
    {
        public static string ExportExcel(string reportData, string reportName, string reportRunDate, Hashtable htColNames, string inputXml, string capabilityName)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //

            string sFile = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CSC.ExtracttoExcel.ExportExcel reportData" + reportData + "reportName" + reportName + "reportRunDate" + reportRunDate + "htColNames" + htColNames + "inputXml" + inputXml + "capabilityName" + capabilityName);
                NGCTrace.NGCTrace.TraceDebug("Start:CSC.ExtracttoExcel.ExportExcel reportData" + reportData + "reportName" + reportName + "reportRunDate" + reportRunDate + "htColNames" + htColNames + "inputXml" + inputXml + "capabilityName" + capabilityName);

                //Response.ContentType = "application/ms-excel";
                XmlDocument reportXml = new XmlDocument();
                reportXml.LoadXml(reportData);
                //Changed for Development of NGCv3.6
                string startDate = null;
                string endDate = null;
                int week = 0;
                string period = "";

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
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
                {
                    DefaultCulture = Helper.GetTripleDESEncryptedCookieValue("Culture").ToString();

                }
                else

                    DefaultCulture = ConfigurationSettings.AppSettings["CultureDefault"];

                RegionInfo country = new RegionInfo(new CultureInfo((string)DefaultCulture, false).LCID);
                string Country = country.DisplayName.ToString();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string reportHeader = "";//Localization.GetLocalizedAttributeString("NGCMarketing.Report" + reportName);
                DateTime Date = DateTime.Now;
                string CurrentDate = Date.ToString("dd_MM_yyyy");

                xlWS.Cells[i, j] = country.DisplayName.ToString();
                i = i + 1;
                xlWS.Cells[i, j] = reportName;//GetLocalResourceObject("CSC.CustomerLoadReport.Report." + reportName).ToString();
                i = i + 2;

                xlWS.get_Range("$A1", "$E1").Font.Bold = true;
                xlWS.get_Range("$A1", "$E1").Font.Size = 14;
                xlWS.get_Range("$A1", "$E1").HorizontalAlignment = XlHAlign.xlHAlignCenter;
                xlWS.get_Range(xlWS.Cells[1, 1], xlWS.Cells[1, 4]).Merge(Type.Missing);

                xlWS.get_Range("$A2", "$E2").Font.Bold = true;
                xlWS.get_Range("$A2", "$E2").Font.Size = 14;
                xlWS.get_Range("$A2", "$E2").HorizontalAlignment = XlHAlign.xlHAlignCenter;
                xlWS.get_Range(xlWS.Cells[2, 1], xlWS.Cells[2, 4]).Merge(Type.Missing);


                //Chnage completed
                Hashtable htInputCriteria = new Hashtable();
                htInputCriteria = XMLToHashTable(inputXml, "InputData");

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
                    xlWS.Cells[i, j] = reportRunDate;//GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName.ReportRunDate").ToString();
                    j = j + 1;
                    xlWS.Cells[i, j] = "'" + date;
                }
                else
                {
                    //xlWS.Cells[i, j] = "Report Run Date";
                    xlWS.Cells[i, j] = reportRunDate;//GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName.ReportRunDate").ToString();
                    j = j + 1;
                    xlWS.Cells[i, j] = date;
                }

                i = i + 2; ; j = 1;
                for (int m = 0; m < count; m++)
                {
                    string colHeader = dsReportData.Tables[0].Columns[m].ColumnName;
                    string localizedHeader = "";
                    if (capabilityName == "ClubcardRegistrationReport" && m > 0)
                    {

                        localizedHeader = colHeader;
                    }
                    if (capabilityName == "PromotionalCodeReport" && m > 1)
                    {
                        localizedHeader = colHeader;
                    }
                    if (capabilityName == "PointsEarnedReport")
                    {

                        xlWS.get_Range(xlWS.Cells[i, j], xlWS.Cells[i, j]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                        localizedHeader = htColNames[colHeader].ToString();
                    }

                    else
                    {
                        localizedHeader = htColNames[colHeader].ToString();//GetLocalResourceObject("CSC.CustomerLoadReport.ColumnName." + colHeader).ToString();
                    }

                    xlWS.Cells[i, j] = localizedHeader;
                    j++;


                }
                if (capabilityName == "ClubcardRegistrationReport" || capabilityName == "PromotionalCodeReport")
                {
                    xlWS.Cells[i, j] = "Total";

                }
                i = i + 1;

                int tablesCount = dsReportData.Tables.Count;

                if (capabilityName == "ClubcardRegistrationReport")
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
                            xlWS.Cells[i + 1, j - 1] = sum;
                            j++;
                        }
                        else
                        {
                            xlWS.Cells[i + 1, j - 1] = "Total";
                            j++;
                        }

                    }


                    Range range;
                    string str = "";
                    range = xlWS.UsedRange;
                    int totalRows = range.Rows.Count;
                    int totalColumn = range.Columns.Count;
                    int totalCount = 0;
                    //for (int rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    //{
                    for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                    {
                        str = Convert.ToString(((range.Cells[totalRows, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2));
                        if (str != "" && str != "Total")
                        {
                            totalCount = Convert.ToInt16(str) + totalCount;
                        }
                    }

                    xlWS.Cells[i, j - 1] = totalCount;
                    //xlWS.Cells[totalRows - 1, totalColumn-1] = totalCount.ToString();

                }
                else if (capabilityName == "PromotionalCodeReport")
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

                string folder = ConfigurationSettings.AppSettings["NGCDownloadReportsFolder"];
                sFile = folder + "MyExcel" + userName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

                NGCTrace.NGCTrace.TraceInfo("End:CSC.ExtracttoExcel.ExportExcel reportData" + reportData + "reportName" + reportName + "reportRunDate" + reportRunDate + "htColNames" + htColNames + "inputXml" + inputXml + "capabilityName" + capabilityName);
                NGCTrace.NGCTrace.TraceDebug("End:CSC.ExtracttoExcel.ExportExcel reportData" + reportData + "reportName" + reportName + "reportRunDate" + reportRunDate + "htColNames" + htColNames + "inputXml" + inputXml + "capabilityName" + capabilityName);

                //excelApplication.Quit(); 
                return sFile;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CSC.ExtracttoExcel.ExportExcel sFile" + sFile + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CSC.ExtracttoExcel.ExportExcel" + sFile + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CSC.ExtracttoExcel.ExportExcel");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            finally
            {
            }
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