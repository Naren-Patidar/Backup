using System;
using System.Runtime.Serialization;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation
{
    /// <summary>
    /// <para>Address Information Entity, Associative Entity for Customer</para>
    /// <para>Author: Akash Jainr</para>
    /// <para>Date: 15/02/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC ,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
  
    public class Address 
    {
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _addressLine4;
        private string _addressLine5;
        private string _addressLine6;
        private string _postCode;

        [DisplayName("Activation_MailingAddressLine1")]
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        public string AddressLine3
        {
            get { return _addressLine3; }
            set { _addressLine3 = value; }
        }

        public string AddressLine4
        {
            get { return _addressLine4; }
            set { _addressLine4 = value; }
        }
        
        public string AddressLine5
        {
            get { return _addressLine5; }
            set { _addressLine5 = value; }
        }
        
        public string AddressLine6
        {
            get { return _addressLine6; }
            set { _addressLine6 = value; }
        }

        [DisplayName("Activation_MailingAddressPostCode")]        
        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }
    }
}
