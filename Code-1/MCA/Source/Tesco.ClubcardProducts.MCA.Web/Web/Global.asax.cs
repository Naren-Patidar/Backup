using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using NLog;
using System.Reflection;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles,System.Globalization.CultureInfo.CurrentCulture.ToString());
                UnityBootStrapper.Initialize();
                AutoMapperConfiguration.Configure();
                DBConfigurationManager.Instance.LoadConfigurations();
                DBConfigurationManager.Instance.LoadClientSecureConfigKeys();
                
                
                var logConfigProvider = LogConfigProvider.Instance;
                
                // TODO: need to be moved to db config xml file
                string strNlogConfigPath = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("NLogConfigPath");
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strNlogConfigPath), true);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest()
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }
    }
}
