using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{

    public class SecurityViewModel
    {
        public int firstSecureDigit { get; set; }
        public int secondSecureDigit { get; set; }
        public int thirdSecureDigit { get; set; }

        public string firstSecureDigitEncrypt { get; set; }
        public string secondSecureDigitEncrypt { get; set; }
        public string thirdSecureDigitEncrypt { get; set; }

        public bool IsBlocked { get; set; }
        public bool isSuccessful { get; set; }
        public bool ShowCaptcha { get; set; }
        public int FailedLoginAttempts { get; set; }

        //start- changes for CCMCA-4454- Added annotation
        [RegularExpression("^[0-9]$")]
        [StringLength(1)]
        public string txtfirstSecureDigit { get; set; }
        [RegularExpression("^[0-9]$")]
        [StringLength(1)]
        public string txtsecondSecureDigit { get; set; }
        [RegularExpression("^[0-9]$")]
        [StringLength(1)]
        public string txtthirdSecureDigit { get; set; }
        //end- changes for CCMCA-4454
        
        public bool showErrorLabel1 { get; set; }
        public bool showErrorLabel2 { get; set; }
        public bool showErrorLabel3 { get; set; }
        public bool showCaptchaError { get; set; }

        public string SiteKey { get; set; }

        public System.Web.HtmlString requiredFieldValidator3Resource1 { get; set; }
        public System.Web.HtmlString requiredFieldValidator4Resource1 { get; set; }
        public System.Web.HtmlString requiredFieldValidator5Resource1 { get; set; }
    }
}
