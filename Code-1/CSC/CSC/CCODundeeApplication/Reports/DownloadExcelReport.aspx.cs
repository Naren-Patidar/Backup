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



namespace CCODundeeApplication.Reports
{
    #region Header
    /// <department>Fujitsu, e-Innovation, eCRM</department>
    /// <copyright>(c) Fujitsu Consulting, 2002</copyright>
    /// <development> 
    ///    <version number="1.10" day="16" month="01" year="2003">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Steve Lang</checker>
    ///			<work_packet>WP/Barcelona/046</work_packet>
    ///			<description>Namespaces conform to standards</description>
    ///	</version>
    ///		<version number="1.02" day="12" month="12" year="2002">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Mark Hart</checker>
    ///			<work_packet>WP/Barcelona/026</work_packet>
    ///			<description>Add layout element to report script</description>
    ///		</version>
    /// </development>
    /// <development> 
    ///    <version number="1.10" day="16" month="01" year="2003">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Steve Lang</checker>
    ///			<work_packet>WP/Barcelona/046</work_packet>
    ///			<description>Namespaces conform to standards</description>
    ///	</version>
    ///		<version number="1.01" day="06" month="11" year="2002">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker></checker>
    ///			<work_packet>WP/Barcelona/017</work_packet>
    ///			<description>Initial version of Report as a generic application</description>
    ///		</version>
    /// </development>
    /// <summary>
    /// 	A page which generates a CSV file which represents the report
    /// </summary>
    #endregion
    public partial class DownloadExcelReport : System.Web.UI.Page
    {
        /// <summary>
        /// XML control for XSLT transformation to CSV
        /// </summary>
        //protected System.Web.UI.WebControls.Xml DownLoadXSL;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
        }

        #region Main Code
        /// <summary>
        /// Gets the latest report which has been stored as XML as a session variable 
        /// Generates a file name which the browser will suggest as the target for the download
        /// Sets the XML document as the source of the XSLT transformation to CSV
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
            Response.ContentType = "application/ms-excel";
            XmlDocument reportXml = new XmlDocument();
            //Changed for Development of NGCv3.6
            string reportName = "Customer Registration Report";





            try
            {
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResultXml")))
                {
                    reportXml.LoadXml(Helper.GetTripleDESEncryptedCookieValue("ResultXml"));
                   
                }
               
              
                   
            }
            catch
            {
                
            }





            //Added to format the data before downloading to Excel
            //XmlNodeList customer_accounts = reportXml.SelectNodes("//JoinRoute");
            XmlNodeList joinRoute = reportXml.SelectNodes("//ClubcardID");
            foreach (XmlNode accountNode in joinRoute)
            {
                accountNode.InnerText = "'" + accountNode.InnerText;
            }

           
            //Added to download the report to Excel instaed of csv
            // ApplicationClass excelApplication = new ApplicationClass();
            Workbook xlWB=null;
            Worksheet xlWS=null;
            xlWB = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Missing.Value);
            //xlWB = excelApplication.Workbooks.Add(Type.Missing);
            //xlWB.Application.Visible = true;
            xlWS = (Microsoft.Office.Interop.Excel.Worksheet)xlWB.ActiveSheet;

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
            reportName = ConfigurationSettings.AppSettings["ReportName1"];            
            string reportHeader ="" ;//Localization.GetLocalizedAttributeString("NGCMarketing.Report" + reportName);
            DateTime Date = DateTime.Now;
            string CurrentDate = Date.ToString("dd_MM_yyyy");
            string reportRunDate = "";// Localization.GetLocalizedAttributeString("NGCMarketing.Report.InputCriteria.ReportRunDate");


            xlWS.Cells[i, j] = country.DisplayName.ToString();
            i = i + 1;
            xlWS.Cells[i, j] = reportName;
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
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Headers")))
            {
                headerXml = Helper.GetTripleDESEncryptedCookieValue("Headers");
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
                xlWS.Cells[i, j] = "Report Run Date";
                j = j + 1;
                xlWS.Cells[i, j] = "'" + date;
            }
            else
            {
                xlWS.Cells[i, j] = "Report Run Date";
                j = j + 1;
                xlWS.Cells[i, j] = date;
            }

            i = i + 2; ; j = 1;
            for (int m = 0; m < count; m++)
            {                
                string localizedHeader = "";
                if (htHeader.Count > 0)
                {
                    localizedHeader = htHeader["Input" + m].ToString(); ;

                }
                xlWS.Cells[i, j] = localizedHeader;
                j++;
            }
            i = i + 1;

            int tablesCount = dsReportData.Tables.Count;

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
            string csvFileName = "UK" +"." + reportName + CardTypeDesc + "." + CurrentDate;
            Response.AddHeader("Content-Disposition", "Attachment; filename=" + csvFileName + ".xls");

            Response.ContentType = "application/vnd.ms-excel";
            Response.WriteFile(sFile);
            Response.Flush();
            if (File.Exists(sFile))
            {
                File.Delete(sFile);
            }
            //End of Implementation
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        /// <summary>Convert XML to HashTable</summary>
        /// <param name="sXml"> XML data to convert into HashTable </param>
        /// <param name="objName"> Name of the node to search </param>
        /// <returns> Returns HashTable</returns>
        /// <remarks>This method accepts XML data in string format and converts into HashTable and returning the HashTable</remarks>
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
