using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.Com.Framework.Security.Tokens;
using System.Web.Mvc;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using MCAAPIDemo.Controllers;

namespace Tesco.ClubcardProducts.MCA.Web.MVCAttributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private ConfigurationProvider _Config;

        public AuthorizeUserAttribute()
        {
            _Config = new ConfigurationProvider();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            LogData _logData = new LogData();
            try
            {
                BaseController controller = filterContext.Controller as BaseController;
                _logData.RecordStep(string.Format("AuthorizeUserAttribute - CustomerID : {0} , dotcomCustID:  {1} ", controller.CustomerId, controller.DotcomCustomerId));

                string customerID = MCACookie.Cookie[MCACookieEnum.CustomerID];
                if (string.IsNullOrEmpty(customerID) || (controller.IsDotcomEnvironmentEnabled && string.IsNullOrEmpty(controller.DotcomCustomerId)))
                {
                    RouteValueDictionary rv = new RouteValueDictionary();
                    rv = new RouteValueDictionary(
                                                 new
                                                 {
                                                     controller = "MCA",
                                                     action = controller.IsDotcomEnvironmentEnabled ? "DotcomLogin" : "Login",
                                                 });

                    if (_Config.GetStringAppSetting(AppConfigEnum.LoginSolution).Equals(ParameterNames.LOGIN_SOLUTION_TYPE_UK))
                    {
                        rv.Add("from", HttpContext.Current.Request.Url.ParseUri());
                    }
                    if (_Config.GetStringAppSetting(AppConfigEnum.LoginSolution).Equals(ParameterNames.LOGIN_SOLUTION_TYPE_GROUP))
                    {
                        rv.Add("returnUrl", HttpContext.Current.Request.Url.ParseUri());
                    }
                    filterContext.Result = new RedirectToRouteResult(rv);
                }
                controller._logger.Submit(_logData);
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException(exp.Message, exp,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
        }
    }
}
