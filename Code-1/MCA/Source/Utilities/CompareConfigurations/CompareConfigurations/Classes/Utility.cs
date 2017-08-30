using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Configuration;
using CompareConfigurations.Entities;
using System.Xml;

namespace CompareConfigurations.Classes
{
    public class Utility
    {
        public static AllCultures LoadCultures()
        {
            string culturesFileName = (ConfigurationManager.AppSettings.AllKeys.Contains("CulturesXMLFile")) ? ConfigurationManager.AppSettings["CulturesXMLFile"] : string.Empty;
            string culturesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, culturesFileName);
            string xmlCultures = Utility.LoadXMLFile(culturesFile);
            return xmlCultures.ToObject<AllCultures>();
        }

        public static DataTable exceldata(string filePath, string sheetName)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            //get the required sheet
            DataRow[] rows = schemaTable.Select("TABLE_NAME = '" + sheetName + "$'");
            if (rows.Length > 0)
            {
                DataRow schemaRow = rows[0];
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT  * FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            return dtexcel;
        }

        public static string LoadXMLFile(string fileName)
        {
            string outXML = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            outXML = doc.InnerXml;
            return outXML;
        }

        
    }
}
