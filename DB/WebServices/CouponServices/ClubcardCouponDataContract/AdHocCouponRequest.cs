using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// AdHocCouponRequest cotains TriggerNo, MailingNo  as mandatory and CC Number as optional parameter
    /// </summary>
    [DataContract]
    public class AdHocCouponRequest
    {
        /// <summary>
        /// Trigger Number represent the value of coupon.
        /// For ex: 223 means 2 pound and 23 penny and 1234 represents 12 pound and 34 penny.
        /// This is required parameter
        /// </summary>
        [DataMember(IsRequired = true)]
        public Int16? TriggerNumber { get; set; }

        /// <summary>
        /// Mailing Number represent the mailing period from which coupon has to be genereated for ex PP0, PP1, PP, etc, 
        /// This should be a two alphabet or three alphabet number 
        /// This is required parameter
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MailingNumber { get; set; }

        /// <summary>
        /// Clubcard Number of the customer, 
        /// This is optional parameter 
        /// </summary>
        [DataMember]
        public string ClubcardNumber { get; set; }

        /// <summary>
        /// CustomerAcctId
        /// This is for internal use to be stored in DB need not to be passed
        /// </summary>
        public Int64 CustomerAcctId { get; set; }
    }

    /// <summary>
    /// This is fault contract 
    /// data contract used to send 
    /// exception to client
    /// </summary>
    [DataContract()]
    public class AdHocException
    {
        /// <summary>
        /// Error Id of error occured
        /// </summary>
        [DataMember()]
        public Int16 ErrorId;

        /// <summary>
        /// Error Description to be returned to client
        /// </summary>
        [DataMember()]
        public string ErrorMessage;
    }
}

