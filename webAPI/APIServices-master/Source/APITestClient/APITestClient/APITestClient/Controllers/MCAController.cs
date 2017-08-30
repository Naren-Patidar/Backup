using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APITestClient.Helper;
using System.Configuration;
using APITestClient.MVCAttributes;
using System.Collections;
using APITestClient.Helper;

namespace APITestClient.Controllers
{
    public class MCAController : BaseController
    {
        string url = ConfigurationManager.AppSettings["APIURL"];
        APIResponseHelper apiHelper = new APIResponseHelper();

        [HttpGet]
        public ActionResult Login(string from, string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                if (IsDotcomEnvironmentEnabled)
                {
                    Redirect(ConfigurationManager.AppSettings["GenericLoginPage"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, string from, string returnUrl)
        {
            try
            {
                long customerID = 0, clubcard = 0;
                string password = ConfigurationManager.AppSettings["Password"];
                string sculture = base.CurrentCulture;

                if (password != string.Empty && password == login.Password)
                {
                    if (!string.IsNullOrEmpty(login.ClubcardNumber) && Int64.TryParse(login.ClubcardNumber, out clubcard))
                    {
                        Dictionary<string, string> headers = apiHelper.GetHeaders();

                        Int64 clubcardNo = login.ClubcardNumber.TryParse<Int64>();
                        Hashtable searchData = new Hashtable();
                        searchData["cardAccountNumber"] = clubcardNo;

                        string conditionXML = Extensions.HashTableToXML(searchData, "customer");

                        string data = "{\"service\":\"CustomerService\",\"operation\":\"GetCustomerDetails\"," +
                                            "\"parameters\":[{\"Key\":\"conditionXML\",\"Value\":\"" + conditionXML + "\"}]," +
                                            "{\"Key\":\"intMaxRows\",\"Value\":\"100\"}]},{\"Key\":\"strCulture\",\"Value\":\"en-GB\"}]}";

                        //APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);

                        customerID = 1234;
                    }
                    if (customerID != 0)
                    {
                        ViewBag.DotcomID = login.DotcomCustomerId;
                        MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID.ToString());
                        return RedirectToAction("Index", "MCA");
                    }
                    else
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DotcomLogin(string from, string returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    from = returnUrl;
                }

                ViewBag.LoginUrl = string.Format(ConfigurationManager.AppSettings["GenericLoginPage"], from);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult Signout()
        {
            try
            {
                ////Delete cookie
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerID);
                MCACookie.Cookie.Remove(MCACookieEnum.DotCustomerID);
                MCACookie.Cookie.Remove(MCACookieEnum.PtsDtls);
                MCACookie.Cookie.Remove(MCACookieEnum.PointSummaryCutOffDate);
                MCACookie.Cookie.Remove(MCACookieEnum.PointSummarySignOffDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasCurrStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasCurrEndDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasNextStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.XmasNextEndDate);

                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeFlag);
                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeStartDate);
                MCACookie.Cookie.Remove(MCACookieEnum.ExchangeEnddate);
                MCACookie.Cookie.Remove(MCACookieEnum.CouponPageDate);
                MCACookie.Cookie.Remove(MCACookieEnum.ShowOrdrRplcmtPage);
                MCACookie.Cookie.Remove(MCACookieEnum.IsSecurityCheckDone);
                MCACookie.Cookie.Remove(MCACookieEnum.IsFuelAccountExist);

                MCACookie.Cookie.Remove(MCACookieEnum.Activated);
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerMailStatus);
                MCACookie.Cookie.Remove(MCACookieEnum.CustomerUseStatus);

                MCACookie.Cookie.Remove(MCACookieEnum.DotcomCustomerID);


                if (ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_UK"].Equals(ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_GROUP"]))
                {
                    //Delete IGHSCustomerIndentity  cookie
                    string strDomainName = ConfigurationManager.AppSettings["DomainName"];
                    if (Request.Cookies["AUID"] != null)
                    {
                        HttpCookie myCookie = new HttpCookie("AUID");
                        myCookie.Expires = DateTime.Now.AddDays(-1d);
                        myCookie.Domain = strDomainName;

                        Response.Cookies.Add(myCookie);
                    }
                }
                //1 will enable the INT,OPS and Live environment(dotcom) and 0 will work for System test environment(non-dotcom)
                if (IsDotcomEnvironmentEnabled)
                {
                    string redirectURL = ConfigurationManager.AppSettings["GenericLogoutPage"];
                    return Redirect(redirectURL);

                }
                else
                {
                    return RedirectToAction("Login", "MCA");
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
            }
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult Index()
        {
            return View();
        }

        #region Coupon Service Methods

        public ActionResult GetAvailableCoupons()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"couponservice\",\"operation\":\"GetAvailableCoupons\"," +
                                "\"parameters\":[{\"Key\":\"houseHoldId\",\"Value\":\"26087395\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetRedeemedCoupons()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"couponservice\",\"operation\":\"GetRedeemedCoupons\"," +
                                "\"parameters\":[{\"Key\":\"houseHoldId\",\"Value\":\"26087395\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }
        
        #endregion

        #region Clubcard Service Methods

        public ActionResult GetOffersForCustomer()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetOffersForCustomer\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"conditionalXml\",\"Value\":\"<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>26852162</CustomerID></PointsInfoCondition>\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]," +
                                "\"parameters\":[{\"Key\":\"maxRowCount\",\"Value\":\"0\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetPointsSummary()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetPointsSummary\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"offerId\",\"Value\":\"1\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetCustomerTransactions()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();
            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetCustomerTransactions\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"37831590\"}]," +
                                "\"parameters\":[{\"Key\":\"offerId\",\"Value\":\"21\"}]," +
                                "\"parameters\":[{\"Key\":\"showMerchantFlag\",\"Value\":\"true\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);
        }

