using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;

using System;
using System.Configuration;

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class ApplicationUser
    {
        #region Header
        ///
        /// <summary>
        /// Application User Details
        /// </summary>
        /// <development>
        ///		<version number="1.00" date="13/Aug/2008">
        ///			<developer>Ramesh</developer>
        ///			<Reviewer></Reviewer>
        ///			<description>Initial Implementation</description>
        ///		</version>
        ///	<development>
        ///	
        #endregion

        #region Fields

        /// Tesco User Identification Number        
        private int userID;

        /// Tesco User Name        
        private string userName;

        /// Tesco User Email Address
        private string emailAddress;

        /// Tesco User Password        
        private string password;

        /// Tesco User Description        
        private string description;

        /// Tesco User Status Code
        private short userStatusCode;

        /// Tesco ISO Language Code
        private char isoLanguageCode;

        /// Last updated by
        private short amendBy;

        

        #endregion

        #region Properties

        /// <summary>
        /// User Identification Number
        /// </summary>
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        /// <summary>
        /// User UserName 
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }


        /// <summary>
        /// User User Password
        /// </summary>
        public string UserPassword
        {
            get { return this.password; }
            set { this.password = value; }
        }


        /// <summary>
        /// User User Description
        /// </summary>
        public string UserDescription
        {
            get { return this.description; }
            set { this.description = value; }
        }
        /// <summary>
        /// User User Status Code
        /// </summary>
        public short UserStatusCode
        {
            get { return this.userStatusCode; }
            set { this.userStatusCode = value; }
        }
        /// <summary>
        /// User ISO Language Code
        /// </summary>
        public char ISOLanguageCode
        {
            get { return this.isoLanguageCode; }
            set { this.isoLanguageCode = value; }
        }
        /// <summary>
        ///  Last updated by 
        /// </summary>
        public short AmendBy
        {
            get { return this.amendBy; }
            set { this.amendBy = value; }
        }

        /// <summary>
        ///  Get EmailAddress
        /// </summary>
        public string EmailAddress
        {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }
        #endregion

         //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ApplicationUser()
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
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="objectXml">User details</param>/// 
        /// <returns>UserID of the new User</returns>
        public bool Add(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {

            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.Add - objectXml" + objectXml.ToString());

                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                this.UserName = (string)htblAppUser[Constants.USER_NAME];
                this.UserDescription = (string)htblAppUser[Constants.USER_DESCRIPTION];
                this.UserStatusCode = Convert.ToInt16(htblAppUser[Constants.USER_STATUS]);
                this.AmendBy = Convert.ToInt16(sessionUserID);
                this.EmailAddress = (string)htblAppUser[Constants.USER_EMAILID];

                object[] objAppUser = { UserName, UserDescription, UserStatusCode, AmendBy, EmailAddress, objectId };
                // object[] objAppUser = { UserName, UserDescription, UserStatusCode, AmendBy,  objectId };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_APPUSER, objAppUser);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.Add");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region SEARCH
        /// <summary>
        /// To search the Application Users
        /// If the number of records in the resultset is greater than 
        /// the maximum row count then the method returns empty resultset
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>ApplicationUser records in xml format</returns>        

        public string Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsAppUsers = new DataSet();

            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.Search - conditionXml" + conditionXml.ToString());
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(conditionXml, "ApplicationUser");
                this.UserName = (string)htblAppUser[Constants.USER_NAME];
                this.UserDescription = (string)htblAppUser[Constants.USER_DESCRIPTION];
                String roleName = (string)htblAppUser[Constants.USER_ROLE_NAME];
                //Execute SP to get the Users
                dsAppUsers = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_APPUSERS, UserName, UserDescription, roleName, culture);
                rowCount = dsAppUsers.Tables[0].Rows.Count;
                if (rowCount > maxRowCount)
                    viewXml = "";
                else
                    viewXml = dsAppUsers.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.Search - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// To update the Application User details 
        /// </summary>
        /// <param name="objectXml">Partner details</param>/// 
        /// <param name="currentSession">Current Session</param>/// 
        public bool Update(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            objectID = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.Update - objectXml:" + objectXml.ToString());
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                this.UserID = Convert.ToInt16(htblAppUser[Constants.USER_ID]);
                this.UserName = (string)htblAppUser[Constants.USER_NAME];
                this.UserDescription = (string)htblAppUser[Constants.USER_DESCRIPTION];
                this.UserStatusCode = Convert.ToInt16(htblAppUser[Constants.USER_STATUS]);
                this.AmendBy = sessionUserID;
                this.EmailAddress = (string)htblAppUser[Constants.USER_EMAILID];

                object[] objAppUser = { UserID, UserName, UserDescription, UserStatusCode, AmendBy, EmailAddress };
                // object[] objAppUser = { UserID, UserName, UserDescription, UserStatusCode, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_APPUSER_DETAILS, objAppUser);
                objectID = this.UserID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.Update");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region DELETE ROLE MEMBERSHIP
        /// <summary>
        /// To delete the Role membership of the  User 
        /// </summary>
        /// <param name="objectXml">User Role details</param>/// 
        /// <param name="userID">Current Session</param>/// 
        public bool DeleteRoleMembership(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            objectID = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership - objectXml :" + objectXml.ToString());
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                this.UserID = Convert.ToInt32(htblAppUser[Constants.USER_ID]);
                short roleID = Convert.ToInt16(htblAppUser[Constants.USER_ROLE_ID]);
                this.AmendBy = sessionUserID;

                object[] objAppUser = { UserID, roleID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_ROLE_MEMBERSHIP, objAppUser);
                objectID = this.UserID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.DeleteRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region VIEW ROLE MEMBERSHIP
        /// <summary>
        /// To view the Role membership of the User
        /// </summary>
        /// <param name="userID">User ID of the User</param>/// 
        /// <returns>Role membership of the User details in xml format</returns>        

        public string ViewRoleMembership(long userID, string culture)
        {

            DataSet dsLastUpdated = new DataSet();
            DataSet dsUserRole = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.ViewRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.ViewRoleMembership - userID :" + userID.ToString());
                //Execute SP to get the role membership details of the User
                this.UserID = (int)userID;
                dsUserRole = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_ROLE_MEMBERSHIP, this.UserID, culture);
                dsUserRole.Tables[0].TableName = "ApplicationUser";

                dsLastUpdated = SqlHelper.ExecuteDataset(connectionString, "USP_GetLastUpdatedDetailsUserRole", this.UserID);
                dsLastUpdated.Tables[0].TableName = "LastUpdated";

                dsUserRole.Tables.Add(dsLastUpdated.Tables[0].Copy());
                viewXml = dsUserRole.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.ViewRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.ViewRoleMembership - viewXml:" + viewXml.ToString());
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.ViewRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.ViewRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.ViewRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region ADD ROLE MEMBERSHIP
        /// <summary>
        /// To add role membership to the User
        /// </summary>
        /// <param name="userID">User ID of the User</param>/// 
        /// <param name="roleID">Role ID of the User</param>/// 
        /// <returns>UserID</returns>        

        public bool AddRoleMembership(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {


            resultXml = string.Empty;
            objectID = -1;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.AddRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.AddRoleMembership - objectXml :" + objectXml.ToString());
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                this.UserID = Convert.ToInt32(htblAppUser[Constants.USER_ID]);
                short roleID = Convert.ToInt16(htblAppUser[Constants.USER_ROLE_ID]);
                this.AmendBy = sessionUserID;

                object[] objAppUser = { UserID, roleID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_ROLE_MEMBERSHIP, objAppUser);
                objectID = this.UserID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.AddRoleMembership");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.AddRoleMembership");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.AddRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.AddRoleMembership - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.AddRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region View
        /// <summary>
        /// To view the details of an Application User
        /// </summary>
        /// <param name="partnerID">unique identifier of the User table</param>/// 
        /// <returns>User record in xml format</returns>
        public String View(long userID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.View - userID:" + userID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_APP_USER, userID, culture);
                ds.Tables[0].TableName = "ApplicationUser";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.View - viewXml:" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        //Get the Connection string -- specifically done for UK
        #region View
        /// <summary>
        /// To view the details of an Application User
        /// </summary>
        /// <param name="partnerID">unique identifier of the User table</param>/// 
        /// <returns>User record in xml format</returns>
        public String GetConnectionString(long userID, string culture)
        {

            string connectionString = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.GetConnectionString");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.GetConnectionString - userID :" + userID.ToString());
                connectionString = "<ConnectionString>" + connectionString + "</ConnectionString>";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.GetConnectionString");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.GetConnectionString - connectionString :" + connectionString.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.GetConnectionString - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.GetConnectionString - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.GetConnectionString");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return connectionString;
        }
        #endregion

        //Get the Connection string -- specifically done for UK
        #region View
        /// <summary>
        /// To view the details of an Application User
        /// </summary>
        /// <param name="partnerID">unique identifier of the User table</param>/// 
        /// <returns>User record in xml format</returns>
        public String GetQNConnectionString(long userID, string culture)
        {

            string connectionString = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.GetQNConnectionString");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.GetQNConnectionString - userID :" + userID.ToString());
                connectionString = ConfigurationSettings.AppSettings["QNConnectionString"].ToString();
                connectionString = "<QNConnectionString>" + connectionString + "</QNConnectionString>";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.GetQNConnectionString");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.GetQNConnectionString - connectionString" + connectionString.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.GetQNConnectionString - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.GetQNConnectionString - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.GetQNConnectionString");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return connectionString;
        }
        #endregion

        #endregion


        // CSC Requirements- Neeta

        #region Get Groups
        /// <summary>
        /// Get the groups 
        /// </summary>
        /// <returns>RGroups of the User details in xml format</returns>        

        public string GetGroupDetails(string insertXml)
        {

            DataSet dsGroups = new DataSet();
            string viewXml = String.Empty;
            long UserID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.GetGroupDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.GetGroupDetails - insertXml : " + insertXml);
                //Execute SP to get the role membership details of the User
                Hashtable htblCapability = ConvertXmlHash.XMLToHashTable(insertXml, "Capability");
                UserID = Convert.ToInt16(htblCapability[Constants.USER_ID]);
                object[] objCapability = { UserID };
                dsGroups = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_GROUPS, objCapability);
                dsGroups.Tables[0].TableName = "ROLE";
                viewXml = dsGroups.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.GetGroupDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.GetGroupDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.GetGroupDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.GetGroupDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.GetGroupDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region ADD ROLE MEMBERSHIP CSC
        /// <summary>
        /// To add role membership to the User
        /// </summary>
        /// <param name="userID">User ID of the User</param>/// 
        /// <param name="roleID">Role ID of the User</param>/// 
        /// <returns>UserID</returns>        

        public bool AddRoleMembershipCSC(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {


            resultXml = string.Empty;
            objectID = -1;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC - objectXml : " + objectXml);
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                this.UserID = Convert.ToInt32(htblAppUser[Constants.USER_ID]);
                short roleID = Convert.ToInt16(htblAppUser[Constants.USER_ROLE_ID]);
                this.AmendBy = sessionUserID;

                object[] objAppUser = { UserID, roleID, AmendBy };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_ROLE_MEMBERSHIP_CSC, objAppUser);
                objectID = this.UserID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ApplicationUser.AddRoleMembershipCSC");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion
    }
}
