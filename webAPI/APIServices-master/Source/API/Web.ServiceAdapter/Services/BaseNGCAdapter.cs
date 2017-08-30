using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.CacheLayer;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class BaseNGCAdapter
    {
        CustomerInfo _custInfo = null;
        string _dotcomid = String.Empty;
        string _uuid = String.Empty;
        string _culture = String.Empty;
        protected double _internalStats = 0;

        ICacheProvider _cacheProvider = null;

        protected string Culture { get { return this._culture; } }
        protected string Dotcomid { get { return this._dotcomid; } }
        protected string UUID { get { return this._uuid; } }

        public BaseNGCAdapter()
        {
        }

        public BaseNGCAdapter(string dotcomid, string uuid, string culture)
        {
            this._dotcomid = dotcomid;
            this._uuid = uuid;
            this._culture = culture;
        }

        protected virtual CustomerInfo GetCustInfo()
        {
            this.FetchCustomerInfoFromCache();
            if (this._custInfo == null && !this.GetType().Name.ToLower().Equals("clubcardserviceadapter"))
            {
                ClubcardServiceAdapter clAdapter = new ClubcardServiceAdapter(this.Dotcomid, this.UUID, this.Culture);
                var custActivationStatus = clAdapter.IGHSCheckCustomerActivatedStatus();

                if (custActivationStatus.Activated != "Y" || 
                    custActivationStatus.CustomerUseStatus == 2 || 
                    custActivationStatus.CustomerUseStatus == 3)
                {
                    throw new Exception("activation error denied access");
                }

                this._custInfo = new CustomerInfo() { uuid = this.UUID, dotcomid = this.Dotcomid };

                this._custInfo.activationstatus = custActivationStatus.Activated;
                this._custInfo.ngccustomerid = custActivationStatus.CustomerId.ToString();

                this.SaveCustomerInfoInCache();
            }

            return this._custInfo;
        }

        protected virtual string GetHouseHoldID()
        {
            this.GetCustInfo();
            if (this._custInfo == null)
            {
                return String.Empty;
            }

            if (String.IsNullOrWhiteSpace(this._custInfo.householdid) && !this.GetType().Name.ToLower().Equals("customerserviceadapter"))
            {
                CustomerServiceAdapter csAdapter = new CustomerServiceAdapter(this.Dotcomid, this.UUID, this.Culture);
                var houseHolds = csAdapter.GetHouseHoldDetailsByCustomer();
                if (houseHolds != null && houseHolds.Count > 0)
                {
                    this._custInfo.householdid = houseHolds[0].HouseHoldID;
                    this.SaveCustomerInfoInCache();
                    return houseHolds[0].HouseHoldID;
                }
            }

            return this._custInfo.householdid;
        }

        protected virtual List<Clubcard> GetClubcardsByType(CardType cType)
        {
            var cards = this._custInfo.clubcards;

            if (cards.Any(c => JsonConvert.DeserializeObject<Clubcard>(c).CardType == cType))
            {
                return cards.Where(c => JsonConvert.DeserializeObject<Clubcard>(c).CardType == cType)
                            .Select<string, Clubcard>(c => JsonConvert.DeserializeObject<Clubcard>(c))
                            .ToList<Clubcard>();
            }

            if (this.GetType().Name.ToLower().Equals("clubcardserviceadapter"))
            {
                return new List<Clubcard>();
            }

            ClubcardServiceAdapter clAdapter = new ClubcardServiceAdapter(this.Dotcomid, this.UUID, this.Culture);

            if (cType == CardType.HouseHoldMembers)
            {
                var houseMembers = clAdapter.GetHouseHoldCustomersData();
                foreach (var member in houseMembers)
                {
                    clAdapter.GetCustomerCards(member.CustomerID).ForEach(c =>
                    {
                        if (!this._custInfo.clubcards.Any(cl =>
                                                            JsonConvert.DeserializeObject<Clubcard>(cl).ClubCardID == c.ClubCardID
                                                            && JsonConvert.DeserializeObject<Clubcard>(cl).CardType == CardType.HouseHoldMembers))
                        {
                            c.CardType = CardType.HouseHoldMembers;
                            this._custInfo.clubcards.Add(c.JsonText());
                        }
                    });
                }
            }

            if (cType == CardType.MyAccount || cType == CardType.AssociateClubcard)
            {
                var accDetails = clAdapter.GetCustomerAccountDetails();
                if (!this._custInfo.clubcards.Any(c => JsonConvert.DeserializeObject<Clubcard>(c).ClubCardID == accDetails.AssociateClubcardID
                                                        && JsonConvert.DeserializeObject<Clubcard>(c).CardType == CardType.AssociateClubcard))
                {
                    Clubcard ccard = new Clubcard()
                    {
                        CardType = CardType.AssociateClubcard,
                        ClubCardID = accDetails.AssociateClubcardID
                    };

                    this._custInfo.clubcards.Add(ccard.JsonText());
                }
                if (!this._custInfo.clubcards.Any(c => JsonConvert.DeserializeObject<Clubcard>(c).ClubCardID == accDetails.ClubcardID
                                                    && JsonConvert.DeserializeObject<Clubcard>(c).CardType == CardType.MyAccount))
                {
                    Clubcard ccard = new Clubcard()
                    {
                        CardType = CardType.MyAccount,
                        ClubCardID = accDetails.ClubcardID
                    };
                    this._custInfo.clubcards.Add(ccard.JsonText());
                }
            }

            this.SaveCustomerInfoInCache();
            return cards.Where(c => JsonConvert.DeserializeObject<Clubcard>(c).CardType == cType)
                        .Select<string, Clubcard>(c => JsonConvert.DeserializeObject<Clubcard>(c))
                        .ToList<Clubcard>();
        }

        private void FetchCustomerInfoFromCache()
        {
            if (this._custInfo == null)
            {
                this._cacheProvider = CacheProviderFactory.GetActiveCacheProvider();
                if (this._cacheProvider != null)
                {
                    var data = this._cacheProvider.GetItem(this.UUID.ToLower());
                    if (String.IsNullOrWhiteSpace(data))
                    {
                        return;
                    }

                    try
                    {
                        this._custInfo = JsonConvert.DeserializeObject<CustomerInfo>(data);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void SaveCustomerInfoInCache()
        {
            if (this._cacheProvider != null)
            {
                this._cacheProvider.AddItem(this._custInfo.uuid.ToLower(), JsonConvert.SerializeObject(this._custInfo));
            }
        }

        protected void SaveCustomerInfoInCache(CustomerInfo custInfo)
        {
            this._custInfo = custInfo;
            this.SaveCustomerInfoInCache();
        }

        protected void HandleFailedResponse(bool response, string errorXml)
        {
            if (!response)
            {
                Exception ex = String.IsNullOrWhiteSpace(errorXml) ? new Exception(StringUtility.FalseWithNoErrorInfo) : new Exception(errorXml);
                throw ex;
            }
            else if (!String.IsNullOrWhiteSpace(errorXml))
            {
                throw new Exception(errorXml);
            }
        }
    }
}
