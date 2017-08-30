using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class HomeViewModel : ComparableEntity<HomeViewModel>
    {
        StampModel _stampView = new StampModel();
        CustomerAddressVerificationModel _customerAddressVerification = new CustomerAddressVerificationModel(); 
        private string _clubcardNumber = string.Empty;
        public string WelcomeMessage { get; set; }
        public string CustomerPointsBalance { get; set; }
        public string CustomerVoucherTypeText { get; set; }
        public string CustomerVoucherTypeValue { get; set; }
        public string MyMessageHeader { get; set; }
        public string CurrentOfferId { get; set; }
        public Boolean IsSecurityCheckDone { get; set; }

        public string ClubcardNumber
        {
            get { return _clubcardNumber; }
            set { _clubcardNumber = value; }
        }

        public StampModel StampView
        {
            get { return _stampView; }
            set { _stampView = value; }
        }

        public CustomerAddressVerificationModel CustomerAddressVerificationView
        {
            get { return _customerAddressVerification; }
            set { _customerAddressVerification = value; }
        }

        internal override bool AreInstancesEqual(HomeViewModel target)
        {
            return ClubcardNumber.Equals(target.ClubcardNumber)
                    && WelcomeMessage.Equals(target.WelcomeMessage)
                    && CustomerPointsBalance.Equals(target.CustomerPointsBalance)
                     && CustomerVoucherTypeText.Equals(target.CustomerVoucherTypeText)
                     && CustomerVoucherTypeValue.Equals(target.CustomerVoucherTypeValue)
                     && MyMessageHeader.Equals(target.MyMessageHeader);
        }
    }

    public class CustomerAddressVerificationModel
    {
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string Postcode { get; set; }
        public Boolean IsVerified { get; set;}
        public Boolean IsCustomerDetailsMatched { get; set; } 
    }
}
