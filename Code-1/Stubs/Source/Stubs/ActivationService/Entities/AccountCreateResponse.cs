using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.WebService.Messages")]

    public class AccountCreateResponse : ResponseBase
    {
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long ClubcardNumberField;

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


    }
}