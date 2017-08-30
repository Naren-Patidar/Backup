using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;


namespace Tesco.Marketing.IT.ClubcardCoupon.DAL
{
    /// <summary>
    /// Clubcard Coupon Data Access Layer to interact with database for DML Operations
    /// </summary>
    /// <remarks>Communicates with Database for DML Operations</remarks>
    public class ClubcardCouponDataAccess
    {
        Database clubcardCouponDb = null;
        static int _intAdHocRedemptionEndPeriod;
        static int _intAdHocRedemptionStartPeriod;

        /// <summary>
        /// This method will support ClubcardCoupon Database creation
        /// </summary>
        public ClubcardCouponDataAccess()
        {
            clubcardCouponDb = DatabaseFactory.CreateDatabase("OLTPDbServer");
        }

        /// <summary>
        /// This method supports creation of database in the specified database provided in the connectionStringName
        /// </summary>
        /// <param name="connectionStringName">connectionStringName for connecting to Clubcard Coupon database</param>
        public ClubcardCouponDataAccess(string connectionStringName)
        {
            clubcardCouponDb = DatabaseFactory.CreateDatabase(connectionStringName);
        }

        /// <summary>
        /// This method will insert the CouponClass obj values into CouponClass table of Clubcard Coupon database
        /// </summary>
        /// <param name="obj">CouponClass obj</param>
        /// <remarks>This method  interacts with database and insert coupon class details to Coupon Class table of Clubcard Coupons Database</remarks>        
        /// <returns>Returns integer</returns>
        /// <exception cref="System.Exception">Occurs if database connectivity has any issue</exception>
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
                Logger.Write("DAL:InsertCouponClass(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return 0;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
            return couponClassId;
        }

        /// <summary>
        /// This Method is used to InsertTillCouponLines to Clubcard Coupon Database for coupons to be issued at Till
        /// </summary>
        /// <param name="obj">CouponLineTextInfo obj</param>
        /// <param name="couponClassId">Any positive integer</param>
        /// <remarks>Inserts TillCouponLines to TillCouponLine table of Clubcard Coupon database which will be used by storeline for printing the coupons at Tills</remarks>
        public void InsertTillCouponLine(CouponLineTextInfo obj, Int64 couponClassId)
        {
            clubcardCouponDb.ExecuteNonQuery("USP_InsertTillCouponLine", couponClassId, obj.LineNumber, obj.LineText, obj.LineUsed, obj.UnderLine, obj.Italic, obj.WhiteOnBlack, obj.Center, obj.Barcode, obj.CharacterWidth, obj.CharacterWeigth);
        }

        /// <summary>
        /// This method will return the Redmeption Status of Till Coupons
        /// </summary>
        /// <param name="dSmartBarCode">The SmartBarcode of a Coupon</param>
        /// <param name="couponInstanceId">InstanceId of a Coupon</param>
        /// <param name="redemptionCount">Specifies how many times a coupon has been redeemed</param>
        /// <param name="maxRedemptionLimit">Specified how many times a coupon can be redeemed</param>
        /// <returns>Returns Integer for each status</returns>
        /// <remarks>This will return 15 if the coupon is already redeemed, 16 if the coupon is expired. If the coupon redemption status is 0, then the coupon will be redeemed at Till</remarks>
        public Int16 GetTillCouponRedemptionStatus(decimal dSmartBarCode, out Int64 couponInstanceId, out Int16 redemptionCount, out Int16 maxRedemptionLimit)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            couponInstanceId = 0;
            redemptionCount = 0;
            maxRedemptionLimit = 0;

            try
            {
                ReadAdHocConfigParams();
                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetCouponRedemptionStatus");

                clubcardCouponDb.AddInParameter(command, "@SmartBarCode", DbType.Decimal, dSmartBarCode);
                clubcardCouponDb.AddInParameter(command, "@AdHocRedemptionStartPeriod", DbType.Int32, _intAdHocRedemptionStartPeriod);
                clubcardCouponDb.AddInParameter(command, "@AdHocRedemptionEndPeriod", DbType.Int32, _intAdHocRedemptionEndPeriod);
                clubcardCouponDb.AddOutParameter(command, "@CouponInstanceId", DbType.Int64, 10);
                clubcardCouponDb.AddOutParameter(command, "@RedemptionCount", DbType.Int16, 5);
                clubcardCouponDb.AddOutParameter(command, "@MaxRedemptionLimit", DbType.Int16, 5);
                clubcardCouponDb.AddParameter(command, "@RETURN_VALUE", DbType.Int16, 5, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null);

                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
                couponInstanceId = Convert.ToInt64((clubcardCouponDb.GetParameterValue(command, "@CouponInstanceId")).ToString());
                redemptionCount = Convert.ToInt16((clubcardCouponDb.GetParameterValue(command, "@RedemptionCount")).ToString());
                maxRedemptionLimit = Convert.ToInt16((clubcardCouponDb.GetParameterValue(command, "@MaxRedemptionLimit")).ToString());
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetTillCouponRedemptionStatus(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        private void ReadAdHocConfigParams()
        {
            if (_intAdHocRedemptionStartPeriod == 0)
                _intAdHocRedemptionStartPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["AdHocRedemptionStartPeriod"]);
            
            if (_intAdHocRedemptionEndPeriod == 0)
                _intAdHocRedemptionEndPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["AdHocRedemptionEndPeriod"]);
        }

        /// <summary>
        /// This method returns the redemption status of GHS Coupons which can be redeemed online
        /// </summary>
        /// <param name="SmartAlphaCode">The SmartAlphaCode of a Coupon</param>
        /// <param name="couponInstanceId">The instanceId of a Coupon</param>
        /// <param name="redemptionCount">Specifies how many times a coupon has been redeemed</param>
        /// <param name="maxRedemptionLimit">Specifies how many times a coupon can be redeemed</param>
        /// <returns>Integer for each status</returns> 
        /// <remarks>This will return 15 if the coupon is already redeemed, 16 if the coupon is expired. If the coupon redemption status is 0, then the customer will be able to redeem the coupon Online</remarks>
        public Int16 GetGHSCouponRedemptionStatus(string SmartAlphaCode, out Int64 couponInstanceId, out Int16 redemptionCount, out Int16 maxRedemptionLimit)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            couponInstanceId = 0;
            redemptionCount = 0;
            maxRedemptionLimit = 0;

