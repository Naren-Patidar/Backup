using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.Data;

namespace Web.ServiceAdapter.Tests
{
    [TestFixture]
    public class ClubcardServiceAdapterTests
    {
        private IClubcardService _clubcardServiceClient;
        private ClubcardServiceAdapter clubcardServiceAdapter;

        [SetUp]
        public void SetUp()
        {
            _clubcardServiceClient = MockRepository.GenerateMock<IClubcardService>();
            //clubcardServiceAdapter = new ClubcardServiceAdapter(_clubcardServiceClient);
        }

        [TearDown]
        public void TestCleanup()
        {
            _clubcardServiceClient = null;
            clubcardServiceAdapter = null;
        }

        [TestCase]
        public void GetHouseHoldCustomersDetails_ClubcardService_Return_True_Valid_Data()
        {
              string resultXml = @"<NewDataSet>
                  <HouseholdCustomers>
                    <PrimaryCustomerID>435388</PrimaryCustomerID>
                    <CustomerID>435388</CustomerID>
                    <TitleEnglish>Mr</TitleEnglish>
                    <Name1>Sahitestqweqq</Name1>
                    <Name2>Sa</Name2>
                    <Name3>Alexanderewe</Name3>
                    <MailingAddressLine1>33 CHAMBERS COMPANY GROVE</MailingAddressLine1>
                    <MailingAddressPostCode>AL7 4FG</MailingAddressPostCode>
                    <CustomerUseStatusID>1</CustomerUseStatusID>
                    <CustomerMailStatus>7</CustomerMailStatus>
                  </HouseholdCustomers>
                  <HouseholdCustomers>
                    <PrimaryCustomerID>435388</PrimaryCustomerID>
                    <CustomerID>1095609850</CustomerID>
                    <TitleEnglish>Mr</TitleEnglish>
                    <Name1>Sahitest</Name1>
                    <Name2>Sa</Name2>
                    <Name3>Alexanderesww</Name3>
                    <MailingAddressLine1>33 CHAMBERS COMPANY GROVE</MailingAddressLine1>
                    <MailingAddressPostCode>AL7 4FG</MailingAddressPostCode>
                    <CustomerUseStatusID>1</CustomerUseStatusID>
                    <CustomerMailStatus>7</CustomerMailStatus>
                  </HouseholdCustomers>
                </NewDataSet>";
              string errorXml = "";
            _clubcardServiceClient.Stub(x=> x.GetHouseholdCustomers(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, Arg<long>.Is.Anything,Arg<string>.Is.Anything)).Return(true);
            List<HouseholdCustomerDetails> houseHoldCustomerDetailsList = new List<HouseholdCustomerDetails>();
            //houseHoldCustomerDetailsList = clubcardServiceAdapter.GetHouseHoldCustomers(1, "en-GB");
            Assert.AreEqual(2, houseHoldCustomerDetailsList.Count);
        }

