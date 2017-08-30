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

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class MyAccountDetailsController : BaseController
    {
        #region Private Fields
        private ICustomerPreferenceBC _customerPreferenceBC;
        private IPersonalDetailsBC _personalDetailsProvider;
        private IAccountBC _accountProvider;
        private IManageCardsBC _myCardsProvider;
        private IConfigurationProvider _configurationProvider;

        #endregion

        #region Constructores

        /// <summary>
        /// Default constructor
        /// </summary>
        public MyAccountDetailsController()
        {
            _customerPreferenceBC = ServiceLocator.Current.GetInstance<ICustomerPreferenceBC>();
            _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
            _myCardsProvider = ServiceLocator.Current.GetInstance<IManageCardsBC>();
            _configurationProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            _accountProvider = ServiceLocator.Current.GetInstance<IAccountBC>();
        }

        /// <summary>
        /// Constructor to instanciate the controller object
        /// </summary>
        /// <param name="_IpersonalDetailsBC">IPersonalDetailsBC</param>
        /// <param name="configurationProvider">IConfigurationProvider</param>
        public MyAccountDetailsController(ICustomerPreferenceBC customerPreferenceBC, IPersonalDetailsBC _IpersonalDetailsBC, IConfigurationProvider configurationProvider)
        {
            _customerPreferenceBC = customerPreferenceBC;
            _personalDetailsProvider = _IpersonalDetailsBC;
            _configurationProvider = configurationProvider;
        }

        #endregion

        #region Public Actions

        #region Options And Benefits

        // GET: /OptionsAndBenefits/
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult OptionsAndBenefits()
        {
            LogData _logData = new LogData();
            try
            {
               // _logData.RecordStep(string.Format("CustomerID : {0}", CustomerId));
                CustomerFamilyMasterData customerData = _personalDetailsProvider.GetCustomerDetails(CustomerId.TryParse<long>());
                CustomerPreference preference = _personalDetailsProvider.GetCustomerPreferences(CustomerId.TryParse<Int64>());
                OptionsAndBenefitsModel model = _customerPreferenceBC.GetOptionAndBenefitsModel(preference, CustomerId);

                model.CustomerEmail = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].EmailAddress : string.Empty;
                if (model.Preference == null && model.Preference.Preference.Count() == 0)
                {
                    throw new Exception();
                }
                else
                {
                    _logData.CaptureData("Options & benefits", model.Preference);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while getting Options & Benefits data on load", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            finally
            {
                _logger.Submit(_logData);  
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult OptionsAndBenefits(OptionsAndBenefitsModel model)
        {
            LogData _logData = new LogData();
            try
            {
                if (_customerPreferenceBC.ValidateCustomerPreferences(model))
                {
                    ModelState.Clear();
                    AccountDetails customerDetails = _accountProvider.GetMyAccountDetail(CustomerId.TryParse<Int64>(), CurrentCulture);
                    customerDetails.EmailAddress = string.IsNullOrEmpty(model.CustomerEmail) ? string.Empty : model.CustomerEmail;
                    _customerPreferenceBC.UpdateCustomerPreferences(model, customerDetails, CustomerId);
                    _logger.Submit(_logData);
                    model.AviosMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Premium || model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Standard) ? model.AviosClubDetails.MembershipID : "";
                    model.BaMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Premium || model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Standard) ? model.BAMilesClubDetails.MembershipID : "";
                    model.VirginMembershipIdLabel = (model.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Virgin_Atlantic) ? model.VirgnClubDetails.MembershipID : "";
                    model.IsSaved = model.IsValid = true;
                    return View(model);
                }
                else
                {
                    model.IsSaved = model.IsValid = false;
                    _logger.Submit(_logData);     
                    return View(model);
                }                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while posting Options & Benefits data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        #endregion        

        #region My Contact Preferences

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult MyContactPreferences()
        {
            LogData _logData = new LogData();
            try
            {
                long lCustID = base.CustomerId.TryParse<long>();

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
               
                
                if (preferences == null || preferences.Preference == null || preferences.Preference.Count() == 0)
                {
                    _logData.RecordStep("No preferences for the customer.");
                    throw new Exception("No preferences for the customer.");
                }  
              
                _logger.Submit(_logData);  
                return View(model);
            }
            catch(Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while getting Contact Preferences data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
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
            LogData _logData = new LogData();
            AccountDetails customerdetails = new AccountDetails();
            try
            {   
                long customerId = CustomerId.TryParse<Int64>();
                if (_customerPreferenceBC.ValidateContactPreferences(model.ContactPreference))
                {
                    model.ContactPreference.IsValid = true;
                    bool isUpdateProfileRequired = false;
                    CustomerFamilyMasterData _orignalCustomerData = _personalDetailsProvider.GetCustomerDetails(customerId);
                    CustomerFamilyMasterDataUpdate _customerData = _customerPreferenceBC.GetCustomerUpdateModel(_orignalCustomerData, model.ContactPreference, out isUpdateProfileRequired);
                    
                    customerdetails = _accountProvider.GetMyAccountDetail(customerId, CurrentCulture);
                    if (isUpdateProfileRequired)
                    {
                        if (!_accountProvider.IsAccountDuplicate(_customerData))
                        {
                            model.ContactPreference.TrackEmail = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "TrackEmail");
                            model.ContactPreference.TrackMobile = HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "TrackMobileNumber");
                            model.ContactPreference.ROEmail = (_orignalCustomerData != null && _orignalCustomerData.CustomerData != null && _orignalCustomerData.CustomerData.Count > 0) ? _orignalCustomerData.CustomerData[0].EmailAddress : string.Empty;
                            _customerPreferenceBC.UpdateContactPreferences(model.ContactPreference, customerdetails);
                            _personalDetailsProvider.UpdateCustomerDetails(_customerData);                            
                            _customerPreferenceBC.SendEmailToCustomer(customerId, model.ContactPreference, customerdetails, HttpContext.GetLocalResource("~/Views/Shared/EditorTemplates/ContactModel.cshtml", "PageName"));
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

                    
                }
                else
                {
                    model.ContactPreference.IsSaved = model.ContactPreference.IsValid = false;
                }
               

                _logger.Submit(_logData);
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while posting Contact Preferences data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
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
                AccountDetails customerdetails = _accountProvider.GetMyAccountDetail(CustomerId.TryParse<Int64>(), CurrentCulture);
                _customerPreferenceBC.UpdateOptIns(model.OptIns, customerdetails);
                model.ContactPreference.IsValid = model.OptIns.IsSaved = true;
                model.ContactPreference = _customerPreferenceBC.RetainContactPrefValues(model.ContactPreference);
                _logger.Submit(_logData);   
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while posting Optins data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        #endregion

        #region View My Cards
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult ViewMyCards()
        {
            LogData _logData = new LogData();
            ManageCardsViewModel model = new ManageCardsViewModel();
         
            try
            {
                List<HouseholdCustomerDetails> customersList = _accountProvider.GetHouseHoldCustomersData(CustomerId.TryParse<long>(), CurrentCulture);
             
                CustomerDisplayName customerName = new CustomerDisplayName();
                GeneralUtility name = new GeneralUtility();
                foreach (var customer in customersList)
                {
                    customerName.TitleEnglish=string.IsNullOrEmpty(customer.TitleEnglish) ? string.Empty : customer.TitleEnglish;
                    customerName.Name1=string.IsNullOrEmpty(customer.Name1) ? string.Empty : customer.Name1;
                    customerName.Name2=string.IsNullOrEmpty(customer.Name2) ? string.Empty : customer.Name2;
                    customerName.Name3=string.IsNullOrEmpty(customer.Name3) ? string.Empty : customer.Name3;
                    customer.FullName = name.GetCustomerDisplayName(customerName, "VIEWMYCARDS");
                }
                
                string welcomeMessageNames = string.Empty;
                string saperator = HttpContext.GetLocalResource("~/Views/MyAccountDetails/ViewMyCards.cshtml", "AndSeprator");
                customersList.ForEach(c => welcomeMessageNames += string.Format("{0} {1} ", c.FullName, saperator));
                welcomeMessageNames = welcomeMessageNames.Substring(0, welcomeMessageNames.Length - (saperator.Length + 1));
                ViewBag.WelcomeMessageNames = welcomeMessageNames;
                foreach (HouseholdCustomerDetails cust in customersList)
                {
                    cust.Cards = _accountProvider.GetClubcardsCustomerData(cust.CustomerID, CurrentCulture);
                }
                Dictionary<string, string> resources = new Dictionary<string, string>();
                resources.Add("lclMore", HttpContext.GetLocalResource("~/Views/MyAccountDetails/ViewMyCards.cshtml", "lclMore"));
                model = _myCardsProvider.GetManageCardsModel(customersList, resources);
                _logger.Submit(_logData);   
                return View(model);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MyAccountDetailsController while getting View my Cards data", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }
        #endregion

        #endregion
    }
}
