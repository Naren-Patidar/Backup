using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    [KnownType(typeof(CouponInformation[]))]
    
    public class CouponResponse 
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ActiveCouponField;

        private CouponInformation[] CouponsField;

        private string ErrorMessageField;

        private long HouseHolIdField;

        private bool StatusField;

        private int TotalCouponField;

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
        public int ActiveCoupon
        {
            get
            {
                return this.ActiveCouponField;
            }
            set
            {
                if ((this.ActiveCouponField.Equals(value) != true))
                {
                    this.ActiveCouponField = value;
                    this.RaisePropertyChanged("ActiveCoupon");
                }
            }
        }

        [DataMember]
        public CouponInformation[] Coupons
        {
            get
            {
                return this.CouponsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouponsField, value) != true))
                {
                    this.CouponsField = value;
                    this.RaisePropertyChanged("Coupons");
                }
            }
        }

        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true))
                {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }

        [DataMember]
        public long HouseHolId
        {
            get
            {
                return this.HouseHolIdField;
            }
            set
            {
                if ((this.HouseHolIdField.Equals(value) != true))
                {
                    this.HouseHolIdField = value;
                    this.RaisePropertyChanged("HouseHolId");
                }
            }
        }

        [DataMember]
        public bool Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                if ((this.StatusField.Equals(value) != true))
                {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }

        [DataMember]
        public int TotalCoupon
        {
            get
            {
                return this.TotalCouponField;
            }
            set
            {
                if ((this.TotalCouponField.Equals(value) != true))
                {
                    this.TotalCouponField = value;
                    this.RaisePropertyChanged("TotalCoupon");
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