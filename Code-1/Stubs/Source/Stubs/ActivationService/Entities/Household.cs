using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class Household : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] PeopleAgesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TotalPeopleField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] PeopleAges
        {
            get
            {
                return this.PeopleAgesField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PeopleAgesField, value) != true))
                {
                    this.PeopleAgesField = value;
                    this.RaisePropertyChanged("PeopleAges");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalPeople
        {
            get
            {
                return this.TotalPeopleField;
            }
            set
            {
                if ((this.TotalPeopleField.Equals(value) != true))
                {
                    this.TotalPeopleField = value;
                    this.RaisePropertyChanged("TotalPeople");
                }
            }
        }
    }
}