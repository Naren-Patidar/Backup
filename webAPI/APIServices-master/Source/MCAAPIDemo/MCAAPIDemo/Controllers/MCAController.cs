using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
//using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.Globalization;

namespace MCAAPIDemo.Controllers
{
    public class MCAController : BaseController
    {
        private VoucherBC _voucherProvider = null;
        ConfigurationProvider _configProvider = null;
        AccountBC _accountProvider;
        PersonalDetailsBC _personalDetailsProvider;

        public MCAController()
        {
            _voucherProvider =  new VoucherBC();
            _configProvider = new ConfigurationProvider();
            _accountProvider = new AccountBC();
            _personalDetailsProvider = new PersonalDetailsBC();
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
        public ActionResult Login(LoginViewModel login, string from, string returnUrl)
        {
            LogData _logData = new LogData();
            try
            {
                long customerID = 0, clubcard = 0;
                string password = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][AppConfigEnum.Password.ToString()].ConfigurationValue1.ToString();
                string sculture = base.CurrentCulture;
                _logData.RecordStep(string.Format("CurrentCulture : {0}", sculture));

                if (password != string.Empty && password == login.Password)
                {
                    if (!string.IsNullOrEmpty(login.ClubcardNumber) && Int64.TryParse(login.ClubcardNumber, out clubcard))
                    {
                        customerID = _accountProvider.GetCustomerId(clubcard, 100, sculture);
                        _logData.CustomerID = customerID.ToString();
                    }
                    if (customerID != 0)
                    {
                        ViewBag.DotcomID = login.DotcomCustomerId;
                        MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID.ToString());
                        return RedirectToAction("Home", "MCA");
                    }
                    else
                    {
                        _logData.RecordStep("Customer ID is either 0 or null/empty");
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                else
                {
                    _logData.RecordStep("Password is either empty or doesn't match");
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Account Controller|Login|POST", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        //[HttpPost]
        //[AuthorizeUser]
        //public ActionResult Home(HomeViewModel model)
        //{
        //    AccountDetails accountDetails = new AccountDetails();
        //    LogData logData = new LogData();
        //    System.Globalization.CultureInfo cultureObj = new System.Globalization.CultureInfo(CurrentCulture);
        //    try
        //    {                
        //        _logger.Submit(logData);
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw GeneralUtility.GetCustomException("Exception in Temporary clubcard generation", ex,
        //                     new Dictionary<string, object>() 
        //                    { 
        //                        { LogConfigProvider.EXCLOGDATAKEY, logData },
        //                        { ParameterNames.FRIENDLY_ERROR_MESSAGE, ""}
        //                    });

        //    }
        //}
        
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
                    return RedirectToAction("Login", "MCA");
                }


            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, ""}
                            });

            }
            finally
            {
                _logger.Submit(_logData);
            }
        }
        
        /// <summary>
        /// Calls the VoucherBC methods to retrieve AccountDetails and Voucher Summary, Unused and used Vouchers data
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public ActionResult Home(string success, string showMessage)
        {
            LogData logData = new LogData();
            try
            {
                ModelState.Clear();

                var customerID = base.CustomerId;

                #region VouchersViewModel
                MCAViewModel mcaViewModel = new MCAViewModel();

                VouchersViewModel vouchersViewModel = new VouchersViewModel();
                vouchersViewModel.isDotcomCustomerIDEmpty = false;

                if (string.IsNullOrEmpty(MCACookie.Cookie[MCACookieEnum.CustomerID]))
                {
                    logData.RecordStep("CustomerId is Null or Empty");
                    return Redirect("http://localhost:56108/api/MCA/home");
                }
                else
                {
                    vouchersViewModel = _voucherProvider.GetVoucherViewDetails(CustomerId.TryParse<Int64>(), base.CurrentCulture);
                }

                mcaViewModel.vouchersViewModel = vouchersViewModel;

                PersonalDetailsViewModel viewModel = GetHome(success, showMessage);
                mcaViewModel.personalDetailsViewModel = viewModel;
                #endregion

                _logger.Submit(logData);
                return View(mcaViewModel);
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

        private PersonalDetailsViewModel GetHome(string success, string showMessage)
        {
            LogData logData = new LogData();
            try
            {
                PersonalDetailsViewModel viewModel = _personalDetailsProvider.GetCustomerDataView(base.GetCustomerId());
                if (viewModel.YearOfBirth == "1")
                {
                    viewModel.DayOfBirth = null;
                    viewModel.MonthOfBirth = null;
                    viewModel.HiddenDOB = null;
                    viewModel.YearOfBirth = HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectYear.Text", CultureInfo.CurrentCulture).ToString();
                }
                if (success == "Yes")
                {
                    ModelState.AddModelError("msgSummary", HttpContext.GetLocalResourceObject(
                                                                            "~/Views/MCA/PersonalDetails.cshtml",
                                                                            "save.success",
                                                                            CultureInfo.CurrentCulture).ToString());
                    logData.RecordStep(string.Format("Getting personal details for customer after form submission with success status is {0} and showmessage value is {1}"
                        , success, showMessage));
                }
                if (showMessage == "true")
                {
                    viewModel.HiddenMessage = "true";
                }
                else
                {
                    viewModel.HiddenMessage = "false";
                }
                if (CultureInfo.CurrentCulture.ToString() == Locale.UK)
                {
                    viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = GeneralUtility.FormatPostalCode(viewModel.HiddenPostcode).ToUpper();
                }
                _logger.Submit(logData);
                return viewModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting personal details", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, ""}
                            });
            }
        }

