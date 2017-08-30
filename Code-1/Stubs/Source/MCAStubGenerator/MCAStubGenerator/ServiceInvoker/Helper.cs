using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public class Helper
    {
        private string _culture = String.Empty;
        private long _customerID = 0;

        public Helper(string culture)
        {
            this._culture = culture;
        }

        public Helper(string culture, long customerID)
            : this(culture)
        {
            this._customerID = customerID;
        }

        public Tuple<DateTime, DateTime> GetXmasDates()
        {
            Tuple<DateTime, DateTime> xmasDates = null;

            DbConfiguration dbConfigs = this.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates });
            DbConfigurationItem xsConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverCurrDates);
            DbConfigurationItem xsNextConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverNextDates);

            DateTime strXmasCurrStartDate = xsConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
            DateTime strXmasCurrEndDate = xsConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();

            DateTime strXmasNextStartDate = xsNextConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
            DateTime strXmasNextEndDate = xsNextConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();

            if (DateTime.Now.Date < strXmasNextStartDate)
            {
                xmasDates = new Tuple<DateTime, DateTime>(strXmasCurrStartDate, strXmasCurrEndDate);
            }
            else if (DateTime.Now.Date >= strXmasNextStartDate)
            {
                xmasDates = new Tuple<DateTime, DateTime>(strXmasNextStartDate, strXmasNextEndDate);
            }
            return xmasDates;
        }

        public Tuple<string, int, int> GetOptedForMile(long customerId)
        {
            Tuple<string, int, int> tReturn = new Tuple<string, int, int>(String.Empty, 0, 0);

            string optedInFor = string.Empty;
            int voucherRate = 0;
            int reasonCode = 0;

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

                    var response = new MCAResponse();

                    PreferenceServiceAdapter pa = new PreferenceServiceAdapter(new Recorder(customerId));

                    response = pa.Get(request);

                    customerPreferences = (CustomerPreference)response.Data;

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
                            optedInFor = "AIRMILES_STD";
                            voucherRate = BusinessConstants.STANDARD_AMILES;  //STANDARD AMILES;
                            reasonCode = BusinessConstants.AIRMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())) //Airmiles premium
                        {
                            optedInFor = "AIRMILES_PREMIUM";
                            voucherRate = BusinessConstants.PRIMIUM_AMILES;
                            reasonCode = BusinessConstants.AIRMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString())) //BAMiles standard
                        {
                            optedInFor = "BAMILES_STD";
                            voucherRate = BusinessConstants.STANDARD_BAMILES;
                            reasonCode = BusinessConstants.BAMILES_REASONCODE;
                        }
                        else if (PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString())) //BAMiles premium
                        {
                            optedInFor = "BAMILES_PREMIUM";
                            voucherRate = BusinessConstants.PRIMIUM_BAMILES;
                            reasonCode = BusinessConstants.BAMILES_REASONCODE;
                        }
                        //Voucher Changes
                        else if (PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString())) //VIRGIN ATLANTIC
                        {
                            optedInFor = "VIRGIN";
                            voucherRate = BusinessConstants.VIRGIN_ATLANTIC;
                            reasonCode = BusinessConstants.VIRGIN_REASONCODE;
                        }
                        tReturn = new Tuple<string, int, int>(optedInFor, voucherRate, reasonCode);
                    }
                }
                return tReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbConfiguration GetDBConfigurations(List<DbConfigurationTypeEnum> configurationTypes)
        {
            DbConfiguration configuration = new DbConfiguration();
            StringBuilder configurationTypesCsv = new StringBuilder();
            CustomerServiceAdapter ca = new CustomerServiceAdapter(new MCA.Web.Common.ResponseRecorder.Recorder(this._customerID));

            configurationTypes.ForEach(c => configurationTypesCsv.Append((int)c + ","));
            MCARequest request = new MCARequest();
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CONFIGURATIONS);
            request.Parameters.Add(ParameterNames.CONFIGURATION_TYPES, configurationTypesCsv.ToString());
            request.Parameters.Add(ParameterNames.CULTURE, this._culture);
            MCAResponse response = ca.Get(request);
            configuration = response.Data as DbConfiguration;

            return configuration;

        }

        public long GetHouseholdID()
        {
            CustomerServiceAdapter ca = new CustomerServiceAdapter(new MCA.Web.Common.ResponseRecorder.Recorder(this._customerID));

            MCARequest request = new MCARequest();

            request.Parameters[ParameterNames.OPERATION_NAME] = OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER;

            var response = ca.Get(request);

            if (response != null && response.Status)
            {
                return (((List<HouseholdCustomerDetails>)response.Data).FirstOrDefault() != null) ? ((List<HouseholdCustomerDetails>)response.Data).FirstOrDefault().HouseHoldID : 0;
            }

            return 0;
        }

        public string GetPointsSummaryAndDetails(MCARequest request)
        {
            ClubcardServiceAdapter ca = new ClubcardServiceAdapter(new Recorder(this._customerID));

            request.Parameters[ParameterNames.OPERATION_NAME] = OperationNames.GET_OFFERS_FOR_CUSTOMER;

            var response = ca.Get(request);

            string outputDir = ca.GetRecorder().OutputDir;

            List<Offer> offers = response.Data as List<Offer>;

            if (offers != null)
            {
                foreach (Offer offer in offers)
                {
                    //Call out Get Points Summary
                    request.Parameters[ParameterNames.OPERATION_NAME] = OperationNames.GET_POINTS_SUMMARY;
                    request.Parameters[ParameterNames.OFFER_ID] = offer.Id;
                    request.Parameters[ParameterNames.CUSTOMER_ID] = this._customerID;
                    response = ca.Get(request);

                    //Call out Get Customer Transactions
                    request.Parameters[ParameterNames.OPERATION_NAME] = OperationNames.GET_CUSTOMER_TRANSACTIONS;
                    request.Parameters[ParameterNames.MERCHANTFLAG] = true;
                    response = ca.Get(request);
                }
            }

            return outputDir;
        }
    }
}
