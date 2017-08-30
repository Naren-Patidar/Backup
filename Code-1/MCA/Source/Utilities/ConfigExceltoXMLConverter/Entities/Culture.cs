using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DBConfigurationXmlUtility.Entities
{
    [Serializable]
    public class Culture
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Country { get; set; }
        [XmlAttribute]
        public string WorksheetName { get; set; }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Country))
                {
                    return string.Format("{0} ({1})", Name, Country);
                }
                else
                {
                    return !string.IsNullOrEmpty(Name) ? Name : !string.IsNullOrEmpty(Country) ? Country : string.Empty;
                }
            }
        }
    }
}
