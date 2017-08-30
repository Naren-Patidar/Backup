using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;
using System.Xml;
using System.Data;
using System.Globalization;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.RewardService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class RewardServiceAdapter : IServiceAdapter
    {
        IRewardService _rewardserviceClient = null;
        private readonly ILoggingService _logger = null;   

        public RewardServiceAdapter(IRewardService rewardserviceClient, ILoggingService logger)
        {
            _rewardserviceClient = rewardserviceClient;
            _logger = logger;
        }

        #region IServiceAdapter Members
        /// <summary>
        /// Data retrieval call for Reward Service
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
                logData.CaptureData("Request Object", request);
                var operation = request.Parameters.Keys.Contains(ParameterNames.OPERATION_NAME) ? request.Parameters[ParameterNames.OPERATION_NAME].ToString() : string.Empty;
                switch (operation)
                {
                    case OperationNames.GET_REWARDS_AND_TOKENS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetRewardAndTokens(
                                    request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<long>(),
                                    request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;

                            logData.RecordStep("Successful Response Received from the Service call.");
                            logData.CaptureData("GetRewardAndTokens Response Object :", res);

                        }
                        break;
                    case OperationNames.GET_TOKENS:
                        if (request.Parameters.Keys.Contains(ParameterNames.TOKEN_GUID) &&
                            request.Parameters.Keys.Contains(ParameterNames.BOOKING_ID_VAL) &&
                            request.Parameters.Keys.Contains(ParameterNames.PRODUCT_LINE_VAL) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetTokens(request.Parameters[ParameterNames.TOKEN_GUID].TryParse<System.Guid>(),
                                request.Parameters[ParameterNames.BOOKING_ID_VAL].TryParse<long>(),
                                request.Parameters[ParameterNames.PRODUCT_LINE_VAL].TryParse<long>(),
                                request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                            logData.RecordStep("Successful Response Received from the Service call.");
                            logData.CaptureData("GET_TOKENS Response Object :", res);

                        }
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter. ", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

            return res;
        }

        public MCAResponse Set<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Delete<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        public MCAResponse Execute(MCARequest request)
        {
            throw new NotImplementedException();
        }

        #endregion IServiceAdapter Members

        private RewardAndToken GetRewardAndTokens(long customerID, string culture)
        {
            LogData _logData = new LogData();
            try
            {
                string resultXml;
                string errorXml;
                RewardAndToken rewardDetails = new RewardAndToken();
                RewardList rewards = new RewardList();
                TokenList tokens = new TokenList();
                rewardDetails.Rewards = rewards.RewardsList;
                rewardDetails.Tokens = tokens.TokensList;
             
                if (_rewardserviceClient.GetRewardDetail(out errorXml, out resultXml, customerID, culture))
                {
                    _logData.CaptureData("GetRewardAndTokens rewardDetails :{0}", rewardDetails);
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        rewards.ConvertFromXml(resultXml);
                        tokens.ConvertFromXml(resultXml);
                        rewardDetails.Rewards = rewards.RewardsList;
                        rewardDetails.Tokens = tokens.TokensList;
                    }
                }
                _logData.CaptureData("Result GetRewardAndTokens rewardDetails :{0}", rewardDetails);
                _logger.Submit(_logData);
                return rewardDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting rewards and tokens . ", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }


        private List<Token> GetTokens(Guid gid, long bookingIdVal, long productLineIdVal, string culture)
        {
            
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("gid :{0} bookingIdVal :{1} productLineIdVal :{2} culture :{3}", gid, bookingIdVal, productLineIdVal, culture));       
            try
            {
                DataSet dsTokenDetails = GetTokenDataSet(gid, bookingIdVal, productLineIdVal, culture);
                List<Token> tokenDetails = GetTokens(dsTokenDetails);
                _logData.CaptureData("Result GetTokens tokenDetails :{0}", tokenDetails);
                _logger.Submit(_logData);
                return tokenDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens . ", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }
        private DataSet GetTokenDataSet(Guid gid, long bookingIdVal, long productLineIdVal, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("gid :{0} bookingIdVal :{1} productLineIdVal :{2} culture :{3}", gid, bookingIdVal, productLineIdVal, culture));     
            try
            {
                XmlDocument resulDoc = null;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                DataSet dsCodes = null;
                 _logData.RecordStep(string.Format("Geting tokens info ..")); 
                if (_rewardserviceClient.GetTokenInfo(out errorXml, out resultXml, gid, bookingIdVal, productLineIdVal, culture))
                {
                    _logData.RecordStep(string.Format("Result tokens info .."));  
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsCodes = new DataSet();
                    dsCodes.ReadXml(new XmlNodeReader(resulDoc));
                }
                _logData.CaptureData("Result Dataset for tokenDetails :{0}", dsCodes);
                _logger.Submit(_logData);
                return dsCodes;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens dataset. ", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }

        }
        private List<Token> GetTokens(DataSet dstokenDetails)
        {
            LogData _logData = new LogData();
            _logData.CaptureData("Dataset dstokenDetail", dstokenDetails);
            try
            {
                if (dstokenDetails == null)
                    return new List<Token>();
                DataTable tokenDetailsTable = dstokenDetails.Tables["TokenInfo"];
                if (tokenDetailsTable == null)
                    return new List<Token>();
                tokenDetailsTable.AddMissingColumns(typeof(TokenEnum));
                IEnumerable<Token> tokenDetails = tokenDetailsTable.AsEnumerable().Select(r => new Token()
                {
                    BookingDate = Convert.ToDateTime(r[TokenEnum.BookingDate.ToString()]),
                    TokenID = Convert.ToInt64(r[TokenEnum.TokenId.ToString()]),
                    TokenDescription = Convert.ToString(r[TokenEnum.TokenDescription.ToString()]),
                    ProductStatus = Convert.ToString(r[TokenEnum.ProductStatus.ToString()]),
                    TokenValue = Convert.ToDecimal(r[TokenEnum.TokenValue.ToString()]),
                    SupplierTokenCode = Convert.ToString(r[TokenEnum.SupplierTokenCode.ToString()]),
                    ValidUntil = Convert.ToDateTime(r[TokenEnum.ValidUntil.ToString()]),
                    QualifyingSpend = Convert.ToString(r[TokenEnum.QualifyingSpend.ToString()]),
                    Includes = Convert.ToString(r[TokenEnum.Includes.ToString()]),
                    Excludes = Convert.ToString(r[TokenEnum.Excludes.ToString()]),
                    TermsAndConditions = Convert.ToString(r[TokenEnum.TermsAndConditions.ToString()])
                });

                _logData.CaptureData("Token Details List :{0}", tokenDetails.ToList());
                _logger.Submit(_logData);
                return tokenDetails.ToList();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens list. ", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }            
        }
    }
}
