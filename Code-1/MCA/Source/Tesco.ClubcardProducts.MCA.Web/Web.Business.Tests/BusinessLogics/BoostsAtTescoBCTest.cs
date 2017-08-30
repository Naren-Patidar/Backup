using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Rhino.Mocks;
using Customer = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.RewardService;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Data;
namespace Web.Business.Tests.BusinessLogics
{
    [TestFixture]
    public class BoostsAtTescoBCTest
    {
        RewardServiceAdapter _rewardServiceAdapter;
        IPDFGenerator _generatePDF;
        CustomerServiceAdapter _customerServiceAdapter;
        IRewardService _rewardserviceClient;
        Customer.ICustomerService _customerServiceClient;
        private IAccountBC _accountProvider;
        ILoggingService _logger;
        BoostsAtTescoBC _boostBC;

        private Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.IConfigurationProvider _configProvider = null;

        MCARequest request = new MCARequest();
        MCAResponse response = new MCAResponse();

        [SetUp]
        public void SetUp()
        {
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _logger = MockRepository.GenerateMock<ILoggingService>();
            _generatePDF = MockRepository.GenerateMock<IPDFGenerator>();

            _configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.Culture)).Return("en-GB");

            _accountProvider = MockRepository.GenerateMock<IAccountBC>();

            _rewardserviceClient = MockRepository.GenerateMock<IRewardService>();
            _rewardServiceAdapter = new RewardServiceAdapter(_rewardserviceClient, _logger);

            _customerServiceClient = MockRepository.GenerateMock<Customer.ICustomerService>();
            _customerServiceAdapter = new CustomerServiceAdapter(_customerServiceClient, _logger);

