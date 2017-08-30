using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.Marketing.IT.ClubcardCoupon.BAL;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using System.Xml.Linq;
using Tesco.Marketing.IT.ClubcardCoupon.CheckOutService.CustomerService;

namespace Tesco.Marketing.IT.ClubcardCoupon.CheckOutService
{
    /// <summary>
    /// Implementation of IClubcardCouponCheckOutService
    /// </summary>     
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClubcardCouponCheckOutService : IClubcardCouponCheckOutService
    {
        /// <summary>
        /// This function is reponsible to redeem the coupon online and offline and is called by storeline and Dotcom site.This function take CheckOutRequest as input parameter and gives CheckoutResponse in return.         
        /// </summary>
        /// <param name="CheckOutRequests">CheckOutRequests</param>
        /// <returns>Returns list of coupons</returns>        
        /// <remarks>If ProcessCheckOut Method is called from Till then, SmartBarcode, SessionId, RedemptionChannel, Storenumber, TillId, TillType, TransactionTimeStamp, IsOfflineTransaction, CashierNumber, IsReversal are the Mandatory Parameters to be passed to the Method.
        /// If ProcessCheckOut Method is called from Dotcom then, SmartAlphaNumericCode, RedemptionChannel, TransactionTimeStamp, IsOfflineTransaction, IsReversal are the Mandatory Parameters to be passed to the Method.
        /// When a request is passed with all the mandatory parameters based on RedemptionChannel, this method will check if the Coupon is Active and Available to redeem. If the coupon is Available, it will redeem the coupon and update the coupon status in Clubcard Coupon Database. If the coupon is not available then, this method will reject the coupon and sends the response with corresponding Error Details</remarks>
        public List<CheckOutResponse> ProcessCheckOut(List<CheckOutRequest> CheckOutRequests)
        {
            List<CheckOutResponse> lstCheckOutResponse = null;
            try
            {
                using (new Tracer("General"))
                {
                    if (!((CheckOutRequests == null) || (CheckOutRequests.Count == 0)))
                    {
                        lstCheckOutResponse = new BusinessLayer().ProcessCheckOut(CheckOutRequests);
                    }
                    else
                    {
                        Logger.Write("Service:ProcessCheckOut(): " + "Request List is NULL or Empty", "General");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:ProcessCheckOut(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            return lstCheckOutResponse;

        }

        /// <summary>
        /// This method supports to Check the redemption status of a coupon whether the coupon is redeemed or not
        /// </summary>
        /// <param name="objCheckList">objCheckList</param>
        /// <returns>Returns List of Coupons after validation</returns>
        /// <remarks>ValidateCoupon Method is called from Dotcom site to Validate whether the Coupon is redeemed or not. SmartAlphanumericCode and HouseholdId are the mandatory parameters to be passed to this method. Returns error code if the coupon is invalid with respective SmartAlphanumericCode</remarks>
        public List<CheckOutResponse> ValidateCoupon(List<CouponInformationRequest> objCheckList)
        {
            List<CheckOutResponse> respList = null;

            CheckOutResponse resp = null;

            //long houseHold = 0;

            try
            {
                if (objCheckList != null && objCheckList.Count > 0)
                {
                    respList = new List<CheckOutResponse>();
                    resp = new CheckOutResponse();

                    foreach (var req in objCheckList)
                    {
                        //if (req.HouseholdId == 0 && (req.ClubcardNumber != 0))
                        //{
                        //    //To resolve ClubcardNumber to HouseholdId
                        //    if (GetHouseHoldId(req.ClubcardNumber.Value, ref houseHold))
                        //    {
                        //        req.HouseholdId = houseHold;
                        //    }
                        //    else
                        //    {
                        //        respList.Add(new CheckOutResponse() { ErrorStatusCode = "103", ErrorMessage = ConfigurationManager.AppSettings["103"] });
                        //    }
                        //}

                        if (!string.IsNullOrWhiteSpace(req.SmartAlphaCode))
                        {
                            resp = new BusinessLayer().ProcessValidateCheckOut(req);
                            resp.SmartAlphaNumericCode = req.SmartAlphaCode;
                            respList.Add(resp);
                        }
                        else
                        {
                            respList.Add(new CheckOutResponse() { SmartAlphaNumericCode = req.SmartAlphaCode, RedemptionStatus = -1, ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                        }
                    }
                }
                else
                {
                    respList.Add(new CheckOutResponse() { RedemptionStatus = -1, ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                    Logger.Write("CheckOutService : Paramenter passed into ValidateCoupon() method is null or an empty", "General");
                }

                return respList;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                respList.Add(new CheckOutResponse() { ErrorStatusCode = "102", ErrorMessage = ConfigurationManager.AppSettings["102"] });
                return respList;
            }
            finally
            {
                //Dispose the objects created
                respList = null;
                resp = null;
            }
        }

        /// <summary>
        /// This method will return the coupons to be issued at Till for input CustomerId       
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>List of coupons at Till</returns>
        /// <remarks>This Method will get the list of Coupons to be issued at Till for a Customer. CustomerId, StoreNumber, TillId, TillBankId, OperatorId are mandatory parameters to be passed to this method.</remarks>
        public List<CouponAtTill> GetCouponsAtTill(CouponAtTillRequest obj)
        {
            List<CouponAtTill> lstGetCouponsAtTillResponse = null;
            try
            {
                using (new Tracer("General"))
                {
                    if (obj != null)
                    {
                        lstGetCouponsAtTillResponse = new BusinessLayer().GetCouponsAtTill(obj);
                    }
                    else
                    {
                        Logger.Write("Service:GetCouponsAtTill(): " + "Request NULL or Empty", "General");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:GetCouponsAtTill(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            return lstGetCouponsAtTillResponse;
        }

        /// <summary>
        /// Method to resolve ClubcardNumber to HouseholdId
        /// </summary>
        /// <param name="clubcardNumber">Clubcard Number</param>
        /// <returns>Returns true if the input Clubcard Number is resolved to HouseholdId</returns>
        /// <remarks>Either ClubcardNumber or HouseholdId are mandatory parameters to be passed to this method. If a ClubcardNumber is passed then, this method Calls NGC CustomerServiceClient service to get HouseholdID of a given Clubcard Number.</remarks>
        private static bool GetHouseHoldId(long clubcardNumber, ref long houseHoldId)
        {
            bool result = false;
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 1;
            int maxRowCount = 0;

            try
            {
                string conditionXml = string.Format("<?xml version='1.0' encoding='utf-16'?><customer><cardAccountNumber>{0}</cardAccountNumber></customer>", clubcardNumber.ToString());

                // change culture according to group if required to call the service
                string culture = ConfigurationManager.AppSettings["DefaultCulture"];

                using (CustomerServiceClient client = new CustomerServiceClient())
                {
                    //get customer details based on clubcard number
                    result = client.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture);

                    if (result)
                    {
                        XElement xe = XElement.Parse(resultXml);

                        var houseHolds = (from c in xe.Descendants("Customer")
                                          select (long)c.Element("HouseHoldID")).Take(1);

                        foreach (var id in houseHolds)
                        {
                            houseHoldId = id;
                            continue;
                        }
                    }
                    else
                    {
                        Logger.Write(string.Format("CheckOutService : The service call failed to resolve ClubcardNumber to HouseHoldId. Error Message : '{0}'", errorXml), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
