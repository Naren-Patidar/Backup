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
    
    public class StoreGroup
    {
        #region Fields

        private int storeGroupID;
        private string storeGroupName;
        private DateTime insertDateTime;
        private short insertBy;      
        private DateTime amendDateTime;        
        private short amendBy;       
        private char isDelete;      

        #endregion

        #region Properties
        public int StoreGroupID { get { return this.storeGroupID; } set { this.storeGroupID = value; } }
        public string StoreGroupName { get { return this.storeGroupName; } set { this.storeGroupName = value; } }
        public DateTime InsertDateTime { get { return this.insertDateTime; } set { this.insertDateTime = value; } }
        public short InsertBy { get { return this.insertBy; } set { this.insertBy = value; } }
        public DateTime AmendDateTime { get { return this.amendDateTime; } set { this.amendDateTime = value; } }
        public short AmendBy { get { return this.amendBy; } set { this.amendBy = value; } }
        public char IsDelete { get { return this.isDelete; } set { this.isDelete = value; } }

               
        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public StoreGroup()
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
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.Search - conditionXml :" + conditionXml.ToString());
                
                Hashtable htblStoreGroup = ConvertXmlHash.XMLToHashTable(conditionXml, "StoreGroup");              
                DataSet dsStores = new DataSet();
                dsStores = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_STOREGROUPS, culture);

                viewXml = dsStores.GetXml();
                NGCTrace.NGCTrace.TraceInfo("Emd:LoyaltyEntityService.StoreGroup.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.Search - viewXml :" + viewXml.ToString());
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }

        #endregion

        #region DELETE StoreGroup
        /// <summary>
        /// To delete the Role membership of the  User 
        /// </summary>
        /// <param name="objectXml">User Role details</param>/// 
        /// <param name="userID">Current Session</param>/// 
        public bool Delete(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
           
            objectID = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.Delete");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.Delete - objectXml :" + objectXml.ToString());
                Hashtable htblStoreGroup = ConvertXmlHash.XMLToHashTable(objectXml, "StoreGroup");
                this.StoreGroupID = Convert.ToInt32(htblStoreGroup[Constants.STOREGROUP_ID]);
                //this.UserID = Convert.ToInt32(htblAppUser[Constants.USER_ID]);
                //short roleID = Convert.ToInt16(htblAppUser[Constants.USER_ROLE_ID]);
                this.AmendBy = sessionUserID;

                object[] objStoreGroup = { StoreGroupID, AmendBy };
                objectID = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_STOREGROUP, objStoreGroup);
                //objectID = this.UserID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.Delete");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.Delete");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
               
            }
            finally
            {
             
            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region ADD Store Group
        /// <summary>
        /// To Add the StoreGroup details to the database
        /// </summary>

        public bool Add(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.Add - objectXml :" + objectXml.ToString());
                Hashtable htStoregroup = ConvertXmlHash.XMLToHashTable(objectXml, "StoreGroup");
                this.StoreGroupName =htStoregroup[Constants.STOREGROUP_NAME].ToString();                
                

                object[] objAddStoreGroup = { StoreGroupName, userID};
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_STOREGROUP, objAddStoreGroup);
                if (objectId != 0)
                {
                    int count = htStoregroup.Count - 1;
                    int storegroupid =Convert.ToInt32( FindStoreGroupId(StoreGroupName));
                    for (int j = 0; j < count; j++)
                    {
                        int TescoStoreID =Convert.ToInt32( htStoregroup["Group" + j.ToString()]);
                        AddGroupStore(TescoStoreID, storegroupid, userID);
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.Add");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                
                return false;
            }
            finally
            {
            
            }
            return SqlHelper.result.Flag;
            //return true;

        }
        #endregion

        #region Update Store Group
        /// <summary>
        /// To Add the StoreGroup details to the database
        /// </summary>

        public bool Update(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.Update - objectXml :" + objectXml.ToString());
                Hashtable htStoregroup = ConvertXmlHash.XMLToHashTable(objectXml, "StoreGroup");
                this.StoreGroupName = htStoregroup[Constants.STOREGROUP_NAME].ToString();


                //object[] objAddStoreGroup = { StoreGroupName, userID };
                //objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_STOREGROUP, objAddStoreGroup);
                //if (objectId != 0)
                //{
                    int count = htStoregroup.Count - 1;
                    int storegroupid = Convert.ToInt32(FindStoreGroupId(StoreGroupName));
                    DeleteGroupStores(storegroupid,userID);
                    for (int j = 0; j < count; j++)
                    {
                        int TescoStoreID = Convert.ToInt32(htStoregroup["Group" + j.ToString()]);
                        object[] objUpdateStoreGroup = { TescoStoreID, storegroupid, userID };
                        objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_STOREGROUP, objUpdateStoreGroup);
                        //AddGroupStore(TescoStoreID, storegroupid, userID);
                    }
               // }
                    NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.Update");
                    NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.Update");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                return false;
            }
            finally
            {
               
            }
            return SqlHelper.result.Flag;
            //return true;

        }
        #endregion


        #region AddGroupStore
        
        private bool AddGroupStore(int TescoStoreID, int storegroupid,int userId)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.AddGroupStore");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.AddGroupStore - TescoStoreID :" + TescoStoreID.ToString() + "- storegroupid :" + storegroupid.ToString() + "- userid :" + userId.ToString());
                string sSql = null;

                sSql = sSql + "insert into GroupStore values( " + storegroupid + ", " + TescoStoreID + ",getdate()," + userId + ",getdate()," + userId + ",'N');";
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sSql);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.AddGroupStore");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.AddGroupStore");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.AddGroupStore - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.AddGroupStore - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.AddGroupStore");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
               
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region FindStoreGroupId
      
        private string FindStoreGroupId(string StoreGroupName)
        {
            string Id="";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.FindStoreGroupId");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.FindStoreGroupId - StoreGroupName :" + StoreGroupName.ToString());
                SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand("SELECT StoreGroupID FROM StoreGroup WHERE StoreGroupName='" + StoreGroupName + "'", connection);
                    command.Parameters.AddWithValue("@StoreGroupName", StoreGroupName);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.SelectCommand = command;
                    connection.Open();
                    Object result = command.ExecuteScalar();
                    Id = String.Format("{0}", result);
                    NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.FindStoreGroupId");
                    NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.FindStoreGroupId - Id :" + Id.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.FindStoreGroupId - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.FindStoreGroupId - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.FindStoreGroupId");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally
            {

            }
                 return Id;      
           
        }
        #endregion

        #region Delete Group Stores

        private bool DeleteGroupStores(int storegroupid, int userId)
        {
            string sSql = null;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.StoreGroup.DeleteGroupStores");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.StoreGroup.DeleteGroupStores - storegroupid :" + storegroupid.ToString() + "- userId :" + userId.ToString());
                sSql = sSql + "update GroupStore set IsDeleted= 'Y',AmendDateTime = getdate(),AmendBy=" + userId + " where StoreGroupID = " + storegroupid;
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sSql);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.StoreGroup.DeleteGroupStores");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.StoreGroup.DeleteGroupStores");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.StoreGroup.DeleteGroupStores - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.StoreGroup.DeleteGroupStores - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.StoreGroup.DeleteGroupStores");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

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

      