using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Xml;
using System.Data;
//using Fujitsu.eCrm.Generic.SharedUtils;
using USLoyaltySecurityServiceLayer;
using NGCTrace;

namespace Tesco.com.ClubcardOnlineService
{
    public class ClubcardService : IClubcardService
    {
        Customer customerObject = null;
        Clubcard clubcardObject = null;
        string errorXml = string.Empty;
        string resultXml = string.Empty;
        USLoyaltySecurityServiceLayer.SecurityService objSecurityService;

        /// <summary>
        /// Check whether logged in customer belongs to Xmas club.
        /// </summary>
        /// <param name="CustomerID">CustomerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if customer belongs to Xmas club; otherwise, False.</returns>
        public bool IsXmasClubMember(Int64 CustomerID, string culture, out string errorXml, out string resultXml)
        {
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.IsXmasClubMember CustomerID" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.IsXmasClubMember resultXml" + resultXml);
                resultXml = customerObject.ViewIsXmasClubMember(CustomerID, culture);
                if (resultXml != "" && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.IsXmasClubMember CustomerID" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.IsXmasClubMember resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.IsXmasClubMember CustomerID" + CustomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.IsXmasClubMember CustomerID" + CustomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.IsXmasClubMember");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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
        /// Gets the xmas saver summary(money topped up)
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the xmas saver summary(money topped up).</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if summary exists; otherwise, False.</returns>
   
        public bool GetChristmasSaverSummary(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            rowCount = 100;
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary resultxml" + resultXml);
                resultXml = customerObject.ViewChristmasSaverSummary(conditionXml, maxRowCount, out rowCount, culture);

                if (resultXml != "" && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary resultxml resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary conditionXml" + conditionXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetChristmasSaverSummary");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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
        /// Gets the account details of customer.
        /// </summary>
        /// <param name="CustomerID">CustomerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the account details of customer.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetMyAccountDetails(Int64 CustomerID, string culture, out string errorXml, out string resultXml)
        {
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetMyAccountDetails UserId" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetMyAccountDetails resultxml" + resultXml);
                resultXml = customerObject.ViewMyAccountDetails(CustomerID, culture);

                if (resultXml != "" && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetMyAccountDetails UserId" + CustomerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetMyAccountDetails resultxml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetMyAccountDetails CustomerID" + CustomerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetMyAccountDetails CustomerID" + CustomerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetMyAccountDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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
        /// IsNewOrderReplacementValid--It checks the applied card is valid for Replacement or not
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if new order is valid; otherwise, False.</returns>
   
        public bool IsNewOrderReplacementValid(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            rowCount = 0;
            resultXml = string.Empty;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid resultXml" + resultXml);
                errorXml = string.Empty;
                Customer customerObj = new Customer();
                resultXml = customerObj.IsNewOrderReplacementValid(conditionXml, maxRowCount, out rowCount, culture);

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid conditionXml" + conditionXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.IsNewOrderReplacementValid");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }
            }
        }

        /// <summary>
        /// AddNewOrderReplacement--To add new replacement card order
        /// </summary>
        /// <param name="updateXml">XML string,which contains the input details to be updated</param>
        /// <param name="consumer">UserID to updated amendby column in the DB.</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="customerID">customerID</param>
        /// <returns>True if new replacement order added successfully.; otherwise, False.</returns>
        public bool AddNewOrderReplacement(string updateXml, string consumer, out string errorXml, out Int64 customerID)
        {
            errorXml = string.Empty;
            customerID = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement UserId" + customerID + "conditionxml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement consumer" + consumer);
                Customer customerObj = new Customer();
                string resultXml = string.Empty;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement consumer" + consumer);

                return customerObj.AddNewOrderReplacement(updateXml, Helper.GetConsumerID(consumer), out customerID, out resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddNewOrderReplacement");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }
            }
        }

