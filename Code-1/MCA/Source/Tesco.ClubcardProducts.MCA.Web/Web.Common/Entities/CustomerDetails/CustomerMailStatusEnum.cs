using System;
using System.Xml.Serialization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable]
    public enum CustomerMailStatusEnum
    {
        [XmlEnum("1")]
        Mailable = 1,
        [XmlEnum("2")]
        Skeleton = 2,
        [XmlEnum("3")]
        AddressInError = 3,
        [XmlEnum("4")]
        NonMailable = 4,
        [XmlEnum("5")]
        PendingActivationMailable = 5,
        [XmlEnum("6")]
        Missing = 6,
        [XmlEnum("7")]
        Deliverable = 7,
        [XmlEnum("8")]
        InError = 8,
        [XmlEnum("")]
        Default=0
    }
}
