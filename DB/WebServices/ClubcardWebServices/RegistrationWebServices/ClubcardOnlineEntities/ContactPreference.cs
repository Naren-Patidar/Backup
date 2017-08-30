using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Contact Preference Entity</para>
    /// <para>Purpose: To store contact preference information</para>
    /// <para>Author: Akash Jain</para>
    /// <para>Date: 15/04/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class ContactPreference : EntityBase
    {
        private string _wantTescoInfo;
        private string _wantPartnerInfo;
        private string _researchContactable;
        private string _contactable;
        
        public static readonly ContactPreference Empty = new ContactPreference(true);

        private ContactPreference(bool isEmpty)
            : base(isEmpty)
        {
        }

        [DataMember]
        public string WantTescoInfo { get { return _wantTescoInfo; } set { _wantTescoInfo = value; } }

        [DataMember]
        public string WantPartnerInfo { get { return _wantPartnerInfo; } set { _wantPartnerInfo = value; } }

        [DataMember]
        public string ResearchContactable { get { return _researchContactable; } set { _researchContactable = value; } }

        [DataMember]
        public string Contactable { get { return _contactable; } set { _contactable = value; } }



    }
}
