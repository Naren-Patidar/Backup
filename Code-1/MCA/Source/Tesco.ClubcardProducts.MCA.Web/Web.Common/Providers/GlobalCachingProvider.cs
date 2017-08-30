using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Runtime.Caching;


namespace Tesco.ClubcardProducts.MCA.Web.Common.Providers
{
    public class GlobalCachingProvider : CachingProviderBase, IGlobalCachingProvider
    {
        #region Singelton (inheriting enabled)

        protected GlobalCachingProvider()
        {

        }

        public static GlobalCachingProvider Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly GlobalCachingProvider instance = new GlobalCachingProvider();
        }

        #endregion

        #region ICachingProvider

        public virtual new void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        public virtual new void AddItem(string key, object value, DateTimeOffset duration)
        {
            base.AddItem(key, value, duration);
        }

        public virtual new void AddItem(string key, object value, CacheItemPolicy _policy)
        {
            base.AddItem(key, value, _policy);
        }

        public virtual object GetItem(string key)
        {
            return base.GetItem(key);
        }

        public virtual new object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

        #endregion

        public string GetAppSetting(string key)
        {
            string appSettingKey = String.Format("appsetting-{0}", key.ToLower());
            var appSettingObject = GlobalCachingProvider.Instance.GetItem(appSettingKey);

            if (appSettingObject == null)
            {
                appSettingObject = ConfigurationManager.AppSettings[key];
                GlobalCachingProvider.Instance.AddItem(appSettingKey, appSettingObject,null);
            }

            string sReturn = String.Empty;
            if (appSettingObject != null)
            {
                sReturn = appSettingObject.ToString();
            }
            return sReturn;
        }
    }
}
