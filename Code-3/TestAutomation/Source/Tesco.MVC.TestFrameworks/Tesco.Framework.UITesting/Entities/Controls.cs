using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Tesco.Framework.UITesting.Enums;

namespace Tesco.Framework.UITesting.Entities
{
    [XmlRoot("Controls")]
    public class Controls
    {
        [XmlElement("Category")]
        public List<CategoryControls> lstCategoryControls { get; set; }
    }

    public class CategoryControls
    {
        [XmlElement("Control")]
        public List<Control> lstControls { get; set; }
        [XmlElement("actcontrol")]
        public List<string> actControls { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class Control
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("classname")]
        public string ClassName { get; set; }
        [XmlAttribute("xpath")]
        public string XPath { get; set; }
        [XmlAttribute("controlid")]
        public string ControlId { get; set; }
        [XmlAttribute("configname")]
        public string DBConfigurations { get; set; }
        [XmlAttribute("type")]
        public string ControlType { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
        [XmlAttribute("datanode")]
        public string DataNode { get; set; }
        [XmlAttribute("propertyname")]
        public string PropertyName { get; set; }
        [XmlText]
        public string Text { get; set; }

        public UIControlTypes UIControlType
        {
            get
            {
                UIControlTypes type = UIControlTypes.None;
                Int32 t = 0;
                string st = this.ControlType;
                if (Int32.TryParse(st, out t))
                {
                    type = (UIControlTypes)t;
                }
                return type;
            }
        }
    }
}
