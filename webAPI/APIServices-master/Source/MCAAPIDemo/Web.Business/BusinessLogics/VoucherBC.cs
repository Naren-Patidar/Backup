using System;
using System.Collections.Generic;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Data;
using System.Configuration;
using WebAPI.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class VoucherBC
    {
        private string APIURL
        {
            get
            {
                return ConfigurationManager.AppSettings["APIURL"];
            }
        }

        APIRequester _APIRequester = null;

        //MCARequest request;
        //MCAResponse response;

        private LoggingService _logger;
        private decimal totalUnusedVouchers;
        private bool vouchersExpired = false;
        string culture = String.Empty;
        bool isDecimalDisabled = false;
        ConfigurationProvider _configProvider = null;        
        AccountBC _accountProvider;

        public VoucherBC()
        {
            //_clubcardServiceAdapter = new ClubcardServiceAdapter();
            //_customerServiceAdapter = new CustomerServiceAdapter();
            //_smartVoucherAdapter = new SmartVoucherServiceAdapter();
            //_preferenceServiceAdapter = new PreferenceServiceAdapter();
            _logger = new LoggingService();
            _configProvider = new ConfigurationProvider();
            //_generatePDFforVoucher = generatePDF;
            this.culture = _configProvider.GetStringAppSetting(AppConfigEnum.Culture);
            this.isDecimalDisabled = _configProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
            _accountProvider = new AccountBC();
            _APIRequester = new APIRequester(this.APIURL);
        }

        #region Public Methods
          
        /// <summary>
        /// Makes calls to the Service to retrieve Voucher Summary, RewardMiles (Avios, BA Avios, Virgin Atlantic), Unused Vouchers and Used Voucher data
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clubcardNumber"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public VouchersViewModel GetVoucherViewDetails(long customerId, string culture)
        {
            AccountDetails accountDetails = null;
            VouchersViewModel vouchersViewModel = new VouchersViewModel();
            bool _blnErrorMessageColorChanged = false;
            long clubcardNumber = 0;
            LogData logData = new LogData();

            try
            {
                accountDetails = GetCustomerAccountDetails(customerId, culture);
                if (accountDetails != null)
                {
                    clubcardNumber = accountDetails.ClubcardID;
                    vouchersViewModel.accountDetails = accountDetails;
                    logData.RecordStep("Get Rewards Overall Summary data");

                    vouchersViewModel.voucherRewardDetailsOverallSummaryModel = this.GetVoucherRewardDetails(clubcardNumber);

                    if (vouchersViewModel.voucherRewardDetailsOverallSummaryModel == null ||
                        (vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalRewardIssued == 0 &&
                                vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalRewardLeftOver == 0 &&
                                vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalVoucherIssuedLastTwoYears == 0))
                    {
                        logData.RecordStep("No Vouchers issued");
                        _blnErrorMessageColorChanged = true;
                    }

                    if (!_blnErrorMessageColorChanged)
                    {
                        logData.RecordStep("Get Unused Vouchers");
                        vouchersViewModel.vouchersUnUsedModel.voucherList = this.ApplyUnusedVoucherDetails(customerId, clubcardNumber, culture);
                        vouchersViewModel.vouchersUnUsedModel.totalUnusedVouchers = totalUnusedVouchers;

                        logData.RecordStep("Get Used Vouchers");
                        vouchersViewModel.voucherUsageSummary = this.ApplyUsedVoucherDetails(clubcardNumber);

                        logData.RecordStep("Get Avios/Virgin Reward Miles data");
                        vouchersViewModel.voucherRewardsMilesModel = this.ApplyRewardMiles(customerId, clubcardNumber);

                        IConfigurationProvider _Config = new ConfigurationProvider();
                        DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.IsChristmasSaverApplicable);

                        //By-passing christmas Saver service call where it is not applicable.
                        if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1) && item.ConfigurationValue1.Equals("1"))
                        {
                            logData.RecordStep("Get Christmas Saver data");
                            vouchersViewModel.voucherChristmasSaverSummaryModel.christmasSaverSummary = new List<ChristmasSaverSummary>();
                            vouchersViewModel.voucherChristmasSaverSummaryModel.isXmasClubMember = false;
                        }
                    }
                    vouchersViewModel.vouchersExpired = vouchersExpired;

                    logData.RecordStep("Received Vouchers ViewModel");
                    _logger.Submit(logData);
                }
                return vouchersViewModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Voucher View Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        ///  Makes call to the Service to retrieve the Customer's Account Details by customerID
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AccountDetails GetCustomerAccountDetails(long customerId, string culture)
        {
            LogData logData = new LogData();
            try
            {
                AccountDetails customerAccountDetails = new AccountDetails();

                string data = "{\"service\":\"ClubcardService\",\"operation\":\"GetCustomerAccountDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });
                if (apiResponseObj.status)
                {
                    customerAccountDetails = JsonConvert.DeserializeObject<AccountDetails>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                }
                _logger.Submit(logData);
                return customerAccountDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Customer Account Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Called from the Security Check Attribute to check for any upsent vouchers before showing the Internal Security page.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool IsUnSpentVouchersExist(long customerId, string culture)
        {
            LogData logData = new LogData();
            try
            {
                if (customerId == 0)
                    return false;


                string data = "{\"service\":\"CustomerService\",\"operation\":\"GetHouseHoldDetailsByCustomer\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });
                
                if (apiResponseObj.status)
                {
                    if (apiResponseObj.data != null || !String.IsNullOrWhiteSpace(apiResponseObj.data.ToString()))
                    {
                        List<HouseholdCustomerDetails> customerDetails = JsonConvert.DeserializeObject<List<HouseholdCustomerDetails>>(apiResponseObj.data.ToString(),
                                                                        new JsonSerializerSettings
                                                                        {
                                                                            NullValueHandling = NullValueHandling.Ignore
                                                                        });

                        data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetUnusedVoucherDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"cardNumber\",\"Value\":\"" + customerDetails[0].ClubcardID + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";

                        apiResponse = this._APIRequester.MakeRequest(data);

                        apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                        new JsonSerializerSettings
                                                                                        {
                                                                                            NullValueHandling = NullValueHandling.Ignore
                                                                                        });

                        if (apiResponseObj.status)
                        {

                            List<VoucherDetails> unspentVouchers = JsonConvert.DeserializeObject<List<VoucherDetails>>(apiResponseObj.data.ToString(),
                                                                            new JsonSerializerSettings
                                                                            {
                                                                                NullValueHandling = NullValueHandling.Ignore
                                                                            });

                            return unspentVouchers.Count > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                }         
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Total Unspent Vouchers", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the method to generate PDF for Voucher
        /// </summary>
        /// <param name="voucherDetailsList"></param>
        /// <param name="customerAccountDetails"></param>
        /// <param name="template"></param>
        //public void PrintVoucher(List<VoucherDetails> voucherDetailsList, AccountDetails customerAccountDetails, PdfBackgroundTemplate template)
        //{
        //    LogData logData = new LogData();
        //    try
        //    {
        //        logData.RecordStep("Get Voucher PDF Document using the template");
        //        _generatePDFforVoucher.GetPDFDocumentStream<VoucherDetails, PdfBackgroundTemplate>(voucherDetailsList, template, customerAccountDetails);

        //        _logger.Submit(logData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw GeneralUtility.GetCustomException("Failed in VoucherBC while Printing Vouchers", ex, new Dictionary<string, object>() 
        //                    { 
        //                        { LogConfigProvider.EXCLOGDATAKEY, logData }
        //                    });
        //    }
        //}
        /// <summary>
        /// To insert the print tokens details for reporting purpose
        /// </summary>
        /// <param name="ds">Input parameter</param>
        //public bool RecordVouchersPrinted(List<VoucherDetails> selectedVouchers, long customerId, long cardNumber)
        //{
        //    //Store the print details for reporting
        //    LogData _logData = new LogData();
        //    DataTable dtPrintDetail = new DataTable();
        //    DataSet dsVouchers = new DataSet("DocumentElement");
        //    try
        //    {
        //        MCAResponse response = new MCAResponse();
        //        MCARequest request = new MCARequest();
        //        dtPrintDetail.TableName = "PrintDetails";
        //        dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
        //        dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
        //        dtPrintDetail.Columns.Add("Value", typeof(Decimal));
        //        dtPrintDetail.Columns.Add("VoucherID", typeof(string));
        //        dtPrintDetail.Columns.Add("VoucherType", typeof(string));
        //        dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
        //        dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
        //        dtPrintDetail.Columns.Add("Flag", typeof(Char));

        //        foreach (VoucherDetails voucherDetails in selectedVouchers)
        //        {
        //            DataRow dr = dtPrintDetail.NewRow();

        //            dr["CustomerId"] = customerId;
        //            dr["PrintDate"] = DateTime.Now;
        //            dr["Value"] = voucherDetails.Value;
        //            dr["VoucherID"] = voucherDetails.BarCode;

        //            if (voucherDetails.VoucherType == "1")
        //            {
        //                dr["VoucherType"] = "Clubcard";
        //            }
        //            else if (voucherDetails.VoucherType == "4")
        //            {
        //                dr["VoucherType"] = "Bonus";
        //            }
        //            if (voucherDetails.VoucherType == "5")
        //            {
        //                dr["VoucherType"] = "Top Up";
        //            }

        //            dr["ExpiryDate"] = voucherDetails.ExpiryDate;
        //            dr["CCNumber"] = cardNumber;
        //            dr["Flag"] = "V";

        //            dtPrintDetail.Rows.Add(dr);
        //        }
        //        dsVouchers.Tables.Add(dtPrintDetail);
        //        _logData.RecordStep("Received Print data");
        //        request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
        //        request.Parameters.Add(ParameterNames.DS_VOUCHER, dsVouchers);
        //        response = this._customerServiceAdapter.Set<VoucherDetails>(request);
        //        _logger.Submit(_logData);
        //        return response.Status;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Recording Vouchers Printed", ex, new Dictionary<string, object>() 
        //                    { 
        //                        { LogConfigProvider.EXCLOGDATAKEY, _logData }
        //                    });
        //    }
        //}

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Calls the Smart Voucher service to get the Rewards Issued/Used
        /// </summary>
        /// <param name="clubcardNumber"></param>
        /// <returns></returns>
        private VoucherRewardDetailsOverallSummaryModel GetVoucherRewardDetails(long clubcardNumber)
        {
            LogData logData = new LogData();
            VoucherRewardDetailsOverallSummaryModel voucherRewardDetailsOverallSummaryModel = null;
            try
            {
                string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetVoucherRewardDetails\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"" + clubcardNumber + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    List<VoucherRewardDetails> voucherRewardDetailsList = JsonConvert.DeserializeObject<List<VoucherRewardDetails>>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    voucherRewardDetailsOverallSummaryModel = new VoucherRewardDetailsOverallSummaryModel(voucherRewardDetailsList);

                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                } 

                _logger.Submit(logData);
                return voucherRewardDetailsOverallSummaryModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Voucher Reward Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the Smart Voucher service to get the Used Voucher Details
        /// </summary>
        /// <param name="clubcardNumber"></param>
        /// <returns></returns>
        private List<VoucherUsageSummary> ApplyUsedVoucherDetails(long clubcardNumber)
        {
            LogData logData = new LogData();
            try
            {
                string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetUsedVoucherDetails\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"" + clubcardNumber + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    List<VoucherUsageSummary> usageSummary = JsonConvert.DeserializeObject<List<VoucherUsageSummary>>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    _logger.Submit(logData);
                    return usageSummary;
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                } 
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Used Voucher Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the Smart Voucher service to get the Unused Vouchers details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="clubcardNumber"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        private List<VoucherDetails> ApplyUnusedVoucherDetails(long customerId, long clubcardNumber, string culture)
        {
            LogData logData = new LogData();
            try
            {
                List<VoucherDetails> voucherList = null; 
                string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetUnusedVoucherDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"cardNumber\",\"Value\":\"" + clubcardNumber + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + culture + "\"}]}";


                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    voucherList = JsonConvert.DeserializeObject<List<VoucherDetails>>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    if (voucherList != null && voucherList.Count > 0)
                    {
                        logData.RecordStep("Get Expired Vouchers");
                        voucherList.ForEach(x => this.totalUnusedVouchers += x.Value.TryParse<decimal>());
                        vouchersExpired = voucherList[0].ExpiryDate <= DateTime.Today.AddMonths(3);
                    }
                    logData.RecordStep("Received Unused Vouchers");
                    _logger.Submit(logData);
                    return voucherList;
                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                } 
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Unused Voucher Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// To get reward details for Airmiles/BAMiles.
        /// </summary>
        /// <param name="svServiceCall"></param>
        private VoucherRewardsMilesModel ApplyRewardMiles(long customerId, long clubcardNumber)
        {
            string optedForMiles, strDispVal = string.Empty;
            string strMiles = string.Empty;
            int voucherRate = 0, reasonCode = 0;
            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel();
            LogData logData = new LogData();

            try
            {
                optedForMiles = "";
                logData.CaptureData("Opted For Miles: ", optedForMiles);

                if (optedForMiles.Contains(ParameterNames.AIRMILES) || optedForMiles.Contains(ParameterNames.BAMILES))
                {
                    strMiles = " Avios ";
                }
                else if (optedForMiles.Contains(ParameterNames.VIRGIN))
                {
                    strMiles = " Flying Club miles ";
                }
                else
                {
                    logData.RecordStep("Not Opted for AirMiles, BAMiles or Virgin");
                    return voucherRewardsMilesModel;
                }
                if (!string.IsNullOrEmpty(optedForMiles))
                {
                    logData.RecordStep("Get Reward Miles Summary");
                    MilesRewardDetailsSummary summary = GetMilesRewardDetailsSummary(clubcardNumber, reasonCode);
                    strDispVal = summary.FormattedVoucherValue;
                    voucherRewardsMilesModel.optedForMiles = optedForMiles;
                    voucherRewardsMilesModel.milesRate = voucherRate.ToString() + strMiles;
                    voucherRewardsMilesModel.totalRewardPoints = summary.TotalRewardPoints.ToString();
                    strDispVal = (isDecimalDisabled) ? GeneralUtility.GetDecimalTrimmedCurrencyVal(strDispVal.ToString()) : string.Empty;
                    voucherRewardsMilesModel.summaryFormattedVoucherValue = strDispVal;
                    logData.RecordStep("Received Miles Reward Summary");
                }
                _logger.Submit(logData);
                return voucherRewardsMilesModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Reward Miles Details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the Smart Voucher service to get the Rewards Miles details
        /// </summary>
        /// <param name="clubcardNumber"></param>
        /// <param name="reasonCode"></param>
        /// <returns></returns>
        private MilesRewardDetailsSummary GetMilesRewardDetailsSummary(long clubcardNumber, int reasonCode)
        {
            LogData logData = new LogData();
            try
            {

                MilesRewardDetailsSummary summary = new MilesRewardDetailsSummary();

                string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetRewardDetailsMiles\"," +
                                 "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"" + clubcardNumber + "\"}," +
                                                 "{\"Key\":\"reasonCode\",\"Value\":\"" + reasonCode + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    List<MilesRewardDetails> milesRewardDetailsList = JsonConvert.DeserializeObject<List<MilesRewardDetails>>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    if (milesRewardDetailsList != null)
                    {
                        foreach (MilesRewardDetails milesRewardDetails in milesRewardDetailsList)
                        {
                            summary.TotalRewardPoints = summary.TotalRewardPoints + milesRewardDetails.RewardPoints;
                        }
                    }
                    else
                    {
                        summary.TotalRewardPoints = summary.TotalRewardPoints + 0;
                    }

                    //To calculate miles from the points.
                    //To calculate vouchers
                    int remd = summary.TotalRewardPoints % 50;
                    int correctedPoints = summary.TotalRewardPoints - remd;
                    float dispVal = ((float)(correctedPoints)) / 100;
                    string strDispVal = dispVal.ToString();

                    if (strDispVal.Contains("."))
                    {
                        string temp = strDispVal.Substring(strDispVal.Length - 2, 1);
                        if (temp != "0")
                            strDispVal += "0";
                    }
                    else
                    {
                        strDispVal += ".00";
                    }
                    summary.FormattedVoucherValue = strDispVal;
                    logData.CaptureData("Miles Reward Details Summary", summary);

                    _logger.Submit(logData);
                    return summary;

                }
                else
                {
                    StringBuilder sbErrors = new StringBuilder();
                    apiResponseObj.errors.ForEach(e => sbErrors.Append(String.Format("Error - {0} - {1}", e.Key, e.Value)));
                    throw new Exception(sbErrors.ToString());
                }    
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Miles Reward Details Summary", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        #endregion Private Methods        
    }
}