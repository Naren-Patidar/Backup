using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;

namespace Tesco.Marketing.IT.ClubcardCoupon.AdHocCouponService
{
    /// <summary>
    /// IClubcardAdHocCouponService interface describes AdHoc Coupon Issuance Service. This service is used to 
    /// issue coupons on adhoc manner.
    /// </summary>
    /// <remarks>
    /// This service will be called by the requestor requiring smart Clubcard Coupons.
    /// </remarks>    
    [ServiceContract(Namespace = "http://Tesco/Marketing/IT/ClubcardCoupon/AdHocCouponService/2012/01/")]
    public interface IClubcardAdHocCouponService
    {
        /// <summary>
        /// This method take AdHoc Coupon request, generate them and sends back to the requester in
        /// AdHocCouponResponse
        /// </summary>
        /// <param name="couponRequest"></param>
        /// <returns>AdHocCouponResponse</returns>
        [OperationContract]
        [FaultContract(typeof(AdHocException))]
        AdHocCouponResponse IssueAdHocCoupons(AdHocCouponRequest couponRequest);
    }
}