        [HttpPost]
        [AuthorizeUser]
        public ActionResult PersonalDetails(PersonalDetailsViewModel viewModel, string Command)
        {
            LogData logData = new LogData();
            try
            {
                Dictionary<string, string> resourceKeys = GetResourceKeys();

                logData.RecordStep(string.Format("Command is {0}", Command));
                logData.RecordStep(string.Format("Resource keys count {0}", resourceKeys.Count));
                logData.RecordStep("Entering into Post Section");

                logData.CaptureData("Resource keys", resourceKeys);

                if (Command == resourceKeys["ConfirmButtonText"] || Command == resourceKeys["SaveChangesButtonText"])
                {
                    logData.RecordStep("Verifying Model");
                    //this.ValidateModelState(viewModel);
                    logData.RecordStep(String.Format("Model Validation Result: {0}", ModelState.IsValid));

                    long lCustomerID = base.GetCustomerId();

                    var errorKeys = (from item in ViewData.ModelState
                                     where item.Value.Errors.Any()
                                     select item.Key).ToList();

                    if (!ModelState.IsValid || errorKeys.Count > 0)
                    {
                        logData.RecordStep(String.Format("Model has errors. Possible count - {0}", errorKeys.Count));
                        logData.CaptureData("ModelState Errors", ModelState);
                        ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "correctFollowing.valid", CultureInfo.CurrentCulture).ToString());
                    }
                    else
                    {
                        logData.RecordStep("Verifying Profanity");
                        //if (_personalDetailsProvider.IsProfaneText(viewModel, lCustomerID, resourceKeys))
                        {
                            logData.RecordStep("Profanity is true");
                            ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "problemWithDetails.valid", CultureInfo.CurrentCulture).ToString());
                        }
                        //else
                        {
                            logData.RecordStep("Verifying Duplicacy");
                            //if (!_personalDetailsProvider.IsAccountDuplicate(viewModel, lCustomerID, resourceKeys))
                            {
                                logData.RecordStep("Account is duplicate");
                                ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "recordExist.valid", CultureInfo.CurrentCulture).ToString());
                            }
                        }
                    }

                    logData.RecordStep(String.Format("Error Keys count after duplicacy check: {0}", errorKeys.Count));

                    if (ModelState.IsValid && errorKeys.Count == 0)
                    {
                        logData.RecordStep(String.Format("Currently Entered Surname - {0}", viewModel.HiddenSurname));
                        logData.RecordStep(String.Format("Stored Surname - {0}", viewModel.CustomerFamilyMasterData.CustomerData[0].LastName));

                        if (viewModel.CustomerFamilyMasterData.CustomerData.Count > 0 && !string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].LastName))
                        {
                            if (_configProvider.GetStringAppSetting(AppConfigEnum.IsReplacementCardWithYourNewName).Equals("1") &&
                                DBConfigurationManager.Instance[DbConfigurationTypeEnum.HideJoinFunctionality][DbConfigurationItemNames.HideOrderAReplacementPage].ConfigurationValue1.Equals("0") &&
                                !viewModel.HiddenSurname.Trim().Equals(viewModel.CustomerFamilyMasterData.CustomerData[0].LastName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                viewModel.HiddenMessage = "true";
                            }
                            else
                            {
                                viewModel.HiddenMessage = "false";
                            }
                        }

                        viewModel.HiddenSentEmail = "false";

                        logData.RecordStep("Attempting to save customer data.");
                        //_personalDetailsProvider.SetCustomerDataView(viewModel, lCustomerID, resourceKeys);
                        logData.RecordStep("Successfully saved customer data.");

                        logData.RecordStep(String.Format("Value of viewModel.HiddenMessage - {0}", viewModel.HiddenMessage));

                        _logger.Submit(logData);

                        if (viewModel.HiddenMessage == "true")
                        {
                            return RedirectToAction("Home", "MCA", new { success = "Yes", showMessage = "true" });
                        }
                        else
                        {
                            return RedirectToAction("Home", "MCA", new { success = "Yes", showMessage = "false" });
                        }
                    }

                    if (!String.IsNullOrEmpty(viewModel.HiddenPostcode) && viewModel.FindAddressClicked)
                    {
                        logData.RecordStep("Detected previous find address click.");
                        AddressDetails addresses = null;//_personalDetailsProvider.PopulateAddress(viewModel.HiddenPostcode);

                        if (addresses != null && !addresses.IsErrorMessage)
                        {
                            if (addresses.HideAddressList != null && addresses.HideAddressList.Count > 0)
                            {
                                addresses.HideAddressformat = String.Join(":", addresses.HideAddressList.ToArray());
                            }
                            else
                            {
                                addresses.AddressList = new Dictionary<int, string>();
                            }
                            viewModel.AddressDetails = addresses;
                        }
                        if ((CultureInfo.CurrentCulture.ToString() == Locale.UK)
                            && (viewModel.CustomerFamilyMasterData != null) && (viewModel.CustomerFamilyMasterData.CustomerData.Count > 0))
                        {
                            viewModel.HiddenPostcode = viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = GeneralUtility.FormatPostalCode(viewModel.HiddenPostcode).ToUpper();
                        }
                    }
                }

                //logData.RecordStep("Section for Find Address Command");
                //if (Command == resourceKeys["FindAddressButtonText"])
                //{
                //    logData.RecordStep("Executing Find Address Command");

                //    viewModel.FindAddressClicked = true;

                //    logData.CaptureData("Year of Birth", viewModel.YearOfBirth);

                //    if (String.IsNullOrEmpty(viewModel.YearOfBirth))
                //    {
                //        viewModel.YearOfBirth = resourceKeys["SelectYear"];
                //    }

                //    AddressDetails addresses = new AddressDetails();

                //    if (viewModel.CustomerFamilyMasterData == null ||
                //        viewModel.CustomerFamilyMasterData.CustomerData == null ||
                //        viewModel.CustomerFamilyMasterData.CustomerData.Count == 0)
                //    {
                //        throw new Exception("Customer data isn't available.");
                //    }

                //    if (String.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode))
                //    {
                //        logData.RecordStep("Mailing Address PostCode is empty.");
                //        addresses.IsErrorMessage = true;
                //        viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = String.Empty;
                //    }
                //    else
                //    {
                //        logData.CaptureData("MailingAddressPostCode", viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode);
                //        logData.RecordStep("Populating address based on postcode.");

                //        addresses = _personalDetailsProvider.PopulateAddress(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode);

                //        if (addresses != null && !addresses.IsErrorMessage)
                //        {
                //            if (addresses.HideAddressList != null && addresses.HideAddressList.Count > 0)
                //            {
                //                addresses.HideAddressformat = String.Join(":", addresses.HideAddressList.ToArray());
                //            }
                //            else
                //            {
                //                addresses.AddressList = new Dictionary<int, string>();
                //            }
                //        }

                //        if ((CultureInfo.CurrentCulture.ToString() == Locale.UK)
                //            && (viewModel.CustomerFamilyMasterData != null) && (viewModel.CustomerFamilyMasterData.CustomerData.Count > 0))
                //        {
                //            viewModel.HiddenPostcode = viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode =
                //                GeneralUtility.FormatPostalCode(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper();
                //        }
                //    }

                //    viewModel.AddressDetails = addresses;
                //    ModelState.Clear();
                //}
                _logger.Submit(logData);
                return View("Home", viewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Setting personal details", ex,
                     new Dictionary<string, object>() 
                                    { 
                                        { LogConfigProvider.EXCLOGDATAKEY, logData },
                                        { ParameterNames.FRIENDLY_ERROR_MESSAGE, ""}
                                    });
            }
        }

        private Dictionary<string, string> GetResourceKeys()
        {
            Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
            resourceKeys.Add("PageName", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "PageName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackFName", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackFName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMName", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackMName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackSName", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackSName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectDay", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectDay.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectMonth", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectMonth.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectYear", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectYear.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEmail", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackEmail", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMobile", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackMobile", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackDayTimePhone", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackDayTimePhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEveningPhone", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackEveningPhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("Race", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "selectrace", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackAddress", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "TrackAddress", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("ConfirmButtonText", HttpContext.GetLocalResourceObject("~/Views/MCA/PersonalDetails.cshtml", "btnConfirmPersonalDtlsResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SaveChangesButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnSaveAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("FindAddressButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnFindAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            return resourceKeys;
        }
    }
}