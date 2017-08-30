using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Globalization;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Web.UI;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class PointsController : BaseController
    {
        private IPointsBC _pointsProvider;
        private IConfigurationProvider _configurationProvider;

        public PointsController()
        {
            _pointsProvider = ServiceLocator.Current.GetInstance<IPointsBC>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();

        }

        public PointsController(IPointsBC _IpointsBC, IConfigurationProvider configurationProvider)
        {
            _pointsProvider = _IpointsBC;
            _configurationProvider = configurationProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            PointsViewModel pointsViewModel = new PointsViewModel();
            try
            {
                bool isDecimalDisabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
                string dateFormat = _configurationProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);

                logData.RecordStep(string.Format("CurrentCulture : {0}", CurrentCulture));

                var customerID = this.GetCustomerId();

                logData.CustomerID = customerID.ToString();

                pointsViewModel = _pointsProvider.GetPointsViewdetails(customerID, CurrentCulture);
                if (pointsViewModel.Offers.Count != 0)
                {
                    MCACookie.Cookie.Add(MCACookieEnum.CurrentOfferID, pointsViewModel.Offers[0].Id.ToString());
                    logData.RecordStep("Points Offers Count is available");
                    bool isDateRangeEnabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DateRangeForCollectionPeriod);
                    ViewBag.CurrentMonthName = Common.Utilities.GeneralUtility.GetColMonthName(pointsViewModel.Offers[0].EndDateTime.Value, true, CultureInfo.CurrentCulture);
                    
                    ViewBag.StartDate = pointsViewModel.Offers[0].StartDateTime.Value.ToString(dateFormat, CultureInfo.InvariantCulture);
                    ViewBag.EndDate = pointsViewModel.Offers[0].EndDateTime.Value.ToString(dateFormat, CultureInfo.InvariantCulture);
                    
                    if (isDecimalDisabled)
                    {
                        logData.RecordStep("Decimal Disabled");
                        pointsViewModel.Offers[0].Vouchers = Common.Utilities.GeneralUtility.GetDecimalTrimmedCurrencyVal(Convert.ToString(pointsViewModel.Offers[0].Vouchers));
                    }

                    if (isDateRangeEnabled)
                    {
                        logData.RecordStep("DateRange is enabled");
                        foreach (var offer in pointsViewModel.Offers)
                        {
                            if (offer.Period != null)
                            {
                                if (offer.Period.ToUpper() == "CURRENT")
                                {
                                    ViewBag.Period = offer.Period = String.Format("{0} - {1}", 
                                        offer.StartDateTime.TryParse<DateTime>().ToString(dateFormat, CultureInfo.InvariantCulture),
                                        offer.EndDateTime.TryParse<DateTime>().ToString(dateFormat, CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    offer.Period = String.Format("{0} - {1}", 
                                        offer.StartDateTime.TryParse<DateTime>().ToString(dateFormat, CultureInfo.InvariantCulture),
                                        offer.EndDateTime.TryParse<DateTime>().ToString(dateFormat, CultureInfo.InvariantCulture));
                                }
                            }
                        }
                    }
                    else
                    {
                        logData.RecordStep("DateRange is disabled");
                        foreach (var offer in pointsViewModel.Offers)
                        {
                            if (offer.Period != null)
                            {
                                if (CurrentCulture != Locale.UK)
                                {
                                    if (offer.Period.ToUpper() == "CURRENT")
                                    {
                                        ViewBag.Period = offer.Period = Common.Utilities.GeneralUtility.GetGroupMonthName(offer.Period.ToString().ToUpper());
                                    }
                                    else
                                    {
                                        offer.Period = Common.Utilities.GeneralUtility.GetGroupMonthName(offer.Period.ToString().ToUpper());
                                    }
                                }
                                else
                                {
                                    if (offer.Period.ToUpper() == "CURRENT")
                                    {
                                        ViewBag.Period = offer.Period;
                                    }
                                }
                            }
                        }
                    }
                    if (pointsViewModel.Offers.Count == 2)
                    {
                        logData.RecordStep("Offers Count is two");
                        if (pointsViewModel.Offers[1].Id.ToString() != string.Empty)
                        {
                            //Cookie implementation
                            MCACookie.Cookie.Add(MCACookieEnum.firstPrevOfferID, pointsViewModel.Offers[0].Id.ToString());
                            MCACookie.Cookie.Add(MCACookieEnum.PrevOfferID, pointsViewModel.Offers[1].Id.ToString());
                        }
                    }
                    else if (pointsViewModel.Offers.Count == 3)
                    {
                        logData.RecordStep("Offers Count is three");
                        if (pointsViewModel.Offers[2].Id.ToString() != string.Empty)
                        {
                            //Cookie implementation
                            MCACookie.Cookie.Add(MCACookieEnum.firstPrevOfferID, pointsViewModel.Offers[1].Id.ToString());
                            MCACookie.Cookie.Add(MCACookieEnum.PrevOfferID, pointsViewModel.Offers[2].Id.ToString());
                        }
                    }
                }
                logData.CaptureData("Points Details", pointsViewModel);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points Contoller GET.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

            return View(pointsViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult PointsStatement(string offerid)
        {
            LogData logData = new LogData();

            PointsSummaryModel pointsSummaryView = new PointsSummaryModel();
            try
            {
                logData.RecordStep(string.Format("Current Culture: {0} ,offerid : {1} ", CurrentCulture, offerid));
                if (!string.IsNullOrEmpty(offerid) && offerid.TryParse<Int32>() != 0)
                {
                    long customerID = base.GetCustomerId();
                    logData.CustomerID = customerID.ToString();
                    pointsSummaryView = _pointsProvider.GetPointsSummaryModel(customerID, offerid.TryParse<Int32>(), CurrentCulture);
                    if (pointsSummaryView.PointsDetails == null)
                    {
                        return RedirectToAction("Home", "Points");
                    }
                }
                else
                {
                    return RedirectToAction("Home", "Points");
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Points Summary View.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return View(pointsSummaryView);
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult TransactionHistory(string offerID, string period, string clubcard, string transaction)
        {
            LogData logData = new LogData();
            logData.RecordStep(string.Format("offerid :{0}, Period: {1},transaction :{2}", offerID, period, transaction));

            try
            {
                if (string.IsNullOrEmpty(offerID) || offerID.TryParse<Int32>() == 0)
                {
                    return RedirectToAction("Home", "Points");
                }
                else
                {
                    long customerID = this.GetCustomerId();
                    logData.CustomerID = customerID.ToString();
                    CustomerTransactions customerTransaction = _pointsProvider.GetCustomerTransactions(customerID, offerID.TryParse<Int32>(), true, CurrentCulture);
                    if (customerTransaction.Offer == null && customerTransaction.Transactions == null)
                    {
                        return RedirectToAction("Home", "Points");
                    }
                    if (period != null)
                    {
                        ViewBag.Period = period.ToUpper();
                    }
                    else
                    {
                        logData.RecordStep("Period is empty");
                        ViewBag.Period = String.Empty;
                    }
                    bool isDecimalDisabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
                    ViewBag.OfferId = offerID;
                    if (customerTransaction.Transactions != null && isDecimalDisabled)
                    {
                        foreach (var trans in customerTransaction.Transactions)
                        {
                            trans.AmountSpent = Common.Utilities.GeneralUtility.GetDecimalTrimmedCurrencyVal(Convert.ToString(trans.AmountSpent));
                        }
                    }
                    this.SetDropdownValues(customerTransaction);
                    ViewBag.ClubcardTransactions = customerTransaction.Transactions;
                    return View(customerTransaction);
                }
            }

            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points Controller while getting Point Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult TransactionHistory(FormCollection selectedValue, string offerID, string period)
        {
            LogData logData = new LogData();
            logData.RecordStep(string.Format("offerid :{0}, selected Value: {1}", offerID, selectedValue));

            try
            {
                if (string.IsNullOrEmpty(offerID) || offerID.TryParse<Int32>() == 0)
                {
                    return RedirectToAction("Home", "Points");
                }
                else
                {
                    long customerID = this.GetCustomerId();
                    logData.CustomerID = customerID.ToString();
                    CustomerTransactions customerTransaction = _pointsProvider.GetCustomerTransactions(customerID, Convert.ToInt32(offerID), true, CurrentCulture);
                    if (customerTransaction.Offer == null && customerTransaction.Transactions == null)
                    {
                        return RedirectToAction("Home", "Points");
                    }
                    if (period != null)
                    {
                        ViewBag.Period = period.ToUpper();
                    }
                    else
                    {
                        logData.RecordStep("Period is empty");
                        ViewBag.Period = String.Empty;
                    }
                    bool isDecimalDisabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
                    ViewBag.OfferId = offerID;
                    if (customerTransaction.Transactions != null && isDecimalDisabled)
                    {
                        foreach (var trans in customerTransaction.Transactions)
                        {
                            if (customerTransaction.Transactions != null)
                            {
                                trans.AmountSpent = Common.Utilities.GeneralUtility.GetDecimalTrimmedCurrencyVal(Convert.ToString(trans.AmountSpent));
                            }
                        }
                    }

                    if (Request.Form[HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "btnClearSelection", CultureInfo.CurrentCulture).ToString()] != null)
                    {
                        logData.RecordStep("Clear Selection button is clicked");
                        this.SetDropdownValues(customerTransaction);
                        ViewBag.ClubcardTransactions = customerTransaction.Transactions;
                    }
                    else if (Request.Form[HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "btnClearSelection", CultureInfo.CurrentCulture).ToString()] == null)
                    {
                        logData.RecordStep("Clear Selection button is not clicked");
                        this.SetDropdownValues(customerTransaction);
                        if (selectedValue.GetValue("ClubcardId").AttemptedValue != HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() && selectedValue.GetValue("TransactionDescription").AttemptedValue != HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString())
                        {
                            if (selectedValue.GetValue("TransactionDescription").AttemptedValue == "Blank")
                            {
                                ViewBag.ClubcardTransactions = customerTransaction.Transactions.FindAll(n => n.ClubcardId.Trim() == selectedValue.Get("ClubcardId").ToString().Trim()
                                                                                                    && n.TransactionDescription.Trim() == "");
                            }
                            else
                            {
                                ViewBag.ClubcardTransactions = customerTransaction.Transactions.FindAll(n => n.ClubcardId.Trim() == selectedValue.Get("ClubcardId").ToString().Trim() && n.TransactionDescription.Trim() == selectedValue.Get("TransactionDescription").ToString().Trim());

                            }

                        }
                        else if (selectedValue.GetValue("ClubcardId").AttemptedValue != HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() && selectedValue.GetValue("TransactionDescription").AttemptedValue == HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString())
                        {
                            ViewBag.ClubcardTransactions = customerTransaction.Transactions.FindAll(n => n.ClubcardId.Trim() == selectedValue.Get("ClubcardId").ToString().Trim());
                        }
                        else if (selectedValue.GetValue("ClubcardId").AttemptedValue == HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() && selectedValue.GetValue("TransactionDescription").AttemptedValue != HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString())
                        {
                            if (selectedValue.GetValue("TransactionDescription").AttemptedValue == "Blank")
                            {
                                ViewBag.ClubcardTransactions = customerTransaction.Transactions.FindAll(n => n.TransactionDescription.Trim() == "");
                            }
                            else
                            {
                                ViewBag.ClubcardTransactions = customerTransaction.Transactions.FindAll(n => n.TransactionDescription.Trim() == selectedValue.Get("TransactionDescription").ToString().Trim());
                            }
                        }
                        else if (selectedValue.GetValue("ClubcardId").AttemptedValue == HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() && selectedValue.GetValue("TransactionDescription").AttemptedValue == HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString())
                        {
                            ViewBag.ClubcardTransactions = customerTransaction.Transactions;
                        }
                        ViewBag.ClubcardId = selectedValue.GetValue("ClubcardId").AttemptedValue;
                        ViewBag.TransactionDescription = selectedValue.GetValue("TransactionDescription").AttemptedValue;
                    }
                    logData.CaptureData("TransactionDescription", selectedValue.GetValue("TransactionDescription").AttemptedValue);
                    _logger.Submit(logData);
                    return View(customerTransaction);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points Controller while setting point details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        [HttpGet]
        [AjaxOnly]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        public JsonResult AvailablePoints()
        {
            LogData logData = new LogData();
            try
            {
                long customerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = customerID.ToString();
                Dictionary<string, string> data = _pointsProvider.GetPreviousePoints(customerID, CurrentCulture);
                return Json(new { points = data["Points"], vouchers = data["Vouchers"], upto = data["UpTo"], next = data["NextDay"] }, JsonRequestBehavior.AllowGet);
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

        private CustomerTransactions SetDropdownValues(CustomerTransactions customerTransaction)
        {
            LogData _logData = new LogData();

            try
            {
                if (customerTransaction.Transactions != null)
                {
                    List<SelectListItem> clubcards = new List<SelectListItem>();
                    var clubcardList = customerTransaction.Transactions.Select(emp => new { emp.ClubcardId }).Distinct().ToList();
                    clubcards.Add(new SelectListItem { Value = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString(), Text = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() });
                    foreach (var clubcard in clubcardList)
                    {
                        clubcards.Add(new SelectListItem { Value = clubcard.ClubcardId, Text = clubcard.ClubcardId });
                    }
                    ViewBag.SelectedClubcard = clubcards;

                    List<SelectListItem> transactions = new List<SelectListItem>();
                    transactions.Add(new SelectListItem { Value = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString(), Text = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString() });
                    var transactionList = customerTransaction.Transactions.Select(emp => new { emp.TransactionDescription }).Distinct().ToList();

                    foreach (var transaction in transactionList)
                    {
                        if (transaction.TransactionDescription == string.Empty)
                        {
                            transactions.Add(new SelectListItem { Value = "Blank", Text = "" });
                        }
                        else
                        {
                            transactions.Add(new SelectListItem { Value = transaction.TransactionDescription.Trim(), Text = transaction.TransactionDescription.Trim() });
                        }
                    }
                    ViewBag.SelectedTransaction = transactions;

                }
                else
                {
                    _logData.RecordStep("Customer Transactions are empty");
                    List<SelectListItem> clubcards = new List<SelectListItem>();
                    clubcards.Add(new SelectListItem { Value = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString(), Text = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlClubcard", CultureInfo.CurrentCulture).ToString() });
                    ViewBag.SelectedClubcard = clubcards;

                    List<SelectListItem> transactions = new List<SelectListItem>();
                    transactions.Add(new SelectListItem { Value = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString(), Text = HttpContext.GetLocalResourceObject("~/Views/Points/TransactionHistory.cshtml", "ddlTransaction", CultureInfo.CurrentCulture).ToString() });
                    ViewBag.SelectedTransaction = transactions;
                }
                _logger.Submit(_logData);

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points controller while setting dropdown values", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return customerTransaction;

        }
    }
}
