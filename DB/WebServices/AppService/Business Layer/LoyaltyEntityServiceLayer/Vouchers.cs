/*
 * File   : Vocuher.cs
 * Author : Kavitha S (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Vouchers
 * Date   : 10/Sep/2008
 * 
 */

#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    #region Header
    ///
    /// <summary>
    /// Set Up Vouchers
    /// </summary>
    /// <development>
    ///		<version number="1.00" date="13/Aug/2008">
    ///			<developer>Kavitha</developer>
    ///			<Reviewer></Reviewer>
    ///			<description>Initial Implementation</description>
    ///		</version>
    ///	<development>
    ///	
    #endregion

    public class Vouchers
    {
        #region Fields
        private decimal minVoucherValue;
        private decimal maxVoucherValue;
        private decimal voucherStepSize;
        private int noOfVouchers;
        private int offerID;
        private int voucherType;
        private string voucherBarcode;
        private int voucherDefinitionStatus;
        private DateTime voucherExpiryDate;
        #endregion

        #region Properties
        public decimal MinVoucherValue { get { return this.minVoucherValue; } set { this.minVoucherValue = value; } }
        public decimal MaxVoucherValue { get { return this.maxVoucherValue; } set { this.maxVoucherValue = value; } }
        public decimal VoucherStepSize { get { return this.voucherStepSize; } set { this.voucherStepSize = value; } }
        public int NumberOfVouchers { get { return this.noOfVouchers; } set { this.noOfVouchers = value; } }
        public int OfferID { get { return this.offerID; } set { this.offerID = value; } }
        public int VoucherType { get { return this.voucherType; } set { this.voucherType = value; } }
        public string VoucherBarcode { get { return this.voucherBarcode; } set { this.voucherBarcode = value; } }
        public int VoucherDefinitionStatus { get { return this.voucherDefinitionStatus; } set { this.voucherDefinitionStatus = value; } }
        public DateTime VoucherExpiryDate { get { return this.voucherExpiryDate; } set { this.voucherExpiryDate = value; } }
        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Vouchers()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCReportDBNGCConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
            }
        }
        //end

        #region Methods

        #region View
        /// <summary>
        /// To get the Vocuher Details
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Vocuher Details in xml format</returns>
        public String View(long offerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Vouchers.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Vouchers.View - offerID :" + offerID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_VOUCHER_PARAMETERS, offerID);
                ds.Tables[0].TableName = "Voucher";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Vouchers.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Vouchers.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Vouchers.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Vouchers.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Vouchers.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  
            }
            finally
            {
                
            }
            return viewXml;
        }
        #endregion

        #region ViewVocuhers
        /// <summary>
        /// To get the Vocuher Barcode Details
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Voucher Details in xml format</returns>
        public String ViewVocuhers(long offerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Vouchers.ViewVocuhers");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Vouchers.ViewVocuhers - offerID :" + offerID.ToString());
                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB              
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_VOUCHERBARCODES, offerID);
                ds.Tables[0].TableName = "VoucherBarcodes";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Vouchers.ViewVocuhers");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Vouchers.ViewVocuhers - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Vouchers.ViewVocuhers - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Vouchers.ViewVocuhers - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Vouchers.ViewVocuhers");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  
            }
            finally
            {
                
            }
            return viewXml;
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Vouchers
        /// </summary>
        /// <param name="objectXml">Voucher details</param>/// 
        /// <returns></returns>
        public bool Add(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Vouchers.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Vouchers.Add - objectXml :" + objectXml.ToString());
                Hashtable htblVoucherParams = ConvertXmlHash.XMLToHashTable(objectXml, "Voucher");
                this.MinVoucherValue = Convert.ToDecimal(htblVoucherParams[Constants.MIN_VOUCHER_VALUE]);
                this.MaxVoucherValue = Convert.ToDecimal(htblVoucherParams[Constants.MAX_VOUCHER_VALUE]);
                this.VoucherStepSize = Convert.ToDecimal(htblVoucherParams[Constants.VOUCHER_STEP_SIZE]);
                this.NumberOfVouchers = Convert.ToInt32(htblVoucherParams[Constants.NUMBER_OF_VOUCHERS]);
                this.VoucherExpiryDate = Convert.ToDateTime(htblVoucherParams[Constants.VOUCHER_EXPIRY_DATE]);
                this.OfferID = Convert.ToInt32(htblVoucherParams[Constants.OFFERID]);
                object[] objVoucherParams = { 
                                        userID,
                                        MinVoucherValue, 
                                        MaxVoucherValue,
                                        VoucherStepSize,
                                        OfferID,
                                        NumberOfVouchers,
                                        VoucherExpiryDate
                                     };
                //calls the SP to add Vouchers
                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB                              

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_VOUCHERS, objVoucherParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Vouchers.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Vouchers.Add");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Vouchers.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Vouchers.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Vouchers.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                resultXml = SqlHelper.resultXml;
               
                success = false;
            }
            finally
            {
                
            }
            return success;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Voucher Barcode
        /// </summary>
        /// <param name="objectXml">Voucher details</param>/// 
        /// <returns></returns>
        public bool Update(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Vouchers.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Vouchers.Update - objectXml :" + objectXml.ToString());
                Hashtable htblVoucherBarcode = ConvertXmlHash.XMLToHashTable(objectXml, "VoucherBarcodes");
                this.VoucherType = Convert.ToInt32(htblVoucherBarcode[Constants.VOUCHER_TYPE]);
                this.VoucherBarcode = Convert.ToString(htblVoucherBarcode[Constants.VOUCHER_BARCODE]);
                this.VoucherDefinitionStatus = Convert.ToInt32(htblVoucherBarcode[Constants.VOUCHER_DEFNITION_STATUS_CODE]);
                this.OfferID = Convert.ToInt32(htblVoucherBarcode[Constants.OFFER_ID]);
                object[] objVoucherParams = { 
                                        userID,
                                        VoucherType, 
                                        VoucherBarcode,
                                        VoucherDefinitionStatus,
                                        OfferID
                                     };
                //calls the SP to Update Voucher Barcode
                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB                          

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_VOUCHERBARCODES, objVoucherParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Vouchers.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Vouchers.Update");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Vouchers.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Vouchers.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Vouchers.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                resultXml = SqlHelper.resultXml;
               
                success = false;
            }
            finally
            {
                
            }
            return success;
        }
        #endregion
        #endregion
    }
}
