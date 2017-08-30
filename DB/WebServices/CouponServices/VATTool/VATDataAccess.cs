using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VATTool
{
    public class VATDataAccess
    {
        Database clubcardCouponDb = null;

        public VATDataAccess()
        {
            clubcardCouponDb = DatabaseFactory.CreateDatabase("ClubcardCouponDBServer");
        }

        #region Insert Coupon Class
        /// <summary>
        /// It will save data to coupon class fetching from the passed object to the database through the  USP_InsertCouponClass SP
        /// </summary>
        /// <param name="obj">Coupon Class object with all input parametres</param>
        /// <returns>Returns unique Coupon Class Id of the inserted record</returns>
        public Int64 InsertCouponClass(CouponClass obj)
        {
            Int64 couponClassId = 0;
            DbCommand command = null;
            decimal? barCode = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(obj.EANBarcode))
                    barCode = Convert.ToDecimal(obj.EANBarcode);
                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_InsertCouponClass", obj.TriggerNumber, obj.StatementNumber, obj.CouponDescription, obj.CouponImageThumbnail, obj.CouponImageFull, obj.ThumbnailImageName, obj.FullImageName, obj.RedemptionEndDate, obj.IssuanceStartDate, obj.IssuanceStartTime, obj.IssuanceEndDate, obj.IssuanceEndTime, obj.IssuanceChannel.ToString(), obj.RedemptionChannel, obj.MaxRedemptionLimit, obj.AlphaCode, barCode, obj.IsGenerateSmartCodes, obj.TillCouponTemplateNumber);
                clubcardCouponDb.ExecuteNonQuery(command);
                couponClassId = Convert.ToInt64(command.Parameters["@RETURN_VALUE"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
            return couponClassId;
        }
        #endregion

        #region Insert Till Coupon Line
        /// <summary>
        /// It will save data to coupon class fetching from the passed object to the database through the  USP_InsertCouponClass SP
        /// </summary>
        /// <param name="obj">Coupon Line Text class object with line information</param>
        /// <param name="couponClassId">Coupon class id to save the line datils against</param>
        public void InsertTillCouponLine(CouponLineTextInfo obj, Int64 couponClassId)
        {
            try
            {
                clubcardCouponDb.ExecuteNonQuery("USP_InsertTillCouponLine", couponClassId, obj.LineNumber, obj.LineText, obj.LineUsed, obj.UnderLine, obj.Italic, obj.WhiteOnBlack, obj.Center, obj.Barcode, obj.CharacterWidth, obj.CharacterWeigth);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get VAT format details
        /// <summary>
        /// Gets the coupon class and cpupon line details based on padded statement number and trigger number
        /// </summary>
        /// <param name="StatementNumber">Mailing Number</param>
        /// <param name="TriggerNumber">Trigger Number</param>
        /// <returns>Redurtns Dataset with two tables, table 1 - coupon class details anf table 2 with coupon line details</returns>
        public DataSet GetVAT(string StatementNumber, int TriggerNumber)
        {
            DbCommand command = null;
            try
            {                
                DataSet dsVAT = new DataSet();

                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetVoucherAtTillCouponLine", StatementNumber, TriggerNumber);
                dsVAT = clubcardCouponDb.ExecuteDataSet(command);

                return dsVAT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
        }
        #endregion
    }

    #region Coupon Class
    public class CouponClass
    {

        public Int16? TriggerNumber { get; set; }


        public string StatementNumber { get; set; }


        public string CouponDescription { get; set; }


        public byte[] CouponImageThumbnail { get; set; }


        public byte[] CouponImageFull { get; set; }


        public string ThumbnailImageName { get; set; }


        public string FullImageName { get; set; }


        public DateTime? RedemptionEndDate { get; set; }


        public DateTime? IssuanceStartDate { get; set; }


        public DateTime? IssuanceStartTime { get; set; }


        public DateTime? IssuanceEndDate { get; set; }


        public DateTime? IssuanceEndTime { get; set; }


        public string IssuanceChannel { get; set; }


        public string RedemptionChannel { get; set; }


        public Int16? MaxRedemptionLimit { get; set; }


        public string AlphaCode { get; set; }


        public string EANBarcode { get; set; }


        public bool IsGenerateSmartCodes { get; set; }


        public string TillCouponTemplateNumber { get; set; }


        public List<CouponLineTextInfo> ListCouponLineInfo { get; set; }
    }

    #endregion

    #region Coupon Line Text
    public class CouponLineTextInfo
    {

        public string LineNumber { get; set; }


        public string LineText { get; set; }


        public char LineUsed { get; set; }


        public char UnderLine { get; set; }


        public char Italic { get; set; }


        public char WhiteOnBlack { get; set; }


        public char Center { get; set; }


        public char Barcode { get; set; }


        public byte CharacterWidth { get; set; }


        public byte CharacterWeigth { get; set; }
    }

    #endregion

    #region LineTextValidation
    public static class LineTextValidation
    {
        public static bool ValidateCouponLine(this CouponLineTextInfo tmpObj)
        {
            if (String.IsNullOrWhiteSpace(tmpObj.LineNumber))
                return false;
            else
            {
                if (tmpObj.LineNumber.IsLineNumberFormat() == false)
                    return false;
            }
            //if (String.IsNullOrWhiteSpace(tmpObj.LineText))
            //    return false;
            if (tmpObj.CharacterWidth < 0 || tmpObj.CharacterWidth > 8)
                return false;
            if (tmpObj.CharacterWeigth < 0 || tmpObj.CharacterWeigth > 8)
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
        /// IsLineNumberFormat is to check the format of 
        /// line number
        /// </summary>
        /// <param name="tmpObj"></param>
        /// <returns></returns>
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

        public static bool IsFormatValid(this char tmpChar)
        {
            if ((tmpChar == 'y') || (tmpChar == 'n') || (tmpChar == 'Y') || (tmpChar == 'N'))
                return true;
            else
                return false;
        }

    }
    #endregion

}
