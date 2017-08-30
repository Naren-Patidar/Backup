using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    [XmlRoot("customer")]
    public class CustomerFamilyMasterDataUpdate
    {
        private long _customerId;
        private string _title = string.Empty;
        private string _firstName = string.Empty;
        private string _initial = string.Empty;
        private string _lastName = string.Empty;
        private string _dateOfBirth = string.Empty;
        private string _sex = string.Empty;
        private string _mailingAddressLine1 = string.Empty;
        private string _mailingAddressLine2= string.Empty;
        private string _mailingAddressLine3 = string.Empty;
        private string _mailingAddressLine4 = string.Empty;
        private string _mailingAddressLine5 = string.Empty;
        private string _mailingAddressLine6 = string.Empty;
        private string _postCode= string.Empty;
        private string _mobile_phone_number= string.Empty;
        private string _mobilePhoneNumber= string.Empty;
        private string _emailAddress= string.Empty;
        private string _email_address= string.Empty;
        private string _evening_phone_number= string.Empty;
        private string _daytime_phone_number= string.Empty;
        private string _ssn= string.Empty;
        private string _passportNo= string.Empty;
        private CustomerMailStatusEnum _mailStatus;
        private CustomerMailStatusEnum _emailStatus;
        private CustomerMailStatusEnum _mobilePhoneStatus;
        private int _customerUseStatus;
        private int _raceId;
        private string _isoLanguageCode= string.Empty;
        private string _culture= string.Empty;
        private short _numberOfHouseholdMembers=0;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        private DateTime? _familyMember1Dob = null;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        private DateTime? _familyMember2Dob = null;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        private DateTime? _familyMember3Dob = null;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        private DateTime? _familyMember4Dob = null;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        private DateTime? _familyMember5Dob = null;

        public long CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        [XmlElement("TitleEnglish")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [XmlElement("Name1")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [XmlElement("Name2")]
        public string Initial
        {
            get { return _initial; }
            set { _initial = value; }
        }

        [XmlElement("Name3")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        
        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        public string MailingAddressLine1
        {
            get { return _mailingAddressLine1; }
            set { _mailingAddressLine1 = value; }
        }
        public string MailingAddressLine2
        {
            get { return _mailingAddressLine2; }
            set { _mailingAddressLine2 = value; }
        }
        public string MailingAddressLine3
        {
            get { return _mailingAddressLine3; }
            set { _mailingAddressLine3 = value; }

        }
        public string MailingAddressLine4
        {
            get { return _mailingAddressLine4; }
            set { _mailingAddressLine4 = value; }
        }
        public string MailingAddressLine5
        {
            get { return _mailingAddressLine5; }
            set { _mailingAddressLine5 = value; }
        }
        public string MailingAddressLine6
        {
            get { return _mailingAddressLine6; }
            set { _mailingAddressLine6 = value; }
        }

        [XmlElement("MailingAddressPostCode")]
        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }
        [XmlElement("mobile_phone_number")]
        public string mobile_phone_number
        {
            get { return _mobile_phone_number; }
            set { _mobile_phone_number = value; }
        }
        [XmlElement("MobilePhoneNumber")]
        public string MobilePhoneNumber
        {
            get { return _mobilePhoneNumber; }
            set { _mobilePhoneNumber = value; }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
        public string email_address
        {
            get { return _email_address; }
            set { _email_address = value; }
        }
        public string evening_phone_number
        {
            get { return _evening_phone_number; }
            set { _evening_phone_number = value; }
        }
        public string daytime_phone_number
        {
            get { return _daytime_phone_number; }
            set { _daytime_phone_number = value; }
        }
        public string SSN
        {
            get { return _ssn; }
            set { _ssn = value; }
        }
        public string PassportNo
        {
            get { return _passportNo; }
            set { _passportNo = value; }
        }
        [XmlElement("CustomerMailStatus")]
        public CustomerMailStatusEnum MailStatus
        {
            get { return _mailStatus; }
            set { _mailStatus = value; }
        }
        [XmlElement("CustomerEmailStatus")]
        public CustomerMailStatusEnum EmailStatus
        {
            get { return _emailStatus; }
            set { _emailStatus = value; }
        }
        [XmlElement("CustomerMobilePhoneStatus")]
        public CustomerMailStatusEnum MobilePhoneStatus
        {
            get { return _mobilePhoneStatus; }
            set { _mobilePhoneStatus = value; }
        }
        [XmlElement("CustomerUseStatusMain")]
        public int CustomerUseStatus
        {
            get { return _customerUseStatus; }
            set { _customerUseStatus = value; }
        }
        public int RaceID
        {
            get { return _raceId; }
            set { _raceId = value; }
        }
        public string ISOLanguageCode
        {
            get { return _isoLanguageCode; }
            set { _isoLanguageCode = value; }
        }
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        
        [XmlElement("number_of_household_members")]
        public short NumberOfHouseholdMembers
        {
            get { return _numberOfHouseholdMembers; }
            set { _numberOfHouseholdMembers = value; }
        }

        
        [XmlElement("family_member_1_dob")]
        public DateTime? FamilyMember1Dob
        {
            get { return _familyMember1Dob; }
            set { _familyMember1Dob = value; }
        }
        
        [XmlElement("family_member_2_dob")]
        public DateTime? FamilyMember2Dob
        {
            get { return _familyMember2Dob; }
            set { _familyMember2Dob = value; }
        }
        
        [XmlElement("family_member_3_dob")]
        public DateTime? FamilyMember3Dob
        {
            get { return _familyMember3Dob; }
            set { _familyMember3Dob = value; }
        }
        
        [XmlElement("family_member_4_dob")]
        public DateTime? FamilyMember4Dob
        {
            get { return _familyMember4Dob; }
            set { _familyMember4Dob = value; }
        }
        
        [XmlElement("family_member_5_dob")]
        public DateTime? FamilyMember5Dob
        {
            get { return _familyMember5Dob; }
            set { _familyMember5Dob = value; }
        }

        /// <summary>
        /// Applies XmlIgnore property on chosen fields dynamically
        /// </summary>
        /// <returns></returns>
        public XmlAttributeOverrides GetXmlAttributeOverrider()
        {
            // Create the XmlAttributeOverrides and XmlAttributes objects.
            XmlAttributeOverrides xOver = new XmlAttributeOverrides();
            

            if (string.IsNullOrEmpty(DateOfBirth))
            {
                XmlAttributes attrs = new XmlAttributes();
                /* Setting XmlIgnore to false overrides the XmlIgnoreAttribute
                   applied to the Comment field. Thus it will be serialized.*/
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "DateOfBirth", attrs);
            }

            if (FamilyMember1Dob == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "FamilyMember1Dob", attrs);
            }
            if (FamilyMember2Dob == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "FamilyMember2Dob", attrs);
            }
            if (FamilyMember3Dob == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "FamilyMember3Dob", attrs);
            } 
            if (FamilyMember4Dob == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "FamilyMember4Dob", attrs);
            }
            if (FamilyMember5Dob == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "FamilyMember5Dob", attrs);
            }
            if (string.IsNullOrEmpty(_mobile_phone_number))
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "mobile_phone_number", attrs);

                attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "MobilePhoneNumber", attrs);

                attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "MobilePhoneStatus", attrs);
            }
            if (string.IsNullOrEmpty(_email_address))
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "email_address", attrs);

                attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "EmailAddress", attrs);

                attrs = new XmlAttributes();
                attrs.XmlIgnore = true;
                xOver.Add(typeof(CustomerFamilyMasterDataUpdate), "EmailStatus", attrs);
            }


            return xOver;
        }
    }
}
