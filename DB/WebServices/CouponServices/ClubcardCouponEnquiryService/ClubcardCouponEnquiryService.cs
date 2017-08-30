using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.Marketing.IT.ClubcardCoupon.BAL;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.Marketing.IT.ClubcardCoupon.EnquiryService.CustomerService;

namespace Tesco.Marketing.IT.ClubcardCoupon.EnquiryService
{
    /// <summary>
    /// Implementation of Clubcard Coupon EnquiryService
    /// </summary>
    public class ClubcardCouponEnquiryService : IClubcardCouponEnquiryService
    {

        /// <summary>
        /// This method will retursn the list of all the available Coupon details for requested Households
        /// </summary>
        /// <param name="availableCouponRequest">availableCouponRequest</param>
        /// <returns>List of AvailableCouponResponse object</returns>
        /// <remarks>Either HouseholdId or ClubcardNumber is mandatory parameter to be passed to this method. ImageRequired is an optional parameter. If the ImageRequired parameter is true then it will return the image as binary data. By default ImageRequired is false. If ImageRequired parameter is false then this method will return the ImageName so that MCA can get the image from shared location. This will allow, for example, MCA to display the Coupons available to the logged in customer based on the coupon allocated for that household.
        /// This service method will provide interfacing with MCA and Customer Service application to provide all available coupons for the given Clubcard number (or associated Household account).</remarks>
        public List<AvailableCouponResponse> GetAvailableCoupons(List<AvailableCouponRequest> availableCouponRequest)
        {
            List<AvailableCouponResponse> responseList = new List<AvailableCouponResponse>();
            AvailableCouponResponse response = new AvailableCouponResponse();
            long houseHold = 0;

            try
            {
                using (new Tracer("General"))
                {
                    
                    if (availableCouponRequest != null && availableCouponRequest.Count > 0)
                    {
                        foreach (var req in availableCouponRequest)
                        {
                            if (req.HouseHoldId.HasValue)
                            {
                                response = new BusinessLayer().ProcessAvailableCouponRequest(req);
                                responseList.Add(response);
                            }
                            else if (req.ClubCardNumber.HasValue)
                            {
                                //To resolve ClubcardNumber to HouseholdId
                                if (GetHouseHoldId(req.ClubCardNumber.ToString(), ref houseHold))
                                {
                                    req.HouseHoldId = houseHold;

                                    if (req.HouseHoldId.HasValue && req.HouseHoldId != 0)
                                    {
                                        response = new BusinessLayer().ProcessAvailableCouponRequest(req);
                                        responseList.Add(response);
                                    }
                                    else
                                    {
                                        responseList.Add(new AvailableCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "103", ErrorMessage = ConfigurationManager.AppSettings["103"] });
                                    }
                                }
                                else
                                {
                                    responseList.Add(new AvailableCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "103", ErrorMessage = ConfigurationManager.AppSettings["103"] });
                                }
                            }
                            else
                            {
                                responseList.Add(new AvailableCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                            }
                        }
                    }
                    else
                    {
                        responseList.Add(new AvailableCouponResponse() { ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                        Logger.Write("EnquiryService : Paramenter passed into GetAvailableCoupons() method is null or an empty", "General");
                    }

                    return responseList;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:GetAvailableCoupons(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                responseList = new List<AvailableCouponResponse>();
                responseList.Add(new AvailableCouponResponse() { ErrorStatusCode = "102", ErrorMessage = ConfigurationManager.AppSettings["102"] });
                return responseList;
            }
            finally
            {
                //Dispose the objects created
                responseList = null;
                response = null;
            }
        }

        /// <summary>
        ///  This service method will allow, for example, MCA to display to customers Coupons which the customer has redeemed within last x number of days
        /// </summary>
        /// <param name="redeemedCouponRequest">redeemedCouponRequest</param>
        /// <returns>List of RedeemedCouponResponse object</returns>
        /// <remarks>Either (HouseholdId or ClubcardNumber) and RedemptionLength are mandatory parameters to be passed to this method. ImageRequired is an optional parameter. If the ImageRequired parameter is true then it will return the image as binary data. By default ImageRequired is false. If ImageRequired parameter is false then this method will return the ImageName so that MCA can get the image from shared location. This method gives Redeemed Coupons that are allocated to the given Clubcard Customer (or related household account). that have been redeemed within X number of days (as per the input RedemptionLength).</remarks>
        public List<RedeemedCouponResponse> GetRedeemedCoupons(List<RedeemedCouponRequest> redeemedCouponRequest)
        {
            List<RedeemedCouponResponse> responseList = new List<RedeemedCouponResponse>();
            RedeemedCouponResponse response = new RedeemedCouponResponse();

            long houseHold = 0;

            try
            {
                using (new Tracer("General"))
                {
                    if (redeemedCouponRequest != null && redeemedCouponRequest.Count > 0)
                    {
                        foreach (var req in redeemedCouponRequest)
                        {
                            if (req.HouseHoldId.HasValue)
                            {
                                response = new BusinessLayer().ProcessRedeemedCouponRequest(req);
                                responseList.Add(response);
                            }
                            else if (req.ClubCardNumber.HasValue)
                            {
                                //To resolve ClubcardNumber to HouseholdId
                                if (GetHouseHoldId(req.ClubCardNumber.ToString(), ref houseHold))
                                {
                                    req.HouseHoldId = houseHold;

                                    if (req.HouseHoldId.HasValue && req.HouseHoldId != 0)
                                    {
                                        response = new BusinessLayer().ProcessRedeemedCouponRequest(req);
                                        responseList.Add(response);
                                    }
                                    else
                                    {
                                        responseList.Add(new RedeemedCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "103", ErrorMessage = ConfigurationManager.AppSettings["103"] });
                                    }
                                }
                                else
                                {
                                    responseList.Add(new RedeemedCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "103", ErrorMessage = ConfigurationManager.AppSettings["103"] });
                                }
                            }
                            else
                            {
                                responseList.Add(new RedeemedCouponResponse() { RequestedHouseHoldId = req.HouseHoldId, ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                            }
                        }
                    }
                    else
                    {
                        responseList.Add(new RedeemedCouponResponse() { ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                        Logger.Write("EnquiryService : Paramenter passed into GetRedeemedCoupons() method is null or an empty", "General");
                    }

                    return responseList;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:GetRedeemedCoupons(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                responseList = new List<RedeemedCouponResponse>();
                responseList.Add(new RedeemedCouponResponse() { ErrorStatusCode = "102", ErrorMessage = ConfigurationManager.AppSettings["102"] });
                return responseList;
            }
            finally
            {
                //Dispose the objects created
                responseList = null;
                response = null;
            }
        }

        /// <summary>
        /// This service method will allow Customer Service Centre to check, in case of enquiry, the issuance and redemption information for the given smart Coupon.
        /// </summary>
        /// <param name="objCouponInfoRequest">objCouponInfoRequest</param>
        /// <returns>List of CouponInformationResponse object</returns>
        /// <remarks>SmartBarcode or SmartAlphanumericCode are mandatory parameters to be passed to this method. ImageRequired is an optional parameter. The 4flying will provide two jpeg images for each Coupon that is present in the system – Thumbnail and Full. The coupon images will be stored within the Clubcard Coupon database.
        /// The CSU system should provide required Coupon information as specified in this section. For example, Coupon Start/End Date, Issuance Start and End Date, Redemption information, Coupon description etc.
        /// The infrastructure present will be in line with that present for Smart Vouchers and therefore the response time between CC and MCA will be the same as Smart Vouchers and MCA.</remarks>
        public List<CouponInformationResponse> GetCouponInformation(List<CouponInformationRequest> objCouponInfoRequest)
        {
            List<CouponInformationResponse> responseList = new List<CouponInformationResponse>();
            CouponInformationResponse response = new CouponInformationResponse();

            try
            {
                using (new Tracer("General"))
                {
                    if (objCouponInfoRequest != null && objCouponInfoRequest.Count > 0)
                    {
                        foreach (var req in objCouponInfoRequest)
                        {
                            if ((!string.IsNullOrWhiteSpace(req.SmartBarcode)) || (!string.IsNullOrWhiteSpace(req.SmartAlphaCode)))
                            {
                                response = new BusinessLayer().ProcessGetCouponInformation(req);
                                responseList.Add(response);
                            }
                            else
                            {
                                responseList.Add(new CouponInformationResponse() { CouponDetails = new CouponInformation() { SmartAlphaNumeric = req.SmartAlphaCode, SmartBarcode = req.SmartBarcode }, ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                            }
                        }
                    }
                    else
                    {
                        responseList.Add(new CouponInformationResponse() { ErrorStatusCode = "100", ErrorMessage = ConfigurationManager.AppSettings["100"] });
                        Logger.Write("EnquiryService : Paramenter passed into GetCouponInformation() method is null or an empty", "General");
                    }

                    return responseList;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Service:GetCouponInformation(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                responseList = new List<CouponInformationResponse>();
                responseList.Add(new CouponInformationResponse() { ErrorStatusCode = "102", ErrorMessage = ConfigurationManager.AppSettings["102"] });
                return responseList;
            }
            finally
            {
                //Dispose the objects created
                responseList = null;
                response = null;
            }
        }

        /// <summary>
        /// This method Supports to get the HouseholdId of the given ClubcardNumber
        /// </summary>
        /// <param name="clubcardNumber">ClubcardNumber</param>
        /// <returns>Household Id of a given ClubcardNumber</returns>
        /// <remarks>This method calls NGC CustomerServiceClient service to get its job done, i:e getting the HouseholdId of a given ClubcardNumber</remarks>
        private static bool GetHouseHoldId(string clubcardNumber, ref long houseHoldId)
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
                            Logger.Write(string.Format("ClubcardCouponEnquiryService : The service call could not find HouseHoldId for the requested ClubcardNumber."), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                            return false;
                        }
                    }
                    else
                    {
                        Logger.Write(string.Format("ClubcardCouponEnquiryService : The service call failed to resolve ClubcardNumber to HouseHoldId. Error Message : '{0}'", errorXml), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("ClubcardCouponEnquiryService:GetHouseHoldId() : The service call failed to resolve ClubcardNumber to HouseHoldId. Error Message : '{0}'", ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return false;
            }
        }

    }
}
