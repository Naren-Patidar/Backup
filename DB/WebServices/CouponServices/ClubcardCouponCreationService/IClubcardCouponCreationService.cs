using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;

namespace Tesco.Marketing.IT.ClubcardCoupon.CreationService
{
    /// <summary>
    /// Interface IClubcardCouponCreationService
    /// </summary>
    [ServiceContract(Namespace = "Tesco.Marketing.IT.ClubcardCoupon.CreationService")]
    public interface IClubcardCouponCreationService
    {
        /// <summary>
        /// Method to load list of Coupon classes to Clubcard Coupons System
        /// </summary>
        /// <param name="listCouponClass">listCouponClass</param>
        /// <remarks>Couponsetup System calls this method to load Coupon Classes to Clubcard Coupons and store in CC database</remarks>
        [OperationContract]
        void LoadCoupon(ListCouponClass listCouponClass);
    }
}
