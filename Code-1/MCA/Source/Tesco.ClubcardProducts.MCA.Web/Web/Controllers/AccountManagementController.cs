using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class AccountManagementController : BaseController
    {
        #region Private Fields

        private ICustomerPreferenceBC _customerPreferenceBC;
        private IPersonalDetailsBC _personalDetailsProvider;
        private IAccountBC _accountProvider;
        private IManageCardsBC _myCardsProvider;
        private IConfigurationProvider _configurationProvider;
        private IOrderAReplacementBC _orderReplacementProvider;
        ILoggingService _AuditLoggerService = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountManagementController()
        {
            _customerPreferenceBC = ServiceLocator.Current.GetInstance<ICustomerPreferenceBC>();
            _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _orderReplacementProvider = ServiceLocator.Current.GetInstance<IOrderAReplacementBC>();
            _AuditLoggerService = new AuditLoggingService();

            _myCardsProvider = ServiceLocator.Current.GetInstance<IManageCardsBC>();
            _accountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
        }

        /// <summary>
        /// Constructor to instanciate the controller object
        /// </summary>
        /// <param name="_IpersonalDetailsBC">IPersonalDetailsBC</param>
        /// <param name="configurationProvider">IConfigurationProvider</param>
        public AccountManagementController(
            ICustomerPreferenceBC customerPreferenceBC, 
            IPersonalDetailsBC _IpersonalDetailsBC, 
            IConfigurationProvider configurationProvider, 
            IOrderAReplacementBC _IOrderReplacementBC, 
            ILoggingService _auditlogger, 
            IAccountBC _IAccountBC)
            : base(_auditlogger, _IAccountBC, configurationProvider)
        {
            _customerPreferenceBC = customerPreferenceBC;
            _personalDetailsProvider = _IpersonalDetailsBC;
            _configurationProvider = configurationProvider;
            _orderReplacementProvider = _IOrderReplacementBC;
            _AuditLoggerService = _auditlogger;
        }

        #endregion

        #region Public Actions

        #region Personal Details

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult PersonalDetails(string success, string showMessage)
        {
            LogData logData = new LogData();
            try
            {
                long lCustomerID = this.GetCustomerId();
                logData.CustomerID = lCustomerID.ToString(); ;
                PersonalDetailsViewModel viewModel = this.GetHome(success, showMessage, lCustomerID);
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
        public ActionResult PersonalDetails(PersonalDetailsViewModel viewModel, string Command)
        {
            LogData logData = new LogData();
            try
            {
                long lCustomerID = base.GetCustomerId();

                logData.CustomerID = lCustomerID.ToString();

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
                        ModelState.AddModelError("errSummary", "CorrectFollowing");
                    }
                    else
                    {
                        logData.RecordStep("Verifying Profanity");
                        if (_personalDetailsProvider.IsProfaneText(viewModel, lCustomerID, resourceKeys))
                        {
                            logData.RecordStep("Profanity is true");
                            ModelState.AddModelError("errSummary", "Profanity");
                        }
                        else
                        {
                            logData.RecordStep("Verifying Duplicacy");
                            if (!_personalDetailsProvider.IsAccountDuplicate(viewModel, lCustomerID, resourceKeys))
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
                        _personalDetailsProvider.SetCustomerDataView(viewModel, lCustomerID, resourceKeys);
                        logData.RecordStep("Successfully saved customer data.");

                        logData.RecordStep(String.Format("Value of viewModel.HiddenMessage - {0}", viewModel.HiddenMessage));

                        _logger.Submit(logData);

                        if (viewModel.HiddenMessage == "true")
                        {
                            return RedirectToAction("PersonalDetails", "AccountManagement", new { success = "Yes", showMessage = "true" });
                        }
                        else
                        {
                            return RedirectToAction("PersonalDetails", "AccountManagement", new { success = "Yes", showMessage = "false" });
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
                                                                            "~/Views/AccountManagement/PersonalDetails.cshtml",
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
                    ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "dob.valid", currCulture).ToString());
                }
                else if (GeneralUtility.GetAge(dob) < 18)
                {
                    logData.RecordStep(String.Format("ModelState IsValid before DOB Min Age Check - {0}", ModelState.IsValid));
                    ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "over18.valid", currCulture).ToString());
                }

                if (!string.IsNullOrEmpty(viewModel.HiddenEmailPref) && viewModel.HiddenEmailPref == "true" 
                    && string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].EmailAddress))
                {
                    ModelState.AddModelError("errEmail", HttpContext.GetLocalResourceObject(
                        "~/Views/AccountManagement/PersonalDetails.cshtml", "changeEmailPref.valid", currCulture).ToString());
                }
                else if (!string.IsNullOrEmpty(viewModel.HiddenMobilePref) && viewModel.HiddenMobilePref == "true" 
                    && string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MobileNumber))
                {
                    ModelState.AddModelError("errMobile", HttpContext.GetLocalResourceObject(
                        "~/Views/AccountManagement/PersonalDetails.cshtml", "textPref.valid", currCulture).ToString());
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
                        ModelState.AddModelError("errHouseno", HttpContext.GetLocalResourceObject(
                            "~/Views/AccountManagement/PersonalDetails.cshtml", "houseNo.valid", currCulture).ToString());
                    }
                }
                if (!errorKeys.Contains("CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode"))
                {
                    if (_configurationProvider.GetStringConfigurations(DbConfigurationTypeEnum.Group_Config_Values, DbConfigurationItemNames.GroupCountryAddress).Equals("0") &&
                        viewModel.CustomerFamilyMasterData.CustomerData.Count > 0 &&
                        !string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode) 
                        && !string.IsNullOrEmpty(viewModel.HiddenPostcode))
                    {
                        if (GeneralUtility.FormatPostalCode(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper() !=
                        GeneralUtility.FormatPostalCode(viewModel.HiddenPostcode).ToUpper() || (viewModel.FindAddressClicked && viewModel.AddressDetails == null))
                        {
                            ModelState.AddModelError("errAddressChange", 
                                HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "addressChange.valid", currCulture).ToString());
                        }
                    }
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in IsValidModelState while validating Data", ex, 
                    new Dictionary<string, object>() 
                        { 
                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                        });
            }
        }

        private Dictionary<string, string> GetResourceKeys()
        {
            Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
            resourceKeys.Add("PageName", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "PageName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackFName", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackFName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMName", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackMName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackSName", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackSName", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectDay", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectDay.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectMonth", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectMonth.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SelectYear", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclSelectYear.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEmail", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackEmail", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackMobile", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackMobile", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackDayTimePhone", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackDayTimePhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackEveningPhone", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackEveningPhone", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("Race", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "selectrace", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("TrackAddress", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "TrackAddress", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("ConfirmButtonText", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/PersonalDetails.cshtml", "btnConfirmPersonalDtlsResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("SaveChangesButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnSaveAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            resourceKeys.Add("FindAddressButtonText", HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "btnFindAddressResource1.Text", CultureInfo.CurrentCulture).ToString());
            return resourceKeys;
        }

        #endregion

        #region Contact Preferences

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult ContactPreferences()
        {
            LogData logData = new LogData();
            try
            {
                long lCustID = base.CustomerId.TryParse<long>();
                logData.CustomerID = lCustID.ToString();

                CustomerFamilyMasterData customerData = _personalDetailsProvider.GetCustomerDetails(lCustID);
                CustomerPreference preferences = _personalDetailsProvider.GetCustomerPreferences(lCustID);

                ContactPreferencesModel model = new ContactPreferencesModel();

                model.ContactPreference = _customerPreferenceBC.GetContactModel(preferences, customerData);
                model.OptIns = _customerPreferenceBC.GetOptIns(preferences, customerData, true);

                //model.ContactPreference.Braille = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", model.ContactPreference.IsPostLargePrintOpted 
                //                                    ? "lblLargeprintOpted" : model.ContactPreference.IsBrailleOpted ? "lblBrailleOpted" : "lblBrailleStatementNotOptIn");
                model.ContactPreference.InvalidEMail = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "errMessageEmail");
                model.ContactPreference.InvalidMobile = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "errMessageMobile");
                model.ContactPreference.CompareEMail = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "errCompareEmail");
                model.ContactPreference.CompareMobile = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "errMessageConfirmMobile");

                model.ContactPreference.ConfirmEmail = string.Empty;
                model.ContactPreference.ConfirmMobile = string.Empty;  

                if (preferences == null || preferences.Preference == null || preferences.Preference.Count() == 0)
                {
                    logData.RecordStep("No preferences for the customer.");
                    throw new Exception("No preferences for the customer.");
                }
                _logger.Submit(logData);
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountManagement while getting Contact Preferences data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        [HttpParamAction]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult ConfirmPreferences(ContactPreferencesModel model)
        {
            LogData logData = new LogData();
            AccountDetails customerdetails = new AccountDetails();
            try
            {
                long customerId = CustomerId.TryParse<Int64>();
                logData.CustomerID = customerId.ToString();

                if (_customerPreferenceBC.ValidateContactPreferences(model.ContactPreference))
                {
                    model.ContactPreference.IsValid = true;
                    bool isUpdateProfileRequired = false;
                    CustomerFamilyMasterData _orignalCustomerData = _personalDetailsProvider.GetCustomerDetails(customerId);
                    CustomerFamilyMasterDataUpdate _customerData = _customerPreferenceBC.GetCustomerUpdateModel(
                        _orignalCustomerData, model.ContactPreference, out isUpdateProfileRequired);

                    customerdetails = _accountProvider.GetMyAccountDetail(customerId, CurrentCulture);
                    if (isUpdateProfileRequired)
                    {
                        if (!_accountProvider.IsAccountDuplicate(_customerData))
                        {
                            model.ContactPreference.TrackEmail = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "TrackEmail");
                            model.ContactPreference.TrackMobile = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "TrackMobileNumber");
                            _customerPreferenceBC.UpdateContactPreferences(model.ContactPreference, customerdetails);
                            _personalDetailsProvider.UpdateCustomerDetails(_customerData);
                            _customerPreferenceBC.SendEmailToCustomer(customerId, model.ContactPreference, 
                                customerdetails, HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "PageName"));
                            model.ContactPreference.IsSaved = true;
                        }
                        else
                        {
                            model.ContactPreference.IsSaved = model.ContactPreference.IsValid = false;
                            model.ContactPreference.ErrorMessage = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "AccountDuplicate");
                        }
                    }
                    else
                    {
                        _customerPreferenceBC.UpdateContactPreferences(model.ContactPreference, customerdetails);
                        model.ContactPreference.IsSaved = true;
                    }

                    ModelState.Remove("ContactPreference.ConfirmEmail");
                    ModelState.Remove("ContactPreference.ConfirmMobile");
                    model.ContactPreference.ConfirmEmail = string.Empty;
                    model.ContactPreference.ConfirmMobile = string.Empty;  
                }
                else
                {
                    model.ContactPreference.IsSaved = model.ContactPreference.IsValid = false;
                }
                _logger.Submit(logData);
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountManagement while posting Contact Preferences data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }

        }

        [HttpParamAction]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult ConfirmOptins(ContactPreferencesModel model)
        {
            LogData _logData = new LogData();
            try
            {
                long customerID = this.CustomerId.TryParse<long>();
                _logData.CustomerID = customerID.ToString();
                AccountDetails customerdetails = _accountProvider.GetMyAccountDetail(customerID, CurrentCulture);
                _customerPreferenceBC.UpdateOptIns(model.OptIns, customerdetails);
                model.ContactPreference.IsValid = model.OptIns.IsSaved = true;
                model.ContactPreference = _customerPreferenceBC.RetainContactPrefValues(model.ContactPreference);
                _logger.Submit(_logData);
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountManagement while posting Optins data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        #endregion

        #region Voucher Schemes

        // GET: /OptionsAndBenefits/
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult VoucherSchemes()
        {
            LogData logData = new LogData();
            try
            {
                long lCustomerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = lCustomerID.ToString();

                CustomerFamilyMasterData customerData = _personalDetailsProvider.GetCustomerDetails(lCustomerID);
                CustomerPreference preference = _personalDetailsProvider.GetCustomerPreferences(lCustomerID);
                OptionsAndBenefitsModel model = _customerPreferenceBC.GetOptionAndBenefitsModel(preference, lCustomerID.ToString());

                model.CustomerEmail = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) 
                                        ? customerData.CustomerData[0].EmailAddress : string.Empty;
                model.Email.Email = model.Email.ValidEmail = model.CustomerEmail;
                if (model.Preference == null && model.Preference.Preference.Count() == 0)
                {
                    throw new Exception();
                }
                else
                {
                    logData.CaptureData("Options & benefits", model.Preference);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while getting Options & Benefits data on load", ex, new Dictionary<string, object>() 
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

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult VoucherSchemes(OptionsAndBenefitsModel model)
        {
            LogData logData = new LogData();
            try
            {
                long lCustomerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = lCustomerID.ToString();
                bool validateMembership = _customerPreferenceBC.ValidateCustomerPreferences(model);                
                if (validateMembership)
                {
                    ModelState.Clear();                    
                    AccountDetails customerDetails = _accountProvider.GetMyAccountDetail(lCustomerID, CurrentCulture);
                    ContactModel contactModel = new ContactModel
                    {
                        Email = model.Email.Email,
                        ConfirmEmail = model.Email.Email,
                        CustomerIDEncr = CryptoUtility.EncryptTripleDES(CustomerId),
                        CustomerID = CustomerId.TryParse<Int64>(),
                        SelectedPreferenceID = (Int16)PreferenceEnum.E_Mail_Contact
                    };
                    bool hasError = false;
                    //check if customer has opted for miles preference
                    if (_customerPreferenceBC.IsMilePreference(model.SelectedPreferenceID))
                    {
                        bool isUpdateProfileRequired = false;
                        CustomerFamilyMasterData _orignalCustomerData = _personalDetailsProvider.GetCustomerDetails(CustomerId.TryParse<Int64>());
                        CustomerFamilyMasterDataUpdate _customerData = _customerPreferenceBC.GetCustomerUpdateModel(_orignalCustomerData, contactModel, out isUpdateProfileRequired);
                        logData.RecordStep(string.Format("customer email is changed : {0}", isUpdateProfileRequired));
                        if (isUpdateProfileRequired)
                        {
                            hasError = _accountProvider.IsAccountDuplicate(_customerData);
                            logData.RecordStep(string.Format("hasError : {0}", hasError));
                            if (!hasError)
                            {
                                customerDetails.EmailAddress = string.IsNullOrEmpty(model.CustomerEmail) ? string.Empty : model.Email.Email;
                                _customerPreferenceBC.UpdateContactPreferences(contactModel, customerDetails);
                                logData.RecordStep("contact preferences set email");
                                _personalDetailsProvider.UpdateCustomerDetails(_customerData);
                                logData.CaptureData("_customerData", _customerData);
                                logData.RecordStep("email updated");
                            }
                        }
                        else
                        {                            
                            customerDetails.EmailAddress = string.IsNullOrEmpty(model.CustomerEmail) ? string.Empty : model.CustomerEmail;
                            _customerPreferenceBC.UpdateContactPreferences(contactModel, customerDetails);
                            logData.RecordStep("contact preferences set email");
                        }
                    }
                    if (!hasError)
                    {
                        customerDetails.EmailAddress = string.IsNullOrEmpty(model.CustomerEmail) ? string.Empty : model.CustomerEmail;
                        _customerPreferenceBC.UpdateCustomerPreferences(model, customerDetails, CustomerId);
                        logData.RecordStep("voucher scheme updated");
                        model.IsSaved = model.IsValid = true;
                        model.Email.IsValid = true;
                    }
                    else
                    {
                        model.IsSaved = model.IsValid = false;
                        model.Email.IsValid = false;
                        model.ErrorIds = "duplicate";
                    }       
                    _logger.Submit(logData);
                    model.AviosMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Premium || model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Standard) ? model.AviosClubDetails.MembershipID : "";
                    model.BaMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Premium || model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Standard) ? model.BAMilesClubDetails.MembershipID : "";
                    model.VirginMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Virgin_Atlantic) ? model.VirgnClubDetails.MembershipID : "";                    
                    return View(model);
                }
                else
                {
                    model.IsSaved = model.IsValid = false;
                    model.ErrorIds = "membership";
                    _logger.Submit(logData);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountManagement while posting Voucher Schemes data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        #endregion

        #region Clubcards On Account

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult ClubcardsOnAccount()
        {
            LogData logData = new LogData();
            ManageCardsViewModel model = new ManageCardsViewModel();

            try
            {
                long lCustomerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = lCustomerID.ToString();
                List<HouseholdCustomerDetails> customersList = _accountProvider.GetHouseHoldCustomersData(lCustomerID, CurrentCulture);

                CustomerDisplayName customerName = new CustomerDisplayName();
                GeneralUtility name = new GeneralUtility();
                foreach (var customer in customersList)
                {
                    customerName.TitleEnglish = string.IsNullOrEmpty(customer.TitleEnglish) ? string.Empty : customer.TitleEnglish;
                    customerName.Name1 = string.IsNullOrEmpty(customer.Name1) ? string.Empty : customer.Name1;
                    customerName.Name2 = string.IsNullOrEmpty(customer.Name2) ? string.Empty : customer.Name2;
                    customerName.Name3 = string.IsNullOrEmpty(customer.Name3) ? string.Empty : customer.Name3;
                    customer.FullName = name.GetCustomerDisplayName(customerName, "CLUBCARDSONACCOUNT");
                }

                string welcomeMessageNames = string.Empty;
                string saperator = HttpContext.GetLocalResource("~/Views/AccountManagement/ClubcardsOnAccount.cshtml", "AndSeprator");
                customersList.ForEach(c => welcomeMessageNames += string.Format("{0} {1} ", c.FullName, saperator));
                welcomeMessageNames = welcomeMessageNames.Substring(0, welcomeMessageNames.Length - (saperator.Length + 1));
                ViewBag.WelcomeMessageNames = welcomeMessageNames;

                foreach (HouseholdCustomerDetails cust in customersList)
                {
                    cust.Cards = _accountProvider.GetClubcardsCustomerData(cust.CustomerID, CurrentCulture);
                }

                model = _myCardsProvider.GetManageCardsModel(customersList);
                _logger.Submit(logData);
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in AccountManagement while getting Clubcards On Account data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }
        #endregion

        #region Order A New Card
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        public ActionResult OrderANewCard()
        {
            LogData logData = new LogData();

            List<string> blackListFields = new List<string>();
            try
            {
                long lCustomerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = lCustomerID.ToString();
                
                ViewBag.dtmTitle = HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "dtmPageTitle.Text", CultureInfo.CurrentCulture).ToString();
                OrderAReplacementModel orderAReplacementModel = new OrderAReplacementModel();

                orderAReplacementModel.IsInProcess = true;
                //orderAReplacementModel = _orderReplacementProvider.GetOrderAReplacementModel(base.CustomerId.TryParse<long>(), CurrentCulture);
                //blackListFields.Add(orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString());
                //logData.BlackLists = blackListFields;
                //logData.CaptureData("Is Order in Process? ", orderAReplacementModel.IsInProcess);

                _logger.Submit(logData);
                return View(orderAReplacementModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while loading Order Replacement page", ex,
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
        public ActionResult OrderANewCard(OrderAReplacementModel orderAReplacementModel)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();

            try
            {
                long lCustomerID = this.GetCustomerId();
                logData.CustomerID = logDataAudit.CustomerID = lCustomerID.ToString();

                orderAReplacementModel.OrderReplacementModel.ClubcardNumber = _orderReplacementProvider.GetOrderAReplacementModel(
                    lCustomerID, CurrentCulture).OrderReplacementModel.ClubcardNumber;
                
                if (orderAReplacementModel.OrderReplacementModel.Reason != null)
                {
                    ViewBag.dtmTitle = HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "dtmOrderSubmitted", CultureInfo.CurrentCulture).ToString();
                    logData.RecordStep(string.Format("Processing Order Replacement Request: {0}", orderAReplacementModel.OrderReplacementModel.Reason));
                    orderAReplacementModel.IsInProcess = _orderReplacementProvider.ProcessOrderReplacementRequest(orderAReplacementModel.OrderReplacementModel);
                    logData.CaptureData("Request Reason for Replacement: ", orderAReplacementModel.OrderReplacementModel.Reason.ToString());
                    logDataAudit.RecordStep(String.Format("Order replacement|{0}",
                           new
                           {
                               Reason = orderAReplacementModel.Reasons[orderAReplacementModel.OrderReplacementModel.Reason].ToString(),
                               CCLastDigits = orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString().Substring(
                               orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString().Length - 4)
                           }.JsonText()));
                }
                else
                {
                    orderAReplacementModel.errorMsg = true;
                    logData.RecordStep(string.Format("Error Message: {0}", orderAReplacementModel.errorMsg));
                }
                _logger.Submit(logData);
                return View(orderAReplacementModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed  while processing Order Replacement request", ex,
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
        #endregion

        #endregion
    }
}
