using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.ServiceModel;
using System.IO;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Security;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class CustomerServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        private ICustomerService _customerServiceClient;
        DateTime _dtStart = DateTime.UtcNow;
        double _internalStats = 0;

        #region Constructors

        public CustomerServiceAdapter()
        {
        }

        public CustomerServiceAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            this._customerServiceClient = new CustomerServiceClient();
        }

        #endregion Constructors

        #region IServiceAdapter Members
        
        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetHouseHoldDetailsByCustomer", new List<HouseholdDetails>(){
                        { 
                            new HouseholdDetails(){
                            }
                        }
                    }
                },
                {
                    "GetCustomerVerificationStatus", new CustomerSecurityBlockerStatus(){
                    }
                },
                {
                    "GetConfiguration", new DbConfiguration(){
                        ConfigurationItems = new List<DbConfigurationItem>(){
                            {
                                new DbConfigurationItem()
                            }
                        },
                        Languages = new List<ISOLanguage>(){
                            {
                                new ISOLanguage()
                            }
                        },
                        Province = new List<Province>(){
                            {
                                new Province()
                            }
                        },
                        Races = new List<Race>(){
                            {
                                new Race()
                            }
                        }
                    }
                },
                { "GetCustomerIDbyGUID", null },
                { 
                    "GetCustomerFamilyMasterDataByCustomerId", new CustomerFamilyMasterData(
                        new List<CustomerMasterData>(){
                            { new CustomerMasterData() }
                        },
                        new List<FamilyMasterData>(){
                            { new FamilyMasterData() }
                        }, 1 )
                },
                { "RecordPrintAtHomeDetails", bool.FalseString },
                { "UpdateCustomerDetailsFromHashTable", bool.FalseString },
                { "UpdateCustomerDetails", bool.FalseString },
                { "NoteSecurityAttemptInAudit", bool.FalseString }
            };
        }

        public string GetName()
        {
            return "customerservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "gethouseholddetailsbycustomer":
                        response.data = this.GetHouseHoldDetailsByCustomer();
                        break;

                    case "getcustomerverificationstatus":
                        response.data = this.GetCustomerVerificationStatus();
                        break;
                    case "getconfiguration":
                        response.data = this.GetConfiguration(request.GetParameter<string>("configurationTypes"));
                        break;
                    case "getcustomeridbyguid":
                        response.data = this.GetCustomerIDbyGUID(
                                                request.GetParameter<string>("guid"));
                        break;
                    case "getcustomerfamilymasterdatabycustomerid":
                        response.data = this.GetCustomerFamilyMasterDataByCustomerId(request.GetParameter<string>("maxrows"));
                        break;
                    case "recordprintathomedetails":
                        response.data = this.RecordPrintAtHomeDetails(
                                                request.GetParameter<string>("dsPrintDetailsText"));
                        break;
                    case "updatecustomerdetailsfromhashtable":
                        response.data = this.UpdateCustomerDetailsFromHashTable(
                                                request.GetParameter<string>("userdatatext"),
                                                request.GetParameter<string>("customertype"));
                        break;
                    case "updatecustomerdetails":
                        response.data = this.UpdateCustomerDetails(
                                                request.GetParameter<string>("customerdatatext"),
                                                request.GetParameter<string>("consumer"));
                        break;
                    case "notesecurityattemptinaudit":
                        response.data = this.NoteSecurityAttemptInAudit(
                                                request.GetParameter<string>("securityattemptaudit"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-CLUBCARD-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion
        
        #region Private Methods

        CustomerFamilyMasterData GetCustomerFamilyMasterDataByClubcard(string clubcardNumber, string maxRows, string culture)
        {
            CustomerFamilyMasterData objCustFamData = new CustomerFamilyMasterData();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 0;
            Int32 intMaxRows = maxRows.TryParse<Int32>();
            Hashtable searchData = new Hashtable();
            
            try
            {
                long lClubcardNumber = clubcardNumber.TryParse<long>();
                if (lClubcardNumber == default(long))
                {
                    throw new Exception("Parameter clubcardNumber is mandatory and must be passed for further processing.");
                }

                searchData["cardAccountNumber"] = lClubcardNumber;
                string conditionXML = GeneralUtility.HashTableToXML(searchData, "customer");

                bool bService = false;

                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._customerServiceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, intMaxRows, culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml); 
                
                if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                {
                    objCustFamData.ConvertFromXml(resultXml);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter while getting Customer Details.", ex, null);
            }

            return objCustFamData;
        }

        CustomerFamilyMasterData GetCustomerFamilyMasterDataByCustomerId(string maxRows)
        {
            CustomerFamilyMasterData res = new CustomerFamilyMasterData();
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            int rowCount = 0;
            Int32 intMaxRows = maxRows.TryParse<Int32>();
            Hashtable searchData = new Hashtable();
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerId = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerId == default(long))
                {
                    throw new Exception("Parameter customerId is mandatory and must be passed for further processing.");
                }

                searchData["CustomerID"] = lCustomerId;

                string conditionXML = GeneralUtility.HashTableToXML(searchData, "customer");

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._customerServiceClient.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, intMaxRows, this.Culture);
                }
                catch (Exception)
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                {
                    res.ConvertFromXml(resultXml);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting Customer Family MasterData by CustomerID", ex, null);
            }
            return res;
        }

        public List<SearchCustomerDetails> GetHouseHoldDetailsByCustomer()
        {
            List<SearchCustomerDetails> houseHolds = new List<SearchCustomerDetails>();
            SearchCustomerDetailsList lstHouseHolds = new SearchCustomerDetailsList();
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows;
            maxRows = 0;
            maxRows = 1;
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter CustomerID is mandatory and must be passed for further processing.");
                }

                Hashtable searchData = new Hashtable();
                searchData["CustomerID"] = lCustomerID;

                //Preparing parameters for service call
                conditionXml = GeneralUtility.HashTableToXML(searchData, "customer");

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._customerServiceClient.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!string.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                {
                    lstHouseHolds.ConvertFromXml(resultXml);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting Household Details for customer", ex, null);
            }
            return lstHouseHolds.List;
        }

        bool RecordPrintAtHomeDetails(string dsPrintDetailsText)
        {
            bool res = false;
            try
            {
                string errorXml = String.Empty;
                DataSet dsPrintDetails = JsonConvert.DeserializeObject<DataSet>(dsPrintDetailsText);

                this._dtStart = DateTime.UtcNow;
                try
                {
                    res = this._customerServiceClient.AddPrintAtHomeDetails(out errorXml, dsPrintDetails);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }
                
                if (!string.IsNullOrEmpty(errorXml))
                {
                    throw new Exception(errorXml);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerService Adapter while getting status for RecordPrintAtHomeDetails", ex, null);
            }   
            return res;
        }

        private bool UpdateCustomerDetailsFromHashTable(string userDataText, string customerType)
        {
            long CustomerID = 0;
            string errorXml = String.Empty;
            string updateXml = String.Empty;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                Hashtable userData = JsonConvert.DeserializeObject<Hashtable>(userDataText);

                userData["CustomerID"] = custInfo.ngccustomerid;

                updateXml = GeneralUtility.HashTableToXML(userData, "customer");

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._customerServiceClient.UpdateCustomerDetails(out errorXml, out CustomerID, updateXml, customerType);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                return true;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details using userdata and customer Type", ex,
                        null);
            }
        }

        public bool UpdateCustomerDetails(string customerDataText, string consumer)
        {
            string errorXml = string.Empty;
            long customerId;
            string customerDataXml = string.Empty;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                CustomerFamilyMasterDataUpdate customerData = JsonConvert.DeserializeObject<CustomerFamilyMasterDataUpdate>(
                                                                                customerDataText,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (customerData != null)
                {
                    customerData.CustomerID = custInfo.ngccustomerid.TryParse<long>();

                    customerDataXml = SerializerUtility<CustomerFamilyMasterDataUpdate>.GetSerializedString(customerData, customerData.GetXmlAttributeOverrider());

                    bool bService = false;
                    this._dtStart = DateTime.UtcNow;

                    try
                    {
                        bService = this._customerServiceClient.UpdateCustomerDetails(out errorXml, out customerId, customerDataXml, consumer);
                    }
                    finally
                    {
                        this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                    }

                    this.HandleFailedResponse(bService, errorXml);

                    return true;
                }
                throw new Exception("Failed to execute UpdateCustomerDetails. Customer Data is null.");
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while updating Customer Details usign customer Data and consumer", ex,
                   null);
            }
        }

        private CustomerSecurityBlockerStatus GetCustomerVerificationStatus()
        {
            CustomerSecurityBlockerStatusList customerSecuritystatusList = new CustomerSecurityBlockerStatusList();
            string error = string.Empty;
            string result = string.Empty;
            
            CustomerSecurityBlockerStatus custSecurityBlockStatus = new CustomerSecurityBlockerStatus();
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                this._dtStart = DateTime.UtcNow;
                bool bService = false;

                try
                {
                    bService = this._customerServiceClient.GetCustomerVerificationDetails(out error, out result, custInfo.ngccustomerid);
                }
                finally
                {
                   this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, error);

                if (!String.IsNullOrWhiteSpace(result) && result != "<NewDataSet />")
                {
                    customerSecuritystatusList.ConvertFromXml(result);
                    if (customerSecuritystatusList != null
                        && customerSecuritystatusList.CustomerSecurityBlockerStatusInstance != null
                        && customerSecuritystatusList.CustomerSecurityBlockerStatusInstance.Count > 0)
                    {
                        custSecurityBlockStatus = customerSecuritystatusList.CustomerSecurityBlockerStatusInstance[0];
                    }
                }

                return custSecurityBlockStatus;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adpater while getting Customer Verification Status.", ex, 
                    null);
            }
        }

        private bool NoteSecurityAttemptInAudit(string securityAttemptAudit)
        {
            string errorXml;
            long customerId;
            CustomerServiceClient serviceClient = new CustomerServiceClient();

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                CustomerSecurityAttemptAudit objSecurityAudit = JsonConvert.DeserializeObject<CustomerSecurityAttemptAudit>(securityAttemptAudit);

                objSecurityAudit.CustomerID = custInfo.ngccustomerid.TryParse<long>();

                string attemptAuditXml = SerializerUtility<CustomerSecurityAttemptAudit>.GetSerializedString(objSecurityAudit);

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = serviceClient.InsertUpdateCustomerVerificationDetails(out customerId, out errorXml, attemptAuditXml);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                return true;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerServiceAdapter while getting Customer ID after SecurityAttemptAudit", ex, null);
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
            this._dtStart = DateTime.UtcNow;

            try
            {
                sCustomerID = customerSvcClient.ValidateEmailLink(guid);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Customer Service Adapter while getting CustomerID by GUID", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
            return sCustomerID;
        }

        /// <summary>
        /// Method to get the configurations from database
        /// </summary>
        /// <param name="configurationTypes"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public DbConfiguration GetConfiguration(string configurationTypes)
        {
            DbConfiguration dbConfigs = new DbConfiguration();
            int rowCount;
            string errorXml = string.Empty, resultXml = string.Empty;
            this._dtStart = DateTime.UtcNow;

            try
            {
                bool bService = false;

                try
                {
                    bService = this._customerServiceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, configurationTypes, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);
                
                if (!string.IsNullOrEmpty(resultXml))
                {
                    dbConfigs.ConvertFromXml(resultXml);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerService Adapter while getting DB Configurations.", ex, null);
            }

            return dbConfigs;
        }

        protected override string GetHouseHoldID()
        {
            var custInfo = this.GetCustInfo();

            if (String.IsNullOrWhiteSpace(custInfo.householdid))
            {
                var houseHolds = this.GetHouseHoldDetailsByCustomer();
                if (houseHolds != null && houseHolds.Count > 0)
                {
                    custInfo.householdid = houseHolds[0].HouseHoldID;
                    this.SaveCustomerInfoInCache(custInfo);
                    return houseHolds[0].HouseHoldID;
                }
            }
            return custInfo.householdid;
        }

        #endregion

    }
}