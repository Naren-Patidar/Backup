using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace ConfigReader
{
    public class AppConfigReader : IAppConfigReader
    {
        #region IAppConfigReader Members

        public string GetConfigValue(string key, string fileName)
        {
            string value = string.Empty;
            XmlDocument doc = new XmlDocument();
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
                XmlNode res = doc.SelectSingleNode(string.Format("//add[@key='{0}']", key));
                if (res != null)
                {
                    value = res.Attributes["value"].Value;
                }
            }
            return value;
        }

        #endregion
    }
}
