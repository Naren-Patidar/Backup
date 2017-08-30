using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;

namespace Web.ServiceAdapter.Tests
{
    [TestFixture]
    public class CustomerServiceAdapterTests
    {
        private ICustomerService _customerServiceClient;
        private CustomerServiceAdapter customerServiceAdapter; 
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            _customerServiceClient = MockRepository.GenerateMock<ICustomerService>();
            customerServiceAdapter = new CustomerServiceAdapter(_customerServiceClient, _logger);
        }

        [TearDown]
        public void TestCleanup()
        {
            _customerServiceClient = null;
            customerServiceAdapter = null;
        }

        [TestCase]
        public void GetCustomerMasterData_Customer_Service_Valid_Data_Return_True()
        {
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
            _customerServiceClient.Stub(x=> x.GetCustomerDetails(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy,
               Arg<string>.Is.Anything, Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData();
            //customerFamilyMasterData = customerServiceAdapter.GetCustomerDetails(1, 1, "en-GB");
            Assert.AreEqual(4, customerFamilyMasterData.CustomerData.Count + customerFamilyMasterData.FamilyData.Count
                + customerFamilyMasterData.NumberOfFamilyMembers);

        }

        [TestCase]
        public void GetCustomerMasterData_Customer_Service_Return_False()
        {
            string resultXml = "";
            string errorXml = "";
            int rowCount = 2;
            _customerServiceClient.Stub(x => x.GetCustomerDetails(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy,
               Arg<string>.Is.Anything, Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData();
            //customerFamilyMasterData = customerServiceAdapter.GetCustomerDetails(1, 1, "en-GB");
            Assert.AreEqual(null, customerFamilyMasterData);

        }

        [TestCase]
        public void GetCustomerMasterData_Customer_Service_No_Family_Data()
        {
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
                <CustomerEmailStatus>7</CustomerEmailStatus></Customer></NewDataSet></CustomerInformation>";
            string errorXml = "";
            int rowCount = 2;
            _customerServiceClient.Stub(x => x.GetCustomerDetails(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy,
               Arg<string>.Is.Anything, Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData();
            //customerFamilyMasterData = customerServiceAdapter.GetCustomerDetails(1, 1, "en-GB");
            Assert.AreEqual(2, customerFamilyMasterData.CustomerData.Count);

        }

        [TestCase]
        public void GetCustomerMasterData_Customer_Service_No_Customer_Data()
        {
            string resultXml = @"<CustomerInformation><NewDataSet><FamilyDetails>
                <FamilyMemberSeqNo>1</FamilyMemberSeqNo><DateOfBirth>2015-01-01T00:00:00+00:00</DateOfBirth>
                <number_of_household_members>2</number_of_household_members></FamilyDetails></NewDataSet>
                <NewDataSet><NoOFFamilyMembers><number_of_household_members>1</number_of_household_members></NoOFFamilyMembers></NewDataSet></CustomerInformation>";
            string errorXml = "";
            int rowCount = 2;
            _customerServiceClient.Stub(x => x.GetCustomerDetails(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy,
               Arg<string>.Is.Anything, Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData();
            //customerFamilyMasterData = customerServiceAdapter.GetCustomerDetails(1, 1, "en-GB");
            Assert.AreEqual(2, customerFamilyMasterData.FamilyData.Count
                + customerFamilyMasterData.NumberOfFamilyMembers);
        }

        [TestCase]
        [ExpectedException]
        public void GetCustomerMasterData_Customer_Service_Expected_Exception_Family_Data()
        {
            string resultXml = @"<CustomerInformation><NewDataSet><FamilyDetails>
                <FamilyMemberSeqNo>//</FamilyMemberSeqNo><DateOfBirth>2015-01-01T00:00:00+00:00</DateOfBirth>
                <number_of_household_members>2</number_of_household_members></FamilyDetails></NewDataSet>
                <NewDataSet><NoOFFamilyMembers><number_of_household_members>1</number_of_household_members></NoOFFamilyMembers></NewDataSet></CustomerInformation>";
            string errorXml = "";
            int rowCount = 2;
            _customerServiceClient.Stub(x => x.GetCustomerDetails(out Arg<string>.Out(errorXml).Dummy,
                out   Arg<string>.Out(resultXml).Dummy, out   Arg<int>.Out(rowCount).Dummy,
               Arg<string>.Is.Anything, Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            CustomerFamilyMasterData customerFamilyMasterData = new CustomerFamilyMasterData();
            //customerFamilyMasterData = customerServiceAdapter.GetCustomerDetails(1, 1, "en-GB");
        }
    }
}
