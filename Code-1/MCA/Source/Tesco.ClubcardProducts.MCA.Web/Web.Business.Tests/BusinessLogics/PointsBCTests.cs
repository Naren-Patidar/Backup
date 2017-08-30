using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.StatementFormatProvider;
using ClubcardOnline.PointsSummarySequencing;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;



namespace Web.Business.Tests
{
    [TestFixture]
    public class PointsBCTests
    {
        //protected IClubcardServiceAdapter _clubcardServiceAdapter;
        private IAccountBC _accountProvider;
        private IServiceAdapter _smartVoucherServiceAdapter;
        private IServiceAdapter _clubcardServiceAdapter;
        private IServiceAdapter _preferenceService;
        private PointsBC pointsBC;
        private IConfigurationProvider _configurationProvider;
        private IStatementFormatProvider _statementFormatProvider;
        ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            _clubcardServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _smartVoucherServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _preferenceService = MockRepository.GenerateMock<IServiceAdapter>();
            _configurationProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _statementFormatProvider = MockRepository.GenerateMock<IStatementFormatProvider>();
            _accountProvider = MockRepository.GenerateMock<IAccountBC>();
            _logger = MockRepository.GenerateMock<ILoggingService>();
            pointsBC = new PointsBC(_accountProvider,_preferenceService, _smartVoucherServiceAdapter, _clubcardServiceAdapter,
                _configurationProvider, _statementFormatProvider, _logger);
        }

        //[TestCase]
        //public void GetPointsCollectionPeriod_Execution_Succesful()
        //{
        //    List<Offer> offersList = new List<Offer>();
        //    var offer = new Offer
        //    {
        //        Period = "Current",
        //        EndDateTime = DateTime.Now,
        //        StartDateTime = DateTime.Now
        //    };
        //    offersList.Add(offer);
        //   offer = new Offer
        //    {
        //        Period = "Aug 2015",
        //        EndDateTime = DateTime.Now,
        //        StartDateTime = DateTime.Now
        //    };
        //   offersList.Add(offer);
        //   offer = new Offer
        //    {
        //        Period = "May 2015",
        //        EndDateTime = DateTime.Now,
        //        StartDateTime = DateTime.Now
        //    };
        //   offersList.Add(offer);

        //   var pointCollectionPeriodList = new List<PointsCollectionPeriodModel>();
        //    var pointCollectionPeriod = new PointsCollectionPeriodModel
        //    {
        //        period="Current"
        //    };
        //    pointCollectionPeriodList.Add(pointCollectionPeriod);
        //    pointCollectionPeriod = new PointsCollectionPeriodModel
        //    {
        //        period = "Aug 2015"
        //    };
        //    pointCollectionPeriodList.Add(pointCollectionPeriod);
        //    pointCollectionPeriod = new PointsCollectionPeriodModel
        //    {
        //        period = "May 2015"
        //    };
        //    pointCollectionPeriodList.Add(pointCollectionPeriod);
        //   for (int i = 0; i < offersList.Count; i++)
        //   {
        //       _mappingService.Stub(x => x.Map<Offer, PointsCollectionPeriodModel>(offersList[i]))
        //           .Return(pointCollectionPeriodList[i]);
        //   }
        //   _clubcardServiceAdapter.Stub(x => x.GetOffersForCustomer(1,"en-GB")).IgnoreArguments().Return(offersList);
        //    var pointsCollectionPeriodModelList = pointsBC.GetPointsViewdetails(1,"en-GB");
        //    Assert.AreEqual(3,pointsCollectionPeriodModelList.Count);
        //}

