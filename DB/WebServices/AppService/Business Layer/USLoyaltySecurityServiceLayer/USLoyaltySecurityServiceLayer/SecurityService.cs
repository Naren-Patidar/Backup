using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.Security;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using NGCTrace;


namespace USLoyaltySecurityServiceLayer
{
    public class SecurityService
    {

        #region global variables

        MembershipUser MUser;
        MembershipCreateStatus createStatus;
        string emailsAddress;
        string password;
        string secretQuestion;
        string secretAns;
        Int64 customerID;
        string userName = string.Empty;
        Security Security = null;

        #endregion
        #region public methods

        /// <summary>
        /// CreateUser - create user 
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="UserStatus"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>

        public bool CreateUser(string objectXml, out string UserStatus)
        {
            NGCTrace.NGCTrace.TraceCritical("Start: USLoyaltySecurityServiceLayer.SecurityService.CreateUser");
            bool success = false;
            UserStatus = string.Empty;
            customerID = 0;
            Security = new Security();


            try
            {
                //muMember = Membership.CreateUser(txtEmailAddres.Text, txtpassword.Text, txtEmailAddres.Text, txtSecretQuestion.Text, txtSecretAns.Text, false, out createStatus);
                Hashtable htblUser = ConvertXmlHash.XMLToHashTable(objectXml, "UserDetails");
                if (htblUser["emailsAddress"] != null)
                    emailsAddress = htblUser["emailsAddress"].ToString();
                if (htblUser["password"] != null)
                    password = htblUser["password"].ToString();
                if (htblUser["secretQuestion"] != null)
                    secretQuestion = htblUser["secretQuestion"].ToString();
                if (htblUser["secretAns"] != null)
                    secretAns = htblUser["secretAns"].ToString();
                if (htblUser["customerId"] != null)
                    customerID = Convert.ToInt64(htblUser["customerId"].ToString());

                MUser = Membership.CreateUser(emailsAddress, password, emailsAddress, secretQuestion, secretAns, false, out createStatus);
                NGCTrace.NGCTrace.TraceCritical("SecurityServiceLayer.CreateUser CreateStatus" + createStatus.ToString());
                NGCTrace.NGCTrace.TraceCritical("SecurityServiceLayer.CreateUser Status Error" + GetErrorMessage(createStatus));

                if (MUser != null)
                {
                    NGCTrace.NGCTrace.TraceCritical("Start: SecurityServiceLayer.CreateUser before insert customerid ");
                    UserStatus = "SUCCESS";
                    success = Security.InsertCustomerID(emailsAddress, customerID);
                    NGCTrace.NGCTrace.TraceCritical("End: SecurityServiceLayer.CreateUser After insert customerid ");
                }
                else
                {
                    NGCTrace.NGCTrace.TraceCritical("Start: SecurityServiceLayer.CreateUser error in creating user");
                    UserStatus = GetErrorMessage(createStatus);
                    success = false;
                }
                NGCTrace.NGCTrace.TraceCritical("End: USLoyaltySecurityServiceLayer.SecurityService.CreateUser");
            }
            catch (Exception ex)
            {
                success = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.CreateUser - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.CreateUser : -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.CreateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return success;
        }

        /// <summary>
        /// CreateUser - create user 
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="UserStatus"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>

        public bool CreateUserCsc(string objectXml, out string UserStatus)
        {
            NGCTrace.NGCTrace.TraceCritical("Start: USLoyaltySecurityServiceLayer.SecurityService.CreateUserCsc");
            bool success = false;
            UserStatus = string.Empty;
            customerID = 0;
            Security = new Security();


            try
            {
                //muMember = Membership.CreateUser(txtEmailAddres.Text, txtpassword.Text, txtEmailAddres.Text, txtSecretQuestion.Text, txtSecretAns.Text, false, out createStatus);
                Hashtable htblUser = ConvertXmlHash.XMLToHashTable(objectXml, "UserDetails");
                if (htblUser["emailsAddress"] != null)
                    emailsAddress = htblUser["emailsAddress"].ToString();
                if (htblUser["password"] != null)
                    password = htblUser["password"].ToString();
                if (htblUser["secretQuestion"] != null)
                    secretQuestion = htblUser["secretQuestion"].ToString();
                if (htblUser["secretAns"] != null)
                    secretAns = htblUser["secretAns"].ToString();
                if (htblUser["customerId"] != null)
                    customerID = Convert.ToInt64(htblUser["customerId"].ToString());

                MUser = Membership.CreateUser(emailsAddress, password, emailsAddress, secretQuestion, secretAns, true, out createStatus);
                if (MUser != null)
                {
                    UserStatus = "SUCCESS";
                    success = Security.InsertCustomerID(emailsAddress, customerID);

                }
                else
                {
                    UserStatus = GetErrorMessage(createStatus);
                    success = false;
                }
                NGCTrace.NGCTrace.TraceCritical("End: USLoyaltySecurityServiceLayer.SecurityService.CreateUserCsc");
            }
            catch (Exception ex)
            {
                success = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.CreateUserCsc - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.CreateUserCsc : -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.CreateUserCsc");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return success;
        }
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public bool ValidateUser(string username, string password)
        {
            bool status = false;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.ValidateUser username" + username);

            try
            {
                status = Membership.ValidateUser(username, password);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ValidateUser username" + username);
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ValidateUser username"+ username +"- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ValidateUser username" + username + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ValidateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            return status;
        }


        public MembershipUser GetUser(string userName)
        {
            MembershipUser ms;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.GetUser");
            try
            {
                ms = Membership.GetUser(userName);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.GetUser");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.GetUser - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.GetUser -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.GetUser");                
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            return ms;

        }

        public void UpdateUser(MembershipUser muser)
        {

            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.UpdateUser");
            try
            {
                Membership.UpdateUser(muser);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.UpdateUser");
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.UpdateUser - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.UpdateUser -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.UpdateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }


        }
        /// <summary>
        /// GetSecretQuestion - used to ge the secret answer for the user
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string GetSecretQuestion(Int64 customerID)
        {
            string passwordQuetion = string.Empty;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.GetSecretQuestion customerID" + customerID);
            try
            {
                string username = GetUserName(customerID);
                MUser = Membership.GetUser(username, false);
                passwordQuetion = MUser.PasswordQuestion;
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.GetSecretQuestion customerID" + customerID);
            }

            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.GetSecretQuestion customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.GetSecretQuestion customerID" + customerID + "  -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.GetSecretQuestion");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return passwordQuetion;

        }
        /// <summary>
        /// Changepassword - used to change the password
        /// </summary>
        /// <param name="custtomerID"></param>
        /// <param name="newPassword"></param>

        public bool Changepassword(Int64 custtomerID, string oldPassword, string newPassword)
        {

            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.Changepassword customerID" + custtomerID);
            bool passQuesstatus = false;
            try
            {
                if (string.IsNullOrEmpty(oldPassword))
                {

                    string userName = GetUserName(custtomerID);

                    string oldPassword1 = Membership.Provider.GetPassword(userName, String.Empty);

                    passQuesstatus = Membership.Provider.ChangePassword(userName, oldPassword1, newPassword);

                    if (passQuesstatus)
                    {

                        passQuesstatus = Membership.Provider.ChangePasswordQuestionAndAnswer(userName, newPassword, "Question", "answer");

                    }

                    NGCTrace.NGCTrace.TraceInfo("end :SecurityServiceLayer Changepassword");
                    return passQuesstatus;
                }
                else
                {
                    string userName = GetUserName(custtomerID);

                    // string oldPassword = Membership.Provider.GetPassword(userName, String.Empty);

                    passQuesstatus = Membership.Provider.ChangePassword(userName, oldPassword, newPassword);

                    if (passQuesstatus)
                    {

                        passQuesstatus = Membership.Provider.ChangePasswordQuestionAndAnswer(userName, newPassword, "Question", "answer");

                    }

                    NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.Changepassword customerID" + custtomerID);
                    return passQuesstatus;
                }
            }

            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.Changepassword customerID" + custtomerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.Changepassword customerID" + custtomerID + "  -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ResetPassword");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;

            }



        }



        /// <summary>
        /// ResetPassword- used to reset the new auto generated passoword 
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="passwordAns"></param>
        /// <returns></returns>
        public string ResetPassword(Int64 customerID, string passwordAns)
        {
            string newPassword;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.ResetPassword customerID" + customerID );
            try
            {
                string userName = GetUserName(customerID);
                newPassword = Membership.Provider.ResetPassword(userName, passwordAns);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ResetPassword customerID" + customerID );
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ResetPassword customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ResetPassword customerID" + customerID + "  -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ResetPassword");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return newPassword;
        }

        public bool ResetPWD(MembershipUser muser, string secretAns)
        {
            bool pwdstatus = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.ResetPWD");
                string userName = muser.UserName;

                string newPassword = Membership.Providers["CustomizedProviderResetPWD"].ResetPassword(userName, secretAns);

                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ResetPWD");
                pwdstatus = true;
            }
            catch (Exception ex)
            {
                pwdstatus = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ResetPWD - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ResetPWD -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ResetPWD");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return pwdstatus;
        }

        /// <summary>
        /// UpdateEmailAddresss - used to update the Email address and username in security framework DB
        /// </summary>
        /// <param name="newUserName"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool UpdateEmailAddresss(string newUserName, Int64 CustomerId)
        {
            bool updatestatus = false;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.UpdateEmailAddresss NewUserName:" + newUserName + " customerID:" + CustomerId);

            try
            {

                Security = new Security();
                updatestatus = Security.UpdateUserName(newUserName, CustomerId);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.UpdateEmailAddresss NewUserName:" + newUserName + " customerID:" + CustomerId);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.UpdateEmailAddresss customerID:" + CustomerId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.UpdateEmailAddresss customerID:" + CustomerId + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.UpdateEmailAddresss");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return updatestatus;
        }


        public bool InsertCustomerID(string UserName, Int64 CustomerId)
        {
            bool updatestatus = false;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.InsertCustomerID UserName:" + UserName + " customerID:" + CustomerId);

            try
            {

                Security = new Security();
                updatestatus = Security.InsertCustomerID(UserName, CustomerId);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.InsertCustomerID UserName:" + UserName + " customerID:" + CustomerId);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.InsertCustomerID customerID:" + CustomerId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.InsertCustomerID customerID:" + CustomerId + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.InsertCustomerID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return updatestatus;
        }
        /// <summary>
        /// GetUserName - used to get the user name from Security framework DB
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GetUserName(Int64 CustomerID)
        {
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.GetUserName customerID:" + customerID);
            try
            {

                Security = new Security();
                userName = Security.GetUserName(CustomerID);
                NGCTrace.NGCTrace.TraceCritical("End :USLoyaltySecurityServiceLayer.SecurityService.GetUserName customerID:" + customerID);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.GetUserName customerID:" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.GetUserName customerID:" + customerID + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.GetUserName");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return userName;
        }

        /// <summary>
        /// GetErrorMessage - get the users login status message
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists.";


                case MembershipCreateStatus.InvalidPassword:
                    return "Invalid Password";

                case MembershipCreateStatus.InvalidEmail:
                    return "Ivalid Email Address";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Invalid password Answer";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Invalid password Question";

                case MembershipCreateStatus.InvalidUserName:
                    return "Invalid UserName";

                case MembershipCreateStatus.ProviderError:
                    return "Provider Error .";

                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return "DuplicateProviderUserKey.";

                case MembershipCreateStatus.InvalidProviderUserKey:
                    return "InvalidProviderUserKey.";

                case MembershipCreateStatus.UserRejected:
                    return "User Creation Rejected";

                case MembershipCreateStatus.Success:
                    return "Success";

                default:
                    return "An unknown error Please try Again.";
            }
        }

        public bool UpdateSecretQns(Int64 customerID, string question, string answer)
        {
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.UpdateSecretQns customerID:" + customerID);
            bool status = false;
            try
            {
                string userName = GetUserName(customerID);
                string oldPassword = Membership.Provider.GetPassword(userName, String.Empty);
                status = Membership.Provider.ChangePasswordQuestionAndAnswer(userName, oldPassword, question, answer);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.UpdateSecretQns customerID:" + customerID);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.UpdateSecretQns customerID:" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.UpdateSecretQns customerID:" + customerID + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.UpdateSecretQns");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                status = false;
                throw ex;
            }

            return status;
        }


        #endregion

        public Int64 GetCustomerID(string emailAddress)
        {

            string passwordQuetion = string.Empty;
            Int64 customerID;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.GetCustomerID UserName:" + emailAddress);
            try
            {
                Security = new Security();
                customerID = Security.GetCutomerID(emailAddress);
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.GetCustomerID UserName:" + emailAddress);
            }


            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.GetCustomerID UserName:" + emailAddress + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.GetCustomerID UserName:" + emailAddress + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.GetCustomerID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return customerID;
        }
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>


        public bool CreateToken(string username, out string resultXml)
        {
            bool status = false;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.CreateToken UserName:" + username);
            DataSet ds = new DataSet();
            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                object[] objDBParams = { username, 1 };
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_CreateToken", objDBParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TokenID";

                }
                resultXml = ds.GetXml();
                status = true;
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.CreateToken UserName:" + username);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.CreateToken UserName:" + username + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.CreateToken UserName:" + username + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.CreateToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                status = false;
                throw ex;

            }

