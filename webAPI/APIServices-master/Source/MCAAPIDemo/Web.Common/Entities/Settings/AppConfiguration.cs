using System.Configuration;
namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    public class AppConfiguration
    {
        private static AppConfiguration _instance = new AppConfiguration();

        private AppConfiguration()
        {
        }

        /// <summary>
        /// To be readonly
        /// </summary>
        public static AppConfiguration Settings
        {
            get { return _instance; }
        }

        public string this[AppConfigEnum configItem]
        {
            get
            {
                return ConfigurationManager.AppSettings[configItem.ToString()].ToString();
            }
        }
               
    }
}
