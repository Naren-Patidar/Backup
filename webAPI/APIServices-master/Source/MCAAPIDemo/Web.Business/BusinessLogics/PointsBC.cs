using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using ClubcardOnline.PointsSummarySequencing;
using System.Web;
using System.Globalization;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common.StatementFormatProvider;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class PointsBC : IPointsBC
    {
        protected IAccountBC _accountProvider;
        private IServiceAdapter _preferenceserviceAdapter;
        private IServiceAdapter _smartVoucherServiceAdapter;
        private IServiceAdapter _clubcardServiceAdapter;
        private IConfigurationProvider _configurationProvider;
        private IStatementFormatProvider _statementFormatProvider;
        private ILoggingService _logger;

        public PointsBC(IAccountBC accountProvider, IServiceAdapter preferenceServiceAdapter, IServiceAdapter smartVoucherServiceAdapter,
            IServiceAdapter clubcardServiceAdapter, IConfigurationProvider configurationProvider, IStatementFormatProvider statementFormatProvider,ILoggingService logger)
        {
            _accountProvider = accountProvider;
            _preferenceserviceAdapter = preferenceServiceAdapter;
            _smartVoucherServiceAdapter = smartVoucherServiceAdapter;
            _clubcardServiceAdapter = clubcardServiceAdapter;
            _configurationProvider = configurationProvider;
            _statementFormatProvider = statementFormatProvider;
            this._logger = logger;
        }

        #region Public Methods

        public PointsViewModel GetPointsViewdetails(long CustomerID, string culture)
        {
            LogData _logData = new LogData();
            PointsViewModel pointsViewModelDetails = new PointsViewModel();
            List<Offer> offerList = new List<Offer>();
            List<Offer> pointInfoOffers = new List<Offer>();
            List<Offer> tempPointInfoOffers = new List<Offer>();
            int tempPointsEarned = 0;
            bool pointsEarnedEver = false;
            try
            {
                
                offerList = GetOffersForCustomer(CustomerID, culture);
                foreach (Offer offer in offerList)
                {
                    tempPointsEarned = Convert.ToInt32(offer.PointsBalanceQty);
                    //Check if the tempPointsEarned not zero, if true then set pointsEarnedEver to true
                    if (tempPointsEarned != 0 && !pointsEarnedEver)
                    {
                        _logData.RecordStep("Points Earned Ever: True");
                        pointsEarnedEver = true;
                    }

                    //For Current collection period row, set the other UI fields like Points total
                    // vouchers and collection period dates
                    if (offer.Period.ToUpper() == "CURRENT")
                    {
                        _logData.RecordStep("Offer Period : Current");
                        string optedPreference = string.Empty;
                        string vouchers = string.Empty;
                        GetOptedPreferenceAndVouchers(CustomerID, culture, offer, out optedPreference, out vouchers);
                        offer.Vouchers = vouchers;
                        pointsViewModelDetails.OptedPreference = optedPreference;
                        pointInfoOffers.Add(offer);
                    }
                    else { tempPointInfoOffers.Add(offer); }
                }

                //Bind filtered data (imported rows) to repeater
                if (pointsEarnedEver)
                {
                    pointInfoOffers.AddRange(tempPointInfoOffers.ToArray());
                    pointsViewModelDetails.IsPointEarnedEver = true;
                }
                else
                {
                    pointsViewModelDetails.IsPointEarnedEver = false;
                }
                pointsViewModelDetails.Offers = pointInfoOffers;
                _logData.CaptureData("Points Details", pointsViewModelDetails);
                _logger.Submit(_logData);                
                return pointsViewModelDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points BC while getting Points Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public PointsSummaryModel GetPointsSummaryModel(long customerID, int offerID, string culture)
        {
            LogData _logData = new LogData();
            PointsSummaryModel pointsSummaryModel = new PointsSummaryModel();
            DateTime dtTemp = DateTime.Now;
            DateTime? ptsSummaryStartDate = null, ptsSummaryEndDate = null;
            try
            {
                DbConfiguration configurations = _accountProvider.GetDBConfigurations
                                                    (
                                                        new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates },
                                                        System.Globalization.CultureInfo.CurrentCulture.Name
                                                    );

                DbConfigurationItem ptsSummaryConfig = configurations.ConfigurationItems
                                                        .Find(c => c.ConfigurationName == DbConfigurationItemNames.PtsSummaryDates);
                _logData.CaptureData("Points Summary Config", ptsSummaryConfig);

                if (ptsSummaryConfig != null)
                {
                    ptsSummaryConfig.ConfigurationValue1.Trim().TryParseDate(out dtTemp);
                    ptsSummaryStartDate = dtTemp;
                }

                if (ptsSummaryConfig != null)
                {
                    ptsSummaryConfig.ConfigurationValue2.TryParseDate(out dtTemp);
                    ptsSummaryEndDate = dtTemp;
                }

                //logData.CaptureData("Holding dates for points page", ptsSummaryConfig);

                if (ptsSummaryStartDate.HasValue && ptsSummaryEndDate.HasValue &&
                    DateTime.Now.Date >= ptsSummaryStartDate && DateTime.Now.Date <= ptsSummaryEndDate
                    && offerID == MCACookie.Cookie[MCACookieEnum.firstPrevOfferID].TryParse<Int16>())
                {
                    pointsSummaryModel.PointsSummaryCutOffDate = ptsSummaryStartDate.Value;
                    pointsSummaryModel.IsHoldingPage = true;
                    _logData.RecordStep(string.Format("IsHoldingPage: {0}", pointsSummaryModel.IsHoldingPage));

                }
                else
                {
                    pointsSummaryModel.IsHoldingPage = false;
                    _logData.RecordStep(string.Format("IsHoldingPage: {0}", pointsSummaryModel.IsHoldingPage));

                }
                PointsSummary pointsSummary = GetPointsSummary(customerID, offerID, culture);
                if (pointsSummary != null)
                {
                    PointsSummaryDetailsModel pointsDetailsModel = new PointsSummaryDetailsModel(pointsSummary);
                    pointsDetailsModel.OfferID = offerID;
                    if (offerID == 81 && pointsDetailsModel.StatementType == StatementTypesEnum.XmasSavers)
                    {
                        pointsDetailsModel.StatementType = StatementTypesEnum.NonReward;
                    }
                    DateTime offerStartDateTime, offerEndDateTime;
                    DateTime.TryParse(pointsSummary.StartDateTime, out offerStartDateTime);
                    DateTime.TryParse(pointsSummary.EndDateTime, out offerEndDateTime);
                    pointsDetailsModel.StartDateTime = offerStartDateTime.ToString(_configurationProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat), CultureInfo.InvariantCulture);
                    pointsDetailsModel.EndDateTime = offerEndDateTime.ToString(_configurationProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat), CultureInfo.InvariantCulture);
                    pointsSummaryModel.PointsDetails = pointsDetailsModel;
                    pointsSummaryModel.TescoBankPointsModelList = LoadPointsBoxes(pointsSummary, pointsDetailsModel.StatementType, "TescoBankPoints", offerID);
                    pointsSummaryModel.TescoPointsModelList = LoadPointsBoxes(pointsSummary, pointsDetailsModel.StatementType, "TescoPoints", offerID);
                }
                _logData.CaptureData("Points Details", pointsSummaryModel);
                _logger.Submit(_logData);     
                return pointsSummaryModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points BC while getting Points Summary Model.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public CustomerTransactions GetCustomerTransactions(long customerID, int offerID, bool merchantFlag,string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("offerid :{0}, Merchant Flag: {1} ", offerID, merchantFlag));

            try
            {
                CustomerTransactions customerTransaction = new CustomerTransactions();
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_TRANSACTIONS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.OFFER_ID, offerID);
                request.Parameters.Add(ParameterNames.MERCHANTFLAG, merchantFlag);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                //request and response object should be logged in adapter class- review comment provided
                MCAResponse response = _clubcardServiceAdapter.Get<CustomerTransactions>(request);

                if (response.Status)
                {
                    customerTransaction = (CustomerTransactions)response.Data;
                }
                if (customerTransaction.Transactions != null)
                {
                    decimal amount;
                    foreach (TransactionDetails transaction in customerTransaction.Transactions)
                    {
                        transaction.ClubcardId = GeneralUtility.MasknFormatClubcard(transaction.ClubcardId,true,'X');
                        Decimal.TryParse(transaction.AmountSpent.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount);
                        transaction.AmountSpent = amount.ToString();
                    }
                }
                _logData.CaptureData("Customer Transactions", customerTransaction);
                _logger.Submit(_logData);
                return customerTransaction;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points BC while getting Customer Transactions", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        public List<Offer> GetOffersForCustomer(long customerId, string culture)
        {
            LogData _logData = new LogData();
            MCARequest request = new MCARequest();
            Hashtable inputParams = new Hashtable();
            List<Offer> offers = new List<Offer>();
            try
            {
               
                inputParams[ParameterNames.CUSTOMER_ID_Points] = customerId;
                //Convert all input variables to xml
                string conditionalXml = GeneralUtility.HashTableToXML(inputParams, "PointsInfoCondition");
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_OFFERS_FOR_CUSTOMER);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID_Points, customerId);
                request.Parameters.Add(ParameterNames.CONDITIONAL_XML, conditionalXml);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                request.Parameters.Add(ParameterNames.MAX_ROWS, 0);

                MCAResponse response = _clubcardServiceAdapter.Get<List<Offer>>(request);
                offers = response.Data as List<Offer>;
                //Response and request obbject - conditionalXML should be logged in Clubcard Service Adapter -review comment provided
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in PointsBC while getting Offers for the customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return offers;
        }

        public PointsSummary GetPointsSummary(long customerID, int offerID, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("Culture :{0}", culture));

            PointsSummary pointsSummary = new PointsSummary();
            MCARequest request = new MCARequest();

            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_POINTS_SUMMARY);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.OFFER_ID, offerID);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                //Request and response should be logged in ClubcadrService Adapter -Review COmment provided
                MCAResponse response = _clubcardServiceAdapter.Get<List<PointsSummary>>(request);
                if (response.Status)
                {
                    pointsSummary = (PointsSummary)response.Data;
                }
                _logger.Submit(_logData);       
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Points Summary.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return pointsSummary;
        }

        #endregion

        #region Private Methods
        private void GetOptedPreferenceAndVouchers(long customerID, string culture, Offer offer, out string optedPreference, out string vouchers)
        {
            LogData _logData = new LogData();
            Decimal VoucherCost = Convert.ToDecimal(offer.Vouchers);
            Decimal miles = 0;
            CustomerPreference customerPreference = new CustomerPreference();
            MCARequest request = new MCARequest();                        
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_PREFERENCES);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.PREFERENCE_TYPE, PreferenceType.NULL);
                request.Parameters.Add(ParameterNames.OPTIONAL_PREFERENCE, false);

                MCAResponse response = _preferenceserviceAdapter.Get<CustomerPreference>(request);
                customerPreference = (CustomerPreference)response.Data;
                List<string> PreferenceIds = new List<string>();
                if (customerPreference != null && customerPreference.Preference != null && customerPreference.Preference.ToList().Count > 0)
                {
                    _logData.RecordStep("Customer Preferences are not null");
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = customerPreference.Preference.ToList();
                    string PrefID = string.Empty;

                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.POptStatus == OptStatus.OPTED_IN)
                        {
                            PrefID = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(PrefID);
                        }
                    }
                    if (PreferenceIds.Contains(BusinessConstants.XMASSAVER.ToString()))    //Ecoupon
                    {
                        optedPreference = "xmasmiles";
                        vouchers = offer.Vouchers;
                    }
                    else if ((PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString())) || PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString()))
                    {
                        optedPreference = "BAmiles";//BAmiles
                        if (PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString()))
                        {
                            miles = (BusinessConstants.PRIMIUM_BAMILES * Convert.ToDecimal(VoucherCost)) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                            vouchers = miles.ToString();
                        }
                        else
                        {
                            miles = (BusinessConstants.STANDARD_BAMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                            vouchers = miles.ToString();
                        }
                    }
                    else if (PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString()))
                    {
                        optedPreference = "virginMiles";//virginMiles
                        miles = (BusinessConstants.VIRGIN_ATLANTIC * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                        vouchers = miles.ToString();
                    }
                    else if ((PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())) || PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString()))
                    {
                        optedPreference = "aviosMiles";//aviosMiles
                        if ((PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())))
                        {
                            miles = (BusinessConstants.PRIMIUM_AMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                            vouchers = miles.ToString();
                        }
                        else
                        {
                            miles = (BusinessConstants.STANDARD_AMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                            vouchers = miles.ToString();
                        }
                    }
                    else
                    {
                        optedPreference = "stdVoucher";
                        vouchers = GetRewardsDetail(customerID, culture).ToString();
                    }
                }
                else
                {
                    _logData.RecordStep("Customer Preferences are  null -Standard Voucher Customer");
                    optedPreference = "stdVoucher"; //stdVoucher
                    vouchers = GetRewardsDetail(customerID, culture).ToString();
                }
                _logData.CaptureData("Opted Preference: ", optedPreference);
                _logData.CaptureData("Vouchers: ", vouchers);
                _logger.Submit(_logData);                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in PointsBC while getting Opted Preferences.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private decimal GetRewardsDetail(long customerID, string culture)
        {
            LogData _logData = new LogData();
            string cardNumber = null;
            AccountDetails customerAccountDetails = new AccountDetails();
            MCARequest request = new MCARequest();
            try
            {
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                MCAResponse response = _clubcardServiceAdapter.Get<AccountDetails>(request);
                //response should be captured in Clubcard Service adapter-review comment provided
                customerAccountDetails = (AccountDetails)response.Data;
                if (response.Status)
                {
                    if (customerAccountDetails != null)
                    {
                        cardNumber = customerAccountDetails.ClubcardID.ToString();
                    }
                }
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_VOUCHER_REWARD_DETAILS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, cardNumber);

                //request object after blacklistingt the clubcard no. and response object should be logged in smart voucher adapter class
                response = _smartVoucherServiceAdapter.Get<List<VoucherRewardDetails>>(request);
                List<VoucherRewardDetails> voucherRewardDetails = (List<VoucherRewardDetails>)response.Data;
                decimal totalRewardLeftOver = 0;
                if (response.Status)
                {
                    if (voucherRewardDetails.Count != 0)
                    {
                        for (int i = 0; i < voucherRewardDetails.Count; i++)
                        {
                            totalRewardLeftOver = totalRewardLeftOver + Convert.ToDecimal((voucherRewardDetails[i].RewardLeftOver.ToString()));
                        }
                    }
                }
                _logData.CaptureData("Total Reward Left Over:", totalRewardLeftOver);
                _logger.Submit(_logData);
                return totalRewardLeftOver;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Points while getting Reward Details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            
        }

        private List<TescoPointsModel> LoadPointsBoxes(PointsSummary pointsSummary, StatementTypesEnum statementType, string sectionType, Int32 requestPrevOfferId)
        {
            #region Local Variables
            LogData _logData = new LogData();
            StatementFormat stformat;
            Statement selStatement;
            ArrayList pointsBoxes;
            short boxCounter = 0, totalRows, totalColumns;
            List<TescoPointsModel> tescoPointsModelList = new List<TescoPointsModel>();

            #endregion

            try
            {
                string path = _configurationProvider.GetStringAppSetting(AppConfigEnum.StatementFormatPath);
               

                stformat = _statementFormatProvider.GetStatementFormat(requestPrevOfferId, path);
                _logData.RecordStep(string.Format("Statement Format Path: {0}", stformat));

                //Load the appropriate statement from statement format with respect to the statement types :
                //Standard, NonReward, Xmas, AirMiles or BAMiles
                selStatement = stformat[statementType.ToString()];
                pointsBoxes = selStatement[sectionType];
                totalRows = Convert.ToInt16(pointsBoxes.Count);
                totalColumns = 1;

                for (int row = 1; row <= totalRows; row++)
                {
                    TescoPointsModel tescoPointsModel = new TescoPointsModel();
                    for (int column = 1; column <= totalColumns; column++)
                    {
                        if (boxCounter < pointsBoxes.Count)
                        {
                            //get the appropriate PointsBox object from the Array
                            PointsBox box = pointsBoxes[boxCounter] as PointsBox;
                            //render the image if the Box has a logo name
                            if (!string.IsNullOrEmpty(box.BoxLogoFileName))
                            {
                                tescoPointsModel.ImageURL = box.BoxLogoFileName;
                                tescoPointsModel.ImageAlt = box.BoxName;
                            }
                            if (!string.IsNullOrEmpty(box.DataColumnName))
                            {
                                PointsTypeEnum pointsType = (PointsTypeEnum)Enum.Parse(typeof(PointsTypeEnum), box.DataColumnName);
                                if (pointsSummary.pointsBreakup.ContainsKey(pointsType))
                                {
                                    tescoPointsModel.TotalPoints = pointsSummary.pointsBreakup[pointsType].TryParse<String>();
                                }
                            }
                        }
                        tescoPointsModelList.Add(tescoPointsModel);
                        boxCounter++;
                    }
                }
                _logData.CaptureData("Points Details", tescoPointsModelList);
                _logger.Submit(_logData);       
                return tescoPointsModelList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while loading point boxes.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }
        #endregion

    }
}
