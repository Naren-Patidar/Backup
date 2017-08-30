using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    [KnownType(typeof(CouponLineTextInfo[]))]
    
    public class CouponClass : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        
        private string AlphaCodeField;

        
        private string CouponDescriptionField;

        
        private byte[] CouponImageFullField;

        
        private byte[] CouponImageThumbnailField;

        
        private string EANBarcodeField;

        
        private string FullImageNameField;

        
        private bool IsGenerateSmartCodesField;

        
        private string IssuanceChannelField;

        
        private System.Nullable<System.DateTime> IssuanceEndDateField;

        
        private System.Nullable<System.DateTime> IssuanceEndTimeField;

        
        private System.Nullable<System.DateTime> IssuanceStartDateField;

        
        private System.Nullable<System.DateTime> IssuanceStartTimeField;

        
        private CouponLineTextInfo[] ListCouponLineInfoField;

        
        private System.Nullable<short> MaxRedemptionLimitField;

        
        private string RedemptionChannelField;

        
        private System.Nullable<System.DateTime> RedemptionEndDateField;

        
        private string StatementNumberField;

        
        private string ThumbnailImageNameField;

        
        private string TillCouponTemplateNumberField;

        
        private System.Nullable<short> TriggerNumberField;

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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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
}