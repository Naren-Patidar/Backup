using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.Com.Framework.Security.Tokens;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;

namespace MCAAPIDemo.Controllers
{
    public class BaseController : Controller
    {
        AccountBC _AccountProvider;
        CustomerActivationStatusdetails _activationDetail = new CustomerActivationStatusdetails();
        public LoggingService _logger;
        ConfigurationProvider _configProvider = null;

        public BaseController()
        {
            _AccountProvider = new AccountBC();
            _logger = new LoggingService();
            _configProvider = new ConfigurationProvider();
        }
        public ConfigurationProvider ConfigProvider
        {
            get { return _configProvider; }
            set { _configProvider = value; }
        }

        /// <summary>
        /// Property to get the curent culture name
        /// </summary>
        public string CurrentCulture
        {
            get
            {
                LogData _logData = new LogData();
                _logData.RecordStep(string.Format("CurrentCulture : {0}", System.Globalization.CultureInfo.CurrentCulture.Name));
                return System.Globalization.CultureInfo.CurrentCulture.Name;
            }
        }

        /// <summary>
        /// Read only property to check if the dotcom environment is enabled
        /// </summary>
        public bool IsDotcomEnvironmentEnabled
        {
            get
            {
                return ConfigProvider.GetStringAppSetting(AppConfigEnum.IsDotcomEnvironment).Equals(((int)DotcomEnvironmentSettingEnum.Enable).ToString());
            }
        }

        /// <summary>
        /// Read Only property for the Dotcom Customer Id
        /// </summary>
        public string DotcomCustomerId
        {
            get
            {
                try
                {
                    string _dotcomCustId = string.Empty;
                    if (IsDotcomEnvironmentEnabled)
                    {
                        if (_configProvider.GetStringAppSetting(AppConfigEnum.LoginSolution).Equals(ParameterNames.LOGIN_SOLUTION_TYPE_UK))
                        {
                            if (HttpContext.Items[ParameterNames.REFMONITOR_IDENTIFICAION_TOKEN] != null)
                            {
                                if (HttpContext.Items[ParameterNames.REFMONTIOR_AUTH_TOKEN] != null
                                && ((AuthorisationToken)HttpContext.Items[ParameterNames.REFMONTIOR_AUTH_TOKEN]).LoggedInSuccessfully == "1")
                                {
                                    _dotcomCustId = Convert.ToString(((IdentificationToken)HttpContext.Items[ParameterNames.REFMONITOR_IDENTIFICAION_TOKEN]).CustomerID);
                                }
                            }
                        }
                        else if (_configProvider.GetStringAppSetting(AppConfigEnum.LoginSolution).Equals(ParameterNames.LOGIN_SOLUTION_TYPE_GROUP))
                        {
                            if (HttpContext.Items[ParameterNames.IGHS_CUSTOMERIDENTITY] != null)
                            {
                                _dotcomCustId = HttpContext.Items[ParameterNames.IGHS_CUSTOMERIDENTITY].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (TempData["DotcomID"] != null)
                        {
                            _dotcomCustId = TempData["DotcomID"].TryParse<string>();
                        }
                    }
                    return _dotcomCustId;
                }
                catch (Exception ex)
                {
                    throw GeneralUtility.GetCustomException("Failed in BaseController while getting DotcomCustomerId", ex, new Dictionary<string, object>() 
                        { 
                            { LogConfigProvider.EXCLOGDATAKEY, new LogData() }
                        });
                }
            }
        }

        /// <summary>
        /// Read only property to get the MCA Customer Id
        /// </summary>
        public string CustomerId
        {
            get
            {
                LogData _logData = new LogData();
                try
                {
                    string sDotComCustomerID = this.DotcomCustomerId;
                    string _CustId = string.Empty;
                    if (MCACookie.Cookie[MCACookieEnum.DotcomCustomerID] == null ||
                        (MCACookie.Cookie[MCACookieEnum.DotcomCustomerID] != null && sDotComCustomerID != MCACookie.Cookie[MCACookieEnum.DotcomCustomerID]))
                    {
                        if (!string.IsNullOrWhiteSpace(sDotComCustomerID))
                        {
                            MCACookie.Cookie.Add(MCACookieEnum.DotcomCustomerID, sDotComCustomerID);
                            _AccountProvider.ParseActivationStatus(ref _activationDetail, sDotComCustomerID, CurrentCulture);
                            MCACookie.Cookie.Add(MCACookieEnum.Activated, _activationDetail.Activated.TryParse<string>());
                            MCACookie.Cookie.Add(MCACookieEnum.CustomerMailStatus, _activationDetail.CustomerMailStatus.TryParse<string>());
                            MCACookie.Cookie.Add(MCACookieEnum.CustomerUseStatus, _activationDetail.CustomerUseStatus.TryParse<string>());
                            string customerID = _activationDetail.CustomerId.TryParse<string>();
                            if (!string.IsNullOrEmpty(customerID))
                            {
                                MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID);
                            }
                            _logData.CaptureData("Activation Details: ", _activationDetail);
                        }
                    }
                    _logger.Submit(_logData);
                    return MCACookie.Cookie[MCACookieEnum.CustomerID];
                }
                catch (Exception ex)
                {
                    throw GeneralUtility.GetCustomException("Failed in BaseController while getting CustomerId", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
                }
            }
        }

        #region GetCustomerId Code

        public long GetCustomerId()
        {
            LogData _logData = new LogData();
            long customerId = 0;
            try
            {

                if (!string.IsNullOrEmpty(MCACookie.Cookie[MCACookieEnum.CustomerID]))
                {
                    customerId = Convert.ToInt64(MCACookie.Cookie[MCACookieEnum.CustomerID]);
                }
                _logData.CaptureData("CustomerID ", CustomerId);
                _logger.Submit(_logData);
                return customerId;
            }
            catch (Exception ex)
            {

                throw GeneralUtility.GetCustomException("Failed in BaseController while getting CustomerId", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });

            }
        }

        #endregion

        public string oAuthToken
        {
            get
            {
                return Request.Cookies[ParameterNames.OAUTH_TOKEN] == null ? string.Empty : Request.Cookies[ParameterNames.OAUTH_TOKEN].Value;
            }
        }
    }
}