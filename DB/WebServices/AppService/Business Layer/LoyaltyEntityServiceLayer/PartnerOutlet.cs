/*
 * File   : PartnerOutlet.cs
 * Author : Netra VK (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Partner Outlet
 * Date   : 26/Aug/2008
 * 
 */
#region using

using System;
using System.Collections;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using System.Configuration;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    /// <summary>
    /// Partner Outlet Details
    /// </summary>
    public class PartnerOutlet
    {

        #region Fields
                
        /// <summary>
        /// CompanyID
        /// </summary>
        private Int64 partnerID;

        /// <summary>
        /// PartnerOutletID
        /// </summary>
        private Int32 partnerOutletNumber;

        /// <summary>
        /// PartnerOutletName
        /// </summary>
        private string partnerOutletName;

        /// <summary>
        /// PartnerOutletRef
        /// </summary>
        private string partnerOutletRef;

        private short amendBy;


        #endregion

        #region Properties
                
        /// <summary>
        ///  CompanyID
        /// </summary>
        public Int64 PartnerID { get { return this.partnerID; } set { this.partnerID= value; } }

        /// <summary>
        ///  ClubcardID
        /// </summary>
        public Int32 PartnerOutletNumber { get { return this.partnerOutletNumber; } set { this.partnerOutletNumber = value; } }

        /// <summary>
        ///  PartnerOutletName
        /// </summary>
        public string PartnerOutletName { get { return this.partnerOutletName; } set { this.partnerOutletName = value; } }

        /// <summary>
        ///  PartnerOutletRef
        /// </summary>
        public string PartnerOutletRef { get { return this.partnerOutletRef; } set { this.partnerOutletRef = value; } }

        public short AmendBy
        {
            get { return this.amendBy; }
            set { this.amendBy = value; }
        }

        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public PartnerOutlet()
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

        #region Add
        /// <summary>To add new PartnerOutlet</summary>
        /// <param name="objectXml">PartnerOutlet details</param>/// 
        /// <returns>Number of records inserted</returns>
        public bool Add(string objectXml, int userId, out long objectId, out string resultXml)
        {
            
            bool success;
            resultXml = string.Empty;
            objectId = 0;

            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.PartnerOutlet.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.PartnerOutlet.Add - objectXml :" + objectXml.ToString());
                Hashtable htblOutlet = ConvertXmlHash.XMLToHashTable(objectXml, "PartnerOutlet");
                this.PartnerID = Convert.ToInt64(htblOutlet["PartnerID"]);
                this.PartnerOutletNumber = Convert.ToInt32(htblOutlet[Constants.XML_TAG_PARTNER_OUTLET_NUMBER]);
                this.PartnerOutletName = Convert.ToString(htblOutlet[Constants.XML_TAG_PARTNER_OUTLET_NAME]);
                this.PartnerOutletRef = Convert.ToString(htblOutlet[Constants.XML_TAG_PARTNER_OUTLET_REFERENCE]);

                object[] objAdd = { PartnerID, PartnerOutletNumber, PartnerOutletName, PartnerOutletRef, userId };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_PARTNEROUTLET, objAdd);
                success = SqlHelper.result.Flag;

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.PartnerOutlet.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.PartnerOutlet.Add");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.PartnerOutlet.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.PartnerOutlet.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.PartnerOutlet.Add");
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
        /// <summary>To add new PartnerOutlet</summary>
        /// <param name="objectXml">PartnerOutlet details</param>/// 
        /// <returns>Number of records inserted</returns>
        public bool Update(string objectXml, int userId, out long objectId, out string resultXml)
        {
            
            bool success;
            resultXml = string.Empty;
            objectId = 0;
            int PartnerOldNumber = 0;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.PartnerOutlet.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.PartnerOutlet.Update - objectXml :" + objectXml.ToString());
                Hashtable htblOutlet = ConvertXmlHash.XMLToHashTable(objectXml, "PartnerOutlet");
                this.PartnerID = Convert.ToInt64(htblOutlet["PartnerID"]);
                this.PartnerOutletNumber = Convert.ToInt32(htblOutlet["partner_outlet_number"]);
                this.PartnerOutletName = Convert.ToString(htblOutlet["partner_outlet_name"]);
                this.PartnerOutletRef = Convert.ToString(htblOutlet["partner_outlet_reference"]);
                PartnerOldNumber = Convert.ToInt32(htblOutlet["OldPartnerOutNumber"]);

                object[] objUpdate = { PartnerID, PartnerOutletNumber, PartnerOutletName, PartnerOutletRef, userId, PartnerOldNumber };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_PARTNEROUTLET, objUpdate);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.PartnerOutlet.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.PartnerOutlet.Update");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.PartnerOutlet.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.PartnerOutlet.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.PartnerOutlet.Update");
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

        #region View
        /// <summary>
        /// View PartnerOutlet details
        /// </summary>
        /// <param name="string objectXml">PartnerID</param>/// 
        /// <returns>Partner Outlet Recordset based on the Partner ID</returns>


        public string View(long partnerID, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.PartnerOutlet.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.PartnerOutlet.View - partnerID :" + partnerID.ToString());
                //Hashtable htblOutlet = ConvertXmlHash.XMLToHashTable(objectXml, "PartnerOutlet");
                //this.PartnerID = (Int64)htblOutlet[Constants.PARTNER_ID];
                object[] objPartnerView = { partnerID };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_PARTNEROUTLET, objPartnerView);
                ds.Tables[0].TableName = "partner_outlet";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.PartnerOutlet.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.PartnerOutlet.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.PartnerOutlet.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.PartnerOutlet.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.PartnerOutlet.View");
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
        /// To search the PartnerOutlet according to the given search criteria
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>PartnerOutlet records in xml format</returns>        
        public String Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.PartnerOutlet.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.PartnerOutlet.Search - conditionXml :" + conditionXml.ToString());
                
                Hashtable htblOutlet = ConvertXmlHash.XMLToHashTable(conditionXml, "PartnerOutlet");
                this.PartnerID = Convert.ToInt64(htblOutlet["PartnerID"]);
                string PartnerOutletNumber = Convert.ToString(htblOutlet["PartnerOutletNumber"]);
                this.PartnerOutletName = Convert.ToString(htblOutlet["PartnerOutletName"]);
                this.PartnerOutletRef = Convert.ToString(htblOutlet["PartnerOutletReference"]);
                string sortBy = Convert.ToString(htblOutlet["SortBy"]);
                string sortOrder = Convert.ToString(htblOutlet["SortOrder"]);

                object[] objOutlet = { PartnerID,PartnerOutletNumber, PartnerOutletName, PartnerOutletRef, sortBy, sortOrder, maxRowCount };

                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_PARTNEROUTLET, objOutlet);
                ds.Tables[0].TableName = "partner_outlet";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.PartnerOutlet.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.PartnerOutlet.Search - viewXml :" + viewXml.ToString());
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.PartnerOutlet.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.PartnerOutlet.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.PartnerOutlet.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region DELETE PartnerOutlet
        /// <summary>
        /// To delete the Role membership of the  User 
        /// </summary>
        /// <param name="objectXml">User Role details</param>/// 
        /// <param name="userID">Current Session</param>/// 
        public bool Delete(string objectXml, int userId, out long objectID, out string resultXml)
        {
            
            objectID = 0;
            bool success;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.PartnerOutlet.Delete");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.PartnerOutlet.Delete - objectXml :" + objectXml.ToString());
                Hashtable htblOutlet = ConvertXmlHash.XMLToHashTable(objectXml, "PartnerOutlet");
                this.PartnerID = Convert.ToInt64(htblOutlet["PartnerID"]);
                this.PartnerOutletNumber = Convert.ToInt32(htblOutlet["PartnerOutletNumber"]);
                //this.PartnerOutletName = Convert.ToString(htblOutlet[Constants.XML_TAG_PARTNER_OUTLET_NAME]);
                //this.PartnerOutletRef = Convert.ToString(htblOutlet[Constants.XML_TAG_PARTNER_OUTLET_REFERENCE]);
                //this.AmendBy = userId;

                object[] objpartnerOutlet = { PartnerID, PartnerOutletNumber};
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_PATNEROUTLET, objpartnerOutlet);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.PartnerOutlet.Delete");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.PartnerOutlet.Delete - objectXml :" + objectXml.ToString());
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.PartnerOutlet.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.PartnerOutlet.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.PartnerOutlet.Delete");
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
        

   

