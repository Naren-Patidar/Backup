using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Activation = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers; 

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class CustomerActivationServiceAdapter : IServiceAdapter
    {
        private ILoggingService _logger = null;

        #region Constructors

        private IClubcardOnlineService _activationServiceClient;

        public CustomerActivationServiceAdapter(IClubcardOnlineService activationServiceClient, ILoggingService logger)
        {
            _activationServiceClient = activationServiceClient;
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
            LogData _logData = new LogData();
            MCAResponse res = new MCAResponse();
            try
            {
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                switch (operation)
                {
                    case OperationNames.GET_CLUBCARD_ACCOUNT_DETAILS:
                        _logData.RecordStep(string.Format("GET_CLUBCARD_ACCOUNT_DETAILS :{0}", OperationNames.GET_CLUBCARD_ACCOUNT_DETAILS));
                        if (request.Parameters.ContainsKey(ParameterNames.CLUBCARD_NUMBER) &&
                            request.Parameters.ContainsKey(ParameterNames.CUSTOMER_ENTITY) &&
                            request.Parameters.ContainsKey(ParameterNames.DB_CONFIGURATION))
                        {
                            res.Data = GetClubcardAccountDetails((request.Parameters[ParameterNames.CLUBCARD_NUMBER]).TryParse<Int64>(), (request.Parameters[ParameterNames.CUSTOMER_ENTITY]) as Activation.ClubcardCustomer, (request.Parameters[ParameterNames.DB_CONFIGURATION]) as DBConfigurations);
                            res.Status = true;                           
                        }
                        break;
                    case OperationNames.REGISTER_DOTCOMID_CUSTOMERACCOUNT:
                        _logData.RecordStep(string.Format("REGISTER_DOTCOMID_CUSTOMERACCOUNT :{0}", OperationNames.REGISTER_DOTCOMID_CUSTOMERACCOUNT));
                        res.Data = RegisterDotcomIdToCustomerAccount((request.Parameters[ParameterNames.DOTCOM_CUSTOMER_ID]).ToString(), (request.Parameters[ParameterNames.CLUBCARD_NUMBER]).TryParse<Int64>());
                        res.Status = true;
                        break;

                    case OperationNames.SEND_ACTIVATION_EMAIL:
                        _logData.RecordStep(string.Format("SEND_ACTIVATION_EMAIL :{0}", OperationNames.SEND_ACTIVATION_EMAIL));
                        res.Status = SendActivationEmail((request.Parameters[ParameterNames.EMAIL_ID_TO]).ToString());
                        break;
                }
                _logger.Submit(_logData);

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { "InputParam", request.JsonText() }
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
            throw new NotImplementedException();
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

        #region IActivation Members

        private AccountFindByClubcardNumberResponse GetClubcardAccountDetails(long clubcardNumber, Activation.ClubcardCustomer customerEntity, DBConfigurations dbConfigurations)
        {
            LogData _logData = new LogData();           
            AccountFindByClubcardNumberResponse accountFindResponse = new AccountFindByClubcardNumberResponse();            
            ClubcardCustomer custEntity = ClubcardCustomerToServiceObject(customerEntity);
            DataSet dsConfiguration = new DataSet();
            try
            {
                DataTable dtConfigurationItems = new DataTable("ActiveDateRangeConfig");
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationType.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationName.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue1.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue2.ToString());

                foreach (KeyValuePair<string, DbConfigurationItem> conf in dbConfigurations.Instance)
                {
                    dtConfigurationItems.Rows.Add(new object[] { (int)conf.Value.ConfigurationType, conf.Value.ConfigurationName, conf.Value.ConfigurationValue1, conf.Value.ConfigurationValue2 });
                }

                dtConfigurationItems =  dtConfigurationItems.AsEnumerable()
                                        .Where(r => r.Field<string>("ConfigurationValue1") == "1")
                                        .CopyToDataTable();

                dsConfiguration.Tables.Add(dtConfigurationItems);
                dsConfiguration.Tables[0].TableName = "ActiveDateRangeConfig";
                _logData.CaptureData("dtConfigurationItems :{0}", dtConfigurationItems);
                accountFindResponse = _activationServiceClient.AccountFindByClubcardNumber(clubcardNumber, custEntity, dsConfiguration);
                _logData.CaptureData("GetClubcardAccountDetails accountFindResponse", accountFindResponse);
                _logger.Submit(_logData);
                return accountFindResponse;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while updating customer preferences.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
           
        }

        private AccountLinkResponse RegisterDotcomIdToCustomerAccount(string dotcomCustomerID, long clubcardNumber)
        {
            LogData _logData = new LogData();      
            try
            {
                _logData.RecordStep(string.Format("dotcomCustomerID :{0}", dotcomCustomerID));

                if (!string.IsNullOrEmpty(dotcomCustomerID))
                {
                    AccountLinkResponse accountLinkResponse = _activationServiceClient.IGHSAccountLink(dotcomCustomerID, clubcardNumber);
                    _logData.CaptureData("RegisterDotcomIdToCustomerAccount accountLinkResponse", accountLinkResponse);
                    _logger.Submit(_logData);

                    return accountLinkResponse;
                }
                else
                {
                    throw new Exception("Passing invalid Dotcom ID - Dotcom ID: " + dotcomCustomerID); 
                }
                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while Registering customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
           
        }

        private bool SendActivationEmail(string toEmailId)
        {
            LogData _logData = new LogData();
            ClubcardOnlineServiceClient clientRegistration = null;
            try
            {
                clientRegistration = new ClubcardOnlineServiceClient();
                bool isMailSent = clientRegistration.SendActivationEmail(toEmailId);
                _logData.RecordStep(string.Format("isMailSent :{0}", isMailSent));
                _logger.Submit(_logData);
                return isMailSent;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while Sending EMail to customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
          
        }

        #endregion

        private ClubcardCustomer ClubcardCustomerToServiceObject(Activation.ClubcardCustomer obj)
        {
            LogData _logData = new LogData();
            ClubcardCustomer serviceObj = new ClubcardCustomer();
            try
            {
                serviceObj.Address = new Address();
                serviceObj.Address.PostCode = obj.Address.PostCode;
                serviceObj.FirstName = obj.FirstName;
                serviceObj.Surname = obj.Surname;
                serviceObj.Title = obj.Title;
                serviceObj.DayOfBirth = obj.DayOfBirth;
                serviceObj.MonthOfBirth = obj.MonthOfBirth;
                serviceObj.YearOfBirth = obj.YearOfBirth;
                serviceObj.DateOfBirth = obj.DateOfBirth;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ClubcardCustomerToServiceObject method.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return serviceObj;
        }

        private DataSet GetConfigurationDataSet(DbConfiguration configuration)
        {
            LogData _logData = new LogData();
            try
            {
                DataSet dsConfiguration = new DataSet();
                DataTable dtConfigurationItems = dsConfiguration.Tables.Add("ActiveDateRangeConfig");

                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationType.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationName.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue1.ToString());
                dtConfigurationItems.Columns.Add(DbConfigurationItemEnum.ConfigurationValue2.ToString());

                foreach (DbConfigurationItem configurationItem in configuration.ConfigurationItems)
                {
                    DataRow dr = dtConfigurationItems.NewRow();

                    dr[DbConfigurationItemEnum.ConfigurationType.ToString()] = Convert.ToString((int)configurationItem.ConfigurationType);
                    dr[DbConfigurationItemEnum.ConfigurationName.ToString()] = configurationItem.ConfigurationName;
                    dr[DbConfigurationItemEnum.ConfigurationValue1.ToString()] = configurationItem.ConfigurationValue1;
                    dr[DbConfigurationItemEnum.ConfigurationValue2.ToString()] = configurationItem.ConfigurationValue2;

                    dtConfigurationItems.Rows.Add(dr);
                }

                dtConfigurationItems.AcceptChanges();
                _logData.CaptureData("dtConfigurationItems", dtConfigurationItems);
                _logger.Submit(_logData);
                return dsConfiguration;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CustomerActivationServiceAdapter while creating Dataset for Activation Configuration Items.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }           
        }
    }
}
