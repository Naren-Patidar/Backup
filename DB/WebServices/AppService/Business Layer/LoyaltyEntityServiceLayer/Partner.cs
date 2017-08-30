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
    ///
    /// <summary>
    /// Partner Details
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

    public class Partner
    {
        #region Fields

        private long partnerID;
        private string partnerName;
        private short partnerType;
        private short partnerStatus;
        private string contactName;
        private long agencyID;
        private int tescoStoreID;
        private DateTime partnerDateJoined;
        private DateTime partnerDateLeft;
        private string partnerRegisteredBy;
        private string partnerBusinessRegNo;
        private string partnerZipPassword;
        private long partnerLastBatchSeqNo;
        private int partnerAddPointsLimit;
        private short outletsToBeMaintained;
        private int partnerSubtractPointsLimit;
        private double pricePerPoint;
        private string addressLine1;
        private string addressLine2;
        private string addressLine3;
        private string addressLine4;
        private string addressLine5;
        private string addressLine6;
        private string postCode;
        private string phoneNumber;
        private string faxNumber;
        private string emailAddress;
        private string websiteURL;
        private int businessType;
        private DateTime dateofJoin;
        private DateTime toDate;
        private DateTime fromDate;
        private int retriveLimit;
        private string partneroutletRef;
        private string transPartnerID;
        private string sort_1;
        private string transAgencyID;
        private string sortOrder;
        //Add PP Transaction
        private Int64 partnerId;
        private Int64 clubcardID;
        private DateTime txnDate;
        private decimal amountSpent;
        private Int64 transactionReasonID;
        private Int64 welcomePointsQty;
        private Int64 manualPointsQty;
        private Int64 skuPointsQty;
        private Int64 greenPointsQty;
        private Int64 sourcePosID;
        private Int64 sourceSystemTransactionID;
        private Int64 txnType;
        private Int64 partnerOutletID;
        private string culture;
        #endregion

        #region Properties

        public Int64 PartnerID { get { return partnerID; } set { partnerID = value; } }
        public string PartnerName { get { return partnerName; } set { partnerName = value; } }
        public short PartnerType { get { return partnerType; } set { partnerType = value; } }
        public short PartnerStatus { get { return partnerStatus; } set { partnerStatus = value; } }
        public string ContactName { get { return contactName; } set { contactName = value; } }
        public long AgencyID { get { return agencyID; } set { agencyID = value; } }
        public int TescoStoreID { get { return tescoStoreID; } set { tescoStoreID = value; } }
        public DateTime PartnerDateJoined { get { return partnerDateJoined; } set { partnerDateJoined = value; } }
        public DateTime PartnerDateLeft { get { return partnerDateLeft; } set { partnerDateLeft = value; } }
        public string PartnerRegisteredBy { get { return partnerRegisteredBy; } set { partnerRegisteredBy = value; } }
        public string PartnerBusinessRegNo { get { return partnerBusinessRegNo; } set { partnerBusinessRegNo = value; } }
        public string PartnerZipPassword { get { return partnerZipPassword; } set { partnerZipPassword = value; } }
        public long PartnerLastBatchSeqNo { get { return partnerLastBatchSeqNo; } set { partnerLastBatchSeqNo = value; } }
        public short OutletsToBeMaintained { get { return outletsToBeMaintained; } set { outletsToBeMaintained = value; } }
        public int PartnerAddPointsLimit { get { return partnerAddPointsLimit; } set { partnerAddPointsLimit = value; } }
        public int PartnerSubtractPointsLimit { get { return partnerSubtractPointsLimit; } set { partnerSubtractPointsLimit = value; } }
        public double PricePerPoint { get { return pricePerPoint; } set { pricePerPoint = value; } }
        public string AddressLine1 { get { return addressLine1; } set { addressLine1 = value; } }
        public string AddressLine2 { get { return addressLine2; } set { addressLine2 = value; } }
        public string AddressLine3 { get { return addressLine3; } set { addressLine3 = value; } }
        public string AddressLine4 { get { return addressLine4; } set { addressLine4 = value; } }
        public string AddressLine5 { get { return addressLine5; } set { addressLine5 = value; } }
        public string AddressLine6 { get { return addressLine6; } set { addressLine6 = value; } }
        public string PostCode { get { return postCode; } set { postCode = value; } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        public string FaxNumber { get { return faxNumber; } set { faxNumber = value; } }
        public string EmailAddress { get { return emailAddress; } set { emailAddress = value; } }
        public string WebsiteURL { get { return websiteURL; } set { websiteURL = value; } }
        public int BusinessType { get { return businessType; } set { businessType = value; } }
        public DateTime DateJoined { get { return dateofJoin; } set { dateofJoin = value; } }

        public DateTime ToDate { get { return toDate; } set { toDate = value; } }
        public DateTime FromDate { get { return fromDate; } set { fromDate = value; } }
        public int RetrieveLimit { get { return retriveLimit; } set { retriveLimit = value; } }
        public string PartnerOutletRef { get { return partneroutletRef; } set { partneroutletRef = value; } }
        public string TransPartnerID { get { return transPartnerID; } set { transPartnerID = value; } }

        public string Sort_1 { get { return sort_1; } set { sort_1 = value; } }
        public string TransAgencyID { get { return transAgencyID; } set { transAgencyID = value; } }
        public string SortOrder { get { return sortOrder; } set { sortOrder = value; } }

        //Add PP Transaction
        public Int64 PartnerId { get { return partnerId; } set { partnerId = value; } }
        public Int64 ClubcardID { get { return clubcardID; } set { clubcardID = value; } }
        public DateTime TxnDate { get { return txnDate; } set { txnDate = value; } }
        public decimal AmountSpent { get { return amountSpent; } set { amountSpent = value; } }
        public Int64 TransactionReasonID { get { return transactionReasonID; } set { transactionReasonID = value; } }
        public Int64 WelcomePointsQty { get { return welcomePointsQty; } set { welcomePointsQty = value; } }
        public Int64 ManualPointsQty { get { return manualPointsQty; } set { manualPointsQty = value; } }
        public Int64 SKUPointsQty { get { return skuPointsQty; } set { skuPointsQty = value; } }
        public Int64 GreenPointsQty { get { return greenPointsQty; } set { greenPointsQty = value; } }
        public Int64 SourcePosID { get { return sourcePosID; } set { sourcePosID = value; } }
        public Int64 SourceSystemTransactionID { get { return sourceSystemTransactionID; } set { sourceSystemTransactionID = value; } }
        public Int64 TxnType { get { return txnType; } set { txnType = value; } }
        public Int64 PartnerOutletID { get { return partnerOutletID; } set { partnerOutletID = value; } }
        public string Culture { get { return culture; } set { culture = value; } }

        #endregion
         //Added as part of ROI conncetion string management
        //begin
        
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Partner()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
            }
        }
        //end


 

        #region Methods

        #region View
        /// <summary>
        /// To get the details of an Partner
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public String View(long partnerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.View - partnerID :" + partnerID.ToString());
                object[] objPartnerView = { partnerID, culture };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNER, objPartnerView);
                ds.Tables[0].TableName = "Partner";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region View Transaction Outlet Details
        /// <summary>
        /// To get the details of an Partner
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public String ViewTransOutlets(long partnerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewTransOutlets");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.ViewTransOutlets - partnerID :" + partnerID.ToString());
                object[] objPartnerViewTrans = { partnerID };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TRANSOUTLETS, objPartnerViewTrans);
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewTransOutlets");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewTransOutlets - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewTransOutlets - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewTransOutlets - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewTransOutlets");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region Search
        /// <summary>
        /// To search the partners according to the given search criteria
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Partner records in xml format</returns>        
        public String Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.Search - conditionXml :" + conditionXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(conditionXml, "Partner");
                this.PartnerName = (string)htblPartner[Constants.PARTNER_NAME];
                this.PartnerID = (int)htblPartner[Constants.PARTNER_NUMBER];
                this.PostCode = (string)htblPartner[Constants.POSTAL_CODE];
                this.PartnerStatus = (short)htblPartner[Constants.STATUS];
                string sortBy = (string)htblPartner[Constants.SORT_BY];
                string sortOrder = (string)htblPartner[Constants.SORT_ORDER];
                object[] objPartner = { PartnerName, PartnerID, PostCode, PartnerStatus, sortBy, sortOrder, maxRowCount, rowCount };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNER, objPartner);
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.Search - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }

        public string ViewPartners(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();

            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewPartners");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.ViewPartners - conditionXml :" + conditionXml.ToString());
                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(conditionXml, "TescoPartner");
                DataSet dsStores = new DataSet();
                dsStores = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TESCO_PARTNERS, culture);

                viewXml = dsStores.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewPartners");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewPartners - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewPartners - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewPartners - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewPartners");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        public string ViewPartnerType(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewPartnerType");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.artner.ViewPartnerType - conditionXml :" + conditionXml.ToString());
                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(conditionXml, "TescoPartner");
                DataSet dsPartners = new DataSet();
                dsPartners = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNERTYPE);

                viewXml = dsPartners.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewPartnerType");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewPartnerType - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewPartnerType - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewPartnerType - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewPartnerType");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        #region SearchTrans
        /// <summary>
        /// To search the partners according to the given search criteria
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Partner records in xml format</returns>        
        public String SearchTrans(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.SearchTrans");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.SearchTrans - conditionXml :" + conditionXml.ToString());

                Hashtable htSearchTrans = ConvertXmlHash.XMLToHashTable(conditionXml, "Partner");

                this.FromDate = Convert.ToDateTime(htSearchTrans[Constants.DATE_FROM]);
                //string TempFromDate = (FromDate.Year + FromDate.Month + FromDate.Day).ToString();
                //this.FromDate = String.Format("{0:yyyyMMdd}", DateTime.Now);
                //this.FromDate = DateTime.Now.Year +  String.Format("{0:yyyyMMdd}", DateTime.Now);

                string TempFromDate = String.Format("{0:yyyy-MM-dd}", FromDate);
                this.ToDate = Convert.ToDateTime(htSearchTrans[Constants.DATE_TO]);
                //string TempToDate = (ToDate.Year+ToDate.Month+ToDate.Day).ToString();
                string TempToDateDate = String.Format("{0:yyyy-MM-dd}", ToDate);

                this.RetrieveLimit = Convert.ToUInt16(htSearchTrans[Constants.RETRIEVENUMBER]);
                this.TransPartnerID = (string)htSearchTrans[Constants.PARTNER_ID];
                this.PartnerOutletRef = (string)htSearchTrans[Constants.PARTNER_OUTLETID];

                this.PartnerName = (string)htSearchTrans[Constants.PARTNER_NAME_TRANS];
                this.Sort_1 = (string)htSearchTrans[Constants.SORT_1];
                this.TransAgencyID = (string)htSearchTrans[Constants.AGENCY_ID_TRANS];
                this.SortOrder = (string)htSearchTrans[Constants.SORT_ORDER_TRANS];

                object[] objSearchTrans = { TempFromDate, TempToDateDate, RetrieveLimit, TransAgencyID, TransPartnerID, PartnerName, PartnerOutletRef, Sort_1, SortOrder };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_SEARCHTRANS, objSearchTrans);
               // rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.SearchTrans");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.SearchTrans - viewXml :" + viewXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.SearchTrans - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.SearchTrans - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.SearchTrans");
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
        /// Add a new partner
        /// </summary>
        /// <param name="objectXml">Partner details</param>/// 
        /// <returns>PartnerID of the new Partner</returns>
        public bool Add(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.Add - objectXml :" + objectXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(objectXml, "Partner");
                this.PartnerName = (string)htblPartner[Constants.PARTNER_NAME];
                this.PartnerAddPointsLimit = Convert.ToInt32(htblPartner[Constants.POINTS_ADD_LIMIT]);
                this.EmailAddress = (string)htblPartner[Constants.EMAIL];
                this.ContactName = (string)htblPartner[Constants.CONTACT_NAME];
                this.PartnerBusinessRegNo = (string)htblPartner[Constants.BUSINESS_REG_NUMBER];
                this.WebsiteURL = (string)htblPartner[Constants.WEBSITE];
                this.PhoneNumber = (string)htblPartner[Constants.PHONE];
                this.PartnerSubtractPointsLimit = Convert.ToInt32(htblPartner[Constants.POINTS_SUB_LIMIT]);
                this.AddressLine2 = (string)htblPartner[Constants.ADDRESS2];
                this.TescoStoreID = Convert.ToInt32(htblPartner[Constants.ASSOCIATED_STORE]);
                this.AddressLine1 = (string)htblPartner[Constants.ADDRESS1];
                this.AgencyID = Convert.ToInt64(htblPartner[Constants.AGENCY_ID]);
                this.PricePerPoint = Convert.ToDouble(htblPartner[Constants.PRICE_PER_POINT]);
                this.PostCode = (string)htblPartner[Constants.POSTAL_CODE];
                this.FaxNumber = (string)htblPartner[Constants.FAX];
                this.PartnerType = Convert.ToInt16(htblPartner[Constants.PARTNER_TYPE]);
                this.PartnerID = Convert.ToInt64(htblPartner[Constants.PARTNER_ID]);
                this.OutletsToBeMaintained = Convert.ToInt16(htblPartner["OutletsToBeMaintained"]);
                this.BusinessType = Convert.ToInt16(htblPartner[Constants.BUSINESS_TYPE]);
                this.PartnerType = Convert.ToInt16(htblPartner[Constants.PARTNER_TYPE]);//VP
                this.PartnerStatus = 1;//VP

                object[] objPartner = {PartnerName, PartnerAddPointsLimit, EmailAddress, ContactName, PartnerBusinessRegNo, WebsiteURL,
                                       PhoneNumber, PartnerSubtractPointsLimit, AddressLine2, TescoStoreID, AddressLine1, AgencyID, 
                                       PricePerPoint, PostCode, FaxNumber, PartnerType, PartnerID, OutletsToBeMaintained, PartnerStatus,
                                       DateTime.Now, BusinessType, DateTime.Now, 
                                       444, 
                                       userID}; //VP
                //object[] objPartner = { PartnerName, PartnerAddPointsLimit,EmailAddress,ContactName,PartnerBusinessRegNo,WebsiteURL,PhoneNumber,PartnerSubtractPointsLimit,AddressLine2,2,AddressLine1,AgencyID,PricePerPoint,PostCode,  FaxNumber,PartnerType,PartnerID,OutletsToBeMaintained,PartnerType,DateTime.Now,PartnerName,DateTime.Now,444,userID}; //VP

                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_PARTNER, objPartner);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.Add");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.SearchTrans - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.SearchTrans - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.SearchTrans");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                return false;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;

        }
        #endregion

        #region Update
        /// <summary>
        /// Update partner details
        /// </summary>
        /// <param name="objectXml">Partner details</param>/// 
        /// <returns>PartnerID of the new Partner</returns>
        public bool Update(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            int retval = 0, identityParam = 0;
            resultXml = string.Empty;
            objectId = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.Update - objectXml :" + objectXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(objectXml, "Partner");

                this.PartnerName = (string)htblPartner[Constants.PARTNER_NAME];
                this.PartnerID = Convert.ToInt64(htblPartner[Constants.PARTNER_NUMBER]);
                this.PartnerType = Convert.ToInt16(htblPartner[Constants.PARTNER_TYPE]);
                this.PartnerStatus = Convert.ToInt16(htblPartner[Constants.STATUS]);
                this.ContactName = (string)htblPartner[Constants.CONTACT_NAME];
                this.AgencyID = Convert.ToInt64(htblPartner[Constants.AGENCYID]);
                this.TescoStoreID = Convert.ToInt32(htblPartner[Constants.ASSOCIATED_STORE]);
                //this.PartnerDateJoined = (DateTime)htblPartner[Constants.DATE_JOINED];
                //this.PartnerDateLeft = (DateTime)htblPartner[Constants.DATE_LEFT];
                this.PartnerRegisteredBy = (string)htblPartner[Constants.REGISTERED_BY];
                this.PartnerBusinessRegNo = (string)htblPartner[Constants.BUSINESS_REG_NUMBER];
                //@PartnerZipPassword NVARCHAR(20),
                this.PartnerLastBatchSeqNo = Convert.ToInt64(htblPartner[Constants.LAST_BATCH_PROCESSED]);
                if (htblPartner[Constants.OUTLETS_TOBE_MAINTAINED].ToString().ToUpper() == "YES")
                {
                    this.OutletsToBeMaintained = 1;
                }
                else
                {
                    this.OutletsToBeMaintained = 0;
                }
                this.PartnerAddPointsLimit = Convert.ToInt32(htblPartner[Constants.POINTS_ADD_LIMIT]);
                this.PartnerSubtractPointsLimit = Convert.ToInt32(htblPartner[Constants.POINTS_SUB_LIMIT]);
                this.PricePerPoint = Convert.ToDouble(htblPartner[Constants.PRICE_PER_POINT]);
                this.AddressLine1 = (string)htblPartner[Constants.ADDRESS1];
                this.AddressLine2 = (string)htblPartner[Constants.ADDRESS2];
                this.PostCode = (string)htblPartner[Constants.POSTAL_CODE];
                this.PhoneNumber = (string)htblPartner[Constants.PHONE];
                this.FaxNumber = (string)htblPartner[Constants.FAX];
                this.EmailAddress = (string)htblPartner[Constants.EMAIL];
                this.WebsiteURL = (string)htblPartner[Constants.WEBSITE];
                this.BusinessType = Convert.ToInt16(htblPartner[Constants.BUSINESS_TYPE]);

                //this.PartnerLastBatchSeqNo = (string)htblPartner[Constants.LAST_BATCH_PROCESSED];             

                object[] objPartner = { 
                                       
                                        PartnerID,
                                        PartnerName, 
                                        PartnerType,
                                        PartnerStatus,
                                        ContactName,
                                        AgencyID,
                                        TescoStoreID,
                                        DateTime.Now,
                                        DateTime.Now,
                                        //PartnerDateJoined,
                                        //PartnerDateLeft,
                                        PartnerRegisteredBy,
                                        PartnerBusinessRegNo,
                                        PartnerLastBatchSeqNo,
                                        OutletsToBeMaintained, 
                                        PartnerAddPointsLimit,
                                        PartnerSubtractPointsLimit,                                        
                                        PricePerPoint,
                                        AddressLine1,
                                        AddressLine2,
                                        "",
                                        "",
                                        "",
                                        "",
                                        PostCode,
                                        PhoneNumber,
                                        FaxNumber,
                                        EmailAddress,
                                        WebsiteURL,                                                                                                                                                          
                                        userID,
                                        BusinessType
                                        //identityParam
                                     };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_PARTNER, objPartner);
                objectId = this.PartnerID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.Update");
                // return retval;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                return false;
            }
            finally
            {
            }
            return true;
        }
        #endregion

        #region ViewAssociatedPartners
        //Harshal added this procedure on 18/Sep/2008
        /// <summary>To get all the partners associated to an Agency according to the search criteria</summary>        
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>///                 
        /// <returns>Partner records in xml format</returns>
        public String ViewAssociatedPartners(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            string sNumber = "";
            string sortBy;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewAssociatedPartners");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.ViewAssociatedPartners - conditionXml :" + conditionXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(conditionXml, "Partner");

                Int64 AgencyID = Convert.ToInt64(htblPartner[Constants.AGENCYID].ToString());

                this.PartnerName = (string)htblPartner[Constants.PARTNER_NAME];
                if (htblPartner["PartnerNumber"] != null)
                {
                    sNumber = htblPartner["PartnerNumber"].ToString();
                    //this.PartnerID = Convert.ToInt64(htblPartner["PartnerNumber"].ToString());
                }
                if (htblPartner[Constants.PARTNER_STATUS] != null)
                {
                    this.PartnerStatus = Convert.ToInt16(htblPartner[Constants.PARTNER_STATUS].ToString());
                }
                if (htblPartner[Constants.PARTNER_STATUS] != null)
                {
                    sortBy = (string)htblPartner[Constants.SORT_BY];
                }
                else
                {
                    sortBy = "0";
                }

                string sortOrder = (string)htblPartner[Constants.SORT_ORDER];

                object[] objPartner = { AgencyID, sNumber, PartnerName, PartnerStatus, sortBy, sortOrder, culture }; //, maxRowCount, rowCount };
                //object[] objPartner = {AgencyID, PartnerID, PartnerName, PartnerStatus, sortBy, sortOrder}; //, maxRowCount, rowCount };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_ASSOCIATED_PARTNERS, objPartner);
                ds.Tables[0].TableName = "Partner";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewAssociatedPartners");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewAssociatedPartners - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewAssociatedPartners - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewAssociatedPartners - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewAssociatedPartners");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }

        #endregion

        #region Search Partner,Add PP Transaction

        /// <summary>
        /// To get the partners according to the given search criteria
        /// </summary>
        public string LookupSearch(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();
            

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.LookupSearch");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.LookupSearch - conditionXml :" + conditionXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.PartnerName = (string)htblPartner[Constants.PARTNER_NAME];
                string partnerId = (string)htblPartner[Constants.PARTNER_NUMBER];
                if (partnerId != "")
                {
                    PartnerId = Int64.Parse(partnerId);
                }
                this.Culture = (string)htblPartner[Constants.CULTURE];
                object[] objDBParams = { PartnerId, PartnerName, Culture };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNERS, objDBParams);
                ds.Tables[0].TableName = "Partner";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.LookupSearch");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.LookupSearch - sReqult :" + sReqult.ToString());
               

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.LookupSearch - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.LookupSearch - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.LookupSearch");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return sReqult;
        }

        #endregion

        #region View Txn Reasons,Add PP Transaction
        /// <summary>
        /// To get the Collection Periods
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public String ViewTxnReason(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            rowCount = 0;
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewTxnReason");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.ViewTxnReason - conditionXml :" + conditionXml.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TXNREASONS);
                ds.Tables[0].TableName = "TxnReasons";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewTxnReason");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewTxnReason - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewTxnReason - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewTxnReason - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewTxnReason");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region Add PP Transaction
        /// <summary>
        /// Add PP Transaction
        /// </summary>
        /// <param name="objectXml">PP Txn details</param>/// 
        /// <returns></returns>
        public bool AddPPTxn(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.AddPPTxn");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.AddPPTxn - objectXml :" + objectXml.ToString());
                Hashtable htblPPTxnDetails = ConvertXmlHash.XMLToHashTable(objectXml, "PPTxnDetails");
                this.ClubcardID = Convert.ToInt64(htblPPTxnDetails[Constants.CLUBCARD_ID]);
                this.TescoStoreID = Convert.ToInt16(htblPPTxnDetails[Constants.TESCO_STOREID]);
                this.TxnDate = Convert.ToDateTime(htblPPTxnDetails[Constants.TXN_DATE]);
                this.AmountSpent = Convert.ToDecimal(htblPPTxnDetails[Constants.AMOUNT_SPENT]);
                this.TransactionReasonID = Convert.ToInt64(htblPPTxnDetails[Constants.TXN_REASON_ID]);
                this.WelcomePointsQty = Convert.ToInt64(htblPPTxnDetails[Constants.WELCOME_POINTS_QTY]);
                this.SKUPointsQty = Convert.ToInt64(htblPPTxnDetails[Constants.SKU_POINTS_QTY]);
                this.ManualPointsQty = Convert.ToInt64(htblPPTxnDetails[Constants.MANUAL_POINTS_QTY]);
                this.GreenPointsQty = Convert.ToInt64(htblPPTxnDetails[Constants.GREEN_POINTS_QTY]);
                this.TxnType = Convert.ToInt64(htblPPTxnDetails[Constants.TXN_TYPE]);
                this.SourcePosID = Convert.ToInt64(htblPPTxnDetails[Constants.SOURCE_POS_ID]);
                this.SourceSystemTransactionID = Convert.ToInt64(htblPPTxnDetails[Constants.SOURCE_SYSTEM_TRANSACTIONID]);
                this.PartnerID = Convert.ToInt64(htblPPTxnDetails[Constants.PARTNER_ID]);
                string partnerOutletId = Convert.ToString(htblPPTxnDetails[Constants.PARTNER_OUTLET_ID]);
                if (partnerOutletId != "")
                {
                    this.PartnerOutletID = Convert.ToInt64(partnerOutletId);
                }
                else
                {
                    this.PartnerOutletID = -1;
                }
                object[] objAddPPTxnParams = { 
                                        ClubcardID,
                                        TescoStoreID, 
                                        TxnDate,
                                        AmountSpent,
                                        TransactionReasonID,
                                        WelcomePointsQty,
                                        SKUPointsQty,
                                        ManualPointsQty,
                                        GreenPointsQty,
                                        TxnType,
                                        SourcePosID,
                                        SourceSystemTransactionID,
                                        PartnerID,
                                        PartnerOutletID,
                                        userID
                                     };
                //calls the SP to Update Voucher Barcode
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_PP_TXN, objAddPPTxnParams);

                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.AddPPTxn");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.AddPPTxn");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.AddPPTxn - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.AddPPTxn - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.AddPPTxn");
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

        #region ViewPartnerOutlet
        /// <summary>
        /// To get the details of an Partner
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public String ViewOutlets(long partnerID, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Partner.ViewOutlets");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Partner.ViewOutlets - partnerID :" + partnerID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNEROUTLETS, partnerID);
                ds.Tables[0].TableName = "PartnerOutlet";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Partner.ViewOutlets");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Partner.ViewOutlets - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Partner.ViewOutlets - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Partner.ViewOutlets - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Partner.ViewOutlets");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #endregion
    }
}
