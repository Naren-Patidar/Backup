using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public partial class GetFuelBalanceResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string DaysLeftForFuelBalanceExpiryField;

        private GetBalanceResponse GetBalanceResponseField;

        private bool IsAccountExistField;

        private int StatusCodeField;

        private string StatusMessageField;

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
        public string DaysLeftForFuelBalanceExpiry
        {
            get
            {
                return this.DaysLeftForFuelBalanceExpiryField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DaysLeftForFuelBalanceExpiryField, value) != true))
                {
                    this.DaysLeftForFuelBalanceExpiryField = value;
                    this.RaisePropertyChanged("DaysLeftForFuelBalanceExpiry");
                }
            }
        }

        [DataMember]
        public GetBalanceResponse GetBalanceResponse
        {
            get
            {
                return this.GetBalanceResponseField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GetBalanceResponseField, value) != true))
                {
                    this.GetBalanceResponseField = value;
                    this.RaisePropertyChanged("GetBalanceResponse");
                }
            }
        }

        [DataMember]
        public bool IsAccountExist
        {
            get
            {
                return this.IsAccountExistField;
            }
            set
            {
                if ((this.IsAccountExistField.Equals(value) != true))
                {
                    this.IsAccountExistField = value;
                    this.RaisePropertyChanged("IsAccountExist");
                }
            }
        }

        [DataMember]
        public int StatusCode
        {
            get
            {
                return this.StatusCodeField;
            }
            set
            {
                if ((this.StatusCodeField.Equals(value) != true))
                {
                    this.StatusCodeField = value;
                    this.RaisePropertyChanged("StatusCode");
                }
            }
        }

        [DataMember]
        public string StatusMessage
        {
            get
            {
                return this.StatusMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StatusMessageField, value) != true))
                {
                    this.StatusMessageField = value;
                    this.RaisePropertyChanged("StatusMessage");
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