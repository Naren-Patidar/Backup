using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SmartVoucherService
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    public class GetUsedVoucherDtlsRsp : ResponseBase
    {
        private System.Data.DataSet dsResponseField;

        [DataMember]
        public System.Data.DataSet dsResponse
        {
            get
            {
                return this.dsResponseField;
            }
            set
            {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true))
                {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
}