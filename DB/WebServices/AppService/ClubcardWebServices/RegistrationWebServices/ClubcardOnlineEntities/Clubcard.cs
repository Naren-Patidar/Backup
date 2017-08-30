using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Clubcard Entity</para>
    /// <para>Purpose: To store clubcard information</para>
    /// <para>Author: Akash Jain</para>
    /// <para>Date: 15/04/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class Clubcard : EntityBase
    {        
        private long _clubcardNumber;
        private DateTime _startDate;
        private DateTime _expiryDate;
        private bool _primary;

        public static readonly Clubcard Empty = new Clubcard(true);

        private Clubcard(bool isEmpty)
            : base(isEmpty)
        {
        }

        [DataMember]
        public long ClubcardNumber { get { return _clubcardNumber; } set { _clubcardNumber = value; } }

        [DataMember]
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }

        [DataMember]
        public DateTime ExpiryDate { get { return _expiryDate; } set { _expiryDate = value; } }

        [DataMember]
        public bool Primary { get { return _primary; } set { _primary = value; } }
    }
}