        public ActionResult GetCustomerAccountDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetCustomerAccountDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetMyCustomerAccountDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetMyCustomerAccountDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetOrderReplacementExistingStatus()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetOrderReplacementExistingStatus\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult ProcessOrderReplacementRequest()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"ClubcardService\",\"operation\":\"ProcessOrderReplacementRequest\"," +
                                "\"parameters\":[{\"Key\":\"model\",\"Value\":\"{\"ClubcardNumber\" : \"634004024037002278\", \"CustomerId\" : \"26852162\", \"CustomerIdEncrypt\" : \"vrmAYr9YBLbtMjwpKO9y9Q==\", \"Reason\" : \"L\", \"RequestType\" : \"NewCardAndKeyFOB\" }\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult IsXmasClubMember()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"IsXmasClubMember\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult IGHSCheckCustomerActivatedStatus()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"IGHSCheckCustomerActivatedStatus\"," +
                                "\"parameters\":[{\"Key\":\"dotcomCustomerID\",\"Value\":\"123456\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetHouseHoldCustomersDataSet()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetHouseHoldCustomersDataSet\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetClubcardsCustomerDataSet()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetClubcardsCustomerDataSet\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetChristmasSaverSummary()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetChristmasSaverSummary\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"171267062\"}]," +
                                "\"parameters\":[{\"Key\":\"startDate\", \"Value\":\"28/10/2013 00:00:00\"}]," +
                                "\"parameters\":[{\"Key\":\"endDate\", \"Value\":\"27/10/2014 00:00:00\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetHouseHoldCustomersData()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetHouseHoldCustomersData\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetClubcardsCustomerData()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"clubcardservice\",\"operation\":\"GetClubcardsCustomerData\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Customer Activation Service Methods

        public ActionResult GetClubcardAccountDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"activationservice\",\"operation\":\"GetClubcardAccountDetails\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"634004024126099771\"}]," +
                                "\"parameters\":[{\"Key\":\"customerEntity\",\"Value\":\"{\"PostCode\" : \"BS30 6LS\", \"Clubcard\" : \"634004024126099771\", \"FirstName\" : \"Josette\", \"LastName\" : \"Saville\"}\"}]," + 
                                "\"parameters\":[{\"Key\":\"dbConfigurations\",\"Value\":\"{\"DbConfigurationTypeEnum\" : \"DbConfigurationTypeEnum.Activation\"}\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult RegisterDotcomIdToCustomerAccount()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"activationservice\",\"operation\":\"RegisterDotcomIdToCustomerAccount\"," +
                                "\"parameters\":[{\"Key\":\"dotcomCustomerID\",\"Value\":\"123456\"}]," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"634004024126099771\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Customer Service Methods

