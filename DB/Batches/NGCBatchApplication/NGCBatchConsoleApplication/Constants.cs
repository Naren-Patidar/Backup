using System;
using System.Collections.Generic;
using System.Text;

namespace Tesco.NGC.BatchConsoleApplication
{
    /// <summary>
    /// Container for all constants. 
    /// eg. Stored Procedure Name
    /// </summary>
    public class Constants
    {
        #region Validation Formats

        public const string DATE_FORMATE_123 = "^((\\d{2}(([02468][048])|([13579][26]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][1235679])|([13579][01345789]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\\s(((0?[1-9])|(1[0-2]))\\:([0-5][0-9])((\\s)|(\\:([0-5][0-9])\\s))([AM|PM|am|pm]{2,2})))?$";
        public const string DATE_FORMATE = "^((\\d{2}(([02468][048])|([13579][26]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][1235679])|([13579][01345789]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\\s(((0?[0-2])|(1[0-9]))\\:([0-5][0-9])((\\s)|(\\:([0-5][0-9]))(\\.([0-9][0-9][0-9])))))?$";

        #endregion

        #region PreferenceIDs -Old Code For Reference
        //Get PreferenceID of the following. 

        //public const string PREFERENCE_ID_VEGETARIAN = "1";
        //public const string PREFERENCE_ID_HALAL = "3";
        //public const string PREFERENCE_ID_KOSHER = "4";
        //public const string PREFERENCE_ID_TEETOTAL = "5";
        //public const string PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_MAIL = "17";
        //public const string PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_PHONE = "16";
        //public const string PREFERENCE_ID_ALLOW_GROUP_PROMOTIONS = "23";
        //public const string PREFERENCE_ID_ALLOW_THIRD_PARTY_PROMOTIONS = "19";
        //public const string PREFERENCE_ID_DIABETIC = "20";
        //public const string PREFERENCE_ID_CELIAC = "24";
        //public const string PREFERENCE_ID_LACTOSE = "25";
        //public const string PREFERENCE_ID_EXPAT = "26";

        ////Added for NGCV3.1 Data Protection Preferences
        //public const string PREFERENCE_ID_TESCOGROUPMAIL = "27";
        //public const string PREFERENCE_ID_TESCOGROUPEMAIL = "28";
        //public const string PREFERENCE_ID_TESCOGROUPPHONE = "29";
        //public const string PREFERENCE_ID_TESCOGROUPSMS = "30";
        //public const string PREFERENCE_ID_PARTNERMAIL = "31";
        //public const string PREFERENCE_ID_PARTNEREMAIL = "32";
        //public const string PREFERENCE_ID_PARTNERPHONE = "33";
        //public const string PREFERENCE_ID_PARTNERSMS = "34";
        //public const string PREFERENCE_ID_RESEARCHMAIL = "35";
        //public const string PREFERENCE_ID_RESEARCHEMAIL = "36";
        //public const string PREFERENCE_ID_RESEARCHPHONE = "37";
        //public const string PREFERENCE_ID_RESEARCHSMS = "38";

        ////Added for V3.1.1 fro optional dietary preferences.
        //public const string PREFERENCE_ID_OPTIONAL1 = "39";
        //public const string PREFERENCE_ID_OPTIONAL2 = "40";
        //public const string PREFERENCE_ID_OPTIONAL3 = "41";


        #endregion

        #region PreferenceIDs
        //Get PreferenceID of the following. 
        //From NGC 3.6 onwards, Preference table will Consistency between UK and group countries

        public const string PREFERENCE_ID_VEGETARIAN = "4";
        public const string PREFERENCE_ID_HALAL = "3";
        public const string PREFERENCE_ID_KOSHER = "2";
        public const string PREFERENCE_ID_TEETOTAL = "5";
        public const string PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_MAIL = "27";
        public const string PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_PHONE = "29";
        public const string PREFERENCE_ID_ALLOW_GROUP_PROMOTIONS = "23";
        public const string PREFERENCE_ID_ALLOW_THIRD_PARTY_PROMOTIONS = "31";
        public const string PREFERENCE_ID_DIABETIC = "1";
        public const string PREFERENCE_ID_CELIAC = "24";
        public const string PREFERENCE_ID_LACTOSE = "25";
        public const string PREFERENCE_ID_EXPAT = "26";

