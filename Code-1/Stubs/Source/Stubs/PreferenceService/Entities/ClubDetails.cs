using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [DataContract]
    public partial class ClubDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string ChangedBirthDateField;

        private short ClubIDField;

        private System.Collections.Generic.List<ClubDetails> ClubInformationField;

        private string CultureField;

        private System.Collections.Generic.List<ClubDetails> DOBDetailsField;

        private string DateOfBirthField;

        private string IsDeletedField;

        private System.DateTime JoinDateField;

        private short MediaField;

        private string MediaDescField;

        private System.Collections.Generic.List<ClubDetails> MediaDetailsField;

        private string MembershipIDField;

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
        public string ChangedBirthDate
        {
            get
            {
                return this.ChangedBirthDateField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ChangedBirthDateField, value) != true))
                {
                    this.ChangedBirthDateField = value;
                    this.RaisePropertyChanged("ChangedBirthDate");
                }
            }
        }

        [DataMember]
        public short ClubID
        {
            get
            {
                return this.ClubIDField;
            }
            set
            {
                if ((this.ClubIDField.Equals(value) != true))
                {
                    this.ClubIDField = value;
                    this.RaisePropertyChanged("ClubID");
                }
            }
        }

        [DataMember]
        public System.Collections.Generic.List<ClubDetails> ClubInformation
        {
            get
            {
                return this.ClubInformationField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClubInformationField, value) != true))
                {
                    this.ClubInformationField = value;
                    this.RaisePropertyChanged("ClubInformation");
                }
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
        public System.Collections.Generic.List<ClubDetails> DOBDetails
        {
            get
            {
                return this.DOBDetailsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DOBDetailsField, value) != true))
                {
                    this.DOBDetailsField = value;
                    this.RaisePropertyChanged("DOBDetails");
                }
            }
        }

        [DataMember]
        public string DateOfBirth
        {
            get
            {
                return this.DateOfBirthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DateOfBirthField, value) != true))
                {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
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
        public System.DateTime JoinDate
        {
            get
            {
                return this.JoinDateField;
            }
            set
            {
                if ((this.JoinDateField.Equals(value) != true))
                {
                    this.JoinDateField = value;
                    this.RaisePropertyChanged("JoinDate");
                }
            }
        }

        [DataMember]
        public short Media
        {
            get
            {
                return this.MediaField;
            }
            set
            {
                if ((this.MediaField.Equals(value) != true))
                {
                    this.MediaField = value;
                    this.RaisePropertyChanged("Media");
                }
            }
        }

        [DataMember]
        public string MediaDesc
        {
            get
            {
                return this.MediaDescField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MediaDescField, value) != true))
                {
                    this.MediaDescField = value;
                    this.RaisePropertyChanged("MediaDesc");
                }
            }
        }

        [DataMember]
        public System.Collections.Generic.List<ClubDetails> MediaDetails
        {
            get
            {
                return this.MediaDetailsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MediaDetailsField, value) != true))
                {
                    this.MediaDetailsField = value;
                    this.RaisePropertyChanged("MediaDetails");
                }
            }
        }

        [DataMember]
        public string MembershipID
        {
            get
            {
                return this.MembershipIDField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MembershipIDField, value) != true))
                {
                    this.MembershipIDField = value;
                    this.RaisePropertyChanged("MembershipID");
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