        public ActionResult GetHouseHoldDetailsByCustomer()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"GetHouseHoldDetailsByCustomer\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]," +
                                "{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetCustomerFamilyMasterDataByClubcard()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"GetCustomerFamilyMasterDataByClubcard\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"634004024037002278\"}]," +
                                "{\"Key\":\"maxRows\",\"Value\":\"100\"}]}," +
                                "{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetCustomerVerificationStatus()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"GetCustomerVerificationStatus\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetConfiguration()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"GetConfiguration\"," +
                                "\"parameters\":[{\"Key\":\"configurationTypes\",\"Value\":\"7,\"}]," +
                                "\"parameters\":[{\"Key\":\"locale\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetCustomerIDbyGUID()       //add value
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();
             
            string data = "{\"service\":\"customerservice\",\"operation\":\"GetCustomerIDbyGUID\"," +
                                "\"parameters\":[{\"Key\":\"guid\",\"Value\":\"394E92BE-8510-4C88-9401-16DE8C6B6CC10123026242\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetCustomerFamilyMasterDataByCustomerId()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"GetCustomerFamilyMasterDataByCustomerId\"," +
                                "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"maxRows\",\"Value\":\"100\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult RecordPrintAtHomeDetails()      //need input
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            //string data = "{\"service\":\"customerservice\",\"operation\":\"RecordPrintAtHomeDetails\"," +
            //                    "\"parameters\":[{\"Key\":\"dsPrintDetails\",\"Value\":\"" + dsPrintDetails + "\"}]}";

            //APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult UpdateCustomerDetailsFromHashTable()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\": \"customerservice\", \"operation\": \"UpdateCustomerDetailsFromHashTable\"," +
                                "\"parameters\": [{" +
                                                "\"Key\": \"userData\",\"Value\": {\"CustomerUseStatusMain\" : \"1\", \"Diabetic\" : \"0\", \"Teetotal\" : \"0\", \"Culture\" : \"en-GB\", \"CustomerID\" : \"171267062\", \"Halal\" : \"0\", \"MailingAddressLine2\" : \"BITTON\", \"Kosher\" : \"0\", \"MailingAddressPostCode\" : \"BS30 6LS\", \"RaceID\" : \"0\", \"CustomerMobilePhoneStatus\" : \"7\", \"MobilePhoneNumber\" : \"07485962314\", \"DynamicPreferences\" : \"\", \"daytime_phone_number\" : \"\", \"ISOLanguageCode\" : \"\", \"evening_phone_number\" : \"\", \"SSN\" : \"\", \"CustomerEmailStatus\" : \"7\", \"Sex\" : \"F\", \"mobile_phone_number\" : \"07485962314\", \"Vegetarian\" : \"0\", \"EmailAddress\" : \"qwerty@abc.com\", \"email_address\" : \"qwerty@abc.com\", \"number_of_household_members\" : \"2\", \"CustomerMailStatus\" : \"7\", \"TitleEnglish\" : \"Miss\", \"family_member_1_dob\" : \"01/01/2003 00:00:00\", \"MailingAddressLine4\" : \"\", \"MailingAddressLine5\" : \"\", \"Name1\" : \"Josette\", \"Name2\" : \"Js\", \"Name3\" : \"Saville\", \"MailingAddressLine3\" : \"BITTON\", \"PassportNo\" : \"\", \"MailingAddressLine1\" : \"21 BARON CLOSE\", \"DateOfBirth\" : \"1/1/1932\", \"MailingAddressLine6\" : \"\"}}," +
                                                "{\"Key\": \"customerType\", \"Value\": \"Dotcom Customer\"}]}";
               
            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult UpdateCustomerDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\": \"customerservice\", \"operation\": \"UpdateCustomerDetails\"," +
                                "\"parameters\": [{\"Key\": \"customerData\", \"Value\": \"{\"Culture\": \"en-GB\", \"CustomerID\": \"171267062\", \"CustomerUseStatus\" : \"1\", \"DateOfBirth\" : \"null\", \"daytime_phone_number\" : \"\", \"email_address\" : \"qwerty@abc.com\", \"EmailAddress\" : \"qwerty@abc.com\", \"EmailStatus\" : \"Default\", \"evening_phone_number\" : \"\", \"FamilyMember1Dob\" : \"{01/01/2003 00:00:00}\", \"FamilyMember2Dob\" : \"null\", \"FamilyMember3Dob\" : \"null\", \"FamilyMember4Dob\" : \"null\", \"FamilyMember5Dob\" : \"null\", \"FirstName\" : \"Josette\", \"Initial\" : \"Js\", \"ISOLanguageCode\" : \"\", \"LastName\" : \"Saville\", \"MailingAddressLine1\" : \"21 BARON CLOSE\", \"MailingAddressLine2\" : \"BITTON\", \"MailingAddressLine3\" : \"BITTON\", \"MailingAddressLine4\" : \"\", \"MailingAddressLine5\" : \"\", \"MailingAddressLine6\" : \"\", \"MailStatus\" : \"Mailable\", \"mobile_phone_number\" : \"07485962314\", \"MobilePhoneNumber\" : \"07485962314\", \"MobilePhoneStatus\" : \"Default\", \"NumberOfHouseholdMembers\" : \"2\", \"PassportNo\" : \"\", \"PostCode\" : \"BS30 6LS\", \"RaceID\" : \"0\", \"Sex\" : \"F\", \"SSN\" : \"\", \"Title\" : \"Miss\" }\"}, {\"Key\": \"consumer\", \"Value\": \"Dotcom Customer\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult NoteSecurityAttemptInAudit()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"customerservice\",\"operation\":\"NoteSecurityAttemptInAudit\"," +
                                "\"parameters\":[{\"Key\":\"securityAttemptAudit\",\"Value\":\"{\"_browserUsed\" : \"Chrome\", \"_customerId\" : \"26852162\",	\"_ipAddress\" : \"fe80::d81f:7762:45bc:f95f%11\", \"_isValidAttempt\" : \"Y\", \"Browserused\" : \"Chrome\", \"CustomerID\" : \"26852162\", \"IPAddress\" : \"fe80::d81f:7762:45bc:f95f%11\", \"IsValidAttempt\" : \"Y\"}\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }
       
