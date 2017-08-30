using System;
using System.Xml.Serialization;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement
{
    [Serializable]
    [XmlRoot("OrderReplacement")]
    public class OrderReplacementModel
    {
        [XmlElement("CustomerID")]
        public long CustomerId
        {
            get
            {
                return CryptoUtility.DecryptTripleDES(this.CustomerIdEncrypt).TryParse<Int64>();
            }

            set
            {

            }
        }
        [XmlIgnore]
        public string CustomerIdEncrypt { get; set; }


        [XmlElement("ClubcardID")]
        public long ClubcardNumber { get; set; }

        [XmlElement("RequestCode")]
        public OrderReplacementTypeEnum RequestType { get; set; }

        [XmlElement("RequestReasonCode")]
        public string Reason { get; set; }
    }
}
