using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardService
{
    [DataContract]
    public partial class GetBalanceResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string AccountIdField;

        private string AccountTypeField;

        private Balance BalanceField;

   
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
        public string AccountId
        {
            get
            {
                return this.AccountIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AccountIdField, value) != true))
                {
                    this.AccountIdField = value;
                    this.RaisePropertyChanged("AccountId");
                }
            }
        }

        [DataMember]
        public string AccountType
        {
            get
            {
                return this.AccountTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AccountTypeField, value) != true))
                {
                    this.AccountTypeField = value;
                    this.RaisePropertyChanged("AccountType");
                }
            }
        }

        [DataMember]
        public Balance Balance
        {
            get
            {
                return this.BalanceField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BalanceField, value) != true))
                {
                    this.BalanceField = value;
                    this.RaisePropertyChanged("Balance");
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