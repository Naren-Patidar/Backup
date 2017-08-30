using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public partial class GetFuelBalanceRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string AccountIDField;

        private AccountIDType AccountIdTypeField;

    
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
        public string AccountID
        {
            get
            {
                return this.AccountIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AccountIDField, value) != true))
                {
                    this.AccountIDField = value;
                    this.RaisePropertyChanged("AccountID");
                }
            }
        }

        [DataMember]
        public AccountIDType AccountIdType
        {
            get
            {
                return this.AccountIdTypeField;
            }
            set
            {
                if ((this.AccountIdTypeField.Equals(value) != true))
                {
                    this.AccountIdTypeField = value;
                    this.RaisePropertyChanged("AccountIdType");
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