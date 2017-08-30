using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages")]
    public partial class ResponseBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorLogIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorStatusCodeField;

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
        public string ErrorLogID
        {
            get
            {
                return this.ErrorLogIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ErrorLogIDField, value) != true))
                {
                    this.ErrorLogIDField = value;
                    this.RaisePropertyChanged("ErrorLogID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorStatusCode
        {
            get
            {
                return this.ErrorStatusCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ErrorStatusCodeField, value) != true))
                {
                    this.ErrorStatusCodeField = value;
                    this.RaisePropertyChanged("ErrorStatusCode");
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