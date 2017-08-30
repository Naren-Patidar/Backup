using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.Validator;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class AddressDetails
    {
        public string Locality { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        
        [AllowHtml]
        [Validators]
        [Display(Name = "PersonalDetails_MailingAddressLine")]
        public string Houseno { get; set; }

        public string AddressChosen { get; set; }
        public bool IsErrorMessage { get; set; }
        public bool IsStreetVisible { get; set; }
        public bool IsLocalityVisible { get; set; }
        public bool IsCountryVisible { get; set; }
        public bool IsCityVisible { get; set; }
        public string hdnMailingAddress { get; set; }
        public int hdnSelectedIndex { get; set; }
        public Dictionary<int,string> AddressList { get; set; }
        public ArrayList HideAddressList { get; set; }
        public string HideAddressformat { get; set; }
    }
}
