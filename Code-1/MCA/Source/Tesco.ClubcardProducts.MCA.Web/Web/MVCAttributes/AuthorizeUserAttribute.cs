using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.Com.Framework.Security.Tokens;
using System.Web.Mvc;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Controllers;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.MVCAttributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private IConfigurationProvider _Config;

        public AuthorizeUserAttribute()
        {
            _Config = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            LogData logData = new LogData();
            try
            {
                BaseController controller = filterContext.Controller as BaseController;

                string sCustomerID = logData.CustomerID = controller.CustomerId;
                string sDotComCustID = controller.DotcomCustomerId;

                logData.RecordStep(string.Format("AuthorizeUserAttribute - CustomerID : {0} , dotcomCustID:  {1} ", sCustomerID, sDotComCustID));

                string customerID = controller.GetCustomerId().ToString();
                if (string.IsNullOrEmpty(customerID) || (controller.IsDotcomEnvironmentEnabled && string.IsNullOrEmpty(sDotComCustID)))
                {
                    RouteValueDictionary rv = new RouteValueDictionary();
                    rv = new RouteValueDictionary(
                                                 new
                                                 {
                                                     controller = "Account",
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
                controller._logger.Submit(logData);
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException(exp.Message, exp,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
                    });
            }
        }
    }
}