            return status;
        }

        public bool ValidateToken(string tokenId, out string resultXml)
        {
            bool status = false;
            NGCTrace.NGCTrace.TraceInfo("start :USLoyaltySecurityServiceLayer.SecurityService.ValidateToken TokenID:" + tokenId);
            DataSet ds = new DataSet();
            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ValidateToken", tokenId);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TokenID";

                }
                resultXml = ds.GetXml();
                status = true;
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ValidateToken TokenID:" + tokenId);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ValidateToken TokenID:" + tokenId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ValidateToken TokenID:" + tokenId + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ValidateToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                status = false;
                throw ex;

            }

            return status;
        }

        public bool ExpireToken(string tokenid)
        {
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenid);
            bool success = false;
            try
            {


                object[] objRewardParams = { tokenid };

                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                SqlHelper.ExecuteNonQuery(connectionString, "USP_ExpireToken", objRewardParams);
                success = SqlHelper.result.Flag;

                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenid);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenid + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenid + " -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ExpireToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
                throw ex;

            }
            finally
            {

            }
            return success;
        }

        public bool ValidateTokenCustomerID(string tokenId, out string resultXml)
        {
            bool status = false;
            NGCTrace.NGCTrace.TraceInfo("Start :USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenId);
            DataSet ds = new DataSet();
            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ValidateTokenCustomerID", tokenId);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TokenID";

                }
                resultXml = ds.GetXml();
                status = true;
                NGCTrace.NGCTrace.TraceInfo("End :USLoyaltySecurityServiceLayer.SecurityService.ExpireToken TokenID:" + tokenId);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:USLoyaltySecurityServiceLayer.SecurityService.ValidateTokenCustomerID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:USLoyaltySecurityServiceLayer.SecurityService.ValidateTokenCustomerID  -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:USLoyaltySecurityServiceLayer.SecurityService.ValidateTokenCustomerID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                status = false;
                throw ex;

            }

            return status;
        }


    }
}