        #endregion

        #region Join Loyalty Service Methods

        public ActionResult CreateClubcardAccount()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            //string data = "{\"service\":\"joinservice\",\"operation\":\"CreateClubcardAccount\"," +
            //                    "\"parameters\":[{\"Key\":\"dotcomCustomerID\",\"Value\":\"\"}]," +
            //                    "\"parameters\":[{\"Key\":\"userData\",\"Value\":\"\"}]," +
            //                    "\"parameters\":[{\"Key\":\"joinRouteCode\",\"Value\":\"\"}]," +
            //                    "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"\"}]}";

            //APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetAccountContextFromHashTable()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"joinservice\",\"operation\":\"GetAccountContextFromHashTable\"," +
                                "\"parameters\":[{\"Key\":\"userData\",\"Value\":\"{\"CustomerUseStatusMain\" : \"1\", \"Diabetic\" : \"0\", \"Teetotal\" : \"0\", \"Culture\" : \"en-GB\", \"CustomerID\" : \"171267062\",	\"Halal\" : \"0\", \"MailingAddressLine2\" : \"BITTON\", \"Kosher\" : \"0\", \"MailingAddressPostCode\" : \"BS30 6LS\",	\"RaceID\" : \"0\",	\"CustomerMobilePhoneStatus\" : \"7\", \"MobilePhoneNumber\" : \"07485962314\",	\"DynamicPreferences\" : \"\", \"daytime_phone_number\" : \"\",	\"ISOLanguageCode\" : \"\", \"evening_phone_number\" : \"\", \"SSN\" : \"\", \"CustomerEmailStatus\" : \"7\", \"Sex\" : \"F\", \"mobile_phone_number\" : \"07485962314\", \"Vegetarian\" : \"0\", \"EmailAddress\" : \"qwerty@abc.com\", \"email_address\" : \"qwerty@abc.com\", \"number_of_household_members\" : \"2\", \"CustomerMailStatus\" : \"7\", \"TitleEnglish\" : \"Miss\", \"family_member_1_dob\" : \"01/01/2003 00:00:00\", \"MailingAddressLine4\" : \"\", \"MailingAddressLine5\" : \"\",	\"Name1\" : \"Josette\", \"Name2\" : \"Js\", \"Name3\" : \"Saville\", \"MailingAddressLine3\" : \"BITTON\", \"PassportNo\" : \"\", \"MailingAddressLine1\" : \"21 BARON CLOSE\", \"DateOfBirth\" : \"1/1/1932\", \"MailingAddressLine6\" : \"\"\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetAccountContext()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"joinservice\",\"operation\":\"GetAccountContext\"," +
                                "\"parameters\":[{\"Key\":\"customerData\",\"Value\":\"{\"Culture\" : \"en-GB\", \"CustomerID\" : \"171267062\", \"CustomerUseStatus\" : \"1\", \"DateOfBirth\" : \"null\", \"daytime_phone_number\" : \"\", \"email_address\" : \"qwerty@abc.com\", \"EmailAddress\" : \"qwerty@abc.com\", \"EmailStatus\" : \"Default\", \"evening_phone_number\" : \"\", \"FamilyMember1Dob\" : \"{01/01/2003 00:00:00}\", \"FamilyMember2Dob\" : \"null\", \"FamilyMember3Dob\" : \"null\", \"FamilyMember4Dob\" : \"null\", \"FamilyMember5Dob\" : \"null\", \"FirstName\" : \"Josette\", \"Initial\" : \"Js\", \"ISOLanguageCode\" : \"\", \"LastName\" : \"Saville\", \"MailingAddressLine1\" : \"21 BARON CLOSE\", \"MailingAddressLine2\" : \"BITTON\", \"MailingAddressLine3\" : \"BITTON\",	\"MailingAddressLine4\" : \"\", \"MailingAddressLine5\" : \"\", \"MailingAddressLine6\" : \"\", \"MailStatus\" : \"Mailable\", \"mobile_phone_number\" : \"07485962314\", \"MobilePhoneNumber\" : \"07485962314\", \"MobilePhoneStatus\" : \"Default\", \"NumberOfHouseholdMembers\" : \"2\", \"PassportNo\" : \"\", \"PostCode\" : \"BS30 6LS\", \"RaceID\" : \"0\", \"Sex\" : \"F\", \"SSN\" : \"\", \"Title\" : \"Miss\" }\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Locator Service Methods

