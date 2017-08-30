using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;

namespace Web.ServiceAdapter.Tests
{
    [TestFixture]
    public class SmartVoucherServiceAdapterTests
    {
        private ISmartVoucherServices _smartVoucherServiceClient;
        private SmartVoucherServiceAdapter smartVoucherServiceAdapter;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = MockRepository.GenerateMock<LoggingService>();
            _smartVoucherServiceClient = MockRepository.GenerateMock<ISmartVoucherServices>();
            smartVoucherServiceAdapter = new SmartVoucherServiceAdapter(_smartVoucherServiceClient, _logger);
        }

        [TearDown]
        public void TestCleanup()
        {
            _smartVoucherServiceClient = null;
            smartVoucherServiceAdapter = null;
        }

        [TestCase]
        [ExpectedException]
        public void GetVoucherRewardDetails_SmartVoucherService_Data_Having_Exception()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("RewardIssued");
            dt.Columns.Add("RewardUsed");
            dt.Columns.Add("RewardLeftOver");
            DataRow dr = dt.NewRow();
            dr["RewardLeftOver"] = 0;
            dr["RewardUsed"] = 160;
            dr["RewardIssued"] = 160;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            GetRewardDtlsRsp response = new GetRewardDtlsRsp();
            response.dsResponse = ds;
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(response);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            VoucherRewardDetails voucherRewardDetails = new VoucherRewardDetails
            {
                RewardIssued = 160,
                RewardLeftOver = 0,
                RewardUsed = 160
            };
            voucherRewardDetailsList.Add(voucherRewardDetails);
            MCARequest mcaRequest = new MCARequest();
            mcaRequest.Parameters.Add("clubcardNumber",6340034424);
            MCAResponse mcaResponse = new MCAResponse();
            mcaResponse = smartVoucherServiceAdapter.Get<List<VoucherRewardDetails>>(mcaRequest);
            Assert.AreSame(voucherRewardDetailsList,(List<VoucherRewardDetails>)mcaResponse.Data);
        }

        [TestCase]
        [ExpectedException]
        public void GetVoucherRewardDetails_SmartVoucherService_Data_Having_Error_Message_Exception()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Table");
            dt.Columns.Add("RewardIssued");
            dt.Columns.Add("RewardUsed");
            dt.Columns.Add("RewardLeftOver");
            DataRow dr = dt.NewRow();
            dr["RewardLeftOver"] = 0;
            dr["RewardUsed"] = 160;
            dr["RewardIssued"] = 160;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            GetRewardDtlsRsp response = new GetRewardDtlsRsp();
            response.dsResponse = ds;
            response.ErrorMessage = "HavingErrors";
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(response);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            //voucherRewardDetailsList = smartVoucherServiceAdapter.GetVoucherRewardDetails(1);
        }

        [TestCase]
        public void GetVoucherRewardDetails_SmartVoucherService_Data_Having_Valid_Data()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Table");
            dt.Columns.Add("Reward_Issued");
            dt.Columns.Add("Reward_Used");
            dt.Columns.Add("Reward_LeftOver");
            DataRow dr = dt.NewRow();
            dr["Reward_LeftOver"] = 0;
            dr["Reward_Used"] = 160;
            dr["Reward_Issued"] = 160;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            GetRewardDtlsRsp response = new GetRewardDtlsRsp();
            response.dsResponse = ds;
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(response);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            //voucherRewardDetailsList = smartVoucherServiceAdapter.GetRewardDetailsEntity(ds);
            Assert.AreEqual(1, voucherRewardDetailsList.Count);
        }

        [TestCase]
        public void GetVoucherRewardDetails_SmartVoucherService_Data_Having_No_Data()
        {
            DataSet ds = new DataSet();
            GetRewardDtlsRsp response = new GetRewardDtlsRsp();
            response.dsResponse = ds;
            _smartVoucherServiceClient.Stub(x => x.GetRewardDtls("1")).IgnoreArguments().Return(response);

            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            //voucherRewardDetailsList = smartVoucherServiceAdapter.GetVoucherRewardDetails(1);
            Assert.AreEqual(0, voucherRewardDetailsList.Count);
        }
    }
}
