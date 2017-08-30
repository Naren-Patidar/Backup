using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Marketing.IT.ClubcardCoupon.BAL
{
    /// <summary>
    /// Async Method caller
    /// </summary>
    public delegate void AsyncMethodCaller();

    /// <summary>
    /// Interface methods
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Method to save Till Redemption Information
        /// </summary>
        void SaveTillRedeemInfo();
        
        /// <summary>
        /// Method to save GHS Redemption Information
        /// </summary>
        void SaveGHSRedeemInfo();
        
        /// <summary>
        /// Method to save Coupon Issuance at Till
        /// </summary>
        void SaveCouponIssuance();
    }

    /// <summary>
    /// Async class
    /// </summary>
    public class RedeemAsync : ICommand
    {
        Int64 couponInstanceId;
        Int16 redemptionCount;
        Int16 maxRedemptionLimit;
        Int16 redemptionStatus;
        CheckOutRequest checkOutRequest;
        AsyncMethodCaller callDelegate;
        List<CouponAtTill> lstCouponAtTill;
        CouponAtTillRequest tillRequest;

        /// <summary>
        /// AsyncCall for GHS coupon redemption
        /// </summary>
        /// <param name="req">CheckoutRequest</param>
        /// <param name="couponInstId">couponInstId</param>
        /// <param name="redempCount">redempCount</param>
        /// <param name="maxRedempLimit">maxRedempLimit</param>
        /// <param name="redempStatus">redempStatus</param>
        public RedeemAsync(CheckOutRequest req, Int64 couponInstId, Int16 redempCount, Int16 maxRedempLimit, Int16 redempStatus)
        {
            checkOutRequest = req;
            couponInstanceId = couponInstId;
            redemptionCount = redempCount;
            maxRedemptionLimit = maxRedempLimit;
            redemptionStatus = redempStatus;
            if (req.RedemptionChannel == CouponCheckOutChannel.Storeline)
                callDelegate = new AsyncMethodCaller(this.SaveTillCouponInfo);
            if (req.RedemptionChannel == CouponCheckOutChannel.DotCom)
                callDelegate = new AsyncMethodCaller(this.SaveGHSCouponInfo);
        }

        /// <summary>
        /// AsyncCall for Till Coupon redemption
        /// </summary>
        /// <param name="objRequest">objRequest</param>
        /// <param name="lstCoupons">lstCoupons</param>
        public RedeemAsync(CouponAtTillRequest objRequest, List<CouponAtTill> lstCoupons)
        {
            tillRequest = objRequest;
            lstCouponAtTill = lstCoupons;
            callDelegate = new AsyncMethodCaller(this.SaveCouponIssuanceInfo);
        }

        /// <summary>
        /// Asynccallback to save Till coupon redemption
        /// </summary>
        public void SaveTillRedeemInfo()
        {
            callDelegate.BeginInvoke(new AsyncCallback(CallbackTillMethod), checkOutRequest.SmartBarcodeNumber.ToString());
        }


        /// <summary>
        /// Asynccallback to save GHS coupon redemption
        /// </summary>
        public void SaveGHSRedeemInfo()
        {
            callDelegate.BeginInvoke(new AsyncCallback(CallbackGHSMethod), checkOutRequest.SmartAlphaNumericCode);
        }

        /// <summary>
        /// This method calls Data Access Layer to save Till Coupon details to Database
        /// </summary>
        public void SaveTillCouponInfo()
        {
            //Call to DAL
            new ClubcardCoupon.DAL.ClubcardCouponDataAccess().SaveStoreRedeemInfo(checkOutRequest, redemptionStatus, couponInstanceId, redemptionCount, maxRedemptionLimit);
        }

        /// <summary>
        /// This method calls Data Access Layer to save GHS coupon details to Database
        /// </summary>
        public void SaveGHSCouponInfo()
        {
            //Call to DAL
            new ClubcardCoupon.DAL.ClubcardCouponDataAccess().SaveDotComRedeemInfo(checkOutRequest, redemptionStatus, couponInstanceId, redemptionCount, maxRedemptionLimit);
        }

        /// <summary>
        /// This method calls Data Access Layer to save coupon details to Database
        /// </summary>
        public void SaveCouponIssuanceInfo()
        {
            //Call to DAL
            new ClubcardCoupon.DAL.ClubcardCouponDataAccess().SaveCouponIssuance(tillRequest, lstCouponAtTill);
        }

        /// <summary>
        /// This method will be called while saving Coupon Till Issuance
        /// </summary>
        public void SaveCouponIssuance()
        {
            callDelegate.BeginInvoke(new AsyncCallback(CallbackCouponIssuanceMethod), tillRequest.CustomerId.ToString());
        }

        /// <summary>
        /// This method will be called while processing Till coupon issuance
        /// </summary>
        /// <param name="aIResult">aIResult</param>
        public void CallbackTillMethod(IAsyncResult aIResult)
        {
            AsyncResult aResult = (AsyncResult)aIResult;
            AsyncMethodCaller caller = (AsyncMethodCaller)aResult.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string formatString = (string)aIResult.AsyncState;

            // Call EndInvoke to retrieve the results.
            try
            {
                caller.EndInvoke(aIResult);
            }
            catch (Exception ex)
            {
                Logger.Write("BAL:RedeemAsync:CallbackTillMethod: Message: Unable to save request with barcode: " + formatString, "General");
                Logger.Write("BAL:RedeemAsync:CallbackTillMethod: Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
        }

        /// <summary>
        /// This method will be called while processing GHS Coupon redemption
        /// </summary>
        /// <param name="aIResult">aIResult</param>
        public void CallbackGHSMethod(IAsyncResult aIResult)
        {
            AsyncResult aResult = (AsyncResult)aIResult;
            AsyncMethodCaller caller = (AsyncMethodCaller)aResult.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string formatString = (string)aIResult.AsyncState;

            // Call EndInvoke to retrieve the results.
            try
            {
                caller.EndInvoke(aIResult);
            }
            catch (Exception ex)
            {
                Logger.Write("BAL:RedeemAsync:CallbackGHSMethod: Message: Unable to save request with alphacode: " + formatString, "General");
                Logger.Write("BAL:RedeemAsync:CallbackGHSMethod: Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
        }

        /// <summary>
        /// This function is called while issueing coupons at Till
        /// </summary>
        /// <param name="aIResult">aIReult</param>
        public void CallbackCouponIssuanceMethod(IAsyncResult aIResult)
        {
            AsyncResult aResult = (AsyncResult)aIResult;
            AsyncMethodCaller caller = (AsyncMethodCaller)aResult.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string formatString = (string)aIResult.AsyncState;

            // Call EndInvoke to retrieve the results.
            try
            {
                caller.EndInvoke(aIResult);
            }
            catch (Exception ex)
            {
                Logger.Write("BAL:RedeemAsync:CallbackCouponIssuanceMethod: Message: Unable to save request with CustomerID: " + formatString, "General");
                Logger.Write("BAL:RedeemAsync:CallbackCouponIssuanceMethod: Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
        }
    }
}
