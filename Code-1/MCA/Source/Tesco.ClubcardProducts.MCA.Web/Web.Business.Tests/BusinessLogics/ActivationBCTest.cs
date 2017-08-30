using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Activation=Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using ActivationService = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;

namespace Web.Business.Tests.BusinessLogics
{
    [TestFixture]
   public class ActivationBCTest
    {
        ILoggingService _logger;   
        ActivationBC _activationBC;
        CustomerActivationServiceAdapter _customerServiceActivationAdapter;
        IClubcardOnlineService _customerServiceActivationClient;
        private Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.IConfigurationProvider _configProvider = null;

        MCARequest request = new MCARequest();
        MCAResponse response = new MCAResponse();

        [SetUp]
        public void SetUp()
        {
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _logger = MockRepository.GenerateMock<ILoggingService>();

            _customerServiceActivationClient = MockRepository.GenerateMock<IClubcardOnlineService>();
            _customerServiceActivationAdapter = new CustomerActivationServiceAdapter(_customerServiceActivationClient, _logger);
            _activationBC = new ActivationBC(_customerServiceActivationAdapter, _logger, _configProvider);
        }

        [TestCase]
        public void ProcessActivationRequest_ErrorMessage()
        {
            var expectedRes = ActivationRequestStatusEnum.ErrorMessage;
            long clubcardNumber=634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ErrorMessage = "There is some error";
            string dotcomCustomerID ="23234234";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);

            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }
        [TestCase]
        public void ProcessActivationRequest_ClubcardDetailsDoesntMatch()
        {
            var expectedRes = ActivationRequestStatusEnum.ClubcardDetailsDoesntMatch;
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "N";
            string dotcomCustomerID = "23234234";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);

            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }

        [TestCase]
        public void ProcessActivationRequest_ActivationSuccessful()
        {
            var expectedRes = ActivationRequestStatusEnum.ActivationSuccessful;
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "Y";
            accountFindResponse.Matched = true;
            string dotcomCustomerID = "23234234";
            ActivationService.AccountLinkResponse registerResponse = new AccountLinkResponse();
            registerResponse.ErrorMessage = string.Empty;

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);
            _customerServiceActivationClient.Stub(x => x.IGHSAccountLink(dotcomCustomerID, clubcardNumber)).IgnoreArguments().Return(registerResponse);


            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }
        [TestCase]
        public void ProcessActivationRequest_DuplicateDotcomID()
        {
            var expectedRes = ActivationRequestStatusEnum.DuplicateDotcomID;
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "Y";
            accountFindResponse.Matched = true;
            string dotcomCustomerID = "23234234";
            ActivationService.AccountLinkResponse registerResponse = new AccountLinkResponse();
            registerResponse.ErrorMessage = "DuplicateDotcomID";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);
            _customerServiceActivationClient.Stub(x => x.IGHSAccountLink(dotcomCustomerID, clubcardNumber)).IgnoreArguments().Return(registerResponse);


            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }
        [TestCase]
        public void ProcessActivationRequest_CustomerIDalready()
        {
            var expectedRes = ActivationRequestStatusEnum.CustomerIDalready;
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "Y";
            accountFindResponse.Matched = true;
            string dotcomCustomerID = "23234234";
            ActivationService.AccountLinkResponse registerResponse = new AccountLinkResponse();
            registerResponse.ErrorMessage = "CustomerID already";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);
            _customerServiceActivationClient.Stub(x => x.IGHSAccountLink(dotcomCustomerID, clubcardNumber)).IgnoreArguments().Return(registerResponse);


            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }
        [TestCase]
        public void ProcessActivationRequest_ActivationFailed()
        {
            var expectedRes = ActivationRequestStatusEnum.ActivationFailed;
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "Y";
            accountFindResponse.Matched = true;
            string dotcomCustomerID = "23234234";
            ActivationService.AccountLinkResponse registerResponse = new AccountLinkResponse();
            registerResponse.ErrorMessage = "Activation Failed";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Return(accountFindResponse);
            _customerServiceActivationClient.Stub(x => x.IGHSAccountLink(dotcomCustomerID, clubcardNumber)).IgnoreArguments().Return(registerResponse);


            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            Assert.AreEqual(expectedRes, ActualRes);

        }

        [TestCase]
        [ExpectedException]
        public void ProcessActivationRequest_Exception()
        {
          
            long clubcardNumber = 634004022010553358;
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();
            accountFindResponse.ContactDetailMatchStatus = "N";
            string dotcomCustomerID = "23234234";

            Activation.ClubcardCustomer clubcardCustomer = GetCustomerEntity();
            ActivationService.ClubcardCustomer CCService = GetCustomerServiceEntity();
            DataSet dsConfiguration = GetDBConfiguration();
            DBConfigurations dbConfigs = GetDBconfigurationEntity();

            _configProvider.Stub(x => x.GetConfigurations(DbConfigurationTypeEnum.Activation)).IgnoreArguments().Return(dbConfigs);
            _customerServiceActivationClient.Stub(x => x.AccountFindByClubcardNumber(clubcardNumber, CCService, dsConfiguration)).IgnoreArguments().Throw(new Exception());

            var ActualRes = _activationBC.ProcessActivationRequest(dotcomCustomerID, clubcardNumber, clubcardCustomer);
            

        }

        #region Private Methods

        private Activation.ClubcardCustomer GetCustomerEntity()
        {
            Activation.ClubcardCustomer clubcardCustomer = new Activation.ClubcardCustomer();
            clubcardCustomer.Address = new Activation.Address();
            clubcardCustomer.ContactDetail = new Activation.ContactDetail();
            clubcardCustomer.FirstName = "test";
            clubcardCustomer.Surname = "test";
            clubcardCustomer.YearOfBirth = "1990";
            clubcardCustomer.MonthOfBirth = "May";
            clubcardCustomer.DayOfBirth = "20";
            clubcardCustomer.SSN = "test";
            clubcardCustomer.Address.AddressLine1 = "test";
            clubcardCustomer.Address.PostCode = "al7 4fg";
            clubcardCustomer.ContactDetail.MobileContactNumber = "073233432434";
            clubcardCustomer.ContactDetail.EmailAddress = "test@test.com";
            return clubcardCustomer;
        }

        private ActivationService.ClubcardCustomer GetCustomerServiceEntity()
        {
            ActivationService.ClubcardCustomer CCService = new ActivationService.ClubcardCustomer();
            CCService.Address = new ActivationService.Address();
            CCService.ContactDetail = new ActivationService.ContactDetail();
            CCService.FirstName = "test";
            CCService.Surname = "test";
            CCService.YearOfBirth = "1990";
            CCService.MonthOfBirth = "May";
            CCService.DayOfBirth = "20";
            CCService.SSN = "test";
            CCService.Address.AddressLine1 = "test";
            CCService.Address.PostCode = "al7 4fg";
            CCService.ContactDetail.MobileContactNumber = "073233432434";
            CCService.ContactDetail.EmailAddress = "test@test.com";
            return CCService;

        }

        private DataSet GetDBConfiguration()
        {
            DataSet dsConfiguration = new DataSet();
            DataTable dtConfigurationItems = new DataTable("ActiveDateRangeConfig");
            dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationType.ToString());
            dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationName.ToString());
            dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue1.ToString());
            dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue2.ToString());
            string[] ActivationConfig = new string[] { "Name1", "Name3", "MailingAddressLine1", "MobilePhoneNumber", "MailingAddressPostCode" };
            foreach (var c1 in ActivationConfig)
            {
                DataRow dr = dtConfigurationItems.NewRow();
                dr[0] = "20";
                dr[1] = c1;
                dr[2] = "1";
                dr[3] = "";
                dtConfigurationItems.Rows.Add(dr);
            }

            dsConfiguration.Tables.Add(dtConfigurationItems);
            return dsConfiguration;
        }

        private DBConfigurations GetDBconfigurationEntity()
        {
            DBConfigurations dbConfigs = new DBConfigurations();

            string[] ActivationConfig1 = new string[] { "Name1", "Name3", "MailingAddressLine1", "MobilePhoneNumber", "MailingAddressPostCode" };
            foreach (var c1 in ActivationConfig1)
            {
                DbConfigurationItem dbitem = new DbConfigurationItem();

                dbitem.ConfigurationName = c1;
                dbitem.ConfigurationType = DbConfigurationTypeEnum.Activation;
                dbitem.ConfigurationValue1 = "1";
                dbitem.ConfigurationValue2 = "";
                dbConfigs.Instance.Add(c1, dbitem);
            }
            return dbConfigs;
        }

        #endregion
    }
}
