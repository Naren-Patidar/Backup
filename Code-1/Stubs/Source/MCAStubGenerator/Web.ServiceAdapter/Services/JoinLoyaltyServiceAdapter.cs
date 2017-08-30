using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.ServiceModel;
using System.Xml;
using System.Collections;

using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.JoinLoyaltyService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Xml.Linq;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class JoinLoyaltyServiceAdapter : IServiceAdapter
    {
        private IJoinLoyaltyService _joinLoyaltyServiceClient;
        private readonly ILoggingService _logger = null;

        #region Constructors        

        public JoinLoyaltyServiceAdapter(IJoinLoyaltyService joinLoyaltyServiceClient, ILoggingService logger)
        {
            _joinLoyaltyServiceClient = joinLoyaltyServiceClient;
            _logger = logger;
        }

        #endregion Constructors

        #region IServiceAdapter Members

        /// <summary>
        /// Data retrieval call for Join Loyalty Service
        /// Methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Get<T>(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();
            try
            {
                var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();
                logData.RecordStep(string.Format("Executing Operation : {0}", operation));
                //Not Tracking request object as it may contain customer sensitive data
                switch (operation)
                {
                    case OperationNames.GET_ACCOUNT_CONTEXT:
                        if (request.Parameters.Keys.Contains(ParameterNames.USER_DATA))
                        {
                            res.Data = this.GetAccountContext((request.Parameters[ParameterNames.USER_DATA]) as Hashtable);
                            res.Status = true;
                            logData.RecordStep("Response received successfully. Result is passed.");
                            logData.CaptureData("Response Object", res);
                        }
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_DATA))
                        {
                            res.Data = this.GetAccountContext((request.Parameters[ParameterNames.CUSTOMER_DATA]) as CustomerFamilyMasterDataUpdate);
                            res.Status = true;
                            logData.RecordStep("Response received successfully. Result is passed.");
                            logData.CaptureData("Response Object", res);
                        }
                        break;
                }
                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service GET Method ", ex,
                   new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
                    });
            }
            return res;
        }

        /// <summary>
        /// Data update call of Join Loyalty Service methods
        /// returning boolean value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Set<T>(Common.Entities.Service.MCARequest request)
        {          
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();
            try
            {
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                logData.RecordStep(string.Format("Executing Operation : {0}", operation));
                switch (operation)
                {
                    case OperationNames.CREATE_CLUBCARD_ACCOUNT:
                        logData.CustomerID = (request.Parameters[ParameterNames.DOTCOM_CUSTOMER_ID]).TryParse<string>();
                        res.Data = CreateClubcardAccount((request.Parameters[ParameterNames.DOTCOM_CUSTOMER_ID]).TryParse<Int64>(), (request.Parameters[ParameterNames.USER_DATA]) as Hashtable, (request.Parameters[ParameterNames.JOIN_ROUTE_CODE]).TryParse<string>(), (request.Parameters[ParameterNames.CULTURE]).TryParse<string>());
                        res.Status = true;
                        logData.RecordStep("Response received successfully. Result is passed.");
                        break;
                }
                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service SET Method ", ex,
                new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, logData}
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
            throw new NotImplementedException();
        }

        #endregion

        #region private methods        
    
        private long CreateClubcardAccount(long dotcomCustomerID, Hashtable userData, string joinRouteCode, string culture)
        {
            LogData _logData = new LogData();            
            try
            {
                _logData.CaptureData("Join Route Code", joinRouteCode);
                string updateXml = GeneralUtility.HashTableToXML(userData, "customer");
                XmlDocument resulDoc = new XmlDocument();
                string resultxml;
                _logData.CaptureData("current culture", culture);
                _logData.CaptureData("Join Route Code", joinRouteCode);
                resultxml = _joinLoyaltyServiceClient.AccountCreate(dotcomCustomerID, updateXml, joinRouteCode, culture);
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

                _logData.RecordStep("Response received successfully. Result is passed.");

                
                return clubcard;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service CreateClubcardAccount Method while getting the Clubcard ", ex,
                  new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });

            }
         
        }

        private AccountContext GetAccountContext(Hashtable userData)
        {
            LogData _logData = new LogData();
            string resultXml = String.Empty;
            AccountContext context = new AccountContext();
            try
            {
                string updateXml = GeneralUtility.HashTableToXML(userData, "customer");
                _logData.RecordStep("Invoking Service AccountDuplicateCheck Method");
                if (_joinLoyaltyServiceClient.AccountDuplicateCheck(out resultXml, updateXml))
                {
                    _logData.RecordStep("Response from joinLoyaltyService received successfully");
                    _logData.CaptureData("resultXml", resultXml);
                    context.ConvertFromXml(resultXml);
                }
                
                return context;
            }
            catch (Exception exception)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service GetAccountContext Method ", exception,
                  new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
        }

        private AccountContext GetAccountContext(CustomerFamilyMasterDataUpdate customerData)
        {
            LogData _logData = new LogData();
            string resultXml = String.Empty;
            AccountContext context = new AccountContext();
            try
            {
                string strXml = SerializerUtility<CustomerFamilyMasterDataUpdate>.GetSerializedString(customerData);
                _logData.RecordStep("Invoking Service AccountDuplicateCheck Method");
                if (_joinLoyaltyServiceClient.AccountDuplicateCheck(out resultXml, strXml))
                {
                    _logData.RecordStep("Response from joinLoyaltyService received successfully");
                    _logData.CaptureData("resultXml", resultXml);
                    context.ConvertFromXml(resultXml);
                }
                
                return context;
            }
            catch (Exception exception)
            {
                throw GeneralUtility.GetCustomException("Failed in Join Loyalty Service GetAccountContext Method ", exception,
                  new Dictionary<string, object>()
                    {
                        {LogConfigProvider.EXCLOGDATAKEY, _logData}
                    });
            }
        }

        #endregion Private Methods

        #region IServiceAdapter Members


        public MCAResponse Get(MCARequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServiceAdapter Members


        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
