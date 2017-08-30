using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    public class CustomerFamilyMasterData : BaseEntity<CustomerFamilyMasterData>
    {
        public CustomerFamilyMasterData()
        {

        }

        public CustomerFamilyMasterData(List<CustomerMasterData> customerData, List<FamilyMasterData> FamilyData, int NumberOfFamilyMembers)
        {
            this._customerData = customerData;
            this._familyData = FamilyData;
            this._numberOfFamilyMembers = NumberOfFamilyMembers;
        }

        #region PrivateFields

        List<CustomerMasterData> _customerData = new List<CustomerMasterData>();
        List<FamilyMasterData> _familyData = new List<FamilyMasterData>();
        int _numberOfFamilyMembers = 0;

        #endregion       
        
        public int NumberOfFamilyMembers { get { return this._numberOfFamilyMembers; } }

        public List<CustomerMasterData> CustomerData
        {
            get { return _customerData; }
        }

        public List<FamilyMasterData> FamilyData
        {
            get { return _familyData; }
        }

        public override void ConvertFromXml(string xml) 
        {
            XDocument xDoc = XDocument.Parse(xml);
            this._customerData = (from t in xDoc.Descendants("Customer")
                                 select new CustomerMasterData
                                 {
                                     CustomerId = t.Element(CustomerMasterDataEnum.CustomerID.ToString()).GetValue<Int64>(),
                                     EmailAddress = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.email_address.ToString()).GetValue<string>()),
                                     MobileNumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.mobile_phone_number.ToString()).GetValue<string>()),
                                     Title = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.TitleEnglish.ToString()).GetValue<string>()),
                                     FirstName = t.Element(CustomerMasterDataEnum.Name1.ToString()).GetValue<string>(),
                                     Initial = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.Name2.ToString()).GetValue<string>()),
                                     LastName = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.Name3.ToString()).GetValue<string>()),
                                     FamilyMemberDOB1 = t.Element(CustomerMasterDataEnum.family_member_1_dob.ToString()).GetValue<string>(),
                                     Sex = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.Sex.ToString()).GetValue<string>()),
                                     MailingAddressLine1 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine1.ToString()).GetValue<string>()),
                                     MailingAddressLine2 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine2.ToString()).GetValue<string>()),
                                     MailingAddressLine3 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine3.ToString()).GetValue<string>()),
                                     MailingAddressLine4 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine4.ToString()).GetValue<string>()),
                                     MailingAddressLine5 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine5.ToString()).GetValue<string>()),
                                     MailingAddressLine6 = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressLine6.ToString()).GetValue<string>()),
                                     MailingAddressPostCode = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.MailingAddressPostCode.ToString()).GetValue<string>()),
                                     DayTimePhonenumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.daytime_phone_number.ToString()).GetValue<string>()),
                                     EveningPhonenumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.evening_phone_number.ToString()).GetValue<string>()),
                                     SSN = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.SSN.ToString()).GetValue<string>()),
                                     PassportNo = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.PassportNo.ToString()).GetValue<string>()),
                                     RaceID = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.RaceID.ToString()).GetValue<string>().Trim().ToString()),
                                     ISOLanguageCode = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.ISOLanguageCode.ToString()).GetValue<string>().Trim().ToString()),
                                     CustomerMailStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerMailStatus.ToString()).GetValue<string>()),
                                     CustomerMobilePhoneStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerMobilePhoneStatus.ToString()).GetValue<string>()),
                                     CustomerEmailStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerEmailStatus.ToString()).GetValue<string>()),
                                     CustomerUseStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerUseStatus.ToString()).GetValue<string>())
                                 }).ToList();

            this._familyData = (from t in xDoc.Descendants("FamilyDetails")
                               select new FamilyMasterData
                               {
                                   NumberOfHouseholdMembers = t.Element(FamilyMasterDataEnum.number_of_household_members.ToString()).GetValue<Int32>(),
                                   DateOfBirth = t.Element(FamilyMasterDataEnum.DateOfBirth.ToString()).GetValue<DateTime>(),
                                   SeqNo = t.Element(FamilyMasterDataEnum.FamilyMemberSeqNo.ToString()).GetValue<Int32>()
                               }).ToList();

            this._numberOfFamilyMembers = (from t in xDoc.Descendants("NoOFFamilyMembers")
                                          select t.GetValue<Int32>()).ToList().FirstOrDefault();
        }

        internal override bool AreInstancesEqual(CustomerFamilyMasterData target)
        {
            bool bReturn = true;

            return bReturn;
        }
    }
}
