using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    [KnownType(typeof(ConsumerSource))]
    
    public class CustomerCouponRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string DotComIDField;

        
        private ConsumerSource SourceField;

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
        public string DotComID
        {
            get
            {
                return this.DotComIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DotComIDField, value) != true))
                {
                    this.DotComIDField = value;
                    this.RaisePropertyChanged("DotComID");
                }
            }
        }

        [DataMember]
        public ConsumerSource Source
        {
            get
            {
                return this.SourceField;
            }
            set
            {
                if ((this.SourceField.Equals(value) != true))
                {
                    this.SourceField = value;
                    this.RaisePropertyChanged("Source");
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