        //Added for NGCV3.1 Data Protection Preferences
        //From NGC 3.6 onwards, Preference table will Consistency between UK and group countries
        public const string PREFERENCE_ID_TESCOGROUPMAIL = "27";
        public const string PREFERENCE_ID_TESCOGROUPEMAIL = "28";
        public const string PREFERENCE_ID_TESCOGROUPPHONE = "29";
        public const string PREFERENCE_ID_TESCOGROUPSMS = "30";
        public const string PREFERENCE_ID_PARTNERMAIL = "31";
        public const string PREFERENCE_ID_PARTNEREMAIL = "32";
        public const string PREFERENCE_ID_PARTNERPHONE = "33";
        public const string PREFERENCE_ID_PARTNERSMS = "34";
        public const string PREFERENCE_ID_RESEARCHMAIL = "35";
        public const string PREFERENCE_ID_RESEARCHEMAIL = "36";
        public const string PREFERENCE_ID_RESEARCHPHONE = "37";
        public const string PREFERENCE_ID_RESEARCHSMS = "38";

        //Added for V3.1.1 fro optional dietary preferences.
        public const string PREFERENCE_ID_OPTIONAL1 = "39";
        public const string PREFERENCE_ID_OPTIONAL2 = "40";
        public const string PREFERENCE_ID_OPTIONAL3 = "41";


        #endregion

        #region Parameters
        public const int MAX_PARAMETERS = 3;
        public const string PARAMETER_USER = "u";
        public const string PARAMETER_PWD = "p";
        public const string PARAMETER_SCRIPT = "s";
        public const string PARAMETER_LIST = "l";
        public const string PARAMETER_HELP = "?";
        #endregion

        #region EventViewer

        public const string EVENTVIEWER_IMPORT_CUSTOMER = "NGC Batch Register Customers";
        public const string EVENTVIEWER_AGGREGATE_OFFER = "NGC Aggregate Offers";
        public const string EVENTVIEWER_AGGREGATE_CUSTOMER = "NGC Aggregate Customers";
        public const string EVENTVIEWER_AGGREGATE_TRANSACTION = "NGC Aggregate Transactions";
        public const string EVENTVIEWER_IMPORT_TRANSACTION = "NGC Import Transactions";

        #endregion

        #region Script Name
        public const string ACTION_IMPORT_CUSTOMER = "import_customer";
        public const string ACTION_TRANSACTION_EXTRACTS = "import_customer_my1";
        public const string ACTION_AGGREGATE_CUSTOMER = "aggregate_customers";
        public const string ACTION_AGGREGATE_OFFER = "aggregate_offers";
        public const string ACTION_AGGREGATE_TRANSACTION = "aggregate_txns";
        public const string ACTION_IMPORT_TRANSACTION = "import_transaction";

        public const string BATCH_HOUSEKEEPING = "housekeeping";
        public const string BATCH_REISSUE_REWARD = "reissue_rewards";
        public const string BATCH_REISSUE_REWARDS = "reissue_high_rewards";
        public const string BATCH_REWARD_MAILING = "reward_mailing";
        public const string BATCH_CALCULATE_REWARDS = "calculate_rewards";
        public const string BATCH_CUSTOMER_INSIGHT_EXTRACT = "migrate_customers_reverse";
        public const string BATCH_POINTS_PARTNER_TRANSACTION_EXTRACT = "migrate_txn4pp_reverse";
        public const string BATCH_CARD_ACCOUNT_EXTRACT = "migrate_card_accounts_reverse";
        public const string BATCH_AGENCY_PARTNER_EXTRACT = "migrate_partner_agency";
        public const string BATCH_ADDRESS_IN_ERROR = "address_in_error";
        public const string BATCH_UPDATECUSTOMERSTATUS_MAILSTATUS = "UpdateCustomerStatus_MailStatus";
        public const string BATCH_UPDATE_CUSTOMER_PREFERENCE = "Update_DataProtection_Preference";
        //added by girish benahal on may 31st 2010 to add the functionality "deleteing trace files older than 30 days"
        public const string ACTION_DELETE_TRACE_FILES = "delete_trace_files";
        #endregion

        #region Stored Procedure Names
        public const string SP_UPDATE_CUSTOMER_DETAILS = "USP_Import_Customer";
        public const string SP_GET_CURRENT_OFFERID = "Get_Current_OfferID";
        public const string SP_MIGRATE_REVERSE_TXN_CREATE = "sp_migrate_reverse_txn_create";
        public const string SP_MIGRATE_REVERSE_TXN = "sp_migrate_reverse_txn";
        public const string SP_MIGRATE_REVERSE_TXN_EXPORT = "sp_migrate_reverse_txn_export";
        public const string SP_AGGREGATE_CUSTOMERS = "USP_AggregateCustomer";
        public const string SP_AGGREGATE_OFFERS = "USP_Aggregate_Offers";
        public const string SP_MIGRATE_REVERSE_CARD_ACCOUNT_CREATE = "USP_MigrateReverseClubcardCreate";
        public const string SP_MIGRATE_REVERSE_CARD_ACCOUNT = "USP_MigrateReverseClubcard";
        public const string SP_MIGRATE_REVERSE_CARD_ACCOUNT_EXPORT = "USP_MigrateReverseClubcardExport";
        public const string SP_MIGRATE_AGENCY_PARTNER_CREATE = "sp_migrate_agency_partner_create";
        public const string SP_MIGRATE_AGENCY_PARTNER = "sp_migrate_agency_partner";
        public const string SP_MIGRATE_AGENCY_PARTNER_EXPORT = "sp_migrate_agency_partner_export";
        public const string SP_AGGREGATE_TRANSACTION = "USP_Aggregate_Txns";
        public const string SP_IMPORT_TRANSACTION = "USP_ImportTxn";
        public const string SP_AUDIT_CUSTOMER_VALIDATION = "USP_AuditCustomerValidation";