            _boostBC = new BoostsAtTescoBC(_accountProvider,_customerServiceAdapter, _rewardServiceAdapter, _generatePDF, _logger, _configProvider);
        }

        [TestCase]
        public void GetRewardAndTokens_Positve_ServiceAdaptor()
        {
            RewardAndToken objExpected = new RewardAndToken();
            List<Reward> rewards = this.GetDummyRewardList();
            List<Token> tokens = this.GetDummyTokenList();

            objExpected.Rewards = rewards;
            objExpected.Tokens = tokens;

            string resultXml = this.GetDummyRewardTokenResultXml();
            string errorXml = String.Empty;

            _rewardserviceClient.Stub(x => x.GetRewardDetail(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(true);           

            var rewardAndTokenActual = _boostBC.GetRewardAndTokens(1);
            Assert.IsTrue(  objExpected.Rewards.SequenceEqual<Reward>(rewardAndTokenActual.Rewards) &&
                            objExpected.Tokens.SequenceEqual<Token>(rewardAndTokenActual.Tokens));
            
        }

        [TestCase]
        public void GetRewardAndTokens_Negative_ServiceAdaptor()
        {
            RewardAndToken objExpected = new RewardAndToken();
            List<Reward> rewards = this.GetDummyRewardList();
            List<Token> tokens = this.GetDummyTokenList();

            objExpected.Rewards = rewards;
            objExpected.Tokens = tokens;

            string resultXml = this.GetDummyRewardTokenResultXml();
            string errorXml = String.Empty;

            _rewardserviceClient.Stub(x => x.GetRewardDetail(out errorXml, out resultXml, 1, "en-GB")).OutRef(errorXml, resultXml).IgnoreArguments().Return(false);

            var rewardAndTokenActual = _boostBC.GetRewardAndTokens(1);
            Assert.IsFalse(objExpected.Rewards.SequenceEqual<Reward>(rewardAndTokenActual.Rewards) &&
                            objExpected.Tokens.SequenceEqual<Token>(rewardAndTokenActual.Tokens));

        }



        [TestCase]
        public void RecordRewardTokenPrintDetails_Positve()
        {
            string errorXml = string.Empty;
            DataSet dsTokens = new DataSet("DocumentElement");
            DataTable dtPrintDetail = new DataTable();
            dtPrintDetail.TableName = "PrintDetails";
            dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
            dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("Value", typeof(Decimal));
            dtPrintDetail.Columns.Add("VoucherID", typeof(string));
            dtPrintDetail.Columns.Add("VoucherType", typeof(string));
            dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
            dtPrintDetail.Columns.Add("Flag", typeof(Char));
            DataRow dr = dtPrintDetail.NewRow();
            dr["CustomerID"] = 64654958;
            dr["PrintDate"] = DateTime.Now;
            dr["Value"] = "0";
            dr["VoucherID"] = "CE4VC-HRBG-KKB9";
            dr["VoucherType"] = string.Empty;
            dr["ExpiryDate"] = "04/02/2016 05:29:59";
            dr["CCNumber"] = 0;
            dr["Flag"] = "T";

            dtPrintDetail.Rows.Add(dr);
            dsTokens.Tables.Add(dtPrintDetail);
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
            request.Parameters.Add(ParameterNames.DS_TOKENS, dsTokens);

            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsTokens)).IgnoreArguments().Return(true);

            bool result = _boostBC.RecordRewardTokenPrintDetails(GetDummyTokenList(), 0, "T");

            Assert.AreEqual(true, result);            
        }

        [TestCase]
        public void RecordRewardTokenPrintDetails_Negative()
        {
            string errorXml = "There is some error";
            DataSet dsTokens = new DataSet("DocumentElement");
            DataTable dtPrintDetail = new DataTable();
            dtPrintDetail.TableName = "PrintDetails";
            dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
            dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("Value", typeof(Decimal));
            dtPrintDetail.Columns.Add("VoucherID", typeof(string));
            dtPrintDetail.Columns.Add("VoucherType", typeof(string));
            dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
            dtPrintDetail.Columns.Add("Flag", typeof(Char));
            DataRow dr = dtPrintDetail.NewRow();
            dr["CustomerID"] = 64654958;
            dr["PrintDate"] = DateTime.Now;
            dr["Value"] = "0";
            dr["VoucherID"] = "CE4VC-HRBG-KKB9";
            dr["VoucherType"] = string.Empty;
            dr["ExpiryDate"] = "04/02/2016 05:29:59";
            dr["CCNumber"] = 0;
            dr["Flag"] = "T";

            dtPrintDetail.Rows.Add(dr);
            dsTokens.Tables.Add(dtPrintDetail);
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
            request.Parameters.Add(ParameterNames.DS_TOKENS, dsTokens);

            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsTokens)).IgnoreArguments().Return(false);

            bool result = _boostBC.RecordRewardTokenPrintDetails(GetDummyTokenList(), 0, "T");

            Assert.AreEqual(false, result);
        }
        [TestCase]
        [ExpectedException]
        public void RecordRewardTokenPrintDetails_Exception()
        {
            string errorXml = "There is some error";
            DataSet dsTokens = new DataSet("DocumentElement");
            DataTable dtPrintDetail = new DataTable();
            dtPrintDetail.TableName = "PrintDetails";
            dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
            dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("Value", typeof(Decimal));
            dtPrintDetail.Columns.Add("VoucherID", typeof(string));
            dtPrintDetail.Columns.Add("VoucherType", typeof(string));
            dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
            dtPrintDetail.Columns.Add("Flag", typeof(Char));
            DataRow dr = dtPrintDetail.NewRow();
            dr["CustomerID"] = 64654958;
            dr["PrintDate"] = DateTime.Now;
            dr["Value"] = "0";
            dr["VoucherID"] = "CE4VC-HRBG-KKB9";
            dr["VoucherType"] = string.Empty;
            dr["ExpiryDate"] = "04/02/2016 05:29:59";
            dr["CCNumber"] = 0;
            dr["Flag"] = "T";

            dtPrintDetail.Rows.Add(dr);
            dsTokens.Tables.Add(dtPrintDetail);
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
            request.Parameters.Add(ParameterNames.DS_TOKENS, dsTokens);

            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsTokens)).IgnoreArguments().Throw(new Exception());

            bool result = _boostBC.RecordRewardTokenPrintDetails(GetDummyTokenList(), 0, "T");

           
        }
        private string GetDummyRewardTokenResultXml()
        {
            return "<NewDataSet><RewardDetails>"+
                "<BookingDate>2015-07-12T00:12:07+01:00</BookingDate>"+
                "<TokenDescription>PHONES &amp; ACCESSORIES TOKEN TO SPEND ONLINE</TokenDescription>" +
                "<ProductStatus>Order Complete</ProductStatus>"+
                "<TokenValue>10.00</TokenValue>"+
                "<SupplierTokenCode>CE9FC-JYRP-QBVR</SupplierTokenCode>"+
                "<ValidUntil>2016-02-03T23:59:59+00:00</ValidUntil>"+
                "</RewardDetails>"+
                "<TokenDetails>"+
                "<BookingDate>2015-07-12T00:12:07+01:00</BookingDate>"+
                "<TokenId>93166974</TokenId>"+
                "<TokenDescription>CAMERAS, AUDIO &amp; SAT NAV TOKEN AT TESCO DIRECT</TokenDescription>"+
                "<ProductStatus>Order Complete</ProductStatus>"+
                "<TokenValue>10</TokenValue>"+
                "<SupplierTokenCode>CE4VC-HRBG-KKB9</SupplierTokenCode>"+
                "<ValidUntil>2016-02-03T23:59:59+00:00</ValidUntil>"+
                "<ProductCode>D0154</ProductCode>"+
                "<ProductTokenValue>10</ProductTokenValue>"+
                "<QualifyingSpend>10</QualifyingSpend>"+
                "<Title>Direct: Cameras, Sat Navs &amp; Audio</Title>"+
                "<Includes>Cameras, Sat Navs &amp; Audio</Includes>"+
                "<Excludes>Excludes: Batteries, Memory, Cables, Sound bars and Home Theatre</Excludes>"
                +"<TermsAndConditions />"
                +"</TokenDetails>"+"</NewDataSet>";
        }

        private List<Reward> GetDummyRewardList()
        {
            List<Reward> lstRewards = new List<Reward>();

            for (int i = 0; i < 1; i++)
            {
                lstRewards.Add(this.GetDummyReward(i, DateTime.Now.AddDays(i)));
            }

            return lstRewards;
        }

        private Reward GetDummyReward(int TokenValue, DateTime dtTemp)
        {
            var reward = new Reward();

            reward.BookingDate = DateTime.Parse("2015-07-12T00:12:07+01:00");
            reward.TokenDescription = "PHONES & ACCESSORIES TOKEN TO SPEND ONLINE";
            reward.ProductStatus = "Order Complete";
            reward.TokenValue = 10;
            reward.SupplierTokenCode = "CE9FC-JYRP-QBVR";
            reward.ValidUntil = DateTime.Parse("2016-02-03T23:59:59+00:00");//dtTemp.ToShortDateString());
            return reward;
        }

        private List<Token> GetDummyTokenList()
        {
            List<Token> lstTokens = new List<Token>();

            for (int i = 0; i < 1; i++)
            {
                lstTokens.Add(this.GetDummyToken(i, DateTime.Now.AddDays(i)));
            }

            return lstTokens;
        }

        private Token GetDummyToken(int TokenValue, DateTime dtTemp)
        {
            var token = new Token();

            token.BookingDate = DateTime.Parse("2015-07-12T00:12:07+01:00");
            token.TokenID = 93166974;
            token.TokenDescription = "CAMERAS, AUDIO & SAT NAV TOKEN AT TESCO DIRECT";
            token.ProductStatus = "Order Complete";
            token.TokenValue = 10;
            token.SupplierTokenCode = "CE4VC-HRBG-KKB9";
            token.ValidUntil = DateTime.Parse("2016-02-03T23:59:59+00:00");
            token.ProductCode = "D0154";
            token.ProductTokenValue = 10;
            token.QualifyingSpend = "10";
            token.Title = "Direct: Cameras, Sat Navs & Audio";
            token.Includes = "Cameras, Sat Navs & Audio";
            token.Excludes = "Excludes: Batteries, Memory, Cables, Sound bars and Home Theatre";
            token.TermsAndConditions = "";

            return token;
        }

    }
}