        /// <summary>
        /// Gets all the customers from the household od logged in customer.
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of household customer details.</param>
        /// <returns></returns>
        public bool GetHouseholdCustomers(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            clubcardObject = new Clubcard();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;




            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers resultXml" + resultXml);
                resultXml = clubcardObject.ViewHouseholdCustomers(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetHouseholdCustomers");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (clubcardObject != null)
                {
                    clubcardObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// Gets all the clubcards and its details of the customer for Dundee.
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains clubcard details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetClubcards(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            clubcardObject = new Clubcard();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetClubcards UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetClubcards resultXml" + resultXml);
                resultXml = clubcardObject.ViewClubcards(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetClubcards UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetClubcards resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetClubcards customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetClubcards customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetClubcards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (clubcardObject != null)
                {
                    clubcardObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// Gets all the clubcards and its details of the customer.
        /// </summary>
        /// <param name="customerID">customerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains clubcards and its details of the customer</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetClubcardsCustomer(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            clubcardObject = new Clubcard();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer resultXml" + resultXml);
                resultXml = clubcardObject.ViewClubcardsCustomer(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer UserId" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetClubcardsCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (clubcardObject != null)
                {
                    clubcardObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// GetPointsForAllCollPeriodByCustomer -- It gets the points for all collectionperiod
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains point details</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
   
        public bool GetPointsForAllCollPeriodByCustomer(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer resultXml" + resultXml);
                Customer customerObj = new Customer();
                resultXml = customerObj.GetPointsInfoForAllColPrdByCustomer(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetPointsForAllCollPeriodByCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {

            }
        }

        /// <summary>
        /// GetTxnDetailsByCustomerAndOfferID--Get transaction details by Customerid and offerid.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name=resultXml">XML string, which contains the list of transaction details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetTxnDetailsByCustomerAndOfferID(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            //set out parameters
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID resultXml" + resultXml);
                errorXml = string.Empty;
                Customer customerObj = new Customer();
                resultXml = customerObj.GetTxnDetailsByCustomerAndOfferID(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {

            }
        }

        /// <summary>
        /// GetTxnDetailsByHouseholdCustomerAndOfferID--Get transaction details based on household customerid and offer id.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of transaction details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetTxnDetailsByHouseholdCustomerAndOfferID(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            //set out parameters
            errorXml = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID resultXml" + resultXml);
                errorXml = string.Empty;
                Customer customerObj = new Customer();
                resultXml = customerObj.GetTxnDetailsByHouseholdCustomerAndOfferID(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {

            }
        }
        /// <summary>
        /// GetPointsSummaryInfo--Get point summary information
        /// </summary>
        /// <param name="dotcomCustomerID">dotcomCustomerID</param>
        /// <param name="activated">activated</param>
        /// <param name="customerID">customerID</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the point summary details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool GetPointsSummaryInfo(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo resultXml");
                errorXml = string.Empty;
                Customer customerObj = new Customer();
                resultXml = customerObj.GetPointsSummaryInfo(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetPointsSummaryInfo");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //handle exception here
                //register it in errorXml
                return false;
            }
            finally
            {

            }
        }
        /// <summary>
        /// CheckCustomerActivated --it checks whether customer is activated or not. It gets customer activate status details,customerid aganist dotcomCustomerID.
        /// modified by Laxmi as a part of IGHS on 21/2/2013.
        /// </summary>
        /// <example>
        /// This is an example of the format of Result XML
        /// <code lang="xml"><![CDATA[
        /// <NewDataSet>
        ///<ViewHouseholdStatusOfCustomer>
        ///<PrimaryCustomerID>392654</PrimaryCustomerID>
        ///<CustomerUseStatus>1</CustomerUseStatus>
        ///<CustomerMailStatus>7</CustomerMailStatus>
        ///<CustomerEmailStatus>0</CustomerEmailStatus>
        ///<CustomerMobilePhoneStatus>0</CustomerMobilePhoneStatus>
        ///</ViewHouseholdStatusOfCustomer>
        ///<ViewCheckCustomerActivated>
        ///<Activated>Y</Activated>
        ///<CustomerID>392654</CustomerID>
        ///</ViewCheckCustomerActivated>
        ///</NewDataSet>
        /// ]]></code>    
        /// </example>
        /// <param name="dotcomCustomerID">The dotcom customer ID</param>
        /// <param name="activated">Output parameter,if it is Y:-customer is activated,if it N:-customer is not activated.</param>
        /// <param name="customerID">Output parameter,Aganist dotcom customer id will get customer id as output parameter.</param>
        /// <param name="culture">The culture identifier, e.g. 'en-GB'</param>
        /// <param name="errorXml">XML string, contains error detail if any,output param</param>
        /// <param name="resultXml">XML string,contains result record details,output param</param>
        /// <returns>True if customer is activated; otherwise, False.</returns>
        public bool CheckCustomerActivated(Int64 dotcomCustomerID, out char activated, out Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;
            activated = '0';
            customerID = 0;
            string dotcomId1 = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.CheckCustomerActivated resultXml" + resultXml);
                dotcomId1 = Convert.ToString(dotcomCustomerID);
                bResult = IGHSCheckCustomerActivated(dotcomId1, out activated, out customerID, culture, out errorXml, out resultXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.CheckCustomerActivated resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID " + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.CheckCustomerActivated  customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.CheckCustomerActivated");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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
        /// IGHSCheckCustomerActivated --it checks whether customer is activated or not. It gets customer activate status details,customerid aganist dotcomCustomerID.
        /// This method Created as a part of IGHS(Dotcomid changed from Int to String)
        /// </summary>
        /// <example>
        /// This is an example of the format of Result XML
        /// <code lang="xml"><![CDATA[
        /// <NewDataSet>
        ///<ViewHouseholdStatusOfCustomer>
        ///<PrimaryCustomerID>392654</PrimaryCustomerID>
        ///<CustomerUseStatus>1</CustomerUseStatus>
        ///<CustomerMailStatus>7</CustomerMailStatus>
        ///<CustomerEmailStatus>0</CustomerEmailStatus>
        ///<CustomerMobilePhoneStatus>0</CustomerMobilePhoneStatus>
        ///</ViewHouseholdStatusOfCustomer>
        ///<ViewCheckCustomerActivated>
        ///<Activated>Y</Activated>
        ///<CustomerID>392654</CustomerID>
        ///</ViewCheckCustomerActivated>
        ///</NewDataSet>
        /// ]]></code>    
        /// </example>
        /// <param name="dotcomCustomerID">The dotcom customer ID</param>
        /// <param name="activated">Output parameter,if it is Y:-customer is activated,if it N:-customer is not activated.</param>
        /// <param name="customerID">Output parameter,Aganist dotcom customer id will get customer id as output parameter.</param>
        /// <param name="culture">The culture identifier, e.g. 'en-GB'</param>
        /// <param name="errorXml">XML string, contains error detail if any,output param</param>
        /// <param name="resultXml">XML string,contains result record details,output param</param>
        /// <returns>True if customer is activated; otherwise, False.</returns>
        public bool IGHSCheckCustomerActivated(string dotcomCustomerID, out char activated, out Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;
            activated = '0';
            customerID = 0;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.CheckCustomerActivated resultXml" + resultXml);
                resultXml = customerObject.CheckCustomerActivated(dotcomCustomerID, out activated, out customerID, culture);
                if (resultXml != "" && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.CheckCustomerActivated resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.CheckCustomerActivated customerID " + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.CheckCustomerActivated  customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.CheckCustomerActivated");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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
        /// CheckHouseholdStatusOfCustomer--it checkes household customer status
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the household customer status details.</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool CheckHouseholdStatusOfCustomer(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            errorXml = string.Empty;
            resultXml = string.Empty;
            customerObject = new Customer();
            bool bResult = false;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer resultXml" + resultXml);
                resultXml = customerObject.CheckHouseholdStatusOfCustomer(customerID, culture);
                if (resultXml != "" && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.CheckHouseholdStatusOfCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
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

        #region ADD PRIMARY CLUBCARD

        /// <summary>
        /// Method to add primary card.
        /// </summary>
        /// <param name="addCardXml">XML string,which contains the input details</param>
        /// <param name="userID">userID</param>
        /// <param name="objectId">objectId</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if the record add successfully; otherwise, False.</returns>
        public bool AddPrimaryCard(string addCardXml, int userID, out long objectId, out string resultXml, out string errorXml)
        {
            objectId = 0;
            resultXml = string.Empty;
            errorXml = string.Empty;



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.AddPrimaryCard userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.AddPrimaryCard resultXml" + resultXml);
                Clubcard clubcardObj = new Clubcard();
                //string resultXml = string.Empty;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.AddPrimaryCard userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.AddPrimaryCard resultXml" + resultXml);
                return clubcardObj.AddPrimaryCard(addCardXml, userID, out  objectId, out resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddPrimaryCard userID" + userID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddPrimaryCard userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddPrimaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                return false;
            }
            finally
            {

            }
        }



        #endregion ADD PRIMARY CLUBCARD

        #region UpdateCustomerStatus
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if the record updated successfully; otherwise, False.</returns>
        public bool UpdateCustomerStatus(string insertXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus insertXml" + insertXml);
                customerObject = new Customer();
                bResult = customerObject.UpdateCustomerStatus(insertXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus resultXml" + insertXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus " + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus " + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.UpdateCustomerStatus");
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

        #region RollBackCustomerDetails
        /// <summary>
        /// RollBackCustomerDetails -- It is used to rollback new customer details from join page 
        /// </summary>
        /// <param name="insertXml">XML string,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if details get rollback; otherwise, False.</returns>
        public string RollBackCustomerDetails(string insertXml, out string errorXml)
        {

            Customer customerObject = null;
            errorXml = string.Empty;
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails insertXml" + insertXml);
                customerObject = new Customer();
                viewXml = customerObject.RollBackCustomerDetails(insertXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails insertXml" + insertXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails insertXml" + insertXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails" + insertXml + "  - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails " + insertXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.RollBackCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                customerObject = null;

            }

            return viewXml;


        }
        #endregion

        /// <summary>
        /// Validate Token for Customer on click on the link
        /// </summary>
        /// <param name="tokenId">tokenId</param>
        /// <param name="resultXml">XML string, which contains the list of customer details</param>
        /// <returns></returns>
        public bool ValidateTokenCustomer(string tokenId, out string resultXml)
        {
            objSecurityService = new USLoyaltySecurityServiceLayer.SecurityService();
            resultXml = string.Empty;
            string RXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer tokenId" + tokenId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer resultXml" + resultXml);
                bResult = objSecurityService.ValidateTokenCustomerID(tokenId, out RXml);
                resultXml = RXml;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer tokenId" + tokenId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer tokenId" + tokenId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer tokenId" + tokenId + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ValidateTokenCustomer");
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

        // SSC Enhancements
        // Card Range

        #region ViewCardRange
        /// <summary>
        /// ViewCardRange --get list clubcard range records which is existing in the DB. 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="out errorXml">XML string, contains error details if any</param>
        /// <param name="out resultXml">XML string, which contains the list of clubcard-range details.</param>
        /// <param name="out rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool ViewCardRange(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ViewCardRange conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ViewCardRange resultXml" + resultXml);
                errorXml = string.Empty;
                ClubcardRange clubcardObj = new ClubcardRange();
                resultXml = clubcardObj.ViewCardRange(conditionXml, maxRowCount, rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ViewCardRange conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ViewCardRange resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ViewCardRange conditionXml" + conditionXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ViewCardRange conditionXml" + conditionXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ViewCardRange");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        #endregion

        #region UpdateCardRange
        /// <summary>
        /// UpdateCardRange -- to update existing clubcardrange
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="out resultXml">XML string, which contains the result details.</param>
        /// <returns>True if the record updated successfully; otherwise, False.</returns>
        public bool UpdateCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {

            errorXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.UpdateCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.UpdateCardRange resultXml" + resultXml);
                ClubcardRange clubcardObj = new ClubcardRange();
                bResult = clubcardObj.UpdateCardRange(conditionXml, userID, objectID, resultXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.UpdateCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.UpdateCardRange resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.UpdateCardRange userID " + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.UpdateCardRange userID " + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.UpdateCardRange");
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

        #region ViewCardType
        /// <summary>
        /// ViewCardType -- to get list of cardtypes existing in the present DB.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of card type details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool ViewCardType(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ViewCardType conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ViewCardType resultXml" + resultXml);
                errorXml = string.Empty;
                ClubcardType clubcardObj = new ClubcardType();
                resultXml = clubcardObj.ViewAllCardTypes(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ViewCardType conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ViewCardType resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ViewCardType conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ViewCardType conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ViewCardType");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        #endregion

        #region AddCardRange
        /// <summary>
        /// AddCardRange --to add new clubcard range to DB. 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if the records add sucessfully; otherwise, False.</returns>
        public bool AddCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {

            errorXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.AddCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.AddCardRange resultXml" + resultXml);
                ClubcardRange clubcardObj = new ClubcardRange();
                bResult = clubcardObj.AddCardRange(conditionXml, userID, objectID, resultXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.AddCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.AddCardRange resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddCardRange userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddCardRange userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddCardRange");
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

        #region DeleteCardRange
        /// <summary>
        /// DeleteCardRange --  to delete cardrange record from DB
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="resultXml">XML string, which contains the result details</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool DeleteCardRange(string conditionXml, int userID, long objectID, string resultXml)
        {

            errorXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.DeleteCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.DeleteCardRange resultXml" + resultXml);
                ClubcardRange clubcardObj = new ClubcardRange();
                bResult = clubcardObj.DeleteCardRange(conditionXml, userID, objectID, resultXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.DeleteCardRange conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.DeleteCardRange resultXml" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.DeleteCardRange userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.DeleteCardRange userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.DeleteCardRange");
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

        //Card Type 

        #region AddCardType
        /// <summary>
        /// AddCardType -- to add new cardtype to DB
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="out resultXml">XML string, which contains the list of customer details.</param>
        /// <returns>True if the records add sucessfully; otherwise, False.</returns>
        public bool AddCardType(string conditionXml, int userID, long objectID, out string resultXml)
        {

            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.AddCardType conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.AddCardType resultXml" + resultXml);
                ClubcardType clubcardObj = new ClubcardType();
                clubcardObj.AddCardTypes(conditionXml, userID, out objectID, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.AddCardType conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.AddCardType resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddCardType userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddCardType userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddCardType");
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

        #region UpdateCardType
        /// <summary>
        /// UpdateCardType -- To updated the clubcard type
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="out resultXml">XML string, which contains the list of customer details.</param>
        /// <returns>True if the record updated sucessfully; otherwise, False.</returns>
        public bool UpdateCardType(string conditionXml, int userID, long objectID, out string resultXml)
        {

            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.UpdateCardType conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.UpdateCardType resultXml" + resultXml);
                ClubcardType clubcardObj = new ClubcardType();
                clubcardObj.Update(conditionXml, userID, out objectID, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.UpdateCardType conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.UpdateCardType resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.UpdateCardType userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.UpdateCardType userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.UpdateCardType");
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

        //Stores

        #region ViewStores
        /// <summary>
        /// ViewStores --
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of store details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool ViewStores(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ViewStores conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ViewStores resultXml" + resultXml);
                errorXml = string.Empty;
                TescoStore storeObj = new TescoStore();
                resultXml = storeObj.Search(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ViewStores conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ViewStores resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ViewStores conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ViewStores conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ViewStores");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        #endregion

        #region ViewStoreRegion
        // <summary>
        /// ViewStoreRegion -- 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of customer details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>

        public bool ViewStoreRegion(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {

            resultXml = "";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ViewStoreRegion conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ViewStoreRegion resultXml" + resultXml);
                errorXml = string.Empty;
                TescoStore storeObj = new TescoStore();
                resultXml = storeObj.ViewStoreRegion(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ViewStoreRegion conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ViewStoreRegion resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ViewStoreRegion conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ViewStoreRegion conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ViewStoreRegion");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        #endregion

        #region ViewStoreFormat
        /// <summary>
        /// ViewStoreFormat -- 
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="maxRowCount">sets the max limit for resulted rows</param>
        /// <param name="culture">culture</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of store format details.</param>
        /// <param name="rowCount">Gives the count of resulted rows</param>
        /// <returns>True if the records exists; otherwise, False.</returns>
        public bool ViewStoreFormat(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {
            resultXml = "";

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.ViewStoreFormat conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.ViewStoreFormat resultXml" + resultXml);
                errorXml = string.Empty;
                TescoStore storeObj = new TescoStore();
                resultXml = storeObj.ViewStoreFormat(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.ViewStoreFormat conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.ViewStoreFormat resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.ViewStoreFormat conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.ViewStoreFormat conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.ViewStoreFormat");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        #endregion

        #region AddStores
        /// <summary>
        /// AddStores -- To add stores to DB.
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="resultXml">XML string, which contains the result details</param>
        /// <returns>True if store add successfully; otherwise, False.</returns>
        public bool AddStores(string conditionXml, int userID, long objectID, out string resultXml)
        {

            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;
            TescoStore clubcardObj = null;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.AddStores conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.AddStores resultXml" + resultXml);
                clubcardObj = new TescoStore();
                clubcardObj.Add(conditionXml, userID, out objectID, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.AddStores conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.AddStores resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddStores userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddStores userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddStores");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                clubcardObj = null;

            }

            return bResult;
        }
        #endregion

        #region UpdateStores
        /// <summary>
        /// UpdateStores -- It Updates the store details
        /// </summary>
        /// <param name="conditionXml">XML string,which contains the input details</param>
        /// <param name="userID">userID</param>
        /// <param name="objectID">objectID</param>
        /// <param name="resultXml">XML string, which contains the result details.</param>
        /// <returns>True if store details updates successfully; otherwise, False.</returns>
        public bool UpdateStores(string conditionXml, int userID, long objectID, out string resultXml)
        {

            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;
            TescoStore storeObj = null;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.UpdateStores conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.UpdateStores resultXml" + resultXml);
                storeObj = new TescoStore();
                storeObj.Update(conditionXml, userID, out objectID, out resultXml);
                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.UpdateStores conditionXml" + conditionXml + "userID" + userID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.UpdateStores resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.UpdateStores userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.UpdateStores userID" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.UpdateStores");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                storeObj = null;

            }

            return bResult;
        }
        #endregion

        #region Get Store name
        /// <summary>
        /// GetStoreNames -- It gets the store details 
        /// </summary>
        /// <param name="storeNumbers">storeNumbers</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <param name="resultXml">XML string, which contains the list of Store details.</param>
        /// <param name="culture">culture</param>
        /// <returns>True if the store details exists; otherwise, False.</returns>
        public bool GetStoreNames(string storeNumbers, out string errorXml, out string resultXml, string culture)
        {
            resultXml = string.Empty;
            errorXml = string.Empty;

            TescoStore storeObj = null;
            bool bResult = false;
            int maxRowCnt = 100;
            int rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + " resultXml" + resultXml);
                storeObj = new TescoStore();

                //Get store name
                resultXml = storeObj.GetStoreName(storeNumbers, maxRowCnt, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + " resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + " resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.AdminSerice.GetStoreNames userID" + storeNumbers + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.AdminSerice.GetStoreNames");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                storeObj = null;
            }

            return bResult;
        }

        #endregion

        #region PrintAtHomeDetailsXML
        /// <summary>
        /// To add print vouchers/Tokens details for reporting purpose.
        /// </summary>
        /// <param name="updateDS">Dataset,which contains the input details</param>
        /// <param name="errorXml">XML string, contains error details if any</param>
        /// <returns>True if print vouchers/Tokens details added successfully; otherwise, False.</returns>
        public bool AddPrintAtHomeDetailsXMLInput(string updateXml, out string errorXml)
        {
            Customer customerObject = null;
            errorXml = string.Empty;
            bool bResult = false;
            //string updateXml = string.Empty;

            try
            {

                //updateXml = updateDS.GetXml();
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.AddPrintAtHomeDetailsXMLInput updateXml" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.PrintAtHomeDetailsXML updateXml" + updateXml);
                customerObject = new Customer();
                customerObject.AddPrintAtHomeDetails(updateXml, out errorXml);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.PrintAtHomeDetailsXML errorXml" + errorXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.PrintAtHomeDetailsXML errorXml" + errorXml);
                bResult = true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.AddPrintAtHomeDetailsXMLInput - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.AddPrintAtHomeDetailsXMLInput - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.AddPrintAtHomeDetailsXMLInput");
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
