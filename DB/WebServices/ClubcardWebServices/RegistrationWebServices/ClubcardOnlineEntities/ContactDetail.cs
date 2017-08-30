using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    
    /// <summary>
    /// <para>Contact details Information Entity, Associative Entity for Customer</para>
    /// <para>Author: Akash Jainr</para>
    /// <para>Date: 15/02/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC ,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class ContactDetail : EntityBase
    {
        private string _emailAddress;
        private string _dayContactNumber;
        private string _eveningContactNumber;
        private string _mobileContactNumber;
        
        public static readonly ContactDetail Empty = new ContactDetail(true);

        private ContactDetail(bool isEmpty)
            : base(isEmpty)
        {
        }

        [DataMember]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        [DataMember]
        public string DayContactNumber
        {
            get { return _dayContactNumber; }
            set { _dayContactNumber = value; }
        }

        [DataMember]
        public string EveningContactNumber
        {
            get { return _eveningContactNumber; }
            set { _eveningContactNumber = value; }
        }

        [DataMember]
        public string MobileContactNumber
        {
            get { return _mobileContactNumber; }
            set { _mobileContactNumber = value; }
        }
    }
}
