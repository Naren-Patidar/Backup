using System;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.MyAccountCustomerService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using System.Threading.Tasks;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class MyAccountServiceAdapter : IServiceAdapter
    {
        ICustomerSvcsCoreSDA _customerServiceCoreSDA = null;
        private readonly ILoggingService _logger = null;

        public MyAccountServiceAdapter(ICustomerSvcsCoreSDA customerServiceCoreSDA, ILoggingService logger)
        {
            _customerServiceCoreSDA = customerServiceCoreSDA;
            _logger = logger;
        }

        /// <summary>
        /// Data retrieval call for My Account Customer Data Service
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
                res.Data = this.GetPersonalDetails((request.Parameters[ParameterNames.DOTCOM_CUSTOMER_ID]).TryParse<int>());
                res.Status = true;
                logData.RecordStep("Response received successfully");
                logData.CaptureData("Entered Text is Profane:  ", res.Data);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in My Account Customer Data Service Adapter GET", ex,
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

        private DotcomCustomerDetails GetPersonalDetails(int webCustomerID)
        {
            LogData logData = new LogData();
            DotcomCustomerDetails dotcomCustDetails = new DotcomCustomerDetails();
            DotcomCustomerAddressDetails dotcomCustAddressDetails = new DotcomCustomerAddressDetails();

            try
            {
                long customerID = (this._customerServiceCoreSDA.CustomerMartiniIDGet(webCustomerID)).TryParse<long>();

                if (customerID > 0)
                {
                    List<Func<string>> tasks = new List<Func<string>>();

                    tasks.Add(() => this._customerServiceCoreSDA.GetPersonalDetails(customerID));
                    tasks.Add(() => this._customerServiceCoreSDA.GetHomeAddress(customerID));

                    List<string> responses = new List<string>();
                    var task = Task.Factory.StartNew(() => Parallel.ForEach<Func<string>>(tasks, t => responses.Add(t())));
                    task.Wait();
                    string strResponse = string.Empty;
                    string strResponseHomePostCode = string.Empty;

                    if (responses != null && responses.Count == 2)
                    {
                        logData.RecordStep("Received 2 responses.");
                        if (responses[0].ToString().Contains("PersonalDetailsEntity"))
                        {
                            strResponse = responses[0].ToString();
                            strResponseHomePostCode = responses[1].ToString();
                        }
                        else if (responses[0].ToString().Contains("AddressEntity"))
                        {
                            strResponse = responses[1].ToString();
                            strResponseHomePostCode = responses[0].ToString();
                        }
                        else
                        {
                            strResponse = responses[0].ToString();
                            strResponseHomePostCode = responses[1].ToString();
                        }                        
                    }
                    else
                    {
                        logData.RecordStep("Did not receive 2 response.");
                    }

                    if (!String.IsNullOrWhiteSpace(strResponse))
                    {
                        logData.RecordStep("Received data for call to GetPersonalDetails");
                        logData.CaptureData("Data for Dotcom customer Personal Details", strResponse);

                        try
                        {
                            dotcomCustDetails.ConvertFromXml(strResponse);
                        }
                        catch (Exception ex)
                        {                            
                            logData.CaptureData("Dotcom service GetPersonalDetails response : ", strResponse);                            
                            _logger.ErrorException(ex);
                        }

                        if (!String.IsNullOrWhiteSpace(strResponseHomePostCode))
                        {
                            logData.RecordStep("Received data for call to GetHomeAddress");
                            logData.CaptureData("Data for Dotcom customer Home Address", strResponseHomePostCode);
                            try
                            {
                                dotcomCustAddressDetails.ConvertFromXml(strResponseHomePostCode);
                            }
                            catch (Exception ex)
                            {
                                logData.CaptureData("Dotcom service GetHomeAddress response : ", strResponseHomePostCode);
                                _logger.ErrorException(ex);
                            }

                            if (dotcomCustAddressDetails != null &&
                                !String.IsNullOrWhiteSpace(dotcomCustAddressDetails.PostCode))
                            {
                                dotcomCustDetails.PostCode = dotcomCustAddressDetails.PostCode;
                            }
                        }
                        else
                        {
                            logData.RecordStep(String.Format("GetHomeAddress Call - No Data returned for customer - {0}", customerID));
                        }
                    }
                    else
                    {
                        logData.RecordStep(String.Format("GetPersonalDetails Call - No Data returned for customer - {0}", customerID));
                    }
                }
                else
                {
                    logData.CaptureData("Online CustomerID ", customerID);
                }
                _logger.Submit(logData);
                return dotcomCustDetails;
            }
            catch (Exception ex)
            {
                logData.CaptureData("Failed in My Account Customer Data Service Adapter while getting personal details", webCustomerID);
                throw GeneralUtility.GetCustomException("Failed in My Account Customer Data Service Adapter while getting personal details", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        #endregion
    }

}
