namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Common
{
    public class BusinessConstants
    {
        #region Your Preferences Constants
        public const int STANDARD_BAMILES = 600;
        public const int PRIMIUM_BAMILES = 800;
        public const int STANDARD_AMILES = 600;
        public const int PRIMIUM_AMILES = 800;
        public const int VIRGIN_ATLANTIC = 625;
        public const string VOUCHER_PERMILE = "2.50";
        public const int TESCOPROMOTION_BYMAIL = 7;
        public const int NONTESCOPROPMATION_BYMAIL = 8;
        public const int RESEARCH_BYPHONE = 9;
        public const int BAMILES_STD = 10;
        public const int AIRMILES_STD = 11;
        public const int AIRMILES_PREMIUM = 12;
        public const int XMASSAVER = 13;
        public const int BAMILES_PREMIUM = 14;
        public const int ECOUPON = 15;
        public const int SAVETREES = 16;
        public const int VIRGIN = 17;
        public const int CLUBCARD_EMAIL = 42;
        public const int EMAIL_CONTACT = 43;
        public const int MOBILE_CONTACT = 44;
        public const int POST_CONTACT = 45;
        public const int LARGEPRINT_CONTACT = 46;
        public const int BRAILLE_CONTACT = 47;
        public const int BABYTODLER_CLUB = 48;

        public const int BAMILES_REASONCODE = 3;
        public const int AIRMILES_REASONCODE = 2;
        public const int VIRGIN_REASONCODE = 11;

        public const int TESCO_GROUP_MAIL = 27;
        public const int TESCO_GROUP_EMAIL = 28;
        public const int TESCO_GROUP_PHONE = 29;
        public const int TESCO_GROUP_SMS = 30;
        public const int TESCO_THIRD_PARTY_MAIL = 31;
        public const int TESCO_THIRD_PARTY_EMAIL = 32;
        public const int TESCO_THIRD_PARTY_PHONE = 33;
        public const int TESCO_THIRD_PARTY_SMS = 34;
        public const int RESEARCH_MAIL = 35;
        public const int RESEARCH_EMAIL = 36;
        public const int RESEARCH_PHONE = 37;
        public const int RESEARCH_SMS = 38;
        #endregion

        #region View your cards
        public const string CUSTOMER_TYPECODE_MAIN = "Main";
        public const string CUSTOMER_TYPECODE_ASSOC = "Associate";
        public const int CUSTOMER_BANNED = 2;
        public const int CUSTOMER_LEFTSCHEME = 3;
        public const int CUSTOMER_DATAREMOVED = 4;
        #endregion

        #region Christmas Savers Constants
        public const int REWARDEE_LIMIT = 100;
        public const string CHRISTMAS_SAVER_DATE = "15/11";
        public const decimal VOUCHERVALUE_TOBECOMPARED = 100;
        public const decimal BONUSVOUCHERVALUE_FOR100 = 6;
        public const decimal BONUSVOUCHERVALUE_FOR50 = 3;
        public const decimal MONEY_TOBESAVED_FORBONUS = 50;
        #endregion

        #region Customer preferences
        public const int DIABETIC = 1;
        public const int KOSHER = 2;
        public const int HALAL = 3;
        public const int VEGETARIAN = 4;
        public const int TEETOTAL = 5;
        #endregion

        #region Order a replacement Constants
        public const string ORDRPL_TYPE_NEW_CARD = "1";
        public const string ORDRPL_TYPE_NEW_KEYFOB = "2";
        public const string ORDRPL_TYPE_NEW_CARD_KEYFOB = "3";
        public const string ORDRPL_RSN_LOST = "L";
        public const string ORDRPL_RSN_DAMAGED = "D";
        public const string ORDRPL_RSN_STOLEN = "S";
        public const string ORDRPL_RSN_MOREFOBS = "M";
        public const string ORDRPL_RSN_OTHER = "O";
        public const string ORDRPL_BOTHSTDNONSTD_CARDCODE = "B";
        #endregion

        #region PreferenceType
        public const int PREFERENCETYPE_ALLERGIC = 1;
        public const int PREFERENCETYPE_DIETRY = 2;
        #endregion

        #region CustomerMailStatus
        public const int CUSTOMERMAILSTATUS_MAILABLE = 1;
        public const int CUSTOMERMAILSTATUS_SKELETON = 2;
        public const int CUSTOMERMAILSTATUS_ADDRESSINERROR = 3;
        public const int CUSTOMERMAILSTATUS_NONMAILABLE = 4;
        public const int CUSTOMERMAILSTATUS_PENDINGACTIVATIONMAILABLE = 5;
        public const int CUSTOMERMAILSTATUS_MISSING = 6;
        public const int CUSTOMERMAILSTATUS_DELIVERABLE = 7;
        public const int CUSTOMERMAILSTATUS_INERROR = 8;
        #endregion

        #region CustomerUseStatus
        public const int CUSTOMERUSESTATUS_NORMAL = 0;
        public const int CUSTOMERUSESTATUS_ACTIVE = 1;
        public const int CUSTOMERUSESTATUS_BANNED = 2;
        public const int CUSTOMERUSESTATUS_LEFTSCHEME = 3;
        public const int CUSTOMERUSESTATUS_DATAREMOVED = 4;
        public const int CUSTOMERUSESTATUS_DUPLICATE = 5;
        public const int CUSTOMERUSESTATUS_INACTIVE = 6;
        #endregion

        #region Club Details
        public const int CLUB_BT = 1;
        public const int CLUB_VIRGIN = 2;
        public const int CLUB_BA = 3;
        public const int CLUB_AVIOS = 4;
        #endregion


        #region My Latest Statement
        //Author:Sabhareesan O.K
        //Date:02-March-2012

        #region Customer Type

        public const string VOUCHER_CUSTOMERS = "A";
        public const string AVOIS_BRITISHAIRWAYS_EXCLUB_CUSTOMERS = "B";
        public const string AVOIS_NON_REWARD_BRITISHAIRWAYS_EXCLUB_CUSTOMERS = "C";
        public const string AVIOS_CUSTOMERS = "D";
        public const string AVIOS_NON_REWARD_CUSTOMERS = "E";
        public const string CHRISTMAS_SAVERS_REWARD_CUSTOMERS = "F";
        public const string CHRISTMAS_SAVERS_NON_REWARD_CUSTOMERS = "G";
        public const string NON_VOUCHER = "H";
        public const string VIRGIN_FREQUENT_FLYERS_CUSTOMERS = "I";
        public const string VIRGIN_FREQUENT_FLYERS_NON_REWARD_CUSTOMERS = "J";
        public const string NON_MAILED_CUSTOMERS = "K";
        public const string NEW_CUSTOMER_CUSTOMERS = "L";
        public const string NEW_NON_VOUCHER_CUSTOMERS = "M";
        #endregion

        #region Avios and BA Avios Text Messages

        public const string AVIOS_MESSAGE_CONTENT = "Avios Text message content comes here";
        public const string BAAVIOS_MESSAGE_CONTENT = "BA Avios Text message content comes here";

        #endregion

        #region Virgin Atlantic Message

        public const string VIRGIN_ATLANTIC_MESSAGE_CONTENT = "Virgin Atlantic Message content comes here";

        #endregion

        #region Video CSS
        public const string CSS_VideoTV = "videotv";
        public const string CSS_Tablet = "tablet";
        #endregion

        #region Avios,BA Avios and Virgin Conversion Rate - Only for MLS Page
        //To display the Additional Customer Message

        //For Fifty Clubcard Point
        public const int VOUCHER_PERMILE_FIFTY = 50;

        #endregion




        #region Statement Type for Showing Marketing Material

        //Showing Rewards and Exchange
        public const string PAPER_STTYPE_1 = "RD,RP,HV,HVC,UV,EDP,REC,REPMP,EP";
        //Showing Rewards and Online/instore
        public const string PAPER_STTYPE_2 = "NEW,NNV,NV,NVC,TDNV,TD,TDT,GON,NM";



        #endregion
        #endregion

        
    }
}
