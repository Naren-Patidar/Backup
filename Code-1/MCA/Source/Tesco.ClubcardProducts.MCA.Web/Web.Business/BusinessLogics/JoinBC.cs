using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using System.Data;
using PreferenceServices = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class JoinBC : IJoinBC
    {
        private IServiceAdapter _customerServiceAdapter;
        private IServiceAdapter _preferenceServiceAdapter;
        private IServiceAdapter _utilityServiceAdapter;
        private IServiceAdapter _joinloyaltyServiceAdapter;
        private IServiceAdapter _clubcardServiceAdapter;
        private IServiceAdapter _locatorServiceAdapter;

        MCARequest request;
        MCAResponse response;

        private readonly ILoggingService _logger;

        CultureInfo cultureInfo;
        string culture = String.Empty;
        string maxRows = String.Empty;
        bool isLegalPolicyEnabled = false;
        DBConfigurations config = null;
        IConfigurationProvider _configProvider = null;

        String StrMonth = String.Empty;
        String StrYear = String.Empty;
        String StrDay = String.Empty;

        private short[] MCAPreferences
        {
            get
            {
                return new short[] {(short)PreferenceEnum.Tesco_Products,
                                    (short)PreferenceEnum.Tesco_Partners,
                                    (short)PreferenceEnum.Customer_Research,
                                    (short)PreferenceEnum.Tesco_Group_Mail,
                                    (short)PreferenceEnum.Partner_3rd_Party_Mail,
                                    (short)PreferenceEnum.Research_Mail};
            }
        }

        #region Constructor

        public JoinBC(IServiceAdapter customerServiceAdapter,
                                 IServiceAdapter preferenceServiceAdapter,
                                 IServiceAdapter utilityServiceAdapter,
                                 IServiceAdapter joinLoyaltyServiceAdapter,
                                 IServiceAdapter clubcardServiceAdapter,
                                 IServiceAdapter locatorServiceAdapter,
                                 ILoggingService logger,
                                 IConfigurationProvider configProvider)
        {
            _customerServiceAdapter = customerServiceAdapter;
            _preferenceServiceAdapter = preferenceServiceAdapter;
            _utilityServiceAdapter = utilityServiceAdapter;
            _joinloyaltyServiceAdapter = joinLoyaltyServiceAdapter;
            _clubcardServiceAdapter = clubcardServiceAdapter;
            _locatorServiceAdapter = locatorServiceAdapter;

            _logger = logger;
            _configProvider = configProvider;
            this.culture = _configProvider.GetStringAppSetting(AppConfigEnum.Culture);
            this.maxRows = _configProvider.GetStringAppSetting(AppConfigEnum.MaxRows);
            //isLegalPolicyEnabled=_configProvider.GetConfigurations(
            config = _configProvider.GetConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality);
            // config.GetConfigurationItem(DbConfigurationItemNames.TrackFirstName).ConfigurationValue

        }

        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// Check if Profanity Enabled 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public bool IsProfaneText(PersonalDetailsViewModel viewModel, long customerID)
        {
            Hashtable hashCustomerDetails = new Hashtable();
            bool blnProfane = false;
            LogData logData = new LogData();
            try
            {
                hashCustomerDetails = GetHashTableFromViewModel(viewModel, customerID);
                if (_configProvider.GetBoolAppSetting(AppConfigEnum.Profanity))
                {
                    if (!IsProfaneText(PrepareProfaneText(hashCustomerDetails)))
                    {
                        logData.RecordStep("Profanity Check is true");
                        blnProfane = true;
                    }
                }
                _logger.Submit(logData);
                return blnProfane;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join BC while checking Profanity", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// For Updating cutomer details
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public void SetCustomerJoinData(PersonalDetailsViewModel viewModel, long customerID)
        {
            Hashtable htCustomerDetails = new Hashtable();
            LogData logData = new LogData();
            try
            {
                logData.RecordStep("Validating Account Duplicacy..");
                htCustomerDetails = this.GetHashTableFromViewModel(viewModel, customerID);
                AccountDuplicacyStatusEnum adStatus = this.ValidateAccountDuplicacy(viewModel, htCustomerDetails);
                logData.CaptureData("Status returned for Duplicate check is ", adStatus);
                switch (adStatus)
                {
                    case AccountDuplicacyStatusEnum.PromoCodeAlreadyExist:
                        {
                            viewModel.ErrorPromotion = HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errPromotion", CultureInfo.CurrentCulture).ToString(); ;
                            break;
                        }
                    case AccountDuplicacyStatusEnum.IsMainAndAlternateIdUnique:
                        {
                            string code = DBConfigurationManager.Instance[DbConfigurationTypeEnum.Join_Route_Code][DbConfigurationItemNames.JoinRouteCode]
                                                                .ConfigurationValue1.ToString();

                            logData.CaptureData("Updating Join data with Join Route Code value ", code);
                            long clubcard = this.CreateClubcardAccount(customerID.TryParse<Int64>(),
                                htCustomerDetails, code, CultureInfo.CurrentCulture.ToString());

                            viewModel.Clubcard = clubcard.ToString();

                            break;
                        }
                    case AccountDuplicacyStatusEnum.None:
                        {
                            viewModel.ErrorDB = HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "errDBCardError", CultureInfo.CurrentCulture).ToString();
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join BC while executing Context Data", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// Check if Account and Promotional Code is Valid
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="customerData"></param>
        /// <returns></returns>
        AccountDuplicacyStatusEnum ValidateAccountDuplicacy(PersonalDetailsViewModel viewModel, Hashtable customerData)
        {
            AccountContext context = new AccountContext();
            AccountDuplicacyStatusEnum status = AccountDuplicacyStatusEnum.None;
            LogData logData = new LogData();
            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.USER_DATA, customerData);
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_ACCOUNT_CONTEXT);

                response = _joinloyaltyServiceAdapter.Get<AccountContext>(request);
                logData.CaptureData("Getting Context for Acount Duplicacy Check", response);
                context = (AccountContext)response.Data;
                if (context != null)
                {
                    if (context.PromotionCodeExist && !string.IsNullOrEmpty(viewModel.PromotionCode))
                    {
                        status = AccountDuplicacyStatusEnum.PromoCodeAlreadyExist;
                    }
                    else if (context.IsMainAccountUnique && context.IsAlternateAccountUnique)
                    {
                        status = AccountDuplicacyStatusEnum.IsMainAndAlternateIdUnique;
                    }
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Validating Account Duplicate Check in Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return status;
        }

        /// <summary>
        /// Get Preferences in Join Page
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public PersonalDetailsViewModel GetJoinData()
        {
            MCAResponse response = new MCAResponse();
            PersonalDetailsViewModel personalDetails = new PersonalDetailsViewModel();
            List<BTClubPreference> btClubList = new List<BTClubPreference>();
            LogData logData = new LogData();
            try
            {
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.HideJoinFunctionality, DbConfigurationItemNames.HideJoinFunctionality.ToString()) == "1")
                {
                    personalDetails.IsJoinEnabled = false;
                    personalDetails.RedirectUrl = _configProvider.GetStringAppSetting(AppConfigEnum.HideJoinFunctionality);
                    logData.RecordStep(string.Format("Join Functionality disabled.Redirecting to Login Url:{0}", personalDetails.RedirectUrl));
                }
                else
                {
                    personalDetails.IsJoinEnabled = true;
                    logData.RecordStep("Join Functionality Enabled. Getting Default Preferences..");
                    CustomerPreference custPreference = GetCustomerPreferences(default(long));
                    if (custPreference != null && custPreference.Preference != null && custPreference.Preference.Length > 0)
                    {
                        foreach (var pref in custPreference.Preference)
                        {
                            if ((pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_ALLERGIC || pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_DIETRY))
                            {
                                btClubList.Add(new BTClubPreference()
                                {
                                    PreferenceID = pref.PreferenceID,
                                    PreferenceType = pref.CustomerPreferenceType,
                                    PreferenceDescLocal = pref.PreferenceDescriptionLocal,
                                    PreferenceDescEnglish = pref.PreferenceDescriptionEng,
                                    OptedStatus = false
                                });
                            }
                        }
                    }
                    personalDetails.PersonalDetailsPreferences = btClubList;
                    logData.CaptureData("Default Preferences for Join page ", btClubList);
                    personalDetails.IsJoinPage = true;
                }
                _logger.Submit(logData);
                return personalDetails;

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Getting Join Data in Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }


        public bool IsLegalPolicyEnabled()
        {
            LogData _logData = new LogData();
            string isLegalPolicyEnabled = config.GetConfigurationItem(DbConfigurationItemNames.HideLegalPolicy).ConfigurationValue1.ToString();

            _logData.RecordStep(string.Format("isLegalPolicyEnabled :{0}", isLegalPolicyEnabled));
            if (string.IsNullOrEmpty(isLegalPolicyEnabled))
            {
                _logData.RecordStep(string.Format("isLegalPolicyEnabled is null or empty"));
                _logger.Submit(_logData);
                return false;
            }
            _logger.Submit(_logData);
            return isLegalPolicyEnabled != "1";
        }
        #endregion Public Methods

        #region Private Methods


        /// <summary>
        /// Gets an object for Join form data
        /// </summary>
        /// <param name="configurationItems"></param>
        /// <returns></returns>
        private string PrepareProfaneText(Hashtable hashCustomerData)
        {

            ProfaneTextProvider profaneTextProvider = new EmptyProfaneTextContributor();
            LogData logData = new LogData();
            List<String> strBlackList = new List<String>();
            try
            {

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name1.ToString()) == "1")
                {
                    if (hashCustomerData["Name1"] != null && !string.IsNullOrEmpty(hashCustomerData["Name1"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name1"].ToString());
                    }
                }

                if (hashCustomerData["Name2"] != null && _configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name2.ToString()) == "1")
                {
                    if (!string.IsNullOrEmpty(hashCustomerData["Name2"].ToString()))
                    {
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name2"].ToString());
                    }
                }

                if (hashCustomerData["Name3"] != null && _configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name3.ToString()) == "1")
                {
                    if (!string.IsNullOrEmpty(hashCustomerData["Name3"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name3"].ToString());
                    }
                }

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressPostCode.ToString()) == "1")
                {
                    if (hashCustomerData["MailingAddressPostCode"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressPostCode"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressPostCode"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressPostCode"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine1.ToString()) == "1")
                {
                    if (hashCustomerData["MailingAddressLine1"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine1"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine1"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine1"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine2.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine2"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine2"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine2"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine2"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine3.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine3"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine3"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine3"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine3"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine4.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine4"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine4"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine4"].ToString());
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine5.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine5"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine5"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine5"].ToString());
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.DaytimePhoneNumber.ToString()) == "1")
                {
                    if (hashCustomerData["DaytimePhoneNumber"] != null && !string.IsNullOrEmpty(hashCustomerData["DaytimePhoneNumber"].ToString()))
                    {
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["DaytimePhoneNumber"].ToString());
                        strBlackList.Add(hashCustomerData["DaytimePhoneNumber"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MobilePhoneNumber.ToString()) == "1")
                {
                    if (hashCustomerData["MobilePhoneNumber"] != null && !string.IsNullOrEmpty(hashCustomerData["MobilePhoneNumber"].ToString()))
                    {
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["MobilePhoneNumber"].ToString());
                        strBlackList.Add(hashCustomerData["MobilePhoneNumber"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.EmailAddress.ToString()) == "1")
                {
                    if (hashCustomerData["EmailAddress"] != null && !string.IsNullOrEmpty(hashCustomerData["EmailAddress"].ToString()))
                    {
                        profaneTextProvider = new EmailProfaneTextContributor(profaneTextProvider, hashCustomerData["EmailAddress"].ToString());
                        strBlackList.Add(hashCustomerData["EmailAddress"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.PrimaryId.ToString()) == "1")
                {
                    if (hashCustomerData["SSN"] != null && !string.IsNullOrEmpty(hashCustomerData["SSN"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["SSN"].ToString());
                        strBlackList.Add(hashCustomerData["SSN"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.SecondaryId.ToString()) == "1")
                {
                    if (hashCustomerData["PassportNumber"] != null && !string.IsNullOrEmpty(hashCustomerData["PassportNumber"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["PassportNumber"].ToString());
                        strBlackList.Add(hashCustomerData["PassportNumber"].ToString());
                    }
                }
                logData.BlackLists = strBlackList;
                logData.CaptureData("Profane Text", profaneTextProvider);
                _logger.Submit(logData);
                return profaneTextProvider.ProfaneText.Trim().ToUpper();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Prepare Profane Text of Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsProfaneText(string text)
        {
            bool isProfaneText = false;
            LogData logData = new LogData();
            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.PROFANE_TEXT, text);

                response = _utilityServiceAdapter.Get<bool>(request);
                logData.CaptureData("Profane text response is ", response);
                if (response.Status)
                {
                    isProfaneText = (bool)response.Data;
                }
                _logger.Submit(logData);
                return isProfaneText;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join BC while getting Profanity response..", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }


        }


        /// <summary>
        /// Convert Personal Details View Model to HashTable
        /// </summary>
        /// <param name="personalDetailsViewModel"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        private Hashtable GetHashTableFromViewModel(PersonalDetailsViewModel personalDetailsViewModel, long customerID)
        {
            CustomerMasterData customerData = new CustomerMasterData();
            AddressDetails address = new AddressDetails();

            int noHHPersons = 0;
            bool islblStRequired = true;
            bool blnAddressLine2 = true;
            bool blnAddressLine3 = true;
            bool blnAddressLine4 = true;
            LogData logData = new LogData();
            List<string> blacklistData = new List<string>();
            try
            {
                customerData = personalDetailsViewModel.CustomerFamilyMasterData.CustomerData[0];
                address = personalDetailsViewModel.AddressDetails;
                Hashtable htCustomer = new Hashtable();
                if (personalDetailsViewModel.AddressDetails != null)
                {
                    if (!string.IsNullOrEmpty(personalDetailsViewModel.AddressDetails.HideAddressformat))
                    {
                        char[] chars = { ':' };
                        ArrayList arr = new ArrayList();
                        arr.AddRange(personalDetailsViewModel.AddressDetails.HideAddressformat.Split(chars));
                        personalDetailsViewModel.AddressDetails.HideAddressList = arr;
                    }
                    if (address.Houseno == null)
                        address.Houseno = string.Empty;
                    if (address.AddressChosen == null)
                        address.AddressChosen = string.Empty;
                    if (address.Street == null)
                        address.Street = string.Empty;
                    if (address.Country == null)
                        address.Country = string.Empty;
                    if (address.City == null)
                        address.City = string.Empty;
                    if (address.Locality == null)
                        address.Locality = string.Empty;
                }
                if (!personalDetailsViewModel.FindAddressClicked)
                {

                    htCustomer["MailingAddressLine1"] = !string.IsNullOrEmpty(customerData.MailingAddressLine1) ? customerData.MailingAddressLine1.Trim().ToUpper()
                                                                                                                : String.Empty;
                    htCustomer["MailingAddressLine2"] = !string.IsNullOrEmpty(customerData.MailingAddressLine2) ? customerData.MailingAddressLine2.Trim().ToUpper()
                                                                                                                : String.Empty;
                    htCustomer["MailingAddressLine3"] = !string.IsNullOrEmpty(customerData.MailingAddressLine3) ? customerData.MailingAddressLine3.Trim().ToUpper()
                                                                                                                : String.Empty;
                    htCustomer["MailingAddressLine4"] = !string.IsNullOrEmpty(customerData.MailingAddressLine4) ? customerData.MailingAddressLine4.Trim().ToUpper()
                                                                                                                : String.Empty;
                    htCustomer["MailingAddressLine5"] = !string.IsNullOrEmpty(customerData.MailingAddressLine5) ? customerData.MailingAddressLine5.Trim().ToUpper()
                                                                                                                : String.Empty;
                    htCustomer["MailingAddressLine6"] = !string.IsNullOrEmpty(customerData.MailingAddressLine6) ? customerData.MailingAddressLine6.Trim().ToUpper()
                                                                                                                 : String.Empty;

                }
                else
                {
                    if (!string.IsNullOrEmpty(address.AddressChosen))
                    {
                        string[] strSplitAddress = address.HideAddressList[address.AddressChosen.TryParse<Int32>()].ToString().Split(',');
                        if (strSplitAddress[0].ToString().Trim().Length <= 4)
                        {
                            htCustomer["MailingAddressLine1"] = strSplitAddress[0] + " " + strSplitAddress[1];
                            islblStRequired = false;
                        }
                        else
                        {
                            htCustomer["MailingAddressLine1"] = strSplitAddress[0].ToString();
                            if (strSplitAddress[1].ToString().Trim().ToUpper() == address.Street.Trim().ToUpper())
                            {
                                islblStRequired = true;
                            }
                            else
                            {
                                islblStRequired = true;
                                address.Street = strSplitAddress[1].ToString().Trim().ToUpper();
                            }
                        }
                    }
                    else
                    {
                        htCustomer["MailingAddressLine1"] = address.Houseno.Trim().ToUpper();
                        string[] arrstrStreet = null;
                        string strStreet = string.Empty;

                        string[] arrStreet = address.Houseno.Trim().Split(' ').ToArray();

                        if (arrStreet.Length > 1)
                        {
                            arrstrStreet = new string[arrStreet.Length];
                            for (int i = 1; i < arrStreet.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(address.Houseno.Trim().Split(' ')[i]))
                                {
                                    arrstrStreet[i] = address.Houseno.Trim().Split(' ')[i].ToString().Trim().ToUpper();
                                }
                            }
                            for (int j = 1; j < arrstrStreet.Length; j++)
                            {
                                if (arrstrStreet[j] != null)
                                {
                                    strStreet = strStreet + " " + arrstrStreet[j].ToString();
                                }
                            }

                            if (strStreet.Trim() == address.Street.Trim().ToUpper())
                            {
                                islblStRequired = false;
                            }
                        }

                    }
                    if (islblStRequired)
                    {
                        if (!string.IsNullOrEmpty(address.Street))
                        {
                            htCustomer["MailingAddressLine2"] = address.Street;
                        }
                        else
                        {
                            blnAddressLine2 = false;
                        }
                    }
                    else
                    {
                        blnAddressLine2 = false;
                        address.Street = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(address.Locality))
                    {
                        htCustomer["MailingAddressLine3"] = address.Locality.Trim().ToUpper();
                    }
                    else
                    {
                        blnAddressLine3 = false;
                    }
                    if (!string.IsNullOrEmpty(address.City))
                    {
                        htCustomer["MailingAddressLine4"] = address.City.Trim().ToUpper();
                    }
                    else
                    {
                        blnAddressLine4 = false;
                    }

                    if (!string.IsNullOrEmpty(address.Country))
                    {
                        htCustomer["MailingAddressLine5"] = address.Country.Trim().ToUpper();
                    }

                    if (!blnAddressLine2)
                    {
                        htCustomer["MailingAddressLine2"] = blnAddressLine3 ? address.Locality.Trim().ToUpper() : address.City.Trim().ToUpper();
                        htCustomer["MailingAddressLine3"] = blnAddressLine3 ? address.City.Trim().ToUpper() : string.Empty;
                        htCustomer["MailingAddressLine4"] = string.Empty;
                    }
                    else if (!blnAddressLine3)
                    {
                        htCustomer["MailingAddressLine3"] = blnAddressLine4 ? address.City.Trim().ToUpper() : address.Country.Trim().ToUpper();
                        htCustomer["MailingAddressLine4"] = blnAddressLine4 ? address.Country.Trim().ToUpper() : string.Empty;
                        htCustomer["MailingAddressLine5"] = string.Empty;
                    }
                }
                if (Convert.ToBoolean(personalDetailsViewModel.GroupAddressAvailable))
                {
                    htCustomer["MailingAddressLine1"] = customerData.MailingAddressLine1.Trim().ToUpper();
                    htCustomer["MailingAddressLine2"] = customerData.MailingAddressLine2.Trim().ToUpper();
                    htCustomer["MailingAddressLine3"] = customerData.MailingAddressLine3.Trim().ToUpper();
                    htCustomer["MailingAddressLine4"] = customerData.MailingAddressLine4.Trim().ToUpper();
                    if (Convert.ToBoolean(_configProvider.GetBoolAppSetting(AppConfigEnum.EnableProvince)))
                    {
                        htCustomer["MailingAddressLine5"] = "-1";
                    }
                    else
                    {
                        htCustomer["MailingAddressLine5"] = customerData.MailingAddressLine5.Trim().ToUpper();
                    }
                    htCustomer["MailingAddressLine6"] = customerData.MailingAddressLine6.Trim().ToUpper();
                    htCustomer["MailingAddressPostCode"] = DBConfigurationManager.Instance[DbConfigurationTypeEnum.ChinaHiddenFunctionality][DbConfigurationItemNames.HidePostCode].ConfigurationValue1 != "0" ? customerData.MailingAddressPostCode.Trim() : string.Empty;
                }
                string lbladdress = string.Empty;
                for (int i = 1; i <= 7; i++)
                {
                    if (htCustomer["MailingAddressLine" + i] != null)
                    {
                        lbladdress += htCustomer["MailingAddressLine" + i].ToString().Trim();
                    }
                }
                personalDetailsViewModel.hdnTrackAddress = lbladdress.Trim();
                htCustomer["CustomerID"] = "0";

                htCustomer["TitleEnglish"] = !string.IsNullOrEmpty(customerData.Title) ? customerData.Title.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;

                htCustomer["Name1"] = !string.IsNullOrEmpty(customerData.FirstName) ? customerData.FirstName.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;
                htCustomer["Name2"] = !string.IsNullOrEmpty(customerData.Initial) ? customerData.Initial.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;
                htCustomer["Name3"] = !string.IsNullOrEmpty(customerData.LastName) ? customerData.LastName.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;

                personalDetailsViewModel.HiddenMessage = _configProvider.GetStringAppSetting(AppConfigEnum.IsReplacementCardWithYourNewName).ToString().Equals("1") ? "true" : "false";


                StrMonth = HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "lclSelectMonth.Text", System.Globalization.CultureInfo.CurrentCulture).ToString();
                StrYear = HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "lclSelectYear.Text", System.Globalization.CultureInfo.CurrentCulture).ToString();
                StrDay = HttpContext.GetLocalResourceObject("~/Views/Join/Home.cshtml", "lclSelectDay.Text", System.Globalization.CultureInfo.CurrentCulture).ToString();


                if (((String.IsNullOrEmpty(personalDetailsViewModel.DayOfBirth) || personalDetailsViewModel.DayOfBirth == StrDay) &&
                    (String.IsNullOrEmpty(personalDetailsViewModel.MonthOfBirth) || personalDetailsViewModel.MonthOfBirth == StrMonth)
                 && (String.IsNullOrEmpty(personalDetailsViewModel.YearOfBirth) || personalDetailsViewModel.YearOfBirth == StrYear)))
                {
                    //DOB is empty then don't update as it not required field. Fix of Defect MKTG00003574
                }
                else
                {
                    String TargetDateformat = _configProvider.GetStringAppSetting(AppConfigEnum.SpecifiedDateFormat);
                    string TargetLinker = _configProvider.GetStringAppSetting(AppConfigEnum.Linkerfordate);

                    if (TargetDateformat == "dd" + TargetLinker + "MM" + TargetLinker + "yyyy" || TargetDateformat == "d/MM/yyyy" || TargetDateformat == "dd/M/yyyy" || TargetDateformat == "d/M/yyyy" || TargetDateformat == "dd/MM/yy")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.YearOfBirth;
                    }
                    else if (TargetDateformat == "MM/dd/yyyy" || TargetDateformat == "M/dd/yyyy" || TargetDateformat == "MM/d/yyyy" || TargetDateformat == "M/d/yyyy")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.YearOfBirth;
                    }

                    else if (TargetDateformat == "yyyy/dd/MM" || TargetDateformat == "yyyy/d/MM" || TargetDateformat == "yyyy/dd/M" || TargetDateformat == "yyyy/d/M")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.YearOfBirth + "/" + personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.MonthOfBirth;
                    }
                    else if (TargetDateformat == "yyyy/MM/dd" || TargetDateformat == "yyyy/M/dd" || TargetDateformat == "yyyy/MM/d" || TargetDateformat == "yyyy/M/d")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.YearOfBirth + "/" + personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.DayOfBirth;
                    }
                }

                // htCustomer["Sex"] = customerData.Sex;
                htCustomer["Sex"] = !string.IsNullOrEmpty(customerData.Sex) ? customerData.Sex.Trim() : String.Empty;

                htCustomer["PromotionCode"] = !string.IsNullOrEmpty(personalDetailsViewModel.PromotionCode) ? personalDetailsViewModel.PromotionCode.Trim() : string.Empty;


                if (!string.IsNullOrEmpty(customerData.MailingAddressPostCode))
                {
                    blacklistData.Add(customerData.MailingAddressPostCode.Trim());
                }
                htCustomer["MailingAddressPostCode"] = !string.IsNullOrEmpty(customerData.MailingAddressPostCode) ? customerData.MailingAddressPostCode.Trim() : String.Empty;

                htCustomer["DaytimePhoneNumber"] = !string.IsNullOrEmpty(customerData.DayTimePhonenumber) ? customerData.DayTimePhonenumber.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.EmailAddress))
                {
                    blacklistData.Add(customerData.EmailAddress.Trim());
                }

                htCustomer["EmailAddress"] = !string.IsNullOrEmpty(customerData.EmailAddress) ? customerData.EmailAddress.Trim() : String.Empty;

                if (!string.IsNullOrEmpty(customerData.EveningPhonenumber))
                {
                    blacklistData.Add(customerData.EveningPhonenumber.Trim());
                }

                htCustomer["EveningPhoneNumber"] = !string.IsNullOrEmpty(customerData.EveningPhonenumber) ? customerData.EveningPhonenumber.Trim() : String.Empty;

                if (!string.IsNullOrEmpty(customerData.MobileNumber))
                {
                    blacklistData.Add(customerData.MobileNumber.Trim());
                }

                htCustomer["MobilePhoneNumber"] = !string.IsNullOrEmpty(customerData.MobileNumber) ? customerData.MobileNumber.Trim() : String.Empty;

                if (!string.IsNullOrEmpty(customerData.SSN))
                {
                    htCustomer["Expat"] = "N";
                    blacklistData.Add(customerData.SSN.Trim());

                }

                htCustomer["SSN"] = !string.IsNullOrEmpty(customerData.SSN) ? customerData.SSN.Trim() : String.Empty;


                if (!string.IsNullOrEmpty(customerData.PassportNo))
                {
                    htCustomer["Expat"] = "Y";
                    blacklistData.Add(customerData.PassportNo.Trim());
                }

                htCustomer["PassportNumber"] = !string.IsNullOrEmpty(customerData.PassportNo) ? customerData.PassportNo.Trim() : String.Empty;
                htCustomer["Language"] = !string.IsNullOrEmpty(customerData.ISOLanguageCode) ? customerData.ISOLanguageCode.Trim() : "0";
                htCustomer["Race"] = !string.IsNullOrEmpty(customerData.RaceID) ? customerData.RaceID.Trim() : "0";

                htCustomer["JoinStore"] = "0";
                if (personalDetailsViewModel.FamilyMemberDob != null && personalDetailsViewModel.FamilyMemberDob.Count > 0)
                {
                    int i = 0;
                    List<string> lstFamilyMembers = personalDetailsViewModel.FamilyMemberDob;
                    foreach (string fmd in lstFamilyMembers)
                    {
                        if (!String.IsNullOrEmpty(fmd) && fmd.ToLower() != "year")
                        {
                            i++;
                            DateTime Dob = DateTime.Parse((fmd.ToString() + "/1/1"));
                            htCustomer["family_member_" + i + "_dob"] = GeneralUtility.GetAge(Dob);
                        }
                    }

                    noHHPersons = i + 1;
                    htCustomer["number_of_household_members"] = Convert.ToInt16(noHHPersons);
                }
                else
                    htCustomer["number_of_household_members"] = 0;

                string dynamicPref = GetDietaryPreferences(personalDetailsViewModel.PersonalDetailsPreferences);
                htCustomer["DynamicPreferences"] = dynamicPref;

                htCustomer["IsDiabetic"] = dynamicPref.Contains("1") ? "Y" : "N";
                htCustomer["IsKosher"] = dynamicPref.Contains("2") ? "Y" : "N";
                htCustomer["IsHalal"] = dynamicPref.Contains("3") ? "Y" : "N";
                htCustomer["IsVegetarian"] = dynamicPref.Contains("4") ? "Y" : "N";
                htCustomer["IsTeeTotal"] = dynamicPref.Contains("5") ? "Y" : "N";
                htCustomer["IsCoeliac"] = dynamicPref.Contains("24") ? "Y" : "N";

                bool isOptInBehaviour = _configProvider.GetBoolAppSetting(AppConfigEnum.IsOptInBehaviour);
                List<PreferencesUIConfig> PrefUIConfiguration = new List<PreferencesUIConfig>();
                DbConfigurationItem item = _configProvider.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.PreferenceUIConfiguration);

                if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1))
                {
                    PrefUIConfiguration.AddRange(item.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
                }

                foreach (var p in this.MCAPreferences)
                {
                    OptIns option = isOptInBehaviour ? personalDetailsViewModel.OptIns.OptOuts.Find(o => o.PreferenceID == p) : personalDetailsViewModel.OptIns.OptIns.Find(o => o.PreferenceID == p);
                    if (option != null && PrefUIConfiguration.Any(uipref => uipref.isvisible && uipref.preferenceid == option.PreferenceID))
                    {
                        var parentPref = this.GetMarketingPreferenceDetails(option.PreferenceID);
                        if (isOptInBehaviour)
                        {
                            htCustomer.Add(parentPref, option.IsOpted ? "Y" : "N");
                        }
                        else
                        {
                            htCustomer.Add(parentPref, option.IsOpted ? "N" : "Y");
                        }
                        var uiPrefCfg = PrefUIConfiguration.Where<PreferencesUIConfig>(uipref => uipref.preferenceid == option.PreferenceID).FirstOrDefault();
                        if (uiPrefCfg.dependentprefidsassame != null && uiPrefCfg.dependentprefidsassame.Count > 0)
                        {
                            uiPrefCfg.dependentprefidsassame.ForEach(uip =>
                            {
                                htCustomer.Add(GetMarketingPreferenceDetails(uip), htCustomer[parentPref]);
                            });
                        }

                        if (uiPrefCfg.dependentprefidsasopp != null && uiPrefCfg.dependentprefidsasopp.Count > 0)
                        {
                            uiPrefCfg.dependentprefidsasopp.ForEach(uip =>
                            {
                                htCustomer.Add(GetMarketingPreferenceDetails(uip), htCustomer[parentPref] == "Y" ? "N" : "Y");
                            });
                        }
                    }
                }
                htCustomer["Culture"] = System.Globalization.CultureInfo.CurrentCulture.Name;

                htCustomer["CustomerMobilePhoneStatus"] = !string.IsNullOrEmpty(customerData.MobileNumber) ? (int)CustomerMailStatusEnum.Deliverable : (int)CustomerMailStatusEnum.Missing;

                htCustomer["CustomerEmailStatus"] = !string.IsNullOrEmpty(customerData.EmailAddress) ? (int)CustomerMailStatusEnum.Deliverable : (int)CustomerMailStatusEnum.Missing;
                if (DBConfigurationManager.Instance[DbConfigurationTypeEnum.ChinaHiddenFunctionality][DbConfigurationItemNames.HidePostCode].ConfigurationValue1 != "0")
                {

                    if (!string.IsNullOrEmpty(htCustomer["MailingAddressPostCode"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine1"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine2"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine3"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine4"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine5"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine6"].ToString().Trim()))
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Deliverable;
                    }
                    else
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Missing;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(htCustomer["MailingAddressLine1"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine2"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine3"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine4"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine5"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine6"].ToString().Trim()))
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Deliverable;
                    }
                    else
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Missing;
                    }
                }
                htCustomer["CustomerUseStatusMain"] = BusinessConstants.CUSTOMERUSESTATUS_ACTIVE;
                logData.BlackLists = blacklistData;
                logData.CaptureData("HashTable for Join", htCustomer);
                _logger.Submit(logData);
                return htCustomer;

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception thrown while reading the HashTable from ViewModel", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// Get Customer Selection for Dietary Preferences
        /// </summary>
        /// <param name="preferences"></param>
        /// <returns></returns>
        private string GetDietaryPreferences(List<BTClubPreference> preferences)
        {
            StringBuilder dynamicPreferences = new StringBuilder(String.Empty);
            LogData logData = new LogData();
            try
            {
                if (preferences != null && preferences.Count > 0)
                {
                    foreach (BTClubPreference btc in preferences)
                    {
                        if (btc.OptedStatus)
                        {
                            dynamicPreferences.Append(btc.PreferenceID).Append(",");
                        }
                    }
                }
                _logger.Submit(logData);
                return dynamicPreferences.ToString();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while Getting Dietary preference in Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        ///     
        /// Preferences
        /// 

        private string GetMarketingPreferenceDetails(short preferenceId)
        {
            string prefName = string.Empty;
            switch (preferenceId)
            {
                case 7:
                    prefName = "WantTescoInfo";
                    break;
                case 8:
                    prefName = "WantPartnerInfo";
                    break;
                case 9:
                    prefName = "IsResearchContactable";
                    break;
                case 27:
                    prefName = "TescoGroupMail";
                    break;
                case 28:
                    prefName = "TescoGroupEmail";
                    break;
                case 29:
                    prefName = "TescoGroupPhone";
                    break;
                case 30:
                    prefName = "TescoGroupSMS";
                    break;
                case 31:
                    prefName = "PartnerMail";
                    break;
                case 32:
                    prefName = "PartnerEmail";
                    break;
                case 33:
                    prefName = "PartnerPhone";
                    break;
                case 34:
                    prefName = "PartnerSMS";
                    break;
                case 35:
                    prefName = "ResearchMail";
                    break;
                case 36:
                    prefName = "ResearchEmail";
                    break;
                case 37:
                    prefName = "ResearchPhone";
                    break;
                case 38:
                    prefName = "ResearchSMS";
                    break;
            }
            return prefName;
        }

        /// <summary>
        /// Get Clubcard Id for the Join Customer
        /// </summary>
        /// <param name="dotcomCustomerID"></param>
        /// <param name="userData"></param>
        /// <param name="joinRouteCode"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        private long CreateClubcardAccount(long dotcomCustomerID, Hashtable userData, string joinRouteCode, string culture)
        {
            Int64 clubcard = 0;
            LogData logData = new LogData();
            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.CREATE_CLUBCARD_ACCOUNT);
                request.Parameters.Add(ParameterNames.DOTCOM_CUSTOMER_ID, dotcomCustomerID);
                request.Parameters.Add(ParameterNames.USER_DATA, userData);
                request.Parameters.Add(ParameterNames.JOIN_ROUTE_CODE, joinRouteCode);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                logData.RecordStep("Create Clubcard Account Step..");
                logData.CaptureData("with join route code", joinRouteCode);

                response = _joinloyaltyServiceAdapter.Set<Int64>(request);
                if (response != null && response.Data != null)
                {
                    clubcard = response.Data.TryParse<Int64>();

                }
                if (clubcard <= 0)
                {
                    Exception ex = new Exception();
                    ex.Data.Add(ParameterNames.FRIENDLY_ERROR_MESSAGE, "Clubcard is not valid");
                    throw ex;
                }

                _logger.Submit(logData);
                return clubcard;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while creating Clubcard in Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// Get Customer Preferences 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private CustomerPreference GetCustomerPreferences(long customerId)
        {
            CustomerPreference customerPreferences = new CustomerPreference();
            LogData logData = new LogData();
            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.PREFERENCE_TYPE, PreferenceType.NULL);
                request.Parameters.Add(ParameterNames.OPTIONAL_PREFERENCE, "true");
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_PREFERENCES);
                logData.CustomerID = customerId.ToString();

                response = _preferenceServiceAdapter.Get<CustomerPreference>(request);
                customerPreferences = (CustomerPreference)response.Data;
                _logger.Submit(logData);
                return customerPreferences;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in GetCustomerPreferences in Join BC", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        #endregion Private Methods

    }
}

