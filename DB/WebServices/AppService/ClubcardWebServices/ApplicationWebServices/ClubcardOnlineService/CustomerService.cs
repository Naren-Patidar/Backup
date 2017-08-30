using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Xml;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.NGC.Security;
//using Fujitsu.eCrm.Generic.SharedUtils;
using USLoyaltySecurityServiceLayer;
using Tesco.NGC.Utils;
using System.Web.Security;
using NGCTrace;
using ClubcardOnlineService.ClubcardCouponService;


using Tesco.com.ClubcardOnlineService;
namespace Tesco.com.ClubcardOnlineService
{
    public class CustomerService :ICustomerService
    {
        Customer customerObject = null;
        Preference preferenceObject = null;

        USLoyaltySecurityServiceLayer.SecurityService objSecurityService;

        /// <summary>
        /// GetCustomerDetailst -- It gets the customer details 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of customer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            XmlNode container = null;
            XmlDocument custInfo = null;
            XmlNode imported = null;
            string custResult = string.Empty;
            string familyDetails = string.Empty;
            string preferenceResult = string.Empty;
            DataSet dsFamilyDetails = null;
            Int64 customerID;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;



            try
            {


                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerDetails conditionXml-" + conditionXml + " resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerDetails conditionXml-" + conditionXml + " resultXml-" + resultXml);

                customerObject = new Customer();
                //create object for preferences
                preferenceObject = new Preference();

                doc = new XmlDocument();

                // the other documents will be merged into this one
                // under the Container elementXmlDocument 

                container = doc.CreateElement("CustomerInformation");
                doc.AppendChild(container);

                //Get Customer Personal details
                custResult = customerObject.Search(conditionXml, maxRowCount, out rowCount, culture);

                // first document to mergeXmlDocument                 
                custInfo = new XmlDocument();
                custInfo.LoadXml(custResult);
                imported = doc.ImportNode(custInfo.DocumentElement, true);
                doc.DocumentElement.AppendChild(imported);

                customerID = Convert.ToInt64((custInfo.FirstChild.FirstChild.FirstChild).InnerXml);

                if (customerID != 0)
                {
                    //Get Customer family and preferences details
                    dsFamilyDetails = customerObject.ViewFamilyDetails(customerID, culture);
                    dsFamilyDetails.Tables[0].TableName = "FamilyDetails";

                    //If family memners are nor present in FamilyMember table
                    if ((dsFamilyDetails != null) && (dsFamilyDetails.Tables[0].Rows.Count == 0))
                    {
                        dsFamilyDetails.Tables[1].TableName = "NoOFFamilyMembers";
                    }

                    //Second document to mergeXmlDocument  
                    familyDetails = dsFamilyDetails.GetXml();

                    if (familyDetails != string.Empty)
                    {
                        custInfo.LoadXml(familyDetails);
                        imported = doc.ImportNode(custInfo.DocumentElement, true);
                        doc.DocumentElement.AppendChild(imported);
                    }

                    //Third document to mergeXmlDocument  
                    //preferenceResult = preferenceObject.ViewCustomerPreference(customerID, culture);

                    //if (preferenceResult != string.Empty)
                    //{
                    //    custInfo.LoadXml(preferenceResult);
                    //    imported = doc.ImportNode(custInfo.DocumentElement, true);

                    //    doc.DocumentElement.AppendChild(imported);
                    //}

                }

                resultXml = doc.InnerXml;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerDetails conditionXml-" + conditionXml + " resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerDetails conditionXml-" + conditionXml + " resultXml-" + resultXml);


                return true;
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerDetails  resultXml " + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerDetails   resultXml " + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerDetails ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;

            }
            finally
            {
                doc = null;
                container = null;
                custInfo = null;
                imported = null;
                customerObject = null;
                if (preferenceObject != null)
                {
                    preferenceObject = null;
                }

            }
        }

        /// <summary>
        /// UpdateCustomerDetails -- It is used to update customer details 
        /// </summary>
        /// <param name="updateXml">XML string containing the Customer details to be updated.</param>
        /// <param name="consumer">UserID to updated amendby column in the DB.</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="customerID">customerid</param>
        /// <returns>True if the customer details updated in DB; otherwise, False.</returns>
        public bool UpdateCustomerDetails(string updateXml, string consumer, out string errorXml, out Int64 customerID)
        {
            Customer customerObject = null;
            short userID = 0;
            errorXml = string.Empty;
            customerID = 0;
            bool bResult = false;


            try
            {


                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID-" + customerID + "updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID -" + customerID + "updateXml" + updateXml);

                //Get the AmendBy Id
                userID = Helper.GetConsumerID(consumer);

                customerObject = new Customer();
                customerObject.UpdateCustomerDetails(updateXml, userID, out customerID, out errorXml);

                bResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID-" + customerID + "updateXml" + updateXml);

                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID -" + customerID + "updateXml" + updateXml);

            }
            catch (Exception ex)
            {
                bResult = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID-" + customerID + "updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateCustomerDetails customerID-" + customerID + "updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);


            }
            finally
            {
                customerObject = null;

            }

