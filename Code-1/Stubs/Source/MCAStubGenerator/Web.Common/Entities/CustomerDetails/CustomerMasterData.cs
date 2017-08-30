﻿using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Validator;
using System.Globalization;
using System.Web.Mvc;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    public class CustomerMasterData : IValidatableObject
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

        public Int64 CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine2")]
        public string MailingAddressLine2
        {
            get { return _mailingAddressLine2; }
            set { _mailingAddressLine2 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine3")]
        public string MailingAddressLine3
        {
            get { return _mailingAddressLine3; }
            set { _mailingAddressLine3 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine4")]
        public string MailingAddressLine4
        {
            get { return _mailingAddressLine4; }
            set { _mailingAddressLine4 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine5")]
        public string MailingAddressLine5
        {
            get { return _mailingAddressLine5; }
            set { _mailingAddressLine5 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine6")]
        public string MailingAddressLine6
        {
            get { return _mailingAddressLine6; }
            set { _mailingAddressLine6 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressPostCode")]
        public string MailingAddressPostCode
        {
            get { return _mailingAddressPostCode; }
            set { _mailingAddressPostCode = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_DaytimePhonenumber")]
        public string DayTimePhonenumber
        {
            get { return _dayTimePhonenumber; }
            set { _dayTimePhonenumber = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_EveningPhonenumber")]
        public string EveningPhonenumber
        {
            get { return _eveningPhonenumber; }
            set { _eveningPhonenumber = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_SSN")]
        public string SSN
        {
            get { return _SSN; }
            set { _SSN = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_PassportNo")]
        public string PassportNo
        {
            get { return _passportNo; }
            set { _passportNo = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_RaceID")]
        public string RaceID
        {
            get { return _raceID; }
            set { _raceID = value; }
        }

        [Validators]
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
        [Validators]
        [Display(Name = "PersonalDetails_EmailAddress")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MobilePhoneNumber")]
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_FirstName")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_Initial")]
        public string Initial
        {
            get { return _Initial; }
            set { _Initial = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_LastName")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_FamilyMemberDOB1")]
        public string FamilyMemberDOB1
        {
            get { return _familyMemberDob1; }
            set { _familyMemberDob1 = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_Sex")]
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine1")]
        public string MailingAddressLine1
        {
            get { return _mailingAddressLine1; }
            set { _mailingAddressLine1 = value; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CultureInfo enGBCulture = new CultureInfo("en-GB");
            var prop = new[] { "Sex" };
            if (!String.IsNullOrEmpty(Sex))
            {
                if ((Sex.ToUpper().Contains("M") &&
                    (this.Title.ToUpper().Equals("MRS") || this.Title.ToUpper().Equals("MISS") || this.Title.ToUpper().Equals("MS")))
                    || (Sex.ToUpper().Contains("F") && (this.Title.ToUpper().Equals("MR"))))
                    yield return new ValidationResult(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "gender.mismatch", enGBCulture).ToString(), prop);
            }
        }
    }
}