            try
            {
                ReadAdHocConfigParams();
                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetCouponRedemptionStatus");

                clubcardCouponDb.AddInParameter(command, "@SmartAlphanumericCode", DbType.String, SmartAlphaCode);
                clubcardCouponDb.AddInParameter(command, "@AdHocRedemptionStartPeriod", DbType.Int32, _intAdHocRedemptionStartPeriod);
                clubcardCouponDb.AddInParameter(command, "@AdHocRedemptionEndPeriod", DbType.Int32, _intAdHocRedemptionEndPeriod);
                clubcardCouponDb.AddOutParameter(command, "@CouponInstanceId", DbType.Int64, 10);
                clubcardCouponDb.AddOutParameter(command, "@RedemptionCount", DbType.Int16, 5);
                clubcardCouponDb.AddOutParameter(command, "@MaxRedemptionLimit", DbType.Int16, 5);
                clubcardCouponDb.AddParameter(command, "@RETURN_VALUE", DbType.Int16, 5, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null);


                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
                couponInstanceId = Convert.ToInt64((clubcardCouponDb.GetParameterValue(command, "@CouponInstanceId")).ToString());
                redemptionCount = Convert.ToInt16((clubcardCouponDb.GetParameterValue(command, "@RedemptionCount")).ToString());
                maxRedemptionLimit = Convert.ToInt16((clubcardCouponDb.GetParameterValue(command, "@MaxRedemptionLimit")).ToString());
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetGHSCouponRedemptionStatus(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        /// <summary>
        /// This method will get the List of Coupons that can be issued at till for the input CustomerId
        /// </summary>
        /// <param name="customerId">CustomerID of which Till Coupons have to be returned</param>
        /// <returns>List of Till Coupons for a specific CustomerId</returns>
        /// <remarks>This is called by Storeline for issueing coupons at Till for a particular HouseholdId</remarks>
        public List<CouponAtTill> GetTillCouponAtTill(Int64 customerId)
        {
            List<CouponAtTill> lstCouponAtTill;
            int dataReaderHasRows = 0;
            IDataReader dataReader = null;
            Int64 couponClassId = 0;
            int noTillCoupons = 1;
            try
            {
                noTillCoupons = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfTillCoupons"]);
                lstCouponAtTill = new List<CouponAtTill>();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetCouponAtTill", customerId))
                {
                    dataReader = clubcardCouponDb.ExecuteReader(cmd);

                    while ((dataReader.Read()) && (dataReaderHasRows < noTillCoupons))
                    {
                        dataReaderHasRows++;

                        CouponAtTill tmpCoupon = new CouponAtTill();

                        tmpCoupon.CouponInstanceId = dataReader.GetInt64(0);
                        if (dataReader.IsDBNull(1) == false)
                            tmpCoupon.SmartBarCode = dataReader.GetDecimal(1);
                        if (dataReader.IsDBNull(2) == false)
                            tmpCoupon.SmartAlphaNumericCode = dataReader.GetString(2);
                        tmpCoupon.CouponTemplateId = dataReader.GetStringValue(3);
                        tmpCoupon.CouponClassId = dataReader.GetInt64(4);
                        lstCouponAtTill.Add(tmpCoupon);
                    }

                    if (dataReaderHasRows == 0)
                    {
                        return lstCouponAtTill;
                    }

                    if (dataReader.NextResult() == false) //Advances to next result set
                        return lstCouponAtTill;

                    while (dataReader.Read())
                    {
                        LineInfo tmpLine = new LineInfo();
                        PopulateLine(tmpLine, dataReader);
                        couponClassId = dataReader.GetInt64(10);
                        CouponAtTill tmpTill = lstCouponAtTill.Find((e) => { return (e.CouponClassId == couponClassId); });
                        if (tmpTill != null)
                        {
                            tmpTill.ListLineInfo.Add(tmpLine);
                            SetBarcodeInLine(tmpTill, tmpLine);
                        }
                    }
                    return lstCouponAtTill;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetTillCouponAtTill(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return null;
            }
            finally
            {
                dataReader.Close();
            }
        }


        /// <summary>
        /// This method is used to set the barcode at the user specified line for Till Coupons
        /// </summary>
        /// <param name="tmpTill">The TMP till.</param>
        /// <param name="tmpLine">The TMP line.</param>
        /// <remarks>A user can specify the line number for the barcode to be printed on Till Coupon, This can be done through CouponSetupSystem using a Screen called CouponAtTill. The user can set the template for Coupon printing at Till</remarks>        
        private void SetBarcodeInLine(CouponAtTill tmpTill, LineInfo tmpLine)
        {
            if (tmpLine.LineFormat.Length == 8)
            {
                if (tmpLine.LineFormat.Substring(5, 1).ToUpper() == "Y")
                    tmpLine.LineText = tmpTill.SmartBarCode.ToString();
            }
        }

        /// <summary>
        /// This method will Populate the Couponline for Till Coupons
        /// </summary>
        /// <param name="tmpLine">tmpLine</param>
        /// <param name="dataReader">dataReader</param>
        /// <remarks>This will assign the value for each filed like linenumber, linetext, lineused, underline, italic, whiteonblack, center, barcode, charwidth, charweight, formatline which will form a line
        /// This is done for all the 10 coupon lines to be printed at Till</remarks>
        private void PopulateLine(LineInfo tmpLine, IDataReader dataReader)
        {
            tmpLine.LineNumber = dataReader.GetString(0);
            tmpLine.LineText = dataReader.GetString(1);
            string lineUsed = dataReader.GetString(2);
            string underLine = dataReader.GetString(3);
            string italic = dataReader.GetString(4);
            string whiteOnBlack = dataReader.GetString(5);
            string center = dataReader.GetString(6);
            string barcode = dataReader.GetString(7);
            Int16 charWidth = dataReader.GetByte(8);
            Int16 charWeight = dataReader.GetByte(9);
            string formatLine = lineUsed + underLine + italic + whiteOnBlack + center + barcode + charWidth.ToString() + charWeight.ToString();
            tmpLine.LineFormat = formatLine;
        }

        /// <summary>
        /// This method is used to Unredeem a Coupon which is redeemed from StoreLine
        /// </summary>
        /// <param name="checkOutRequest">checkOutRequest</param>
        /// <returns>Returns 1 if the coupon has to be unredeemed, else 0</returns>
        /// <remarks>When a coupon is redeemed from storeline and before completing the transaction if a customer wanted to void the coupon then this method will be called to unredeem the redeemed coupon</remarks>
        public Int16 UnRedeemStoreLineCoupon(CheckOutRequest checkOutRequest)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            Int64? bigClubcardNumber = null;

            try
            {
                decimal dSmartBarCode = Convert.ToDecimal(checkOutRequest.SmartBarcodeNumber);
                if (!String.IsNullOrWhiteSpace(checkOutRequest.ClubcardNumber))
                    bigClubcardNumber = Convert.ToInt64(checkOutRequest.ClubcardNumber);
                Int16 channelCode = 1;
                Int16 storeNumber = checkOutRequest.StoreNumber;
                Int16 tillId = checkOutRequest.TillId;
                Int16 tillType = checkOutRequest.TillType;
                DateTime txnTime = checkOutRequest.TxnTimeStamp;
                string cashierNumber = checkOutRequest.CashierNumber;
                bool isOffline = checkOutRequest.IsOfflineTransaction;
                bool isReversal = checkOutRequest.IsReversal;
                if (checkOutRequest.IsReversal)
                    command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_UnRedeemTillCoupon", dSmartBarCode, bigClubcardNumber, channelCode, storeNumber, tillId, tillType, txnTime, cashierNumber, isOffline, isReversal);
                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:RedeemStoreLineCoupon(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        /// <summary>
        /// To method is used to unredeem a coupon which is redeemed from dotcom
        /// </summary>
        /// <param name="checkOutRequest">checkOutRequest</param>
        /// <returns>Returns 1 if the coupon has to be unredeemed, else 0</returns>
        /// <remarks>When a customer redeems a coupon Online and before completing the transaction if customer wanted to void the coupon due to some reason then this method will be used to unredeem the coupon</remarks>
        public Int16 UnRedeemDotComCoupon(CheckOutRequest checkOutRequest)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            Int64? bigClubcardNumber = null;
            try
            {

                string smartAlphaCode = checkOutRequest.SmartAlphaNumericCode;
                if (!String.IsNullOrWhiteSpace(checkOutRequest.ClubcardNumber))
                    bigClubcardNumber = Convert.ToInt64(checkOutRequest.ClubcardNumber);
                Int16 channelCode = 0;
                DateTime txnTime = checkOutRequest.TxnTimeStamp;
                bool isOffline = checkOutRequest.IsOfflineTransaction;
                bool isReversal = checkOutRequest.IsReversal;
                if (checkOutRequest.IsReversal)
                    command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_UnRedeemGHSCoupon", smartAlphaCode, bigClubcardNumber, channelCode, txnTime, isOffline, isReversal);
                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:RedeemDotComCoupon(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        /// <summary>
        /// This method is used to Save the redemptions details when a coupon is redeemed from store
        /// </summary>
        /// <param name="checkOutRequest">checkOutRequest</param>
        /// <param name="redemptionStatus">Redemption status of a Coupon</param>
        /// <param name="couponInstanceId">The coupon instanceId of a Coupon</param>
        /// <param name="redemptionCount">Specifies how many times a coupon has been redeemed</param>
        /// <param name="maxRedemptionLimit">Specifies how many times a coupon can be redeemed</param>
        /// <returns>Returns integer based on the redemption status of a Coupon</returns>
        /// <remarks>When a Coupon is redeemed from storeline, this method is called to save redemption details of the coupon.
        /// Assigns 15 if the coupon is redeemed successfully and the details will be saved in CC database for that particular coupon</remarks>
        public Int16 SaveStoreRedeemInfo(CheckOutRequest checkOutRequest, Int16 redemptionStatus, Int64 couponInstanceId, Int16 redemptionCount, Int16 maxRedemptionLimit)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            Int64? bigClubcardNumber = null;
            try
            {
                decimal dSmartBarCode = Convert.ToDecimal(checkOutRequest.SmartBarcodeNumber);
                if (!String.IsNullOrWhiteSpace(checkOutRequest.ClubcardNumber))
                    bigClubcardNumber = Convert.ToInt64(checkOutRequest.ClubcardNumber);
                Int16 channelCode = 1;
                Int16 storeNumber = checkOutRequest.StoreNumber;
                Int16 tillId = checkOutRequest.TillId;
                Int16 tillType = checkOutRequest.TillType;
                DateTime txnTime = checkOutRequest.TxnTimeStamp;
                string cashierNumber = checkOutRequest.CashierNumber;
                bool isOffline = checkOutRequest.IsOfflineTransaction;
                bool isReversal = checkOutRequest.IsReversal;
                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_RedeemCoupon");

                clubcardCouponDb.AddInParameter(command, "@SmartBarCode", DbType.Decimal, dSmartBarCode);
                clubcardCouponDb.AddInParameter(command, "@ClubcardNumber", DbType.Int64, bigClubcardNumber);
                clubcardCouponDb.AddInParameter(command, "@ChannelCode", DbType.Int16, channelCode);
                clubcardCouponDb.AddInParameter(command, "@StoreNumber", DbType.Int16, storeNumber);
                clubcardCouponDb.AddInParameter(command, "@TillId", DbType.Int16, tillId);
                clubcardCouponDb.AddInParameter(command, "@TillType", DbType.Int16, tillType);
                clubcardCouponDb.AddInParameter(command, "@TxnTimeStamp", DbType.DateTime, txnTime);
                clubcardCouponDb.AddInParameter(command, "@CashierNumber", DbType.String, cashierNumber);
                clubcardCouponDb.AddInParameter(command, "@IsOfflineTransaction", DbType.Boolean, isOffline);
                clubcardCouponDb.AddInParameter(command, "@IsReversal", DbType.Boolean, isReversal);
                clubcardCouponDb.AddInParameter(command, "@RedemptionStatusId", DbType.Int16, redemptionStatus);
                clubcardCouponDb.AddInParameter(command, "@CouponInstanceId", DbType.Int64, couponInstanceId);
                clubcardCouponDb.AddInParameter(command, "@RedemptionCount", DbType.Int16, redemptionCount);
                clubcardCouponDb.AddInParameter(command, "@MaxRedemptionLimit", DbType.Int16, maxRedemptionLimit);
                clubcardCouponDb.AddParameter(command, "@RETURN_VALUE", DbType.Int16, 5, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null);

                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:SaveStoreRedeemInfo(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        /// <summary>
        /// This method is used to Save the redemptions details when a coupon is redeemed from Dotcom
        /// </summary>
        /// <param name="checkOutRequest">Request has the coupon details of a coupon to be redeemed</param>
        /// <param name="redemptionStatus">Redemption status of a Coupon</param>
        /// <param name="couponInstanceId">The CouponInstanceId of a Coupon</param>
        /// <param name="redemptionCount">Specifies how many times a coupon has been redeemed</param>
        /// <param name="maxRedemptionLimit">Specifies how many times a coupon can be redeemed</param>
        /// <returns>Returns integer based on the redemption status of a Coupon</returns>
        /// <remarks>When a Coupon is redeemed from Dotcom, this method is called to save redemption details of the coupon.
        /// Assigns 15 if the coupon is redeemed successfully and the details will be saved in CC database for that particular coupon</remarks>
        public Int16 SaveDotComRedeemInfo(CheckOutRequest checkOutRequest, Int16 redemptionStatus, Int64 couponInstanceId, Int16 redemptionCount, Int16 maxRedemptionLimit)
        {
            Int16 resultValue = 0;
            DbCommand command = null;
            Int64? bigClubcardNumber = null;
            try
            {

                string smartAlphaCode = checkOutRequest.SmartAlphaNumericCode;
                if (!String.IsNullOrWhiteSpace(checkOutRequest.ClubcardNumber))
                    bigClubcardNumber = Convert.ToInt64(checkOutRequest.ClubcardNumber);
                Int16 channelCode = 0;
                DateTime txnTime = checkOutRequest.TxnTimeStamp;
                bool isOffline = checkOutRequest.IsOfflineTransaction;
                bool isReversal = checkOutRequest.IsReversal;
                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_RedeemCoupon");

                clubcardCouponDb.AddInParameter(command, "@SmartAlphanumericCode", DbType.String, smartAlphaCode);
                clubcardCouponDb.AddInParameter(command, "@ClubcardNumber", DbType.Int64, bigClubcardNumber);
                clubcardCouponDb.AddInParameter(command, "@ChannelCode", DbType.Int16, channelCode);
                clubcardCouponDb.AddInParameter(command, "@TxnTimeStamp", DbType.DateTime, txnTime);
                clubcardCouponDb.AddInParameter(command, "@IsOfflineTransaction", DbType.Boolean, isOffline);
                clubcardCouponDb.AddInParameter(command, "@IsReversal", DbType.Boolean, isReversal);
                clubcardCouponDb.AddInParameter(command, "@RedemptionStatusId", DbType.Int16, redemptionStatus);
                clubcardCouponDb.AddInParameter(command, "@CouponInstanceId", DbType.Int64, couponInstanceId);
                clubcardCouponDb.AddInParameter(command, "@RedemptionCount", DbType.Int16, redemptionCount);
                clubcardCouponDb.AddInParameter(command, "@MaxRedemptionLimit", DbType.Int16, maxRedemptionLimit);
                clubcardCouponDb.AddParameter(command, "@RETURN_VALUE", DbType.Int16, 5, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null);

                clubcardCouponDb.ExecuteNonQuery(command);
                resultValue = Convert.ToInt16(command.Parameters["@RETURN_VALUE"].Value);
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:SaveDotComRedeemInfo(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                return -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }

            return resultValue;
        }

        /// <summary>
        /// This method is used to Save the details of the Coupon once the coupon is issued at Till
        /// </summary>
        /// <param name="tillRequest">Till Request will provide the details of the coupon to be issued at Till</param>
        /// <param name="lstCouponAtTill">List of Till Coupons that are issued for the input request</param>
        /// <remarks>This method is called by storeline to SaveCouponIssuance details after a coupon has been issued for a customer.</remarks>
        public void SaveCouponIssuance(CouponAtTillRequest tillRequest, List<CouponAtTill> lstCouponAtTill)
        {
            DbCommand command = null;

            foreach (var couponsTill in lstCouponAtTill)
            {
                try
                {
                    command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_SaveCouponAtTill", 1, couponsTill.CouponInstanceId, tillRequest.ClubcardNumber, tillRequest.StoreNumber, tillRequest.TillId, tillRequest.TillBankId, tillRequest.OperatorId);
                    clubcardCouponDb.ExecuteNonQuery(command);
                }
                catch (Exception ex)
                {
                    Logger.Write("DAL:SaveCouponIssuance(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }

        #region Enquiry Service Methods
        /// <summary>
        /// This Method is used to get all the available coupons of a customer which are active and ready to be redeemed
        /// </summary>
        /// <param name="houseHoldId">HouseHoldId of the Customer</param>
        /// <param name="ImageRequired">ImageRequired</param>
        /// <returns>Returns Available Coupon Response</returns>        
        public AvailableCouponResponse GetAvailableCouponResponse(long houseHoldId, bool ImageRequired)
        {
            AvailableCouponResponse availableCouponResp = null;
            CouponInformation couponInfo = null;

            List<CouponInformation> availableCouponInfos = null;
            Int32 totCouponCount = 0;

            try
            {
                availableCouponResp = new AvailableCouponResponse();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetAvailableCoupons"))
                {
                    clubcardCouponDb.AddInParameter(cmd, "@HouseholdID", DbType.Int64, houseHoldId);
                    clubcardCouponDb.AddInParameter(cmd, "@GetImage", DbType.Boolean, ImageRequired);

                    IDataReader dataReader = clubcardCouponDb.ExecuteReader(cmd);
                    while (dataReader.Read())
                    {
                        couponInfo = new CouponInformation();
                        couponInfo.IssuanceChannel = dataReader.GetString(0);
                        //Added as part of CR-09 Change.
                        if (DALUtilityClass.IsIsuanceChannelInList(couponInfo.IssuanceChannel))
                        {
                            totCouponCount++;
                        }
                    }
                    //Count of total coupons based on Configuration Entry.
                    availableCouponResp.TotalCoupon = totCouponCount;

                    availableCouponInfos = new List<CouponInformation>();
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            couponInfo = new CouponInformation();
                            couponInfo.TriggerNumber = dataReader.GetInt16(0);
                            couponInfo.StatementNumber = dataReader.GetString(1);
                            couponInfo.CouponImageThumbnail = GetImageBytes(dataReader, 2);
                            couponInfo.CouponImageFull = GetImageBytes(dataReader, 3);
                            couponInfo.ThumbnailImageName = dataReader.GetStringValue(10);
                            couponInfo.FullImageName = dataReader.GetStringValue(11);
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
                            couponInfo.RedemptionEndDate = dataReader.GetDateTime(4);
                            couponInfo.SmartAlphaNumeric = dataReader.GetStringValue(5);
                            couponInfo.SmartBarcode = dataReader.GetDecimalValue(6);
                            couponInfo.IssuanceStartDate = dataReader.GetDateTime(7);
                            couponInfo.CouponDescription = dataReader.GetString(8);

                            //Multiple redemption proposed 1.5 modifications
                            couponInfo.MaxRedemptionLimit = dataReader.GetInt16(12);
                            couponInfo.RedemptionUtilized = dataReader.GetInt16(13);
                            couponInfo.IssuanceChannel = dataReader.GetStringValue(14);
                            if (DALUtilityClass.IsIsuanceChannelInList(couponInfo.IssuanceChannel))
                            {
                                availableCouponInfos.Add(couponInfo);
                            }
                        }
                    }
                    dataReader.Close();


                    availableCouponResp.RequestedHouseHoldId = houseHoldId;
                    availableCouponResp.ActiveCoupon = Convert.ToInt32((availableCouponInfos.Count()));

                    availableCouponResp.ErrorStatusCode = "0";
                    availableCouponResp.ErrorMessage = ConfigurationManager.AppSettings["0"];

                    availableCouponResp.CouponList = availableCouponInfos;

                    return availableCouponResp;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetAvailableCouponResponse(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                availableCouponResp.RequestedHouseHoldId = houseHoldId;
                availableCouponResp.ErrorStatusCode = "102";
                availableCouponResp.ErrorMessage = ConfigurationManager.AppSettings["102"];
                return availableCouponResp;
            }
            finally
            {
                availableCouponResp = null;
                couponInfo = null;
                availableCouponInfos = null;
            }
        }

        /// <summary>
        /// This Method is used to get all the redeemed coupons of a input householdId
        /// </summary>
        /// <param name="houseHoldId">HouseHoldId of a customer for which redeemed coupons have to be returned</param>
        /// <param name="redemptionLength">Number of days for which the redeemed coupon information is required</param>
        /// <returns>Redeemed Coupon Response</returns>
        public RedeemedCouponResponse GetRedeemedCouponDetails(long houseHoldId, int? redemptionLength)
        {
            RedeemedCouponResponse redeemResp = null;
            List<CouponInformation> couponInfos = null;
            CouponInformation couponInfo = null;
            Int32 totRedeemedCouponCount = 0;

            try
            {
                redeemResp = new RedeemedCouponResponse();
                couponInfos = new List<CouponInformation>();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetRedeemedCoupons"))
                {
                    clubcardCouponDb.AddInParameter(cmd, "@HouseholdID", DbType.Int64, houseHoldId);
                    clubcardCouponDb.AddInParameter(cmd, "@RedemptionLength", DbType.Int32, redemptionLength ?? 30);

                    IDataReader dataReader = clubcardCouponDb.ExecuteReader(cmd);

                    //Added as part of CR-09 Change.
                    while (dataReader.Read())
                    {
                        couponInfo = new CouponInformation();
                        couponInfo.IssuanceChannel = dataReader.GetString(1);
                        if (DALUtilityClass.IsIsuanceChannelInList(couponInfo.IssuanceChannel))
                        {
                            totRedeemedCouponCount++;
                        }
                    }
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            couponInfo = new CouponInformation();

                            redeemResp.RequestedHouseHoldId = houseHoldId;
                            couponInfo.CouponDescription = dataReader.GetString(0);
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
                            couponInfo.IssuanceStartDate = dataReader.GetDateTime(1);

                            //Multiple redemption proposed 1.5 modifications
                            //couponInfo.RedemptionDate = dataReader.GetDateTime(2);

                            //Multiple redemption proposed 1.5 modifications
                            //couponInfo.RedemptionStore = dataReader.GetInteger16(3);

                            /*
                            * Multiple redemption proposed 1.5 modifications required to send 
                            * 1. MaxRedemptionLimit
                            * 2. RedemptionUtilized
                            * 3. SmartBarcode
                            * 4. SmartAlphaNumeric
                            * 5. and all redemption information
                            */

                            couponInfo.MaxRedemptionLimit = dataReader.GetInt16(2);
                            couponInfo.RedemptionUtilized = dataReader.GetInt16(3);
                            couponInfo.CouponInstanceId = dataReader.GetInt64(4);
                            couponInfo.SmartBarcode = dataReader.GetDecimalValue(5);
                            couponInfo.SmartAlphaNumeric = dataReader.GetStringValue(6);
                            couponInfo.IssuanceChannel = dataReader.GetString(7);

                            //Added as part of CR-09 Change.
                            if (DALUtilityClass.IsIsuanceChannelInList(couponInfo.IssuanceChannel))
                            {
                                couponInfos.Add(couponInfo);
                            }
                        }
                    }

                    //Multiple redemption proposed 1.5 modifications required to send all redemption information
                    if (dataReader.NextResult())
                    {
                        couponInfos.ForEach(x => x.ListRedemptionInfo = new List<RedemptionInfo>());
                        var redList = this.GetAllRedemptionInfo(dataReader);
                        foreach (var coupon in couponInfos)
                        {
                            coupon.ListRedemptionInfo = redList.Where(r => (r.CouponInstanceId == coupon.CouponInstanceId)).ToList<RedemptionInfo>();
                        }
                    }
                    dataReader.Close();
                    redeemResp.TotalRedeemCoupon = totRedeemedCouponCount;
                    redeemResp.ErrorStatusCode = "0";
                    redeemResp.ErrorMessage = ConfigurationManager.AppSettings["0"];

                    redeemResp.CouponList = couponInfos;
                    redeemResp.RequestedHouseHoldId = houseHoldId;

                    return redeemResp;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetRedeemedCouponDetails(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                redeemResp.RequestedHouseHoldId = houseHoldId;
                redeemResp.ErrorStatusCode = "102";
                redeemResp.ErrorMessage = ConfigurationManager.AppSettings["102"];
                return redeemResp;
            }
            finally
            {
                redeemResp = null;
                couponInfos = null;
                couponInfo = null;
            }
        }

        /// <summary>
        /// This Method is used to get Coupon Information for redeemed coupons and issued coupons
        /// </summary>
        /// <param name="smartBarcode">The smart barcode for which the Clubcard Coupon information is required</param>
        /// <param name="smartAlphaCode">The alphanumeric code for which the Clubcard Coupon information is required. </param>
        /// <param name="householdId">The household for which the coupon history is required.Only required if the alphanumeric code has been provided.</param>
        /// <param name="ImageRequired">ImageRequired</param>
        /// <returns>Coupon Information Response</returns>
        /// <remarks>This method will be used by CSC team in a case if customer wanted to enquire about coupon issuance or redemption</remarks>
        public CouponInformationResponse GetCouponInfoDetails(string smartBarcode, string smartAlphaCode, bool ImageRequired)
        {
            CouponInformationResponse infoResp = null;
            CouponInformation info = null;

            object objSmartBarCode = (string.IsNullOrWhiteSpace(smartBarcode) ? (object)DBNull.Value : smartBarcode);
            object objsmartAlphaCode = (string.IsNullOrWhiteSpace(smartAlphaCode) ? (object)DBNull.Value : smartAlphaCode);

            try
            {
                ReadAdHocConfigParams();

                infoResp = new CouponInformationResponse();

                using (IDataReader dataReader = clubcardCouponDb.ExecuteReader("dbo.USP_GetCouponDetails", objSmartBarCode, objsmartAlphaCode, ImageRequired, _intAdHocRedemptionStartPeriod, _intAdHocRedemptionEndPeriod))
                {
                    bool dataReaderHasRows = false;

                    while (dataReader.Read())
                    {
                        info = new CouponInformation();

                        dataReaderHasRows = true;
                        info.TriggerNumber = dataReader.GetInt16(0);
                        info.StatementNumber = dataReader.GetString(1);
                        info.SmartBarcode = dataReader.GetDecimalValue(2);
                        info.SmartAlphaNumeric = dataReader.GetStringValue(3);
                        info.RedemptionEndDate = dataReader.GetDateTime(4);
                        info.IssuanceChannel = dataReader.GetString(5);
                        info.CouponImageThumbnail = GetImageBytes(dataReader, 6);
                        info.CouponImageFull = GetImageBytes(dataReader, 7);
                        info.CouponStatusId = dataReader.GetInt16(8);
                        info.CouponIssuanceChannel = dataReader.GetInteger16(9); //checking for null values
                        info.IssuanceDate = dataReader.GetDateTimeValue(10);
                        info.IssuanceStore = dataReader.GetInteger16(11); //checking for null values

                        //Multiple redemption proposal 1.5 modifications
                        //info.RedemptionChannelCode = dataReader.GetInteger16(12);
                        //info.RedemptionDate = dataReader.GetDateTimeValue(13); 
                        //info.RedemptionStore = dataReader.GetInteger16(14);


                        /*
                        * Multiple redemption proposed 1.5 modifications required to send 
                        * 1. MaxRedemptionLimit
                        * 2. RedemptionUtilized
                        * 3. SmartBarcode
                        * 4. SmartAlphaNumeric
                        * 5. and all redemption information
                        */

                        info.MaxRedemptionLimit = dataReader.GetInt16(12);
                        info.RedemptionUtilized = dataReader.GetInt16(13);
                        info.RedemptionChannel = dataReader.GetString(14);

                        info.ThumbnailImageName = dataReader.GetStringValue(15);
                        info.FullImageName = dataReader.GetStringValue(16);

                        info.CouponDescription = dataReader.GetStringValue(17);
                        info.HouseholdId = dataReader.GetInteger64(18).ToString();

                        infoResp.CouponDetails = info;

                        infoResp.ErrorStatusCode = "0";
                        infoResp.ErrorMessage = ConfigurationManager.AppSettings["0"].ToString();
                    }

                    //Multiple redemption proposed 1.5 modifications required to send all redemption information
                    if (dataReader.NextResult())
                    {
                        if (infoResp != null && infoResp.CouponDetails != null)
                        {
                            infoResp.CouponDetails.ListRedemptionInfo = new List<RedemptionInfo>();
                            this.GetAllRedemptionInfo(dataReader).ForEach((x) => infoResp.CouponDetails.ListRedemptionInfo.Add(x));
                        }
                    }

                    if (!dataReaderHasRows)
                    {
                        info = new CouponInformation();
                        info.SmartAlphaNumeric = smartAlphaCode;
                        info.SmartBarcode = objSmartBarCode.ToString();
                        infoResp.CouponDetails = info;
                        infoResp.ErrorStatusCode = "104";
                        infoResp.ErrorMessage = ConfigurationManager.AppSettings["104"].ToString();
                    }

                    return infoResp;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("DAL:GetCouponInfoDetails(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                info = new CouponInformation();
                info.SmartAlphaNumeric = smartAlphaCode;
                info.SmartBarcode = objSmartBarCode.ToString();
                infoResp.CouponDetails = info;
                infoResp.ErrorStatusCode = "102";
                infoResp.ErrorMessage = ConfigurationManager.AppSettings["102"];
                return infoResp;
            }
            finally
            {
                infoResp = null;
                info = null;
            }
        }

        /// <summary>
        /// This method is used to Check if the given coupon is valid to use online, Mca customer can see if coupon is redeemed
        /// </summary>
        /// <param name="smartAlphaCode">Smart Alphanumeric Code</param>
        /// <param name="houseHoldId">Household Id</param>
        /// <returns>CheckOutResponse object which RedemptionStatus value</returns>
        public CheckOutResponse ValidateCheckOut(string smartAlphaCode)
        {
            CheckOutResponse resp = null;

            try
            {
                resp = new CheckOutResponse();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetCouponStatus", smartAlphaCode))
                {
                    clubcardCouponDb.ExecuteScalar(cmd);

                    resp.RedemptionStatus = Convert.ToInt16((clubcardCouponDb.GetParameterValue(cmd, "@return_value")));

                    if (resp.RedemptionStatus != -1)
                    {
                        resp.ErrorStatusCode = "0";
                        resp.ErrorMessage = ConfigurationManager.AppSettings["0"];
                    }
                    else
                    {
                        resp.ErrorStatusCode = "104";
                        resp.ErrorMessage = ConfigurationManager.AppSettings["104"];
                    }

                    resp.SmartAlphaNumericCode = smartAlphaCode;

                    return resp;
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Write(sqlEx.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                resp.RedemptionStatus = -1;
                resp.SmartAlphaNumericCode = smartAlphaCode;
                resp.ErrorStatusCode = "102";
                resp.ErrorMessage = ConfigurationManager.AppSettings["102"];
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                resp.RedemptionStatus = -1;
                resp.SmartAlphaNumericCode = smartAlphaCode;
                resp.ErrorStatusCode = "102";
                resp.ErrorMessage = ConfigurationManager.AppSettings["102"];
                return resp;
            }
            finally
            {
                resp = null;
            }
        }

        /// <summary>
        /// This method is used to Get the Image Bytes from database varbinary column
        /// </summary>
        /// <param name="dataReader">IDataReader object</param>
        /// <param name="index">Index of the column</param>
        /// <returns>byte array</returns>
        private static byte[] GetImageBytes(IDataReader dataReader, int index)
        {
            byte[] outByte = null;
            long byteSize;

            try
            {
                //get the total count of bytes
                if (dataReader.IsDBNull(index))
                {
                    outByte = null;
                }
                else
                {
                    byteSize = dataReader.GetBytes(index, 0, outByte, 0, 0);

                    outByte = new byte[byteSize];

                    // Read whole bytes into outByte[]
                    dataReader.GetBytes(index, 0, outByte, 0, (int)byteSize);
                }

                return outByte;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// This Method is used to get Redemption information of both redeemed and active coupons
        /// </summary>
        /// <param name="reader">The data reader object.</param>
        /// <returns>List of Redemption Info objects</returns>
        /// <remarks>GetAllRedemptionInfo() function will return (multiple)redemption information for all coupon instances</remarks>
        private List<RedemptionInfo> GetAllRedemptionInfo(IDataReader reader)
        {
            List<RedemptionInfo> redInfo = null;

            try
            {
                redInfo = new List<RedemptionInfo>();

                while (reader.Read())
                {
                    redInfo.Add(new RedemptionInfo()
                    {
                        ClubcardNumber = reader.GetInteger64(0).ToString(),
                        ChannelCode = reader.GetInteger16(1),
                        StoreNumber = reader.GetInteger16(2),
                        TillId = reader.GetInteger16(3),
                        TillType = reader.GetInteger16(4),
                        RedemptionDateTime = reader.GetDateTime(5),
                        CashierNumber = reader.GetStringValue(6),
                        IsOffline = reader.GetBoolean(7),
                        RedemptionType = reader.GetBoolean(8) ? RedeemType.UnRedeem : RedeemType.Redeem,
                        //RedemptionType = reader.IsDBNull(23) ? (RedeemType?)null : (reader.GetBooleanValue(23).Value ? RedeemType.UnRedeem : RedeemType.Redeem),

                        CouponInstanceId = reader.GetInt64(9)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                throw ex;
            }

            return redInfo;
        }
        #endregion

        /// <summary>
        /// GetAdHocCouponClass will search for valid CouponClassId 
        /// and return that row back
        /// </summary>
        /// <param name="couponRequest"></param>
        /// <returns></returns>
        public AdHocCouponResponse GetAdHocCouponClass(AdHocCouponRequest couponRequest)
        {

            AdHocCouponResponse adHocCouponResponse = null;
            Int16 returnValue = 0;

            try
            {
                adHocCouponResponse = new AdHocCouponResponse();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetAdHocCouponClass"))
                {
                    clubcardCouponDb.AddInParameter(cmd, "@TriggerNo", DbType.Int16, couponRequest.TriggerNumber);
                    clubcardCouponDb.AddInParameter(cmd, "@MailingNo", DbType.String, couponRequest.MailingNumber);
                    clubcardCouponDb.AddParameter(cmd, "@RETURN_VALUE", DbType.Int16, 5, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null);

                    IDataReader dataReader = clubcardCouponDb.ExecuteReader(cmd);

                    if (dataReader.Read())
                    {
                        adHocCouponResponse.CouponClassId = dataReader.GetInt64(0);
                        adHocCouponResponse.CouponExpiryDate = dataReader.GetDateTime(1);
                        adHocCouponResponse.MaxRedemptionLimit = dataReader.GetInt16(2);
                        adHocCouponResponse.EANBarcode = DALUtilityClass.GetDecimalValue(dataReader, 3);
                        adHocCouponResponse.AlphaCode = DALUtilityClass.GetStringValue(dataReader, 4);
                        adHocCouponResponse.RedemptionChannel = DALUtilityClass.GetStringValue(dataReader, 5);
                        adHocCouponResponse.CouponErrorCode = 0;
                        adHocCouponResponse.CouponErrorMessage = "Success";
                    }
                    dataReader.Close();
                    returnValue = Convert.ToInt16(cmd.Parameters["@RETURN_VALUE"].Value);
                    if (returnValue != 0)
                    {
                        adHocCouponResponse.CouponErrorCode = returnValue;
                        adHocCouponResponse.CouponErrorMessage = GetErrorMessage(returnValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                adHocCouponResponse.CouponErrorCode = -1;
                adHocCouponResponse.CouponErrorMessage = "Internal Error Occured";
                Logger.Write("DAL:GetAdHocCouponClass(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            return adHocCouponResponse;
        }

        /// <summary>
        /// Will fetch error message from web.config
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private string GetErrorMessage(string errorCode)
        {
            string errorMessage;
            try
            {
                errorMessage = ConfigurationManager.AppSettings[errorCode];
            }
            catch
            {
                errorMessage = "Error in adHocService validation";
            }
            return errorMessage;
        }

        /// <summary>
        /// GetAdHocSmartCodes will fetch smart numbers from db
        /// </summary>
        /// <param name="adHocResponse"></param>
        /// <param name="RandomBarCode"></param>
        /// <param name="AlphaNumber"></param>
        public void GetAdHocSmartCodes(AdHocCouponResponse adHocResponse, out Int32? RandomBarCode, out string AlphaNumber)
        {
            RandomBarCode = 0;
            AlphaNumber = string.Empty;
            int? nextAlphacodeRowNumber = null;
            int? nextSmartBarcodeRowNumber = null;
            IDataReader dataReader = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(adHocResponse.EANBarcode) && adHocResponse.RedemptionChannel.ToUpper().Contains("STORE"))
                    nextSmartBarcodeRowNumber = BarcodeRowNumberGenerator.Instance.GetNextRowNumber();

                if (!string.IsNullOrWhiteSpace(adHocResponse.AlphaCode) && adHocResponse.RedemptionChannel.ToUpper().Contains("GHS"))
                    nextAlphacodeRowNumber = AlphaCodeRowNumberGenerator.Instance.GetNextRowNumber();

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetAdHocRandomValue"))
                {
                    cmd.CommandTimeout = 5;
                    clubcardCouponDb.AddInParameter(cmd, "@BarcodeRowNumber", DbType.Int32, nextSmartBarcodeRowNumber);
                    clubcardCouponDb.AddInParameter(cmd, "@AlphaCodeRowNumber", DbType.Int32, nextAlphacodeRowNumber);

                    using (dataReader = clubcardCouponDb.ExecuteReader(cmd))
                    {
                        if (dataReader.Read())
                        {
                            RandomBarCode = DALUtilityClass.GetInteger32(dataReader, 0);
                            AlphaNumber = DALUtilityClass.GetStringValue(dataReader, 1);
                            adHocResponse.CouponErrorCode = 0;
                            adHocResponse.CouponErrorMessage = "Success";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                adHocResponse.CouponErrorCode = -1;
                adHocResponse.CouponErrorMessage = "Internal Error Occured";
                Logger.Write("DAL:GetAdHocSmartCodes(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }



            //RandomBarCode = 0;
            //AlphaNumber = string.Empty;
            //bool requireSmartAlphacode = false;
            //bool requireSmartBarcode = false;
            //IDataReader dataReader = null;


            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(adHocResponse.EANBarcode) && adHocResponse.RedemptionChannel.ToUpper().Contains("STORE"))
            //        requireSmartBarcode = true;

            //    if (!string.IsNullOrWhiteSpace(adHocResponse.AlphaCode) && adHocResponse.RedemptionChannel.ToUpper().Contains("GHS"))
            //        requireSmartAlphacode = true;

            //    using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetAdHocRandomValue"))
            //    {
            //        cmd.CommandTimeout = 5;
            //        clubcardCouponDb.AddInParameter(cmd, "@GetBarcode", DbType.Boolean, requireSmartBarcode);
            //        clubcardCouponDb.AddInParameter(cmd, "@GetAlphaCode", DbType.Boolean, requireSmartAlphacode);

            //        using (dataReader = clubcardCouponDb.ExecuteReader(cmd))
            //        {
            //            if (dataReader.Read())
            //            {
            //                RandomBarCode = DALUtilityClass.GetInteger32(dataReader, 0);
            //                AlphaNumber = DALUtilityClass.GetStringValue(dataReader, 1);
            //                adHocResponse.CouponErrorCode = 0;
            //                adHocResponse.CouponErrorMessage = "Success";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    adHocResponse.CouponErrorCode = -1;
            //    adHocResponse.CouponErrorMessage = "Internal Error Occured";
            //    Logger.Write("DAL:GetAdHocSmartCodes(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            //}
        }

        /// <summary>
        /// This method will save adHoc coupon issuace details 
        /// in couponinstance and issuance table and will give
        /// back CouponInstanceId
        /// </summary>
        /// <param name="couponRequest"></param>
        /// <param name="adHocResponse"></param>
        /// <returns></returns>
        public AdHocCouponResponse SaveAdHocCouponInfo(AdHocCouponRequest couponRequest, AdHocCouponResponse adHocResponse)
        {
            DbCommand command = null;
            Int64? bigClubcardNumber = null;
            decimal? dSmartBarCode = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(adHocResponse.SmartBarcodeNumber))
                    dSmartBarCode = Convert.ToDecimal(adHocResponse.SmartBarcodeNumber);

                if (!String.IsNullOrWhiteSpace(couponRequest.ClubcardNumber))
                    bigClubcardNumber = Convert.ToInt64(couponRequest.ClubcardNumber);

                command = clubcardCouponDb.GetStoredProcCommand("dbo.USP_SaveAdHocCoupon");

                clubcardCouponDb.AddInParameter(command, "@CouponClassId", DbType.Int64, adHocResponse.CouponClassId);
                clubcardCouponDb.AddInParameter(command, "@SmartBarcode", DbType.Decimal, dSmartBarCode);
                clubcardCouponDb.AddInParameter(command, "@SmartAlphanumericCode", DbType.String, adHocResponse.SmartAlphaNumericCode);
                clubcardCouponDb.AddInParameter(command, "@ClubcardAccountID", DbType.Int64, couponRequest.CustomerAcctId);
                clubcardCouponDb.AddInParameter(command, "@IssuanceChannelId", DbType.Int16, 2);
                clubcardCouponDb.AddInParameter(command, "@ClubcardNumber", DbType.Int64, bigClubcardNumber);
                clubcardCouponDb.AddOutParameter(command, "@CouponInstanceId", DbType.Int64, 8);

                clubcardCouponDb.ExecuteNonQuery(command);
                adHocResponse.CouponInstanceId = Convert.ToInt64(clubcardCouponDb.GetParameterValue(command, "@CouponInstanceId"));

            }
            catch (Exception ex)
            {
                adHocResponse.CouponErrorCode = -1;
                adHocResponse.CouponErrorMessage = "Internal Error Occured";
                Logger.Write("DAL:SaveStoreRedeemInfo(): " + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
            return adHocResponse;
        }


        #region V@T
        /// <summary>
        /// This Method to GetActiveVATVoucher at Till
        /// </summary>
        /// <param name="primClubcardNumber">primClubcardNumber</param>
        /// <param name="customerId">customerId</param>
        /// <param name="goForCAT">goForCAT</param>
        /// <returns>List of CouponAtTill></returns>
        public List<CouponAtTill> GetActiveVATVoucher(long primClubcardNumber, long customerId, out bool goForCAT)
        {

            Int16 vaTStatus = 0;
            Int16 voucherExists = -1;
            List<CouponAtTill> listCouponAtTill = null;
            CouponAtTill objCouponAtTill = null;
            LineInfo objLineInfo = null;
            DataTable vaTDT = null;
            int noOfTillVouchers = 1;
            goForCAT = false;
            try
            {
                listCouponAtTill = new List<CouponAtTill>();
                noOfTillVouchers = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfTillVouchers"]);
                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetActiveVaTVouchers"))
                {
                    clubcardCouponDb.AddInParameter(cmd, "@pClubcard_Number", DbType.Int64, primClubcardNumber);
                    clubcardCouponDb.AddOutParameter(cmd, "@VaTStatus", DbType.Int16, 2);
                    DataSet ds = clubcardCouponDb.ExecuteDataSet(cmd);
                    vaTDT = ds.Tables.Count != 0 ? ds.Tables[0] : null;
                    vaTStatus = Convert.ToInt16((clubcardCouponDb.GetParameterValue(cmd, "@VaTStatus")));
                    if (vaTStatus == 20)// This means clubcard does not exists in SV system
                        vaTStatus = 0;
                    if ((vaTStatus == 1) || (vaTStatus == 2)) //Modify this condition as part of enhancement of newly added vatStatus 2
                    {
                        if (vaTDT != null && vaTDT.Rows.Count > 0)
                        {
                            DataTable tvtDT = new DataTable();
                            tvtDT.Columns.Add("PrimaryCardNo", typeof(long));
                            tvtDT.Columns.Add("BarCode", typeof(decimal));
                            tvtDT.Columns.Add("ExpiryDate", typeof(DateTime));
                            tvtDT.Columns.Add("Value", typeof(decimal));
                            tvtDT.Columns.Add("AlphaCode", typeof(string));
                            for (int i = 0; i < vaTDT.Rows.Count; i++)
                            {
                                tvtDT.Rows.Add(primClubcardNumber, vaTDT.Rows[i]["22DigitVoucher_Number"], vaTDT.Rows[i]["Expiry Date"], vaTDT.Rows[i]["Value"], vaTDT.Rows[i]["Online Code"]);
                            }
                            //conversion to (SqlDatabase) is required to pass data table as input parameter
                            SqlDatabase database = (SqlDatabase)DatabaseFactory.CreateDatabase("OLTPDbServer");
                            using (DbCommand dbCommand = database.GetStoredProcCommand("dbo.USP_GetAvailableVoucher"))
                            {
                                database.AddInParameter(dbCommand, "@VoucherTable", SqlDbType.Structured, tvtDT);
                                database.AddInParameter(dbCommand, "@NoOfTillVouchers", DbType.Int16, noOfTillVouchers);
                                database.AddOutParameter(dbCommand, "@VoucherExists", DbType.Int16, 2);
                                IDataReader reader = database.ExecuteReader(dbCommand);
                                var anonType = new { SmartBarCode = "", ExpiryDate = "", Value = "", SmartAlphaNumericCode = "" };
                                var listAnonType = MakeList(anonType);
                                int rowCount = 0;
                                while (reader.Read() && (rowCount <= noOfTillVouchers))
                                {
                                    objCouponAtTill = null;
                                    objCouponAtTill = new CouponAtTill();
                                    listAnonType.Add(new { SmartBarCode = reader.GetDecimal(1).ToString(), ExpiryDate = reader.GetDateTime(2).ToString(), Value = reader.GetDecimal(3).ToString(), SmartAlphaNumericCode = reader.GetString(4).ToString() });
                                    objCouponAtTill.SmartBarCode = reader.GetDecimal(1);
                                    objCouponAtTill.SmartAlphaNumericCode = reader.GetString(4);
                                    objCouponAtTill.CouponTemplateId = reader.GetString(5);
                                    listCouponAtTill.Add(objCouponAtTill);
                                    rowCount++;
                                }
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    //objLineInfo = new LineInfo();
                                    //PopulateLine(objLineInfo, reader);
                                    foreach (var item in listCouponAtTill)
                                    {
                                        objLineInfo = null;
                                        objLineInfo = new LineInfo();
                                        PopulateLine(objLineInfo, reader);
                                        item.ListLineInfo.Add(objLineInfo);
                                    }
                                }
                                for (int i = 0; i < listCouponAtTill.Count; i++)
                                {
                                    //Read current culture from config file and convert the voucher value to currency 
                                    //Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
                                    string currentCulture = ConfigurationManager.AppSettings["DefaultCulture"];

                                    //To fix the defect MKTG00008469
                                    objCouponAtTill = null;
                                    objCouponAtTill = listCouponAtTill[i];

                                    objCouponAtTill.ListLineInfo[0].LineText = string.Format(new CultureInfo(currentCulture), "{0:C}", Convert.ToDecimal(listAnonType[i].Value));

                                    //To fix the defect MKTG00007201
                                    //To fix the defect MKTG00008616 
                                    string cultureSpecificDateString = string.Format(new CultureInfo(currentCulture), "{0:d}", DateTime.ParseExact(listAnonType[i].ExpiryDate.ToString(), "M/d/yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture).ToString());


                                    //To fix the defect MKTG00007357
                                    Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
                                    objCouponAtTill.ListLineInfo[6].LineText = Convert.ToDateTime(cultureSpecificDateString).ToShortDateString();
                                    objCouponAtTill.ListLineInfo[7].LineText = listAnonType[i].SmartBarCode;
                                    objCouponAtTill.ListLineInfo[8].LineText = listAnonType[i].SmartAlphaNumericCode;
                                }
                                reader.Close();
                                voucherExists = Convert.ToInt16(database.GetParameterValue(dbCommand, "@VoucherExists"));
                                if (voucherExists == 1)
                                {
                                    return listCouponAtTill;
                                }
                                else if (voucherExists == 0)
                                {
                                    if (vaTStatus == 1) //Added this condition as part of enhancement of newly added vatStatus 2
                                    {
                                        clubcardCouponDb = DatabaseFactory.CreateDatabase("OLTPDbServer");
                                        goForCAT = true;
                                        return null;
                                    }
                                    if (vaTStatus == 2) //Added this condition as part of enhancement of newly added vatStatus 2
                                    {
                                        return this.NotifyAvailableVoucher(primClubcardNumber, customerId, out goForCAT);
                                    }
                                }
                                else
                                {
                                    Logger.Write(string.Format("DAL:GetActiveVATVoucher():The request with ClubcardNumber = {0} and CustomerId = {1} has returned VoucherExists = {2} as not expected", primClubcardNumber, customerId, voucherExists), "Information");
                                }
                            }
                        }
                        else
                        {
                            Logger.Write(string.Format("DAL:GetActiveVATVoucher():The request with ClubcardNumber = {0} and CustomerId = {1} has returned zero or null rows", primClubcardNumber, customerId), "Information");
                            listCouponAtTill = new List<CouponAtTill>();
                            listCouponAtTill.Add(new CouponAtTill("", "104", ConfigurationManager.AppSettings["104"]));
                        }
                    }
                    else if (vaTStatus == 0)
                    {
                        clubcardCouponDb = DatabaseFactory.CreateDatabase("OLTPDbServer");
                        goForCAT = true;
                        return listCouponAtTill;
                    }
                    else if (vaTStatus == -1)
                    {
                        return this.NotifyAvailableVoucher(primClubcardNumber, customerId, out goForCAT);
                    }
                    else
                    {
                        Logger.Write(string.Format("DAL:GetActiveVATVoucher():The request with ClubcardNumber = {0}  and CustomerId = {1} has returned vaTStatus = {2} as not expected", primClubcardNumber, customerId, vaTStatus), "Information");
                    }
                }
                return listCouponAtTill;
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("DAL:GetActiveVATVoucher():The request with ClubcardNumber = {0} and CustomerId = {1} Error Message: {2}", primClubcardNumber, customerId, ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                listCouponAtTill = new List<CouponAtTill>();
                listCouponAtTill.Add(new CouponAtTill("", "102", ConfigurationManager.AppSettings["102"]));
                return listCouponAtTill;
            }
            finally
            {
                listCouponAtTill = null;
                objLineInfo = null;
                vaTDT = null;
            }
        }

        /// <summary>
        /// This method is used to Prepare a list
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="itemOftype">itemOftype</param>
        /// <returns>Retursn list</returns>
        public static List<T> MakeList<T>(T itemOftype)
        {
            List<T> newList = new List<T>();
            return newList;
        }

        /// <summary>
        /// This method is used to Notify and save Available Vouchers for the requested primary ClubcardNumber
        /// </summary>
        /// <param name="primClubcardNumber">Primaary ClubcardNumber</param>
        /// <param name="customerId">Customer Id</param>
        /// <returns>List of CouponAtTill if voucher needs to be notified else null</returns>
        private List<CouponAtTill> NotifyAvailableVoucher(long primClubcardNumber, long customerId, out bool goForCAT)
        {
            List<CouponAtTill> listCouponAtTill = null;
            CouponAtTill objCouponAtTill = null;
            LineInfo objLineInfo = null;
            string couponTemplateId = string.Empty;
            short notifiStatus = -1;

            goForCAT = false;

            try
            {
                listCouponAtTill = new List<CouponAtTill>();
                objCouponAtTill = new CouponAtTill();

                clubcardCouponDb = DatabaseFactory.CreateDatabase("OLTPDbServer"); //ConfigurationManager.ConnectionStrings["OLTPDbServer"].ConnectionString

                using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetAvailableNotification"))
                {
                    clubcardCouponDb.AddInParameter(cmd, "@PrimaryCardId", DbType.Int64, primClubcardNumber);
                    clubcardCouponDb.AddOutParameter(cmd, "@NotificStatus", DbType.Int16, 2);
                    clubcardCouponDb.AddOutParameter(cmd, "@TemplateNo", DbType.String, 2);

                    IDataReader dataReader = clubcardCouponDb.ExecuteReader(cmd);

                    while (dataReader.Read())
                    {
                        objLineInfo = new LineInfo();
                        PopulateLine(objLineInfo, dataReader);
                        objCouponAtTill.ListLineInfo.Add(objLineInfo);
                    }

                    dataReader.Close();

                    notifiStatus = Convert.ToInt16((clubcardCouponDb.GetParameterValue(cmd, "@NotificStatus")));
                    objCouponAtTill.CouponTemplateId = couponTemplateId = clubcardCouponDb.GetParameterValue(cmd, "@TemplateNo").ToString();

                    listCouponAtTill.Add(objCouponAtTill);

                    if (notifiStatus == 0)
                    {
                        goForCAT = true;
                        return listCouponAtTill;
                    }
                    else if (notifiStatus == 1)
                    {
                        return listCouponAtTill;
                    }
                    else
                    {
                        Logger.Write(string.Format("DAL:NotifyAvailableVoucher():The request with ClubcardNumber = {0} and CustomerId = {1} has returned notifiStatus = {2} as not expected", primClubcardNumber, customerId, notifiStatus), "Information");
                    }
                }
                return listCouponAtTill;
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("DAL:NotifyAvailableVoucher():The request with ClubcardNumber = {0} and CustomerId = {1} returned Error Message: {2}", primClubcardNumber, customerId, ex.Message), "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                listCouponAtTill = new List<CouponAtTill>();
                listCouponAtTill.Add(new CouponAtTill("", "102", ConfigurationManager.AppSettings["102"]));
                return listCouponAtTill;
            }
            finally
            {
                listCouponAtTill = null;
                objCouponAtTill = null;
                objLineInfo = null;
            }
        }

        #endregion
    }

    #region DataReaderNullableMethods
    /// <summary>
    /// Data Access Layer Utility Class for Checking Nulls
    /// </summary>
    public static class DALUtilityClass
    {

        /// <summary>
        /// This method Gets the integer16.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>Any Positive integer</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static short? GetInteger16(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return null;
            else
                return tmpDr.GetInt16(fieldNumber);
        }

        /// <summary>
        /// This method Gets the integer32.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>Any Positive integer</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static int? GetInteger32(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return null;
            else
                return tmpDr.GetInt32(fieldNumber);
        }

        /// <summary>
        /// This method Gets the integer64.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>Any Positive integer</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static long? GetInteger64(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return null;
            else
                return tmpDr.GetInt64(fieldNumber);
        }

        /// <summary>
        /// This method returns the Value as Null if it is DBNull.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>True, False or Null</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static bool? GetBooleanValue(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return null;
            else
                return tmpDr.GetBoolean(fieldNumber);
        }

        /// <summary>
        /// This method Gets the string value.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>String</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static string GetStringValue(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return string.Empty;
            else
                return tmpDr.GetString(fieldNumber);
        }

        /// <summary>
        /// This method Gets the decimal value.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>String</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static string GetDecimalValue(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return string.Empty;
            else
                return tmpDr.GetDecimal(fieldNumber).ToString();
        }

        /// <summary>
        /// This method Gets the date time value.
        /// </summary>
        /// <param name="tmpDr">The TMP dr.</param>
        /// <param name="fieldNumber">The field number.</param>
        /// <returns>DateTime</returns>
        /// <remarks>Checking for DBNull of a given field number</remarks>
        public static DateTime? GetDateTimeValue(this IDataReader tmpDr, int fieldNumber)
        {
            if (tmpDr.IsDBNull(fieldNumber))
                return null;
            else
                return tmpDr.GetDateTime(fieldNumber);
        }

        /// <summary>
        /// Returns true if the IssuanceChannel is equal to Configured Issuance Channel in web.config
        /// </summary>
        /// <param name="issuancechannel"></param>
        /// <returns></returns>
        public static bool IsIsuanceChannelInList(string issuancechannel)
        {
            string[] configuredIssuanceChannel = null;
            string issuanceChannel = string.Empty;
            issuanceChannel = ConfigurationManager.AppSettings["DisplayIssuanceChannel"].ToString();
            if (issuanceChannel == string.Empty)
            {
                throw new Exception("Issuance channels are not configured in web.config");
            }
            configuredIssuanceChannel = issuanceChannel.Split(',');
            for (int i = 0; i <= configuredIssuanceChannel.Length - 1; i++)
            {
                if (string.Compare(issuancechannel, configuredIssuanceChannel[i].ToString(), true) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
}
