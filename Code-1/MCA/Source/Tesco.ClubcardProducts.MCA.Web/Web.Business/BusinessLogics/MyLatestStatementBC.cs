using System;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class MyLatestStatementBC : IMyLatestStatementBC
    {
        IAccountBC _accountProvider;
        IPointsBC _pointsProvider;
        IServiceAdapter _clubcardServiceAdapter;
        private ILoggingService _logger;
        IConfigurationProvider _configProvider = null;
        string culture = String.Empty;

        public MyLatestStatementBC(IAccountBC accountProvider, IPointsBC pointsProvider, IServiceAdapter clubcardServiceAdapter, ILoggingService logger, IConfigurationProvider configProvider)
        {
            _accountProvider = accountProvider;
            _pointsProvider = pointsProvider;
            _clubcardServiceAdapter = clubcardServiceAdapter;
            _logger = logger;
            _configProvider = configProvider;
            
            this.culture = System.Globalization.CultureInfo.CurrentCulture.Name;
        }
        
        /// <summary>
        /// Checks if the Holding page is enabled/disabled
        /// </summary>
        /// <param name="ptsSummaryStartDate"></param>
        /// <param name="ptsSummaryEndDate"></param>
        /// <returns></returns>
        public bool IsHoldingPageEnabled(out DateTime? ptsSummaryStartDate, out DateTime? ptsSummaryEndDate)
        {
            LogData logData = new LogData();
            try
            {
                bool isHoldingPageEnabled = true;
                DateTime dtTemp = DateTime.Now;
                logData.RecordStep("Getting DB Configurations for MLS..");
                DbConfiguration configurations = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, System.Globalization.CultureInfo.CurrentCulture.Name);
                DbConfigurationItem mlsConfig = configurations.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.MyLatestStatementDates);
                DbConfigurationItem ptsSummaryConfig = configurations.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.PtsSummaryDates);
                logData.CaptureData("MLS Dates", mlsConfig);
                logData.CaptureData("Point Summary Dates", ptsSummaryConfig);
                DateTime? mlsStartDate = null;
                
                if(mlsConfig != null)
                {
                    mlsConfig.ConfigurationValue1.Trim().TryParseDate(out dtTemp);
                    mlsStartDate = dtTemp;
                }

                DateTime? mlsEndDate = null;
                if (mlsConfig != null)
                {
                    mlsConfig.ConfigurationValue2.Trim().TryParseDate(out dtTemp);
                    mlsEndDate = dtTemp;
                }

                if (ptsSummaryConfig != null)
                {
                    ptsSummaryConfig.ConfigurationValue1.Trim().TryParseDate(out dtTemp);
                    ptsSummaryStartDate = dtTemp;
                }
                else
                {
                    ptsSummaryStartDate = null;
                }

                if (ptsSummaryConfig != null)
                {
                    ptsSummaryConfig.ConfigurationValue2.TryParseDate(out dtTemp);
                    ptsSummaryEndDate = dtTemp;
                }
                else
                {
                    ptsSummaryEndDate = null;
                }

                //checking the holding dates is active or not
                //Display the Holding page only if current dates is between holding dates, otherwise show MLS page
                DateTime dtCurrent = DateTime.Now;
                isHoldingPageEnabled = ((mlsStartDate.HasValue && mlsEndDate.HasValue) && (dtCurrent >= mlsStartDate && dtCurrent <= mlsEndDate));
                logData.CaptureData("Is Holding Page Enabled value:  ", isHoldingPageEnabled);
                _logger.Submit(logData);
                return isHoldingPageEnabled;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MLS BC while checking Holding Page Enabled value", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// Gets the MLS View Model details for the MLS page
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public MyLatestStatementViewModel GetMLSViewDetails(long CustomerId)
        {
            LogData logData = new LogData();
            MyLatestStatementViewModel myLatestStatementViewModel = new MyLatestStatementViewModel();
            int offerNumber = 0;
            Offer prevOffer = null;
            DateTime? currentOfferStartDate = null, currentOfferEndDate = null;
            string sRewardMilesRateFlag = string.Empty;
            try
            {
                List<Offer> offers = _pointsProvider.GetOffersForCustomer(CustomerId, this.culture);
                logData.CaptureData("Offer list for Customer is ", offers);
                string sDateFormat = _configProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);
                logData.RecordStep(String.Format("Current Date format is {0}",sDateFormat));
                foreach (Offer offer in offers)
                {
                    offerNumber++;
                    if (offer.Period.ToUpper() == "CURRENT")
                    {
                        currentOfferStartDate = offer.StartDateTime;
                        currentOfferEndDate = offer.EndDateTime;
                        logData.RecordStep(string.Format("Current offer Start & End Dates :{0},{1}", currentOfferStartDate,currentOfferEndDate));
                    }
                    //Fetch the Previous Collection Period Dates and OfferId
                    if (offerNumber == 2)
                    {

                        prevOffer = offer;
                        myLatestStatementViewModel.prevOfferId = offer.Id;
                        myLatestStatementViewModel.pointsPrevStartDate = offer.StartDateTime.HasValue ? offer.StartDateTime : null;
                        myLatestStatementViewModel.pointsPrevEndDate = offer.EndDateTime.HasValue ? offer.EndDateTime : null;
                    }
                }

                #region Holding page date to be displayed

                DateTime? ptsSummaryStartDate = null;
                DateTime? ptsSummaryEndDate = null;
                myLatestStatementViewModel.isHoldingPageEnabled = this.IsHoldingPageEnabled(out ptsSummaryStartDate, out ptsSummaryEndDate);
                logData.CaptureData("Is Holding Page Enabled ", myLatestStatementViewModel.isHoldingPageEnabled);

                if (myLatestStatementViewModel.isHoldingPageEnabled || CustomerId == 0 || myLatestStatementViewModel.prevOfferId == 0)
                {
                    logData.RecordStep("Return to display Holding page date");
                    _logger.Submit(logData);
                    return myLatestStatementViewModel;
                }

                #endregion

                #region Get Points Summary to Variables

                
                PointsSummary pointSummary = _pointsProvider.GetPointsSummary(CustomerId, myLatestStatementViewModel.prevOfferId, this.culture);
                
                logData.CaptureData("Points Summary Data : ", pointSummary);

                if (pointSummary == null || pointSummary.CustomerType.Equals("0"))
                {
                    myLatestStatementViewModel.isHoldingPageEnabled = true;
                    logData.RecordStep("Holding Page is enabled.");
                    _logger.Submit(logData);
                    return myLatestStatementViewModel;
                }

                myLatestStatementViewModel.pointsSummary = pointSummary;

                //To get Standard and Premium flag
                //H Denotes Premium 
                //L Denotes Standard
                if (!String.IsNullOrWhiteSpace(pointSummary.RewardMilesRate))
                {
                    logData.CaptureData("Rewards Miles Rate is ", pointSummary.RewardMilesRate);
                    sRewardMilesRateFlag = pointSummary.RewardMilesRate.Trim();
                }

                
                myLatestStatementViewModel.dMiles = this.FindConversionRate(sRewardMilesRateFlag, pointSummary.CustomerType.ToUpper());
                
                
                myLatestStatementViewModel.voucherPerMileFifty = BusinessConstants.VOUCHER_PERMILE_FIFTY.ToString();
                
                #endregion
                _logger.Submit(logData);
                return myLatestStatementViewModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MLS BC while getting MLS View Details", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        /// <summary>
        /// Conversion Rate for different CustomerType to get the offered Miles
        /// </summary>
        /// <param name="dMiles"></param>
        /// <param name="sRewardMilesRateFlag"></param>
        /// <param name="sCustomerType"></param>
        public decimal FindConversionRate(string sRewardMilesRateFlag, string sCustomerType)
        {
            LogData logData = new LogData();
            try
            {
                decimal dMiles = 0;
                logData.CaptureData("Rewards Miles Rate flag is  ", sRewardMilesRateFlag);
                logData.CaptureData("Customer Type is   ", sCustomerType);

                //Convert 2.50 to 250 to calculate miles rate
                decimal VOUCHER_PERMILE = Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString(), CultureInfo.InvariantCulture) * 100;

                #region Premium

                //BA Avios -Premium
                if (sRewardMilesRateFlag.Equals("H") && (sCustomerType.Equals(BusinessConstants.AVOIS_BRITISHAIRWAYS_EXCLUB_CUSTOMERS) || sCustomerType.Equals(BusinessConstants.AVOIS_NON_REWARD_BRITISHAIRWAYS_EXCLUB_CUSTOMERS)))
                {
                    //800 * 50 / 250 = XX Miles -- > Premium
                    logData.RecordStep("Get Miles Rate for BA Avios - Premium");
                    dMiles = (BusinessConstants.PRIMIUM_BAMILES * BusinessConstants.VOUCHER_PERMILE_FIFTY) / VOUCHER_PERMILE;
                    
                }
                //Avios -Premium
                else if (sRewardMilesRateFlag.Equals("H") && (sCustomerType.Equals(BusinessConstants.AVIOS_CUSTOMERS) || sCustomerType.Equals(BusinessConstants.AVIOS_NON_REWARD_CUSTOMERS)))
                {
                    //800 * 50 / 250 = XX Miles -- > Premium
                    logData.RecordStep("Get Miles Rate for Avios - Premium");
                    dMiles = (BusinessConstants.PRIMIUM_AMILES * BusinessConstants.VOUCHER_PERMILE_FIFTY) / VOUCHER_PERMILE;
                   
                }

                #endregion

                #region Standard

                //BA Avios -Standard
                //For low and empty flag
                else if ((sRewardMilesRateFlag.Equals("L") || sRewardMilesRateFlag.Equals(string.Empty)) && (sCustomerType.Equals(BusinessConstants.AVOIS_BRITISHAIRWAYS_EXCLUB_CUSTOMERS) || sCustomerType.Equals(BusinessConstants.AVOIS_NON_REWARD_BRITISHAIRWAYS_EXCLUB_CUSTOMERS)))
                {
                    //600 * 50 / 250 = XX Miles -- > Standard
                    logData.RecordStep("Get Miles Rate for BA Avios - Standard");
                    dMiles = (BusinessConstants.STANDARD_BAMILES * BusinessConstants.VOUCHER_PERMILE_FIFTY) / VOUCHER_PERMILE;
                    
                }
                //Avios -Standard
                else if ((sRewardMilesRateFlag.Equals("L") || sRewardMilesRateFlag.Equals(string.Empty)) && (sCustomerType.Equals(BusinessConstants.AVIOS_CUSTOMERS) || sCustomerType.Equals(BusinessConstants.AVIOS_NON_REWARD_CUSTOMERS)))
                {
                    //600 * 50 / 250 = XX Miles -- > Standard
                    logData.RecordStep("Get Miles Rate for  Avios - Standard");
                    dMiles = (BusinessConstants.STANDARD_AMILES * BusinessConstants.VOUCHER_PERMILE_FIFTY) / VOUCHER_PERMILE;
                    
                }

                #endregion

                //Virgin Flyer Customer 
                else if (sCustomerType.Equals(BusinessConstants.VIRGIN_FREQUENT_FLYERS_CUSTOMERS) || sCustomerType.Equals(BusinessConstants.VIRGIN_FREQUENT_FLYERS_NON_REWARD_CUSTOMERS))
                {
                    //625 * 50 / 250 = XX Miles 
                    logData.RecordStep("Get Miles Rate for Virgin Flyer Customer");
                    dMiles = (BusinessConstants.VIRGIN_ATLANTIC * BusinessConstants.VOUCHER_PERMILE_FIFTY) / VOUCHER_PERMILE;
                    
                }

                logData.CaptureData("Conversion rate is ", dMiles);
                _logger.Submit(logData);
                return dMiles;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in MLS BC while getting Conversion Rates", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }
    }
}