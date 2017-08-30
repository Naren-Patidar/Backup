using System;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using AutoMapper;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;


namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class BoostsAtTescoBC : IBoostsAtTescoBC
    {
        IServiceAdapter _rewardServiceAdapter;
        IPDFGenerator _generatePDFforVoucher;
        IServiceAdapter _customerServiceAdapter;
        IAccountBC _accountProvider;
        private readonly ILoggingService _logger;
        string culture = System.Globalization.CultureInfo.CurrentCulture.Name;
        Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.IConfigurationProvider _configProvider = null;

        public BoostsAtTescoBC(
                        IAccountBC accountProvider,
                        IServiceAdapter customerServiceAdapter,
                        IServiceAdapter rewardServiceAdapter,
                        IPDFGenerator generatePDF,
                        ILoggingService logger,
                        Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider.IConfigurationProvider configProvider)
        {
            _accountProvider = accountProvider;
            this._customerServiceAdapter = customerServiceAdapter;
            this._rewardServiceAdapter = rewardServiceAdapter;            
            this._logger = logger;
            this._configProvider = configProvider;
            this._generatePDFforVoucher = generatePDF;         
        }

        #region Public Methods

        /// <summary>
        /// GetRewardAndTokens
        /// </summary>        
        /// <param name="customerId"></param>
        public RewardAndToken GetRewardAndTokens(long customerId)
        {
            LogData _logData = new LogData();
            MCARequest request = new MCARequest();
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_REWARDS_AND_TOKENS);
            request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
            request.Parameters.Add(ParameterNames.CULTURE, culture);
            _logData.RecordStep(string.Format("Method GET_REWARDS_AND_TOKENS :{0}", OperationNames.GET_REWARDS_AND_TOKENS));
            MCAResponse response = _rewardServiceAdapter.Get<RewardAndToken>(request);
            _logData.CaptureData("response", response);
            _logData.RecordStep(string.Format("Response status :{0}", response.Status));
            _logger.Submit(_logData);
            if (response.Status)
            {
                _logData.RecordStep(string.Format("Method GET_REWARDS_AND_TOKENS :{0}", OperationNames.GET_REWARDS_AND_TOKENS));
                _logger.Submit(_logData);
                return response.Data as RewardAndToken;
            }
            else
            {
                _logData.RecordStep(string.Format("Service does not return any Rewards and Token"));
                Exception ex = new Exception();
                ex.Data.Add(ParameterNames.FRIENDLY_ERROR_MESSAGE, "Service does not return any Rewards and Token");
                throw GeneralUtility.GetCustomException("Failed in BoostAtTescoBC while Geting Reward And Tokens.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });             
            }
        }

        public bool IsCurrenltyBCVEPeriod()
        {
            LogData _logData = new LogData();
            DateTime dtTemp = DateTime.Now;
            DbConfiguration configurations = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, System.Globalization.CultureInfo.CurrentCulture.Name);
            DbConfigurationItem exchangeConfig = configurations.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.YourExchangesDates);
            _logData.RecordStep(string.Format("exchangeConfig :{0}", exchangeConfig));
            DateTime? exchangeStartDate = null;
            if (exchangeConfig != null)
            {
                exchangeConfig.ConfigurationValue1.Trim().TryParseDate(out dtTemp);
                exchangeStartDate = dtTemp;
                _logData.RecordStep(string.Format("exchangeStartDate :{0}", exchangeStartDate));
            }
            DateTime? exchangeEndDate = null;
            if (exchangeConfig != null)
            {
                exchangeConfig.ConfigurationValue2.Trim().TryParseDate(out dtTemp);
                exchangeEndDate = dtTemp;
                _logData.RecordStep(string.Format("exchangeEndDate :{0}", exchangeEndDate));
            }         

            if (!exchangeStartDate.HasValue || !exchangeEndDate.HasValue)
            {
                _logData.RecordStep(string.Format("Start and End date has value."));
                _logger.Submit(_logData);
                return false;
            }


            _logger.Submit(_logData);
            return (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) >= exchangeStartDate)
                && (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) <= exchangeEndDate);
        }

        public bool IsExchangeEnabled()
        {
            LogData _logData = new LogData();
            DbConfiguration configurations = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, System.Globalization.CultureInfo.CurrentCulture.Name);
            DbConfigurationItem exchangeConfig = configurations.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.YourExchangesFlag);
            _logData.RecordStep(string.Format("exchangeConfig :{0}", exchangeConfig));
            string exchangeFlag = string.Empty;
            if (exchangeConfig != null)
            {
                exchangeFlag = exchangeConfig.ConfigurationValue1.Trim();
                _logData.RecordStep(string.Format("exchangeFlag :{0}", exchangeFlag));
            }
            if (string.IsNullOrEmpty(exchangeFlag))
            {
                _logData.RecordStep(string.Format("Exchange Flag is null or empty"));
                _logger.Submit(_logData);
                return false;
            }
            _logger.Submit(_logData);
            return exchangeFlag != "1";
        }

        public bool IsUnSpentBoostTokensAvailable(long customerId)
        {
            LogData _logData = new LogData();
            bool result = false;
            try
            {
                if (!IsExchangeEnabled())
                {
                    _logData.RecordStep(string.Format("IsExchangeEnabled :{0}", IsExchangeEnabled()));
                    result = false;
                }
                if (!IsCurrenltyBCVEPeriod())
                {
                    _logData.RecordStep(string.Format("IsCurrenltyBCVEPeriod :{0}", IsCurrenltyBCVEPeriod()));
                    result = false;
                }
                RewardAndToken rewardAndTokenDetails = GetRewardAndTokens(customerId);
                if (rewardAndTokenDetails != null)
                {
                    _logData.CaptureData("rewardAndTokenDetails :{0}", rewardAndTokenDetails);
                    if (rewardAndTokenDetails.Tokens != null && rewardAndTokenDetails.Tokens.Count > 0)
                    {
                        _logData.RecordStep(string.Format("rewardAndTokenDetails.Tokens.Count :{0}", rewardAndTokenDetails.Tokens.Count));
                        result = true;
                    }
                    if (rewardAndTokenDetails.Rewards != null && rewardAndTokenDetails.Rewards.Count > 0)
                    {
                        _logData.RecordStep(string.Format("rewardAndTokenDetails.Rewards.Count :{0}", rewardAndTokenDetails.Rewards.Count));
                        result = true;
                    }
                }
                _logger.Submit(_logData);
                return result;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BoostAtTescoBC while checking UnSpent Boost Tokens Available.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

      

        public bool RecordRewardTokenPrintDetails(List<Token> tokens, long customerId, string tokenFlag)
        {
            DataTable dtPrintDetail = new DataTable();
            string errorXml = string.Empty;
            DataSet dsTokens = new DataSet("DocumentElement");
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("tokenFlag :{0}", tokenFlag));
            _logData.CaptureData("tokenFlag", tokens);
            try
            {
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();

                dtPrintDetail.TableName = "PrintDetails";
                dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
                dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
                dtPrintDetail.Columns.Add("Value", typeof(Decimal));
                dtPrintDetail.Columns.Add("VoucherID", typeof(string));
                dtPrintDetail.Columns.Add("VoucherType", typeof(string));
                dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
                dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
                dtPrintDetail.Columns.Add("Flag", typeof(Char));

                foreach (Token token in tokens)
                {
                    DataRow dr = dtPrintDetail.NewRow();
                    if (tokenFlag == "E")
                        dr["CustomerID"] = DBNull.Value;
                    else
                        dr["CustomerID"] = customerId;
                    dr["PrintDate"] = DateTime.Now;
                    dr["Value"] = token.TokenValue;
                    dr["VoucherID"] = token.SupplierTokenCode;
                    dr["VoucherType"] = string.Empty;
                    dr["ExpiryDate"] = token.ValidUntil;
                    dr["CCNumber"] = 0;
                    dr["Flag"] = tokenFlag;

                    dtPrintDetail.Rows.Add(dr);
                }

               dsTokens.Tables.Add(dtPrintDetail);
               _logData.CaptureData("dtPrintDetail", dtPrintDetail);
               request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
               request.Parameters.Add(ParameterNames.DS_TOKENS, dsTokens);
               _logData.RecordStep(string.Format("Recording details ....."));
               response = this._customerServiceAdapter.Set<Token>(request);
               _logData.CaptureData("response", response);
               _logger.Submit(_logData);
               return response.Status;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BoostAtTescoBC while Recording Reward and Token Print Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            
        }


        public List<Token> GetTokens(Guid gid, long bookingidval, long productlineidval, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("gid :{0} bookingIdVal :{1} productLineIdVal :{2} culture :{3}", gid, bookingidval, productlineidval, culture));  
            List<Token> tokendeatils = null;
            try
            {
                tokendeatils = new List<Token>();
                MCARequest request = new MCARequest();
                MCAResponse response = new MCAResponse();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_TOKENS);
                request.Parameters.Add(ParameterNames.TOKEN_GUID, gid);
                request.Parameters.Add(ParameterNames.BOOKING_ID_VAL, bookingidval);
                request.Parameters.Add(ParameterNames.PRODUCT_LINE_VAL, productlineidval);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                _logData.RecordStep(string.Format("Calling service"));
                response = this._rewardServiceAdapter.Get<List<Token>>(request);
                tokendeatils = response.Data as List<Token>;
                _logData.CaptureData("token details", tokendeatils);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in BoostAtTescoBC while getting Tokens.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            _logger.Submit(_logData);
            return tokendeatils;
        }
        
        #endregion
    }
}
