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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;

namespace Web.Business.Tests
{
    [TestFixture]
    public class HomeBCTests
    {
        private HomeBC homeBC;
        private IAccountBC _accountProvider;
        private IPointsBC _pointsProvider;
        private IConfigurationProvider _configurationProvider;
        private IVoucherBC _vouchersProvider;
        private ILoggingService _logger;     
        protected IPersonalDetailsBC _personalDetailsProvider;

        [SetUp]
        public void SetUp()
        {
            _accountProvider = MockRepository.GenerateMock<IAccountBC>();
            _pointsProvider = MockRepository.GenerateMock<IPointsBC>();
            _configurationProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _vouchersProvider = MockRepository.GenerateMock<IVoucherBC>();
            _logger = MockRepository.GenerateMock<ILoggingService>();
            _personalDetailsProvider = MockRepository.GenerateMock<IPersonalDetailsBC>();
            homeBC = new HomeBC(_accountProvider, _vouchersProvider, _pointsProvider, _configurationProvider, _logger);
        }

        [TestCase]
        public void GetHouseHoldCustomers_Execution_Succesful()
        {
            List<HouseholdCustomerDetails> houseHoldCustomerDetailsList = new List<HouseholdCustomerDetails>();
            var houseHoldCustomerDetails = new HouseholdCustomerDetails
            {
                CustomerID = 0,
                ClubcardID = 100,
                Name1 = "Nitin",
                Name2 = "Goyal",
                Name3 = "Mr",
                PrimaryCustomerID = 0,
                TitleEnglish = "en",
                MailingAddressLine1 = "UK",
                MailingAddressPostCode = "AL74FG",
                MsgCardHolder = "Nitin",
                HouseHoldID = 89,
                CustomerUseStatusID = 1,
                CustomerType = "7",
                CustomerMailStatus = 1
            };
            houseHoldCustomerDetailsList.Add(houseHoldCustomerDetails);
            houseHoldCustomerDetails = new HouseholdCustomerDetails
            {
                CustomerID = 0,
                ClubcardID = 100,
                Name1 = "Nitin",
                Name2 = "Goyal",
                Name3 = "Mr",
                PrimaryCustomerID = 0,
                TitleEnglish = "en",
                MailingAddressLine1 = "UK",
                MailingAddressPostCode = "AL74FG",
                MsgCardHolder = "Nitin",
                HouseHoldID = 89,
                CustomerUseStatusID = 1,
                CustomerType = "7",
                CustomerMailStatus = 1
            };

            houseHoldCustomerDetailsList.Add(houseHoldCustomerDetails);
            _accountProvider.Stub(x => x.GetHouseHoldCustomersData(1, "en-GB")).IgnoreArguments()
    .Return(houseHoldCustomerDetailsList);

            List<HouseholdCustomerDetails> houseHoldCustomerDetailsListActual = homeBC.GetHouseHoldCustomers(1);
            Assert.AreSame(houseHoldCustomerDetailsList[0], houseHoldCustomerDetailsListActual[0]);
            Assert.AreSame(houseHoldCustomerDetailsList[1], houseHoldCustomerDetailsListActual[1]);
        }

        [TestCase]
        [ExpectedException]
        public void GetHouseHoldCustomers_Execution_Exception()
        {
            List<HouseholdCustomerDetails> householdCustomerDetailsList = null;
            _accountProvider.Stub(x => x.GetHouseHoldCustomersData(1, "enGB")).IgnoreArguments().Return(householdCustomerDetailsList);

            var householdCustomerDetailsModelList = homeBC.GetHouseHoldCustomers(1) as List<HouseholdCustomerDetails>;
            var householdCustomerDetailsModel = householdCustomerDetailsModelList[0];
        }


        [TestCase]
        public void GetHomePage_For_Without_FirstName_Customer()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers="20"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            PointsViewModel pointsViewModel = new PointsViewModel();
            pointsViewModel.Offers = offersList;
            //pointsViewModel.OptedPreference = "EquivalentVirginMiles";
            _pointsProvider.Stub(x => x.GetPointsViewdetails(1, "en-GB")).IgnoreArguments().Return(pointsViewModel);

            AccountDetails accountDetails = new AccountDetails();
            accountDetails.ClubcardID = 6342323232323;
            accountDetails.Name1 = "Nitin";
            accountDetails.Name3 = "Goy";
            accountDetails.TitleEnglish = "Mr.";
            accountDetails.PointsBalanceQty = 345;

