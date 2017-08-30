using System.Runtime.Serialization;




    [DataContract(Name = "CouponClass", Namespace = "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract")]
    [KnownType(typeof(CouponInformation))]
    public class CouponClass : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AlphaCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CouponDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] CouponImageFullField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] CouponImageThumbnailField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EANBarcodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FullImageNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsGenerateSmartCodesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IssuanceChannelField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> IssuanceEndDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> IssuanceEndTimeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> IssuanceStartDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> IssuanceStartTimeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private CouponLineTextInfo[] ListCouponLineInfoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> MaxRedemptionLimitField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RedemptionChannelField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> RedemptionEndDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatementNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ThumbnailImageNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TillCouponTemplateNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> TriggerNumberField;

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
        public string AlphaCode
        {
            get
            {
                return this.AlphaCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AlphaCodeField, value) != true))
                {
                    this.AlphaCodeField = value;
                    this.RaisePropertyChanged("AlphaCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CouponDescription
        {
            get
            {
                return this.CouponDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouponDescriptionField, value) != true))
                {
                    this.CouponDescriptionField = value;
                    this.RaisePropertyChanged("CouponDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] CouponImageFull
        {
            get
            {
                return this.CouponImageFullField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouponImageFullField, value) != true))
                {
                    this.CouponImageFullField = value;
                    this.RaisePropertyChanged("CouponImageFull");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] CouponImageThumbnail
        {
            get
            {
                return this.CouponImageThumbnailField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouponImageThumbnailField, value) != true))
                {
                    this.CouponImageThumbnailField = value;
                    this.RaisePropertyChanged("CouponImageThumbnail");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EANBarcode
        {
            get
            {
                return this.EANBarcodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EANBarcodeField, value) != true))
                {
                    this.EANBarcodeField = value;
                    this.RaisePropertyChanged("EANBarcode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FullImageName
        {
            get
            {
                return this.FullImageNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.FullImageNameField, value) != true))
                {
                    this.FullImageNameField = value;
                    this.RaisePropertyChanged("FullImageName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsGenerateSmartCodes
        {
            get
            {
                return this.IsGenerateSmartCodesField;
            }
            set
            {
                if ((this.IsGenerateSmartCodesField.Equals(value) != true))
                {
                    this.IsGenerateSmartCodesField = value;
                    this.RaisePropertyChanged("IsGenerateSmartCodes");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IssuanceChannel
        {
            get
            {
                return this.IssuanceChannelField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IssuanceChannelField, value) != true))
                {
                    this.IssuanceChannelField = value;
                    this.RaisePropertyChanged("IssuanceChannel");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> IssuanceEndDate
        {
            get
            {
                return this.IssuanceEndDateField;
            }
            set
            {
                if ((this.IssuanceEndDateField.Equals(value) != true))
                {
                    this.IssuanceEndDateField = value;
                    this.RaisePropertyChanged("IssuanceEndDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> IssuanceEndTime
        {
            get
            {
                return this.IssuanceEndTimeField;
            }
            set
            {
                if ((this.IssuanceEndTimeField.Equals(value) != true))
                {
                    this.IssuanceEndTimeField = value;
                    this.RaisePropertyChanged("IssuanceEndTime");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> IssuanceStartDate
        {
            get
            {
                return this.IssuanceStartDateField;
            }
            set
            {
                if ((this.IssuanceStartDateField.Equals(value) != true))
                {
                    this.IssuanceStartDateField = value;
                    this.RaisePropertyChanged("IssuanceStartDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> IssuanceStartTime
        {
            get
            {
                return this.IssuanceStartTimeField;
            }
            set
            {
                if ((this.IssuanceStartTimeField.Equals(value) != true))
                {
                    this.IssuanceStartTimeField = value;
                    this.RaisePropertyChanged("IssuanceStartTime");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public CouponLineTextInfo[] ListCouponLineInfo
        {
            get
            {
                return this.ListCouponLineInfoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ListCouponLineInfoField, value) != true))
                {
                    this.ListCouponLineInfoField = value;
                    this.RaisePropertyChanged("ListCouponLineInfo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> MaxRedemptionLimit
        {
            get
            {
                return this.MaxRedemptionLimitField;
            }
            set
            {
                if ((this.MaxRedemptionLimitField.Equals(value) != true))
                {
                    this.MaxRedemptionLimitField = value;
                    this.RaisePropertyChanged("MaxRedemptionLimit");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RedemptionChannel
        {
            get
            {
                return this.RedemptionChannelField;
            }
            set
            {
                if ((object.ReferenceEquals(this.RedemptionChannelField, value) != true))
                {
                    this.RedemptionChannelField = value;
                    this.RaisePropertyChanged("RedemptionChannel");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> RedemptionEndDate
        {
            get
            {
                return this.RedemptionEndDateField;
            }
            set
            {
                if ((this.RedemptionEndDateField.Equals(value) != true))
                {
                    this.RedemptionEndDateField = value;
                    this.RaisePropertyChanged("RedemptionEndDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StatementNumber
        {
            get
            {
                return this.StatementNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StatementNumberField, value) != true))
                {
                    this.StatementNumberField = value;
                    this.RaisePropertyChanged("StatementNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ThumbnailImageName
        {
            get
            {
                return this.ThumbnailImageNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ThumbnailImageNameField, value) != true))
                {
                    this.ThumbnailImageNameField = value;
                    this.RaisePropertyChanged("ThumbnailImageName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TillCouponTemplateNumber
        {
            get
            {
                return this.TillCouponTemplateNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TillCouponTemplateNumberField, value) != true))
                {
                    this.TillCouponTemplateNumberField = value;
                    this.RaisePropertyChanged("TillCouponTemplateNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> TriggerNumber
        {
            get
            {
                return this.TriggerNumberField;
            }
            set
            {
                if ((this.TriggerNumberField.Equals(value) != true))
                {
                    this.TriggerNumberField = value;
                    this.RaisePropertyChanged("TriggerNumber");
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