        [TestCase]
        public void GetOffersForCustomerDataset_ClubcardService_Return_True_Valid_Data()
        {
            string resultXml = @"<NewDataSet>
              <PointsInfoAllCollPrds>
                <CustomerID>435388</CustomerID>
                <OfferID>1</OfferID>
                <OfferPeriod>December 2015</OfferPeriod>
                <OfferPeriodMCA>Aralik 2015</OfferPeriodMCA>
                <StartDateTime>Dec  7 2015 10:27AM</StartDateTime>
                <EndDateTime>Dec  7 2015 10:27AM</EndDateTime>
                <PointsBalanceQty>0</PointsBalanceQty>
                <Vouchers>0.00</Vouchers>
              </PointsInfoAllCollPrds>
              <PointsInfoAllCollPrds>
                <CustomerID>435388</CustomerID>
                <OfferID>199</OfferID>
                <OfferPeriod>November 2015</OfferPeriod>
                <OfferPeriodMCA>Kasim 2015</OfferPeriodMCA>
                <StartDateTime>Oct 22 2015 11:59PM</StartDateTime>
                <EndDateTime>Oct 22 2015 11:59PM</EndDateTime>
                <PointsBalanceQty>0</PointsBalanceQty>
                <Vouchers>0.00</Vouchers>
              </PointsInfoAllCollPrds>
              <PointsInfoAllCollPrds>
                <CustomerID>435388</CustomerID>
                <OfferID>300</OfferID>
                <OfferPeriod>November 2015</OfferPeriod>
                <OfferPeriodMCA>Kasim 2015</OfferPeriodMCA>
                <StartDateTime>Oct 22 2015 12:00AM</StartDateTime>
                <EndDateTime>Oct 22 2015 11:59PM</EndDateTime>
                <PointsBalanceQty>0</PointsBalanceQty>
                <Vouchers>0.00</Vouchers>
              </PointsInfoAllCollPrds>
            </NewDataSet>";
            
            string errorXml = "";
            int rowCount = 3;
            _clubcardServiceClient.Stub(x => x.GetPointsForAllCollPeriodByCustomer(out Arg<string>.Out(errorXml).Dummy,
                out Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy, 
                Arg<string>.Is.Anything,Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
              List<Offer> Offers= new List<Offer>();
              //Offers = clubcardServiceAdapter.GetOffersForCustomer(1234, "en-GB");
              Assert.AreEqual(3, Offers.Count);

            
        }

        [TestCase]
        public void GetHouseHoldCustomersDetails_ClubcardService_Return_False()
        {
            string resultXml = "";
            string errorXml = "";
            _clubcardServiceClient.Stub(x => x.GetHouseholdCustomers(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, Arg<long>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            List<HouseholdCustomerDetails> houseHoldCustomerDetailsList = new List<HouseholdCustomerDetails>();
            //houseHoldCustomerDetailsList = clubcardServiceAdapter.GetHouseHoldCustomers(1, "en-GB");
            Assert.AreEqual(0, houseHoldCustomerDetailsList.Count);
        }

        [TestCase]
        public void GetHouseHoldCustomersDetails_ClubcardService_No_Household_Customers_Data()
        {
            string resultXml = @"<NewDataSet>
                </NewDataSet>";
            string errorXml = "";
            _clubcardServiceClient.Stub(x => x.GetHouseholdCustomers(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, Arg<long>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            List<HouseholdCustomerDetails> houseHoldCustomerDetailsList = new List<HouseholdCustomerDetails>();
            //houseHoldCustomerDetailsList = clubcardServiceAdapter.GetHouseHoldCustomers(1, "en-GB");
            Assert.AreEqual(0, houseHoldCustomerDetailsList.Count);
        }

        [TestCase]
        public void GetHouseHoldCustomersDetails_ClubcardService_CustomerUseStatusID_Not_Present()
        {
            string resultXml = @"<NewDataSet>
                  <HouseholdCustomers>
                    <PrimaryCustomerID>435388</PrimaryCustomerID>
                    <CustomerID>435388</CustomerID>
                    <TitleEnglish>Mr</TitleEnglish>
                    <Name1>Sahitestqweqq</Name1>
                    <Name2>Sa</Name2>
                    <Name3>Alexanderewe</Name3>
                    <MailingAddressLine1>33 CHAMBERS COMPANY GROVE</MailingAddressLine1>
                    <MailingAddressPostCode>AL7 4FG</MailingAddressPostCode>
                    <CustomerMailStatus>7</CustomerMailStatus>
                  </HouseholdCustomers>
                  <HouseholdCustomers>
                    <PrimaryCustomerID>435388</PrimaryCustomerID>
                    <CustomerID>1095609850</CustomerID>
                    <TitleEnglish>Mr</TitleEnglish>
                    <Name1>Sahitest</Name1>
                    <Name2>Sa</Name2>
                    <Name3>Alexanderesww</Name3>
                    <MailingAddressLine1>33 CHAMBERS COMPANY GROVE</MailingAddressLine1>
                    <MailingAddressPostCode>AL7 4FG</MailingAddressPostCode>
                    <CustomerMailStatus>7</CustomerMailStatus>
                  </HouseholdCustomers>
                </NewDataSet>";
            string errorXml = "";
            _clubcardServiceClient.Stub(x => x.GetHouseholdCustomers(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, Arg<long>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            List<HouseholdCustomerDetails> houseHoldCustomerDetailsList = new List<HouseholdCustomerDetails>();
            //houseHoldCustomerDetailsList = clubcardServiceAdapter.GetHouseHoldCustomers(1, "en-GB");
            Assert.AreEqual(2, houseHoldCustomerDetailsList.Count);
        }
    }
}
