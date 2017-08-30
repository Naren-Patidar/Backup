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
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class RewardServiceAdapter : IServiceAdapter
    {
        IRewardService _rewardserviceClient = new RewardServiceClient();
        Recorder _recorder = null;

        public RewardServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
        }

        #region IServiceAdapter Members

        public MCAResponse Get(MCARequest request)
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
            string resultXml;
            string errorXml;
            RewardAndToken rewardDetails = new RewardAndToken();
            RewardList rewards = new RewardList();
            TokenList tokens = new TokenList();
            rewardDetails.Rewards = rewards.RewardsList;
            rewardDetails.Tokens = tokens.TokensList;

            if (_rewardserviceClient.GetRewardDetail(out errorXml, out resultXml, customerID, culture))
            {
                this._recorder.RecordResponse(new RecordLog { Result = resultXml, Error = errorXml },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.RewardServices.ToString(),
                    "GetRewardDetail", ResponseType.Xml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    rewards.ConvertFromXml(resultXml);
                    tokens.ConvertFromXml(resultXml);
                    rewardDetails.Rewards = rewards.RewardsList;
                    rewardDetails.Tokens = tokens.TokensList;
                }
            }

            return rewardDetails;
        }

        private List<Token> GetTokens(Guid gid, long bookingIdVal, long productLineIdVal, string culture)
        {
            try
            {
                DataSet dsTokenDetails = GetTokenDataSet(gid, bookingIdVal, productLineIdVal, culture);
                List<Token> tokenDetails = GetTokens(dsTokenDetails);
                
                return tokenDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet GetTokenDataSet(Guid gid, long bookingIdVal, long productLineIdVal, string culture)
        {
            XmlDocument resulDoc = null;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            DataSet dsCodes = null;
            if (_rewardserviceClient.GetTokenInfo(out errorXml, out resultXml, gid, bookingIdVal, productLineIdVal, culture))
            {
                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.RewardServices.ToString(),
                    "GetTokenInfo", ResponseType.Xml);

                resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsCodes = new DataSet();
                dsCodes.ReadXml(new XmlNodeReader(resulDoc));
            }
            return dsCodes;
        }
        
        private List<Token> GetTokens(DataSet dstokenDetails)
        {
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

                return tokenDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        #region IServiceAdapter Members


        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion
    }
}