        public ActionResult GetAddressesForPostCodeList()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"locatorservice\",\"operation\":\"GetAddressesForPostCodeList\"," +
                                "\"parameters\":[{\"Key\":\"postCode\",\"Value\":\"SA1 4HH\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Preference Service Methods

        public ActionResult GetCustomerPreferences()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"preferenceservice\",\"operation\":\"GetCustomerIDbyGUID\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]," +
                                "\"parameters\":[{\"Key\":\"preferenceType\",\"Value\":\"NULL\"}]," +
                                "\"parameters\":[{\"Key\":\"optionalPreference\",\"Value\":\"false\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetClubDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"preferenceservice\",\"operation\":\"GetClubDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult SendEmailToCustomers()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"preferenceservice\",\"operation\":\"SendEmailToCustomers\"," +
                                "\"parameters\":[{\"Key\":\"objcustPref\",\"Value\":\"null\"}," + 
                                    "{\"Key\":\"customerdetails\",\"Value\":\"{\"AssociateClubcardID\" : \"0\", \"AssociateCustName1\" : \"JOSETTE\", \"AssociateCustName2\" : \" \", \"AssociateCustName3\" : \"SAVILLE\", \"ClubcardID\" : \"634004024126099771\", \"CustomerID\" : \"171267062\", \"EmailAddress\" : \"qwerty@abc.com\", \"Name1\" : \"JOSETTE\", \"Name3\" : \"SAVILLE\", \"PointsBalanceQty\" : \"2180\", \"PrimaryClubcardID\" : \"634004024126099771\", \"PrimaryCustomerFullName\" : \"Miss J SAVILLE\", \"PrimaryCustomerName1\" : \"JOSETTE\", \"PrimaryCustomerName2\" : \"\", \"PrimaryCustomerName3\" : \"SAVILLE\", \"TitleEnglish\" : \"Miss\", \"Vouchers\" : \"21.5\" }\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult SendEmailNoticeToCustomers()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"preferenceservice\",\"operation\":\"SendEmailNoticeToCustomers\"," +
                                "\"parameters\":[{\"Key\":\"CustomerID\",\"Value\":\"171267062\"}," +
                                    "{\"Key\":\"objcustPref\",\"Value\":\"null\"}," +
                                    "{\"Key\":\"customerdetails\",\"Value\":\"{\"AssociateClubcardID\" : \"0\", \"AssociateCustName1\" : \"JOSETTE\", \"AssociateCustName2\" : \" \", \"AssociateCustName3\" : \"SAVILLE\", \"ClubcardID\" : \"634004024126099771\", \"CustomerID\" : \"171267062\", \"EmailAddress\" : \"qwerty@abc.com\", \"Name1\" : \"JOSETTE\", \"Name3\" : \"SAVILLE\", \"PointsBalanceQty\" : \"2180\", \"PrimaryClubcardID\" : \"634004024126099771\", \"PrimaryCustomerFullName\" : \"Miss J SAVILLE\", \"PrimaryCustomerName1\" : \"JOSETTE\", \"PrimaryCustomerName2\" : \"\", \"PrimaryCustomerName3\" : \"SAVILLE\", \"TitleEnglish\" : \"Miss\", \"Vouchers\" : \"21.5\" }\"}," +
                                    "{\"Key\":\"PageName\",\"Value\":\"Preference\"}," +
                                    "{\"Key\":\"trackHT\",\"Value\":\"{\"newEmailaddress\" : \"qwerty@abc.com\", \"bEmailChange\" : \"bEmailChange\", \"oldEmailAddress\" : \"qwerty@abc.com\", \"Email\" : \"E-mail address\"}\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult UpdateCustomerPreferences()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"preferenceservice\",\"operation\":\"UpdateCustomerPreferences\"," +
                                "\"parameters\":[{\"Key\":\"CustomerID\",\"Value\":\"171267062\"}," +
                                    "{\"Key\":\"objcustPref\",\"Value\":\"{\"Culture\" : \"en-GB\", \"CustomerID\" : \"0\", \"CustomerPreferenceType\" : \"\", \"EmailSubject\" : \"null\", \"IsDeleted\" : \"null\", \"POptStatus\" : \"NOT_SELECTED\", \"Preference\" : [{\"Culture\" : \"null\", \"CustomerID\" : \"0\", \"CustomerPreferenceType\" : \"0\", \"EmailSubject\" : \"Your preferences have been updated successfully\", \"IsDeleted\" : \"null\", \"POptStatus\" : \"OPTED_IN\", \"Preference\" : \"null\", \"PreferenceDescriptionEng\" : \"null\", \"PreferenceDescriptionLocal\" : \"null\", \"PreferenceID\" : \"43\", \"Sortseq\" : \"0\", \"Status\" : \"null\", \"UpdateDateTime\" : \"{20/11/2016 20:51:11}\", \"UserID\" : \"null\" }, {\"Culture\" : \"null\", \"CustomerID\" : \"0\", \"CustomerPreferenceType\" : \"0\", \"EmailSubject\" : \"null\", \"IsDeleted\" : \"null\", \"POptStatus\" : \"OPTED_OUT\", \"Preference\" : \"null\", \"PreferenceDescriptionEng\" : \"null\", \"PreferenceDescriptionLocal\" : \"null\", \"PreferenceID\" : \"44\", \"Sortseq\" : \"0\", \"Status\" : \"null\", \"UpdateDateTime\" : \"{20/11/2016 20:51:11}\", \"UserID\" : \"null\" }, {\"Culture\" : \"null\", \"CustomerID\" : \"0\", \"CustomerPreferenceType\" : \"0\", \"EmailSubject\" : \"null\", \"IsDeleted\" : \"null\", \"POptStatus\" : \"OPTED_OUT\", \"Preference\" : \"null\", \"PreferenceDescriptionEng\" : \"null\", \"PreferenceDescriptionLocal\" : \"null\", \"PreferenceID\" : \"45\", \"Sortseq\" : \"0\", \"Status\" : \"null\", \"UpdateDateTime\" : \"{20/11/2016 20:51:11}\", \"UserID\" : \"null\" }, {\"Culture\" : \"null\", \"CustomerID\" : \"0\", \"CustomerPreferenceType\" : \"0\", \"EmailSubject\" : \"null\", \"IsDeleted\" : \"null\", \"POptStatus\" : \"OPTED_OUT\", \"Preference\" : \"null\", \"PreferenceDescriptionEng\" : \"null\", \"PreferenceDescriptionLocal\" : \"null\", \"PreferenceID\" : \"46\", \"Sortseq\" : \"0\", \"Status\" : \"null\", \"UpdateDateTime\" : \"{20/11/2016 20:51:11}\", \"UserID\" : \"null\"}], \"PreferenceDescriptionEng\" : \"null\", \"PreferenceDescriptionLocal\" : \"null\", \"PreferenceID\" : \"0\", \"Sortseq\" : \"0\", \"Status\" : \"null\", \"UpdateDateTime\" : \"{01/01/0001 00:00:00}\", \"UserID\" : \"Dotcom Customer\"}\"}," +
                                    "{\"Key\":\"customerdetails\",\"Value\":\"{\"AssociateClubcardID\" : \"0\", \"AssociateCustName1\" : \"JOSETTE\", \"AssociateCustName2\" : \" \", \"AssociateCustName3\" : \"SAVILLE\", \"ClubcardID\" : \"634004024126099771\", \"CustomerID\" : \"171267062\", \"EmailAddress\" : \"qwerty@abc.com\", \"Name1\" : \"JOSETTE\", \"Name3\" : \"SAVILLE\", \"PointsBalanceQty\" : \"2180\", \"PrimaryClubcardID\" : \"634004024126099771\", \"PrimaryCustomerFullName\" : \"Miss J SAVILLE\", \"PrimaryCustomerName1\" : \"JOSETTE\", \"PrimaryCustomerName2\" : \"\", \"PrimaryCustomerName3\" : \"SAVILLE\", \"TitleEnglish\" : \"Miss\", \"Vouchers\" : \"21.5\"}\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult UpdateClubDetails()
        {
            //Dictionary<string, string> headers = apiHelper.GetHeaders();

            //string data = "{\"service\":\"preferenceservice\",\"operation\":\"UpdateClubDetails\"," +
            //                    "\"parameters\":[{\"Key\":\"customerId\",\"Value\":\"26852162\"}]," +
            //                    "\"parameters\":[{\"Key\":\"clubDetails\",\"Value\":\"26852162\"}]," +
            //                    "\"parameters\":[{\"Key\":\"emailIdTo\",\"Value\":\"26852162\"}]}";

            //APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Reward Service Methods

        public ActionResult GetRewardAndTokens()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"rewardservice\",\"operation\":\"GetRewardAndTokens\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"26852162\"}," +
                                    "{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        public ActionResult GetTokens()
        {
            //Dictionary<string, string> headers = apiHelper.GetHeaders();

            //string data = "{\"service\":\"rewardservice\",\"operation\":\"GetRewardAndTokens\"," +
            //                    "\"parameters\":[{\"Key\":\"gid\",\"Value\":\"26852162\"}]," +
            //                    "\"parameters\":[{\"Key\":\"bookingIdVal\",\"Value\":\"en-GB\"}]," +
            //                    "\"parameters\":[{\"Key\":\"productLineIdVal\",\"Value\":\"en-GB\"}]," +
            //                    "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            //APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

        #region Voucher Service Methods

        public ActionResult GetVoucherRewardDetails()
        {
            UpdateCustomerDetailsFromHashTable();
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"smartvoucherservice\",\"operation\":\"GetVoucherRewardDetails\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"634004024037002278\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);      
        }

        public ActionResult GetUnusedVoucherDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetUnusedVoucherDetails\"," +
                                "\"parameters\":[{\"Key\":\"customerID\",\"Value\":\"37831590\"}]," +
                                "\"parameters\":[{\"Key\":\"cardNumber\",\"Value\":\"634004024007890751\"}]," +
                                "\"parameters\":[{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);
        }

