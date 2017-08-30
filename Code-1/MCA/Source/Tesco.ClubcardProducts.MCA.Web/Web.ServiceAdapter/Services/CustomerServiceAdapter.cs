using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.ServiceModel;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class CustomerServiceAdapter : IServiceAdapter
    {
        private readonly ILoggingService _logger = null;

        #region Constructors

        private ICustomerService _customerServiceClient;

        public CustomerServiceAdapter(ICustomerService customerServiceClient, ILoggingService logger)
        {
            _customerServiceClient = customerServiceClient;
            _logger = logger;
        }

        #endregion Constructors

        #region IServiceAdapter Members

        /// <summary>
        /// Data retrieval call for Customer Service
        /// Methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Get<T>(Common.Entities.Service.MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData _logData = new LogData();
            List<string> blacklistData = new  List<string>();
            
            if (request.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
            {
                blacklistData.Add(request.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString());
                _logData.BlackLists = blacklistData;
            }
            _logData.CaptureData("Request Object", request);
            
            try
            {
                if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID))
                {
                    _logData.CustomerID = request.Parameters[ParameterNames.CUSTOMER_ID].ToString();
                }
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                switch (operation)
                {
                   
                    case OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetHouseHoldDetailsByCustomer(request.Parameters[ParameterNames.CUSTOMER_ID], request.Parameters[ParameterNames.CULTURE]);
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMER_FAMILY_BY_CLUBCARD:
                        if (request.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER) &&
                            request.Parameters.Keys.Contains(ParameterNames.MAX_ROWS) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = GetCustomerFamilyMasterDataByClubcard(request.Parameters[ParameterNames.CLUBCARD_NUMBER], request.Parameters[ParameterNames.MAX_ROWS], request.Parameters[ParameterNames.CULTURE]);
                        }
                        break;
                    case OperationNames.GET_CUSTOMER_VERIFICATION_STATUS_DETAILS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID))
                        {
                          
                            res.Data = GetCustomerVerificationStatus(request.Parameters[ParameterNames.CUSTOMER_ID].ToString());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CONFIGURATIONS:
                        if (request.Parameters.ContainsKey(ParameterNames.CONFIGURATION_TYPES) && request.Parameters.ContainsKey(ParameterNames.CULTURE))
                        {
                            res.Data = GetConfiguration(request.Parameters[ParameterNames.CONFIGURATION_TYPES] as string, request.Parameters[ParameterNames.CULTURE] as string);
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMERID_BY_GUID:
                        if (request.Parameters.ContainsKey(ParameterNames.CUSTOMER_GUID))
                        {
                            res.Data = GetCustomerIDbyGUID(request.Parameters[ParameterNames.CUSTOMER_GUID] as string);
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMER_FAMILY_BY_CUSTID:
                        if (request.Parameters.ContainsKey(ParameterNames.CUSTOMER_ID) && request.Parameters.ContainsKey(ParameterNames.MAX_ROWS) && request.Parameters.ContainsKey(ParameterNames.CULTURE))
                        {
                           
                            res.Data = this.GetCustomerFamilyMasterDataByCustomerId(
                                                    request.Parameters[ParameterNames.CUSTOMER_ID], 
                                                    request.Parameters[ParameterNames.MAX_ROWS], 
                                                    request.Parameters[ParameterNames.CULTURE]);
                            res.Status = true;
                        }
                        break;
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.ErrorMessage = ex.Message;
                ex.Data.Add("Get():InputParam", request);
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter|GET", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return res;
        }

        /// <summary>
        /// Data update call of Customer Service methods
        /// returning boolean value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Set<T>(Common.Entities.Service.MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData _logData = new LogData();

            try
            {
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                switch (operation)
                {
                    case OperationNames.RECORD_PRINT_AT_HOME_DETAILS:
                        _logData.CaptureData("Request object", request);
                        string error = string.Empty;
                        if (request.Parameters.ContainsKey(ParameterNames.DS_COUPONS))
                        {
                            res.Status = RecordPrintAtHomeDetails(request.Parameters[ParameterNames.DS_COUPONS] as DataSet, out error);
                            res.ErrorMessage = error;
                        }
                        else if (request.Parameters.ContainsKey(ParameterNames.DS_TOKENS))
                        {
                            res.Status = RecordPrintAtHomeDetails(request.Parameters[ParameterNames.DS_TOKENS] as DataSet, out error);
                            res.ErrorMessage = error;
                        }
                        else if(request.Parameters.ContainsKey(ParameterNames.DS_VOUCHER))
                        {
                            res.Status = RecordPrintAtHomeDetails(request.Parameters[ParameterNames.DS_VOUCHER] as DataSet, out error);
                            res.ErrorMessage = error;
                        }
                        break;
                    case OperationNames.UPDATE_CUSTOMER_DETAILS:
                        if (request.Parameters.ContainsKey(ParameterNames.USER_DATA) && request.Parameters.ContainsKey(ParameterNames.DOTCOM_CUSTOMER))
                        {
                            res.Status = UpdateCustomerDetails(request.Parameters[ParameterNames.USER_DATA] as Hashtable,request.Parameters[ParameterNames.DOTCOM_CUSTOMER] as string);
                        }
                        if (request.Parameters.ContainsKey(ParameterNames.CUSTOMER_DATA) && request.Parameters.ContainsKey(ParameterNames.DOTCOM_CUSTOMER))
                        {
                            res.Status = UpdateCustomerDetails(request.Parameters[ParameterNames.CUSTOMER_DATA] as CustomerFamilyMasterDataUpdate, request.Parameters[ParameterNames.DOTCOM_CUSTOMER] as string);
                        }
                        break;
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter|SET", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
           
            return res;
        }

        public MCAResponse Delete<T>(Common.Entities.Service.MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Execute(Common.Entities.Service.MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData _logData = new LogData();
            try
            {
                _logData.CaptureData("Request Object", request);
                var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();
                switch (operation)
                {
                    case OperationNames.NOTE_SECURITY_ATTEMPT_AUDIT:

                        if (request.Parameters.Keys.Contains(ParameterNames.SECURITY_ATTEMPT_AUDIT))
                        {
                            res.Data = NoteSecurityAttemptInAudit(request.Parameters[ParameterNames.SECURITY_ATTEMPT_AUDIT]);
                            res.Status = true;
                        }
                        break;
                }
                _logger.Submit(_logData);      
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter|EXECUTE", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return res;
        }

        #endregion

        #region Private Methods

        CustomerFamilyMasterData GetCustomerFamilyMasterDataByClubcard(object clubcardNumber, object maxRows, object culture)
        {
            CustomerFamilyMasterData res = new CustomerFamilyMasterData();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 0;
            Int64 lngClubcardNumber = clubcardNumber.TryParse<Int64>();
            string strCulture = culture.TryParse<string>();
            Int32 intMaxRows = maxRows.TryParse<Int32>();
            Hashtable searchData = new Hashtable();
            searchData["cardAccountNumber"] = lngClubcardNumber;
            string conditionXML = GeneralUtility.HashTableToXML(searchData, "customer");
            LogData _logData = new LogData();
            try
            {
                if (_customerServiceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, intMaxRows, strCulture))
                {
                    if (string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                        {
                            res.ConvertFromXml(resultXml);
                        }
                    }
                    else
                    {
                        _logData.CaptureData("errorXml", errorXml);

                    }
                }
               
                _logger.Submit(_logData);      
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter while getting Customer Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            
            return res;
        }

        CustomerFamilyMasterData GetCustomerFamilyMasterDataByCustomerId(object customerId, object maxRows, object culture)
        {
            LogData _logData = new LogData();
            CustomerFamilyMasterData res = new CustomerFamilyMasterData();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 0;

            Int64 lngCustomerId = customerId.TryParse<Int64>();
            string strCulture = culture.TryParse<string>();
            Int32 intMaxRows = maxRows.TryParse<Int32>();

            Hashtable searchData = new Hashtable();

            searchData["CustomerID"] = lngCustomerId;

            string conditionXML = GeneralUtility.HashTableToXML(searchData, "customer");
            try
            {
                if (_customerServiceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, intMaxRows, strCulture))
                {
                    if (string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                        {
                            res.ConvertFromXml(resultXml);
                        }
                        _logData.CaptureData("response", res);
                    }
                    else
                    {
                        _logData.CaptureData("errorXml", errorXml);
                    }
                }

                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting Customer Family MasterData by CustomerID", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return res;
        }

        private List<HouseholdCustomerDetails> GetHouseHoldDetailsByCustomer(object customerID, object culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("culture: {0}", culture));
            List<HouseholdCustomerDetails> houseHolds = new List<HouseholdCustomerDetails>();
            HouseholdCustomerDetailsList lstHouseHolds = new HouseholdCustomerDetailsList();
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows;
            maxRows = 0;
            maxRows = 1;
            Int64 lngCustomerId = customerID.TryParse<Int64>();
            string strCulture = culture.TryParse<string>();

            try
            {
                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = lngCustomerId;
                //Preparing parameters for service call
                conditionXml = GeneralUtility.HashTableToXML(searchData, "customer");
                if (_customerServiceClient.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, strCulture))
                {
                    //testing
                    if (string.IsNullOrEmpty(errorXml))
                    // if (!string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                        {
                            lstHouseHolds.ConvertFromXml(resultXml);
                        }
                    }
                }
            //    _logData.CaptureData("HouseHolds List", lstHouseHolds.List);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting Household Details for customer", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return lstHouseHolds.List;
        }

        bool RecordPrintAtHomeDetails(DataSet dsPrintDetails, out string errorXml)
        {
            LogData _logData = new LogData();
            bool res = false;
            try
            {
                res = _customerServiceClient.AddPrintAtHomeDetails(out errorXml, dsPrintDetails);
                if (!string.IsNullOrEmpty(errorXml))
                {
                    _logData.CaptureData("errorXml", errorXml);
                }
                _logData.CaptureData("response", res);
                _logger.Submit(_logData);      
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerService Adapter while getting status for RecordPrintAtHomeDetails", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }   
            return res;
        }

        private bool UpdateCustomerDetails(Hashtable userData, string customerType)
        {
            LogData _logData = new LogData();
            long CustomerID = 0;
            string errorXml = String.Empty;
            string updateXml = String.Empty;
            
            try
            {
                updateXml = GeneralUtility.HashTableToXML(userData, "customer");
                bool bUpdatedCustomerDetails = _customerServiceClient.UpdateCustomerDetails(out errorXml, out CustomerID, updateXml, customerType);

                _logData.CustomerID = CustomerID.ToString();
                _logData.RecordStep(String.Format("IsUpdatedCustomerDetails: {0}", bUpdatedCustomerDetails));

                if(!bUpdatedCustomerDetails || !String.IsNullOrEmpty(errorXml))
                {
                    throw new Exception(String.Format("Error XML: {0}", errorXml));
                }
                    
                _logger.Submit(_logData);
                return true;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details using userdata and customer Type", ex, 
                        new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public bool UpdateCustomerDetails(CustomerFamilyMasterDataUpdate customerData, string consumer)
        {
            string errorXml = string.Empty;
            long customerId;
            bool IsSuccessful = false;
            string customerDataXml = string.Empty;
            LogData _logData = new LogData();
            if (!string.IsNullOrEmpty(Convert.ToString(customerData.CustomerID)))
            {
                _logData.CustomerID = customerData.CustomerID.ToString();
            }
            try
            {                
                if (customerData != null)
                {
                    customerDataXml = SerializerUtility<CustomerFamilyMasterDataUpdate>.GetSerializedString(customerData, customerData.GetXmlAttributeOverrider());
                    if (_customerServiceClient.UpdateCustomerDetails(out errorXml, out customerId, customerDataXml, consumer))
                    {
                        IsSuccessful = true;
                    }
                }
                _logData.CaptureData("IsSuccessful", IsSuccessful);
                _logger.Submit(_logData);  
                return IsSuccessful;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details usign customer Data and consumer", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private CustomerSecurityBlockerStatus GetCustomerVerificationStatus(string customerID)
        {
            CustomerSecurityBlockerStatusList customerSecuritystatusList = new CustomerSecurityBlockerStatusList();
            string error = string.Empty;
            string result = string.Empty;
            LogData _logData = new LogData();
            CustomerSecurityBlockerStatus custSecurityBlockStatus = null;
            try
            {
                if (_customerServiceClient.GetCustomerVerificationDetails(out error, out result, customerID))
                {
                    if (result != string.Empty && result != "<NewDataSet />")
                    {
                        customerSecuritystatusList.ConvertFromXml(result);
                        if (customerSecuritystatusList != null 
                            && customerSecuritystatusList.CustomerSecurityBlockerStatusInstance != null 
                            && customerSecuritystatusList.CustomerSecurityBlockerStatusInstance.Count > 0)
                        {
                            custSecurityBlockStatus = customerSecuritystatusList.CustomerSecurityBlockerStatusInstance[0];
                        }
                    }
                }
                else
                {
                    throw new Exception(error);
                }
                _logData.CaptureData("Response Oject ", customerSecuritystatusList.CustomerSecurityBlockerStatusInstance);
                _logger.Submit(_logData);
                return custSecurityBlockStatus;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adpater while getting Customer Verification Status.", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private long NoteSecurityAttemptInAudit(object securityAttemptAudit)
        {
            CustomerSecurityAttemptAudit objSecurityAudit = (CustomerSecurityAttemptAudit)securityAttemptAudit;
            string errorXml;
            long customerId;
            CustomerServiceClient serviceClient = new CustomerServiceClient();
            string attemptAuditXml = SerializerUtility<CustomerSecurityAttemptAudit>.GetSerializedString(objSecurityAudit);
            LogData _logData = new LogData();
            try
            {

                serviceClient.InsertUpdateCustomerVerificationDetails(out customerId, out errorXml, attemptAuditXml);
                if (!string.IsNullOrEmpty(errorXml))
                {
                    _logData.CaptureData("errorXml", errorXml);
                    throw new Exception(errorXml);
                }
                _logData.CaptureData("customerId", customerId);
                _logger.Submit(_logData);                
                return customerId;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter while getting Customer ID after SecurityAttemptAudit", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }

        }

        /// <summary>
        /// Method to get the customerid from database
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns customerid></returns>
        private string GetCustomerIDbyGUID(string guid)
        {
            string sCustomerID = String.Empty;
            CustomerServiceClient customerSvcClient = new CustomerServiceClient();
            LogData _logData = new LogData();
            try
            {
                sCustomerID = customerSvcClient.ValidateEmailLink(guid);
                _logData.CaptureData("sCustomerID", sCustomerID);
                _logData.CustomerID = sCustomerID;
                _logger.Submit(_logData);
                    
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting CustomerID by GUID", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return sCustomerID;

        }

        /// <summary>
        /// Method to get the configurations from database
        /// </summary>
        /// <param name="configurationTypes"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public DbConfiguration GetConfiguration(string configurationTypes, string locale)
        {
            LogData _logData = new LogData();
            DbConfiguration dbConfigs = new DbConfiguration();
            int rowCount;
            string errorXml = string.Empty, resultXml = string.Empty;
            try
            {
                if (_customerServiceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, configurationTypes, locale))
                {
                    if (!string.IsNullOrEmpty(resultXml))
                    {
                        dbConfigs.ConvertFromXml(resultXml);
                    }
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerService Adapter while getting DB Configurations.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            _logData.CaptureData("apdbConfigspsettings", dbConfigs);
            _logger.Submit(_logData);                
            return dbConfigs;
        }

      

        #endregion
    }
}
