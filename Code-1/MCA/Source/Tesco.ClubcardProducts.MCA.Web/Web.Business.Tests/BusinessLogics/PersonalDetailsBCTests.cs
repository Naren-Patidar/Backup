using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using CommonEntities = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.NGCUtilityService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.JoinLoyaltyService;
using PreferenceServices = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Collections;

namespace Web.Business.Tests
{
    [TestFixture]
    public class PersonalDetailsBCTests
    {
        private IServiceAdapter _customerServiceAdapter;
        private IServiceAdapter _preferenceServiceAdapter;
        private IServiceAdapter _utilityServiceAdapter;
        private IServiceAdapter _joinloyaltyServiceAdapter;
        private IServiceAdapter _clubcardServiceAdapter;
        private IServiceAdapter _locatorServiceAdapter;
        private IServiceAdapter _myAccountCustomerServiceAdapter;
        private PersonalDetailsBC _personalDetailsProvider;
        private ILoggingService _logger;
        private IConfigurationProvider _configProvider = null;
        private ICustomerService _customerServiceClient;
        private IUtilityService _utilityServiceClient;
        private IClubcardService _clubcardServiceClient;
        private IJoinLoyaltyService _joinLoyaltyServiceClient;
        private PreferenceServices.IPreferenceService _preferenceServiceClient;
        private ILocatorSvcSDA _locatorServiceClient;

        [SetUp]
        public void SetUp()
        {
            _customerServiceClient = MockRepository.GenerateMock<ICustomerService>();
            _utilityServiceClient = MockRepository.GenerateMock<IUtilityService>();
            _clubcardServiceClient = MockRepository.GenerateMock<IClubcardService>();
            _joinLoyaltyServiceClient = MockRepository.GenerateMock<IJoinLoyaltyService>();
            _preferenceServiceClient = MockRepository.GenerateMock<PreferenceServices.IPreferenceService>();
            _locatorServiceClient = MockRepository.GenerateMock<ILocatorSvcSDA>();
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _logger = MockRepository.GenerateMock<ILoggingService>();

            _customerServiceAdapter = MockRepository.GenerateMock<CustomerServiceAdapter>(_customerServiceClient, _logger);
            _preferenceServiceAdapter = MockRepository.GenerateMock<PreferenceServiceAdapter>(_preferenceServiceClient, _logger);
            _utilityServiceAdapter = MockRepository.GenerateMock<UtilityServiceAdapter>(_utilityServiceClient, _logger);
            _joinloyaltyServiceAdapter = MockRepository.GenerateMock<JoinLoyaltyServiceAdapter>(_joinLoyaltyServiceClient, _logger);
            _clubcardServiceAdapter = MockRepository.GenerateMock<ClubcardServiceAdapter>(_clubcardServiceClient, _logger, _configProvider);
            _locatorServiceAdapter = MockRepository.GenerateMock<LocatorSvcSDAAdapter>(_locatorServiceClient, _logger);
            _myAccountCustomerServiceAdapter = MockRepository.GenerateMock<MyAccountServiceAdapter>(_locatorServiceClient, _logger);


            _personalDetailsProvider = new PersonalDetailsBC(_customerServiceAdapter,
                                _preferenceServiceAdapter,
                                 _utilityServiceAdapter,
                                 _joinloyaltyServiceAdapter,
                                 _clubcardServiceAdapter,
                                 _locatorServiceAdapter,
                                 _myAccountCustomerServiceAdapter,
                                 _logger,
                                 _configProvider);
        }

