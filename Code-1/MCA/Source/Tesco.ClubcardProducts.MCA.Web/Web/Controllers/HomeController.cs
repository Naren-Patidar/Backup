using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IConfigurationProvider _configurationProvider;
        IPDFGenerator _pdfProvider;
        private IHomeBC _homeProvider;
        private IPersonalDetailsBC _personalDetailsProvider;

        public HomeController()
        {
            _pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _homeProvider = ServiceLocator.Current.GetInstance<IHomeBC>();
            _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
        }

        public HomeController(IHomeBC homeProvider, IPDFGenerator pdfGenerator, IConfigurationProvider configurationProvider, IPersonalDetailsBC personalDetailsProvider)
        {
            _pdfProvider = pdfGenerator;
            _configurationProvider = configurationProvider;
            _homeProvider = homeProvider;
            _personalDetailsProvider = personalDetailsProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Home");
        }

        [HttpGet]
        [AjaxOnly]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        public ActionResult ModalPaperless()
        {
            PopUpModel Popupmodel = new PopUpModel();
            DBConfigurations hideControlConfig = DBConfigurationManager.Instance[DbConfigurationTypeEnum.ChinaHiddenFunctionality];
            bool PopUpEnabled = hideControlConfig[DbConfigurationItemNames.HideModalForPaperless].ConfigurationValue1.Equals("0") && Request.Cookies["popUpStatus"] == null;
            if (PopUpEnabled)
            {
                Popupmodel = _homeProvider.GetPopUpViewModel("gopaperless", base.CustomerId.TryParse<Int64>());
                return PartialView("_PopUp", Popupmodel);
            }
            else
            {
                return Content("");
            }
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult OneAccountPersonalDetails()
        {            
            MCACookie.Cookie.Add(MCACookieEnum.IsCustomerDetailsVerificationDone, string.Format("{0}_{1}_{2}_{3}", "Y", base.CustomerId, base.DotcomCustomerId, "div1"));
            return new EmptyResult();
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult OneAccountPersonalDetails2()
        {
            MCACookie.Cookie.Add(MCACookieEnum.IsCustomerDetailsVerificationDone, string.Format("{0}_{1}_{2}_{3}", "Y", base.CustomerId, base.DotcomCustomerId, "div2"));
            return new EmptyResult();
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            string isSecurityCheckDone = string.Empty;
            string strCustomerId = string.Empty;
            string strDotcomId = string.Empty;

            try
            {
                logData.CustomerID = strCustomerId = base.CustomerId;
                strDotcomId = base.DotcomCustomerId;
                Int64 lCustID = strCustomerId.TryParse<Int64>();
                
                HomeViewModel homeViewModel = _homeProvider.GetHomeViewModel(lCustID, CurrentCulture);
                
                var showCustomerDetailsBanner = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.IsCustomerDetailsBannerVisible.ToString()].ConfigurationValue1.ToString();
                if (showCustomerDetailsBanner == "1")
                {
                    Int64 intDotcomId = 0;
                    //--This will run with dotcom envirnoment only
                    if (Int64.TryParse(strDotcomId, out intDotcomId) && intDotcomId > 0)
                    {
                        homeViewModel.CustomerAddressVerificationView = _personalDetailsProvider.GetMyAccountData(intDotcomId, lCustID);
                    } 

                    //--Use this for local debuging
                    //homeViewModel.CustomerAddressVerificationView = _personalDetailsProvider.GetMyAccountData(0, lCustID);  
                    
                }
                List<string> urls = new List<string>();
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp1"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp2"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp3"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp4"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp5"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp6"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp7"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp8"));
                urls.Add(HttpContext.GetLocalResource("~/Views/Home/Home.cshtml", "urlStamp9"));
                homeViewModel.StampView = _homeProvider.GetStampViewModel(urls);
                logData.CaptureData("Activation  Details", new object[] { Activated, CustomerMailStatus, CustomerUseStatus });

                bool isDecimalDisabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);

                if (isDecimalDisabled)
                {
                    logData.RecordStep("Decimal Disabled");
                    homeViewModel.CustomerVoucherTypeValue = Common.Utilities.GeneralUtility.GetDecimalTrimmedCurrencyVal(Convert.ToString(homeViewModel.CustomerVoucherTypeValue));
                }

                if (IsDotcomEnvironmentEnabled)
                {
                    if (Activated == ActivationStatusType.CustomerActivated && CustomerMailStatus == (int)CustomerMailStatusEnum.AddressInError)
                    {
                        homeViewModel.MyMessageHeader = "AddressError";
                        logData.RecordStep("Inside activation error message for home page view");
                    }
                }

                VouchersBanner voucherStrip = new VouchersBanner();
                voucherStrip.OptedScheme = homeViewModel.OptedSchemeId;
                voucherStrip.IsSecurityCheckDone = homeViewModel.IsSecurityCheckDone = this.GetSecurityVerificationStatus();
                homeViewModel.VoucherStrip = voucherStrip;

                homeViewModel.CustomerAddressVerificationView.IsVerifiedLastStep = 
                    MCACookie.Cookie[MCACookieEnum.IsCustomerDetailsVerificationDone] == string.Format("{0}_{1}_{2}_{3}", "Y", strCustomerId, strDotcomId, "div2");

                homeViewModel.CustomerAddressVerificationView.IsVerifiedFirstStep =
                    MCACookie.Cookie[MCACookieEnum.IsCustomerDetailsVerificationDone] == string.Format("{0}_{1}_{2}_{3}", "Y", strCustomerId, strDotcomId, "div1");
                
                _logger.Submit(logData);
                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Get Home page method", ex,
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
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        public ActionResult Home(HomeViewModel model)
        {
            AccountDetails accountDetails = new AccountDetails();
            LogData logData = new LogData();          
            logData.RecordStep("Entering into Post Section");           
            System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(CurrentCulture);
            try
            {
                logData.CustomerID = base.GetCustomerId().ToString();
                string tempcardNumber = CryptoUtility.DecryptTripleDES(model.ClubcardNumber);
                if (!string.IsNullOrEmpty(tempcardNumber))
                {
                    PdfBackgroundTemplate template = this.GetBackgroundTemplate(CurrentCulture);
                    List<string> clubcardNo = new List<string>();
                    clubcardNo.Add(tempcardNumber);
                    using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<string, PdfBackgroundTemplate>(clubcardNo, template, accountDetails))
                    {
                        _logger.Submit(logData);
                        return File(document.ToArray(), "application/pdf", Server.UrlEncode(HttpContext.GetGlobalResourceObject(
                            "GenerateVouchersPDF", "lblstrClubcardFileName", cultureObj).ToString()));
                    }
                }
                logData.RecordStep("Tempcard number is empty");
                _logger.Submit(logData);
                return View();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Temporary clubcard generation", ex,
                    new Dictionary<string, object>()
                        {
                            { LogConfigProvider.EXCLOGDATAKEY, logData },
                            { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                        });
            }
        }

        public PdfBackgroundTemplate GetBackgroundTemplate(string culture)
        {
            LogData logData = new LogData();
            try
            {
                System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(culture);
                PdfBackgroundTemplate template = new PdfBackgroundTemplate();
                template.ReplaceClubcardPrefix = _configurationProvider.GetStringAppSetting(AppConfigEnum.ReplaceClubcardPrefix);
                template.PrintBGImagePath = HttpContext.Server.MapPath(_configurationProvider.GetStringAppSetting(AppConfigEnum.PrintBGImagePath));
                template.FontPath = _configurationProvider.GetStringAppSetting(AppConfigEnum.FontPath);
                template.IsAlphaCodeRequired = _configurationProvider.GetStringAppSetting(AppConfigEnum.IsAlphaCodeRequired);
                template.Culture = CurrentCulture;
                template.lblstrTitleClubcard = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTitleClubcard", cultureObj).ToString();
                logData.CaptureData("Background Template data", template);
                _logger.Submit(logData);
                return template;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Get BackgroundTemplate method for customer", ex,
                             new Dictionary<string, object>()
                            {
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });

            }
        }
    }
}