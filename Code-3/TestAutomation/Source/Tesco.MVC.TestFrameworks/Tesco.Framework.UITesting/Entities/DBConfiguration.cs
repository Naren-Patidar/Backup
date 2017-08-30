using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.UITesting.Entities
{
    [XmlRoot("Configuration")]
    public class DBConfiguration
    {
        public DBConfiguration()
        {
            this.ConfigurationValue1 = string.Empty;
            this.ConfigurationValue2 = string.Empty;
        }

        [XmlElement("ConfigurationType")]
        public string ConfigurationType { get; set; }

        [XmlElement("ConfigurationName")]
        public string ConfigurationName { get; set; }

        [XmlElement("ConfigurationValue1")]
        public string ConfigurationValue1 { get; set; }

        [XmlElement("ConfigurationValue2")]
        public string ConfigurationValue2 { get; set; }

        [XmlElement("IsDeleted")]
        public string IsDeleted { get; set; }
    }
}
