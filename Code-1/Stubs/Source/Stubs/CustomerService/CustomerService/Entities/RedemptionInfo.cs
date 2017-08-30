using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    [KnownType(typeof(RedeemType))]
    
    public class RedemptionInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        
        private string CashierNumberField;

        
        private System.Nullable<short> ChannelCodeField;

        
        private string ClubcardNumberField;

        
        private bool IsOfflineField;

        
        private System.DateTime RedemptionDateTimeField;

        
        private RedeemType RedemptionTypeField;

        
        private System.Nullable<short> StoreNumberField;

        
        private System.Nullable<short> TillIdField;

        
        private System.Nullable<short> TillTypeField;

        [IgnoreDataMember]
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
        public string CashierNumber
        {
            get
            {
                return this.CashierNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CashierNumberField, value) != true))
                {
                    this.CashierNumberField = value;
                    this.RaisePropertyChanged("CashierNumber");
                }
            }
        }

        [DataMember]
        public System.Nullable<short> ChannelCode
        {
            get
            {
                return this.ChannelCodeField;
            }
            set
            {
                if ((this.ChannelCodeField.Equals(value) != true))
                {
                    this.ChannelCodeField = value;
                    this.RaisePropertyChanged("ChannelCode");
                }
            }
        }

        [DataMember]
        public string ClubcardNumber
        {
            get
            {
                return this.ClubcardNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClubcardNumberField, value) != true))
                {
                    this.ClubcardNumberField = value;
                    this.RaisePropertyChanged("ClubcardNumber");
                }
            }
        }

        [DataMember]
        public bool IsOffline
        {
            get
            {
                return this.IsOfflineField;
            }
            set
            {
                if ((this.IsOfflineField.Equals(value) != true))
                {
                    this.IsOfflineField = value;
                    this.RaisePropertyChanged("IsOffline");
                }
            }
        }

        [DataMember]
        public System.DateTime RedemptionDateTime
        {
            get
            {
                return this.RedemptionDateTimeField;
            }
            set
            {
                if ((this.RedemptionDateTimeField.Equals(value) != true))
                {
                    this.RedemptionDateTimeField = value;
                    this.RaisePropertyChanged("RedemptionDateTime");
                }
            }
        }

        [DataMember]
        public RedeemType RedemptionType
        {
            get
            {
                return this.RedemptionTypeField;
            }
            set
            {
                if ((this.RedemptionTypeField.Equals(value) != true))
                {
                    this.RedemptionTypeField = value;
                    this.RaisePropertyChanged("RedemptionType");
                }
            }
        }

        [DataMember]
        public System.Nullable<short> StoreNumber
        {
            get
            {
                return this.StoreNumberField;
            }
            set
            {
                if ((this.StoreNumberField.Equals(value) != true))
                {
                    this.StoreNumberField = value;
                    this.RaisePropertyChanged("StoreNumber");
                }
            }
        }

        [DataMember]
        public System.Nullable<short> TillId
        {
            get
            {
                return this.TillIdField;
            }
            set
            {
                if ((this.TillIdField.Equals(value) != true))
                {
                    this.TillIdField = value;
                    this.RaisePropertyChanged("TillId");
                }
            }
        }

        [DataMember]
        public System.Nullable<short> TillType
        {
            get
            {
                return this.TillTypeField;
            }
            set
            {
                if ((this.TillTypeField.Equals(value) != true))
                {
                    this.TillTypeField = value;
                    this.RaisePropertyChanged("TillType");
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