        #endregion

        #region CalculateRewards

        //Stored Procedure 
        public const string CalculateRewards = "USP_CalculateRewards";
        public const string GetPreviousOffer = "USP_GetPreviousOffer";
        public const string UpdateOffer4Rewards = "USP_UpdateOffer4Rewards";
        public const string CalculareRewardsProcess2 = "USP_CalculateRewardProcess2";
        #endregion

        #region HouseKepping
        //Stored Procedure        
        public const string SP_HOUSEKEEPING_INITIALISE = "USP_HousekeepingInitialise";
        public const string SP_HOUSEKEEPING_WRITELINE = "USP_HousekeepingWriteline";
        public const string SP_HOUSEKEEPING_WRITE_OFFER_XML = "USP_HousekeepingWriteOfferXml";
        public const string SP_HOUSEKEEPING_EXPORT = "USP_HousekeepingExport";
        public const string SP_HOUSEKEEPING_OFFER = "USP_HousekeepingOffer";
        public const string SP_HOUSEKEEPING_WRITELE_TXN_XML = "USP_HousekeepingWriteTxnXml";
        public const string SP_HOUSEKEEPING_WRITE_TRANSACTION_ALL_IN_ONE = "USP_HousekeepingTxn";
        public const string SP_HOUSEKEEPING_WRITE_OFFER_ALL_IN_ONE = "USP_HousekeepingClubcardOffer";
        public const string GetCurrentOffer = "USP_HousekeepingGetCurrentOffer";

        #endregion

        #region ReIssueRewards
        public const string EVENTVIEWER_REISSUE_REWARD = "NGC Batch Reissue high Rewards";
        public const string EVENTVIEWER_REISSUE_NORMAL_REWARD = "NGC Batch Reissue Rewards";

        #endregion

        #region RewardMailing

        //Stored Procedure  
        public const string CreateHighRewardMailingXml = "USP_CreateHighRewardXml";
        public const string RewardMailingInitialize = "USP_RewardMailingInitialize";
        public const string RewardMailing = "USP_RewardMailing";
        public const string RewardMailingExport = "USP_RewardMailingExport";
        public const string UpdateOffer4Mailing = "USP_UpdateOffer4Mailing";

        //New SPs for Reward mailing      
        public const string UpdatePrevOfferMailStatusCode = "USP_UpdatePrevOfferFullMailStatus";
        public const string UpdatePrevOfferDetails = "USP_UpdatePrevOfferDetails";

        #endregion

        #region CustomerInsightExtract
        //Stored Procedure
        public const string SP_MIGRATE_REVERSE_CUSTOMER_CREATE = "sp_migrate_reverse_customer_create";
        public const string SP_MIGRATE_REVERSE_CUSTOMER = "sp_migrate_reverse_customer";
        public const string SP_MIGRATE_REVERSE_CUSTOMER_EXPORT = "sp_migrate_reverse_customer_export";
        #endregion

        #region Points Partner Transaction Extract

        public const string SP_MIGRATE_REVERSE_TXN4PP_CREATE = "sp_migrate_reverse_txn4pp_create";
        public const string SP_MIGRATE_REVERSE_TXN4PP = "sp_migrate_reverse_txn4pp";
        public const string SP_MIGRATE_REVERSE_TXN4PP_EXPORT = "sp_migrate_reverse_txn4pp_export";
        public const string GET_CURRENT_OFFERID = "Get_Current_OfferID";
        //For Log File 
        public const string LOG_POINTS_PARTNER_TRANS = "pointspartnerTransaction";

        #region ReIssue Rewards
        public const string ReIssueNormalRewardInitialise = "USP_ReIssueNormalRewardInitialise";
        public const string ReIssueNormalRewards = "USP_ReIssueNormalRewards";
        public const string ReIssueNormalRewardExport = "USP_ReIssueNormalRewardExport";

        #endregion

        #region ReIssue High Rewards Rewards
        public const string ReIssueHighRewardInitialise = "USP_ReIssueHighRewardInitialise";
        public const string ReIssueHighRewards = "USP_ReIssueHighRewards";
        public const string ReIssueHighRewardExport = "USP_ReIssueHighRewardExport";


        #endregion
        #endregion


    }
}
