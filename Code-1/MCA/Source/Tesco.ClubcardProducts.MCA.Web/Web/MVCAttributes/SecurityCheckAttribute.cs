using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.Com.Framework.Security.Tokens;
using System.Web.Mvc;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Controllers;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.MVCAttributes
{
    public class SecurityCheckAttribute : AuthorizeAttribute
    {
        private IConfigurationProvider _Config;
        private bool _IsEnterpriseServiceEnabled = false;
        private BaseController _baseCtrl = null;

        public SecurityCheckAttribute()
        {
            _Config = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
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
                var controller = filterContext.Controller as BaseController;
                this._baseCtrl = controller;
                this._IsEnterpriseServiceEnabled = controller.IsEnterpriceServiceCallsEnabled;

                long customerId = controller.CustomerId.TryParse<Int64>();
                var dotcomCustomerID = controller.DotcomCustomerId;
                var currentculture = controller.CurrentCulture;
                var _logger = controller._logger;
                _logData.RecordStep(string.Format("Security Check Attribute - Requested Page : {0}", HttpContext.Current.Request.QueryString["from"]));

                bool issecurityverification = false;

                issecurityverification = ApplySecurity(customerId, dotcomCustomerID, currentculture, filterContext);

                if (!issecurityverification)
                {
                    RouteValueDictionary rv = new RouteValueDictionary(new
                                                                        {
                                                                            controller = "Account",
                                                                            action = "SecurityHome"
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
                _logger.Submit(_logData);
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

        public bool IsDotcomEnvironmentEnabled
        {
            get
            {
                DotcomEnvironmentSettingEnum setting = (DotcomEnvironmentSettingEnum)_Config.GetStringAppSetting(AppConfigEnum.IsDotcomEnvironment).TryParse<Int32>();
                return DotcomEnvironmentSettingEnum.Enable == setting;
            }
        }

        protected DbConfigurationItem IsViewEnabled(DbConfigurationTypeEnum configType, string configName)
        {
            return _Config.GetConfigurations(configType, configName);
        }

        private bool AuthorisationCheck(long customerId, string dotcomCustomerID)
        {
            return customerId != 0 && this._baseCtrl.GetSecurityVerificationStatus();
        }

        private bool ApplySecurity(long customerId, string dotcomCustomerID, string CurrentCulture, AuthorizationContext filterContext)
        {
            LogData _logData = new LogData();
            var vouchercontroller = filterContext.Controller as VouchersController;
            var boostsattescocontroller = filterContext.Controller as BoostsAtTescoController;
            var basecontroller = filterContext.Controller as BaseController;
            var _logger = basecontroller._logger;
            bool isvouchersavailable = false;
            bool isAuthorized = false;

            try
            {
                RouteData currentRoute = HttpContext.Current.Request.RequestContext.RouteData;
                
                string currentPage = !currentRoute.GetRequiredString("action").ToUpper().Equals("HOME") ? 
                                        currentRoute.GetRequiredString("action") : currentRoute.GetRequiredString("controller");

                DbConfigurationItem pageDBconfig = this.IsViewEnabled(DbConfigurationTypeEnum.SecurityCheck, currentPage.ToUpper());

                if (pageDBconfig.ConfigurationValue1.Trim().Equals("0"))
                {
                    if (pageDBconfig.ConfigurationValue2.Trim().Equals("0"))
                    {
                        isAuthorized = this.AuthorisationCheck(customerId, dotcomCustomerID);
                    }
                    else
                    {
                        if (currentPage.ToUpper() == ParameterNames.MY_VOUCHERS.ToUpper())
                        {
                            isvouchersavailable = vouchercontroller.IsUnSpentVouchersExist(customerId, CurrentCulture);
                            isAuthorized = isvouchersavailable ? this.AuthorisationCheck(customerId, dotcomCustomerID) : true;
                        }
                        else if (currentPage.ToUpper() == ParameterNames.MY_BOOSTS.ToUpper())
                        {
                            isvouchersavailable = boostsattescocontroller.IsUnSpentBoostTokensAvailable(customerId);
                            isAuthorized = isvouchersavailable ? this.AuthorisationCheck(customerId, dotcomCustomerID) : true;
                        }
                        else
                        {
                            isAuthorized = this.AuthorisationCheck(customerId, dotcomCustomerID);
                        }
                    }
                }
                else
                {
                    isAuthorized = true;
                }
                    
                _logger.Submit(_logData);
                return isAuthorized;
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


        private bool GetSecurityverificationstatus(long customerId, string dotcomCustomerID)
        {
            bool isverificationdone = true;

            string isSecurityCheckDone = MCACookie.Cookie[MCACookieEnum.IsSecurityCheckDone];

            if (IsDotcomEnvironmentEnabled)
            {
                if (isSecurityCheckDone != "Y_" + dotcomCustomerID + "_" + customerId)
                {
                    isverificationdone = false;
                }
            }
            else
            {
                if (isSecurityCheckDone != "Y_" + customerId)
                {
                    isverificationdone = false;
                }
            }
            return isverificationdone;
        }
    }
}