using System;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using CommonEntities = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
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

namespace Web.Business.Tests.BusinessLogics
{
    [TestFixture]
    public class JoinBCTests
    {
        private IServiceAdapter _customerServiceAdapter;
        private IServiceAdapter _preferenceServiceAdapter;
        private IServiceAdapter _utilityServiceAdapter;
        private IServiceAdapter _joinloyaltyServiceAdapter;
        private IServiceAdapter _clubcardServiceAdapter;
        private IServiceAdapter _locatorServiceAdapter;
        private JoinBC _joinBC;
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


            _joinBC = new JoinBC(_customerServiceAdapter,
                                _preferenceServiceAdapter,
                                 _utilityServiceAdapter,
                                 _joinloyaltyServiceAdapter,
                                 _clubcardServiceAdapter,
                                 _locatorServiceAdapter,
                                 _logger,
                                 _configProvider);
        }

        [TestCase]
        public void IsProfaneText_ExecutionSuccessful()
        {
            PersonalDetailsViewModel personaldetailsViewModel = new PersonalDetailsViewModel();
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
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData(customerMasterDataList, familyMasterDataList, 1);
            personaldetailsViewModel.CustomerFamilyMasterData = customerFamilyMasterData;
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
            var isProfane = _joinBC.IsProfaneText(personaldetailsViewModel, 0);
            //breaks in JoinBC while setting drop downs and reading ChinaHiddenFunctionality Postcode from dbxml
        }
    }
}