        [TestCase]
        public void GetCustomerDataView_Execution_Successful()
        {
            #region Customer Data

            var customerMasterDataList = new List<CustomerMasterData>();
            var customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            List<FamilyMasterData> familyMasterDataList = new List<FamilyMasterData>();

            var familyMasterData = new FamilyMasterData
            {
                SeqNo = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                NumberOfHouseholdMembers = 1
            };
            familyMasterDataList.Add(familyMasterData);
            #endregion

            #region Preference Data from common entities
            CommonEntities.CustomerPreference preference = new CommonEntities.CustomerPreference();
            CommonEntities.CustomerPreference[] customerPreferences = new CommonEntities.CustomerPreference[8];

            var preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 43,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[0] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 44,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[1] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 45,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[2] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 1,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[3] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 2,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[4] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 3,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[5] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 4,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[6] = preferences;
            preferences = new CommonEntities.CustomerPreference
            {
                PreferenceID = 5,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = CommonEntities.OptStatus.OPTED_IN
            };
            customerPreferences[7] = preferences;
            preference.Preference = customerPreferences;
            #endregion

            #region Customer Data  from Service call
            var customerFamilyMasterData = new CustomerFamilyMasterData(customerMasterDataList, familyMasterDataList, 1);
            string resultXml = @"<CustomerInformation><NewDataSet><Customer><CustomerID>435388</CustomerID>
                <CompleteName>Sahitestqweqq Sa</CompleteName><official_id>34234242423</official_id><postal_code>AL7 4FG
                </postal_code><is_primary_customer_of_household>Yes</is_primary_customer_of_household><family_member_1_dob>
                1976-06-16T00:00:00+01:00</family_member_1_dob><TitleEnglish>Mr</TitleEnglish><Name1>Sahitestqweqq</Name1>
                <Name2>Sa</Name2><Name3>Alexanderewe</Name3><Sex>M</Sex><MailingAddressLine1>33 CHAMBERS COMPANY GROVE
                </MailingAddressLine1><MailingAddressLine2>WELWYN GARDEN CITY</MailingAddressLine2><MailingAddressLine3>
                WELWYN GARDEN CITY</MailingAddressLine3><MailingAddressLine4>WELWYN GARDEN CITY</MailingAddressLine4>
                <MailingAddressLine5>WELWYN GARDEN CITY</MailingAddressLine5><email_address>Sahitest122@abc.com
                </email_address><daytime_phone_number>07345678933</daytime_phone_number><mobile_phone_number>
                07345678808</mobile_phone_number><evening_phone_number /><SSN /><PassportNo /><ISOLanguageCode>- S  
                </ISOLanguageCode><RaceID>0</RaceID><JoinedStoreID>5424</JoinedStoreID><MailingAddressPostCode>AL7 4FG
                </MailingAddressPostCode><CustomerUseStatusID>1</CustomerUseStatusID><CustomerMailStatus>7</CustomerMailStatus>
                <CustomerMobilePhoneStatus>7</CustomerMobilePhoneStatus><CustomerEmailStatus>7</CustomerEmailStatus></Customer>
                <Customer><CustomerID>435388</CustomerID><CompleteName>Sahitestqweqq Sa</CompleteName><official_id>0065749870
                </official_id><postal_code>AL7 4FG</postal_code><is_primary_customer_of_household>Yes</is_primary_customer_of_household>
                <family_member_1_dob>1976-06-16T00:00:00+01:00</family_member_1_dob><TitleEnglish>Mr</TitleEnglish><Name1>Sahitestqweqq
                </Name1><Name2>Sa</Name2><Name3>Alexanderewe</Name3><Sex>M</Sex><MailingAddressLine1>33 CHAMBERS COMPANY GROVE</MailingAddressLine1>
                <MailingAddressLine2>WELWYN GARDEN CITY</MailingAddressLine2><MailingAddressLine3>WELWYN GARDEN CITY</MailingAddressLine3>
                <MailingAddressLine4>WELWYN GARDEN CITY</MailingAddressLine4><MailingAddressLine5>WELWYN GARDEN CITY</MailingAddressLine5>
                <email_address>Sahitest122@abc.com</email_address><daytime_phone_number>07345678933</daytime_phone_number>
                <mobile_phone_number>07345678808</mobile_phone_number><evening_phone_number /><SSN /><PassportNo />
                <ISOLanguageCode>- S  </ISOLanguageCode><RaceID>0</RaceID><JoinedStoreID>5424</JoinedStoreID>
                <MailingAddressPostCode>AL7 4FG</MailingAddressPostCode><CustomerUseStatusID>1</CustomerUseStatusID>
                <CustomerMailStatus>7</CustomerMailStatus><CustomerMobilePhoneStatus>7</CustomerMobilePhoneStatus>
                <CustomerEmailStatus>7</CustomerEmailStatus></Customer></NewDataSet><NewDataSet><FamilyDetails>
                <FamilyMemberSeqNo>1</FamilyMemberSeqNo><DateOfBirth>2015-01-01T00:00:00+00:00</DateOfBirth>
                <number_of_household_members>2</number_of_household_members></FamilyDetails></NewDataSet>
                <NewDataSet><NoOFFamilyMembers><number_of_household_members>1</number_of_household_members></NoOFFamilyMembers></NewDataSet></CustomerInformation>";
            string errorXml = "";
            int rowCount = 2;
            _customerServiceClient.Stub(x => x.GetCustomerDetails(out errorXml,
                out resultXml, out rowCount,
               "sdf", 1, "dfg")).OutRef(errorXml, resultXml, rowCount).IgnoreArguments().Return(true);
            #endregion

            #region Preference Data from Preference entities
            PreferenceServices.CustomerPreference preferenceFromService = new PreferenceServices.CustomerPreference();
            PreferenceServices.CustomerPreference[] customerPreferencesFromService = new PreferenceServices.CustomerPreference[8];

            var preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 43,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[0] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 44,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[1] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 45,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[2] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 1,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[3] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 2,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[4] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 3,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[5] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 4,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[6] = preferencesFromPreferenceService;
            preferencesFromPreferenceService = new PreferenceServices.CustomerPreference
            {
                PreferenceID = 5,
                CustomerPreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                POptStatus = PreferenceServices.OptStatus.OPTED_IN
            };
            customerPreferencesFromService[7] = preferencesFromPreferenceService;
            preferenceFromService.Preference = customerPreferencesFromService;

            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(9, PreferenceServices.PreferenceType.NULL, false)).IgnoreArguments().Return(preferenceFromService);

