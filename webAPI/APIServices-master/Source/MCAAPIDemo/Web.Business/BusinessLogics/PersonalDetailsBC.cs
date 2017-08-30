using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using WebAPI.Contracts;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class PersonalDetailsBC
    {
        private string APIURL
        {
            get
            {
                return ConfigurationManager.AppSettings["APIURL"];
            }
        }

        APIRequester _APIRequester = null;


        MCARequest request;
        MCAResponse response;

        //private readonly ILoggingService _logger;
        string maxRows = String.Empty;
        //IConfigurationProvider _configProvider = null;

        String StrMonth = String.Empty;
        String StrYear = String.Empty;
        String StrDay = String.Empty;
        
        LoggingService _logger;
        ConfigurationProvider _configProvider;

        #region Constructor

        public PersonalDetailsBC()
        {
            _logger = new LoggingService();
            _configProvider = new ConfigurationProvider();
            this.maxRows = _configProvider.GetStringAppSetting(AppConfigEnum.MaxRows);
            _APIRequester = new APIRequester(this.APIURL);
        }

        #endregion Constructor

        #region Public Methods

        public CustomerFamilyMasterData GetCustomerMasterData(long customerId, int maxRows)
        {
            MCAResponse response = new MCAResponse();
            LogData logData = new LogData();
            try
            {
                string data = "{\"service\":\"CustomerService\",\"operation\":\"GetCustomerFamilyMasterDataByCustomerId\"," +
                               "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"" + customerId + "\"}," +
                                               "{\"Key\":\"maxRows\",\"Value\":\"" + maxRows + "\"}," +
                                               "{\"Key\":\"culture\",\"Value\":\"" + System.Globalization.CultureInfo.CurrentCulture.Name + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    CustomerFamilyMasterData cData = JsonConvert.DeserializeObject<CustomerFamilyMasterData>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    _logger.Submit(logData);
                    return cData;                    
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
                throw GeneralUtility.GetCustomException("Exception in getting customer master data", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return (CustomerFamilyMasterData)response.Data;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        public PersonalDetailsViewModel GetCustomerDataView(long customerId)
        {
            MCAResponse response = new MCAResponse();
            PersonalDetailsViewModel personalDetails = null;
            LogData logData = new LogData();
            string emailPref = string.Empty;
            string postPref = string.Empty;
            string mobilePref = string.Empty;
            try
            {
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);

                CustomerFamilyMasterData customerFamilyMasterData = this.GetCustomerDetails(customerId);
                CustomerPreference custPreference = new CustomerPreference();
                logData.RecordStep(string.Format("Getting customer data with customer data count is {0} and family count is {1}"
                    , customerFamilyMasterData.CustomerData.Count, customerFamilyMasterData.FamilyData.Count));
                personalDetails = new PersonalDetailsViewModel(customerFamilyMasterData);
                personalDetails.PersonalDetailsPreferences = this.LoadCustomerPreference(custPreference, out emailPref, out mobilePref, out postPref);
                personalDetails.HiddenEmailPref = emailPref;
                personalDetails.HiddenMobilePref = mobilePref;
                personalDetails.HiddenPostPref = postPref;
                List<string> strEmailDietaryPref = new List<string>();
                List<string> strEmailAllergyPref = new List<string>();
                List<string> strDietaryPrefList = new List<string>();
                List<string> strAllergyPrefList = new List<string>();

                for (int i = 0; i < personalDetails.PersonalDetailsPreferences.Count; i++)
                {


                    //Thank customer for opting less email statements
                    if (personalDetails.PersonalDetailsPreferences[i].OptedStatus && personalDetails.PersonalDetailsPreferences[i].PreferenceType == 1)
                    {
                        strEmailAllergyPref.Add(personalDetails.PersonalDetailsPreferences[i].PreferenceID.ToString());
                    }
                    if (personalDetails.PersonalDetailsPreferences[i].OptedStatus && personalDetails.PersonalDetailsPreferences[i].PreferenceType == 2)
                    {
                        strEmailDietaryPref.Add(personalDetails.PersonalDetailsPreferences[i].PreferenceID.ToString());
                    }
                    if (personalDetails.PersonalDetailsPreferences[i].PreferenceType == 1)
                    {
                        strAllergyPrefList.Add(personalDetails.PersonalDetailsPreferences[i].PreferenceID.ToString());
                    }
                    if (personalDetails.PersonalDetailsPreferences[i].PreferenceType == 2)
                    {
                        strDietaryPrefList.Add(personalDetails.PersonalDetailsPreferences[i].PreferenceID.ToString());
                    }
                }
                personalDetails.HiddenAllergyPreferences = string.Join(",", strAllergyPrefList.ToArray());
                personalDetails.HiddenDietaryPreferences = string.Join(",", strDietaryPrefList.ToArray());
                personalDetails.HiddenDietary = string.Join(",", strEmailDietaryPref.ToArray());
                personalDetails.HiddenAllergy = string.Join(",", strEmailAllergyPref.ToArray());

                personalDetails.HiddenSendEmailForDietary = _configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Send_Preference_Email, Convert.ToInt32(PreferenceEnum.Kosher).ToString());
                personalDetails.HiddenSendEmailForAllergy = _configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Send_Preference_Email, Convert.ToInt32(PreferenceEnum.Diabetic1).ToString());

                DateTime customerDateOfBirth;
                string DOB = personalDetails.CustomerFamilyMasterData.CustomerData[0].FamilyMemberDOB1;
                if (DOB.TryParseDate(out customerDateOfBirth))
                {
                    personalDetails.DayOfBirth = customerDateOfBirth.Day.ToString();
                    personalDetails.MonthOfBirth = customerDateOfBirth.Month.ToString();
                    personalDetails.YearOfBirth = customerDateOfBirth.Year.ToString();
                }
                List<string> FamilyDOBs = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    if (i < personalDetails.CustomerFamilyMasterData.FamilyData.Count)
                        FamilyDOBs.Add(personalDetails.CustomerFamilyMasterData.FamilyData[i].DateOfBirth.Value.Year.ToString());
                    else
                        FamilyDOBs.Add("Year");
                }
                personalDetails.FamilyMemberDob = FamilyDOBs;

                personalDetails.HiddenFirstName = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].FirstName) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].FirstName.Trim().ToTitleCase(System.Globalization.CultureInfo.CurrentCulture) : String.Empty;
                personalDetails.HiddenSurname = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].LastName) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].LastName.Trim().ToTitleCase(System.Globalization.CultureInfo.CurrentCulture) : String.Empty;
                personalDetails.HiddenMidName = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].Initial) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].Initial.Trim().ToTitleCase(System.Globalization.CultureInfo.CurrentCulture) : String.Empty;
                personalDetails.HiddenDOB = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].FamilyMemberDOB1) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].FamilyMemberDOB1 : String.Empty;
                personalDetails.HiddenSex = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].Sex) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].Sex.Trim().ToTitleCase(System.Globalization.CultureInfo.CurrentCulture) : String.Empty;
                personalDetails.HiddenAddressLine1 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine1) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine1 : String.Empty;
                personalDetails.HiddenAddressLine2 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine2) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine2 : String.Empty;
                personalDetails.HiddenAddressLine3 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine3) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine3 : String.Empty;
                personalDetails.HiddenAddressLine4 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine4) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine4 : String.Empty;
                personalDetails.HiddenAddressLine5 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine5) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine5 : String.Empty;
                personalDetails.HiddenAddressLine6 = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine6) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressLine6 : String.Empty;
                personalDetails.HiddenPostcode = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MailingAddressPostCode : String.Empty;
                personalDetails.HiddenEmail = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].EmailAddress) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].EmailAddress : String.Empty;
                personalDetails.HiddenSSN = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].SSN) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].SSN : String.Empty;
                personalDetails.HiddenPassportNo = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].PassportNo) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].PassportNo : String.Empty;
                personalDetails.HiddenISOLanguageCode = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].ISOLanguageCode) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].ISOLanguageCode : String.Empty;
                personalDetails.HiddenRaceID = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].RaceID) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].RaceID : String.Empty;
                personalDetails.HiddenDayPhoneNumber = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].DayTimePhonenumber) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].DayTimePhonenumber : String.Empty;
                personalDetails.HiddenEveningPhoneNumber = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].EveningPhonenumber) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].EveningPhonenumber : String.Empty;
                personalDetails.HiddenMobileNumber = !string.IsNullOrEmpty(personalDetails.CustomerFamilyMasterData.CustomerData[0].MobileNumber) ? personalDetails.CustomerFamilyMasterData.CustomerData[0].MobileNumber : String.Empty;

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting customer data view", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return personalDetails;
        }

        public bool IsProfaneText(PersonalDetailsViewModel viewModel, long customerID, Dictionary<string, string> resourceKeys)
        {
            return false;
        }

        /// <summary>
        /// For Updating cutomer details
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool SetCustomerDataView(PersonalDetailsViewModel viewModel, long customerID, Dictionary<string, string> resourceKeys)
        {
            bool status;
            bool bTrackFields = false;
            bool bUpdateCompleted = false;
            LogData logData = new LogData();
            Hashtable hashCustomerDetails = new Hashtable();
            try
            {
                hashCustomerDetails = GetHashTableFromViewModel(viewModel, customerID, resourceKeys);
                status = UpdateCustomerDetails(hashCustomerDetails, resourceKeys, customerID);
                if (status)
                {
                    logData.RecordStep("Updated customer details");
                    CustomerPreference[] preferencelist = null;
                    if (viewModel.PersonalDetailsPreferences != null && viewModel.PersonalDetailsPreferences.Count > 0)
                    {
                        preferencelist = new CustomerPreference[viewModel.PersonalDetailsPreferences.Count];

                        for (int i = 0; i < viewModel.PersonalDetailsPreferences.Count; i++)
                        {
                            if (viewModel.PersonalDetailsPreferences[i].OptedStatus)
                            {
                                CustomerPreference custpref = new CustomerPreference();
                                custpref.PreferenceID = Convert.ToInt16(viewModel.PersonalDetailsPreferences[i].PreferenceID);
                                custpref.POptStatus = OptStatus.OPTED_IN;
                                custpref.UpdateDateTime = DateTime.Now;

                                #region TBUpdates- with group country updates
                                bool dietaryEmailSubjectAdded = false;
                                bool allergyEmailSubjectAdded = false;
                                //Changes for Thank customer for opting less email statements
                                if (!String.IsNullOrEmpty(viewModel.HiddenDietaryPreferences))
                                {
                                    if ((!Array.Exists(viewModel.HiddenDietaryPreferences.Trim().Split(',').ToArray(), c => c == viewModel.PersonalDetailsPreferences[i].PreferenceID.ToString()) && !string.IsNullOrEmpty(viewModel.HiddenSendEmailForDietary)) && !dietaryEmailSubjectAdded)
                                    {
                                        custpref.EmailSubject = viewModel.HiddenSendEmailForDietary;
                                        dietaryEmailSubjectAdded = true;
                                    }
                                    else
                                    {
                                        custpref.EmailSubject = string.Empty;
                                    }
                                }
                                if (!String.IsNullOrEmpty(viewModel.HiddenAllergyPreferences))
                                {
                                    if ((!Array.Exists(viewModel.HiddenAllergyPreferences.Trim().Split(',').ToArray(), c => c == viewModel.PersonalDetailsPreferences[i].PreferenceID.ToString()) && !string.IsNullOrEmpty(viewModel.HiddenSendEmailForAllergy)) && !allergyEmailSubjectAdded)
                                    {
                                        custpref.EmailSubject = viewModel.HiddenSendEmailForAllergy;
                                        allergyEmailSubjectAdded = true;
                                    }
                                    else
                                    {
                                        custpref.EmailSubject = string.Empty;
                                    }
                                }
                                //R1.6 Changes for Thank customer for opting less email statements
                                #endregion TBUpdates- with group country updates
                                preferencelist[i] = custpref;
                            }
                            else
                            {
                                CustomerPreference custpref = new CustomerPreference
                                {
                                    PreferenceID = Convert.ToInt16(viewModel.PersonalDetailsPreferences[i].PreferenceID),
                                    POptStatus = OptStatus.OPTED_OUT,
                                    UpdateDateTime = DateTime.Now,
                                    EmailSubject = string.Empty
                                };
                                preferencelist[i] = custpref;
                            }
                        }
                    }
                    CustomerPreference objcustPref = new CustomerPreference();
                    objcustPref.CustomerID = customerID;
                    objcustPref.Preference = preferencelist;
                    objcustPref.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                    objcustPref.UserID = _configProvider.GetStringAppSetting(AppConfigEnum.ServiceConsumer);

                    AccountDetails objCustomerDetails = new AccountDetails();
                    objCustomerDetails.EmailAddress = hashCustomerDetails["EmailAddress"].ToString();
                    objCustomerDetails.Name3 = hashCustomerDetails["Name3"].ToString();
                    objCustomerDetails.TitleEnglish = hashCustomerDetails["TitleEnglish"].ToString();
                    objCustomerDetails.Name1 = hashCustomerDetails["Name1"].ToString();
                    objCustomerDetails.CustomerID = customerID;
                    objCustomerDetails.ClubcardID = GetClubcardNumber(customerID);


                    if (viewModel.HiddenSentEmail != null && !viewModel.HiddenSentEmail.TryParse<bool>())
                    {
                        bTrackFields = UpdateAndMailCustomerPreferences(viewModel, objcustPref, objCustomerDetails, resourceKeys, customerID);
                        bUpdateCompleted = true;
                        logData.RecordStep("Sending email for changing email address");
                    }
                    if (bTrackFields == false && preferencelist != null)
                    {
                        //status = UpdateCustomerPreferences(objcustPref.CustomerID, objcustPref, objCustomerDetails);
                        bUpdateCompleted = true;
                        logData.RecordStep("Updated customer prefernce only");
                    }

                    #region TBUpdated- with group country updates
                    //preferenceserviceClient = new PreferenceServiceClient();
                    //customerID = GetCustomerId();
                    //presenter.UpdateClubDetails(customerID, objClubDetails, string.Empty);// (customerID, objClubDetails, "");
                    //if (!Convert.ToBoolean(hdnBTExists.Value))
                    //{
                    //    divBt.Attributes.Add("style", "display:none");
                    //}
                    //LoadBabyTodlerDetails();
                    //ResetHiddenValues();
                    //plViewAddress.Visible = true;
                    //plEditAddress.Visible = false;

                    //lblContactAddress.Text = string.Empty;//Clear the previous address

                    ////Load address and other fields
                    //for (int i = 1; i <= 7; i++)
                    //{
                    //    if (customerData["MailingAddressLine" + i] != null)
                    //    {
                    //        string Address = customerData["MailingAddressLine" + i].ToString();
                    //        lblContactAddress.Text += string.IsNullOrEmpty(Address) ? string.Empty : Address + "<br/>";//customerData["MailingAddressLine" + i].ToString() + "<br/>";
                    //    }
                    //}

                    //lblSuccessMessage.Text = GetLocalResourceObject("save.success").ToString(); //"Your changes have been saved";
                    //lblSuccessMessage.Visible = true;
                    //lblContactAddress.Text= hdnTrackAddress1.Value.ToString ();
                    #endregion TBUpdated- with group country updates
                }
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in setting customer data  view", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return bUpdateCompleted;
        }

        

        private AddressDetails GetAddressBuildingNo(List<AddressByPostCode> addressByPostCodeList)
        {
            ArrayList buildingNoStreetList = new ArrayList();
            ArrayList buildingNoStreetListWithoutStreet = new ArrayList();
            AddressDetails addressDetails = new AddressDetails();
            for (int i = 0; i < addressByPostCodeList.Count; i++)
            {
                if (addressByPostCodeList[i].Street.ToString().Trim() != string.Empty)
                {
                    if (addressByPostCodeList[i].SubBuilding.ToString().Trim() != string.Empty)
                    {
                        if (addressByPostCodeList[i].BuildingNumber.ToString().Trim() != string.Empty)
                        {
                            buildingNoStreetList.Add(string.Format("{0} {1} {2} {3}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                            buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2} {3}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        }
                        else
                        {
                            buildingNoStreetList.Add(string.Format("{0} {1} {2}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                            buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        }
                    }
                    else if (addressByPostCodeList[i].BuildingName.ToString().Trim() != string.Empty)
                    {
                        if (addressByPostCodeList[i].BuildingNumber.ToString().Trim() != string.Empty)
                        {
                            buildingNoStreetList.Add(string.Format("{0} {1} {2}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                            buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        }
                        else
                        {
                            buildingNoStreetList.Add(string.Format("{0} {1}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                            buildingNoStreetListWithoutStreet.Add(string.Format("{0},{1}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        }
                    }
                    else
                    {
                        buildingNoStreetList.Add(string.Format("{0} {1}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0},{1}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                }
            }
            if (buildingNoStreetList.Count > 0)
            {
                buildingNoStreetList.Sort();
                Dictionary<int, string> addresses = new Dictionary<int, string>();
                for (int i = 0; i < buildingNoStreetList.Count; i++)
                {
                    addresses.Add(i, buildingNoStreetList[i].ToString());
                }
                addressDetails.AddressList = addresses;
            }
            if (buildingNoStreetListWithoutStreet.Count > 0)
            {
                buildingNoStreetListWithoutStreet.Sort();
                addressDetails.HideAddressList = buildingNoStreetListWithoutStreet;
            }
            return addressDetails;
        }

        private AddressDetails GetAddressBuildingName(List<AddressByPostCode> addressByPostCodeList)
        {
            ArrayList buildingNameStreetList = new ArrayList();
            ArrayList buildingNoStreetListWithoutStreet = new ArrayList();
            AddressDetails addressDetails = new AddressDetails();
            for (int i = 0; i < addressByPostCodeList.Count; i++)
            {
                if (addressByPostCodeList[i].SubBuilding.ToString().Trim() != string.Empty)
                {
                    if (addressByPostCodeList[i].BuildingNumber.ToString().Trim() != string.Empty)
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1} {2} {3}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2} {3}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                    else
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1} {2}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2}", addressByPostCodeList[i].SubBuilding.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                }
                else if (addressByPostCodeList[i].BuildingNumber.ToString().Trim() != string.Empty)
                {
                    if (addressByPostCodeList[i].BuildingName.ToString().Trim() != string.Empty)
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1} {2}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0} {1},{2}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                    else
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0},{1}", addressByPostCodeList[i].BuildingNumber.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                }
                else
                {
                    if (addressByPostCodeList[i].BuildingName.ToString().Trim() != string.Empty)
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0},{1}", addressByPostCodeList[i].BuildingName.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                    else if (addressByPostCodeList[i].Organisation.ToString().Trim() != string.Empty)
                    {
                        buildingNameStreetList.Add(string.Format("{0} {1}", addressByPostCodeList[i].Organisation.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                        buildingNoStreetListWithoutStreet.Add(string.Format("{0},{1}", addressByPostCodeList[i].Organisation.ToString().Trim(), addressByPostCodeList[i].Street.ToString().Trim()));
                    }
                }
            }
            if (buildingNameStreetList.Count > 0)
            {
                buildingNameStreetList.Sort();
                Dictionary<int, string> addresses = new Dictionary<int, string>();
                for (int i = 0; i < buildingNameStreetList.Count; i++)
                {
                    addresses.Add(i, buildingNameStreetList[i].ToString());
                }
                addressDetails.AddressList = addresses;
            }
            if (buildingNoStreetListWithoutStreet.Count > 0)
            {
                buildingNoStreetListWithoutStreet.Sort();
                addressDetails.HideAddressList = buildingNoStreetListWithoutStreet;
            }
            return addressDetails;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Update Customer Details and Send Emai
        /// </summary>
        /// <param name="pdViewModel"></param>
        /// <param name="customerPreferences"></param>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        private bool UpdateAndMailCustomerPreferences(PersonalDetailsViewModel pdViewModel, CustomerPreference customerPreferences, AccountDetails customerDetails, Dictionary<string, string> resourceKeys, long customerId)
        {
            LogData logData = new LogData();
            String ChangedFields = System.String.Empty;
            bool bTrackChange = false;
            bool tracksuccess = false;
            Hashtable htTrackFields = new Hashtable();
            try
            {
                DBConfigurations EmailNotificationConfiguration = _configProvider.GetConfigurations(DbConfigurationTypeEnum.EmailNotification);
                logData.CaptureData("Email configuration", EmailNotificationConfiguration);
                if (EmailNotificationConfiguration != null)
                {
                    CustomerMasterData cmd = new CustomerMasterData();
                    cmd = pdViewModel.CustomerFamilyMasterData.CustomerData[0];

                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackFirstName).ConfigurationValue1 != "0")
                    {
                        pdViewModel.HiddenFirstName = !string.IsNullOrEmpty(pdViewModel.HiddenFirstName) ? pdViewModel.HiddenFirstName : string.Empty;
                        if (!pdViewModel.HiddenFirstName.Equals(cmd.FirstName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bTrackChange = true;
                            htTrackFields["TrackFName"] = resourceKeys["TrackFName"];
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackMiddleName).ConfigurationValue1 != "0")
                    {
                        pdViewModel.HiddenMidName = !string.IsNullOrEmpty(pdViewModel.HiddenMidName) ? pdViewModel.HiddenMidName : string.Empty;
                        if (!pdViewModel.HiddenMidName.Equals(cmd.Initial, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bTrackChange = true;
                            htTrackFields["TrackMName"] = resourceKeys["TrackMName"];
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackSurname).ConfigurationValue1 != "0")
                    {
                        pdViewModel.HiddenSurname = !string.IsNullOrEmpty(pdViewModel.HiddenSurname) ? pdViewModel.HiddenSurname : string.Empty;
                        if (!pdViewModel.HiddenSurname.Equals(cmd.LastName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bTrackChange = true;
                            htTrackFields["TrackSName"] = resourceKeys["TrackSName"];
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackDOB).ConfigurationValue1 != "0")
                    {
                        DateTime customerDateOfBirth;
                        if (!string.IsNullOrEmpty(pdViewModel.HiddenDOB) && pdViewModel.HiddenDOB.TryParseDate(out customerDateOfBirth))
                        {
                            if (customerDateOfBirth.Day.ToString() != pdViewModel.DayOfBirth)
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectDay"];
                            }
                            if (customerDateOfBirth.Month.ToString() != pdViewModel.MonthOfBirth)
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectMonth"];
                            }
                            if (customerDateOfBirth.Year.ToString() != pdViewModel.YearOfBirth)
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectYear"];
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pdViewModel.DayOfBirth))
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectDay"];
                            }
                            if (!string.IsNullOrEmpty(pdViewModel.MonthOfBirth))
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectMonth"];
                            }
                            if (!string.IsNullOrEmpty(pdViewModel.YearOfBirth))
                            {
                                bTrackChange = true;
                                htTrackFields["TrackDOB"] = resourceKeys["SelectYear"];
                            }
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackEmail).ConfigurationValue1 != "0")
                    {
                        pdViewModel.HiddenEmail = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;
                        if (!pdViewModel.HiddenEmail.Equals(cmd.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bTrackChange = true;
                            htTrackFields["Email"] = resourceKeys["TrackEmail"];
                            if (!string.IsNullOrEmpty(pdViewModel.HiddenEmail))
                            {
                                htTrackFields["oldEmailAddress"] = pdViewModel.HiddenEmail;
                                htTrackFields["bEmailChange"] = bTrackChange;
                            }
                            else
                                htTrackFields["oldEmailAddress"] = string.Empty;
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackMobileNumber).ConfigurationValue1 != "0")
                    {
                        if (pdViewModel.HiddenMobileNumber != cmd.MobileNumber)
                        {
                            bTrackChange = true;
                            htTrackFields["TrackMobile"] = resourceKeys["TrackMobile"];
                            htTrackFields["oldEmailAddress"] = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackDayTimePhoneNumber).ConfigurationValue1 != "0")
                    {
                        if (pdViewModel.HiddenDayPhoneNumber != cmd.DayTimePhonenumber)
                        {
                            bTrackChange = true;
                            htTrackFields["TrackDayTimePhone"] = resourceKeys["TrackDayTimePhone"];
                            htTrackFields["oldEmailAddress"] = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackEvengingPhoneNumber).ConfigurationValue1 != "0")
                    {
                        if (pdViewModel.HiddenEveningPhoneNumber != cmd.EveningPhonenumber)
                        {
                            bTrackChange = true;
                            htTrackFields["TrackEveningPhone"] = resourceKeys["TrackEveningPhone"];
                            htTrackFields["oldEmailAddress"] = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;
                        }
                    }
                    if (EmailNotificationConfiguration.GetConfigurationItem(DbConfigurationItemNames.TrackAddress).ConfigurationValue1 != "0")
                    {
                        if (pdViewModel.FindAddressClicked == true)
                        {

                            bTrackChange = true;
                            htTrackFields["TrackAddress"] = resourceKeys["TrackAddress"];
                            htTrackFields["oldEmailAddress"] = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;

                        }
                        //Group mail configuration & Notification 
                        else
                        {
                            pdViewModel.HiddenAddressLine1 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine1) ? pdViewModel.HiddenAddressLine1 : string.Empty;
                            pdViewModel.HiddenAddressLine2 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine2) ? pdViewModel.HiddenAddressLine2 : string.Empty;
                            pdViewModel.HiddenAddressLine3 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine3) ? pdViewModel.HiddenAddressLine3 : string.Empty;
                            pdViewModel.HiddenAddressLine4 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine4) ? pdViewModel.HiddenAddressLine4 : string.Empty;
                            pdViewModel.HiddenAddressLine5 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine5) ? pdViewModel.HiddenAddressLine5 : string.Empty;
                            pdViewModel.HiddenAddressLine6 = !string.IsNullOrEmpty(pdViewModel.HiddenAddressLine6) ? pdViewModel.HiddenAddressLine6 : string.Empty;
                            pdViewModel.HiddenPostcode = !string.IsNullOrEmpty(pdViewModel.HiddenPostcode) ? pdViewModel.HiddenPostcode : string.Empty;

                            if (!pdViewModel.HiddenAddressLine1.Equals(cmd.MailingAddressLine1, StringComparison.CurrentCultureIgnoreCase)
                                || !pdViewModel.HiddenAddressLine2.Equals(cmd.MailingAddressLine2, StringComparison.CurrentCultureIgnoreCase)
                                || !pdViewModel.HiddenAddressLine3.Equals(cmd.MailingAddressLine3, StringComparison.CurrentCultureIgnoreCase)
                                || !pdViewModel.HiddenAddressLine4.Equals(cmd.MailingAddressLine4, StringComparison.CurrentCultureIgnoreCase)
                                || !(pdViewModel.HiddenAddressLine5.Equals(cmd.MailingAddressLine5, StringComparison.CurrentCultureIgnoreCase)
                                && _configProvider.GetStringAppSetting(AppConfigEnum.EnableProvince) == "0")
                                || !pdViewModel.HiddenAddressLine6.Equals(cmd.MailingAddressLine6, StringComparison.CurrentCultureIgnoreCase)
                                || !pdViewModel.HiddenPostcode.Equals(cmd.MailingAddressPostCode, StringComparison.CurrentCultureIgnoreCase))
                            {
                                bTrackChange = true;
                                htTrackFields["TrackAddress"] = resourceKeys["TrackAddress"];
                                htTrackFields["oldEmailAddress"] = !string.IsNullOrEmpty(pdViewModel.HiddenEmail) ? pdViewModel.HiddenEmail : string.Empty;
                            }
                        }
                    }
                    //End Group mail configuration
                }

                //verify any modification in the customer data
                if (bTrackChange == true)
                {
                    if (!string.IsNullOrEmpty(pdViewModel.CustomerFamilyMasterData.CustomerData[0].EmailAddress))
                    {
                        pdViewModel.HiddenEmail = pdViewModel.CustomerFamilyMasterData.CustomerData[0].EmailAddress.Trim();
                    }
                    else
                    {
                        pdViewModel.HiddenEmail = string.Empty;
                    }
                    if (htTrackFields.Contains("oldEmailAddress"))
                    {
                        if (htTrackFields["oldEmailAddress"].ToString() == string.Empty)
                        {
                            request = new MCARequest();
                            request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCES, customerPreferences);
                            request.Parameters.Add(ParameterNames.CUST_DETAILS, customerDetails);
                            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.SEND_EMAIL_TO_CUSTOMERS);
                            //response = _preferenceServiceAdapter.Set<bool>(request);
                            logData.RecordStep("Calling send email to customers");
                        }
                    }

                    request = new MCARequest();
                    request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                    request.Parameters.Add(ParameterNames.CUSTOMER_PREFERENCES, customerPreferences);
                    request.Parameters.Add(ParameterNames.CUST_DETAILS, customerDetails);
                    request.Parameters.Add(ParameterNames.PAGE_NAME, resourceKeys["PageName"]);
                    request.Parameters.Add(ParameterNames.TRACKHT, htTrackFields);
                    request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.SEND_EMAILNOTICE_TO_CUSTOMERS);
                    logData.RecordStep("Calling send email notice to customers");
                    //response = _preferenceServiceAdapter.Set<bool>(request);
                    if (true)
                    {
                        tracksuccess = true;
                        pdViewModel.HiddenSentEmail = "true";
                    }
                }
                logData.RecordStep(string.Format("Track success is {0}", tracksuccess));
                _logger.Submit(logData);

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Update and Mail customer preference", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return tracksuccess;
        }

        /// <summary>
        /// Get ClubcardDetails for Customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private long GetClubcardNumber(long customerId)
        {
            long clubcardNumber = 0;
            LogData logData = new LogData();
            try
            {
                AccountDetails customerAccountDetails = new AccountDetails();
                string data = "{\"service\":\"ClubcardService\",\"operation\":\"GetCustomerAccountDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + System.Globalization.CultureInfo.CurrentCulture.Name + "\"}]}";

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

                    clubcardNumber = customerAccountDetails.ClubcardID;
                    logData.CaptureData("Account details", customerAccountDetails);
                    logData.RecordStep("Profanity is not enabled");
                    _logger.Submit(logData);
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
                throw GeneralUtility.GetCustomException("Exception in Getting clubcard number", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return clubcardNumber;
        }

        
        /// <summary>
        /// Gets an object for Join form data
        /// </summary>
        /// <param name="configurationItems"></param>
        /// <returns></returns>
        private string PrepareProfaneText(Hashtable hashCustomerData, long customerId)
        {
            LogData logData = new LogData();
            ProfaneTextProvider profaneTextProvider = new EmptyProfaneTextContributor();
            List<String> strBlackList = new List<String>();

            try
            {
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name1.ToString()) == "1")
                {
                    if (hashCustomerData["Name1"] != null && !string.IsNullOrEmpty(hashCustomerData["Name1"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name1"].ToString());
                }

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name2.ToString()) == "1")
                {
                    if (hashCustomerData["Name2"] != null && !string.IsNullOrEmpty(hashCustomerData["Name2"].ToString()))
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name2"].ToString());
                }

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.Name3.ToString()) == "1")
                {
                    if (hashCustomerData["Name3"] != null && !string.IsNullOrEmpty(hashCustomerData["Name3"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["Name3"].ToString());
                }

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressPostCode.ToString()) == "1")
                {
                    if (hashCustomerData["MailingAddressPostCode"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressPostCode"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressPostCode"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressPostCode"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine1.ToString()) == "1")
                {
                    if (hashCustomerData["MailingAddressLine1"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine1"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine1"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine1"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine2.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine2"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine2"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine2"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine2"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine3.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine3"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine3"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine3"].ToString());
                        strBlackList.Add(hashCustomerData["MailingAddressLine3"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine4.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine4"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine4"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine4"].ToString());
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MailingAddressLine5.ToString()) == "1")
                {
                    //Groupcountry AddressProfanity
                    if (hashCustomerData["MailingAddressLine5"] != null && !string.IsNullOrEmpty(hashCustomerData["MailingAddressLine5"].ToString()))
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["MailingAddressLine5"].ToString());
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.DaytimePhoneNumber.ToString()) == "1")
                {
                    if (hashCustomerData["daytime_phone_number"] != null && !string.IsNullOrEmpty(hashCustomerData["daytime_phone_number"].ToString()))
                    {
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["daytime_phone_number"].ToString());
                        strBlackList.Add(hashCustomerData["DaytimePhoneNumber"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.MobilePhoneNumber.ToString()) == "1")
                {
                    if (hashCustomerData["MobilePhoneNumber"] != null && !string.IsNullOrEmpty(hashCustomerData["MobilePhoneNumber"].ToString()))
                    {
                        profaneTextProvider = new SingleProfaneTextContributor(profaneTextProvider, hashCustomerData["MobilePhoneNumber"].ToString());
                        strBlackList.Add(hashCustomerData["MobilePhoneNumber"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.EmailAddress.ToString()) == "1")
                {
                    if (hashCustomerData["EmailAddress"] != null && !string.IsNullOrEmpty(hashCustomerData["EmailAddress"].ToString()))
                    {
                        profaneTextProvider = new EmailProfaneTextContributor(profaneTextProvider, hashCustomerData["EmailAddress"].ToString());
                        strBlackList.Add(hashCustomerData["EmailAddress"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.PrimaryId.ToString()) == "1")
                {
                    if (hashCustomerData["SSN"] != null && !string.IsNullOrEmpty(hashCustomerData["SSN"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["SSN"].ToString());
                        strBlackList.Add(hashCustomerData["SSN"].ToString());
                    }
                }
                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.Profanity_check_fields, DbConfigurationItemNames.SecondaryId.ToString()) == "1")
                {
                    if (hashCustomerData["PassportNo"] != null && !string.IsNullOrEmpty(hashCustomerData["PassportNo"].ToString()))
                    {
                        profaneTextProvider = new MultipleProfaneTextContributor(profaneTextProvider, hashCustomerData["PassportNo"].ToString());
                        strBlackList.Add(hashCustomerData["PassportNumber"].ToString());
                    }
                }
                logData.CaptureData("Profane Text", profaneTextProvider.ProfaneText.Trim().ToUpper());
                logData.BlackLists = strBlackList;
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Prepare profane text", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            return profaneTextProvider.ProfaneText.Trim().ToUpper();
        }
        /// <summary>
        /// Update Customer Data in DB
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        private bool UpdateCustomerDetails(Hashtable userData, Dictionary<string, string> resourceKeys, long customerId)
        {
            LogData logData = new LogData();
            bool isUpdatedCustomerDetails = false;
            try
            {
                string data = "{\"service\":\"CustomerService\",\"operation\":\"UpdateCustomerDetails\"," +
                               "\"parameters\":[{\"Key\":\"userData\",\"Value\":\"" + userData + "\"}," +
                                               "{\"Key\":\"customerType\",\"Value\":\"" + _configProvider.GetStringAppSetting(AppConfigEnum.ServiceConsumer) + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    bool cData = JsonConvert.DeserializeObject<bool>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    isUpdatedCustomerDetails = (bool)response.Status;
                    logData.RecordStep(string.Format("Customer details updated with status {0}", isUpdatedCustomerDetails));
                    _logger.Submit(logData);
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
                throw GeneralUtility.GetCustomException("Exception in Update customer details", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }

            return isUpdatedCustomerDetails;
        }

        /// <summary>
        /// Populated Customer Details from CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerFamilyMasterData GetCustomerDetails(long customerId)
        {
            CustomerFamilyMasterData customerFamilyData = new CustomerFamilyMasterData();
            LogData logData = new LogData();
            try
            {
                string data = "{\"service\":\"CustomerService\",\"operation\":\"GetCustomerFamilyMasterDataByCustomerId\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"" + customerId + "\"}," +
                                                "{\"Key\":\"maxRows\",\"Value\":\"" + maxRows + "\"}," +
                                                "{\"Key\":\"culture\",\"Value\":\"" + System.Globalization.CultureInfo.CurrentCulture.Name + "\"}]}";

                var apiResponse = this._APIRequester.MakeRequest(data);

                APIResponse apiResponseObj = JsonConvert.DeserializeObject<APIResponse>(apiResponse,
                                                                                new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });

                if (apiResponseObj.status)
                {
                    customerFamilyData = JsonConvert.DeserializeObject<CustomerFamilyMasterData>(apiResponseObj.data.ToString(),
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });

                    _logger.Submit(logData);
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
                throw GeneralUtility.GetCustomException("Exception in Get customer details", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }

            return customerFamilyData;
        }

        /// <summary>
        /// Convert Personal Details View Model to HashTable
        /// </summary>
        /// <param name="personalDetailsViewModel"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        private Hashtable GetHashTableFromViewModel(PersonalDetailsViewModel personalDetailsViewModel, long customerID, Dictionary<string, string> resourceKeys)
        {
            LogData logData = new LogData();
            CustomerMasterData customerData = new CustomerMasterData();
            AddressDetails address = new AddressDetails();
            int noHHPersons = 0;
            bool islblStRequired = true;
            bool blnAddressLine2 = true;
            bool blnAddressLine3 = true;
            bool blnAddressLine4 = true;
            Hashtable htCustomer = new Hashtable();
            List<string> blacklistData = new List<string>();
            try
            {
                customerData = personalDetailsViewModel.CustomerFamilyMasterData.CustomerData[0];
                address = personalDetailsViewModel.AddressDetails;
                if (personalDetailsViewModel.AddressDetails != null)
                {
                    if (!string.IsNullOrEmpty(personalDetailsViewModel.AddressDetails.HideAddressformat))
                    {
                        char[] chars = { ':' };
                        ArrayList arr = new ArrayList();
                        arr.AddRange(personalDetailsViewModel.AddressDetails.HideAddressformat.Split(chars));
                        personalDetailsViewModel.AddressDetails.HideAddressList = arr;
                        logData.RecordStep("Set HideAddressList");
                    }
                    if (address.Houseno == null)
                    {
                        address.Houseno = string.Empty;
                    }
                    if (address.AddressChosen == null)
                    {
                        address.AddressChosen = string.Empty;
                    }
                    if (address.Street == null)
                    {
                        address.Street = string.Empty;
                    }
                    if (address.Country == null)
                    {
                        address.Country = string.Empty;
                    }
                    if (address.City == null)
                    {
                        address.City = string.Empty;
                    }
                    if (address.Locality == null)
                    {
                        address.Locality = string.Empty;
                    }
                }
                if (!personalDetailsViewModel.FindAddressClicked)
                {
                    htCustomer["MailingAddressLine1"] = !string.IsNullOrEmpty(customerData.MailingAddressLine1) ? customerData.MailingAddressLine1.Trim().ToUpper() : String.Empty;
                    htCustomer["MailingAddressLine2"] = !string.IsNullOrEmpty(customerData.MailingAddressLine2) ? customerData.MailingAddressLine2.Trim().ToUpper() : String.Empty;
                    htCustomer["MailingAddressLine3"] = !string.IsNullOrEmpty(customerData.MailingAddressLine3) ? customerData.MailingAddressLine3.Trim().ToUpper() : String.Empty;
                    htCustomer["MailingAddressLine4"] = !string.IsNullOrEmpty(customerData.MailingAddressLine4) ? customerData.MailingAddressLine4.Trim().ToUpper() : String.Empty;
                    if (_configProvider.GetStringAppSetting(AppConfigEnum.EnableProvince) == "1")
                    {
                        htCustomer["MailingAddressLine5"] = !string.IsNullOrEmpty(customerData.MailingAddressLine5) ? customerData.MailingAddressLine5.Trim().ToUpper() : "-1";
                    }
                    else
                    {
                        htCustomer["MailingAddressLine5"] = !string.IsNullOrEmpty(customerData.MailingAddressLine5) ? customerData.MailingAddressLine5.Trim().ToUpper() : String.Empty;
                    }
                    htCustomer["MailingAddressLine6"] = !string.IsNullOrEmpty(customerData.MailingAddressLine6) ? customerData.MailingAddressLine6.Trim().ToUpper() : String.Empty;
                    logData.RecordStep("Set all Address Lines when Find Address is not clicked");
                }
                else
                {
                    if (!string.IsNullOrEmpty(address.AddressChosen))
                    {
                        logData.RecordStep("Address Chosen from Dropdown list");
                        if (address.HideAddressList.Count >= address.AddressChosen.TryParse<Int32>())
                        {
                            string[] strSplitAddress = address.HideAddressList[address.AddressChosen.TryParse<Int32>()].TryParse<string>().Split(',');
                            if (strSplitAddress.Count() > 0)
                            {
                                if (strSplitAddress[0].ToString().Trim().Length <= 4)
                                {
                                    htCustomer["MailingAddressLine1"] = strSplitAddress[0] + " " + strSplitAddress[1];
                                    islblStRequired = false;
                                }
                                else
                                {
                                    htCustomer["MailingAddressLine1"] = strSplitAddress[0].ToString();
                                    if (strSplitAddress[1].ToString().Trim().ToUpper() == address.Street.Trim().ToUpper())
                                    {
                                        islblStRequired = true;
                                    }
                                    else
                                    {
                                        islblStRequired = true;
                                        address.Street = strSplitAddress[1].TryParse<string>().Trim().ToUpper();
                                    }
                                }
                                logData.RecordStep("Set Address Line 1 from ddl");
                            }
                        }
                    }
                    else
                    {
                        htCustomer["MailingAddressLine1"] = address.Houseno.Trim().ToUpper();
                        logData.RecordStep("Set Address Line 1 based on House No");
                        string[] arrstrStreet = null;
                        string strStreet = string.Empty;

                        string[] arrStreet = address.Houseno.Trim().Split(' ').ToArray();

                        if (arrStreet.Length > 1)
                        {
                            arrstrStreet = new string[arrStreet.Length];
                            for (int i = 1; i < arrStreet.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(address.Houseno.Trim().Split(' ')[i]))
                                {
                                    arrstrStreet[i] = address.Houseno.Trim().Split(' ')[i].ToString().Trim().ToUpper();
                                }
                            }
                            for (int j = 1; j < arrstrStreet.Length; j++)
                            {
                                if (arrstrStreet[j] != null)
                                {
                                    strStreet = strStreet + " " + arrstrStreet[j].ToString();
                                }
                            }

                            if (strStreet.Trim() == address.Street.Trim().ToUpper())
                            {
                                islblStRequired = false;
                            }
                        }

                    }
                    if (islblStRequired)
                    {
                        if (!string.IsNullOrEmpty(address.Street))
                        {
                            htCustomer["MailingAddressLine2"] = address.Street;
                            logData.RecordStep("Set Address Line 2");
                        }
                        else
                        {
                            blnAddressLine2 = false;
                        }
                    }
                    else
                    {
                        blnAddressLine2 = false;
                        address.Street = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(address.Locality))
                    {
                        htCustomer["MailingAddressLine3"] = address.Locality.Trim().ToUpper();
                        logData.RecordStep("Set Address Line 3");
                    }
                    else
                    {
                        blnAddressLine3 = false;
                    }
                    if (!string.IsNullOrEmpty(address.City))
                    {
                        htCustomer["MailingAddressLine4"] = address.City.Trim().ToUpper();
                        logData.RecordStep("Set Address Line 4");
                    }
                    else
                    {
                        blnAddressLine4 = false;
                    }

                    if (!string.IsNullOrEmpty(address.Country))
                    {
                        htCustomer["MailingAddressLine5"] = address.Country.Trim().ToUpper();
                        logData.RecordStep("Set Address Line 5");
                    }

                    if (!blnAddressLine2)
                    {
                        htCustomer["MailingAddressLine2"] = blnAddressLine3 ? address.Locality.Trim().ToUpper() : address.City.Trim().ToUpper();
                        htCustomer["MailingAddressLine3"] = blnAddressLine3 ? address.City.Trim().ToUpper() : string.Empty;
                        htCustomer["MailingAddressLine4"] = string.Empty;
                        logData.RecordStep("Set Address Line 2, 3, 4 when blnAddressLine2 is false");
                    }
                    else if (!blnAddressLine3)
                    {
                        htCustomer["MailingAddressLine3"] = blnAddressLine4 ? address.City.Trim().ToUpper() : address.Country.Trim().ToUpper();
                        htCustomer["MailingAddressLine4"] = blnAddressLine4 ? address.Country.Trim().ToUpper() : string.Empty;
                        htCustomer["MailingAddressLine5"] = string.Empty;
                        logData.RecordStep("Set Address Line 3, 4, 5 when blnAddressLine3 is false");
                    }
                }
                if (Convert.ToBoolean(personalDetailsViewModel.GroupAddressAvailable))
                {
                    htCustomer["MailingAddressLine1"] = customerData.MailingAddressLine1.Trim().ToUpper();
                    htCustomer["MailingAddressLine2"] = customerData.MailingAddressLine2.Trim().ToUpper();
                    htCustomer["MailingAddressLine3"] = customerData.MailingAddressLine3.Trim().ToUpper();
                    htCustomer["MailingAddressLine4"] = customerData.MailingAddressLine4.Trim().ToUpper();
                    logData.RecordStep("Set Address Line 1, 2, 3, 4 when Group Address is Available");

                    if (_configProvider.GetStringAppSetting(AppConfigEnum.EnableProvince) == "1")
                    {
                        //if (ddlProvince.SelectedIndex != 0)
                        //htCustomer["MailingAddressLine5"] = this.ddlProvince.SelectedValue.ToString();
                        //else
                        htCustomer["MailingAddressLine5"] = !string.IsNullOrEmpty(customerData.MailingAddressLine5) ? customerData.MailingAddressLine5.Trim().ToUpper() : "-1";
                        logData.RecordStep("Set Address Line 5 when Province is enabled");
                    }
                    else
                    {
                        htCustomer["MailingAddressLine5"] = !string.IsNullOrEmpty(customerData.MailingAddressLine5) ? customerData.MailingAddressLine5.Trim().ToUpper() : String.Empty;
                        logData.RecordStep("Set Address Line 5 when Province is disabled");
                    }
                    htCustomer["MailingAddressLine6"] = customerData.MailingAddressLine6.Trim().ToUpper();
                    logData.RecordStep("Set Address Line 6");

                    if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HidePostCode) != "0")
                    {
                        htCustomer["MailingAddressPostCode"] = customerData.MailingAddressPostCode.Trim();
                    }
                    else
                    {
                        htCustomer["MailingAddressPostCode"] = string.Empty;
                    }
                    logData.RecordStep("Set Post Code");
                }
                string lbladdress = string.Empty;
                for (int i = 1; i <= 7; i++)
                {
                    if (htCustomer["MailingAddressLine" + i] != null)
                    {
                        lbladdress += htCustomer["MailingAddressLine" + i].ToString().Trim();
                    }
                }
                personalDetailsViewModel.hdnTrackAddress = lbladdress.Trim();
                logData.RecordStep("Track Address");

                htCustomer["CustomerID"] = customerID;
                htCustomer["TitleEnglish"] = !string.IsNullOrEmpty(customerData.Title) ? customerData.Title.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;
                htCustomer["Name1"] = !string.IsNullOrEmpty(customerData.FirstName) ? customerData.FirstName.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;
                htCustomer["Name2"] = !string.IsNullOrEmpty(customerData.Initial) ? customerData.Initial.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;
                htCustomer["Name3"] = !string.IsNullOrEmpty(customerData.LastName) ? customerData.LastName.Trim().ToTitleCase(CultureInfo.CurrentCulture) : String.Empty;

                if (string.IsNullOrEmpty(personalDetailsViewModel.DayOfBirth) && string.IsNullOrEmpty(personalDetailsViewModel.MonthOfBirth) && string.IsNullOrEmpty(personalDetailsViewModel.YearOfBirth))
                {
                    //DOB is empty then don't update as it not required field.
                }
                else
                {
                    String TargetDateformat = _configProvider.GetStringAppSetting(AppConfigEnum.SpecifiedDateFormat); // ConfigurationManager.AppSettings["SpecifiedDateFormat"].ToString();
                    string TargetLinker = _configProvider.GetStringAppSetting(AppConfigEnum.Linkerfordate); // ConfigurationManager.AppSettings["Linkerfordate"].ToString();

                    if (TargetDateformat == "dd" + TargetLinker + "MM" + TargetLinker + "yyyy" || TargetDateformat == "d/MM/yyyy" || TargetDateformat == "dd/M/yyyy" || TargetDateformat == "d/M/yyyy" || TargetDateformat == "dd/MM/yy")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.YearOfBirth;
                    }
                    else if (TargetDateformat == "MM/dd/yyyy" || TargetDateformat == "M/dd/yyyy" || TargetDateformat == "MM/d/yyyy" || TargetDateformat == "M/d/yyyy")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.YearOfBirth;
                    }
                    else if (TargetDateformat == "yyyy/dd/MM" || TargetDateformat == "yyyy/d/MM" || TargetDateformat == "yyyy/dd/M" || TargetDateformat == "yyyy/d/M")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.YearOfBirth + "/" + personalDetailsViewModel.DayOfBirth + "/" + personalDetailsViewModel.MonthOfBirth;
                    }
                    else if (TargetDateformat == "yyyy/MM/dd" || TargetDateformat == "yyyy/M/dd" || TargetDateformat == "yyyy/MM/d" || TargetDateformat == "yyyy/M/d")
                    {
                        htCustomer["DateOfBirth"] = personalDetailsViewModel.YearOfBirth + "/" + personalDetailsViewModel.MonthOfBirth + "/" + personalDetailsViewModel.DayOfBirth;
                    }
                }

                //   htCustomer["Sex"] = customerData.Sex;
                htCustomer["Sex"] = !string.IsNullOrEmpty(customerData.Sex) ? customerData.Sex.Trim() : String.Empty;

                logData.RecordStep("Set Customer Details");

                if (!string.IsNullOrEmpty(customerData.MailingAddressPostCode))
                {
                    blacklistData.Add(customerData.MailingAddressPostCode.Trim());
                }
                htCustomer["MailingAddressPostCode"] = !string.IsNullOrEmpty(customerData.MailingAddressPostCode) ? customerData.MailingAddressPostCode.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.DayTimePhonenumber))
                {
                    blacklistData.Add(customerData.DayTimePhonenumber.Trim());
                }
                htCustomer["daytime_phone_number"] = !string.IsNullOrEmpty(customerData.DayTimePhonenumber) ? customerData.DayTimePhonenumber.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.EmailAddress))
                {
                    blacklistData.Add(customerData.EmailAddress.Trim());
                }
                htCustomer["EmailAddress"] = !string.IsNullOrEmpty(customerData.EmailAddress) ? customerData.EmailAddress.Trim() : String.Empty;
                htCustomer["email_address"] = !string.IsNullOrEmpty(customerData.EmailAddress) ? customerData.EmailAddress.Trim() : String.Empty;

                /*The below fields are not updated by user and If we pass empty string to 
                 * the stored procedure it will make the data blank. So instead we are using hidden field so that we can 
                 pass the same data which we retrieve while loading the screen*/
                if (!string.IsNullOrEmpty(customerData.EveningPhonenumber))
                {
                    blacklistData.Add(customerData.EveningPhonenumber.Trim());
                }
                htCustomer["evening_phone_number"] = !string.IsNullOrEmpty(customerData.EveningPhonenumber) ? customerData.EveningPhonenumber.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.MobileNumber))
                {
                    blacklistData.Add(customerData.MobileNumber.Trim());
                }
                htCustomer["MobilePhoneNumber"] = !string.IsNullOrEmpty(customerData.MobileNumber) ? customerData.MobileNumber.Trim() : String.Empty;
                htCustomer["mobile_phone_number"] = !string.IsNullOrEmpty(customerData.MobileNumber) ? customerData.MobileNumber.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.SSN))
                {
                    blacklistData.Add(customerData.SSN.Trim());
                }
                htCustomer["SSN"] = !string.IsNullOrEmpty(customerData.SSN) ? customerData.SSN.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.PassportNo))
                {
                    blacklistData.Add(customerData.PassportNo.Trim());
                }
                htCustomer["PassportNo"] = !string.IsNullOrEmpty(customerData.PassportNo) ? customerData.PassportNo.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.ISOLanguageCode))
                {
                    blacklistData.Add(customerData.ISOLanguageCode.Trim());
                }
                htCustomer["ISOLanguageCode"] = !string.IsNullOrEmpty(customerData.ISOLanguageCode) ? customerData.ISOLanguageCode.Trim() : String.Empty;
                if (!string.IsNullOrEmpty(customerData.RaceID))
                {
                    blacklistData.Add(customerData.RaceID.ToString());
                }
                htCustomer["RaceID"] = !string.IsNullOrEmpty(customerData.RaceID) ? customerData.RaceID.ToString() : "0";

                if (personalDetailsViewModel.FamilyMemberDob != null && personalDetailsViewModel.FamilyMemberDob.Count > 0)
                {
                    int i = 0;
                    List<string> lstFamilyMembers = personalDetailsViewModel.FamilyMemberDob;
                    foreach (string fmd in lstFamilyMembers)
                    {
                        if (!String.IsNullOrEmpty(fmd) && fmd.ToLower() != "year")
                        {
                            i++;
                            htCustomer["family_member_" + i + "_dob"] = (fmd.ToString() + "/1/1").TryParse<DateTime>().TryParse<string>();
                        }
                    }
                    logData.RecordStep("Set Family Members DOB");

                    noHHPersons = i + 1;
                    htCustomer["number_of_household_members"] = Convert.ToInt16(noHHPersons);
                }
                else
                {
                    htCustomer["number_of_household_members"] = 0;
                }

                //Set the dietary preferences
                htCustomer["Diabetic"] = "0";
                htCustomer["Teetotal"] = "0";
                htCustomer["Vegetarian"] = "0";
                htCustomer["Halal"] = "0";
                htCustomer["Kosher"] = "0";
                htCustomer["DynamicPreferences"] = string.Empty;
                htCustomer["Culture"] = System.Globalization.CultureInfo.CurrentCulture.Name;
                htCustomer["CustomerMobilePhoneStatus"] = !string.IsNullOrEmpty(customerData.MobileNumber) ? (int)CustomerMailStatusEnum.Deliverable : (int)CustomerMailStatusEnum.Missing;
                htCustomer["CustomerEmailStatus"] = !string.IsNullOrEmpty(customerData.EmailAddress) ? (int)CustomerMailStatusEnum.Deliverable : (int)CustomerMailStatusEnum.Missing;
                logData.RecordStep("Set Dietary Preferences");

                if (_configProvider.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.HidePostCode) != "0")
                {
                    if (!string.IsNullOrEmpty(htCustomer["MailingAddressPostCode"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine1"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine2"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine3"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine4"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine5"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine6"].ToString().Trim()))
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Deliverable;
                    }
                    else
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Missing;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(htCustomer["MailingAddressLine1"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine2"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine3"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine4"].ToString().Trim()) || !string.IsNullOrEmpty(htCustomer["MailingAddressLine5"].ToString().Trim())
                        || !string.IsNullOrEmpty(htCustomer["MailingAddressLine6"].ToString().Trim()))
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Deliverable;
                    }
                    else
                    {
                        htCustomer["CustomerMailStatus"] = (int)CustomerMailStatusEnum.Missing;
                    }
                }
                htCustomer["CustomerUseStatusMain"] = BusinessConstants.CUSTOMERUSESTATUS_ACTIVE;
                logData.RecordStep("Set Customer Mail and Use Status");

                logData.BlackLists = blacklistData;
                logData.CaptureData("Hash table", htCustomer);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Get Hash Table from view model", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }

            return htCustomer;

        }

        /// <summary>
        /// Populate Customer Preferences
        /// </summary>
        /// <param name="custPreference"></param>
        /// <returns></returns>
        private List<BTClubPreference> LoadCustomerPreference(CustomerPreference custPreference,
            out string emailPreference, out string mobileContactPreference, out string postPreference)
        {
            LogData logData = new LogData();
            List<BTClubPreference> btClubList = new List<BTClubPreference>();
            emailPreference = string.Empty;
            mobileContactPreference = string.Empty;
            postPreference = string.Empty;
            try
            {
                if (custPreference != null && custPreference.Preference != null && custPreference.Preference.Length > 0)
                {
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    for (int i = 0; i < custPreference.Preference.Length; i++)
                    {
                        objPreferenceFilter.Add(custPreference.Preference[i]);
                    }
                    foreach (var pref in objPreferenceFilter)
                    {
                        if ((pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_ALLERGIC || pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_DIETRY))
                        {
                            btClubList.Add(new BTClubPreference()
                            {
                                PreferenceID = pref.PreferenceID,
                                PreferenceType = pref.CustomerPreferenceType,
                                PreferenceDescLocal = pref.PreferenceDescriptionLocal,
                                PreferenceDescEnglish = pref.PreferenceDescriptionEng,
                                OptedStatus = pref.POptStatus.ToString() == "OPTED_IN" ? true : false
                            });
                        }

                        else if (pref.PreferenceID == BusinessConstants.EMAIL_CONTACT && pref.POptStatus.ToString() == "OPTED_IN")//opted in
                        {
                            emailPreference = "true";
                        }
                        else if (pref.PreferenceID == BusinessConstants.MOBILE_CONTACT && pref.POptStatus.ToString() == "OPTED_IN")//opted in
                        {
                            mobileContactPreference = "true";
                        }
                        else if (pref.PreferenceID == BusinessConstants.POST_CONTACT && pref.POptStatus.ToString() == "OPTED_IN")//opted in
                        {
                            postPreference = "true";
                        }
                    }
                }
                logData.CaptureData("Preferences", btClubList);
                logData.CaptureData("Post Prefrence", postPreference);
                logData.CaptureData("Mobile Preference", mobileContactPreference);
                logData.CaptureData("Email Preference", emailPreference);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in loading customer prefrences", ex, new Dictionary<string, object>() 
                                                                        { 
                                                                            { LogConfigProvider.EXCLOGDATAKEY, logData }
                                                                        });
            }
            if (string.IsNullOrEmpty(emailPreference))
                emailPreference = string.Empty;
            if (string.IsNullOrEmpty(mobileContactPreference))
                mobileContactPreference = string.Empty;
            if (string.IsNullOrEmpty(postPreference))
                postPreference = string.Empty;
            return btClubList;
        }

        #endregion Private Methods
    }
}