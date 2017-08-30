using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using System.IO;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.VoucherandCouponDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class EmailDownloadController : BaseController
    {
        private IEmailDownloadBC _emaildownloadprovider;
        private IVoucherBC _voucherprovider;
        ICouponBC _couponProvider;
        IConfigurationProvider _configProvider = null;
        IPDFGenerator _pdfProvider;
        IBoostsAtTescoBC _boostsAtTescoProvider;

        public EmailDownloadController()
        {
            _voucherprovider = ServiceLocator.Current.GetInstance<IVoucherBC>();
            _emaildownloadprovider= ServiceLocator.Current.GetInstance<IEmailDownloadBC>();
            _couponProvider = ServiceLocator.Current.GetInstance<ICouponBC>();
            _configProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();
            _boostsAtTescoProvider = ServiceLocator.Current.GetInstance<IBoostsAtTescoBC>();
        }

        [HttpGet]
        public ActionResult DownloadVouchersandCoupon(string a)
        {
            string customerID = String.Empty;
            long houseHoldId = 0;
            int totalCoupons = 0;
            string cardNumber = null;
            long lCustID = 0;
            LogData logData = new LogData();
            try
            {
                if (a != null)
                {
                    logData.CustomerID= customerID = _emaildownloadprovider.GetCustomeridbyGUID(a);
                    lCustID = customerID.TryParse<Int64>();
                    logData.CustomerID = customerID;

                }
                if (lCustID > 0)
                {
                    List<HouseholdCustomerDetails> customersList = _emaildownloadprovider.GetHouseholdDetailsofCustomer(lCustID, CurrentCulture);
                    logData.RecordStep("Response received successfully from business layer. Result is passed.");

                    if (customersList != null && customersList.Count > 0)
                    {

                        //AS per the review comments loop has been removed.
                        houseHoldId = customersList[0].HouseHoldID;
                        logData.RecordStep(string.Format("houseHoldId : {0}", houseHoldId));
                        cardNumber = customersList[0].ClubcardID.ToString();

                    }
                    if (houseHoldId != 0)
                    {
                        logData.RecordStep(string.Format("CurrentCulture : {0}", CurrentCulture));
                        AccountDetails customerAccountDetails = _voucherprovider.GetCustomerAccountDetails(lCustID, CurrentCulture);
                        logData.RecordStep("Response received successfully from business layer. Result is passed.");

                        if (customerAccountDetails != null)
                        {
                            List<CouponDetails> couponDetailsList = _couponProvider.GetAvailableCoupons(out totalCoupons, houseHoldId);
                            logData.RecordStep("Response received successfully from business layer. Result is passed.");

                            List<VoucherDetails> UnusedVoucherDeatils = _emaildownloadprovider.GetUnusedVoucherDeatils(lCustID, Convert.ToInt64(cardNumber), CurrentCulture);
                            logData.RecordStep("Response received successfully from business layer. Result is passed.");

                            if (couponDetailsList != null && UnusedVoucherDeatils != null)
                            {
                                logData.RecordStep("Before:GetBackgroungTemplate");
                                PdfBackgroundTemplate template = GetBackgroungTemplate();
                                logData.RecordStep("After:GetBackgroungTemplate");
                                using (MemoryStream document = _pdfProvider.GetCouponsAndVouchersDocument(
                                    new VoucherandCouponDetails { CouponDetails = couponDetailsList, VoucherDetails = UnusedVoucherDeatils }, customerAccountDetails, template))
                                {
                                    _emaildownloadprovider.RecordCouponAndVoucherPrintedDataSet(UnusedVoucherDeatils, couponDetailsList, customerID, cardNumber, 0);
                                    return File(document.ToArray(), "application/pdf", Server.UrlEncode(template.lblstrVouchersAndCouponsFileName));
                                }


                            }
                            else
                            {
                                logData.RecordStep("Both coupon and voucher details are null for this customer.");
                                return RedirectToAction("Login");
                            }
                        }
                        else
                        {
                            logData.RecordStep("Customer account details are not presented in DB.");
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        logData.RecordStep("Household ID is null for this customer.");
                        return RedirectToAction("Login");
                    }

                }
                logData.RecordStep("Customer Id is null or NOT_VALID");
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed to download coupon and voucher details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            finally
            {
                _logger.Submit(logData);
            }
        }

        [HttpGet]
        public ActionResult DownloadTokens(string a, string b, string c)
        {
            long bookingidval = Convert.ToInt64(b);
            long productlineidval = Convert.ToInt64(c);
            Guid gid = new Guid(a);
            List<Token> tokenDetails = null;
            
            bool bPdfGenerated = false;
            LogData logdata = new LogData();
            try
            {
                tokenDetails = new List<Token>();
                logdata.RecordStep(string.Format("gid : {0}", gid));
                logdata.RecordStep(string.Format("bookingidval : {0}", bookingidval));
                logdata.RecordStep(string.Format("productlineidval : {0}", productlineidval));

                tokenDetails = _boostsAtTescoProvider.GetTokens(gid, bookingidval, productlineidval, CurrentCulture);
                
                logdata.RecordStep("Response received successfully from business layer. Result is passed.");
                    
                if (tokenDetails.Count > 0)
                {
                    logdata.RecordStep("Before:GetBoostBackgroundTemplate");
                    BoostBackgroundTemplate boostBackGroundTemplate = GetBoostBackgroundTemplate();
                    logdata.RecordStep("After:GetBoostBackgroundTemplate");
                    using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<Token, BoostBackgroundTemplate>(tokenDetails, boostBackGroundTemplate, null))
                    {
                        bPdfGenerated = true;
                        return File(document.ToArray(), "application/pdf", Server.UrlEncode(boostBackGroundTemplate.lblstrTokenFileName));
                    }
                }
                else
                {
                    logdata.RecordStep("Token details are null for this customer");
                    return RedirectToAction("Login");
                }
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException("", exp, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            finally
            {
                if (bPdfGenerated)
                {
                    long custID = 0;
                    string tokenFalg = "E";
                    _boostsAtTescoProvider.RecordRewardTokenPrintDetails(tokenDetails, custID, tokenFalg);
                    _logger.Submit(logdata);
                }
            }
            

        }
        
        private BoostBackgroundTemplate GetBoostBackgroundTemplate()
        {
            BoostBackgroundTemplate template = new BoostBackgroundTemplate();
            try
            {
                template.ReplaceClubcardPrefix = this._configProvider.GetStringAppSetting(AppConfigEnum.ReplaceClubcardPrefix);
                template.PrintBGImagePath = HttpContext.Server.MapPath(this._configProvider.GetStringAppSetting(AppConfigEnum.PrintBGImagePath));
                template.FontPath = this._configProvider.GetStringAppSetting(AppConfigEnum.FontPath);
                template.IsAlphaCodeRequired = this._configProvider.GetStringAppSetting(AppConfigEnum.IsAlphaCodeRequired);
                template.CultureDefaultloc = this._configProvider.GetStringAppSetting(AppConfigEnum.Culture);

                template.lblstrCurrencySymbol = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCurrencySymbol").ToString();
                template.lblstrClubcardBoostatTesco = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardBoostatTesco").ToString();
                template.lblstrCustomerPrintedClubcardBoostToken = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCustomerPrintedClubcardBoostToken").ToString();
                template.lblstrDateFormat = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrDateFormat").ToString();
                template.lblstrDatePrinted = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrDatePrinted").ToString();

                template.lblstrInaSingleTransaction = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrInaSingleTransaction").ToString();
                template.lblstrLine1 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine1").ToString();
                template.lblstrLine2 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine2").ToString();
                template.lblstrLine3 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine3").ToString();
                template.lblstrLine4 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine4").ToString();
                template.lblstrLine5 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine5").ToString();

                template.lblstrLine6 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine6").ToString();
                template.lblstrLine7 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine7").ToString();
                template.lblstrLine8 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine8").ToString();

                template.lblstrLine9 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine9").ToString();
                template.lblstrLine10 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine10").ToString();
                template.lblstrLine11 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine11").ToString();
                template.lblstrLine12 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine12").ToString();
                template.lblstrLine13 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrLine13").ToString();

                template.lblstrOFF = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrOFF").ToString();
                template.lblstrOrMoreOn = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrOrMoreOn").ToString();
                template.lblstrSerialNumber = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrSerialNumber").ToString();
                template.lblstrToken = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrToken").ToString();
                template.lblstrTokenFileName = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTokenFileName").ToString();

                template.lblstrValidUntil = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrValidUntil").ToString();
                template.lblstrWhenYouSpend = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrWhenYouSpend").ToString();
            }
            catch (Exception exp)
            {

                throw;
            }
          
            return template;
        }

        private PdfBackgroundTemplate GetBackgroungTemplate()
        {
            PdfBackgroundTemplate template=null;
            
            try
            {
                string culture = CurrentCulture;
                System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(culture);
                string cultureNoHyphen = culture.Replace("-", String.Empty);

                template = new PdfBackgroundTemplate();
                template.ReplaceClubcardPrefix = _configProvider.GetStringAppSetting(AppConfigEnum.ReplaceClubcardPrefix);
                template.PrintBGImagePath = HttpContext.Server.MapPath(this._configProvider.GetStringAppSetting(AppConfigEnum.PrintBGImagePath));
                template.IsAlphaCodeRequired = _configProvider.GetStringAppSetting(AppConfigEnum.IsAlphaCodeRequired);
                template.IsHideCustomerName = _configProvider.GetStringAppSetting(AppConfigEnum.IsHideCustomerName);

                template.lblstrClubcardVouchers = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrClubcardVouchers", cultureObj).ToString();
                template.lblstrBonusvoucher = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrBonusvoucher", cultureObj).ToString();
                template.lblstrTopup = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrTopup", cultureObj).ToString();
                template.lblstrTopup_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", String.Format("lblstrTopup{0}", cultureNoHyphen), cultureObj).ToString();
                template.lblClubcardVoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", String.Format("lblClubcardVoucher{0}", cultureNoHyphen), cultureObj).ToString();
              
                template.lblstrBonusvoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", String.Format("lblstrBonusvoucher{0}", cultureNoHyphen), cultureObj).ToString();
                template.lblClubcardVoucher_Culture = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", String.Format("lblClubcardVoucher{0}", cultureNoHyphen), cultureObj).ToString();
                template.lblstrTopup = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", String.Format("lblstrTopup{0}", cultureNoHyphen), cultureObj).ToString();
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
                template.Culture = CurrentCulture;
               

                
            }
            catch (Exception exp)
            {
                
                throw exp;
            }
         
            return template;
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            if (IsDotcomEnvironmentEnabled)
            {
                Redirect(ConfigurationManager.AppSettings[ParameterNames.GENERIC_LOGOUT_PAGE].ToString());
            }
            
            return View(model);
        }
    }
}
