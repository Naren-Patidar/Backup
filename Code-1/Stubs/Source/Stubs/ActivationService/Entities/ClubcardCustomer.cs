using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]

    public class ClubcardCustomer : EntityBase
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.Address AddressField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.Clubcard[] ClubcardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.ContactDetail ContactDetailField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.ContactPreference ContactPreferenceField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateOfBirthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DayOfBirthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.DietaryPreferences DietaryPreferencesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DotcomCustomerIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GenderField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClubcardOnline.Web.Entities.CustomerActivationServices.Household[] HouseholdsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InitialsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MonthOfBirthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SSNField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SurnameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string YearOfBirthField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.Address Address
        {
            get
            {
                return this.AddressField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressField, value) != true))
                {
                    this.AddressField = value;
                    this.RaisePropertyChanged("Address");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.Clubcard[] Clubcard
        {
            get
            {
                return this.ClubcardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClubcardField, value) != true))
                {
                    this.ClubcardField = value;
                    this.RaisePropertyChanged("Clubcard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.ContactDetail ContactDetail
        {
            get
            {
                return this.ContactDetailField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContactDetailField, value) != true))
                {
                    this.ContactDetailField = value;
                    this.RaisePropertyChanged("ContactDetail");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.ContactPreference ContactPreference
        {
            get
            {
                return this.ContactPreferenceField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContactPreferenceField, value) != true))
                {
                    this.ContactPreferenceField = value;
                    this.RaisePropertyChanged("ContactPreference");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateOfBirth
        {
            get
            {
                return this.DateOfBirthField;
            }
            set
            {
                if ((this.DateOfBirthField.Equals(value) != true))
                {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DayOfBirth
        {
            get
            {
                return this.DayOfBirthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DayOfBirthField, value) != true))
                {
                    this.DayOfBirthField = value;
                    this.RaisePropertyChanged("DayOfBirth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.DietaryPreferences DietaryPreferences
        {
            get
            {
                return this.DietaryPreferencesField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DietaryPreferencesField, value) != true))
                {
                    this.DietaryPreferencesField = value;
                    this.RaisePropertyChanged("DietaryPreferences");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DotcomCustomerID
        {
            get
            {
                return this.DotcomCustomerIDField;
            }
            set
            {
                if ((this.DotcomCustomerIDField.Equals(value) != true))
                {
                    this.DotcomCustomerIDField = value;
                    this.RaisePropertyChanged("DotcomCustomerID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName
        {
            get
            {
                return this.FirstNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true))
                {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gender
        {
            get
            {
                return this.GenderField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GenderField, value) != true))
                {
                    this.GenderField = value;
                    this.RaisePropertyChanged("Gender");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClubcardOnline.Web.Entities.CustomerActivationServices.Household[] Households
        {
            get
            {
                return this.HouseholdsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.HouseholdsField, value) != true))
                {
                    this.HouseholdsField = value;
                    this.RaisePropertyChanged("Households");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Initials
        {
            get
            {
                return this.InitialsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.InitialsField, value) != true))
                {
                    this.InitialsField = value;
                    this.RaisePropertyChanged("Initials");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MonthOfBirth
        {
            get
            {
                return this.MonthOfBirthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MonthOfBirthField, value) != true))
                {
                    this.MonthOfBirthField = value;
                    this.RaisePropertyChanged("MonthOfBirth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SSN
        {
            get
            {
                return this.SSNField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SSNField, value) != true))
                {
                    this.SSNField = value;
                    this.RaisePropertyChanged("SSN");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Surname
        {
            get
            {
                return this.SurnameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SurnameField, value) != true))
                {
                    this.SurnameField = value;
                    this.RaisePropertyChanged("Surname");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TitleField, value) != true))
                {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string YearOfBirth
        {
            get
            {
                return this.YearOfBirthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.YearOfBirthField, value) != true))
                {
                    this.YearOfBirthField = value;
                    this.RaisePropertyChanged("YearOfBirth");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    
}