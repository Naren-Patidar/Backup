using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public partial class Balance : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private BalanceByExpiry[] BalanceByExpiryField;

        private decimal ReservedBalanceField;

        private decimal TotalBalanceField;

        
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
        public BalanceByExpiry[] BalanceByExpiry
        {
            get
            {
                return this.BalanceByExpiryField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BalanceByExpiryField, value) != true))
                {
                    this.BalanceByExpiryField = value;
                    this.RaisePropertyChanged("BalanceByExpiry");
                }
            }
        }

        [DataMember]
        public decimal ReservedBalance
        {
            get
            {
                return this.ReservedBalanceField;
            }
            set
            {
                if ((this.ReservedBalanceField.Equals(value) != true))
                {
                    this.ReservedBalanceField = value;
                    this.RaisePropertyChanged("ReservedBalance");
                }
            }
        }

        [DataMember]
        public decimal TotalBalance
        {
            get
            {
                return this.TotalBalanceField;
            }
            set
            {
                if ((this.TotalBalanceField.Equals(value) != true))
                {
                    this.TotalBalanceField = value;
                    this.RaisePropertyChanged("TotalBalance");
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