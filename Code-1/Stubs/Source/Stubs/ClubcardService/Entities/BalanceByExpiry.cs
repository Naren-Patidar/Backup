using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public partial class BalanceByExpiry : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private decimal BalanceField;

        private System.Nullable<System.DateTime> ExpiryField;

        
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
        public decimal Balance
        {
            get
            {
                return this.BalanceField;
            }
            set
            {
                if ((this.BalanceField.Equals(value) != true))
                {
                    this.BalanceField = value;
                    this.RaisePropertyChanged("Balance");
                }
            }
        }

        [DataMember]
        public System.Nullable<System.DateTime> Expiry
        {
            get
            {
                return this.ExpiryField;
            }
            set
            {
                if ((this.ExpiryField.Equals(value) != true))
                {
                    this.ExpiryField = value;
                    this.RaisePropertyChanged("Expiry");
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