            #endregion
            var customerFamilyMasterDataModel = _personalDetailsProvider.GetCustomerDataView(1);
        }

        [TestCase]
        public void SetCustomerDataView_Execution_Successful()
        {
            PersonalDetailsViewModel personalDetailsViewModel = new PersonalDetailsViewModel();
            #region Customer Data

            var customerMasterDataList = new List<CustomerMasterData>();
            var customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            List<FamilyMasterData> familyMasterDataList = new List<FamilyMasterData>();

            var familyMasterData = new FamilyMasterData
            {
                SeqNo = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                NumberOfHouseholdMembers = 1
            };
            familyMasterDataList.Add(familyMasterData);
            #endregion
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData(customerMasterDataList, familyMasterDataList, 1);
            personalDetailsViewModel.CustomerFamilyMasterData = customerFamilyMasterData;

            #region Club Preference Data
            List<BTClubPreference> preferenceList = new List<BTClubPreference>();
            var preferences = new BTClubPreference
                        {
                            PreferenceID = 1,
                            PreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                            OptedStatus = true
                        };
            preferenceList.Add(preferences);
            preferences = new BTClubPreference
                        {
                            PreferenceID = 2,
                            PreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                            OptedStatus = true
                        };
            preferenceList.Add(preferences);
            preferences = new BTClubPreference
                        {
                            PreferenceID = 3,
                            PreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                            OptedStatus = true
                        };
            preferenceList.Add(preferences);
            preferences = new BTClubPreference
                        {
                            PreferenceID = 4,
                            PreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                            OptedStatus = true
                        };
            preferenceList.Add(preferences);
            preferences = new BTClubPreference
                        {
                            PreferenceID = 5,
                            PreferenceType = BusinessConstants.PREFERENCETYPE_DIETRY,
                            OptedStatus = true
                        };
            preferenceList.Add(preferences);
            personalDetailsViewModel.PersonalDetailsPreferences = preferenceList;
            #endregion

            _configProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.AccountDuplicateCheckRequired)).Return(false);
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HidePostCode)).Return("0");
            _configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.ServiceConsumer)).Return("Dotcom Customer");
            _configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.IsReplacementCardWithYourNewName)).Return("1");
            _configProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.Profanity)).Return(false);
            string resultXml = @"<NewDataSet>
  <Duplicate>
    <IsAlternateIDDuplicate>0</IsAlternateIDDuplicate>
    <PromotionCodeExist>0</PromotionCodeExist>
    <ISDuplicate>0</ISDuplicate>
    <DuplicateCustomerID>0</DuplicateCustomerID>
    <DuplicateAltCustomerID>0</DuplicateAltCustomerID>
  </Duplicate>
