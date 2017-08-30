using System;
using System.Xml.Serialization;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.OrderReplacement
{
    [XmlRoot("OrderReplacement")]
    public class OrderReplacementModel
    {
        [XmlElement("CustomerID")]
        public long CustomerId { get; set; }

        [XmlElement("ClubcardID")]
        public long ClubcardNumber { get; set; }

        [XmlElement("RequestCode")]
        public OrderReplacementTypeEnum RequestType { get; set; }

        [XmlElement("RequestReasonCode")]
        public string Reason { get; set; }
    }
}
