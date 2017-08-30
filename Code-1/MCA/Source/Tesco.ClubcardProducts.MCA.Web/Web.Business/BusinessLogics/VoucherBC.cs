using System;
using System.Collections.Generic;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
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
namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class VoucherBC : IVoucherBC
    {
        IServiceAdapter _clubcardServiceAdapter;
        IServiceAdapter _customerServiceAdapter;
        IAccountBC _accountProvider;
        IServiceAdapter _smartVoucherAdapter;
        IServiceAdapter _preferenceServiceAdapter;
        IPDFGenerator _generatePDFforVoucher;

        MCARequest request;
        MCAResponse response;

        private readonly ILoggingService _logger;
        private decimal totalUnusedVouchers;
        private bool vouchersExpired = false;
        string culture = String.Empty;
        bool isDecimalDisabled = false;
        IConfigurationProvider _configProvider = null;

        public VoucherBC(
                        IServiceAdapter clubcardServiceAdapter,
                        IServiceAdapter customerServiceAdapter,
                        IServiceAdapter smartVoucherServiceAdapter,
                        IServiceAdapter preferenceServiceAdapter,
                        IPDFGenerator generatePDF,
                        ILoggingService logger,
                        IConfigurationProvider configProvider,
                        IAccountBC accountProvider)
        {
            _clubcardServiceAdapter = clubcardServiceAdapter;
            _customerServiceAdapter = customerServiceAdapter;
            _smartVoucherAdapter = smartVoucherServiceAdapter;
            _preferenceServiceAdapter = preferenceServiceAdapter;
            _logger = logger;
            _configProvider = configProvider;
            _generatePDFforVoucher = generatePDF;
            this.culture = _configProvider.GetStringAppSetting(AppConfigEnum.Culture);
            this.isDecimalDisabled = _configProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);
            this._accountProvider = accountProvider;
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
            bool bShowHoldingPage = false;
            bool isXMasClubMember = false;
            long clubcardNumber = 0;
            LogData logData = new LogData();
            string optedForMiles = string.Empty;
            int voucherRate, reasonCode = 0;
            try
            {
                accountDetails = GetCustomerAccountDetails(customerId, culture);
                if (accountDetails != null)
                {
                    clubcardNumber = accountDetails.ClubcardID;
                    vouchersViewModel.accountDetails = accountDetails;
                    logData.RecordStep("Get Rewards Overall Summary data");

                    optedForMiles = GetOptedForMile(customerId, out voucherRate, out reasonCode);
                    logData.CaptureData("Opted For Miles: ", optedForMiles);

                    if (!String.IsNullOrWhiteSpace(optedForMiles))
                    {
                        logData.RecordStep("Get Avios/Virgin Reward Miles data");
                        vouchersViewModel.voucherRewardsMilesModel = this.ApplyRewardMiles(customerId, clubcardNumber, optedForMiles, voucherRate, reasonCode);
                        if (vouchersViewModel.voucherRewardsMilesModel.totalRewardPoints == "0")
                        {
                            logData.RecordStep("No points collected from past 2 years");
                        }
                    }
                    else
                    {
                        vouchersViewModel.voucherRewardDetailsOverallSummaryModel = this.GetVoucherRewardDetails(clubcardNumber);
                        if (vouchersViewModel.voucherRewardDetailsOverallSummaryModel == null ||
                            (vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalRewardIssued == 0 &&
                                    vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalRewardLeftOver == 0 &&
                                    vouchersViewModel.voucherRewardDetailsOverallSummaryModel.TotalVoucherIssuedLastTwoYears == 0))
                        {
                            logData.RecordStep("No Vouchers issued");
                            bShowHoldingPage = true;
                        }
                    }

                    if (!bShowHoldingPage)
                    {
                        logData.RecordStep("Get Unused Vouchers");
                        vouchersViewModel.vouchersUnUsedModel.voucherList = this.ApplyUnusedVoucherDetails(customerId, clubcardNumber, culture);
                        vouchersViewModel.vouchersUnUsedModel.totalUnusedVouchers = totalUnusedVouchers;

                        logData.RecordStep("Get Used Vouchers");
                        vouchersViewModel.voucherUsageSummary = this.ApplyUsedVoucherDetails(clubcardNumber);

                        IConfigurationProvider _Config = new ConfigurationProvider();
                        DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.IsChristmasSaverApplicable);

                        //By-passing christmas Saver service call where it is not applicable.
                        if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1) && item.ConfigurationValue1.Equals("1"))
                        {
                            logData.RecordStep("Get Christmas Saver data");
                            vouchersViewModel.voucherChristmasSaverSummaryModel.christmasSaverSummary = this.ApplyXmasClubMemberArea(customerId, out isXMasClubMember);
                            vouchersViewModel.voucherChristmasSaverSummaryModel.isXmasClubMember = isXMasClubMember;
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
                request = new MCARequest();

                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                response = _clubcardServiceAdapter.Get<AccountDetails>(request);
                customerAccountDetails = (AccountDetails)response.Data;

                if (response.Status)
                {
                    if (customerAccountDetails != null)
                    {
                        logData.RecordStep("Return Customer Account details");
                        return customerAccountDetails;
                    }
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

                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                response = _customerServiceAdapter.Get<List<HouseholdCustomerDetails>>(request);
                List<HouseholdCustomerDetails> customerDetails = response.Data as List<HouseholdCustomerDetails>;

                if (response == null)
                    return false;

                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_UNUSED_VOUCHER_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, customerDetails[0].ClubcardID);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                response = new MCAResponse();

                response = _smartVoucherAdapter.Get<List<VoucherDetails>>(request);
                List<VoucherDetails> vouchersList = response.Data as List<VoucherDetails>;

                logData.CaptureData("Unspent Vouchers Exists: {0}", vouchersList.Count() > 0);
                _logger.Submit(logData);
                return vouchersList.Count() > 0;
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
        public void PrintVoucher(List<VoucherDetails> voucherDetailsList, AccountDetails customerAccountDetails, PdfBackgroundTemplate template)
        {
            LogData logData = new LogData();
            try
            {
                logData.RecordStep("Get Voucher PDF Document using the template");
                _generatePDFforVoucher.GetPDFDocumentStream<VoucherDetails, PdfBackgroundTemplate>(voucherDetailsList, template, customerAccountDetails);

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while Printing Vouchers", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }
        /// <summary>
        /// To insert the print tokens details for reporting purpose
        /// </summary>
        /// <param name="ds">Input parameter</param>
        public bool RecordVouchersPrinted(List<VoucherDetails> selectedVouchers, long customerId, long cardNumber)
        {
            //Store the print details for reporting
            LogData _logData = new LogData();
            DataTable dtPrintDetail = new DataTable();
            DataSet dsVouchers = new DataSet("DocumentElement");
            try
            {
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();
                dtPrintDetail.TableName = "PrintDetails";
                dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
                dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
                dtPrintDetail.Columns.Add("Value", typeof(Decimal));
                dtPrintDetail.Columns.Add("VoucherID", typeof(string));
                dtPrintDetail.Columns.Add("VoucherType", typeof(string));
                dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
                dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
                dtPrintDetail.Columns.Add("Flag", typeof(Char));

                foreach (VoucherDetails voucherDetails in selectedVouchers)
                {
                    DataRow dr = dtPrintDetail.NewRow();

                    dr["CustomerId"] = customerId;
                    dr["PrintDate"] = DateTime.Now;
                    dr["Value"] = voucherDetails.Value;
                    dr["VoucherID"] = voucherDetails.BarCode;

                    if (voucherDetails.VoucherType == "1")
                    {
                        dr["VoucherType"] = "Clubcard";
                    }
                    else if (voucherDetails.VoucherType == "4")
                    {
                        dr["VoucherType"] = "Bonus";
                    }
                    if (voucherDetails.VoucherType == "5")
                    {
                        dr["VoucherType"] = "Top Up";
                    }

                    dr["ExpiryDate"] = voucherDetails.ExpiryDate;
                    dr["CCNumber"] = cardNumber;
                    dr["Flag"] = "V";

                    dtPrintDetail.Rows.Add(dr);
                }
                dsVouchers.Tables.Add(dtPrintDetail);
                _logData.RecordStep("Received Print data");
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
                request.Parameters.Add(ParameterNames.DS_VOUCHER, dsVouchers);
                response = this._customerServiceAdapter.Set<VoucherDetails>(request);
                _logger.Submit(_logData);
                return response.Status;

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Recording Vouchers Printed", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public VoucherRewardDetailsOverallSummaryModel GetVoucherRewardCounts(long customerId, string culture)
        {
            VoucherRewardDetailsOverallSummaryModel voucherCountModel = null;
            AccountDetails accountDetails = null;
            LogData logData = new LogData();
            try
            {
                accountDetails = GetCustomerAccountDetails(customerId, culture);
                if (accountDetails != null)
                {
                    voucherCountModel = this.GetVoucherRewardDetails(accountDetails.ClubcardID);
                    logData.RecordStep("Received Vouchers ViewModel");
                    _logger.Submit(logData);
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Voucher Count", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return voucherCountModel;
        }

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
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_VOUCHER_REWARD_DETAILS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);

                response = _smartVoucherAdapter.Get<List<VoucherRewardDetails>>(request);

                List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
                if (response.Status)
                {
                    voucherRewardDetailsList = (List<VoucherRewardDetails>)response.Data;
                    voucherRewardDetailsOverallSummaryModel = new VoucherRewardDetailsOverallSummaryModel(voucherRewardDetailsList);
                    logData.RecordStep("Received Vouchers Overall Summary");
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
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_USED_VOUCHER_DETAILS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);

                response = _smartVoucherAdapter.Get<List<VoucherUsageSummary>>(request);

                List<VoucherUsageSummary> usageSummary = new List<VoucherUsageSummary>();

                if (response.Status)
                {
                    usageSummary = (List<VoucherUsageSummary>)response.Data;
                    logData.RecordStep("Received Used Vouchers");
                }
                _logger.Submit(logData);
                return usageSummary;
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
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_UNUSED_VOUCHER_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                response = _smartVoucherAdapter.Get<List<VoucherDetails>>(request);

                List<VoucherDetails> voucherList = null;
                if (response.Status)
                {
                    voucherList = (List<VoucherDetails>)response.Data;

                    if (voucherList != null && voucherList.Count > 0)
                    {
                        logData.RecordStep("Get Expired Vouchers");
                        voucherList.ForEach(x => this.totalUnusedVouchers += x.Value.TryParse<decimal>());
                        vouchersExpired = voucherList[0].ExpiryDate <= DateTime.Today.AddMonths(3);
                    }
                }
                logData.RecordStep("Received Unused Vouchers");
                _logger.Submit(logData);
                return voucherList;
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
        private VoucherRewardsMilesModel ApplyRewardMiles(long customerId, long clubcardNumber,string optedForMiles,int voucherRate,int reasonCode)
        {
            string strDispVal = string.Empty;
            string strMiles = string.Empty;
            VoucherRewardsMilesModel voucherRewardsMilesModel = new VoucherRewardsMilesModel();
            LogData logData = new LogData();

            try
            {
                    logData.RecordStep("Get Reward Miles Summary");
                    MilesRewardDetailsSummary summary = GetMilesRewardDetailsSummary(clubcardNumber, reasonCode);
                    strDispVal = summary.FormattedVoucherValue;
                    voucherRewardsMilesModel.optedForMiles = optedForMiles;
                    voucherRewardsMilesModel.milesRate = voucherRate.ToString();
                    voucherRewardsMilesModel.totalRewardPoints = summary.TotalRewardPoints.ToString();
                    strDispVal = (isDecimalDisabled) ? GeneralUtility.GetDecimalTrimmedCurrencyVal(strDispVal.ToString()) : string.Empty;
                    voucherRewardsMilesModel.summaryFormattedVoucherValue = strDispVal;
                    logData.RecordStep("Received Miles Reward Summary");
                
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
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_REWARD_DETAILS_MILES);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, clubcardNumber);
                request.Parameters.Add(ParameterNames.REASON_CODE, reasonCode);

                response = new MCAResponse();

                response = _smartVoucherAdapter.Get<MilesRewardDetails>(request);

                List<MilesRewardDetails> milesRewardDetailsList = response.Data as List<MilesRewardDetails>;
                MilesRewardDetailsSummary summary = new MilesRewardDetailsSummary();
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
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Miles Reward Details Summary", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// To get customer preference, voucher rate and reason code.
        /// <param name="customerId"></param>
        /// <param name="voucherRate"></param>
        /// <param name="reasonCode"></param>
        /// </summary>
        private string GetOptedForMile(long customerId, out int voucherRate, out int reasonCode)
        {
            string optedInFor = string.Empty;
            voucherRate = 0;
            reasonCode = 0;
            LogData logData = new LogData();

            try
            {
                if (customerId != 0)
                {
                    CustomerPreference customerPreferences = new CustomerPreference();

                    MCARequest request = new MCARequest();
                    request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_PREFERENCES);
                    request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                    request.Parameters.Add(ParameterNames.PREFERENCE_TYPE, PreferenceType.NULL);
                    request.Parameters.Add(ParameterNames.OPTIONAL_PREFERENCE, false);

                    response = new MCAResponse();

                    response = _preferenceServiceAdapter.Get<CustomerPreference>(request);

                    customerPreferences = (CustomerPreference)response.Data;
                    logData.RecordStep("Received Customer Preferences data");

                    List<string> PreferenceIds = new List<string>();
                    if (customerPreferences != null && customerPreferences.Preference != null && customerPreferences.Preference.ToList().Count > 0)
                    {
                        List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                        objPreferenceFilter = customerPreferences.Preference.ToList();
                        string PrefID = string.Empty;
                        foreach (var pref in objPreferenceFilter)
                        {
                            if (pref.POptStatus == OptStatus.OPTED_IN)
                            {
                                PrefID = pref.PreferenceID.ToString().Trim();
                                PreferenceIds.Add(PrefID);
                            }
                        }
                        if (PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString())) //Airmiles standard
                        {
                            logData.RecordStep("Opted Preference: AirMiles");
                            optedInFor = "AIRMILES_STD";
                            voucherRate = BusinessConstants.STANDARD_AMILES;  //STANDARD AMILES;
                            reasonCode = BusinessConstants.AIRMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())) //Airmiles premium
                        {
                            logData.RecordStep("Opted Preference: AirMiles Premium");
                            optedInFor = "AIRMILES_PREMIUM";
                            voucherRate = BusinessConstants.PRIMIUM_AMILES;
                            reasonCode = BusinessConstants.AIRMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString())) //BAMiles standard
                        {
                            logData.RecordStep("Opted Preference: BA Miles Standard");
                            optedInFor = "BAMILES_STD";
                            voucherRate = BusinessConstants.STANDARD_BAMILES;
                            reasonCode = BusinessConstants.BAMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString())) //BAMiles premium
                        {
                            logData.RecordStep("Opted Preference: BA Miles Premium");
                            optedInFor = "BAMILES_PREMIUM";
                            voucherRate = BusinessConstants.PRIMIUM_BAMILES;
                            reasonCode = BusinessConstants.BAMILES_REASONCODE;
                        }
                        //Voucher Changes
                        else if (PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString())) //VIRGIN ATLANTIC
                        {
                            logData.RecordStep("Opted Preference: Virgin");
                            optedInFor = "VIRGIN";
                            voucherRate = BusinessConstants.VIRGIN_ATLANTIC;
                            reasonCode = BusinessConstants.VIRGIN_REASONCODE;
                        }
                        //Voucher Changes
                    }
                }
                _logger.Submit(logData);
                return optedInFor;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Opted for Mile", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the Clubcard service to check if it is a Christmas saver customer and get the Christmas saver details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="isXMasClubMember"></param>
        /// <returns></returns>
        private List<ChristmasSaverSummary> ApplyXmasClubMemberArea(long customerId, out bool isXMasClubMember)
        {
            LogData logData = new LogData();
            try
            {
                isXMasClubMember = false;
                List<ChristmasSaverSummary> christmasSaverSummaryList = null;
                if (customerId >= default(long))
                {
                    request = new MCARequest();
                    request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.IS_XMAS_CLUBMEMBER);
                    request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                    request.Parameters.Add(ParameterNames.CULTURE, System.Globalization.CultureInfo.CurrentCulture.Name);

                    response = _clubcardServiceAdapter.Get<bool>(request);

                    isXMasClubMember = response.Data.TryParse<Boolean>();

                    if (response.Status && !isXMasClubMember)
                    {
                        christmasSaverSummaryList = this.GetChristmasSaverSummary(customerId);
                        logData.RecordStep("Return Christmas Saver Summary List");
                    }
                }
                _logger.Submit(logData);

                return christmasSaverSummaryList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Christmas Saver Summary List in ApplyXmasClubMemberArea", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Calls the Clubcard service to get the Christmas saver details 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private List<ChristmasSaverSummary> GetChristmasSaverSummary(long customerId)
        {
            LogData logData = new LogData();
            DateTime startDate, endDate;
            List<string> blackListData = new List<string>();

            try
            {
                DbConfiguration dbConfigs = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, System.Globalization.CultureInfo.CurrentCulture.Name);
                DbConfigurationItem xsConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverCurrDates);
                DbConfigurationItem xsNextConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverNextDates);

                DateTime strXmasCurrStartDate = xsConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                DateTime strXmasCurrEndDate = xsConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                blackListData.Add(strXmasCurrStartDate.ToString());
                blackListData.Add(strXmasCurrEndDate.ToString());

                DateTime strXmasNextStartDate = xsNextConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                DateTime strXmasNextEndDate = xsNextConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                blackListData.Add(strXmasNextStartDate.ToString());
                blackListData.Add(strXmasNextEndDate.ToString());
                logData.BlackLists = blackListData;

                //To check the start date and end date for Xmas saver period.
                if (DateTime.Now.Date < strXmasNextStartDate)
                {
                    logData.RecordStep("Current date is less than the Next Christmas Start Date");
                    startDate = strXmasCurrStartDate;
                    endDate = strXmasCurrEndDate;
                }
                else
                {
                    logData.RecordStep("Current date is greater than the Next Christmas Start Date");
                    startDate = strXmasNextStartDate;
                    endDate = strXmasNextEndDate;
                }

                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CHRISTMAS_SAVER_SUMMARY);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.START_DATE, startDate);
                request.Parameters.Add(ParameterNames.END_DATE, endDate);
                request.Parameters.Add(ParameterNames.CULTURE, System.Globalization.CultureInfo.CurrentCulture.Name);

                response = _clubcardServiceAdapter.Get<List<ChristmasSaverSummary>>(request);

                List<ChristmasSaverSummary> summaryList = response.Data as List<ChristmasSaverSummary>;
                _logger.Submit(logData);
                return summaryList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in VoucherBC while getting Christmas Saver Summary", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        #endregion Private Methods
    }
}