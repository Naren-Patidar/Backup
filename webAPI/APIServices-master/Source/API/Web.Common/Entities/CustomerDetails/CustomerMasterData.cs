using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Web;
using System.Globalization;
using System.Web.Mvc;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class CustomerMasterData
    {
        private Int64 _CustomerId;
        private string _emailAddress;
        private string _mobileNumber;
        private string _title;
        private string _firstName;
        private string _Initial;
        private string _lastName;
        private string _familyMemberDob1;
        private string _sex;
        private string _mailingAddressLine1;
        private string _mailingAddressLine2;
        private string _mailingAddressLine3;
        private string _mailingAddressLine4;
        private string _mailingAddressLine5;
        private string _mailingAddressLine6;
        private string _mailingAddressPostCode;
        private string _dob;
        private string _passportNo;
        private string _dayTimePhonenumber;
        private string _eveningPhonenumber;
        private string _raceID;
        private string _ISOLanguageCode;
        private string _customerEmailStatus;
        private string _customerMobilePhoneStatus;
        private string _customerUseStatus;
        private string _SSN;

        public string CustomerId
        {
            get { return _CustomerId.ToString(); }
            set { _CustomerId = value.TryParse<long>(); }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_MailingAddressLine2")]
        public string MailingAddressLine2
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine2)?_mailingAddressLine2 : string.Empty; }
            set { _mailingAddressLine2 = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_MailingAddressLine3")]
        public string MailingAddressLine3
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine3) ? _mailingAddressLine3 : string.Empty; ; }
            set { _mailingAddressLine3 = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_MailingAddressLine4")]
        public string MailingAddressLine4
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine4) ? _mailingAddressLine4 : string.Empty; ; }
            set { _mailingAddressLine4 = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_MailingAddressLine5")]
        public string MailingAddressLine5
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine5) ? _mailingAddressLine5 : string.Empty; ; }
            set { _mailingAddressLine5 = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_MailingAddressLine6")]
        public string MailingAddressLine6
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine6) ? _mailingAddressLine6 : string.Empty; ; }
            set { _mailingAddressLine6 = value; }
        }


        public string MailingAddressPostCode
        {
            get { return !string.IsNullOrEmpty(_mailingAddressPostCode) ? _mailingAddressPostCode : string.Empty; ; }
            set { _mailingAddressPostCode = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_DaytimePhonenumber")]
        public string DayTimePhonenumber
        {
            get { return _dayTimePhonenumber; }
            set { _dayTimePhonenumber = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_EveningPhonenumber")]
        public string EveningPhonenumber
        {
            get { return _eveningPhonenumber; }
            set { _eveningPhonenumber = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_SSN")]
        public string SSN
        {
            get { return _SSN; }
            set { _SSN = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_PassportNo")]
        public string PassportNo
        {
            get { return _passportNo; }
            set { _passportNo = value; }
        }

        [Display(Name = "PersonalDetails_RaceID")]
        public string RaceID
        {
            get { return _raceID; }
            set { _raceID = value; }
        }

        [Display(Name = "PersonalDetails_ISOLanguageCode")]
        public string ISOLanguageCode
        {
            get { return _ISOLanguageCode; }
            set { _ISOLanguageCode = value; }
        }
        private string _customerMailStatus;

        public string CustomerMailStatus
        {
            get { return _customerMailStatus; }
            set { _customerMailStatus = value; }
        }

        public string CustomerMobilePhoneStatus
        {
            get { return _customerMobilePhoneStatus; }
            set { _customerMobilePhoneStatus = value; }
        }

        public string CustomerEmailStatus
        {
            get { return _customerEmailStatus; }
            set { _customerEmailStatus = value; }
        }

        public string CustomerUseStatus
        {
            get { return _customerUseStatus; }
            set { _customerUseStatus = value; }
        }

        public string DOB
        {
            get { return _dob; }
            set { _dob = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_EmailAddress")]
        public string EmailAddress
        {
            get { return !string.IsNullOrEmpty(_emailAddress) ? _emailAddress : string.Empty; ; }
            set { _emailAddress = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_MobilePhoneNumber")]
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; }
        }

        [Display(Name = "PersonalDetails_Title")]
        public string Title
        {
            get { return !string.IsNullOrEmpty(_title) ? _title : string.Empty; ; }
            set { _title = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_FirstName")]
        public string FirstName
        {
            get { return !string.IsNullOrEmpty(_firstName) ? _firstName : string.Empty; ; }
            set { _firstName = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_Initial")]
        public string Initial
        {
            get { return !string.IsNullOrEmpty(_Initial) ? _Initial : string.Empty; ; }
            set { _Initial = value; }
        }

        [AllowHtml]
        [Display(Name = "PersonalDetails_LastName")]
        public string LastName
        {
            get { return !string.IsNullOrEmpty(_lastName) ? _lastName : string.Empty; ; }
            set { _lastName = value; }
        }

        [Display(Name = "PersonalDetails_FamilyMemberDOB1")]
        public string FamilyMemberDOB1
        {
            get { return _familyMemberDob1; }
            set { _familyMemberDob1 = value; }
        }

        [Display(Name = "PersonalDetails_Sex")]
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        [AllowHtml]        
        [Display(Name = "PersonalDetails_MailingAddressLine1")]
        public string MailingAddressLine1
        {
            get { return !string.IsNullOrEmpty(_mailingAddressLine1) ? _mailingAddressLine1 : string.Empty; ; }
            set { _mailingAddressLine1 = value; }
        }
    }
}

