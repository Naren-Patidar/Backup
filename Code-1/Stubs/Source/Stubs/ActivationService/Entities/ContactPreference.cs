using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class ContactPreference : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContactableField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResearchContactableField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WantPartnerInfoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WantTescoInfoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Contactable
        {
            get
            {
                return this.ContactableField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContactableField, value) != true))
                {
                    this.ContactableField = value;
                    this.RaisePropertyChanged("Contactable");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResearchContactable
        {
            get
            {
                return this.ResearchContactableField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ResearchContactableField, value) != true))
                {
                    this.ResearchContactableField = value;
                    this.RaisePropertyChanged("ResearchContactable");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WantPartnerInfo
        {
            get
            {
                return this.WantPartnerInfoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.WantPartnerInfoField, value) != true))
                {
                    this.WantPartnerInfoField = value;
                    this.RaisePropertyChanged("WantPartnerInfo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WantTescoInfo
        {
            get
            {
                return this.WantTescoInfoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.WantTescoInfoField, value) != true))
                {
                    this.WantTescoInfoField = value;
                    this.RaisePropertyChanged("WantTescoInfo");
                }
            }
        }
    }
}