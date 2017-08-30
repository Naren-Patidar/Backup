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
        public const string LEFT_NAVIGATION = "CommonControls_leftNavigation";
        public const string VALIDATION_ERRORS = "CommonControls_ValidationErrors";
        public const string FIELDSET_HOUSEHOLD_DETAILS = "CommonControls_FieldSetHouseholdDetails";
        public const string HOUSEHOLDINFO = "CommonControls_HouseholdInfo";
        public const string ENTERAGE = "CommonControls_EnterAge";
        public const string CHECKBOX = "CommonControls_CheckBox";
		
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
        public const string SECURITY_MAXATTEMPTMSG= "SecurityControls_maxAttemptMsg";
        public const string SECURITY_SECDETAILSNOTMATCHMSG = "SecurityControls_secDetailsNotMatchMsg";
        public const string SECURITY_LOGOUT = "SecurityControls_btnSecurityLogout";
        public const string SECURITY_LNKORDERREPLACEMENT = "SecurityControls_lnkOrderReplacement";
        public const string SECURITY_MAXATTEMPTMSG1 = "SecurityControls_maxAttemptMsg1";
        public const string SECURITY_MAXATTEMPTMSG2 = "SecurityControls_maxAttemptMsg2";
        public const string SECURITY_MAXATTEMPTMSG3 = "SecurityControls_maxAttemptMsg3";
        public const string SECURITY_ACCOUNTVERIFY = "SecurityControls_lblAccountVerify";
        public const string SECURITY_PAGEHEAD = "SecurityControls_lblPageHead";
        public const string SECURITY_HEADERMSG = "SecurityControls_lblHeaderMsg";

            
            
        //DBT Login Keys
        public const string LOGIN_TEXTVERIFICATION = "LoginControls_txtVerification";
        public const string DBTLOGIN_CLUBCARD = "LoginControls_btnMCAloginDbt";
        public const string DBTLOGIN_PASSWORD = "LoginControls_btnMCAPassDbt";
        public const string DBTLOGIN_SUBMIT = "LoginControls_btnSubmit";
        public const string DBTLOGIN_DOTCOMID = "LoginControls_btnMCADotcomDbt";

        //PPE Login Keys 
        public const string PPELOGINUK_EMAIL = "LoginControls_btnUKMCAEmailPpe";
        public const string PPELOGINUK_PASSWORD = "LoginControls_btnUKMCAPassPpe";
        public const string PPELOGINUK_SIGNIN = "LoginControls_btnSignInUK";
        public const string PPELOGIN_EMAIL = "LoginControls_btnMCAEmailPpe";
        public const string PPELOGIN_PASSWORD = "LoginControls_btnMCAPassPpe";
        public const string PPELOGIN_SIGNIN = "LoginControls_btnSignIn";

        public const string UKPPELOGIN_EMAIL = "LoginControls_btnUKMCAEmailPpe";
        public const string UKPPELOGIN_PASSWORD = "LoginControls_btnUKMCAPassPpe";
        public const string UKPPELOGIN_SIGNIN = "LoginControls_btnUKSignIn";



        public const string PRINTTEMPORARY = "LoginControls_btnTemp";

        //DBT Logout Keys
        public const string DBTLOGOUT = "LogOutControls_btnMCAlogOutDbt";
        public const string MCALOGOUT = "LogOutControls_btnMCAlogOut";
        public const string SIGNOUTCOMPLETELY = "LogOutControls_chkSignoutCompletely";
        public const string DOTCOMLOGOUT = "LogOutControls_btnDotComLogout";

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
        public const string HOME_ViewVouchersHome = "HomePage_ViewVouchersHome";
        public const string HOME_SECONDPOINTSBOX_VALUE = "HomePage_SecondPointsBoxValue";
        public const string HOME_CHRISTMAS_SAVER = "HomePage_ChristmasSaver";
        public const string HOME_WELCOME_MSGNAME = "HomePage_lblWelcomeMessageName";
        public const string HOME_POINTSMSG = "HomePage_lblPointsMsg";
        public const string HOME_PREFMSG = "HomePage_lblPrefMsg";
        public const string HOME_ADDRESSMSG = "HomePage_lblAddressErr";

        public const string HOME_BANNER = "HomePage_Banner";
        public const string HOME_BANNER_POSTCODE = "HomePage_BannerPostcode";
        public const string HOME_BANNER_SURNAME = "HomePage_BannerSurname";
        public const string HOME_BANNER_YES = "HomePage_BannerYes";
        public const string HOME_BANNER_NO = "HomePage_BannerNo";
        public const string HOME_BANNER_TescoDotcomBannerYes = "HomePage_TescoDotcomBannerYes";
        public const string HOME_BANNER_TescoDotcomBannerDetailsNotCorrect = "HomePage_TescoDotcomBannerDetailsNotCorrect";
        
        public const string HOME_BANNER_ON_PD = "HomePage_BannerOnPersonalDetails";
        public const string HOME_VOUCHER_LINK = "HomePage_VouchersLink";

        public const string HOME_SECONDARYBOX = "HomePage_SecondaryBox";

        public const string HOME_COUPONBANNER = "HomePage_CouponBanner";
        public const string HOME_COUPONBANNERCOUNT = "HomePage_CouponBannerCount";
        public const string HOME_HISTORICALVOUCHERS = "HomePage_HomeHistoricalVoucher";
        public const string HOME_HISTORICALVOUCHERSLEFT = "HomePage_HistoricalVoucherLeft";



        
            
            
        //Header Keys

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
        public const string HEADER_TESCOCOM = "HeaderControls_lblTescoSite";
        public const string HEADER_HREFTESCOCOM = "HeaderControls_hrefTescoSite";
        public const string HEADER_SIGNOUTKEY = "HeaderControls_lblSignout";
        public const string HEADER_STORELOCATOR= "HeaderControls_lblStoreLocator";
        public const string HEADER_WEBSITEFEEDBACK = "HeaderControls_lblWebsiteFeedback";
        public const string HEADER_SEARCHTEXTBOX = "HeaderControls_txtSearch";
        public const string HEADER_SEARCHBUTTON = "HeaderControls_btnSearch";
        public const string HEADER_PRIMARYNAV = "HeaderControls_navPrimary";
        public const string HEADER_SPENDVOUCHER= "HeaderControls_lblSpendVoucher";
        public const string HEADER_SPENDVOUCHERNAV = "HeaderControls_navSpendVoucher";
        public const string HEADER_FUNNAV = "HeaderControls_navFun";
        public const string HEADER_EATINGOUTNAV = "HeaderControls_navEatingOut";
        public const string HEADER_TRAVELNAV = "HeaderControls_navTravel";
        public const string HEADER_HOMENAV = "HeaderControls_navHome";
        public const string HEADER_LIFESTYLENAV = "HeaderControls_navLifestyle";
        public const string HEADER_4XNAV = "HeaderControls_nav4xValue";
       
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
        public const string ACCOUNT_MC_WHEREUSEDWITHTYPEOFCARD = "MyAccountDetails_lblWhereUsedwithTypeofCard";
        public const string ACCOUNT_MC_ASSOLASTUSED = "MyAccountDetails_lblLastUsedAsso";
        public const string ACCOUNT_MC_ASSOWHEREUSEDWITHTYPEOFCARD = "MyAccountDetails_lblWhereUsedAssowithTypeofCard";
        public const string ACCOUNT_MC_SHOWTYPEOFCARDDETAILS = "MyAccountDetails_lblShowTypeofCardDetails";
        public const string ACCOUNT_MC_ASSO_SHOWTYPEOFCARDDETAILS = "MyAccountDetails_lblShowTypeofCardDetailsasso";
        public const string ACCOUNT_MC_TextStatement1 = "MyAccountDetails_Statement1";
        public const string ACCOUNT_MC_TextStatement2 = "MyAccountDetails_Statement2";
        public const string ACCOUNT_MC_TextStatement3 = "MyAccountDetails_Statement3";
        public const string ACCOUNT_MC_TextStatement4 = "MyAccountDetails_Statement4";

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
        public const string MYVOUCHER_TEXTLCLVIEWSUMMARY = "MyVoucher_lclViewSummary";
        public const string MYVOUCHER_LCLVOUCHERSUMHEADER = "MyVoucher_lclVoucherSumHeader";
        public const string MYVOUCHER_LCLFBTWTHEADER = "MyVoucher_lclFBtwtHeader";
        public const string MYVOUCHER_LCLTELLFRIENDS = "MyVoucher_LCLTELLFRIENDS";
        public const string MYVOUCHER_LCLFBTWTSHAREMESSAGE = "MyVoucher_lclFBtwtShareMessage";
        public const string MYVOUCHER_LCLUNUSEDVOUCHERSHEADER = "MyVoucher_lclUnUsedVouchersHeader";
        public const string MYVOUCHER_LCLVOUCHERSUSEDONCE = "MyVoucher_lclVouchersUsedOnce";
        public const string MYVOUCHER_LCLCURRENTUNSPENDVOUCHERS = "MyVoucher_lclCurrentUnspendVouchers";
        public const string MYVOUCHER_LCLFOUNDBELOW = "MyVoucher_lclFoundBelow";
        public const string MYVOUCHER_LCLASPENTRESOURCE = "MyVoucher_lclAspentResource";
        public const string MYVOUCHER_LCLDOTCOMRESOURCE = "MyVoucher_lclDotcomResource";
        public const string MYVOUCHER_LCLCPYVOUCHERCODERESOURCE = "MyVoucher_lclcpyVoucherCodeResource";
        public const string MYVOUCHER_LCLCHECKOUTRESOURCE = "MyVoucher_lclCheckoutResource";
        public const string MYVOUCHER_LCLPRINTVOUCHERRESOURCE = "MyVoucher_lclPrintVoucherResource";
        public const string MYVOUCHER_LCLSELECTVOUCHERRESOURCE = "MyVoucher_lclSelectVoucherResource";
        public const string MYVOUCHER_LCLCLICKPRINTRESOURCE = "MyVoucher_lclClickPrintResource";
        public const string MYVOUCHER_LCLPRINTRESOURCE = "MyVoucher_lclPrintResource";
        public const string MYVOUCHER_LCLTILLRESOURCE = "MyVoucher_lclTillResource";
        public const string MYVOUCHER_LCLREDEMVOUCHERRESOURCE = "MyVoucher_lclRedemVoucherResource";
        public const string MYVOUCHER_LCLEXPDATERESOURCE = "MyVoucher_lclExpDateResource";
        public const string MYVOUCHER_LBLCURSYMS = "MyVoucher_lblTotalOnTop";
        public const string MYVOUCHER_TOTALINACTIVEGRID = "MyVoucher_lblTotalInActiveGrid";
        public const string MYVOUCHER_VOUCHERTOTAL = "MyVoucher_lblVoucherTotal";
        //MY Points Key
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
        public const string MYCURRENTPOINTS_HEADER = "MyPoints_h1CurrentPointsDetails";
        public const string MYPOINTS_HEADER = "MyPoints_h2MyPoints";
        public const string MYPOINTS_TD_TRAN_DATE = "MyPoints_tdltrtxndatetime";
        public const string MYPOINTS_TD_TRAN_DETAILS = "MyPoints_tdltrtxnDesc";
        public const string MYPOINTS_TD_TOTAL_POINTS = "MyPoints_tdltrttlptls";
        public const string MYPOINTS_TD_AMOUNT_SPENT = "MyPoints_tdltrAmountSpent";
        public const string MYPOINTS_DIV_TRANSACTIONS = "MyPoints_divTransactions";
        public const string MYPOINTS_ViewVouchers = "MyPoints_ViewVouchers";
        public const string MYPOINTS_ViewVouchersPointsSummary = "MyPoints_ViewVouchersPointsSummary";
        public const string MYPOINTS_POINTSCOLLECTED = "MyPoints_pPointsCollected";
        public const string MYPOINTS_OFFERDATE = "MyPoints_pOfferDate";
        public const string MYPOINTS_OFFERENDDATE = "MyPoints_pOfferEndDate";
        public const string MYPOINTS_LBLWUDLIKE = "MyPoints_h2lblWudLike";
        public const string MYPOINTS_CURPNTSTOTAL = "MyPoints_tdCurrentPointsTotal";
        public const string MYPOINTS_POINTSTOTAL = "MyPoints_tdPoints";
        public const string MYPOINTS_WHICHCOLPERIOD = "MyPoints_pWhichColPeriod";
        public const string MYPOINTS_MISPOINTS = "MyPoints_h2MisPoints";
        public const string MYPOINTS_MISSINGPOINTS = "MyPoints_pMissingPoints";
        public const string MYPOINTS_CALLCC = "MyPoints_plblCallCC";
        public const string MYPOINTS_COMMSG1 = "MyPoints_smlblcommsg1";
        public const string MYPOINTS_COMMSG2 = "MyPoints_smlblcommsg2";
        public const string MYPOINTS_COLPERIODHEADER = "MyPoints_thlbllclPeriodcltedResource";
        public const string MYPOINTS_SUMMARYHEADER = "MyPoints_thlblSummary";
        public const string DETAILHEADER = "MyPoints_thlbllclDtls";
        



        //PointsSummary Key

        public const string POINTSSUMMARY_POINTSVALUE = "PointsSummary_Pointssummaryheader";
        public const string POINTSSUMMARY_TOP_TABLE1BODY = "PointsSummary_TopSectionTable1Body";
        public const string POINTSSUMMARY_TOP_TABLE2BODY = "PointsSummary_TopSectionTable2Body";
        public const string POINTSSUMMARY_TOP_TABLE3BODY = "PointsSummary_TopSectionTable3Body";
        public const string POINTSSUMMARY_TESCOPOINTS_TABLE = "PointsSummary_TableTescoPoints";
        public const string POINTSSUMMARY_TESCOBANKPOINTS_TABLE = "PointsSummary_TableTescoBankPoints";
        public const string POINTSSUMMARY_PAGEHEADING = "PointsSummary_TopHeading";
        public const string POINTSSUMMARY_PAGESUMMARY = "PointsSummary_TopSummary";
        public const string POINTSSUMMARY_BOTTOMSUMMARY = "PointsSummary_BottomSummary";

        

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
        public const string MYCOUPON_TEXTDESCRIPTION = "MyCoupon_TextDescription";
        public const string MYCOUPON_TEXTCOUPONSUMMARY = "MyCoupon_TextCouponSummary";
        public const string MYCOUPON_TEXTINSTOREHEADING = "MyCoupon_TextInstoreHeading";
        public const string MYCOUPON_TEXTONLINEHEADING = "MyCoupon_TextOnlineHeading";
        public const string MYCOUPON_TEXTHOWTOUSEHEADING = "MyCoupon_TextHowtoUseHeading";
        public const string MYCOUPON_TEXTINSTORE = "MyCoupon_TextInstore";
        public const string MYCOUPON_TEXTONLINE = "MyCoupon_TextOnline";
        public const string MYCOUPON_TEXTACTIVEHEADING = "MyCoupon_TextActiveHeading";
        public const string MYCOUPON_TEXTFOOTERLABLE = "MyCoupon_TextFooterLable";
        public const string MYCOUPON_TEXTACTIVESUBHEADING = "MyCoupon_TextActiveSubHeading";
        public const string MYCOUPON_TEXTUSEDCOUPONSECTIONHEADING = "MyCoupon_TextUsedCouponSectionHeading";
        public const string MYCOUPON_TEXTNOREDEEMPTIONMESSAGE = "MyCoupon_TextNoRedeemptionMessage";
        public const string MYCOUPON_LBLCOUPONHEADER = "MyCoupon_lblCouponHeader";
        public const string MYCOUPON_CHKCOUPONHEADER = "MyCoupon_chkCouponHeader";


        //My Latest Statement

        //Login page
        public const string HOMEPAGE_MYACCOUNT = "LoginControls_btnSubmit";

        //  //Join page keys.
        public const string JOIN = "JoinControls";
        public const string JOIN_JOINCLUBCARD = "JoinControls_JoinClubcard";
        public const string JOIN_YOURDETAILS = "JoinControls_Yourdetails";
        public const string JOIN_BTNCONFIRM = "JoinControls_btnJoin";
        public const string JOIN_FIRSTNAME = "JoinControls_txtFirstname";
        public const string JOIN_MIDDLENAME = "JoinControls_txtmiddlename";
        public const string JOIN_SURNAME = "JoinControls_txtsurname";
        public const string JOIN_BTNRADIOMALE = "JoinControls_btnmaleradio";
        public const string JOIN_TXTPOSTCODE = "JoinControls_txtpostcode";
        public const string JOIN_BTNPOSTCODE = "JoinControls_btnpostcode";
        public const string JOIN_EMAIL = "JoinControls_txtEmail";
        public const string JOIN_HOUSENUMBER = "JoinControls_txthousenumber";
        public const string JOIN_EVENINGNUMBER = "JoinControls_txteveningphone";
        public const string JOIN_MSGCONFIRM = "JoinControls_msgconfirm";
        public const string JOIN_IMGALLDONE = "JoinControls_imgalldone";
        public const string JOIN_DAY = "JoinControls_ddlDay";
        public const string JOIN_MONTH = "JoinControls_ddlMonth";
        public const string JOIN_YEAR = "JoinControls_ddlYear";
        public const string JOIN_TITLE = "JoinControls_ddlTitle";
        public const string JOIN_ADDRESSLINE1 = "JoinControls_addressline1";
        public const string JOIN_ADDRESSLINE2 = "JoinControls_addressline2";
        public const string JOIN_ADDRESSLINE4 = "JoinControls_addressline4";
        public const string JOIN_ADDRESSLINE5 = "JoinControls_addressline5";
        public const string JOIN_PHONENUMBER = "JoinControls_phonenumber";
        public const string JOIN_MOBILENUMBER = "JoinControls_mobilenumber";
        public const string JOIN_TERMSANDCONDITION = "JoinControls_chktermsAndCondition";
        public const string JOIN_PRIVACYPOLICY = "JoinControls_chkPrivacyPolicy";
        public const string JOIN_WELCOMEMSG = "JoinControls_welcomeMsg";
        public const string JOIN_CONGRATSMSG = "JoinControls_congratsMsg";
        public const string JOIN_CONFTEXT = "JoinControls_confText";
        public const string JOIN_PDFDOWNLOAD = "JoinControls_clickHeretodownloadpdf";
        public const string JOIN_TXTPROMOCODE = "JoinControls_txtPromoCode";
        public const string JOIN_ERRORPROMOCODE = "JoinControls_errorPromoCode";
        public const string JOIN_HOUSEDROPDOWN = "JoinControls_houseddl";
        public const string JOIN_HEADERIMAGETEXT = "JoinControls_HeaderImageText";
        public const string JOIN_HEADERTEXT = "JoinControls_HeaderText";
        public const string JOIN_HEADERTEXTCONTACTDETAILS = "JoinControls_HeaderTextContactDetails";
        public const string JOIN_LABELTERMSANDCONDITIONS = "JoinControls_LabelTermsAndConditions";
        public const string JOIN_LABELAGREE = "JoinControls_LabelAgree";
        public const string JOIN_LABELCONFIRMCLICK = "JoinControls_LabelConfirmClick";
        public const string JOIN_LABELCALLRATE = "JoinControls_LabelCallRates";
        public const string JOIN_CAPTCHATEXT = "JoinControls_LabelCaptcha";
        public const string JOIN_OPTINSTEXTONE = "JoinControls_OptInsHeadingOne";
        public const string JOIN_OPTINSTEXTTWO = "JoinControls_OptInsHeadingTwo";
        public const string JOIN_OPTINSTEXTTHREE = "JoinControls_OptInsHeadingThree";
        public const string JOIN_OPTINSTEXTFOUR = "JoinControls_OptInsHeadingFour";
        public const string JOIN_OPTINSTEXTFIVE = "JoinControls_OptInsHeadingFive";
        public const string JOIN_OPTOUTSTEXTONE = "JoinControls_OptOutsHeadingOne";
        public const string JOIN_OPTOUTSTEXTTWO = "JoinControls_OptOutsHeadingTwo";
        public const string JOIN_OPTOUTSTEXTTHREE = "JoinControls_OptOutsHeadingThree";
        public const string JOIN_OPTOUTSTEXTFOUR = "JoinControls_OptOutsHeadingFour";
        public const string JOIN_OPTFOOTERONE = "JoinControls_OptFooterOne";
        public const string JOIN_OPTFOOTERTWO = "JoinControls_OptFooterTwo";
        public const string JOIN_DAYPHONE = "JoinControls_txtDayphone";
        public const string JOIN_MOBILE = "JoinControls_txtMobilenumber";
        public const string JOIN_RACE = "JoinControls_ddlRace";
        public const string JOIN_LANGUAGE = "JoinControls_ddlPrefLang";
        public const string JOIN_PRIMARY_ID = "JoinControls_txtPrimaryID";
        public const string JOIN_SECONDARY_ID = "JoinControls_txtSecondaryID";
        public const string JOIN_EVENINGPHONE = "JoinControls_txtEveningphone";
        public const string JOIN_TXTADDRESSLINE1 = "JoinControls_txtAddressLine1";
        public const string JOIN_TXTADDRESSLINE2 = "JoinControls_txtAddressLine2";
        public const string JOIN_TXTADDRESSLINE3 = "JoinControls_txtAddressLine3";
        public const string JOIN_TXTADDRESSLINE4 = "JoinControls_txtAddressLine4";
        public const string JOIN_TXTADDRESSLINE5 = "JoinControls_txtAddressLine5";
        public const string JOIN_DDLADDRESSLINE5 = "JoinControls_ddlAddressLine5";
        public const string JOIN_TXTADDRESSLINE6 = "JoinControls_txtAddressLine6";
        public const string JOIN_ERROR_SUMMARY = "JoinControls_errorSummary";
        public const string JOIN_DDLADDRESS = "JoinControls_ddlAddress";
        public const string JOIN_PREFERENCEFOOTERNOTE = "JoinControls_lblFooterNote";
        public const string JOIN_PREFERENCEFOOTERNOTEOPTOUT = "JoinControls_lblNote";
        public const string JOIN_CONFIRMWELMSG = "JoinControls_JoinwelcomeMsg";

        
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
        public const string JOIN_ERRORINVALIDPOSTCODE = "JoinControls_ErrorInvalidPostcode";

        //Activation page Keys.
        public const string ACTIVATION_TITLE = "ActivationControls_pageTitle";
        public const string ACTIVATION_RECONFIRMTITLE = "ActivationControls_reconfirmpageTitle";
        public const string ACTIVATION_RECONFIRMTITLE1 = "ActivationControls_reconfirmpageTitle1";
        public const string ACTIVATION_CONF_TITLE = "ActivationControls_ConfrimationpageTitle";
        // public const string ACTIVATION_TEXT = "ActivationControls_btnActivation";
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
        public const string ACTIVATION_LBLDateOfBirth = "ActivationControls_lblDOB";
        public const string ACTIVATION_DDLDOB = "ActivationControls_ddlDOB";
        public const string ACTIVATION_DDLMOB = "ActivationControls_ddlMOB";
        public const string ACTIVATION_DDLYOB = "ActivationControls_ddlYOB";
        public const string ACTIVATION_LBLADDRESSLINE1 = "ActivationControls_lblAddressLine1";
        public const string ACTIVATION_LBLPOSTCODE = "ActivationControls_lblPostcode";
        public const string ACTIVATION_LBLMOBILENUMBER = "ActivationControls_lblPMobileNumber";
        public const string ACTIVATION_LBLSSN = "ActivationControls_lblSSN";
        public const string ACTIVATION_LBLEMAIL = "ActivationControls_lblEmailAddress";
        public const string ACTIVATION_EMAIL = "ActivationControls_txtemail";
        public const string ACTIVATION_PASSWORD = "ActivationControls_txtpassword";
        public const string ACTIVATION_REPASSWORD = "ActivationControls_txtrepassword";
        public const string ACTIVATION_REGISTER = "ActivationControls_txtregister";
        public const string ACTIVATION_ErrorMessageDOB = "ActivationControls_ErrorMessageDOB";
        public const string ACTIVATION_ErrorMessageMOB = "ActivationControls_ErrorMessageMOB";
        public const string ACTIVATION_ErrorMessageYOB = "ActivationControls_ErrorMessageYOB";
        public const string ACTIVATION_LinkedCard = "ActivationControls_LinkedCulbcardTxt";
        public const string ACTIVATION_HOMELINK = "ActivationControls_HomeButton";

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

        //Options and benefit page
        public const string OPTIONSBENEFIT_HEADER = "OptionsAndBenefit_pageTitle";
        public const string OPTIONSBENEFIT_ConfirmButton = "OptionsAndBenefit_btnConfirm";
        public const string OPTIONSBENEFIT_Message = "OptionsAndBenefit_lblmessage";
        public const string OPTIONSBENEFIT_BAAVIOSRADIOBTN = "OptionsAndBenefit_radioBtnBAAvios";
        public const string OPTIONSBENEFIT_AVIOSRADIOBTN = "OptionsAndBenefit_radioBtnAvios";
        public const string OPTIONSBENEFIT_XMASSAVER = "OptionsAndBenefit_radioBtnXmasSaver";
        public const string OPTIONSBENEFIT_VOUCHERS = "OptionsAndBenefit_radioBtnVoucher";        
        public const string OPTIONSBENEFIT_VIRGIN = "OptionsAndBenefit_radioBtnVirgin";
        public const string OPTIONSBENEFIT_CLEAR = "OptionsAndBenefit_btnClear";
        public const string OPTIONSBENEFIT_SELECTED_OPTION = "OptionsAndBenefit_hdnSelectedOption";
        public const string OPTIONSBENEFIT_OPTIONS_TAB = "OptionsAndBenefit_tabOption";
        public const string OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE = "OptionsAndBenefit_spanError";
        public const string OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID = "OptionsAndBenefit_txtVirginMembershipIDblock";
        public const string OPTIONSBENEFIT_BA_MEMBERSHIPID = "OptionsAndBenefit_txtBAAviosMembershipIDblock";
        public const string OPTIONSBENEFIT_AVIOS_MEMBERSHIPID = "OptionsAndBenefit_txtAviosMembershipIDblock";
        public const string OPTIONSBENEFIT_BAAVIOSVALIDATEMESSAGE = "OptionsAndBenefit_BAError";
          public const string OPTIONSBENEFIT_AAVIOSVALIDATEMESSAGE = "OptionsAndBenefit_AviosError";
          public const string OPTIONSBENEFIT_VIRGINVALIDATEMESSAGE = "OptionsAndBenefit_VirginError";
        public const string OPTIONSBENEFIT_LBLHEADER = "OptionsAndBenefit_lblHeader";
        public const string OPTIONSBENEFIT_LBLPAGEDESC = "OptionsAndBenefit_lblPageDesc";
        public const string OPTIONSBENEFIT_LBLHEADER1 = "OptionsAndBenefit_lblHeader1";
        public const string OPTIONSBENEFIT_LBLHEADER2 = "OptionsAndBenefit_lblHeader2";
        public const string OPTIONSBENEFIT_TXTCSSAVER = "OptionsAndBenefit_txtChristmassaver";
        public const string OPTIONSBENEFIT_TXTCSMSG = "OptionsAndBenefit_txtCSMsg";
        public const string OPTIONSBENEFIT_TXTAVIOS = "OptionsAndBenefit_txtAvios";
        public const string OPTIONSBENEFIT_TXTAVIOSDESC1 = "OptionsAndBenefit_txtAviosDesc1";
        public const string OPTIONSBENEFIT_TXTAVIOSDESC2 = "OptionsAndBenefit_txtAviosDesc2";
        public const string OPTIONSBENEFIT_TXTAVIOSDESC3 = "OptionsAndBenefit_txtAviosDesc3";
        public const string OPTIONSBENEFIT_TXTAVIOSDESC4 = "OptionsAndBenefit_txtAviosDesc4";
        public const string OPTIONSBENEFIT_TXTVIRGINATLANTIC = "OptionsAndBenefit_txtVirginAtlantic";
        public const string OPTIONSBENEFIT_TXTVADESC1 = "OptionsAndBenefit_txtVirginAtlanticDesc1";
        public const string OPTIONSBENEFIT_TXTVADESC2 = "OptionsAndBenefit_txtVirginAtlanticDesc2";
        public const string OPTIONSBENEFIT_TXTVADESC3 = "OptionsAndBenefit_txtVirginAtlanticDesc3";
        public const string OPTIONSBENEFIT_TXTVADESC4 = "OptionsAndBenefit_txtVirginAtlanticDesc4";
        public const string OPTIONSBENEFIT_TXTBAAVIOS = "OptionsAndBenefit_txtBAAvios";
        public const string OPTIONSBENEFIT_TXTBAAVIOSDESC1 = "OptionsAndBenefit_txtBAAviosDesc1";
        public const string OPTIONSBENEFIT_TXTBAAVIOSDESC2 = "OptionsAndBenefit_txtBAAviosDesc2";
        public const string OPTIONSBENEFIT_TXTBAAVIOSDESC3 = "OptionsAndBenefit_txtBAAviosDesc3";
        public const string OPTIONSBENEFIT_TXTBAAVIOSDESC4 = "OptionsAndBenefit_txtBAAviosDesc4";
        public const string OPTIONSBENEFIT_LBLOTHEROPTIONHEADER = "OptionsAndBenefit_lblOtherOptionsHeader";
        public const string OPTIONSBENEFIT_TXTCLUBCARDEMAIL = "OptionsAndBenefit_txtClubcardEmail";
        public const string OPTIONSBENEFIT_TXTEMAILDESC = "OptionsAndBenefit_txtEmailDesc";
        public const string OPTIONSBENEFIT_TXTAVIOSMEMEBRSHIPIDBLOCK = "txtAviosMembershipIDblock";
        public const string OPTIONSBENEFIT_TXTVIRGINMEMEBRSHIPIDBLOCK = "txtVirginMembershipIDblock";
        public const string OPTIONSBENEFIT_TXTBAAVIOSMEMEBRSHIPIDBLOCK = "txtBAAviosMembershipIDblock";
        public const string OPTIONSBENEFIT_TXTENSUREMSG = "OptionsAndBenefit_txtEnsureMsg";
        public const string OPTIONSBENEFIT_TXTIMPMSG = "OptionsAndBenefit_txtImpMsg";
        public const string OPTIONSBENEFIT_TXTMOREINFO = "OptionsAndBenefit_txtMoreInfo";
        public const string OPTIONSBENEFIT_TXTAVIOSOPTEDOUT2 = "OptionsAndBenefit_txtAviosOptedOut2";
        public const string OPTIONSBENEFIT_TXTVAMOREINFO = "OptionsAndBenefit_txtMoreInfoVA";
        public const string OPTIONSBENEFIT_TXTVAENSUREMSG = "OptionsAndBenefit_txtVirginAtlanticEnsureMsg";
        public const string OPTIONSBENEFIT_TXTVACLUBMEMBER = "OptionsAndBenefit_txtVAClubMember";
        public const string OPTIONSBENEFIT_TXTBAAVIOSMOREINFO = "OptionsAndBenefit_txtMoreInfoBAAvios";
        public const string OPTIONSBENEFIT_TXTBAAVIOSENSUREMSG = "OptionsAndBenefit_txtBAAviosEnsureMsg";
        public const string OPTIONSBENEFIT_TXTBAAVIOSCLUBMEMBER = "OptionsAndBenefit_txtBAAviosClubMember";
        public const string OPTIONSBENEFIT_TXTEMAIL = "OptionsAndBenefit_txtEmail";
        public const string OPTIONSBENEFIT_TXTCONFIRMEMAIL = "OptionsAndBenefit_txtConfirmEmail";
        public const string OPTIONSBENEFIT_CHANGEEMAIL = "OptionsAndBenefit_btnChangeEmail";
        public const string OPTIONSBENEFIT_EMAILERROR = "OptionsAndBenefit_errEmail";
        public const string OPTIONSBENEFIT_CONFIRMEMAILERROR = "OptionsAndBenefit_errConfirmEmail";
        public const string OPTIONSBENEFIT_MISMATCHEMAILERROR = "OptionsAndBenefit_errMismatchEmail";
        

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
        public const string STAMPS = "HomePage_Stamps";
        public const string STAMPKEY1 = "Stamp1";
        public const string STAMPKEY2 = "Stamp2";
        public const string STAMPKEY3 = "Stamp3";
        public const string STAMPKEY4 = "Stamp4";
        public const string STAMPKEY5 = "Stamp5";
        public const string STAMPKEY6 = "Stamp6";
        public const string STAMPKEY7 = "Stamp7";
        public const string STAMPKEY8 = "Stamp8";
        public const string STAMPKEY9 = "Stamp9";



        //BoostAtTesco keys
        public const string BOOSTTOKEN = "Boost_divBoostOnline";
        public const string BOOSTORDERSTATUS = "Boost_BoostOrderStatus";
        public const string BOOSTTokenValue = "Boost_BoostTokenValue";
        public const string BOOSTTOKENECODE = "Boost_BoostTokenEcode";
        public const string BOOSTEXPIRYDATE = "Boost_BoostExpiryDate";
        public const string BOOSTDATEORDERED = "Boost_BoostDateOrdered";
        public const string BOOSTPRINTALL = "Boost_TokenPrintAll";
        public const string BOOSTSELECTALL = "Boost_TokenSelectAll";
        public const string BOOSTMESSAGE = "Boost_TokenMessage";
        public const string BOOSTTEXTEXCHANGEON = "Boost_lblExchangeOn";
        public const string BOOSTTEXTLBLMANAGECARDDESCRIPTION = "Boost_lblManageCardDescription";
        public const string BOOSTTEXTLBLMANAGECARDDESCRIPTION2 = "Boost_lblManageCardDescription2";
        public const string BOOSTTEXTLBLMANAGECARDDESCRIPTION3 = "Boost_lblManageCardDescription3";
        public const string BOOSTTEXTLBLEXCHANGESFORM = "Boost_lblExchangesForm";
        public const string BOOSTTEXTLBLINFOMESSAGE = "Boost_lblInfoMessage";
        public const string BOOSTTEXTLBLUSEONLINE = "Boost_lblUseOnline";
        public const string BOOSTTEXTBOOSTONLINELIST = "Boost_boostonlinelist";
        public const string BOOSTTEXTLBLPURCHASEDONLINE = "Boost_lblPurchasedOnline";
        public const string BOOSTTEXTLBLCODESUSEDONCE = "Boost_lblCodesUsedOnce";
        public const string BOOSTTEXTLBLSTOREEXCCODES = "Boost_lblStoreExcCodes";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG1 = "Boost_divforBoostmsgs1";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG2 = "Boost_divforBoostmsgs2";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG3 = "Boost_divforBoostmsgs3";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG4 = "Boost_divforBoostmsgs4";
        public const string BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSPRINT = "Boost_liExcTokensprint";
        public const string BOOST_DIVFORREWARDANDTOKENOLPRINTTOKENS = "Boost_liPrintTokens";
        public const string BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSTORE = "Boost_liExcTokenStore";
        public const string BOOSTTEXTLBLSHWERRORMSG = "Boost_lblshwerrormsg";
        public const string BOOSTTEXTLBLIWANTTO = "Boost_lblIWantto";
        public const string BOOSTTEXTPNLFORINFO = "Boost_pnlforinfo";
        public const string BOOSTTEXTLBLBOOSTINFO3 = "Boost_lblboostinfo3";


        public const string TOKENINSTOREDATE = "Boost_TokenInstoreDate";
        public const string TOKENINSTOREDESCRIPTION = "Boost_TokenInstoreDescription";
        public const string TOKENINSTOREPRODUCTSTATUS = "Boost_TokenInstoreProductStatus";
        public const string TOKENINSTORETOKENVALUE = "Boost_TokenInstoreTokenValue";
        public const string TOKENINSTOREVALIDUNTIL = "Boost_TokenInstoreValidUntil";

        //My contact Preference keys
        public const string OPTINS_CONTAINER = "MyContactPreference_OptInsContainer";
         public const string OPTINS_SUCCESSMSG = "MyContactPreference_DivSecPrefSuccessMsg";
        public const string PREFERENCE_PageTitle = "MyContactPreference_PageTitle";
        public const string VALIDATION_MESSAGE = "MyContactPreference_ValidationMsg";
        public const string BUTTON_CLICK = "MyContactPreference_BtnConfirm";
        public const string RADIOBUTTON_EMAIL = "MyContactPreference_RadioBtnEmail";
        public const string EMAILTEXT_VARIFICATION = "MyContactPreference_EmailTextValue";
        public const string CONFIRMEMAILTEXT_VARIFICATION = "MyContactPreference_ConfirmEmailTextValue";
        public const string RADIOBUTTON_SMS = "MyContactPreference_RadioBtnSMS";
        public const string RADIOBUTTON_POST = "MyContactPreference_RadioBtnPost";
        public const string PHONENUMBER_VARIFICATION = "MyContactPreference_PhoneNumberValue";
        public const string CONFIRMPHONENUMBER_VARIFICATION = "MyContactPreference_ConfirmPhoneNumberValue";
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
        public const string CHKGROUPTESCOPRODUCT = "MyContactPreference_chkGrpTescoProduct";
        public const string CHKGROUPPARTNEROFFER = "MyContactPreference_chkGrpPartneroffer";
        public const string CHKGROUPRESEARCH = "MyContactPreference_chkGrpResearch";
        public const string NOGRID = "MyContactPreference_divNoGrid";
        public const string JOIN_CHKTESCOOFFER = "JoinControls_joinChkTescoOffers";
        public const string JOIN_CHKPARTNEROFFER = "JoinControls_joinChkPartnerOffers";
        public const string JOIN_CHKRESEARCH = "JoinControls_joinChkResearch";
        public const string CHKTESCOPRODUCT = "MyContactPreference_chkTescoProduct";
        public const string CHKPARTNEROFFER = "MyContactPreference_chkPartneroffer";
        public const string CHKRESEARCH = "MyContactPreference_chkResearch";
        public const string OPTOUT_CHKTESCOPRODUCT = "MyContactPreference_chkOptOutTescoProduct";
        public const string OPTOUT_CHKPARTNEROFFER = "MyContactPreference_chkOptOutPartneroffer";
        public const string OPTOUT_CHKRESEARCH = "MyContactPreference_chkOptOutResearch";
        public const string PREFERENCE_TAB = "MyContactPreference_TabControl";
        public const string PREFERENCE_TABHEADERS = "MyContactPreference_TabHeaders";
        public const string PREFERENCE_HDNEMAIL = "MyContactPreference_HdnEmailValue";
        public const string PREFERENCE_HDNMOBILE = "MyContactPreference_HdnMobileValue";

        public const string PREFERENCE_LBLHEADER = "MyContactPreference_lblHeader";
        public const string PREFERENCE_TEXTPARA1 = "MyContactPreference_textParar1";
        public const string PREFERENCE_LBLHEADING = "MyContactPreference_lblHeading";
        public const string PREFERENCE_LBLDESCP = "MyContactPreference_lblDescp";
        public const string PREFERENCE_LBLMANDMSG = "MyContactPreference_lblMandMsg";
        public const string PREFERENCE_TXTTERMS = "MyContactPreference_txtTerms";
        public const string PREFERENCE_TXTTERMS1 = "MyContactPreference_txtTerms1";
        public const string PREFERENCE_TXTTERMS2 = "MyContactPreference_txtTerms2";
        public const string PREFERENCE_LNKTNC = "MyContactPreference_lnkTC";
        public const string PREFERENCE_LBLEMAILHEADING = "MyContactPreference_lblEmailHeading";
        public const string PREFERENCE_LBLENTEREMAIL = "MyContactPreference_lblEnterEmail";
        public const string PREFERENCE_LBLCNFRMEMAIL = "MyContactPreference_lblCnfrmEmail";
        public const string PREFERENCE_LBLEMAILDESC = "MyContactPreference_lblEmailDesc";
        public const string PREFERENCE_LBLEMAILDESC1 = "MyContactPreference_lblEmailDesc1";
        public const string PREFERENCE_LBLTEXTHEADING = "MyContactPreference_lblTextHeading";
        public const string PREFERENCE_LBLENTERTEXT = "MyContactPreference_lblEnterText";
        public const string PREFERENCE_LBLCNFRMETEXT = "MyContactPreference_lblCnfrmText";
        public const string PREFERENCE_LBLTEXTDESC = "MyContactPreference_lblTextDesc";
        public const string PREFERENCE_LBLTEXTDESC1 = "MyContactPreference_lblTextDesc1";
        public const string PREFERENCE_LBLPOSTHEADING = "MyContactPreference_lblPostHeading";
        public const string PREFERENCE_LBLADDRESSLINE1 = "MyContactPreference_lblAaddressLine1";
        public const string PREFERENCE_LBLWRONGADDRESS = "MyContactPreference_lblWrongAddress";
        public const string PREFERENCE_LBLLATESTSTATEMENT = "MyContactPreference_lblLatestStatement";
        public const string PREFERENCE_LBLLARGESTATE = "MyContactPreference_lblLargeState";
        public const string PREFERENCE_LBLCUSTOMERCARE = "MyContactPreference_lblCustomerCare";
        public const string PREFERENCE_TXTPHN1 = "MyContactPreference_txtPhn1";
        public const string PREFERENCE_TXTPHN2 = "MyContactPreference_txtPhn2";
        public const string PREFERENCE_TXTLBSTAT = "MyContactPreference_txtLBStat";

        public const string PREFERENCE_LBLHEADER1 = "MyContactPreference_lblHeader1";
        public const string PREFERENCE_LBLHEADING1 = "MyContactPreference_lblHeading1";
        public const string PREFERENCE_LBLDESCRIPTION = "MyContactPreference_lblDescription";
        public const string PREFERENCE_LNKCLICKHERE = "MyContactPreference_lnkClickHere";
        public const string PREFERENCE_LBLHEADING2 = "MyContactPreference_lblHeading2";
        public const string PREFERENCE_LBLDESCRIPTION2 = "MyContactPreference_lblDescription2";
        public const string PREFERENCE_TXTCHKOPT1 = "MyContactPreference_txtChkopt1";
        public const string PREFERENCE_TXTCHKOPT2 = "MyContactPreference_txtChkopt2";
        public const string PREFERENCE_TXTCHKOPT3 = "MyContactPreference_txtChkopt3";
        public const string PREFERENCE_TXTVIEWCND = "MyContactPreference_txtViewCnd";
        public const string PREFERENCE_TXTCUSTOMERCHARTER = "MyContactPreference_txtCustomerCharter";
        public const string PREFERENCE_OPSOUTDESCRIPTION = "MyContactPreference_lblOutDescription";
        public const string PREFERENCE_LBLOUTS1 = "MyContactPreference_lblOutS1";
        public const string PREFERENCE_LBLOUTS2 = "MyContactPreference_lblOutS2";
        public const string PREFERENCE_LBLOUTS3 = "MyContactPreference_lblOutS3";



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
        public const string PERSONALDETAILS_PAGEDESCTXTFOUR = "MyPersonalDetails_PageDescTxtfOUR";
        public const string PERSONALDETAILS_CheckText = "MyPersonalDetails_CheckText";
        public const string PERSONALDETAILS_YCDETAILSTXT = "MyPersonalDetails_YCDetailsText";
        public const string PERSONALDETAILS_REQUIREDWARNINGTXT = "MyPersonalDetails_AstricTxt";
        public const string PERSONALDETAILS_CONTACTUSTXT = "MyPersonalDetails_ContactUS";
        public const string PERSONALDETAILS_YOURHOUSEHOLDDETAILSTXT = "MyPersonalDetails_YourHouseHoldDetails";
        public const string PERSONALDETAILS_YOURHOUSEHOLDDETAILSTitleTXT = "MyPersonalDetails_YourHouseHoldDetailsTitle";
        public const string PERSONALDETAILS_JOINTACCOUNTTXT = "MyPersonalDetails_JOINTACCOUNT";

        public const string PERSONALDETAILS_TESCODOTCOMLINKTEXT = "MyPersonalDetails_tescodotcomaccount";
        public const string PERSONALDETAILS_CHANGEPASSWORDLINK = "MyPersonalDetails_passwordChange";
        public const string PERSONALDETAILS_SURNAMEUPDATECLICKHERE = "MyPersonalDetails_surnameupdateClickhere";
        public const string PERSONALDETAILS_SURNAMEUPDATETEXT = "MyPersonalDetails_surnameupdateText";
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
        public const string PERSONALDETAILS_DAYTIMENUMBER = "MyPersonalDetails_txtdayphone";
        public const string PERSONALDETAILS_ERROREMAIL = "MyPersonalDetails_ErrorEmail";
        public const string PERSONALDETAILS_ERRORGENDERMISMATCH = "MyPersonalDetails_ErrorGenderMismatch";
        public const string PERSONALDETAILS_ERRORADDRESSLINE1 = "MyPersonalDetails_ErrorAddressLine1";
        public const string PERSONALDETAILS_ERRORADDRESSLINE2 = "MyPersonalDetails_ErrorAddressLine2";
        public const string PERSONALDETAILS_ERRORADDRESSLINE3 = "MyPersonalDetails_ErrorAddressLine3";
        public const string PERSONALDETAILS_ERRORADDRESSLINE4 = "MyPersonalDetails_ErrorAddressLine4";
        public const string PERSONALDETAILS_ERRORADDRESSLINE5 = "MyPersonalDetails_ErrorAddressLine5";
        public const string PERSONALDETAILS_ERRORADDRESSLINE6 = "MyPersonalDetails_ErrorAddressLine6";
        public const string PERSONALDETAILS_ERRORINVALIDPOSTCODE = "MyPersonalDetails_ErrorInvalidPostcode";
        public const string PERSONALDETAILS_POSTCODEINSTRUCTION = "MyPersonalDetails_lblPostcodeInstruction";

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
        public const string PERSONALDETAILS_DDLPREFLANG = "MyPersonalDetails_ddlPrefLang";
        public const string PERSONALDETAILS_ERRORDAYNUMBER = "MyPersonalDetails_Errordayphone";
        public const string PERSONALDETAILS_ERROREVNGNUMBER = "MyPersonalDetails_ErrorEvengphone";

        public const string PERSONALDETAILS_FLD_ADDRESS = "MyPersonalDetails_fldaddress";


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
        public const string ORDERREPLACEMENT_CardReplacement = "OrderAReplacementControls_CardReplacement";
        public const string ORDERREPLACEMENT_SafelyDestroy = "OrderAReplacementControls_SafelyDestroy";
        public const string ORDERREPLACEMENT_lostcard = "OrderAReplacementControls_lostcard";
        public const string ORDERREPLACEMENT_OrderAReplacement = "OrderAReplacementControls_OrderAReplacement";
        public const string ORDERREPLACEMENT_RequestReason = "OrderAReplacementControls_RequestReason";
        public const string ORDERREPLACEMENT_Communication = "OrderAReplacementControls_Communication";

        //--Xmus saver
        public const string XMUSSAVER_YOUHAVESAVEDMSG = "XmusSaver_spnYouHaveSavedMsg";
        public const string XMUSSAVER_NOVEMBERMSG = "XmusSaver_spnNovemberMsg";
        public const string XMUSSAVER_SPNTTLTOPPEDUPMONEY = "XmusSaver_spnttlToppedUpMoney";
        public const string XMUSSAVER_SPNCCVOUCHERSSAVED = "XmusSaver_spnCCVouchersSaved";
        public const string XMUSSAVER_DVMONNEYTOPPEDUP = "XmusSaver_dvMoneyToppedUp";
        public const string XMUSSAVER_SPNBONUSVOUCHER = "XmusSaver_spnBonusVoucher";
        public const string XMUSSAVER_SPNTTLVOUCHERSSOFAR = "XmusSaver_spnTtlVouchersSoFar";
        public const string XMUSSAVER_SPNYEAR1 = "XmusSaver_spnYear";
        public const string XMUSSAVER_SPNTTLPNTS = "XmusSaver_spnTtlPnts";

        public const string XMUSSAVER_SPNBONUSVALUEFOR50 = "XmusSaver_spnBonusValueFor50";
        public const string XMUSSAVER_SPNMONEYTOBESAVEDFORBONUS6 = "XmusSaver_spnMoneyTobeSavedForBonus6";
        public const string XMUSSAVER_SPNBONUSVALUEFOR100_1 = "XmusSaver_spnBonusValueFor100AND1";
        public const string XMUSSAVER_SPNBONUSVALUEFOR100 = "XmusSaver_spnBonusValueFor100";
        public const string XMUSSAVER_HEADERTITLE = "XmusSaver_lblHeaderTitle";
        public const string XMUSSAVER_WELCOMEMESSAGE = "XmusSaver_lblWelcomeMessage";
        public const string XMUSSAVER_THANKYOUMESSAGE = "XmusSaver_lblThankYouMessage";
        public const string XMUSSAVER_OPTIONSANDBENEFITSMESSAGE = "XmusSaver_lblOptionsAndBenefitsMessage";

        //MLS

        public const string MLS_SPNTHANKYOUMESSAGE = "MLS_spnthankyoumessage";
        public const string MLS_MYACCOUNTSUMMARY = "MLS_MyAccountSummary";
        public const string MLS_MYPOINTSLABEL = "MLS_MyPOints";
        public const string MLS_MYPOINTSVALUE = "MLS_MyPOintsValue";
        public const string MLS_MYVOUCHERLABEL = "MLS_MyPOints";
        public const string MLS_MYVOUCHERVALUE = "MLS_ltrCurVouchers";
        public const string MLS_MYCOUPONVALUE = "MLS_ltrMyCoupons";
        public const string MLS_MYCOUPONLABEL = "MLS_MyCoupons";
        public const string MLS_LBLTESCOBANK = "MLS_lblTescoBank";
        public const string MLS_LBLPOINTSTOVOUCHER = "MLS_lblpointstoVoucher";
        public const string MLS_BTNMYCOUPONS = "MLS_btnmycoupons";
        public const string MLS_BTNMYVOUCHERS = "MLS_btnmyvouchers";
        public const string MLS_BTNMYPOINTS = "MLS_btnmyPoints";
        public const string MLS_DIV_FOOTER = "MLS_divfooter";
        public const string MLS_DIV_header_1 = "MLS_divheader";
        public const string MLS_DIV_header_2 = "MLS_divheadertwo";
        public const string MLS_MYAVIOS = "MLS_myavios";
        public const string MLS_LTRMYAVIOS = "MLS_ltrmyAvios";
        public const string MLS_LCLPOINTSTOAVIOS = "MLS_lclPointsToAvios";
        public const string MLS_MYTOPUP = "MLS_MyTopUp";
        public const string MLS_LTRMYTOPUP = "MLS_ltrmytopup";
        public const string MLS_LBLYOURTOPUPVOUCHERS = "MLS_lblYourTopUpVouchers";
        public const string MLS_MYBONUS = "MLS_MyBonus";
        public const string MLS_LTRMYBONUS = "MLS_ltrmybonus";
        public const string MLS_LBLBONUSVOUCHERS = "MLS_lblBonusVouchers";
        public const string MLS_MYVIRGINATLANTIC = "MLS_MyVirginAtlantic";
        public const string MLS_LTRMYVIRGINMILES = "MLS_ltrmyvirginmiles";
        public const string MLS_LCLPOINTSTOVIRGINATLANTIC = "MLS_lclPointsToVirginAtlantic";
        public const string MLS_POINTSTEXT = "MLS_PointsText";

        //--HTMLS
        public const string HTML_HEADER = "Htmls_pageheader";
        public const string HTML_HEADERNAV = "Htmls_pageheadernav";
        public const string HTML_FOOTER = "Htmls_pagefooter";
        public const string HTML_FOOTERLINKS = "Htmls_footer";
        public const string HTML_XMASSAVER_HOLDINGPAGE = "Htmls_xmasHoldingpage";
        public const string HTML_VOUCHER_HOLDINGPAGE = "Htmls_voucherHoldingpage";
        public const string HTML_BOOST_HOLDINGPAGE = "Htmls_boostHoldingpage";
        public const string HTML_COUPONS_HOLDINGPAGE = "Htmls_couponsHoldingpage";
        public const string HTML_MLS_HOLDINGPAGE = "Htmls_mlsHoldingpage";
        public const string HTML_POINTS_HOLDINGPAGE = "Htmls_pointsHoldingpage";

    }
}