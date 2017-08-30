using System.Runtime.Serialization;


    [DataContract(Name = "CouponLineTextInfo", Namespace = "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract")]
    public partial class CouponLineTextInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char BarcodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char CenterField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte CharacterWeigthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte CharacterWidthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char ItalicField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LineNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LineTextField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char LineUsedField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char UnderLineField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private char WhiteOnBlackField;

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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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

        [System.Runtime.Serialization.DataMemberAttribute()]
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
