using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardCouponServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Customer = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Web.Business.Tests.BusinessLogics
{
    [TestFixture]
    public class CouponBCTest
    {
        IServiceAdapter _IClubcardCouponServiceAdapter;
        IServiceAdapter _ICustomerServiceAdapter;
        IClubcardCouponService _clubcardCouponServiceClient;
        Customer.ICustomerService _customerServiceClient;
        CustomerServiceAdapter _customerServiceAdapter;
        ClubcardCouponServiceAdapter _clubcardCouponServiceAdapter;
        ILoggingService _logger;
        IConfigurationProvider _config;
        CouponBC couponBC;

        MCARequest request = new MCARequest();
        MCAResponse response = new MCAResponse();
        int totalCoupons = 2;
        Int64 houseHoldId = 0;
        string culture = "en-GB";
        AccountDetails customerAccountDetails = new AccountDetails();

        CouponInformation[] coupons = getAvailableCouponInformation();
        List<CouponDetails> couponDetails = getExpectedAvailableCouponDetails();
        List<CouponDetails> couponDetailsExpected = getExpectedAvailableCouponDetails();

        [SetUp]
        public void SetUp()
        {
            _clubcardCouponServiceClient = MockRepository.GenerateMock<IClubcardCouponService>();
            _customerServiceClient = MockRepository.GenerateMock<Customer.ICustomerService>();
            _IClubcardCouponServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _ICustomerServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _logger = MockRepository.GenerateMock<ILoggingService>();
            _config = MockRepository.GenerateMock<IConfigurationProvider>();

            _customerServiceAdapter = new CustomerServiceAdapter(_customerServiceClient, _logger);
            _clubcardCouponServiceAdapter = new ClubcardCouponServiceAdapter(_clubcardCouponServiceClient, _logger);
            couponBC = new CouponBC(_clubcardCouponServiceAdapter, _customerServiceAdapter, _logger, _config);
        }

        [TestCase]
        public void GetAvailableCoupons_ServiceAdaptor()
        {
            string errorXml = string.Empty;
            GetAvailableCouponsSetup();            
            response = _IClubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);

            //check service adaptor
            couponDetails = response.Data as List<CouponDetails>;
            Assert.IsTrue(couponDetailsExpected.SequenceEqual(couponDetails));
        }

        [TestCase]
        public void GetAvailableCoupons_Business()
        {
            string errorXml = string.Empty;
            GetAvailableCouponsSetup();
            response = _IClubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);           

            // check business class
            couponBC.GetAvailableCoupons(out totalCoupons, houseHoldId);
            Assert.IsTrue(couponDetailsExpected.SequenceEqual(couponDetails));
        }

        [TestCase]
        public void GetIssuedCoupons_Business()
        {
            string errorXml = string.Empty;
            GetAvailableCouponsSetup();
            response = _IClubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);            
            Assert.IsTrue(couponDetails.Count.Equals(2));
        }

        [TestCase]
        public void GetRedeemedCoupons_ServiceAdaptor()
        {
            GetRedeemedCouponsSetup();
            response = _IClubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);

            //check service adaptor
            couponDetails = response.Data as List<CouponDetails>;
            Assert.IsTrue(couponDetailsExpected.SequenceEqual(couponDetails));
        }

        [TestCase]
        public void GetRedeemedCoupons_Business()
        {
            GetRedeemedCouponsSetup();
            Dictionary<string, string> res = new Dictionary<string, string>();
            res["strCouponUsed"] = "coupon used";
            res["strCouponVoided"] = "coupon voided";
            res["strOf"] = "of";
            // check business class
            couponBC.GetRedeemedCoupons(houseHoldId, culture, res);
            Assert.IsTrue(couponDetailsExpected.SequenceEqual(couponDetails));
        }

        [TestCase]
        public void RecordCouponPrintDetails_Positve()
        {
            string errorXml;
            DataSet dsCoupons;
            SetupRecordData(out errorXml, out dsCoupons);
            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsCoupons)).IgnoreArguments().Return(true);
            bool result = couponBC.RecordPrintAtHomeDetails(getExpectedAvailableCouponDetails(), "1", customerAccountDetails);
            Assert.AreEqual(true, result);
        }

        [TestCase]
        public void RecordCouponPrintDetails_Negative()
        {
            string errorXml;
            DataSet dsCoupons;
            SetupRecordData(out errorXml, out dsCoupons);
            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsCoupons)).IgnoreArguments().Return(false);
            bool result = couponBC.RecordPrintAtHomeDetails(getExpectedAvailableCouponDetails(), "1", customerAccountDetails);
            Assert.AreEqual(false, result);
        }

        [TestCase]
        [ExpectedException]
        public void RecordCouponPrintDetails_Exception()
        {
            string errorXml;
            DataSet dsCoupons;
            SetupRecordData(out errorXml, out dsCoupons);
            _customerServiceClient.Stub(x => x.AddPrintAtHomeDetails(out errorXml, dsCoupons)).IgnoreArguments().Throw(new Exception());
            bool result = couponBC.RecordPrintAtHomeDetails(getExpectedAvailableCouponDetails(),"1",customerAccountDetails);
        }

        #region Private Methods

        /// <summary>
        /// Method to get the mock coupons data
        /// </summary>
        /// <returns></returns>
        static CouponInformation[] getAvailableCouponInformation()
        {
            CouponInformation[] coupons = new CouponInformation[2];
            coupons[0] = new CouponInformation
            {
                AlphaCode = "ABCD#$@#1234",
                CouponDescription = "test description",
                FullImageName = "imageName_F.jpg",
                ThumbnailImageName = "imageName_T.jpg",
                HouseholdId = "0",
                RedemptionEndDate = DateTime.Now,
                IssuanceEndDate = DateTime.Now.AddDays(-5)
            };
            coupons[1] = new CouponInformation
            {
                AlphaCode = "XYZ123409324",
                CouponDescription = "test description second",
                FullImageName = "anotherImageName_F.jpg",
                ThumbnailImageName = "imageName_T.jpg",
                HouseholdId = "0",
                RedemptionEndDate = DateTime.Now.AddDays(1),
                IssuanceEndDate = DateTime.Now.AddDays(-10)
            };
            return coupons;
        }

        /// <summary>
        /// Method to get the expected coupon details
        /// </summary>
        /// <returns></returns>
        static List<CouponDetails> getExpectedAvailableCouponDetails()
        {
            List<CouponDetails> coupons = new List<CouponDetails>();
            coupons.Add(new CouponDetails
            {
                AlphaCode = "ABCD#$@#1234",
                CouponDescription = "test description",
                FullImageName = "imageName_F.jpg",
                ThumbnailImageName = "imageName_T.jpg",
                RedemptionEndDate = DateTime.Now,
                ExpiryDate = "04/02/2016 05:29:59"
            });
            coupons.Add(new CouponDetails
            {
                AlphaCode = "XYZ123409324",
                CouponDescription = "test description second",
                FullImageName = "anotherImageName_F.jpg",
                ThumbnailImageName = "imageName_T.jpg",
                RedemptionEndDate = DateTime.Now.AddDays(1),
                ExpiryDate = "04/02/2016 05:29:59"
            });
            return coupons;
        }

        /// <summary>
        /// Method to get the mock xml data for service call GetRedeemedCoupons
        /// </summary>
        /// <returns></returns>
        private string getAvailableCouponXml()
        {
            string couponXml = "<NewDataSet />";
            return couponXml;
        }

        static List<CouponDetails> getExpectedRedeemedCouponDetails()
        {
            List<CouponDetails> coupons = new List<CouponDetails>();
            return coupons;
        }

        static List<CouponDetails> getRedeemedCouponInformation()
        {
            List<CouponDetails> coupons = new List<CouponDetails>();            
            return coupons;
        }

        void GetAvailableCouponsSetup()
        {
            string errorXml = string.Empty;
            CouponInformation[] coupons = getAvailableCouponInformation();            
            // Mock service client method
            _clubcardCouponServiceClient.Stub(x => x.GetAvailableCoupons(out errorXml, out coupons, out totalCoupons, houseHoldId)).IgnoreArguments().Return(true);

            _config.Stub(x => x.GetStringAppSetting(AppConfigEnum.CouponImageFolder)).Return("~/Content/images/couponimages");
            // Mock service adaptor method            
            response.Data = couponDetails;
            if (!request.Parameters.Keys.Contains(ParameterNames.HOUSEHOLD_ID))
            {
                request.Parameters.Add(ParameterNames.HOUSEHOLD_ID, houseHoldId);
            }
            _IClubcardCouponServiceAdapter.Stub(x => x.Get<List<CouponDetails>>(request)).IgnoreArguments().Return(response);
            if (!response.OutParameters.Keys.Contains(ParameterNames.TOTAL_COUPONS_VALUE))
            {
                response.OutParameters.Add(ParameterNames.TOTAL_COUPONS_VALUE, totalCoupons);
            }
        }

        void GetRedeemedCouponsSetup()
        {
            int totalCoupons = 2;
            Int64 houseHoldId = 0;
            string errorXml = string.Empty;
            couponDetails = getRedeemedCouponInformation();
            couponDetailsExpected = getExpectedRedeemedCouponDetails();
            // Mock service client method
            _clubcardCouponServiceClient.Stub(x => x.GetAvailableCoupons(out errorXml, out coupons, out totalCoupons, houseHoldId)).IgnoreArguments().Return(true);

            // Mock service adaptor method            
            response.Data = couponDetails;
            if (!request.Parameters.Keys.Contains(ParameterNames.HOUSEHOLD_ID))
            {
                request.Parameters.Add(ParameterNames.HOUSEHOLD_ID, houseHoldId);
            }
            if (!request.Parameters.Keys.Contains(ParameterNames.CULTURE))
            {
                request.Parameters.Add(ParameterNames.CULTURE, culture);
            }
            _IClubcardCouponServiceAdapter.Stub(x => x.Get<List<CouponDetails>>(request)).IgnoreArguments().Return(response);
            if (!response.OutParameters.Keys.Contains(ParameterNames.TOTAL_COUPONS_VALUE))
            {
                response.OutParameters.Add(ParameterNames.TOTAL_COUPONS_VALUE, totalCoupons);
            }            
        }

        private void SetupRecordData(out string errorXml, out DataSet dsCoupons)
        {
            customerAccountDetails.ClubcardID = 634004024006785259;
            errorXml = string.Empty;
            dsCoupons = new DataSet("DocumentElement");
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
            dr["VoucherID"] = "9925890010122890010303";
            dr["VoucherType"] = string.Empty;
            dr["ExpiryDate"] = "04/02/2016 05:29:59";
            dr["CCNumber"] = "634004024006785259";
            dr["Flag"] = "C";

            dtPrintDetail.Rows.Add(dr);
            dsCoupons.Tables.Add(dtPrintDetail);
            request.Parameters.Remove(ParameterNames.OPERATION_NAME);
            request.Parameters.Remove(ParameterNames.DS_TOKENS);
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
            request.Parameters.Add(ParameterNames.DS_TOKENS, dsCoupons);
        }

        #endregion


    }
}
