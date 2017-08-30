using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.UITesting.Entities
{
       [XmlRoot("Messages")]
        public class Messages
        {
            [XmlElement("Message")]
            public List<Message> lstMessages { get; set; }
        }

        public class Message
        {
            [XmlAttribute("id")]
            public string Id { get; set; }
            [XmlAttribute("title")]
            public string Title { get; set; }
            [XmlAttribute("type")]
            public string Type { get; set; }
            [XmlAttribute("description")]
            public string Description { get; set; }
            [XmlAttribute("Clubcard")]
            public string Clubcard { get; set; }
            [XmlAttribute("Password")]
            public string Password { get; set; }
            [XmlAttribute("EmailID")]
            public string EmailID { get; set; }

            [XmlText]
            public string Text { get; set; }
        }
    
}
