using System;
using System.Runtime.Serialization;
using Tesco.ClubcardProducts.MCA.Web.Common.Validator;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{
    
  
    public class ContactDetail 
    {
        private string _emailAddress;
        private string _dayContactNumber;
        private string _eveningContactNumber;
        private string _mobileContactNumber;
        
        public static readonly ContactDetail Empty = new ContactDetail();

        [Validators]
        [DisplayName("Activation_EmailAddress")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        
        public string DayContactNumber
        {
            get { return _dayContactNumber; }
            set { _dayContactNumber = value; }
        }

        
        public string EveningContactNumber
        {
            get { return _eveningContactNumber; }
            set { _eveningContactNumber = value; }
        }

        [Validators]
        [DisplayName("Activation_MobilePhoneNumber")]
        public string MobileContactNumber
        {
            get { return _mobileContactNumber; }
            set { _mobileContactNumber = value; }
        }
    }
}
