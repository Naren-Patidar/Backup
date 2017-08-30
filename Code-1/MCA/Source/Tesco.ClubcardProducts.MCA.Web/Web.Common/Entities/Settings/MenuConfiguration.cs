using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    public class MenuConfiguration
    {
        private static MenuConfiguration _links = new MenuConfiguration();
        private static List<DbConfigurationItem> _configurationItems = null;
        public static int iconfigDetailCount = 0;

        private MenuConfiguration()
        {
        }

        /// <summary>
        /// Let Global.asax initialize the Menu Configuration as part of its Application_Start.
        /// </summary>
        /// <param name="configurationItems"></param>
        public static void Initialize(List<DbConfigurationItem> configurationItems)
        {
            _configurationItems = configurationItems;
            iconfigDetailCount = _configurationItems.Count;
        }

        /// <summary>
        /// To be readonly
        /// </summary>
        public static MenuConfiguration Exists
        {
            get { return _links; }
        }

        public bool this[MenuConfigEnum item]
        {
            get
            {
                if (_configurationItems != null)
                {
                    return _configurationItems.Exists(x => x.ConfigurationName == item.ToString());
                }

                return false;
            }
        }

        ///// <summary>
        ///// Returns whether
        ///// </summary>
        ///// <param name="HideView"></param>
        ///// <returns></returns>
        //public bool this[MenuConfigEnum item]
        //{
        //    get
        //    {
        //        DataSet menuConfiguration = GetMenuCookieDataSet();

        //        if (menuConfiguration.Tables.Count > 0)
        //        {
        //            return menuConfiguration.Tables[0].Columns.Contains(item.ToString());
        //        }

        //        return false;
        //    }
        //}

        //private DataSet GetMenuCookieDataSet()
        //{
        //    DataSet menuConfiguration = new DataSet();

        //    string menuXml = MCACookie.Cookie[MCACookieEnum.MenuConfiguration];

        //    if (!string.IsNullOrEmpty(menuXml))
        //    {
        //        //string menuXml = CookieUtility.GetTripleDESEncryptedCookieValue("MenuConfiguration");
        //        XmlDocument resulDoc = new XmlDocument();
        //        resulDoc.LoadXml(menuXml);

        //        menuConfiguration.ReadXml(new XmlNodeReader(resulDoc));
        //    }

        //    return menuConfiguration;
        //}

    }
}
