using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System.Collections;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class PersonalDetailsController : BaseController
    {
        //
        // GET: /PersonalDetails/
        private IPersonalDetailsBC _personalDetailsProvider;
        private IConfigurationProvider _configurationProvider;

        public PersonalDetailsController()
        {
            _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();

        }
        public PersonalDetailsController(IPersonalDetailsBC _IpersonalDetailsBC, IConfigurationProvider configurationProvider)
        {
            _personalDetailsProvider = _IpersonalDetailsBC;
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
        public ActionResult Home(string success, string showMessage)
        {
            LogData logData = new LogData();
            try
            {
                long customerID = this.GetCustomerId();
                logData.CustomerID = customerID.ToString();
                PersonalDetailsViewModel viewModel = this.GetHome(success, showMessage, customerID);
                _logger.Submit(logData);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting personal details", ex,
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
        [PageAuthorization(Order = 5)]
        public ActionResult Home(PersonalDetailsViewModel viewModel, string Command)
        {
            LogData logData = new LogData();
            try
            {
                long customerID = this.GetCustomerId();
                logData.CustomerID = customerID.ToString();

                Dictionary<string, string> resourceKeys = GetResourceKeys();

                logData.RecordStep(string.Format("Command is {0}", Command));
                logData.RecordStep(string.Format("Resource keys count {0}", resourceKeys.Count));
                logData.RecordStep("Entering into Post Section");

                logData.CaptureData("Resource keys", resourceKeys);

                if (Command == resourceKeys["ConfirmButtonText"] || Command == resourceKeys["SaveChangesButtonText"])
                {
                    logData.RecordStep("Verifying Model");
                    this.ValidateModelState(viewModel);
                    logData.RecordStep(String.Format("Model Validation Result: {0}", ModelState.IsValid));

                    var errorKeys = (from item in ViewData.ModelState
                                     where item.Value.Errors.Any()
                                     select item.Key).ToList();

                    if (!ModelState.IsValid || errorKeys.Count > 0)
                    {
                        logData.RecordStep(String.Format("Model has errors. Possible count - {0}", errorKeys.Count));
                        logData.CaptureData("ModelState Errors", ModelState);
                        ModelState.AddModelError("errSummary",  "CorrectFollowing");
                    }
                    else
                    {
                        logData.RecordStep("Verifying Profanity");
                        if (_personalDetailsProvider.IsProfaneText(viewModel, customerID, resourceKeys))
                        {
                            logData.RecordStep("Profanity is true");
                            ModelState.AddModelError("errSummary", "Profanity");
                        }
                        else
                        {
                            logData.RecordStep("Verifying Duplicacy");
                            if (!_personalDetailsProvider.IsAccountDuplicate(viewModel, customerID, resourceKeys))
                            {
                                logData.RecordStep("Account is duplicate");
                                ModelState.AddModelError("errSummary", "Duplicate");
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
                            if (_configurationProvider.GetStringAppSetting(AppConfigEnum.IsReplacementCardWithYourNewName).Equals("1") &&
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
                        _personalDetailsProvider.SetCustomerDataView(viewModel, customerID, resourceKeys);
                        logData.RecordStep("Successfully saved customer data.");

                        logData.RecordStep(String.Format("Value of viewModel.HiddenMessage - {0}", viewModel.HiddenMessage));

                        _logger.Submit(logData);

                        if (viewModel.HiddenMessage == "true")
                        {
                            return RedirectToAction("Home", "PersonalDetails", new { success = "Yes", showMessage = "true" });
                        }
                        else
                        {
                            return RedirectToAction("Home", "PersonalDetails", new { success = "Yes", showMessage = "false" });
                        }
                    }

                    if (!String.IsNullOrEmpty(viewModel.HiddenPostcode) && viewModel.FindAddressClicked)
                    {
                        logData.RecordStep("Detected previous find address click.");
                        AddressDetails addresses = _personalDetailsProvider.PopulateAddress(viewModel.HiddenPostcode);

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
                            viewModel.HiddenPostcode = viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = 
                                GeneralUtility.FormatPostalCode(viewModel.HiddenPostcode).ToUpper();
                        }
                    }
                }

                logData.RecordStep("Section for Find Address Command");
                if (Command == resourceKeys["FindAddressButtonText"])
                {
                    logData.RecordStep("Executing Find Address Command");

                    viewModel.FindAddressClicked = true;

                    logData.CaptureData("Year of Birth", viewModel.YearOfBirth);

                    if (String.IsNullOrEmpty(viewModel.YearOfBirth))
                    {
                        viewModel.YearOfBirth = resourceKeys["SelectYear"];
                    }

                    AddressDetails addresses = new AddressDetails();

                    if (viewModel.CustomerFamilyMasterData == null ||
                        viewModel.CustomerFamilyMasterData.CustomerData == null ||
                        viewModel.CustomerFamilyMasterData.CustomerData.Count == 0)
                    {
                        throw new Exception("Customer data isn't available.");
                    }

                    if (String.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode))
                    {
                        logData.RecordStep("Mailing Address PostCode is empty.");
                        addresses.IsErrorMessage = true;
                        viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = String.Empty;
                    }
                    else
                    {
                        logData.CaptureData("MailingAddressPostCode", viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode);
                        logData.RecordStep("Populating address based on postcode.");

                        addresses = _personalDetailsProvider.PopulateAddress(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode);

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
                        }

                        if ((CultureInfo.CurrentCulture.ToString() == Locale.UK)
                            && (viewModel.CustomerFamilyMasterData != null) && (viewModel.CustomerFamilyMasterData.CustomerData.Count > 0))
                        {
                            viewModel.HiddenPostcode = viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode =
                                GeneralUtility.FormatPostalCode(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper();
                        }
                    }

                    viewModel.AddressDetails = addresses;
                    ModelState.Clear();
                }
                _logger.Submit(logData);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Setting personal details", ex,
                     new Dictionary<string, object>() 
                                    { 
                                        { LogConfigProvider.EXCLOGDATAKEY, logData },
                                        { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                                    });
            }
        }

        private PersonalDetailsViewModel GetHome(string success, string showMessage, long customerID)
        {
            LogData logData = new LogData();
            try
            {
                PersonalDetailsViewModel viewModel = _personalDetailsProvider.GetCustomerDataView(customerID);
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
                                                                            "~/Views/PersonalDetails/Home.cshtml",
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
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private void ValidateModelState(PersonalDetailsViewModel viewModel)
        {
            //bool isError = false;
            DateTime dob = DateTime.MinValue;
            CultureInfo currCulture = CultureInfo.CurrentCulture;
            var errorKeys = (from item in ViewData.ModelState
                             where item.Value.Errors.Any()
                             select item.Key).ToList();
            LogData logData = new LogData();
            try
            {
                logData.RecordStep(String.Format("ModelState IsValid before DOB null Check - {0}", ModelState.IsValid));

                string inputDate = String.Format("{0}/{1}/{2}", viewModel.DayOfBirth, viewModel.MonthOfBirth, viewModel.YearOfBirth);

                if (String.IsNullOrWhiteSpace(inputDate.Replace("/", String.Empty)) &&
                    DBConfigurationManager.Instance[DbConfigurationTypeEnum.Mandatory_fields][DbConfigurationItemNames.DateOfBirth].ConfigurationValue1 == "1")
                {
                    logData.RecordStep("Inside DOB Mandatory check");
                    ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "FamilyMemberDOB1", currCulture).ToString());
                }
                else if (!String.IsNullOrWhiteSpace(inputDate.Replace("/", String.Empty)) && !inputDate.TryParseDate(out dob))
                {
                    logData.RecordStep(String.Format("Invalid DOB provided - {0}", inputDate));
                    ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "dob.valid", currCulture).ToString());
                }
                else if (GeneralUtility.GetAge(dob) < 18)
                {
                    logData.RecordStep(String.Format("ModelState IsValid before DOB Min Age Check - {0}", ModelState.IsValid));
                    ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "over18.valid", currCulture).ToString());
                }

                if (!string.IsNullOrEmpty(viewModel.HiddenEmailPref) && viewModel.HiddenEmailPref == "true" 
                    && string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].EmailAddress))
                {
                    ModelState.AddModelError("errEmail", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "changeEmailPref.valid", currCulture).ToString());
                }
                else if (!string.IsNullOrEmpty(viewModel.HiddenMobilePref) && viewModel.HiddenMobilePref == "true" 
                    && string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MobileNumber))
                {
                    ModelState.AddModelError("errMobile", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "textPref.valid", currCulture).ToString());
                }

                if (viewModel.AddressDetails != null)
                {
                    if (string.IsNullOrEmpty(viewModel.AddressDetails.AddressChosen)
                        && (string.IsNullOrEmpty(viewModel.AddressDetails.Houseno)
                        || (!RegexUtility.IsRegexMatch(viewModel.AddressDetails.Houseno,
                                    DBConfigurationManager.Instance[DbConfigurationTypeEnum.Format]
                                                            [DbConfigurationItemNames.MailingAddressLine].ConfigurationValue1.ToString(), false, false)
                                )))
                    {
                        ModelState.AddModelError("errHouseno", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "houseNo.valid", currCulture).ToString());
                    }
                }
                if (!errorKeys.Contains("CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode"))
                {
                    if (_configurationProvider.GetStringConfigurations(DbConfigurationTypeEnum.Group_Config_Values, DbConfigurationItemNames.GroupCountryAddress).Equals("0") &&
                        viewModel.CustomerFamilyMasterData.CustomerData.Count > 0 &&
                        !string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode) && !string.IsNullOrEmpty(viewModel.HiddenPostcode))
                    {
                        if (GeneralUtility.FormatPostalCode(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper() !=
                        GeneralUtility.FormatPostalCode(viewModel.HiddenPostcode).ToUpper() || (viewModel.FindAddressClicked && viewModel.AddressDetails == null))
                        {
                            ModelState.AddModelError("errAddressChange", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "addressChange.valid", currCulture).ToString());
                        }
                    }
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in IsValidModelState while validating Data", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            //return isError;
        }

        private Dictionary<string, string> GetResourceKeys()
        {
            Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
            resourceKeys.Add("PageName", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "PageName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackFName", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackFName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMName", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackMName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackSName", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackSName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectDay", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectDay.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectMonth", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectMonth.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectYear", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectYear.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEmail", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackEmail", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMobile", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackMobile", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackDayTimePhone", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackDayTimePhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEveningPhone", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackEveningPhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("Race", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "selectrace", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackAddress", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "TrackAddress", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("ConfirmButtonText", HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "btnConfirmPersonalDtlsResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SaveChangesButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnSaveAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("FindAddressButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnFindAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            return resourceKeys;
        }

    }
}
