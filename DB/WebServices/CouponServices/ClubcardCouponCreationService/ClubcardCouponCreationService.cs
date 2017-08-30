using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.Marketing.IT.ClubcardCoupon.BAL;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Marketing.IT.ClubcardCoupon.CreationService
{
    /// <summary>
    /// Implementation class of IClubcardCouponCreationService
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single, AddressFilterMode=AddressFilterMode.Any)]
    public class ClubcardCouponCreationService : IClubcardCouponCreationService
    {


        /// <summary>
        /// Method to load list of Coupon classes from CouponSetupSystem to Clubcard Coupons System
        /// </summary>
        /// <param name="listCouponClass">listCouponClass</param>
        /// <remarks>TriggerNumber, StatementNumber, CouponDescription, RedemptionEndDate, RedemptionChannel, MaxRedemptionLimit, AlphaCode, EANBarcode are the mandatory parameters to be passed to this method. Loads Coupon Classes from CouponSetup System to Clubcard Coupon System</remarks>
        public void LoadCoupon(ListCouponClass listCouponClass)
        {
            try
            {
                if (!((listCouponClass == null) || (listCouponClass.Count == 0)))
                {
                    Logger.Write("Service:LoadCoupon(): Received  " + listCouponClass.Count + " objects in request list", "General");
                    new BusinessLayer().LoadCoupon(listCouponClass);
                }
                else
                {
                    Logger.Write("Service:LoadCoupon(): " + "Request List is NULL or Empty", "General");
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:LoadCoupon(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
        }
    }
}
