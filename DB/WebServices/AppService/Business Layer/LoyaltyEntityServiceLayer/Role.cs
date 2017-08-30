using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;

using System.Configuration;

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class Role
    {
        #region Header
        ///
        /// <summary>
        /// Role Details
        /// </summary>
        /// <development>
        ///		<version number="1.00" date="13/Aug/2008">
        ///			<developer>Ramesh</developer>
        ///			<Reviewer></Reviewer>
        ///			<description>Initial Implementation</description>
        ///		</version>
        ///		<version number="1.1" date="13/Aug/2008">
        ///			<developer>Aneesh</developer>
        ///			<Reviewer></Reviewer>
        ///			<description>Complete Implementation</description>
        ///		</version>
        ///	<development>
        ///	
        #endregion

        #region Fields

        /// Tesco User Role Identification Number      
        private short roleID;

        // User Role Name
        private string roleName;

        /// Tesco User Role Desctiption   
        private string roleDesc;

        // Points Add limit
        private int addLimit;

        // Points Subtract limit   
        private int subLimit;

        // Amend By    
        private short amendBy;

        private string promotionCode;
        private string PromoDescription;
        private string startDate;
        private string endDate;




        #endregion

        #region Properties

        /// <summary>
        /// User Role Identification Number
        /// </summary>
        public short RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
        }

        /// <summary>
        /// User Role Name
        /// </summary>
        public string RoleName
        {
            get { return this.roleName; }
            set { this.roleName = value; }
        }

        /// <summary>
        /// User Role Description
        /// </summary>
        public string RoleDesc
        {
            get { return this.roleDesc; }
            set { this.roleDesc = value; }
        }

        /// <summary>
        ///  Points Add Limit
        /// </summary>
        public int AddLimit
        {
            get { return this.addLimit; }
            set { this.addLimit = value; }
        }

        /// <summary>
        ///  Points Subtract Limit
        /// </summary>
        public int SubLimit
        {
            get { return this.subLimit; }
            set { this.subLimit = value; }
        }

        /// <summary>
        ///  Amend By
        /// </summary>
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
        public Role()
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

        #region VIEW
        /// <summary>
        /// To View all the Role details
        /// </summary>
        /// <param name="roleID">unique identifier of the Role table</param>/// 
        /// <returns>Role record in xml format</returns>
        public String View(long roleID, string culture)
        {

            DataSet dsRole = new DataSet();
            string viewXml = String.Empty;
            

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.View - roleID :" + roleID.ToString());
               
                dsRole = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_ROLEDETAILS, RoleID, culture);
                dsRole.Tables[0].TableName = "Role";
                viewXml = dsRole.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.View - viewXml :" + viewXml.ToString());
               
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.View");
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
        /// To search the Roles
        /// If the number of records in the resultset is greater than 
        /// the maximum row count then the method returns empty resultset
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Role records in xml format</returns>        

        public string Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsRole = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.Search - conditionXml :" + conditionXml.ToString());
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(conditionXml, "Role");
                this.RoleName = (string)htblAppUser[Constants.ROLE_NAME];
                this.RoleDesc= (string)htblAppUser[Constants.ROLE_DESC];

                //Execute SP to get the Roles
                dsRole = SqlHelper.ExecuteDataset(connectionString, Constants.SP_SEARCH_ROLES, RoleName, RoleDesc, culture);
                dsRole.Tables[0].TableName = "Role";
                rowCount = dsRole.Tables[0].Rows.Count;
                if (rowCount > maxRowCount)
                    viewXml = "";
                else
                    viewXml = dsRole.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.Search - viewXml :" + viewXml.ToString());
               
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region ADD ROLE
        /// <summary>
        /// To Create a new the Role
        /// </summary>

        public bool AddRole(string objectXml, int userID, out long objectId, out string resultXml)
        {
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.AddRole");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.AddRole - objectXml :" + objectXml.ToString());
                
                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "Role");
                this.RoleName = Convert.ToString(htblOffer[Constants.ROLE_NAME]);
                this.RoleDesc = Convert.ToString(htblOffer[Constants.ROLE_DESC]);
                this.AmendBy = Convert.ToInt16(userID);

                object[] objAddRole = { RoleName, RoleDesc, AmendBy, objectId };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_ROLE, objAddRole);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.AddRole");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.AddRole - objectXml");
                
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.AddRole - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.AddRole - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.AddRole");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region VIEW ROLE CAPABILITY
        /// <summary>
        /// To view the Capapbility of a Role
        /// </summary>
        /// <param name="roleID">ID of the Role</param>/// 
        /// <returns>Capability of the Role in xml format</returns>        

        public string ViewRoleCapability(long roleID, string culture)
        {

            DataSet dsCapability = new DataSet();
            DataSet dsPointsLimit = new DataSet();
            DataSet dsLastUpdated = new DataSet();
            
            
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.ViewRoleCapability");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.ViewRoleCapability - roleID :" + roleID.ToString());
                //Execute SP to get the role membership details of the User
                this.RoleID = Convert.ToInt16(roleID);
                dsCapability = SqlHelper.ExecuteDataset(connectionString, "USP_GetRoleCapability", this.RoleID, culture);
                dsCapability.Tables[0].TableName = "RoleCapability";

                dsPointsLimit = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_ROLEDETAILS, RoleID, culture);
                dsPointsLimit.Tables[0].TableName = "PointsLimit";

                dsLastUpdated = SqlHelper.ExecuteDataset(connectionString, "USP_GetLastUpdatedDetailsRole", this.RoleID);
                dsLastUpdated.Tables[0].TableName = "LastUpdated";

                dsCapability.Tables.Add(dsPointsLimit.Tables[0].Copy());
                dsCapability.Tables.Add(dsLastUpdated.Tables[0].Copy());
                viewXml = dsCapability.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.ViewRoleCapability");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.ViewRoleCapability - viewXml :" + viewXml.ToString());
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.ViewRoleCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.ViewRoleCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.ViewRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion
        
        #region ADD ROLE CAPABILITY
        /// <summary>
        /// To Add the Capability to Role
        /// </summary>
        public bool AddRoleCapability(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.AddRoleCapability");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.AddRoleCapability - objectXml :" + objectXml.ToString());
                Hashtable htblCapability = ConvertXmlHash.XMLToHashTable(objectXml, "RoleCapability");
                this.RoleID = Convert.ToInt16(htblCapability[Constants.ROLE_ID]);
                Int16 capabilityID = Convert.ToInt16(htblCapability[Constants.CAPABILITY_ID]);
                this.AmendBy = sessionUserID;
                object[] objCapability = { RoleID, capabilityID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CAPABILITY, objCapability);
                objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.AddRoleCapability");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.AddRoleCapability");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.AddRoleCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.AddRoleCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.AddRoleCapability");
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

        #region ROMOVE ROLE CAPABILITY
        /// <summary>
        /// To Remove the Capability of the Role
        /// </summary>
        public bool RemoveCapability(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.RemoveCapability");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.RemoveCapability - objectXml :" + objectXml.ToString());
                Hashtable htblCapability = ConvertXmlHash.XMLToHashTable(objectXml, "RoleCapability");
                this.RoleID = Convert.ToInt16(htblCapability[Constants.ROLE_ID]);
                short capabilityID = Convert.ToInt16(htblCapability[Constants.CAPABILITY_ID]);
                this.AmendBy = Convert.ToInt16(sessionUserID);

                object[] objCapability = { RoleID, capabilityID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, "USP_DeleteRoleCapability", objCapability);
                objectId = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.RemoveCapability");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.RemoveCapability");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.RemoveCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.RemoveCapability - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.RemoveCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;

        }
        #endregion

        #region UPDATE
        /// <summary>
        /// To update the Role details 
        /// </summary>
        public bool Update(string objectXml, short sessionUserID,out long objectID, out string resultXml)
        {
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.Update - objectXml :" + objectXml.ToString());
                Hashtable htblRole = ConvertXmlHash.XMLToHashTable(objectXml, "Role");
                this.RoleID = Convert.ToInt16(htblRole[Constants.ROLE_ID]);
                this.RoleName = Convert.ToString(htblRole[Constants.ROLE_NAME]);
                this.RoleDesc = Convert.ToString(htblRole[Constants.ROLE_DESC]);
                this.AmendBy = sessionUserID;

                object[] objRole = { RoleID, RoleName, RoleDesc, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_ROLEDETAILS, objRole);
                objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.Update");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
           
            }
            finally
            {
               
            }
            return SqlHelper.result.Flag;
        }
        #endregion            

        #region DELETE
        /// <summary>
        /// To delete the Role by updating its IsDeleted filed 'Y'  
        /// </summary>
        public bool Delete(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.Delete");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.Delete - objectXml :" + objectXml.ToString());
                Hashtable htblRole = ConvertXmlHash.XMLToHashTable(objectXml, "Role");
                this.RoleID = Convert.ToInt16(htblRole[Constants.ROLE_ID]);
                this.AmendBy = sessionUserID;

                object[] objRole = { RoleID,AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETEE_ROLE, objRole);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.Delete");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.Delete");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.Delete - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.Delete");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }
        #endregion            

        #region UPDATE POINTS LIMIT
        /// <summary>
        /// To update the Points limit
        /// </summary>
        public bool UpdatePointsLimit(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.UpdatePointsLimit");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.UpdatePointsLimit - objectXml :" + objectXml.ToString());
                Hashtable htblPartner = ConvertXmlHash.XMLToHashTable(objectXml, "Role");
                this.RoleID = Convert.ToInt16(htblPartner[Constants.ROLE_ID]);
                this.AddLimit = Convert.ToInt32(htblPartner["AddPointsLimit"]);
                this.SubLimit = Convert.ToInt32(htblPartner["SubtractPointsLimit"]);
                this.AmendBy = sessionUserID;
                object[] objRole = { RoleID, AddLimit, SubLimit,AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_POINTS_LIMIT, objRole);
                objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.UpdatePointsLimit");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.UpdatePointsLimit");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.UpdatePointsLimit - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.UpdatePointsLimit - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.UpdatePointsLimit");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
            }
            finally
            {
            }
            //resultXml = SqlHelper.resultXml;
            return SqlHelper.result.Flag;
        }
        #endregion            

        // CSC Requirements- Neeta

        #region Get Groups
        /// <summary>
        /// Get the groups 
        /// </summary>
        /// <returns>RGroups of the User details in xml format</returns>        

        public string GetCapabilty(string objectXml)
        {

            DataSet dsGroups = new DataSet();
        
            string viewXml = String.Empty;
            long RoleID=0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.GetCapabilty");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.GetCapabilty - objectXml :" + objectXml.ToString());
                //Execute SP to get the role membership details of the User
                Hashtable htblCapability = ConvertXmlHash.XMLToHashTable(objectXml, "Capability");
                RoleID = Convert.ToInt16(htblCapability[Constants.ROLE_ID]);
                object[] objCapability = { RoleID };
                dsGroups = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_Capabilities,objCapability);
                dsGroups.Tables[0].TableName = "Capability";
                viewXml = dsGroups.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.GetCapabilty");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.GetCapabilty - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.GetCapabilty - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.GetCapabilty - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.GetCapabilty");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region ADD ROLE CAPABILITY CSC
        /// <summary>
        /// To Add the Capability to Role
        /// </summary>
        public bool AddRoleCapabilityCSC(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.AddRoleCapabilityCSC");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.AddRoleCapabilityCSC - objectXml :" + objectXml.ToString());
                Hashtable htblCapability = ConvertXmlHash.XMLToHashTable(objectXml, "RoleCapability");
                this.RoleID = Convert.ToInt16(htblCapability[Constants.ROLE_ID]);
                Int16 capabilityID = Convert.ToInt16(htblCapability[Constants.CAPABILITY_ID]);
                this.AmendBy = sessionUserID;
                object[] objCapability = { RoleID, capabilityID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CAPABILITY_CSC, objCapability);
                objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.AddRoleCapabilityCSC");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.AddRoleCapabilityCSC");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.AddRoleCapabilityCSC - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.AddRoleCapabilityCSC - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.AddRoleCapabilityCSC");
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

        #region UPDATE ROLE DETAILS CSC
        /// <summary>
        /// To update the Role details 
        /// </summary>
        public bool UpdateRoleDetails(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
           
            resultXml = string.Empty;
            objectID = 0;
            
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.UpdateRoleDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.UpdateRoleDetails - objectXml :" + objectXml.ToString());
                Hashtable htblRole = ConvertXmlHash.XMLToHashTable(objectXml, "Role");
                this.RoleID = Convert.ToInt16(htblRole[Constants.ROLE_ID]);
                this.RoleName = Convert.ToString(htblRole[Constants.ROLE_NAME]);
                this.RoleDesc = Convert.ToString(htblRole[Constants.ROLE_DESC]);
                this.AddLimit = Convert.ToInt32(htblRole[Constants.ROLE_ADD_LIMIT]);
                this.SubLimit = Convert.ToInt32(htblRole[Constants.ROLE_SUB_LIMIT]);
                this.AmendBy = sessionUserID;

                object[] objRole = { RoleID, RoleName, RoleDesc, AddLimit,SubLimit, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_ROLEDETAILS_CSC, objRole);
                objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.UpdateRoleDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.UpdateRoleDetails");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.UpdateRoleDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.UpdateRoleDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.UpdateRoleDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
               
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;
        }
        #endregion            


        #endregion

        #region PromotionCode
        public string GetPromotionCode()
        {

            DataSet dsPromotionCode = new DataSet();

            string viewXml = String.Empty;
           
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.GetPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.GetPromotionCode - objectXml :");
                //Execute SP to get the role membership details of the User
                dsPromotionCode = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GetPromotionCode);
                dsPromotionCode.Tables[0].TableName = "TblPromotionCode";
                viewXml = dsPromotionCode.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.GetPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.GetPromotionCode - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.GetPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.GetPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.GetPromotionCode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }


        public bool AddPromotionCode(string objectXml, int userID,out string resultXml)
        {
           
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.AddPromotionCode - objectXml :" + objectXml.ToString());

                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "TblPromotionCode");
                this.promotionCode = Convert.ToString(htblOffer["PromotionCode"]);
                this.PromoDescription = Convert.ToString(htblOffer["PromotionCodeDescEnglish"]);
                if (htblOffer["StartDate"] != null)
                {
                    this.startDate = Convert.ToDateTime(htblOffer["StartDate"]).ToString();
                }
                if (htblOffer["EndDate"] != null)
                {
                    this.endDate = Convert.ToDateTime(htblOffer["EndDate"]).ToString();
                }
                this.AmendBy = Convert.ToInt16(userID);

                object[] objAddPromotionCode = { promotionCode, PromoDescription, startDate, endDate, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.USP_ADDPromotionCode, objAddPromotionCode);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.AddPromotionCode - objectXml");

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.AddPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.AddPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }

        public bool UpdatePromotioncodeAgPC(string objectXml, int userID, out string resultXml)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.AddPromotionCode - objectXml :" + objectXml.ToString());

                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "TblPromotionCode");
                this.promotionCode = Convert.ToString(htblOffer["PromotionCode"]);
                this.PromoDescription = Convert.ToString(htblOffer["PromotionCodeDescEnglish"]);
                if (htblOffer["StartDate"] != null)
                {
                    this.startDate = Convert.ToDateTime(htblOffer["StartDate"]).ToString();
                }
                if (htblOffer["EndDate"] != null)
                {
                    this.endDate = Convert.ToDateTime(htblOffer["EndDate"]).ToString();
                }
                this.AmendBy = Convert.ToInt16(userID);

                object[] objAddPromotionCode = { promotionCode, PromoDescription, startDate, endDate, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.USP_UpdatePromotionCodePC, objAddPromotionCode);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.AddPromotionCode - objectXml");

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.AddPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.AddPromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.AddPromotionCode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }
       
          public bool UpdatePromotionCode(string objectXml, int userID,out string resultXml)
        {
           
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Role.UpdatePromotionCode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Role.UpdatePromotionCode - objectXml :" + objectXml.ToString());

                Hashtable htblOffer = ConvertXmlHash.XMLToHashTable(objectXml, "TblPromotionCode");
                this.promotionCode = Convert.ToString(htblOffer["PromotionCode"]);
                this.AmendBy = Convert.ToInt16(userID);

                object[] objAddPromotionCode = { promotionCode, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.USP_UpdatePromotionCode, objAddPromotionCode);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Role.UpdatePromotionCode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Role.UpdatePromotionCode - objectXml");

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Role.UpdatePromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Role.UpdatePromotionCode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Role.UpdatePromotionCode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }


        #endregion


    }
}
