using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Household Entity</para>
    /// <para>Purpose: To store household information</para>
    /// <para>Author: Akash Jain</para>
    /// <para>Date: 15/04/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC Pte Ltd,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class Household : EntityBase
    {
        private int _totalPeople;
        private int[] _peopleAges;
        
        public static readonly Household Empty = new Household(true);

        private Household(bool isEmpty)
            : base(isEmpty)
        {
        }

        [DataMember]
        public int TotalPeople { get { return _totalPeople; } set { _totalPeople = value; } }

        [DataMember]
        public int[] PeopleAges { get { return _peopleAges; } set { _peopleAges = value; } }
    }
}
