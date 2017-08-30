using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    public class ParameterNames
    {
        public const string OPERATION_NAME = "Operation";
        public const string CUSTOMER_ID = "customerId";
        public const string CUSTOMER_ID_Points = "CustomerID";
        public const string CLUBCARD_NUMBER = "clubcardNumber";
        public const string CULTURE = "culture";
        public const string MAX_ROWS = "maxRows";
        public const string HOUSEHOLD_ID = "houseHoldId";
        public const string TOTAL_COUPONS_VALUE = "totalCouponsVal";
        public const string AIRMILES = "AIRMILES";
        public const string BAMILES = "BAMILES";
        public const string VIRGIN = "VIRGIN";
        public const string SUMMARY = "Summary";
        public const string MERCHANTFLAG= "showMerchantFlag";
        public const string OFFER_ID = "offerId";
        public const string PREFERENCE_TYPE = "PreferenceType";
        public const string OPTIONAL_PREFERENCE = "optionalPreference";
        public const string CUSTOMER_PREFERENCE = "CustomerPreference";
        public const string CUSTOMER_DETAILS = "CustomerDetails";
        public const string CUSTOMER_PREFERENCES = "customerPreferences";
        public const string CUST_DETAILS = "customerDetails";
        public const string PAGE_NAME ="PageName";
        public const string TRACKHT = "trackHT";
        public const string CLUB_DETAILS = "clubDetails";
        public const string EMAIL_ID_TO = "emailIdTo";
        public const string USER_DATA = "userData";
        public const string CUSTOMER_DATA = "customerData";
        public const string PROFANE_TEXT = "text";
        public const string DOTCOM_CUSTOMER_ID = "dotcomcustomerId";
        public const string GENERIC_LOGOUT_PAGE="GenericLogoutPage";
        public const string MODEL = "model";
        public const string DOTCOM_CUSTOMER = "DotcomCustomer";
        
        #region SecurityPage

        public const string SECURITY_ATTEMPT_AUDIT = "securityAttemptAudit";
        public const string MY_PERSONAL_DETAILS = "PersonalDetails";
        public const string MY_VOUCHERS = "Vouchers";
        public const string MY_VOUCHER1 = "Voucher1";
        public const string HOME_PAGE = "Home";
        public const string ERROR_PAGE = "Error";
        public const string MY_BOOSTS = "BoostsAtTesco";
        public const string LOGIN_SOLUTION_TYPE_UK = "MARTINI";
        public const string REFMONITOR_IDENTIFICAION_TOKEN = "ReferenceMonitorIdentificationToken";
        public const string REFMONTIOR_AUTH_TOKEN = "ReferenceMonitorAuthToken";
        public const string LOGIN_SOLUTION_TYPE_GROUP = "IGHS";
        public const string IGHS_CUSTOMERIDENTITY = "CustomerIdentity";
        public const string IS_SECURITY_CHECKDONE="IsSecurityCheckDone";

        #endregion

        #region ActivaionCheck

        public const int CUSTOMERUSESTATUS_BANNED = 2;
        public const int CUSTOMERUSESTATUS_LEFTSCHEME = 3;
        public const char CustomerActivated = 'Y';
        public const char CustomerPending = 'P';
        public const char CustomerNotactivated = 'N';
        #endregion

        public const string DS_COUPONS = "dsCouponsToInsert";
        public const string DS_VOUCHER = "dsPrintVoucherDetails";
        public const string CONDITIONAL_XML = "conditionalXml";
        public const string START_DATE = "startDate";
        public const string END_DATE = "endDate";
        public const string REASON_CODE = "reasonCode";
        public const string DS_TOKENS = "dsTokens";
        public const string CONFIGURATION_TYPES = "configurationTypes";

        //token downloading
        public const string TOKEN_GUID = "gid";
        public const string PRODUCT_LINE_VAL = "productLineIdVal";
        public const string BOOKING_ID_VAL = "bookingIdVal";


        public const string CONFIGURATION_TABLE_NAME = "ActiveDateRangeConfig";
        public const string RACE_TABLE_NAME = "Table1";
        public const string LANGUAGE_TABLE_NAME = "Table2";
        public const string PROVINCE_TABLE_NAME = "Table3";
        public const string FRIENDLY_ERROR_MESSAGE = "FriendlyError";

        //Voucher_and_coupon_download
        public const string CUSTOMER_GUID = "guid";

        public const string DB_CONFIGURATION = "dbConfigurations";
        public const string CUSTOMER_ENTITY = "customerEntity";
        public const string POST_CODE = "PostCode";
        //JoinPage
        public const string JOIN_ROUTE_CODE = "joinRouteCode";
    }
}
