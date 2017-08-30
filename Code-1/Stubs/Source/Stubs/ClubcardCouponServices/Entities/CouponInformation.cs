using System.Runtime.Serialization;



    [DataContract(Name = "CouponInformation", Namespace = "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract")]
    public partial class CouponInformation : CouponClass
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> CouponIssuanceChannelField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CouponStatusIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HouseholdIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> IssuanceDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> IssuanceStoreField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RedemptionInfo[] ListRedemptionInfoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short RedemptionUtilizedField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SmartAlphaNumericField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SmartBarcodeField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> CouponIssuanceChannel
        {
            get
            {
                return this.CouponIssuanceChannelField;
            }
            set
            {
                if ((this.CouponIssuanceChannelField.Equals(value) != true))
                {
                    this.CouponIssuanceChannelField = value;
                    this.RaisePropertyChanged("CouponIssuanceChannel");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> CouponStatusId
        {
            get
            {
                return this.CouponStatusIdField;
            }
            set
            {
                if ((this.CouponStatusIdField.Equals(value) != true))
                {
                    this.CouponStatusIdField = value;
                    this.RaisePropertyChanged("CouponStatusId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HouseholdId
        {
            get
            {
                return this.HouseholdIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.HouseholdIdField, value) != true))
                {
                    this.HouseholdIdField = value;
                    this.RaisePropertyChanged("HouseholdId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> IssuanceDate
        {
            get
            {
                return this.IssuanceDateField;
            }
            set
            {
                if ((this.IssuanceDateField.Equals(value) != true))
                {
                    this.IssuanceDateField = value;
                    this.RaisePropertyChanged("IssuanceDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> IssuanceStore
        {
            get
            {
                return this.IssuanceStoreField;
            }
            set
            {
                if ((this.IssuanceStoreField.Equals(value) != true))
                {
                    this.IssuanceStoreField = value;
                    this.RaisePropertyChanged("IssuanceStore");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public RedemptionInfo[] ListRedemptionInfo
        {
            get
            {
                return this.ListRedemptionInfoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ListRedemptionInfoField, value) != true))
                {
                    this.ListRedemptionInfoField = value;
                    this.RaisePropertyChanged("ListRedemptionInfo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public short RedemptionUtilized
        {
            get
            {
                return this.RedemptionUtilizedField;
            }
            set
            {
                if ((this.RedemptionUtilizedField.Equals(value) != true))
                {
                    this.RedemptionUtilizedField = value;
                    this.RaisePropertyChanged("RedemptionUtilized");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SmartAlphaNumeric
        {
            get
            {
                return this.SmartAlphaNumericField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SmartAlphaNumericField, value) != true))
                {
                    this.SmartAlphaNumericField = value;
                    this.RaisePropertyChanged("SmartAlphaNumeric");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SmartBarcode
        {
            get
            {
                return this.SmartBarcodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SmartBarcodeField, value) != true))
                {
                    this.SmartBarcodeField = value;
                    this.RaisePropertyChanged("SmartBarcode");
                }
            }
        }
    }
