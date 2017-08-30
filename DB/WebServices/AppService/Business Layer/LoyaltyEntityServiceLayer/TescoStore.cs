#region using

using System;
using System.Collections.Generic;
using System.Text;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;

using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    #region Header
    ///
    /// <summary>
    /// Partner Details
    /// </summary>
    /// <development>
    ///		<version number="1.00" date="13/Aug/2008">
    ///			<developer>Netra</developer>
    ///			<Reviewer></Reviewer>
    ///			<description>Initial Implementation</description>
    ///		</version>
    ///	<development>
    ///	    /// <development>
    ///		<version number="V3.1.2" date="03/Sep/2009">
    ///			<developer>Netra</developer>
    ///			<Reviewer></Reviewer>
    ///			<description>Modified the method(Add and Update) to add and get storefotmat information</description>
    ///		</version>
    ///	<development>
    ///	
    #endregion

    public class TescoStore
    {
        #region Fields

        private int tescoStoreID;
        private int tescoStoreNumber;
        private string tescoStoreName;
        private int storeWelcomePointsQty;
        private int storeRegionID;
        private int storeFormatID;
        private int serverType;
        private DateTime InsertDateTime;
        private string tescoStoreIDs;

        #endregion

        #region Properties
        public int TescoStoreID { get { return this.tescoStoreID; } set { this.tescoStoreID = value; } }
        public int TescoStoreNumber { get { return this.tescoStoreNumber; } set { this.tescoStoreNumber = value; } }
        public string TescoStoreName { get { return this.tescoStoreName; } set { this.tescoStoreName = value; } }
        public int StoreFormatID { get { return this.storeFormatID; } set { this.storeFormatID = value; } }
        public int StoreWelcomePointsQty { get { return this.storeWelcomePointsQty; } set { this.storeWelcomePointsQty = value; } }
        public int StoreRegionID { get { return this.storeRegionID; } set { this.storeRegionID = value; } }
        public string TescoStoreIDs { get { return this.tescoStoreIDs; } set { this.tescoStoreIDs = value; } }
        
        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public TescoStore()
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

        #region Search
        /// <summary>
        /// To get all the stores in the database
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();
           
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.Search - conditionXml :" + conditionXml.ToString());
                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(conditionXml, "TescoStore");
                //this.UserName = (string)htblAppUser[Constants.USER_NAME];
                //this.UserDescription = (string)htblAppUser[Constants.USER_DESCRIPTION];
                //String roleName = (string)htblAppUser[Constants.USER_ROLE_NAME];
                //Execute SP to get the Users
                DataSet dsStores = new DataSet();
                dsStores = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_STORES, culture);

                viewXml = dsStores.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.Search - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.Search");
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
        /// Add a new Store
        /// </summary>
        /// <param name="objectXml">Store details</param>/// 
        /// <returns>TescoStoreID of the new Store</returns>
        public bool Add(string objectXml, int userId, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            bool success;
            resultXml = string.Empty;
            
                    try
                    {
                        NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.Add");
                        NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.Add - objectXml :" + objectXml.ToString());
                        Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(objectXml, "TescoStore");
                        this.TescoStoreID = Convert.ToInt32(htblTescoStore[Constants.STORE_ID]);
                        this.TescoStoreName = Convert.ToString(htblTescoStore[Constants.STORE_NAME]);
                        this.StoreFormatID = Convert.ToInt16(htblTescoStore[Constants.STORE_FORMAT_ID]);
                        this.StoreWelcomePointsQty = Convert.ToInt16(htblTescoStore[Constants.STORE_WELCOME_POINTS]);
                        this.StoreRegionID = Convert.ToInt16(htblTescoStore[Constants.STORE_REGION_ID]);
                        
                        object[] objTescoStore = { 
                                        TescoStoreID, 
                                        TescoStoreName,
                                        StoreFormatID,
                                        StoreWelcomePointsQty,
                                        StoreRegionID,
                                        userId
                                     };
                        //calls the SP to add new store in TescoStore Table
                        objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_STORE_DETAILS, objTescoStore);
                        success = SqlHelper.result.Flag;

                        Hashtable htblStoreIpAddress = new Hashtable();
                        //To Add the values to TescoStoreIPAddress table, fetching data from the hashtable to another hash table
                        htblStoreIpAddress.Add(0, htblTescoStore[Constants.IP_ADDRESS1]);
                        htblStoreIpAddress.Add(1, htblTescoStore[Constants.IP_ADDRESS2]);
                        htblStoreIpAddress.Add(2, htblTescoStore[Constants.IP_ADDRESS3]);
                        htblStoreIpAddress.Add(3, htblTescoStore[Constants.IP_ADDRESS4]);
                        htblStoreIpAddress.Add(4, htblTescoStore[Constants.IP_ADDRESS5]);
                        htblStoreIpAddress.Add(5, htblTescoStore[Constants.IP_ADDRESS6]);

                        Hashtable htblStorePort = new Hashtable();
                        htblStorePort.Add(0, null);
                        htblStorePort.Add(1, null);
                        htblStorePort.Add(2, htblTescoStore[Constants.PORT1]);
                        htblStorePort.Add(3, htblTescoStore[Constants.PORT2]);
                        htblStorePort.Add(4, htblTescoStore[Constants.PORT3]);
                        htblStorePort.Add(5, htblTescoStore[Constants.PORT4]);
                        
                        //Method to add the data to the TescoStoreIpAddress  table
                        success = AddIpPortData(htblStoreIpAddress, htblStorePort, TescoStoreID, userId, out resultXml );
                        NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.Add");
                        NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.Add");
                    }
                    catch (Exception ex)
                    {
                        NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.Add - Error Message :" + ex.ToString());
                        NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.Add - Error Message :" + ex.ToString());
                        NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.Add");
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
        /// Edit a existing  Store
        /// </summary>
        /// <param name="objectXml">Store details</param>/// 
        /// <returns>TescoStoreID of the Store</returns>
        public bool Update(string objectXml, int userId, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;

                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(objectXml, "TescoStore");
                this.TescoStoreID = Convert.ToInt32(htblTescoStore[Constants.STORE_ID]);
                this.TescoStoreName = Convert.ToString(htblTescoStore[Constants.STORE_NAME]);                
                this.StoreFormatID = Convert.ToInt16(htblTescoStore[Constants.STORE_FORMAT_ID]);
                this.StoreWelcomePointsQty = Convert.ToInt16(htblTescoStore[Constants.STORE_WELCOME_POINTS]);
                this.StoreRegionID = Convert.ToInt16(htblTescoStore[Constants.STORE_REGION_ID]);

                object[] objTescoStore = { 
                                        TescoStoreID,
                                        TescoStoreName,
                                        StoreFormatID,
                                        StoreWelcomePointsQty,
                                        StoreRegionID,
                                        userId
                                        
                                     };               
                using (SqlTransaction transaction = SqlHelper.BeginTrans())
                {
                    try
                    {
                        NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.Update");
                        NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.Update - objectXml :" + objectXml.ToString());
                        //To update the TescoStore Table
                        SqlHelper.ExecuteNonQuery(transaction, Constants.SP_UPDATE_STORE_DETAILS, objTescoStore);
                        objectId = TescoStoreID;

                        //Hashtable htblStoreIpAddress = new Hashtable();
                        //    //To update the values to TescoStoreIPAddress table, fetching data from the hashtable to another hash table;
                        //htblStoreIpAddress.Add(0, htblTescoStore[Constants.IP_ADDRESS1]);
                        //htblStoreIpAddress.Add(1, htblTescoStore[Constants.IP_ADDRESS2]);
                        //htblStoreIpAddress.Add(2, htblTescoStore[Constants.IP_ADDRESS3]);
                        //htblStoreIpAddress.Add(3, htblTescoStore[Constants.IP_ADDRESS4]);
                        //htblStoreIpAddress.Add(4, htblTescoStore[Constants.IP_ADDRESS5]);
                        //htblStoreIpAddress.Add(5, htblTescoStore[Constants.IP_ADDRESS6]);

                        //Hashtable htblStorePort = new Hashtable();
                        //htblStorePort.Add(0, null);
                        //htblStorePort.Add(1, null);
                        //htblStorePort.Add(2, htblTescoStore[Constants.PORT1]);
                        //htblStorePort.Add(3, htblTescoStore[Constants.PORT2]);
                        //htblStorePort.Add(4, htblTescoStore[Constants.PORT3]);
                        //htblStorePort.Add(5, htblTescoStore[Constants.PORT4]);
                        //if (Convert.ToString(htblStorePort[4]) == "")
                        //{
                        //    htblStorePort.Remove(4);
                        //    htblStorePort.Add(4, null);
                        //}
                        //if (Convert.ToString(htblStorePort[5]) == "")
                        //{
                        //    htblStorePort.Remove(5);
                        //    htblStorePort.Add(5, null);
                        //}

                        ////Method to update the data to the TescoStoreIpAddress  table
                        //UpdateIpPortData(transaction, htblStoreIpAddress, htblStorePort, TescoStoreID, userId);

                        SqlHelper.CommitTrans(transaction);
                        NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.Update");
                        NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.Update");
                    }
                    catch (Exception ex)
                    {
                        NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.Update - Error Message :" + ex.ToString());
                        NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.Update - Error Message :" + ex.ToString());
                        NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.Update");
                        NGCTrace.NGCTrace.ExeptionHandling(ex);
                        resultXml = SqlHelper.resultXml;
                        SqlHelper.RollbackTrans(transaction);
                        
                        return false;
                    }
                    finally
                    {
                       
                    }
                    return true;
                }
             
        }
        
        #endregion

       // Method to add the data to the StoreIpAddress  table
        #region AddIPAddress
        /// <summary>
        /// Add IP Addresses
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="hashIP"></param>
        /// <param name="hashPort"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        private bool AddIpPortData( Hashtable hashIP, Hashtable hashPort, int TescoStoreID, int userId,out string resultXml)
        {
            resultXml = string.Empty;         
            string sSql = null;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.AddIpPortData");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.AddIpPortData - TescoStoreID :" + TescoStoreID.ToString() + "- userid :" + userId.ToString());
                for (int i = 0; i <= 5; i++)
                {
                    if (i <= 1)
                        sSql = sSql + "insert into StoreIPAddress values( " + TescoStoreID + ",'" + Convert.ToString(hashIP[i]) + "','0','POS','" + Convert.ToInt32(hashPort[i]) + "',getdate()," + userId + ",getdate()," + userId + ",'N');";
                    else
                        sSql = sSql + "insert into StoreIPAddress values( " + TescoStoreID + ",'" + Convert.ToString(hashIP[i]) + "','0','NGC','" + Convert.ToString(hashPort[i]) + "',getdate()," + userId + ",getdate()," + userId + ",'N');";

                }
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sSql);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.AddIpPortData");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.AddIpPortData");

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.AddIpPortData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.AddIpPortData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.AddIpPortData");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                RollBack(TescoStoreID);
                return false;
            }
            finally { }
            return true;
        }
        #endregion

        #region RollBack
        private void RollBack(int TescoStoreID)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.RollBack");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.RollBack - TescoStoreID :" + TescoStoreID.ToString());
                string dSql;
                dSql = "delete from TescoStore where TescoStoreID=" + TescoStoreID + "";
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, dSql);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.RollBack");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.RollBack");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.RollBack - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.RollBack - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.RollBack");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
               
            }
            finally { }
        }

        #endregion

        //Method to update the data to the StoreIpAddress  table
        #region UpdateIPPort
        /// <summary>
        /// update ip addresses
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="hashIP"></param>
        /// <param name="hashPort"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        private bool UpdateIpPortData(SqlTransaction transaction,Hashtable hashIP, Hashtable hashPort, int TescoStoreID, int userId)
        {
            string sSql = null;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.UpdateIpPortData");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.UpdateIpPortData - TescoStoreID :" + TescoStoreID.ToString() + "- userid :" + userId.ToString());
                sSql = "delete from StoreIPAddress where TescoStoreID=" + TescoStoreID + ";";
                for (int i = 0; i <= 5; i++)
                {
                    if (i <= 1)
                        sSql = sSql + "insert into StoreIPAddress values( " + TescoStoreID + ",'" + Convert.ToString(hashIP[i]) + "','0','POS'," + Convert.ToInt32(hashPort[i]) + ",getdate()," + userId + ",getdate()," + userId + ",'N');";
                    else
                        sSql = sSql + "insert into StoreIPAddress values( " + TescoStoreID + ",'" + Convert.ToString(hashIP[i]) + "','0','NGC'," + Convert.ToInt32(hashPort[i]) + ",getdate()," + userId + ",getdate()," + userId + ",'N');";
                }
                SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, sSql);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.UpdateIpPortData");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.UpdateIpPortData");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.UpdateIpPortData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.UpdateIpPortData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.UpdateIpPortData");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally { }
            return SqlHelper.result.Flag;
        }
             

        #endregion

        #region ViewRegion
        /// <summary>
        /// To get all the stores in the database
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string ViewStoreRegion(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();
          
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.ViewStoreRegion");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.ViewStoreRegion - conditionXml :" + conditionXml.ToString());
                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(conditionXml, "TescoStore");
                DataSet dsStores = new DataSet();
                dsStores = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_STOREREGION);

                viewXml = dsStores.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.ViewStoreRegion");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.ViewStoreRegion - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.ViewStoreRegion - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.ViewStoreRegion - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.ViewStoreRegion");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                
            }
            return viewXml;
        }

        #endregion

        #region ViewStoreFormat
        /// <summary>
        /// To get all the stores in the database
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string ViewStoreFormat(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();
                     string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.ViewStoreFormat");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.ViewStoreFormat - conditionXml :" + conditionXml.ToString());
                Hashtable htblTescoStore = ConvertXmlHash.XMLToHashTable(conditionXml, "TescoStore");
                DataSet dsStores = new DataSet();
                dsStores = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_STOREFORMAT);

                viewXml = dsStores.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.ViewStoreFormat");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.ViewStoreFormat - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.ViewStoreFormat - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.ViewStoreFormat - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.ViewStoreFormat");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  
            }
            finally
            {
               
            }
            return viewXml;
        }

        #endregion

        #region Get store name
        /// <summary>
        /// This method gets all the store name(s) for store number(s) passed as input parameter.
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string GetStoreName(string storeNumbers, int maxRowCount, out int rowCount, string culture)
        {
            DataSet dsStoreNames;

            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.TescoStore.GetStoreName");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.TescoStore.GetStoreName - conditionXml :" + storeNumbers.ToString());
                
                object[] objRewardParams = { 
                                        storeNumbers,
                                        culture
                                     };

                dsStoreNames = new DataSet();
                dsStoreNames = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_STORENAME, objRewardParams);

                viewXml = dsStoreNames.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.TescoStore.GetStoreName");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.TescoStore.GetStoreName - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.TescoStore.GetStoreName - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.TescoStore.GetStoreName - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.TescoStore.GetStoreName");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                dsStoreNames = null;
            }

            return viewXml;
        }
        #endregion

        #endregion

    }
}        

      