        public ActionResult GetCustomerVoucherValCPS()
        {
            DateTime stDate = new DateTime();
            DateTime enDate = new DateTime();
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"smartvoucherservice\",\"operation\":\"GetCustomerVoucherValCPS\"," +
                                "\"parameters\":[{\"Key\":\"cardNumber\",\"Value\":\"634004022011286438\"}]," +
                                "\"parameters\":[{\"Key\":\"stDate\",\"Value\":\"00010101\"}]," +
                                "\"parameters\":[{\"Key\":\"enDate\",\"Value\":\"00010101\"}]}";
            
            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);
        }

        public ActionResult GetUsedVoucherDetails()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetUsedVoucherDetails\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"634004024007890751\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);
        }

        public ActionResult GetRewardDetailsMiles()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"SmartVoucherService\",\"operation\":\"GetRewardDetailsMiles\"," +
                                "\"parameters\":[{\"Key\":\"clubcardNumber\",\"Value\":\"6340049003867800\"}]," +
                                "\"parameters\":[{\"Key\":\"reasonCode\",\"Value\":\"2\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index", apiResponse);
        }

        #endregion

        #region Utility Service Methods

        public ActionResult IsProfaneText()
        {
            Dictionary<string, string> headers = apiHelper.GetHeaders();

            string data = "{\"service\":\"rewardservice\",\"operation\":\"GetRewardAndTokens\"," +
                                "\"parameters\":[{\"Key\":\"text\",\"Value\":\"loser\"}]}";

            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);
            return View("Index");
        }

        #endregion

    }
}