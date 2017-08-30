
using System.Runtime.Serialization;

 [DataContract(Name = "RedeemType", Namespace = "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract")]
    public enum RedeemType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Redeem = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        UnRedeem = 1,
    }
