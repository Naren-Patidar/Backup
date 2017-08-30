using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Constants
{
    public class DBConfigKeys
    {
        #region HideJoinFunctionality
        public const string HIDELATESTSTATEMENTPAGE = "HideLatestStatementPage";
        public const string HIDEPERSONALDETAILS = "HidePersonalDetails";
        public const string HIDEPREFERENCESPAGE = "HidePreferencesPage";
        public const string HIDEOPTIONSANDBENEFITS = "HideOptionsandBenefits";
        public const string HIDEMANAGECARDSPAGE = "HideManageCardsPage";
        public const string HIDEORDERAREPLACEMENTPAGE = "HideOrderAReplacementPage";
        public const string HIDEEXCHANGESPAGE = "HideExchangesPage";
        public const string HIDEECOUPONPAGE = "HideeCouponPage";
        public const string HIDEPOINTSPAGE = "HidePointsPage";
        public const string HIDEVOUCHERSPAGE = "HideVouchersPage";
        public const string HIDEJOINPAGE = "HideJoinFunctionality";
        public const string HIDEPOINTSSUMMARYPAGE = "HidePointsSummaryPage";
        public const string HIDECHRISTMASSAVERPAGE = "HideChristmasSaversPage";
         public const string HIDEPOINTSSTARTDATE = "HidePointsStartDate";
         public const string HIDEPOINTSENDDATE = "HidePointsEndDate";
        
            
        #endregion

        #region HomePage
        public const string STAMP_OPTIONBENEFIT = "FlyingSchemes";
        public const string STAMP_ORDERREPLACEMENT = "OrderAReplacement";
        public const string STAMP_TESCOBOOST = "BoostsAtTesco";
        public const string STAMP_CHRISTMASSAVER = "ChristmasSaver";
        public const string STAMP_COUPON = "Coupons";
        public const string STAMP_LATESTSTATEMENT = "MyLatestStatement";
        public const string STAMP_MYPOINTS = "Points";
        public const string STAMP_MYVOUCHERS = "Vouchers";
        public const string STAMP_MYPERSONALDETAILS = "PersonalDetails";
        public const string ENABLEHOMEPAGE = "EnableSecurityLayerOnMCAHome";
        public const string ISBANNERENABLED = "IsCustomerDetailsBannerVisible";
        public const string ISMESSAGEBOXVISIBLE = "IsMyMessageBoxVisible";
        public const string ISVOUCHERSTRIPVISIBLE = "IsVoucherStripVisible";
        #endregion

        #region SecurityPage

        public const string HOME = "HOME";
        public const string PERSONALDETAILS = "PERSONALDETAILS";
        public const string MYCONTACTPREFERENCES = "MYCONTACTPREFERENCES";
        public const string OPTIONSANDBENEFITS = "OPTIONSANDBENEFITS";
        public const string BOOSTSATTESCO = "BOOSTSATTESCO";
        public const string COUPONS = "COUPONS";
        public const string VIEWMYCARDS = "VIEWMYCARDS";
        public const string JOIN = "JOIN";
        public const string VOUCHERS = "VOUCHERS";
        public const string ACTIVATION = "ACTIVATION";
        public const string ORDERAREPLACEMENT = "ORDERAREPLACEMENT";
        public const string MYLATESTSTATEMENT = "MYLATESTSTATEMENT";
        public const string POINTS = "POINTS";
        public const string CHRISTMASSAVER = "CHRISTMASSAVER";
        public const string SECURITYAFTERACTIVATION = "EnableSecurityAfterActivation";

        #endregion

        #region Print_Config_Values
        public const string PRINT_TEMPCLUBCARD = "EnablePrintTempCard";
        #endregion

        #region ChinaHiddenFunctionality
        public const string HIDELEGALPOLICY = "HideLegalPolicy";
        public const string HIDEPROMOTIONALCODE = "HidePromotionalCodeInJoinPage";
        public const string HIDETITLE = "ChinaHiddenFunctionalityTitle";
        public const string HIDENAME2 = "ChinaHiddenFunctionalityMiddleName";
        public const string HIDEEVENINGNUMBER = "ChinaHiddenFunctionalityEveningPhoneNo";
        public const string HIDEMOBILENUMBER = "HideMobilePhoneNo";
        public const string HIDENAME1 = "HideFirstName";
        public const string HIDENAME3 = "HideSurName";
        public const string HIDEPOSTCODE = "HidePostCode";
        public const string HIDEDAYTIMENUMBER = "HideDayTimePhoneNo";
        public const string HIDEFIRSTNAMEINSALUTATION = "HideFirstNameinSalutation";
        public const string HIDEEMAIL = "HideEmail";
        public const string HIDEADDRESSLINE1 = "HideAddressLine1";
        public const string HIDEADDRESSLINE2 = "HideAddressLine2";
        public const string HIDEADDRESSLINE3 = "HideAddressLine3";
        public const string HIDEADDRESSLINE4 = "HideAddressLine4";
        public const string HIDEADDRESSLINE5 = "HideAddressLine5";
        public const string HIDEADDRESSLINE6 = "HideAddressLine6";
        public const string HIDEDOB = "HideDOB";
        public const string HIDEGENDER = "HideGender";
        public const string HIDEPREFERREDLANGUAGE = "HidePreferredLanguage";
        public const string HIDEPRIMARYID = "HidePrimaryId";
        public const string HIDERACE = "HideRace";

        #endregion

        #region Profanity_check_fields
        public const string NAME1 = "Name1";
        public const string NAME2 = "Name2";
        public const string NAME3 = "Name3";
        public const string HOUSENAME = "MailingAddressPostCode";
        public const string MAILINGADDRESSLINE1 = "MailingAddressLine1";
        public const string MAILINGADDRESSLINE2 = "MailingAddressLine2";
        public const string MAILINGADDRESSLINE3 = "MailingAddressLine3";
        public const string MAILINGADDRESSLINE4 = "MailingAddressLine4";
        public const string MAILINGADDRESSLINE5 = "MailingAddressLine5";

        #endregion

        #region Mandatory_fileds
        public const string DOB = "DateOfBirth";
        public const string POSTCODE = "MailingAddressPostCode";
        public const string PROVINCE = "MailingAddressLine5";
        public const string PREFEREDLANGUAGE = "Language";
        #endregion

        #region Format
        public const string REGEXFORPOSTCODE = "MailingAddressPostCode";
        public const string OUTOFRANGEPOSTCODE = "MailingAddressPostCode";
        public const string DISPLAYDATEFORMAT = "DisplayDateFormat";
        public const string REGEXFOREMAIL = "EmailAddress";
        public const string REGEXFORADDRESSLINE = "MailingAddressLine";

        #endregion

        #region ShowMyCoupon
        public const string SHOWMYCOUPON = "ShowMyCouponPage";
        #endregion
        #region Group_Config_Values
        public const string GROUPCOUNTRYADDRESS = "GroupCountryAddress";
        public const string REPLACEMENTTEXT = "IsReplacementCardWithYourNewName";
        #endregion

        #region Voucher
        public const string ISFACEBOOKREQUIRED = "IsFacebookRequired";
        public const string ISTWITTERREQUIRED = "IsTwitterRequired";
        #endregion

        #region Appsettings WebConfig
        public const string DATERANGE_FOR_COLLECTION_PERIOD = "DateRangeForCollectionPeriod";
        public const string DISPLAY_DATE_FORMAT = "DisplayDateFormat";
        public const string DISABLE_BONUS_POINTS = "DisableBonusPoints";
        public const string DISABLE_CURRENCY_DECIMAL = "DisableCurrencyDecimal";
        public const string SHOW_TYPE_OF_CARD = "ShowTypeofCard";
        public const string DISABLEDIEATARYPREFERENCE = "DisableDiaetoryPreference";
        public const string DUPLICATECHECKREQUIRED = "AccountDuplicateCheckRequired";
        public const string PROFANITYREQUIRED = "Profanity";
        public const string LOCALEENABLED = "IsLocale";
        public const string PROVINCEENABLED = "EnableProvince";
        public const string ISCAPTCHAENABLED = "IsCaptchaEnabled";
        public const string ISOPTINBEHAVIOUR = "IsOptInBehaviour";
        public const string ISDOTCOMENABLED = "IsDotcomEnvironment";
        public const string ONLINESECTION = "ShowTescoDotcomCoupons";
        public const string TRANSACTION_TYPE = "Transaction_type";
        public const string IS_ADDRESSAPI_ENABLED = "AddressAPI";
        public const string ISMEMBERSHIPFORAVIOSENABLE = "IsMembershipForAviosEnable";
        public const string PREFERENCEFOOTERLINK = "IsPrivacyPolicyVisibleForPreference";
        public const string PREFERENCEUICONFIGURATION = "PreferenceUIConfiguration";
        #endregion

        #region Length_of_the_input_fields
        public const string NAME3LENGTH = "Name3";
        public const string NAME1LENGTH = "Name1";
        public const string NAME2LENGTH = "Name2";
        public const string MOBILENUMBER = "MobilePhoneNumber";
        public const string DAYTIMENUMBER = "DaytimePhoneNumber";
        public const string EVENINGNUMBER = "EveningPhoneNumber";
        public const string LN_ADDRESSLINE1 = "MailingAddressLine1";
        public const string LN_ADDRESSLINE2 = "MailingAddressLine2";
        public const string LN_ADDRESSLINE3 = "MailingAddressLine3";
        public const string LN_ADDRESSLINE4 = "MailingAddressLine4";
        public const string LN_ADDRESSLINE5 = "MailingAddressLine5";
        public const string LN_ADDRESSLINE6 = "MailingAddressLine6";
        #endregion

        #region Prefix
        public const string MOBILENUMBERPREFIX = "MobilePhoneNumber";
        public const string DAYNUMBERPREFIX = "DaytimePhoneNumber";
        public const string EVENINGNUMBERPREFIX = "EveningPhoneNumber";


        #endregion

        #region Activation

        public const string ACTIVATION_DOB = "DayofBirth";
        public const string ACTIVATION_DAY_OF_BIRTH = "DayofBirth";
        public const string ACTIVATION_MONTH_OF_BIRTH = "MonthofBirth";
        public const string ACTIVATION_YEAR_OF_BIRTH = "YearofBirth";
        public const string ACTIVATION_FIRSTNAME = "Name1";
        public const string ACTIVATION_SURNAME = "Name3";
        public const string ACTIVATION_POSTCODE = "MailingAddressPostCode";

        #endregion

        #region Name Configurations
        public const string DISPLAY_NAME1 = "DisplayName1";
        public const string DISPLAY_NAME2 = "DisplayName2";
        public const string DISPLAY_NAME3 = "DisplayName3";
        public const string NAME1_ABBREVIATION = "Name1Abbreviation";
        public const string NAME2_ABBREVIATION = "Name2Abbreviation";
        public const string NAME3_ABBREVIATION = "Name3Abbreviation";
        public const string NAME1_CASING = "Name1Casing";
        public const string NAME2_CASING = "Name2Casing";
        public const string NAME3_CASING = "Name3Casing";
        #endregion

        #region  Segmentmember

        public const string CONTACT_API_URL = "ContactApiUrl";

        public const string TopicId = "TopicId";

        public const string IsEnterpriceServiceCallsEnabled = "IsEnterpriceServiceCallsEnabled";

        #endregion

        public const string COLMONTHNAME = "ColMonthName";
        public const string COLLECTIONPERIODMONTH = "CollectionPeriodMonth";
    }
}