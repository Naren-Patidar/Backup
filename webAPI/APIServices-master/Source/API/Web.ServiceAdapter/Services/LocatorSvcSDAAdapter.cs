using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using System.Xml;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class LocatorSvcSDAAdapter : BaseNGCAdapter, IServiceAdapter
    {
        #region Constructors

        private ILocatorSvcSDA _locatorServiceClient;
        DateTime _dtStart = DateTime.UtcNow;

        public LocatorSvcSDAAdapter()
        {
        }

        public LocatorSvcSDAAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            this._locatorServiceClient = new LocatorSvcSDAClient();
        }

        #endregion

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetAddressesForPostCodeList", new AddressByPostCodeList(){
                    AddressByPostCodeLists = new List<AddressByPostCode>(){
                        new AddressByPostCode(){
                        }
                    }
                }}
            };
        }

        public string GetName()
        {
            return "locatorservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getaddressesforpostcodelist":
                        response.data = this.GetAddressesForPostCodeList(
                                                request.GetParameter<string>("postcode"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-LOCATOR-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion

        #region Private Methods

        private AddressByPostCodeList GetAddressesForPostCodeList(string postCode)
        {
            AddressByPostCodeList lstAddressByPostCode = new AddressByPostCodeList();
            string resultAddress = String.Empty;
            this._dtStart = DateTime.UtcNow;
            try
            {
                try
                {
                    resultAddress = this._locatorServiceClient.FindAddressLite(postCode, null, null);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                if (!String.IsNullOrWhiteSpace(resultAddress))
                {
                    lstAddressByPostCode.ConvertFromXml(resultAddress);
                }
                return lstAddressByPostCode;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Get Address for postcode address list", ex, null);
            }
        }

        #endregion

    }
}