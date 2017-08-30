using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;

namespace Tesco.Marketing.IT.ClubcardCoupon.CheckOutService
{
    /// <summary>
    /// Interface IClubcardCouponCheckOutService
    /// </summary>
    [ServiceContract(Namespace = "Tesco.Marketing.IT.ClubcardCoupon.CheckOutService"), XmlSerializerFormat]
    public interface IClubcardCouponCheckOutService
    {
        /// <summary>
        /// This function is reponsible to redeem the coupon online and offline and is called by storeline and Dotcom site.This function take CheckOutRequest as input parameter and gives CheckoutResponse in return.         
        /// </summary>
        /// <param name="CheckOutRequests">CheckOutRequests</param>
        /// <returns>Returns list of coupons</returns>
        /// <remarks>When a coupon is swiped at Storeline, it will call this method to check if the coupon is available to be redeemed, if the coupon is available it will redeem the coupon Online or Offline mode. If the coupon is not available it will reject the coupon</remarks>
        [OperationContract]
        List<CheckOutResponse> ProcessCheckOut(List<CheckOutRequest> CheckOutRequests);



        /// <summary>
        /// This method supports to Check the redemption status of a coupon whether the coupon is redeemed or not
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>Returns List of Coupons after validation</returns>
        /// <remarks>Returns error code if the coupon is invalid with respective SmartAlphanumericCode</remarks>
        [OperationContract]
        List<CheckOutResponse> ValidateCoupon(List<CouponInformationRequest> obj);

        /// <summary>
        /// This method will return the coupons to be issued at Till        
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>List of coupons at Till</returns>
        /// <remarks>This Method will get the list of Coupons to be issued at Till for a Customer</remarks>
        [OperationContract]
        List<CouponAtTill> GetCouponsAtTill(CouponAtTillRequest obj);
    }
}
