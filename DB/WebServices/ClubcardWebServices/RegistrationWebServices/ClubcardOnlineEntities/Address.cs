using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Address Information Entity, Associative Entity for Customer</para>
    /// <para>Author: Akash Jainr</para>
    /// <para>Date: 15/02/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC ,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class Address : EntityBase
    {
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _addressLine4;
        private string _addressLine5;
        private string _addressLine6;
        private string _postCode;

        public static readonly Address Empty = new Address(true);

        private Address(bool isEmpty) : base(isEmpty)
        {
        }

        [DataMember]
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        [DataMember]
        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        [DataMember]
        public string AddressLine3
        {
            get { return _addressLine3; }
            set { _addressLine3 = value; }
        }

        [DataMember]
        public string AddressLine4
        {
            get { return _addressLine4; }
            set { _addressLine4 = value; }
        }

        [DataMember]
        public string AddressLine5
        {
            get { return _addressLine5; }
            set { _addressLine5 = value; }
        }

        [DataMember]
        public string AddressLine6
        {
            get { return _addressLine6; }
            set { _addressLine6 = value; }
        }


        [DataMember]
        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }
    }
}