</NewDataSet>";
            string resultXmls = @"<NewDataSet>
  <ViewMyAccountDetails>
    <TitleEnglish>Mrs</TitleEnglish>
    <Name3>Kewlanif</Name3>
    <Name1>Test</Name1>
    <PointsBalanceQty>3040</PointsBalanceQty>
    <Vouchers>30.0</Vouchers>
    <CustomerID>64654958</CustomerID>
    <PrimaryCustName>Mrs T Kewlanif</PrimaryCustName>
    <ClubcardID>634004024093742262</ClubcardID>
    <PrimaryCustName1>Test</PrimaryCustName1>
    <PrimaryCustName2>sk</PrimaryCustName2>
    <PrimaryCustName3>Kewlanif</PrimaryCustName3>
    <PrimaryClubcardID>634004024093742262</PrimaryClubcardID>
  </ViewMyAccountDetails>
</NewDataSet>";
            PreferenceServices.CustomerPreference custPref = null;
            PreferenceServices.CustomerDetails objCustomerDetails = null;
            Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
            resourceKeys.Add("PageName", "PageName");
            resourceKeys.Add("TrackFName", "TrackFName");
            resourceKeys.Add("TrackMName", "TrackMName");
            resourceKeys.Add("TrackSName", "TrackSName");
            resourceKeys.Add("SelectDay", "SelectDay");
            resourceKeys.Add("SelectMonth", "SelectMonth");
            resourceKeys.Add("SelectYear", "SelectYear");
            resourceKeys.Add("TrackEmail", "TrackEmail");
            resourceKeys.Add("TrackMobile", "TrackMobile");
            resourceKeys.Add("TrackDayTimePhone", "TrackDayTimePhone");
            resourceKeys.Add("TrackEveningPhone", "TrackEveningPhone");
            resourceKeys.Add("RecordExist", "RecordExist");
            resourceKeys.Add("Race", "Race");
            string errorXml = "";
            long customerId = 0;
            _joinLoyaltyServiceClient.Stub(x => x.AccountDuplicateCheck(out resultXml, "sdsd")).OutRef(resultXml).IgnoreArguments().Return(true);
            _customerServiceClient.Stub(x => x.UpdateCustomerDetails(out errorXml, out customerId, "dgdg", "sd")).OutRef(errorXml, customerId).IgnoreArguments().Return(true);
            _clubcardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXmls, 1, "en-GB")).OutRef(errorXml, resultXmls).IgnoreArguments().Return(true);
            _preferenceServiceClient.Stub(x => x.SendEmailToCustomers(custPref, objCustomerDetails)).IgnoreArguments().Return(true);
            _preferenceServiceClient.Stub(x => x.SendEmailNoticeToCustomers(1, custPref, objCustomerDetails, "", "")).IgnoreArguments().Return(true);
            _preferenceServiceClient.Stub(x => x.MaintainCustomerPreference(1, custPref, objCustomerDetails)).IgnoreArguments();
            bool isUpdated = _personalDetailsProvider.SetCustomerDataView(personalDetailsViewModel, 1, resourceKeys);
            Assert.IsTrue(isUpdated);
        }

        [TestCase]
        public void IsProfaneText_ExecutionSuccessful()
        {
            PersonalDetailsViewModel personalDetailsViewModel = new PersonalDetailsViewModel();
            #region Customer Data

            var customerMasterDataList = new List<CustomerMasterData>();
            var customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            customerMasterData = new CustomerMasterData
            {
                EmailAddress = "nitin_goyal01@infosys.com",
                MobileNumber = "9888904921",
                Title = "mr.",
                FirstName = "nitin",
                Initial = "ni",
                LastName = "goyal",
                FamilyMemberDOB1 = "27-06-1985",
                Sex = "M",
                MailingAddressLine1 = "405",
                MailingAddressLine2 = "heritage",
                MailingAddressLine3 = "apartment",
                MailingAddressLine4 = "",
                MailingAddressLine5 = "",
                MailingAddressLine6 = "",
                MailingAddressPostCode = "AL74FG",
                DOB = "27-60-1985",
                PassportNo = "3314144",
                DayTimePhonenumber = "1213131313",
                EveningPhonenumber = "1213131313",
                RaceID = "12",
                ISOLanguageCode = "",
                CustomerEmailStatus = "4",
                CustomerMobilePhoneStatus = "3",
                CustomerUseStatus = "3"
            };
            customerMasterDataList.Add(customerMasterData);

            List<FamilyMasterData> familyMasterDataList = new List<FamilyMasterData>();

            var familyMasterData = new FamilyMasterData
            {
                SeqNo = 1,
                DateOfBirth = DateTime.Now.AddYears(-30),
                NumberOfHouseholdMembers = 1
            };
            familyMasterDataList.Add(familyMasterData);
            #endregion

                        Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
            resourceKeys.Add("PageName","PageName");
            resourceKeys.Add("TrackFName", "TrackFName");
            resourceKeys.Add("TrackMName", "TrackMName");
            resourceKeys.Add("TrackSName", "TrackSName");
            resourceKeys.Add("SelectDay", "SelectDay");
            resourceKeys.Add("SelectMonth", "SelectMonth");
            resourceKeys.Add("SelectYear", "SelectYear");
            resourceKeys.Add("TrackEmail", "TrackEmail");
            resourceKeys.Add("TrackMobile", "TrackMobile");
            resourceKeys.Add("TrackDayTimePhone", "TrackDayTimePhone");
            resourceKeys.Add("TrackEveningPhone", "TrackEveningPhone");
            resourceKeys.Add("RecordExist", "RecordExist");
            resourceKeys.Add("Race", "Race");
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData(customerMasterDataList, familyMasterDataList, 1);
            personalDetailsViewModel.CustomerFamilyMasterData = customerFamilyMasterData;
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name1)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name2)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name3)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressPostCode)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine1)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine2)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine3)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine4)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine5)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.DaytimePhoneNumber)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MobilePhoneNumber)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.EmailAddress)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.PrimaryId)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.SecondaryId)).IgnoreArguments().Return("1");
            _configProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HidePostCode)).Return("0");
            _configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.IsReplacementCardWithYourNewName)).Return("1");
            _configProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.Profanity)).Return(true);

            string errorXml = "";
            string resultXml = @"<NewDataSet>
  <ProfanityCheck>
    <IsProfane>0</IsProfane>
  </ProfanityCheck>
