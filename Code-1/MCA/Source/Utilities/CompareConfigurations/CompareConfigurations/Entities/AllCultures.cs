using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompareConfigurations.Entities
{
    [Serializable]
    public class AllCultures
    {
        List<Culture> cultures = new List<Culture>();
        [XmlElement]
        public List<Culture> Cultures
        {
            get { return cultures; }
            set { value = cultures; }
        }
    }
}
