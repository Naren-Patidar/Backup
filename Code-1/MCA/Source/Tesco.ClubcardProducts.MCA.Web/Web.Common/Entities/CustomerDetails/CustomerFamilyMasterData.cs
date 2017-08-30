using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Globalization;

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
                                      CustomerId = t.Element(CustomerMasterDataEnum.CustomerID).GetValue<Int64>(),
                                      EmailAddress = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.email_address).GetValue<string>()),
                                      MobileNumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.mobile_phone_number).GetValue<string>()),
                                      Title = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.TitleEnglish).GetValue<string>()),
                                      FirstName = t.Element(CustomerMasterDataEnum.Name1).GetValue<string>().Trim().ToTitleCase(CultureInfo.CurrentCulture),
                                      Initial = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.Name2).GetValue<string>().Trim().ToTitleCase(CultureInfo.CurrentCulture)),
                                      LastName = System.Net.WebUtility.HtmlDecode(t.Element(CustomerMasterDataEnum.Name3).GetValue<string>().Trim().ToTitleCase(CultureInfo.CurrentCulture)),
                                      FamilyMemberDOB1 = t.Element(CustomerMasterDataEnum.family_member_1_dob).GetValue<string>(),
                                      Sex = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.Sex).GetValue<string>()),
                                      MailingAddressLine1 = t.Element(CustomerMasterDataEnum.MailingAddressLine1).GetValue<string>(),
                                      MailingAddressLine2 = t.Element(CustomerMasterDataEnum.MailingAddressLine2).GetValue<string>(),
                                      MailingAddressLine3 = t.Element(CustomerMasterDataEnum.MailingAddressLine3).GetValue<string>(),
                                      MailingAddressLine4 = t.Element(CustomerMasterDataEnum.MailingAddressLine4).GetValue<string>(),
                                      MailingAddressLine5 = t.Element(CustomerMasterDataEnum.MailingAddressLine5).GetValue<string>(),
                                      MailingAddressLine6 = t.Element(CustomerMasterDataEnum.MailingAddressLine6).GetValue<string>(),
                                      MailingAddressPostCode = System.Net.WebUtility.HtmlDecode(t.Element(CustomerMasterDataEnum.MailingAddressPostCode).GetValue<string>()),
                                      DayTimePhonenumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.daytime_phone_number).GetValue<string>()),
                                      EveningPhonenumber = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.evening_phone_number).GetValue<string>()),
                                      SSN = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.SSN).GetValue<string>()),
                                      PassportNo = System.Net.WebUtility.HtmlDecode(t.Element(CustomerMasterDataEnum.PassportNo).GetValue<string>()),
                                      RaceID = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.RaceID).GetValue<string>().Trim().ToString()),
                                      ISOLanguageCode = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.ISOLanguageCode).GetValue<string>().Trim().ToString()),
                                      CustomerMailStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerMailStatus).GetValue<string>()),
                                      CustomerMobilePhoneStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerMobilePhoneStatus).GetValue<string>()),
                                      CustomerEmailStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerEmailStatus).GetValue<string>()),
                                      CustomerUseStatus = StringUtility.HTMLEncode(t.Element(CustomerMasterDataEnum.CustomerUseStatus).GetValue<string>())
                                  }).ToList();

            this._familyData = (from t in xDoc.Descendants("FamilyDetails")
                                select new FamilyMasterData
                                {
                                    NumberOfHouseholdMembers = t.Element(FamilyMasterDataEnum.number_of_household_members).GetValue<Int32>(),
                                    DateOfBirth = t.Element(FamilyMasterDataEnum.DateOfBirth).GetValue<DateTime>(),
                                    SeqNo = t.Element(FamilyMasterDataEnum.FamilyMemberSeqNo).GetValue<Int32>()
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
