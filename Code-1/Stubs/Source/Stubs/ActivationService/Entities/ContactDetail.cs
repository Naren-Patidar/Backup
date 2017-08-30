using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class ContactDetail : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DayContactNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EveningContactNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MobileContactNumberField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DayContactNumber
        {
            get
            {
                return this.DayContactNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DayContactNumberField, value) != true))
                {
                    this.DayContactNumberField = value;
                    this.RaisePropertyChanged("DayContactNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAddress
        {
            get
            {
                return this.EmailAddressField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EmailAddressField, value) != true))
                {
                    this.EmailAddressField = value;
                    this.RaisePropertyChanged("EmailAddress");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EveningContactNumber
        {
            get
            {
                return this.EveningContactNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EveningContactNumberField, value) != true))
                {
                    this.EveningContactNumberField = value;
                    this.RaisePropertyChanged("EveningContactNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MobileContactNumber
        {
            get
            {
                return this.MobileContactNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MobileContactNumberField, value) != true))
                {
                    this.MobileContactNumberField = value;
                    this.RaisePropertyChanged("MobileContactNumber");
                }
            }
        }
    }
}