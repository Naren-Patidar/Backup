using System.Collections.Generic;
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
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine2) ? _mailingAddressLine2.Trim() : string.Empty ;}
            set { _mailingAddressLine2 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine3")]
        public string MailingAddressLine3
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine3) ? _mailingAddressLine3.Trim() : string.Empty; }
            set { _mailingAddressLine3 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine4")]
        public string MailingAddressLine4
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine4) ? _mailingAddressLine4.Trim() : string.Empty;  }
            set { _mailingAddressLine4 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine5")]
        public string MailingAddressLine5
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine5) ? _mailingAddressLine5.Trim() : string.Empty; }
            set { _mailingAddressLine5 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine6")]
        public string MailingAddressLine6
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine6) ? _mailingAddressLine6.Trim() : string.Empty; }
            set { _mailingAddressLine6 = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressPostCode")]
        public string MailingAddressPostCode
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressPostCode) ? _mailingAddressPostCode.Trim() : string.Empty; }
            set { _mailingAddressPostCode = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_DaytimePhonenumber")]
        public string DayTimePhonenumber
        {
            get { return !string.IsNullOrWhiteSpace(_dayTimePhonenumber) ? _dayTimePhonenumber.Trim() : string.Empty; }
            set { _dayTimePhonenumber = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_EveningPhonenumber")]
        public string EveningPhonenumber
        {
            get { return !string.IsNullOrWhiteSpace(_eveningPhonenumber) ? _eveningPhonenumber.Trim() : string.Empty; }
            set { _eveningPhonenumber = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_SSN")]
        public string SSN
        {
            get { return !string.IsNullOrWhiteSpace(_SSN) ? _SSN.Trim() : string.Empty; }
            set { _SSN = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_PassportNo")]
        public string PassportNo
        {
            get { return !string.IsNullOrWhiteSpace(_passportNo) ? _passportNo.Trim() : string.Empty;}
            set { _passportNo = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_RaceID")]
        public string RaceID
        {
            get { return !string.IsNullOrWhiteSpace(_raceID) ? _raceID.Trim() : string.Empty;  }
            set { _raceID = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_ISOLanguageCode")]
        public string ISOLanguageCode
        {
            get { return !string.IsNullOrWhiteSpace(_ISOLanguageCode) ? _ISOLanguageCode.Trim() : string.Empty;  }
            set { _ISOLanguageCode = value; }
        }
        private string _customerMailStatus;

        public string CustomerMailStatus
        {
            get { return !string.IsNullOrWhiteSpace(_customerMailStatus) ? _customerMailStatus.Trim() : string.Empty; }
            set { _customerMailStatus = value; }
        }

        public string CustomerMobilePhoneStatus
        {
            get { return !string.IsNullOrWhiteSpace(_customerMobilePhoneStatus) ? _customerMobilePhoneStatus.Trim() : string.Empty; }
            set { _customerMobilePhoneStatus = value; }
        }

        public string CustomerEmailStatus
        {
            get { return !string.IsNullOrWhiteSpace(_customerEmailStatus) ? _customerEmailStatus.Trim() : string.Empty;  }
            set { _customerEmailStatus = value; }
        }

        public string CustomerUseStatus
        {
            get { return !string.IsNullOrWhiteSpace(_customerUseStatus) ? _customerUseStatus.Trim() : string.Empty;  }
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
            get { return !string.IsNullOrWhiteSpace(_emailAddress) ? _emailAddress.Trim() : string.Empty;  }
            set { _emailAddress = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MobilePhoneNumber")]
        public string MobileNumber
        {
            get { return !string.IsNullOrWhiteSpace(_mobileNumber) ? _mobileNumber.Trim() : string.Empty;  }
            set { _mobileNumber = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_Title")]
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(_title) ? _title.Trim() : string.Empty; }
            set { _title = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_FirstName")]
        public string FirstName
        {
            get { return !string.IsNullOrWhiteSpace(_firstName) ? _firstName.Trim() : string.Empty;  }
            set { _firstName = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_Initial")]
        public string Initial
        {
            get { return !string.IsNullOrWhiteSpace(_Initial) ? _Initial.Trim() : string.Empty;  }
            set { _Initial = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_LastName")]
        public string LastName
        {
            get { return !string.IsNullOrWhiteSpace(_lastName) ? _lastName.Trim() : string.Empty;  }
            set { _lastName = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_FamilyMemberDOB1")]
        public string FamilyMemberDOB1
        {
            get { return !string.IsNullOrWhiteSpace(_familyMemberDob1) ? _familyMemberDob1.Trim() : string.Empty;  }
            set { _familyMemberDob1 = value; }
        }

        [Validators]
        [Display(Name = "PersonalDetails_Sex")]
        public string Sex
        {
            get { return !string.IsNullOrWhiteSpace(_sex) ? _sex.Trim() : string.Empty;  }
            set { _sex = value; }
        }

        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine1")]
        public string MailingAddressLine1
        {
            get { return !string.IsNullOrWhiteSpace(_mailingAddressLine1) ? _mailingAddressLine1.Trim() : string.Empty;  }
            set { _mailingAddressLine1 = value; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var prop = new[] { "Sex" };
            if (!String.IsNullOrEmpty(Sex))
            {
                if (DBConfigurationManager.Instance[DbConfigurationTypeEnum.ChinaHiddenFunctionality][DbConfigurationItemNames.ChinaHiddenFunctionalityTitle].ConfigurationValue1 != "1"
                || DBConfigurationManager.Instance[DbConfigurationTypeEnum.ChinaHiddenFunctionality][DbConfigurationItemNames.HideGender].ConfigurationValue1 != "1")
                {
                    if ((Sex.ToUpper().Contains("M") &&
                     (this.Title.ToUpper().Equals("MRS") || this.Title.ToUpper().Equals("MISS") || this.Title.ToUpper().Equals("MS")))
                     || (Sex.ToUpper().Contains("F") && (this.Title.ToUpper().Equals("MR"))))
                        yield return new ValidationResult(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "gender.mismatch", System.Globalization.CultureInfo.CurrentCulture).ToString(), prop);
                }
            }
        }
    }
}
