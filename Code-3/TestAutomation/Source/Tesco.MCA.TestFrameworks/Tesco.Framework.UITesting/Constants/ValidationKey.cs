using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Constants
{
    public class ValidationKey
    {
        #region Home_Page
        public const string HOME_MESSAGELABLE = "MyMessages";
        public const string HOME_MESSAGETEXT3 = "MyAccountVariablePreferenceMessage";
        public const string HOME_MESSAGETEXT2 = "MyAccountVariableProcessingMessage";
        public const string HOME_MESSAGETEXT1 = "MyAccountVariableProcessingMessage";
        public const string HOME_CURRENTPOINTSLBL = "lclCurrentPointResource1Text";
        public const string Xmas_Saver = "xmasmiles";
        public const string Virgin_Atlantic = "virginMiles";
        public const string HOME_STDVOUCHERSLBL = "stdVoucher";
        //public const string Airmiles_Premium = "aviosMiles";
        public const string BA_Miles_Standard = "BAmiles";
        public const string HOME_VOUCHERSLBL = "lblVouchersResource1Text";
        public const string HOME_CURRENCYSYMBOL = "symPoundResource1Text";
        public const string Avios_standerd = "aviosMiles";

        #endregion
        #region HomeSecurity

        public const string FIRSTDIGIT_REQUIRED = "RequiredFieldValidator3Resource1Text";
        public const string SECONDDIGIT_REQUIRED = "RequiredFieldValidator4Resource1Text";
        public const string THIRDDIGIT_REQUIRED = "RequiredFieldValidator5Resource1Text";
        public const string INVALIDCHARMSG = "CompareValidator1Resource1ErrorMessage";
        public const string DEFAULTSECURITYMSG = "lclHeaderMsgText";
        public const string INVALIDSECURITYNUMBERMSG = "lclErrorMsgHeadText";
        public const string ERRORMAXATTEMPTTEXT = "ErrorMaximumAttempText";

        #endregion
        #region MyPersonalDetails

        public const string VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE = "savevalid";
        public const string PERSONALDETAILS_REPLACEMENTTEXT = "Do you need a replacement card with your new name? Click here";
        public const string PERSONALDETAILS_ERRORDOB = "dobvalid";
        public const string PERSONALDETAILS_GENERICERROR = "correctFollowingvalid";
        public const string PERSONALDETAILS_ERRORNAME1 = "firstNamevalid";
        public const string PERSONALDETAILS_ERRORNAME3 = "surNamevalid";
        public const string PERSONALDETAILS_ERRORMOBILENUMBER = "mobPhonevalid";
        public const string PERSONALDETAILS_ERRORDAYTIMENUMBER = "daytimePhonevalid";
        public const string PERSONALDETAILS_ERROREVENINGNUMBER = "eveningPhonevalid";
        public const string PERSONALDETAILS_ERROREMAIL = "emailvalid";
        public const string PERSONALDETAILS_ERRORGENDERMISMATCH = "gendermismatch";
        public const string PERSONALDETAILS_ERRORADDRESSLINE1 = "houseNovalid";
        public const string PERSONALDETAILS_ERRORADDRESSLINE2 = "addressLine2valid";
        public const string PERSONALDETAILS_ERRORADDRESSLINE3 = "addressLine3valid";
        public const string PERSONALDETAILS_ERRORADDRESSLINE4 = "addressLine4valid";
        public const string PERSONALDETAILS_ERRORADDRESSLINE5 = "addressLine5valid";
        public const string PERSONALDETAILS_ERRORADDRESSLINE6 = "addressLine6valid";
        public const string PERSONALDETAILS_ERRORDUPLICACY = "recordExistvalid";
        public const string PERSONALDETAILS_ERRORPROFANITY = "problemWithDetailsvalid";
        
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

        #endregion
        #region Activation

        public const string MESSAGEFORACTIVATINGCLUBCARD = "lclMultipleDotcomID1Text";
        public const string MESSAGEFORRACTIVATIONERROR1 = "Span2ReConfirmClubcardDetailsText";
        public const string MESSAGEFORRACTIVATIONERROR2 = "Span3ReconfirmDetailsMessageText";
        public const string SUCCESSFULMESSAGEFORACTIVATION = "lclConfirm1Resource1Text";
        public const string ERRORFORMANDATORYCLUBCARD = "Clubcardvalid";
        public const string ERRORFORMANDATORYFIRSTNAME = "FirstNamevalid";
        public const string ERRORFORMANDATORYSURNAME = "SurNamevalid";
        public const string ERRORFORMANDATORYPOSTCODE = "PostCodevalid";


        #endregion
        # region Coupon
        public const string NOACTIVECOUPON_PRESENT = "lclMyActiveCoupons1Resource1Text";
        public const string NOREDEEMEDCOUPON_4WEEKS = "lclpMsgNoRedeemedCouponsResource1Text";
        public const string AVAILABLECOUPON_SUMMARY1 = "lclCouponsCurrently1Resource1Text";
        public const string AVAILABLECOUPON_SUMMARY = "lclCouponsCurrentlyResource1Text";
        public const string USEDCOUPON4WEEKS_SUMMARY = "lclCouponsUsedResource1Text";
        public const string USEDCOUPON4WEEKS_SUMMARY1 = "lclCouponsUsed1Resource1Text";
        public const string COUPONSENT_SUMMARY = "lclCoupon1Resource1Text";
        public const string COUPONSENT_SUMMARY1 = "Localize2Resource1Text";
        public const string SELECTCOUPONMESSAGE = "lclMyActiveCoupons3Resource1Text";


        #endregion
        # region My contact preference
        public const string CONTACTPREFERENCE_LABLEONE = "lclConfirmcontactResource1Text";
        public const string CONTACTPREFERENCE_LABLETWO = "lclMyContactPrefResource1Text";
        public const string CONTACTPREFERENCE_VALIDATIONMESSAGE = "lclMyContactPrefResource1Text";
        public const string CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG = "lclConfirmSaveMsgResource1Text";

        #endregion
        # region Option and Benifits

        public const string OPTIONANDBENIFITS_VALIDATIONMESSAGE = "lclSaveMsg1Resource1Text";
       
        #endregion

        #region Join
        public const string ERRORPLEASECORRECTINFO = "errCorrectInformation";
        public const string ERRORTITLE = "lcltittleErrMsgResource1Text";
        public const string ERRORNAME1 = "errValidFirstName";
        public const string ERRORNAME2 = "errValidLetter";
        public const string ERRORNAME3 = "errValidSurname";
        public const string ERRORGENDER = "errSelectGender";
        public const string ERRORDOB = "errValidDOB";
        public const string ERRORPOSTCODE_UK = "errPostcodeRequired";//errValidPostCode
        public const string ERRORPOSTCODE_GC = "errValidPostCode";//errValidPostCode
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
        public const string PD_HOUSEHOLDAGEYOU = "lclAgeText";
        public const string PD_HOUSEHOLDAGEPERSON2 = "lclPerson2Text";
        public const string PD_HOUSEHOLDAGEPERSON3 = "lclPerson3Text";
        public const string PD_HOUSEHOLDAGEPERSON4 = "lclPerson4Text";
        public const string PD_HOUSEHOLDAGEPERSON5 = "lclPerson5Text";
        public const string PD_HOUSEHOLDAGEPERSON6 = "lclPerson6Text";
        public const string PD_HOUSEHOLDTEXTYEAR = "ddlHouseholdYearResource1Text";
        public const string PD_HOUSEHOLDMEMBER1YEAR = "lclSelectYearText";



        #endregion
        #region OrderAReplacement

        public const string ORDERREPLACEMENT_SORRY = "lclonlinehelpResource1Text";
        public const string ORDERREPLACEMENT_CUSTOMERCARE = "lclcardcuscardResource1Text";
        public const string ORDERREPLACEMENT_COMMMSG1 = "lclCommunicateMsg1Resource1Text";
        public const string ORDERREPLACEMENT_COMMMSG2 = "lclCommunicateMsg2Resource1Text";

        #endregion
        #region ViewMYCards
        public const string VALIDATIONMESSAGEFORMAIN = "CustomerTypeMain";
        public const string VALIDATIONMESSAGEFORASSO = "CustomerTypeAssoc";
        public const string VALIDATIONMESSAGEFORMAINCARDHOLDER = "CardHolder";
        public const string VALIDATIONTYPEOFCARD = "lclTypeofcardResource1";
        public const string VALIDATIONWHEREUSED = "lclWhereResource1";
        public const string VALIDATIONMORESIXMONTHS = "lclMore.valid";
        public const string VALIDATIONANDSEPRATOR = "AndSeprator";
        public const string VALIDATAIONNAMEOFACCOUNT = "lclNameAccResource1Text";

        #endregion

        #region Xmus_Saver
        public const string Xmus_ERRORPOSTCODE1 = "sorryvalid";
        #endregion
    }
}

