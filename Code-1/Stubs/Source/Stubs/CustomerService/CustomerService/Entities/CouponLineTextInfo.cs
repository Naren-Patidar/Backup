using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    
    public class CouponLineTextInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        
        private char BarcodeField;

        
        private char CenterField;

        
        private byte CharacterWeigthField;

        
        private byte CharacterWidthField;

        
        private char ItalicField;

        
        private string LineNumberField;

        
        private string LineTextField;

        
        private char LineUsedField;

        
        private char UnderLineField;

        
        private char WhiteOnBlackField;

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
        public char Barcode
        {
            get
            {
                return this.BarcodeField;
            }
            set
            {
                if ((this.BarcodeField.Equals(value) != true))
                {
                    this.BarcodeField = value;
                    this.RaisePropertyChanged("Barcode");
                }
            }
        }

        [DataMember]
        public char Center
        {
            get
            {
                return this.CenterField;
            }
            set
            {
                if ((this.CenterField.Equals(value) != true))
                {
                    this.CenterField = value;
                    this.RaisePropertyChanged("Center");
                }
            }
        }

        [DataMember]
        public byte CharacterWeigth
        {
            get
            {
                return this.CharacterWeigthField;
            }
            set
            {
                if ((this.CharacterWeigthField.Equals(value) != true))
                {
                    this.CharacterWeigthField = value;
                    this.RaisePropertyChanged("CharacterWeigth");
                }
            }
        }

        [DataMember]
        public byte CharacterWidth
        {
            get
            {
                return this.CharacterWidthField;
            }
            set
            {
                if ((this.CharacterWidthField.Equals(value) != true))
                {
                    this.CharacterWidthField = value;
                    this.RaisePropertyChanged("CharacterWidth");
                }
            }
        }

        [DataMember]
        public char Italic
        {
            get
            {
                return this.ItalicField;
            }
            set
            {
                if ((this.ItalicField.Equals(value) != true))
                {
                    this.ItalicField = value;
                    this.RaisePropertyChanged("Italic");
                }
            }
        }

        [DataMember]
        public string LineNumber
        {
            get
            {
                return this.LineNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LineNumberField, value) != true))
                {
                    this.LineNumberField = value;
                    this.RaisePropertyChanged("LineNumber");
                }
            }
        }

        [DataMember]
        public string LineText
        {
            get
            {
                return this.LineTextField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LineTextField, value) != true))
                {
                    this.LineTextField = value;
                    this.RaisePropertyChanged("LineText");
                }
            }
        }

        [DataMember]
        public char LineUsed
        {
            get
            {
                return this.LineUsedField;
            }
            set
            {
                if ((this.LineUsedField.Equals(value) != true))
                {
                    this.LineUsedField = value;
                    this.RaisePropertyChanged("LineUsed");
                }
            }
        }

        [DataMember]
        public char UnderLine
        {
            get
            {
                return this.UnderLineField;
            }
            set
            {
                if ((this.UnderLineField.Equals(value) != true))
                {
                    this.UnderLineField = value;
                    this.RaisePropertyChanged("UnderLine");
                }
            }
        }

        [DataMember]
        public char WhiteOnBlack
        {
            get
            {
                return this.WhiteOnBlackField;
            }
            set
            {
                if ((this.WhiteOnBlackField.Equals(value) != true))
                {
                    this.WhiteOnBlackField = value;
                    this.RaisePropertyChanged("WhiteOnBlack");
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