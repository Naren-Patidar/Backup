using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.RestClient;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;
using System.Configuration;

namespace CCODundeeApplication.App_Code
{
    public class RestClient
    {
        RestProxies _restClientManager = new RestProxies();
        string apiUrl = ConfigurationManager.AppSettings["WebAPIUrl"].ToString();
        string webAPIUrlforToken = ConfigurationManager.AppSettings["WebAPIUrlforToken"].ToString();

        string ngcApiUrl = ConfigurationManager.AppSettings["NGCWebAPIUrl"].ToString();
        string WebAPIaddressLookup = ConfigurationManager.AppSettings["WebAPIaddressLookup"].ToString();

        public List<addressList> getAddressDetails(string postCode, string Authorization)
        {

            addressList addressLst = new addressList();
            List<addressList> _addressLst = new List<addressList>();           
            try
            {
                WebAPIaddressLookup = WebAPIaddressLookup.Replace("{POSTCODE}", postCode);

                var response = _restClientManager.RestGet<List<addressList>>(WebAPIaddressLookup, Authorization);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        _addressLst = (List<addressList>)response.Body;
                        return _addressLst;
                    default:
                        throw new Exception("getAddressDetails method Failure.");
                }
            }
            catch
            {
                throw;
            }

        }

        public CustomerUUID getCustomerUUID(string methodName)
        {

            CustomerUUID customerUUIDobject = new CustomerUUID();
            try
            {
                string url = AppendUrl(ngcApiUrl, methodName);
                var response = _restClientManager.RestGet<CustomerUUID>(url, "");
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        customerUUIDobject = (CustomerUUID)response.Body;
                        return customerUUIDobject;
                    default:
                        throw new Exception("get customer UUID method Failure.");
                }
            }
            catch
            {
                throw;
            }

        }

        public NewCardEntitled getCardEntitlementStatus(string methodName, string UUID)
        {
            NewCardEntitled newCard = new NewCardEntitled();
            try
            {
                string identityApiUrl = AppendUrl(apiUrl, methodName);
                var response = _restClientManager.RestGet<NewCardEntitled>(identityApiUrl, UUID);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        newCard = (NewCardEntitled)response.Body;
                        return newCard;
                    default:
                        throw new Exception(" getCardEntitlementStatus method Failure.");
                }
            }
            catch
            {
                throw;
            }
        }

        public CreateCardsetResponse CreateCardset(string methodName, string UUID, CreateCardsetRequest req)
        {
            CreateCardsetResponse createCardSet = new CreateCardsetResponse();

            try
            {
                string identityApiUrl = AppendUrl(apiUrl, methodName);
                var response = _restClientManager.RestPost<CreateCardsetResponse, CreateCardsetRequest>(identityApiUrl, req, UUID);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        createCardSet = (CreateCardsetResponse)response.Body;
                        return createCardSet;
                    case System.Net.HttpStatusCode.NoContent:
                        return createCardSet;
                    default:
                        throw new Exception("CreateCardset method Failure.");
                }
            }
            catch
            {
                throw;
            }

        }

        public GetAccessTokenResponse GetAccessToken(string methodName, string UUID, GetAccessTokenRequest req)
        {
            GetAccessTokenResponse accessToken = new GetAccessTokenResponse();
            try
            {
                string identityApiUrl = AppendUrl(webAPIUrlforToken, methodName);
                var response = _restClientManager.RestPost<GetAccessTokenResponse, GetAccessTokenRequest>(identityApiUrl, req, UUID);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        accessToken = (GetAccessTokenResponse)response.Body;
                        return accessToken;
                    default:
                        throw new Exception("GetAccessToken method Failure.");
                }
            }
            catch
            {
                throw;
            }
        }

        public string AppendUrl(string baseURL, string methodName)
        {
            string URL = string.Empty;
            URL = string.Format("{0}{1}", baseURL, methodName);
            return URL;
        }


    }
}