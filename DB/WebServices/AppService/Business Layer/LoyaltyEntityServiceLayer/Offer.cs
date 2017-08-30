using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    #region Header
    /// <summary>
    /// Offer Details
    /// </summary>
    /// <development>
    ///		<version number="1.00" date="13/Aug/2008">
    ///			<developer>Aneesh</developer>
    ///			<Reviewer></Reviewer>
    ///			<description>Initial Implementation</description>
    ///		</version>
    ///	<development>
    ///	
    #endregion
    public class Offer
    {
        #region Fields

        
        private int offerID;
        private string offerName;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private Single minimumVoucherValue;
        private Single maximumVoucherValue;
        private Single voucherValueStep;
        private int numberOfVouchers;
        private Byte customerSegmentID;
        private int offerWelcomePointsQty;
        private Single amountToPointsConversionRate;
        private Single pointsToRewardConversionRate;
        private int rewardPointsThreshold;
        private DateTime allowReissueDate;
        private DateTime rewardConversionDate;
        private int rewardConversionStatus;
        private int rewardReconversionStatus;
        private DateTime mailingApprovedDate;
        private char mailNonRewardCustomerClubcard;
        private char mailNonRewardCustomerBizcard;
        private int amendBy;

        /// <summary>
        /// Reward Mailing Overview
        /// </summary>
        private int fullMailingStatus;
        //private int offerID;
        //private DateTime mailingApprovedDate;
        //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

        /// <summary>
        /// View Mailing Details
        /// </summary>
        private int viewMailingReportStatusCode;
        // private int offerID;

        /// <summary>
        /// View Reports
        /// </summary>
        private string userName;
        // private int offerID;

        #endregion

        #region Properties

        public int OfferID { get { return offerID; } set { offerID = value; } }
        public string OfferName { get { return offerName; } set { offerName = value; } }
        public DateTime StartDateTime { get { return startDateTime; } set { startDateTime = value; } }
        public DateTime EndDateTime { get { return endDateTime; } set { endDateTime = value; } }
        public Single MinimumVoucherValue { get { return minimumVoucherValue; } set { minimumVoucherValue = value; } }
        public Single MaximumVoucherValue { get { return maximumVoucherValue; } set { maximumVoucherValue = value; } }
        public Single VoucherValueStep { get { return voucherValueStep; } set { voucherValueStep = value; } }
        public int NumberOfVouchers { get { return numberOfVouchers; } set { numberOfVouchers = value; } }
        public Byte CustomerSegmentID { get { return customerSegmentID; } set { customerSegmentID = value; } }
        public int OfferWelcomePointsQty { get { return offerWelcomePointsQty; } set { offerWelcomePointsQty = value; } }
        public Single AmountToPointsConversionRate { get { return amountToPointsConversionRate; } set { amountToPointsConversionRate = value; } }
        public Single PointsToRewardConversionRate { get { return pointsToRewardConversionRate; } set { pointsToRewardConversionRate = value; } }
        public int RewardPointsThreshold { get { return rewardPointsThreshold; } set { rewardPointsThreshold = value; } }
        public DateTime AllowReissueDate { get { return allowReissueDate; } set { allowReissueDate = value; } }
        public DateTime RewardConversionDate { get { return rewardConversionDate; } set { rewardConversionDate = value; } }
        public int RewardConversionStatus { get { return rewardConversionStatus; } set { rewardConversionStatus = value; } }
        public int RewardReconversionStatus { get { return rewardReconversionStatus; } set { rewardReconversionStatus = value; } }
        public DateTime MailingApprovedDate { get { return mailingApprovedDate; } set { mailingApprovedDate = value; } }
        public char MailNonRewardCustomerClubcard { get { return mailNonRewardCustomerClubcard; } set { mailNonRewardCustomerClubcard = value; } }
        public char MailNonRewardCustomerBizcard { get { return mailNonRewardCustomerBizcard; } set { mailNonRewardCustomerBizcard = value; } }
        public int AmendBy { get { return amendBy; } set { amendBy = value; } }

        /// <summary>
        /// Reward Mailing Overview
        /// </summary>
        public int FullMailingStatus { get { return this.fullMailingStatus; } set { this.fullMailingStatus = value; } }
        //public int OfferID { get { return this.offerID; } set { this.offerID = value; } }
        //public DateTime MailingApprovedDate { get { return this.mailingApprovedDate; } set { this.mailingApprovedDate = value; } }

        /// <summary>
        /// View Mailing Details
        /// </summary>
        public int ViewMailingReportStatusCode { get { return this.viewMailingReportStatusCode; } set { this.viewMailingReportStatusCode = value; } }
        //public int OfferID { get { return this.offerID; } set { this.offerID = value; } }

        /// <summary>
        /// View Reports
        /// </summary>
        public string UserName { get { return this.userName; } set { this.userName = value; } }


        #endregion

         //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        private string reportDbconnectionString = "";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Offer()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
                reportDbconnectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCReportDBNGCConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                reportDbconnectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
            }
        }
        //end

        #region View Collection Period
        /// <summary>
        /// To get the Collection Periods
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public String ViewCollectionPeriod(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
           
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            rowCount = 0;
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.ViewCollectionPeriod");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.ViewCollectionPeriod - conditionXml :" + conditionXml);
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetAllCollectionPeriod");
                ds.Tables[0].TableName = "Offer";

                ds1 = SqlHelper.ExecuteDataset(connectionString, "USP_GetLatestOffer");
                ds1.Tables[0].TableName = "LatestOffer";

                ds.Tables.Add(ds1.Tables[0].Copy());

                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.ViewCollectionPeriod");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.ViewCollectionPeriod - viewXml :" + viewXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewCollectionPeriod - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewCollectionPeriod - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region SEARCH
        /// <summary>
        /// To search the Offers
        /// If the number of records in the resultset is greater than 
        /// the maximum row count then the method returns empty resultset
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Offer records in xml format</returns>        

        public string Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            
            DataSet ds = new DataSet();
            rowCount = 0;
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.Search - conditionXml :" + conditionXml);
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetALLOfferDetails");
                ds.Tables[0].TableName = "Offer";
                rowCount = ds.Tables[0].Rows.Count;
                if (rowCount > maxRowCount)
                    viewXml = "";
                else
                    viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.Search - viewXml :" + viewXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region View
        /// <summary>
        /// To get the details of an Offer
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Offer Details in xml format</returns>
        public String View(long offerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.View - offerID :" + offerID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetOfferDetails", offerID);
                ds.Tables[0].TableName = "Offer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                
            }
            return viewXml;
        }
        #endregion

        #region HouseholdCustomerOffer
        /// <summary>
        /// To get the Household's Customer Offer Details
        /// </summary>
        /// <param name="ClubcardID">unique identifier of the offer table</param>/// 
        /// <returns>Offer Details in xml format</returns>
        public String GetHouseholdCustomerOffer(long CustomerID, string culture)
        {
          
            
            DataSet dsPrevColPeriod = new DataSet();
            DataSet dsCurColPeriod = new DataSet();
            DataSet dsPrevOffer = new DataSet();
            DataSet dsCurOffer = new DataSet();
            DataSet dsHouseholdOffer = new DataSet();
            string viewXml = String.Empty;
            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer - CustomerID :" + CustomerID.ToString());
                short IsPrevOffer = 1;
                //Get the Previous Collection Period
                dsPrevColPeriod = SqlHelper.ExecuteDataset(connectionString, "USP_GetCollectionPeriod", IsPrevOffer);
                dsPrevColPeriod.Tables[0].TableName = "PrevCollectionPeriod";

                IsPrevOffer = 0;
                dsCurColPeriod = SqlHelper.ExecuteDataset(connectionString, "USP_GetCollectionPeriod", IsPrevOffer);
                dsCurColPeriod.Tables[0].TableName = "CurCollectionPeriod";

                //Get the Previous offer details
                IsPrevOffer = 1;
                dsPrevOffer = SqlHelper.ExecuteDataset(connectionString, "USP_GetHouseholdCustomerOffer", CustomerID, IsPrevOffer);
                dsPrevOffer.Tables[0].TableName = "PreviousOffer";

                //Get the current offer details
                IsPrevOffer = 0;
                dsCurOffer = SqlHelper.ExecuteDataset(connectionString, "USP_GetHouseholdCustomerOffer", CustomerID, IsPrevOffer);
                dsCurOffer.Tables[0].TableName = "CurrentOffer";

                //Merge all the tables to make single xml file
                dsHouseholdOffer.Tables.Add(dsPrevColPeriod.Tables[0].Copy());
                dsHouseholdOffer.Tables.Add(dsCurColPeriod.Tables[0].Copy());
                dsHouseholdOffer.Tables.Add(dsPrevOffer.Tables[0].Copy());
                dsHouseholdOffer.Tables.Add(dsCurOffer.Tables[0].Copy());

                viewXml = dsHouseholdOffer.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.GetHouseholdCustomerOffer");
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
        /// Add a new Offer
        /// </summary>
        /// <param name="objectXml">Offer details</param>/// 
        /// <returns>Success True/False</returns>
        /// <returns out param>OfferID of the new Offer</returns>
        public bool Add(string objectXml, int userID, long objectId, string resultXml)
        {
        
            objectId = 0;
            resultXml = string.Empty;
            string publication = "";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.Add - objectXml :" + objectXml.ToString());
                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "Offer");
                publication = Convert.ToString(ConfigurationSettings.AppSettings["PublicationName"]);
                this.OfferName = Convert.ToString(htblOffer[Constants.OFFER_NAME]);
                if (htblOffer[Constants.OFFER_START_DATETIME] != null)
                    this.StartDateTime = Convert.ToDateTime(htblOffer[Constants.OFFER_START_DATETIME]);
                if (htblOffer[Constants.OFFER_END_DATETIME] != null)
                    this.EndDateTime = Convert.ToDateTime(htblOffer[Constants.OFFER_END_DATETIME]);
                if (htblOffer[Constants.OFFER_MIN_VOUCHER_VALUE] != null)
                    this.MinimumVoucherValue = Convert.ToSingle(htblOffer[Constants.OFFER_MIN_VOUCHER_VALUE]);
                if (htblOffer[Constants.OFFER_MAX_VAOUCHER_VALUE] != null)
                    this.MaximumVoucherValue = Convert.ToSingle(htblOffer[Constants.OFFER_MAX_VAOUCHER_VALUE]);
                if (htblOffer[Constants.OFFER_VOUCHER_VALUE_STEP] != null)
                    this.VoucherValueStep = Convert.ToSingle(htblOffer[Constants.OFFER_VOUCHER_VALUE_STEP]);
                if (htblOffer[Constants.OFFER_NUMBER_OF_VOUCHERS] != null)
                    this.NumberOfVouchers = Convert.ToInt32(htblOffer[Constants.OFFER_NUMBER_OF_VOUCHERS]);
                //if (htblOffer[Constants.OFFER_CUST_SEGMENTID] != null)
                //    this.CustomerSegmentID = Convert.ToByte(htblOffer[Constants.OFFER_CUST_SEGMENTID]);
                if (htblOffer[Constants.OFFER_WELCOME_POINTS_QTY] != null)
                    this.OfferWelcomePointsQty = Convert.ToInt32(htblOffer[Constants.OFFER_WELCOME_POINTS_QTY]);
                if (htblOffer[Constants.OFFER_AMT_TO_POINTS_CONV_RATE] != null)
                    this.AmountToPointsConversionRate = Convert.ToSingle(htblOffer[Constants.OFFER_AMT_TO_POINTS_CONV_RATE]);
                if (htblOffer[Constants.OFFER_POINTS_TO_RWRD_CONV_RATE] != null)
                    this.PointsToRewardConversionRate = Convert.ToSingle(htblOffer[Constants.OFFER_POINTS_TO_RWRD_CONV_RATE]);
                if (htblOffer[Constants.OFFER_REWARD_POINTS_THRESHOLD] != null)
                    this.RewardPointsThreshold = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_POINTS_THRESHOLD]);
                if (htblOffer[Constants.OFFER_ALLOW_ISSUE_DATE] != null)
                    this.AllowReissueDate = Convert.ToDateTime(htblOffer[Constants.OFFER_ALLOW_ISSUE_DATE]);
                if (htblOffer[Constants.OFFER_REWARD_CONV_DATE] != null)
                    this.RewardConversionDate = Convert.ToDateTime(htblOffer[Constants.OFFER_REWARD_CONV_DATE]);
                if (htblOffer[Constants.OFFER_REWARD_CONV_STATUS] != null)
                    this.RewardConversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_CONV_STATUS]);
                if (htblOffer[Constants.OFFER_REWARD_RECONV_STATUS] != null)
                    this.RewardReconversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_RECONV_STATUS]);
                if (htblOffer[Constants.OFFER_REWARD_RECONV_STATUS] != null)
                    this.RewardReconversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_RECONV_STATUS]);
                if (htblOffer[Constants.OFFER_MAILING_APPROVED_DATE] != null)
                    this.MailingApprovedDate = Convert.ToDateTime(htblOffer[Constants.OFFER_MAILING_APPROVED_DATE]);
                if (htblOffer[Constants.OFFER_MAILING_NON_REWARD_CUST_CLUBCARD] != null)
                    this.MailNonRewardCustomerClubcard = Convert.ToChar(htblOffer[Constants.OFFER_MAILING_NON_REWARD_CUST_CLUBCARD]);
                if (htblOffer[Constants.OFFER_MAILING_NON_REWARD_BIZ_CLUBCARD] != null)
                    this.MailNonRewardCustomerBizcard = Convert.ToChar(htblOffer[Constants.OFFER_MAILING_NON_REWARD_BIZ_CLUBCARD]);
                this.AmendBy = userID;

                object[] objOffer = { 
                                        OfferName, 
                                        StartDateTime,
                                        EndDateTime,
                                        MinimumVoucherValue,
                                        MaximumVoucherValue,
                                        VoucherValueStep,
                                        NumberOfVouchers,
                                        PointsToRewardConversionRate,
                                        AllowReissueDate,
                                        RewardConversionStatus,  
                                        RewardPointsThreshold,
                                        MailNonRewardCustomerClubcard,
                                        MailNonRewardCustomerBizcard,
                                        AmendBy,publication
                                     };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_OFFER, objOffer);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.Add");

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Offer details
        /// </summary>
        /// <returns>Success True/False</returns>
        /// <returns outparam>OfferID of the updated Offer</returns>
        public bool Update(string objectXml, int userID, long objectId, string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.Update - objectXml :" + objectXml.ToString());
                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "Offer");
                this.OfferID = Convert.ToInt32(htblOffer[Constants.OFFER_ID]);
                this.OfferName = Convert.ToString(htblOffer[Constants.OFFER_NAME]);
                if (htblOffer[Constants.OFFER_START_DATETIME] != null)
                    this.StartDateTime = Convert.ToDateTime(htblOffer[Constants.OFFER_START_DATETIME]);
                if (htblOffer[Constants.OFFER_END_DATETIME] != null)
                    this.EndDateTime = Convert.ToDateTime(htblOffer[Constants.OFFER_END_DATETIME]);
                if (htblOffer[Constants.OFFER_MIN_VOUCHER_VALUE] != null)
                    this.MinimumVoucherValue = Convert.ToSingle(htblOffer[Constants.OFFER_MIN_VOUCHER_VALUE]);
                if (htblOffer[Constants.OFFER_MAX_VAOUCHER_VALUE] != null)
                    this.MaximumVoucherValue = Convert.ToSingle(htblOffer[Constants.OFFER_MAX_VAOUCHER_VALUE]);
                if (htblOffer[Constants.OFFER_VOUCHER_VALUE_STEP] != null)
                    this.VoucherValueStep = Convert.ToSingle(htblOffer[Constants.OFFER_VOUCHER_VALUE_STEP]);
                //if (htblOffer[Constants.OFFER_CUST_SEGMENTID] != null)
                //    this.CustomerSegmentID = Convert.ToByte(htblOffer[Constants.OFFER_CUST_SEGMENTID]);
                //if (htblOffer[Constants.OFFER_WELCOME_POINTS_QTY] != null)
                //    this.OfferWelcomePointsQty = Convert.ToInt32(htblOffer[Constants.OFFER_WELCOME_POINTS_QTY]);
                //if (htblOffer[Constants.OFFER_AMT_TO_POINTS_CONV_RATE] != null)
                //    this.AmountToPointsConversionRate = Convert.ToSingle(htblOffer[Constants.OFFER_AMT_TO_POINTS_CONV_RATE]);
                if (htblOffer[Constants.OFFER_POINTS_TO_RWRD_CONV_RATE] != null)
                    this.PointsToRewardConversionRate = Convert.ToSingle(htblOffer[Constants.OFFER_POINTS_TO_RWRD_CONV_RATE]);
                if (htblOffer[Constants.OFFER_REWARD_POINTS_THRESHOLD] != null)
                    this.RewardPointsThreshold = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_POINTS_THRESHOLD]);
                if (htblOffer[Constants.OFFER_ALLOW_ISSUE_DATE] != null)
                    this.AllowReissueDate = Convert.ToDateTime(htblOffer[Constants.OFFER_ALLOW_ISSUE_DATE]);
                //if (htblOffer[Constants.OFFER_REWARD_CONV_DATE] != null)
                //    this.RewardConversionDate = Convert.ToDateTime(htblOffer[Constants.OFFER_REWARD_CONV_DATE]);
                //if (htblOffer[Constants.OFFER_REWARD_CONV_STATUS] != null)
                //    this.RewardConversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_CONV_STATUS]);
                //if (htblOffer[Constants.OFFER_REWARD_RECONV_STATUS] != null)
                //    this.RewardReconversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_RECONV_STATUS]);
                //if (htblOffer[Constants.OFFER_REWARD_RECONV_STATUS] != null)
                //    this.RewardReconversionStatus = Convert.ToInt32(htblOffer[Constants.OFFER_REWARD_RECONV_STATUS]);
                //if (htblOffer[Constants.OFFER_MAILING_APPROVED_DATE] != null)
                //    this.MailingApprovedDate = Convert.ToDateTime(htblOffer[Constants.OFFER_MAILING_APPROVED_DATE]);
                if (htblOffer[Constants.OFFER_MAILING_NON_REWARD_CUST_CLUBCARD] != null)
                    this.MailNonRewardCustomerClubcard = Convert.ToChar(htblOffer[Constants.OFFER_MAILING_NON_REWARD_CUST_CLUBCARD]);
                if (htblOffer[Constants.OFFER_MAILING_NON_REWARD_BIZ_CLUBCARD] != null)
                    this.MailNonRewardCustomerBizcard = Convert.ToChar(htblOffer[Constants.OFFER_MAILING_NON_REWARD_BIZ_CLUBCARD]);
                this.AmendBy = userID;

                object[] objOffer = { 
                                        OfferID,
                                        OfferName, 
                                        EndDateTime,
                                        AllowReissueDate,
                                        MinimumVoucherValue,
                                        VoucherValueStep,
                                        PointsToRewardConversionRate,
                                        RewardPointsThreshold,
                                        MailNonRewardCustomerClubcard,
                                        MailNonRewardCustomerBizcard,   
                                        AmendBy
                                     };

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_OFFER, objOffer);
                objectId = this.OfferID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.Update");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delte an Offer
        /// </summary>
        /// <param name="objectXml">OfferID</param>/// 
        /// <returns>Success True/False</returns>
        public bool Delete(string objectXml, int userID, long objectId, string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.Delete");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.Delete - objectXml :" + objectXml.ToString());
                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "Offer");
                this.OfferID = Convert.ToInt32(htblOffer[Constants.OFFER_ID]);
                this.AmendBy = userID;
                object[] objOffer = { OfferID, AmendBy };

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_OFFER, objOffer);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.Delete");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.Delete");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.Delete");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }
        #endregion


        #region View CurrentOffer
        /// <summary>
        /// To get (view) the Current Details
        /// </summary>
        public String GetCurrentOfferDetails(string objectXml, int userID, long objectId, string resultXml)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.GetCurrentOfferDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.GetCurrentOfferDetails - objectXml :" + objectXml.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CURRENT_OFFER_DETAILS);
                ds.Tables[0].TableName = "Offer";
                viewXml = ds.GetXml();

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.GetCurrentOfferDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.GetCurrentOfferDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.GetCurrentOfferDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.GetCurrentOfferDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.GetCurrentOfferDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region View Reward Mailing Details
        /// <summary>
        /// To view the mailing Details
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Mailing details in xml Format</returns>
        public String ViewRewardMailingDetails(long offerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.ViewRewardMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.ViewRewardMailingDetails - offerID :" + offerID.ToString());

                ds = SqlHelper.ExecuteDataset(reportDbconnectionString, Constants.SP_VIEW_OFFER_DETAILS, offerID);
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.ViewRewardMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.ViewRewardMailingDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewRewardMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewRewardMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewRewardMailingDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region Update Reward Mailing Details
        /// <summary>
        /// Update Mailing Detals
        /// </summary>
        /// <param name="objectXml">Mailing Details to be updated</param>
        /// <param name="userID">User who is updation the Mailing Details</param>
        /// <param name="objectId"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool UpdateRewardMailingDetails(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
          
            bool success;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.UpdateRewardMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.UpdateRewardMailingDetails - objectXml :" + objectXml.ToString());
                Hashtable htblFullMailing = ConvertXmlHash.XMLToHashTable(objectXml, "Offer");
                this.FullMailingStatus = Convert.ToInt32(htblFullMailing[Constants.FULL_MAILING_STATUS]);
                this.OfferID = Convert.ToInt32(htblFullMailing[Constants.OFFERID]);
                if (htblFullMailing.Contains(Constants.MAILING_APPROVED_DATE))
                {
                    this.MailingApprovedDate = Convert.ToDateTime(htblFullMailing[Constants.MAILING_APPROVED_DATE]);
                    object[] objVoucherParams = { 
                                        userID,
                                        FullMailingStatus, 
                                        OfferID,
                                        MailingApprovedDate
                                     };
                    //calls the SP to Update Mailing Status
                    SqlHelper.ExecuteNonQuery(reportDbconnectionString, Constants.SP_UPDATE_MAILING_STATUS, objVoucherParams);
                    success = SqlHelper.result.Flag;
                }
                else
                {
                    object[] objVoucherParams = { 
                                        userID,
                                        FullMailingStatus, 
                                        OfferID,
                                        null
                                     };
                    //calls the SP to Update Mailing Status
                    SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_MAILING_STATUS, objVoucherParams);
                    success = SqlHelper.result.Flag;
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.ViewRewardMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.ViewRewardMailingDetails - resultXml :" + resultXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewRewardMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewRewardMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewRewardMailingDetails");
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

        #region View Mailing Details
        /// <summary>
        /// To get the details of the Mailing
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Mailing Details in xml format</returns>
        public String ViewMailingDetails(long offerID, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.ViewMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.ViewMailingDetails - offerID :" + offerID.ToString());


                ds = SqlHelper.ExecuteDataset(reportDbconnectionString, Constants.SP_VIEW_MAILING);
                ds.Tables[0].TableName = "Offer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.ViewMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.ViewMailingDetails - viewXml :" + viewXml.ToString());
               
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewMailingDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region Update Mailing Details
        /// <summary>
        /// To update the Mailing Report Status
        /// </summary>
        /// <param name="objectXml">Mailing Details</param>/// 
        /// <returns></returns>
        public bool UpdateMailingDetails(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            bool success;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.UpdateMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.UpdateMailingDetails - objectXml :" + objectXml.ToString());
                
                Hashtable htblFullMailing = ConvertXmlHash.XMLToHashTable(objectXml, "Offer");
                this.ViewMailingReportStatusCode = Convert.ToInt32(htblFullMailing[Constants.VIEWMAILINGREPORTSTATUSCODE]);
                this.OfferID = Convert.ToInt32(htblFullMailing[Constants.OFFERID]);
                object[] objVoucherParams = { 
                                        userID,
                                        ViewMailingReportStatusCode, 
                                        OfferID
                                     };
                //calls the SP to update mailing status
                SqlHelper.ExecuteNonQuery(reportDbconnectionString, Constants.SP_UPDATE_MAILING_STATUS_CODE, objVoucherParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.UpdateMailingDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.UpdateMailingDetails");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewMailingDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewMailingDetails");
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

        #region Get the Optinal Use Status Descriptions
        /// <summary>
        /// To get the details of the Mailing
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Mailing Details in xml format</returns>
        public String GetOptinalCustomerStatus(long offerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.GetOptinalCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.GetOptinalCustomerStatus - offerID :" + offerID.ToString());

                ds = SqlHelper.ExecuteDataset(reportDbconnectionString, Constants.SP_GET_CUSTOMEROPTIONALSTATUS);
                ds.Tables[0].TableName = "CustomerUseStatus";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.GetOptinalCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.GetOptinalCustomerStatus - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.GetOptinalCustomerStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.GetOptinalCustomerStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.GetOptinalCustomerStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region Get the list of Reports and Schedule Information
        /// <summary>
        /// To fetch all the Report Descriptions
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Partner records in xml format</returns>        
        public String ViewReports(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
          
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Offer.ViewReports");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Offer.ViewReports - conditionXml :" + offerID.ToString());
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "Reports");
                this.UserName = Convert.ToString(htblVouchers[Constants.USER_NAME]);
                object[] objReportParams = { 
                                        UserName,
                                        culture
                                     };
                ds = SqlHelper.ExecuteDataset(reportDbconnectionString, Constants.SP_VIEW_REPORTS, objReportParams);
                ds.Tables[0].TableName = "AccountsReports";
                ds.Tables[1].TableName = "ColPrdReports";
                ds.Tables[2].TableName = "PPReports";
                ds.Tables[3].TableName = "FraudReports";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Offer.ViewReports");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Offer.ViewReports - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Offer.ViewReports - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Offer.ViewReports - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Offer.ViewReports");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion
    }
}
