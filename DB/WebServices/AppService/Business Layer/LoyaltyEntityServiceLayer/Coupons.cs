/*
 * File   : Coupons.cs
 * Author : Kavitha S (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Coupons
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
    /// Set Up Coupons
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

    public class Coupons
    {

        #region Fields
        private string couponDesc;
        private string couponTypeBarcode1;
        private string couponTypeBarcode2;
        private string rewardCustomerInd;
        private string nonRewardCustomerInd;
        private string culture;
        private int offerID;
        private int couponType;
        
        #endregion

        #region Properties
        public string CouponDesc { get { return this.couponDesc; } set { this.couponDesc = value; } }
        public string CouponTypeBarcode1 { get { return this.couponTypeBarcode1; } set { this.couponTypeBarcode1 = value; } }
        public string CouponTypeBarcode2 { get { return this.couponTypeBarcode2; } set { this.couponTypeBarcode2 = value; } }
        public string RewardCustomerInd { get { return this.rewardCustomerInd; } set { this.rewardCustomerInd = value; } }
        public string NonRewardCustomerInd { get { return this.nonRewardCustomerInd; } set { this.nonRewardCustomerInd = value; } }
        public string Culture { get { return this.culture; } set { this.culture = value; } }
        public int OfferID { get { return this.offerID; } set { this.offerID = value; } }
        public int CouponType { get { return this.couponType; } set { this.couponType = value; } }
        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Coupons()
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
        /// To get the Coupon Details
        /// </summary>
        /// <param name="objectXml">Coupon Details</param>/// 
        /// <returns></returns>
        public String View(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Coupons.View");
            NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Coupons.View - conditionXml :" + conditionXml);
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                Hashtable htblCoupons = ConvertXmlHash.XMLToHashTable(conditionXml, "Coupons");
                this.Culture = Convert.ToString(htblCoupons[Constants.CULTURE]);
                this.OfferID = Convert.ToInt32(htblCoupons[Constants.OFFER_ID]);
                object[] objCouponsParams = { 
                                        Culture,
                                        OfferID
                                     };
                

                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_COUPONS_PARAMETERS, objCouponsParams);
                ds.Tables[0].TableName = "Coupons";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Coupons.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Coupons.View - viewXml :" + viewXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Coupons.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Coupons.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Coupons.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }

        #endregion

        #region Update Coupons
        /// <summary>
        /// Update Coupons
        /// </summary>
        /// <param name="objectXml">Coupon details</param>/// 
        /// <returns></returns>
        public bool Update(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Coupons.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Coupons.Update - objectXml :" + objectXml);
                Hashtable htblCouponsParams = ConvertXmlHash.XMLToHashTable(objectXml, "Coupons");
                this.CouponType = Convert.ToInt32(htblCouponsParams[Constants.COUPON_TYPE]);
                this.CouponDesc = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_DESC]);
                this.CouponTypeBarcode1 = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_BARCODE1]);
                this.CouponTypeBarcode2 = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_BARCODE2]);
                this.RewardCustomerInd = Convert.ToString(htblCouponsParams[Constants.REWARD_CUSTOMER_IND]);
                this.NonRewardCustomerInd = Convert.ToString(htblCouponsParams[Constants.NON_REWARDCUSTOMER_IND]);
                this.Culture = Convert.ToString(htblCouponsParams[Constants.CULTURE]);
                object[] objCouponsParams = { 
                                    userID,
                                    CouponType,
                                    CouponDesc, 
                                    CouponTypeBarcode1,
                                    CouponTypeBarcode2,
                                    RewardCustomerInd,
                                    NonRewardCustomerInd,                                    
                                    Culture
                                 };
                

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_COUPONS, objCouponsParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Coupons.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Coupons.Update");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Coupons.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Coupons.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Coupons.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {
          
            }
            return success;
        }
        #endregion


        #region Insert Coupons
        /// <summary>
        /// Update Coupons
        /// </summary>
        /// <param name="objectXml">Coupon details</param>/// 
        /// <returns></returns>
        public bool Insert(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Coupons.Insert");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Coupons.Insert - objectXml :" + objectXml);
                Hashtable htblCouponsParams = ConvertXmlHash.XMLToHashTable(objectXml, "Coupons");

                this.CouponDesc = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_DESC]);
                this.CouponTypeBarcode1 = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_BARCODE1]);
                this.CouponTypeBarcode2 = Convert.ToString(htblCouponsParams[Constants.COUPON_TYPE_BARCODE2]);
                this.RewardCustomerInd = Convert.ToString(htblCouponsParams[Constants.REWARD_CUSTOMER_IND]);
                this.NonRewardCustomerInd = Convert.ToString(htblCouponsParams[Constants.NON_REWARDCUSTOMER_IND]);
                this.OfferID = Convert.ToInt32(htblCouponsParams[Constants.OFFERID]);
                this.Culture = Convert.ToString(htblCouponsParams[Constants.CULTURE]);
                object[] objCouponsParams = { 
                                        userID,
                                        CouponDesc, 
                                        CouponTypeBarcode1,
                                        CouponTypeBarcode2,
                                        RewardCustomerInd,
                                        NonRewardCustomerInd,
                                        OfferID,
                                        Culture
                                     };
               
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_COUPONS, objCouponsParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Coupons.Insert");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Coupons.Insert");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Coupons.Insert - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Coupons.Insert - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Coupons.Insert");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {
            }
            return success;
        }
        #endregion

        #region Insert Coupons
        /// <summary>
        /// Delete Coupons
        /// </summary>
        /// <param name="objectXml">Coupon details</param>/// 
        /// <returns></returns>
        public bool Delete(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Coupons.Delete");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Coupons.Delete - objectXml :" + objectXml);
                Hashtable htblCouponsParams = ConvertXmlHash.XMLToHashTable(objectXml, "Coupons");
                this.CouponType = Convert.ToInt32(htblCouponsParams[Constants.COUPON_TYPE]);
                this.OfferID = Convert.ToInt32(htblCouponsParams[Constants.OFFERID]);
                object[] objCouponsParams = { 
                                    userID,
                                    CouponType,
                                    OfferID                                
                                 };
                

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_COUPONS, objCouponsParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Coupons.Delete");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Coupons.Delete");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Coupons.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Coupons.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Coupons.Delete");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
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
