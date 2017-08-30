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
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.RewardService;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class RewardServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        IRewardService _rewardserviceClient = null;
        DateTime _dtStart = DateTime.UtcNow;
        string _culture = String.Empty;

        public RewardServiceAdapter()
        {
        }

        public RewardServiceAdapter(string dotcomid, string uuid, string culture) : base (dotcomid, uuid, culture)
        {
            _rewardserviceClient = new RewardServiceClient();
        }

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetRewardAndTokens", new RewardAndToken(){
                        Rewards = new List<Reward>(){
                            {
                                new Reward(){                                    
                                }
                            }
                        },
                        Tokens = new List<Token>(){
                            {
                                new Token(){                                    
                                }
                            }
                        }
                    }
                },
                {
                    "GetTokens", new List<Token>(){
                        {
                            new Token(){
                            }
                        }
                    }
                }
            };
        }

        public string GetName()
        {
            return "rewardservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getrewardandtokens":
                        response.data = this.GetRewardAndTokens();
                        break;

                    case "gettokens":
                        response.data = this.GetTokens(
                                                request.GetParameter<string>("gid"),
                                                request.GetParameter<string>("bookingidval"),
                                                request.GetParameter<string>("productlineidval"),
                                                request.GetParameter<string>("culture"));
                        break;
                  
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-REWARD-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion IServiceAdapter Members

        private RewardAndToken GetRewardAndTokens()
        {
            try
            {
                string resultXml;
                string errorXml;
                RewardAndToken rewardDetails = new RewardAndToken();
                RewardList rewards = new RewardList();
                TokenList tokens = new TokenList();
                rewardDetails.Rewards = rewards.RewardsList;
                rewardDetails.Tokens = tokens.TokensList;

                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();

                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerID is not available for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._rewardserviceClient.GetRewardDetail(out errorXml, out resultXml, lCustomerID, this._culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    rewards.ConvertFromXml(resultXml);
                    tokens.ConvertFromXml(resultXml);
                    rewardDetails.Rewards = rewards.RewardsList;
                    rewardDetails.Tokens = tokens.TokensList;
                }
                return rewardDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting rewards and tokens.", ex, null);
            }
        }

        private List<Token> GetTokens(string gid, string bookingIdVal, string productLineIdVal, string culture)
        {
            try
            {
                long lBookingIdVal = bookingIdVal.TryParse<long>();
                long lProductLineVal = productLineIdVal.TryParse<long>();

                DataSet dsTokenDetails = this.GetTokenDataSet(gid, lBookingIdVal, lProductLineVal, culture);
                List<Token> tokenDetails = this.GetTokensFromDataset(dsTokenDetails);

                if (tokenDetails == null)
                {
                    return new List<Token>();
                }

                return tokenDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens . ", ex, null);
            }
        }

        private DataSet GetTokenDataSet(string gid, long bookingIdVal, long productLineIdVal, string culture)
        {
            try
            {
                XmlDocument resulDoc = null;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                DataSet dsCodes = null;
                Guid guidOutput;

                if (!Guid.TryParse(gid, out guidOutput))
                {
                    throw new Exception(String.Format("Not valid guid - {0}", gid));
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._rewardserviceClient.GetTokenInfo(out errorXml, out resultXml, guidOutput, bookingIdVal, productLineIdVal, culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!String.IsNullOrWhiteSpace(resultXml))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsCodes = new DataSet();
                    dsCodes.ReadXml(new XmlNodeReader(resulDoc));
                }

                return dsCodes;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens dataset. ", ex, null);
            }
        }
        
        private List<Token> GetTokensFromDataset(DataSet dstokenDetails)
        {
            try
            {
                if (dstokenDetails == null || dstokenDetails.Tables["TokenInfo"] == null)
                {
                    return new List<Token>();
                }

                DataTable tokenDetailsTable = dstokenDetails.Tables["TokenInfo"];
                DateTime dtTemp;
                tokenDetailsTable.AddMissingColumns(typeof(TokenEnum));
                IEnumerable<Token> tokenDetails = tokenDetailsTable.AsEnumerable().Select(r => new Token()
                {
                    BookingDate = r[TokenEnum.BookingDate.ToString()].ToString().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                    TokenID = r[TokenEnum.TokenId.ToString()].ToString(),
                    TokenDescription = Convert.ToString(r[TokenEnum.TokenDescription.ToString()]),
                    ProductStatus = Convert.ToString(r[TokenEnum.ProductStatus.ToString()]),
                    TokenValue = Convert.ToDecimal(r[TokenEnum.TokenValue.ToString()]),
                    SupplierTokenCode = Convert.ToString(r[TokenEnum.SupplierTokenCode.ToString()]),
                    ValidUntil = r[TokenEnum.ValidUntil.ToString()].ToString().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                    QualifyingSpend = Convert.ToString(r[TokenEnum.QualifyingSpend.ToString()]),
                    Includes = Convert.ToString(r[TokenEnum.Includes.ToString()]),
                    Excludes = Convert.ToString(r[TokenEnum.Excludes.ToString()]),
                    TermsAndConditions = Convert.ToString(r[TokenEnum.TermsAndConditions.ToString()])
                });

                return tokenDetails.ToList();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in RewardServiceAdapter while getting tokens list. ", ex, null);
            }            
        }
    }
}