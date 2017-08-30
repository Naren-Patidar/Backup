using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Seema Kudari
    /// Class Name: Token
    /// </summary>

    [DataContract]
    public class Token
    {
        [DataMember]
        public int TokenValue { get; set; }
        [DataMember]
        public string EAN { get; set; }
        [DataMember]
        public string Alpha { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public DateTime UsedByDate { get; set; }
        [DataMember]
        public int VendorCode { get; set; }
        [DataMember]
        public DateTime SupplyDate { get; set; }
        [DataMember]
        public int ProductLineId { get; set; }
        [DataMember]
        public int TokenId { get; set; }
        [DataMember]
        public int ResponseCode { get; set; }
        [DataMember] 
        public int SupplierTokenCodeId { get; set; }
        [DataMember]
        public DateTime CustomerDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }

        ///constructor
        public Token()
        {
        }

        /// constructor with EAN as a parameter
        public Token(string ean)
        {
            this.EAN = ean;
        }

        /// constructor with ProductLineId as a parameter
        public Token(int productlineid)
        {
            this.ProductLineId = productlineid;
        }

        /// constructor used for reprint
        public Token(string alpha,
                                    string ean,
                                    string productcode,
                                    DateTime supplydate,
                                    DateTime customerdate,
                                    DateTime enddate,
                                    int productlineid,
                                    int tokenid)
        {

            this.Alpha = alpha;
            this.EAN = ean;

            this.ProductCode = productcode;
            this.SupplyDate = supplydate;
            this.CustomerDate = customerdate;
            this.EndDate = enddate;

            this.ProductLineId = productlineid;
            this.TokenId = tokenid;
        }





    }
}
