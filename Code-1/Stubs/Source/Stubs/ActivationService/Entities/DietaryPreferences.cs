using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClubcardOnline.Web.Entities.CustomerActivationServices
{
     [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public partial class DietaryPreferences : EntityBase
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsDiabeticField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsHalalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsKosharField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsTeetotalField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsVegiterianField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsDiabetic
        {
            get
            {
                return this.IsDiabeticField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsDiabeticField, value) != true))
                {
                    this.IsDiabeticField = value;
                    this.RaisePropertyChanged("IsDiabetic");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsHalal
        {
            get
            {
                return this.IsHalalField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsHalalField, value) != true))
                {
                    this.IsHalalField = value;
                    this.RaisePropertyChanged("IsHalal");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsKoshar
        {
            get
            {
                return this.IsKosharField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsKosharField, value) != true))
                {
                    this.IsKosharField = value;
                    this.RaisePropertyChanged("IsKoshar");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsTeetotal
        {
            get
            {
                return this.IsTeetotalField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsTeetotalField, value) != true))
                {
                    this.IsTeetotalField = value;
                    this.RaisePropertyChanged("IsTeetotal");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsVegiterian
        {
            get
            {
                return this.IsVegiterianField;
            }
            set
            {
                if ((object.ReferenceEquals(this.IsVegiterianField, value) != true))
                {
                    this.IsVegiterianField = value;
                    this.RaisePropertyChanged("IsVegiterian");
                }
            }
        }
    }
}