using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Xml;

namespace DBConfigurationXmlUtility.Classes
{
    public class ConfigurationXML
    {
        /// <summary>
        /// Method to genarate the xml configuration data.
        /// The 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GenerateConfigurationXml(DataTable data)
        {
            string xmlData = string.Empty;
            XDocument configurations = new XDocument();
            XElement root = new XElement("NewDataSet");
            foreach (DataRow row in data.Rows)
            {
                XElement config = new XElement("ActiveDateRangeConfig", new object[] {
                    new XElement("ConfigurationType",row[0]),
                    new XElement("ConfigurationName",row[1]),
                    new XElement("ConfigurationValue1",row[2]),
                    new XElement("ConfigurationValue2",row[3]),
                    new XElement("IsDeleted",row[4]),
                    new XElement("Comments",row[5])
                });
                root.Add(config);
            }
            configurations.Add(root);
            xmlData = configurations.ToString();
            return xmlData;
        }

        public static string GenerateXSDConfigurationXml(string schemaFile, DataTable data)
        {
            string xmlData = string.Empty;
            XDocument configurations = new XDocument();
            XDocument schema = XDocument.Load(schemaFile);
            XElement root = schema.Root;
            configurations.Add(new XElement(root.Name));
            XElement element = root.Descendants().First();
            List<XElement> subElements = element.Descendants().ToList();
            
            foreach (DataRow row in data.Rows)
            {
                Int32 index = 0;
                XElement config = new XElement(element.Name, new object[] {
                    new XElement(subElements[index].Name,row[index++]),
                    new XElement(subElements[index].Name,row[index++]),
                    new XElement(subElements[index].Name,row[index++]),
                    new XElement(subElements[index].Name,row[index++]),
                    new XElement(subElements[index].Name,row[index++]),
                    new XElement(subElements[index].Name,row[index++])
                });
                configurations.Elements().First().Add(config);
            }
            
            xmlData = configurations.ToString();
            return xmlData;
        }
    }
}
