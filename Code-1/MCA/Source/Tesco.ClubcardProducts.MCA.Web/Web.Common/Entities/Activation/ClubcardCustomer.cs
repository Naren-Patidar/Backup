﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Validator;
using System.ComponentModel;

namespace  Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{

    public class ClubcardCustomer 
    {        
        private Address AddressField = new Address();        
        private Clubcard ClubcardField=new Clubcard();        
        private ContactDetail ContactDetailField = new ContactDetail();        
        private ContactPreference ContactPreferenceField = new ContactPreference();        
        private System.DateTime DateOfBirthField;        
        private string DayOfBirthField;
        private DietaryPreferences DietaryPreferencesField = new DietaryPreferences();        
        private int DotcomCustomerIDField;        
        private string FirstNameField;        
        private string GenderField;        
        private Household HouseholdsField= new Household();        
        private string InitialsField;        
        private string MonthOfBirthField;        
        private string SSNField;        
        private string SurnameField;        
        private string TitleField;        
        private string YearOfBirthField;

        public ClubcardCustomer()
        { }

        public ClubcardCustomer(ClubcardCustomer obj)
        {
            if (obj != null)
            {
                this.Address.PostCode = obj.Address.PostCode;
                this.FirstName = obj.FirstName;
                this.Surname = obj.Surname;
                this.DateOfBirth = obj.DateOfBirth;
                this.DayOfBirth = obj.DayOfBirth;
                this.MonthOfBirth = obj.MonthOfBirth;
                this.YearOfBirth = obj.YearOfBirth;
                this.SSN = obj.SSN;
                this.Title = obj.Title;
                this.Gender = obj.Gender;
                this.Clubcard = obj.Clubcard;
            }
        }

        public Address  Address
        {
            get
            {
                return this.AddressField;
            }
            set
            {
                this.AddressField = value;
            }
        }

        public Clubcard Clubcard
        {
            get
            {
                return this.ClubcardField;
            }
            set
            {
                this.ClubcardField = value;
            }
        }

       
        public ContactDetail ContactDetail
        {
            get
            {
                return this.ContactDetailField;
            }
            set
            {
                this.ContactDetailField = value;
            }
        }

       
        public ContactPreference ContactPreference
        {
            get
            {
                return this.ContactPreferenceField;
            }
            set
            {
                this.ContactPreferenceField = value;
            }
        }

       
        public System.DateTime DateOfBirth
        {
            get
            {
                return this.DateOfBirthField;
            }
            set
            {
                this.DateOfBirthField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_DayOfBirth")]
        public string DayOfBirth
        {
            get
            {
                return this.DayOfBirthField;
            }
            set
            {
                this.DayOfBirthField = value;
            }
        }

       
        public DietaryPreferences DietaryPreferences
        {
            get
            {
                return this.DietaryPreferencesField;
            }
            set
            {
                this.DietaryPreferencesField = value;
            }
        }

       
        public int DotcomCustomerID
        {
            get
            {
                return this.DotcomCustomerIDField;
            }
            set
            {
                this.DotcomCustomerIDField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_FirstName")]
        public string FirstName
        {
            get
            {
                return this.FirstNameField;
            }
            set
            {
                this.FirstNameField = value;
            }
        }

       
        public string Gender
        {
            get
            {
                return this.GenderField;
            }
            set
            {
                this.GenderField=value;
            }
        }

       
        public Household Households
        {
            get
            {
                return this.HouseholdsField;
            }
            set
            {
                this.HouseholdsField = value;
            }
        }

       
        public string Initials
        {
            get
            {
                return this.InitialsField;
            }
            set
            {
                this.InitialsField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_MonthOfBirth")]
        public string MonthOfBirth
        {
            get
            {
                return this.MonthOfBirthField;
            }
            set
            {
                this.MonthOfBirthField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_SSN")]
        public string SSN
        {
            get
            {
                return this.SSNField;
            }
            set
            {
                this.SSNField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_LastName")]
        public string Surname
        {
            get
            {
                return this.SurnameField;
            }
            set
            {
                this.SurnameField = value;
            }
        }

       
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                this.TitleField = value;
            }
        }

        [Validators]
        [DisplayName("Activation_YearOfBirth")]
        public string YearOfBirth
        {
            get
            {
                return this.YearOfBirthField;
            }
            set
            {
                this.YearOfBirthField = value;
            }
        }

      }
}
