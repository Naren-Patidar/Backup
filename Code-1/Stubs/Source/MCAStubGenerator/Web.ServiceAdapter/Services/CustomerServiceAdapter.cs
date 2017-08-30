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
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class CustomerServiceAdapter : IServiceAdapter
    {
        Recorder _recorder = null;

        #region Constructors

        private ICustomerService _customerServiceClient = new CustomerServiceClient();

        public CustomerServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
        }

        #endregion Constructors

        #region IServiceAdapter Members

        public MCAResponse Get(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            List<string> blacklistData = new List<string>();

            if (request.Parameters.Keys.Contains(ParameterNames.CLUBCARD_NUMBER))
            {
                blacklistData.Add(request.Parameters[ParameterNames.CLUBCARD_NUMBER].ToString());
            }

            try
            {
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
                            res.Status = true;
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
                
            }
            catch (Exception ex)
            {
                throw ex;
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
                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter|SET", ex, new Dictionary<string, object>() 
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
                    case OperationNames.NOTE_SECUREITY_ATTEMPT_AUDIT:

                        if (request.Parameters.Keys.Contains(ParameterNames.SECURITY_ATTEMPT_AUDIT))
                        {
                            res.Data = NoteSecurityAttemptInAudit(request.Parameters[ParameterNames.SECURITY_ATTEMPT_AUDIT]);
                            res.Status = true;
                        }
                        break;
                }
                      
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter|EXECUTE", ex, new Dictionary<string, object>() 
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
         
            if (_customerServiceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, intMaxRows, strCulture))
            {
                this._recorder.RecordResponse(new RecordLog { Result = resultXml, Error = errorXml, RowCount = rowCount },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
                    "GetCustomerDetails", ResponseType.Xml);

                if (string.IsNullOrEmpty(errorXml))
                {
                    res.ConvertFromXml(resultXml);
                }
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
                        res.ConvertFromXml(resultXml);
                    //    _logData.CaptureData("response", res);
                    }
                    else
                    {
                        _logData.CaptureData("errorXml", errorXml);
                    }
                }

                
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

            Hashtable searchData = new Hashtable();
            searchData["CustomerID"] = lngCustomerId;
            //Preparing parameters for service call
            conditionXml = GeneralUtility.HashTableToXML(searchData, "customer");
            if (_customerServiceClient.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, strCulture))
            {
                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
                    "SearchCustomer", ResponseType.Xml);

                //testing
                if (string.IsNullOrEmpty(errorXml))
                // if (!string.IsNullOrEmpty(errorXml))
                {
                    lstHouseHolds.ConvertFromXml(resultXml);
                }
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
            long CustomerID;
            bool IsUpdatedCustomerDetails = false;
            string errorXml = string.Empty;
            string updateXml = GeneralUtility.HashTableToXML(userData, "customer");
            try
            {
                if (_customerServiceClient.UpdateCustomerDetails(out errorXml, out CustomerID, updateXml, customerType))
                {
                    IsUpdatedCustomerDetails = true;
                    if (!string.IsNullOrEmpty(errorXml))
                        throw new Exception(errorXml);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(CustomerID)))
                {
                    _logData.CustomerID = CustomerID.ToString();
                }
                _logData.CaptureData("IsUpdatedCustomerDetails", IsUpdatedCustomerDetails);
                  
                return IsUpdatedCustomerDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details using userdata and customer Type", ex, new Dictionary<string, object>() 
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
                  
                return IsSuccessful;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details usign customer Data and consumer", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private List<CustomerSecurityBlockerStatus> GetCustomerVerificationStatus(string customerID)
        {
            CustomerSecurityBlockerStatusList customerSecuritystatusList = new CustomerSecurityBlockerStatusList();
            string error = string.Empty;
            string result = string.Empty;

            if (_customerServiceClient.GetCustomerVerificationDetails(out error, out result, customerID))
            {
                this._recorder.RecordResponse(new RecordLog { Error = error, Result = result }, 
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
                    "GetCustomerVerificationDetails", ResponseType.Xml);

                if (result != string.Empty && result != "<NewDataSet />")
                {
                    customerSecuritystatusList.ConvertFromXml(result);
                }
            }
            else
            {
                throw new Exception(error);
            }

            return customerSecuritystatusList.CustomerSecurityBlockerStatusInstance;
        }

        private long NoteSecurityAttemptInAudit(object securityAttemptAudit)
        {
            CustomerSecurityAttemptAudit objSecurityAudit = (CustomerSecurityAttemptAudit)securityAttemptAudit;
            string errorXml;
            long customerId;
            CustomerServiceClient serviceClient = new CustomerServiceClient();
            string attemptAuditXml = SerializerUtility<CustomerSecurityAttemptAudit>.GetSerializedString(objSecurityAudit);

            serviceClient.InsertUpdateCustomerVerificationDetails(out customerId, out errorXml, attemptAuditXml);

            this._recorder.RecordResponse(new RecordLog { Error = errorXml, CustomerID = customerId },
               Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
               "InsertUpdateCustomerVerificationDetails", ResponseType.Xml);

            if (!string.IsNullOrEmpty(errorXml))
            {
                throw new Exception(errorXml);
            }

            return customerId;
        }

        /// <summary>
        /// Method to get the customerid from database
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns customerid></returns>
        private string GetCustomerIDbyGUID(string guid)
        {
            CustomerServiceClient customerSvcClient = new CustomerServiceClient();
            string sCustomerID = customerSvcClient.ValidateEmailLink(guid);

            this._recorder.RecordResponse(new RecordLog { CustomerID = sCustomerID.TryParse<long>() },
               Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
               "ValidateEmailLink", ResponseType.Xml);

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
            DbConfiguration dbConfigs = new DbConfiguration();
            int rowCount;
            string errorXml = string.Empty, resultXml = string.Empty;
            if (_customerServiceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, configurationTypes, locale))
            {
                this._recorder.RecordResponse(new RecordLog{ Error = errorXml, Result = resultXml, RowCount = rowCount},
                       Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.CustomerService.ToString(),
                       "GetConfigDetails", ResponseType.Xml);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    dbConfigs.ConvertFromXml(resultXml);
                }
            }

            return dbConfigs;
        }      

        #endregion

        #region IServiceAdapter Members

        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion
    }
}