            _vouchersProvider.Stub(x => x.GetCustomerAccountDetails(1, "en-GB")).IgnoreArguments().Return(accountDetails);

            DbConfiguration dbConfigurationPoints = new DbConfiguration();
            DbConfigurationItem dbConfigPointsItem = new DbConfigurationItem();
            dbConfigPointsItem.ConfigurationName = DbConfigurationItemNames.PtsSummaryDates;
            dbConfigPointsItem.ConfigurationValue1 = DateTime.Now.AddDays(-1).ToString();
            dbConfigPointsItem.ConfigurationValue2 = DateTime.Now.AddDays(-4).ToString();
            dbConfigPointsItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;
            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigPointsItem);
            dbConfigurationPoints.ConfigurationItems = lstDbConfigurationItems;

            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates },"en-GB")).IgnoreArguments().Return(dbConfigurationPoints);
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.ChinaHiddenFunctionalityTitle)).Return("0");
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HideFirstNameinSalutation)).Return("0");
            _configurationProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal)).Return(false);

            HomeViewModel homeViewModelActual =homeBC.GetHomeViewModel(1, "en-GB");
            HomeViewModel homeViewModelExpected = new HomeViewModel
            {
                ClubcardNumber = "efk1pyJJhu4bUmHD57I1Pg==",
                WelcomeMessage = " Mr. Goy",
                CustomerPointsBalance = "345",
                //OptedSchemeId = "EquivalentVirginMiles",
                MyMessageHeader = "Preference",
                CustomerVoucherTypeValue = "20"            
            };

            Assert.IsTrue(homeViewModelActual.Equals(homeViewModelExpected));
        }

        [TestCase]
        public void GetHomePage_For_Without_Title_Customer()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers = "20"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            PointsViewModel pointsViewModel = new PointsViewModel();
            pointsViewModel.Offers = offersList;
            //pointsViewModel.OptedPreference = "EquivalentVirginMiles";
            _pointsProvider.Stub(x => x.GetPointsViewdetails(1, "en-GB")).IgnoreArguments().Return(pointsViewModel);

            AccountDetails accountDetails = new AccountDetails();
            accountDetails.ClubcardID = 6342323232323;
            accountDetails.Name1 = "Nitin";
            accountDetails.Name3 = "Goy";
            accountDetails.TitleEnglish = "Mr.";
            accountDetails.PointsBalanceQty = 345;

            _vouchersProvider.Stub(x => x.GetCustomerAccountDetails(1, "en-GB")).IgnoreArguments().Return(accountDetails);

            DbConfiguration dbConfigurationPoints = new DbConfiguration();
            DbConfigurationItem dbConfigPointsItem = new DbConfigurationItem();
            dbConfigPointsItem.ConfigurationName = DbConfigurationItemNames.PtsSummaryDates;
            dbConfigPointsItem.ConfigurationValue1 = DateTime.Now.AddDays(-1).ToString();
            dbConfigPointsItem.ConfigurationValue2 = DateTime.Now.AddDays(-4).ToString();
            dbConfigPointsItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;
            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigPointsItem);
            dbConfigurationPoints.ConfigurationItems = lstDbConfigurationItems;

            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbConfigurationPoints);
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.ChinaHiddenFunctionalityTitle)).Return("1");
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HideFirstNameinSalutation)).Return("0");
            _configurationProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal)).Return(true);

            HomeViewModel homeViewModelActual = homeBC.GetHomeViewModel(1, "en-GB");
            HomeViewModel homeViewModelExpected = new HomeViewModel
            {
                ClubcardNumber = "efk1pyJJhu4bUmHD57I1Pg==",
                WelcomeMessage = " Goy",
                CustomerPointsBalance = "345",
                //OptedSchemeId = "EquivalentVirginMiles",
                MyMessageHeader = "Preference",
                CustomerVoucherTypeValue = "20"
            };

            Assert.IsTrue(homeViewModelActual.Equals(homeViewModelExpected));
        }

        [TestCase]
        public void GetHomePage_For_With_PointsError_MessageHeader()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers = "20"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            PointsViewModel pointsViewModel = new PointsViewModel();
            pointsViewModel.Offers = offersList;
            //pointsViewModel.OptedPreference = "EquivalentVirginMiles";
            _pointsProvider.Stub(x => x.GetPointsViewdetails(1, "en-GB")).IgnoreArguments().Return(pointsViewModel);

            AccountDetails accountDetails = new AccountDetails();
            accountDetails.ClubcardID = 6342323232323;
            accountDetails.Name1 = "Nitin";
            accountDetails.Name3 = "Goy";
            accountDetails.TitleEnglish = "Mr.";
            accountDetails.PointsBalanceQty = 345;

            _vouchersProvider.Stub(x => x.GetCustomerAccountDetails(1, "en-GB")).IgnoreArguments().Return(accountDetails);

            DbConfiguration dbConfigurationPoints = new DbConfiguration();
            DbConfigurationItem dbConfigPointsItem = new DbConfigurationItem();
            dbConfigPointsItem.ConfigurationName = DbConfigurationItemNames.PtsSummaryDates;
            dbConfigPointsItem.ConfigurationValue1 = "Feb 1 2016 12:00AM";
            dbConfigPointsItem.ConfigurationValue2 = "Feb 20 2016 12:00AM";
            dbConfigPointsItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;
            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigPointsItem);
            dbConfigurationPoints.ConfigurationItems = lstDbConfigurationItems;

            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbConfigurationPoints);
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.ChinaHiddenFunctionalityTitle)).Return("1");
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HideFirstNameinSalutation)).Return("0");
            _configurationProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal)).Return(false);

            HomeViewModel homeViewModelActual = homeBC.GetHomeViewModel(1, "en-GB");
            HomeViewModel homeViewModelExpected = new HomeViewModel
            {
                ClubcardNumber = "efk1pyJJhu4bUmHD57I1Pg==",
                WelcomeMessage = " Goy",
                CustomerPointsBalance = "345",
                //OptedSchemeId = "EquivalentVirginMiles",
                MyMessageHeader = "PointsError",
                CustomerVoucherTypeValue = "20"
            };

            Assert.IsTrue(homeViewModelActual.Equals(homeViewModelExpected));
        }

        [TestCase]
        public void GetHomePage_For_With_PointsError_Config_Values_NULL()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers = "20"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "0"
            };
            offersList.Add(offer);
            PointsViewModel pointsViewModel = new PointsViewModel();
            pointsViewModel.Offers = offersList;
            //pointsViewModel.OptedPreference = "EquivalentVirginMiles";
            _pointsProvider.Stub(x => x.GetPointsViewdetails(1, "en-GB")).IgnoreArguments().Return(pointsViewModel);

            AccountDetails accountDetails = new AccountDetails();
            accountDetails.ClubcardID = 6342323232323;
            accountDetails.Name1 = "Nitin";
            accountDetails.Name3 = "Goy";
            accountDetails.TitleEnglish = "Mr.";
            accountDetails.PointsBalanceQty = 345;

            _vouchersProvider.Stub(x => x.GetCustomerAccountDetails(1, "en-GB")).IgnoreArguments().Return(accountDetails);

            DbConfiguration dbConfigurationPoints = new DbConfiguration();
            DbConfigurationItem dbConfigPointsItem = new DbConfigurationItem();
            dbConfigPointsItem.ConfigurationName = DbConfigurationItemNames.PrimId;
            dbConfigPointsItem.ConfigurationValue1 = null;
            dbConfigPointsItem.ConfigurationValue2 = null;
            dbConfigPointsItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;
            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigPointsItem);
            dbConfigurationPoints.ConfigurationItems = lstDbConfigurationItems;

            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbConfigurationPoints);
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.ChinaHiddenFunctionalityTitle)).Return("1");
            _configurationProvider.Stub(x => x.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HideFirstNameinSalutation)).Return("0");
            _configurationProvider.Stub(x => x.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal)).Return(false);

            HomeViewModel homeViewModelActual = homeBC.GetHomeViewModel(1, "en-GB");
            HomeViewModel homeViewModelExpected = new HomeViewModel
            {
                ClubcardNumber = "efk1pyJJhu4bUmHD57I1Pg==",
                WelcomeMessage = " Goy",
                CustomerPointsBalance = "345",
                //OptedSchemeId = "EquivalentVirginMiles",
                MyMessageHeader = "Preference",
                CustomerVoucherTypeValue = "20"
            };

            Assert.IsTrue(homeViewModelActual.Equals(homeViewModelExpected));
        }

        [TearDown]
        public void TestCleanup()
        {
            _accountProvider = null;
            _pointsProvider = null;
            _vouchersProvider = null;
            _configurationProvider = null;
        }
    }
}
