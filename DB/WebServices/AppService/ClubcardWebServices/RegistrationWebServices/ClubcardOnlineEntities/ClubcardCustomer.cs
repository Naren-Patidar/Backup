using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tesco.com.ClubcardOnline.Entities
{
    /// <summary>
    /// <para>Clubcard Customer entity</para>
    /// <para>Author: Akash Jainr</para>
    /// <para>Date: 15/02/2010</para>
    /// <para>Copyrights (C) 2010, Tesco HSC ,81-82, EPIP Area, WhiteFiled, Bangalore-66</para>
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClubcardCustomer
    {
        private int _dotcomCustomerID;
        private string _title;
        private string _firstName;
        private string _initials;
        private string _surname;
        private DateTime _dateOfBirth;
        private string _dayOfBirth;
        private string _monthOfBirth;
        private string _yearOfBirth;
        private string _ssN;
        private string _gender;
        private ContactPreference _contactPreference = ContactPreference.Empty;
        private Address _address = Address.Empty;
        private DietaryPreferences _dietaryPreferences = DietaryPreferences.Empty;
        private ContactDetail _contactDetail = ContactDetail.Empty;
        private Clubcard[] _clubcard;
        private Household[] _households;

        [DataMember]
        public int DotcomCustomerID
        {
            get { return _dotcomCustomerID; }
            set { _dotcomCustomerID = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        [DataMember]
        public string Initials
        {
            get { return _initials; }
            set { _initials = value; }
        }
        [DataMember]
        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        [DataMember]
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }


        [DataMember]
        public String MonthOfBirth
        {
            get { return _monthOfBirth; }
            set { _monthOfBirth = value; }
        }

        [DataMember]
        public String DayOfBirth
        {
            get { return _dayOfBirth; }
            set { _dayOfBirth = value; }
        }

        [DataMember]
        public String YearOfBirth
        {
            get { return _yearOfBirth; }
            set { _yearOfBirth = value; }
        }
        [DataMember]
        public String SSN
        {
            get { return _ssN; }
            set { _ssN = value; }
        }

        [DataMember]
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        [DataMember]
        public ContactPreference ContactPreference
        {
            get { return this._contactPreference; }
            set { this._contactPreference = value; }
        }

        [DataMember]
        public Address Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        [DataMember]
        public DietaryPreferences DietaryPreferences
        {
            get { return this._dietaryPreferences; }
            set { this._dietaryPreferences = value; }
        }

        [DataMember]
        public ContactDetail ContactDetail
        {
            get { return this._contactDetail; }
            set { this._contactDetail = value; }
        }

        [DataMember]
        public Clubcard[] Clubcard
        {
            get { return this._clubcard; }
            set { this._clubcard = value; }
        }

        [DataMember]
        public Household[] Households
        {
            get { return this._households; }
            set { this._households = value; }
        }
    }
}
