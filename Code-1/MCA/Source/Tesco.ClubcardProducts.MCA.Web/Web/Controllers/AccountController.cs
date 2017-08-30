using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Activation = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class AccountController : BaseController
    {
        DBConfigurations SecurityPageConfiguration;
        DbConfigurationItem config = new DbConfigurationItem();

        private readonly IAccountBC _AccountProvider;
        public string firstDigitvalue;
        public string secondDigitvalue;
        public string thirdDigitvalue;
        ILoggingService _AuditLoggerService = null;

        public AccountController()
        {
            _AccountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
            _AuditLoggerService = new AuditLoggingService();
        }

        public AccountController(IAccountBC _IAccountBC, ILoggingService _auditlogger, IConfigurationProvider configProvider)
            : base(_auditlogger, _IAccountBC, configProvider)
        {
            _AccountProvider = _IAccountBC;
            _AuditLoggerService = _auditlogger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string from, string returnUrl)
        {
            LogData _logData = new LogData();
            LoginViewModel model = new LoginViewModel();
            try
            {
                if (IsDotcomEnvironmentEnabled)
                {
                    _logger.Submit(_logData);
                    Redirect(ConfigProvider.GetStringAppSetting(AppConfigEnum.GenericLogoutPage));
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller|Login|GET", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login, string from, string returnUrl)
        {
            LogData logData = new LogData();
            try
            {
                long customerID = 0, clubcard = 0;
                string password = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.Password.ToString()].ConfigurationValue1.ToString();
                string sculture = base.CurrentCulture;
                logData.RecordStep(string.Format("CurrentCulture : {0}", sculture));

                if (password != string.Empty && password == login.Password)
                {
                    if (!string.IsNullOrEmpty(login.ClubcardNumber) && Int64.TryParse(login.ClubcardNumber, out clubcard))
                    {
                        customerID = _AccountProvider.GetCustomerId(clubcard, 100, sculture);
                        logData.CustomerID = customerID.ToString();
                    }
                    if (customerID != 0)
                    {
                        ViewBag.DotcomID = login.DotcomCustomerId;
                        MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID.ToString());
                        return GetAction(from);
                    }
                    else
                    {
                        logData.RecordStep("Customer ID is either 0 or null/empty");
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                else
                {
                    logData.RecordStep("Password is either empty or doesn't match");
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller|Login|POST", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        [HttpGet]
        public ActionResult DotcomLogin(string from, string returnUrl)
        {
            LogData logData = new LogData();
            try
            {
                IConfigurationProvider config = new Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.ConfigurationProvider();

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    from = returnUrl;
                }

                ViewBag.LoginUrl = string.Format(config.GetStringAppSetting(AppConfigEnum.GenericLoginPage), from);

                if (string.IsNullOrWhiteSpace(config.GetStringAppSetting(AppConfigEnum.GenericLoginPage)) 
                    && config.GetStringAppSetting(AppConfigEnum.GenericLoginPage).Length < 10)
                {
                    logData.RecordStep(string.Format("Login URL is empty: {0}", config.GetStringAppSetting(AppConfigEnum.GenericLoginPage)));
                }

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller|DotcomLogin|GET", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return View();
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        public ActionResult SecurityHome(string target, string from, string returnUrl)
        {
            LogData logData = new LogData();
            try
            {
                string customerId = base.CustomerId;
                logData.CustomerID = customerId;

                if (this.GetSecurityVerificationStatus())
                {
                    logData.RecordStep(string.Format("issecuritycheckdone: {0}", true));
                    _logger.Submit(logData);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        from = returnUrl;
                    }
                    return GetAction(from);
                }
                else
                {
                    SecurityViewModel model = this.ValidateClubcardSecurity(customerId.ToString());
                    TempData["Target"] = target;
                    _logger.Submit(logData);
                    return View("SecurityHome", model);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller while getting Security Page  ", ex,
                             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        public ActionResult SecurityHome(SecurityViewModel model, string target, string from, string returnUrl)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();
            string dotcomCustomerId = string.Empty;
            bool isSuccessful = false;
            TempData["Target"] = target;

            try
            {
                string custID = base.GetCustomerId().ToString();
                logData.CustomerID = logDataAudit.CustomerID = custID;
                string sDotcomid = this.DotcomCustomerId;

                int VirtualLockThreshold = 0;
                if(!string.IsNullOrEmpty(DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue2))
                {
                    Int32.TryParse(DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue2, 
                        out VirtualLockThreshold);
                }

                bool isVirtualLock = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue1.Equals("1");

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    from = returnUrl;
                }
                SecurityViewModel modelTemp = this.ValidateClubcardSecurity(custID);

                if (modelTemp.ShowCaptcha)
                {
                    var capchaTest = base.ValidateCaptcha();

                    if (!capchaTest.Item1)
                    {
                        modelTemp.showCaptchaError = true;
                        logData.RecordStep(String.Format("Captcha Error(s): {0}", capchaTest.Item2));
                        _logger.Submit(logData);
                        logDataAudit.RecordStep("False|Captcha Validation Failed");

                        return View("SecurityHome", modelTemp);
                    }
                }

                if (!string.IsNullOrEmpty(model.txtfirstSecureDigit) &&
                    !string.IsNullOrEmpty(model.txtsecondSecureDigit) &&
                    !string.IsNullOrEmpty(model.txtthirdSecureDigit))
                {
                    string strHostName = System.Net.Dns.GetHostName();
                    string sClientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    string sPageRequested = TempData["PageName"].TryParse<string>();
                    string sActionName = TempData["ActionName"].TryParse<string>();

                    long customerId = custID.TryParse<Int64>();

                    dotcomCustomerId = sDotcomid;

                    logData.RecordStep(string.Format("Dotcom Customer ID : {0}, PageName : {1}", dotcomCustomerId, sPageRequested));

                    CustomerSecurityAttemptAudit attempt = new CustomerSecurityAttemptAudit()
                    {
                        CustomerID = customerId,
                        Browserused = Request.Browser.Browser.ToString(),
                        IPAddress = sClientIPAddress,
                        IsValidAttempt = "N"
                    };

                    var securityStatus = this.CheckSecurityStatus(custID);
                    model.FailedLoginAttempts = securityStatus.Item2;

                    // changes for CCMCA-4454- check if input is valid else do not process security attempt
                    if (!this.GetEncryptyvalue(model) || !ModelState.IsValid)
                    {
                        attempt.IsValidAttempt = "N";
                        this.RecordSecurityAttemptInAudit(attempt);
                        isSuccessful = false;
                    }
                    else
                    {
                        CustomerSecurityAttemptContext customerAttemptContext = new CustomerSecurityAttemptContext()
                        {
                            CustomerId = customerId,
                            CardDigitPosition1 = Convert.ToInt16(firstDigitvalue.ToString()),
                            CardDigitPosition2 = Convert.ToInt16(secondDigitvalue.ToString()),
                            CardDigitPosition3 = Convert.ToInt16(thirdDigitvalue.ToString()),

                            //start- changes for CCMCA-4454- updated convert.toint16 to Try parse for non integer input handling
                            CardDigitPosition1InputValue = model.txtfirstSecureDigit.TryParse<Int16>(),
                            CardDigitPosition2InputValue = model.txtsecondSecureDigit.TryParse<Int16>(),
                            CardDigitPosition3InputValue = model.txtthirdSecureDigit.TryParse<Int16>()
                            //End- changes for CCMCA-4454- updated convert.toint16 to Try parse for non integer input handling
                        };
                        isSuccessful = this.GetIsSuccessfulStatus(isVirtualLock, VirtualLockThreshold, securityStatus.Item1,
                            securityStatus.Item2, attempt, customerAttemptContext, dotcomCustomerId);
                    }

                    if (isSuccessful)
                    {
                        logDataAudit.RecordStep("True|Valid security digits entered");
                        logData.RecordStep(model.JsonText());
                        _logger.Submit(logData);

                        return GetAction(from);
                    }
                    else
                    {
                        securityStatus = this.CheckSecurityStatus(custID);
                        securityStatus = isVirtualLock && securityStatus.Item2 >= VirtualLockThreshold ? new Tuple<string, int>("Y", securityStatus.Item2) : securityStatus;

                        logDataAudit.RecordStep(String.Format("False|{0}",
                            new
                            {
                                FirstDigit = String.Format("Key-{0},Value-{1}", firstDigitvalue, model.txtfirstSecureDigit),
                                SecondDigit = String.Format("Key-{0},Value-{1}", secondDigitvalue, model.txtsecondSecureDigit),
                                ThirdDigit = String.Format("Key-{0},Value-{1}", thirdDigitvalue, model.txtthirdSecureDigit),
                                AttemptCount = securityStatus.Item2,
                                IsBlocked = securityStatus.Item1
                            }.JsonText()));

                        logData.RecordStep(string.Format("Security Status: {0}", securityStatus.JsonText()));

                        ModelState.Clear();
                        if (securityStatus.Item1 == "N")
                        {
                            model = this.GenerateRandomDigits();
                            model.FailedLoginAttempts = securityStatus.Item2;
                            var captchaResult = this.DetermineCaptchaShowUp(securityStatus.Item2, customerId.ToString());
                            model.ShowCaptcha = captchaResult.Item1;
                            model.SiteKey = CryptoUtility.DecryptTripleDES(captchaResult.Item2);
                        }
                        else
                        {
                            model.txtfirstSecureDigit = String.Empty;
                            model.txtsecondSecureDigit = String.Empty;
                            model.txtthirdSecureDigit = String.Empty;
                        }
                        model.FailedLoginAttempts = securityStatus.Item2;
                        model.IsBlocked = securityStatus.Item1 == "Y";
                        model.isSuccessful = isSuccessful;
                    }
                    TempData["PageName"] = sPageRequested;
                    TempData["ActionName"] = sActionName;
                    logData.RecordStep(model.JsonText());
                    _logger.Submit(logData);
                    return View("SecurityHome", model);
                }
                else
                {
                    logDataAudit.RecordStep(String.Format("False|{0}",
                            new
                            {
                                Reason = "No data entered",
                                AttemptCount = modelTemp.FailedLoginAttempts,
                                IsBlocked = modelTemp.IsBlocked
                            }.JsonText()));

                    ModelState.Clear();

                    model = this.GenerateRandomDigits();
                    model.ShowCaptcha = modelTemp.ShowCaptcha;
                    model.SiteKey = modelTemp.SiteKey;

                    logData.RecordStep("Customer hasn't entered all the Security Digits");

                    if (string.IsNullOrEmpty(model.txtfirstSecureDigit))
                    {
                        ModelState.AddModelError("requiredmsg1", HttpContext.GetLocalResourceObject("~/Views/Account/SecurityHome.cshtml", "RequiredFieldValidator3Resource1", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        model.showErrorLabel1 = true;
                    }
                    if (string.IsNullOrEmpty(model.txtsecondSecureDigit))
                    {
                        ModelState.AddModelError("requiredmsg2", HttpContext.GetLocalResourceObject("~/Views/Account/SecurityHome.cshtml", "RequiredFieldValidator4Resource1", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        model.showErrorLabel2 = true;
                    }
                    if (string.IsNullOrEmpty(model.txtthirdSecureDigit))
                    {
                        ModelState.AddModelError("requiredmsg3", HttpContext.GetLocalResourceObject("~/Views/Account/SecurityHome.cshtml", "RequiredFieldValidator5Resource1", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        model.showErrorLabel3 = true;
                    }

                    _logger.Submit(logData);
                    return View("SecurityHome", model);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while setting Security Home Page", ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
            finally
            {
                if (_AuditLoggerService != null)
                {
                    _AuditLoggerService.Submit(logDataAudit);
                }
            }
        }

        [HttpGet]
        public ActionResult RedirectTo(string from)
        {
            LogData logData = new LogData();
            try
            {
                logData.RecordStep(string.Format("query string: {0}", from));
                var newUriBuilder = new UriBuilder(from);
                logData.RecordStep(string.Format("New Url Host: {0}", newUriBuilder.Host));

                if (HttpContext.Request.Url.Host.Equals(newUriBuilder.Host, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.Submit(logData);
                    ViewBag.Target = from;
                    return View();
                }
                else
                {
                    _logger.Submit(logData);
                    return RedirectToAction("Home", "MCAError", new { from = from });
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting URL", ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private ActionResult GetAction(string from)
        {
            LogData logData = new LogData();
            try
            {
                logData.RecordStep(string.Format("query string: {0}", from));
                if (string.IsNullOrEmpty(from))
                {
                    _logger.Submit(logData);
                    return RedirectToAction(ParameterNames.HOME_PAGE, ParameterNames.HOME_PAGE);
                }
                else
                {
                    _logger.Submit(logData);
                    return RedirectToAction("RedirectTo", new { from = from });
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting URL", ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        public ActionResult Signout()
        {
            LogData _logData = new LogData();
            try
            {
                ////Delete cookie
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerID);
                MCACookie.Cookie.Remove(MCACookieEnum.DotCustomerID);
                MCACookie.Cookie.Remove(MCACookieEnum.PtsDtls);
                MCACookie.Cookie.Remove(MCACookieEnum.PointSummaryCutOffDate);
                MCACookie.Cookie.Remove(MCACookieEnum.PointSummarySignOffDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasCurrStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasCurrEndDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasNextStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasNextEndDate);

                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeFlag);
                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeEnddate);
                MCACookie.Cookie.Remove(MCACookieEnum.CouponPageDate);
                MCACookie.Cookie.Remove(MCACookieEnum.ShowOrdrRplcmtPage);
                MCACookie.Cookie.Remove(MCACookieEnum.IsSecurityCheckDone);
                MCACookie.Cookie.Remove(MCACookieEnum.IsFuelAccountExist);

                MCACookie.Cookie.Remove(MCACookieEnum.Activated);
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerMailStatus);
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerUseStatus);

                MCACookie.Cookie.Remove(MCACookieEnum.DotcomCustomerID);
                MCACookie.Cookie.Remove(MCACookieEnum.LoginRetry);
                this.ClearCookie(ParameterNames.JWT_TOKEN);


                if (DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.LoginSolution.ToString()].ConfigurationValue1.ToString().Equals(ParameterNames.LOGIN_SOLUTION_TYPE_GROUP))
                {
                    //Delete IGHSCustomerIndentity  cookie
                    string strDomainName = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.DomainName.ToString()].ConfigurationValue1.ToString();
                    if (Request.Cookies["AUID"] != null)
                    {
                        HttpCookie myCookie = new HttpCookie("AUID");
                        myCookie.Expires = DateTime.Now.AddDays(-1d);
                        myCookie.Domain = strDomainName;

                        Response.Cookies.Add(myCookie);
                    }
                }
                //1 will enable the INT,OPS and Live environment(dotcom) and 0 will work for System test environment(non-dotcom)
                if (IsDotcomEnvironmentEnabled)
                {
                    string redirectURL = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.GenericLogoutPage.ToString()].ConfigurationValue1.ToString();
                    _logData.RecordStep(String.Format("Redirecting to URL - ", redirectURL));
                    _logger.Submit(_logData);
                    return Redirect(redirectURL);
                }
                else
                {
                    _logData.RecordStep("Redirecting to Account controller login action");
                    _logger.Submit(_logData);
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
            finally
            {
                _logger.Submit(_logData);
            }
        }

        protected SecurityViewModel ValidateClubcardSecurity(string customerID)
        {
            LogData logData = new LogData();
            try
            {
                SecurityViewModel model = new SecurityViewModel();
                //Changes for CCMCA-4454, clearing model state would clear the validation errors
                // ModelState.Clear();

                var attempStatus = this.CheckSecurityStatus(customerID);

                logData.RecordStep(string.Format("Security Check Status :{0}", attempStatus.JsonText()));

                int VirtualLockThreshold = 0;
                if(!string.IsNullOrEmpty(DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue2))
                {
                    Int32.TryParse(DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue2, out VirtualLockThreshold);
                }

                bool isVirtualLock = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.VirtualLockout.ToString()].ConfigurationValue1.Equals("1");
                
                attempStatus = isVirtualLock && attempStatus.Item2 >= VirtualLockThreshold ? new Tuple<string, int>("Y", attempStatus.Item2) : attempStatus;
                
                if (attempStatus.Item1 == "N")
                {
                    model = this.GenerateRandomDigits();
                    var captchaStatus = this.DetermineCaptchaShowUp(attempStatus.Item2, base.CustomerId);
                    model.ShowCaptcha = captchaStatus.Item1;
                    model.SiteKey = CryptoUtility.DecryptTripleDES(captchaStatus.Item2);
                }

                model.FailedLoginAttempts = attempStatus.Item2;
                model.IsBlocked = attempStatus.Item1 == "Y";
                model.isSuccessful = !model.IsBlocked;

                logData.RecordStep(String.Format("Security Model: {0}", model.JsonText()));

                _logger.Submit(logData);

                return model;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller while validating Clubcard Security digits entered", ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private Tuple<string, int> CheckSecurityStatus(string customerID)
        {
            LogData _logData = new LogData();
            string customerId = string.Empty;
            CustomerSecurityBlockerStatus custVerificationDetails = new CustomerSecurityBlockerStatus();
            Tuple<string, int> secStatus = new Tuple<string, int>("N", 0);
            try
            {
                custVerificationDetails = _AccountProvider.ParseSecurityVerificationStatus(customerID);

                if (custVerificationDetails != null && !string.IsNullOrEmpty(custVerificationDetails.ISBLOCKED.ToString()))
                {
                    secStatus = new Tuple<string, int>("N", 0);

                    if (custVerificationDetails.ISBLOCKED.ToString() != "\0")
                    {
                        secStatus = new Tuple<string, int>(custVerificationDetails.ISBLOCKED.ToString(), custVerificationDetails.ACCESSATTEMPTS);
                    }
                }
                _logger.Submit(_logData);
                return secStatus;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller GetMethod() while getting Customer.IsBlocked Status", ex,
                              new Dictionary<string, object>() 
                                { 
                                    { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                    { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                                });
            }
        }

        private SecurityViewModel GenerateRandomDigits()
        {
            LogData logData = new LogData();
            SecurityViewModel model = new SecurityViewModel();
            try
            {
                SecurityPageConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.SecurityCheck];
                DbConfigurationItem DtSecureDigits = SecurityPageConfiguration.GetConfigurationItem(DbConfigurationItemNames.ClubcardSecureDigits);
                logData.RecordStep(string.Format("Security Digits", DtSecureDigits));

                if (DtSecureDigits != null)
                {
                    string secureDigits = DtSecureDigits.ConfigurationValue1;

                    List<int> lstDigitPositions = new List<int>();

                    int iTemp = 0;

                    foreach (string s in secureDigits.Split(','))
                    {
                        if (int.TryParse(s, out iTemp))
                        {
                            lstDigitPositions.Add(iTemp);
                        }
                    }

                    List<int> result = this.GetRandomNumbersFromCollection(lstDigitPositions, 3);

                    model.firstSecureDigit = result[0];
                    model.secondSecureDigit = result[1];
                    model.thirdSecureDigit = result[2];

                    model.firstSecureDigitEncrypt = CryptoUtility.EncryptTripleDES(model.firstSecureDigit.ToString());
                    model.secondSecureDigitEncrypt = CryptoUtility.EncryptTripleDES(model.secondSecureDigit.ToString());
                    model.thirdSecureDigitEncrypt = CryptoUtility.EncryptTripleDES(model.thirdSecureDigit.ToString());
                }

                model.txtfirstSecureDigit = string.Empty;
                model.txtsecondSecureDigit = string.Empty;
                model.txtthirdSecureDigit = string.Empty;

                logData.CaptureData("model", model);
                _logger.Submit(logData);
                return model;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Acocunt Conroller while Generating Random Digits", ex,
                               new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private bool ProcessSecurityAttempt(CustomerSecurityAttemptAudit attemptAudit, CustomerSecurityAttemptContext attemptContext, 
            string dotcomCustomerId)
        {
            LogData logData = new LogData();
            string attemptAuditXml = SerializerUtility<CustomerSecurityAttemptAudit>.GetSerializedString(attemptAudit);
            string attemptContextXml = SerializerUtility<CustomerSecurityAttemptContext>.GetSerializedString(attemptContext);
            List<long> clubcards = new List<long>();
            try
            {
                logData.CustomerID = attemptAudit.CustomerID.ToString();
                if (attemptContext != null)
                {
                    List<HouseholdCustomerDetails> customersList = _AccountProvider.GetHouseHoldCustomersData(attemptContext.CustomerId, CurrentCulture);
                    if (customersList != null && customersList.Count > 0)
                    {
                        int maxCardDigitPosition = Math.Max(attemptContext.CardDigitPosition1, Math.Max(attemptContext.CardDigitPosition2, attemptContext.CardDigitPosition3));
                        foreach (var customer in customersList)
                        {
                            List<Clubcard> clubcardsDetails = _AccountProvider.GetClubcardsCustomerData(customer.CustomerID, CurrentCulture);
                            foreach (var clubcardid in clubcardsDetails)
                            {
                                if (clubcardid.ClubCardID.ToString().Length >= maxCardDigitPosition)
                                {
                                    clubcards.Add(clubcardid.ClubCardID);
                                }
                            }
                        }
                    }
                }

                bool bCardMatchFound = clubcards.Exists(cardNumber => 
                    (attemptContext.CardDigitPosition1InputValue == this.GetCardDigitAt(cardNumber, attemptContext.CardDigitPosition1)) &&
                    (attemptContext.CardDigitPosition2InputValue == this.GetCardDigitAt(cardNumber, attemptContext.CardDigitPosition2)) &&
                    (attemptContext.CardDigitPosition3InputValue == this.GetCardDigitAt(cardNumber, attemptContext.CardDigitPosition3)));
                
                logData.RecordStep(string.Format("Matching clubcard Found: {0}", bCardMatchFound));

                if (!bCardMatchFound)
                {
                    attemptAudit.IsValidAttempt = "N";
                }
                else
                {
                    attemptAudit.IsValidAttempt = "Y";
                    bool bDotComEnvEnabled = base.IsDotcomEnvironmentEnabled;

                    logData.RecordStep(string.Format("IsDotcomEnvironmentEnabled: {0}", bDotComEnvEnabled));
                    
                    GeneralUtility generalUtility = new GeneralUtility();
                    if (this.IsEnterpriceServiceCallsEnabled)
                    {
                        //using a local variable instead of trusting the parameter because we trust the jwt tid more than 
                        //anything else.
                        string dotcomID = this.DotcomCustomerId;
                        this.SetTokenPayload(null, dotcomID, true, default(long));

                        //This is just a temporary code. It should go away once boost team develops an alternate logic to determine
                        //if the customer cleared the security or not.
                        generalUtility.CreateSecurityClearedCookie(bDotComEnvEnabled, dotcomID, attemptContext.CustomerId);
                    }
                    else
                    {
                        generalUtility.CreateSecurityClearedCookie(bDotComEnvEnabled, dotcomCustomerId, attemptContext.CustomerId);
                    }
                }
                this.RecordSecurityAttemptInAudit(attemptAudit);
                logData.CaptureData("isClubcardExists", bCardMatchFound);
                _logger.Submit(logData);
                return bCardMatchFound;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountController while getting ProcessingSecurityAttempt", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private void RecordSecurityAttemptInAudit(CustomerSecurityAttemptAudit securityAttempAudit)
        {
            LogData logData = new LogData();
            try
            {
                string attemptAuditXml = SerializerUtility<CustomerSecurityAttemptAudit>.GetSerializedString(securityAttempAudit);
                this._AccountProvider.NoteSecurityAttemptInAudit(securityAttempAudit);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Acocunts Conroller during call to noteSecurityAttemptInAudit", ex,
                                new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private short GetCardDigitAt(long cardNumber, int position)
        {
            LogData logData = new LogData();
            try
            {
                short digit = Convert.ToInt16(Convert.ToString(cardNumber).Substring(position - 1, 1));
                logData.RecordStep(string.Format("digit :", digit));
                logData.CaptureData("digit", digit);
                _logger.Submit(logData);
                return digit;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller while getting Card Digits", ex,
                                   new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private bool GetEncryptyvalue(SecurityViewModel model)
        {
            LogData logData = new LogData();

            try
            {
                ArrayList lstEncryptedValues = new ArrayList();

                lstEncryptedValues.Add(model.firstSecureDigitEncrypt);

                if (lstEncryptedValues.Contains(model.secondSecureDigitEncrypt))
                {
                    return false;
                }
                else
                {
                    lstEncryptedValues.Add(model.secondSecureDigitEncrypt);
                }

                if (lstEncryptedValues.Contains(model.thirdSecureDigitEncrypt))
                {
                    return false;
                }
                else
                {
                    lstEncryptedValues.Add(model.thirdSecureDigitEncrypt);
                }

                firstDigitvalue = CryptoUtility.DecryptTripleDES(model.firstSecureDigitEncrypt);
                secondDigitvalue = CryptoUtility.DecryptTripleDES(model.secondSecureDigitEncrypt);
                thirdDigitvalue = CryptoUtility.DecryptTripleDES(model.thirdSecureDigitEncrypt);

                return true;
            }
            catch (Exception ex)
            {
                Exception customEx = GeneralUtility.GetCustomException("Failed encryption attempt, security violation.", ex,
                                      new Dictionary<string, object>() 
                                        { 
                                            { LogConfigProvider.EXCLOGDATAKEY, logData }                                            
                                        });

                _logger.ErrorException(customEx);
            }
            return false;
        }

        private List<int> GetRandomNumbersFromCollection(List<int> originalList, int n)
        {
            List<int> lstResults = new List<int>();
            Random random = new Random();
            int idx = 0;

            for (int i = 0; i < n; i++)
            {
                idx = random.Next(0, originalList.Count);
                lstResults.Add(originalList[idx]);
                originalList.RemoveAt(idx);
            }

            lstResults.Sort();

            return lstResults;
        }

        private Tuple<bool, string> DetermineCaptchaShowUp(int attemptCount, string custID)
        {
            int iMaxNonCaptchaAttemptsAllowed = 0;

            bool bShowCaptchaSecPage = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                            [AppConfigEnum.EnableSecurityPageCaptcha.ToString()]
                                                            .ConfigurationValue1 == "1";

            bool bShowPartialCaptcha = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                            [AppConfigEnum.EnablePartialCaptchaShowup.ToString()]
                                                            .ConfigurationValue1 == "1";

            iMaxNonCaptchaAttemptsAllowed = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                            [AppConfigEnum.MaxNonCaptchaAttemptsAllowed.ToString()]
                                                            .ConfigurationValue1.TryParse<Int32>();

            string siteKey = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                            [AppConfigEnum.GCSiteKey.ToString()]
                                                            .ConfigurationValue1;

            string whiteListedCusts = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                            [AppConfigEnum.CaptchaWhitelistCustomers.ToString()]
                                                            .ConfigurationValue1;

            bool bIsWhiteListedCustomer = false;

            if (!String.IsNullOrWhiteSpace(whiteListedCusts))
            {
                foreach (string c in whiteListedCusts.Split(','))
                {
                    if (c == custID)
                    {
                        bIsWhiteListedCustomer = true;
                        break;
                    }
                }
            }

            return new Tuple<bool, string>(
                bShowCaptchaSecPage && ((bShowPartialCaptcha && iMaxNonCaptchaAttemptsAllowed <= attemptCount) || (!bShowPartialCaptcha)) && !bIsWhiteListedCustomer,
                siteKey);
        }

        private bool GetIsSuccessfulStatus(bool virtualLockEnabled, int virtualLockThreshold, string isBlocked, int failedAttempts,
            CustomerSecurityAttemptAudit attempt, CustomerSecurityAttemptContext context, string dotcomCustomerID)
        {
            if (isBlocked == "N" && (!virtualLockEnabled || (virtualLockEnabled && failedAttempts < virtualLockThreshold)))
            {
                return this.ProcessSecurityAttempt(attempt, context, dotcomCustomerID);
            }

            return false;
        }
    }

}
