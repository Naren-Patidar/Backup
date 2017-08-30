using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Constants
{
    public class ValidationKey
    {
        #region HomeSecurity

        public const string FIRSTDIGIT_REQUIRED = "RequiredFieldValidator3Resource1";
        public const string SECONDDIGIT_REQUIRED = "RequiredFieldValidator4Resource1";
        public const string THIRDDIGIT_REQUIRED = "RequiredFieldValidator5Resource1";
        public const string INVALIDCHARMSG = "CompareValidator1Resource1ErrorMessage";
        public const string DEFAULTSECURITYMSG = "lclHeaderMsg";
        public const string INVALIDSECURITYNUMBERMSG = "lclErrorMsgHead";
        public const string ERRORMAXATTEMPTTEXT = "ErrorMaximumAttemp";
        public const string ERRORMAXATTEMPT1 = "lclMaxAttemptsMsg1";
        public const string ERRORMAXATTEMPT2 = "lclMaxAttemptsMsg2";
        public const string ERRORMAXATTEMPT3 = "lclMaxAttemptsMsg3";
        public const string BANNERPOSTCODE = "lblPostCode";
        public const string BANNERSURNAME = "lblSurname";

        #endregion
        #region MyPersonalDetails
        public const string VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE = "savesuccess";
        public const string PERSONALDETAILS_REPLACEMENTTEXT = "Do you need a replacement card with your new name? Click here";
        public const string PERSONALDETAILS_ERRORDOB = "dobvalid";
        public const string PERSONALDETAILS_GENERICERROR = "correctFollowingvalid";
        public const string PERSONALDETAILS_ERRORNAME1 = "FirstName";
        public const string PERSONALDETAILS_ERRORNAME3 = "LastName";
        public const string PERSONALDETAILS_ERRORNAME2 = "Initial";
        public const string PERSONALDETAILS_ERRORMOBILENUMBER = "MobilePhoneNumber";
        public const string PERSONALDETAILS_ERRORDAYTIMENUMBER = "Len_DayTimePhonenumber";
        public const string PERSONALDETAILS_ERROREVENINGNUMBER = "Len_EveningPhonenumber";
        public const string PERSONALDETAILS_ERROREMAIL = "EmailAddress";
        public const string PERSONALDETAILS_ERRORGENDERMISMATCH = "gendermismatch";
        public const string PERSONALDETAILS_ERRORADDRESSLINE1 = "MailingAddressLine1";
        public const string PERSONALDETAILS_ERRORADDRESSLINE2 = "MailingAddressLine2";
        public const string PERSONALDETAILS_ERRORADDRESSLINE3 = "MailingAddressLine3";
        public const string PERSONALDETAILS_ERRORADDRESSLINE4 = "MailingAddressLine4";
        public const string PERSONALDETAILS_ERRORADDRESSLINE5 = "MailingAddressLine5";
        public const string PERSONALDETAILS_ERRORADDRESSLINE6 = "MailingAddressLine6";
        public const string PERSONALDETAILS_ERRORDUPLICACY = "recordExistvalid";
        public const string PERSONALDETAILS_ERRORPROFANITY = "problemWithDetailsvalid";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE1 = "Len_MailingAddressLine1";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE2 = "Len_MailingAddressLine2";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE3 = "Len_MailingAddressLine3";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE4 = "Len_MailingAddressLine4";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE5 = "Len_MailingAddressLine5";
        public const string PERSONALDETAILS_LNERRORADDRESSLINE6 = "Len_MailingAddressLine6";
        public const string PERSONALDETAILS_HOUSEHOLDMEMBER1YEAR = "lclSelectYearText";
        public const string PERSONALDETAILS_HOUSEHOLDTEXTYEAR = "ddlHouseholdYearResource1Text";

        #endregion
        #region FooterLinks

        //Footer Links
        public const string FOOTER_SITEMAP = "SMSitemap";
        public const string FOOTER_CONTACTUS = "SMContactus";
        public const string FOOTER_HELP_FAQS = "SMHelp";
        public const string FOOTER_TERMS_CONDITIONS = "SMConditions";
        public const string FOOTER_PRIVACY = "SMYourprivacy";
        public const string FOOTER_EMAILSIGNUP = "SMEmailSignup";
        public const string FOOTER_LOSTCLUBCARD = "SMLostyourClubcard";
        public const string FOOTER_TESCODOTCOM = "SMTescoDotcom";

        #endregion
        #region HeaderLinks
        public const string ABOUT_CLUBCARD = "JMSpendandCollect";
        public const string CLUBCARD_BOOST = "SMExchangeandRewards";
        public const string HEADER_CLUBCARDPERKS = "JMCCPerks";
        public const string HEADER_FACEBOOK = "SMFacebook";
        public const string HEADER_TWITTER = "SMTwitter";
        public const string HEADER_YOUTUBE = "JMYouTube";
        public const string HEADER_BASKET = "JMBasket";
        public const string HEADER_SIGNOUT = "SMSignOut";
        public const string HEADER_EMAILSIGNUP = "JMEmailSignup";
        public const string HEADER_WEBSITE_FEEDBACK = "JMEmailSignup";
        public const string HEADER_TESCO_DOTCOM = "JMTescocom";
        public const string HEADER_TESCO_CLUBCARD = "SMHome";
        public const string WELCOMETESCOCLUBCARD = "ltrWelcomeMsgResource1Text";

        public const string HEADER_TESCOCOM = "lblTescoSite";
        public const string HEADER_SIGNOUTKEY = "lblSignOut";
        public const string HEADER_STORELOCATOR = "lblStoreLocator";
        public const string HEADER_FEEDBACK = "lblFeedback";
        public const string HEADER_HREFTESCOCOM = "hrefTescoSite";
        public const string HEADER_HREFSTORELOCATOR = "hrefStoreLocator";
        public const string HEADER_HREFBASKET = "hrefBasket";
        public const string HEADER_ABOUTCLUBCARD = "lblAboutClubcard";
        public const string HEADER_COLLECTPOINTS = "lblCollectPoints";
        public const string HEADER_SPENDVOUCHERS = "lblSpendVouchers";
        public const string HEADER_MCA = "lblMCA";

        public const string HEADER_HOWTOSPEND = "lblSV_How";
        public const string HEADER_CBOOST = "lblSV_Boost";
        public const string HEADER_FUN = "lblSV_Fun";
        public const string HEADER_EATINGOUT = "lblSV_EatingOut";
        public const string HEADER_TRAVEL = "lblSV_Travel";
        public const string HEADER_HOME = "lblSV_HomeEssen";
        public const string HEADER_LIFESTYLE = "lblSV_Lifestyle";
        public const string HEADER_VALUE = "lblSV_4XValue";

        public const string HEADER_FINDADAY = "lblFun_FindDay";
        public const string HEADER_DAYSOUT = "lblFun_DaysOut";
        public const string HEADER_EXP = "lblFun_Experience";
        public const string HEADER_ENTERTAINMENT = "lblFun_Entertainment";
        public const string HEADER_VIEWALL = "lblFun_ViewAll";

        public const string HEADER_FINDREST = "lblEO_FindRestaurant";
        public const string HEADER_PUB = "lblEO_Pubs";
        public const string HEADER_REST = "lblEO_Restaurant";

        public const string HEADER_AIRLINE = "lblTravel_Airlines";
        public const string HEADER_HOLIDAY = "lblTravel_Holiday";
        public const string HEADER_HOTEL = "lblTravel_HotelBreak";
        public const string HEADER_TRANSPORT = "lblTravel_Transport";

        public const string HEADER_GARDEN = "lblHome_Garden";
        public const string HEADER_SOFTWARE = "lblHome_Learning";
        public const string HEADER_MOTORING = "lblHome_Motoring";

        public const string HEADER_HEALTH = "lblLifestyle_Health";
        public const string HEADER_SHOPPING = "lblLifestyle_Shopping";
        public const string HEADER_MAGAZINE = "lblLifestyle_Magazine";
        public const string HEADER_MEMBERSHIP = "lblLifestyle_Membership";

        public const string HEADER_VALUEFINDADAY = "lbl4XValue_DayOut";
        public const string HEADER_VALUEATTRACTION = "lbl4XValue_Attraction";
        public const string HEADER_VALUEREST = "lbl4XValue_Pub";

        #endregion
        #region Activation

        public const string MESSAGEFORACTIVATINGCLUBCARD = "lclMultipleDotcomID1Text";
        public const string MESSAGEFORRACTIVATIONERROR1 = "Span2ReConfirmClubcardDetailsText";
        public const string MESSAGEFORRACTIVATIONERROR2 = "Span3ReconfirmDetailsMessageText";
        public const string SUCCESSFULMESSAGEFORACTIVATION = "lclConfirm1Resource1Text";
        public const string ERRORFORMANDATORYCLUBCARD = "ClubcardNumber";
        public const string ERRORFORMANDATORYFIRSTNAME = "FirstName";
        public const string ERRORFORMANDATORYSURNAME = "LastName";
        public const string ERRORFORMANDATORYPOSTCODE = "MailingAddressPostCode";
        public const string DayOfBirth = "DayOfBirth";
        public const string YearofBirth = "YearofBirth";
        public const string MonthOfBirth = "MonthOfBirth";
        public const string RECONFIRMPAGETITLE = "Span2ReConfirmClubcardDetailsText";
        public const string MESSAGEFORRACTIVECARD = "lclMultipleDotcomID1Text";


        #endregion
        # region Coupon
        public const string NOACTIVECOUPON_PRESENT = "NoCouponsLable";
        public const string NOREDEEMEDCOUPON_4WEEKS = "NoRedeemptionMessage";
        public const string AVAILABLECOUPON_SUMMARY1 = "CouponAvailableLable";
        public const string AVAILABLECOUPON_SUMMARY = "lclCouponsCurrentlyResource1Text";
        public const string USEDCOUPON4WEEKS_SUMMARY = "CouponUsedLable";
        public const string USEDCOUPON4WEEKS_SUMMARY1 = "lclCouponsUsed1Resource1Text";
        public const string COUPONSENT_SUMMARY = "CouponsSentLable";
        public const string COUPONSENT_SUMMARY1 = "Localize2Resource1Text";
        public const string SELECTCOUPONMESSAGE = "NoCouponSeletedError";
        public const string COUPONDESCRIPTION = "Description";
        public const string SUMMARYHEADING = "SummaryHeading";
        public const string INSTOREHEADING = "InStoreHeading";
        public const string ONLINEHEADING = "OnlineHeading";
        public const string HOWTOUSEHEADING = "Heading";//"HowtoUseHeading";
        public const string INSTOREHTML = "InStoreHtml";
        public const string ONLINEHTML = "OnlineHtml";
        public const string ACTIVEHEADING = "Heading";
        public const string ACTIVESUBHEADING = "SubHeading";
        public const string FOOTERLABEL = "FooterLable";
        public const string USEDCOUPONSECTIONHEADING = "UsedCouponSectionHeading";
        public const string NOREDEEMPTIONMESSAGE = "NoRedeemptionMessage";


        #endregion
        # region My contact preference
        public const string CONTACTPREFERENCE_LABLEONE = "lclConfirmcontactResource1Text";
        public const string CONTACTPREFERENCE_LABLETWO = "lclMyContactPrefResource1Text";
        public const string CONTACTPREFERENCE_VALIDATIONMESSAGE = "lclMyContactPrefResource1Text";
        public const string CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG = "lclConfirmSaveMsgResource1Text";
        public const string CONTACTPREFERENCE_MESSAGEDATASAVED = "MessageDataSaved";
        #endregion
        #region Join
        public const string ERRORPLEASECORRECTINFO = "errCorrectInformation";
        public const string ERRORTITLE= "lcltittleErrMsgResource1Text";
        public const string ERRORNAME1 = "errValidFirstName";
        public const string ERRORNAME2 = "Initial";
        public const string ERRORNAME3 = "errValidSurname";
        public const string ERRORGENDER = "errSelectGender";
        public const string ERRORDOB = "errValidDOB";
        public const string ERRORPOSTCODE_UK = "errSorryFindAddress";//errValidPostCode
        public const string ERRORPOSTCODE_GC = "MailingAddressPostCode";//errValidPostCode
        public const string ERRORADDRESUK = "houseNovalid";
        public const string ERRORADDRESSLINE1 = "errValidHouseNumber";
        public const string ERRORADDRESSLINE2 = "errValidAddressline2";
        public const string ERRORADDRESSLINE4 = "errValidAddressline4";
        public const string ERRORADDRESSLINE5 = "errValidAddressline5";
        public const string ERROREMAIL = "errValidEmail";
        public const string ERRORPRIVACY = "errAcceptPolicy";
        public const string ERRORPROFANE = "errRegistration";
        public const string ERRORPROMOCODE = "errValideCode";
        public const string ERRORMOBILENUMBER = "errValidMobilePhoneNumber";
        public const string ERRORDAYTIMENUMBER = "errValidDayPhoneNumber";
        public const string ERROREVENINGNUMBER = "errValidEveningPhoneNumber";
        public const string ERRORDUPLICACY = "errDBCardError";
        public const string CAPTCHA_ERROR = "lclCaptcha1Resource1Text";
        public const string ERROR_SUMMARY = "SuccessMessage";

        #endregion
        #region PersonalDetails
        public const string PD_ERRORPOSTCODE1 = "sorryvalid";
        public const string PD_ERRORPOSTCODE2 = "lclFindAddressMsgResourcetext";
        public const string PD_ERRORPOSTCODE3 = "pleaseTryvalid";
        public const string PD_ERRORPOSTCODE4 = "lclCommunicateMsg1Resource1Text";
        public const string PD_ERRORPOSTCODE5 = "lclCommunicateMsg2Resource1Text";
        public const string PD_POSTCODESEC1 = "lclCantFindAdressmsgtext";
        public const string PD_POSTCODESEC2 = "lclCantFindAddressText";
        public const string PD_SAVESUCCESSFULLMSG = "savevalid";
        public const string PD_CheckAmendText = "lclblueheaderbarText";
        public const string PD_YCDETAILSText = "lclYourContDetailsText";
        public const string PD_REQUIREDWARNINGText = "lclRequireWarningText";
        public const string PD_PAGEDESCONEText = "lclPageDescforEmailNotfnText";
        public const string PD_PAGEDESCTWOTextstart = "lclpageDesc2Text";
        public const string PD_PAGEDESCTWOTextlink = "lclpageDesc3Text";
        public const string PD_PAGEDESCTWOTextend = "lclpageDesc4Text";
        public const string PD_PAGEDESCTHREETextstart = "lclpageDesc5Text";
        public const string PD_PAGEDESCTHREETextlink = "lclpageDesc6Text";
        public const string PD_PAGEDESCTHREETextend = "lclpageDesc7Text";
        public const string PD_CONTACTUSTEXT1 = "lclCommunicateMsg1Resource1Text";
        public const string PD_CONTACTUSTEXT2 = "lclCommunicateMsg2Resource1Text";
        public const string PD_YOURHOUSEHOLDDETAILSTITLE = "lclHouseHoldDetailsText";
        public const string PD_YOURHOUSEHOLDDETAILS = "lclNoteTextText";
        public const string PD_JOINTACCOUNT = "lclFinalMessageText";
        #endregion
        #region ViewMYCards
        public const string VALIDATIONMESSAGEFORMAIN = "CustomerTypeMain";
        public const string VALIDATIONMESSAGEFORASSO = "CustomerTypeAssoc";
        public const string VALIDATIONMESSAGEFORMAINCARDHOLDER = "CardHolder";
        public const string VALIDATIONTYPEOFCARD = "Typeofcard";
        public const string VALIDATIONWHEREUSED = "WhereResource1";
        public const string VALIDATIONMORESIXMONTHS = "lclMore";
        public const string VALIDATIONANDSEPRATOR = "AndSeprator";
        public const string VALIDATAIONNAMEOFACCOUNT = "ltrNamesResource1";
        public const string Statement1 = "CardsVariousResource1";
        public const string Statement2 = "lclAnothercardlistedResource1";
        public const string Statement2Contact1 = "lcltollfree1Resource1";
        public const string Statement2Contact2 = "lcltollfree2Resource2";
        public const string Statement2Star1 = "lcltollfree1starResource1";
        public const string Statement2Star2 = "lcltollfree2starResource2";
        public const string Statement3start = "Localize6Resource1";
        public const string Statement3mid = "Localize1Resource1";
        public const string Statement3link = "FAQSResource1";
        public const string Statement3end = "furtheirdtlsResource1";
        public const string PhoneStatement1 = "CommunicateMsg1Resource1";
        public const string PhoneStatement2 = "CommunicateMsg2Resource1";


        #endregion
        #region OrderAReplacement

        public const string ORDERREPLACEMENT_SORRY = "MaxOrderLimitReached";
        public const string ORDERREPLACEMENT_CUSTOMERCARE = "CallCustomerCare";
        public const string ORDERREPLACEMENT_COMMMSG1 = "CommunicationMsg";
        public const string ORDERREPLACEMENT_COMMMSG2 = "LocalRatesMsg";
        public const string ORDERREPLACEMENT_CardReplacement = "NeedNewCard";
        public const string ORDERREPLACEMENT_SafelyDystroy = "SafelyDamageMsg";
        public const string ORDERREPLACEMENT_LostCard = "CardLostorStolenMsg";
        public const string ORDERREPLACEMENT_LostCardContact1 = "TollFreeNumber1";
        public const string ORDERREPLACEMENT_LostCardcontact2 = "TollFreeNumber2";
        public const string ORDERREPLACEMENT_LostCardcondition1 = "Condition1withOr";
        public const string ORDERREPLACEMENT_LostCardcondition2 = "Condition2";
        public const string ORDERREPLACEMENT_OrderAReplacement = "OrderStandardCard";
        public const string ORDERREPLACEMENT_RequestReason = "OrderRequestReason";
        public const string ORDERREPLACEMENT_PhoneStatement1 = "CommunicationMsg";
        public const string ORDERREPLACEMENT_PhoneStatement2 = "LocalRatesMsg";


        #endregion
        #region Boost
        public const string BOOST_LBLEXCHANGEON = "lclExchangeOn";
        public const string BOOST_LBLMANAGECARDDESCRIPTION = "lclManageCardDescription";
        public const string BOOST_LBLMANAGECARDDESCRIPTION2 = "lclManageCardDescription2";
        public const string BOOST_LBLMANAGECARDDESCRIPTION3 = "lclManageCardDescription3";
        public const string BOOST_LBLEXCHANGESFORM = "lclExchangesForm";
        public const string BOOST_LBLINFOMESSAGE = "lclInfoMessage";
        public const string BOOST_LBLUSEONLINE = "lclUseOnline";
        public const string BOOST_BOOSTCOPYTOKENSLIST = "lclCopyTokens";
        public const string BOOST_BOOSTCHECKOUT = "lclBoostCheckout";
        public const string BOOST_LBLPURCHASEDONLINE = "lclPurchasedOnline";
        public const string BOOST_LBLCODESUSEDONCE = "lclCodesUsedOnce";
        public const string BOOST_LBLSTOREEXCCODES = "lclStoreExcCodes";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG1 = "lclmsg1";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG2 = "lclmsg2";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG3 = "lclmsg3";
        public const string BOOST_DIVFORBOOSTMSGSLCLMSG4 = "lclmsg4";
        public const string BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSPRINT = "lclExcTokensprint";
        public const string BOOST_DIVFORREWARDANDTOKENOLPRINTTOKENS = "lclPrintTokens";
        public const string BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSTORE = "lclExcTokenStore";
        public const string BOOST_LBLSHWERRORMSG = "lblShowErrorMsg";
        public const string BOOST_LBLIWANTTO = "lclWantto";
        public const string BOOST_PNLFORINFO = "lclInfo1";
        public const string BOOST_LBLBOOSTINFO3 = "lclInfo3";
        #endregion
        #region Voucher
        public const string VOUCHER_LCLVIEWSUMMARY = "lclViewSummary";
        public const string VOUCHER_LCLVOUCHERSUMHEADER = "lclVoucherSumHeader";
        public const string VOUCHER_LCLFBTWTHEADER = "lclFBtwtHeader";
        public const string VOUCHER_LCLTELLFRIENDS = "lclTellfriends";
        public const string VOUCHER_LCLFBTWTSHAREMESSAGE = "lclFBtwtShareMessage";
        public const string VOUCHER_LCLUNUSEDVOUCHERSHEADER = "lclUnUsedVouchersHeader";
        public const string VOUCHER_LCLVOUCHERSUSEDONCE = "lclVouchersUsedOnce";
        public const string VOUCHER_LCLCURRENTUNSPENDVOUCHERS = "lclCurrentUnspendVouchers";
        public const string VOUCHER_LCLFOUNDBELOW = "lclFoundBelow";
        public const string VOUCHER_LCLASPENTRESOURCE = "lclAspentResource";
        public const string VOUCHER_LCLDOTCOMRESOURCE = "lclDotcomResource";
        public const string VOUCHER_LCLCPYVOUCHERCODERESOURCE = "lclcpyVoucherCodeResource";
        public const string VOUCHER_LCLCHECKOUTRESOURCE = "lclCheckoutResource";
        public const string VOUCHER_LCLPRINTVOUCHERRESOURCE = "lclPrintVoucherResource";
        public const string VOUCHER_LCLSELECTVOUCHERRESOURCE = "lclSelectVoucherResource";
        public const string VOUCHER_LCLCLICKPRINTRESOURCE = "lclClickPrintResource";
        public const string VOUCHER_LCLPRINTRESOURCE = "lclPrintResource";
        public const string VOUCHER_LCLTILLRESOURCE = "lclTillResource";
        public const string VOUCHER_LCLREDEMVOUCHERRESOURCE = "lclRedemVoucherResource";
        public const string VOUCHER_LCLEXPDATERESOURCE = "lclExpDateResource";
        public const string VOUCHER_ = "";
        #endregion
        #region Home_Page
        public const string HOME_MESSAGELABLE = "MyMessages";
        public const string HOME_MESSAGETEXT3 = "MyAccountVariablePreferenceMessage";
        public const string HOME_MESSAGETEXT2 = "MyAccountVariableProcessingMessage";
        public const string HOME_MESSAGETEXT1 = "MyAccountAddressError";
        public const string HOME_CURRENTPOINTSLBL = "lclCurrentPointResource1Text";
        public const string Xmas_Saver = "xmasmiles";
        public const string Virgin_Atlantic = "virginMiles";
        public const string HOME_STDVOUCHERSLBL = "stdVoucher";
        public const string BA_Miles_Standard = "BAmiles";
        public const string HOME_VOUCHERSLBL = "lblVouchersResource1Text";
        public const string HOME_CURRENCYSYMBOL = "CurrencySymbol";
        public const string Avios_standerd = "aviosMiles";

        public const string STAMP_URL = "urlStamp";

        #endregion

    }
}

