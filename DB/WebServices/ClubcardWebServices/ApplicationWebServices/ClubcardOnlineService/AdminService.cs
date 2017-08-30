using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.NGC.Security;

using USLoyaltySecurityServiceLayer;
using Tesco.NGC.Utils;
using System.Web.Security;
using NGCTrace;

namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the class name "AdminService" here, you must also update the reference to "AdminService" in App.config.
    public class AdminService : IAdminService
    {
        Customer customerObject = null;
        USLoyaltySecurityServiceLayer.SecurityService objSecurityService;

        #region AddRole-Group
        public bool AddRole(string objectXml, int userID, out long objectId, out string resultXml)
        {
            resultXml = string.Empty;
            objectId = 0;

            Role RoleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRole userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRole  userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                RoleObject = new Role();

                //Add Role
                RoleObject.AddRole(objectXml, userID, out objectId, out resultXml);

                bResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRole userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRole  userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRole userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRole userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRole");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                RoleObject = null;
            }

            return bResult;
        }
        #endregion

        #region  ADD AddRoleCapability
        public bool AddRoleCapability(string objectXml, short userID, out long objectId, out string resultXml)
        {
            resultXml = string.Empty;
            objectId = 0;

            Role RoleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                RoleObject = new Role();

                //Add Role
                RoleObject.AddRoleCapabilityCSC(objectXml, userID, out objectId, out resultXml);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                RoleObject = null;

            }

            return bResult;
        }
        #endregion
        #region  ADD Role Membership

        public bool AddRoleMembership(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            objectId = 0;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRoleMembership objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRoleMembership  objectXml" + objectXml + " resultXml" + resultXml);
                AppUserObject = new ApplicationUser();
                AppUserObject.AddRoleMembershipCSC(objectXml, Convert.ToInt16(sessionUserID), out objectId, out resultXml);
                bResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRoleMembership objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRoleMembership  objectXml" + objectXml + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRoleMembership objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRoleMembership objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                AppUserObject = null;

            }

            return bResult;
        }
        #endregion

        #region Delete Role MemberShip
        public bool DeleteRoleMembership(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {
            resultXml = string.Empty;
            objectId = 0;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.DeleteRoleMembership objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.DeleteRoleMembership  objectXml" + objectXml + " resultXml" + resultXml);
                AppUserObject = new ApplicationUser();
                AppUserObject.DeleteRoleMembership(objectXml, Convert.ToInt16(sessionUserID), out objectId, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.DeleteRoleMembership objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.DeleteRoleMembership  objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.DeleteRoleMembership objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.DeleteRoleMembershipobjectXml objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.DeleteRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                AppUserObject = null;

            }

            return bResult;
        }

        #endregion

        #region Get Capability Details
        /// <summary>
        /// Gets the Groups.
        /// </summary>
        /// <returns></returns>
        public bool GetCapabilty(string insertXml, out string errorXml, out string resultXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" + insertXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCapabilty  insertXml" + insertXml + " resultXml" + resultXml);
                roleObject = new Role();

                //Get Customer Personal details
                resultXml = roleObject.GetCapabilty(insertXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" + insertXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCapabilty  insertXml" + insertXml + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" + insertXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" + insertXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCapabilty");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                roleObject = null;

            }

            return bResult;
        }

        #endregion

        #region Delete Role MemberShip
        public bool RemoveCapability(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {
            resultXml = string.Empty;
            objectId = 0;

            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.RemoveCapability objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.RemoveCapability  objectXml" + objectXml + " resultXml" + resultXml);
                roleObject = new Role();
                roleObject.RemoveCapability(objectXml, sessionUserID, out objectId, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.RemoveCapability objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.RemoveCapability  objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.RemoveCapability objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.RemoveCapability objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.RemoveCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                roleObject = null;

            }

            return bResult;
        }

        #endregion

        #region check whether user is admin or not

        /// <summary>
        /// Method to check whether user is admin or not.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsAdminUser(string userName)
        {
            //Declare a boolean variable and set to false.
            bool isAdmin = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.IsAdminUser  userName" + userName);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.IsAdminUser userName" + userName);
                //Initialize Customer object.
                customerObject = new Customer();

                //Call Get coupon method.
                isAdmin = customerObject.IsAdminUser(userName);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.IsAdminUser  userName" + userName);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.IsAdminUser userName" + userName);

            }
            catch (Exception ex)
            {
                //Log the exception.
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.IsAdminUser  userName" + userName + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.IsAdminUser userName" + userName + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.IsAdminUser ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //Return false.
                isAdmin = false;
            }
            finally
            {
                //Set object to null.
                customerObject = null;

            }
            //Return true or false.
            return isAdmin;
        }

        #endregion

        #region SearchRole
        public bool SearchRole(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.SearchRole conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.SearchRole conditionXml" + conditionXml + "  resultXml" + resultXml);
                roleObject = new Role();

                //Get Customer Personal details

                resultXml = roleObject.Search(conditionXml, maxRowCount, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.SearchRole conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.SearchRole conditionXml" + conditionXml + "  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.SearchRole conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.SearchRole oconditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.SearchRole");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region  Update Role Details

        public bool UpdateRoleDetails(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            objectId = 0;

            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateRoleDetails objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateRoleDetails  objectXml" + objectXml + " resultXml" + resultXml);
                roleObject = new Role();

                //Get Customer Personal details
                roleObject.UpdateRoleDetails(objectXml, Convert.ToInt16(sessionUserID), out objectId, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateRoleDetails objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateRoleDetails  objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateRoleDetails objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateRoleDetails objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateRoleDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                roleObject = null;

            }

            return bResult;
        }
        #endregion

        #region Get Role Capability

        public bool ViewRoleCapability(long RoleID, out string errorXml, out string resultXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "  resultXml" + resultXml);
                roleObject = new Role();

                //Get Customer Personal details
                resultXml = roleObject.ViewRoleCapability(RoleID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ViewRoleCapability RoleID" + RoleID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ViewRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                roleObject = null;

            }

            return bResult;
        }

        #endregion
        #region Get Role MemberShip

        public bool ViewRoleMembership(long userID, out string errorXml, out string resultXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + " resultXml" + resultXml);
                AppUserObject = new ApplicationUser();

                //Get Customer Personal details
                resultXml = AppUserObject.ViewRoleMembership(userID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ViewRoleMembership userID" + userID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ViewRoleMembership");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                AppUserObject = null;
            }

            return bResult;
        }

        #endregion

        #region PromotionCode
       // public bool GetPromotionCode(out string errorXml, out string resultXml)
        public bool GetPromotionCode(out string errorXml, out string resultXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            Role roleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCapabilty insertXml"  + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCapabilty  insertXml" + " resultXml" + resultXml);
                roleObject = new Role();

                //Get Customer Personal details
                resultXml = roleObject.GetPromotionCode();

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCapabilty insertXml"  + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCapabilty  insertXml"  + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCapabilty insertXml" +  "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCapabilty");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                roleObject = null;

            }

            return bResult;
        }



        public bool AddPromotionCode(string objectXml, short userID, out string resultXml)
        {
            resultXml = string.Empty;
      

            Role RoleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                RoleObject = new Role();

                //Add Role
                RoleObject.AddPromotionCode(objectXml, userID, out resultXml);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                RoleObject = null;

            }

            return bResult;
        }

        public bool UpdatePromotionCodePC(string objectXml, short userID, out string resultXml)
        {
            resultXml = string.Empty;


            Role RoleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                RoleObject = new Role();

                //Add Role
                RoleObject.UpdatePromotioncodeAgPC(objectXml, userID, out resultXml);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                RoleObject = null;

            }

            return bResult;
        }



        public bool UpdatePromotionCode(string objectXml, short userID, out string resultXml)
        {
            resultXml = string.Empty;
           
            Role RoleObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                RoleObject = new Role();

                //Add Role
                RoleObject.UpdatePromotionCode(objectXml, userID, out resultXml);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddRoleCapability userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddRoleCapability");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                RoleObject = null;

            }

            return bResult;
        }
        #endregion


    }
}
