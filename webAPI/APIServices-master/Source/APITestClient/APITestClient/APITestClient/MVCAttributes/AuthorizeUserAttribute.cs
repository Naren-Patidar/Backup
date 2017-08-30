using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.Com.Framework.Security.Tokens;
using System.Web.Mvc;
using System.Web.Routing;
using APITestClient.Controllers;
using APITestClient.Helper;
using System.Configuration;

namespace APITestClient.MVCAttributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            try
            {
                BaseController controller = filterContext.Controller as BaseController;

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

                    if (ConfigurationManager.AppSettings["GenericLoginPage"].Equals(ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_UK"]))
                    {
                        rv.Add("from", HttpContext.Current.Request.Url.ParseUri());
                    }
                    if (ConfigurationManager.AppSettings["GenericLoginPage"].Equals(ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_GROUP"]))
                    {
                        rv.Add("returnUrl", HttpContext.Current.Request.Url.ParseUri());
                    }
                    filterContext.Result = new RedirectToRouteResult(rv);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
