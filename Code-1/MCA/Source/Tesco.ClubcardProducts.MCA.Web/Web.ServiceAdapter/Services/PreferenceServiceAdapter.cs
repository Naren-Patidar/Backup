using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using CommonEntities = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using PreferencesEntities = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;


namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class PreferenceServiceAdapter : IServiceAdapter
    {
        private readonly ILoggingService _logger = null;

        #region Constructors

        IPreferenceService _preferenceServiceClient = null;
        public PreferenceServiceAdapter(IPreferenceService preferenceServiceClient, ILoggingService logger)
        {
            _preferenceServiceClient = preferenceServiceClient;
            _logger = logger;
        }

        #endregion Constructors

        #region IServiceAdapter Members
        /// <summary>
        /// Data retrieval call for Preference Service
        /// Methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public MCAResponse Get<T>(MCARequest req)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();


            try
            {
                if (req.Parameters[ParameterNames.CUSTOMER_ID] != null)
                {
                    logData.CustomerID = req.Parameters[ParameterNames.CUSTOMER_ID].TryParse<string>();
                }
                var operation = req.Parameters[ParameterNames.OPERATION_NAME].ToString();
                logData.CaptureData("Request Object ", req);
                switch (operation)
                {
                    case OperationNames.GET_CUSTOMER_PREFERENCES:
                        if (req.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && req.Parameters.Keys.Contains(ParameterNames.PREFERENCE_TYPE) && req.Parameters.Keys.Contains(ParameterNames.OPTIONAL_PREFERENCE))
                        {
                            res.Data = GetCustomerPreferences(
                            req.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                            req.Parameters[ParameterNames.PREFERENCE_TYPE].TryParse<CommonEntities.PreferenceType>(),
                            req.Parameters[ParameterNames.OPTIONAL_PREFERENCE].TryParse<Boolean>());
                            res.Status = true;

                            logData.RecordStep("Response received successfully");
                            logData.CaptureData(string.Format("Preference Details  for {0} ", req.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<string>(), res.Data);
                        }
                        break;
                    case OperationNames.GET_CLUB_DETAILS:
                        if (req.Parameters.ContainsKey(ParameterNames.CUSTOMER_ID))
                        {

                            res.Data = GetClubDetails((req.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>());
                            res.Status = true;
                            logData.RecordStep("Response received successfully");
                            logData.CaptureData("Response Object", res);
                        }
                        break;


                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Preference Service Adapter GET", ex,
                            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { "InputParam", req.JsonText() }
                            });

            }
            return res;
        }

        /// <summary>
        /// Data update call returning boolean value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public MCAResponse Set<T>(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();

            var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();
            try
            {
                if (request.Parameters.ContainsKey(ParameterNames.CUSTOMER_ID))
                {
                    logData.CustomerID = request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<string>();
                }
                switch (operation)
                {
                    case OperationNames.SEND_EMAIL_TO_CUSTOMERS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_PREFERENCES) && request.Parameters.Keys.Contains(ParameterNames.CUST_DETAILS))
                        {
                            logData.CaptureData("Request Object - Customer Preference", request.Parameters[ParameterNames.CUSTOMER_PREFERENCES] as CommonEntities.CustomerPreference);
                            res.Data = SendEmailToCustomers((request.Parameters[ParameterNames.CUSTOMER_PREFERENCES]) as CommonEntities.CustomerPreference,
                                                            (request.Parameters[ParameterNames.CUST_DETAILS]) as CommonEntities.AccountDetails);
                            res.Status = true;
                            logData.RecordStep("Response received successfully");
                        }
                        break;
                    case OperationNames.SEND_EMAILNOTICE_TO_CUSTOMERS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_PREFERENCES) && request.Parameters.Keys.Contains(ParameterNames.CUST_DETAILS) && request.Parameters.Keys.Contains(ParameterNames.PAGE_NAME) && request.Parameters.Keys.Contains(ParameterNames.TRACKHT))
                        {
                            logData.CaptureData("Request Object - Customer Preference", request.Parameters[ParameterNames.CUSTOMER_PREFERENCES] as CommonEntities.CustomerPreference);
                            logData.CaptureData("Request Object - Page Name", request.Parameters[ParameterNames.PAGE_NAME].TryParse<string>());
                            logData.CaptureData("Request Object - TRACKHT", request.Parameters[ParameterNames.TRACKHT] as Hashtable);
                            res.Data = SendEmailNoticeToCustomers((request.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>(),
                                                                   (request.Parameters[ParameterNames.CUSTOMER_PREFERENCES]) as CommonEntities.CustomerPreference,
                                                                   (request.Parameters[ParameterNames.CUST_DETAILS]) as CommonEntities.AccountDetails,
                                                                   (request.Parameters[ParameterNames.PAGE_NAME]).TryParse<string>(),
                                                                   (request.Parameters[ParameterNames.TRACKHT]) as Hashtable);
                            res.Status = true;
                            logData.RecordStep("Response received successfully");
                        }
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Preference Service Adapter SET", ex,
                             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { "InputParam", request.JsonText() }
                            });

            }

            return res;
        }

        public MCAResponse Delete<T>(MCARequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Data update call with no return type
        /// </summary>
        /// <param name="request"></param>
        public MCAResponse Execute(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            LogData logData = new LogData();
            var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();
            try
            {
                if (request.Parameters[ParameterNames.CUSTOMER_ID] != null)
                {
                    logData.CustomerID = request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<string>();
                }
                switch (operation)
                {
                    case OperationNames.UPDATE_CUSTOMER_PREFERENCES:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_PREFERENCE) && request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_DETAILS))
                        {

                            logData.CaptureData("Request Object - Customer Preference", request.Parameters[ParameterNames.CUSTOMER_PREFERENCE] as CommonEntities.CustomerPreference);
                            UpdateCustomerPreferences((request.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>(),
                                                       (request.Parameters[ParameterNames.CUSTOMER_PREFERENCE] as CommonEntities.CustomerPreference),
                                                       (request.Parameters[ParameterNames.CUSTOMER_DETAILS]) as AccountDetails);
                            res.Status = true;
                            logData.RecordStep("Response received successfully");

                        }
                        break;
                    case OperationNames.UPDATE_CLUB_DETAILS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CLUB_DETAILS) && request.Parameters.Keys.Contains(ParameterNames.EMAIL_ID_TO))
                        {
                            logData.CaptureData("Request Object - Club Details", request.Parameters[ParameterNames.CLUB_DETAILS] as PreferencesEntities.ClubDetails);
                            logData.CaptureData("Request Object ", request);
                            UpdateClubDetails((request.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>(),
                                               (request.Parameters[ParameterNames.CLUB_DETAILS]) as PreferencesEntities.ClubDetails,
                                               (request.Parameters[ParameterNames.EMAIL_ID_TO]).TryParse<string>());
                            res.Status = true;
                            logData.RecordStep("Response received successfully");
                        }
                        break;
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Preference Service Adapter EXECUTE", ex,
                             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { "InputParam", request.JsonText() }
                            });
            }
            return res;
        }


        #endregion IServiceAdapter Members

        #region Private Members

        private CommonEntities.CustomerPreference GetCustomerPreferences(long customerID, CommonEntities.PreferenceType preferenceType, bool optionalPreference)
        {
            CommonEntities.CustomerPreference preferences = new CommonEntities.CustomerPreference();
            string strpreferenceType = SerializerUtility<CommonEntities.PreferenceType>.GetSerializedString(preferenceType);
            LogData logData = new LogData();


            try
            {
                if (!string.IsNullOrEmpty(customerID.ToString()))
                {
                    logData.CustomerID = customerID.ToString();
                }
                PreferenceServices.PreferenceType prefType = PreferenceServices.PreferenceType.NULL;
                logData.RecordStep(string.Format("Get Customer Preference method executing "));
                PreferenceServices.CustomerPreference customerPreferences = _preferenceServiceClient.ViewCustomerPreference(customerID, prefType, true);
                string xml = customerPreferences.ToXmlString();
                logData.CaptureData("Customer preferences XML ", xml);
                preferences.ConvertFromXml(xml);
                logData.CaptureData("Customer Preferences: ", preferences);
                _logger.Submit(logData);
                return preferences;
            }
            catch (Exception ex)
            {
                logData.CaptureData("Failed while getting Customer Preferences", preferences);
                throw GeneralUtility.GetCustomException(ex.Message, ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private bool SendEmailToCustomers(CommonEntities.CustomerPreference objcustPref, AccountDetails customerdetails)
        {
            bool retVal = false;
            LogData logData = new LogData();

            try
            {
                if (string.IsNullOrEmpty(customerdetails.CustomerID.ToString()))
                {
                    logData.CustomerID = customerdetails.CustomerID.ToString();
                }
                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                logData.CaptureData("Customer preferences XML ", xml);
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();

                if (objcustPref.Preference != null)
                {
                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];
                    for (int i = 0; i < objcustPref.Preference.Length; i++)
                    {
                        var preference = new PreferenceServices.CustomerPreference
                        {
                            PreferenceID = objcustPref.Preference[i].PreferenceID,
                            POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                            UpdateDateTime = objcustPref.Preference[i].UpdateDateTime,
                            EmailSubject = objcustPref.Preference[i].EmailSubject
                        };
                        preferences[i] = preference;
                    }
                    custPref.Preference = preferences;
                }
                else
                {
                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[0];
                    custPref.Preference = preferences;
                }
                CustomerDetails objCustomerDetails = new CustomerDetails()
                {
                    Title = customerdetails.TitleEnglish,
                    Firstname = customerdetails.Name1.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    Surname = customerdetails.Name3.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    CardNumber = customerdetails.ClubcardID.TryParse<string>(),
                    EmailId = string.IsNullOrEmpty(customerdetails.EmailAddress) ? string.Empty : customerdetails.EmailAddress
                };
                retVal = _preferenceServiceClient.SendEmailToCustomers(custPref, objCustomerDetails);
                logData.RecordStep(string.Format("Status for SendEmailToCustomer Call: {0}", retVal));
                _logger.Submit(logData);
                return retVal;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Status for SendEmailToCustomer Call", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private bool SendEmailNoticeToCustomers(long CustomerID, CommonEntities.CustomerPreference objcustPref, AccountDetails customerdetails, string PageName, Hashtable trackHT)
        {
            bool retVal = false;
            string trackxml = GeneralUtility.HashTableToXML(trackHT, "TrackFields");
            LogData logData = new LogData();

            try
            {
                if (!string.IsNullOrEmpty(CustomerID.ToString()))
                {
                    logData.CustomerID = CustomerID.ToString();
                }
                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                logData.CaptureData("Customer preferences XML ", xml);
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();
                if (objcustPref.Preference != null)
                {
                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];
                    for (int i = 0; i < objcustPref.Preference.Length; i++)
                    {
                        var preference = new PreferenceServices.CustomerPreference
                        {
                            PreferenceID = objcustPref.Preference[i].PreferenceID,
                            POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                            UpdateDateTime = objcustPref.Preference[i].UpdateDateTime,
                            EmailSubject = objcustPref.Preference[i].EmailSubject
                        };
                        preferences[i] = preference;
                    }
                    custPref.Preference = preferences;
                }
                else
                {
                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[0];
                    custPref.Preference = preferences;
                }
                CustomerDetails objCustomerDetails = new CustomerDetails()
                {
                    Title = customerdetails.TitleEnglish,
                    Firstname = customerdetails.Name1.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    Surname = customerdetails.Name3.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    CardNumber = customerdetails.ClubcardID.TryParse<string>(),
                    EmailId = string.IsNullOrEmpty(customerdetails.EmailAddress) ? string.Empty : customerdetails.EmailAddress
                };
                retVal = _preferenceServiceClient.SendEmailNoticeToCustomers(CustomerID, custPref, objCustomerDetails, PageName, trackxml);
                logData.RecordStep(string.Format("Status for SendEmailNoticeToCustomers Call: {0}", retVal));
                _logger.Submit(logData);
                return retVal;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for SendEmailNoticeToCustomers call ", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        /// <summary>
        /// Update Customer Preferences : Update opt in and opt outs for Preferences.
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="objcustPref"></param>
        /// <param name="customerdetails"></param>
        private void UpdateCustomerPreferences(long CustomerID, CommonEntities.CustomerPreference objcustPref, AccountDetails customerdetails)
        {
            LogData logData = new LogData();

            try
            {
                if (!string.IsNullOrEmpty(CustomerID.ToString()))
                {
                    logData.CustomerID = CustomerID.ToString();
                }
                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                logData.CaptureData("Customer preferences XML ", xml);
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();
                PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];
                for (int i = 0; i < objcustPref.Preference.Length; i++)
                {
                    var preference = new PreferenceServices.CustomerPreference
                    {
                        PreferenceID = objcustPref.Preference[i].PreferenceID,
                        POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                        UpdateDateTime = objcustPref.Preference[i].UpdateDateTime,
                        EmailSubject = objcustPref.Preference[i].EmailSubject
                    };
                    preferences[i] = preference;
                }
                custPref.Preference = preferences;
                CustomerDetails objCustomerDetails = new CustomerDetails()
                {
                    Title = customerdetails.TitleEnglish,
                    Firstname = customerdetails.Name1.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    Surname = customerdetails.Name3.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture),
                    CardNumber = customerdetails.ClubcardID.TryParse<string>(),
                    EmailId = string.IsNullOrEmpty(customerdetails.EmailAddress) ? string.Empty : customerdetails.EmailAddress
                };
                _preferenceServiceClient.MaintainCustomerPreference(CustomerID, custPref, objCustomerDetails);
                logData.RecordStep("Customer Preferences Updated");
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while updating Customer Preferences", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }


        private PreferencesEntities.ClubDetails GetClubDetails(long customerID)
        {

            ClubDetails objClubs = null;
            PreferencesEntities.ClubDetails clubs = new PreferencesEntities.ClubDetails();
            LogData logData = new LogData();

            try
            {
                if (!string.IsNullOrEmpty(customerID.ToString()))
                {
                    logData.CustomerID = customerID.ToString();
                }
                objClubs = new ClubDetails();
                objClubs = _preferenceServiceClient.ViewClubDetails(customerID);
                string xml = objClubs.ToXmlString();
                clubs = xml.ToObject<PreferencesEntities.ClubDetails>();
                logData.CaptureData("Club Details", clubs);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Club Details", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return clubs;
        }

        private void UpdateClubDetails(long customerId, PreferencesEntities.ClubDetails clubDetails, string emailIdTo)
        {
            LogData logData = new LogData();
            try
            {
                if (!string.IsNullOrEmpty(customerId.ToString()))
                {
                    logData.CustomerID = customerId.ToString();
                }
                if (clubDetails == null)
                {
                    return;
                }
                ClubDetails clubs = new ClubDetails();
                string clubXml = clubDetails.ToXmlString();
                logData.CaptureData("Club XML", clubXml);
                clubs = clubXml.ToObject<ClubDetails>();
                _preferenceServiceClient.MaintainClubDetails(customerId, clubs, emailIdTo);
                logData.RecordStep("Club Details Updated");
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while updating Club Details", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        #endregion Private Members
    }
}
