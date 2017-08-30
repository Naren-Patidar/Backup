using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Web.Routing;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Collections;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class CustomerPreferenceBC : ICustomerPreferenceBC
    {
        MCARequest request;
        MCAResponse response;
        IServiceAdapter _preferenceServiceAdapter;
        private ILoggingService _Logger;
        private IConfigurationProvider _Config;

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

        public CustomerPreferenceBC(IServiceAdapter preferenceServiceAdapter, IConfigurationProvider config, ILoggingService logger)
        {
            this._preferenceServiceAdapter = preferenceServiceAdapter;
            this._Config = config;
            this._Logger = logger;
        }

        #region ClubDetails

        /// <summary>
        /// Method to get the club details for the customer
        /// </summary>
        /// <param name="customerId">long customerid</param>
        /// <returns>ClubDetails</returns>
        public ClubDetails GetClubDetails(long customerId)
        {
            LogData _logData = new LogData();
         //   _logData.RecordStep(string.Format("customerId : {0}", customerId));
            ClubDetails clubDetails = new ClubDetails();
            try
            {
                request = new MCARequest();

                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUB_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                _logData.CaptureData("request", request);
                response = _preferenceServiceAdapter.Get<ClubDetails>(request);
                clubDetails = response.Data as ClubDetails;
                _Logger.Submit(_logData);
            }
            catch(Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while getting clubdetails.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return clubDetails;
        }

        /// <summary>
        /// Method to update the clubdetails
        /// </summary>
        /// <param name="customerId">long customerid</param>
        /// <param name="clubDetails">ClubDetails clubdetails</param>
        /// <param name="emailIdTo">string emailid</param>
        /// <returns>bool status</returns>
        public bool UpdateClubDetails(long customerId, ClubDetails clubDetails, string emailIdTo)
        {
            LogData _logData = new LogData();
            List<string> blackListData = new List<string>();
            bool status;
            try
            {                
                response = new MCAResponse();
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.UPDATE_CLUB_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CLUB_DETAILS, clubDetails);
                request.Parameters.Add(ParameterNames.EMAIL_ID_TO, emailIdTo);
                blackListData.Add(emailIdTo);
                _logData.BlackLists = blackListData;
                _logData.CaptureData("request", request);
                response = this._preferenceServiceAdapter.Execute(request);
                status = response.Status;
                _logData.RecordStep(string.Format("Updation Status about CLubDetails: {0}", status));
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while updating clubdetails.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return status;
        }

        #endregion

        #region Options and Benefits

        /// <summary>
        /// Method to get the selected Options n benefits spreference
        /// </summary>
        /// <param name="preferences">List<CustomerPreference> preferences</param>
        /// <returns>short preferenceId</returns>
        public short GetSelectedStatementPreference(List<CustomerPreference> preferences)
        {
            LogData _logData = new LogData();
            short prefid = default(short);
            try
            {
                List<CustomerPreference> statement_preferences = preferences.FindAll(p => new short[] { (short)PreferenceEnum.Xmas_Saver,
                                                                                                    (short)PreferenceEnum.Airmiles_Standard,
                                                                                                    (short)PreferenceEnum.Airmiles_Premium,
                                                                                                    (short)PreferenceEnum.Virgin_Atlantic,
                                                                                                    (short)PreferenceEnum.BA_Miles_Standard,
                                                                                                    (short)PreferenceEnum.BA_Miles_Premium}.Contains(p.PreferenceID));
                List<CustomerPreference> optedprefs = statement_preferences.FindAll(p => p.POptStatus == OptStatus.OPTED_IN);
                CustomerPreference opted = statement_preferences.Find(p => p.POptStatus == OptStatus.OPTED_IN);
                prefid = (opted != null) ? opted.PreferenceID : (short)0;
                _logData.CaptureData("Selected Preference", prefid);
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while getting selected statement preference.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return prefid;
        }

        /// <summary>
        /// Method to get the Options and Benefits model 
        /// </summary>
        /// <param name="preference">CustomerPreference preferences</param>
        /// <param name="customerId">string customerId</param>
        /// <returns>OptionsAndBenefitsModel model</returns>
        public OptionsAndBenefitsModel GetOptionAndBenefitsModel(CustomerPreference preference, string customerId)
        {
            LogData _logData = new LogData();
            OptionsAndBenefitsModel model = new OptionsAndBenefitsModel();
            try
            {
                ClubDetails clubDetails = GetClubDetails(customerId.TryParse<Int64>());
                _logData.CaptureData("Club Details", clubDetails);
                model.Preference = preference;
                _logData.CaptureData("Preferences", preference);
                if (clubDetails.ClubInformation.FindAll(c => c.ClubID == BusinessConstants.CLUB_BA && c.IsDeleted.ToUpper() == "N").Count > 0)
                {
                    model.BAMilesClubDetails = clubDetails.ClubInformation.Find(c => c.ClubID == BusinessConstants.CLUB_BA && c.IsDeleted.ToUpper() == "N");
                }
                if (clubDetails.ClubInformation.FindAll(c => c.ClubID == BusinessConstants.CLUB_VIRGIN && c.IsDeleted.ToUpper() == "N").Count > 0)
                {
                    model.VirgnClubDetails = clubDetails.ClubInformation.Find(c => c.ClubID == BusinessConstants.CLUB_VIRGIN && c.IsDeleted.ToUpper() == "N");
                }
                if (clubDetails.ClubInformation.FindAll(c => c.ClubID == BusinessConstants.CLUB_AVIOS && c.IsDeleted.ToUpper() == "N").Count > 0)
                {
                    model.AviosClubDetails = clubDetails.ClubInformation.Find(c => c.ClubID == BusinessConstants.CLUB_AVIOS && c.IsDeleted.ToUpper() == "N");
                }

                model.IsXmasSaverVisible = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Xmas_Saver).Count > 0;
                model.IsAviosStandardVisisble = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Airmiles_Standard).Count > 0;
                model.IsAviosPremiumVisisble = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Airmiles_Premium).Count > 0;
                model.IsVirginMilesVisisble = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Virgin_Atlantic).Count > 0;
                model.IsBAMilesStandardVisisble = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.BA_Miles_Standard).Count > 0;
                model.IsBAMilesPremiumVisisble = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.BA_Miles_Premium).Count > 0;
                model.IsEmailStatementVisible = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Clubcard_E_Mails).Count > 0;

                model.IsXmasSaverOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Xmas_Saver && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.IsAviosStandardOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Airmiles_Standard && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.IsAviosPremiumOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Airmiles_Premium && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.IsVirginMilesOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.Virgin_Atlantic && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.IsBAMilesStandardOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.BA_Miles_Standard && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.IsBAMilesPremiumOpted = model.Preference.Preference.ToList().FindAll(p => p.PreferenceID == (short)PreferenceEnum.BA_Miles_Premium && p.POptStatus == OptStatus.OPTED_IN).Count > 0;
                model.AviosMembershipIdLabel = model.AviosClubDetails.MembershipID;
                model.VirginMembershipIdLabel = model.VirgnClubDetails.MembershipID;
                model.BaMembershipIdLabel = model.BAMilesClubDetails.MembershipID;

                model.PreviousSelectedPreferenceID = model.SelectedPreferenceID = GetSelectedStatementPreference(model.Preference.Preference.ToList());
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            { 
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while preparing model for Options n Benefits View.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return model;
        }

        /// <summary>
        /// Method to validate customer's Options n Benefits Preferences
        /// </summary>
        /// <param name="optnModel">OptionsAndBenefitsModel model</param>
        /// <returns>bool</returns>
        public bool ValidateCustomerPreferences(OptionsAndBenefitsModel optnModel)
        {
            LogData _logData = new LogData();
            bool chk = true;
            string regMembershipID = @"^[0-9]+$"; 
            bool IsMembershipForAviosEnable = _Config.GetBoolAppSetting(AppConfigEnum.IsMembershipForAviosEnable);
            try
            {
               
                switch (optnModel.SelectedPreferenceID.TryParse<PreferenceEnum>())
                {
                        
                    case PreferenceEnum.Virgin_Atlantic:
                        if (optnModel.VirgnClubDetails.MembershipID != null)
                        {
                           chk= GeneralUtility.ValidateVirginMembershipNumber(optnModel.VirgnClubDetails.MembershipID.Trim(), optnModel.VirgnClubDetails.MembershipID.Length.ToString(),regMembershipID);
                        }
                        break;
                    case PreferenceEnum.BA_Miles_Standard:
                    case PreferenceEnum.BA_Miles_Premium:
                        if (optnModel.BAMilesClubDetails.MembershipID != null)
                        {
                            chk = GeneralUtility.ValidateBAAviosMembership(optnModel.BAMilesClubDetails.MembershipID.Trim(), regMembershipID);
                        }
                        break;
                    case PreferenceEnum.Airmiles_Premium:
                    case PreferenceEnum.Airmiles_Standard:
                        if (optnModel.AviosClubDetails.MembershipID != null && IsMembershipForAviosEnable)
                        {
                            chk = GeneralUtility.ValidateAviosMembership(optnModel.AviosClubDetails.MembershipID.Trim(), regMembershipID);
                        }
                        break;
                }
                _logData.CaptureData("Selected Optins", optnModel.SelectedPreferenceID.TryParse<PreferenceEnum>());
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while validating Options n Benefits model.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return chk;
        }

        /// <summary>
        /// Method to get the Tab id for Options and Benefits
        /// </summary>
        /// <param name="optnModel">OptionsAndBenefitsModel model</param>
        /// <returns>tab id</returns>
        public int GetTabId(OptionsAndBenefitsModel optnModel)
        {
            LogData _logData = new LogData();
            int tab = -1;
            try
            {
                PreferenceEnum selected = optnModel.SelectedPreferenceID.TryParse<PreferenceEnum>();
                switch (selected)
                {
                    case PreferenceEnum.Xmas_Saver:
                        tab = 0;
                        break;
                    case PreferenceEnum.Airmiles_Premium:
                    case PreferenceEnum.Airmiles_Standard:
                        tab = 1;
                        break;
                    case PreferenceEnum.Virgin_Atlantic:
                        tab = 2;
                        break;
                    case PreferenceEnum.BA_Miles_Standard:
                    case PreferenceEnum.BA_Miles_Premium:
                        tab = 3;
                        break;

                }
                _logData.CaptureData("Selected Optins", optnModel.SelectedPreferenceID.TryParse<PreferenceEnum>());
                _logData.CaptureData("Tab Id for selected Options & Benefits", tab);
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while validating Options n Benefits model.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return tab;
        }

        public bool IsMilePreference(Int16 prefId)
        {
            LogData _logData = new LogData();
            bool chk = false;
            try
            {
                chk = new PreferenceEnum[] { PreferenceEnum.Airmiles_Standard, PreferenceEnum.BA_Miles_Standard, PreferenceEnum.Virgin_Atlantic }.Contains(prefId.TryParse<PreferenceEnum>());
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while checking if preference is MilePreference.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return chk;
        }

        #endregion        

        #region Contact Preferences

        #region Optins

        /// <summary>
        /// Method to get Optins for Contact Preference module
        /// </summary>
        /// <param name="preferences">CustomerPreference preference</param>
        /// <param name="customerData">CustomerFamilyMasterData customerData</param>
        /// <param name="isCustomerJoined">bool isCustomerJoined</param>
        /// <returns>OptInsModel model</returns>
        public OptInsModel GetOptIns(CustomerPreference preferences, CustomerFamilyMasterData customerData, bool isCustomerJoined)
        {
            LogData _logData = new LogData();
            OptInsModel model = new OptInsModel();
            try
            {
                model = this.GetOptIns(preferences);
                model.IsCustomerJoined = isCustomerJoined;
                model.CustomerIDEncr = CryptoUtility.EncryptTripleDES(preferences.CustomerID.ToString());
                model.CustomerEmail = model.CustomerEmail = (customerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].EmailAddress : string.Empty;
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while getting Contact preference optins.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return model;
        }

        /// <summary>
        /// Method to get Optins model for Join module
        /// </summary>
        /// <param name="preferences">ustomerPreference preference</param>
        /// <returns>OptInsModel model</returns>
        public OptInsModel GetOptIns(CustomerPreference preferences)
        {
            LogData _logData = new LogData();
            OptInsModel model = new OptInsModel();
            try
            {
                _logData.CaptureData("preference", preferences);
                
                model.IsOptInBehaviour = _Config.GetBoolAppSetting(AppConfigEnum.IsOptInBehaviour);
                _logData.RecordStep(String.Format("Optin Behaviour - {0}", model.IsOptInBehaviour));

                DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.PreferenceUIConfiguration);

                if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1))
                {
                    model.PrefUIConfiguration.AddRange(item.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
                }
                
                List<OptIns> optIns = new List<OptIns>();
                foreach (var prefOptin in this.MCAPreferences)
                {
                    CustomerPreference pref = preferences.Preference.ToList().Find(p => p.PreferenceID == prefOptin);
                    if (pref != null)
                    {
                        optIns.Add(new OptIns()
                                        {
                                            PreferenceID = prefOptin,
                                            IsAlreadyOpted = pref.POptStatus == OptStatus.OPTED_IN,
                                            IsOpted = pref.POptStatus == OptStatus.OPTED_IN,
                                            IsVisible = model.PrefUIConfiguration.Where<PreferencesUIConfig>(p => p.preferenceid == prefOptin).FirstOrDefault().isvisible
                                        });
                    }
                }

                if (!model.IsOptInBehaviour)
                {
                    optIns.ForEach(o => o.IsOpted = !o.IsOpted);
                    model.OptIns = optIns;
                    _logData.CaptureData("Optin ", model.OptIns);
                }
                else
                {
                    model.OptOuts = optIns;
                    _logData.CaptureData("OptOuts ", model.OptOuts);
                }
                model.IsCustomerJoined = false;
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            { 
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while getting COntact Preference Optins for Join.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return model;
        }

        /// <summary>
        /// Method to update optins
        /// </summary>
        /// <param name="model">OptInsModel model</param>
        /// <param name="customerdetails">AccountDetails customerdetails</param>
        /// <returns>bool status</returns>
        public bool UpdateOptIns(OptInsModel model, AccountDetails customerdetails)
        {
            LogData _logData = new LogData();
            bool status = false;
            try
            {
                CustomerPreference objcustPref = new CustomerPreference();
                customerdetails.EmailAddress = model.CustomerEmail;
                objcustPref.Preference = GetMyOptInsPreferences(model).ToArray();
                objcustPref.UserID = _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer);
                objcustPref.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                _logData.CaptureData("Culture ", objcustPref.Culture);
                response = new MCAResponse();
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.UPDATE_CUSTOMER_PREFERENCES);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, CryptoUtility.DecryptTripleDES(model.CustomerIDEncr));
                request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCE, objcustPref);
                request.Parameters.Add(ParameterNames.CUSTOMER_DETAILS, customerdetails);
                response = this._preferenceServiceAdapter.Execute(request);
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while updating Contact Preference optins.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return status;
        }

        #endregion

        #region ContactModel

        /// <summary>
        /// Method to get the customer preferences
        /// </summary>
        /// <param name="preferences">CustomerPreference preferences</param>
        /// <returns>ContactPreferencesModel</returns>
        public ContactModel GetContactModel(CustomerPreference preferences, CustomerFamilyMasterData customerData)
        {
            LogData _logData = new LogData();
            ContactModel model = new ContactModel();
            try
            {
                model.IsEMailContactVisible = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.E_Mail_Contact);
                model.IsMobileSMSVisisble = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Mobile_SMS);
                model.IsPostContactVisisble = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Post_Contact);
                model.IsPostLargePrintVisisble = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Post_Large_Print);

                model.IsEMailContactOpted = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.E_Mail_Contact && p.POptStatus == OptStatus.OPTED_IN);
                model.IsMobileSMSOpted = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Mobile_SMS && p.POptStatus == OptStatus.OPTED_IN);
                model.IsPostContactOpted = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Post_Contact && p.POptStatus == OptStatus.OPTED_IN);
                model.IsPostLargePrintOpted = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Post_Large_Print && p.POptStatus == OptStatus.OPTED_IN);
                model.IsBrailleOpted = preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Braille && p.POptStatus == OptStatus.OPTED_IN);

                var filteredPrefTypes = new short[] { (short)PreferenceEnum.E_Mail_Contact, (short)PreferenceEnum.Mobile_SMS, (short)PreferenceEnum.Post_Contact };

                List<CustomerPreference> contact_preferences = preferences.Preference.ToList().FindAll(p => filteredPrefTypes.Contains(p.PreferenceID));

                CustomerPreference opted = contact_preferences.Where(p => p.POptStatus == OptStatus.OPTED_IN).FirstOrDefault();

                model.PreviousSelectedPreferenceID = model.SelectedPreferenceID = (opted != null) ? opted.PreferenceID : (short)0;

                model.ROMobile = model.Mobile = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].MobileNumber : string.Empty;
                model.ROEmail = model.Email = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].EmailAddress : string.Empty;
                model.PostalAddress = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].MailingAddressLine1 : string.Empty;
                model.CustomerID = (customerData != null && customerData.CustomerData != null && customerData.CustomerData.Count > 0) ? customerData.CustomerData[0].CustomerId : 0;
                model.CustomerIDEncr = CryptoUtility.EncryptTripleDES(model.CustomerID.ToString());

                model.MobilePrefixes = _Config.GetConfigurations(DbConfigurationTypeEnum.Prefix, DbConfigurationItemNames.MobilePhoneNumber).ConfigurationValue2;
                DbConfigurationItem length_configs = _Config.GetConfigurations(DbConfigurationTypeEnum.Length_of_the_input_fields, DbConfigurationItemNames.MobilePhoneNumber);

                model.MobileMinLength = length_configs.ConfigurationValue1.TryParse<int>();
                model.MobileMaxLength = length_configs.ConfigurationValue2.TryParse<int>();
                if (model.SelectedPreferenceID == 0)
                {
                    model.IsPostContactOpted = true;
                    model.IsPostLargePrintOpted = model.IsBrailleOpted = false;
                    model.SelectedPreferenceID = (short)PreferenceEnum.Post_Contact;
                }
                switch (model.SelectedPreferenceID.TryParse<PreferenceEnum>())
                {
                    case PreferenceEnum.E_Mail_Contact:
                        model.ConfirmEmail = model.Email;
                        break;
                    case PreferenceEnum.Mobile_SMS:
                        model.ConfirmMobile = model.Mobile;
                        break;
                }

                model.IsValid = true;
                
                _logData.BlackLists.AddRange(new List<string> { model.Mobile, model.Email, model.PostalAddress });
                _logData.CaptureData("Contact Data ", model);
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return model;
        }

        /// <summary>
        /// Method to update the contact preferences
        /// </summary>
        /// <param name="model">ContactModel</param>
        /// <param name="customerdetails">AccountDetails</param>
        /// <returns>bool</returns>
        public bool UpdateContactPreferences(ContactModel model, AccountDetails customerdetails)
        {
            LogData _logData = new LogData();
            bool status = false;
            try
            {
                // check if the preference is not changed
                if (model.SelectedPreferenceID != model.PreviousSelectedPreferenceID)
                {
                    CustomerPreference objcustPref = new CustomerPreference();
                    customerdetails.EmailAddress = string.IsNullOrEmpty(model.Email) ? model.ROEmail : model.Email;
                    objcustPref.Preference = GetMyContactPreferences(model).ToArray();
                    objcustPref.UserID = _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer);
                    objcustPref.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                    response = new MCAResponse();
                    request = new MCARequest();
                    request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.UPDATE_CUSTOMER_PREFERENCES);
                    request.Parameters.Add(ParameterNames.CUSTOMER_ID, CryptoUtility.DecryptTripleDES(model.CustomerIDEncr));
                    request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCE, objcustPref);
                    request.Parameters.Add(ParameterNames.CUSTOMER_DETAILS, customerdetails);
                    response = this._preferenceServiceAdapter.Execute(request);
                    status = response.Status;                    
                }
                model = RetainContactPrefValues(model);
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC in Updating Contact Preferences.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return status;
 
        }

        public ContactModel RetainContactPrefValues(ContactModel model)
        {
            LogData _logData = new LogData();
            try
            {
                switch (model.SelectedPreferenceID.TryParse<PreferenceEnum>())
                {
                    case PreferenceEnum.E_Mail_Contact:
                        model.ConfirmMobile = string.Empty;
                        model.ConfirmEmail = model.Email = string.IsNullOrEmpty(model.Email) ? model.ROEmail : model.Email;
                        model.Mobile = model.ROMobile;
                        break;
                    case PreferenceEnum.Mobile_SMS:
                        model.ConfirmEmail = string.Empty;
                        model.ConfirmMobile = model.Mobile = string.IsNullOrEmpty(model.Mobile) ? model.ROMobile : model.Mobile;
                        model.Email = model.ROEmail;
                        break;
                    case PreferenceEnum.Post_Contact:
                        model.ConfirmEmail = string.Empty;
                        model.ConfirmMobile = string.Empty;
                        model.Mobile = model.ROMobile;
                        model.Email = model.ROEmail;
                        break;
                }
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return model;
        }

        /// <summary>
        /// Method to get the customer data update model to check the duplicate account
        /// </summary>
        /// <param name="customerData">CustomerFamilyMasterData</param>
        /// <param name="model">ContactModel</param>
        /// <returns>CustomerFamilyMasterDataUpdate</returns>
        public CustomerFamilyMasterDataUpdate GetCustomerUpdateModel(CustomerFamilyMasterData customerData, ContactModel model, out bool checkRequired)
        {
            LogData _logData = new LogData();
            CustomerFamilyMasterDataUpdate updateModel = new CustomerFamilyMasterDataUpdate();
            checkRequired = false;
            try
            {
                CustomerMasterData _customerMaster = customerData.CustomerData.FirstOrDefault();
                FamilyMasterData _familyMaster = customerData.FamilyData.FirstOrDefault();
                if (_customerMaster != null)
                {
                    updateModel.CustomerID = _customerMaster.CustomerId;
                    updateModel.Title = _customerMaster.Title;
                    updateModel.FirstName = _customerMaster.FirstName;
                    updateModel.Initial = _customerMaster.Initial;
                    updateModel.LastName = _customerMaster.LastName;
                    updateModel.EmailAddress = updateModel.email_address = _customerMaster.EmailAddress;
                    updateModel.EmailStatus = string.IsNullOrEmpty(_customerMaster.CustomerEmailStatus) ? CustomerMailStatusEnum.Missing : (CustomerMailStatusEnum)_customerMaster.CustomerEmailStatus.TryParse<Int32>();
                    updateModel.MobilePhoneNumber = updateModel.mobile_phone_number = _customerMaster.MobileNumber;
                    updateModel.MobilePhoneStatus = string.IsNullOrEmpty(_customerMaster.CustomerMobilePhoneStatus) ? CustomerMailStatusEnum.Missing : (CustomerMailStatusEnum)_customerMaster.CustomerMobilePhoneStatus.TryParse<Int32>();

                    if (((PreferenceEnum)model.SelectedPreferenceID) == PreferenceEnum.E_Mail_Contact)
                    {
                        updateModel.EmailAddress = updateModel.email_address = model.ConfirmEmail;
                        updateModel.EmailStatus = CustomerMailStatusEnum.Deliverable;
                        checkRequired = model.ConfirmEmail != _customerMaster.EmailAddress;
                    }
                    else if (((PreferenceEnum)model.SelectedPreferenceID) == PreferenceEnum.Mobile_SMS)
                    {
                        updateModel.MobilePhoneNumber = updateModel.mobile_phone_number = model.ConfirmMobile;
                        updateModel.MobilePhoneStatus = CustomerMailStatusEnum.Deliverable;
                        checkRequired = model.Mobile != _customerMaster.MobileNumber;
                    }
                    updateModel.CustomerUseStatus = string.IsNullOrEmpty(_customerMaster.CustomerUseStatus) ? BusinessConstants.CUSTOMERUSESTATUS_ACTIVE : _customerMaster.CustomerUseStatus.TryParse<Int32>();
                    updateModel.DateOfBirth = _customerMaster.FamilyMemberDOB1;
                    updateModel.Sex = _customerMaster.Sex;

                    updateModel.MailingAddressLine1 = _customerMaster.MailingAddressLine1;
                    updateModel.MailingAddressLine2 = _customerMaster.MailingAddressLine2;
                    updateModel.MailingAddressLine3 = _customerMaster.MailingAddressLine3;
                    updateModel.MailingAddressLine4 = _customerMaster.MailingAddressLine4;
                    updateModel.MailingAddressLine5 = _customerMaster.MailingAddressLine5;
                    updateModel.MailingAddressLine6 = _customerMaster.MailingAddressLine6;
                    updateModel.PostCode = _customerMaster.MailingAddressPostCode;
                    updateModel.evening_phone_number = _customerMaster.EveningPhonenumber;
                    updateModel.daytime_phone_number = _customerMaster.DayTimePhonenumber;
                    updateModel.SSN = _customerMaster.SSN;
                    updateModel.PassportNo = _customerMaster.PassportNo;
                    updateModel.MailStatus = string.IsNullOrEmpty(_customerMaster.CustomerMailStatus) ? CustomerMailStatusEnum.Missing : (CustomerMailStatusEnum)_customerMaster.CustomerMailStatus.TryParse<Int32>();
                    updateModel.RaceID = _customerMaster.RaceID.TryParse<Int32>();

                    updateModel.ISOLanguageCode = _customerMaster.ISOLanguageCode;
                    updateModel.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                   
                    if (_familyMaster != null)
                    {
                        updateModel.NumberOfHouseholdMembers = _familyMaster.NumberOfHouseholdMembers.TryParse<short>();
                        updateModel.FamilyMember1Dob = customerData.FamilyData.Count > 0 ? customerData.FamilyData[0].DateOfBirth : null;
                        updateModel.FamilyMember2Dob = customerData.FamilyData.Count > 1 ? customerData.FamilyData[1].DateOfBirth : null;
                        updateModel.FamilyMember3Dob = customerData.FamilyData.Count > 2 ? customerData.FamilyData[2].DateOfBirth : null;
                        updateModel.FamilyMember4Dob = customerData.FamilyData.Count > 3 ? customerData.FamilyData[3].DateOfBirth : null;
                        updateModel.FamilyMember5Dob = customerData.FamilyData.Count > 4 ? customerData.FamilyData[4].DateOfBirth : null;
                    }
                }
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return updateModel;
        }

        /// <summary>
        /// Method to validate the user input for contact preferences
        /// </summary>
        /// <param name="contactModel">Contact Preference Model</param>
        /// <returns>bool</returns>
        public bool ValidateContactPreferences(ContactModel contactModel)
        {
            LogData _logData = new LogData();
            bool chk = true;
            DbConfigurationItem FormatEmail = _Config.GetConfigurations(DbConfigurationTypeEnum.Format, DbConfigurationItemNames.EmailAddress);
            //@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$";
            DbConfigurationItem FormatMobile= _Config.GetConfigurations(DbConfigurationTypeEnum.Format, DbConfigurationItemNames.MobilePhoneNumber); 
            //"^[0-9]*$";
            try
            {
                switch (contactModel.SelectedPreferenceID.TryParse<PreferenceEnum>())
                {
                    case PreferenceEnum.E_Mail_Contact:
                        chk = !string.IsNullOrEmpty(contactModel.Email)
                             && !string.IsNullOrEmpty(contactModel.ConfirmEmail)
                             && RegexUtility.IsRegexMatch(contactModel.Email.Trim(), FormatEmail.ConfigurationValue1, false, false)
                             && RegexUtility.IsRegexMatch(contactModel.ConfirmEmail.Trim(), FormatEmail.ConfigurationValue1, false, false);
                        if (!chk)
                        {
                            contactModel.ErrorMessage = contactModel.InvalidEMail;;                            
                        }
                        else if (contactModel.Email.Trim() != contactModel.ConfirmEmail.Trim())
                        {
                            chk = false;
                            contactModel.ErrorMessage = contactModel.CompareEMail;
                        }
                        break;
                    case PreferenceEnum.Mobile_SMS:
                        chk = !string.IsNullOrEmpty(contactModel.Mobile)
                            && !string.IsNullOrEmpty(contactModel.ConfirmMobile)
                            && RegexUtility.IsRegexMatch(contactModel.Mobile.Trim(), FormatMobile.ConfigurationValue1, false, false)
                            && RegexUtility.IsRegexMatch(contactModel.ConfirmMobile.Trim(), FormatMobile.ConfigurationValue1, false, false)
                            && contactModel.Mobile.Length >= contactModel.MobileMinLength
                            && (from t in contactModel.MobilePrefixes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) where contactModel.Mobile.StartsWith(t) select true).ToList().FirstOrDefault();
                        if (!chk)
                        {
                            contactModel.ErrorMessage = contactModel.InvalidMobile;
                        }
                        else if(contactModel.Mobile.Trim() != contactModel.ConfirmMobile.Trim())
                        {
                            chk = false;
                            contactModel.ErrorMessage = contactModel.CompareMobile;   
                        }                        
                        break;
                }
                _logData.CaptureData("Selected Optins", contactModel.SelectedPreferenceID.TryParse<PreferenceEnum>());
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while validating contact preference model.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return chk;
        }

        /// <summary>
        /// Method to send email notifications to customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <param name="contactModel">Customer Preferences</param>
        /// <param name="customerDetails">Customer Details</param>
        /// <param name="PageName">page Name</param>
        /// <returns></returns>
        public bool SendEmailToCustomer(long customerId, ContactModel contactModel, AccountDetails customerDetails, string PageName)
        {
            LogData _logData = new LogData();
            bool chk = true;
            try
            {
                DBConfigurations config = _Config.GetConfigurations(DbConfigurationTypeEnum.EmailNotification);
                DbConfigurationItem emailconfig = config[DbConfigurationItemNames.EmailNotification];
                DbConfigurationItem trackEmail = config[DbConfigurationItemNames.TrackEmail];
                DbConfigurationItem trackMobile = config[DbConfigurationItemNames.TrackMobileNumber];
                if (!emailconfig.IsDeleted && emailconfig.ConfigurationValue1.TryParse<Int32>() > 0)
                {
                    Hashtable htTrackFields = new Hashtable();
                    bool chkEmail = false, chkMobile = false;
                    switch (contactModel.SelectedPreferenceID.TryParse<PreferenceEnum>())
                    {
                        case PreferenceEnum.E_Mail_Contact:
                            htTrackFields["Email"] = string.IsNullOrEmpty(contactModel.TrackEmail) ? string.Empty : contactModel.TrackEmail;
                            htTrackFields["newEmailaddress"] = string.IsNullOrEmpty(contactModel.Email) ? string.Empty : contactModel.Email;
                            htTrackFields["bEmailChange"] = "bEmailChange";
                            htTrackFields["oldEmailAddress"] = string.IsNullOrEmpty(contactModel.ROEmail) ? string.Empty : contactModel.ROEmail;
                            chkEmail = (!trackEmail.IsDeleted && trackEmail.ConfigurationValue1.TryParse<Int32>() > 0)
                                && !htTrackFields["newEmailaddress"].ToString().ToUpper().Equals(htTrackFields["oldEmailAddress"].ToString().ToUpper());
                            break;
                        case PreferenceEnum.Mobile_SMS:
                            htTrackFields["TrackMobile"] = string.IsNullOrEmpty(contactModel.TrackMobile) ? string.Empty : contactModel.TrackMobile;
                            htTrackFields["oldEmailAddress"] = string.IsNullOrEmpty(contactModel.ROEmail) ? string.Empty : contactModel.ROEmail;
                            contactModel.ROMobile = string.IsNullOrEmpty(contactModel.ROMobile) ? string.Empty : contactModel.ROMobile;
                            chkMobile = (!trackMobile.IsDeleted && trackMobile.ConfigurationValue1.TryParse<Int32>() > 0)
                                && !contactModel.Mobile.Equals(contactModel.ROMobile);
                            break;
                    }
                    if (chkEmail || chkMobile)
                    {
                        CustomerPreference objcustPref = new CustomerPreference();
                        objcustPref.Preference = GetMyContactPreferences(contactModel).ToArray();
                        objcustPref.UserID = _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer);
                        objcustPref.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;

                        //CCMCA-5671
                        objcustPref.CustomerID = customerId;
                        customerDetails.EmailAddress = string.IsNullOrEmpty(contactModel.Email) && chkEmail ? contactModel.ROEmail : contactModel.Email;
                        //CCMCA-5671

                        request = new MCARequest();
                        request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                        request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCES, objcustPref);
                        request.Parameters.Add(ParameterNames.CUST_DETAILS, customerDetails);
                        request.Parameters.Add(ParameterNames.PAGE_NAME, PageName);
                        request.Parameters.Add(ParameterNames.TRACKHT, htTrackFields);
                        request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.SEND_EMAILNOTICE_TO_CUSTOMERS);
                        response = _preferenceServiceAdapter.Set<bool>(request);
                        if (response.Status)
                        {
                            chk = true;
                        }
                    }
                }
                _Logger.Submit(_logData);
                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while sending email notifications to customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return chk;
        }

        #endregion       

        #endregion        

        #region Common

        public bool UpdateCustomerPreferences(OptionsAndBenefitsModel optionModel, AccountDetails customerdetails, string customerId)
        {
            LogData _logData = new LogData();
            bool status = false;
            try
            {
                CustomerPreference objcustPref = new CustomerPreference();
                objcustPref.CustomerID = customerId.TryParse<Int64>();
                objcustPref.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                objcustPref.UserID = _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer);
                objcustPref.Preference = GetMyClubcardStatementPreferences(optionModel).ToArray();
                _logData.CaptureData("objcustPref", objcustPref);
                response = new MCAResponse();
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.UPDATE_CUSTOMER_PREFERENCES);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, objcustPref.CustomerID);
                request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCE, objcustPref);
                request.Parameters.Add(ParameterNames.CUSTOMER_DETAILS, customerdetails);
                response = this._preferenceServiceAdapter.Execute(request);
                if (response.Status)
                {
                    _logData.RecordStep("Customer Preferences Updated...");
                    optionModel.AviosClubDetails.IsDeleted = new PreferenceEnum[] { PreferenceEnum.Airmiles_Standard, PreferenceEnum.Airmiles_Premium }.Contains(optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>()) ? "N" : "Y";
                    optionModel.BAMilesClubDetails.IsDeleted = new PreferenceEnum[] { PreferenceEnum.BA_Miles_Standard, PreferenceEnum.BA_Miles_Premium }.Contains(optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>()) ? "N" : "Y";
                    optionModel.VirgnClubDetails.IsDeleted = new PreferenceEnum[] { PreferenceEnum.Virgin_Atlantic }.Contains(optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>()) ? "N" : "Y";

                    ClubDetails clubDetails = new ClubDetails();
                    clubDetails.UserID = _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer);
                    clubDetails.JoinDate = DateTime.Now;
                    clubDetails.ClubInformation.Add(optionModel.AviosClubDetails);
                    clubDetails.ClubInformation.Add(optionModel.BAMilesClubDetails);
                    clubDetails.ClubInformation.Add(optionModel.VirgnClubDetails);
                    status = UpdateClubDetails(objcustPref.CustomerID, clubDetails, optionModel.CustomerEmail);
                }
                optionModel.IsXmasSaverOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Xmas_Saver;
                optionModel.IsVirginMilesOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Virgin_Atlantic; 
                optionModel.IsBAMilesStandardOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Standard; 
                optionModel.IsBAMilesPremiumOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.BA_Miles_Premium; 
                optionModel.IsAviosStandardOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Standard; 
                optionModel.IsAviosPremiumOpted = optionModel.SelectedPreferenceID.TryParse<PreferenceEnum>() == PreferenceEnum.Airmiles_Premium;
                
                optionModel.PreviousSelectedPreferenceID = optionModel.SelectedPreferenceID;
                _logData.RecordStep(string.Format("Club Details Updation Status", status));
                _Logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerPreferenceBC while updating customer preferences.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return status;
        }

        #endregion

        #region Private Methods

        private List<CustomerPreference> GetMyClubcardStatementPreferences(OptionsAndBenefitsModel model)
        {
            LogData _logData = new LogData();
            try
            {
                List<CustomerPreference> userPreferences = new List<CustomerPreference>();
                List<PreferenceEnum> allPreferences = new List<PreferenceEnum> { PreferenceEnum.Xmas_Saver,
                                                                                 PreferenceEnum.Airmiles_Standard,
                                                                                 PreferenceEnum.Airmiles_Premium,
                                                                                 PreferenceEnum.Virgin_Atlantic,
                                                                                 PreferenceEnum.BA_Miles_Standard,
                                                                                 PreferenceEnum.BA_Miles_Premium};
                DBConfigurations config = _Config.GetConfigurations(DbConfigurationTypeEnum.Send_Preference_Email);
                foreach (PreferenceEnum p in allPreferences)
                {
                    CustomerPreference userPreference;
                    DbConfigurationItem prefConfig = config[((int)p).ToString()];
                    bool IsEmailRequired = !prefConfig.IsDeleted && !string.IsNullOrEmpty(prefConfig.ConfigurationValue1);
                    if (p == model.SelectedPreferenceID.TryParse<PreferenceEnum>())
                    {
                        userPreference = new CustomerPreference()
                        {
                            PreferenceID = (short)p,
                            UpdateDateTime = DateTime.Now,
                            POptStatus = OptStatus.OPTED_IN
                        };
                        // check if new selection was not previously selected
                        if ((int)p != model.PreviousSelectedPreferenceID && IsEmailRequired)
                        {
                            userPreference.EmailSubject = prefConfig.ConfigurationValue1;
                        }
                        _logData.CaptureData("User Preference ", userPreference);
                    }
                    else
                    {
                        userPreference = new CustomerPreference()
                        {
                            PreferenceID = (short)p,
                            UpdateDateTime = DateTime.Now,
                            POptStatus = OptStatus.OPTED_OUT
                        };
                    }
                    userPreferences.Add(userPreference);
                    _logData.CaptureData("User Preference ", userPreference);
                }
               
                _Logger.Submit(_logData);
                return userPreferences;
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException("Failed while getting list of My Clubcard Statement Preferences in CustomerPreferenceBC.", exp, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private List<CustomerPreference> GetMyOptInsPreferences(OptInsModel model)
        {
            LogData _logData = new LogData();
            List<CustomerPreference> userPreferences = new List<CustomerPreference>();
            try
            {
                DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.PreferenceUIConfiguration);

                if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1))
                {
                    model.PrefUIConfiguration.AddRange(item.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
                }

                model.OptIns.ForEach(o => o.IsOpted = !o.IsOpted);
                DBConfigurations configs = _Config.GetConfigurations(DbConfigurationTypeEnum.Send_Preference_Email);

                foreach (var p in this.MCAPreferences)
                {
                    DbConfigurationItem prefConfig = configs[((Int16)p).ToString()];
                    OptIns option = model.IsOptInBehaviour ? model.OptOuts.Find(o => o.PreferenceID == p) : model.OptIns.Find(o => o.PreferenceID == p);

                    if (option != null && model.PrefUIConfiguration.Any(uipref => uipref.isvisible && uipref.preferenceid == option.PreferenceID))
                    {
                        bool bEmailNeeded = option.IsOpted && !option.IsAlreadyOpted && !string.IsNullOrEmpty(prefConfig.ConfigurationValue1);
                        // This is to make sure only expected preferences are accepted as part of the post request.
                        userPreferences.Add(new CustomerPreference()
                                            {
                                                PreferenceID = p,
                                                UpdateDateTime = DateTime.Now,
                                                POptStatus = option.OptStatus,
                                                IsDeleted = "N",
                                                EmailSubject = bEmailNeeded ? prefConfig.ConfigurationValue1 : String.Empty
                                            });

                        var uiPrefCfg = model.PrefUIConfiguration.Where<PreferencesUIConfig>(uipref => uipref.preferenceid == option.PreferenceID).FirstOrDefault();
                        if (uiPrefCfg.dependentprefidsassame != null && uiPrefCfg.dependentprefidsassame.Count > 0)
                        {
                            uiPrefCfg.dependentprefidsassame.ForEach(uip =>
                            {
                                if (!userPreferences.Any(up => up.PreferenceID == uip))
                                {
                                    userPreferences.Add(new CustomerPreference()
                                                        {
                                                            PreferenceID = uip,
                                                            UpdateDateTime = DateTime.Now,
                                                            POptStatus = option.OptStatus,
                                                            IsDeleted = "N",
                                                            EmailSubject = bEmailNeeded ? prefConfig.ConfigurationValue1 : String.Empty
                                                        });
                                }
                            });
                        }

                        if (uiPrefCfg.dependentprefidsasopp != null && uiPrefCfg.dependentprefidsasopp.Count > 0)
                        {
                            uiPrefCfg.dependentprefidsasopp.ForEach(uip =>
                            {
                                if (!userPreferences.Any(up => up.PreferenceID == uip))
                                {
                                    userPreferences.Add(new CustomerPreference()
                                    {
                                        PreferenceID = uip,
                                        UpdateDateTime = DateTime.Now,
                                        POptStatus = option.OptStatus == OptStatus.OPTED_IN ? OptStatus.OPTED_OUT : OptStatus.OPTED_IN,
                                        IsDeleted = "N",
                                        EmailSubject = bEmailNeeded ? prefConfig.ConfigurationValue1 : String.Empty
                                    });
                                }
                            });
                        }
                    }
                }
            
                _logData.CaptureData("User Preferences", userPreferences);
                _Logger.Submit(_logData);
                return userPreferences;
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException("Failed while getting list of My Optins Preferences in CustomerPreferenceBC.", exp, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private List<CustomerPreference> GetMyContactPreferences(ContactModel model)
        {
            LogData _logData = new LogData();
            try
            {
                List<CustomerPreference> userPreferences = new List<CustomerPreference>();
                List<PreferenceEnum> allPreferences = new List<PreferenceEnum> { PreferenceEnum.E_Mail_Contact,
                                                                                PreferenceEnum.Mobile_SMS,
                                                                                PreferenceEnum.Post_Contact,
                                                                                PreferenceEnum.Post_Large_Print
                                                                                };
                DBConfigurations config = _Config.GetConfigurations(DbConfigurationTypeEnum.Send_Preference_Email);
                foreach (PreferenceEnum p in allPreferences)
                {
                    CustomerPreference userPreference;
                    DbConfigurationItem prefConfig = config[((int)p).ToString()];
                    bool IsEmailRequired = !prefConfig.IsDeleted && !string.IsNullOrEmpty(prefConfig.ConfigurationValue1);
                    if (p == model.SelectedPreferenceID.TryParse<PreferenceEnum>())
                    {
                        userPreference = new CustomerPreference()
                        {
                            PreferenceID = (short)p,
                            UpdateDateTime = DateTime.Now,
                            POptStatus = OptStatus.OPTED_IN
                        };
                        // check if new selection was not previously selected
                        if ((int)p != model.PreviousSelectedPreferenceID && IsEmailRequired)
                        {
                            userPreference.EmailSubject = prefConfig.ConfigurationValue1;
                        }
                    }
                    else
                    {
                        userPreference = new CustomerPreference()
                        {
                            PreferenceID = (short)p,
                            UpdateDateTime = DateTime.Now,
                            POptStatus = OptStatus.OPTED_OUT
                        };
                    }
                    userPreferences.Add(userPreference);
                    _logData.CaptureData("User Preference ", userPreference);
                }
                _Logger.Submit(_logData);
                return userPreferences;
            }
            catch (Exception exp)
            {
                throw GeneralUtility.GetCustomException("Failed while getting list of My Contact Preferences in CustomerPreferenceBC.", exp, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }


        #endregion
        
    }
}
