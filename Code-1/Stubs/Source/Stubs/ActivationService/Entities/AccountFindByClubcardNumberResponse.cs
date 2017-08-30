using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
    [DataContract(Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages")]

    public partial class AccountFindByClubcardNumberResponse : ResponseBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContactDetailMatchStatusField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool MatchedField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContactDetailMatchStatus
        {
            get
            {
                return this.ContactDetailMatchStatusField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContactDetailMatchStatusField, value) != true))
                {
                    this.ContactDetailMatchStatusField = value;
                    this.RaisePropertyChanged("ContactDetailMatchStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Matched
        {
            get
            {
                return this.MatchedField;
            }
            set
            {
                if ((this.MatchedField.Equals(value) != true))
                {
                    this.MatchedField = value;
                    this.RaisePropertyChanged("Matched");
                }
            }
        }
    }
}