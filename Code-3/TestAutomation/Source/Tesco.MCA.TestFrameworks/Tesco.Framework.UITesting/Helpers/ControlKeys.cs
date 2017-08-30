using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Helpers
{
    public class ControlKeys
    {
        //Common Keys
        public const string PAGE_TITLE = "CommonControls_lblPageTitle";
        //Security Keys
        public const string TEXT_VERIFICATION = "SecurityControls_txtVerification";
        public const string SECURITY_FIRSTDIGIT = "SecurityControls_lblFirstDigit";
        public const string SECURITY_SECONDDIGIT = "SecurityControls_lblSecondDigit";
        public const string SECURITY_THIRDDIGIT = "SecurityControls_lblThirdDigit";
        public const string SECURITY_FIRSTANSWER = "SecurityControls_txtFirstAnswer";
        public const string SECURITY_SECONDANSWER = "SecurityControls_txtSecondAnswer";
        public const string SECURITY_THIRDANSWER = "SecurityControls_txtThirdAnswer";
        public const string SECURITY_BUTTON = "SecurityControls_btnSubmit";
        public const string SECURITY_DEFAULTMESSAGE = "SecurityControls_lblSecurityMsg";
        public const string SECURITY_INVALIDNOMESSAGE = "SecurityControls_lblSecurityErrorMsg";
        public const string SECURITY_INVALIDCHARACTERERRORMSG1 = "SecurityControls_lblSecurityCharErrorMsg1";
        public const string SECURITY_INVALIDCHARACTERERRORMSG2 = "SecurityControls_lblSecurityCharErrorMsg2";
        public const string SECURITY_INVALIDCHARACTERERRORMSG3 = "SecurityControls_lblSecurityCharErrorMsg3";
        public const string SECURITY_INVALIDCHARMSG1 = "SecurityControls_lblSecurityCharErrorMsg1";
        public const string SECURITY_INVALIDCHARMSG2 = "SecurityControls_lblSecurityCharErrorMsg2";
        public const string SECURITY_INVALIDCHARMSG3 = "SecurityControls_lblSecurityCharErrorMsg3";
        public const string SECURITY_INVALIDSPACEERRORMSG1 = "SecurityControls_lblRequiredFieldValidator1";
        public const string SECURITY_INVALIDSPACEERRORMSG2 = "SecurityControls_lblRequiredFieldValidator2";
        public const string SECURITY_INVALIDSPACEERRORMSG3 = "SecurityControls_lblRequiredFieldValidator3";
        public const string SECURITY_MAXATTEMPTMSG = "SecurityControls_maxAttemptMsg";
        public const string SECURITY_LOGOUT = "SecurityControls_btnSecurityLogout";

        //DBT Login Keys
        public const string LOGIN_TEXTVERIFICATION = "LoginControls_txtVerification";
        public const string DBTLOGIN_CLUBCARD = "LoginControls_btnMCAloginDbt";
        public const string DBTLOGIN_PASSWORD = "LoginControls_btnMCAPassDbt";
        public const string DBTLOGIN_SUBMIT = "LoginControls_btnSubmit";

        //PPE Login Keys 
        public const string PPELOGIN_EMAIL = "LoginControls_btnMCAEmailPpe";
        public const string PPELOGIN_PASSWORD = "LoginControls_btnMCAPassPpe";
        public const string PPELOGIN_SIGNIN = "LoginControls_btnSignIn";

        //DBT Logout Keys
        public const string DBTLOGOUT = "LogOutControls_btnMCAlogOutDbt";

        //Home Keys
        public const string HOME_TITLE = "Home_title";
        public const string HOMEPAGE_LINK = "HomePage_lnkHomepage";
        public const string HOMEPAGE_LEFTNAVIGATION = "HomePage_lnkHomepage1";
        public const string HOME_PRINT_CLUBCARD = "HomePage_btnPrintClubcard";
        public const string HOME_MESSAGE_LBL = "HomePage_HomelblMESSAGE";
        public const string HOME_MESSAGE_Text = "HomePage_MESSAGEText";
        public const string HOME_PTSSMRY_LBL = "HomePage_PtsSummarylbl";
        public const string HOME_PTSSMRY_VALUE = "HomePage_PtsSummaryValue";
        public const string HOME_SECONDPOINTSBOX_LBL = "HomePage_SecondPointsBoxlbl";
        public const string HOME_SECONDPOINTSBOX_VALUE = "HomePage_SecondPointsBoxValue";
        //Header Keys

        public const string HEADER_CLUBCARDLINKS = "HeaderControls_Clubcardlinks";
        public const string HEADER_EXTERNALINKS = "HeaderControls_Externallinks";

        public const string HEADER_ABOUTCLUBCARD = "HeaderControls_aboutclubcard";
        public const string HEADER_CLUBCARDBOOST = "HeaderControls_clubcardboost";
        public const string HEADER_CLUBCARDPERKS = "HeaderControls_clubcardperks";
        public const string HEADER_FACEBOOK = "HeaderControls_facebook";
        public const string HEADER_TWITTER = "HeaderControls_twitter";
        public const string HEADER_YOUTUBE = "HeaderControls_youtube";
        public const string HEADER_BASKET = "HeaderControls_basket";
        public const string HEADER_SIGNOUT = "HeaderControls_signout";
        public const string HEADER_EMAILSIGNUP = "HeaderControls_emailsignup";
        public const string HEADER_WEBSITE_FEEDBACK = "HeaderControls_websitefeedback";
        public const string HEADER_Tesco_DOTCOM = "HeaderControls_tescocom";
        public const string HEADER_TESCO_CLUBCARD = "HeaderControls_tescoclubcard";
        public const string HEADER_WELCOME_TESCO = "HeaderControls_welcometotescoclubcard";

        //Footer Keys
        public const string FOOTER_LINKS = "FooterControls_footerlinks";

        //common key
        public const string LINK_CLICK = "CommonControls_linkName";

        //Account Details Key
        public const string ACCOUNT_CLICK = "MyAccountDetails_lblMyAccDetail";

        //Account
        public const string ACCOUNT_MC_MAINMSG = "MyAccountDetails_lblViewMyCardMainMsg";
        public const string ACCOUNT_MC_COMMONMSG = "MyAccountDetails_lblViewMyCardCommonMsg";
        public const string ACCOUNT_MC_ASSOMSG = "MyAccountDetails_lblViewMyCardAssoMsg";
        public const string ACCOUNT_MC_MAINCLUBCARDS = "MyAccountDetails_lblMainClubcards";
        public const string ACCOUNT_MC_ASSOCLUBCARDS = "MyAccountDetails_lblAssoClubcards";
        public const string ACCOUNT_MC_SHOWTYPEOFCARD = "MyAccountDetails_lblShowTypeofCard";
        public const string ACCOUNT_MC_WHEREUSED = "MyAccountDetails_lblWhereUsed";
        public const string ACCOUNT_MC_WHEREUSEDDETAILS = "MyAccountDetails_lblWhereUsedDetails";
        public const string ACCOUNT_MC_Asso_WHEREUSED = "MyAccountDetails_lblWhereUsedasso";
        public const string ACCOUNT_MC_ASSO_WHEREUSEDDETAILS = "MyAccountDetails_lblWhereUsedDetailsasso";
        public const string ACCOUNT_MC_LASTUSED = "MyAccountDetails_lblLastUsed";
        public const string ACCOUNT_MC_LASTUSEDWITHTYPEOFCARD = "MyAccountDetails_lblLastUsedwithTypeofCard";
        public const string ACCOUNT_MC_ASSOLASTUSED = "MyAccountDetails_lblLastUsedAsso";
        public const string ACCOUNT_MC_ASSOLASTUSEDWITHTYPEOFCARD = "MyAccountDetails_lblLastUsedAssowithTypeofCard";
        public const string ACCOUNT_MC_SHOWTYPEOFCARDDETAILS = "MyAccountDetails_lblShowTypeofCardDetails";
        public const string ACCOUNT_MC_ASSO_SHOWTYPEOFCARDDETAILS = "MyAccountDetails_lblShowTypeofCardDetailsasso";

        //Voucher Key
        public const string MYVOUCHER_CLICK = "MyVoucher_lblMyVoucher";
        public const string MYVOUCHER_SelectAll = "MyVoucher_chkSelectAll";
        public const string MYVOUCHER_SELECTONE = "MyVoucher_chkSelectOne";
        public const string MYVOUCHER_PrintVoucher = "MyVoucher_btnPrintVoucher";
        public const string MYVOUCHER_LBLUNSEDVOUCHER = "MyVoucher_lblUnUsedVoucher";
        public const string MYVOUCHER_SOCIALSITE = "MyVoucher_lblSocialSite";
        public const string MYVOUCHER_TWITTER = "MyVoucher_imgTwitter";
        public const string MYVOUCHER_FACEBOOK = "MyVoucher_imgTFaceBook";
        public const string MYVOUCHER_POINTBOX1 = "MyVoucher_pointBox1";
        public const string MYVOUCHER_POINTBOX1TEXT = "MyVoucher_lblpointBox1Text";
        public const string MYVOUCHER_POINTBOX2 = "MyVoucher_pointBox2";
        public const string MYVOUCHER_POINTBOX2TEXT = "MyVoucher_lblpointBox2Text";
        public const string MYVOUCHER_POINTBOX3 = "MyVoucher_pointBox3";
        public const string MYVOUCHER_POINTBOX3TEXT = "MyVoucher_lblpointBox3Text";
        public const string MYVOUCHER_TOTALONTOP = "MyVoucher_lblTotalOnTop";
        public const string MYVOUCHER_TOTALCOUNT = "MyVoucher_lblTotalCount";
        public const string MYVOUCHER_LBLUSEDVOUCHER = "MyVoucher_lblUsedVoucher";
        public const string MYVOUCHER_TABLEREDEEMED = "MyVoucher_tableRedeemed";
        public const string MYVOUCHER_REDEEMEDVALUE = "MyVoucher_txtRedeemedValue";
        public const string MYVOUCHER_TABLEUNUSED = "MyVoucher_tableUnused";
        public const string MYVOUCHER_DIVUSEDVOUCHERSECTION = "MyVoucher_divUsedVoucherSection";
        public const string MYVOUCHER_TXTPOINTCOLLECTED = "MyVoucher_txtPointsCollected";
        public const string MYVOUCHER_TXTCONVERTEDTO = "MyVoucher_txtConvertedTo";
        public const string MYVOUCHER_IMGARROW = "MyVoucher_imgArrow";
        public const string MYVOUCHER_IMGAVIOS = "MyVoucher_imgAvios";
        public const string MYVOUCHER_IMGVIRGINATLANTIC = "MyVoucher_imgVirginAtlantic";
        public const string MYVOUCHER_IMGBAAVIOS = "MyVoucher_imgVirginAtlantic";
        public const string MYVOUCHER_TOTALONTOP1 = "MyVoucher_lblTotalOnTop1";

        //Points Key
        public const string MYPOINTS_CLICK = "MyPoints_lblMyPoints";
        public const string MYCURRENTPOINTS_CLICK = "MyPoints_lblMyCurrentPoints";
        public const string MYCURRENTPOINTS_Details_CLICK = "MyPoints_lblMyCurrentPointsDetails";
        public const string MYCURRENTPOINTS_CurrentDetails_CLICK = "MyPoints_lnkCurrentDetails";
        public const string MYCURRENTPOINTS_Previous1Details_CLICK = "MyPoints_lnkPrevious1Details";
        public const string MYCURRENTPOINTS_Previous1Summary_CLICK = "MyPoints_lnkPrevious1Summary";
        public const string MYCURRENTPOINTS_Previous2Details_CLICK = "MyPoints_lnkPrevious2Details";
        public const string MYCURRENTPOINTS_Previous2Summary_CLICK = "MyPoints_lnkPrevious2Summary";
        public const string MYCURRENTPOINTS_COLLECTION_PERIOD_TABLE = "MyPoints_tblCollectionPeriodGrid";
        public const string MYCURRENTPOINTS_TARNSACTION_TABLE = "MyPoints_tblPointsTransactionGrid";
        public const string MYCURRENTPOINTS_SELECT_CLUBCARD = "MyPoints_selectClubcard";
        public const string MYCURRENTPOINTS_SELECT_STORE = "MyPoints_selectStore";

        //PointsSummary Key

        public const string POINTSSUMMARY_VOUCHERSTOTALLABEL = "PointsSummary_lblVouchersTotal";
        public const string POINTSSUMMARY_CARRIEDFORWARDLABEL = "PointsSummary_lblTotalCarriedForward";
        public const string POINTSSUMMARY_POINTSFROMTESCOLABELS = "PointsSummary_lbTotalPointsFromTesco";
        public const string POINTSSUMMARY_VOUCHERSTOTALTEXT = "PointsSummary_txtVouchersTotal";
        public const string POINTSSUMMARY_CARRIEDFORWARDTEXT = "PointsSummary_txtCarriedForwardTotal";
        public const string POINTSSUMMARY_POINTSFROMTESCOTEXTS = "PointsSummary_txtTotalPointsFromTesco";
        public const string POINTSSUMMARY_POINTSFROMTESCOSECTION = "PointsSummary_lblPointsFromTescoSection";
        public const string POINTSSUMMARY_POINTSFROMTESCOBANKSECTION = "PointsSummary_lblPointsFromTescoBankSection";
        public const string POINTSSUMMARY_POINTSFROMCHRISTMASSAVER = "PointsSummary_lblPointsForChristmasSaver";
        public const string POINTSSUMMARY_VOUCHERSFROMCHRISTMASSAVER = "PointsSummary_lblVouchersForChristmasSaver";
        public const string POINTSSUMMARY_VOUCHERVALUEFROMCHRISTMASSAVER = "PointsSummary_txtChristmasSaverVoucher";
        public const string POINTSSUMMARY_ALLVOUCHERSVALUEFROMCHRISTMASSAVER = "PointsSummary_txtChristmasSaverAllVouchers";
        public const string POINTSSUMMARY_MILESBOXLABEL = "PointsSummary_lblMilesBox";
        public const string POINTSSUMMARY_MILESBOXTEXT = "PointsSummary_txtMilesBox";
        public const string POINTSSUMMARY_LOWERLEFTLABELS = "PointsSummary_lblLowerLeftBox";
        public const string POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD = "PointsSummary_txtLowerLeftBoxWithBold";
        public const string POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD = "PointsSummary_txtLowerLeftBoxWithoutBold";
        public const string POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX = "PointsSummary_lblLowerLeftBoxWithTextBox";
        public const string POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX = "PointsSummary_txtLowerLeftBoxWithTextBox";
        public const string POINTSSUMMARY_TESCOPOINTSSECTION = "PointsSummary_sectionTescoPoints";
        public const string POINTSSUMMARY_TESCOBANKPOINTSSECTION = "PointsSummary_sectionTescoBankPoints";

        //Coupon Key
        public const string MYCOUPON_PrintButton = "MyCoupon_btnPrintCoupon";
        public const string MYCOUPON_CouponImages = "MyCoupon_CouponImages";
        public const string MYCOUPON_PrintIcon = "MyCoupon_iconPrintCoupon";
        public const string MYCOUPON_SelectCouponMessage = "MyCoupon_SelectCouponMessage";
        public const string MYCOUPON_SelectCoupon = "MyCoupon_chkSelectFirstCoupon";
        public const string MYCOUPON_SelectAllCoupon = "MyCoupon_chkSelectAllCoupon";
        public const string MYCOUPON_NOACTIVECOUPON = "MyCoupon_NoActiveCoupon";
        public const string MYCOUPON_NOCOUPONREDEEMED = "MyCoupon_NoCouponRedeemed";
        public const string MYCOUPON_STORENAME = "MyCoupon_StoreName";
        public const string MYCOUPON_DATE = "MyCoupon_DateRedeemed";
        public const string MYCOUPON_AVAILABLETEXT = "MyCoupon_AvailableText";
        public const string MYCOUPON_AVAILABLECOUNT = "MyCoupon_AvailableCount";
        public const string MYCOUPON_USEDSUMMARY = "MyCoupon_CouponUsedCountSummary";
        public const string MYCOUPON_USEDSUMMARYTEXT = "MyCoupon_CouponusedtextSummary";
        public const string MYCOUPON_SENTSUMMARY = "MyCoupon_CouponSentCountSummary";
        public const string MYCOUPON_SENTSUMMARYTEXT = "MyCoupon_CouponSenttextSummary";


        //My Latest Statement

        //Login page
        public const string HOMEPAGE_MYACCOUNT = "LoginControls_btnSubmit";

        //  //Join page keys.
        public const string JOIN_JOINCLUBCARD = "JoinControls_JoinClubcard";
        public const string JOIN_YOURDETAILS = "JoinControls_Yourdetails";
        public const string JOIN_BTNCONFIRM = "JoinControls_btnJoin";
        public const string JOIN_FIRSTNAME = "JoinControls_txtFirstname";
        public const string JOIN_MIDDLENAME = "JoinControls_txtmiddlename";
        public const string JOIN_SURNAME = "JoinControls_txtsurname";
        public const string JOIN_BTNRADIOMALE = "JoinControls_btnmaleradio";
        public const string JOIN_TXTPOSTCODE = "JoinControls_txtpostcode";
        public const string JOIN_BTNPOSTCODE = "JoinControls_btnpostcode";
        public const string JOIN_EMAIL = "JoinControls_txtemail";
        public const string JOIN_HOUSENUMBER = "JoinControls_txthousenumber";
        public const string JOIN_EVENINGNUMBER = "JoinControls_txteveningphone";
        public const string JOIN_MSGCONFIRM = "JoinControls_msgconfirm";
        public const string JOIN_IMGALLDONE = "JoinControls_imgalldone";
        public const string JOIN_DAY = "JoinControls_day";
        public const string JOIN_MONTH = "JoinControls_month";
        public const string JOIN_YEAR = "JoinControls_year";
        public const string JOIN_TITLE = "JoinControls_title";
        public const string JOIN_ADDRESSLINE1 = "JoinControls_addressline1";
        public const string JOIN_ADDRESSLINE2 = "JoinControls_addressline2";
        public const string JOIN_ADDRESSLINE4 = "JoinControls_addressline4";
        public const string JOIN_ADDRESSLINE5 = "JoinControls_addressline5";
        public const string JOIN_PHONENUMBER = "JoinControls_phonenumber";
        public const string JOIN_MOBILENUMBER = "JoinControls_mobilenumber";
        public const string JOIN_TERMSANDCONDITION = "JoinControls_chktermsAndCondition";
        public const string JOIN_PRIVACYPOLICY = "JoinControls_chkPrivacyPolicy";
        public const string JOIN_THANKYOUMSG = "JoinControls_thankyoumsg";
        public const string JOIN_CONFTEXT = "JoinControls_confText";
        public const string JOIN_PDFDOWNLOAD = "JoinControls_clickHeretodownloadpdf";
        public const string JOIN_TXTPROMOCODE = "JoinControls_txtPromoCode";
        public const string JOIN_ERRORPROMOCODE = "JoinControls_errorPromoCode";
        public const string JOIN_HOUSEDROPDOWN = "JoinControls_houseddl";

        //Join Error Messages
        public const string JOIN_ERRORNAME1 = "JoinControls_errorName1";
        public const string JOIN_ERRORNAME2 = "JoinControls_errorName2";
        public const string JOIN_ERRORNAME3 = "JoinControls_errorName3";
        public const string JOIN_PLEASECORRECTINFO = "JoinControls_PleaseCorrectInfo";
        public const string JOIN_ERRORGENDER = "JoinControls_errorGender";
        public const string JOIN_ERRORDOB = "JoinControls_errorDOB";
        public const string JOIN_ERRORPOSTCODE = "JoinControls_errorPostCode";
        public const string JOIN_ERRORADDRESSUK = "JoinControls_errorAddressUK";
        public const string JOIN_ERRORADDRESSLINE1 = "JoinControls_errorAddressLine1";
        public const string JOIN_ERRORADDRESSLINE2 = "JoinControls_errorAddressLine2";
        public const string JOIN_ERRORADDRESSLINE4 = "JoinControls_errorAddressLine4";
        public const string JOIN_ERRORADDRESSLINE5 = "JoinControls_errorAddressLine5";
        public const string JOIN_ERROREMAIL = "JoinControls_errorEmail";
        public const string JOIN_ERRORPRIVACY = "JoinControls_errorPrivacy";
        public const string JOIN_ERRORPROFANE = "JoinControls_errorProfane";
        public const string JOIN_ERRORMOBILENUMBR = "JoinControls_errorMobilePhoneNumber";
        public const string JOIN_ERROREVNGNUMBR = "JoinControls_errorDayTimePhoneNumber";
        public const string JOIN_ERRORDAYNUMBR = "JoinControls_errorDayTimePhoneNumber";
        public const string JOIN_ERRORTITLE = "JoinControls_errorTitle";
        public const string JOIN_LBLYOU = "JoinControls_lblYou";
        public const string JOIN_LBLPERSON2 = "JoinControls_lblPerson2";
        public const string JOIN_LBLPERSON3 = "JoinControls_lblPerson3";
        public const string JOIN_LBLPERSON4 = "JoinControls_lblPerson4";
        public const string JOIN_LBLPERSON5 = "JoinControls_lblPerson5";
        public const string JOIN_LBLPERSON6 = "JoinControls_lblPerson6";
        public const string JOIN_TXTHOUSEHOLD = "JoinControls_txtHousehold";
        public const string JOIN_TXTOPTIONALINFO = "JoinControls_txtOptionalInfo";
        public const string JOIN_TXTYEAROPTIONALINFO = "JoinControls_txtYearInOptionalInfo";
        public const string JOIN_DDLPERSON2AGE = "JoinControls_ddlPerson2Age";

        //Join contact Preference keys
        public const string JOIN_LBLTESCOPRODUCTS = "JoinControls_joinliTescoproducts";
        public const string JOIN_LBLGRPTESCOPRODUCTS = "JoinControls_joinliGrpTescoproducts";
        public const string JOIN_LBLTESCOOFFER = "JoinControls_joinliTescoOffer";
        public const string JOIN_LBLGRPTESCOOFFER = "JoinControls_joinliGrpTescoOffer";
        public const string JOIN_LBLTESCORESEARCH = "JoinControls_joinliTescoCustomerReasearch";
        public const string JOIN_LBLGRPTESCORESEARCH = "JoinControls_joinliGrpTescoCustomerReasearch";
        public const string JOIN_CHKTESCOOFFER = "JoinControls_joinChkTescoOffers";
        public const string JOIN_CHKGROUPTESCOOFFER = "JoinControls_joinChkGrpTescoProducts";
        public const string JOIN_CHKPARTNEROFFER = "JoinControls_joinChkPartnerOffers";
        public const string JOIN_CHKGROUPPARTNEROFFER = "JoinControls_joinChkGrpPartnerOffers";
        public const string JOIN_CHKRESEARCH = "JoinControls_joinChkResearch";
        public const string JOIN_CHKGROUPRESEARCH = "JoinControls_joinChkGrpResearch";
        public const string JOIN_CHKTGMAIL = "JoinControls_joinChkTGMail";
        public const string JOIN_CHKTGEMAIL = "JoinControls_joinChkTGEmail";
        public const string JOIN_CHKTGPHONE = "JoinControls_joinChkTGPhone";
        public const string JOIN_CHKTGSMS = "JoinControls_joinChkTGSms";
        public const string JOIN_CHKTPMAIL = "JoinControls_joinChkTPMail";
        public const string JOIN_CHKTPEMAIL = "JoinControls_joinChkTPEmail";
        public const string JOIN_CHKTPPHONE = "JoinControls_joinChkTPPhone";
        public const string JOIN_CHKTPSMS = "JoinControls_joinChkTPSms";
        public const string JOIN_CHKRMAIL = "JoinControls_joinChkRMail";
        public const string JOIN_CHKREMAIL = "JoinControls_joinChkREmail";
        public const string JOIN_CHKRPHONE = "JoinControls_joinChkRPhone";
        public const string JOIN_CHKRSMS = "JoinControls_joinChkRSms";
        public const string JOIN_LBLGRIDTESCOPRODUCTS = "JoinControls_jointdGrpTescoproducts";
        public const string JOIN_LBLGRIDTESCOPARTNER = "JoinControls_jointdGrpTescopartner";
        public const string JOIN_LBLGRIDTESCORESEARCH = "JoinControls_jointdGrpTescoresearch";

        //Activation page Keys.
        public const string ACTIVATION_TEXT = "ActivationControls_btnActivation";
        public const string ACTIVATION_LABEL = "ActivationControls_lblConfirmyourClubcarddetails";
        public const string ACTIVATION_CLUBCARDNUMBER = "ActivationControls_txtClubcardno";
        public const string ACTIVATION_FIRSTNAME = "ActivationControls_txtFirstName";
        public const string ACTIVATION_SURNAME = "ActivationControls_txtSurname";
        public const string ACTIVATION_POSTCODE = "ActivationControls_txtPostcode";
        public const string ACTIVATION_CONFIRMBUTTON = "ActivationControls_btnConfirm";
        public const string ACTIVATION_SUCCESSMSG = "ActivationControls_lblActivationSuccessMsg";
        public const string ACTIVATION_ERRORMSG1 = "ActivationControls_lblActivationErrorMsg1";
        public const string ACTIVATION_SUCCESSCONFIRMMSG = "ActivationControls_lblActivationConfirmMsg";
        public const string ACTIVATION_ERRORMSG2 = "ActivationControls_lblActivationErrorMsg2";
        public const string ACTIVATION_CLUBCARDERRORMSG = "ActivationControls_lblClubCardErrorMsg";
        public const string ACTIVATION_FIRSTNAMEERRORMSG = "ActivationControls_lblFirstNameErrorMsg";
        public const string ACTIVATION_SURNAMEERRORMSG = "ActivationControls_lblSurnameErrorMsg";
        public const string ACTIVATION_POSTCODEERRORMSG = "ActivationControls_lblPostCodeErrorMsg";
        public const string ACTIVATION_LBLFIRSTNAME = "ActivationControls_lblfirstname";
        public const string ACTIVATION_LBLSURNAME = "ActivationControls_lblSurname";
        public const string ACTIVATION_LBLDOB = "ActivationControls_lblDOB";
        public const string ACTIVATION_LBLMOB = "ActivationControls_lblMOB";
        public const string ACTIVATION_LBLYOB = "ActivationControls_lblYOB";
        public const string ACTIVATION_LBLADDRESSLINE1 = "ActivationControls_lblAddressLine1";
        public const string ACTIVATION_LBLPOSTCODE = "ActivationControls_lblPostcode";
        public const string ACTIVATION_LBLMOBILENUMBER = "ActivationControls_lblPMobileNumber";
        public const string ACTIVATION_LBLSSN = "ActivationControls_lblSSN";
        public const string ACTIVATION_LBLEMAIL = "ActivationControls_lblEmailAddress";
        public const string ACTIVATION_EMAIL = "ActivationControls_txtemail";
        public const string ACTIVATION_PASSWORD = "ActivationControls_txtpassword";
        public const string ACTIVATION_REPASSWORD = "ActivationControls_txtrepassword";
        public const string ACTIVATION_REGISTER = "ActivationControls_txtregister";

        //CSC Login And Logout Keys
        public const string CSCLOGIN_USERID = "CSCLoginLogoutControls_txtUserId";
        public const string CSCLOGIN_PASSWORD = "CSCLoginLogoutControls_txtPassword";
        public const string CSCLOGIN_DOMAIN = "CSCLoginLogoutControls_ddlSelectDomain";
        public const string CSCLOGIN_SUBMIT = "CSCLoginLogoutControls_btnSignIn";
        public const string CSCLOGOUT = "CSCLoginLogoutControls_btnLogOut";

        //CSC Controls for Customer Search
        public const string CSC_CUSTOMERSEARCH_CLUBCARD = "CSCSearchCustomerControls_txtCardNumber";
        public const string CSC_CUSTOMERSEARCH_SEARCHCUSTOMER = "CSCSearchCustomerControls_btnFindCustomer";

        //CSC Controls for Customer Details
        public const string CSC_CUSTOMERDETAILS_FIRSTNAME = "CSCCustomerDetailsControls_txtFirstName";
        public const string CSC_CUSTOMERDETAILS_SURNAME = "CSCCustomerDetailsControls_txtSurname";
        public const string CSC_CUSTOMERDETAILS_POSTCODE = "CSCCustomerDetailsControls_txtPostCode";

        //CSC Controls for Delink Account
        public const string CSCDELINK_CLICK = "CSCDelinkAccountControls_lnkDelinkAcc";
        public const string CSC_DELINKACC = "CSCDelinkAccountControls_lnkDelinkCustomerAcc";


        //stamps
        public const string STAMP1 = "HomePage_Stamp1";
        public const string STAMP2 = "HomePage_Stamp2";
        public const string STAMP3 = "HomePage_Stamp3";
        public const string STAMP4 = "HomePage_Stamp4";
        public const string STAMP5 = "HomePage_Stamp5";
        public const string STAMP6 = "HomePage_Stamp6";
        public const string STAMP7 = "HomePage_Stamp7";
        public const string STAMP8 = "HomePage_Stamp8";
        public const string STAMP9 = "HomePage_Stamp9";
        public const string STAMP = "HomePage_Stamp";

        //My contact Preference keys
        public const string VALIDATION_MESSAGE = "MyContactPreference_ValidationMsg";
        public const string BUTTON_CLICK = "MyContactPreference_BtnConfirm";
        public const string RADIOBUTTON_EMAIL = "MyContactPreference_RadioBtnEmail";
        public const string EMAILTEXT_VARIFICATION = "MyContactPreference_EmailTextValue";
        public const string RADIOBUTTON_SMS = "MyContactPreference_RadioBtnSMS";
        public const string PHONENUMBER_VARIFICATION = "MyContactPreference_PhoneNumberValue";
        public const string LABLEONE_VARIFICATION = "MyContactPreference_lblContactPreferenceOne";
        public const string LABLETWO_VARIFICATION = "MyContactPreference_lblContactPreferenceTwo";
        public const string GRIDTABLE = "MyContactPreference_gdtable";
        public const string LBLVALIDATION_MSG = "MyContactPreference_lblValidationMsg";
        public const string CHECK_BOX1_PR = "MyContactPreference_chekBox1";
        public const string LIST_CHECKBOXES_PR = "MyContactPreference_lstCheckBox";
        public const string BTNPREFSUBMIT = "MyContactPreference_btnConfirmMailingPref";
        public const string PREFSUCCESSMSG = "MyContactPreference_prefSuccessMsg";
        public const string CHKBOXEMAIL = "MyContactPreference_chkboxEmail";
        public const string CHKBOXSMS = "MyContactPreference_chkboxSMS";
        public const string CHKTESCOPRODUCT = "MyContactPreference_chkTescoProduct";
        public const string CHKPARTNEROFFER = "MyContactPreference_chkPartneroffer";
        public const string CHKRESEARCH = "MyContactPreference_chkResearch";
        public const string CHKGROUPTESCOPRODUCT = "MyContactPreference_chkGrpTescoProduct";
        public const string CHKGROUPPARTNEROFFER = "MyContactPreference_chkGrpPartneroffer";
        public const string CHKGROUPRESEARCH = "MyContactPreference_chkGrpResearch";
        public const string NOGRID = "MyContactPreference_divNoGrid";


        //Options and benifit keys
        public const string Button_Clear = "OptionsAndBenefit_btnClearSection";
        //public const string Button_Confirm = "OptionsAndBenefit_btnConfirm";
        public const string Lable_Christmasmessage = "OptionsAndBenefit_lblChristmasmessage";

        public const string RADIOBUTTON_CHISTMAS = "OptionsAndBenefit_radioBtnChistmas";
        //public const string RADIOBUTTON_AVIOUS = "OptionsAndBenefit_radioBtnAvios";
        public const string RADIOBUTTON_VIRGINATLANTICFLYINGCLUB = "OptionsAndBenefit_radioBtnVirginAtlanticFlyingClub";
        public const string TEXTBOX_VIRGINMEMBERSHIPID = "OptionsAndBenefit_txtVirgnMembershipID";
        public const string TEXTBOX_BAVIOSID = "OptionsAndBenefit_txtBAviosID";

        public const string RADIOBUTTON_BRITISHAIRWAYSEXECUTIVECLUB_AVIOS = "OptionsAndBenefit_radioBtnBritishAirwaysExecutiveAvios";

        public const string OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE = "OptionsAndBenefit_lblAviousValidateMessage";
        public const string OPTIONSBENEFIT_BAVALIDATEMESSAGE = "OptionsAndBenefit_lblBAValidateMessage";


        //ctl00_ctl00_PageContainer_MyAccountContainer_spanVirgin
        public const string OPTIONSBENEFIT_ConfirmButton = "OptionsAndBenefit_btnConfirm";
        public const string OPTIONSBENEFIT_Message = "OptionsAndBenefit_lblmessage";
        public const string OPTIONSBENEFIT_BAAVIOSRADIOBTN = "OptionsAndBenefit_radioBtnBAAvios";
        public const string OPTIONSBENEFIT_AVIOSRADIOBTN = "OptionsAndBenefit_radioBtnAvios";


        public const string OPTIONSBENEFIT_LINKVIRGINATLANTICINFORMATION = "OptionsAndBenefit_lnkVirginAtlanticFlyingClubMoreInformation";

        public const string OPTIONSBENEFIT_LINKBAAVIOS = "OptionsAndBenefit_lnkBAAVIOSMoreInformation";
        public const string OPTIONSBENEFIT_LINKAVIOS = "OptionsAndBenefit_lnkAVIOSMoreInformation";
        public const string OPTIONSBENEFIT_LINKCHISTMAS = "OptionsAndBenefit_lnkChistmasSMoreInformation";


        // Personal Details keys
        public const string PERSONALDETAILS_Message = "MyPersonalDetails_lblmessage";
        public const string PERSONALDETAILS_ConfirmButton = "MyPersonalDetails_btnConfirm";
        public const string PERSONALDETAILS_ConfirmButtonDietaryDisabled = "MyPersonalDetails_btnConfirmDietaryDisabled";
        public const string PERSONALDETAILS_TXTPOSTCODE = "MyPersonalDetails_txtpostcode";
        public const string PERSONALDETAILS_BTNPOSTCODE = "MyPersonalDetails_btnpostcode";
        public const string PERSONALDETAILS_HOUSEDROPDOWN = "MyPersonalDetails_ddlhouse";
        public const string PERSONALDETAILS_HOUSENUMBER = "MyPersonalDetails_Housenumber";
        public const string PERSONALDETAILS_POSTCODESEC1 = "MyPersonalDetails_lblpostcodesec1";
        public const string PERSONALDETAILS_POSTCODESEC2 = "MyPersonalDetails_lblpostcodesec2";
        public const string PERSONALDETAILS_BTNSAVECHANGES = "MyPersonalDetails_btnSaveChanges";
        public const string PERSONALDETAILS_LBLSUCESSFULMSG = "MyPersonalDetails_lblSuccessfulMsg";
        public const string PERSONALDETAILS_EMAIL = "MyPersonalDetails_txtemail";
        public const string PERSONALDETAILS_FIRSTNAME = "MyPersonalDetails_txtFirstname";
        public const string PERSONALDETAILS_MIDDLENAME = "MyPersonalDetails_txtmiddlename";
        public const string PERSONALDETAILS_SURNAME = "MyPersonalDetails_txtsurname";
        public const string PERSONALDETAILS_EVENINGNUMBER = "MyPersonalDetails_txteveningphone";
        public const string PERSONALDETAILS_MOBILENUMBER = "MyPersonalDetails_mobilenumber";
        public const string PERSONALDETAILS_PHONENUMBER = "MyPersonalDetails_phonenumber";
        public const string PERSONALDETAILS_BTNRADIOMALE = "MyPersonalDetails_btnmaleradio";
        public const string PERSONALDETAILS_BTNRADIOFEMALE = "MyPersonalDetails_btnfemaleradio";
        public const string PERSONALDETAILS_DAY = "MyPersonalDetails_day";
        public const string PERSONALDETAILS_MONTH = "MyPersonalDetails_month";
        public const string PERSONALDETAILS_YEAR = "MyPersonalDetails_year";
        public const string PERSONALDETAILS_TITLE = "MyPersonalDetails_title";

        public const string PERSONALDETAILS_PAGEDESCTXTONE = "MyPersonalDetails_PageDescTxtone";
        public const string PERSONALDETAILS_PAGEDESCTXTTWO = "MyPersonalDetails_PageDescTxtTwo";
        public const string PERSONALDETAILS_PAGEDESCTXTTHREE = "MyPersonalDetails_PageDescTxtThree";
        public const string PERSONALDETAILS_CheckText = "MyPersonalDetails_CheckText";
        public const string PERSONALDETAILS_YCDETAILSTXT = "MyPersonalDetails_YCDetailsText";
        public const string PERSONALDETAILS_REQUIREDWARNINGTXT = "MyPersonalDetails_AstricTxt";

        public const string PERSONALDETAILS_TESCODOTCOMLINKTEXT = "MyPersonalDetails_tescodotcomaccount";
        public const string PERSONALDETAILS_CHANGEPASSWORDLINK = "MyPersonalDetails_passwordChange";
        public const string PERSONALDETAILS_SURNAMEUPDATECLICKHERE = "MyPersonalDetails_surnameupdateClickhere";
        public const string PERSONALDETAILS_SURNAMEUPDATETEXT = "MyPersonalDetails_surnameupdateText";
        public const string PERSONALDETAILS_HouseholdAgeTextYou = "MyPersonalDetails_YHDAgeYou";
        public const string PERSONALDETAILS_HouseholdAgePerson2 = "MyPersonalDetails_YHDAgePerson2";
        public const string PERSONALDETAILS_HouseholdAgePerson3 = "MyPersonalDetails_YHDAgePerson3";
        public const string PERSONALDETAILS_HouseholdAgePerson4 = "MyPersonalDetails_YHDAgePerson4";
        public const string PERSONALDETAILS_HouseholdAgePerson5 = "MyPersonalDetails_YHDAgePerson5";
        public const string PERSONALDETAILS_HouseholdAgePerson6 = "MyPersonalDetails_YHDAgePerson6";
        public const string PERSONALDETAILS_HouseholdAgeTextYouValue = "MyPersonalDetails_YHDAgeYouValue";
        public const string PERSONALDETAILS_DDYearsPerson2 = "MyPersonalDetails_YHDDDPerson2";
        public const string PERSONALDETAILS_DDYearsPerson3 = "MyPersonalDetails_YHDDDPerson3";
        public const string PERSONALDETAILS_DDYearsPerson4 = "MyPersonalDetails_YHDDDPerson4";
        public const string PERSONALDETAILS_DDYearsPerson5 = "MyPersonalDetails_YHDDDPerson5";
        public const string PERSONALDETAILS_DDYearsPerson6 = "MyPersonalDetails_YHDDDPerson6";

        public const string PERSONALDETAILS_LBLDIETARYNEEDDIABETIC = "MyPersonalDetails_LblDietaryNeedDiabetic";
        public const string PERSONALDETAILS_LBLDIETARYNEEDKOSHER = "MyPersonalDetails_LblDietaryNeedKosher";
        public const string PERSONALDETAILS_LBLDIETARYNEEDHALAL = "MyPersonalDetails_LblDietaryNeedHalal";
        public const string PERSONALDETAILS_LBLDIETARYNEEDVEGETERIAN = "MyPersonalDetails_LblDietaryNeedVegeterian";
        public const string PERSONALDETAILS_LBLDIETARYNEEDTEATOTAL = "MyPersonalDetails_LblDietaryNeedTeaTotal";

        public const string PERSONALDETAILS_CHKDIETARYNEEDDIABETIC = "MyPersonalDetails_ChkDietaryNeedDiabetic";
        public const string PERSONALDETAILS_CHKDIETARYNEEDKOSHER = "MyPersonalDetails_ChkDietaryNeedKosher";
        public const string PERSONALDETAILS_CHKDIETARYNEEDHALAL = "MyPersonalDetails_ChkDietaryNeedHalal";
        public const string PERSONALDETAILS_CHKDIETARYNEEDVEGETERIAN = "MyPersonalDetails_ChkDietaryNeedVegeterian";
        public const string PERSONALDETAILS_CHKDIETARYNEEDTEATOTAL = "MyPersonalDetails_ChkDietaryNeedTeaTotal";
        public const string PERSONALDETAILS_CHKDIETARYNEED = "MyPersonalDetails_ChkDietaryNeeds";
        public const string PERSONALDETAILS_PROVINCE = "MyPersonalDetails_Province";
        public const string PERSONALDETAILS_RACE = "MyPersonalDetails_Race";
        public const string PERSONALDETAILS_ADDRESSLINE5 = "MyPersonalDetails_AddressLine5";
        public const string PERSONALDETAILS_ADDRESSLINE1 = "MyPersonalDetails_AddressLine1";
        public const string PERSONALDETAILS_ADDRESSLINE2 = "MyPersonalDetails_AddressLine2";
        public const string PERSONALDETAILS_ADDRESSLINE3 = "MyPersonalDetails_AddressLine3";
        public const string PERSONALDETAILS_ADDRESSLINE4 = "MyPersonalDetails_AddressLine4";
        public const string PERSONALDETAILS_ADDRESSLINE6 = "MyPersonalDetails_AddressLine6";
        public const string PERSONALDETAILS_PRIMARYID = "MyPersonalDetails_PrimaryID";
        public const string PERSONALDETAILS_REPLACEMENTTEXT = "MyPersonalDetails_ReplacementText";
        public const string PERSONALDETAILS_ERRORDOB = "MyPersonalDetails_ErrorDOB";
        public const string PERSONALDETAILS_GENERICERROR = "MyPersonalDetails_GenericError";
        public const string PERSONALDETAILS_ERRORNAME1 = "MyPersonalDetails_ErrorName1";
        public const string PERSONALDETAILS_ERRORNAME2 = "MyPersonalDetails_ErrorName2";
        public const string PERSONALDETAILS_ERRORNAME3 = "MyPersonalDetails_ErrorName3";
        public const string PERSONALDETAILS_ERRORMOBILENUMBER = "MyPersonalDetails_ErrorMobileNumber";
        public const string PERSONALDETAILS_DAYTIMENUMBER = "MyPersonalDetails_mobilenumber";
        public const string PERSONALDETAILS_ERROREMAIL = "MyPersonalDetails_ErrorEmail";
        public const string PERSONALDETAILS_ERRORGENDERMISMATCH = "MyPersonalDetails_ErrorGenderMismatch";
        public const string PERSONALDETAILS_ERRORADDRESSLINE1 = "MyPersonalDetails_ErrorAddressLine1";
        public const string PERSONALDETAILS_ERRORADDRESSLINE2 = "MyPersonalDetails_ErrorAddressLine2";
        public const string PERSONALDETAILS_ERRORADDRESSLINE3 = "MyPersonalDetails_ErrorAddressLine3";
        public const string PERSONALDETAILS_ERRORADDRESSLINE4 = "MyPersonalDetails_ErrorAddressLine4";
        public const string PERSONALDETAILS_ERRORADDRESSLINE5 = "MyPersonalDetails_ErrorAddressLine5";
        public const string PERSONALDETAILS_ERRORADDRESSLINE6 = "MyPersonalDetails_ErrorAddressLine6";

        


        //Order A Replacement Keys
        public const string ORDERREPLACEMENT_CLUBCARDNUMBER = "OrderAReplacementControls_lblCardNumber";
        public const string ORDERREPLACEMENT_REQUESTREASONERROR = "OrderAReplacementControls_lblRequestReasonError";
        public const string ORDERREPLACEMENT_DIVSTDNONSTDTEXT = "OrderAReplacementControls_lblDivStdNonStdText";
        public const string ORDERREPLACEMENT_CLUBCARDTYPEB = "OrderAReplacementControls_txtForCLubcardTypeB";
        public const string ORDERREPLACEMENT_CLUBCARDTYPEN = "OrderAReplacementControls_txtForCLubcardTypeN";
        public const string ORDERREPLACEMENT_MAXORDERSREACHED = "OrderAReplacementControls_lblMaxOrdersReached";
        public const string ORDERREPLACEMENT_REASONERRORMESSAGE = "OrderAReplacementControls_txtReasonErrorMessage";
        public const string ORDERREPLACEMENT_RADIOLOST = "OrderAReplacementControls_rdbtnLost";
        public const string ORDERREPLACEMENT_RADIODAMAGED = "OrderAReplacementControls_rdbtnDamaged";
        public const string ORDERREPLACEMENT_RADIOSTOLEN = "OrderAReplacementControls_rdbtnStolen";
        public const string ORDERREPLACEMENT_RADIOMOREFOBS = "OrderAReplacementControls_rdbtnMoreFobs";
        public const string ORDERREPLACEMENT_RADIOOTHER = "OrderAReplacementControls_rdbtnOther";
        public const string ORDERREPLACEMENT_BTNCONFIRM = "OrderAReplacementControls_btnConfirm";
        public const string ORDERREPLACEMENT_ORDERINPROCESSMSG = "OrderAReplacementControls_txtOrderInProcessMsg";
        public const string ORDERREPLACEMENT_CLUBCARDTYPEN_SK = "OrderAReplacementControls_txtForCLubcardTypeN_SK";






        //--Xmus saver
        public const string XMUSSAVER_YOUHAVESAVEDMSG = "XmusSaver_youHaveSavedMsg";
        public const string XMUSSAVER_SPNTTLTOPPEDUPMONEY = "XmusSaver_spnttlToppedUpMoney";
        public const string XMUSSAVER_SPNCCVOUCHERSSAVED = "XmusSaver_spnCCVouchersSaved";
        public const string XMUSSAVER_DVMONNEYTOPPEDUP = "XmusSaver_dvMoneyToppedUp";
        public const string XMUSSAVER_SPNBONUSVOUCHER = "XmusSaver_spnBonusVoucher";
        public const string XMUSSAVER_SPNTTLVOUCHERSSOFAR = "XmusSaver_spnTtlVouchersSoFar";
        public const string XMUSSAVER_SPNYEAR1 = "XmusSaver_spnYear1";
        public const string XMUSSAVER_SPNTTLPNTS = "XmusSaver_spnTtlPnts";

        public const string XMUSSAVER_SPNBONUSVALUEFOR50 = "XmusSaver_spnBonusValueFor50";
        public const string XMUSSAVER_SPNMONEYTOBESAVEDFORBONUS6 = "XmusSaver_spnMoneyTobeSavedForBonus6";
        public const string XMUSSAVER_SPNBONUSVALUEFOR100_1 = "XmusSaver_spnBonusValueFor100AND1";
        public const string XMUSSAVER_SPNBONUSVALUEFOR100 = "XmusSaver_spnBonusValueFor100";

    }
}
