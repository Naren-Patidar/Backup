using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using System.Xml;
using System.Collections;

using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.JoinLoyaltyService;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.CustomerDetails;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common;
using System.Xml.Linq;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class JoinLoyaltyServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        private IJoinLoyaltyService _joinLoyaltyServiceClient;
        DateTime _dtStart = DateTime.UtcNow; 

        #region Constructors

        public JoinLoyaltyServiceAdapter()
        {
            _joinLoyaltyServiceClient = new JoinLoyaltyServiceClient();
        }

        public JoinLoyaltyServiceAdapter(IJoinLoyaltyService joinLoyaltyServiceClient)
        {
            _joinLoyaltyServiceClient = joinLoyaltyServiceClient;
        }

        #endregion Constructors

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "CreateClubcardAccount", Int64.MinValue
                },
                {
                    "GetAccountContextFromHashTable", new AccountContext(){
                    }
                },
                {
                    "GetAccountContext", new AccountContext(){
                    }
                }
            };
        }

        public string GetName()
        {
            return "joinservice";
        }
        
        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "createclubcardaccount":
                        response.data = this.CreateClubcardAccount(
                                                request.GetParameter<string>("dotcomcustomerid"),
                                                request.GetParameter<string>("userdatatext"),
                                                request.GetParameter<string>("joinroutecode"),
                                                request.GetParameter<string>("culture"));
                        break;

                    case "getaccountcontextfromhashtable":
                        response.data = this.GetAccountContextFromHashTable(
                                                request.GetParameter<string>("userdatatext"));
                        break;
                    case "getaccountcontext":
                        response.data = this.GetAccountContext(
                                                request.GetParameter<string>("customerdatatext"));
                        break;

                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-JOINLOYALITY-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion

        #region private methods
    
        private long CreateClubcardAccount(string dotcomCustomerId, string userDataText, string joinRouteCode, string culture)
        {
            try
            {
                long lDotcomCustomerID = dotcomCustomerId.TryParse<long>();
                if (lDotcomCustomerID == default(long))
                {
                    throw new Exception("Parameter dotcomCustomerID is mandatory and must be passed for further processing.");
                }

                Hashtable userData = JsonConvert.DeserializeObject<Hashtable>(userDataText);
                string updateXml = GeneralUtility.HashTableToXML(userData, "customer");
                XmlDocument resulDoc = new XmlDocument();
                string resultxml = String.Empty;

                this._dtStart = DateTime.UtcNow;
                
                try
                {
                    resultxml = _joinLoyaltyServiceClient.AccountCreate(lDotcomCustomerID, updateXml, joinRouteCode, culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }
                
                long clubcard = 0;

                if (!string.IsNullOrEmpty(resultxml))
                {
                    XDocument xDoc = XDocument.Parse(resultxml);
                    if (xDoc.Element("NewDataSet").Element("Clubcard").HasElements
                        && !string.IsNullOrEmpty(xDoc.Element("NewDataSet").Element("Clubcard").Element("Column1").Value))
                    {
                        clubcard = xDoc.Element("NewDataSet").Element("Clubcard").Element("Column1").Value.TryParse<Int64>();
                    }
                }

                return clubcard;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service CreateClubcardAccount Method while getting the Clubcard ", ex,
                  null);
            }
        }

        private AccountContext GetAccountContextFromHashTable(string userDataText)
        {
            string resultXml = String.Empty;
            AccountContext context = new AccountContext();
            
            try
            {
                Hashtable userData = JsonConvert.DeserializeObject<Hashtable>(userDataText);
                string updateXml = GeneralUtility.HashTableToXML(userData, "customer");

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._joinLoyaltyServiceClient.AccountDuplicateCheck(out resultXml, updateXml);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (String.IsNullOrWhiteSpace(resultXml))
                {
                    throw new Exception("Failed to get GetAccountContext data. No more details available form service.");
                }
                else
                {
                    context.ConvertFromXml(resultXml);
                }
                
                return context;
            }
            catch (Exception exception)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service GetAccountContext Method ", exception,
                  null);
            }
        }

        private AccountContext GetAccountContext(string customerDataText)
        {
            string resultXml = String.Empty;
            AccountContext context = new AccountContext();
            
            try
            {
                CustomerFamilyMasterDataUpdate customerData = JsonConvert.DeserializeObject<CustomerFamilyMasterDataUpdate>(customerDataText);
                string strXml = SerializerUtility<CustomerFamilyMasterDataUpdate>.GetSerializedString(customerData);

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._joinLoyaltyServiceClient.AccountDuplicateCheck(out resultXml, strXml);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (String.IsNullOrWhiteSpace(resultXml))
                {
                    throw new Exception("Failed to get GetAccountContext data. No more details available form service.");
                }
                else
                {
                    context.ConvertFromXml(resultXml);
                }
                return context;
            }
            catch (Exception exception)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service GetAccountContext Method ", exception,
                  null);
            }
        }

        #endregion Private Methods
    }
}