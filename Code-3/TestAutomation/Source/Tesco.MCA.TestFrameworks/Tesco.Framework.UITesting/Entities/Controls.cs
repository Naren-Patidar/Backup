using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

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

        [XmlText]
        public string Text { get; set; }
    }
}
