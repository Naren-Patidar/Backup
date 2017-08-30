using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [DataContract]
    [KnownType(typeof(System.Collections.Generic.List<CustomerPreference>))]
    public class CustomerPreference : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string CultureField;
        
        private long CustomerIDField;
        
        private short CustomerPreferenceTypeField;
        
        private string EmailSubjectField;

        private string IsDeletedField;
        
        private OptStatus POptStatusField;
        
        private System.Collections.Generic.List<CustomerPreference> PreferenceField;
        
        private string PreferenceDescriptionEngField;
        
        private string PreferenceDescriptionLocalField;
        
        private short PreferenceIDField;
        
        private string PreferenceOptStatusField;
        
        private System.Collections.Generic.List<CustomerPreference> PreferenceTypeField;
        
        private short SortseqField;
        
        private System.DateTime UpdateDateTimeField;
        
        private string UserIDField;        
        
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
        public string Culture
        {
            get
            {
                return this.CultureField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CultureField, value) != true))
                {
                    this.CultureField = value;
                    this.RaisePropertyChanged("Culture");
                }
            }
        }

        [DataMember]
        public long CustomerID
        {
            get
            {
                return this.CustomerIDField;
            }
            set
            {
                if ((this.CustomerIDField.Equals(value) != true))
                {
                    this.CustomerIDField = value;
                    this.RaisePropertyChanged("CustomerID");
                }
            }
        }

        [DataMember]
        public short CustomerPreferenceType
        {
            get
            {
                return this.CustomerPreferenceTypeField;
            }
            set
            {
                if ((this.CustomerPreferenceTypeField.Equals(value) != true))
                {
                    this.CustomerPreferenceTypeField = value;
                    this.RaisePropertyChanged("CustomerPreferenceType");
                }
            }
        }

        [DataMember]
        public string EmailSubject
        {
            get
            {
                return this.EmailSubjectField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EmailSubjectField, value) != true))
                {
                    this.EmailSubjectField = value;
                    this.RaisePropertyChanged("EmailSubject");
                }
            }
        }

        [DataMember]
        public string IsDeleted
        {
            get
            {
                return this.IsDeletedField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsDeletedField, value) != true))
                {
                    this.IsDeletedField = value;
                    this.RaisePropertyChanged("IsDeleted");
                }
            }
        }

        [DataMember]
        public OptStatus POptStatus
        {
            get
            {
                return this.POptStatusField;
            }
            set
            {
                if ((this.POptStatusField.Equals(value) != true))
                {
                    this.POptStatusField = value;
                    this.RaisePropertyChanged("POptStatus");
                }
            }
        }

        [DataMemberAttribute]
        public System.Collections.Generic.List<CustomerPreference> Preference
        {
            get
            {
                return this.PreferenceField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferenceField, value) != true))
                {
                    this.PreferenceField = value;
                    this.RaisePropertyChanged("Preference");
                }
            }
        }

        [DataMember]
        public string PreferenceDescriptionEng
        {
            get
            {
                return this.PreferenceDescriptionEngField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferenceDescriptionEngField, value) != true))
                {
                    this.PreferenceDescriptionEngField = value;
                    this.RaisePropertyChanged("PreferenceDescriptionEng");
                }
            }
        }

        [DataMember]
        public string PreferenceDescriptionLocal
        {
            get
            {
                return this.PreferenceDescriptionLocalField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferenceDescriptionLocalField, value) != true))
                {
                    this.PreferenceDescriptionLocalField = value;
                    this.RaisePropertyChanged("PreferenceDescriptionLocal");
                }
            }
        }

        [DataMember]
        public short PreferenceID
        {
            get
            {
                return this.PreferenceIDField;
            }
            set
            {
                if ((this.PreferenceIDField.Equals(value) != true))
                {
                    this.PreferenceIDField = value;
                    this.RaisePropertyChanged("PreferenceID");
                }
            }
        }

        [DataMember]
        public string PreferenceOptStatus
        {
            get
            {
                return this.PreferenceOptStatusField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferenceOptStatusField, value) != true))
                {
                    this.PreferenceOptStatusField = value;
                    this.RaisePropertyChanged("PreferenceOptStatus");
                }
            }
        }

        [DataMember]
        public System.Collections.Generic.List<CustomerPreference> PreferenceType
        {
            get
            {
                return this.PreferenceTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PreferenceTypeField, value) != true))
                {
                    this.PreferenceTypeField = value;
                    this.RaisePropertyChanged("PreferenceType");
                }
            }
        }

        [DataMember]
        public short Sortseq
        {
            get
            {
                return this.SortseqField;
            }
            set
            {
                if ((this.SortseqField.Equals(value) != true))
                {
                    this.SortseqField = value;
                    this.RaisePropertyChanged("Sortseq");
                }
            }
        }

        [DataMember]
        public System.DateTime UpdateDateTime
        {
            get
            {
                return this.UpdateDateTimeField;
            }
            set
            {
                if ((this.UpdateDateTimeField.Equals(value) != true))
                {
                    this.UpdateDateTimeField = value;
                    this.RaisePropertyChanged("UpdateDateTime");
                }
            }
        }

        [DataMember]
        public string UserID
        {
            get
            {
                return this.UserIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UserIDField, value) != true))
                {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
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