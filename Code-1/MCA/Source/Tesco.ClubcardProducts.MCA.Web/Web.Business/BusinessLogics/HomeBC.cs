using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class HomeBC : IHomeBC
    {
        protected IAccountBC _accountProvider;
        protected IPointsBC _pointsProvider;
        protected IConfigurationProvider _configurationProvider;
        protected IVoucherBC _vouchersProvider;
        protected ILoggingService _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountProvider"></param>
        /// <param name="vouchersProvider"></param>
        /// <param name="pointsProvider"></param>
        /// <param name="configurationProvider"></param>
        /// <param name="logger"></param>
        public HomeBC(  IAccountBC accountProvider,
                        IVoucherBC vouchersProvider,
                        IPointsBC pointsProvider,
                        IConfigurationProvider configurationProvider,
                        ILoggingService logger)
        {
            _accountProvider = accountProvider;
            _vouchersProvider = vouchersProvider;
            _pointsProvider = pointsProvider;
            _configurationProvider = configurationProvider;
            _logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public List<HouseholdCustomerDetails> GetHouseHoldCustomers(long customerID)
        {
            LogData logData = new LogData();
            try
            {
                _logger.Submit(logData);
                return _accountProvider.GetHouseHoldCustomersData(customerID, System.Globalization.CultureInfo.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting household customer details", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });

            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public HomeViewModel GetHomeViewModel(long customerId, string culture)
        {
            LogData logData = new LogData();
            try
            {
                AccountDetails accountDetails = _vouchersProvider.GetCustomerAccountDetails(customerId, culture);
                logData.CaptureData("Getting account details", accountDetails.CustomerID);

                PointsViewModel pointsViewModel = _pointsProvider.GetPointsViewdetails(customerId, culture);
                logData.CaptureData("Getting points details", pointsViewModel.Offers.Count);

                HomeViewModel homeViewModel = new HomeViewModel();

                bool isDecimalDisabled = _configurationProvider.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal);

                if (accountDetails.ClubcardID >= 0)
                {
                    homeViewModel.ClubcardNumber = CryptoUtility.EncryptTripleDES(accountDetails.ClubcardID.TryParse<string>());
                }

                if (isDecimalDisabled)
                {
                    homeViewModel.CustomerVoucherTypeValue = Common.Utilities.GeneralUtility.GetDecimalTrimmedCurrencyVal(Convert.ToString(pointsViewModel.Offers[0].Vouchers));
                }
                else
                {
                    homeViewModel.CustomerVoucherTypeValue = pointsViewModel.Offers[0].Vouchers.TryParse<String>();
                }
                
                homeViewModel.CustomerPointsBalance = accountDetails.PointsBalanceQty.ToString();

                #region NameFieldConfigurations

                CustomerDisplayName customerName = new CustomerDisplayName();
                customerName.TitleEnglish =string.IsNullOrEmpty(accountDetails.TitleEnglish) ? string.Empty :  accountDetails.TitleEnglish;
                customerName.Name1=string.IsNullOrEmpty(accountDetails.Name1) ? string.Empty : accountDetails.Name1;
                customerName.Name2 = string.IsNullOrEmpty(accountDetails.Name2) ? string.Empty : accountDetails.Name2;
                customerName.Name3 =string.IsNullOrEmpty(accountDetails.Name3) ? string.Empty : accountDetails.Name3;

                GeneralUtility name = new GeneralUtility();
                homeViewModel.WelcomeMessage = name.GetCustomerDisplayName(customerName,"HOME");
                #endregion NameFieldConfigurations

                DateTime dtTemp = DateTime.Now;
                DateTime? ptsSummaryStartDate = null, ptsSummaryEndDate = null;

                DbConfiguration configurations = _accountProvider.GetDBConfigurations
                                                    (
                                                        new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates },
                                                        System.Globalization.CultureInfo.CurrentCulture.Name
                                                    );

                DbConfigurationItem ptsSummaryConfig = configurations.ConfigurationItems
                                                        .Find(c => c.ConfigurationName == DbConfigurationItemNames.PtsSummaryDates);
                                
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

                logData.RecordStep("Got points summary config data  for holding  dates");
                double NoOfDaysLeftForConversionMesg = _configurationProvider.GetStringAppSetting(AppConfigEnum.NoOfDaysLeftForConversionMesg).TryParse<Int64>();
                pointsViewModel.PointsStrip.IsClickable = true;

                homeViewModel.PointsStrip = pointsViewModel.PointsStrip;

                if (ptsSummaryStartDate.HasValue && ptsSummaryEndDate.HasValue)
                {
                    if (DateTime.Now.Date >= ptsSummaryStartDate && DateTime.Now.Date <= ptsSummaryEndDate)
                    {
                        homeViewModel.MyMessageHeader = "PointsError";
                    }
                    else
                    {
                        homeViewModel.MyMessageHeader = "Preference";
                    }

                    if (DateTime.Now.Date <= ptsSummaryStartDate)
                    {
                        if (ptsSummaryStartDate <= DateTime.Now.AddDays(Convert.ToInt32(NoOfDaysLeftForConversionMesg)))
                        {
                            homeViewModel.DaysLeftForConversion = Convert.ToInt32(ptsSummaryStartDate.Value.Subtract(DateTime.Now.Date).TotalDays);
                        }
                    }
                }
                else
                {
                    homeViewModel.MyMessageHeader = "Preference";
                }
                logData.RecordStep(string.Format("Message headers section for home page is {0}",homeViewModel.MyMessageHeader));
                _logger.Submit(logData);
                return homeViewModel;
            }
            catch(Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting Home View Model method", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }

        public PopUpModel GetPopUpViewModel(string popupType, long customerid)
        {
            LogData logData = new LogData();
            PopUpModel popUpModel = new PopUpModel();
            
            try
            {
                switch (popupType.ToLower())
                {
                    case "gopaperless":
                        logData.RecordStep("Calling paperless modal ");
                        popUpModel = this.GetGoPaperlessModel(customerid);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting popup view model", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }

            return popUpModel;
        }

        private PopUpModel GetGoPaperlessModel(long customerID)
        {
            LogData logData = new LogData();
            PopUpModel popUpModel = new PopUpModel();

            try
            {
                IPersonalDetailsBC _personalDetailsProvider = ServiceLocator.Current.GetInstance<IPersonalDetailsBC>();
                logData.RecordStep("Invoking Get Customer Preferences required for Modal");
                CustomerPreference preferences = _personalDetailsProvider.GetCustomerPreferences(customerID);
                logData.RecordStep("Got customer preferences back from the provider");
                logData.CaptureData("GetCustomerPreferences", preferences);

                popUpModel.Template = "_ModalPaperless";
                popUpModel.trigger = (preferences != null &&
                        (preferences.Preference != null && preferences.Preference.Length > 0) &&
                        (preferences.Preference.Any(p => p.PreferenceID == (short)PreferenceEnum.Post_Contact && p.POptStatus == OptStatus.OPTED_IN))) ? "load" : "none";

                popUpModel.PrimaryButton.IsVisible = true;
                popUpModel.PrimaryButton.Text = "btnContactPreferences";
                popUpModel.PrimaryButton.Link = "lnkContactPreferences";
                return popUpModel;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting popup view model for preferences", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
        }


        public StampModel GetStampViewModel(List<string> Urls)
        {
            LogData _logData = new LogData();
            StampModel model = new StampModel();
            try
            {
                model.Stamp1 = GetActionUrl(Urls[0]);
                model.Stamp2 = GetActionUrl(Urls[1]);
                model.Stamp3 = GetActionUrl(Urls[2]);
                model.Stamp4 = GetActionUrl(Urls[3]);
                model.Stamp5 = GetActionUrl(Urls[4]);
                model.Stamp6 = GetActionUrl(Urls[5]);
                model.Stamp7 = GetActionUrl(Urls[6]);
                model.Stamp8 = GetActionUrl(Urls[7]);
                model.Stamp9 = GetActionUrl(Urls[8]);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting stamps at home screen", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, _logData }
                                                                        });

            }
            return model;
        }

        private MVCURL GetActionUrl(string value)
        {
            LogData _logData = new LogData();
            try
            {
                MVCURL url = new MVCURL();
                string[] values = value.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                url.Controller = values.Length > 0 ? values[0] : string.Empty;
                url.Action = values.Length > 1 ? values[1] : string.Empty;
                _logger.Submit(_logData);
                return url;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting stamps at home screen", ex, new Dictionary<string, object>()
                                                                        {
                                                                            { LogConfigProvider.EXCLOGDATAKEY, _logData }
                                                                        });

            }

        }
    }
}
