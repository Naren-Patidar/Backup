using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCODundeeApplication
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
        //LCM changes
        public const int BONUSCOUPON_BYMAIL = 49;
        //LCM changes
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
        //LCM changes
        public const int BONUSCOUPON_MAIL = 50;
        public const int BONUSCOUPON_EMAIL=51;
        public const int BONUSCOUPON_PHONE=52;
        public const int BONUSCOUPON_SMS=53;
        //LCM changes
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

        #region Club Details
        public const int CLUB_BT = 1;
        public const int CLUB_VIRGIN = 2;
        public const int CLUB_BA = 3;
        public const int CLUB_AVIOS = 4;
        #endregion

        #region CustomerMailStatus
        public const int CUSTOMERMAILSTATUS_MAILABLE = 1;
        public const int CUSTOMERMAILSTATUS_SKELETON = 2;
        public const int CUSTOMERMAILSTATUS_ADDRESSINERROR = 3;
        public const int CUSTOMERMAILSTATUS_NONMAILABLE = 4;

        //For Email and Mobile Status.
        public const int CUSTOMERMAILSTATUS_MISSING = 6;
        public const int CUSTOMERMAILSTATUS_DELIVERABLE = 7;
        public const int CUSTOMERMAILSTATUS_INERROR = 8;

        public const int CUSTOMERMAILADDSTATUS_MISSING = 4;
        public const int CUSTOMERMAILADDSTATUS_DELIVERABLE = 1;
        public const int CUSTOMERMAILADDSTATUS_INERROR = 3;

        #endregion

        #region Customer Email and Mobile Stauts Constants

       // public const string CUST_MAIL_STATUS_MAILABLE = "Mailable";
        //public const string CUST_MAIL_STATUS_ADDRESSINERROR = "Address IN Error";
       // public const string CUST_MAIL_STATUS_NonMailable = "Non Mailable";

        //public const string CUST_EMAIL_STATUS_MISSING = "Missing";
        //public const string CUST_EMAIL_STATUS_DELIVERABLE = "Deliverable";
        //public const string CUST_EMAIL_STATUS_INERROR = "In Error";

       // public const string CUST_MOBILE_STATUS_MISSING = "Missing";
        //public const string CUST_MOBILE_STATUS_DELIVERABLE = "Deliverable";
       // public const string CUST_MOBILE_STATUS_INERROR = "In Error";

        #endregion


        #region Customer Email and Mobile Stauts Constants
        //commented by Lakshmi for localization.

         //public const string CUSTOMER_USESTATUS_SKELETON= "Skeleton";
         //public const string CUSTOMER_USESTATUS_ACTIVE = "Active";
         //public const string CUSTOMER_USESTATUS_BANNED = "Banned";
         //public const string CUSTOMER_USESTATUS_LEFTSCHEME = "Left Scheme";
         //public const string CUSTOMER_USESTATUS_DATAREMOVED = "Data removed";
         //public const string CUSTOMER_USESTATUS_DUPLICATE = "Duplicate";
         //public const string CUSTOMER_USESTATUS_UNDERAGE = "Underage";
         //public const string CUSTOMER_USESTATUS_UNSIGNED = "Unsigned";
         //public const string CUSTOMER_USESTATUS_UNDERAGEANDUNSIGNED = "Underage and Unsigned";
         //public const string CUSTOMER_USESTATUS_POSSIBLEFRAUD = "Possible Fraud";
         //public const string CUSTOMER_USESTATUS_OTHER = "Other";
         //public const string CUSTOMER_USESTATUS_MANUALENTRY = "Manual Entry";
         //public const string CUSTOMER_USESTATUS_PENDINGACTIVATION = "Pending Activation";
         //public const string CUSTOMER_USESTATUS_CARDLESS = "Cardless";
         //public const string CUSTOMER_USESTATUS_PROFANITYERROR = "Profanity Error";
         //public const string CUSTOMER_USESTATUS_INACTIVE= "Inactive";
         //public const string CUSTOMER_USESTATUS_DECEASED= "Deceased";
        #endregion

        #region CustomerUseStatus

        public const int CUSTOMERUSESTATUS_SKELETON= 0;
        public const int CUSTOMERUSESTATUS_ACTIVE = 1;
        public const int CUSTOMERUSESTATUS_BANNED = 2;
        public const int CUSTOMERUSESTATUS_LEFTSCHEME = 3;
        public const int CUSTOMERUSESTATUS_DATAREMOVED = 4;
        public const int CUSTOMERUSESTATUS_DUPLICATE = 5;
         public const int CUSTOMERUSESTATUS_UNDERAGE = 6;
         public const int CUSTOMERUSESTATUS_UNSIGNED = 7;
        public const int CUSTOMERUSESTATUS_UNDERAGEANDUNSIGNED = 8;
         public const int CUSTOMERUSESTATUS_POSSIBLEFRAUD = 9;
         public const int CUSTOMERUSESTATUS_OTHER = 10;
         public const int CUSTOMERUSESTATUS_MANUALENTRY = 11;
         public const int CUSTOMERUSESTATUS_PENDINGACTIVATION = 12;
         public const int CUSTOMERUSESTATUS_CARDLESS = 13;
         public const int CUSTOMERUSESTATUS_PROFANITYERROR = 14;
         public const int CUSTOMERUSESTATUS_INACTIVE= 15;
        public const int CUSTOMERUSESTATUS_DECEASED= 16;
        #endregion
    }
}
