using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.NGCUtilityService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Xml.Linq;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class UtilityServiceAdapter : IServiceAdapter
    {
        IUtilityService _utilityServiceClient = null;
        private readonly ILoggingService _logger = null;

        public UtilityServiceAdapter(IUtilityService utilityServiceClient, ILoggingService logger)
        {
            _utilityServiceClient = utilityServiceClient;
            _logger = logger;
        }

        /// <summary>
        /// Data retrieval call for Utility Service
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
                res.Data = this.IsProfaneText((request.Parameters[ParameterNames.PROFANE_TEXT]).TryParse<string>());
                res.Status = true;
                logData.RecordStep("Response received successfully");
                logData.CaptureData("Entered Text is Profane:  ", res.Data);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Utility Service Adapter GET", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { "InputParam", request.JsonText() }
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

        #region private Members

        private bool IsProfaneText(string text)
        {

            XmlDocument resulDoc = new XmlDocument();
            string errorXml, resultxml;
            int rowCount = 0;
            bool isProfane = false;
            DataSet dsProfanity = null;
            LogData logData = new LogData();
            try
            {
                if (_utilityServiceClient.ProfanityCheck(out errorXml, out resultxml, out rowCount, text))
                {
                    logData.CaptureData("Profanity XML", resultxml);
                    if (!string.IsNullOrEmpty(resultxml))
                    {
                        if (XElement.Parse(resultxml).DescendantsAndSelf("NewDataSet").Elements().First().Elements().First().Value == "0")
                        {
                            isProfane = true;
                        }
                        else
                            isProfane = false;
                    }
                }
                else
                    isProfane = false;

                if (dsProfanity != null)
                {
                    dsProfanity.Dispose();
                }
                logData.CaptureData("Error XML", errorXml);
                _logger.Submit(logData);
                return isProfane;

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Utility Service Adapter while validating profanity on the input text", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });

            }
        }

        #endregion
    }

}
