
using System.Runtime.Serialization;

    [DataContract(Name="RedemptionInfo", Namespace = "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract")]
    public class RedemptionInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CashierNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> ChannelCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClubcardNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsOfflineField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime RedemptionDateTimeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RedeemType RedemptionTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> StoreNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> TillIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> TillTypeField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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
