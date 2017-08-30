using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    [KnownType(typeof(RedemptionInfo[]))]
    public class CouponInformation : System.ComponentModel.INotifyPropertyChanged
    {

        private System.Nullable<short> CouponIssuanceChannelField;

        private System.Nullable<int> CouponStatusIdField;

        private string HouseholdIdField;

        private System.Nullable<System.DateTime> IssuanceDateField;

        private System.Nullable<short> IssuanceStoreField;

        private RedemptionInfo[] ListRedemptionInfoField;

        private short RedemptionUtilizedField;

        private string SmartAlphaNumericField;

        private string SmartBarcodeField;

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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