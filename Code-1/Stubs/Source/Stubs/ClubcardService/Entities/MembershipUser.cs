using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CustomerService
{
    [DataContract]
    
    public class MembershipUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string _CommentField;

        private System.DateTime _CreationDateField;

        private string _EmailField;

        private bool _IsApprovedField;

        private bool _IsLockedOutField;

        private System.DateTime _LastActivityDateField;

        private System.DateTime _LastLockoutDateField;

        private System.DateTime _LastLoginDateField;

        private System.DateTime _LastPasswordChangedDateField;

        private string _PasswordQuestionField;

        private string _ProviderNameField;

        private object _ProviderUserKeyField;

        private string _UserNameField;

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
        public string _Comment
        {
            get
            {
                return this._CommentField;
            }
            set
            {
                if ((object.ReferenceEquals(this._CommentField, value) != true))
                {
                    this._CommentField = value;
                    this.RaisePropertyChanged("_Comment");
                }
            }
        }

        [DataMember]
        public System.DateTime _CreationDate
        {
            get
            {
                return this._CreationDateField;
            }
            set
            {
                if ((this._CreationDateField.Equals(value) != true))
                {
                    this._CreationDateField = value;
                    this.RaisePropertyChanged("_CreationDate");
                }
            }
        }

        [DataMember]
        public string _Email
        {
            get
            {
                return this._EmailField;
            }
            set
            {
                if ((object.ReferenceEquals(this._EmailField, value) != true))
                {
                    this._EmailField = value;
                    this.RaisePropertyChanged("_Email");
                }
            }
        }

        [DataMember]
        public bool _IsApproved
        {
            get
            {
                return this._IsApprovedField;
            }
            set
            {
                if ((this._IsApprovedField.Equals(value) != true))
                {
                    this._IsApprovedField = value;
                    this.RaisePropertyChanged("_IsApproved");
                }
            }
        }

        [DataMember]
        public bool _IsLockedOut
        {
            get
            {
                return this._IsLockedOutField;
            }
            set
            {
                if ((this._IsLockedOutField.Equals(value) != true))
                {
                    this._IsLockedOutField = value;
                    this.RaisePropertyChanged("_IsLockedOut");
                }
            }
        }

        [DataMember]
        public System.DateTime _LastActivityDate
        {
            get
            {
                return this._LastActivityDateField;
            }
            set
            {
                if ((this._LastActivityDateField.Equals(value) != true))
                {
                    this._LastActivityDateField = value;
                    this.RaisePropertyChanged("_LastActivityDate");
                }
            }
        }

        [DataMember]
        public System.DateTime _LastLockoutDate
        {
            get
            {
                return this._LastLockoutDateField;
            }
            set
            {
                if ((this._LastLockoutDateField.Equals(value) != true))
                {
                    this._LastLockoutDateField = value;
                    this.RaisePropertyChanged("_LastLockoutDate");
                }
            }
        }

        [DataMember]
        public System.DateTime _LastLoginDate
        {
            get
            {
                return this._LastLoginDateField;
            }
            set
            {
                if ((this._LastLoginDateField.Equals(value) != true))
                {
                    this._LastLoginDateField = value;
                    this.RaisePropertyChanged("_LastLoginDate");
                }
            }
        }

        [DataMember]
        public System.DateTime _LastPasswordChangedDate
        {
            get
            {
                return this._LastPasswordChangedDateField;
            }
            set
            {
                if ((this._LastPasswordChangedDateField.Equals(value) != true))
                {
                    this._LastPasswordChangedDateField = value;
                    this.RaisePropertyChanged("_LastPasswordChangedDate");
                }
            }
        }

        [DataMember]
        public string _PasswordQuestion
        {
            get
            {
                return this._PasswordQuestionField;
            }
            set
            {
                if ((object.ReferenceEquals(this._PasswordQuestionField, value) != true))
                {
                    this._PasswordQuestionField = value;
                    this.RaisePropertyChanged("_PasswordQuestion");
                }
            }
        }

        [DataMember]
        public string _ProviderName
        {
            get
            {
                return this._ProviderNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this._ProviderNameField, value) != true))
                {
                    this._ProviderNameField = value;
                    this.RaisePropertyChanged("_ProviderName");
                }
            }
        }

        [DataMember]
        public object _ProviderUserKey
        {
            get
            {
                return this._ProviderUserKeyField;
            }
            set
            {
                if ((object.ReferenceEquals(this._ProviderUserKeyField, value) != true))
                {
                    this._ProviderUserKeyField = value;
                    this.RaisePropertyChanged("_ProviderUserKey");
                }
            }
        }

        [DataMember]
        public string _UserName
        {
            get
            {
                return this._UserNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this._UserNameField, value) != true))
                {
                    this._UserNameField = value;
                    this.RaisePropertyChanged("_UserName");
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