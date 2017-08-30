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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.IO;
using BotDetect.Web.UI.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class JoinController : BaseController
    {
        private IJoinBC _JoinProvider;
        private IPersonalDetailsBC _personalDetailsProvider;
        private IConfigurationProvider _configurationProvider;
        ICustomerPreferenceBC _customerPreferenceBC;
        IPDFGenerator _pdfProvider;

        #region Constructors

        public JoinController()
        {
            _customerPreferenceBC = ServiceLocator.Current.GetInstance<ICustomerPreferenceBC>();
            _JoinProvider = ServiceLocator.Current.GetInstance<IJoinBC>();
            _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();

        }

        public JoinController(ICustomerPreferenceBC customerPreferenceBC,
                                IJoinBC _IJoinBC,
                                IPersonalDetailsBC _IPDBC,
                                IConfigurationProvider configurationProvider,
                                IPDFGenerator pdfGenerator)
        {
            _customerPreferenceBC = customerPreferenceBC;
            _JoinProvider = _IJoinBC;
            _personalDetailsProvider = _IPDBC;
            _configurationProvider = configurationProvider;
            _pdfProvider = pdfGenerator;
        }

        #endregion Constructors

        [HttpPost]
        [CaptchaValidation("JoinCaptchaCode", "JoinCaptcha")] //Captcha Validation Attribute. To be included for Live only
        [ValidateAntiForgeryToken]
        public ActionResult Home(PersonalDetailsViewModel viewModel, string Command)
        {
            DateTime dob = DateTime.MinValue;
            bool isError = false;
            LogData logData = new LogData();
            try
            {
                bool IsCaptchaEnabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.IsCaptchaEnabled);
                if (ModelState.ContainsKey("JoinCaptchaCode") && !IsCaptchaEnabled)
                {
                    ModelState.Remove("JoinCaptchaCode");
                }
                var modelStateErrors = ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                if (Command == HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "btnConfirmJoinResource2.Text", CultureInfo.CurrentCulture).ToString())
                {
                    logData.RecordStep("Entering Join Confirm click...");
                    if (!string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode))
                    {
                        viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode =
                            GeneralUtility.FormatPostalCode(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper();
                    }

                    logData.RecordStep(String.Format("ModelState IsValid before DOB null Check - {0}", ModelState.IsValid));

                    string inputDate = String.Format("{0}/{1}/{2}", viewModel.DayOfBirth, viewModel.MonthOfBirth, viewModel.YearOfBirth);

                    if (String.IsNullOrWhiteSpace(inputDate.Replace("/", String.Empty)) &&
                        DBConfigurationManager.Instance[DbConfigurationTypeEnum.Mandatory_fields][DbConfigurationItemNames.DateOfBirth].ConfigurationValue1 == "1")
                    {
                        logData.RecordStep("Inside DOB Mandatory check");
                        ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "FamilyMemberDOB1", CultureInfo.CurrentCulture).ToString());
                    }
                    else if (!String.IsNullOrWhiteSpace(inputDate.Replace("/", String.Empty)) && !inputDate.TryParseDate(out dob))
                    {
                        logData.RecordStep(String.Format("Invalid DOB provided - {0}", inputDate));
                        ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "dob.valid", CultureInfo.CurrentCulture).ToString());
                    }
                    else if (GeneralUtility.GetAge(dob) < 18)
                    {
                        logData.RecordStep(String.Format("ModelState IsValid before DOB Min Age Check - {0}", ModelState.IsValid));
                        ModelState.AddModelError("errDoB", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "over18.valid", CultureInfo.CurrentCulture).ToString());
                    }

                    if (_JoinProvider.IsLegalPolicyEnabled())
                    {
                        viewModel.IsLegalPolicyError = false;
                        if (!viewModel.OptLegalPolicy1 || !viewModel.OptLegalPolicy2)
                        {
                            isError = true;
                            viewModel.IsLegalPolicyError = true;
                        }
                    }

                    if (_configurationProvider.GetStringConfigurations(DbConfigurationTypeEnum.Group_Config_Values, DbConfigurationItemNames.GroupCountryAddress).Equals("0"))
                    {
                        if (viewModel.AddressDetails != null)
                        {
                            if (String.IsNullOrWhiteSpace(viewModel.AddressDetails.AddressChosen) && String.IsNullOrWhiteSpace(viewModel.AddressDetails.Houseno))
                            {
                                ModelState.AddModelError("errHouseno", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "houseNo.valid", System.Globalization.CultureInfo.CurrentCulture).ToString());
                                isError = true;
                            }
                            else if (String.IsNullOrWhiteSpace(viewModel.AddressDetails.AddressChosen) && !String.IsNullOrWhiteSpace(viewModel.AddressDetails.Houseno)
                                && !RegexUtility.IsRegexMatch(viewModel.AddressDetails.Houseno,
                                                                DBConfigurationManager.Instance[Common.Entities.Settings.DbConfigurationTypeEnum.Format]
                                                                [DbConfigurationItemNames.MailingAddressLine].ConfigurationValue1.ToString(), false, false
                                                            ))
                            {
                                ModelState.AddModelError("errHouseno", HttpContext.GetLocalResourceObject(
                                    "~/Views/Join/Home.cshtml", "houseNo.valid", System.Globalization.CultureInfo.CurrentCulture).ToString());
                                isError = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode) 
                            && !viewModel.FindAddressClicked && viewModel.AddressDetails == null)
                        {
                            ModelState.AddModelError("errPostcode", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errPostcodeFindAddress", 
                                System.Globalization.CultureInfo.CurrentCulture).ToString() + 
                                HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress2", 
                                System.Globalization.CultureInfo.CurrentCulture).ToString());
                            isError = true;
                        }
                        else if (viewModel.FindAddressClicked && viewModel.AddressDetails == null)
                        {
                            ModelState.AddModelError("errPostcode", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errPostcodeFindAddress", 
                                System.Globalization.CultureInfo.CurrentCulture).ToString() + 
                                HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress2", 
                                System.Globalization.CultureInfo.CurrentCulture).ToString());
                            isError = true;
                        }
                    }

                    if (isError || !ModelState.IsValid)
                    {
                        var errorKeys = (from item in ViewData.ModelState
                                         where item.Value.Errors.Any()
                                         select item.Key).ToList();

                        if (isError || errorKeys.Count > 0)
                        {
                            logData.CaptureData("Model State Not Valid, Validation Error Found..", modelStateErrors);
                            ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject(
                                "~/Views/Join/Home.cshtml", "errCorrectInformation", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                        logData.CaptureData("ModelState for errors", ModelState);

                    }
                    else
                    {
                        if (_JoinProvider.IsProfaneText(viewModel, base.GetCustomerId()))
                        {
                            logData.RecordStep("Profane Text Found");
                            ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject(
                                "~/Views/Join/Home.cshtml", "errRegistration", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                    }

                    if (!isError && ModelState.IsValid)
                    {
                        logData.CaptureData("Model State is Valid for Customer Id", viewModel.CustomerFamilyMasterData.CustomerData[0].CustomerId);
                        _JoinProvider.SetCustomerJoinData(viewModel, viewModel.CustomerFamilyMasterData.CustomerData[0].CustomerId);
                        if (!string.IsNullOrEmpty(viewModel.Clubcard))
                        {
                            logData.RecordStep("Clubcard Generated for the New Join Customer");
                            viewModel.IsConfirmation = true;
                            logData.RecordStep("calling utility method to format & mask Clubcard..");
                            ViewBag.Clubcard = viewModel.Clubcard;
                            logData.RecordStep("calling utility method to encrypt Clubcard..");
                            viewModel.Clubcard = CryptoUtility.EncryptTripleDES(viewModel.Clubcard);
                            AddressDetails addresses = new AddressDetails();
                            logData.RecordStep("Calling populate Address Method when valid data has been entered....");

                            if (_configurationProvider.GetStringConfigurations(DbConfigurationTypeEnum.Group_Config_Values, DbConfigurationItemNames.GroupCountryAddress).Equals("0"))
                            {
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
                                    viewModel.AddressDetails = addresses;
                                }
                            }
                        }
                        else if (!String.IsNullOrEmpty(viewModel.ErrorDB))
                        {
                            logData.RecordStep("Clubcard not generated for the New Join Customer, Duplicate Records found.");
                            ModelState.AddModelError("errSummary", HttpContext.GetLocalResourceObject(
                                "~/Views/Join/Home.cshtml", "errDBCardError", System.Globalization.CultureInfo.CurrentCulture).ToString());
                            isError = true;
                        }
                        else if (!String.IsNullOrEmpty(viewModel.ErrorPromotion))
                        {
                            logData.RecordStep("Clubcard not generated for the New Join Customer, Promotion Code not correct.");
                            ModelState.AddModelError("errPromotion", HttpContext.GetLocalResourceObject(
                                "~/Views/Join/Home.cshtml", "errPromotion", System.Globalization.CultureInfo.CurrentCulture).ToString());
                            isError = true;
                        }
                    }

                    if (viewModel.FindAddressClicked)
                    {
                        if (!string.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode))
                        {
                            AddressDetails addresses = new AddressDetails();
                            logData.RecordStep("Calling populate Address Method when Find address is already clicked....");
                            addresses = _personalDetailsProvider.PopulateAddress(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode);
                            logData.CaptureData("Find Address button clicked and Address details are ", addresses);

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
                            else
                            {
                                ModelState.AddModelError("errPostcode", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errPostcodeRequired", System.Globalization.CultureInfo.CurrentCulture).ToString());
                                isError = true;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("errPostcode", HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress1", System.Globalization.CultureInfo.CurrentCulture).ToString() + HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress2", System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                    }

                    viewModel.IsJoinPage = true;
                    MvcCaptcha.ResetCaptcha("JoinCaptcha");
                    //viewModel.noShowCaseSensistiveText = true;
                    viewModel.captchaValidationFlag = ModelState.ContainsKey("JoinCaptchaCode");
                    logData.RecordStep("Exiting Join Confirm Click.. ");
                }

                if (Command == HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "btnFindAddressResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString())
                {
                    logData.RecordStep("Entering Join- Find Address click...");
                    ModelState.Clear();
                    viewModel.FindAddressClicked = true;

                    if (String.IsNullOrEmpty(viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode))
                    {
                        logData.RecordStep("Postcode is empty.");
                        ModelState.AddModelError("errPostcode", String.Format("{0}{1}",
                            HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress1", CultureInfo.CurrentCulture).ToString(),
                            HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errSorryFindAddress2", CultureInfo.CurrentCulture).ToString()));
                        isError = true;
                    }

                    if (!isError)
                    {
                        AddressDetails addresses = new AddressDetails();
                        logData.RecordStep("Calling populate Address Method when no error....");
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
                        viewModel.AddressDetails = addresses;
                        viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode = GeneralUtility.FormatPostalCode(
                            viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode).ToUpper();
                        logData.CaptureData(string.Format("Address details for Postcode {0}", viewModel.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode), addresses);
                        ModelState.Clear();
                    }
                    if (viewModel.YearOfBirth == null)
                    {
                        viewModel.YearOfBirth = HttpContext.GetLocalResourceObject(
                            "~/Views/Shared/_PersonalDetails.cshtml", "lclSelectYear.Text", System.Globalization.CultureInfo.CurrentCulture).ToString();
                    }
                    viewModel.IsJoinPage = true;
                    logData.RecordStep("Exiting Join Find Address Click.. ");
                }
                _logger.Submit(logData);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Posting request on Join", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        [HttpGet]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            
            try
            {
                PersonalDetailsViewModel joinViewModel = _JoinProvider.GetJoinData();
                if (joinViewModel.IsJoinEnabled)
                {
                    logData.RecordStep("Join Functionality enabled..");
                    CustomerPreference preference = _personalDetailsProvider.GetCustomerPreferences(default(long));
                    if (preference == null || preference.Preference == null || preference.Preference.Count() == 0)
                    {
                        throw new Exception();
                    }
                    logData.CaptureData("Join Default Preferences", preference);
                    ContactPreferencesModel contactPreferenceOptInsmodel = new ContactPreferencesModel();

                    joinViewModel.OptIns = _customerPreferenceBC.GetOptIns(preference);
                    for (int i = 0; i < joinViewModel.OptIns.OptIns.Count; i++)
                    {
                        joinViewModel.OptIns.OptIns[i].IsOpted = false;
                    }
                    logData.CaptureData("Join Default OPT INS", joinViewModel.OptIns);
                }
                else
                {
                    if (!string.IsNullOrEmpty(joinViewModel.RedirectUrl))
                    {
                        logData.CaptureData("Join Disabled , redirecting to ..", joinViewModel.RedirectUrl);
                        return Redirect(joinViewModel.RedirectUrl.ToString());
                    }
                }
                _logger.Submit(logData);
                return View(joinViewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Get Operation in Join Page", ex,
                        new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }

        }

        [HttpGet]
        public ActionResult PrintClubcard()
        {
            LogData logData = new LogData();
            logData.RecordStep("Entering Join Confirmation - ShowPrintCC link from email....");
            AccountDetails accountDetails = new AccountDetails();
            logData.RecordStep("Retreiving clubcard from query string....");
            string Clubcard = Request.QueryString["clubcardID"].ToString();

            PdfBackgroundTemplate template = this.GetBackgroundTemplate(CurrentCulture);
            List<string> clubcardNo = new List<string>() { Clubcard };

            using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<string, PdfBackgroundTemplate>(clubcardNo, template, accountDetails))
            {
                logData.RecordStep("Generating PDF...");
                _logger.Submit(logData);
                return File(document.ToArray(), "application/pdf", Server.UrlEncode("Clubcard.pdf"));
            }
        }

        [HttpGet]
        public ActionResult PrintCC(string Clubcard)
        {
            LogData logData = new LogData();
            logData.RecordStep("Entering Join Confirmation - ShowPrintCard click...");
            try
            {
                AccountDetails accountDetails = new AccountDetails();
                logData.RecordStep("Calling utility method to decrypt clubcard....");
                Clubcard = CryptoUtility.DecryptTripleDES(Clubcard);
                if (!string.IsNullOrEmpty(Clubcard))
                {
                    PdfBackgroundTemplate template = this.GetBackgroundTemplate(CurrentCulture);
                    List<string> clubcardNo = new List<string>() { Clubcard };

                    using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<string, PdfBackgroundTemplate>(clubcardNo, template, accountDetails))
                    {
                        logData.RecordStep("Generating PDF...");
                        return File(document.ToArray(), "application/pdf", Server.UrlEncode("Clubcard.pdf"));
                    }
                }
                return View("Home");
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Get Operation in Join Page", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        private PdfBackgroundTemplate GetBackgroundTemplate(string culture)
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
                logData.CaptureData("PDF Template generating for     ", template);
                _logger.Submit(logData);
                return template;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed While generating PDF in Join confirmation", ex,
                             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });

            }
        }

    }
}