            return bResult;
        }

        /// <summary>
        /// GetCustomerPreferences--It gets the customer Preference details
        /// </summary>
        /// <param name="customerID">CustomerID, it fetches the corresponding customer preference details.</param>
        /// <param name="culture">Culture</param>
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="resultXml">XML string which contains the list of preferences.</param>
        /// <returns></returns>
        public bool GetCustomerPreferences(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {

            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);


                preferenceObject = new Preference();

                resultXml = preferenceObject.ViewCustomerPreference(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);

                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerPreferences   customerID-" + customerID + "  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerPreferences    customerID-" + customerID + "  resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerPreferences ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (preferenceObject != null)
                {
                    preferenceObject = null;
                }


            }

            return bResult;
        }

        /// <summary>
        /// UpdateCustomerPreferences--Updates customer preferences.
        /// </summary>
        /// <param name="updateXml">XML string containing the preferences to be updated.</param>
        /// <param name="consumer">UserID to updated amendby column in the DB.</param>
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="customerID">CustomerID</param>
        /// <returns></returns>
        public bool UpdateCustomerPreferences(string updateXml, string consumer, out string errorXml, out Int64 customerID, char level)
        {
            preferenceObject = new Preference();
            errorXml = string.Empty;
            bool bResult = false;
            int userID = 0;
            customerID = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                //Get the AmendBy Id
                userID = Helper.GetConsumerID(consumer);
                preferenceObject.UpdateCustomerPreference(updateXml, userID, out customerID, out errorXml, level);

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                bResult = true;
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences  customerID-" + customerID + "  updateXml-" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateCustomerPreferences ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (preferenceObject != null)
                {
                    preferenceObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// SearchCustomer--This method is used for Dundee screen's search customer method.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">Culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string which contains the list of customer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns></returns>
        public bool SearchCustomer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Customer customerObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml);
                customerObject = new Customer();
                //Get Customer Personal details
                resultXml = customerObject.GetCustomersForDundee(conditionXml, maxRowCount, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.SearchCustomer conditionXml-" + conditionXml + "  resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.SearchCustomer ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;

            }

            return bResult;
        }

        /// <summary>
        /// GetTitles--Gets the titles details.
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of title </param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <param name="culture">Culture</param>
        /// <returns></returns>
        public bool GetTitles(out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Customer customerObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml);
                customerObject = new Customer();

                //Get Customer Personal details
                resultXml = customerObject.GetTitles(out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetTitles resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetTitles ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }

        /// <summary>
        /// This method is used to Authenticate Dundee user and find their autherization
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cultureCode"></param>
        /// <param name="AppName"></param>
        /// <param name="LDPath"></param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains details of the authenticated user details</param>
        /// <param name="userID"></param>
        /// <returns>True if the User is authenticate to login; otherwise, False.</returns>
        public bool AuthenticateUser(string domain, string userName, string password, string cultureCode, string AppName, string LDPath, out string errorXml, out string resultXml, out int userID)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            userID = 0;

            Session autherizeObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AuthenticateUser  userName" + userName + "resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AuthenticateUser userName" + userName + "resultXml-" + resultXml);
                autherizeObject = new Session(domain, userName, password, cultureCode, AppName, LDPath);

                //Get Customer Personal details

                resultXml = autherizeObject.CapabilityXml.ToString();

                userID = autherizeObject.UserId;

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AuthenticateUser  userName" + userName + "resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AuthenticateUser userName" + userName + "resultXml-" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AuthenticateUser  userName" + userName + "resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AuthenticateUser userName" + userName + "resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AuthenticateUser ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                autherizeObject = null;

            }

            return bResult;
        }


        #region Methods for Join module of F&E Loyalty application By Mohan

        #region InsertCustomerDetails
        /// <summary>
        /// InsertCustomerDetails -- It is used to insert new customer details from join page 
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any.</param>
        public string InsertCustomerDetails(string insertXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.InsertCustomerDetails  insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.InsertCustomerDetails insertXml" + insertXml);
                customerObject = new Customer();
                string welcomePoints = ConfigurationSettings.AppSettings["WelcomePoints"].ToString();
                viewXml = customerObject.InsertCustomerDetails(insertXml, out errorXml, welcomePoints);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.InsertCustomerDetails  insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.InsertCustomerDetails insertXml" + insertXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.InsertCustomerDetails  insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.InsertCustomerDetails insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.InsertCustomerDetails ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                customerObject = null;

            }

            return viewXml;


        }
        #endregion

        #region GetSecretQtns
        /// <summary>
        /// GetSecretQtns -- It is used to fetch secret questions from database table 
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of secret questions from database table.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <param name="culture">culture</param>
        public bool GetSecretQtns(out string errorXml, out string resultXml, out int rowCount, string culture)
        {

            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetSecretQtns  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetSecretQtns resultXml" + resultXml);
                customerObject = new Customer();
                resultXml = customerObject.GetSecretQtns(out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetSecretQtns  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetSecretQtns resultXml" + resultXml);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetSecretQtns  resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetSecretQtns resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetSecretQtns ");
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

        #region GetExistingCustomerDetails
        /// <summary>
        /// GetExistingCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the existing customer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool GetExistingCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;

            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetExistingCustomerDetails(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetExistingCustomerDetails ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region GetFriendCustomerDetails
        /// <summary>
        /// GetFriendCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool GetFriendCustomerDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetFriendCustomerDetails(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetFriendCustomerDetails ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region UpdateExistingCustomerDetails
        /// <summary>
        /// UpdateExistingCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="updateXml">XML string containing the existing customer details to be updated.</param>
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        public bool UpdateExistingCustomerDetails(string updateXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.UpdateCustomerDetails(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateExistingCustomerDetails ");
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

        #region GetPrimaryCard
        /// <summary>
        /// GetPrimaryCard -- It is used to fetch primary card no of a customer
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the primary clubcard details of a customer</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool GetPrimaryCard(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetPrimaryCard conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetPrimaryCard conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.GetPrimaryCard(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetPrimaryCard conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetPrimaryCard conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetPrimaryCard  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetPrimaryCard conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetPrimaryCard ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region AddPhoneNoToAccount
        /// <summary>
        /// AddPhoneNoToAccount -- It is used to add phone no to a customer's account
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool AddPhoneNoToAccount(string insertXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount insertXml" + insertXml);
                customerObject = new Customer();
                bResult = customerObject.AddPhoneNoToAccount(insertXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount insertXml" + insertXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount  insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddPhoneNoToAccount ");
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

        #region EmailValidation
        /// <summary>
        /// EmailValidation -- It is used to check an email address  already exists in database or not
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the resulted records</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool EmailValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;

            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.EmailValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.EmailValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.EmailValidation(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.EmailValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.EmailValidation conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.EmailValidation  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.EmailValidation conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.EmailValidation ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region PhoneNoValidation
        /// <summary>
        /// PhoneNoValidation -- It is used to check a phone no  already exists in database or not
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool PhoneNoValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;

            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.PhoneNoValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.PhoneNoValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.PhoneNoValidation(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.PhoneNoValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.PhoneNoValidation conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.PhoneNoValidation  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.PhoneNoValidation conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.PhoneNoValidation ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region AlternateIdValidation
        /// <summary>
        /// AlternateIdValidation -- It is used to check an user alredy entered phone no as alternate id or not
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of customer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        public bool AlternateIdValidation(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;

            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AlternateIdValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AlternateIdValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.AlternateIdValidation(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AlternateIdValidation conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AlternateIdValidation conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AlternateIdValidation  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AlternateIdValidation conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AlternateIdValidation ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion

        #region GetCustIdByEmailId
        /// <summary>
        /// GetCustIdByEmailId -- It is used to get custid of a customer providing mailid
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustIdByEmailId(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;

            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustIdByEmailId conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustIdByEmailId conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.GetCustIdByEmailId(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustIdByEmailId conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustIdByEmailId conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustIdByEmailId  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustIdByEmailId conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustIdByEmailId");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }

        #endregion

        #region InsertCardNo
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool InsertCardNo(string insertXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.InsertCardNo insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.InsertCardNo insertXml" + insertXml);
                customerObject = new Customer();
                bResult = customerObject.InsertCardNo(insertXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.InsertCardNo insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.InsertCardNo insertXml" + insertXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.InsertCardNo  insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.InsertCardNo insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.InsertCardNo");
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

        #region GetCardNoByCustId
        /// <summary>
        /// GetCardNoByCustId -- It is used to get cardno by customer id of a customer
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCardNoByCustId(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCardNoByCustId conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCardNoByCustId conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.GetCardNoByCustId(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCardNoByCustId conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCardNoByCustId conditionXml" + conditionXml + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCardNoByCustId  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCardNoByCustId conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCardNoByCustId");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }

        #endregion

        #region GetTransactionHistory
        /// <summary>
        /// GetTransactionHistory -- It is used to get transaction history of a customer by customer id
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetTransactionHistory(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {

            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetTransactionHistory conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetTransactionHistory conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetTransactionHistory(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetTransactionHistory conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetTransactionHistory conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetTransactionHistory  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetTransactionHistory conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetTransactionHistory");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }

            return bResult;
        }
        #endregion
        /// <summary>
        /// CreateUser -- 
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="UserStatus">sets the max limit for resulted rows</param>
        /// <returns>True if the user created successfully; otherwise, False.</returns>
        public bool CreateUser(string objectXml, out string UserStatus)
        {

            bool bResult = false;
            string usercreateStatus = string.Empty;

            UserStatus = string.Empty;
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.CreateUser UserStatus" + UserStatus);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.CreateUser UserStatus" + UserStatus);
                bResult = objSecurityService.CreateUser(objectXml, out usercreateStatus);
                UserStatus = usercreateStatus;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.CreateUser UserStatus" + UserStatus);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.CreateUser UserStatus" + UserStatus);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.CreateUser  UserStatus" + UserStatus + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.CreateUser UserStatus" + UserStatus + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.CreateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {

            }
            return bResult;

        }
        /*Menu*/

        /// <summary>
        /// GetAccountOverviewDetails --  
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of customer details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetAccountOverviewDetails(Int64 customerID, out string errorXml, out string resultXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails customerID" + customerID + "resultXml" + resultXml);
                customerObject = new Customer();
                XmlDocument doc = new XmlDocument();

                resultXml = customerObject.GetAccountOverviewDetails(customerID);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails customerID" + customerID + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails  customerID" + customerID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails customerID" + customerID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetAccountOverviewDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;

            }
            finally
            {

            }

            return bResult;
        }
        /*Menu*/
        #endregion

        #region My Profile- sathesh

        #region GetMyProfileDetails
        /// <summary>
        /// GetExistingCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of Existingcustomer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetMyProfileDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetMyProfileDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetMyProfileDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetMyProfileDetails(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetMyProfileDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetMyProfileDetails conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetMyProfileDetails  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetMyProfileDetails conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetMyProfileDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }
        #endregion

        #region UpdateMyProfileDetails
        /// <summary>
        /// UpdateExistingCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool UpdateMyProfileDetails(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.UpdateMyProfileDetails(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateMyProfileDetails");
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

        #region GetAboutmeDetails

        /// <summary>
        /// GetAboutmeDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="out errorXml">XML string, contains error details if any</param>
        /// <param name="out resultXml">XML string, which contains the existing customer details against customerid</param>
        /// <param name="out rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool GetAboutmeDetails(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetAboutmeDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetAboutmeDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetAboutMeDetails(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetAboutmeDetails conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetAboutmeDetails conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetAboutmeDetails  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetAboutmeDetails conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetAboutmeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }

        #endregion

        #region UpdateAboutMeDetails
        /// <summary>
        /// UpdateAboutMe-- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool UpdateAboutMeDetails(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.UpdateAboutmeDetails(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateAboutMeDetails");
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

        #region UpdateDietaryPreferences
        /// <summary>
        /// UpdateDietaryPreferences -- It is used to update DietaryPreferences of a customer against customerid of a customer 
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool UpdateDietaryPreferences(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.UpdateDietaryPreferences(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateDietaryPreferences");
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

        #region AddSupplementaryCard
        /// <summary>
        /// AddSupplementaryCard -- It is used to add supplimentarycard
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="objectId">objectId</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if Supplementary Card added successfully.; otherwise, False.</returns>
        public bool AddSupplementaryCard(string objectXml, out long objectId, out string errorXml, out string resultXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            resultXml = string.Empty;
            objectId = 0;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddSupplementaryCard objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddSupplementaryCard objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml);
                customerObject = new Customer();
                bResult = customerObject.AddSupplementaryCard(objectXml, out objectId, out resultXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddSupplementaryCard objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddSupplementaryCard objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddSupplementaryCard  objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddSupplementaryCard objectXml" + objectXml + "objectId" + objectId + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddSupplementaryCard");
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

        //#region Change Password


        ///// <summary>
        ///// This method is used to change password
        ///// </summary>
        ///// <param name="customerid"></param>
        ///// <param name="newpassword"></param>
        ///// <returns></returns>
        //public bool Changepassword(Int64 custtomerID, string newPassword)
        //{
        //    USLoyaltySecurityServiceLayer.SecurityService securityObject = null;
        //    bool bResult = false;
        //    try
        //    {
        //        securityObject = new USLoyaltySecurityServiceLayer.SecurityService();
        //        securityObject.Changepassword(custtomerID, newPassword);
        //        bResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "InsertXML:");
        //        bResult = false;
        //    }
        //    finally
        //    {
        //        securityObject = null;
        //    }
        //    return bResult;
        //}
        //#endregion

        #region Change Password

        /// <summary>
        /// Changepassword -- This method is used to change password
        /// </summary>
        /// <param name="custtomerID">custtomerID</param>
        /// <param name="oldPassword">oldPassword</param>
        /// <param name="newPassword">newPassword</param>
        ///<returns>True password changed successfully.; otherwise, False.</returns>

        public bool Changepassword(Int64 custtomerID, string oldPassword, string newPassword)
        {

            USLoyaltySecurityServiceLayer.SecurityService securityObject = null;

            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.Changepassword custtomerID" + custtomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.Changepassword custtomerID" + custtomerID);
                securityObject = new USLoyaltySecurityServiceLayer.SecurityService();

                bResult = securityObject.Changepassword(custtomerID, oldPassword, newPassword);

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.Changepassword custtomerID" + custtomerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.Changepassword custtomerID" + custtomerID);

            }

            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.Changepassword  custtomerID" + custtomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.Changepassword custtomerID" + custtomerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.Changepassword");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                bResult = false;

            }

            finally
            {

                securityObject = null;

            }

            return bResult;

        }

        #endregion

        #region GetSecretQuestion


        /// <summary>
        /// This method is used to get the secret question
        /// </summary>
        ///<param name="custtomerID">custtomerID</param>
        /// <returns>Secret question</returns>
        public string GetSecretQuestion(Int64 custtomerID)
        {
            USLoyaltySecurityServiceLayer.SecurityService securityObject = null;
            string secquestion = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetSecretQuestion custtomerID" + custtomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetSecretQuestion custtomerID" + custtomerID);
                securityObject = new USLoyaltySecurityServiceLayer.SecurityService();
                secquestion = securityObject.GetSecretQuestion(custtomerID);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetSecretQuestion custtomerID" + custtomerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetSecretQuestion custtomerID" + custtomerID);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetSecretQuestion  custtomerID" + custtomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetSecretQuestion custtomerID" + custtomerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetSecretQuestion");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                securityObject = null;
            }
            return secquestion;
        }


        #endregion

        #region UpdateSecretQuestion


        /// <summary>
        /// This method is used to get the secret question
        /// </summary>
        ///<param name="customerID">customerID</param>
        ///<param name="question">question</param>
        ///<param name="answer">answer</param>
        /// <returns>Secret question</returns>
        public bool UpdateSecretQns(Int64 customerID, string question, string answer)
        {
            USLoyaltySecurityServiceLayer.SecurityService securityObject = null;
            bool sResult = false;
            string secquestion = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateSecretQns customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateSecretQns customerID" + customerID);
                securityObject = new USLoyaltySecurityServiceLayer.SecurityService();
                if (securityObject.UpdateSecretQns(customerID, question, answer))
                {
                    sResult = true;
                }
                else
                {
                    sResult = false;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateSecretQns customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateSecretQns customerID" + customerID);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateSecretQns  custtomerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateSecretQns custtomerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateSecretQns");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                securityObject = null;
            }
            return sResult;
        }


        #endregion

        #region UpdateEmailPreferences
        /// <summary>
        /// UpdateEmailPreferences -- It is used to update email preferences of a customer against customerid 
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool UpdateEmailPreferences(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateEmailPreferences updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateEmailPreferences updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.UpdateEmailPreferences(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateEmailPreferences updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateEmailPreferences updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateEmailPreferences  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateEmailPreferences updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateEmailPreferences");
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

        #region GetEmailPreferences
        //USLOYALTY
        /// <summary>
        /// This method is used to get Customer Email Preferences
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of Customer Email Preferences.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        

        public bool GetEmailPreferences(Int64 customerID, out string errorXml, out string resultXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetEmailPreferences customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetEmailPreferences customerID" + customerID + "resultXml" + resultXml);
                customerObject = new Customer();
                XmlDocument doc = new XmlDocument();

                resultXml = customerObject.GetEmailPreferences(customerID);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetEmailPreferences customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetEmailPreferences customerID" + customerID + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetEmailPreferences  customerID" + customerID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetEmailPreferences customerID" + customerID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetEmailPreferences");
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

        #region UpdateEmail
        /// <summary>
        /// UpdateEmail -- It is used to update email of a customer against customerid in the security framework.
        /// </summary>
        /// <param name="newUserName">newUserName</param>
        /// <param name="CustomerId">CustomerId</param>
        public bool UpdateEmailAddresss(string newUserName, Int64 CustomerId)
        {
            USLoyaltySecurityServiceLayer.SecurityService securityObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateEmailAddresss CustomerId" + CustomerId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateEmailAddresss CustomerId" + CustomerId);
                securityObject = new USLoyaltySecurityServiceLayer.SecurityService();
                bResult = securityObject.UpdateEmailAddresss(newUserName, CustomerId);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateEmailAddresss CustomerId" + CustomerId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateEmailAddresss CustomerId" + CustomerId);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateEmailAddresss  CustomerId" + CustomerId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateEmailAddresss CustomerId" + CustomerId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateEmailAddresss");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                securityObject = null;
            }
            return bResult;
        }

        #endregion

        #endregion


        #region My Rewards -Sudhakar

        /// <summary>
        /// This method is used to Get the Customer Rewards From FD
        /// </summary>     
        /// <param name="PrimaryCardNumber">PrimaryCardNumber</param>
        /// <param name="Rewards">Rewards</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustomerRewards(Int64 PrimaryCardNumber, out float Rewards)
        {
            Rewards = 0;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerRewards PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerRewards PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards);
                customerObject = new Customer();
                bResult = customerObject.GetCustomerRewards(PrimaryCardNumber, out Rewards);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerRewards PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerRewards PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerRewards  PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerRewards PrimaryCardNumber" + PrimaryCardNumber + "Rewards" + Rewards + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerRewards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;

            }
            finally
            {

            }

            return bResult;
        }




        /// <summary>
        /// This method is used to Convert the Points to Cash though the FD
        /// </summary>
        /// <param name="PrimaryCardID">PrimaryCardID</param> 
        /// <param name="TotalPoints">TotalPoints</param>
        /// <returns>True if Points converted to cash though the FD; otherwise, False.</returns>

        public bool ConvertPointsToCash(Int64 PrimaryCardID, Int32 TotalPoints)
        {
            customerObject = new Customer();
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ConvertPointsToCash PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ConvertPointsToCash PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints);
                bResult = customerObject.ConvertPointsToCash(PrimaryCardID, TotalPoints);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ConvertPointsToCash PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ConvertPointsToCash PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ConvertPointsToCash  PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ConvertPointsToCash PrimaryCardID" + PrimaryCardID + "TotalPoints" + TotalPoints + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ConvertPointsToCash");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrimaryCardID">PrimaryCardID</param>        
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="resultXml">XML string which contains the list of preferences.</param>
        /// <returns></returns>
        public bool GetPointsExpiryDetails(Int64 PrimaryCardID, out string errorXml, out string resultXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                resultXml = customerObject.GetPointsExpiry(PrimaryCardID);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails  PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetPointsExpiryDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrimaryCardID">PrimaryCardID</param>        
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="resultXml">XML string which contains the list of preferences.</param>
        /// <returns></returns>
        public bool GetRewardExpiryDate(Int64 PrimaryCardID, out string errorXml, out string resultXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetRewardExpiryDate PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetRewardExpiryDate PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                resultXml = customerObject.GetRewardExpiryDate(PrimaryCardID);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetRewardExpiryDate PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetRewardExpiryDate PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetRewardExpiryDate  PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetRewardExpiryDate PrimaryCardID" + PrimaryCardID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetRewardExpiryDate");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }


        #endregion

        #region SignIn
        /// <summary>
        /// UpdateCustomerTermsNConstionsStatus--it updates customer terms and conditions status 
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if status updated; otherwise, False.</returns>
        public bool UpdateCustomerTermsNConstionsStatus(Int64 customerID, out string errorXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus customerID" + customerID);
                bResult = customerObject.UpdateCustomerTermsNConstionsStatus(customerID);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus customerID" + customerID);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus  customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateCustomerTermsNConstionsStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// GetCustomerStatus--it gets 
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the customer status details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustomerStatus(string username, out string errorXml, out string resultXml)
        {
            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerStatus username" + username + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerStatus username" + username + "resultXml" + resultXml);
                resultXml = customerObject.GetCustomerStatus(username);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerStatus username" + username + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerStatus username" + username + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetMyProfileDetails  username" + username + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetMyProfileDetails username" + username + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetMyProfileDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// GetCustomerTNCInfo
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted record details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustomerTNCInfo(string userName, out string errorXml, out string resultXml)
        {

            customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo userName" + userName + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo userName" + userName + "resultXml" + resultXml);
                resultXml = customerObject.GetCustomerStatus(userName);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo userName" + userName + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo userName" + userName + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo  userName" + userName + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo userName" + userName + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerTNCInfo");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;

        }
        /// <summary>
        /// GetCustomerIDSecurityDB--
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="CustomerID">CustomerID</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustomerIDSecurityDB(string userName, out Int64 CustomerID)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();
            CustomerID = 0;

            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB userName" + userName + "CustomerID" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB userName" + userName + "CustomerID" + CustomerID);
                CustomerID = objSecurityService.GetCustomerID(userName);

                bResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB userName" + userName + "CustomerID" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB userName" + userName + "CustomerID" + CustomerID);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB  userName" + userName + "CustomerID" + CustomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB userName" + userName + "CustomerID" + CustomerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerIDSecurityDB");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// ValidateUser--it validate the user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if the user is valid user; otherwise, False.</returns>
        public bool ValidateUser(string username, string password)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();

            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ValidateUser username" + username);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ValidateUser username" + username);
                bResult = objSecurityService.ValidateUser(username, password);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ValidateUser username" + username);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ValidateUser username" + username);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ValidateUser  username" + username + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ValidateUser username" + username + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ValidateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public MembershipUser GetUser(string userName)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();

            MembershipUser muser = null;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetUser userName" + userName);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetUser userName" + userName);
                muser = objSecurityService.GetUser(userName);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetUser userName" + userName);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetUser userName" + userName);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetUser userName" + userName + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetUser userName" + userName + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return muser;
        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="muser"></param>

        public void UpdateUser(MembershipUser muser)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateUser muser" + muser);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateUser muser" + muser);
                objSecurityService.UpdateUser(muser);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateUser muser" + muser);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateUser muser" + muser);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateUser  muser" + muser + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateUser muser" + muser + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateUser");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }



        }

        /// <summary>
        /// Resetpwd--it resets the password 
        /// </summary>
        /// <param name="muser"></param>
        /// <param name="secretans"></param>
        /// <returns>True if password is reset; otherwise, False.</returns>
        public bool Resetpwd(MembershipUser muser, string secretans)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();

            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.Resetpwd muser" + muser);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.Resetpwd muser" + muser);
                bResult = objSecurityService.ResetPWD(muser, secretans);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.Resetpwd muser" + muser);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.Resetpwd muser" + muser);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.Resetpwd  muser" + muser + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.Resetpwd muser" + muser + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.Resetpwd");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;

        }
        /// <summary>
        /// Create token for microsite
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resultXml"></param>
        /// <returns>True if token created for microsite; otherwise, False.</returns>
        public bool CreateToken(string userName, out string resultXml)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();

            bool bResult = false;
            resultXml = string.Empty;
            string RXml = string.Empty;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.CreateToken userName" + userName + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.CreateTokenuserName" + userName + "resultXml" + resultXml);
                bResult = objSecurityService.CreateToken(userName, out RXml);
                resultXml = RXml;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.CreateToken userName" + userName + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.CreateToken userName" + userName + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.CreateToken  userName" + userName + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.CreateToken userName" + userName + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.CreateToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// Validate Token for Customer on click on the link
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId, out string resultXml)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();
            resultXml = string.Empty;
            string RXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ValidateToken tokenId" + tokenId + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ValidateToken tokenId" + tokenId + "resultXml" + resultXml);
                bResult = objSecurityService.ValidateToken(tokenId, out RXml);
                resultXml = RXml;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ValidateToken tokenId" + tokenId + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ValidateToken tokenId" + tokenId + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ValidateToken  tokenId" + tokenId + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ValidateToken tokenId" + tokenId + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ValidateToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// Expire token
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ExpireToken(string tokenId)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();


            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ExpireToken tokenId" + tokenId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ExpireToken tokenId" + tokenId);
                bResult = objSecurityService.ExpireToken(tokenId);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ExpireToken tokenId" + tokenId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ExpireToken tokenId" + tokenId);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ExpireToken  tokenId" + tokenId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ExpireToken tokenId" + tokenId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ExpireToken");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (objSecurityService != null)
                {
                    objSecurityService = null;
                }

            }

            return bResult;
        }
        /// <summary>
        /// SendEmailET--
        /// </summary>
        /// <param name="email"></param>
        /// <param name="linkUrl"></param>
        /// <param name="emailType"></param>
        /// <returns></returns>
        public bool SendEmailET(string email, string linkUrl, string emailType)
        {

            customerObject = new Customer();
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType);
                bResult = customerObject.SendEmail(email, linkUrl, emailType);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.SendEmailET email" + email + "linkUrl" + linkUrl + "emailType" + emailType + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.SendEmailET");
                NGCTrace.NGCTrace.ExeptionHandling(ex); bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }

        #endregion

        #region Reactivate Account -Neeta

        #region GetCustUseMailStatus- Neeta
        /// <summary>
        /// GetCustUseMailStatus -- Get Customer's use mail status 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the  customer mail status details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCustUseMailStatus(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustUseMailStatus conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustUseMailStatus conditionXml" + conditionXml + "resultXml" + resultXml);
                customerObject = new Customer();
                doc = new XmlDocument();
                resultXml = customerObject.GetCustUseMailStatus(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustUseMailStatus conditionXml" + conditionXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustUseMailStatus conditionXml" + conditionXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustUseMailStatus  conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustUseMailStatus conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustUseMailStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }
        #endregion

        #region UpdateMyProfileDetails
        /// <summary>
        /// UpdateMyProfileDetails --To update customer's use and mail status 
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        public bool UpdateUseMailStatus(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateUseMailStatus updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateUseMailStatus updateXml" + updateXml);

                customerObject = new Customer();
                customerObject.UpdateUseMailStatus(updateXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateUseMailStatus updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateUseMailStatus updateXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateUseMailStatus  updateXml" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateUseMailStatus updateXml" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateUseMailStatus");
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

        #endregion

        #region Reward Card Managemnt -- Kavitha

        #region Lost or Stolen or Damaged Card
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="errorMessage">errorMessage</param>
        /// <returns></returns>
        public bool UpdateCardStatus(string insertXml, out string errorXml, out string errorMessage)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            errorMessage = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml);
                customerObject = new Customer();
                bResult = customerObject.UpdateCardStatus(insertXml, out errorXml, out errorMessage);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateCardStatus insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateCardStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;

            }

            return bResult;
        }
        /// <summary>
        /// Test--
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns></returns>
        public bool Test(string insertXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml);
                customerObject = new Customer();
                customerObject.UpdateCardStatusCSC(insertXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.Test insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.Test");
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

        #region GetAllClubcardDetails
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetAllClubcards(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            Customer customerObject = new Customer();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml);
                resultXml = customerObject.GetAllClubcards(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetAllClubcards customerID" + customerID + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetAllClubcards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }
        #endregion

        #region Update Phone Number
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns></returns>
        public bool UpdatePhoneNumber(string insertXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml);
                customerObject = new Customer();
                customerObject.UpdatePhoneNumber(insertXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdatePhoneNumber insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdatePhoneNumber");
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

        #region Update Clubcard ID
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="insertXml"></param>
        /// <param name="errorXml"></param>
        /// <returns></returns>
       
        public bool UpdatePrimaryClubcardID(string insertXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml);
                customerObject = new Customer();
                customerObject.UpdatePrimaryClubcard(insertXml, out errorXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID insertXml" + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdatePrimaryClubcardID");
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

        #endregion

        #region My Coupons -- Noushad

        /// <summary>
        /// Method to get coupons based on customerID.
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="resultXml">XML string, which contains the list of coupons details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCoupons(Int64 customerID, out string resultXml)
        {
            //Set return variable to empty.
            resultXml = string.Empty;

            //Declare a boolean variable and set to false.
            bool result = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCoupons customerID-" + customerID + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCoupons customerID-" + customerID + "  resultXml-" + resultXml);
                //Initialize Customer object.
                customerObject = new Customer();

                //Call Get coupon method.
                resultXml = customerObject.GetCoupons(customerID);

                //Check whether resultxml is empty or null.
                if ((resultXml != null) && (resultXml != "") && (resultXml != "<NewDataSet />"))
                {
                    //Set return variable to true.
                    result = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCoupons customerID-" + customerID + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCoupons customerID-" + customerID + "  resultXml-" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCoupons   customerID-" + customerID + "  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCoupons    customerID-" + customerID + "  resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCoupons ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
                //Set object to null.
                customerObject = null;
            }
            //Return true or false.
            return result;
        }

        /// <summary>
        /// Method to update coupons based on customerID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="couponStatus"></param>
        /// <returns>True if coupon status update; otherwise, False.</returns>
        public Boolean UpdateCoupons(Int64 customerID, string couponStatus)
        {
            //Declare an object of Customer.
            Customer customerObject = null;

            //Declare a boolean variable and set to false.
            bool result = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateCoupons customerID-" + customerID + "  couponStatus" + couponStatus);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateCoupons customerID-" + customerID + "  couponStatus" + couponStatus);
                //Initialize Customer object.
                customerObject = new Customer();

                //Call update coupon method.
                customerObject.UpdateCoupons(customerID, couponStatus);

                //Set return variable to true.
                result = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateCoupons customerID-" + customerID + "  couponStatus" + couponStatus);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateCoupons customerID-" + customerID + "  couponStatus" + couponStatus);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateCoupons   customerID-" + customerID + "couponStatus" + couponStatus + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateCoupons customerID-" + customerID + "couponStatus" + couponStatus + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateCoupons ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //Set return variable to false.
                result = false;
            }
            finally
            {
                //Set object to null.
                customerObject = null;

            }
            //Return true or false.
            return result;
        }



        #endregion


        #region CSC - Neeta

        #region Get Group Details
        /// <summary>
        /// Gets the Groups.
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of group details.</param>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="culture">culture</param>
        /// <returns></returns>
        public bool GetGroupDetails(out string errorXml, out string resultXml, string insertXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "  resultXml" + resultXml);
                AppUserObject = new ApplicationUser();

                //Get Customer Personal details
                resultXml = AppUserObject.GetGroupDetails(insertXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetGroupDetails insertXml" + insertXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetGroupDetails");
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

        #region  ADD USER
        /// <summary>
        /// Add -- To add user to application
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="sessionUserID">sessionUserID</param>
        /// <param name="objectId">objectId</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if user add successfully; otherwise, False.</returns>

        public bool Add(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            objectId = 0;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "  resultXml" + resultXml);
                AppUserObject = new ApplicationUser();

                //Get Customer Personal details
                if (AppUserObject.Add(objectXml, sessionUserID, out objectId, out resultXml))
                {
                    Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "ApplicationUser");
                    htblAppUser["UserID"] = objectId;
                    String NewXml = Helper.HashTableToXML(htblAppUser, "ApplicationUser");
                    string Remove = "Helper.HashTableToXML ObjName: ApplicationUser";
                    if (NewXml.Contains(Remove))
                    {
                        NewXml = NewXml.Remove(0, Remove.Length);
                    }

                    AppUserObject.AddRoleMembership(NewXml, Convert.ToInt16(sessionUserID), out objectId, out resultXml);
                }
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "  resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.Add objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.Add");
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

        #region SearchUSer
        /// <summary>
        /// SearchUSer --gets the user details
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of result details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool SearchUser(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            ApplicationUser appUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "  resultXml" + resultXml);
                appUserObject = new ApplicationUser();

                //Get Customer Personal details

                resultXml = appUserObject.Search(conditionXml, maxRowCount, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.SearchUser conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.SearchUser");
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



        #region Delete Role MemberShip
        /// <summary>
        /// Delete Role MemberShip -- 
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="sessionUserID">sessionUserID</param>
        /// <param name="objectId">objectId</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if role deleted successfully; otherwise, False.</returns>

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


        #region  Update USER

        /// <summary>
        /// GetCustomerDetailst -- It updates existing user details
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="sessionUserID">sessionUserID</param>
        /// <param name="objectId">objectId</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool Update(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            objectId = 0;

            ApplicationUser AppUserObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.Update objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.Update  objectXml" + objectXml + " resultXml" + resultXml);
                AppUserObject = new ApplicationUser();

                //Get Customer Personal details
                AppUserObject.Update(objectXml, Convert.ToInt16(sessionUserID), out objectId, out resultXml);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.Update objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.Update  objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.Update objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.Update objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.Update");
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


















        #region  ADD Customer
        /// <summary>
        /// AddCustomer -- To Add Customer to DB
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="sessionUserID">sessionUserID</param>
        /// <param name="resultXml">XML string, which contains the Result details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if the record add successfully; otherwise, False.</returns>

        public bool AddCustomer(string objectXml, int sessionUserID, out string resultXml, out string errorXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            Customer custObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddCustomer objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddCustomer  objectXml" + objectXml + " resultXml" + resultXml);
                custObject = new Customer();

                //Get Customer Personal details
                int welcomePoints = Convert.ToInt32(ConfigurationSettings.AppSettings["WelcomePoints"].ToString());
                custObject.AddCustomer(objectXml, sessionUserID, out resultXml, welcomePoints);
                bResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddCustomer objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddCustomer  objectXml" + objectXml + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddCustomer objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddCustomer objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                custObject = null;

            }

            return bResult;
        }
        #endregion





        #region GetCustomerID


        /// <summary>
        /// This method is used to get the secret question
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <returns>Secret question</returns>
        public string GetCustomerID(Int64 customerID)
        {
            USLoyaltySecurityServiceLayer.SecurityService securityObject = null;
            string sResult = string.Empty;
            string secquestion = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID);
                securityObject = new USLoyaltySecurityServiceLayer.SecurityService();
                string UserName = securityObject.GetUserName(customerID);
                if (UserName != null || UserName != string.Empty)
                {

                    sResult = UserName;
                }

                else
                {

                    sResult = null;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCustomerID customerID-" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCustomerID ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

                securityObject = null;
            }
            return sResult;
        }


        #endregion

        /// <summary>
        /// GetCardStatus -- It gets the CardStatus
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of Card status details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetCardStatus(out string errorXml, out string resultXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            Customer customerObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml);
                customerObject = new Customer();

                //Get Customer Personal details
                resultXml = customerObject.GetCardStatus();

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetCardStatus resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetCardStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;

            }

            return bResult;
        }


        #region TransactionsByOffer
        /// <summary>
        /// TransactionsByOffer -- To Fetch Transaction Details using offer ID
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list ofTransaction Details</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool TransactionsByOffer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "  resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.TransactionsByOfferCSC(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "  resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.TransactionsByOffer conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.TransactionsByOffer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }
        #endregion


        #region GetTransactionReasonCode
        /// <summary>
        /// Gets TransactionReason Code
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of TransactionReason Code details.</param>
        /// <param name="culture">culture</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetTransactionReasonCode(out string errorXml, out string resultXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            Customer custObj = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml);
                custObj = new Customer();

                //Get Customer Personal details
                resultXml = custObj.GetTransactionReasonCode();

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetTransactionReasonCode resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetTransactionReasonCode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                custObj = null;

            }

            return bResult;
        }

        #endregion


        #region TransactionsByOffer
        /// <summary>
        /// TransactionsByOffer -- To Fetch Transaction Details using offer ID
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the Transaction Details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool GetPointsBalance(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            Customer customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "  resultXml" + resultXml);
                customerObject = new Customer();

                doc = new XmlDocument();
                resultXml = customerObject.GetPointsBalance(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "  resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetPointsBalance conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetPointsBalance");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;

            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }
        #endregion

        #region AddPoints
        /// <summary>
        /// AddPoints -- To Add Points to Customer
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="objectId">objectId</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records</param>
        /// <param name="userID">userID</param>
        /// <returns>True if points added successfully; otherwise, False.</returns>
        public bool AddPoints(string objectXml, out long objectId, out string errorXml, out string resultXml, int userID)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            resultXml = string.Empty;
            objectId = 0;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                customerObject = new Customer();
                customerObject.AddPoints(objectXml, out objectId, out resultXml, userID);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddPoints userID" + userID + "objectXml" + objectXml + " resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddPoints");
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


        /// <summary>
        /// GetPointsSummary -- It gets the customer details 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of point </param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetPointsSummary(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "  resultXml" + resultXml);
                errorXml = string.Empty;
                Customer customerObj = new Customer();
                resultXml = customerObj.GetPointsSummary(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "  resultXml" + resultXml);
                return true;

            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetPointsSummary conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetPointsSummary");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {
                errorXml = null;
                customerObject = null;
            }
        }
        /// <summary>
        /// Gets the Config Details.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <param name="culture">culture</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetConfigDetails(string conditionXml, out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                customerObject = new Customer();
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                //Get Customer Personal details
                resultXml = customerObject.GetConfigDetails(conditionXml, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetConfigDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetConfigDetails  - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetConfigDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }

        /// <summary>
        /// UpdateConfig -- It is used to update config details 
        /// </summary>
        /// <param name="updateXml">ML string,which contains the input details to be updated</param>
        /// <param name="consumer">UserID to updated amendby column in the DB.</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="customerID">customerID</param>
        /// <returns>True if the records updated successfully; otherwise, False.</returns>
        public bool UpdateConfig(string updateXml, string consumer, out string errorXml, out Int64 customerID)
        {
            Customer customerObject = null;
            short userID = 0;
            errorXml = string.Empty;
            customerID = 0;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.UpdateConfig consumer" + consumer);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.UpdateConfig consumer" + consumer);
                //Get the AmendBy Id
                userID = Helper.GetConsumerID(consumer);

                customerObject = new Customer();
                customerObject.UpdateConfig(updateXml, userID, out customerID, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.UpdateConfig customerID" + customerID + "errorXml" + errorXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.UpdateConfig customerID" + customerID + "errorXml" + errorXml);
                bResult = true;

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.UpdateConfig customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.UpdateConfig customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.UpdateConfig");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }

        /// <summary>
        /// To add print vouchers/Tokens details for reporting purpose.
        /// </summary>
        /// <param name="updateDS">Dataset,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if print vouchers/Tokens details added successfully; otherwise, False.</returns>
        public bool AddPrintAtHomeDetails(DataSet updateDS, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;
            string updateXml = string.Empty;

            try
            {

                updateXml = updateDS.GetXml();
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddPrintAtHomeDetails updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.AddPoints updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.AddPrintAtHomeDetails(updateXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.AddPoints errorXml" + errorXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.AddPoints errorXml" + errorXml);
                bResult = true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.AddPrintAtHomeDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.AddPrintAtHomeDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.AddPrintAtHomeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }

        //public bool UpdateCardStatusCSC(string insertXml, out string errorXml)
        //{
        //    Customer customerObject = null;
        //    errorXml = string.Empty;
        //    bool bResult = false;

        //    #region Trace
        //    Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
        //    ITraceState trState = trace.StartProc("CustomerService.UpdateCardStatusCSC");
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(" InsertXML: " + insertXml);
        //    trace.WriteInfo(sb.ToString());
        //    #endregion

        //    try
        //    {
        //        customerObject = new Customer();
        //        customerObject.UpdateCardStatusCSC(insertXml, out errorXml);
        //        bResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "InsertXML:" + insertXml);
        //        errorXml = ex.InnerException.ToString();
        //        bResult = false;
        //    }
        //    finally
        //    {
        //        customerObject = null;

        //    }

        //    return bResult;
        //}


        #endregion

        #region DelinkingAccounts

        /// <summary>
        /// GetAlternativeIds -- It gets the corresponding dotcom-ids of the customer
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetAlternativeIds(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;

            errorXml = string.Empty;
            rowCount = 0;
            maxRowCount = 0;
            CustomerAlternateID customerObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "  resultXml" + resultXml);
                customerObject = new CustomerAlternateID();

                doc = new XmlDocument();
                resultXml = customerObject.GetAlternativeIds(conditionXml, maxRowCount, out rowCount, culture);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "  resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetAlternativeIds conditionXml" + conditionXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetAlternativeIds");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;

            }
            finally
            {
                doc = null;
                errorXml = null;
                customerObject = null;

            }
            return bResult;
        }


        /// <summary>
        /// DeLinkingDotcomAccounts -- It delinks corresponding dotcom-Id account from the customer
        /// </summary>
        /// <param name="objectXml">XML string,which contains the input details</param>
        /// <param name="resultXml">XML string, which contains the list of customer details.</param>
        /// <returns>True if the record delink with the customer; otherwise, False.</returns>
        public bool DeLinkingDotcomAccounts(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            CustomerAlternateID objCustomerAlternateID = null;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts  objectXml" + objectXml + " resultXml" + resultXml);
                objCustomerAlternateID = new CustomerAlternateID();
                objCustomerAlternateID.DeLinkingDotcomAccounts(objectXml, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts objectXml" + objectXml + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts  objectXml" + objectXml + " resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts objectXml" + objectXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts objectXml" + objectXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.DeLinkingDotcomAccounts");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                objCustomerAlternateID = null;

            }

            return bResult;



        }

        #endregion

        /// <summary>
        /// LoadPreferences -- It loads the preferences
        /// </summary>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of resulted records.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <param name="culture">culture</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool LoadPreferences(out string errorXml, out string resultXml, out int rowCount, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Customer customerObject = null;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.LoadPreferences");
                customerObject = new Customer();

                //Get Customer Personal details
                resultXml = customerObject.LoadPreferences(out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.LoadPreferences");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetTitles  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetTitles resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetTitles ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }


        #region Validate Email Link
        /// <summary>
        /// To validate the customer when user clicks on the email link.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string ValidateEmailLink(string guid)
        {
            string customerID = null;
            
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.LoadPreferences");
                customerObject = new Customer();

                //Get Customer Personal details
                customerID = customerObject.ValidateEmailLink(guid);

               
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.LoadPreferences");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetTitles  customerID-" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetTitles customerID-" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetTitles ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                
            }
            finally
            {
                customerObject = null;
            }

            return customerID;
        }

        #endregion




        //Define a method as follows to be exposed as a service method to dotcom
        #region GetCustomerCoupons
        /// <summary>
        /// GetCustomerCoupons()
        /// </summary>
        /// <param name="objCouponRequest"></param>
        ///Description :This Method will be invoked by passing dotcomId as parameter as a couponrequest class object
        ///It will check whether the dotcomId is activated in MCA 
        ///If the dotcomId is activated it will return customerId against the dotcomId and using the customerid 
        ///it will fetch the household id
        ///using the householdid we can retrieve the available coupons and return the coupons list in the form of
        ///couponresponse object
        public CouponResponse GetCustomerCoupons(CouponRequest objCouponRequest)
        {
            NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            ClubcardOnlineService.ClubcardService objClubcardService = null;
            ClubcardCouponServiceClient objCouponClient = null;
            CouponResponse objCouponResponse = null; 
            List<CouponInformation> couponList = null;
            
            
            try
            {
                Int64 customerID = 0;
                char activated = 'N';
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                long houseHoldID = 0;
                int totalCoupons = 0;
                string errorMessage = "";
                string pCulture = ConfigurationManager.AppSettings["Culture"].ToString();
                objCouponResponse = new CouponResponse();
                objClubcardService = new ClubcardOnlineService.ClubcardService();

                if (objClubcardService.CheckCustomerActivated(objCouponRequest.DotComID, out activated, out customerID, pCulture, out errorXml, out resultXml))
                {

                    //If account is activted in Clubcard System, then user is able to see available coupons
                    if (activated == 'Y')
                    {

                        if (customerID != 0)
                        {
                            houseHoldID = getHouseHoldID(customerID, pCulture);
                            
                            if (houseHoldID != 0)
                            {
                                objCouponClient = new ClubcardCouponServiceClient();
                                couponList = new List<CouponInformation>();
                                objCouponClient.GetAvailableCoupons(out errorXml, out couponList, out totalCoupons, houseHoldID);

                               
                                objCouponResponse.Coupons = couponList;
                                objCouponResponse.TotalCoupon = totalCoupons;
                                objCouponResponse.ActiveCoupon=couponList.Count;
                                objCouponResponse.HouseHolId = houseHoldID;

                                
                            }
                            else
                            {
                                
                                errorMessage = ConfigurationManager.AppSettings["APIHouseHoldIdErrorMessage"].ToString();
                                objCouponResponse.Status = false;
                                objCouponResponse.ErrorMessage = errorMessage;
                            }
                        }
                        else 
                        {
                            errorMessage = ConfigurationManager.AppSettings["APICustomerIdErrorMessage"].ToString();
                            objCouponResponse.Status = false;
                            objCouponResponse.ErrorMessage = errorMessage;
                        }
                    }
                    else if (activated == 'P')
                    {
                        errorMessage = ConfigurationManager.AppSettings["APIPendingErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;
                    }
                    
                    else if (activated == 'N')
                    {
                        errorMessage = ConfigurationManager.AppSettings["APIErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;
                    }
                    else
                    {

                        errorMessage = ConfigurationManager.AppSettings["APIErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;

                    }
                }
                else

                {
                    objCouponResponse.Status = false;
                    objCouponResponse.ErrorMessage = "Service Error:" + errorXml;
                   
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            }
            catch (Exception ex)
            {
                objCouponResponse.Status = false;
                objCouponResponse.ErrorMessage = "Service Error:" + ex.Message;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerCoupons - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                
            }
            finally
            {
                objClubcardService = null;
                objCouponClient = null;
                couponList = null;                             
            }

            return objCouponResponse;
        }


        #endregion

        /// <summary>
        /// This method created as a part of IGHS(Dotcomid datatype changed fron int to string).
        /// </summary>
        /// <param name="objCouponRequest">Coupon Request Data</param>
        ///Description :This Method will be invoked by passing dotcomId as parameter as a couponrequest class object
        ///It will check whether the dotcomId is activated in MCA 
        ///If the dotcomId is activated it will return customerId against the dotcomId and using the customerid 
        ///it will fetch the household id
        ///using the householdid we can retrieve the available coupons and return the coupons list in the form of
        ///couponresponse object
        /// <returns>True if the records exists; otherwise, False</returns>
        public CouponResponse IGHSGetCustomerCoupons(CustomerCouponRequest objCouponRequest)
        {
            NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            ClubcardOnlineService.ClubcardService objClubcardService = null;
            ClubcardCouponServiceClient objCouponClient = null;
            CouponResponse objCouponResponse = null;
            List<CouponInformation> couponList = null;


            try
            {
                Int64 customerID = 0;
                char activated = 'N';
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                long houseHoldID = 0;
                int totalCoupons = 0;
                string errorMessage = "";
                string pCulture = ConfigurationManager.AppSettings["Culture"].ToString();
                objCouponResponse = new CouponResponse();
                objClubcardService = new ClubcardOnlineService.ClubcardService();

                if (objClubcardService.IGHSCheckCustomerActivated(objCouponRequest.DotComID, out activated, out customerID, pCulture, out errorXml, out resultXml))
                {

                    //If account is activted in Clubcard System, then user is able to see available coupons
                    if (activated == 'Y')
                    {

                        if (customerID != 0)
                        {
                            houseHoldID = getHouseHoldID(customerID, pCulture);

                            if (houseHoldID != 0)
                            {
                                objCouponClient = new ClubcardCouponServiceClient();
                                couponList = new List<CouponInformation>();
                                objCouponClient.GetAvailableCoupons(out errorXml, out couponList, out totalCoupons, houseHoldID);


                                objCouponResponse.Coupons = couponList;
                                objCouponResponse.TotalCoupon = totalCoupons;
                                objCouponResponse.ActiveCoupon = couponList.Count;
                                objCouponResponse.HouseHolId = houseHoldID;


                            }
                            else
                            {

                                errorMessage = ConfigurationManager.AppSettings["APIHouseHoldIdErrorMessage"].ToString();
                                objCouponResponse.Status = false;
                                objCouponResponse.ErrorMessage = errorMessage;
                            }
                        }
                        else
                        {
                            errorMessage = ConfigurationManager.AppSettings["APICustomerIdErrorMessage"].ToString();
                            objCouponResponse.Status = false;
                            objCouponResponse.ErrorMessage = errorMessage;
                        }
                    }
                    else if (activated == 'P')
                    {
                        errorMessage = ConfigurationManager.AppSettings["APIPendingErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;
                    }

                    else if (activated == 'N')
                    {
                        errorMessage = ConfigurationManager.AppSettings["APIErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;
                    }
                    else
                    {

                        errorMessage = ConfigurationManager.AppSettings["APIErrorMessage"].ToString();
                        objCouponResponse.Status = false;
                        objCouponResponse.ErrorMessage = errorMessage;

                    }
                }
                else
                {
                    objCouponResponse.Status = false;
                    objCouponResponse.ErrorMessage = "Service Error:" + errorXml;

                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetCustomerCoupons");
            }
            catch (Exception ex)
            {
                objCouponResponse.Status = false;
                objCouponResponse.ErrorMessage = "Service Error:" + ex.Message;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetCustomerCoupons - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally
            {
                objClubcardService = null;
                objCouponClient = null;
                couponList = null;
            }

            return objCouponResponse;
        }
 /// <summary>
/// getHouseHoldID(long customerID, string culture)
/// </summary>
/// <param name="customerID"></param>
/// <param name="culture"></param>
        private long getHouseHoldID(long customerID, string culture)
        {
            NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.getHouseHoldID");
            NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.getHouseHoldID");
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows = 1;
            long houseHoldID = 0;
            DataSet dsCustomerInfo = null;
            Hashtable searchData = new Hashtable();
            searchData["CustomerID"] = customerID;
            //Preparing parameters for service call
            conditionXml = Helper.HashTableToXML(searchData, "customer");
            CustomerService objCustomerService = new CustomerService();
            if (objCustomerService.SearchCustomer(conditionXml, maxRows, culture, out errorXml, out resultXml, out rowCount))
            {
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsCustomerInfo = new DataSet();
                dsCustomerInfo.ReadXml(new XmlNodeReader(resulDoc));

                if (dsCustomerInfo.Tables.Count > 0)
                {
                    if (dsCustomerInfo.Tables["Customer"].Columns.Contains("HouseHoldID") != false
                        && !string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString()))
                    {
                        houseHoldID = Convert.ToInt64(dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString().Trim());
                    }
                }
            }
            NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.getHouseHoldID");
            NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.getHouseHoldID");

            return houseHoldID;
        }

        #region SecurityLayer - Madhu

        public bool InsertUpdateCustomerVerificationDetails(string updateXml, out long objectID, out string errorXml)
        {
            errorXml = string.Empty;
            objectID = 0;
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                customerObject = new Customer();
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                //Get Customer Personal details
                bResult = customerObject.InsertUpdateCustomerVerificationDetails(updateXml);



                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + updateXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetConfigDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetConfigDetails  - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetConfigDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }
        public bool GetCustomerVerificationDetails(string conditionXml, out string errorXml, out string resultXml)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
           
            Customer customerObject = null;
            bool bResult = false;

            try
            {
                customerObject = new Customer();
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.GetConfigDetails ");
                //Get Customer Personal details
                resultXml = customerObject.GetCustomerVerificationDetails(conditionXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.GetConfigDetails  resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                errorXml = ex.InnerException.ToString();
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.GetConfigDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.GetConfigDetails  - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.GetConfigDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                customerObject = null;
            }

            return bResult;
        }
        
        #endregion

    }
}