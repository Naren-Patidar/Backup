using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.Com.Framework.Security.Tokens;
using System.Web.Routing;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Net;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity;
using System.Web.Caching;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class BaseController : Controller
    {

        #region GlobalMembers

        IAccountBC _accountProvider;
        CustomerActivationStatusdetails _activationDetail = new CustomerActivationStatusdetails();
        public ILoggingService _logger;
        IConfigurationProvider _configProvider = null;
        ValidationDetails validationDetails = null;

        #endregion GlobalMembers

        #region Constructors


        public BaseController(ILoggingService _loggerService, IAccountBC _IAccountBC, IConfigurationProvider _configurationProvider)
        {
            _accountProvider = _IAccountBC;
            _logger = _loggerService;
            _configProvider = _configurationProvider;
        }

        public BaseController()
        {
            _accountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
            _logger = ServiceLocator.Current.GetInstance<ILoggingService>();
            _configProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
        }

        #endregion Constructors

        #region Public Members

        #region Props

        protected IConfigurationProvider ConfigProvider
        {
            get { return _configProvider; }
            set { _configProvider = value; }
        }

        public string CurrentCulture
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.Name;
            }
        }

        public bool IsDotcomEnvironmentEnabled
        {
            get
            {
                return ConfigProvider.GetStringAppSetting(AppConfigEnum.IsDotcomEnvironment).Equals(((int)DotcomEnvironmentSettingEnum.Enable).ToString());
            }
        }

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
                            _dotcomCustId = this.IsEnterpriceServiceCallsEnabled ? this.GetDotComIDFromIdentity() : this.GetDotComIDFromReffMon();
                        }
                        else if (_configProvider.GetStringAppSetting(AppConfigEnum.LoginSolution).Equals(ParameterNames.LOGIN_SOLUTION_TYPE_GROUP))
                        {
                            RouteData currentRoute = System.Web.HttpContext.Current.Request.RequestContext.RouteData;
                            string currentModule = currentRoute.GetRequiredString("controller");
                            HttpContextWrapper httpContextWrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
                            if (currentModule.ToUpper() != "JOIN")
                            {
                                _dotcomCustId = this.GetUserIdentityInfo();
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
                    System.Web.HttpContext.Current.Session["dcid"] = _dotcomCustId;
                    return _dotcomCustId;
                }
                catch (Exception ex)
                {
                    throw GeneralUtility.GetCustomException("Failed in BaseController while getting DotcomCustomerId", ex,
                        new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, new LogData() }
                            });
                }
            }
        }

        public string CustomerId
        {
            get
            {
                LogData logData = new LogData();
                
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    string custID = logData.CustomerID = this.GetSecurityItem("CustomerID");
                    logData.RecordStep("CustomerID from GetSecurityItem:" + custID);
                    System.Web.HttpContext.Current.Session["cid"] = custID;
                    _logger.Submit(logData);
                    return custID;
                }
                else
                {
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
                                _accountProvider.ParseActivationStatus(ref _activationDetail, sDotComCustomerID, CurrentCulture);
                                MCACookie.Cookie.Add(MCACookieEnum.Activated, _activationDetail.Activated.TryParse<string>());
                                MCACookie.Cookie.Add(MCACookieEnum.CustomerMailStatus, _activationDetail.CustomerMailStatus.TryParse<string>());
                                MCACookie.Cookie.Add(MCACookieEnum.CustomerUseStatus, _activationDetail.CustomerUseStatus.TryParse<string>());
                                string customerID = _activationDetail.CustomerId.TryParse<string>();
                                if (!string.IsNullOrEmpty(customerID))
                                {
                                    MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID);
                                }
                                logData.CaptureData("Activation Details: ", _activationDetail);
                            }
                        }
                        string custID = logData.CustomerID = MCACookie.Cookie[MCACookieEnum.CustomerID];
                        _logger.Submit(logData);
                        return custID;
                    }
                    catch (Exception ex)
                    {
                        throw GeneralUtility.GetCustomException("Failed in BaseController while getting CustomerId", ex, 
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
                    }
                }
            }
        }

        public char Activated
        {
            get
            {
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    return String.Compare(this.GetSecurityItem("IsActivated"), "True", true) == 0 ? 'Y' : 'N';
                    
                }
                else
                {
                    return MCACookie.Cookie[MCACookieEnum.Activated].TryParse<char>();
                }
            }
        }

        public int CustomerUseStatus 
        { 
            get 
            {
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    return this.GetSecurityItem("CustomerUseStatus").TryParse<int>();
                }
                else
                {
                    return MCACookie.Cookie[MCACookieEnum.CustomerUseStatus].TryParse<int>();
                }
            } 
        }

        public int CustomerMailStatus
        {
            get
            {
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    return this.GetSecurityItem("CustomerMailStatus").TryParse<int>();
                }
                else
                {
                    return MCACookie.Cookie[MCACookieEnum.CustomerMailStatus].TryParse<int>();
                }
            }
        }

        public bool IsEnterpriceServiceCallsEnabled
        {
            get
            {
                return ConfigProvider.GetStringAppSetting(AppConfigEnum.IsEnterpriceServiceCallsEnabled) == "1";
            }
        }

        public bool GetSecurityVerificationStatus()
        {
            LogData logData = new LogData();
            bool bIsVerificationdone = true;

            try
            {
                long customerId = this.CustomerId.TryParse<Int64>();
                logData.CustomerID = customerId.ToString();

                string dotcomCustomerID = this.DotcomCustomerId;

                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    return String.Compare(this.GetSecurityItem("HasCleared2LA"), "true", true) == 0;
                }
                else
                {
                    string isSecurityCheckDone = MCACookie.Cookie[MCACookieEnum.IsSecurityCheckDone];
                    if (this.IsDotcomEnvironmentEnabled)
                    {
                        logData.RecordStep(string.Format(" customerId :{0}, dotcomCustomerID :{1}, IsDotcomEnvironmentEnabled :{2}, isSecurityCheckDone : {3}",
                            customerId, dotcomCustomerID, IsDotcomEnvironmentEnabled, isSecurityCheckDone));

                        if (isSecurityCheckDone != "Y_" + dotcomCustomerID + "_" + customerId)
                        {
                            bIsVerificationdone = false;
                        }
                    }
                    else if (customerId != 0)
                    {
                        logData.RecordStep("Dotcom Environment is not enabled and customer ID is not zero");

                        if (isSecurityCheckDone != "Y_" + customerId)
                        {
                            bIsVerificationdone = false;
                        }
                    }
                }
                logData.RecordStep(string.Format(" isverificationdone :{0}", bIsVerificationdone));
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller while getting security Verification Status", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return bIsVerificationdone;
        }

        #endregion Props

        #region Functions

        /*
        protected void GetRequestedPage()
        {
            LogData _logData = new LogData();

            try
            {
                string pagename = HttpContext.Request.Url.PathAndQuery;
                string Defaultpage = "/Clubcard/MyAccount/";


                if (pagename.ToUpper().Contains(ParameterNames.MY_PERSONAL_DETAILS.ToUpper()))
                {
                    TempData["PageName"] = ParameterNames.MY_PERSONAL_DETAILS;
                }
                else if (pagename.ToUpper().Contains(ParameterNames.MY_VOUCHERS.ToUpper()))
                {
                    TempData["PageName"] = ParameterNames.MY_VOUCHERS;
                }
                else if (pagename.ToUpper().Contains(ParameterNames.MY_BOOSTS.ToUpper()))
                {
                    TempData["PageName"] = ParameterNames.MY_BOOSTS;
                }
                else if (pagename.ToUpper().Contains(ParameterNames.HOME_PAGE.ToUpper()) || pagename.ToUpper().Contains(Defaultpage.ToUpper()))
                {
                    TempData["PageName"] = ParameterNames.HOME_PAGE;
                }
                _logData.CaptureData("Page Name", TempData["PageName"]);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BaseController while getting Requested Page.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }
        */

        public long GetCustomerId()
        {
            LogData logData = new LogData();
            long customerId = default(long);
            try
            {
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    long custID = this.CustomerId.TryParse<Int64>();
                    logData.CustomerID = custID.ToString();
                    _logger.Submit(logData);
                    return custID;
                }
                else
                {
                    if (!string.IsNullOrEmpty(MCACookie.Cookie[MCACookieEnum.CustomerID]))
                    {
                        customerId = MCACookie.Cookie[MCACookieEnum.CustomerID].TryParse<long>();
                    }
                    logData.CaptureData("CustomerID ", CustomerId);
                    _logger.Submit(logData);
                    return customerId;
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BaseController while getting CustomerId", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        protected virtual Tuple<bool, string> ValidateCaptcha()
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];

            var capchaTest = ValidateGoogleCaptcha.Validate(EncodedResponse);

            string errors = capchaTest.ErrorCodes != null ? String.Join(", ", capchaTest.ErrorCodes) : String.Empty;

            return new Tuple<bool, string>(capchaTest.Success.ToLower() == "true", errors);
        }

        protected virtual void ClearCookie(string key)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }

        #endregion Functions

        #endregion Public Members

        #region Overrides

        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            _logger.ErrorException(exception);
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var baseException = filterContext.Exception.GetBaseException();
            var statusCode = (int)HttpStatusCode.InternalServerError;
            if (baseException is HttpException)
            {
                statusCode = ((HttpException)baseException).GetHttpCode();
            }

            ViewResult errorView = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
            errorView.ViewBag.ErrorCode = statusCode;
            filterContext.Result = errorView;

            // Prepare the response code.
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsDotcomEnvironmentEnabled)
            {
                ViewBag.DotcomID = TempData["DotcomID"];
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string servedby;
            base.OnActionExecuted(filterContext);
            servedby = this.ServerIdentifier;
            Response.AppendHeader("CCOFW", servedby);
            ViewBag.IsGARequired = _configProvider.GetStringAppSetting(AppConfigEnum.IsGoogleAnalyticsRequired);
            ViewBag.DotcomCustomerId = DotcomCustomerId;
            if (!IsDotcomEnvironmentEnabled)
            {
                TempData["DotcomID"] = ViewBag.DotcomID;
            }

            var dccookieVal = System.Configuration.ConfigurationManager.AppSettings[ParameterNames.RESPONSE_COOKIES];

            if (!String.IsNullOrWhiteSpace(dccookieVal))
            {
                string[] elements = dccookieVal.Split(new string[] { "###"}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string el in elements)
                {
                    string[] subelements = el.Split('=');
                    if (subelements.Length == 2 && !String.IsNullOrWhiteSpace(subelements[0]) && !String.IsNullOrWhiteSpace(subelements[1]))
                    {
                        System.Web.HttpContext.Current.Response.Cookies.Set(
                                    new HttpCookie(subelements[0], subelements[1])
                                    {
                                        HttpOnly = true,
                                        Secure = true
                                    });
                    }
                }
            }
        }

        #endregion Overrides

        #region Private Members

        #region Props

        private string ServerIdentifier
        {
            get
            {
                IPHostEntry host;
                string localIP = "?";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }

                if (!String.IsNullOrWhiteSpace(localIP) && localIP.LastIndexOf(".") > 0)
                {
                    int istartIndex = localIP.LastIndexOf(".");
                    localIP = localIP.Substring(istartIndex, localIP.Length - istartIndex);
                    localIP = localIP.Replace(".", "");
                }
                return localIP;
            }
        }

        private string OAuthToken
        {
            get
            {
                return Request.Cookies[ParameterNames.OAUTH_TOKEN] == null ? String.Empty : Request.Cookies[ParameterNames.OAUTH_TOKEN].Value;
            }
        }

        private string LoginUUID
        {
            get
            {
                return Request.Cookies[ParameterNames.UUID] == null ? String.Empty : Request.Cookies[ParameterNames.UUID].Value;
            }
        }

        #endregion Props

        #region Functions

        private string GetDotComIDFromReffMon()
        {
            string dotcomIDNGC = string.Empty;
            if (HttpContext.Items[ParameterNames.REFMONITOR_IDENTIFICAION_TOKEN] != null)
            {
                if (HttpContext.Items[ParameterNames.REFMONTIOR_AUTH_TOKEN] != null
                && ((AuthorisationToken)HttpContext.Items[ParameterNames.REFMONTIOR_AUTH_TOKEN]).LoggedInSuccessfully == "1")
                {
                    dotcomIDNGC = Convert.ToString(((IdentificationToken)HttpContext.Items[ParameterNames.REFMONITOR_IDENTIFICAION_TOKEN]).CustomerID);
                }
            }
            return dotcomIDNGC;
        }

        private string GetDotComIDFromIdentity()
        {
            return this.GetSecurityItem("DotcomID");
        }

        private string GetMCAServiceToken()
        {
            string keyToken = "mca_service_token";
            LogData logData = new LogData();
            MCAServiceTokenRes resServiceToken = null;
            string rMCAServiceToken = String.Empty;

            try
            {
                object cachedMCAServiceToken = HttpContext.Cache.Get(keyToken);

                if (cachedMCAServiceToken == null)
                {
                    resServiceToken = this._accountProvider.GetMCAServiceToken();
                    logData.CaptureData("MCAServicetoken details: ", resServiceToken);

                    if (resServiceToken != null && !String.IsNullOrEmpty(resServiceToken.access_token))
                    {
                        rMCAServiceToken = resServiceToken.access_token;

                        logData.RecordStep(String.Format("Saving token in cache for duration - {0} seconds", resServiceToken.expires_in));

                        HttpContext.Cache.Insert(
                                            keyToken,
                                            resServiceToken.access_token,
                                            null,
                                            DateTime.Now.AddSeconds(resServiceToken.expires_in),
                                            TimeSpan.Zero);
                    }

                    logData.RecordStep("MCAServiceToken from service:" + resServiceToken.access_token);
                }
                else
                {
                    logData.RecordStep("cachedMCAServiceToken:" + cachedMCAServiceToken.ToString());
                    rMCAServiceToken = cachedMCAServiceToken.ToString();
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting MCA service token details from identity service", ex,
                    new Dictionary<string, object>() 
                        { 
                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                        });
            }
            _logger.Submit(logData);
            return rMCAServiceToken;

        }

        private string GetDotcomIDFromLegacyService()
        {
            LogData logData = new LogData();
            string dotcomID = string.Empty;

            try
            {
                string mcaAccessToken = this.GetMCAServiceToken();
                LegacyLookupRes legacyLookupRes = this._accountProvider.GetDotComIDFromLookup(mcaAccessToken, this.LoginUUID);
                if (legacyLookupRes != null)
                {
                    dotcomID = legacyLookupRes.fields.Single(x => 
                                String.Compare(x.Id, ParameterNames.DXSH_CUSTOMERID, true) == 0).Value;
                }

                logData.CaptureData("Legacy lookup call details: ", legacyLookupRes);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting dotcom ID from identity service", ex,
                 new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

            return dotcomID;
        }

        private SecurityDefinition CheckTokenValidity()
        {
            LogData logData = new LogData();
            CustomerActivationStatusdetails activationDetail = new CustomerActivationStatusdetails();
            try
            {
                validationDetails = this._accountProvider.ValidateToken(this.OAuthToken);
                logData.CaptureData("IdentityServiceCall details: ", validationDetails);

                if ((validationDetails != null) && (validationDetails.Status.ToUpper().Equals("VALID")))
                {
                    logData.RecordStep("Validation status is valid");

                    if (!String.IsNullOrWhiteSpace(validationDetails.UserId)
                        && validationDetails.UserId.Replace("trn:tesco:uid:uuid:", String.Empty)
                        .Equals(this.LoginUUID, StringComparison.InvariantCultureIgnoreCase))
                    {
                        logData.RecordStep("Validate token uuid matches browser cookie uuid");
                        
                        string dotcomID = this.GetDotcomIDFromLegacyService();

                        logData.RecordStep(String.Format("DotcomIDFromLegacyLookup - {0}", dotcomID));
                        
                        this._accountProvider.ParseActivationStatus(ref activationDetail, dotcomID, this.CurrentCulture);
                        
                        return this.SetTokenPayload(activationDetail, dotcomID, false, validationDetails.Expiration.TryParse<long>());
                    }
                    else
                    {
                        logData.RecordStep("Validate token uuid doesn't match browser cookie uuid");
                        throw new MCASecurityException(SecurityErrors.E_401_6);
                    }
                }
                else
                {
                    logData.RecordStep("Either validation details response from identity is blank or the status is not valid");
                    throw new MCASecurityException(SecurityErrors.E_401_5);
                }
            }
            finally
            {
                this._logger.Submit(logData);
            }
        }

        protected SecurityDefinition SetTokenPayload(CustomerActivationStatusdetails activationStatus, string dotcomID, 
                                                        bool cleared2LA, long expiration)
        {
            SecurityDefinition secDef = null;
            SecurityHelper shelp = new SecurityHelper();
            LogData logData = new LogData();
            logData.RecordStep(new { activationStatus = activationStatus, dotcomID = dotcomID, cleared2LA = cleared2LA, expiration = expiration}.JsonText());
            try
            {
                secDef = shelp.GetTokenPayLoad();
                if (secDef == null)
                {
                    logData.RecordStep("Payload is null");

                    secDef = new SecurityDefinition();
                    if (expiration == default(long))
                    {
                        logData.RecordStep(SecurityErrors.E_401_7.ToString());
                        throw new MCASecurityException(SecurityErrors.E_401_7);
                    }

                    if (String.IsNullOrWhiteSpace(dotcomID))
                    {
                        logData.RecordStep(SecurityErrors.E_401_8.ToString());
                        throw new MCASecurityException(SecurityErrors.E_401_8);
                    }

                    if (activationStatus == null)
                    {
                        logData.RecordStep(SecurityErrors.E_401_9.ToString());
                        throw new MCASecurityException(SecurityErrors.E_401_9);
                    }
                }

                if (expiration != default(long))
                {
                    secDef.exp = expiration;
                }

                if (!String.IsNullOrWhiteSpace(dotcomID))
                {
                    secDef.DotcomID = dotcomID;
                }

                if (!secDef.HasCleared2LA)
                {
                    secDef.HasCleared2LA = cleared2LA;
                }

                secDef.AccessToken = this.OAuthToken;
                secDef.UUID = this.LoginUUID;

                if (activationStatus != null)
                {
                    secDef.IsActivated = activationStatus.Activated == 'Y';
                    secDef.IsBanned = activationStatus.CustomerUseStatus == ParameterNames.CUSTOMERUSESTATUS_BANNED;
                    secDef.CustomerID = activationStatus.CustomerId.ToString();
                    secDef.CustomerMailStatus = activationStatus.CustomerMailStatus.ToString();
                    secDef.CustomerUseStatus = activationStatus.CustomerUseStatus.ToString();
                    secDef.HasLeftScheme = activationStatus.CustomerUseStatus == ParameterNames.CUSTOMERUSESTATUS_LEFTSCHEME;
                }

                var token = JWTSerializerUtility.JWTSserialize(secDef);

                this.ClearCookie(ParameterNames.JWT_TOKEN);
                System.Web.HttpContext.Current.Response.Cookies.Set(
                        new HttpCookie(ParameterNames.JWT_TOKEN, token)
                        {
                            HttpOnly = true,
                            Secure = true
                        });

                //MCACookie.Cookie.Remove(MCACookieEnum.JWToken);
                //MCACookie.Cookie.Add(MCACookieEnum.JWToken, token);
                logData.RecordStep("Token updated");
            }
            finally
            {
                this._logger.Submit(logData);
            }
            return secDef;
        }
        
        private bool ContinueLoginRedirect()
        {
            try
            {
                int iCount = 0;
                var retryCount = MCACookie.Cookie[MCACookieEnum.LoginRetry];
                if (!String.IsNullOrWhiteSpace(retryCount) && Int32.TryParse(retryCount, out iCount) && iCount > 3)
                {
                    return false;
                }
                else
                {
                    MCACookie.Cookie.Add(MCACookieEnum.LoginRetry, (iCount + 1).ToString());
                    return true;
                }
            }
            catch { }
            return true;
        }
        
        /*
        private SecurityDefinition GetTokenPayLoad()
        {
            SecurityDefinition secDef = null;
            LogData logData = new LogData();
            try
            {
                var token = System.Web.HttpContext.Current.Request.Cookies.Get(ParameterNames.JWT_TOKEN);

                if (token == null || String.IsNullOrWhiteSpace(token.Value))
                {
                    logData.RecordStep("Token empty in request. Checking response...");
                    token = System.Web.HttpContext.Current.Response.Cookies.Get(ParameterNames.JWT_TOKEN);
                    if (token == null || String.IsNullOrWhiteSpace(token.Value))
                    {
                        logData.RecordStep("Token empty. Cannot proceed.");
                        return secDef;
                    }
                }

                try
                {
                    secDef = JWTSerializerUtility.JWTDeserialize(token.Value);
                    logData.RecordStep("Token serialized");
                }
                catch (TokenExpiredException)
                {
                    logData.RecordStep("Token expired");
                }
                catch (SignatureVerificationException)
                {
                    logData.RecordStep("Token has invalid signature");
                }

                return secDef;
            }
            finally
            {
                this._logger.Submit(logData);
            }
        }
        */

        private string GetSecurityItem(string item)
        {
            LogData logData = new LogData();
            logData.RecordStep("GetSecurityItem");
            SecurityHelper shelp = new SecurityHelper();
            try
            {
                if (String.IsNullOrWhiteSpace(this.LoginUUID))
                {
                    throw new MCASecurityException(SecurityErrors.E_401_1);
                }

                if (String.IsNullOrWhiteSpace(this.OAuthToken))
                {
                    throw new MCASecurityException(SecurityErrors.E_401_2);
                }

                SecurityDefinition secDef = shelp.GetTokenPayLoad();

                if (secDef == null || String.IsNullOrWhiteSpace(secDef.UUID) || String.IsNullOrWhiteSpace(secDef.AccessToken))
                {
                    logData.RecordStep("payload is null");
                    secDef = this.CheckTokenValidity();
                }
                else if (secDef.AccessToken != this.OAuthToken)
                {                    
                    this.ClearCookie(ParameterNames.JWT_TOKEN);
                    throw new MCASecurityException(SecurityErrors.E_401_10);
                }
                else if (secDef.UUID != this.LoginUUID)
                {
                    this.ClearCookie(ParameterNames.JWT_TOKEN);
                    throw new MCASecurityException(SecurityErrors.E_401_11);
                }
                else if (String.IsNullOrWhiteSpace(secDef.DotcomID))
                {
                    this.ClearCookie(ParameterNames.JWT_TOKEN);
                    throw new MCASecurityException(SecurityErrors.E_401_12);
                }
                else if (String.IsNullOrWhiteSpace(secDef.CustomerID) || secDef.CustomerID == "0")
                {
                    logData.RecordStep("CustomerID in payload is null or zero");
                    CustomerActivationStatusdetails activationDetail = new CustomerActivationStatusdetails();
                    this._accountProvider.ParseActivationStatus(ref activationDetail, secDef.DotcomID, this.CurrentCulture);
                    logData.CaptureData("ActivationdetailsObject:", activationDetail);
                    secDef = this.SetTokenPayload(activationDetail, secDef.DotcomID, false, default(long));
                }

                logData.CaptureData("payloadObject:", secDef);
                string rVal = shelp.GetPropertyFromObject(item, secDef);
                MCACookie.Cookie.Remove(MCACookieEnum.LoginRetry);
                return rVal;
            }
            catch (MCASecurityException exSec)
            {
                if (this.ContinueLoginRedirect())
                {
                    RedirectToAction("DotcomLogin", "Account");
                    return String.Empty;
                }
                else
                {
                    throw GeneralUtility.GetCustomException(String.Format("Encountered Security Exception - {0}", exSec.Error.ToString()),
                        exSec, new Dictionary<string, object>() 
                                    { 
                                        { LogConfigProvider.EXCLOGDATAKEY, logData }
                                    });
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Unknown exception while trying to access property from JWT", ex,
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private string GetUserIdentityInfo()
        {
            LogData _logData = new LogData();
            string dotcomid = string.Empty;
            try
            {
                string valueforGivenKey = _configProvider.GetStringAppSetting(AppConfigEnum.anonCookieName);

                HttpCookieCollection cookies = Request.Cookies;
                if (cookies[valueforGivenKey] != null)
                {
                    string auidcookie = cookies.Get(valueforGivenKey).Value;

                    if (String.IsNullOrWhiteSpace(auidcookie))
                    {
                        _logData.RecordStep("Auidcookie cookie value is null or empty");
                    }
                    else
                    {
                        _logData.CaptureData("Auidcookie:", auidcookie);
                        dotcomid = this._accountProvider.GetIdentityInfomation(auidcookie);
                    }
                }
                else
                {
                    _logData.CaptureData("Auidcookie cookie  is not available ", "");
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BaseController while getting GetUserIdentityInfo", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return dotcomid;
        }

        #endregion Functions

        #endregion Private Members
        
    }
}
