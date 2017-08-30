using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;


namespace Tesco.com.IntegrationServices.Messages
{
    [Serializable]
    [DataContract]
    public sealed class ClubcardOnlineGetUnusedVoucherDetailsResponse : ResponseBase
    {
        private DataSet _dsResponse;

        public ClubcardOnlineGetUnusedVoucherDetailsResponse()
            : this(null, null, null, null)
        {
        }

        public ClubcardOnlineGetUnusedVoucherDetailsResponse(DataSet dsResponse)
            : this(null, null, null, dsResponse)
        {
        }

        public ClubcardOnlineGetUnusedVoucherDetailsResponse(string errorLogID, string errorStatusCode, string errorMessage, DataSet dsResponse)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
            this._dsResponse = dsResponse;
        }

        [DataMember]
        public DataSet dsResponse
        {
            get { return this._dsResponse; }
            set { this._dsResponse = value; }
        }
    }
}
