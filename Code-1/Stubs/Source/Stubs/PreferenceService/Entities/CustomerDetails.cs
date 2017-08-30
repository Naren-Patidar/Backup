using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [DataContract]
    public class CustomerDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string CardNumberField;

        private string EmailIdField;

        private string FirstnameField;

        private System.Collections.Generic.List<string> PreferencelistField;

        private string SurnameField;

        private string TitleField;

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

        [DataMember]
        public string CardNumber
        {
            get
            {
                return this.CardNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CardNumberField, value) != true))
                {
                    this.CardNumberField = value;
                    this.RaisePropertyChanged("CardNumber");
                }
            }
        }

        [DataMember]
        public string EmailId
        {
            get
            {
                return this.EmailIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EmailIdField, value) != true))
                {
                    this.EmailIdField = value;
                    this.RaisePropertyChanged("EmailId");
                }
            }
        }

        [DataMember]
        public string Firstname
        {
            get
            {
                return this.FirstnameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FirstnameField, value) != true))
                {
                    this.FirstnameField = value;
                    this.RaisePropertyChanged("Firstname");
                }
            }
        }
        
        [DataMember]
        public System.Collections.Generic.List<string> Preferencelist
        {
            get
            {
                return this.PreferencelistField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferencelistField, value) != true))
                {
                    this.PreferencelistField = value;
                    this.RaisePropertyChanged("Preferencelist");
                }
            }
        }

        [DataMember]
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

        [DataMember]
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