using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;

namespace Tesco.Marketing.IT.ClubcardCoupon.EnquiryService
{
    /// <summary>
    /// Interface IClubcardCouponEnquiryService
    /// </summary>
    [ServiceContract(Namespace = "http://Tesco/Marketing/IT/ClubcardCoupon/EnquiryService/2012/01/")]
    public interface IClubcardCouponEnquiryService
    {

        /// <summary>
        /// This method Provides all coupons which are available for the customer to use for requested house holds
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>List of AvailableCouponResponse</returns>
        /// <remarks>This will allow, for example, MCA to display the Coupons available to the logged in customer based on the coupon allocated for that household
        /// This service method will provide interfacing with MCA and Customer Service application to provide all available coupons for the given Clubcard number (or associated Household account)
        /// </remarks>
        [OperationContract]
        List<AvailableCouponResponse> GetAvailableCoupons(List<AvailableCouponRequest> obj);



        /// <summary>
        /// This service method will allow, for example, MCA to display to customers Coupons which the customer has redeemed within last x number of days
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>List of RedeemedCouponResponse object</returns>
        /// <remarks>This method gives Coupons that are allocated to the given Clubcard Customer or related household account. Coupons that has been redeemed within given number of days as per the input parameter</remarks>        
        [OperationContract]
        List<RedeemedCouponResponse> GetRedeemedCoupons(List<RedeemedCouponRequest> obj);



        /// <summary>
        /// This service method will allow Customer Service Centre to check, in case of enquiry, the issuance and redemption information for the given smart Coupon.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>List of CouponInformationResponse object</returns>
        /// <remarks>The 4flying will provide two jpeg images for each Coupon that is present in the system – Thumbnail and Full. The coupon images will be stored within the Clubcard Coupon database.
        /// The CSU system should provide required Coupon information as specified in this section. For example, Coupon Start/End Date, Issuance Start and End Date, Redemption information, Coupon description etc.
        /// The infrastructure present will be in line with that present for Smart Vouchers and therefore the response time between CC and MCA will be the same as Smart Vouchers and MCA.</remarks>
        [OperationContract]
        List<CouponInformationResponse> GetCouponInformation(List<CouponInformationRequest> obj);
    }    
}
