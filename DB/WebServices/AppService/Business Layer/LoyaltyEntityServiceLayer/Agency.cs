using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;
using System.Configuration;

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{

    #region Header

    /// <summary>
    /// Agency Details
    /// </summary>
    /// <development>
    ///           <version number="1.00" date="13/Aug/2008">
    ///                 <developer>Netra</developer>
    ///                 <Reviewer></Reviewer>
    ///                 <description>Initial Implementation</description>
    ///           </version>
    ///     <development>

    #endregion

    public class Agency
    {

        #region Fields

        private string agencyID;
        private string agencyName;
        private string contactName;
        private string agencyBusinessRegNo;
        private string agencyZipPassword;
        private int agencyStatus;
        private string agencyRegisteredBy;
        private int agencyLastBatchSeqNo;
        private int tescoStoreID;
        private DateTime startDate;
        private DateTime endDate;
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
        
        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Agency()
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


        #endregion

        #region Properties

        public string AgencyID { get { return agencyID; } set { agencyID = value; } }
        public string AgencyName { get { return agencyName; } set { agencyName = value; } }
        public string ContactName { get { return contactName; } set { contactName = value; } }
        public string AgencyBusinessRegNo { get { return agencyBusinessRegNo; } set { agencyBusinessRegNo = value; } }
        public string AgencyZipPassword { get { return agencyZipPassword; } set { agencyZipPassword = value; } }
        public int AgencyStatus { get { return agencyStatus; } set { agencyStatus = value; } }
        public string AgencyRegisteredBy { get { return agencyRegisteredBy; } set { agencyRegisteredBy = value; } }
        public int AgencyLastBatchSeqNo { get { return agencyLastBatchSeqNo; } set { agencyLastBatchSeqNo = value; } }
        public int TescoStoreID { get { return tescoStoreID; } set { tescoStoreID = value; } }
        public DateTime StartDate { get { return startDate; } set { startDate = value; } }
        public DateTime EndDate { get { return endDate; } set { endDate = value; } }
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

        #endregion

        #region Methods

        #region View

        /// <summary>
        /// To get the details of an Agency
        /// </summary>
        /// <param name="agencyID">unique identifier of the Agency table</param>/// 
        /// <returns>Agency record in xml format</returns>
        public String View(Int64 agencyID, string culture)
        {

           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Agency.View - agencyID :"+agencyID);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Agency.View - agencyID :" + agencyID);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_AGENCY, agencyID, culture);
                ds.Tables[0].TableName = "Agency";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Agency.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Agency.View - viewXml :" + viewXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Agency.View - Error Message :" + ex.ToString() + " - agencyID :" + agencyID);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Agency.View - Error Message :" + ex.ToString() + " - agencyID :" + agencyID);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Agency.View");
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
        /// To search the agencies according to the given search criteria
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Agency records in xml format</returns>
        public String Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
           
            DataSet ds = new DataSet();
            Int16 PartnerType = 0;
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Agency.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Agency.Search - conditionXml :" + conditionXml.ToString());
                Hashtable htblAgency = ConvertXmlHash.XMLToHashTable(conditionXml, "agency");
                this.AgencyName = (string)htblAgency[Constants.AGENCY_NAME];
                this.AgencyID = (string)htblAgency[Constants.AGENCY_ID];
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Agency.Search - agencyID :" + this.AgencyID);
                this.PostCode = (string)htblAgency["AddressLine1"];
                if (htblAgency[Constants.AGENCY_STATUS] != null)
                {
                    this.AgencyStatus = Convert.ToInt16(htblAgency[Constants.AGENCY_STATUS].ToString());
                }
                string sortBy = (string)htblAgency[Constants.AGENCY_SORT_BY];
                string sortOrder = (string)htblAgency[Constants.AGENCY_SORT_ORDER];

                if (htblAgency["PartnerType"] != null)
                {
                    PartnerType = Convert.ToInt16(htblAgency["PartnerType"]);
                }

                
                object[] objAgency = { AgencyID, AgencyName, PostCode, PartnerType, AgencyStatus, sortBy, sortOrder };//, maxRowCount, rowCount };

                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_SEARCH_AGENCY_AND_PARTNER, objAgency);
                ds.Tables[0].TableName = "Partner";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Agency.Search ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Agency.Search - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Agency.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Agency.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Agency.Search");
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
        /// <returns>True or False </returns>
        public bool Add(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            
            int identityParam = 0;
            resultXml = string.Empty;
            objectID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Agency.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Agency.Add - objectXml" + objectXml.ToString());
                Hashtable htblAgency = ConvertXmlHash.XMLToHashTable(objectXml, "agency");

                this.AgencyID = (string)htblAgency[Constants.AGENCYID];
                this.AgencyName = (string)htblAgency[Constants.AGENCY_NAME];
                this.ContactName = (string)htblAgency[Constants.AGENCY_CONTACT_NAME];
                this.AgencyBusinessRegNo = (string)htblAgency[Constants.AGENCY_BUSINESS_REG_NUMBER];
                this.AgencyStatus = Convert.ToInt16(htblAgency[Constants.AGENCY_STATUS]);
                this.AgencyRegisteredBy = (string)htblAgency[Constants.AGENCY_REGISTERED_BY];
                this.TescoStoreID = Convert.ToInt16(htblAgency[Constants.AGENCY_ASSOCIATED_STORE].ToString());

                if (htblAgency[Constants.AGENCY_START_DATE] != null)
                {
                    this.StartDate = Convert.ToDateTime(htblAgency[Constants.AGENCY_START_DATE]);
                }

                this.AddressLine1 = (string)htblAgency["AddressLine1"];
                this.AddressLine2 = (string)htblAgency["AddressLine2"];
                this.PostCode = (string)htblAgency[Constants.AGENCY_POSTAL_CODE];
                this.PhoneNumber = (string)htblAgency[Constants.AGENCY_PHONE];
                this.FaxNumber = (string)htblAgency[Constants.AGENCY_FAX];
                this.EmailAddress = (string)htblAgency["EmailAddress"];
                this.WebsiteURL = (string)htblAgency[Constants.AGENCY_WEBSITE];


                /*  NOTE : By Harshal --> There are no user screen(textboxes) for the following fileds. So comment it since these fields are 
                    nullable in the database
                 
                 this.AgencyZipPassword = (string)htblAgency[Constants.AGENCY_ZIP_PASSWORD];                
                 this.AgencyLastBatchSeqNo = Convert.ToInt32(htblAgency[Constants.AGENCY_LAST_BATCH_SEQ_NO].ToString());
                 if (htblAgency[Constants.AGENCY_END_DATE].ToString() != "")
                 {
                     this.EndDate = Convert.ToDateTime(htblAgency[Constants.AGENCY_END_DATE]);
                 }                
                 this.AddressLine3 = (string)htblAgency[Constants.AGENCY_ADDRESS3];
                 this.AddressLine4 = (string)htblAgency[Constants.AGENCY_ADDRESS4];
                 this.AddressLine5 = (string)htblAgency[Constants.AGENCY_ADDRESS5];
                 this.AddressLine6 = (string)htblAgency[Constants.AGENCY_ADDRESS6];
                 */


               

                object[] objAgency = { AgencyID, AgencyName, ContactName, AgencyBusinessRegNo, AgencyZipPassword, AgencyStatus, 
                                       AgencyRegisteredBy, AgencyLastBatchSeqNo, TescoStoreID, StartDate,
                                       null, //EndDate
                                       AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5, AddressLine6,
                                       PostCode, PhoneNumber, FaxNumber, EmailAddress, WebsiteURL, sessionUserID, identityParam };

                objectID = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_Agency, objAgency);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Agency.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Agency.Add");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Agency.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Agency.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Agency.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
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
        /// Update agency details
        /// </summary>
        /// <param name="objectXml">Partner details</param>/// 
        /// <returns>True or False</returns>
        public bool Update(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            
            int identityParam = 0;
            resultXml = string.Empty;
            objectID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Agency.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Agency.Update - objectXml :" + objectXml.ToString());
                Hashtable htblAgency = ConvertXmlHash.XMLToHashTable(objectXml, "agency");

                this.AgencyID = (string)htblAgency[Constants.AGENCY_NUMBER];
                this.AgencyName = (string)htblAgency[Constants.AGENCY_NAME];
                this.ContactName = (string)htblAgency[Constants.AGENCY_CONTACT_NAME];
                this.AgencyBusinessRegNo = (string)htblAgency[Constants.AGENCY_BUSINESS_REG_NUMBER];
                this.AgencyZipPassword = (string)htblAgency[Constants.AGENCY_ZIP_PASSWORD];
                if (htblAgency[Constants.AGENCY_STATUS] != null)
                {
                    this.AgencyStatus = Convert.ToInt16(htblAgency[Constants.AGENCY_STATUS]);
                }
                this.AgencyRegisteredBy = (string)htblAgency[Constants.AGENCY_REGISTERED_BY];
                //                this.AgencyLastBatchSeqNo = Convert.ToInt32(htblAgency[Constants.AGENCY_LAST_BATCH_SEQ_NO].ToString());
                this.TescoStoreID = Convert.ToInt32(htblAgency[Constants.AGENCY_ASSOCIATED_STORE].ToString());

                if (htblAgency[Constants.AGENCY_START_DATE] != null)
                {
                    this.StartDate = Convert.ToDateTime(htblAgency[Constants.AGENCY_START_DATE]);
                }
                if (htblAgency[Constants.AGENCY_END_DATE] != null)
                {
                    this.EndDate = Convert.ToDateTime(htblAgency[Constants.AGENCY_END_DATE]);
                }

                this.AddressLine1 = (string)htblAgency[Constants.AGENCY_ADDRESS1];
                this.AddressLine2 = (string)htblAgency[Constants.AGENCY_ADDRESS2];
                this.AddressLine3 = (string)htblAgency[Constants.AGENCY_ADDRESS3];
                this.AddressLine4 = (string)htblAgency[Constants.AGENCY_ADDRESS4];
                this.AddressLine5 = (string)htblAgency[Constants.AGENCY_ADDRESS5];
                this.AddressLine6 = (string)htblAgency[Constants.AGENCY_ADDRESS6];
                this.PostCode = (string)htblAgency[Constants.AGENCY_POSTAL_CODE];
                this.PhoneNumber = (string)htblAgency[Constants.AGENCY_PHONE];
                this.FaxNumber = (string)htblAgency[Constants.AGENCY_FAX];
                this.EmailAddress = (string)htblAgency[Constants.AGENCY_EMAIL];
                this.WebsiteURL = (string)htblAgency[Constants.AGENCY_WEBSITE];

                

                object[] objAgency = { AgencyID, AgencyName, ContactName, AgencyBusinessRegNo, AgencyZipPassword, AgencyStatus, 
                                       AgencyRegisteredBy, AgencyLastBatchSeqNo, TescoStoreID, StartDate, EndDate, 
                                       AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5, AddressLine6,
                                       PostCode, PhoneNumber, FaxNumber, EmailAddress, WebsiteURL, sessionUserID };

                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_AGENCY, objAgency);
                objectID = Convert.ToInt64(this.AgencyID);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Agency.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Agency.Update");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Agency.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Agency.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Agency.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
               
            }

            return SqlHelper.result.Flag;
        }

        #endregion

        #endregion
    }
}


