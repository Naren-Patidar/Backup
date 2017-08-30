using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    [Serializable]
    public class DbConfigurationItem
    {
        string _ConfigurationName = string.Empty;
        string _ConfigurationValue1 = string.Empty;
        string _ConfigurationValue2 = string.Empty;       

        public string ConfigurationTypeName { get { return this.ConfigurationType.ToString(); } }
        public DbConfigurationTypeEnum ConfigurationType { get; set; }
        public string ConfigurationName
        {
            get { return _ConfigurationName; }
            set { _ConfigurationName = value; }
        }
        public string ConfigurationValue1
        {
            get { return _ConfigurationValue1; }
            set { _ConfigurationValue1 = value; }
        }
        public string ConfigurationValue2
        {
            get { return _ConfigurationValue2; }
            set { _ConfigurationValue2 = value; }
        }
        public bool IsDeleted { get; set; }
    }
}
