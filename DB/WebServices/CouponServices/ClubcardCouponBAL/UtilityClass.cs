using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;
using Tesco.Marketing.IT.ClubcardCoupon.DAL;

namespace Tesco.Marketing.IT.ClubcardCoupon.BAL
{
    /// <summary>
    /// Utility class for generic validations
    /// </summary>
    public static class UtilityClass
    {
        /// <summary>
        /// This is an extension method it it will
        /// Validate if Coupon class object is properly
        /// populated or not
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns true if the couopn class object is properly populated</returns>
        public static bool ValidateCouponClass(this CouponClass tmpObj)
        {

            if ((tmpObj.TriggerNumber == null) || (tmpObj.TriggerNumber == 0))
                return false;
            if (String.IsNullOrWhiteSpace(tmpObj.StatementNumber))
                return false;
            if (String.IsNullOrWhiteSpace(tmpObj.CouponDescription))
                return false;
            if ((tmpObj.RedemptionEndDate == null) || (tmpObj.RedemptionEndDate == DateTime.MinValue))
                return false;
            if (String.IsNullOrWhiteSpace(tmpObj.IssuanceChannel))
                return false;
            else
            {
                if (tmpObj.IssuanceChannel.ToUpper().Contains("TILL"))
                    tmpObj.IssuanceChannel = "TILL";
            }

            if (String.IsNullOrWhiteSpace(tmpObj.RedemptionChannel))
                return false;
            if ((tmpObj.MaxRedemptionLimit == null) || (tmpObj.MaxRedemptionLimit == 0))
                return false;
            if (!((tmpObj.ListCouponLineInfo == null) || (tmpObj.ListCouponLineInfo.Count == 0)))
            {
                if (String.IsNullOrWhiteSpace(tmpObj.TillCouponTemplateNumber))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This is an extension method it it will
        /// Validate if CouponLineTextInfo object is properly
        /// populated or not
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns true if the couponlinetextinfo is properly populated</returns>
        public static bool ValidateCouponLine(this CouponLineTextInfo tmpObj)
        {
            if (String.IsNullOrWhiteSpace(tmpObj.LineNumber))
                return false;
            else
            {
                if (tmpObj.LineNumber.IsLineNumberFormat() == false)
                    return false;
            }
            if (String.IsNullOrWhiteSpace(tmpObj.LineText))
                return false;
            if (tmpObj.CharacterWidth == 0)
                return false;
            if (tmpObj.CharacterWeigth == 0)
                return false;
            if (tmpObj.LineUsed.IsFormatValid() == false)
                return false;
            if (tmpObj.UnderLine.IsFormatValid() == false)
                return false;
            if (tmpObj.Italic.IsFormatValid() == false)
                return false;
            if (tmpObj.WhiteOnBlack.IsFormatValid() == false)
                return false;
            if (tmpObj.Center.IsFormatValid() == false)
                return false;
            if (tmpObj.Barcode.IsFormatValid() == false)
                return false;
            return true;
        }

        /// <summary>
        /// This is an extension method it it will
        /// Validate if CheckOutrequest object is properly
        /// populated or not as per Store redeem request
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns>Returns 100 if SmartBarcode not available, returns 102 for any Internal operation failures</returns>
        public static int ValidateStoreRedeem(this CheckOutRequest tmpObj, ref string errorMessage)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(tmpObj.SmartBarcodeNumber))
                {
                    errorMessage = "SmartBarcode not available";
                    return 100;
                }
                if (tmpObj.TxnTimeStamp == DateTime.MinValue)
                {
                    errorMessage = "TxnTimeStamp not available";
                    return 100;
                }
                if (tmpObj.StoreNumber == 0)
                {
                    errorMessage = "Store No not available";
                    return 100;
                }
                if (tmpObj.TillType == 0)
                {
                    errorMessage = "TillType not available";
                    return 100;
                }
                if (tmpObj.TillId == 0)
                {
                    errorMessage = "TillId not available";
                    return 100;
                }
                if (!tmpObj.SmartBarcodeNumber.IsBarCodeFormat())
                {
                    errorMessage = "SmartBarcode is not in proper format";
                    return 101;
                }
                if (!String.IsNullOrWhiteSpace(tmpObj.ClubcardNumber))
                {
                    if (!tmpObj.ClubcardNumber.IsClubcardFormat())
                    {
                        errorMessage = "Clubcard No is not in proper format";
                        return 101;
                    }
                }
            }
            catch
            {
                errorMessage = "Internal operation failed";
                return 102;
            }
            return 0;

        }

