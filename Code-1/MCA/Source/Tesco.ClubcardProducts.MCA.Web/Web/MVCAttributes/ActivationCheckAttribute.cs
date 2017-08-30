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
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.MVCAttributes
{
    public class ActivationCheckAttribute : AuthorizeAttribute
    {
        private IConfigurationProvider _Config;

        public ActivationCheckAttribute()
        {
            _Config = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            LogData _logdata = new LogData();
            BaseController controller = filterContext.Controller as BaseController;
            try
            {
                if (controller != null)
                {
                    var dotcomCustId = controller.DotcomCustomerId;
                    var customerId = controller.CustomerId;
                    var _logger = controller._logger;
                    _logdata.RecordStep(string.Format("ActivationCheckAttribute -  ActivationDetail: {0}, customerId: {1}, DotcomCustomerId: {2}, Action: {3}"
                        , controller.Activated, customerId, dotcomCustId, filterContext.ActionDescriptor.ActionName));
                    RouteValueDictionary rv = new RouteValueDictionary();
                    switch (controller.Activated)
                    {
                        case ParameterNames.CustomerPending:
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(
                                    new
                                    {
                                        controller = "Activation",
                                        action = "Pending"
                                    })
                                );
                            break;
                        case ParameterNames.CustomerNotactivated:
                            rv = new RouteValueDictionary(
                                                new
                                                {
                                                    controller = "Activation",
                                                    action = "Home"
                                                });
                            if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["from"]))
                            {
                                rv.Add("from", filterContext.HttpContext.Request.QueryString["from"]);
                            }
                            if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["returnUrl"]))
                            {
                                rv.Add("from", filterContext.HttpContext.Request.QueryString["returnUrl"]);
                            }
                            filterContext.Result = new RedirectToRouteResult(rv);
                            break;
                    }
                    switch (controller.CustomerUseStatus)
                    {
                        case ParameterNames.CUSTOMERUSESTATUS_BANNED:
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(
                                    new
                                    {
                                        controller = "Activation",
                                        action = "BannedHousehold"
                                    })
                                );
                            break;
                        case ParameterNames.CUSTOMERUSESTATUS_LEFTSCHEME:
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(
                                    new
                                    {
                                        controller = "Activation",
                                        action = "LeftScheme"
                                    })
                                );
                            break;
                    }
                    _logger.Submit(_logdata);
                }
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException(exp.Message, exp,
                    new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logdata}
                    });
            }
        }
    }
}