using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages")]

    public class AccountDuplicateCheckResponse : ResponseBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool HasDuplicatesField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool HasDuplicates
        {
            get
            {
                return this.HasDuplicatesField;
            }
            set
            {
                if ((this.HasDuplicatesField.Equals(value) != true))
                {
                    this.HasDuplicatesField = value;
                    this.RaisePropertyChanged("HasDuplicates");
                }
            }
        }

    }
}