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
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using System.Collections.Specialized;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using PreferenceService = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;

namespace Web.Business.Tests
{
	[TestFixture]
	public class VoucherBCTests
	{
		protected IServiceAdapter _clubcardServiceAdapter;
		private IServiceAdapter _customerServiceAdapter;
		private IServiceAdapter _smartVoucherServiceAdapter;
		private IServiceAdapter _preferenceServiceAdapter;
		private VoucherBC voucherBC;
        private ILoggingService _logger;
        private IAccountBC _accountProvider;
		private IClubcardService _clubCardServiceClient;
		private ISmartVoucherServices _smartVoucherServiceClient;
		private PreferenceService.IPreferenceService _preferenceServiceClient;
		private ICustomerService _customerServiceClient;
		private IPDFGenerator _generatePDF;
        private IConfigurationProvider _configProvider = null;

		[SetUp]
		public void SetUp()
		{
			_logger = MockRepository.GenerateMock<ILoggingService>();
			_generatePDF = MockRepository.GenerateMock<IPDFGenerator>();
            _configProvider = MockRepository.GenerateMock<Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.IConfigurationProvider>();
            _accountProvider = MockRepository.GenerateMock<IAccountBC>();
            //_accountProvider = new AccountBC(_customerServiceAdapter, _clubcardServiceAdapter, _configProvider, _logger);

			_clubCardServiceClient = MockRepository.GenerateMock<IClubcardService>();
            _clubcardServiceAdapter = MockRepository.GenerateMock<ClubcardServiceAdapter>(_clubCardServiceClient, _logger, _configProvider);

			_customerServiceClient = MockRepository.GenerateMock<ICustomerService>();
            _customerServiceAdapter = MockRepository.GenerateMock<CustomerServiceAdapter>(_customerServiceClient, _logger);

			_smartVoucherServiceClient = MockRepository.GenerateMock<ISmartVoucherServices>();
            _smartVoucherServiceAdapter = MockRepository.GenerateMock<SmartVoucherServiceAdapter>(_smartVoucherServiceClient, _logger);

			_preferenceServiceClient = MockRepository.GenerateMock<PreferenceService.IPreferenceService>();
            _preferenceServiceAdapter = MockRepository.GenerateMock<PreferenceServiceAdapter>(_preferenceServiceClient, _logger);

			_configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.DisplayDateFormat)).Return("dd-MM-yyyy");
			_configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.FontName)).Return("verdana");            
			_configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.Culture)).Return("en-GB");

			voucherBC = new VoucherBC(  _clubcardServiceAdapter,
										_customerServiceAdapter, 
										_smartVoucherServiceAdapter, 
										_preferenceServiceAdapter,
                                        _generatePDF, _logger, _configProvider, _accountProvider);
		}

		[TestCase]
		public void GetVoucherViewDetails_Execution_Succesful()
        {
            VouchersViewModel vouchersViewModelExpected = this.GetVoucherViewData();

            VoucherChristmasSaverSummaryModel xmasModel = new VoucherChristmasSaverSummaryModel
            {
                christmasSaverSummary = null,
                isXmasClubMember = false
            };

            vouchersViewModelExpected.voucherChristmasSaverSummaryModel.isXmasClubMember = xmasModel.isXmasClubMember;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 11,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);
            
            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                    vouchersViewModelExpected.voucherRewardDetailsOverallSummaryModel.Equals(vouchersViewModelActual.voucherRewardDetailsOverallSummaryModel)
                    && vouchersViewModelExpected.vouchersUnUsedModel.Equals(vouchersViewModelActual.vouchersUnUsedModel)
                    && vouchersViewModelExpected.voucherChristmasSaverSummaryModel.isXmasClubMember.Equals(vouchersViewModelActual.voucherChristmasSaverSummaryModel.isXmasClubMember)
                    && isVoucherUsageSummaryEqual
                    );
        }

        [TestCase]
        public void GetVoucherViewdetails_PreferenceAIRMilesStandard_Execution_Successful()
        {
            VouchersViewModel vouchersViewModelExpected = new VouchersViewModel();// = this.GetVoucherViewData();

            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel
            {
                optedForMiles = "AIRMILES_STD",
                milesRate = "600 Avios ",
                summaryFormattedVoucherValue = "",
                totalRewardPoints = "0"
            };

            vouchersViewModelExpected.voucherRewardsMilesModel = voucherRewardsMilesModel;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 11,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);


            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                     vouchersViewModelExpected.voucherRewardsMilesModel.Equals(vouchersViewModelActual.voucherRewardsMilesModel)
                    );
        }

        [TestCase]
        public void GetVoucherViewdetails_PreferenceAIRMilesPremium_Execution_Successful()
        {
            VouchersViewModel vouchersViewModelExpected = this.GetVoucherViewData();

            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel
            {
                optedForMiles = "AIRMILES_PREMIUM",
                milesRate = "800 Avios ",
                summaryFormattedVoucherValue = "",
                totalRewardPoints = "0"
            };

            List<ChristmasSaverSummary> summaryList = GetChristmasSaverSummaryData();

            VoucherChristmasSaverSummaryModel xmasModel = new VoucherChristmasSaverSummaryModel
            {
                christmasSaverSummary = null,//summaryList,
                isXmasClubMember = false
            };

            vouchersViewModelExpected.voucherChristmasSaverSummaryModel.isXmasClubMember = xmasModel.isXmasClubMember;
            vouchersViewModelExpected.voucherRewardsMilesModel = voucherRewardsMilesModel;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            int rowCount = 1;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 12,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);


            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();
            rowCount = 1;

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                    vouchersViewModelExpected.voucherRewardDetailsOverallSummaryModel.Equals(vouchersViewModelActual.voucherRewardDetailsOverallSummaryModel) 
                    && vouchersViewModelExpected.vouchersUnUsedModel.Equals(vouchersViewModelActual.vouchersUnUsedModel)
                    && vouchersViewModelExpected.voucherRewardsMilesModel.Equals(vouchersViewModelActual.voucherRewardsMilesModel)
                    && vouchersViewModelExpected.voucherChristmasSaverSummaryModel.isXmasClubMember.Equals(vouchersViewModelActual.voucherChristmasSaverSummaryModel.isXmasClubMember)
                    && isVoucherUsageSummaryEqual
                    );
        }

        [TestCase]
        public void GetVoucherViewdetails_PreferenceBAmilesStandard_Execution_Successful()
        {
            VouchersViewModel vouchersViewModelExpected = new VouchersViewModel();

            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel
            {
                optedForMiles = "BAMILES_STD",
                milesRate = "600 Avios ",
                summaryFormattedVoucherValue = "",
                totalRewardPoints = "0"
            };

            vouchersViewModelExpected.voucherRewardsMilesModel = voucherRewardsMilesModel;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 10,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);


            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                     vouchersViewModelExpected.voucherRewardsMilesModel.Equals(vouchersViewModelActual.voucherRewardsMilesModel)
                    );
        }

		[TestCase]
		public void GetVoucherViewdetails_PreferenceBAmilesPremium_Execution_Successful()
		{
            VouchersViewModel vouchersViewModelExpected = new VouchersViewModel();

            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel
            {
                optedForMiles = "BAMILES_PREMIUM",
                milesRate = "800 Avios ",
                summaryFormattedVoucherValue = "",
                totalRewardPoints = "0"
            };

            vouchersViewModelExpected.voucherRewardsMilesModel = voucherRewardsMilesModel;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 14,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);


            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                     vouchersViewModelExpected.voucherRewardsMilesModel.Equals(vouchersViewModelActual.voucherRewardsMilesModel)
                    );
		}

        [TestCase]
        public void GetVoucherViewdetails_PreferenceVirgin_Execution_Successful()
        {            
            VouchersViewModel vouchersViewModelExpected = new VouchersViewModel();

            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel
            {
                optedForMiles = "VIRGIN",
                milesRate = "625 Flying Club miles ",
                summaryFormattedVoucherValue = "",
                totalRewardPoints = "0"
            };

            vouchersViewModelExpected.voucherRewardsMilesModel = voucherRewardsMilesModel;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 17,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);


            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            errorXml = string.Empty;
            resultXml = String.Empty;
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");

            bool isVoucherUsageSummaryEqual = false;
            for (int i = 0; i < vouchersViewModelExpected.voucherUsageSummary.Count; i++)
            {
                isVoucherUsageSummaryEqual = vouchersViewModelExpected.voucherUsageSummary[i].Equals(vouchersViewModelActual.voucherUsageSummary[i]);
            }

            Assert.IsTrue(
                     vouchersViewModelExpected.voucherRewardsMilesModel.Equals(vouchersViewModelActual.voucherRewardsMilesModel)
                    );
        }

        [TestCase]
        public void GetVoucherViewDetails_ChristmasSaver_Succesful()
        {
            VouchersViewModel vouchersViewModelExpected = this.GetVoucherViewData();

            List<ChristmasSaverSummary> summaryList = GetChristmasSaverSummaryData();

            VoucherChristmasSaverSummaryModel xmasModel = new VoucherChristmasSaverSummaryModel
            {
                christmasSaverSummary = summaryList,
                isXmasClubMember = true
            };

            vouchersViewModelExpected.voucherChristmasSaverSummaryModel = xmasModel;
            vouchersViewModelExpected.voucherChristmasSaverSummaryModel.isXmasClubMember = xmasModel.isXmasClubMember;
            vouchersViewModelExpected.vouchersExpired = false;
            vouchersViewModelExpected._isDotcomCustomerIDEmpty = true;

            var accountDetails = new AccountDetails
            {
                CustomerID = 37831590
            };

            string resultXml = this.GetDummyAccountDetailsResultXml();
            string errorXml = String.Empty;

            _clubCardServiceClient.Stub(x => x.GetMyAccountDetails(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            GetRewardDtlsRsp getRewardDtlsResponse = this.GetDummyRewardDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(getRewardDtlsResponse);

            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsResponse = this.GetDummyUnusedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUnusedVoucherDtls("1")).IgnoreArguments().Return(getUnusedVoucherDtlsResponse);

            PreferenceService.CustomerPreference preference = new PreferenceService.CustomerPreference();
            var preferences = new PreferenceService.CustomerPreference
            {
                PreferenceID = 11,
                POptStatus = PreferenceService.OptStatus.OPTED_IN
            };
            PreferenceService.CustomerPreference[] customerPreferences = new PreferenceService.CustomerPreference[1];
            customerPreferences[0] = preferences;
            preference.Preference = customerPreferences;
            MCARequest mcaRequestPref = new MCARequest();
            MCAResponse mcaResponsetPref = new MCAResponse();
            mcaResponsetPref.Data = preference;
            mcaResponsetPref.Status = true;
            _preferenceServiceClient.Stub(x => x.ViewCustomerPreference(1, PreferenceService.PreferenceType.NULL, true)).IgnoreArguments().Return(preference);

            GetRewardDtlsMilesRsp getRewardDtlsMilesRsp = this.GetDummyRewardMilesDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtlsMiles("1", 2)).IgnoreArguments().Return(getRewardDtlsMilesRsp);

            errorXml = string.Empty;
            resultXml = this.GetDummyIsXmasClubMemberData();

            _clubCardServiceClient.Stub(x => x.IsXmasClubMember(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);

            errorXml = string.Empty;
            resultXml = this.GetDummyChristmasSaverXml();
            int rowCount = 1;
            _clubCardServiceClient.Stub(x => x.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, "1", 2, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);
            
            GetUsedVoucherDtlsRsp getUsedVoucherDtlsResponse = this.GetDummyUsedVoucherDetailsResponse();
            _smartVoucherServiceClient.Stub(x => x.GetUsedVoucherDtls("1")).IgnoreArguments().Return(getUsedVoucherDtlsResponse);

            DbConfiguration dbConfigs = getDbConfigurations();
            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbConfigs);

            var vouchersViewModelActual = voucherBC.GetVoucherViewDetails(1, "en-GB");
            
            Assert.IsTrue(
                    vouchersViewModelExpected.voucherChristmasSaverSummaryModel.Equals(vouchersViewModelActual.voucherChristmasSaverSummaryModel)
                    );
        }

		[TestCase]
		[ExpectedException]
		public void GetVoucherViewDetails_Execution_Exception()
		{
			//List<VoucherRewardDetails> voucherRewardDetailsList = null;
			//_smartVoucherServiceAdapter.Stub(x => x.GetVoucherRewardDetails(1)).IgnoreArguments().Return(voucherRewardDetailsList);
			var voucherRewardDetailsOverallSummaryModel = voucherBC.GetVoucherViewDetails(1, "en-GB");

		}

		[TearDown]
		public void TestCleanup()
		{
			_clubcardServiceAdapter = null;
			_customerServiceAdapter = null;
			_smartVoucherServiceAdapter = null;
			voucherBC = null;
		}

        private VouchersViewModel GetVoucherViewData()
        {
            List<VoucherRewardDetails> voucherRewardDetailsList = this.GetVoucherRewardData();
            VoucherRewardDetailsOverallSummaryModel voucherRewardDetailsOverallSummaries = new VoucherRewardDetailsOverallSummaryModel(voucherRewardDetailsList);

            List<VoucherUsageSummary> usageSummary = this.GetUsedVoucherData();

            VouchersUnUsedModel vouchersUnused = new VouchersUnUsedModel();
            vouchersUnused.voucherList = this.GetUnusedVoucherData();
            vouchersUnused.totalUnusedVouchers = 2;
            var vouchersViewModelExpected = new VouchersViewModel
            {
                voucherRewardDetailsOverallSummaryModel = voucherRewardDetailsOverallSummaries,
                vouchersUnUsedModel = vouchersUnused,
                voucherUsageSummary = usageSummary
            };
            return vouchersViewModelExpected;
        }

		private List<VoucherRewardDetails> GetVoucherRewardData()
		{
			List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
			var firstRewardList = new VoucherRewardDetails
			{
				RewardIssued = 2,
				RewardLeftOver = 2,
				RewardUsed = 0
			};
			voucherRewardDetailsList.Add(firstRewardList);
			return voucherRewardDetailsList;
		}

		private List<VoucherUsageSummary> GetUsedVoucherData()
		{
			List<VoucherUsageSummary> usageSummary = new List<VoucherUsageSummary>();
			VoucherUsageSummary voucherdata = new VoucherUsageSummary();
			VoucherUsageSummary voucherdata1 = new VoucherUsageSummary();
			voucherdata.AdditionalInfo = "1 of 1";
            voucherdata.HouseholdId = 0;
			voucherdata.PeriodName = "January-2016";
            voucherdata.Value = 15;
			voucherdata.WhenUsed = "22/09/2015";
			voucherdata.WhereUsed = "TESTING";

			usageSummary.Add(voucherdata);
            voucherdata1.AdditionalInfo = "No info";
            voucherdata.HouseholdId = 0;
			voucherdata1.PeriodName = "January-2018";
			voucherdata1.Value = 24;
			voucherdata1.WhenUsed = "22/09/2000";
			voucherdata1.WhereUsed = "TESTING";

			usageSummary.Add(voucherdata1);
			return usageSummary;
		}

		private List<VoucherDetails> GetUnusedVoucherData()
		{
			List<VoucherDetails> voucherDetails = new List<VoucherDetails>();
			string[] formats = {"dd/MM/yyyy"};
			var voucherList = new VoucherDetails
			{
				AlphaCode = "ABK764D6NPPP",
				AssociateCustomerCardnumber	= 0,
				AssociateCustomerFirstname = null,
				AssociateCustomerLastname =	null,
				AssociateCustomerMiddlename	= null,
				BarCode	= "9611014002947521884257",
				ExpiryDate = DateTime.ParseExact("31/12/2017", formats, new CultureInfo("en-GB"), DateTimeStyles.None),
				HouseholdId = "2157",
				PeriodName = "May-2015",
				PrimaryCustomerCardnumber = 0,
				PrimaryCustomerFirstname = null,
				PrimaryCustomerLastname = null,
				PrimaryCustomerMiddlename = null,
				Selected = false,
				Value = "0.50",
				VoucherNumber = "947521884257",
				VoucherNumberToPrint = "9611014002947521884257",
				VoucherType = "1"
			};
			voucherDetails.Add(voucherList);

			voucherList = new VoucherDetails
			{
				AlphaCode = "AWK69HBC9PPP",
				AssociateCustomerCardnumber = 0,
				AssociateCustomerFirstname = null,
				AssociateCustomerLastname = null,
				AssociateCustomerMiddlename = null,
				BarCode = "9611014004947521884258",
				ExpiryDate = DateTime.ParseExact("31/05/2017", formats, new CultureInfo("en-GB"), DateTimeStyles.None),
				HouseholdId = "2157",
				PeriodName = "May-2015",
				PrimaryCustomerCardnumber = 0,
				PrimaryCustomerFirstname = null,
				PrimaryCustomerLastname = null,
				PrimaryCustomerMiddlename = null,
				Selected = false,
				Value = "1.50",
				VoucherNumber = "947521884258",
				VoucherNumberToPrint = "9611014004947521884258",
				VoucherType = "1"
			};
			voucherDetails.Add(voucherList);
			return voucherDetails;
		}

        private List<ChristmasSaverSummary> GetChristmasSaverSummaryData() {
            
            ChristmasSaverSummary summary1 = new ChristmasSaverSummary
            {                
                TransactionDateTime = new DateTime(2013, 11, 08),
                AmountSpent = "20.00"
            };

            ChristmasSaverSummary summary2 = new ChristmasSaverSummary
            {
                TransactionDateTime = new DateTime(2013, 11, 08),
                AmountSpent = "20.00"
            };

            ChristmasSaverSummary summary3 = new ChristmasSaverSummary
            {
                TransactionDateTime = new DateTime(2013, 11, 08),
                AmountSpent = "20.00"
            };

            ChristmasSaverSummary summary4 = new ChristmasSaverSummary
            {
                TransactionDateTime = new DateTime(2013, 11, 08),
                AmountSpent = "20.00"
            };

            List<ChristmasSaverSummary> summaryList = new List<ChristmasSaverSummary>();
            summaryList.Add(summary1);
            summaryList.Add(summary2);
            summaryList.Add(summary3);
            summaryList.Add(summary4);
            return summaryList;
        }

        private DbConfiguration getDbConfigurations()
        {
            DbConfiguration dbConfiguration = new DbConfiguration();
            DbConfigurationItem dbConfigItem = new DbConfigurationItem();
            dbConfigItem.ConfigurationName = DbConfigurationItemNames.XmasSaverCurrDates;
            dbConfigItem.ConfigurationValue1 = "Jan 15 2016 12:00AM";
            dbConfigItem.ConfigurationValue2 = "Oct 26 2016 11:59PM";
            dbConfigItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;

            DbConfigurationItem dbConfigItem1 = new DbConfigurationItem();
            dbConfigItem1.ConfigurationName = DbConfigurationItemNames.XmasSaverNextDates;
            dbConfigItem1.ConfigurationValue1 = "Oct 28 2016 12:00AM";
            dbConfigItem1.ConfigurationValue2 = "Oct 27 2017 11:59PM";
            dbConfigItem1.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;

            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigItem);
            lstDbConfigurationItems.Add(dbConfigItem1);
            dbConfiguration.ConfigurationItems = lstDbConfigurationItems;

            return dbConfiguration;
        }

        private string GetDummyAccountDetailsResultXml()
        {
            return "<NewDataSet>"
                    + "<ViewMyAccountDetails>"
                    + "<TitleEnglish>Mr</TitleEnglish>"
                    + "<Name3>Testing</Name3>"
                    + "<Name1>Test</Name1>"
                    + "<PointsBalanceQty>2852</PointsBalanceQty>"
                    + "<Vouchers>28.5</Vouchers>"
                    + "<CustomerID>37831590</CustomerID>"
                    + "<PrimaryCustName>Mr T Testing</PrimaryCustName>"
                    + "<ClubcardID>634004024007890751</ClubcardID>"
                    + "<PrimaryCustName1>Test</PrimaryCustName1>"
                    + "<PrimaryCustName2>Sa</PrimaryCustName2>"
                    + "<PrimaryCustName3>Testing</PrimaryCustName3>"
                    + "<PrimaryClubcardID>634004024007890751</PrimaryClubcardID>"
                    + "<AssociateCustName1>Jason</AssociateCustName1>"
                    + "<AssociateCustName2 />"
                    + "<AssociateCustName3>Alex</AssociateCustName3>"
                    + "<AssociateClubcardID>634002400019724265</AssociateClubcardID>"
                    + "</ViewMyAccountDetails>"
                    + "</NewDataSet>";
        }

		private GetRewardDtlsRsp GetDummyRewardDetailsResponse()
		{
			GetRewardDtlsRsp getRewardDtlsResponse = new GetRewardDtlsRsp();

			DataSet ds = new DataSet();
			DataTable dt = new DataTable("Table");
			DataColumn dc = new DataColumn(VoucherRewardDetailsEnum.RewardIssued);
			dt.Columns.Add(dc);
			dc = new DataColumn(VoucherRewardDetailsEnum.RewardLeftOver);
			dt.Columns.Add(dc);
			dc = new DataColumn(VoucherRewardDetailsEnum.RewardUsed);
			dt.Columns.Add(dc);

			DataRow dr = dt.NewRow();
			dr.ItemArray = new object[] { 2, 2, 0 };

			dt.Rows.Add(dr);

			ds.Tables.Add(dt);

			getRewardDtlsResponse.dsResponse = ds; 
			return getRewardDtlsResponse;
		}
               
		private GetUnusedVoucherDtlsRsp GetDummyUnusedVoucherDetailsResponse()
        {
            GetUnusedVoucherDtlsRsp getUnusedVoucherDtlsRsp = new GetUnusedVoucherDtlsRsp();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Table");
            string[] formats = { "dd/MM/yyyy" };

            DataColumn dc = new DataColumn(VoucherDetailsProperties.HouseholdId);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.PeriodName);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.AlphaCode);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.ExpiryDate);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.Value);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.VoucherNumber);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.VoucherType);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.VoucherNumberToPrint);
            dt.Columns.Add(dc); 
            dc = new DataColumn(VoucherDetailsProperties.PrimaryCustomerFirstname);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.PrimaryCustomerMiddlename);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.PrimaryCustomerLastname);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.PrimaryCustomerCardnumber);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.AssociateCustomerFirstname);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.AssociateCustomerMiddlename);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.AssociateCustomerLastname);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.AssociateCustomerCardnumber);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.BarCode);
            dt.Columns.Add(dc);
            dc = new DataColumn(VoucherDetailsProperties.Selected);
            //dc = new DataColumn(VoucherDetailsProperties.TwentyTwoDigitVoucher_Number);
            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { "2157", "May-2015", "ABK764D6NPPP", DateTime.ParseExact("31/12/2017", formats, new CultureInfo("en-GB"), DateTimeStyles.None), "0.50", "947521884257", "1", "9611014002947521884257", null, null, null, 0, null, null, null, 0, "9611014002947521884257", false };
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr.ItemArray = new object[] { "2157", "May-2015", "AWK69HBC9PPP", DateTime.ParseExact("31/05/2017", formats, new CultureInfo("en-GB"), DateTimeStyles.None), "1.50", "947521884258", "1", "9611014004947521884258", null, null, null, 0, null, null, null, 0, "9611014004947521884258", false };
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            getUnusedVoucherDtlsRsp.dsResponse = ds;
            return getUnusedVoucherDtlsRsp;
        }

		private GetUsedVoucherDtlsRsp GetDummyUsedVoucherDetailsResponse()
		{
			GetUsedVoucherDtlsRsp responseUsed = new GetUsedVoucherDtlsRsp();
			DataSet dsUsed = new DataSet();
			DataTable dtUsed = new DataTable("Table");
			DataColumn dcUsed = null;

            dcUsed = new DataColumn(VoucherUsageSummaryEnum.AdditionalInfo);
            dtUsed.Columns.Add(dcUsed);
            dcUsed = new DataColumn(VoucherUsageSummaryEnum.HouseholdId);
            dtUsed.Columns.Add(dcUsed);
            dcUsed = new DataColumn(VoucherUsageSummaryEnum.PeriodName);
			dtUsed.Columns.Add(dcUsed);
            dcUsed = new DataColumn(VoucherUsageSummaryEnum.Value);
			dtUsed.Columns.Add(dcUsed);
            dcUsed = new DataColumn(VoucherUsageSummaryEnum.WhenUsed);
			dtUsed.Columns.Add(dcUsed);
            dcUsed = new DataColumn(VoucherUsageSummaryEnum.WhereUsed);
			dtUsed.Columns.Add(dcUsed);

			DataRow drUsed = dtUsed.NewRow();
            drUsed.ItemArray = new object[] { "1 of 1", 0, "January-2016", 15, "22/09/2015", "TESTING" };
            dtUsed.Rows.Add(drUsed);

            drUsed = dtUsed.NewRow();
            drUsed.ItemArray = new object[] { "No info", 0, "January-2018", 24, "22/09/2000", "TESTING" };
			dtUsed.Rows.Add(drUsed);

			dsUsed.Tables.Add(dtUsed);
			responseUsed.dsResponse = dsUsed;
			return responseUsed;
		}

        private GetRewardDtlsMilesRsp GetDummyRewardMilesDetailsResponse()
		{
            GetRewardDtlsMilesRsp responseReward = new GetRewardDtlsMilesRsp();
			DataSet dsReward = new DataSet();
            DataTable dtReward = new DataTable("Table");
            DataColumn dcReward = null;

            dcReward = new DataColumn(MilesRewardDetailsEnum.Household_ID);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Period_Name);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Reward_Points);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Reward_Issued);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Reward_Used);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Reason_Code);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.Reward_Left_Over);
            dtReward.Columns.Add(dcReward);
            dcReward = new DataColumn(MilesRewardDetailsEnum.ColPeriodNo);
            dtReward.Columns.Add(dcReward);

            DataRow drReward = dtReward.NewRow();
            drReward.ItemArray = new object[] { "1", "2", 12, 10, 4, "5", "6", "7" };
            dtReward.Rows.Add(drReward);
            dsReward.Tables.Add(dtReward);
            responseReward.dsResponse = dsReward;
            return responseReward;
        }

        private string GetDummyIsXmasClubMemberData()
        {
            return "<NewDataSet>"
                    + "<ViewIsXmasSaverMember>"
                    + "<Column1>1</Column1>"
                    + "</ViewIsXmasSaverMember>"
                    + "</NewDataSet>";
        }

        private string GetDummyChristmasSaverXml()
        {
            return "<NewDataSet>"
                    + "<ViewIsXmasSaverSummary>"
                        + "<ClubCardID>6340049003867804</ClubCardID>"
                        + "<TransactionType>4</TransactionType>"
                        + "<TransactionDateTime>08 Nov 2013</TransactionDateTime>"
                        + "<AmountSpent>20.00</AmountSpent>"
                    + "</ViewIsXmasSaverSummary>"
                    + "<ViewIsXmasSaverSummary>"
                        + "<ClubCardID>6340049003867804</ClubCardID>"
                        + "<TransactionType>4</TransactionType>"
                        + "<TransactionDateTime>08 Nov 2013</TransactionDateTime>"
                        + "<AmountSpent>20.00</AmountSpent>"
                    + "</ViewIsXmasSaverSummary>"
                    + "<ViewIsXmasSaverSummary>"
                        + "<ClubCardID>6340049003867804</ClubCardID>"
                        + "<TransactionType>4</TransactionType>"
                        + "<TransactionDateTime>08 Nov 2013</TransactionDateTime>"
                        + "<AmountSpent>20.00</AmountSpent>"
                    + "</ViewIsXmasSaverSummary>"
                    + "<ViewIsXmasSaverSummary>"
                        + "<ClubCardID>6340049003867804</ClubCardID>"
                        + "<TransactionType>4</TransactionType>"
                        + "<TransactionDateTime>08 Nov 2013</TransactionDateTime>"
                        + "<AmountSpent>20.00</AmountSpent>"
                    + "</ViewIsXmasSaverSummary>"
                    + "</NewDataSet>";
        }
	}
}
