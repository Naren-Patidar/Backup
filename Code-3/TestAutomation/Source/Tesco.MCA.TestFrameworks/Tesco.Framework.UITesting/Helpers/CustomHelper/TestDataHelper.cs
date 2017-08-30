using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace Tesco.Framework.UITesting.Helpers.CustomHelper
{
    class TestDataHelper<T>
    {
        T testData;

        public T TestData
        {
            get { return testData; }
            set { testData = value; }
        }

        public void LoadData(string xmlPath, string xPath, string domain)
        {
            try
            {
                XDocument xData = XDocument.Load(xmlPath);
                List<XElement> xList = new List<XElement>();

                xList = (from t in xData.Descendants(xPath)
                         where t.Attribute("ID").Value.ToUpper().Equals(domain.ToUpper())                
                            select t).ToList();
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                foreach (XElement element in xList)
                {
                    var stringReader = new System.IO.StringReader(element.ToString());
                    object xmlData = deserializer.Deserialize(stringReader);
                    testData = (T)xmlData;                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    
}
