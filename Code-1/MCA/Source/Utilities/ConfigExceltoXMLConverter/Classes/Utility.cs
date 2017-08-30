﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace DBConfigurationXmlUtility.Classes
{
    public class Utility
    {
        public static string LoadXMLFile(string fileName)
        {
            string outXML = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            outXML = doc.InnerXml;
            return outXML;
        }

        public static void WriteXmlFile(string fileName, string content)
        {
            using (StreamWriter file = new System.IO.StreamWriter(fileName, false, Encoding.UTF8))
            {
                file.Write(content);
                file.Close();
            }
        }

        public static object XMLStringToObject(Type type, string xmlObj)
        {
            var stringReader = new System.IO.StringReader(xmlObj);
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(stringReader);
        }

        public static string ObjectToXMLString(Type type, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }
                return textWriter.ToString();
            }
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
    }
}
