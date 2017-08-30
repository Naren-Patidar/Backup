using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class CouponsController : BaseController
    {
        IAccountBC accountProvider;
        ICouponBC couponProvider;
        IPDFGenerator pdfProvider;
        ILoggingService _AuditLoggerService = null;

        #region Constructors

        /// <summary>
        /// Default parameter less constructor
        /// </summary>
        public CouponsController()
        {
            accountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
            couponProvider = ServiceLocator.Current.GetInstance<ICouponBC>();
            pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();
            _AuditLoggerService = new AuditLoggingService();
        }

        /// <summary>
        /// Constructor with all required business interfaces as parameter
        /// </summary>
        /// <param name="accountProvider"></param>
        public CouponsController(IAccountBC accountProvider, ICouponBC couponProvider, IPDFGenerator pdfGenerator, ILoggingService _auditlogger, IConfigurationProvider configProvider)
            : base(_auditlogger, accountProvider, configProvider)
        {
            this.accountProvider = accountProvider;
            this.couponProvider = couponProvider;
            this.pdfProvider = pdfGenerator;
            this._AuditLoggerService = _auditlogger;
        }

        #endregion

        [HttpGet]
        [AuthorizeUser(Order=1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            CouponViewModel model = new CouponViewModel();
            try
            {
                string customerID = logData.CustomerID = base.GetCustomerId().ToString();
                logData.RecordStep(string.Format("CurrentCulture : {0}", CurrentCulture));
                var householdID = accountProvider.GetHouseholdId(CustomerId.TryParse<Int64>(), CurrentCulture);
                logData.RecordStep(string.Format("householdID : {0}", householdID));
                int totalCoupons = 0;
                model.HouseholdID = householdID;
                model.AvailableCoupons = couponProvider.GetAvailableCoupons(out totalCoupons, householdID);
                Dictionary<string, string> res = new Dictionary<string, string>();
                res["strCouponUsed"] = HttpContext.GetLocalResource("~/Views/Coupons/Home.cshtml", "strCouponUsed");
                res["strCouponVoided"] = HttpContext.GetLocalResource("~/Views/Coupons/Home.cshtml", "strCouponVoided");
                res["strOf"] = HttpContext.GetLocalResource("~/Views/Coupons/Home.cshtml", "strOf");
                model.RedeemedCoupons = couponProvider.GetRedeemedCoupons(householdID, CurrentCulture, res);
                model.AvalableCouponCount = model.AvailableCoupons.Count;
                model.RedeemedCouponCount = model.RedeemedCoupons.Count;
                model.IssuedCouponCount = totalCoupons;
                ViewData[ParameterNames.TOTAL_COUPONS_VALUE] = totalCoupons;
                logData.CaptureData("model", model);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Coupons Controller GET",ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult Home(List<CouponDetails> couponModel)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();
            try
            {
                long lCustomerID = base.CustomerId.TryParse<Int64>();

                logData.CustomerID = logDataAudit.CustomerID = lCustomerID.ToString();

                logData.RecordStep(string.Format("CurrentCulture : {0}", CurrentCulture));
                List<CouponDetails> selectedCoupons = couponModel.FindAll(c => c.IsSelected);
                logData.CaptureData("couponModel", couponModel);
                logData.CaptureData("selectedCoupons", selectedCoupons);
                List<CouponDetails> availableCoupons = new List<CouponDetails>();
                int totalCoupons = 0;
                string couponTitle = string.Empty;
                
                System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(CurrentCulture);

                if (selectedCoupons.Count > 0)
                {
                    couponTitle = HttpContext.GetLocalResource("~/Views/Coupons/Home.cshtml", "PDFTitleCoupon");
                    logData.RecordStep(string.Format("couponTitle: {0}", couponTitle));
                    Int64 householdID = accountProvider.GetHouseholdId(lCustomerID, CurrentCulture);
                    logData.RecordStep(string.Format("householdID: {0}", householdID));
                    availableCoupons = couponProvider.GetAvailableCoupons(out totalCoupons, householdID);
                    List<CouponDetails> coupons = availableCoupons.FindAll(c => couponModel.FindAll(c1 => c1.Id == c.Id && c1.IsSelected).Count > 0);
                    logData.CaptureData("coupons", coupons);
                    //check if coupon image is avalable
                    if (coupons.FindAll(c => string.IsNullOrEmpty(c.FullImageName)).Count > 0)
                    {
                        Exception noCouponImage = new Exception();
                        if (!noCouponImage.Data.Keys.Cast<string>().Contains(ParameterNames.FRIENDLY_ERROR_MESSAGE))
                        {
                            noCouponImage.Data.Add(ParameterNames.FRIENDLY_ERROR_MESSAGE, "");
                        }
                        _logger.Submit(logData);
                        throw noCouponImage;
                    }
                    else
                    {
                        PdfBackgroundTemplate template = couponProvider.GetCouponBackgroungTemplate(couponTitle, CurrentCulture);
                        AccountDetails account = accountProvider.GetMyAccountDetail(lCustomerID, CurrentCulture);
                        logData.CaptureData("account", account);
                        using (MemoryStream document = pdfProvider.GetPDFDocumentStream<CouponDetails, PdfBackgroundTemplate>(coupons, template, account))
                        {
                            logData.RecordStep("PDF generated successfully !!");
                            couponProvider.RecordPrintAtHomeDetails(coupons, CustomerId, account);
                            logData.RecordStep("Download action recorded successfully !!");

                            string strCouponInfo = GetAuditDetailForCoupons(coupons);
                            logDataAudit.RecordStep(string.Format("Downloaded Coupons|{0}", strCouponInfo));


                            return File(document.ToArray(), "application/pdf", Server.UrlEncode(HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCouponFileName", cultureObj).ToString()));
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("VoucherNotSelected",
                                    HttpContext.GetLocalResourceObject(
                                            "~/Views/Shared/_CouponsAvailable.cshtml",
                                            "NoCouponSeletedError",
                                            System.Globalization.CultureInfo.CurrentCulture).ToString());
                    return RedirectToAction("Home", "Coupons", new { error = "noCoupons" });
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
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

        private static string GetAuditDetailForCoupons(List<CouponDetails> availableCoupons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (CouponDetails v in availableCoupons)
            {
                if (v.CouponDescription.Length > 0)
                {
                    sb.Append(v.FullImageName.ToString());                    
                    sb.Append(", ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult CouponPreview(string image, string description)
        {
            LogData logData = new LogData();
            try
            {
                logData.CustomerID = base.GetCustomerId().ToString();
                CouponImageViewModel model = new CouponImageViewModel { CouponImageFile = string.IsNullOrEmpty(image) ? "NA" : image, CouponDescription = description };
                _logger.Submit(logData);
                return View(model);
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
        public JsonResult AvailableCoupon()
        {
            LogData logData = new LogData();
            try
            {
                string customerID = logData.CustomerID = this.GetCustomerId().ToString();
                var householdID = accountProvider.GetHouseholdId(CustomerId.TryParse<Int64>(), CurrentCulture);
                logData.RecordStep(string.Format("householdID : {0}", householdID));
                int totalCoupons = 0;                
                List<CouponDetails> coupons = couponProvider.GetAvailableCoupons(out totalCoupons, householdID);                
                _logger.Submit(logData);
                return Json(new { count = coupons.Count }, JsonRequestBehavior.AllowGet);
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
    }
}
