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
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;
using Tesco.ClubcardProducts.MCA.Web.Common;


namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    public class PreferenceServiceAdapter : IServiceAdapter
    {
        Recorder _recorder = null;

        #region Constructors

        IPreferenceService _preferenceServiceClient = new PreferenceServiceClient();
        public PreferenceServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
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
        public MCAResponse Get(MCARequest req)
        {
            MCAResponse res = new MCAResponse();
            var operation = req.Parameters[ParameterNames.OPERATION_NAME].ToString();

            switch (operation)
            {
                case OperationNames.GET_CUSTOMER_PREFERENCES:
                    if (req.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID))
                    {
                        res.Data = GetCustomerPreferences(req.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>());
                        res.Status = true;
                    }
                    break;
                case OperationNames.GET_CLUB_DETAILS:
                    if (req.Parameters.ContainsKey(ParameterNames.CUSTOMER_ID))
                    {

                        res.Data = GetClubDetails((req.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>());
                        res.Status = true;
                    }
                    break;
            }

            return res;
        }

        #endregion IServiceAdapter Members

        #region Private Members

        private CommonEntities.CustomerPreference GetCustomerPreferences(long customerID)
        {
            CommonEntities.CustomerPreference preferences = new CommonEntities.CustomerPreference();

            PreferenceServices.PreferenceType prefType = PreferenceServices.PreferenceType.NULL;

            var customerPreferences = _preferenceServiceClient.ViewCustomerPreference(customerID, prefType, true);

            this._recorder.RecordResponse(new RecordLog { Result = customerPreferences.ToXmlString() },
                      Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.PreferenceService.ToString(),
                      "ViewCustomerPreference", ResponseType.Xml);

            string xml = customerPreferences.ToXmlString();
            preferences.ConvertFromXml(xml);
            return preferences;
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
                retVal = _preferenceServiceClient.SendEmailToCustomers(custPref, objCustomerDetails);
                logData.RecordStep(string.Format("Status for SendEmailToCustomer Call: {0}", retVal));
                
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
                retVal = _preferenceServiceClient.SendEmailNoticeToCustomers(CustomerID, custPref, objCustomerDetails, PageName, trackxml);
                logData.RecordStep(string.Format("Status for SendEmailNoticeToCustomers Call: {0}", retVal));
                
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
            ClubDetails objClubs = _preferenceServiceClient.ViewClubDetails(customerID);

            this._recorder.RecordResponse(new RecordLog { Result = objClubs.JsonText() },
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.PreferenceService.ToString(),
                "GetRewardDetail", ResponseType.js);

            return objClubs.ToXmlString().ToObject<PreferencesEntities.ClubDetails>();
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

        #region IServiceAdapter Members


        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion
    }
}
