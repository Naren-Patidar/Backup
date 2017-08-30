using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using Tesco.ClubcardProducts.MCA.API.ServiceAdapter;
using Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using CommonEntities = Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using PreferencesEntities = Tesco.ClubcardProducts.MCA.API.Common.Entities.Preferences;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    public class PreferenceServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        IPreferenceService _preferenceServiceClient = null;
        DateTime _dtStart = DateTime.UtcNow; 

        #region Constructors

        public PreferenceServiceAdapter()
        {
            
        }

        public PreferenceServiceAdapter(string dotcomid, string uuid, string culture) : base (dotcomid, uuid, culture)
        {
            this._preferenceServiceClient = new PreferenceServiceClient();
        }

        #endregion Constructors

        #region IServiceAdapter Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                {
                    "GetCustomerPreferences", new CommonEntities.CustomerPreference(){
                        Preference = new List<CommonEntities.CustomerPreference>(){
                        }.ToArray()
                    }
                },
                {
                    "GetClubDetails", new PreferencesEntities.ClubDetails(){
                    }
                },
                { "SendEmailToCustomers", null },
                { "SendEmailNoticeToCustomers", null },
                { "UpdateCustomerPreferences", null },
                { "UpdateClubDetails", null }
            };
        }

        public string GetName()
        {
            return "preferenceservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                switch (request.operation.ToLower())
                {
                    case "getcustomerpreferences":
                        response.data = this.GetCustomerPreferences(
                                                request.GetParameter<string>("preferencetypetext"),
                                                request.GetParameter<bool>("optionalpreference"));
                        break;

                    case "getclubdetails":
                        response.data = this.GetClubDetails();
                        break;

                    case "sendemailtocustomers":
                        response.data = this.SendEmailToCustomers(
                                                request.GetParameter<string>("objcustpreftext"),
                                                request.GetParameter<string>("customerdetailstext"));
                        break;
                    case "sendemailnoticetocustomers":
                        response.data = this.SendEmailNoticeToCustomers(                                                
                                                request.GetParameter<string>("objcustpreftext"),
                                                request.GetParameter<string>("customerdetailstext"),
                                                request.GetParameter<string>("pagename"),
                                                request.GetParameter<string>("trackhttext"));
                        break;
                    case "updatecustomerpreferences":
                        response.data = this.UpdateCustomerPreferences(
                                                request.GetParameter<string>("customerid"),
                                                request.GetParameter<string>("objcustpreftext"),
                                                request.GetParameter<string>("customerdetailstext"));
                        break;
                    case "updateclubdetails":
                        response.data = this.UpdateClubDetails(
                                               request.GetParameter<string>("customerid"),
                                               request.GetParameter<string>("clubdetailstext"),
                                               request.GetParameter<string>("emailidto"));
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-PREFERENCE-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion IServiceAdapter Members

        #region Private Members

        private CommonEntities.CustomerPreference GetCustomerPreferences(string preferenceTypeText, bool optionalPreference)
        {
            CommonEntities.CustomerPreference preferences = new CommonEntities.CustomerPreference();
            
            try
            {
                CommonEntities.PreferenceType preferenceType = JsonConvert.DeserializeObject<CommonEntities.PreferenceType>(preferenceTypeText);
                string strpreferenceType = SerializerUtility<CommonEntities.PreferenceType>.GetSerializedString(preferenceType);
                PreferenceServices.PreferenceType prefType = PreferenceServices.PreferenceType.NULL;

                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                PreferenceServices.CustomerPreference customerPreferences = new PreferenceServices.CustomerPreference();
                this._dtStart = DateTime.UtcNow;
                try
                {
                    customerPreferences = this._preferenceServiceClient.ViewCustomerPreference(lCustomerID, prefType, true);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }
                
                string xml = customerPreferences.ToXmlString();
                preferences.ConvertFromXml(xml);
                return preferences;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex, null);
            }
        }

        private bool SendEmailToCustomers(string objcustPrefText, string customerdetailsText)
        {
            try
            {
                CommonEntities.CustomerPreference objcustPref = JsonConvert.DeserializeObject<CommonEntities.CustomerPreference>(objcustPrefText);
                AccountDetails customerdetails = JsonConvert.DeserializeObject<AccountDetails>(customerdetailsText);

                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();

                if (objcustPref.Preference != null)
                {
                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];

                    for (int i = 0; i < objcustPref.Preference.Length; i++)
                    {
                        DateTime dtTemp;
                        var preference = new PreferenceServices.CustomerPreference
                        {
                            PreferenceID = objcustPref.Preference[i].PreferenceID,
                            POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                            UpdateDateTime = objcustPref.Preference[i].UpdateDateTime.TryParseDate(out dtTemp) ? dtTemp : DateTime.UtcNow,
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

                this._dtStart = DateTime.UtcNow;
                return this._preferenceServiceClient.SendEmailToCustomers(custPref, objCustomerDetails);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Status for SendEmailToCustomer Call", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        private bool SendEmailNoticeToCustomers(string objcustPrefText, string customerdetailsText, string pageName, string trackHTText)
        {
            bool retVal = false;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter CustomerID is mandatory and must be passed for further processing.");
                }

                CommonEntities.CustomerPreference objcustPref = JsonConvert.DeserializeObject<CommonEntities.CustomerPreference>(objcustPrefText);

                objcustPref.CustomerID = lCustomerID.ToString();

                AccountDetails customerdetails = JsonConvert.DeserializeObject<AccountDetails>(customerdetailsText);

                customerdetails.CustomerID = custInfo.ngccustomerid;

                Hashtable trackHT = JsonConvert.DeserializeObject<Hashtable>(trackHTText);
                string trackxml = GeneralUtility.HashTableToXML(trackHT, "TrackFields");

                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();
                if (objcustPref.Preference != null)
                {
                    DateTime dtTemp;

                    PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];
                    for (int i = 0; i < objcustPref.Preference.Length; i++)
                    {
                        var preference = new PreferenceServices.CustomerPreference
                        {
                            PreferenceID = objcustPref.Preference[i].PreferenceID,
                            POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                            UpdateDateTime = objcustPref.Preference[i].UpdateDateTime.TryParseDate(out dtTemp) ? dtTemp : DateTime.UtcNow,
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

                this._dtStart = DateTime.UtcNow;
                return this._preferenceServiceClient.SendEmailNoticeToCustomers(lCustomerID, custPref, objCustomerDetails, pageName, trackxml);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting status for SendEmailNoticeToCustomers call ", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Update Customer Preferences : Update opt in and opt outs for Preferences.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="objcustPref"></param>
        /// <param name="customerdetails"></param>
        private bool UpdateCustomerPreferences(string customerId, string objcustPrefText, string customerdetailsText)
        {
            try
            {
                long lCustomerID = customerId.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter CustomerID is mandatory and must be passed for further processing.");
                }

                CommonEntities.CustomerPreference objcustPref = JsonConvert.DeserializeObject<CommonEntities.CustomerPreference>(objcustPrefText);
                AccountDetails customerdetails = JsonConvert.DeserializeObject<AccountDetails>(customerdetailsText);

                PreferenceServices.CustomerPreference custPref = new PreferenceServices.CustomerPreference();
                string xml = objcustPref.ToXmlString();
                custPref = xml.ToObject<PreferenceServices.CustomerPreference>();
                PreferenceServices.CustomerPreference[] preferences = new PreferenceServices.CustomerPreference[objcustPref.Preference.Length];

                DateTime dtTemp;
                for (int i = 0; i < objcustPref.Preference.Length; i++)
                {
                    var preference = new PreferenceServices.CustomerPreference
                    {
                        PreferenceID = objcustPref.Preference[i].PreferenceID,
                        POptStatus = (PreferenceServices.OptStatus)objcustPref.Preference[i].POptStatus,
                        UpdateDateTime = objcustPref.Preference[i].UpdateDateTime.TryParseDate(out dtTemp) ? dtTemp : DateTime.UtcNow,
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

                this._dtStart = DateTime.UtcNow;
                _preferenceServiceClient.MaintainCustomerPreference(lCustomerID, custPref, objCustomerDetails);
                return true;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while updating Customer Preferences", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        private PreferencesEntities.ClubDetails GetClubDetails()
        {
            ClubDetails objClubs = new ClubDetails();
            PreferencesEntities.ClubDetails clubs = new PreferencesEntities.ClubDetails();
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter CustomerID is mandatory and must be passed for further processing.");
                }

                this._dtStart = DateTime.UtcNow;
                try
                {
                    objClubs = _preferenceServiceClient.ViewClubDetails(lCustomerID);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }
                
                string xml = objClubs.ToXmlString();
                clubs = xml.ToObject<PreferencesEntities.ClubDetails>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting Club Details", ex, null);
            }
            return clubs;
        }

        private bool UpdateClubDetails(string customerId, string clubDetailsText, string emailIdTo)
        {
            try
            {
                long lCustomerID = customerId.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter CustomerID is mandatory and must be passed for further processing.");
                }

                PreferencesEntities.ClubDetails clubDetails = JsonConvert.DeserializeObject<PreferencesEntities.ClubDetails>(clubDetailsText);
                if (clubDetails == null)
                {
                    return true;
                }
                ClubDetails clubs = new ClubDetails();
                string clubXml = clubDetails.ToXmlString();
                clubs = clubXml.ToObject<ClubDetails>();

                this._dtStart = DateTime.UtcNow;
                _preferenceServiceClient.MaintainClubDetails(lCustomerID, clubs, emailIdTo);
                
                return true;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while updating Club Details", ex, null);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }
        }

        #endregion Private Members
    }
}