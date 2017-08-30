using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.Marketing.IT.ClubcardCoupon.BAL;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.ClubcardCoupon.AdHocCoupon.CustomerService;
using System.Configuration;
using System.Xml.Linq;

namespace Tesco.Marketing.IT.ClubcardCoupon.AdHocCouponService
{
    /// <summary>
    /// This class is implemenataion class of IClubcardAdHocCouponService interface and provides
    /// functionality to Ad Hoc service
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClubcardAdHocCouponService : IClubcardAdHocCouponService
    {
        /// <summary>
        /// This method take AdHoc Coupon request, generate them and sends back 
        /// to the requester in AdHocCouponResponse
        /// </summary>
        /// <param name="couponRequest">This is of type AdHocCouponRequest</param>
        /// <returns>AdHocCouponResponse</returns>
        public AdHocCouponResponse IssueAdHocCoupons(AdHocCouponRequest couponRequest)
        {
            AdHocCouponResponse adHocCouponResponse = null;
            //long lngHHID = 0;
            try
            {
                using (new Tracer("General"))
                {
                    if (couponRequest != null)
                    {
                        if (string.IsNullOrWhiteSpace(couponRequest.ClubcardNumber) == false)
                        {
                            Int64 bigClubcardNumber = Convert.ToInt64(couponRequest.ClubcardNumber);
                            // NGC call is not required as per project clock performance changes
                            //Convert Clubcard Number into Account Id by calling NGC if exists 
                            //if (bigClubcardNumber > 0)
                            //{
                            //    GetHouseHoldId(couponRequest.ClubcardNumber, ref lngHHID);
                            //    couponRequest.CustomerAcctId = lngHHID;                                
                            //}
                        }
                        
                        //Proceed to generate ad hoc coupon
                        adHocCouponResponse = new BusinessLayer().IssueAdHocCoupons(couponRequest);
                    }
                    else
                    {
                        Logger.Write("Service:IssueAdHocCoupons(): " + "Request List is NULL or Empty", "General");
                    }
                }
            }
            catch (Exception ex)
            {
                adHocCouponResponse = new AdHocCouponResponse();
                adHocCouponResponse.CouponErrorCode = -1;
                adHocCouponResponse.CouponErrorMessage = "Internal Error Occured";
                Logger.Write("Service:IssueAdHocCoupons(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            //Throw if error is there in fault
            if (adHocCouponResponse.CouponErrorCode != 0)
            {
                AdHocException objException = new AdHocException();
                objException.ErrorId = adHocCouponResponse.CouponErrorCode;
                objException.ErrorMessage = adHocCouponResponse.CouponErrorMessage;
                throw new FaultException<AdHocException>(objException, new FaultReason("Error in AdHoc Service"));
            }
            return adHocCouponResponse;
        }


        /// <summary>
        /// Method to resolve ClubcardNumber to HouseholdId by calling NGC service
        /// </summary>
        /// <param name="clubcardNumber">Clubcard Number</param>
        /// <returns>Household Id</returns>
        private bool GetHouseHoldId(string clubcardNumber, ref long houseHoldId)
        {
            bool result = false;
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 1;
            int maxRowCount = 0;

            try
            {
                string conditionXml = string.Format("<?xml version='1.0' encoding='utf-16'?><customer><cardAccountNumber>{0}</cardAccountNumber></customer>", clubcardNumber);

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

                        if (houseHolds.Count() > 0)
                        {
                            foreach (var id in houseHolds)
                            {
                                houseHoldId = id;
                                continue;
                            }
                        }
                        else
                        {
                            Logger.Write(string.Format("Service:GetHouseHoldId() : The service call could not find HouseHoldId for the requested ClubcardNumber."), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                            return false;
                        }
                    }
                    else
                    {
                        Logger.Write(string.Format("Service:GetHouseHoldId() : The service call failed to resolve ClubcardNumber to HouseHoldId. Error Message : '{0}'", errorXml), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("Service:GetHouseHoldId() : The service call failed to resolve ClubcardNumber to HouseHoldId. Error Message : '{0}'", ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return false;
            }
        }
    }
}
