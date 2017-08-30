using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Marketing.IT.ClubcardCoupon.DAL;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;

namespace Tesco.Marketing.IT.ClubcardCoupon.BAL
{
    /// <summary>
    /// Clubcard Coupon Business Application Layer
    /// </summary>
    public class BusinessLayer
    {
        /// <summary>
        /// This function will take list of coupon class objects from CouponSetup System
        /// and after validating them, it will send to data access layer to be
        /// saved to Clubcard Coupon Database
        /// </summary>
        /// <param name="obj">List of Coupon Classes</param>
        /// <exception cref="System.Exception">Exception occurs if any of the required data member is missing</exception>
        public void LoadCoupon(ListCouponClass obj)
        {
            String tempString = string.Empty;
            Int64 couponClassId;
            foreach (var couponClass in obj)
            {
                try
                {
                    if (couponClass.ValidateCouponClass())
                    {
                        couponClassId = new ClubcardCouponDataAccess().InsertCouponClass(couponClass);
                        if (couponClassId == 0)
                            return;

                        if (!((couponClass.ListCouponLineInfo == null) || (couponClass.ListCouponLineInfo.Count == 0)))
                        {
                            foreach (var couponLine in couponClass.ListCouponLineInfo)
                            {
                                try
                                {
                                    if (couponLine.ValidateCouponLine())
                                    {
                                        new ClubcardCouponDataAccess().InsertTillCouponLine(couponLine, couponClassId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Write("BAL:LoadCoupon(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        tempString = "BAL:LoadCoupon(): CouponClass object with: TriggerNumber - " + couponClass.TriggerNumber.ToString() + " StatementNumber - " + couponClass.StatementNumber + " is not valid.";
                        Logger.Write(tempString, "General");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write("BAL:LoadCoupon(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                }
            }
        }

        /// <summary>
        /// This function is reponsible to redeem the coupon online and offline and is called by storeline and Dotcom site
        /// </summary>
        /// <param name="lstCheckOutRequest">lstCheckOutRequest</param>
        /// <returns>This method will return the list of redeemed and unredeemed coupons</returns>
        public List<CheckOutResponse> ProcessCheckOut(List<CheckOutRequest> lstCheckOutRequest)
        {
            List<CheckOutResponse> lstCheckOutResponse = new List<CheckOutResponse>();

            foreach (var checkOutRequest in lstCheckOutRequest)
            {
                try
                {
                    Logger.Write("BAL:ProcessCheckOut(): Input Request - " + checkOutRequest.LogInfo(), "Information");
                    if (checkOutRequest == null)
                        continue;
                    if (checkOutRequest.RedemptionChannel == CouponCheckOutChannel.DotCom)
                        lstCheckOutResponse.Add(ProcessDotComCheckOut(checkOutRequest));
                    if (checkOutRequest.RedemptionChannel == CouponCheckOutChannel.Storeline)
                        lstCheckOutResponse.Add(ProcessStoreCheckOut(checkOutRequest));
                }
                catch (Exception ex)
                {
                    Logger.Write("BAL:ProcessCheckOut(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                }
            }
            return lstCheckOutResponse;

        }

        /// <summary>
        /// This function is reponsible for coupon redemption from Storeline
        /// </summary>
        /// <param name="checkOutRequest">checkOutRequest</param>
        /// <returns>Returns CheckoutResponse</returns>
        private CheckOutResponse ProcessStoreCheckOut(CheckOutRequest checkOutRequest)
        {
            CheckOutResponse checkOutResponse = new CheckOutResponse();
            Int64 couponInstanceId;
            Int16 redemptionCount, maxRedemptionLimit;

            try
            {
                string tmpErrorMessage = string.Empty;
                int validRequest = checkOutRequest.ValidateStoreRedeem(ref tmpErrorMessage);
                if (validRequest != 0)
                {
                    checkOutResponse.RedemptionStatus = -1;
                    checkOutResponse.SessionId = checkOutRequest.SessionId;
                    checkOutResponse.SmartBarcodeNumber = checkOutRequest.SmartBarcodeNumber;
                    checkOutResponse.ErrorStatusCode = validRequest.ToString();
                    checkOutResponse.ErrorMessage = tmpErrorMessage;
                    Logger.Write("BAL:ProcessStoreCheckOut(): BarCode: " + checkOutRequest.SmartBarcodeNumber + " Error Code: " + validRequest.ToString() + " ErrorMessage: " + tmpErrorMessage, "General");
                    Logger.Write("BAL:ProcessStoreCheckOut(): Output Response - " + checkOutResponse.LogInfo(), "Information");
                    return checkOutResponse;
                }

                if (checkOutRequest.IsReversal)
                    checkOutResponse.RedemptionStatus = new ClubcardCouponDataAccess().UnRedeemStoreLineCoupon(checkOutRequest);
                else
                {
                    checkOutResponse.RedemptionStatus = new ClubcardCouponDataAccess().GetTillCouponRedemptionStatus(Convert.ToDecimal(checkOutRequest.SmartBarcodeNumber), out couponInstanceId, out redemptionCount, out maxRedemptionLimit);
                    if (!((checkOutResponse.RedemptionStatus == -1) || (checkOutResponse.RedemptionStatus == 16)))
                    {
                        //Call asynchronously
                        RedeemAsync asyncRedeem = new RedeemAsync(checkOutRequest, couponInstanceId, redemptionCount, maxRedemptionLimit, checkOutResponse.RedemptionStatus);
                        asyncRedeem.SaveTillRedeemInfo();
                    }
                    if (checkOutResponse.RedemptionStatus == 0)
                        checkOutResponse.RedemptionStatus = 1;
                }

                checkOutResponse.SessionId = checkOutRequest.SessionId;
                checkOutResponse.SmartBarcodeNumber = checkOutRequest.SmartBarcodeNumber;
                checkOutResponse.ErrorStatusCode = "0";
            }
            catch (Exception ex)
            {
                checkOutResponse.RedemptionStatus = -1;
                checkOutResponse.SessionId = checkOutRequest.SessionId;
                checkOutResponse.SmartBarcodeNumber = checkOutRequest.SmartBarcodeNumber;
                checkOutResponse.ErrorStatusCode = "102";
                Logger.Write("BAL:ProcessStoreCheckOut(): BarCode: " + checkOutRequest.SmartBarcodeNumber + " Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            Logger.Write("BAL:ProcessStoreCheckOut(): Output Response - " + checkOutResponse.LogInfo(), "Information");
            return checkOutResponse;
        }

        /// <summary>
        /// This function is reponsible for redemption of coupon from Dotcom
        /// </summary>
        /// <param name="checkOutRequest">checkOutRequest</param>
        /// <returns>Returns CheckoutResponse</returns>
        private CheckOutResponse ProcessDotComCheckOut(CheckOutRequest checkOutRequest)
        {
            CheckOutResponse checkOutResponse = new CheckOutResponse();
            Int64 couponInstanceId;
            Int16 redemptionCount, maxRedemptionLimit;

            try
            {
                string tmpErrorMessage = string.Empty;
                int validRequest = checkOutRequest.ValidateGHSRedeem(ref tmpErrorMessage);
                if (validRequest != 0)
                {
                    checkOutResponse.RedemptionStatus = -1;
                    checkOutResponse.SessionId = checkOutRequest.SessionId;
                    checkOutResponse.SmartAlphaNumericCode = checkOutRequest.SmartAlphaNumericCode;
                    checkOutResponse.ErrorStatusCode = validRequest.ToString();
                    checkOutResponse.ErrorMessage = tmpErrorMessage;
                    Logger.Write("BAL:ProcessDotComCheckOut(): AlphaCode: " + checkOutRequest.SmartAlphaNumericCode + " Error Code: " + validRequest.ToString() + " ErrorMessage: " + tmpErrorMessage, "General");
                    Logger.Write("BAL:ProcessDotComCheckOut(): Output Response - " + checkOutResponse.LogInfo(), "Information");
                    return checkOutResponse;
                }

                if (checkOutRequest.IsReversal)
                    checkOutResponse.RedemptionStatus = new ClubcardCouponDataAccess().UnRedeemDotComCoupon(checkOutRequest);
                else
                {
                    checkOutResponse.RedemptionStatus = new ClubcardCouponDataAccess().GetGHSCouponRedemptionStatus(checkOutRequest.SmartAlphaNumericCode, out couponInstanceId, out redemptionCount, out maxRedemptionLimit);
                    if (!((checkOutResponse.RedemptionStatus == -1) || (checkOutResponse.RedemptionStatus == 16)))
                    {
                        //Call asynchronously
                        RedeemAsync asyncRedeem = new RedeemAsync(checkOutRequest, couponInstanceId, redemptionCount, maxRedemptionLimit, checkOutResponse.RedemptionStatus);
                        asyncRedeem.SaveGHSRedeemInfo();
                    }
                    if (checkOutResponse.RedemptionStatus == 0)
                        checkOutResponse.RedemptionStatus = 1;
                }

                checkOutResponse.SessionId = checkOutRequest.SessionId;
                checkOutResponse.SmartAlphaNumericCode = checkOutRequest.SmartAlphaNumericCode;
                checkOutResponse.ErrorStatusCode = "0";
            }
            catch (Exception ex)
            {
                checkOutResponse.RedemptionStatus = -1;
                checkOutResponse.SessionId = checkOutRequest.SessionId;
                checkOutResponse.SmartAlphaNumericCode = checkOutRequest.SmartAlphaNumericCode;
                checkOutResponse.ErrorStatusCode = "102";
                Logger.Write("BAL:ProcessDotComCheckOut(): BarCode: " + checkOutRequest.SmartAlphaNumericCode + " Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            Logger.Write("BAL:ProcessDotComCheckOut(): Output Response - " + checkOutResponse.LogInfo(), "Information");
            return checkOutResponse;
        }

        /// <summary>
        /// This method will return list of Coupons At Till
        /// </summary>
        /// <param name="tillRequest">CouponAtTillRequest object</param>
        /// <returns>List of CouponAtTill object</returns>
        public List<CouponAtTill> GetCouponsAtTill(CouponAtTillRequest tillRequest)
        {
            List<CouponAtTill> lstCouponAtTill = null;

            try
            {
                lstCouponAtTill = new List<CouponAtTill>();

                //if VaT enabled is true
                if (Convert.ToBoolean(Convert.ToInt16(ConfigurationManager.AppSettings["VaTEnable"])))
                {
                    lstCouponAtTill = GetVoucherAtTill(tillRequest);
                }
                else
                {
                    lstCouponAtTill = GetCouponsAtTill(tillRequest, false);
                }
            }
            catch (Exception ex)
            {
                lstCouponAtTill = new List<CouponAtTill>();
                lstCouponAtTill.Add(new CouponAtTill("", "102", "Internal Operation Failed"));
                Logger.Write(string.Format("BAL:GetCouponsAtTill(): CustomerId: {0} - ClubcardNumber = {1} - Erro Message: {2}", tillRequest.CustomerId, tillRequest.ClubcardNumber, ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }

            return lstCouponAtTill;
        }

        /// <summary>
        /// This method will send the coupons at Till
        /// </summary>
        /// <param name="tillRequest">tillRequest</param>
        /// <param name="flag">flag</param>
        /// <returns>List of CouponAtTill object</returns>
        public List<CouponAtTill> GetCouponsAtTill(CouponAtTillRequest tillRequest, bool flag)
        {
            List<CouponAtTill> lstCouponAtTill = null;

            try
            {
                Logger.Write("BAL:GetCouponsAtTill(): Input Request - CustomerId = " + tillRequest.CustomerId.ToString(), "Information");
                string tmpErrorMessage = string.Empty;
                int validRequest = tillRequest.ValidateTillRequest(ref tmpErrorMessage);
                if (validRequest != 0)
                {
                    lstCouponAtTill = new List<CouponAtTill>();
                    lstCouponAtTill.Add(new CouponAtTill("", validRequest.ToString(), tmpErrorMessage));
                    Logger.Write("BAL:GetCouponsAtTill(): CustomerId: " + tillRequest.CustomerId.ToString() + " Error Code: " + validRequest.ToString() + " ErrorMessage: " + tmpErrorMessage, "General");
                    return lstCouponAtTill;
                }

                lstCouponAtTill = new ClubcardCouponDataAccess().GetTillCouponAtTill(tillRequest.CustomerId);
                if ((lstCouponAtTill == null) || (lstCouponAtTill.Count == 0))
                {
                    lstCouponAtTill = new List<CouponAtTill>();
                    lstCouponAtTill.Add(new CouponAtTill("", "102", "Unable to fetch Coupons at till or Coupon does not exist"));
                }
                else
                {
                    //Call asynchronously
                    RedeemAsync asyncRedeem = new RedeemAsync(tillRequest, lstCouponAtTill);
                    asyncRedeem.SaveCouponIssuance();
                }
            }
            catch (Exception ex)
            {
                lstCouponAtTill = new List<CouponAtTill>();
                lstCouponAtTill.Add(new CouponAtTill("", "102", "Internal Operation Failed"));
                Logger.Write("BAL:GetCouponsAtTill(): CustomerId: " + tillRequest.CustomerId.ToString() + " Message: " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            Logger.Write("BAL:GetCouponsAtTill(): Output Response - No Of Coupons: " + lstCouponAtTill.Count.ToString() + " ErrorCode: " + lstCouponAtTill[0].ErrorStatusCode + " Error Message: " + lstCouponAtTill[0].ErrorMessage, "Information");
            return lstCouponAtTill;
        }

        #region V@T
        /// <summary>
        /// This method Will get the Vouchers to be issued at Till
        /// </summary>
        /// <param name="tillRequest">CouponAtTillRequest object</param>
        /// <returns>List of Coupons at Till</returns>
        public List<CouponAtTill> GetVoucherAtTill(CouponAtTillRequest tillRequest)
        {
            List<CouponAtTill> lstCouponAtTill = null;
            bool goForCAT = false;
            try
            {
                Logger.Write(string.Format("BAL:GetVoucherAtTill():- Input Request :- CustomerId = {0} - ClubcardNumber = {1}", tillRequest.CustomerId.ToString(), tillRequest.ClubcardNumber.ToString()), "Information");
                string tmpErrorMessage = string.Empty;
                int validRequest = tillRequest.ValidateVoucherRequest(ref tmpErrorMessage);
                if (validRequest != 0)
                {
                    lstCouponAtTill = new List<CouponAtTill>();
                    lstCouponAtTill.Add(new CouponAtTill("", validRequest.ToString(), tmpErrorMessage));
                    Logger.Write(string.Format("BAL:GetVoucherAtTill(): CustomerId:-CustomerId = {0} - ClubcardNumber = {1} - Error Code: {2} - ErrorMessage: {3}", tillRequest.CustomerId.ToString(), tillRequest.ClubcardNumber.ToString(), validRequest.ToString(), tmpErrorMessage), "General");
                    return lstCouponAtTill;
                }

                lstCouponAtTill = new ClubcardCouponDataAccess("SmartVoucherDbServer").GetActiveVATVoucher(tillRequest.ClubcardNumber, tillRequest.CustomerId, out goForCAT);

                if ((lstCouponAtTill == null) || (lstCouponAtTill.Count == 0))
                {
                    Logger.Write(string.Format("BAL:GetVoucherAtTill(): Input Request with :- CustomerId: {0} - ClubcardNumber = {1} :- does not have an active VAT voucher to be issued or notified.", tillRequest.CustomerId, tillRequest.ClubcardNumber), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                }

                if (goForCAT)
                {
                    Logger.Write(string.Format("BAL:GetVoucherAtTill(): Input Request with :- CustomerId: {0} - ClubcardNumber = {1} :- does not have an active VAT voucher to be issued or notified. Hence going for CAT", tillRequest.CustomerId, tillRequest.ClubcardNumber), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                    lstCouponAtTill = this.GetCouponsAtTill(tillRequest, false);
                }
            }
            catch (Exception ex)
            {
                lstCouponAtTill = new List<CouponAtTill>();
                lstCouponAtTill.Add(new CouponAtTill("", "102", ConfigurationManager.AppSettings["102"]));
                Logger.Write(string.Format("BAL:GetVoucherAtTill(): CustomerId: {0} - ClubcardNumber = {1} - Message: {2}", tillRequest.CustomerId, tillRequest.ClubcardNumber, ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }

            return lstCouponAtTill;
        }
        #endregion

        #region Enquery Servide Methods

        /// <summary>
        /// This method validates the available coupon request and passes the request to data access layer on successful validation of the input request
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>Returns available coupon response</returns>
        /// <remarks>This is where we will do validation of input request and on successfull validation of the request, call the DAL method to process the request
        /// If the validation fails, we will pass the proper error code and error message as the response</remarks>
        public AvailableCouponResponse ProcessAvailableCouponRequest(AvailableCouponRequest req)
        {
            AvailableCouponResponse response = null;

            try
            {
                response = new AvailableCouponResponse();

                //Validates all input parameters
                int respCode = req.ValidateCouponRequest();

                if (respCode == 0)
                {
                    response = new ClubcardCouponDataAccess("ReportingServer").GetAvailableCouponResponse(req.HouseHoldId.Value, req.ImageRequired);
                }
                else
                {
                    response.ErrorStatusCode = "100";
                    response.ErrorMessage = ConfigurationManager.AppSettings["100"];
                    Logger.Write("EnquiryService : At least HouseHoldId or ClubCardNumber should be provided to retrieve available coupon information.", "General");
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrorStatusCode = "102";
                response.ErrorMessage = ConfigurationManager.AppSettings["102"];
                Logger.Write("BAL:ProcessAvailableCouponRequest(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return response;
            }
            finally
            {
                response = null;
            }
        }


        /// <summary>
        /// This method validates the redeemed coupon request and passes the request to data access layer on successful validation of the input request
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>RedeemedCouponResponse object</returns>
        /// <remarks>This is where we will do validation of input request and on successfull validation of the request, call the DAL method to process the request
        /// If the validation fails, we will pass the proper error code and error message as the response</remarks>
        public RedeemedCouponResponse ProcessRedeemedCouponRequest(RedeemedCouponRequest req)
        {
            RedeemedCouponResponse response = null;

            try
            {
                response = new RedeemedCouponResponse();

                //Validates all input parameters
                int respCode = req.ValidateCouponRequest();

                if (respCode == 0)
                {
                    response = new ClubcardCouponDataAccess("ReportingServer").GetRedeemedCouponDetails(req.HouseHoldId.Value, req.RedemptionLength);
                }
                else
                {
                    response.RequestedHouseHoldId = req.HouseHoldId;
                    response.ErrorStatusCode = "100";
                    response.ErrorMessage = ConfigurationManager.AppSettings["100"];
                    Logger.Write("EnquiryService : At least HouseHoldId or ClubCardNumber should be provided to retrieve redeemed coupon information.", "General");
                }

                return response;
            }
            catch (Exception ex)
            {
                response.RequestedHouseHoldId = req.HouseHoldId;
                response.ErrorStatusCode = "102";
                response.ErrorMessage = ConfigurationManager.AppSettings["102"];
                Logger.Write("BAL:ProcessRedeemedCouponRequest(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return response;
            }
            finally
            {
                response = null;
            }
        }


        /// <summary>
        /// This method validates the coupon information request and passes the request to data access layer on successful validation of the input request and get the coupon information details
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>CouponInformationResponse object</returns>
        /// <remarks>This is where we will do validation of input request and on successfull validation of the request, call the DAL method to process the request
        /// If the validation fails, we will pass the proper error code and error message as the response</remarks>
        public CouponInformationResponse ProcessGetCouponInformation(CouponInformationRequest req)
        {
            CouponInformationResponse response = null;
            CouponInformation info = null;

            try
            {
                response = new CouponInformationResponse();
                info = new CouponInformation();

                //Validate for all input parameters
                int respCode = req.ValidateCouponInfoRequest();

                if (respCode == 0)
                {
                    response = new ClubcardCouponDataAccess("ReportingServer").GetCouponInfoDetails(req.SmartBarcode, req.SmartAlphaCode, req.ImageRequired);
                }
                else
                {
                    response.CouponDetails = info;
                    response.CouponDetails.SmartAlphaNumeric = req.SmartAlphaCode;
                    response.CouponDetails.SmartBarcode = req.SmartBarcode;
                    //response.ErrorStatusCode = "100";
                    response.ErrorStatusCode = respCode.ToString();
                    //response.ErrorMessage = ConfigurationManager.AppSettings["100"];
                    response.ErrorMessage = ConfigurationManager.AppSettings[respCode.ToString()];
                    Logger.Write("EnquiryService : At least SmartBarcode or SmartAlphaCode should be provided as a parameter to the service.", "General");
                }

                return response;
            }
            catch (Exception ex)
            {
                response.CouponDetails = new CouponInformation();
                response.CouponDetails.SmartAlphaNumeric = req.SmartAlphaCode;
                response.CouponDetails.SmartBarcode = req.SmartBarcode;
                response.ErrorStatusCode = "102";
                response.ErrorMessage = ConfigurationManager.AppSettings["102"];
                Logger.Write("BAL:ProcessGetCouponInformation(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return response;
            }
            finally
            {
                response = null;
                info = null;
            }
        }

        /// <summary>
        /// This method validates the coupon information request and passes the request to data access layer on successful validation of the input request
        /// </summary>
        /// <param name="req">CheckOutRequest object</param>
        /// <returns>CheckOutResponse object</returns>
        /// <remarks>This is where we will do validation of input request and on successfull validation of the request, call the DAL method to process the request
        /// If the validation fails, we will pass the proper error code and error message as the response</remarks>
        public CheckOutResponse ProcessValidateCheckOut(CouponInformationRequest req)
        {
            CheckOutResponse response = null;

            try
            {
                response = new CheckOutResponse();

                //Validate for all input parameters
                int respCode = req.ValidateCheckOutRequest();

                if (respCode == 0)
                {
                    response = new ClubcardCouponDataAccess().ValidateCheckOut(req.SmartAlphaCode);
                }
                else
                {
                    response.RedemptionStatus = -1;
                    response.SmartAlphaNumericCode = req.SmartAlphaCode;
                    response.ErrorStatusCode = respCode.ToString();
                    response.ErrorMessage = ConfigurationManager.AppSettings[response.ErrorStatusCode];
                }

                return response;
            }
            catch (Exception ex)
            {
                response.SmartAlphaNumericCode = req.SmartAlphaCode;
                response.RedemptionStatus = -1;
                response.ErrorStatusCode = "102";
                response.ErrorMessage = ConfigurationManager.AppSettings["102"];
                Logger.Write(ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return response;
            }
            finally
            {
                response = null;
            }
        }
        #endregion

        #region AdHoc Service Methods

        /// <summary>
        /// This function will process each request object by performing business validation and 
        /// forwarding to dal layer.
        /// </summary>
        /// <param name="couponRequest"></param>
        /// <returns>ListAdHocCouponResponse</returns>
        public AdHocCouponResponse IssueAdHocCoupons(AdHocCouponRequest couponRequest)
        {
            AdHocCouponResponse adHocResponse = new AdHocCouponResponse();

            try
            {
                //Logging in text file
                Logger.Write("BAL:IssueAhHocCoupons(): Input Request - " + couponRequest.LogInfo(), "Information");
                string tmpErrorMessage = string.Empty;
                short validRequest = couponRequest.ValidateAdHocCouponRequest(ref tmpErrorMessage);
                if (validRequest != 0)
                {
                    adHocResponse.CouponErrorCode = validRequest;
                    adHocResponse.CouponErrorMessage = tmpErrorMessage;
                }
                else
                {
                    //1. Fetch appropriate coupon class
                    adHocResponse = new ClubcardCouponDataAccess().GetAdHocCouponClass(couponRequest);
                    // If there is no error in getting coupon class info then proceed
                    if (adHocResponse.CouponErrorCode == 0)
                    {
                        //2. Generate smart barcode and alphacode
                        GetSmartCodes(adHocResponse);
                    }
                    // If there is no error in generation of smart barcodes and alphacodes then proceed
                    if (adHocResponse.CouponErrorCode == 0)
                    {
                        //3. Save issuance info in tables
                        adHocResponse = new ClubcardCouponDataAccess().SaveAdHocCouponInfo(couponRequest, adHocResponse);
                    }
                }
                Logger.Write("BAL:IssueAhHocCoupons(): Output Response - " + adHocResponse.LogInfo(), "Information");
            }
            catch (Exception ex)
            {
                adHocResponse.CouponErrorCode = -1;
                adHocResponse.CouponErrorMessage = "Internal Error Occured";
                Logger.Write("BAL:IssueAhHocCoupons(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            return adHocResponse;
        }


        /// <summary>
        /// Will generate smart codes if required
        /// </summary>
        /// <param name="adHocResponse"></param>
        private void GetSmartCodes(AdHocCouponResponse adHocResponse)
        {
            Int32? randomBarCode = null;
            string alphaNumber = string.Empty;
            new ClubcardCouponDataAccess().GetAdHocSmartCodes(adHocResponse, out randomBarCode, out alphaNumber);
            if (String.IsNullOrWhiteSpace(alphaNumber) == false)
                adHocResponse.SmartAlphaNumericCode = adHocResponse.AlphaCode + alphaNumber;
            else //Copy alphacode from couponclass table
                adHocResponse.SmartAlphaNumericCode = adHocResponse.AlphaCode;

            if ((randomBarCode != null) && (randomBarCode > 0))
                adHocResponse.SmartBarcodeNumber = UtilityClass.GetSmartBarcode(Convert.ToString(randomBarCode), Convert.ToString(adHocResponse.EANBarcode));
            else //Copy EANBarcode from couponclass table
                adHocResponse.SmartBarcodeNumber = adHocResponse.EANBarcode;
        }

        #endregion
    }
}