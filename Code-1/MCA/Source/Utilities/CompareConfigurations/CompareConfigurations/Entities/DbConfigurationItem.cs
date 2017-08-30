using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareConfigurations.Entities
{
    [Serializable]
    public class DbConfigurationItem
    {
        public string ConfigurationType { get; set; }
        public string ConfigurationName { get; set; }
        public string ConfigurationValue1 { get; set; }
        public string ConfigurationValue2 { get; set; }
        public string IsDeleted { get; set; }
    }
}
