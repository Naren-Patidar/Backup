using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web;
using System.Web.Optimization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.IO;

namespace MCAAPIDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles, System.Globalization.CultureInfo.CurrentCulture.ToString());

            DBConfigurationManager.Instance.LoadConfigurations();
            var logConfigProvider = LogConfigProvider.Instance;

            // TODO: need to be moved to db config xml file
            string strNlogConfigPath = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("NLogConfigPath");
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strNlogConfigPath), true);
        }
    }
}