        [TestCase]
        public void GetPointsViewdetails_NoPointsEarnedEver_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now
            };
            offersList.Add(offer);
            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            List<CustomerPreference> customerPreference = new List<CustomerPreference>();
            var preference = new CustomerPreference
            {
                PreferenceID = 1,
                POptStatus = OptStatus.OPTED_IN
            };
            customerPreference.Add(preference);
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var pointsOffersList = pointsBC.GetPointsViewdetails(671969397, "en-GB");
            Assert.AreEqual(1, pointsOffersList.Offers.Count);
        }

        [TestCase]
        public void GetPointsViewdetails_PointsEarnedEver_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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
            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            List<CustomerPreference> customerPreference = new List<CustomerPreference>();
            var preference = new CustomerPreference
            {
                PreferenceID = 1,
                POptStatus = OptStatus.OPTED_IN
            };
            customerPreference.Add(preference);
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var pointsOffersList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            Assert.AreEqual(3, pointsOffersList.Offers.Count);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferencestdVoucher_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            List<CustomerPreference> customerPreference = new List<CustomerPreference>();
            var preference = new CustomerPreference
            {
                PreferenceID = 1,
                POptStatus = OptStatus.OPTED_IN
            };
            customerPreference.Add(preference);
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("stdVoucher", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceEcoupon_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 13,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("Ecoupon", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceBAmilesPremium_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 14,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("BAmiles", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceBAmilesStandard_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 10,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);


            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("BAmiles", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceVirginMiles_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 17,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("virginMiles", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceAIRMilesPremium_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 12,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);
            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("aviosMiles", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_PreferenceAIRMilesStandard_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 11,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards);

            var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            //Assert.AreEqual("stdVoucher", preferenceList.OptedPreference);
        }

        [TestCase]
        public void GetPointsViewdetails_NoPreference_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20"
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

            MCARequest mcaRequestOffer = new MCARequest();
            MCAResponse mcaResponseOffer = new MCAResponse();
            mcaResponseOffer.Data = offersList;
            mcaResponseOffer.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<Offer>>(mcaRequestOffer)).IgnoreArguments().Return(mcaResponseOffer);

            CustomerPreference preference = new CustomerPreference();
            var preferences = new CustomerPreference
            {
                PreferenceID = 0,
                POptStatus = OptStatus.OPTED_IN
            };
            CustomerPreference[] customerPreferences = new CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceService.Stub(x => x.Get<CustomerPreference>(mcaRequestPref)).IgnoreArguments().Return(mcaResponsetPref);

            var accountDetails = new AccountDetails
            {
                ClubcardID = 1
            };
            MCARequest mcaRequestaAccount = new MCARequest();
            MCAResponse mcaResponseAccount = new MCAResponse();
            mcaResponseAccount.Data = accountDetails;
            mcaResponseAccount.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<AccountDetails>(mcaRequestaAccount)).IgnoreArguments().Return(mcaResponseAccount);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var voucherRewards = new VoucherRewardDetails
            {
                RewardIssued = 0,
                RewardLeftOver = 0,
                RewardUsed = 0
            };
            voucherRewardDetailsList.Add(voucherRewards);
            MCARequest mcaRequestRewards = new MCARequest();
            MCAResponse mcaResponseRewards = new MCAResponse();
            mcaResponseRewards.Data = voucherRewardDetailsList;
            mcaResponseRewards.Status = true;
            _smartVoucherServiceAdapter.Stub(x => x.Get<List<VoucherRewardDetails>>(mcaRequestRewards)).IgnoreArguments().Return(mcaResponseRewards); var preferenceList = pointsBC.GetPointsViewdetails(223981236, "en-GB");
            
            //Assert.AreEqual("stdVoucher", preferenceList.OptedPreference);
        }

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForstdVoucher_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForBAmilesPremium_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForBAmilesStandard_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForVirginMiles_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForAIRMilesPremium_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForAIRMilesStandard_Execution_Succesful()
        //{

        //}

        //[TestCase]
        //public void GetPointsViewDetails_VoucherValueForNoVoucher_Execution_Succesful()
        //{

        //}
        [TestCase]
        public void GetPointsSummary_Execution_Successful()
        {
            List<PointsSummary> pointsSummaryList = new List<PointsSummary>();
            PointsSummary pointsSummary = new PointsSummary
            {
                BonusVouchers = "1.5",
                CustomerType = "",
                EndDateTime = "31/12/2015",
                MainClubcardID = "",
                NoCoupons = "",
                PointSummaryDescEnglish = "XmasSavers",
                RewardMilesRate = "12",
                Salutation = "",
                StartDateTime = "01/10/2015",
                StatementType = "XmasSavers",
                StatementVideo = "",
                TopUpVouchers = "2.4",
                TotalPoints = "23",
                TotalReward = "233",
                TotalRewardMiles = "23"
            };

            foreach (string pointsTypeLiteral in Enum.GetNames(typeof(PointsTypeEnum)))
            {
                PointsTypeEnum pointsType = (PointsTypeEnum)Enum.Parse(typeof(PointsTypeEnum), pointsTypeLiteral);

                pointsSummary.pointsBreakup.Add(pointsType, 233);
            }

            foreach (string rewardsTypeLiteral in Enum.GetNames(typeof(RewardsTypeEnum)))
            {
                RewardsTypeEnum pointsType = (RewardsTypeEnum)Enum.Parse(typeof(RewardsTypeEnum), rewardsTypeLiteral);
                pointsSummary.rewardsBreakup.Add(pointsType, 23);
            }
            pointsSummaryList.Add(pointsSummary);
            MCARequest mcaRequestPointsSummary = new MCARequest();
            MCAResponse mcaResponsePointsSummary = new MCAResponse();
            mcaResponsePointsSummary.Data = pointsSummaryList;
            mcaResponsePointsSummary.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<List<PointsSummary>>(mcaRequestPointsSummary)).IgnoreArguments().Return(mcaResponsePointsSummary);

            _configurationProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.DisplayDateFormat)).Return("dd/MM/yyyy");
            _configurationProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.TescoPointsImagePath)).Return("../Content/images/");

            StatementFormat stFormat = new StatementFormat();
            stFormat.OfferID = 5;
            Statement st = new Statement();
            st.StatementType = "XmasSavers";
            PointsBox pointsBox = new PointsBox
            {
                BoxLogoFileName = "f.png",
                BoxName = "Petrol",
                DataColumnName = "TescoPoints1",
                SectionType = "TescoPoints",
                Position = 1
            };
            st.PointsBoxes.Add(pointsBox);
            pointsBox = new PointsBox
                        {
                            BoxLogoFileName = "f.png",
                            BoxName = "Petrol",
                            DataColumnName = "TescoPoints2",
                            SectionType = "TescoPoints",
                            Position = 1
                        };
            st.PointsBoxes.Add(pointsBox);
            pointsBox = new PointsBox
                       {
                           BoxLogoFileName = "f.png",
                           BoxName = "Petrol",
                           DataColumnName = "TescoBankPoints1",
                           SectionType = "TescoBankPoints",
                           Position = 1
                       };
            st.PointsBoxes.Add(pointsBox);
            pointsBox = new PointsBox
                        {
                            BoxLogoFileName = "f.png",
                            BoxName = "Petrol",
                            DataColumnName = "TescoBankPoints2",
                            SectionType = "TescoBankPoints",
                            Position = 1
                        };
            st.PointsBoxes.Add(pointsBox);
            stFormat.Statements.Add(st);
            _statementFormatProvider.Stub(x => x.GetStatementFormat(5, "")).IgnoreArguments().Return(stFormat);
            var PointsSummaryList = pointsBC.GetPointsSummary(671969397, 21, "en-GB");
        }

        [TestCase]
        public void GetPointsDetails_Execution_Succesful()
        {
            CustomerTransactions customerTransactions = new CustomerTransactions();

            OfferDetails offer = new OfferDetails
            {
                OfferID = 211,
                StartDateTime = Convert.ToDateTime("23/12/2014"),
                EndDateTime = Convert.ToDateTime("15/12/2015"),
                PointsToRewardConversionRate="20",
                CollectionPeriodNumber ="2",
            };
            
            List<TransactionDetails> transactions = new List<TransactionDetails>();
            var transaction = new TransactionDetails
            {
                ClubCardTransactionId = 1,
                ClubcardId = "6345267189",
                ClubCardStatusDescEnglish = "active",
                CustType = "std",
                TransactionDateTime = "23/3/2015",
                PartnerId = 1,
                TescoStoreId = 1,
                AmountSpent = "23.45",
                NormalPoints = "12",
                BonusPoints = "10",
                TotalPoints = "22",
                TransactionDescription = "Merchant",
                PointIssuePartnerGroupId = "1",
                PointIssuePartnerGroupDesc = "Box"
            };
            transactions.Add(transaction);

            transaction = new TransactionDetails
            {
                ClubCardTransactionId = 1,
                ClubcardId = "6345267189",
                ClubCardStatusDescEnglish = "active",
                CustType = "std",
                TransactionDateTime = "23/3/2015",
                PartnerId = 1,
                TescoStoreId = 1,
                AmountSpent = "23.45",
                NormalPoints = "12",
                BonusPoints = "10",
                TotalPoints = "22",
                TransactionDescription = "Merchant",
                PointIssuePartnerGroupId = "1",
                PointIssuePartnerGroupDesc = "Box"
            };
            transactions.Add(transaction);

            customerTransactions.Offer = offer;
            customerTransactions.Transactions = transactions;

            MCARequest mcaRequestCustomerTransaction = new MCARequest();
            MCAResponse mcaResponseCustomerTransaction = new MCAResponse();
            mcaResponseCustomerTransaction.Data = customerTransactions;
            mcaResponseCustomerTransaction.Status = true;
            _clubcardServiceAdapter.Stub(x => x.Get<CustomerTransactions>(mcaRequestCustomerTransaction)).IgnoreArguments().Return(mcaResponseCustomerTransaction);

            var customerTrans= pointsBC.GetCustomerTransactions(6546,211,true, "en-GB");
            Assert.AreEqual(2, customerTrans.Transactions.Count);
        }

        [TearDown]
        public void TestCleanup()
        {
            _clubcardServiceAdapter = null;
            _smartVoucherServiceAdapter = null;
            _preferenceService = null;
            _configurationProvider = null;
            pointsBC = null;
        }
    }
}
