using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.NGCUtilityService;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Xml.Linq;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class UtilityServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        IUtilityService _utilityServiceClient = null;
        double _internalStats = 0;
        DateTime _dtStart = DateTime.UtcNow; 

        public UtilityServiceAdapter()
        {
        }

        public UtilityServiceAdapter(string dotcomid, string uuid, string culture) : base (dotcomid, uuid, culture)
        {
            _utilityServiceClient = new UtilityServiceClient();
        }

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                { "IsProfaneText", null }
            };
        }

        public string GetName()
        {
            return "utilityservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "isprofanetext":
                        response.data = this.IsProfaneText(
                                                request.GetParameter<string>("text"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-UTILITY-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion IServiceAdapter Members

        #region private Members

        private bool IsProfaneText(string text)
        {
            XmlDocument resulDoc = new XmlDocument();
            string errorXml, resultxml;
            int rowCount = 0;
            bool bService = false;
            this._dtStart = DateTime.UtcNow;
            try
            {
                try
                {
                    bService = this._utilityServiceClient.ProfanityCheck(out errorXml, out resultxml, out rowCount, text);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                return (!string.IsNullOrEmpty(resultxml) &&
                        XElement.Parse(resultxml).DescendantsAndSelf("NewDataSet").Elements().First().Elements().First().Value == "0");
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Utility Service Adapter while validating profanity on the input text", ex, null);
            }
        }

        #endregion
    }

}