using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.UITesting.Entities
{   
    [XmlRoot("add")]
    public class WebConfiguration
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class PreferencesUIConfig
    {
        Int16 _PreferenceID = 0;
        bool _IsVisible;

        public Int16 preferenceid
        {
            get { return _PreferenceID; }
            set { _PreferenceID = value; }
        }

        public bool isvisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }

        public List<short> dependentprefidsassame { get; set; }

        public List<short> dependentprefidsasopp { get; set; }
    }

}
