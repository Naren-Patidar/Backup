using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class Address : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine3Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine4Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine5Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressLine6Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostCodeField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine1
        {
            get
            {
                return this.AddressLine1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine1Field, value) != true))
                {
                    this.AddressLine1Field = value;
                    this.RaisePropertyChanged("AddressLine1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine2
        {
            get
            {
                return this.AddressLine2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine2Field, value) != true))
                {
                    this.AddressLine2Field = value;
                    this.RaisePropertyChanged("AddressLine2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine3
        {
            get
            {
                return this.AddressLine3Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine3Field, value) != true))
                {
                    this.AddressLine3Field = value;
                    this.RaisePropertyChanged("AddressLine3");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine4
        {
            get
            {
                return this.AddressLine4Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine4Field, value) != true))
                {
                    this.AddressLine4Field = value;
                    this.RaisePropertyChanged("AddressLine4");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine5
        {
            get
            {
                return this.AddressLine5Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine5Field, value) != true))
                {
                    this.AddressLine5Field = value;
                    this.RaisePropertyChanged("AddressLine5");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AddressLine6
        {
            get
            {
                return this.AddressLine6Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddressLine6Field, value) != true))
                {
                    this.AddressLine6Field = value;
                    this.RaisePropertyChanged("AddressLine6");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PostCode
        {
            get
            {
                return this.PostCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PostCodeField, value) != true))
                {
                    this.PostCodeField = value;
                    this.RaisePropertyChanged("PostCode");
                }
            }
        }

    }
}