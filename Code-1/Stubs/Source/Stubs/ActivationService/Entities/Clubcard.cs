using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class Clubcard : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long ClubcardNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ExpiryDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool PrimaryField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartDateField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ClubcardNumber
        {
            get
            {
                return this.ClubcardNumberField;
            }
            set
            {
                if ((this.ClubcardNumberField.Equals(value) != true))
                {
                    this.ClubcardNumberField = value;
                    this.RaisePropertyChanged("ClubcardNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ExpiryDate
        {
            get
            {
                return this.ExpiryDateField;
            }
            set
            {
                if ((this.ExpiryDateField.Equals(value) != true))
                {
                    this.ExpiryDateField = value;
                    this.RaisePropertyChanged("ExpiryDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Primary
        {
            get
            {
                return this.PrimaryField;
            }
            set
            {
                if ((this.PrimaryField.Equals(value) != true))
                {
                    this.PrimaryField = value;
                    this.RaisePropertyChanged("Primary");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime StartDate
        {
            get
            {
                return this.StartDateField;
            }
            set
            {
                if ((this.StartDateField.Equals(value) != true))
                {
                    this.StartDateField = value;
                    this.RaisePropertyChanged("StartDate");
                }
            }
        }
    }
}