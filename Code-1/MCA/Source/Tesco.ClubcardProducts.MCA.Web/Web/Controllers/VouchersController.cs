using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Collections.Generic;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using System.Linq;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Globalization;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class VouchersController : BaseController
    {
        private IVoucherBC _voucherProvider;
        IPDFGenerator _pdfProvider;
        IConfigurationProvider _configProvider = null;
        IAccountBC _accountProvider;
        IPointsBC _pointsProvider;
        IXmasSaverBC _xmasProvider;
        ILoggingService _AuditLoggerService = null;

        public VouchersController()
        {
            _pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();
            _voucherProvider = ServiceLocator.Current.GetInstance<IVoucherBC>();
            _configProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _accountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
            _AuditLoggerService = new AuditLoggingService();
            _pointsProvider = ServiceLocator.Current.GetInstance<IPointsBC>();
            _xmasProvider = ServiceLocator.Current.GetInstance<IXmasSaverBC>();
        }

        public VouchersController(IVoucherBC _IvoucherBC, IPDFGenerator pdfGenerator, IConfigurationProvider configProvider, ILoggingService _auditlogger, IAccountBC _iAccountBC, IPointsBC _iPointsProvider, IXmasSaverBC _ixmasProvider)
            : base(_auditlogger, _iAccountBC, configProvider)
        {
            this._voucherProvider = _IvoucherBC;
            this._pdfProvider = pdfGenerator;
            this._configProvider = configProvider;
            this._AuditLoggerService = _auditlogger;
            this._accountProvider = _iAccountBC;
            this._pointsProvider = _iPointsProvider;
            this._xmasProvider = _ixmasProvider;
        }

        /// <summary>
        /// Calls the VoucherBC methods to retrieve AccountDetails and Voucher Summary, Unused and used Vouchers data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult Home()
        {
            LogData logData = new LogData(); 
            try
            {
                ModelState.Clear();

                #region VouchersViewModel

                VouchersViewModel vouchersViewModel = new VouchersViewModel();
                vouchersViewModel.isDotcomCustomerIDEmpty = false;

                long custID = this.GetCustomerId();
                logData.CustomerID = custID.ToString();
                
                if (custID == default(long))
                {
                    logData.RecordStep("CustomerId is Null or Empty");
                    return View("~/Views/Home/Home.cshtml");
                }
                else
                {
                    if (this.IsCurrentDateInHoldingDates())
                    {
                        ViewBag.Name = "Voucher Holding Page";
                        logData.RecordStep("Holding Page");
                    }  
                    vouchersViewModel = _voucherProvider.GetVoucherViewDetails(this.CustomerId.TryParse<Int64>(), base.CurrentCulture);
                }

                #endregion

               // TempData["UnUsedVouchers"] = vouchersViewModel;
                _logger.Submit(logData);
                return View(vouchersViewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, string.Empty }
                            });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult Home(VouchersUnUsedModel vsmodel)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();
            System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(CurrentCulture);

            try
            {
                ModelState.Clear();
                long custID = this.CustomerId.TryParse<long>();
                logData.CustomerID = logDataAudit.CustomerID = custID.ToString();

                VouchersViewModel voucherModel = _voucherProvider.GetVoucherViewDetails(custID, base.CurrentCulture);
                logData.CaptureData("Vouchers View Model", voucherModel);
                // VouchersViewModel voucherModel = TempData["UnUsedVouchers"] as VouchersViewModel;

                List<VoucherDetails> vList = voucherModel.vouchersUnUsedModel.voucherList;
                List<VoucherDetails> selectedVouchers = vsmodel.getSelectedVouchers(vList);
                if (selectedVouchers.Count > 0)
                {
                    logData.RecordStep("Printing Selected Vouchers");
                    PdfBackgroundTemplate voucherBackGroundTemplate = this.GetBackgroungTemplate(CurrentCulture);
                    using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<VoucherDetails, PdfBackgroundTemplate>(
                                    selectedVouchers, voucherBackGroundTemplate, voucherModel.accountDetails))
                    {
                        logData.RecordStep("PDF generated successfully !!");
                        _voucherProvider.RecordVouchersPrinted(selectedVouchers, voucherModel.accountDetails.CustomerID, voucherModel.accountDetails.ClubcardID);

                        string strVoucherInfo = GetAuditDetailForVouchers(selectedVouchers);
                        logDataAudit.RecordStep(string.Format("Downloaded Vouchers|{0}", strVoucherInfo));


                        logData.RecordStep("Download action recorded successfully !!");
                        return File(document.ToArray(), "application/pdf", Server.UrlEncode(HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrVoucherFileName", cultureObj).ToString()));
                    }
                }
                else
                {
                    logData.RecordStep("No Vouchers Selected");
                    ModelState.AddModelError("VoucherNotSelected", HttpContext.GetLocalResourceObject("~/Views/Vouchers/Home.cshtml", "lblShowErrorMsgResource", System.Globalization.CultureInfo.CurrentCulture).ToString());
                    voucherModel.vouchersUnUsedModel.error = true;
                }

                // TempData["UnUsedVouchers"] = voucherModel;
                _logger.Submit(logData);
                return View(voucherModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, string.Empty }
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

        private static string GetAuditDetailForVouchers(List<VoucherDetails> selectedVouchers)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (VoucherDetails v in selectedVouchers)
            {
                if (v.VoucherNumberToPrint.Length > 0)
                {
                    sb.Append(v.VoucherNumberToPrint.ToString().Substring(v.VoucherNumberToPrint.Length - 4));
                    sb.Append(", ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);                
            }
            return sb.ToString();
        }

        private PdfBackgroundTemplate GetBackgroungTemplate(string culture)
        {
            LogData logData = new LogData();
            try
            {
                logData.RecordStep("Create Voucher Template");
                System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(culture);

                PdfBackgroundTemplate template = new PdfBackgroundTemplate();
                template.ReplaceClubcardPrefix = _configProvider.GetStringAppSetting(AppConfigEnum.ReplaceClubcardPrefix);
                template.PrintBGImagePath = HttpContext.Server.MapPath(_configProvider.GetStringAppSetting(AppConfigEnum.PrintBGImagePath));
                //template.FontPath = AppConfiguration.Settings[AppConfigEnum.FontPath];
                template.IsAlphaCodeRequired = _configProvider.GetStringAppSetting(AppConfigEnum.IsAlphaCodeRequired);
                template.IsHideCustomerName = _configProvider.GetStringAppSetting(AppConfigEnum.IsHideCustomerName);

                template.lblstrClubcardVouchers = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardVouchers", cultureObj).ToString();
                template.lblstrBonusvoucher = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrBonusvoucher", cultureObj).ToString();
                template.lblstrTopup = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTopup", cultureObj).ToString();
                template.lblstrTopup_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTopup" + culture.Replace("-", string.Empty), cultureObj).ToString();
                template.lblClubcardVoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblClubcardVoucher" + culture.Replace("-", string.Empty), cultureObj).ToString();

                template.lblstrBonusvoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrBonusvoucher" + culture.Replace("-", string.Empty), cultureObj).ToString();
                template.lblClubcardVoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblClubcardVoucher" + culture.Replace("-", string.Empty), cultureObj).ToString();
                template.lblstrTopup = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTopup" + culture.Replace("-", string.Empty), cultureObj).ToString();
                template.lblstrCouponsFileName = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCouponsFileName", cultureObj).ToString();
                template.lblstrCustomerPrinted = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCustomerPrinted", cultureObj).ToString();
                template.lblstrClubcardVoucher = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardVoucher", cultureObj).ToString();

                template.lblstrFromTescoBank = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrFromTescoBank", cultureObj).ToString();
                template.lblstrClubcardWinner = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardWinner", cultureObj).ToString();
                template.lblstrTitleVoucher = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTitleVoucher", cultureObj).ToString();

                template.lblstrClubcardChristmas = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardChristmas", cultureObj).ToString();
                template.lblstrBonusVouchers = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrBonusVouchers", cultureObj).ToString();
                template.lblstrSaverTop_UpVoucher = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrSaverTop_UpVoucher", cultureObj).ToString();
                template.lblstrOnlineCode = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrOnlineCode", cultureObj).ToString();
                template.lblstrValidUntil = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrValidUntil", cultureObj).ToString();


                template.lblstrCurrencySymbol = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCurrencySymbol", cultureObj).ToString();
                template.lblstrCurrencyAllignment = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCurrencyAllignment", cultureObj).ToString();
                template.lblstrCurrencyDecimalSymbol = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCurrencyDecimalSymbol", cultureObj).ToString();
                template.lblstrTitleCoupon = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTitleCoupon", cultureObj).ToString();
                template.lblstrVouchersAndCouponsFileName = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrVouchersAndCouponsFileName", cultureObj).ToString();
                template.isDisableDecimal = _configProvider.GetStringAppSetting(AppConfigEnum.DisableCurrencyDecimal);
                template.FontName = _configProvider.GetStringAppSetting(AppConfigEnum.FontName);
                template.DateFormat = _configProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);

                template.DocumentWidth = 590;
                template.Culture = CurrentCulture;
                template.ItemPerPage = 3;
                template.Top = template.Left = 10;
                _logger.Submit(logData);
                return template;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, string.Empty }
                            });
            }
        }
        
        /// <summary>
        ///  This method will check if any unspent voucher is available with user
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public bool IsUnSpentVouchersExist(long customerId, string CurrentCulture)
        {
            LogData logData = new LogData();
            try
            {
                bool unSpentVoucherExists = _voucherProvider.IsUnSpentVouchersExist(customerId, CurrentCulture);
                logData.CaptureData("UnSpent Vouchers Exists : {0}", unSpentVoucherExists);
                _logger.Submit(logData);
                return unSpentVoucherExists;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, string.Empty }
                            });
            }
        }

        private bool IsCurrentDateInHoldingDates()
        {
            LogData logData = new LogData();
            string sHoldingStartDate = string.Empty;
            string sHoldingEndDate = string.Empty;

            logData.RecordStep("ServiceCall:Start:GetDBConfiguration - HoldingDates");
            DbConfiguration dbConfigs = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, 
                                                    CultureInfo.CurrentCulture.Name);
            logData.RecordStep("SeviceCall:End:GetDBConfiguration - HoldingDates");

            DbConfigurationItem vConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.SmartVoucherDates);

            sHoldingStartDate = Convert.ToDateTime(vConfig.ConfigurationValue1.Trim()).ToShortDateString();
            sHoldingEndDate = Convert.ToDateTime(vConfig.ConfigurationValue2.Trim()).ToShortDateString();
            logData.CaptureData("Voucher Holding Start Date", sHoldingStartDate);
            logData.CaptureData("Voucher Holding End Date", sHoldingEndDate);

            _logger.Submit(logData);
            return ((Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) >= Convert.ToDateTime(sHoldingStartDate)) && (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) <= Convert.ToDateTime(sHoldingEndDate)));
        }

        [HttpGet]
        [AjaxOnly]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        public JsonResult GetUnusedVoucherRewardsCount()
        {
            LogData logData = new LogData();
            VoucherRewardDetailsOverallSummaryModel objVoucherSummary = null;
            try
            {
                long custID = this.CustomerId.TryParse<long>();
                logData.CustomerID = custID.ToString();

                objVoucherSummary = _voucherProvider.GetVoucherRewardCounts(custID, base.CurrentCulture);
               
                decimal rewardLeftOver = 0;
                decimal xVal = 0;

                if (objVoucherSummary != null)
                {
                    rewardLeftOver = objVoucherSummary.TotalRewardLeftOver;
                    bool isDecimalDisabled = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.DisableCurrencyDecimal].ConfigurationValue1.TryParse<bool>();
                     if (isDecimalDisabled)
                            {
                                xVal = GeneralUtility.GetDecimalTrimmedCurrencyVal(rewardLeftOver.TryParse<string>()).TryParse<decimal>();

                            }
                            else
                            {
                                xVal = rewardLeftOver;
                            }
                     logData.RecordStep(string.Format("objVoucherSummary.TotalRewardLeftOver : {0}", xVal.ToString()));
                }               
              
                _logger.Submit(logData);
                return Json(new { count = xVal.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        [HttpGet]
        [AjaxOnly]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        public JsonResult AvailableVouchers(string image, string description)
        {
            LogData _logData = new LogData();
            string vouchers = string.Empty;
            List<Offer> offers = new List<Offer>();
            Offer currentOffer = new Offer();
            try
            {
                string xmasCal = ConfigProvider.GetStringAppSetting(AppConfigEnum.XMASVoucherCalculation);
                bool isDecimalDisabled = ConfigProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
                string optedPreference = string.Empty;
                Int32 prefID = _pointsProvider.GetOptedSchemeId(CustomerId.TryParse<long>(), out optedPreference);
                if (prefID == 0)
                {
                    // this section will be executed for the voucher type customers
                    AccountDetails accDetails = _accountProvider.GetMyAccountDetail(CustomerId.TryParse<long>(), CurrentCulture);
                    vouchers = _pointsProvider.GetRewardsDetailForVoucherCustomer(accDetails.ClubcardID, CurrentCulture).ToString();
                }
                else if (prefID == BusinessConstants.XMASSAVER)
                {
                    if (xmasCal.Equals("0"))
                    {
                        offers = _pointsProvider.GetOffersForCustomer(CustomerId.TryParse<long>(), CurrentCulture);
                        currentOffer = offers.Find(o => o.Period.ToUpper() == "CURRENT");
                        vouchers = _pointsProvider.GetRewardsDetailForSchemeCustomer(prefID, currentOffer).ToString();
                    }
                    else
                    {
                        vouchers = _xmasProvider.GetVouchersForXmasSaverCustomer(CustomerId, CurrentCulture);                        
                    }
                }
                else
                {
                    // This section will be executed for the air miles customer
                    offers = _pointsProvider.GetOffersForCustomer(CustomerId.TryParse<long>(), CurrentCulture);
                    currentOffer = offers.Find(o => o.Period.ToUpper() == "CURRENT");
                    vouchers = _pointsProvider.GetRewardsDetailForSchemeCustomer(prefID, currentOffer).ToString();
                }
                vouchers = isDecimalDisabled ? GeneralUtility.GetDecimalTrimmedCurrencyVal(vouchers) : vouchers;
                _logger.Submit(_logData);
                return Json(new { count = vouchers }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

    }
}