        /// <summary>
        /// This is an extension method it it will
        /// Validate if CheckOutrequest object is properly
        /// populated or not as per GHS redeem request
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns>Returns 100 if SmartAlphaNumericCode not available, returns 102 for any Internal operation failure</returns>
        public static int ValidateGHSRedeem(this CheckOutRequest tmpObj, ref string errorMessage)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(tmpObj.SmartAlphaNumericCode))
                {
                    errorMessage = "SmartAlphaNumericCode not available";
                    return 100;
                }
                if (tmpObj.TxnTimeStamp == DateTime.MinValue)
                {
                    errorMessage = "TxnTimeStamp not available";
                    return 100;
                }

                if (!String.IsNullOrWhiteSpace(tmpObj.ClubcardNumber))
                {
                    if (!tmpObj.ClubcardNumber.IsClubcardFormat())
                    {
                        errorMessage = "Clubcard No is not in proper format";
                        return 101;
                    }
                }

            }
            catch
            {
                errorMessage = "Internal operation failed";
                return 102;
            }
            return 0;
        }

        /// <summary>
        /// This method is used to Check the valid Barcode format
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns true for valid barcode format</returns>
        public static bool IsBarCodeFormat(this string tmpObj)
        {
            try
            {
                Convert.ToDecimal(tmpObj);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to Check the valid Clubcard format
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns true for valid clubcard format</returns>
        public static bool IsClubcardFormat(this string tmpObj)
        {
            try
            {
                Convert.ToInt64(tmpObj);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to Check the valid LineNumber format        
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns true for valid linenumber format</returns>
        public static bool IsLineNumberFormat(this string tmpObj)
        {
            Int64 tmpInt;
            try
            {
                tmpInt = Convert.ToInt64(tmpObj);
                if ((tmpInt > 0) && (tmpInt < 11))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This method is used to Check the valid format
        /// </summary>
        /// <param name="tmpChar">tmpChar</param>
        /// <returns>Returns true for valid format</returns>
        public static bool IsFormatValid(this char tmpChar)
        {
            if ((tmpChar == 'y') || (tmpChar == 'n') || (tmpChar == 'Y') || (tmpChar == 'N'))
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method will give back log info for CheckOutRequest
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns log information of checkout request</returns>
        public static string LogInfo(this CheckOutRequest tmpObj)
        {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(" SessionId = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SessionId))
            {
                sbTemp.Append(tmpObj.SessionId);
            }
            sbTemp.Append(" RedemptionChannel = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.RedemptionChannel.ToString()))
            {
                sbTemp.Append(tmpObj.RedemptionChannel.ToString());
            }
            sbTemp.Append(" SmartBarcodeNumber = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartBarcodeNumber))
            {
                sbTemp.Append(tmpObj.SmartBarcodeNumber);
            }
            sbTemp.Append(" SmartAlphaNumericCode = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartAlphaNumericCode))
            {
                sbTemp.Append(tmpObj.SmartAlphaNumericCode);
            }
            sbTemp.Append(" Clubcard No = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.ClubcardNumber))
            {
                sbTemp.Append(tmpObj.ClubcardNumber);
            }

            sbTemp.Append(" TxnTimeStamp = " + tmpObj.TxnTimeStamp.ToString());

            sbTemp.Append(" IsOfflineTransaction = " + tmpObj.IsOfflineTransaction.ToString());

            sbTemp.Append(" IsReversal = " + tmpObj.IsReversal.ToString());

            return sbTemp.ToString();
        }

        /// <summary>
        /// This method will give back log info for CheckOutResponse
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <returns>Returns log information for checkout response</returns>
        public static string LogInfo(this CheckOutResponse tmpObj)
        {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(" SessionId = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SessionId))
            {
                sbTemp.Append(tmpObj.SessionId);
            }
            sbTemp.Append(" SmartBarcodeNumber = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartBarcodeNumber))
            {
                sbTemp.Append(tmpObj.SmartBarcodeNumber);
            }
            sbTemp.Append(" SmartAlphaNumericCode = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartAlphaNumericCode))
            {
                sbTemp.Append(tmpObj.SmartAlphaNumericCode);
            }

            sbTemp.Append(" RedemptionStatus = ");
            sbTemp.Append(tmpObj.RedemptionStatus.ToString());


            sbTemp.Append(" ErrorStatusCode = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.ErrorStatusCode))
            {
                sbTemp.Append(tmpObj.ErrorStatusCode);
            }

            sbTemp.Append(" ErrorMessage = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.ErrorMessage))
            {
                sbTemp.Append(tmpObj.ErrorMessage);
            }
            return sbTemp.ToString();
        }

        /// <summary>
        /// Give back log info for AdHocCouponRequest
        /// to log into text file
        /// </summary>
        /// <param name="tmpObj"></param>
        /// <returns></returns>
        public static string LogInfo(this AdHocCouponRequest tmpObj)
        {
            StringBuilder sbTemp = new StringBuilder();

            sbTemp.Append(" ClubcardNumber = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.ClubcardNumber))
            {
                sbTemp.Append(tmpObj.ClubcardNumber);
            }
            sbTemp.Append(" MailingNumber = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.MailingNumber))
            {
                sbTemp.Append(tmpObj.MailingNumber);
            }

            sbTemp.Append(" TriggerNumber = ");
            sbTemp.Append(tmpObj.TriggerNumber.ToString());


            return sbTemp.ToString();
        }

        /// <summary>
        /// Give back log info for AdHocCouponResponse
        /// </summary>
        /// <param name="tmpObj"></param>
        /// <returns></returns>
        public static string LogInfo(this AdHocCouponResponse tmpObj)
        {
            StringBuilder sbTemp = new StringBuilder();

            sbTemp.Append(" CouponExpiryDate = ");
            sbTemp.Append(tmpObj.CouponExpiryDate.ToString());

            sbTemp.Append(" CouponInstanceId = ");
            sbTemp.Append(tmpObj.CouponInstanceId.ToString());

            sbTemp.Append(" MaxRedemptionLimit = ");
            sbTemp.Append(tmpObj.MaxRedemptionLimit.ToString());

            sbTemp.Append(" SmartBarcodeNumber = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartBarcodeNumber))
            {
                sbTemp.Append(tmpObj.SmartBarcodeNumber);
            }

            sbTemp.Append(" SmartAlphaNumericCode = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.SmartAlphaNumericCode))
            {
                sbTemp.Append(tmpObj.SmartAlphaNumericCode);
            }

            sbTemp.Append(" CouponErrorCode = ");
            sbTemp.Append(tmpObj.CouponErrorCode.ToString());

            sbTemp.Append(" CouponErrorMessage = ");
            if (!String.IsNullOrWhiteSpace(tmpObj.CouponErrorMessage))
            {
                sbTemp.Append(tmpObj.CouponErrorMessage);
            }
            return sbTemp.ToString();
        }

        /// <summary>
        /// This method will Validate Till request
        /// </summary>
        /// <param name="tmpObj">tmpObj</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns>Returns 100 if customerId is not available, returns 102 for any Internal operation failures</returns>
        public static int ValidateTillRequest(this CouponAtTillRequest tmpObj, ref string errorMessage)
        {
            try
            {
                if (tmpObj.CustomerId == 0)
                {
                    errorMessage = "CustomerId not available";
                    return 100;
                }
            }
            catch
            {
                errorMessage = "Internal operation failed";
                return 102;
            }
            return 0;
        }

        /// <summary>
        /// This method will Validate Voucher till requestk
        /// </summary>
        /// <param name="tmpObj">Coupon At TillRequest object</param>
        /// <param name="errorMessage">Error Message</param>
        /// <returns>Returns 100 if customerId not available and returns 102 for any internal operation failure</returns>
        public static int ValidateVoucherRequest(this CouponAtTillRequest tmpObj, ref string errorMessage)
        {
            try
            {
                if (tmpObj.CustomerId == 0)
                {
                    //check customer id is required
                    errorMessage = "CustomerId not available";
                    return 100;
                }
                if (tmpObj.ClubcardNumber == 0)
                {
                    errorMessage = "Primary ClubcardNumber not available";
                    return 100;
                }
            }
            catch
            {
                errorMessage = "Internal operation failed";
                return 102;
            }
            return 0;
        }

        /// <summary>
        /// Validate AdHocCoupon Request
        /// </summary>
        /// <param name="tmpObj"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static short ValidateAdHocCouponRequest(this AdHocCouponRequest tmpObj, ref string errorMessage)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(tmpObj.MailingNumber))
                {
                    errorMessage = "Mailing Number not present in request or invalid format";
                    return 12;
                }

                if (!((tmpObj.MailingNumber.Trim().Length == 2) || (tmpObj.MailingNumber.Trim().Length == 3)))
                {
                    errorMessage = "Mailing Number not present in request or invalid format";
                    return 12;
                }

                if ((tmpObj.TriggerNumber == null) || (tmpObj.TriggerNumber == 0))
                {
                    errorMessage = "Trigger Number not present in request or invalid format";
                    return 15;
                }
            }
            catch
            {
                errorMessage = "Internal operation failed";
                return 102;
            }
            return 0;
        }

        /// <summary>
        /// Generate 22 digit barcode
        /// </summary>
        /// <param name="randomNumber"></param>
        /// <param name="EanBarcode"></param>
        /// <returns>22 Digit smart barcode</returns>
        public static string GetSmartBarcode(string randomNumber, string EanBarcode)
        {
            return CheckDigit3("92" + randomNumber + EanBarcode.Substring(2, 10));
        }

        /// <summary>
        /// Generate 22 digit barcode by passing 21 digit number
        /// </summary>
        /// <param name="SmartBarCode">Pass 21 digit to get 22nd check digit to form 22 digit barcode</param>
        /// <returns>22 Digit smart barcode </returns>
        private static string CheckDigit3(string SmartBarCode)
        {
            int CharByCharOut = 0, Alternates13 = 1, CheckSum = 0;
            foreach (char myChar in SmartBarCode)
            {
                CharByCharOut = Convert.ToInt32(myChar.ToString());
                CharByCharOut = CharByCharOut * Alternates13;
                CheckSum = CheckSum + CharByCharOut;
                Alternates13 = (Alternates13 == 1 ? 3 : 1);
            }

            CheckSum = CheckSum % 10;
            if (CheckSum != 0)
                CheckSum = 10 - CheckSum;
            return SmartBarCode + CheckSum.ToString();
        }

        #region Enquiry Service Methods
        /// <summary>
        /// This method Validates the service request objects for the populated parameteres
        /// </summary>
        /// <param name="requestObj">requestObj</param>
        /// <returns>zero if validation success else appropriate error code</returns>
        public static int ValidateCouponRequest(this AvailableCouponRequest requestObj)
        {
            if (requestObj.HouseHoldId.HasValue && requestObj.HouseHoldId != 0)
            {
                return 0;
            }
            else if (requestObj.ClubCardNumber.HasValue && requestObj.ClubCardNumber != 0)
            {
                return 0;
            }

            return 100;
        }

        /// <summary>
        /// This method Validates the service request object for the populated parameteres
        /// </summary>
        /// <param name="requestObj">requestObj</param>
        /// <returns>zero if validation success else appropriate error code</returns>
        public static int ValidateRedeemedCouponRequest(this RedeemedCouponRequest requestObj)
        {
            if (!requestObj.RedemptionLength.HasValue)
            {
                requestObj.RedemptionLength = -30;
            }
            else if (requestObj.RedemptionLength < -180)
            {
                requestObj.RedemptionLength = -180;
            }

            if (requestObj.HouseHoldId.HasValue && requestObj.HouseHoldId != 0)
            {
                return 0;
            }
            else if (requestObj.ClubCardNumber.HasValue && requestObj.ClubCardNumber != 0)
            {
                return 0;
            }

            return 100;
        }

        /// <summary>
        /// This method Validates the request objects for the populated parameteres
        /// </summary>
        /// <param name="requestObj">requestObj</param>
        /// <returns>zero if validation success else appropriate error code</returns>
        public static int ValidateCouponInfoRequest(this CouponInformationRequest requestObj)
        {
            if (!string.IsNullOrWhiteSpace(requestObj.SmartBarcode))
            {
                return 0;
            }
            else if (!string.IsNullOrWhiteSpace(requestObj.SmartAlphaCode))
            {
                if (requestObj.SmartAlphaCode.Length <= 12)
                    return 0;
                else
                    return 101;
            }

            return 100;
        }

        /// <summary>
        /// This method Validates the request objects for the populated parameteres
        /// </summary>
        /// <param name="requestObj">requestObj</param>
        /// <returns>True if validation success else false</returns>
        public static int ValidateCheckOutRequest(this CouponInformationRequest requestObj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestObj.SmartAlphaCode))
                {
                    return 100;
                }

                //else if ((!requestObj.ClubcardNumber.HasValue || requestObj.ClubcardNumber == 0)
                //     && (!requestObj.HouseholdId.HasValue || requestObj.HouseholdId == 0))
                //{
                //    return 100;
                //}
            }
            catch
            {
                return 102;
            }

            return 0;
        }
        #endregion
    }
}
