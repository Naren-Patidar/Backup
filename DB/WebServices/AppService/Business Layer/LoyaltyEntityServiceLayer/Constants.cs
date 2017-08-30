#region using

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{

    /// <summary>
    /// Container for all constants. 
    /// eg. Stored Procedure Name
    /// </summary>
    public class Constants
    {

        #region Stores
        
        public const string STORE_ID = "TescoStoreID";
        public const string STORE_NUMBER = "TescoStoreNumber";
        public const string STORE_NAME = "TescoStoreName";
        // Added for V312 development by Netra [Req.No:002a]
        public const string STORE_FORMAT_ID = "StoreFormatID";
        //Change Complete
        public const string STORE_WELCOME_POINTS = "store_welcome_points";
        public const string STORE_REGION_ID = "StoreRegionID";

        public const string SP_VIEW_STORES = "USP_ViewStores";
        public const string SP_ADD_STORE_DETAILS = "USP_AddNewStore";
        public const string SP_UPDATE_STORE_DETAILS = "USP_AmendStore";
        public const string SP_VIEW_AGENCY = "USP_ViewAgency";
        public const string SP_ADD_Agency = "USP_AddNewAgency";
        public const string SP_UPDATE_AGENCY = "USP_AmendAgency";
        public const string SP_SEARCH_AGENCY_AND_PARTNER = "USP_SearchAgecnyPartner";
        public const string SP_GET_ASSOCIATED_PARTNERS = "USP_SearchAssociatedPartners";

        public const string SP_VIEW_STOREFORMAT = "USP_Get_StoreFormat";
        public const string SP_VIEW_STOREREGION = "USP_Get_Region";
        public const string SP_GET_STORENAME = "USP_Get_StoreName";

        public const string SP_GET_CUSTOMERS = "USP_GetCustomers";
        public const string SP_VIEW_CUSTOMER_DETAILS = "USP_ViewCustomerDetails";
        public const string SP_VIEW_CUSTOMER_FAMILY_DETAILS = "USP_ViewCustomerFamilyDetails";
        public const string SP_VIEW_TRANSACTIONS_BY_OFFER_ID = "USP_ViewTransactionsByOfferID";

        //CSC Changes
        public const string SP_VIEW_TRANSACTIONS_BY_OFFER_ID_CSC = "USP_ViewTransactionsByOfferIDCSC";
        public const string SP_VIEW_TRANSACTIONS = "USP_ViewTransactions";
        public const string SP_UPDATE_CUSTOMER_DETAILS = "USP_UpdateCustomerDetails";
        public const string SP_ADD_CUSTOMER_DETAILS = "USP_CsaCreateCustomerSkeleton";
        public const string SP_VIEW_CUSTOMER_DIETARY_DETAILS = "USP_ViewCustomerDietaryDetails";
        public const string SP_UPDATED_CUSTOMER_INFORMATION = "USP_UpdatedCustomerInformation";
        public const string SP_VIEW_CUSTOMER_DATAPROTECTION_DETAILS = "USP_ViewCustomerDataProtectionDetails";
        public const string SP_ADD_CUSTOMER_HELPLINE = "USP_LogCall";
        public const string SP_VIEW_CUSTOMER_HELPLINE_INFORMATION = "USP_ViewCallHistory";
        public const string SP_VIEW_CUSTOMER_CALLDETAIL_INFORMATION = "USP_ViewCallDetail";

        // added as part of CCO convergence        

        public const string SP_GET_CUSTOMER_FORDUNDEE = "USP_GetCustomersforDundee";
        public const string SP_GET_TITLES = "USP_Get_TitleEnglish";
        public const string SP_LOAD_PREFERENCES = "USP_LoadPrefernces";
        #endregion

        #region StoreIPAddress
                     
        public const string IP_ADDRESS1 = "posipaddress1";
        public const string IP_ADDRESS2 = "posipaddress2";
        public const string IP_ADDRESS3 = "ngcipaddress1";
        public const string IP_ADDRESS4 = "ngcipaddress2";
        public const string IP_ADDRESS5 = "ngcipaddress3";
        public const string IP_ADDRESS6 = "ngcipaddress4";


        public const string PORT1 = "port1";
        public const string PORT2 = "port2";
        public const string PORT3 = "port3";
        public const string PORT4 = "port4";


        #endregion

        #region Customer
        public const string CUSTOMER_PREFFERED_STORE_ID = "PreferredStoreID";
        public const string CUSTOMER_JOINED_DATE = "JoinedDate";
        public const string DEFAULT_DATA_PROTECTION_PREF = "DataProtectionDefault";
        public const string CUSTOMER_ID = "CustomerID";
        public const string CUSTOMER_CLUBCARD_ID = "cardAccountNumber";
        public const string CUSTOMER_NAME1 = "Name1";
        public const string CUSTOMER_NAME2 = "Name2";
        public const string CUSTOMER_USE_STATUS_ID = "CustomerUseStatusID";
        public const string CUSTOMER_MAILING_ADDRESS_1 = "MailingAddressLine1";
        public const string CUSTOMER_MAILING_ADDRESS_2 = "MailingAddressLine2";
        public const string CUSTOMER_MAILING_ADDRESS_3 = "MailingAddressLine3";
        public const string CUSTOMER_MAILING_ADDRESS_4 = "MailingAddressLine4";
        //public const string CUSTOMER_MAILING_ADDRESS_5 = "province_code";
        //NGC 3.6 Address field for group country
        public const string CUSTOMER_MAILING_ADDRESS_5 = "MailingAddressLine5";
        public const string CUSTOMER_MAILING_ADDRESS_6 = "MailingAddressLine6";
        public const string CUSTOMER_MAILING_POST_CODE = "MailingAddressPostCode";

        public const string CUSTOMER_BUSINESS_ADDRESS_1 = "BusinessAddressLine1";
        public const string CUSTOMER_BUSINESS_ADDRESS_2 = "BusinessAddressLine2";
        public const string CUSTOMER_BUSINESS_ADDRESS_3 = "BusinessAddressLine3";
        public const string CUSTOMER_BUSINESS_ADDRESS_4 = "BusinessAddressLine4";
        public const string CUSTOMER_BUSINESS_ADDRESS_5 = "BusinessAddressLine5";
        public const string CUSTOMER_BUSINESS_ADDRESS_6 = "BusinessAddressLine6";
        public const string CUSTOMER_BUSINESS_POST_CODE = "BusinessAddressPostCode";

        public const string CUSTOMER_DAY_TIME_PHONE_NUMBER = "daytime_phone_number";
        public const string CUSTOMER_EVENING_PHONE_NUMBER = "evening_phone_number";
        public const string CUSTOMER_MOBILE_NUMBER = "mobile_phone_number";
        public const string CUSTOMER_EMAIL = "email_address";
        public const string CUSTOMER_DOD = "family_member_1_dob";
        public const string CUSTOMER_SEX = "Sex";
        public const string CUSTOMER_TITLE_ENGLISH = "TitleEnglish";
        public const string CUSTOMER_FAX = "FaxNumber";
        public const string CUSTOMER_RACE_ID = "RaceID";
        public const string CUSTOMER_PREFRENCE_ID = "preferred_contact_type_code";
        public const string CUSTOMER_PREFERRED_MAILING_ADDRESS_ID = "preferred_mailing_address_flag";
        public const string CUSTOMER_ALTERNATEID = "official_id";
        public const string CUSTOMER_MAIL_STATUS = "CustomerMailStatus";
        public const string CUSTOMER_BUSINESS_TYPE = "BusinessType";
        public const string CUSTOMER_BUSINESS_REG_NO = "BusinessRegistrationNumber";
        public const string CUSTOMER_BUSINESS_NAME = "BusinessName";
        public const string CUSTOMET_SSN = "SSN";
        public const string CUSTOMER_EXPAT = "Expat";
        /*below constants are added as part of convergence*/
        public const string IS_XMAS_SAVER_CUSTOMER = "IsXmasSaverCustomer";
        public const string SP_UPDATE_CUSTOMER_DETAILS_ORCHESTRATION = "USP_UpdateCustomerDetails_Orchestration";
        public const string SP_VIEW_CUSTOMER_PREFERENCES = "USP_Get_Customer_Preferences";
        public const string SP_UPDATE_CUSTOMER_PREFERENCES = "USP_Update_MailingStatementPreferences";
        public const string SP_IS_XMAS_SAVER_CUSTOMER = "USP_Get_IsXmasSaverCustomer";
        public const string SP_VIEW_XMAS_SAVER_SUMMARY = "USP_Get_XmasSaverSummary";
        public const string USP_GETCLUBCARDREGISTRATIONREPORT = "USP_GetClubcardRegistrationReport";
        public const string USP_GETPROMOTIONALCODEREPORT = "USP_GetPromotionCodeReport";
        public const string USP_GETCUSTOMERLOADREPORT = "USP_GetCustomerLoadReport";
        public const string CUSTOMER_NAME3 = "Name3";
        public const string CUSTOMER_DOB = "DateOfBirth";
       
        //below constants are added as part of USLoyalty
        public const string FIRST_NAME = "Fstname";
        public const string LAST_NAME = "Lstname";
        public const string EMAIL_ID = "Emailid";
        public const string PASSWORD = "Password";
        public const string SECRET_QUESTION = "secretqtn";
        public const string SECRET_ANSWER = "secretans";
        public const string ZIP_CODE = "zipcode";
        public const string STORELOCATOR_ID = "storeLocatorId";
        public const string PHONE_NO = "phoneno";
        public const string CUST_ID = "custid";
        public const string ALTERNATE_ID = "altidtype";
        public const string EXISTINGCUSTOMER_ID = "CustomerID";
        public const string CLUBCARDID = "clubcardid";
        public const string YEAR = "year";
        public const string MONTH = "month";
        public const string MODE = "mode";
        public const string WELCOMEPOINTS = "welcomepoints";
        public const string SP_GET_SECRETQUESTIONS = "USP_Get_SecretQuestions";
        public const string SP_INSERT_CUSTOMER_DETAILS = "USP_InsertCustomerDetails";
        public const string SP_ROLLBACK_CUSTOMER_DETAILS = "USP_RollBackCustomerData";
        public const string SP_UPDATE_CUSTOMER_STATUS = "USP_Update_CustomerStatus";
        public const string SP_GET_CUSTOMERDETAILS = "USP_Get_Customer_Details";
        public const string SP_GET_FRIENDCUSTOMERDETAILS = "USP_Get_FriendCustomer_Details";
        public const string SP_UPDATE_EXISTING_CUSTOMER_DETAILS = "USP_UpdateExistingCustomerDetails";
        public const string SP_ADD_PHONENO_TO_ACCOUNT = "USP_AddPhoneNoToAccount";
        public const string SP_GET_PRIMARY_CARD = "USP_GetPrimaryCard";
        public const string SP_CHECK_DUPLICATE_EMAILID = "USP_CheckDuplicateEmailId";
        public const string SP_CHECK_DUPLICATE_PHONENO = "USP_CheckDuplicatePhoneNo";
        public const string SP_CHECK_DUPLICATE_ALTERNATEID = "USP_CheckDuplicateAlternateId";
        public const string SP_GET_CUSTID_BY_EMAILID = "USP_GetCustIdByEmailId";
        public const string USP_INSERT_CLUBCARD = "Usp_InsertClubCard";
        public const string USP_GET_CARDNO_BY_CUSTID = "USP_GetCardNoByCustId";
        public const string USP_GET_TRANSACTION_HISTORY = "USP_GetTransactionHistory";
        public const string USP_GET_TRANSACTION_BY_MON_YEAR = "USP_GetTransactionByMonYear";
        public const string SP_UPDATE_MYPROFILE_DETAILS = "USP_UpdateMyProfileDetails";
        public const string SP_UPDATE_USEMAIL_STATUS = "USP_UpdateUseMailStatus";
        public const string SP_GET_USEMAILStatus = "USP_GetUseMailStatus";

        //NGC Changes Email and Phone No Search   
        public const string CUSTOMER_EMAIL_ADDRESS = "Email";
        public const string CUSTOMER_PHONE_NUMBER = "PhoneNumber";
        //NGC Changes

        //NGC Join Changes
        public const string SP_CREATE_PENDINGCUSTOMER = "USP_CreatePendingCustomer_UK";
        public const string SP_CREATE_CUSTOMER = "USP_CreateCustomer";

        #region Constants F&E CustomerDetails -sathesh

        public const string SP_GET_MYPROFILE_DETAILS = "USP_GetMyProfileDetails";

        public const string DAY = "Day";
        public const string DOBMONTH = "Month";
        public const string TESCOGROUPEMAILID = "TescoGroupEmailFlag";
        public const string PARTNEREMAILID = "PartnerEmailFlag";
        public const string RESEARCHEMAILID = "ResearchEmailFlag";
        public const string PREFFERED_STOREID = "PreferredStoreID";

        public const string DIETARYPREFERENCEINPUT = "PreferenceInput";

        public const string SP_GET_USP_GETACCOUNTOVERVIEWDETAILS = "USP_GetAccountOverviewDetails";

        public const string SP_UPDATE_EXISTING_CUSTOMER_PROFILE_DETAILS = "USP_UpdateExistingCustomerProfileDetails";

        public const string SP_GETABOUTMEDETAILS = "USP_GetAboutMeDetails";

        public const string SP_UPDATEABOUTMEDETAILS = "USP_UpdateAboutMeDetails";

        public const string SP_UPDATEDIETARYPREFERENCES = "USP_UpdateDietaryPreferences";

        public const string SP_ADD_SUPPLEMENTARY_CLUBCARD = "USP_AddSupplementaryCard";

        public const string SP_UPDATE_SUPPLEMENTARY_CLUBCARD = "USP_AmendSupplementaryCard";

        public const string SP_UPDATE_EMAILPREFERENCES = "USP_UpdateDataProtectionDetails";

        public const string SP_GETEMAILPREFERENCES = "USP_GetEmailPreferences";

        public const string SP_ADD_CUSTOMER = "USP_AddCustomer";
        // MCA NEW constants for convergence
        public const string SP_INSERT_PRINTDETAILS = "USP_InsertPrintDetails";
        public const string SP_UPDATE_CONFIGDETAILS = "USP_Update_ConfigDetails";
        public const string SP_GET_CONFIGDETAILS = "USP_Get_ConfigDetails";
        #endregion


        #region GetAccountOverviewDetails
        /*Menu*/
        //public const string SP_GET_USP_GETACCOUNTOVERVIEWDETAILS = "USP_GetAccountOverviewDetails";
        /*Menu*/
        #endregion

        #endregion

        #region ClubcardRange

        public const string SP_VIEW_CLUBCARDRANGE = "USP_ViewClubcardRangeDetails";
        public const string SP_ADD_CLUBCARDRANGE = "USP_AddClubcardRange";  //USP_AddCardRange
        public const string SP_UPDATE_CLUBCARDRANGE = "USP_AmendClubcardRange"; //USP_UpdateCardRange
        public const string SP_DELETE_CLUBCARDRANGE = "USP_DeleteUpdateCardRange";
        public const string SP_VIEW_CLUBCARDRANGE_POPUP = "USP_GetPopUpCardRange";

        public const string MIN_CARD_NUMBER = "MinCardNumber";
        public const string MAX_CARD_NUMBER = "MaxCardNumber";
        public const string CARD_TYPE = "ClubCardType";
        public const string CARD_RANGE_ID = "ClubcardRangeID";

        #endregion

        #region ClubcardType
        public const string CLUBCARD_TYPE = "ClubcardType";
        public const string CLUBCARD_NUMBERLENGTH = "CardNumberLength";
        public const string CLUBCARD_DESC = "ClubcardTypeDescEnglish";
        public const string CLUBCARD_DESC_UPDATE = "ClubcardTypeDesc";

        public const string SP_VIEW_ALL_CLUBCARDTYPES = "USP_GetClubcardTypes";
        public const string SP_ADD_CLUBCARDType = "USP_Add_ClubcardType";
        public const string SP_UPDATE_CLUBCARDType = "USP_Update_ClubcardType";
        public const string SP_UPDATE_CLUBCARD_STATUS = "USP_UpdateClubcardStatus";

        public const string SP_UPDATE_CLUBCARD_STATUS_CSC = "USP_UpdateClubcardStatusCSC";
        public const string SP_GET_CLUBCARD_STATUS = "USP_GetclubcardStatus";

        public const string CLUBCARD_SEQ_TABLE = "ClubcardSeqTable";

        #endregion

        #region ClubCard
        public const string CLUBCARD_ID = "ClubcardID";
        public const string CUSTOMER_NO = "CustomerID";
        public const string CLUBCARD_STATUS = "CardStatus";
        public const string NAME_AS_IN_NRIC = "NameAsinNRIC";
        public const string SEX = "CardMemberGender";
        public const string CLUBCARD_NO = "ClubcardNO";
        public const string CUST_DATE_OF_BIRTH = "CardMemberDOB";
        public const string CLUBCARD_TRANS_ID = "ClubcardTransactionID";
        public const string EXP_BATCH_DATE = "ExpiryBatchDate";

        public const string SP_VIEW_CLUBCARD_DETAILS = "USP_ViewCardDetails";
        public const string SP_ADD_PRIMARY_CLUBCARD = "USP_CreatePrimaryClubcard";
        public const string SP_CHANGE_PRIMARY_CLUBCARD = "USP_ChangePrimaryClubcard";
        //public const string SP_ADD_SUPPLEMENTARY_CLUBCARD = "USP_CreateSupplementaryCard";
        //public const string SP_UPDATE_SUPPLEMENTARY_CLUBCARD = "USP_AmendSupplementaryCard";

        /* add for CCO Convergence */
        public const string SP_VIEW_CLUBCARDS_CUSTOMER = "USP_Get_Clubcard_Recs_Customer";

        /* CCO Convergence Constants*/
        public const string SP_VIEW_MYACCOUNT_DETAILS = "USP_Get_MyAccountDetails";
        public const string SP_VIEW_CLUBCARDS = "USP_Get_Clubcard_Recs";
        public const string SP_VIEW_HOUSEHOLD_CUSTOMERS = "USP_Get_Household_Customers";
        public const string SP_Get_TransactionReasons = "USP_GetTransactionReason";
        public const string SP_GET_POINTSBALANCE = "USP_GetPointBalance";
        #endregion

        #region Role
        public const string ROLE_ID = "RoleID";
        public const string ROLE_NAME = "RoleName";
        public const string ROLE_DESC = "RoleDesc";
        public const string ROLE_ADD_LIMIT = "RoleAddPointsLimit";
        public const string ROLE_SUB_LIMIT = "RoleSubtractPointsLimit";
        public const string CAPABILITY_ID = "CapabilityID";

        public const string SP_VIEW_ROLEDETAILS = "USP_ViewRoleDetails";
        public const string SP_SEARCH_ROLES = "USP_SearchRoles";
        public const string SP_ADD_ROLE = "USP_AddRole";
        public const string SP_UPDATE_ROLEDETAILS = "USP_UpdateRoleDetails";
        public const string SP_UPDATE_ROLEDETAILS_CSC = "USP_UpdateRoleDetailsCSC";
        public const string SP_DELETEE_ROLE = "USP_DeleteRole";
        public const string SP_UPDATE_POINTS_LIMIT = "USP_UpdatePointsLimit";
        public const string SP_ADD_CAPABILITY = "USP_AddRoleCapability";
        public const string SP_REMOVE_CAPABILITY = "USP_RemoveRoleCapability";
        public const string SP_ADD_CAPABILITY_CSC = "USP_AddRoleCapability_CSC";

        #endregion

        #region ApplicationUser
        public const string USER_ID = "UserID";
        public const string USER_NAME = "UserName";
        public const string USER_DESCRIPTION = "UserDescription";
        public const string USER_ROLE_NAME = "RoleNameEnglish";
        public const string USER_ROLE_ID = "RoleID";
        public const string USER_STATUS = "UserStatusCode";
        public const string USER_EMAILID = "EmailAddress";

        public const string SP_GET_APPUSERS = "USP_GetApplicationUsers";
        public const string SP_ADD_APPUSER = "USP_AddApplicationUser";
        public const string SP_VIEW_APP_USER = "USP_ViewUserDetails";
        public const string SP_GET_ROLE_MEMBERSHIP = "USP_GetRoleMembership";
        public const string SP_UPDATE_APPUSER_DETAILS = "USP_UpdateApplicationUser";
        public const string SP_DELETE_ROLE_MEMBERSHIP = "USP_DeleteRoleMembership";
        public const string SP_ADD_ROLE_MEMBERSHIP = "USP_AddRoleMembership";
        public const string SP_ADD_ROLE_MEMBERSHIP_CSC = "USP_AddRoleMembershipCSC";
        //public const string SP_Delete_APPUSER = "deleteAppUser";
        //CSC Requirements
        public const string SP_GET_GROUPS = "USP_Get_Groups";
        public const string SP_GET_Capabilities = "USP_Get_Capabilties";



        #endregion

        #region Partner
        public const string PARTNER_NAME = "PartnerName";
        //public const string PARTNER_NUMBER = "partner_number";
        public const string PARTNER_NUMBER = "PartnerID";
        public const string PARTNER_TYPE = "PartnerType";
        public const string ASSOCIATED_STORE = "TescoStoreID";
        public const string POINTS_ADD_LIMIT = "PartnerAddPointsLimit";
        public const string POINTS_SUB_LIMIT = "PartnerSubtractPointsLimit";
        public const string BUSINESS_REG_NUMBER = "PartnerBusinessRegNo";
        public const string PRICE_PER_POINT = "PricePerPoint";
        public const string MAINTAIN_OUTLETS = "maintain_outlets";
        public const string CONTACT_NAME = "ContactName";
        public const string ADDRESS1 = "partner_contact_address_1";
        public const string ADDRESS2 = "partner_contact_address_2";
        public const string POSTAL_CODE = "partner_contact_postal_address";
        public const string PHONE = "PhoneNumber";
        public const string FAX = "FaxNumber";
        public const string EMAIL = "partner_contact_email_address";
        public const string WEBSITE = "WebsiteURL";
        public const string LAST_BATCH_PROCESSED = "PartnerLastBatchSeqNo";
        public const string STATUS = "partner_status_code";
        public const string REGISTERED_BY = "registered_by";
        public const string DATE_JOINED = "date_joined";
        public const string DATE_LEFT = "date_left";
        public const string SORT_BY = "sort_by";
        public const string SORT_ORDER = "sort_order";
        public const string TESCO_STOREID = "TescoStoreID";
        public const string AGENCYID = "AgencyID";
        public const string PARTNER_ID = "PartnerID";
        public const string OUTLETS_TOBE_MAINTAINED = "OutletsToBeMaintained";
        public const string BUSINESS_TYPE = "BusinessType";
        public const string PARTNER_STATUS = "PartnerStatus";

        public const string DATE_TO = "date_to";
        public const string DATE_FROM = "date_from";
        public const string PARTNER_OUTLETID = "PartnerOutletRef";
        public const string RETRIEVENUMBER = "retrieveNumber";

        public const string PARTNER_NAME_TRANS = "PartnerName";
        public const string SORT_1 = "Sort_1";
        public const string AGENCY_ID_TRANS = "AgencyID";
        public const string SORT_ORDER_TRANS = "SortOrder";

        public const string TXN_DATE = "TxnDate"; //Add PP Transaction
        public const string AMOUNT_SPENT = "AmountSpent"; //Add PP Transaction
        public const string TXN_REASON_ID = "TxnReasonID"; //Add PP Transaction        
        public const string WELCOME_POINTS_QTY = "WelcomePointsQty"; //Add PP Transaction        
        public const string SKU_POINTS_QTY = "SKUPointsQty"; //Add PP Transaction        
        public const string MANUAL_POINTS_QTY = "ManualPointsQty"; //Add PP Transaction        
        public const string GREEN_POINTS_QTY = "GreenPointsQty"; //Add PP Transaction        
        public const string TXN_TYPE = "TransactionType"; //Add PP Transaction        
        public const string SOURCE_POS_ID = "SourcePOSID"; //Add PP Transaction        
        public const string PARTNER_OUTLET_ID = "PartnerOutletNumber"; //Add PP Transaction        
        public const string SOURCE_SYSTEM_TRANSACTIONID = "SourceSystemTransactionID"; //Add PP Transaction        

        public const string SP_ADD_PARTNER = "USP_AddNewPartner";//USP_AddPartner
        public const string SP_UPDATE_PARTNER = "USP_AmendPartner";
        public const string SP_VIEW_PARTNER = "USP_ViewPartner"; //VP "viewPartner";
        //public const string SP_GET_PARTNER = "getPartners";
        //public const string SP_SEARCHTRANS = "USP_FindTransaction";
        public const string SP_SEARCHTRANS = "USP_ViewPartnerTxns";
        public const string SP_VIEW_TRANSOUTLETS = "USP_GetPartnerOutletDetails";
        public const string SP_VIEW_PARTNERS = "USP_GetPartnerDetails";//Add PP Transaction
        public const string SP_VIEW_TXNREASONS = "USP_GetATxnReason";//Add PP Transaction
        public const string SP_ADD_PP_TXN = "USP_Partner_Txn_Add";//Add PP Transaction   
        public const string SP_VIEW_PARTNEROUTLETS = "USP_View_PartnerOutlets";//Add PP Transaction    
        public const string SP_VIEW_TESCO_PARTNERS = "USP_ViewPartners";
        public const string SP_VIEW_PARTNERTYPE = "USP_ViewPartnerTypes";


        #endregion

        #region Partner Outlet

        public const string XML_TAG_PARTNER_OUTLET_PARTNERID = "PartnetID";
        public const string XML_TAG_PARTNER_OUTLET_NUMBER = "PartnerOutletID";
        public const string XML_TAG_PARTNER_OUTLET_REFERENCE = "PartnerOutletRef";
        public const string XML_TAG_PARTNER_OUTLET_NAME = "PartnerOutletName";

        public const string SP_ADD_PARTNEROUTLET = "USP_AddPartnerOutlet";
        public const string SP_UPDATE_PARTNEROUTLET = "USP_AmendPartnerOutlet";
        public const string SP_VIEW_PARTNEROUTLET = "USP_SearchPartnerOutlet";
        public const string SP_GET_PARTNEROUTLET = "USP_GetPartnerOutletDetails";
        public const string SP_DELETE_PATNEROUTLET = "USP_DeletePartnerOutlet";

        #endregion

        #region Agency

        public const string AGENCY_NAME = "AgencyName";
        public const string AGENCY_ID = "AgencyID";
        public const string AGENCY_NUMBER = "AgencyNumber";
        public const string AGENCY_BUSINESS_REG_NUMBER = "AgencyBusinessRegNo";
        public const string AGENCY_ZIP_PASSWORD = "AgencyZipPassword";
        public const string AGENCY_POSTAL_CODE = "PostCode";
        public const string AGENCY_ASSOCIATED_STORE = "TescoStoreID";
        public const string AGENCY_CONTACT_NAME = "ContactName";
        public const string AGENCY_ADDRESS1 = "agency_contact_address_1";
        public const string AGENCY_ADDRESS2 = "agency_contact_address_2";
        public const string AGENCY_ADDRESS3 = "AddressLine3";
        public const string AGENCY_ADDRESS4 = "AddressLine5";
        public const string AGENCY_ADDRESS5 = "AddressLine5";
        public const string AGENCY_ADDRESS6 = "AddressLine6";
        public const string AGENCY_PHONE = "PhoneNumber";
        public const string AGENCY_FAX = "FaxNumber";
        public const string AGENCY_EMAIL = "agency_contact_email_address";
        public const string AGENCY_WEBSITE = "WebsiteURL";
        public const string AGENCY_STATUS = "AgencyStatus";
        public const string AGENCY_REGISTERED_BY = "AgencyRegisteredBy";
        public const string AGENCY_LAST_BATCH_SEQ_NO = "AgencyLastBatchSeqNo";
        public const string AGENCY_START_DATE = "StartDate";
        public const string AGENCY_END_DATE = "EndDate";
        public const string AGENCY_SORT_BY = "Sort_1";
        public const string AGENCY_SORT_ORDER = "SortOrder";

        #endregion

        #region Offer

        public const string OFFER_NAME = "OfferName";
        public const string OFFER_ID = "OfferID";
        public const string OFFER_START_DATETIME = "StartDateTime";
        public const string OFFER_END_DATETIME = "EndDateTime";
        public const string OFFER_MIN_VOUCHER_VALUE = "MinimumVoucherValue";
        public const string OFFER_MAX_VAOUCHER_VALUE = "MaximumVoucherValue";
        public const string OFFER_VOUCHER_VALUE_STEP = "VoucherValueStep";
        public const string OFFER_NUMBER_OF_VOUCHERS = "NumberOfVouchers";
        public const string OFFER_CUST_SEGMENTID = "CustomerSegmentID";
        public const string OFFER_WELCOME_POINTS_QTY = "OfferWelcomePointsQty";
        public const string OFFER_AMT_TO_POINTS_CONV_RATE = "AmountToPointsConversionRate";
        public const string OFFER_POINTS_TO_RWRD_CONV_RATE = "PointsToRewardConversionRate";
        public const string OFFER_REWARD_POINTS_THRESHOLD = "RewardPointsThreshold";
        public const string OFFER_ALLOW_ISSUE_DATE = "AllowReissueDate";
        public const string OFFER_REWARD_CONV_DATE = "RewardConversionDate";
        public const string OFFER_REWARD_CONV_STATUS = "RewardConversionStatus";
        public const string OFFER_REWARD_RECONV_STATUS = "RewardReconversionStatus";
        public const string OFFER_MAILING_APPROVED_DATE = "MailingApprovedDate";
        public const string OFFER_MAILING_NON_REWARD_CUST_CLUBCARD = "MailNonRewardCustomerClubcard";
        public const string OFFER_MAILING_NON_REWARD_BIZ_CLUBCARD = "MailNonRewardCustomerBizcard";

        public const string SP_VIEW_COLLECTION_PERIOD = "USP_GetCollectionPeriod";
        public const string SP_ADD_OFFER = "USP_AddOffer";
        public const string SP_UPDATE_OFFER = "USP_UpdateOffer";
        public const string SP_DELETE_OFFER = "USP_DeleteOffer";
        public const string SP_VIEW_CURRENT_OFFER_DETAILS = "USP_ViewCurrentOfferDetails";
        public const string SP_VIEW_REPORTS = "USP_ViewReports";

        //part of CCO Convergence
        public const string CLUBCARDOFFER_POINT_BALANCE_QTY = "PointsBalanceQty";
        public const string CLUBCARDOFFER_VOUCHERS = "Vouchers";

        #endregion

        #region Vouchers
        public const string MIN_VOUCHER_VALUE = "MinimumVoucherValue";
        public const string MAX_VOUCHER_VALUE = "MaximumVoucherValue";
        public const string VOUCHER_STEP_SIZE = "VoucherValueStep";
        public const string NUMBER_OF_VOUCHERS = "NumberOfVouchers";
        public const string OFFERID = "OfferID";
        public const string VOUCHER_TYPE = "VoucherID";
        public const string VOUCHER_BARCODE = "voucher_type_barcode";
        public const string VOUCHER_DEFNITION_STATUS_CODE = "VoucherDefinitionStatusCode";
        public const string VOUCHER_EXPIRY_DATE = "VoucherTypeExpiryDate";

        public const string SP_VIEW_VOUCHER_PARAMETERS = "usp_viewvoucherparams";
        public const string SP_ADD_VOUCHERS = "USP_Insert_VoucherTypeAndInOffer";
        public const string SP_GET_VOUCHERBARCODES = "USP_Get_Voucher_Barcodes";
        public const string SP_UPDATE_VOUCHERBARCODES = "USP_Update_Voucher_Barcodes";

        //part of CCO Convergence
        public const string XMAS_VOUCHER_START_DATE = "StartDate";
        public const string XMAS_VOUCHER_END_DATE = "EndDate";

        #endregion

        #region Database Operation Mode

        public const string SP_OPERATION_ADD = "ADD";
        public const string SP_OPERATION_UPDATE = "UPDATE";
        public const string SP_OPERATION_DELETE = "DELETE";
        public const string SP_OPERATION_ADDCAPABILITY = "ADDCAPABILITY";
        public const string SP_OPERATION_REMOVECAPABILITY = "REMOVECAPABILITY";

        #endregion

        #region RewardMailingOverview
        public const string FULL_MAILING_STATUS = "FullMailingStatus";
        public const string MAILING_APPROVED_DATE = "MailingApprovedDate";

        public const string SP_VIEW_OFFER_DETAILS = "USP_GetOfferDetails";
        public const string SP_UPDATE_MAILING_STATUS = "USP_Update_Mailing_Status";
        #endregion

        #region ViewMailing
        public const string VIEWMAILINGREPORTSTATUSCODE = "ViewMailingReportStatusCode";

        public const string SP_VIEW_MAILING = "USP_GetMailingDetails";
        public const string SP_UPDATE_MAILING_STATUS_CODE = "USP_Update_MailingStatus_Code";
        public const string SP_GET_CUSTOMEROPTIONALSTATUS = "USP_GetOptionalCustStatus";
        #endregion

        #region ViewRewards
        public const string SP_VIEW_REWARDS = "USP_ViewRewardDetails";
        public const string SP_GET_ALL_OFFER_DETAILS = "USP_GetALLOfferDetails";
        public const string SP_VIEW_VOUCHER_TXN_DETAILS = "USP_Get_Voucher_Txn_Details";
        #endregion

        #region ViewVouchersIssued
        public const string REWARD_REISSUE_REQUESTED_DATE = "RewardReissueRequestedDate";
        public const string REWARD_REISSUE_REQUESTED_BY = "RewardReissueRequestedBy";

        public const string SP_GET_VOUCHERS = "[USP_GetVouchersIssued]";
        public const string SP_UPDATE_REISSUE_REQUESTED_DATE = "[USP_UpdateReissueRequestedDate]";
        #endregion

        #region Coupons
        public const string ACTION = "Action";
        public const string CULTURE = "Culture";
        public const string COUPON_TYPE_DESC = "CouponTypeDesc";
        public const string COUPON_TYPE_BARCODE1 = "CouponTypeBarcode1";
        public const string COUPON_TYPE_BARCODE2 = "CouponTypeBarcode2";
        public const string REWARD_CUSTOMER_IND = "RewardCustomerInd";
        public const string NON_REWARDCUSTOMER_IND = "NonRewardCustomerInd";
        public const string COUPON_TYPE = "CouponType";

        public const string SP_VIEW_COUPONS_PARAMETERS = "USP_ViewCoupons";
        public const string SP_ADD_COUPONS = "USP_AddCoupons";
        public const string SP_UPDATE_COUPONS = "USP_UpdateCoupons";
        public const string SP_DELETE_COUPONS = "USP_DeleteCoupons";
        #endregion

        #region MergeCustomer
        public const string SOURCE_CUSTOMER_ID = "SourceCustID";
        public const string DST_CUSTOMER_ID = "TargetCustID";

        public const string SP_VIEW_CUSTOMERDETAILS = "USP_View_Customer_Details";
        public const string SP_MERGE_CUSTOMER = "USP_CSACustomerMoveHousehold";
        #endregion

        #region ViewRewardGroup
        public const string SP_VIEW_REWARDGROUP = "USP_ViewRewardGroup";
        public const string SP_CHANGE_PRIMARY_CUSTOMER = "USP_CSAChangeHouseholder";
        #endregion

        #region AddTransation
        public const string PRODUCT_POINTS = "product_points";
        public const string TXN_REASON = "txn_reason";
        public const string TOTAL_POINTS = "total_points";
        public const string TXN_NBR = "txn_nbr";
        public const string STANDARD_POINTS = "standard_points";
        public const string SALES_AMOUNT = "sales_amount";
        public const string STORE = "store";
        public const string POS_ID = "pos_id";
        public const string EXTRA_POINTS1 = "extra_points_1";
        public const string WELCOME_POINTS = "welcome_points";
        public const string SOURCE_SYSTEM_TRANSACTION_ID = "txn_nbr";

        public const string SP_VIEW_TRANSACTIONREASON = "USP_ViewTransactionReason";
        public const string SP_ADD_TRANSACTION = "USP_AddTransaction";
        public const string SP_ADD_POINTS = "USP_AddPointsCsc";

        #endregion

        #region ConfigManagement
        public const string NAME_1 = "Name1";
        public const string NAME_2 = "Name2";
        public const string NAME_3 = "Name3";

        public const string DESC_0 = "Desc0";
        public const string DESC_1 = "Desc1";
        public const string DESC_2 = "Desc2";
        public const string DESC_3 = "Desc3";
        public const string DESC_4 = "Desc4";

        public const string MAILABLE_0 = "Mailable0";
        public const string MAILABLE_1 = "Mailable1";
        public const string MAILABLE_2 = "Mailable2";
        public const string MAILABLE_3 = "Mailable3";
        public const string MAILABLE_4 = "Mailable4";

        public const string NotMAILABLE_0 = "NotMailable0";
        public const string NotMAILABLE_1 = "NotMailable1";
        public const string NotMAILABLE_2 = "NotMailable2";
        public const string NotMAILABLE_3 = "NotMailable3";
        public const string NotMAILABLE_4 = "NotMailable4";
        public const string CULTURECONFIG = "culture";
        public const string USERID = "userid";

        public const string Reasoncode1 = "Reasoncode1";
        public const string Reasoncode2 = "Reasoncode2";
        public const string Reasoncode3 = "Reasoncode3";
        public const string Reasoncode4 = "Reasoncode4";
        public const string Reasoncode5 = "Reasoncode5";

        public const string ValidateReasoncode = "ValidateReasoncode";
        public const string OptionalReasoncode0 = "Reason0";
        public const string OptionalReasoncode1 = "Reason1";
        public const string OptionalReasoncode2 = "Reason2";
        public const string OptionalReasoncode3 = "Reason3";
        public const string OptionalReasoncode4 = "Reason4";


        public const string VIEW_DEITARYPREFERENCES = "USP_GetOptionalDietaryPreferences";
        public const string VIEW_CUSTOMERPREFERENCES = "USP_GetOptionalCustomerStatus";
        public const string UPDATE_CONFIGURATION = "USP_UpdateCofiguration";
        public const string CHECK_REASONCODE = "USP_CheckReasoncode";
        public const string UPDATEOPTIONAL_REASONCODE = "USP_UpdateOptionalReasoncode";

        #endregion


        // Added for V312 development by Netra [Req.No:002a]

        #region StoreGroups

        public const string STOREGROUP_NAME = "StoreGroupName";
        public const string STOREGROUP_ID = "StoreGroupID";



        public const string SP_VIEW_STOREGROUPS = "USP_ViewStoreGroups";
        public const string SP_ADD_STOREGROUP = "USP_AddStoreGroup";
        public const string SP_UPDATE_STOREGROUP = "USP_AmendStoreGroup";
        public const string SP_DELETE_STOREGROUP = "USP_DeleteStoreGroup";
        #endregion

        //Change Complete


        #region ReportSchedule

        public const string ReportID = "ReportId";
        public const string UserID = "UserId";
        public const string ReoccurenceType = "Reoccurencetype";
        public const string ScheduleHours = "Schedulehours";
        public const string ScheduleMinutes = "Scheduleminutes";
        public const string EmailRecepients = "Emailrecepients";
        public const string ReportParams = "Reportparams";
        public const string NGCreportFormatterdirectory = "NGCReportFormatterDirectory";
        public const string capabilityName = "Capabilityname";
        public const string userName = "UserName";
        public const string scheduleTimeHHMM = "ScheduleTimeHHMM";
        public const string emailRecepients = "EmailRecepients";
        public const string reportHeadings = "ReportHeadings";
        public const string culture = "Culture";
        public const string defaultculture = "DefaultCulture";
        public const string ReoccurenceType1 = "RecurrenceType";
        public const string ReportParams1 = "ReportParams";
        public const string LocalisationPath = "LocalisationPath";
        
        public const string AddReportSchedule = "USP_AddReportSchedule";
        public const string DeleteReportSchedule = "USP_DeleteReportSchedule";
        public const string ViewReportSchedule = "USP_ViewReportSchedule";

        #endregion

        //part of CCO Convergence
        #region CCO Profanity

        public const string USP_PROFANITY_CHECK = "USP_GetProfanityCheckValues";
        public const string PROFANITY_WORDS = "Profanitywords";

        #endregion

        #region CCO Order A Replacement SPs
        public const string SP_VALIDATEFORORDERREPLACEMENT = "USP_ValidateforOrderReplacement";
        public const string SP_INSERT_NEWORDERREPLACEMENT = "USP_Insert_NewOrderReplacement";
        public const string ORDRPL_PROCESSWINDOW = "OrderProcessWindow";
        public const string ORDRPL_REQUESTCODE = "RequestCode";
        public const string ORDRPL_REQUESTREASONCODE = "RequestReasonCode";
        #endregion

        #region CCO Points Section SPs
        public const string SP_GET_MYPOINTSBALQTYSUMMARY = "USP_Get_MyPointsBalQtySummary";
        public const string SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID = "USP_Get_TransDetailsByCustIDandOfferID";
        public const string SP_GET_TRANSDETAILSBYCUSTIDANDOFFERIDHOUSEHOLD = "USP_Get_TransDetailsByCustIDandOfferID_CSC";
        public const string SP_GET_POINTSSUMMARYREC = "USP_Get_PointsSummaryRec";

        #endregion

        #region Account Activation Details
        public const string ACTIVATED = "Activated";
        public const string DOTCOMCUSTOMERID = "DotcomCustomerID";
        public const string SP_GET_CHECKCUSTOMERACTIVATED = "USP_CheckCustomerActivated";
        #endregion

        #region Household Status of Customer
        public const string SP_GET_CHECKHOUSEHOLDSTATUSOFCUSTOMER = "USP_CheckHouseholdStatusOfCustomer";
        #endregion

        //CONSTANTS

        #region Sudhakar- Rewards
        public const string SP_GIFTCARD_ADJUSTMENT = "USP_AddGiftcardAdjustment";
        public const string SP_GET_POINTSEXPIRY = "USP_GetPointsExpiry";
        public const string SP_GET_REWARDEXPIRYDATE = "USP_GetRewardsExpirydate";
        //public const string SP_GET_USP_GETACCOUNTOVERVIEWDETAILS = "USP_GetAccountOverviewDetails";
        #endregion

        #region Sign In
        public const string SP_GET_TERMSNCONDITIONSTATUS = "USP_GetTemsNConditionsStatus";
        public const string SP_GET_UPDATETERMSNCONDITION = "USP_UpdateTemsNConditionsStatus";
        #endregion

        //Reward Card Management -- Kavitha

        public const string SP_GET_ALl_CLUBCARD = "USP_GetAllClubcards";
        public const string SP_UPDATE_PHONENUMBER = "USP_UpdatePhoneNumber";
        public const string SP_UPDATE_PRIMARYCLUBCARDID = "USP_UpdatePrimaryClubcardID";
        public const string SP_CREATE_SUPPLEMENTARYCLUBCARDID = "Usp_CreateSupplementaryClubCard";

        //End of Reward Card Management

        //Delinking Accounts--Lakshmi
        public const string SP_GET_ALTERNATIVEACCOUNTS = "USP_GetDelinkingAccounts";
        public const string SP_DELETEDELINKINGACCOUNTS="Usp_DeletelinkingAccounts";
        //End of Delinking Accounts

        #region My Coupons -- Noushad

        public const string SP_USP_GETCOUPONS = "USP_GetCoupons";

        public const string SP_USP_UPDATECOUPONS = "USP_UpdateCoupons";

        public const string SP_GET_POINTSSUMMARY = "USP_Get_PointsSummaryDetails";

        public const string SP_GET_POINTSSUMMARY_YEARLY = "USP_Get_PointsSummaryDetailsforYearly";



        #endregion

        #region CSC -- Noushad

        public const string SP_USP_GETUSERROLE = "USP_GetUserRole";

        #endregion

        #region SVNInterfaces -- Kavitha
        public const string SP_UPDATE_REISSUEREQUEST = "USP_UpdatedReissueRequestSVN";
        public const string SP_UPDATE_ROLLOVERREQUEST = "USP_UpdatedRolloverRequestSVN";
        #endregion

        #region Join -- v3.6
        public const string SP_CheckDuplicate = "USP_CheckCustomerDuplication";
        #endregion

        #region Repot module

        public const string USP_GetPointsEarnedReport = "USP_GetEarnedPointsReport";


        #endregion

        #region Reports -- v3.6
        public const string USP_GETDROPDOWNDATA = "USP_PopulateReportData";
        #endregion

        #region GetLatestStatement ---v3.6
        public const string USP_GETLATESTSTATEMENT = "USP_GetMyLatestStatementDetails";

        #endregion

        #region PromotionCode

        public const string USP_GetPromotionCode="USP_GetPromotionCode";
        public const string USP_ADDPromotionCode = "USP_ADDPromotionCode";
        public const string USP_UpdatePromotionCode = "USP_UpdatePromotionCode";
        public const string USP_UpdatePromotionCodePC = "USP_UpdatePromotionCodeAgPC";
        #endregion


        public const string SP_VALIDATE_EMAIL_LINK = "USP_ValidateEmailLink";

        #region SecurityLayerSPs - Madhu
        public const string SP_SET_CUSTOMERVERIFICATION= "USP_SetCustomerVerificationDetails";
        public const string SP_GET_CUSTOMERVERIFICATION= "USP_GetCustomerVerificationDetails";
        #endregion

    }

}
