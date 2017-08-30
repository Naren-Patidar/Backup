using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.Common.Reporting.Entities
{
    public class Times
    {
        [XmlAttribute("start")]
        public DateTime Start { get; set; }

        [XmlAttribute("finish")]
        public DateTime Finish { get; set; }

        [XmlAttribute("queuing")]
        public DateTime Queuing { get; set; }

        [XmlAttribute("creation")]
        public DateTime Creation { get; set; }
    }
}
