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

        public string IsBlocked { get; set; }
        public bool isSuccessful { get; set; }
        public string txtfirstSecureDigit { get; set; }
        public string txtsecondSecureDigit { get; set; }
        public string txtthirdSecureDigit { get; set; }
        public string errorLable1 { get; set; }
        public string errorLable2 { get; set; }
        public string errorLable3 { get; set; }
        public System.Web.HtmlString requiredFieldValidator3Resource1 { get; set; }
        public System.Web.HtmlString requiredFieldValidator4Resource1 { get; set; }
        public System.Web.HtmlString requiredFieldValidator5Resource1 { get; set; }
    }

}
