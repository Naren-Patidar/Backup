using System;
using System.Xml.Serialization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement
{
    [Serializable]
    public enum OrderReplacementTypeEnum
    {
        [XmlEnum("1")]
        NewCard = 1,

        [XmlEnum("2")]
        NewKeyFOB = 2,
        
        [XmlEnum("3")]
        NewCardAndKeyFOB = 3
    }
}