</NewDataSet>";
            int rowCount = 2;
            _utilityServiceClient.Stub(x => x.ProfanityCheck(out errorXml, out resultXml, out rowCount, "sds")).OutRef(errorXml, resultXml, rowCount).IgnoreArguments().Return(true);
            var isProfane = _personalDetailsProvider.IsProfaneText(personalDetailsViewModel, 0, resourceKeys);
        }

        [TestCase]
        public void GetAddresses_Execution_Successful()
        {
            string resultXml = @"<ArrayOfAddressLiteEntity>
  <AddressLiteEntity SubBuilding='' BuildingNumber='1' BuildingName='' Locality='' Town='WELWYN GARDEN CITY' Postcode='AL74FG' Street='CHAMBERS GROVE'
Organisation='' County='' GridEast='00524' GridNorth='00211' isBusinessAddress='false' isBlockedAddress='false' />
<AddressLiteEntity SubBuilding='sdfd' BuildingNumber='1' BuildingName='' Locality='' Town='WELWYN GARDEN CITY' Postcode='AL74FG' Street='CHAMBERS GROVE'
Organisation='' County='' GridEast='00524' GridNorth='00211' isBusinessAddress='false' isBlockedAddress='false' />
 </ArrayOfAddressLiteEntity>";
            DbConfigurationItem dbConfigPostCode = new DbConfigurationItem();
            dbConfigPostCode.ConfigurationName = DbConfigurationItemNames.MailingAddressPostCode;
            dbConfigPostCode.ConfigurationType = DbConfigurationTypeEnum.Format;
            dbConfigPostCode.ConfigurationValue1 = "^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
            dbConfigPostCode.ConfigurationValue2 = "^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Format, DbConfigurationItemNames.MailingAddressPostCode)).IgnoreArguments().Return(dbConfigPostCode);
            _locatorServiceClient.Stub(x => x.FindAddressLite("AL74FG", null, null)).IgnoreArguments().Return(resultXml);
            var AddressList = _personalDetailsProvider.PopulateAddress("AL74FG");
        }

        [TearDown]
        public void TestCleanup()
        {
            _customerServiceAdapter = null;
            _preferenceServiceAdapter = null;
            _locatorServiceAdapter = null;
            _joinloyaltyServiceAdapter = null;
            _logger = null;
            _configProvider = null;
            _clubcardServiceAdapter = null;
            _utilityServiceAdapter = null;
        }
    }
}
