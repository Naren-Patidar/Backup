/*
 * File   : Customer.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Tesco Customers
 * Date   : 05/Aug/2008
 * 
 */
#region using

using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using USLoyaltySecurityServiceLayer;
using System.Xml;
using LoyaltyEntityServiceLayer.FDRequest;
using LoyaltyEntityServiceLayer.ExactTargetService;

#endregion


namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    /// <summary>
    /// Tesco Customer Details
    /// </summary>
    public class Customer
    {

        #region Fields

        /// <summary>
        /// CustomerID
        /// </summary>
        private string sessionID;

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int64 customerID;

        /// <summary>
        /// Customer Title
        /// </summary>
        private string titleEnglish;

        /// <summary>
        /// Customer Name1
        /// </summary>
        private string name1;

        /// <summary>
        /// Customer Name2
        /// </summary>
        private string name2;

        /// <summary>
        /// Customer Name3
        /// </summary>
        private string name3;

        /// <summary>
        /// Customer DateOfBirth
        /// </summary>
        private string dateOfBirth;

        /// <summary>
        /// Customer DateOfBirth1
        /// </summary>
        private string dateOfBirth1;

        /// <summary>
        /// Customer DateOfBirth2
        /// </summary>
        private string dateOfBirth2;

        /// <summary>
        /// Customer DateOfBirth3
        /// </summary>
        private string dateOfBirth3;

        /// <summary>
        /// Customer DateOfBirth4
        /// </summary>
        private string dateOfBirth4;

        /// <summary>
        /// Customer DateOfBirth5
        /// </summary>
        private string dateOfBirth5;

        /// <summary>
        /// Customer Sex
        /// </summary>
        private string sex;

        /// <summary>
        /// Customer MailingAddressLine1
        /// </summary>
        private string mailingAddressLine1;

        /// <summary>
        /// Customer MailingAddressLine2
        /// </summary>
        private string mailingAddressLine2;

        /// <summary>
        /// Customer MailingAddressLine3
        /// </summary>
        private string mailingAddressLine3;

        /// <summary>
        /// Customer MailingAddressLine4
        /// </summary>
        private string mailingAddressLine4;

        /// <summary>
        /// Customer MailingAddressLine5
        /// </summary>
        private string mailingAddressLine5;

        /// <summary>
        /// Customer MailingAddressLine6
        /// </summary>
        private string mailingAddressLine6;

        /// <summary>
        /// Customer MailingAddressPostCode
        /// </summary>
        private string mailingAddressPostCode;

        /// <summary>
        /// Customer BusinessAddressLine1
        /// </summary>
        private string businessAddressLine1;

        /// <summary>
        /// Customer BusinessAddressLine2
        /// </summary>
        private string businessAddressLine2;

        /// <summary>
        /// Customer BusinessAddressLine3
        /// </summary>
        private string businessAddressLine3;

        /// <summary>
        /// Customer BusinessAddressLine4
        /// </summary>
        private string businessAddressLine4;

        /// <summary>
        /// Customer BusinessAddressLine5
        /// </summary>
        private string businessAddressLine5;

        /// <summary>
        /// Customer BusinessAddressLine6
        /// </summary>
        private string businessAddressLine6;

        /// <summary>
        /// Customer BusinessAddressPostCode
        /// </summary>
        private string businessAddressPostCode;

        /// <summary>
        /// Customer Longitude
        /// </summary>
        private Double longitude;

        /// <summary>
        /// Customer Latitude
        /// </summary>
        private double latitude;

        /// <summary>
        /// Customer DaytimePhoneNumber
        /// </summary>
        private string daytimePhoneNumber;

        /// <summary>
        /// Customer EveningPhoneNumber
        /// </summary>
        private string eveningPhoneNumber;

        /// <summary>
        /// Customer MobilePhoneNumber
        /// </summary>
        private string mobilePhoneNumber;

        /// <summary>
        /// Customer FaxNumber
        /// </summary>
        private string faxNumber;

        /// <summary>
        /// Customer EmailAddress
        /// </summary>
        private string emailAddress;

        /// <summary>
        /// Customer ISOLanguageCode
        /// </summary>
        private string iSOLanguageCode;

        /// <summary>
        /// Customer RaceID
        /// </summary>
        private string raceID;

        /// <summary>
        /// Customer RaceID
        /// </summary>
        private Int16 raceID1;

        /// <summary>
        /// Customer IncomeBandID
        /// </summary>
        private Int16 incomeBandID;

        /// <summary>
        /// Customer CustomerMailStatus
        /// </summary>
        private Int16 customerMailStatus;


        /// <summary>
        /// Customer CustomerMobilePhoneStatus
        /// </summary>
        private Int16 customerMobilePhoneStatus;

        /// <summary>
        /// Customer CustomerEmailStatus
        /// </summary>
        private Int16 customerEmailStatus;

        /// <summary>
        /// Customer BusinessCustomerInd
        /// </summary>
        private string businessCustomerInd;

        /// <summary>
        /// Customer BusinessType
        /// </summary>
        private string businessType;

        /// <summary>
        /// Customer Dynamic Preference for Group
        /// </summary>
        private string dynamicPreferences;

        /// <summary>
        /// Customer CustomerSegmentID
        /// </summary>
        private Int16 customerSegmentID;

        /// <summary>
        /// Customer PreferredStoreID
        /// </summary>
        private Int32 preferredStoreID;

        /// <summary>
        /// Customer JoinedDate
        /// </summary>
        private DateTime joinedDate;

        /// <summary>
        /// Default Data Protection Preference
        /// </summary>
        private Int16 defaultDataProtectionPref;

        /// <summary>
        /// Customer JoinedStoreID
        /// </summary>
        private Int16 joinedStoreID;

        /// <summary>
        /// Customer InactiveCollectionPeriodQty
        /// </summary>
        private Int16 inactiveCollectionPeriodQty;

        /// <summary>
        /// Customer CustomerUseStatusID
        /// </summary>
        private Int16 customerUseStatusID;

        /// <summary>
        /// Customer PreviousLoyaltySchemeClubcardId
        /// </summary>
        private Int64 previousLoyaltySchemeClubcardId;

        /// <summary>
        /// Customer ClubcardID
        /// </summary>
        private Int64 clubcardID;

        /// <summary>
        /// Customer Alternate ID
        /// </summary>
        private string custAlternateID;

        /// <summary>
        /// Customer PreferenceID
        /// </summary>
        private Int16 preferenceID;

        /// <summary>
        /// Customer Preferred Mailing Address ID (Business/Home)
        /// </summary>
        private Int16 preferredMailingAddressID;

        /// <summary>
        /// ReasonCode ID
        /// </summary>
        private Int16 reasonCodeID;

        /// <summary>
        /// Call Details
        /// </summary>
        private string callDetails;

        /// <summary>
        ///Clubcard Status
        /// </summary>
        private int clubcardStatus;



        /// <summary>
        /// Config PtSummStartDate
        /// </summary>
        private DateTime ptSummStartDate;

        /// <summary>
        /// Config PtSummEndDate
        /// </summary>
        private DateTime ptSummEndDate;

        /// <summary>
        /// Config ExchangesStartDate
        /// </summary>
        private DateTime exchangesStartDate;

        /// <summary>
        /// Config exchangesEndDate
        /// </summary>
        private DateTime exchangesEndDate;

        /// <summary>
        /// Config CurXmasStartDate
        /// </summary>
        private DateTime curXmasStartDate;

        /// <summary>
        /// Config CurXmasEndDate
        /// </summary>
        private DateTime curXmasEndDate;

        /// <summary>
        /// Config NextXmasStartDate
        /// </summary>
        private DateTime nextXmasStartDate;
        /// <summary>
        /// Config NextXmasEndDate
        /// </summary>
        private DateTime nextXmasEndDate;

        /// <summary>
        /// Config VoucherStartDate
        /// </summary>
        private DateTime voucherStartDate;


        /// <summary>
        /// Config VoucherEndDate
        /// </summary>
        private DateTime voucherEndDate;

        /// <summary>
        /// Config Flag
        /// </summary>
        private Int16 flag;

        /// <summary>
        /// Config My LatestStatement Start Date
        /// </summary>
        private DateTime latestStatementStartDate;

        /// <summary>
        /// Config My LatestStatement End Date
        /// </summary>
        private DateTime latestStatementEndDate;


        /// <summary>
        /// Customer BusinessAddressPostCode
        /// </summary>
        private string ssn;

         /// <summary>
        /// passportNo
        /// </summary>
        private string passportNo;

        /// <summary>
        /// Customer Language
        /// </summary>
        private string isoLanguage;
        ///// <summary>
        ///// Call ID
        ///// </summary>
        //private int64 callID;

        private Int16 diabeticFlag;
        private Int16 teetotalFlag;
        private Int16 vegetarianFlag;
        private Int16 halalFlag;
        private Int16 celiacFlag;
        private Int16 lactoseFlag;
        private string businessName;
        private string businessRegistrationNumber;
        private string expat;
        private string OptionDesc1;
        private string OptionDesc2;
        private string OptionDesc3;
        private Int16 Option1;
        private Int16 Option2;
        private Int16 Option3;
        ///added as part ofCCO Convergence
        private Int16 kosherFlag;



        //Data OptOut Preferences --V3.1[Req ID - 007]
        private Int16 tescoGroupMailFlag;
        private Int16 tescoGroupEmailFlag;
        private Int16 tescoGroupPhoneFlag;
        private Int16 tescoGroupSMSFlag;
        private Int16 partnerMailFlag;
        private Int16 partnerEmailFlag;
        private Int16 partnerPhoneFlag;
        private Int16 partnerSMSFlag;
        private Int16 researchMailFlag;
        private Int16 researchEmailFlag;
        private Int16 researchPhoneFlag;
        private Int16 researchSMSFlag;
        /// <summary>
        ///  SaveTree
        /// </summary>
        private Int16 SaveTree;

        /// <summary>
        /// Customer Search Criteria
        /// </summary>
        private string customerSearchCriteria;

        /// <summary>
        /// Merge Customer
        /// </summary>
        private string sourceCustomerID;
        private string dstCustomerID;
       

        /// <summary>
        /// Reward Details
        /// </summary>
        //private int customerID;
        private int offerID;
        private string culture;
        private DateTime rewardReissueRequestedDate;
        private string rewardReissueRequestedBy;
        private Int16 noOfhouseHold;


        /*CCO Fields*/
        /// <summary>
        /// author:sudhakar
        /// </summary>
        //private Int16 Ecoupon;
        //private Int16 BAMilesStd;
        //private Int16 BAMilesPremium;
        //private Int16 AirMilesStd;
        //private Int16 AirMilesPremium;
        //private Int16 ViginAtlantic;
        //private Int16 XmasSaver;

        /* For NGC 3.6*/
        // Author: Sabhareesan O.K
        //Updated Date : 04-02-2012

        //private Int16 _ViaMail;
        //private Int16 _ViaSMS;
        //private Int16 _ViaPost;
        //private Int16 _ClubcardEMails;
        //private Int16 _BabyToddler;

        //private int _PreferenceMembershipID;
        //private string _PreferenceEmailID;
        //private string _PreferenceMobile;

        /* For NGC 3.6*/


        private DateTime startDate;
        private DateTime endDate;


        /// <summary>
        /// Clubcard offer details
        /// </summary>
        private int pointsBalanceQty;
        private int isXmasSaver;

        /// <summary>
        /// Check customer activated

        /// </summary>
        private Int64 dotcomCustomerID;
        private char activated;


        /*USLoyalty Fields*/
        /*Menu*/
        private int totalPoints;
        /*Menu*/
        private string fstName;
        private string lstName;
        private string emailId;
        private string password;
        private string secretQtn;
        private string secretAns;
        private string zipCode;
        private string storeLocator;
        private string phoneNo;
        private string existingCustomerId;
        private string alternateId;
        private string custid;
        private string clubcardid;
        private string year;
        private string month;
        private string mode;
        private string welcomepoints;




        #region USLOYALTY sathesh

        //AccountOverView_F&E

        /// <summary>
        /// Total Points
        /// </summary>
        //private int totalPoints;
        private string firstname;
        private string lastname;
        private string zipcode;
        private string phoneno;
        private int day;
        private int dobmonth;
        private string preferenceinput;
        private string isdeleted;
        private string amendby;

        #endregion

        #region SecurityLayer - Madhu

        /// <summary>
        /// BrowserUsed
        /// </summary>
        private string BrowserUsed;

        /// <summary>
        /// IPAddress
        /// </summary>
        private string IPAddress;
        /// <summary>
        /// IsValidAttempt
        /// </summary>
        private string IsValidAttempt;
        #endregion 

        #endregion

        #region Properties

        /// <summary>
        ///  SessionID
        /// </summary>
        public string SessionID { get { return this.sessionID; } set { this.sessionID = value; } }

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int64 CustomerID { get { return this.customerID; } set { this.customerID = value; } }

        /// <summary>
        ///  Customer Title
        /// </summary>
        public string TitleEnglish { get { return this.titleEnglish; } set { this.titleEnglish = value; } }

        /// <summary>
        ///  Customer Name1
        /// </summary>
        public string Name1 { get { return this.name1; } set { this.name1 = value; } }

        /// <summary>
        ///  Customer Name2
        /// </summary>
        public string Name2 { get { return this.name2; } set { this.name2 = value; } }

        /// <summary>
        ///  Customer Name3
        /// </summary>
        public string Name3 { get { return this.name3; } set { this.name3 = value; } }

        /// <summary>
        ///  Customer DateOfBirth
        /// </summary>
        public string DateOfBirth { get { return this.dateOfBirth; } set { this.dateOfBirth = value; } }

        /// <summary>
        ///  Customer DateOfBirth1
        /// </summary>
        public string DateOfBirth1 { get { return this.dateOfBirth1; } set { this.dateOfBirth1 = value; } }

        /// <summary>
        ///  Customer DateOfBirth2
        /// </summary>
        public string DateOfBirth2 { get { return this.dateOfBirth2; } set { this.dateOfBirth2 = value; } }

        /// <summary>
        ///  Customer DateOfBirth3
        /// </summary>
        public string DateOfBirth3 { get { return this.dateOfBirth3; } set { this.dateOfBirth3 = value; } }

        /// <summary>
        ///  Customer DateOfBirth4
        /// </summary>
        public string DateOfBirth4 { get { return this.dateOfBirth4; } set { this.dateOfBirth4 = value; } }

        /// <summary>
        ///  Customer DateOfBirth5
        /// </summary>
        public string DateOfBirth5 { get { return this.dateOfBirth5; } set { this.dateOfBirth5 = value; } }

        /// <summary>
        ///  Customer Sex
        /// </summary>
        public string Sex { get { return this.sex; } set { this.sex = value; } }


        /// <summary>
        ///  Customer Language
        /// </summary>
        public string IsoLanguage { get { return this.isoLanguage; } set { this.isoLanguage = value; } }

        /// <summary>
        ///  Customer MailingAddressLine1
        /// </summary>
        public string MailingAddressLine1 { get { return this.mailingAddressLine1; } set { this.mailingAddressLine1 = value; } }

        /// <summary>
        ///  Customer MailingAddressLine2
        /// </summary>
        public string MailingAddressLine2 { get { return this.mailingAddressLine2; } set { this.mailingAddressLine2 = value; } }

        /// <summary>
        ///  Customer MailingAddressLine3
        /// </summary>
        public string MailingAddressLine3 { get { return this.mailingAddressLine3; } set { this.mailingAddressLine3 = value; } }

        /// <summary>
        ///  Customer MailingAddressLine4
        /// </summary>
        public string MailingAddressLine4 { get { return this.mailingAddressLine4; } set { this.mailingAddressLine4 = value; } }

        /// <summary>
        ///  Customer MailingAddressLine5
        /// </summary>
        public string MailingAddressLine5 { get { return this.mailingAddressLine5; } set { this.mailingAddressLine5 = value; } }

        /// <summary>
        ///  Customer MailingAddressLine6
        /// </summary>
        public string MailingAddressLine6 { get { return this.mailingAddressLine6; } set { this.mailingAddressLine6 = value; } }

        /// <summary>
        ///  Customer MailingAddressPostCode
        /// </summary>
        public string MailingAddressPostCode { get { return this.mailingAddressPostCode; } set { this.mailingAddressPostCode = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine1
        /// </summary>
        public string BusinessAddressLine1 { get { return this.businessAddressLine1; } set { this.businessAddressLine1 = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine2
        /// </summary>
        public string BusinessAddressLine2 { get { return this.businessAddressLine2; } set { this.businessAddressLine2 = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine3
        /// </summary>
        public string BusinessAddressLine3 { get { return this.businessAddressLine3; } set { this.businessAddressLine3 = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine4
        /// </summary>
        public string BusinessAddressLine4 { get { return this.businessAddressLine4; } set { this.businessAddressLine4 = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine5
        /// </summary>
        public string BusinessAddressLine5 { get { return this.businessAddressLine5; } set { this.businessAddressLine5 = value; } }

        /// <summary>
        ///  Customer BusinessAddressLine6
        /// </summary>
        public string BusinessAddressLine6 { get { return this.businessAddressLine6; } set { this.businessAddressLine6 = value; } }

        /// <summary>
        ///  Customer BusinessAddressPostCode
        /// </summary>
        public string BusinessAddressPostCode { get { return this.businessAddressPostCode; } set { this.businessAddressPostCode = value; } }

        /// <summary>
        ///  Customer Longitude
        /// </summary>
        public Double Longitude { get { return this.longitude; } set { this.longitude = value; } }

        /// <summary>
        ///  Customer Latitude
        /// </summary>
        public Double Latitude { get { return this.latitude; } set { this.latitude = value; } }

        /// <summary>
        ///  Customer DaytimePhoneNumber
        /// </summary>
        public string DaytimePhoneNumber { get { return this.daytimePhoneNumber; } set { this.daytimePhoneNumber = value; } }

        /// <summary>
        ///  Customer EveningPhoneNumber
        /// </summary>
        public string EveningPhoneNumber { get { return this.eveningPhoneNumber; } set { this.eveningPhoneNumber = value; } }

        /// <summary>
        ///  Customer MobilePhoneNumber
        /// </summary>
        public string MobilePhoneNumber { get { return this.mobilePhoneNumber; } set { this.mobilePhoneNumber = value; } }

        /// <summary>
        ///  Customer FaxNumber
        /// </summary>
        public string FaxNumber { get { return this.faxNumber; } set { this.faxNumber = value; } }

        /// <summary>
        ///  Customer EmailAddress
        /// </summary>
        public string EmailAddress { get { return this.emailAddress; } set { this.emailAddress = value; } }

        /// <summary>
        ///  Customer ISOLanguageCode
        /// </summary>
        public string ISOLanguageCode { get { return this.iSOLanguageCode; } set { this.iSOLanguageCode = value; } }

        /// <summary>
        ///  Customer RaceID
        /// </summary>
        public string  RaceID { get { return this.raceID; } set { this.raceID = value; } }


        /// <summary>
        ///  Customer RaceID
        /// </summary>
        public Int16 RaceID1 { get { return this.raceID1; } set { this.raceID1 = value; } }


        /// <summary>
        ///  Customer IncomeBandID
        /// </summary>
        public Int16 IncomeBandID { get { return this.incomeBandID; } set { this.incomeBandID = value; } }

        /// <summary>
        ///  Customer CustomerMailStatus
        /// </summary>
        public Int16 CustomerMailStatus { get { return this.customerMailStatus; } set { this.customerMailStatus = value; } }

        /// <summary>
        ///  Customer BusinessCustomerInd
        /// </summary>
        public string BusinessCustomerInd { get { return this.businessCustomerInd; } set { this.businessCustomerInd = value; } }

        /// <summary>
        ///  Customer BusinessType
        /// </summary>
        public string BusinessType { get { return this.businessType; } set { this.businessType = value; } }

        /// <summary>
        ///  Customer DynamicPreferences
        /// </summary>
        public string DynamicPreferences { get { return this.dynamicPreferences; } set { this.dynamicPreferences = value; } }


        /// <summary>
        ///  Customer CustomerSegmentID
        /// </summary>
        public Int16 CustomerSegmentID { get { return this.customerSegmentID; } set { this.customerSegmentID = value; } }

        /// <summary>
        ///  Customer PreferredStoreID
        /// </summary>
        public Int32 PreferredStoreID { get { return this.preferredStoreID; } set { this.preferredStoreID = value; } }

        /// <summary>
        ///  Customer JoinedDate
        /// </summary>
        public DateTime JoinedDate { get { return this.joinedDate; } set { this.joinedDate = value; } }

        /// <summary>
        /// Default Data Protection Preference --V3.1[Req ID - 007]
        /// </summary>
        public Int16 DefaultDataProtectionPref { get { return this.defaultDataProtectionPref; } set { this.defaultDataProtectionPref = value; } }

        /// <summary>
        ///  Customer JoinedStoreID
        /// </summary>
        public Int16 JoinedStoreID { get { return this.joinedStoreID; } set { this.joinedStoreID = value; } }

        /// <summary>
        ///  Customer InactiveCollectionPeriodQty
        /// </summary>
        public Int16 InactiveCollectionPeriodQty { get { return this.inactiveCollectionPeriodQty; } set { this.inactiveCollectionPeriodQty = value; } }

        /// <summary>
        ///  Customer CustomerUseStatusID
        /// </summary>
        public Int16 CustomerUseStatusID { get { return this.customerUseStatusID; } set { this.customerUseStatusID = value; } }

        /// <summary>
        ///  Customer PreviousLoyaltySchemeClubcardId
        /// </summary>
        public Int64 PreviousLoyaltySchemeClubcardId { get { return this.previousLoyaltySchemeClubcardId; } set { this.previousLoyaltySchemeClubcardId = value; } }

        /// <summary>
        ///  ClubcardID
        /// </summary>
        public Int64 ClubcardID { get { return this.clubcardID; } set { this.clubcardID = value; } }

        /// <summary>
        ///  CustAlternateID
        /// </summary>
        public string CustAlternateID { get { return this.custAlternateID; } set { this.custAlternateID = value; } }

        /// <summary>
        ///  BusinessName
        /// </summary>
        public string BusinessName { get { return this.businessName; } set { this.businessName = value; } }

        /// <summary>
        ///  BusinessRegistrationNumber
        /// </summary>
        public string BusinessRegistrationNumber { get { return this.businessRegistrationNumber; } set { this.businessRegistrationNumber = value; } }

        /// <summary>
        ///  BusinessRegistrationNumber
        /// </summary>
        public string SSN { get { return this.ssn; } set { this.ssn = value; } }

        /// <summary>
        ///  PassportNo
        /// </summary>
        public string PassportNo { get { return this.passportNo; } set { this.passportNo = value; } }

        /// <summary>
        ///  PreferenceID
        /// </summary>
        public Int16 PreferenceID { get { return this.preferenceID; } set { this.preferenceID = value; } }

        /// <summary>
        ///  Preferred Mailing Address ID (Home/Business)
        /// </summary>
        public Int16 PreferredMailingAddressID { get { return this.preferredMailingAddressID; } set { this.preferredMailingAddressID = value; } }

        /// <summary>
        ///  DiabeticFlag
        /// </summary>
        public Int16 DiabeticFlag { get { return this.diabeticFlag; } set { this.diabeticFlag = value; } }

        /// <summary>
        ///  TeetotalFlag
        /// </summary>
        public Int16 TeetotalFlag { get { return this.teetotalFlag; } set { this.teetotalFlag = value; } }

        /// <summary>
        ///  VegetarianFlag
        /// </summary>
        public Int16 VegetarianFlag { get { return this.vegetarianFlag; } set { this.vegetarianFlag = value; } }

        /// <summary>
        ///  HalalFlag
        /// </summary>
        public Int16 HalalFlag { get { return this.halalFlag; } set { this.halalFlag = value; } }

        /// <summary>
        ///  CeliacFlag
        /// </summary>
        public Int16 CeliacFlag { get { return this.celiacFlag; } set { this.celiacFlag = value; } }

        /// <summary>
        ///  LactoseFlag
        /// </summary>
        public Int16 LactoseFlag { get { return this.lactoseFlag; } set { this.lactoseFlag = value; } }

        /// <summary>
        ///  Expat
        /// </summary>
        public string Expat { get { return this.expat; } set { this.expat = value; } }

        //Data OptOut Preferences--V3.1[Req ID - 007]
        /// <summary>
        ///  TescoMail
        /// </summary>
        public Int16 TescoGroupMailFlag { get { return this.tescoGroupMailFlag; } set { this.tescoGroupMailFlag = value; } }

        /// <summary>
        ///  TescoEmail
        /// </summary>
        public Int16 TescoGroupEmailFlag { get { return this.tescoGroupEmailFlag; } set { this.tescoGroupEmailFlag = value; } }

        /// <summary>
        ///  TescoPhone
        /// </summary>
        public Int16 TescoGroupPhoneFlag { get { return this.tescoGroupPhoneFlag; } set { this.tescoGroupPhoneFlag = value; } }

        /// <summary>
        ///  TescoSMS
        /// </summary>
        public Int16 TescoGroupSMSFlag { get { return this.tescoGroupSMSFlag; } set { this.tescoGroupSMSFlag = value; } }

        /// <summary>
        ///  PartnerMail
        /// </summary>
        public Int16 PartnerMailFlag { get { return this.partnerMailFlag; } set { this.partnerMailFlag = value; } }

        /// <summary>
        ///  PartnerEmail
        /// </summary>
        public Int16 PartnerEmailFlag { get { return this.partnerEmailFlag; } set { this.partnerEmailFlag = value; } }

        /// <summary>
        ///  PartnerPhone
        /// </summary>
        public Int16 PartnerPhoneFlag { get { return this.partnerPhoneFlag; } set { this.partnerPhoneFlag = value; } }

        /// <summary>
        ///  PartnerSMS
        /// </summary>
        public Int16 PartnerSMSFlag { get { return this.partnerSMSFlag; } set { this.partnerSMSFlag = value; } }

        /// <summary>
        ///  ResearchMail
        /// </summary>
        public Int16 ResearchMailFlag { get { return this.researchMailFlag; } set { this.researchMailFlag = value; } }

        /// <summary>
        ///  ResearchEmail
        /// </summary>
        public Int16 ResearchEmailFlag { get { return this.researchEmailFlag; } set { this.researchEmailFlag = value; } }

        /// <summary>
        ///  ResearchPhone
        /// </summary>
        public Int16 ResearchPhoneFlag { get { return this.researchPhoneFlag; } set { this.researchPhoneFlag = value; } }

        /// <summary>
        ///  ResearchSMS
        /// </summary>
        public Int16 ResearchSMSFlag { get { return this.researchSMSFlag; } set { this.researchSMSFlag = value; } }

        /// <summary>
        ///  Customer Search Criteria
        /// </summary>
        public string CustomerSearchCriteria { get { return this.customerSearchCriteria; } set { this.customerSearchCriteria = value; } }

        /// <summary>
        ///  Merger Customer
        /// </summary>
        public string SourceCustomerID { get { return this.sourceCustomerID; } set { this.sourceCustomerID = value; } }
        public string DstCustomerID { get { return this.dstCustomerID; } set { this.dstCustomerID = value; } }

        /// <summary>
        /// Reward Details
        /// </summary>
        //public int CustomerID { get { return this.customerID; } set { this.customerID = value; } }
        public int OfferID { get { return this.offerID; } set { this.offerID = value; } }
        public string Culture { get { return this.culture; } set { this.culture = value; } }
        public DateTime RewardReissueRequestedDate { get { return this.rewardReissueRequestedDate; } set { this.rewardReissueRequestedDate = value; } }
        public string RewardReissueRequestedBy { get { return this.rewardReissueRequestedBy; } set { this.rewardReissueRequestedBy = value; } }

        /// <summary>
        ///  Number Of House Hold
        /// </summary>
        public Int16 NoOfHouseHold { get { return this.noOfhouseHold; } set { this.noOfhouseHold = value; } }

        /// <summary>
        ///  ReasonCode ID
        /// </summary>
        public Int16 ReasonCodeID { get { return this.reasonCodeID; } set { this.reasonCodeID = value; } }

        /// <summary>
        ///  Call Details
        /// </summary>
        public string CallDetails { get { return this.callDetails; } set { this.callDetails = value; } }



        /*Start CCO Properties*/
        ///<summary>
        /// Voucher and My account details development
        ///added by sudhakar
        ///Date: 3/11/2010
        ///</summary>
        public int PointsBalanceQty { get { return this.pointsBalanceQty; } set { this.pointsBalanceQty = value; } }

        public DateTime StartDate { get { return this.startDate; } set { this.startDate = value; } }

        public DateTime EndDate { get { return this.endDate; } set { this.endDate = value; } }


        #region Preference Info moved from Customer to Preference Class

        //Added by Sadanand on 26-MAR-2010
        ///// <summary>
        /////  Ecoupon
        ///// </summary>
        //public Int16 ECOUPON { get { return this.Ecoupon; } set { this.Ecoupon = value; } }

        ///// <summary>
        /////  BAMilesStd
        ///// </summary>
        //public Int16 BAMILESSTD { get { return this.BAMilesStd; } set { this.BAMilesStd = value; } }

        ///// <summary>
        /////  BAMilesPremium
        ///// </summary>
        //public Int16 BAMILESPREMIUM { get { return this.BAMilesPremium; } set { this.BAMilesPremium = value; } }

        ///// <summary>
        /////  AirMilesStd
        ///// </summary>
        //public Int16 AIRMILESSTD { get { return this.AirMilesStd; } set { this.AirMilesStd = value; } }

        ///// <summary>
        /////  AirMilesPremium
        ///// </summary>
        //public Int16 AIRMILESPREMIUM { get { return this.BAMilesPremium; } set { this.AirMilesPremium = value; } }
        ///// <summary>
        /////  VirginAtlantic
        ///// </summary>
        //public Int16 VIRGINATLANTIC { get { return this.ViginAtlantic; } set { this.ViginAtlantic = value; } }
        ///// <summary>
        /////  XmasSaver
        ///// </summary>
        //public Int16 XMASSAVER { get { return this.XmasSaver; } set { this.XmasSaver = value; } } 

        #endregion


        #region For NGC 3.6 Release

        //Moved from Customer to Preference

        //Author : Sabhareesan O.K

        ///// <summary>
        ///// ViaMail
        ///// </summary>
        //public Int16 ViaMail { get { return this._ViaMail; } set { this._ViaMail = value; } }

        ///// <summary>
        ///// ViaSMS
        ///// </summary>
        //public Int16 ViaSMS { get { return this._ViaSMS; } set { this._ViaSMS = value; } }

        ///// <summary>
        ///// ViaPost
        ///// </summary>
        //public Int16 ViaPost { get { return this._ViaPost; } set { this._ViaPost = value; } }

        ///// <summary>
        ///// ClubcardEMails
        ///// </summary>
        //public Int16 ClubcardEMails { get { return this._ClubcardEMails; } set { this._ClubcardEMails = value; } }

        ///// <summary>
        ///// BabyToddler
        ///// </summary>
        //public Int16 BabyToddler { get { return this._BabyToddler; } set { this._BabyToddler = value; } }

        ///// <summary>
        ///// Preference MembershipID
        ///// </summary>
        //public int PreferenceMembershipID { get { return this._PreferenceMembershipID; } set { this._PreferenceMembershipID = value; } }
        ///// <summary>
        ///// Preference EmailID
        ///// </summary>
        //public string PreferenceEmailID { get { return this._PreferenceEmailID; } set { this._PreferenceEmailID = value; } }
        ///// <summary>
        ///// Preference Mobile
        ///// </summary>
        //public string PreferenceMobile { get { return this._PreferenceMobile; } set { this._PreferenceMobile = value; } }



        #endregion

          //Added as part of ROI conncetion string management
        //begin
        private string connectionString="";
        private string reportDbConnectionString = "";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Customer()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
                reportDbConnectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCReportDBNGCConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                reportDbConnectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);

            }
        }
        //end

        /// <summary>
        ///  ISXmasSaver
        /// </summary>
        public int IsXmasSaver { get { return this.isXmasSaver; } set { this.isXmasSaver = value; } }

        /// <summary>
        ///  Check Customer Activated
        /// </summary>
        public Int64 DotcomCustomerID { get { return this.dotcomCustomerID; } set { this.dotcomCustomerID = value; } }

        public char Activated { get { return this.activated; } set { this.activated = value; } }



        /// <summary>
        ///  KosherFlag
        /// </summary>
        public Int16 KosherFlag { get { return this.kosherFlag; } set { this.kosherFlag = value; } }
        /*End CCO Properties*/

        //private string fstName;
        //private string lstName;
        //private string emailId;
        //private string password;
        //private string secretQtn;
        //private string secretAns;
        //private string zipCode;
        //private string phoneNo;

        /*USLOYALTY Properties*/
        /*Menu*/
        ///<summary>
        ///TotalPoints
        ///</summary>
        public int Totalpoints { get { return this.totalPoints; } set { this.totalPoints = value; } }
        /*Menu*/
        public String FirstName { get { return this.fstName; } set { this.fstName = value; } }
        public String LastName { get { return this.lstName; } set { this.lstName = value; } }
        public String EmailId { get { return this.emailId; } set { this.emailId = value; } }
        public String Password { get { return this.password; } set { this.password = value; } }
        public String SecretQtn { get { return this.secretQtn; } set { this.secretQtn = value; } }
        public String SecretAns { get { return this.secretAns; } set { this.secretAns = value; } }
        public string ZipCode { get { return this.zipCode; } set { this.zipCode = value; } }
        public string StoreLocatorId { get { return this.storeLocator; } set { this.storeLocator = value; } }
        public string PhoneNo { get { return this.phoneNo; } set { this.phoneNo = value; } }
        public string ExistingCustomerId { get { return this.existingCustomerId; } set { this.existingCustomerId = value; } }
        public string AlternateId { get { return this.alternateId; } set { this.alternateId = value; } }
        public string CustId { get { return this.custid; } set { this.custid = value; } }
        public string ClubcardId { get { return this.clubcardid; } set { this.clubcardid = value; } }
        public string Year { get { return this.year; } set { this.year = value; } }
        public string Month { get { return this.month; } set { this.month = value; } }
        public int DOBMonth { get { return this.dobmonth; } set { this.dobmonth = value; } }
        public string Mode { get { return this.mode; } set { this.mode = value; } }
        public string WelcomePoints { get { return this.welcomepoints; } set { this.welcomepoints = value; } }

        /// <summary>
        ///  SaveTree
        /// </summary>
        public Int16 SAVETREE { get { return this.SaveTree; } set { this.SaveTree = value; } }

        /* Satheesh my profile */
        # region USLOYALTY


        /// <summary>
        /// USLOYALTY_Satish
        /// </summary>
        /// 
        ///<summary>
        ///TotalPoints
        ///</summary>



        public int Day { get { return this.day; } set { this.day = value; } }

        public string PreferenceInput { get { return this.preferenceinput; } set { this.preferenceinput = value; } }

        public string IsDeleated { get { return this.isdeleted; } set { this.isdeleted = value; } }

        public string AmendBy { get { return this.amendby; } set { this.amendby = value; } }





        #endregion

        //Reward Card Management -- Kavitha
        // <summary>
        ///  Clubcard Status
        /// </summary>
        public int ClubcardStatus { get { return this.clubcardStatus; } set { this.clubcardStatus = value; } }
        //End of Reward Card Management

        /// <summary>
        ///  Customer CustomerMobilePhoneStatus
        /// </summary>
        public Int16 CustomerMobilePhoneStatus { get { return this.customerMobilePhoneStatus; } set { this.customerMobilePhoneStatus = value; } }

        /// <summary>
        ///  Customer CustomerEmailStatus
        /// </summary>
        public Int16 CustomerEmailStatus { get { return this.customerEmailStatus; } set { this.customerEmailStatus = value; } }
        #endregion

        #region Methods

        #region Search

        /// <summary>
        /// To get the customers according to the given search criteria
        /// </summary>
        public string Search(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Search");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Search - conditionXml :" + conditionXml.ToString());

                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                if (htblCustomer[Constants.CUSTOMER_ID] != null)
                {
                    if (htblCustomer[Constants.CUSTOMER_ID].ToString() != "")
                    {
                        this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                    }
                }
                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null)
                {
                    if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString() != "")
                    {
                        this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                    }
                }
                this.Name1 = (string)htblCustomer[Constants.CUSTOMER_NAME1];
                this.Name2 = (string)htblCustomer[Constants.CUSTOMER_NAME2];
                this.CustAlternateID = (string)htblCustomer[Constants.CUSTOMER_ALTERNATEID];
                this.BusinessName = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_NAME];
                this.BusinessRegistrationNumber = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_REG_NO];
                DateTime dob;
                if (htblCustomer[Constants.CUSTOMER_DOD] != null)
                {
                    //dob = Convert.ToDateTime(htblCustomer[Constants.CUSTOMER_DOD]);
                    this.DateOfBirth = Convert.ToDateTime(htblCustomer[Constants.CUSTOMER_DOD]).ToString();
                }
                //this.DateOfBirth = (string)htblCustomer[Constants.CUSTOMER_DOD];
                this.MobilePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_MOBILE_NUMBER];
                this.MailingAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE];

                object[] objDBParams = { CustomerID,ClubcardID, Name1, Name2, CustAlternateID, BusinessName, BusinessRegistrationNumber,
                                         DateOfBirth , 
                                         MobilePhoneNumber, MailingAddressPostCode };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CUSTOMERS, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.Search - sReqult :" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.Search - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }

        #endregion

        #region View
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public String View(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Search - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Search - customerID :" + customerID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_DETAILS, customerID, culture);
                ds.Tables[0].TableName = "Customer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.Search");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.Search - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.Search - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.Search - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.Search");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }


        #endregion

        //Start : Modification For 3.1.1(Requirement ID 033b) Added by Netra 

        #region View Family Details
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public DataSet ViewFamilyDetails(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewFamilyDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewFamilyDetails - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_FAMILY_DETAILS, customerID);
                ds.Tables[0].TableName = "FamilyDetails";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewFamilyDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewFamilyDetails - customerID :" + customerID.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewFamilyDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewFamilyDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewFamilyDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return ds;
        }

        #endregion

        #region View Dietary Details
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public DataSet ViewDietaryDetails(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewDietaryDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewDietaryDetails - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_DIETARY_DETAILS, customerID);
                ds.Tables[0].TableName = "Dietary";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewDietaryDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewDietaryDetails - customerID :" + customerID.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewFamilyDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewFamilyDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewFamilyDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return ds;
        }

        #endregion

        #region View Data Protection Details
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public DataSet ViewDataProtectionDetails(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewDataProtectionDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewDataProtectionDetails - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_DATAPROTECTION_DETAILS, customerID);
                ds.Tables[0].TableName = "DataProtection";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewDataProtectionDetails - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewDataProtectionDetails - customerID :" + customerID.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewDataProtectionDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewDataProtectionDetails - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewDataProtectionDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return ds;
        }

        #endregion

        #region View Helpline Information
        /// <summary>
        /// To get (view) the customer Helpline details based on the given CustomerID
        /// </summary>
        public String ViewCustomerHelplineInformation(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_HELPLINE_INFORMATION, customerID, culture);
                ds.Tables[0].TableName = "HelplineDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewCustomerHelplineInformation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        #region View CallDetails Information
        /// <summary>
        /// To get (view) the customer Helpline details based on the given CustomerID
        /// </summary>
        public String ViewCustomerCallDetailInformation(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_HELPLINE_INFORMATION, customerID, culture);
                ds.Tables[0].TableName = "HelplineCallDetail";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation - viewXml :" + viewXml.ToString());



            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewCustomerCallDetailInformation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion



        #region View Customer Information
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public String ViewCustomerInformation(Int64 customerID, string culture)
        {

            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewCustomerInformation - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewCustomerInformation - customerID :" + customerID.ToString());
                ds1 = ViewFamilyDetails(customerID, culture);
                ds1.Tables[0].TableName = "FamilyDetails";
                ds2 = ViewDietaryDetails(customerID, culture);
                ds2.Tables[0].TableName = "Dietary";
                ds3 = ViewDataProtectionDetails(customerID, culture);
                ds3.Tables[0].TableName = "DataProtection";
                ds1.Tables.Add(ds2.Tables[0].Copy());
                ds1.Tables.Add(ds3.Tables[0].Copy());

                viewXml = ds1.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewCustomerInformation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewCustomerInformation - viewXml :" + viewXml.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewCustomerInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewCustomerInformation - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewCustomerInformation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        //End of Modifications

        #region View Transactions
        /// <summary>
        /// To get (view) the Transactions based on the given CustomerID
        /// </summary>
        public String ViewTransactions(Int64 primaryClubcardID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewTransactions - primaryClubcardID :" + primaryClubcardID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewTransactions - primaryClubcardID :" + primaryClubcardID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TRANSACTIONS, primaryClubcardID, culture);
                ds.Tables[0].TableName = "ClubcardTransaction";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewTransactions");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewTransactions - viewXml :" + viewXml.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewTransactions - Error Message :" + ex.ToString() + " - primaryClubcardID :" + primaryClubcardID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewTransactions - Error Message :" + ex.ToString() + " - primaryClubcardID :" + primaryClubcardID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewTransactions");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        #region View Points and Rewards
        /// <summary>
        /// To get (view) the Transactions based on the given CustomerID
        /// </summary>
        public String ViewPointsAndRewards(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewPointsAndRewards - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewPointsAndRewards - customerID :" + customerID.ToString());
                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB
                // string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);           
                

                this.CustomerID = customerID;
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ViewPointsAndRewards", CustomerID, culture);
                ds.Tables[0].TableName = "ClubcardPoints";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewPointsAndRewards");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewPointsAndRewards - viewXml :" + viewXml.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewPointsAndRewards - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewPointsAndRewards - Error Message :" + ex.ToString() + " - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewPointsAndRewards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        #region Update
        /// <summary>Update Customer details</summary>
        /// <returns> Returns 0 or >0  if success, otherwise returns -1 </returns>
        /// <remarks>To Update Customer details </remarks>

        public bool Update(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Update - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "customer");
                //string UserName = (string)htblCustomer["InsertBy"];
                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Update - CustomerID :" + this.CustomerID);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Update - CustomerID :" + this.CustomerID);
                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null) this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                this.Name1 = (string)htblCustomer[Constants.CUSTOMER_NAME1];
                this.Name2 = (string)htblCustomer[Constants.CUSTOMER_NAME2];
                this.CustAlternateID = (string)htblCustomer[Constants.CUSTOMER_ALTERNATEID];

                if (htblCustomer[Constants.CUSTOMER_USE_STATUS_ID] != null)
                {
                    this.CustomerUseStatusID = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_USE_STATUS_ID].ToString());
                }
                else
                {
                    this.CustomerUseStatusID = -100;
                }

                this.MailingAddressLine1 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_1];
                this.MailingAddressLine2 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_2];
                this.MailingAddressLine3 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_3];
                this.MailingAddressLine4 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_4];
                this.MailingAddressLine5 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_5];
                this.MailingAddressLine6 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_6];

                this.MailingAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE];
                this.DaytimePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_DAY_TIME_PHONE_NUMBER];
                this.EveningPhoneNumber = (string)htblCustomer[Constants.CUSTOMER_EVENING_PHONE_NUMBER];
                this.MobilePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_MOBILE_NUMBER];
                this.EmailAddress = (string)htblCustomer[Constants.CUSTOMER_EMAIL];
                this.Sex = (string)htblCustomer[Constants.CUSTOMER_SEX];
                this.TitleEnglish = (string)htblCustomer[Constants.CUSTOMER_TITLE_ENGLISH];
                this.FaxNumber = (string)htblCustomer[Constants.CUSTOMER_FAX];
                if (htblCustomer[Constants.CUSTOMER_PREFRENCE_ID] != null)
                {
                    this.PreferenceID = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_PREFRENCE_ID].ToString());
                }
                else
                {
                    this.PreferenceID = -100;
                }
                if (htblCustomer[Constants.CUSTOMER_MAIL_STATUS] != null)
                {
                    this.CustomerMailStatus = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_MAIL_STATUS].ToString());
                }
                else
                {
                    this.CustomerMailStatus = -100;
                }

                this.DateOfBirth = (string)htblCustomer[Constants.CUSTOMER_DOD];
                this.RaceID = (string)htblCustomer[Constants.CUSTOMER_RACE_ID];
                this.BusinessType = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_TYPE];
                this.BusinessRegistrationNumber = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_REG_NO];
                this.BusinessName = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_NAME];
                if (htblCustomer[Constants.CUSTOMER_PREFERRED_MAILING_ADDRESS_ID] != null)
                {
                    this.PreferredMailingAddressID = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_PREFERRED_MAILING_ADDRESS_ID].ToString());
                }
                else
                {
                    this.PreferredMailingAddressID = -100;
                }

                this.Expat = (string)htblCustomer[Constants.CUSTOMER_EXPAT];

                this.BusinessAddressLine1 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_1];
                this.BusinessAddressLine2 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_2];
                this.BusinessAddressLine3 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_3];
                this.BusinessAddressLine4 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_4];
                this.BusinessAddressLine5 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_5];
                this.BusinessAddressLine6 = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_ADDRESS_6];
                this.BusinessAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_POST_CODE];

                object[] objDBParams = {sessionUserID, CustomerID, ClubcardID, Name1, Name2, CustAlternateID, CustomerUseStatusID, 
                                           MailingAddressLine1, MailingAddressLine2,MailingAddressLine4,MailingAddressLine5, MailingAddressLine6, 
                                           MailingAddressPostCode, DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber, EmailAddress, 
                                           PreferenceID, CustomerMailStatus, htblCustomer[Constants.CUSTOMER_DOD] != null ? DateOfBirth :null,
                                           Sex, TitleEnglish, MailingAddressLine3, FaxNumber, 
                                           htblCustomer[Constants.CUSTOMER_RACE_ID] != null ? RaceID : null, 
                                           BusinessType, BusinessRegistrationNumber, BusinessName, PreferredMailingAddressID, Expat, 
                                           BusinessAddressLine1, BusinessAddressLine2, BusinessAddressLine3, BusinessAddressLine4,
                                           BusinessAddressLine5, BusinessAddressLine6, BusinessAddressPostCode };

                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_DETAILS, objDBParams);
                objectID = Convert.ToInt64(this.CustomerID);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.Update- CustomerID");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.Update- CustomerID");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.Update - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.Update - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region Add
        /// <summary>Add Customer details</summary>
        /// <returns> Returns 0 or >0  if success, otherwise returns -1 </returns>
        /// <remarks>To Add Customer details </remarks>

        public bool Add(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;
            //string UserName = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Add - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "customer");

                //this.SessionID = (string)htblCustomer["SessionID"];
                this.Name1 = (string)htblCustomer[Constants.CUSTOMER_NAME1];
                if (this.Name1 == null || this.Name1 == "" || this.Name1 == string.Empty)
                {
                    this.Name1 = "Name Unknown";
                }

                if (htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID] != null)
                {
                    this.PreferredStoreID = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID]);
                }

                if (htblCustomer[Constants.CUSTOMER_ID] != null)
                {
                    this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                }
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Add - CustomerID : " + this.CustomerID);
                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null)
                {
                    this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                }

                if (htblCustomer[Constants.CUSTOMER_JOINED_DATE] != null)
                {
                    this.JoinedDate = Convert.ToDateTime(htblCustomer[Constants.CUSTOMER_JOINED_DATE].ToString());
                }

                if (htblCustomer[Constants.DEFAULT_DATA_PROTECTION_PREF] != null)
                {
                    this.DefaultDataProtectionPref = Convert.ToInt16(htblCustomer[Constants.DEFAULT_DATA_PROTECTION_PREF].ToString());
                }

                //UserName = (string)htblCustomer["InsertBy"];

                
                object[] objDBParams = {sessionUserID, ClubcardID, Name1, PreferredStoreID, JoinedDate
                                           , -1 //@CustomerAlternateID BIGINT,
                                           , PreferredStoreID
                                           , "0" //@reason_code VARCHAR(1)
                                           ,DefaultDataProtectionPref
                                           , CustomerID};

                objectID = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CUSTOMER_DETAILS, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.Add - CustomerID : " + this.CustomerID);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.Add - CustomerID : " + this.CustomerID);
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.Update - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.Update - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        //Start : Modification For 3.1.1(Requirement ID 033b) Added by Netra 

        #region Update Customer Information
        /// <summary>Update Customer Information </summary>
        /// <returns> Returns 0 or >0  if success, otherwise returns -1 </returns>
        /// <remarks>To Update Customer Family Details</remarks>

        public bool UpdateCustomerInformation(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerInformation");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerInformation - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "CustomerInformation");
                string AmendBy = sessionUserID.ToString();// (string)htblCustomer["AmendBy"];                
                string CustomerCreatedBy = sessionUserID.ToString();// (string)htblCustomer["CustomerCreatedBy"];
                string Joined_Date = (string)htblCustomer["Joined_Date"];
                if (htblCustomer["PreviousLoyaltySchemeClubcardId"] != null)
                {
                    if (htblCustomer["PreviousLoyaltySchemeClubcardId"].ToString() != "")
                    {
                        this.PreviousLoyaltySchemeClubcardId = Convert.ToInt64(htblCustomer["PreviousLoyaltySchemeClubcardId"]);
                    }
                }

                ////string PreviousLoyaltySchemeInd = (string)htblCustomer["PreviousLoyaltySchemeInd"];

                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());

                this.DateOfBirth1 = (string)htblCustomer["family_member_1_dob"];
                this.DateOfBirth2 = (string)htblCustomer["family_member_2_dob"];
                this.DateOfBirth3 = (string)htblCustomer["family_member_3_dob"];
                this.DateOfBirth4 = (string)htblCustomer["family_member_4_dob"];
                this.DateOfBirth5 = (string)htblCustomer["family_member_5_dob"];

                //Added for V3.1.1 development by Netra for requirementId 033b optional dietary preferences.
                if (htblCustomer["Diabetic"] != null) this.DiabeticFlag = Convert.ToInt16(htblCustomer["Diabetic"].ToString());
                if (htblCustomer["Teetotal"] != null) this.TeetotalFlag = Convert.ToInt16(htblCustomer["Teetotal"].ToString());
                if (htblCustomer["Vegetarian"] != null) this.VegetarianFlag = Convert.ToInt16(htblCustomer["Vegetarian"].ToString());
                if (htblCustomer["Halal"] != null) this.HalalFlag = Convert.ToInt16(htblCustomer["Halal"].ToString());
                if (htblCustomer["Lactose"] != null) this.LactoseFlag = Convert.ToInt16(htblCustomer["Lactose"].ToString());
                if (htblCustomer["Celiac"] != null) this.CeliacFlag = Convert.ToInt16(htblCustomer["Celiac"].ToString());
                if (htblCustomer["OptinalValue1"] != null) this.Option1 = Convert.ToInt16(htblCustomer["OptinalValue1"].ToString());
                if (htblCustomer["OptinalValue2"] != null) this.Option2 = Convert.ToInt16(htblCustomer["OptinalValue2"].ToString());
                if (htblCustomer["OptinalValue3"] != null) this.Option3 = Convert.ToInt16(htblCustomer["OptinalValue3"].ToString());


                //Data Protection Preferences--V3.1[Req ID - 007]
                if (htblCustomer["TescoMailFlag"] != null) this.TescoGroupMailFlag = Convert.ToInt16(htblCustomer["TescoMailFlag"].ToString());
                if (htblCustomer["TescoEMailFlag"] != null) this.TescoGroupEmailFlag = Convert.ToInt16(htblCustomer["TescoEMailFlag"].ToString());
                if (htblCustomer["TescoPhoneFlag"] != null) this.TescoGroupPhoneFlag = Convert.ToInt16(htblCustomer["TescoPhoneFlag"].ToString());
                if (htblCustomer["TescoSMSFlag"] != null) this.TescoGroupSMSFlag = Convert.ToInt16(htblCustomer["TescoSMSFlag"].ToString());

                if (htblCustomer["PartnerMailFlag"] != null) this.PartnerMailFlag = Convert.ToInt16(htblCustomer["PartnerMailFlag"].ToString());
                if (htblCustomer["PartnerEMailFlag"] != null) this.PartnerEmailFlag = Convert.ToInt16(htblCustomer["PartnerEMailFlag"].ToString());
                if (htblCustomer["PartnerPhoneFlag"] != null) this.PartnerPhoneFlag = Convert.ToInt16(htblCustomer["PartnerPhoneFlag"].ToString());
                if (htblCustomer["PartnerSMSFlag"] != null) this.PartnerSMSFlag = Convert.ToInt16(htblCustomer["PartnerSMSFlag"].ToString());

                if (htblCustomer["ResearchMailFlag"] != null) this.ResearchMailFlag = Convert.ToInt16(htblCustomer["ResearchMailFlag"].ToString());
                if (htblCustomer["ResearchEMailFlag"] != null) this.ResearchEmailFlag = Convert.ToInt16(htblCustomer["ResearchEMailFlag"].ToString());
                if (htblCustomer["ResearchPhoneFlag"] != null) this.ResearchPhoneFlag = Convert.ToInt16(htblCustomer["ResearchPhoneFlag"].ToString());
                if (htblCustomer["ResearchSMSFlag"] != null) this.ResearchSMSFlag = Convert.ToInt16(htblCustomer["ResearchSMSFlag"].ToString());

                if (htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID] != null)
                {
                    this.PreferredStoreID = Convert.ToInt16(htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID]);
                }

                if (htblCustomer["number_of_household_members"] != null) this.NoOfHouseHold = Convert.ToInt16(htblCustomer["number_of_household_members"].ToString());

                //this below line is added as part of code convergence
                this.KosherFlag = 0;

                object[] objDBParams = { CustomerID, AmendBy, CustomerCreatedBy, Joined_Date, PreviousLoyaltySchemeClubcardId, PreferredStoreID, 
                                       DateOfBirth1, DateOfBirth2, DateOfBirth3, DateOfBirth4, DateOfBirth5,
                                       DiabeticFlag, TeetotalFlag, VegetarianFlag, HalalFlag,
                                        TescoGroupMailFlag,TescoGroupEmailFlag,TescoGroupPhoneFlag,TescoGroupSMSFlag,
                                       PartnerMailFlag,PartnerEmailFlag,PartnerPhoneFlag,PartnerSMSFlag,
                                       ResearchMailFlag,ResearchEmailFlag,ResearchPhoneFlag,ResearchSMSFlag,CeliacFlag,LactoseFlag,
                                       NoOfHouseHold,Option1,Option2,Option3,KosherFlag};

                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATED_CUSTOMER_INFORMATION, objDBParams);
                //objectID = Convert.ToInt64(this.CustomerID);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerInformation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerInformation");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerInformation - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerInformation - Error Message :" + ex.ToString() + " - customerID :" + this.CustomerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerInformation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        //End of Modifications
        #region View TransactionsByOffer
        /// <summary>
        /// To get (view) the Transactions based on the given CustomerID
        /// </summary>
        public String TransactionsByOffer(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
            Int64 offerID = 0;
            Int64 primaryCustomerID = 0;
            string houseHold = "";
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.TransactionsByOffer");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.TransactionsByOffer - conditionXml :" + conditionXml.ToString());
                if (htblCustomer["OfferNumberToShow"] != null)
                {
                    if (htblCustomer["OfferNumberToShow"].ToString() != "")
                    {
                        offerID = Convert.ToInt64(htblCustomer["OfferNumberToShow"].ToString());
                    }
                }
                if (htblCustomer["PrimaryCustomerID"] != null)
                {
                    if (htblCustomer["PrimaryCustomerID"].ToString() != "")
                    {
                        primaryCustomerID = Convert.ToInt64(htblCustomer["PrimaryCustomerID"].ToString());
                    }
                }
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.TransactionsByOffer - PrimaryCustomerID :" + primaryCustomerID.ToString());
                if (htblCustomer["CustomerID"] != null)
                {
                    if (htblCustomer["CustomerID"].ToString() != "")
                    {
                        this.CustomerID = Convert.ToInt64(htblCustomer["CustomerID"].ToString());
                    }
                }
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.TransactionsByOffer - CustomerID :" + this.CustomerID.ToString());
                houseHold = (string)htblCustomer["HouseHold"];

                object[] objDBParams = { offerID, primaryCustomerID, CustomerID, houseHold, culture };

                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TRANSACTIONS_BY_OFFER_ID, objDBParams);
                if (ds.Tables.Count > 0)
                    ds.Tables[0].TableName = "ClubcardTransaction";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.TransactionsByOffer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.TransactionsByOffer - viewXml:" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.TransactionsByOffer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.TransactionsByOffer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.TransactionsByOffer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        /*public String TransactionsByOffer(Int64 offerID, string culture)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("Customer.ViewTransactions");
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ViewTransactionsByOfferID", offerID, culture);
                //ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_FAMILY_DETAILS, customerID);
                ds.Tables[0].TableName = "ClubcardTransaction";
                viewXml = ds.GetXml();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            finally
            {
                trState.EndProc();
            }
            return viewXml;
        }*/
        #endregion

        #region Search A Customer to Merge

        /// <summary>
        /// To get the customers according to the given search criteria
        /// </summary>
        public string SearchCustomer(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.SearchCustomer - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.SearchCustomer - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMERDETAILS, customerID);
                ds.Tables[0].TableName = "CustomerDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.SearchCustomer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.SearchCustomer - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.SearchCustomer - Error Message :" + ex.ToString() + " - customerID :" + customerID);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.SearchCustomer - Error Message :" + ex.ToString() + " - customerID :" + customerID);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.SearchCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region Merge Customer
        /// <summary>
        /// To merge the customers
        /// </summary>
        public bool Merge(string objectXml, int userID, out long objectId, out string resultXml)
        {

            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.Merge");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.Merge - objectXml :" + objectXml.ToString());
                Hashtable htblMergeCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "Customer");
                this.SourceCustomerID = Convert.ToString(htblMergeCustomer[Constants.SOURCE_CUSTOMER_ID]);
                this.DstCustomerID = Convert.ToString(htblMergeCustomer[Constants.DST_CUSTOMER_ID]);
                object[] objCustomerParams = { 
                                        SourceCustomerID,
                                        DstCustomerID,
                                        userID
                                     };
                //calls the SP to Update Voucher Barcode
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_MERGE_CUSTOMER, objCustomerParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.Merge ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.Merge ");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.Merge - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.Merge - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.Merge");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {

            }
            return success;
        }
        #endregion

        #region View Reward Details
        /// <summary>
        /// To get the Reward Details
        /// </summary>
        /// <param name="conditionXml">Reward Details</param>/// 
        /// <returns></returns>
        public String SearchRewardDetails(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.SearchRewardDetails ");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.SearchRewardDetails - conditionXml :" + conditionXml.ToString());
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "Vouchers");
                this.CustomerID = Convert.ToInt64(htblVouchers[Constants.CUSTOMER_ID]);
                this.OfferID = Convert.ToInt32(htblVouchers[Constants.OFFER_ID]);
                object[] objRewardParams = { 
                                        CustomerID,
                                        OfferID
                                     };
                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB

                ds = SqlHelper.ExecuteDataset(reportDbConnectionString, Constants.SP_GET_VOUCHERS, objRewardParams);
                ds.Tables[0].TableName = "Vouchers";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.SearchRewardDetails ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.SearchRewardDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.SearchRewardDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.SearchRewardDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.SearchRewardDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region Update Reward Details
        /// <summary>
        /// Update Reissue Date
        /// </summary>
        /// <param name="objectXml">Reward details</param>/// 
        /// <returns></returns>
        public bool UpdateRewardDetails(string objectXml, int userID, out long objectId, out string resultXml)
        {

            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateRewardDetails ");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateRewardDetails - objectXml :" + objectXml.ToString());
                Hashtable htblReissueDate = ConvertXmlHash.XMLToHashTable(objectXml, "Reissue");
                this.RewardReissueRequestedDate = Convert.ToDateTime(htblReissueDate[Constants.REWARD_REISSUE_REQUESTED_DATE]);
                this.RewardReissueRequestedBy = Convert.ToString(htblReissueDate[Constants.REWARD_REISSUE_REQUESTED_BY]);
                this.CustomerID = Convert.ToInt64(htblReissueDate[Constants.CUSTOMER_ID]);
                this.OfferID = Convert.ToInt32(htblReissueDate[Constants.OFFER_ID]);
                object[] objRewardParams = { 
                                        userID,                                   
                                        RewardReissueRequestedBy,
                                        CustomerID,
                                        OfferID
                                     };

                SqlHelper.ExecuteNonQuery(reportDbConnectionString, Constants.SP_UPDATE_REISSUE_REQUESTED_DATE, objRewardParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateRewardDetails ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateRewardDetails ");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateRewardDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateRewardDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateRewardDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {

            }
            return success;
        }
        #endregion

        #region SearchTxnDetails
        /// <summary>
        /// To get the Vocuher Details
        /// </summary>
        /// <param name="offerID">unique identifier of the offer table</param>/// 
        /// <returns>Vocuher Details in xml format</returns>
        public String TxnDetailsSearch(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.TxnDetailsSearch ");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.TxnDetailsSearch - conditionXml :" + conditionXml.ToString());
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "Vouchers");
                this.CustomerID = Convert.ToInt64(htblVouchers[Constants.CUSTOMER_ID]);
                this.OfferID = Convert.ToInt32(htblVouchers[Constants.OFFER_ID]);
                this.Culture = Convert.ToString(htblVouchers[Constants.CULTURE]);
                object[] objRewardParams = { 
                                        CustomerID,
                                        OfferID,Culture
                                     };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_VOUCHER_TXN_DETAILS, objRewardParams);
                ds.Tables[0].TableName = "Voucher";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.TxnDetailsSearch ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.TxnDetailsSearch - viewXml" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.TxnDetailsSearch - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.TxnDetailsSearch - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.TxnDetailsSearch");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region View Reward Group
        /// <summary>
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String ViewRewardGroup(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewRewardGroup ");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewRewardGroup - customerID: " + customerID.ToString());
               
                object[] objRewardGroupParams = { 
                                        customerID,
                                        culture
                                     };
                ds = SqlHelper.ExecuteDataset(reportDbConnectionString, Constants.SP_VIEW_REWARDGROUP, objRewardGroupParams);
                ds.Tables[0].TableName = "RewardGroup";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewRewardGroup ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewRewardGroup - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewRewardGroup - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewRewardGroup - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewRewardGroup");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }


        #endregion

        #region Update Reward Group
        /// <summary>
        /// Update Reissue Date
        /// </summary>
        /// <param name="objectXml">Reward details</param>/// 
        /// <returns></returns>
        public bool UpdateRewardGroup(string objectXml, int userID, out long objectId, out string resultXml)
        {

            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateRewardGroup");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateRewardGroup - objectXml :" + objectXml);
                Hashtable htblRewardGroup = ConvertXmlHash.XMLToHashTable(objectXml, "RewardGroup");
                this.CustomerID = Convert.ToInt64(htblRewardGroup[Constants.CUSTOMER_ID]);
                object[] objRewardParams = {                                   
                                        CustomerID,
                                        userID
                                     };

                SqlHelper.ExecuteNonQuery(reportDbConnectionString, Constants.SP_CHANGE_PRIMARY_CUSTOMER, objRewardParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateRewardGroup");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateRewardGroup");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateRewardGroup - Error Message :" + ex.ToString() + "- userID :" + userID);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateRewardGroup - Error Message :" + ex.ToString() + "- userID :" + userID);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateRewardGroup");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {
            }
            return success;
        }
        #endregion

        #region View Rewards[Voucher Details]
        /// <summary>
        /// To get the Vocuher Details
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public String ViewRewards(long customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewRewards - customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewRewards - customerID :" + customerID.ToString());

                ds = SqlHelper.ExecuteDataset(reportDbConnectionString, Constants.SP_VIEW_REWARDS, customerID);
                ds.Tables[0].TableName = "ClubcardOffer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewRewards");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewRewards - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewRewards - Error Message :" + ex.ToString() + "- customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewRewards - Error Message :" + ex.ToString() + "- customerID :" + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewRewards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region View Offer Details
        /// <summary>
        /// To get the Offer Details
        /// </summary>
        /// <param name="offerID"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public String ViewOffer(long offerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewOffer - offerID :" + offerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewOffer - offerID :" + offerID.ToString());

                ds = SqlHelper.ExecuteDataset(reportDbConnectionString, Constants.SP_GET_ALL_OFFER_DETAILS);
                ds.Tables[0].TableName = "Offer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewRewards");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewRewards - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewOffer - Error Message :" + ex.ToString() + "- offerID :" + offerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewOffer - Error Message :" + ex.ToString() + "- offerID :" + offerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewOffer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion


        #region Add Helpline Details
        public bool AddHelplineInformation(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;
            bool success;
            string AmendBy = sessionUserID.ToString();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddHelplineInformation");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddHelplineInformation - objectXml :" + objectXml.ToString());
                Hashtable htblHelpline = ConvertXmlHash.XMLToHashTable(objectXml, "Helpline");
                this.CallDetails = (string)htblHelpline["CallDetails"];
                this.ReasonCodeID = Convert.ToInt16(htblHelpline["ReasonCodeID"]);
                this.CustomerID = Convert.ToInt64(htblHelpline["CustomerID"]);

                object[] objHelpParams ={
                                            CallDetails,
                                            ReasonCodeID,
                                            CustomerID,
                                            AmendBy
                                      };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CUSTOMER_HELPLINE, objHelpParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddHelplineInformation:");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddHelplineInformation");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddHelplineInformation - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddHelplineInformation - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddHelplineInformation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {

            }
            return success;
        }
        #endregion



        #region CCO methods
        /// <summary>
        /// cco methods are added as part of COde convergence
        /// added by sudhakar 0n 3/11/2010
        /// </summary>

        /*Start CCO Code modification*/
        #region View MyAccountDetails

        ///<summary>
        /// To view Account details based on ClubcardID
        /// </summary>
        public String ViewMyAccountDetails(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewMyAccountDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewMyAccountDetails - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_MYACCOUNT_DETAILS, customerID);
                ds.Tables[0].TableName = "ViewMyAccountDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewMyAccountDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewMyAccountDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewMyAccountDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewMyAccountDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewMyAccountDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region UpdateCustomerDetails
        /// <summary>Update Customer details</summary>
        /// <returns> Returns 0 or >0  if success, otherwise returns -1 </returns>
        /// <remarks>To Update CCO Customer details </remarks>

        public bool UpdateCustomerDetails(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;
            //char dietCode = '0';

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerDetails - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "customer");

                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null) this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                if (htblCustomer[Constants.CUSTOMER_ALTERNATEID] != null) this.CustAlternateID = (string)htblCustomer[Constants.CUSTOMER_ALTERNATEID];

                this.TitleEnglish = (string)htblCustomer[Constants.CUSTOMER_TITLE_ENGLISH];
                this.Name1 = (string)htblCustomer[Constants.CUSTOMER_NAME1];
                this.Name2 = (string)htblCustomer[Constants.CUSTOMER_NAME2];
                this.Name3 = (string)htblCustomer[Constants.CUSTOMER_NAME3];

                this.DateOfBirth = (string)htblCustomer[Constants.CUSTOMER_DOB];
                this.Sex = (string)htblCustomer[Constants.CUSTOMER_SEX];

                this.MailingAddressLine1 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_1];
                this.MailingAddressLine2 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_2];
                this.MailingAddressLine3 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_3];
                this.MailingAddressLine4 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_4];
                this.MailingAddressLine5 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_5];
                this.MailingAddressLine6 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_6];

                this.MailingAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE];
                this.DaytimePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_DAY_TIME_PHONE_NUMBER];
                this.EveningPhoneNumber = (string)htblCustomer[Constants.CUSTOMER_EVENING_PHONE_NUMBER];
                this.MobilePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_MOBILE_NUMBER];
                this.EmailAddress = (string)htblCustomer[Constants.CUSTOMER_EMAIL];

                if (htblCustomer["number_of_household_members"] != null) this.NoOfHouseHold = Convert.ToInt16(htblCustomer["number_of_household_members"].ToString());
                if (htblCustomer["family_member_1_dob"] != null) this.DateOfBirth1 = (string)htblCustomer["family_member_1_dob"];
                if (htblCustomer["family_member_2_dob"] != null) this.DateOfBirth2 = (string)htblCustomer["family_member_2_dob"];
                if (htblCustomer["family_member_3_dob"] != null) this.DateOfBirth3 = (string)htblCustomer["family_member_3_dob"];
                if (htblCustomer["family_member_4_dob"] != null) this.DateOfBirth4 = (string)htblCustomer["family_member_4_dob"];
                if (htblCustomer["family_member_5_dob"] != null) this.DateOfBirth5 = (string)htblCustomer["family_member_5_dob"];

                //NGC Changes Customer Status Update
                if (htblCustomer["CustomerUseStatusMain"] != null) this.CustomerUseStatusID = Convert.ToInt16(htblCustomer["CustomerUseStatusMain"]);
                if (htblCustomer["CustomerMailStatus"] != null) this.CustomerMailStatus = Convert.ToInt16(htblCustomer["CustomerMailStatus"]);
                if (htblCustomer["CustomerMobilePhoneStatus"] != null) this.CustomerMobilePhoneStatus = Convert.ToInt16(htblCustomer["CustomerMobilePhoneStatus"]);
                if (htblCustomer["CustomerEmailStatus"] != null) this.CustomerEmailStatus = Convert.ToInt16(htblCustomer["CustomerEmailStatus"]);
                if (htblCustomer["SSN"] != null) this.SSN = (string)htblCustomer["SSN"]; 
                if (htblCustomer["RaceID"] != null) this.RaceID1 = Convert.ToInt16(htblCustomer["RaceID"]);
                if (htblCustomer["PassportNo"] != null) this.PassportNo = (string)htblCustomer["PassportNo"];
                if (htblCustomer["ISOLanguageCode"] != null) this.IsoLanguage  = (string)htblCustomer["ISOLanguageCode"];

                if (htblCustomer["DynamicPreferences"] != null) this.DynamicPreferences = (string)htblCustomer["DynamicPreferences"];
                if (htblCustomer["Culture"] != null) this.Culture = (string)htblCustomer["Culture"];

                //if (htblCustomer["Diabetic"] != null) this.DiabeticFlag = Convert.ToInt16(htblCustomer["Diabetic"].ToString());
                //if (htblCustomer["Teetotal"] != null) this.TeetotalFlag = Convert.ToInt16(htblCustomer["Teetotal"].ToString());

                //if (htblCustomer["Vegetarian"] != null && ((string)htblCustomer["Vegetarian"] == "1")) dietCode = 'V';
                //else if (htblCustomer["Halal"] != null && ((string)htblCustomer["Halal"] == "1")) dietCode = 'H';
                //else if (htblCustomer["Kosher"] != null && ((string)htblCustomer["Kosher"] == "1")) dietCode = 'K';

                if (htblCustomer["Diabetic"] != null) this.DiabeticFlag = Convert.ToInt16(htblCustomer["Diabetic"].ToString());
                if (htblCustomer["Teetotal"] != null) this.TeetotalFlag = Convert.ToInt16(htblCustomer["Teetotal"].ToString());
                if (htblCustomer["Vegetarian"] != null) this.VegetarianFlag = Convert.ToInt16(htblCustomer["Vegetarian"].ToString());
                if (htblCustomer["Halal"] != null) this.HalalFlag = Convert.ToInt16(htblCustomer["Halal"].ToString());
                if (htblCustomer["Lactose"] != null) this.LactoseFlag = Convert.ToInt16(htblCustomer["Lactose"].ToString());
                if (htblCustomer["Celiac"] != null) this.CeliacFlag = Convert.ToInt16(htblCustomer["Celiac"].ToString());
                if (htblCustomer["Kosher"] != null) this.KosherFlag = Convert.ToInt16(htblCustomer["Kosher"].ToString());
                if (htblCustomer["OptinalValue1"] != null) this.Option1 = Convert.ToInt16(htblCustomer["OptinalValue1"].ToString());
                if (htblCustomer["OptinalValue2"] != null) this.Option2 = Convert.ToInt16(htblCustomer["OptinalValue2"].ToString());
                if (htblCustomer["OptinalValue3"] != null) this.Option3 = Convert.ToInt16(htblCustomer["OptinalValue3"].ToString());


                object[] objDBParams = {CustomerID, CustAlternateID, ClubcardID, TitleEnglish, Name1, Name2, Name3,
                                           htblCustomer[Constants.CUSTOMER_DOB] != null ? DateOfBirth :null , Sex,RaceID1,SSN,PassportNo,IsoLanguage,
                                           MailingAddressLine1, MailingAddressLine2,MailingAddressLine3,MailingAddressLine4,
                                           MailingAddressLine5, MailingAddressLine6, MailingAddressPostCode, DaytimePhoneNumber, 
                                           EveningPhoneNumber, MobilePhoneNumber, EmailAddress,
                                           DateOfBirth1, DateOfBirth2,DateOfBirth3,DateOfBirth4,DateOfBirth5,NoOfHouseHold,
                                           DiabeticFlag, TeetotalFlag, VegetarianFlag, HalalFlag, CeliacFlag, LactoseFlag, 
                                           Option1, Option2, Option3, KosherFlag,sessionUserID, CustomerUseStatusID,CustomerMailStatus,CustomerMobilePhoneStatus,CustomerEmailStatus,DynamicPreferences,Culture};




                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_DETAILS_ORCHESTRATION, objDBParams);
                objectID = Convert.ToInt64(this.CustomerID);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerDetails - objectXml :" + objectXml.ToString());
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region Order A Replacement
        /// <summary>
        /// Checks the new order replacement card if valid
        /// <para>Internally Checks following conditions:</para>
        /// <para>card validity, only standard cards to be replaced</para>
        /// <para>previous order condition</para>
        /// <para>max 3 order in a year condition</para>
        /// <para>calls SP_CCO_VALIDATEFORORDERREPLACEMENT stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>26/03/2010</para>
        /// </summary>
        /// <param name="customerID">Customer ID who logged in</param>
        /// <param name="culture">client culture</param>
        /// <returns>result xml having oldOrderExists, noOfDaysLeftToProcess, countOrdersPlacedInYear, standardCardNumber values</returns>
        public string IsNewOrderReplacementValid(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            #region Local Variable Declaration

            string viewXml = String.Empty;
            Hashtable orderReplacementResult = new Hashtable(), htCheckOrderReplacement;
            //System.Data.SqlClient.SqlParameter[] objOrderParams;
            DataSet resultDS;
            #endregion

            //trace entry for the current function

            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.IsNewOrderReplacementValid");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.IsNewOrderReplacementValid - conditionXml :" + conditionXml.ToString());
                ////pass customer id as input parameter to Stored procedure
                //object[] objOrderParams = { customerID };

                //load all xml values in hashtable
                htCheckOrderReplacement = ConvertXmlHash.XMLToHashTable(conditionXml, "CheckOrderReplacement");

                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htCheckOrderReplacement[Constants.CUSTOMER_ID]);
                short orderProcessWindow = Convert.ToInt16(htCheckOrderReplacement[Constants.ORDRPL_PROCESSWINDOW].ToString());

                object[] objOrderParams = { this.CustomerID, orderProcessWindow };

                //execute SP_CCO_VALIDATEFORORDERREPLACEMENT Stored procedure and load the result dataset
                resultDS = SqlHelper.ExecuteDataset(this.connectionString, Constants.SP_VALIDATEFORORDERREPLACEMENT, objOrderParams);

                //load the dataset values in Hashtable
                orderReplacementResult["oldOrderExists"] = resultDS.Tables[0].Rows[0]["IsExists"].ToString();
                orderReplacementResult["noOfDaysLeftToProcess"] = resultDS.Tables[0].Rows[0]["NoofDaysLeft"].ToString();
                orderReplacementResult["countOrdersPlacedInYear"] = resultDS.Tables[0].Rows[0]["RecCountforaYear"].ToString();
                orderReplacementResult["standardCardNumber"] = resultDS.Tables[0].Rows[0]["NewlyIssuedStdCC"].ToString();
                orderReplacementResult["ClubcardTypeIndicatior"] = resultDS.Tables[0].Rows[0]["ClubcardTypeIndicatior"].ToString();

                if (resultDS.Tables[0] != null && resultDS.Tables[0].Rows.Count > 0)
                    rowCount = resultDS.Tables[0].Rows.Count;

                //convert hashtable into xml and return back to callee function
                viewXml = ConvertXmlHash.HashTableToXML(orderReplacementResult, "OrderReplacement");
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.IsNewOrderReplacementValid");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.IsNewOrderReplacementValid - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                //log exception if any
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.IsNewOrderReplacementValid - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.IsNewOrderReplacementValid - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.IsNewOrderReplacementValid");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml; //return back the result xml
        }

        /// <summary>
        /// Inserts a new order replacement entry with request type and reason
        /// <para>also records the standard card number against new card replacement is requested</para>
        /// <para>interanally calls SP_CCO_INSERT_NEWORDERREPLACEMENT stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>26/03/2010</para>
        /// </summary>
        /// <param name="objectXml">xml string containig CustomerID, ClubcardID, RequestCode, RequestReasonCode</param>
        /// <param name="sessionUserID">user session id who logged in</param>
        /// <param name="objectID"></param>
        /// <param name="resultXml">xml string having exception details if any otherwise the number of days left for order processing as noOfDays</param>
        /// <returns></returns>
        public bool AddNewOrderReplacement(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            #region Local Variable Declaration

            resultXml = string.Empty;
            objectID = 0;
            Hashtable htOrderReplacement;
            System.Data.SqlClient.SqlParameter[] objOrderParams;
            #endregion

            //trace entry for the current function

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddNewOrderReplacement");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddNewOrderReplacement - objectXml :" + objectXml.ToString());
                //load all xml values in hashtable
                htOrderReplacement = ConvertXmlHash.XMLToHashTable(objectXml, "OrderReplacement");

                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htOrderReplacement[Constants.CUSTOMER_ID]);
                this.ClubcardID = Convert.ToInt64(htOrderReplacement[Constants.CLUBCARD_ID]);
                string cardReplacement_RequestCode = htOrderReplacement[Constants.ORDRPL_REQUESTCODE].ToString();
                string cardReplacement_ReasonCode = htOrderReplacement[Constants.ORDRPL_REQUESTREASONCODE].ToString();

                //Load all SQL parameters with subsequent values
                objOrderParams = new System.Data.SqlClient.SqlParameter[5];
                objOrderParams[0] = new System.Data.SqlClient.SqlParameter("@pCustomerID", this.CustomerID);
                objOrderParams[1] = new System.Data.SqlClient.SqlParameter("@pClubcardID", this.ClubcardID);
                objOrderParams[2] = new System.Data.SqlClient.SqlParameter("@pCardReqCode", cardReplacement_RequestCode);
                objOrderParams[3] = new System.Data.SqlClient.SqlParameter("@pCardReqRsn", cardReplacement_ReasonCode);
                objOrderParams[4] = new System.Data.SqlClient.SqlParameter("@pInsertBy", sessionUserID);

                //number of days SQL variable is set to Output direction

                //Call SP_CCO_INSERT_NEWORDERREPLACEMENT stored procedure to insert new order replacement record
                objectID = SqlHelper.ExecuteNonQuery(this.connectionString, CommandType.StoredProcedure,
                                                     Constants.SP_INSERT_NEWORDERREPLACEMENT, objOrderParams);

                //not used anymore as the condition handled in the presentation layer
                #region load noOfdays from SQL Output parameter
                /*//load noOfdays from SQL Output parameter
                if (objOrderParams[4].Value != null)
                    noOfDays = objOrderParams[5].Value.ToString();
                htOrderReplacement["noOfDays"] = noOfDays;

                //convert and return the noOfdays variable in XML
                resultXml = ConvertXmlHash.HashTableToXML(htOrderReplacement, "OrderReplacement");*/
                #endregion
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddNewOrderReplacement");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddNewOrderReplacement");
            }
            catch (Exception ex)
            {
                //log exception if any
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddNewOrderReplacement - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddNewOrderReplacement - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddNewOrderReplacement");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            //if query is successful this flag is true
            return SqlHelper.result.Flag;
        }
        #endregion

        #region Your Preferences - Moved from Customer tp Preference Class

        ///// <summary>
        ///// To get (view) the customer preference details based on the given CustomerID.
        ///// Added by Sadanand on 25-Mar-2010.
        ///// </summary>
        //public String ViewCustomerPreference(Int64 customerID, string culture)
        //{

        //    DataSet ds = new DataSet();
        //    string viewXml = String.Empty;

        //    try
        //    {
        //        NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewCustomerPreference");
        //        NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewCustomerPreference - customerID :" + customerID.ToString());
        //        string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        //        ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_PREFERENCES, customerID);
        //        ds.Tables[0].TableName = "CustomerPreference";
        //        viewXml = ds.GetXml();
        //        NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewCustomerPreference");
        //        NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewCustomerPreference - viewXml :" + viewXml.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewCustomerPreference - Error Message :" + ex.ToString());
        //        NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewCustomerPreference - Error Message :" + ex.ToString());
        //        NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewCustomerPreference");
        //        NGCTrace.NGCTrace.ExeptionHandling(ex);
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    return viewXml;
        //}

        ///// <summary>
        ///// Update Customer Preference
        ///// </summary>
        ///// <param name="objectXml">Preference details</param>/// 
        ///// <returns></returns>
        //public bool UpdateCustomerPreference(string objectXml, int userID, out long objectId, out string resultXml, char level)
        //{

        //    objectId = 0;
        //    bool success;
        //    resultXml = string.Empty;

        //    try
        //    {
        //        NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerPreference");
        //        NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerPreference - objectXml :" + objectXml.ToString());
        //        Hashtable htblCustPreference = ConvertXmlHash.XMLToHashTable(objectXml, "CustomerPreference");
        //        this.CustomerID = Convert.ToInt32(htblCustPreference[Constants.CUSTOMER_ID]);

        //        //Data Protection Preferences--V3.1[Req ID - 007]
        //        if (htblCustPreference["TescoMailFlag"] != null) this.TescoGroupMailFlag = Convert.ToInt16(htblCustPreference["TescoMailFlag"].ToString());
        //        if (htblCustPreference["PartnerMailFlag"] != null) this.PartnerMailFlag = Convert.ToInt16(htblCustPreference["PartnerMailFlag"].ToString());
        //        if (htblCustPreference["ResearchPhoneFlag"] != null) this.ResearchPhoneFlag = Convert.ToInt16(htblCustPreference["ResearchPhoneFlag"].ToString());
        //        if (htblCustPreference["Ecoupon"] != null) this.Ecoupon = Convert.ToInt16(htblCustPreference["Ecoupon"].ToString());
        //        if (htblCustPreference["BAMilesStd"] != null) this.BAMilesStd = Convert.ToInt16(htblCustPreference["BAMilesStd"].ToString());
        //        //if (htblCustPreference["BAMilesPremium"] != null) this.BAMilesPremium = Convert.ToInt16(htblCustPreference["BAMilesPremium"].ToString());
        //        if (htblCustPreference["BAMilesPremium"] != null) this.BAMilesPremium = Convert.ToInt16(DBNull.Value);
        //        if (htblCustPreference["AirMilesStd"] != null) this.AirMilesStd = Convert.ToInt16(htblCustPreference["AirMilesStd"].ToString());
        //        //if (htblCustPreference["AirMilesPremium"] != null) this.AirMilesPremium = Convert.ToInt16(htblCustPreference["AirMilesPremium"].ToString());
        //        if (htblCustPreference["AirMilesPremium"] != null) this.AirMilesPremium = Convert.ToInt16(DBNull.Value);
        //        if (htblCustPreference["VirginAtlantic"] != null) this.ViginAtlantic = Convert.ToInt16(htblCustPreference["VirginAtlantic"].ToString());
        //        if (htblCustPreference["XmasSaver"] != null) this.XmasSaver = Convert.ToInt16(htblCustPreference["XmasSaver"].ToString());
        //        if (htblCustPreference["SaveTree"] != null) this.SaveTree = Convert.ToInt16(htblCustPreference["SaveTree"].ToString());
        //        #region For NGC 3.6 - Added New Preferences
        //        //Author : Sabhareesan O.K
        //        //Modified Date: 01-Feb-2011
        //        //after AirMilesPremium - virigin

        //        if (htblCustPreference["ViaMail"] != null) this.ViaMail = Convert.ToInt16(htblCustPreference["ViaMail"].ToString());
        //        if (htblCustPreference["ViaSMS"] != null) this.ViaSMS = Convert.ToInt16(htblCustPreference["ViaSMS"].ToString());
        //        if (htblCustPreference["ViaPost"] != null) this.ViaPost = Convert.ToInt16(htblCustPreference["ViaPost"].ToString());
        //        if (htblCustPreference["ClubcardEMails"] != null) this.ClubcardEMails = Convert.ToInt16(htblCustPreference["ClubcardEMails"].ToString());
        //        if (htblCustPreference["BabyToddler"] != null) this.BabyToddler = Convert.ToInt16(htblCustPreference["BabyToddler"].ToString());

        //        #endregion


        //        object[] objCustPrefParams = {             
        //                                level,
        //                                CustomerID,
        //                                userID, 
        //                                TescoGroupMailFlag,
        //                                PartnerMailFlag,
        //                                ResearchPhoneFlag,
        //                                BAMilesStd,
        //                                BAMilesPremium,
        //                                AirMilesStd,
        //                                AirMilesPremium,                                        
        //                                XmasSaver,
        //                                Ecoupon,
        //                                SaveTree,
        //                                ViaMail,
        //                                ViaSMS,
        //                                ViaPost,
        //                                ViginAtlantic,
        //                                ClubcardEMails,
        //                                BabyToddler                                        
        //                             };

        //        //calls the SP to Update Customer Preference.
        //        connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        //        SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_PREFERENCES, objCustPrefParams);
        //        success = SqlHelper.result.Flag;
        //        NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerPreference");
        //        NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerPreference");
        //    }
        //    catch (Exception ex)
        //    {
        //        resultXml = SqlHelper.resultXml;
        //        NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerPreference - Error Message :" + ex.ToString());
        //        NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerPreference - Error Message :" + ex.ToString());
        //        NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerPreference");
        //        NGCTrace.NGCTrace.ExeptionHandling(ex);
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    return success;
        //}
        #endregion

        #region Your Points
        /// <summary>
        /// Returns all the transactions for a customer and a perticular OfferId 
        /// <para>interanally calls SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>24/04/2010</para>
        /// </summary>
        /// <param name="conditionXml">xml string containig CustomerID, OfferID</param>
        /// <param name="resultXml"></param>
        /// <returns>xml for Transactions DataSet</returns>
        public String GetTxnDetailsByHouseholdCustomerAndOfferID(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsTransactions = new DataSet();
            DataSet dsOfferDetails = new DataSet();
            string viewXml = String.Empty;
            short showMerchantFlag = 0;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - conditionXml :" + conditionXml.ToString());
                //load all xml values in hashtable
                Hashtable htblTransactions = ConvertXmlHash.XMLToHashTable(conditionXml, "TransactionCondition");

                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htblTransactions[Constants.CUSTOMER_ID]);
                this.OfferID = Convert.ToInt32(htblTransactions[Constants.OFFER_ID]);
                this.Culture = Convert.ToString(htblTransactions[Constants.CULTURE]);
                if (htblTransactions.ContainsKey("ShowMerchantFlag"))
                    showMerchantFlag = Convert.ToInt16(htblTransactions["ShowMerchantFlag"]);
                //prepare the object array for ExecuteDataset call,
                object[] objRewardParams = { 
                                        CustomerID,
                                        OfferID,
                                        showMerchantFlag//,Culture
                                     };

                //call the the stored procedure SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID why ExecuteDataset call
                dsTransactions = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_TRANSDETAILSBYCUSTIDANDOFFERIDHOUSEHOLD, objRewardParams);
                if (dsTransactions.Tables.Count > 0)
                {
                    dsTransactions.Tables[0].TableName = "Transactions";
                    rowCount = dsTransactions.Tables[0].Rows.Count;
                }

                //Fetch the offer details for the offer id passed
                dsOfferDetails = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_OFFER_DETAILS, this.OfferID);
                if (dsOfferDetails.Tables.Count > 0)
                {
                    dsOfferDetails.Tables[0].TableName = "OfferDetails";
                    //add the offer details table to transaction dataset to get the xml
                    dsTransactions.Tables.Add(dsOfferDetails.Tables[0].Copy());
                }
                //return back the transaction Dataset as xml
                viewXml = dsTransactions.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }


        /// <summary>
        /// Returns all the transactions for a customer and a perticular OfferId 
        /// <para>interanally calls SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>24/04/2010</para>
        /// </summary>
        /// <param name="conditionXml">xml string containig CustomerID, OfferID</param>
        /// <param name="resultXml"></param>
        /// <returns>xml for Transactions DataSet</returns>
        public String GetTxnDetailsByCustomerAndOfferID(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet dsTransactions = new DataSet();
            DataSet dsOfferDetails = new DataSet();
            string viewXml = String.Empty;
            short showMerchantFlag = 0;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - conditionXml :" + conditionXml.ToString());
                //load all xml values in hashtable
                Hashtable htblTransactions = ConvertXmlHash.XMLToHashTable(conditionXml, "TransactionCondition");

                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htblTransactions[Constants.CUSTOMER_ID]);
                this.OfferID = Convert.ToInt32(htblTransactions[Constants.OFFER_ID]);
                this.Culture = Convert.ToString(htblTransactions[Constants.CULTURE]);
                if (htblTransactions.ContainsKey("ShowMerchantFlag"))
                    showMerchantFlag = Convert.ToInt16(htblTransactions["ShowMerchantFlag"]);
                //prepare the object array for ExecuteDataset call,
                object[] objRewardParams = { 
                                        CustomerID,
                                        OfferID,
                                        showMerchantFlag//,Culture
                                     };

                //call the the stored procedure SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID why ExecuteDataset call
                dsTransactions = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID, objRewardParams);
                if (dsTransactions.Tables.Count > 0)
                {
                    dsTransactions.Tables[0].TableName = "Transactions";
                    rowCount = dsTransactions.Tables[0].Rows.Count;
                }

                //Fetch the offer details for the offer id passed
                dsOfferDetails = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_OFFER_DETAILS, this.OfferID);
                if (dsOfferDetails.Tables.Count > 0)
                {
                    dsOfferDetails.Tables[0].TableName = "OfferDetails";
                    //add the offer details table to transaction dataset to get the xml
                    dsTransactions.Tables.Add(dsOfferDetails.Tables[0].Copy());
                }
                //return back the transaction Dataset as xml
                viewXml = dsTransactions.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetTxnDetailsByCustomerAndOfferID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        /// <summary>
        /// Returns all the transactions for a customer and a perticular OfferId 
        /// <para>interanally calls SP_GET_TRANSDETAILSBYCUSTIDANDOFFERID stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>24/04/2010</para>
        /// </summary>
        /// <param name="conditionXml">xml string containig CustomerID, OfferID</param>
        /// <param name="resultXml"></param>
        /// <returns>xml for Transactions DataSet</returns>
        public String GetPointsInfoForAllColPrdByCustomer(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;
            //short NoofMonths = 24;//Defaulted to 2 years commented the no. of months input parameter to SP 
            //as now only current and previous 2 collection periods will be retrieved from the stored procedure
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer - conditionXml :" + conditionXml.ToString());
                //load all xml values in hashtable
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "PointsInfoCondition");

                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htblVouchers[Constants.CUSTOMER_ID]);
                //NoofMonths = Convert.ToInt16(htblVouchers["NoofMonths"].ToString());
                this.Culture = culture;

                //prepare the object array for ExecuteDataset call,
                object[] objRewardParams = { 
                                        CustomerID
                                        //,NoofMonths
                                        //,Culture
                                     };

                //call the the stored procedure SP_GET_MYPOINTSBALQTYSUMMARY via ExecuteDataset call
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_MYPOINTSBALQTYSUMMARY, objRewardParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "PointsInfoAllCollPrds";
                    rowCount = ds.Tables[0].Rows.Count;
                }

                //return back the returned Dataset as xml
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPointsInfoForAllColPrdByCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        /// <summary>
        /// Returns Points Summary Record for the customer id and offer id
        /// <para>Internally calls SP_GET_POINTSSUMMARYREC stored procedure</para>
        /// <para>Author: Padmanabh Ganorkar</para>
        /// <para>Date: 25/06/2010</para>
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public String GetPointsSummaryInfo(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            int offerID;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPointsSummaryInfo");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPointsSummaryInfo - conditionXml :" + conditionXml.ToString());
                //load all xml values in hashtable
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "PointsSummaryCondition");
                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htblVouchers[Constants.CUSTOMER_ID]);
                offerID = Convert.ToInt16(htblVouchers["OfferID"].ToString());
                this.Culture = culture;


                object[] objRewardParams = { 
                                        customerID,
                                        offerID
                                        //,Culture
                                     };
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_POINTSSUMMARYREC, objRewardParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "PointsSummaryRec";
                    rowCount = ds.Tables[0].Rows.Count;
                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPointsSummaryInfo");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPointsSummaryInfo - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPointsSummaryInfo - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPointsSummaryInfo - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPointsSummaryInfo");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        #endregion

        ///<summary>
        /// To check customer is a member of XMAS club or not
        /// Author: Robin Apoto
        /// Date: 14/04/2010
        /// </summary>
        public String ViewIsXmasClubMember(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            int isXmasSaver = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewIsXmasClubMember");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewIsXmasClubMember - customerID :" + customerID.ToString());
               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_IS_XMAS_SAVER_CUSTOMER, customerID, isXmasSaver);
                ds.Tables[0].TableName = "ViewIsXmasSaverMember";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewIsXmasClubMember");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewIsXmasClubMember - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewIsXmasClubMember - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewIsXmasClubMember - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewIsXmasClubMember");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        ///<summary>
        /// To get the XMAS saver summary records
        /// </summary>
        public String ViewChristmasSaverSummary(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            rowCount = 0;
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewChristmasSaverSummary");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewChristmasSaverSummary - conditionXml :" + conditionXml.ToString());
                Hashtable htblVouchers = ConvertXmlHash.XMLToHashTable(conditionXml, "XmasSaver");
                this.CustomerID = Convert.ToInt64(htblVouchers[Constants.CUSTOMER_ID]);
                this.StartDate = Convert.ToDateTime(htblVouchers[Constants.XMAS_VOUCHER_START_DATE]);
                this.EndDate = Convert.ToDateTime(htblVouchers[Constants.XMAS_VOUCHER_END_DATE]);
                object[] objRewardParams = { 
                                        CustomerID,
                                        StartDate,EndDate
                                     };
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_XMAS_SAVER_SUMMARY, objRewardParams);
                ds.Tables[0].TableName = "ViewIsXmasSaverSummary";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewChristmasSaverSummary");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewChristmasSaverSummary - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewChristmasSaverSummary - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewChristmasSaverSummary - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewChristmasSaverSummary");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        ///<summary>
        /// Description: To check customer is activated in the Clubcard Online System
        /// Author: Robin Apoto
        /// Date: 02/06/2010
        /// </summary>
        public String CheckCustomerActivated(string dotcomCustomerID, out char activated, out Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            activated = '0';
            customerID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.CheckCustomerActivated");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.CheckCustomerActivated - dotcomCustomerID :" + dotcomCustomerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CHECKCUSTOMERACTIVATED, dotcomCustomerID, activated, customerID);
                if (ds.Tables.Count >= 2)
                {
                    ds.Tables[0].TableName = "ViewHouseholdStatusOfCustomer";
                    ds.Tables[1].TableName = "ViewCheckCustomerActivated";
                    if (ds.Tables[1].Rows[0]["Activated"].ToString() != null)
                    {
                        activated = Convert.ToChar(ds.Tables[1].Rows[0]["Activated"].ToString());
                    }
                    if (ds.Tables[1].Rows[0]["CustomerID"].ToString() != null)
                    {
                        customerID = Convert.ToInt64(ds.Tables[1].Rows[0]["CustomerID"].ToString());
                    }
                }
                else
                {
                    ds.Tables[0].TableName = "ViewCheckCustomerActivated";
                    if (ds.Tables[0].Rows[0]["Activated"].ToString() != null)
                    {
                        activated = Convert.ToChar(ds.Tables[0].Rows[0]["Activated"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["CustomerID"].ToString() != null)
                    {
                        customerID = Convert.ToInt64(ds.Tables[0].Rows[0]["CustomerID"].ToString());
                    }
                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.CheckCustomerActivated");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.CheckCustomerActivated - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.CheckCustomerActivated - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.CheckCustomerActivated - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.CheckCustomerActivated");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        /// <summary>
        /// Description: To check whether the Primary Customer is Banned or Left Scheme or Address in Error
        /// Author:Robin Apoto
        /// Date:10/06/2010
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public String CheckHouseholdStatusOfCustomer(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer - customerID :" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CHECKHOUSEHOLDSTATUSOFCUSTOMER, customerID);
                ds.Tables[0].TableName = "ViewHouseholdStatusOfCustomer";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.CheckHouseholdStatusOfCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        #region Dundee screen methods

        #region GetCustomersForDundee
        /// <summary>
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String GetCustomersForDundee(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustomersForDundee");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustomersForDundee - conditionXml :" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");

                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null)
                {
                    if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString() != "")
                    {
                        this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                    }
                }

                this.Name1 = (string)htblCustomer[Constants.CUSTOMER_NAME1];
                this.Name3 = (string)htblCustomer[Constants.CUSTOMER_NAME3];

                if (htblCustomer[Constants.CUSTOMER_DOD] != null)
                {
                    this.DateOfBirth = Convert.ToDateTime(htblCustomer[Constants.CUSTOMER_DOD]).ToString();
                }

                //NGC Changes for Email and Phone No Search   
                this.EmailAddress = (string)htblCustomer[Constants.CUSTOMER_EMAIL_ADDRESS];
                this.DaytimePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_PHONE_NUMBER];
                this.EveningPhoneNumber = (string)htblCustomer[Constants.CUSTOMER_EVENING_PHONE_NUMBER];
                this.MobilePhoneNumber = (string)htblCustomer[Constants.CUSTOMER_MOBILE_NUMBER];
                //NGC Changes  
                this.BusinessRegistrationNumber = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_REG_NO];
                this.BusinessName = (string)htblCustomer[Constants.CUSTOMER_BUSINESS_NAME];
                this.SSN = (String)htblCustomer[Constants.CUSTOMET_SSN];

                this.MailingAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE];

                if (!string.IsNullOrEmpty((string)htblCustomer[Constants.CUSTOMER_ID]))
                {
                    this.CustomerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                }

                
                object[] objDBParams = { ClubcardID, Name1, Name3, MailingAddressPostCode, DateOfBirth, EmailAddress, DaytimePhoneNumber,EveningPhoneNumber,MobilePhoneNumber,culture, CustomerID, BusinessRegistrationNumber, BusinessName, SSN };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CUSTOMER_FORDUNDEE, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustomersForDundee");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustomersForDundee - sReqult :" + sReqult.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustomersForDundee - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCustomersForDundee - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustomersForDundee");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region GetTitles
        /// <summary>
        /// Gets all the titles.
        /// </summary>
        /// <returns></returns>
        public String GetTitles(out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetTitles");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetTitles");
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_TITLES);
                ds.Tables[0].TableName = "Titles";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetTitles");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetTitles - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetTitles - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetTitles - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetTitles");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region GetCardStatus
        /// <summary>
        /// Gets active card status.
        /// </summary>
        /// <returns></returns>
        public String GetCardStatus()
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCardStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCardStatus");
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CLUBCARD_STATUS);
                ds.Tables[0].TableName = "ClubcardStatus";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCardStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCardStatus - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCardStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCardStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCardStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #endregion
        /*End: CCO code modification*/
        #endregion







        #region Methods for Join module of F&E Loyalty application By Mohan

        #region GetSecretQtns
        /// <summary>
        /// GetSecretQtns -- It is used to fetch secret questions from database table 
        /// </summary>
        /// <param name="string">out rowCount</param>
        /// <param name="string"> culture</param>
        public String GetSecretQtns(out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetSecretQtns");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetSecretQtns");
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_SECRETQUESTIONS);
                ds.Tables[0].TableName = "secretquestions";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetSecretQtns");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetSecretQtns - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetSecretQtns Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetSecretQtns Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetSecretQtns");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

            }
            finally
            {

            }

            return viewXml;
        }
        #endregion

        #region InsertCustomerDetails
        /// <summary>
        /// InsertCustomerDetails -- It is used to insert new customer details from join page 
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public string InsertCustomerDetails(string objectXml, out string erroXml, string WelcomePoints)
        {

            erroXml = string.Empty;
            string viewXml = String.Empty;
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.InsertCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.InsertCustomerDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "newcustomer");
                if (htblCustomer[Constants.FIRST_NAME] != null) this.FirstName = (string)htblCustomer[Constants.FIRST_NAME].ToString();
                if (htblCustomer[Constants.LAST_NAME] != null) this.LastName = (string)htblCustomer[Constants.LAST_NAME].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailId = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                if (htblCustomer[Constants.ZIP_CODE] != null) this.ZipCode = (string)htblCustomer[Constants.ZIP_CODE].ToString();
                if (htblCustomer[Constants.STORELOCATOR_ID] != null) this.StoreLocatorId = (string)htblCustomer[Constants.STORELOCATOR_ID].ToString();
                if (htblCustomer[Constants.PHONE_NO] != null) this.PhoneNo = (string)htblCustomer[Constants.PHONE_NO].ToString();
                object[] objDBParams = { FirstName, LastName, EmailId, ZipCode, StoreLocatorId, int.Parse(WelcomePoints) };
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_INSERT_CUSTOMER_DETAILS, objDBParams);
                ds.Tables[0].TableName = "CustomerId";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.InsertCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.InsertCustomerDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.InsertCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.InsertCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.InsertCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }

            return viewXml;


        }
        #endregion

        #region RollBackCustomerDetails
        /// <summary>
        /// RollBackCustomerDetails -- It is used to rollback new customer details from join page 
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public string RollBackCustomerDetails(string objectXml, out string erroXml)
        {

            erroXml = string.Empty;
            string viewXml = String.Empty;
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.RollBackCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.RollBackCustomerDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "newcustomer");
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailId = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                object[] objDBParams = { EmailId };
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_ROLLBACK_CUSTOMER_DETAILS, objDBParams);
                ds.Tables[0].TableName = "CustomerId";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.RollBackCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.RollBackCustomerDetails - viewXml:" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.RollBackCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.RollBackCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.RollBackCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }

            return viewXml;


        }
        #endregion

        #region GetExistingCustomerDetails
        /// <summary>
        /// GetExistingCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>
        public String GetExistingCustomerDetails(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetExistingCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetExistingCustomerDetails - conditionXml:" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID];
                
                object[] objDBParams = { EmailAddress };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CUSTOMERDETAILS, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetExistingCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetExistingCustomerDetails - sReqult:" + sReqult.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetExistingCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetExistingCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetExistingCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region GetFriendCustomerDetails
        /// <summary>
        /// GetFriendCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>
        public String GetFriendCustomerDetails(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetFriendCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetFriendCustomerDetails - conditionXml:" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID];
                
                object[] objDBParams = { EmailAddress };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_FRIENDCUSTOMERDETAILS, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetFriendCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetFriendCustomerDetails - sReqult:" + sReqult.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetFriendCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetFriendCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetFriendCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region UpdateCustomerDetails
        /// <summary>
        /// UpdateCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateCustomerDetails(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.FIRST_NAME] != null) this.FirstName = (string)htblCustomer[Constants.FIRST_NAME].ToString();
                if (htblCustomer[Constants.LAST_NAME] != null) this.LastName = (string)htblCustomer[Constants.LAST_NAME].ToString();
                if (htblCustomer[Constants.ZIP_CODE] != null) this.ZipCode = (string)htblCustomer[Constants.ZIP_CODE].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                if (htblCustomer[Constants.STORELOCATOR_ID] != null) this.StoreLocatorId = (string)htblCustomer[Constants.STORELOCATOR_ID].ToString();
                //if (htblCustomer[Constants.WELCOMEPOINTS] != null) this.WelcomePoints = (string)htblCustomer[Constants.WELCOMEPOINTS].ToString();
                object[] objDBParams = { FirstName, LastName, ZipCode, EmailAddress, StoreLocatorId };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_EXISTING_CUSTOMER_DETAILS, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerDetails");

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion


        #region GetPrimaryCard
        /// <summary>
        /// GetPrimaryCard -- It is used to fetch primary card no of a customer
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>

        public String GetPrimaryCard(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPrimaryCard");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPrimaryCard - conditionXml:" + conditionXml.ToString());
                Hashtable htblPhoneNo = ConvertXmlHash.XMLToHashTable(conditionXml, "primarycard");
                this.EmailAddress = (string)htblPhoneNo[Constants.EMAIL_ID];
                
                object[] objDBParams = { EmailAddress };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_PRIMARY_CARD, objDBParams);
                ds.Tables[0].TableName = "primarycard";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPrimaryCard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPrimaryCard - sReqult:" + sReqult.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPrimaryCard Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPrimaryCard Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPrimaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region AddPhoneNoToAccount
        /// <summary>
        /// AddPhoneNoToAccount -- It is used to add phone no to a customer's account
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool AddPhoneNoToAccount(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddPhoneNoToAccount");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddPhoneNoToAccount - objectXml:" + objectXml.ToString());
                Hashtable htblPhoneNo = ConvertXmlHash.XMLToHashTable(objectXml, "phoneno");
                if (htblPhoneNo[Constants.EMAIL_ID] != null) this.EmailAddress = (string)htblPhoneNo[Constants.EMAIL_ID].ToString();
                if (htblPhoneNo[Constants.PHONE_NO] != null) this.PhoneNo = (string)htblPhoneNo[Constants.PHONE_NO].ToString();
                object[] objDBParams = { EmailAddress, PhoneNo };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_PHONENO_TO_ACCOUNT, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddPhoneNoToAccount");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddPhoneNoToAccount");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddPhoneNoToAccount Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddPhoneNoToAccount Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddPhoneNoToAccount");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region EmailValidation
        /// <summary>
        /// EmailValidation -- It is used to check an email address already exists in database or not
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        ///  <param name="string">culture</param>

        public String EmailValidation(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.EmailValidation");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.EmailValidation - conditionXml:" + conditionXml.ToString());
                Hashtable htblEmail = ConvertXmlHash.XMLToHashTable(conditionXml, "emailid");
                this.EmailAddress = (string)htblEmail[Constants.EMAIL_ID];
               
                object[] objDBParams = { EmailAddress };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_CHECK_DUPLICATE_EMAILID, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.EmailValidation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.EmailValidation - sReqult:" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.EmailValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.EmailValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.EmailValidation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {


            }
            return sReqult;
        }
        #endregion

        #region PhoneNoValidation
        /// <summary>
        /// PhoneNoValidation -- It is used to check a phone no  already exists in database or not
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        ///  <param name="string">culture</param>

        public String PhoneNoValidation(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.PhoneNoValidation");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.PhoneNoValidation - conditionXml:" + conditionXml.ToString());
                Hashtable htblEmail = ConvertXmlHash.XMLToHashTable(conditionXml, "phoneno");
                this.PhoneNo = (string)htblEmail[Constants.PHONE_NO];
                
                object[] objDBParams = { PhoneNo };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_CHECK_DUPLICATE_PHONENO, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.PhoneNoValidation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.PhoneNoValidation - sReqult:" + sReqult.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.PhoneNoValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.PhoneNoValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.PhoneNoValidation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {


            }
            return sReqult;
        }
        #endregion

        #region AlternateIdValidation
        /// <summary>
        /// AlternateIdValidation -- It is used to check an user alredy entered phone no as alternate id or not
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        ///  <param name="string">culture</param>

        public String AlternateIdValidation(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AlternateIdValidation");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AlternateIdValidation - conditionXml:" + conditionXml.ToString());
                Hashtable htblAlternateId = ConvertXmlHash.XMLToHashTable(conditionXml, "alternateid");
                this.EmailId = htblAlternateId[Constants.EMAIL_ID].ToString();
                this.alternateId = htblAlternateId[Constants.ALTERNATE_ID].ToString();
                
                object[] objDBParams = { EmailId, int.Parse(alternateId) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_CHECK_DUPLICATE_ALTERNATEID, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AlternateIdValidation");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AlternateIdValidation - sReqult:" + sReqult.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AlternateIdValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AlternateIdValidation Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AlternateIdValidation");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {


            }
            return sReqult;
        }
        #endregion


        #region GetCustIdByEmailId
        /// <summary>
        /// GetCustIdByEmailId -- It is used to get custid of a customer providing mailid
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>

        public String GetCustIdByEmailId(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustIdByEmailId");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustIdByEmailId - conditionXml:" + conditionXml.ToString());
                Hashtable htblEmail = ConvertXmlHash.XMLToHashTable(conditionXml, "emailid");
                this.EmailAddress = (string)htblEmail[Constants.EMAIL_ID];
                
                object[] objDBParams = { EmailAddress };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CUSTID_BY_EMAILID, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustIdByEmailId");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustIdByEmailId - sReqult:" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustIdByEmailId Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCustIdByEmailId Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustIdByEmailId");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {


            }
            return sReqult;
        }
        #endregion

        #region InsertCardNo
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool InsertCardNo(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            bool success = true;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.InsertCardNo");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.InsertCardNo - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "cardno");
                if (htblCustomer[Constants.CLUBCARDID] != null) this.ClubcardId = (string)htblCustomer[Constants.CLUBCARDID].ToString();
                if (htblCustomer[Constants.PHONE_NO] != null) this.PhoneNo = (string)htblCustomer[Constants.PHONE_NO].ToString();
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                
                object[] objDBParams = { long.Parse(CustId), long.Parse(ClubcardId), PhoneNo };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.USP_INSERT_CLUBCARD, objDBParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.InsertCardNo");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.InsertCardNo");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.InsertCardNo Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.InsertCardNo Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.InsertCardNo");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {

            }

            return success;
        }
        #endregion

        #region UpdateCustomerStatus
        /// <summary>
        /// InsertCardNo -- It is used to insert card no of a customer into account
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool UpdateCustomerStatus(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            bool success = true;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerStatus - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "custid");
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                
                object[] objDBParams = { long.Parse(CustId) };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_STATUS, objDBParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerStatus");


            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerStatus Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerStatus Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
            }
            finally
            {

            }

            return success;
        }
        #endregion

        #region GetCardNoByCustId

        /// <summary>
        /// GetCardNoByCustId -- It is used to get cardno by customer id of a customer
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>

        public String GetCardNoByCustId(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCardNoByCustId");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCardNoByCustId - conditionXml:" + conditionXml.ToString());
                Hashtable htblEmail = ConvertXmlHash.XMLToHashTable(conditionXml, "customerid");
                this.CustId = (string)htblEmail[Constants.CUST_ID];
                
                object[] objDBParams = { long.Parse(CustId.ToString()) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GET_CARDNO_BY_CUSTID, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCardNoByCustId");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCardNoByCustId - sReqult:" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCardNoByCustId Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCardNoByCustId Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCardNoByCustId");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {


            }
            return sReqult;
        }
        #endregion

        #region GetTransactionHistory
        /// <summary>
        /// GetTransactionHistory -- It is used to get transaction history of a customer by customer id
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>
        public String GetTransactionHistory(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetTransactionHistory");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetTransactionHistory - conditionXml:" + conditionXml.ToString());

                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "CustomerId");
               

                this.CustId = (string)htblCustomer[Constants.CUST_ID];
                this.Year = htblCustomer[Constants.YEAR].ToString();
                this.Month = htblCustomer[Constants.MONTH].ToString();
                object[] objDBParams = { long.Parse(CustId), int.Parse(Month), int.Parse(Year) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GET_TRANSACTION_BY_MON_YEAR, objDBParams);

                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetTransactionHistory");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetTransactionHistory - sReqult:" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetTransactionHistory Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetTransactionHistory Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetTransactionHistory");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion


        #endregion

        #region My Profile Methods  -  satheesh


        #region GetMyProfileDetails
        /// <summary>
        /// GetExistingCustomerDetails -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>
        public String GetMyProfileDetails(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();



            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetMyProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetMyProfileDetails - conditionXml:" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID];
                
                object[] objDBParams = { int.Parse(ExistingCustomerId) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_MYPROFILE_DETAILS, objDBParams);
                ds.Tables[0].TableName = "Myprofile";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetMyProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetMyProfileDetails - sReqult:" + sReqult.ToString());



            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetMyProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetMyProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetMyProfileDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region UpdateMyProfileDetails
        /// <summary>
        /// UpdateCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateMyProfileDetails(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateMyProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateMyProfileDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.FIRST_NAME] != null) this.FirstName = (string)htblCustomer[Constants.FIRST_NAME].ToString();
                if (htblCustomer[Constants.LAST_NAME] != null) this.LastName = (string)htblCustomer[Constants.LAST_NAME].ToString();
                if (htblCustomer[Constants.ZIP_CODE] != null) this.ZipCode = (string)htblCustomer[Constants.ZIP_CODE].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                if (htblCustomer[Constants.EXISTINGCUSTOMER_ID] != null) this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID].ToString();
                if (htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID] != null) this.PreferredStoreID = Convert.ToInt32(htblCustomer[Constants.CUSTOMER_PREFFERED_STORE_ID].ToString());
                object[] objDBParams = { FirstName, LastName, ZipCode, EmailAddress, ExistingCustomerId, PreferredStoreID };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_MYPROFILE_DETAILS, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateMyProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateMyProfileDetails");

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateMyProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateMyProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateMyProfileDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region UpdateCustomerDetails
        /// <summary>
        /// UpdateCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateCustomerProfileDetails(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.FIRST_NAME] != null) this.FirstName = (string)htblCustomer[Constants.FIRST_NAME].ToString();
                if (htblCustomer[Constants.LAST_NAME] != null) this.LastName = (string)htblCustomer[Constants.LAST_NAME].ToString();
                if (htblCustomer[Constants.ZIP_CODE] != null) this.ZipCode = (string)htblCustomer[Constants.ZIP_CODE].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                if (htblCustomer[Constants.EXISTINGCUSTOMER_ID] != null) this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID].ToString();
                object[] objDBParams = { FirstName, LastName, ZipCode, EmailAddress, ExistingCustomerId };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_EXISTING_CUSTOMER_PROFILE_DETAILS, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerProfileDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region GetAboutMeDetails

        public String GetAboutMeDetails(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetAboutMeDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetAboutMeDetails - conditionXml:" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID];
                
                object[] objDBParams = { int.Parse(ExistingCustomerId) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GETABOUTMEDETAILS, objDBParams);
                ds.Tables[0].TableName = "Customer";
                ds.Tables[1].TableName = "DietaryPref";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetAboutMeDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetAboutMeDetails - sReqult:" + sReqult.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetAboutMeDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetAboutMeDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetAboutMeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
            }
            return sReqult;
        }



        #endregion

        #region UpdateAboutmeDetails
        /// <summary>
        /// UpdateCustomerDetails -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateAboutmeDetails(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateAboutmeDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateAboutmeDetails - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.CUSTOMER_SEX] != null) this.Sex = (string)htblCustomer[Constants.CUSTOMER_SEX].ToString();
                if (htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_1] != null) this.MailingAddressLine1 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_1].ToString();
                if (htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_2] != null) this.MailingAddressLine2 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_2].ToString();
                if (htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_4] != null) this.mailingAddressLine4 = (string)htblCustomer[Constants.CUSTOMER_MAILING_ADDRESS_4].ToString();
                if (htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE] != null) this.MailingAddressPostCode = (string)htblCustomer[Constants.CUSTOMER_MAILING_POST_CODE].ToString();
                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.customerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                if (htblCustomer[Constants.DAY] != null) this.Day = Convert.ToInt32(htblCustomer[Constants.DAY].ToString());
                if (htblCustomer[Constants.DOBMONTH] != null) this.DOBMonth = Convert.ToInt32(htblCustomer[Constants.DOBMONTH].ToString());

                object[] objDBParams = { customerID, Sex, MailingAddressLine1, MailingAddressLine2, MailingAddressLine4, MailingAddressPostCode, Day, DOBMonth };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATEABOUTMEDETAILS, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateAboutmeDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateAboutmeDetails");


            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateAboutmeDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateAboutmeDetails Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateAboutmeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region UpdateDietaryPrferences
        /// <summary>
        /// UpdateDietaryPrferences -- It is used to update Dietary details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateDietaryPreferences(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateDietaryPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateDietaryPreferences - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.customerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                if (htblCustomer[Constants.DIETARYPREFERENCEINPUT] != null) this.PreferenceInput = htblCustomer[Constants.DIETARYPREFERENCEINPUT].ToString();
                object[] objDBParams = { customerID, PreferenceInput };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATEDIETARYPREFERENCES, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateDietaryPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateDietaryPreferences");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateDietaryPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateDietaryPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateDietaryPreferences");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion


        #region ADD SUPPLEMENTARY CARD
        /// <summary>
        /// To Add the ClubCard Type details to the database
        /// </summary>

        public bool AddSupplementaryCard(string objectXml, out long objectId, out string resultXml)
        {

            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddSupplementaryCard");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddSupplementaryCard - objectXml:" + objectXml.ToString());
                Hashtable htPCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "cardno");
                this.CustomerID = Convert.ToInt64(htPCardDetails[Constants.CUSTOMER_NO]);
                this.ClubcardID = Convert.ToInt64(htPCardDetails[Constants.CLUBCARD_ID]);
                
                object[] objAddClubcard = { CustomerID, ClubcardID };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_CREATE_SUPPLEMENTARYCLUBCARDID, objAddClubcard);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddSupplementaryCard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddSupplementaryCard");


            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddSupplementaryCard Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddSupplementaryCard Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddSupplementaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
            return SqlHelper.result.Flag;

        }
        #endregion


        #region UpdateEmailPrferences
        /// <summary>
        /// UpdateDietaryPrferences -- It is used to update Dietary details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateEmailPreferences(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateEmailPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateEmailPreferences - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "emailpreferences");
                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.customerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                if (htblCustomer[Constants.CUSTOMER_ID] != null) this.customerID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_ID].ToString());
                if (htblCustomer[Constants.TESCOGROUPEMAILID] != null) this.TescoGroupEmailFlag = Convert.ToInt16(htblCustomer[Constants.TESCOGROUPEMAILID].ToString());
                if (htblCustomer[Constants.PARTNEREMAILID] != null) this.PartnerEmailFlag = Convert.ToInt16(htblCustomer[Constants.PARTNEREMAILID].ToString());
                if (htblCustomer[Constants.RESEARCHEMAILID] != null) this.ResearchEmailFlag = Convert.ToInt16(htblCustomer[Constants.RESEARCHEMAILID].ToString());

                object[] objDBParams = { customerID, customerID, 0, TescoGroupEmailFlag, 0, 0, 0, PartnerEmailFlag, 0, 0, 0, ResearchEmailFlag, 0, 0 };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_EMAILPREFERENCES, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateEmailPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateEmailPreferences");


            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateEmailPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateEmailPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateEmailPreferences");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion


        #region Get EmailPreferences

        public String GetEmailPreferences(Int64 customerID)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetEmailPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetEmailPreferences - customerID:" + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GETEMAILPREFERENCES, customerID);
                ds.Tables[0].TableName = "EmailPreferences";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetEmailPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetEmailPreferences - viewXml:" + viewXml.ToString());


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetEmailPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetEmailPreferences Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetEmailPreferences");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #endregion


        #region My rewards - sudhakar
        /// <summary>
        /// Get Customer Rewards points from FD
        /// </summary>
        /// <param name="PrimaryCardNumber"></param>
        /// <param name="rewards"></param>
        /// <returns></returns>

        public bool GetCustomerRewards(Int64 PrimaryCardNumber, out float rewards)
        {


            ProcessFDRequestClient pClient = null;
            bool rstatus = false;
            rewards = 0;
            string strResponse = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustomerRewards PrimaryCardNumber-" + PrimaryCardNumber.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustomerRewards PrimaryCardNumber-" + PrimaryCardNumber.ToString());
                float balance = 0;
                pClient = new ProcessFDRequestClient();
                Logger.Write("", "GetRewards", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "star: Loyalty Get Rewards PrimaryCardNum  " + PrimaryCardNumber);
                int status = pClient.BalanceEnquiry(out balance, PrimaryCardNumber);
                if (status == 0)
                {
                    rewards = balance;
                    Logger.Write("", "GetRewards", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "end: Loyalty GetRewards PrimaryCardNum  " + PrimaryCardNumber);
                    rstatus = true;
                }
                else
                {
                   
                    ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetFDResponseCode", status);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strResponse = ds.Tables[0].Rows[0]["ResponseDesc"].ToString();
                        Logger.Write("", "GetRewards", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "Loyalty GetRewards Response Code,Primary card   " + strResponse + " , " + PrimaryCardNumber);
                    }
                    rstatus = false;
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustomerRewards");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustomerRewards");

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustomerRewards PrimaryCardNumber-" + PrimaryCardNumber + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Microsite.LoyaltyEntityService.Customer.GetCustomerRewards PrimaryCardNumber-" + PrimaryCardNumber + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustomerRewards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                rstatus = false;
                throw ex;
            }
            finally
            {
                if (pClient != null)
                {
                    pClient.Close();

                }
            }
            return rstatus;
        }

        ///<summary>
        /// To Convert points to Cash and update the detials in NGC Db

        /// </summary>
        public bool ConvertPointsToCash(Int64 primaryCardID, Int32 totalPoints)
        {
            ProcessFDRequestClient pClient = null;
            bool success = false;
            DataSet ds = new DataSet();
            string strResponse = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints" + totalPoints.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints" + totalPoints.ToString());
                Int32 convertionRate = Convert.ToInt32(ConfigurationSettings.AppSettings["ConvertionRate"].ToString());
                Int32 tPoints = totalPoints;
                Int32 cash = 0;
                Int32 remainPoints = 0;
                Int32 convertedPoints = 0;
                cash = tPoints / convertionRate;
                remainPoints = tPoints % convertionRate;
                convertedPoints = totalPoints - remainPoints;
                Logger.Write("", "PointsConvertionData", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "Loyalty Points Convertion PrimaryCardNum and Cash " + primaryCardID + "," + cash);
                pClient = new ProcessFDRequestClient();
                int status = pClient.LoadRewards(primaryCardID, cash);

                if (status == 0)
                {
                    object[] objRewardParams = { primaryCardID, cash, convertedPoints, remainPoints };
                    
                    SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_GIFTCARD_ADJUSTMENT, objRewardParams);
                    success = true;
                }
                else
                {
                    
                    ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetFDResponseCode", status);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strResponse = ds.Tables[0].Rows[0]["ResponseDesc"].ToString();
                        Logger.Write("", "LoadRewards", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "Loyalty Load Rewards Response Code, Primarycard   " + strResponse + " , " + primaryCardID);
                    }
                    return false;
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints - " + totalPoints.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints - " + totalPoints.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints - " + totalPoints.ToString() + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ConvertPointsToCash primaryCardID-" + primaryCardID.ToString() + "TotalPoints - " + totalPoints.ToString() + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ConvertPointsToCash");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
                throw ex;

            }
            finally
            {
                if (pClient != null)
                {
                    pClient.Close();

                }
            }
            return success;

        }

        public String GetAccountOverviewDetails(Int64 customerID)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetAccountOverviewDetails customerID-" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetAccountOverviewDetails customerID-" + customerID.ToString());
               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_USP_GETACCOUNTOVERVIEWDETAILS, customerID);
                ds.Tables[0].TableName = "Points";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetAccountOverviewDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetAccountOverviewDetails viewXml-" + viewXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetAccountOverviewDetails customerID-" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetAccountOverviewDetails customerID-" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetAccountOverviewDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }




        ///<summary>
        /// TO Get the points expired pointa and Expiry Date

        /// </summary>
        public string GetPointsExpiry(Int64 PrimaryCardID)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPointsExpiry PrimaryCardID-" + PrimaryCardID);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPointsExpiry PrimaryCardID-" + PrimaryCardID);
               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_POINTSEXPIRY, PrimaryCardID);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "PointsExpirydate";
                    ds.Tables[1].TableName = "ExpiredPoints";

                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPointsExpiry");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPointsExpiry viewXml-" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPointsExpiry PrimaryCardID-" + PrimaryCardID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPointsExpiry PrimaryCardID-" + PrimaryCardID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPointsExpiry");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;

        }

        ///<summary>
        /// TO Get the points expired pointa and Expiry Date

        /// </summary>
        public string GetRewardExpiryDate(Int64 PrimaryCardID)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetRewardExpiryDate PrimaryCardID-" + PrimaryCardID);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetRewardExpiryDate PrimaryCardID-" + PrimaryCardID);
               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_REWARDEXPIRYDATE, PrimaryCardID);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RewadsExpiry";

                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetRewardExpiryDate");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetRewardExpiryDate viewXml-" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetRewardExpiryDate PrimaryCardID-" + PrimaryCardID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetRewardExpiryDate PrimaryCardID-" + PrimaryCardID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetRewardExpiryDate");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;

        }
        #endregion


        #region GetCustUseMailStatus - ReActivate Account
        /// <summary>
        /// GetCustUseMailStatus -- It is used to fetch existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>
        /// <param name="int">out rowCount</param>
        /// <param name="string">culture</param>
        public String GetCustUseMailStatus(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustUseMailStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustUseMailStatus - conditionXml :" + conditionXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
                this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID];
                
                object[] objDBParams = { int.Parse(ExistingCustomerId) };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_USEMAILStatus, objDBParams);
                ds.Tables[0].TableName = "Customer";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustUseMailStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustUseMailStatus - sReqult :" + sReqult.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustUseMailStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCustUseMailStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustUseMailStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return sReqult;
        }
        #endregion

        #region UpdateCustomerStatus

        /// <summary>

        /// InsertCardNo -- It is used to insert card no of a customer into account

        /// </summary>

        /// <param name="string">objectXml</param>

        /// <param name="string">out resultXml</param>

        public bool UpdateCustStatus(string objectXml, out string resultXml)
        {

            resultXml = string.Empty;

            bool success = true;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustStatus - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "custid");
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                
                object[] objDBParams = { long.Parse(CustId) };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_STATUS, objDBParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustStatus");
            }

            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                success = false;

            }

            finally
            {



            }

            return success;

        }

        #endregion



        #region UpdateUseMailStatus
        /// <summary>
        /// UpdateUseMailStatus -- It is used to update existing customer details against customerid of a customer 
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool UpdateUseMailStatus(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            bool result = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateUseMailStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateUseMailStatus - objectXml" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "existingcustomer");
                if (htblCustomer[Constants.EXISTINGCUSTOMER_ID] != null) this.ExistingCustomerId = (string)htblCustomer[Constants.EXISTINGCUSTOMER_ID].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailId = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                object[] objDBParams = { ExistingCustomerId };
                
                // Change by Neeta for T n C to get userName
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_UPDATE_USEMAIL_STATUS, objDBParams);
                resultXml = ds.Tables[0].Rows[0]["EmailID"].ToString();
                result = true;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateUseMailStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateUseMailStatus");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateUseMailStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateUseMailStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateUseMailStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                result = false;
                resultXml = SqlHelper.resultXml;
                throw ex;
            }
            finally
            {

            }
            return result;
            //return SqlHelper.result.Flag;
        }
        #endregion
        //Methods for Reward Card Management -- Kavitha


        #region Update Card Status as Stolen
        /// <summary>
        /// UpdateCardStatus -- It is used to update the card status as stolen,lost or damaged
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool UpdateCardStatus(string objectXml, out string resultXml, out string errorMessage)
        {
            resultXml = string.Empty;
            errorMessage = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCardStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCardStatus - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "cardno");
                if (htblCustomer[Constants.CLUBCARDID] != null) this.ClubcardId = (string)htblCustomer[Constants.CLUBCARDID].ToString();
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                if (htblCustomer[Constants.CLUBCARD_STATUS] != null) this.ClubcardStatus = int.Parse(htblCustomer[Constants.CLUBCARD_STATUS].ToString());
                //Update the Clubcard Status in FD

                ProcessFDRequestClient pClient = new ProcessFDRequestClient();
                Int32 responseCode = 0;
                try
                {
                    responseCode = pClient.ReportLostStolen(Convert.ToInt64(ClubcardId));
                }
                catch (TimeoutException ex)
                {
                    
                    object[] objDBParams = { Int64.Parse(CustId), Int64.Parse(ClubcardId), ClubcardStatus, 0 };
                    SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARD_STATUS, objDBParams);
                }
                catch (Exception ex)
                {
                    
                    object[] objDBParams = { Int64.Parse(CustId), Int64.Parse(ClubcardId), ClubcardStatus, 0 };
                    SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARD_STATUS, objDBParams);
                }
                if (responseCode.ToString() == "0")
                {
                    
                    object[] objDBParams = { Int64.Parse(CustId), Int64.Parse(ClubcardId), ClubcardStatus, 0 };
                    SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARD_STATUS, objDBParams);
                }
                else
                {

                    
                    ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetFDResponseCode", responseCode);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        errorMessage = ds.Tables[0].Rows[0]["ResponseDesc"].ToString();
                        Logger.Write("", "LoadRewards", 1, 9500, System.Diagnostics.TraceEventType.Verbose, "Loyalty Change Card Status Response Code   " + errorMessage);
                    }
                    return false;
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCardStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCardStatus");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCardStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCardStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCardStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region Get All Clubcards
        /// <summary>
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String GetAllClubcards(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetAllClubcards customerID -  " + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetAllClubcards customerID -  " + customerID.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_ALl_CLUBCARD, customerID);
                ds.Tables[0].TableName = "Clubcards";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetAllClubcards customerID -  " + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetAllClubcards viewXml -  " + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetAllClubcards - customerID :  " + customerID.ToString() + " Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetAllClubcards - customerID :  " + customerID.ToString() + " Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetAllClubcards");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region Update PhoneNumber
        /// <summary>
        /// UpdateCardStatus -- It is used to update the card status as stolen,lost or damaged
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool UpdatePhoneNumber(string objectXml, out string resultXml)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdatePhoneNumber");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdatePhoneNumber - objectXml :" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "PhoneNumber");
                if (htblCustomer[Constants.PHONE_NO] != null) this.PhoneNo = (string)htblCustomer[Constants.PHONE_NO].ToString();
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                
                object[] objDBParams = { Int64.Parse(CustId), PhoneNo };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_PHONENUMBER, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdatePhoneNumber");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdatePhoneNumber");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdatePhoneNumber - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdatePhoneNumber - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdatePhoneNumber");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                throw ex;
            }
            finally
            {

            }
            return SqlHelper.result.Flag;
        }
        #endregion

        #region Update Primary Clubcard
        /// <summary>
        /// UpdateCardStatus -- It is used to update the card status as stolen,lost or damaged
        /// </summary>
        /// <param name="string">objectXml</param>
        /// <param name="string">out resultXml</param>
        public bool UpdatePrimaryClubcard(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdatePrimaryClubcard");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdatePrimaryClubcard - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "PhoneNumber");
                if (htblCustomer[Constants.CLUBCARD_ID] != null) this.ClubcardId = (string)htblCustomer[Constants.CLUBCARD_ID].ToString();
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                
                object[] objDBParams = { Int64.Parse(CustId), long.Parse(ClubcardId) };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_PRIMARYCLUBCARDID, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdatePrimaryClubcard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdatePrimaryClubcard");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdatePrimaryClubcard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdatePrimaryClubcard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdatePrimaryClubcard");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion
        //End of Reward Card Management


        #endregion

        #region SignIn

        /// <summary>
        ///  to get the Terms and Conditions status
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string GetCustomerStatus(string username)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustomerStatus - UserName : " + username);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustomerStatus - UserName : " + username);
               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_TERMSNCONDITIONSTATUS, username);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TNCstatus";

                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustomerStatus - viewXml : " + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustomerStatus - Error Message : " + ex.ToString() + " - UserName : " + username);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCustomerStatus - Error Message : " + ex.ToString() + " - UserName : " + username);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustomerStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }



        ///<summary>
        /// To Convert points to Cash and update the detials in NGC Db

        /// </summary>
        public bool UpdateCustomerTermsNConstionsStatus(Int64 customerID)
        {
            bool success;
            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus - customerID : " + customerID.ToString());
                object[] objRewardParams = { customerID };
                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_GET_UPDATETERMSNCONDITION, objRewardParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus");

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCustomerTermsNConstionsStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                success = false;
                throw ex;

            }
            finally
            {
            }
            return success;

        }
        #endregion

        #region My Coupons -- Noushad

        /// <summary>
        /// Method to get coupons based on customerID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public String GetCoupons(Int64 customerID)
        {

            //Declare an object of DataSet.
            DataSet dsCoupons = new DataSet();

            //Declare and set return variable to empty.
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCoupons - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCoupons - customerID : " + customerID.ToString());

            

                //Call ExecuteDataset method to get coupons.
                dsCoupons = SqlHelper.ExecuteDataset(connectionString, Constants.SP_USP_GETCOUPONS, customerID);

                //Set dataset table name.
                dsCoupons.Tables[0].TableName = "Coupons";

                //Convert dataset to xml.
                viewXml = dsCoupons.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCoupons");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCoupons - viewXml : " + viewXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCoupons - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCoupons - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCoupons");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                //Throw the exception.
                throw ex;


            }
            finally
            {

            }
            //Return result xml.
            return viewXml;
        }

        /// <summary>
        /// Method to update coupons based on customerID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public Boolean UpdateCoupons(Int64 customerID, string couponStatus)
        {
            //Declare return variable and set to false.
            Boolean result = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCoupons - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCoupons - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
                //Call ExecuteNonQuery to update coupons.
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_USP_UPDATECOUPONS, customerID, couponStatus);

                //Set return variable to true.
                result = true;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCoupons - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCoupons - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCoupons - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCoupons - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString() + " - CouponStatus :" + couponStatus);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCoupons");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //Throw the exception.
                throw ex;


            }
            finally
            {

            }
            //Return true or false.
            return result;
        }

        #endregion



        public string GetAboutMeDeatils(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            throw new NotImplementedException();
        }

        // CSC Change - Neeta

        #region AddCustomer
        /// <summary>
        /// AddCustomer -- It is used to Add Customer --  Join Functionality.
        /// </summary>
        /// <param name="string">updateXml</param>
        /// <param name="string">out errorXml</param>
        public bool AddCustomer(string objectXml, int sessionUserID, out string resultXml, int welcomepoint)
        {
            bool status = false;
            resultXml = string.Empty;
            string userStatus = string.Empty;
            DataSet ds = new DataSet();
            SqlHelper.resultXml = string.Empty;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddCustomer - sessionUserID : " + sessionUserID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddCustomer - objectXml : " + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "AddCustomer");
                if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID] != null)
                {
                    if (htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString() != "")
                    {
                        this.ClubcardID = Convert.ToInt64(htblCustomer[Constants.CUSTOMER_CLUBCARD_ID].ToString());
                    }
                }
                if (htblCustomer[Constants.FIRST_NAME] != null) this.FirstName = (string)htblCustomer[Constants.FIRST_NAME].ToString();
                if (htblCustomer[Constants.LAST_NAME] != null) this.LastName = (string)htblCustomer[Constants.LAST_NAME].ToString();
                if (htblCustomer[Constants.EMAIL_ID] != null) this.EmailAddress = (string)htblCustomer[Constants.EMAIL_ID].ToString();
                object[] objDBParams = { ClubcardID, FirstName, LastName, EmailAddress, sessionUserID, welcomepoint };
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_ADD_CUSTOMER, objDBParams);
                if (ds.Tables.Count > 0)
                {
                    USLoyaltySecurityServiceLayer.SecurityService objsec = new SecurityService();

                    Hashtable htSecurityService = new Hashtable();
                    htSecurityService["emailsAddress"] = EmailAddress;
                    htSecurityService["password"] = "A@%d&d9m0i@";
                    htSecurityService["secretQuestion"] = "what is pet name?";
                    htSecurityService["secretAns"] = "j34j34h9sd";
                    htSecurityService["customerId"] = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                    string insertSecurityXml = HashTableToXML(htSecurityService, "UserDetails");
                    objsec.CreateUserCsc(insertSecurityXml, out userStatus);
                    if (userStatus == "Username already exists.")
                        status = false;
                    else
                        status = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddCustomer - sessionUserID : " + sessionUserID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddCustomer");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddCustomer - Error Message : " + ex.ToString() + " - sessionUserID : " + sessionUserID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddCustomer - Error Message : " + ex.ToString() + " - sessionUserID : " + sessionUserID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = "<exception><message>" + ex.Message + "</message></exception>";
                status = false;
            }
            finally
            {

            }

            return status;
        }
        #endregion
        public static string HashTableToXML(Hashtable ht, string objName)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    writer.WriteStartElement(objName);
                    foreach (DictionaryEntry item in ht)
                    {
                        writer.WriteStartElement(item.Key.ToString());
                        writer.WriteValue(item.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Convert.ToString(sb);
        }
        #region View TransactionsByOffer CSC
        /// <summary>
        /// To get (view) the Transactions based on the given CustomerID
        /// </summary>
        public String TransactionsByOfferCSC(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");
            Int64 offerID = 0;
            Int64 primaryCustomerID = 0;
            Int64 ClubcardTransID = 0;
            string houseHold = "";
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.TransactionsByOfferCSC");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.TransactionsByOfferCSC - conditionXml:" + conditionXml.ToString());
                if (htblCustomer["OfferNumberToShow"] != null)
                {
                    if (htblCustomer["OfferNumberToShow"].ToString() != "")
                    {
                        offerID = Convert.ToInt64(htblCustomer["OfferNumberToShow"].ToString());
                    }
                }
                if (htblCustomer["PrimaryCustomerID"] != null)
                {
                    if (htblCustomer["PrimaryCustomerID"].ToString() != "")
                    {
                        primaryCustomerID = Convert.ToInt64(htblCustomer["PrimaryCustomerID"].ToString());
                    }
                }
                if (htblCustomer["CustomerID"] != null)
                {
                    if (htblCustomer["CustomerID"].ToString() != "")
                    {
                        this.CustomerID = Convert.ToInt64(htblCustomer["CustomerID"].ToString());
                    }
                }
                if (htblCustomer["ClubcardTransactionID"] != null)
                {
                    if (htblCustomer["ClubcardTransactionID"].ToString() != "")
                    {
                        ClubcardTransID = Convert.ToInt64(htblCustomer["ClubcardTransactionID"].ToString());
                    }
                }
                houseHold = (string)htblCustomer["HouseHold"];

                object[] objDBParams = { offerID, primaryCustomerID, CustomerID, houseHold, ClubcardTransID, culture };

                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TRANSACTIONS_BY_OFFER_ID_CSC, objDBParams);
                if (ds.Tables.Count > 0)
                    ds.Tables[0].TableName = "ClubcardTransaction";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.TransactionsByOfferCSC");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.TransactionsByOfferCSC - viewXml:" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.TransactionsByOfferCSC - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.TransactionsByOfferCSC - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.TransactionsByOfferCSC");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        public bool SendEmail(string email, string urlLink, string emailType)
        {
            bool emailStatus = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.SendEmail - Email : " + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.SendEmail - Email : " + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);
                ExactTargetServiceSoapClient objET = new ExactTargetServiceSoapClient();
                SendEmailRequestRequest objETS = new SendEmailRequestRequest();
                SendEmailRequestRequestDetails[] emailDetails = new SendEmailRequestRequestDetails[4];

                SendEmailRequestRequestDetails detLink = new SendEmailRequestRequestDetails();
                SendEmailRequestRequestDetails detFN = new SendEmailRequestRequestDetails();
                SendEmailRequestRequestDetails detLN = new SendEmailRequestRequestDetails();
                SendEmailRequestRequestDetails detTitle = new SendEmailRequestRequestDetails();

                detLink.Name = "Link_Url";
                detLink.Value = urlLink;
                
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetCustomerName", email);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    detFN.Name = "LastName";
                    detFN.Value = ds.Tables[0].Rows[0]["Lastname"].ToString();
                    detLN.Name = "FirstName";
                    detLN.Value = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    if (ds.Tables[0].Rows[0]["Title"] != null)
                    {
                        detTitle.Name = "Title";
                        detTitle.Value = ds.Tables[0].Rows[0]["Title"].ToString();
                    }
                }
                else
                {
                    detFN.Name = "LastName";
                    detFN.Value = "";
                    detLN.Name = "FirstName";
                    detLN.Value = "";
                    detTitle.Name = "Title";
                    detTitle.Value = "";
                }

                emailDetails[0] = detLink;
                emailDetails[1] = detFN;
                emailDetails[2] = detLN;
                objETS.EmailAddress = email;
                objETS.EmailType = emailType;// "TestingAPI2";
                objETS.Details = emailDetails;
                objET.SendEmailRequest(objETS);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.SendEmail - Email : " + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.SendEmail - Email : " + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);

                emailStatus = true;

            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.SendEmail - Error Message : " + Ex.ToString() + " - Email :" + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.SendEmail - Error Message : " + Ex.ToString() + " - Email :" + email + " - Urllink : " + urlLink + " - Emailtype : " + emailType);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.SendEmail");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
                emailStatus = true;
            }

            return emailStatus;

        }


        #endregion

        #region GetTransactionReasonCode
        /// <summary>
        /// Get the groups 
        /// </summary>
        /// <returns>Get Transaction ReasonCode details in xml format</returns>        

        public string GetTransactionReasonCode()
        {

            DataSet dsGroups = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetTransactionReasonCode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetTransactionReasonCode");
                //Execute SP to get the role membership details of the User
                dsGroups = SqlHelper.ExecuteDataset(connectionString, Constants.SP_Get_TransactionReasons);
                dsGroups.Tables[0].TableName = "Clubcard";
                viewXml = dsGroups.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetTransactionReasonCode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetTransactionReasonCode - viewXml:" + viewXml.ToString());
            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetTransactionReasonCode - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetTransactionReasonCode - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetTransactionReasonCode");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

        #region Get Points Balance
        /// <summary>
        /// To get (view) the Transactions based on the given CustomerID
        /// </summary>
        public String GetPointsBalance(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "customer");

            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPointsBalance");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPointsBalancece - conditionXml:" + conditionXml.ToString());
                if (htblCustomer["CustomerID"] != null)
                {
                    if (htblCustomer["CustomerID"].ToString() != "")
                    {
                        this.CustomerID = Convert.ToInt64(htblCustomer["CustomerID"].ToString());
                    }
                }

                object[] objDBParams = { CustomerID };

               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_POINTSBALANCE, objDBParams);
                if (ds.Tables.Count > 0)
                    ds.Tables[0].TableName = "Clubcard";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPointsBalance");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPointsBalance - viewXml:" + viewXml.ToString());
            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPointsBalance - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPointsBalance - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPointsBalance");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
            }
            finally
            {

            }
            return viewXml;
        }



        #endregion

        #region ADD POINTS
        /// <summary>
        /// To Add the ClubCard Type details to the database
        /// </summary>

        public bool AddPoints(string objectXml, out long objectId, out string resultXml, int userID)
        {

            objectId = 0;
            int MaxPoints = 0;
            int MinPoints = 0;

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddPoints");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddPoints - objectXml:" + objectXml.ToString());
                Hashtable htPCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htPCardDetails[Constants.CUSTOMER_NO]);
                this.pointsBalanceQty = Convert.ToInt32(htPCardDetails[Constants.TOTAL_POINTS]);
                this.ReasonCodeID = Convert.ToInt16(htPCardDetails[Constants.TXN_REASON_ID]);
                MaxPoints = Convert.ToInt32(htPCardDetails["MaxPoints"]);
                MinPoints = Convert.ToInt32(htPCardDetails["MinPoints"]);
                
                object[] objAddClubcard = { pointsBalanceQty, CustomerID, ReasonCodeID, MaxPoints, MinPoints, userID };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_POINTS, objAddClubcard);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddPoints");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddPoints");
            }
            catch (Exception Ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddPoints - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddPoints - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddPoints");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
                return false;
            }
            finally
            {

            }
            return SqlHelper.result.Flag;

        }
        #endregion

        public String GetPointsSummary(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            DataSet dsYearly = new DataSet();
            string viewXml = String.Empty;
            string ExpBatchDate;
            rowCount = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetPointsSummary");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetPointsSummary - conditionXml:" + conditionXml.ToString());
                //load all xml values in hashtable
                Hashtable htmlPointSummary = ConvertXmlHash.XMLToHashTable(conditionXml, "PointSummary");
                //load values in hashtable in object properties and other variables
                this.CustomerID = Convert.ToInt64(htmlPointSummary[Constants.CUSTOMER_ID]);
                ExpBatchDate = Convert.ToString(htmlPointSummary[Constants.EXP_BATCH_DATE]);
                this.Culture = culture;


                object[] objRewardParams = { 
                                        customerID,
                                        ExpBatchDate
                                     };
              
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_POINTSSUMMARY, objRewardParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "PointsSummary";
                    rowCount = ds.Tables[0].Rows.Count;
                }
                dsYearly = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_POINTSSUMMARY_YEARLY, objRewardParams);

                if (dsYearly.Tables.Count > 0)
                {
                    dsYearly.Tables[0].TableName = "PointsSummaryYearly";
                    ds.Tables.Add(dsYearly.Tables[0].Copy());
                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetPointsSummary");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetPointsSummary - viewXml" + viewXml.ToString());
            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetPointsSummary - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetPointsSummary - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetPointsSummary");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
                throw Ex;
            }
            finally
            {

            }
            return viewXml;
        }

        public bool UpdateCardStatusCSC(string objectXml, out string resultXml)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCardStatusCSC");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCardStatusCSC - objectXml:" + objectXml.ToString());
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "cardno");
                if (htblCustomer[Constants.CLUBCARDID] != null) this.ClubcardId = (string)htblCustomer[Constants.CLUBCARDID].ToString();
                if (htblCustomer[Constants.CUST_ID] != null) this.CustId = (string)htblCustomer[Constants.CUST_ID].ToString();
                if (htblCustomer[Constants.CLUBCARD_STATUS] != null) this.ClubcardStatus = int.Parse(htblCustomer[Constants.CLUBCARD_STATUS].ToString());
                //this.ClubcardStatus = cardStatus;
              
                object[] objDBParams = { Int64.Parse(CustId), Int64.Parse(ClubcardId), ClubcardStatus, 0 };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARD_STATUS_CSC, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCardStatusCSC");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCardStatusCSC");
            }
            catch (Exception Ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCardStatusCSC - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCardStatusCSC - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCardStatusCSC");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
                throw Ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }



        #region CSC -- Noushad

        /// <summary>
        /// Method to check whether user is admin or not.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsAdminUser(string userName)
        {

            //Declare an object of DataSet.
            DataSet dsUserDetails = new DataSet();

            //Declare and set return variable false.
            bool isAdmin = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.IsAdminUser - UserName :" + userName);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.IsAdminUser - UserName :" + userName);
                //Get connection string from machine.config.
                

                //Call ExecuteDataset method to get coupons.
                dsUserDetails = SqlHelper.ExecuteDataset(connectionString, Constants.SP_USP_GETUSERROLE, userName);

                //Set dataset table name.
                dsUserDetails.Tables[0].TableName = "UserDetails";

                if (dsUserDetails != null)
                {
                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables["UserDetails"].Rows.Count > 0)
                        {
                            isAdmin = true;
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.IsAdminUser - UserName :" + userName);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.IsAdminUser - UserName :" + userName);
            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.IsAdminUser - Error Message : " + Ex.ToString() + " - UserName :" + userName);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.IsAdminUser - Error Message : " + Ex.ToString() + " - UserName :" + userName);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.IsAdminUser");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);

                //Throw the exception.
                throw Ex;


            }
            finally
            {

            }
            //Return result xml.
            return isAdmin;
        }

        #endregion
        #region GetConfigDetails
        /// <summary>
        /// Gets all the titles.
        /// </summary>
        /// <returns></returns>
        public String GetConfigDetails(string objectXml, out int rowCount, string culture)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetConfigDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetConfigDetails");
              
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CONFIGDETAILS, objectXml);
                ds.Tables[0].TableName = "ActiveDateRangeConfig";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetConfigDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetConfigDetails");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetConfigDetails - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetConfigDetails - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetConfigDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
                ds = null;
            }
            return viewXml;
        }
        #endregion

        #region UpdateConfig
        /// <summary>Update Config details</summary>
        /// <returns> Returns 0 or >0  if success, otherwise returns -1 </returns>
        /// <remarks>To Update CCO Config details </remarks>

        public bool UpdateConfig(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            resultXml = string.Empty;
            objectID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateConfig");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateConfig");
                Hashtable htConfig = ConvertXmlHash.XMLToHashTable(objectXml, "ActiveDateRangeConfig");
                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");



                if (htConfig["PtSummStartDate"] != null) this.ptSummStartDate = Convert.ToDateTime(htConfig["PtSummStartDate"].ToString(), enGBCulture);
                if (htConfig["PtSummEndDate"] != null) this.ptSummEndDate = Convert.ToDateTime(htConfig["PtSummEndDate"].ToString(), enGBCulture);
                if (htConfig["ExchangesStartDate"] != null) this.exchangesStartDate = Convert.ToDateTime(htConfig["ExchangesStartDate"].ToString(), enGBCulture);
                if (htConfig["ExchangesEndDate"] != null) this.exchangesEndDate = Convert.ToDateTime(htConfig["ExchangesEndDate"].ToString(), enGBCulture);
                if (htConfig["CurXmasStartDate"] != null) this.curXmasStartDate = Convert.ToDateTime(htConfig["CurXmasStartDate"].ToString(), enGBCulture);
                if (htConfig["CurXmasEndDate"] != null) this.curXmasEndDate = Convert.ToDateTime(htConfig["CurXmasEndDate"].ToString(), enGBCulture);
                if (htConfig["NextXmasStartDate"] != null) this.nextXmasStartDate = Convert.ToDateTime(htConfig["NextXmasStartDate"].ToString(), enGBCulture);
                if (htConfig["NextXmasEndDate"] != null) this.nextXmasEndDate = Convert.ToDateTime(htConfig["NextXmasEndDate"].ToString(), enGBCulture);
                if (htConfig["VoucherStartDate"] != null) this.voucherStartDate = Convert.ToDateTime(htConfig["VoucherStartDate"].ToString(), enGBCulture);
                if (htConfig["VoucherEndDate"] != null) this.voucherEndDate = Convert.ToDateTime(htConfig["VoucherEndDate"].ToString(), enGBCulture);

                if (htConfig["LatestStatementStartDate"] != null) this.latestStatementStartDate = Convert.ToDateTime(htConfig["LatestStatementStartDate"].ToString(), enGBCulture);
                if (htConfig["LatestStatementEndDate"] != null) this.latestStatementEndDate = Convert.ToDateTime(htConfig["LatestStatementEndDate"].ToString(), enGBCulture);

                if (htConfig["Flag"] != null) this.flag = Convert.ToInt16(htConfig["Flag"].ToString());


                object[] objDBParams = { ptSummStartDate, ptSummEndDate,exchangesStartDate,exchangesEndDate,curXmasStartDate,
                                          curXmasEndDate,nextXmasStartDate,nextXmasEndDate,voucherStartDate,voucherEndDate,latestStatementStartDate,latestStatementEndDate, flag, sessionUserID };

                
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CONFIGDETAILS, objDBParams);
                objectID = Convert.ToInt64(this.CustomerID);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateConfig");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateConfig");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateConfig - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateConfig - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateConfig");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region Print@home report
        /// <summary>
        /// To add print vouchers/Tokens details for reporting purpose.
        /// </summary>
        /// <param name="objectXml">Input parameter as XML string</param>
        /// <param name="resultXml">Output parameter</param>
        /// <returns></returns>
        public bool AddPrintAtHomeDetails(string objectXml, out string resultXml)
        {
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddPrintAtHomeDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddPrintAtHomeDetails");
               
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_INSERT_PRINTDETAILS, objectXml);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddPrintAtHomeDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddPrintAtHomeDetails");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddPrintAtHomeDetails - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddPrintAtHomeDetails - Error Message : " + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddPrintAtHomeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region LoadPreferences
        /// <summary>
        /// Gets all the titles.
        /// </summary>
        /// <returns></returns>
        public String LoadPreferences(out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.LoadPreferences");
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_LOAD_PREFERENCES);
                ds.Tables[0].TableName = "Preferences";
                rowCount = ds.Tables[0].Rows.Count;
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.LoadPreferences");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.LoadPreferences - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.LoadPreferences - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.LoadPreferences - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.LoadPreferences");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion


        #region Validate Email Link
        /// <summary>
        /// To validate the customer when user clicks on the email link.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string ValidateEmailLink(string guid)
        {
            
            string customerID = String.Empty;
            
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ValidateEmailLink");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ValidateEmailLink");
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                object[] objParams={guid};
                object obj = SqlHelper.ExecuteScalar(connectionString, Constants.SP_VALIDATE_EMAIL_LINK, objParams);
                if(obj!=null)
                customerID = Convert.ToString(obj);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ValidateEmailLink");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ValidateEmailLink - customerID :" + customerID);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ValidateEmailLink - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ValidateEmailLink - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ValidateEmailLink");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return customerID;

        }
        #endregion 


        #region SecurityLayer - Madhu

        public string GetCustomerVerificationDetails(string CustID)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.GetCustomerVerificationDetails - CustID : " + CustID);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.GetCustomerVerificationDetails - CustID : " + CustID);

                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_CUSTOMERVERIFICATION, CustID);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "SecurityStatus";

                }
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.GetCustomerVerificationDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.GetCustomerVerificationDetails - viewXml : " + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.GetCustomerVerificationDetails - Error Message : " + ex.ToString() + " - CustID : " + CustID);
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.GetCustomerVerificationDetails - Error Message : " + ex.ToString() + " - CustID : " + CustID);
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.GetCustomerVerificationDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        public bool InsertUpdateCustomerVerificationDetails(string objectXml)
        {
           
            //resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails - objectXml : " + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails - objectXml : " + objectXml);

                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "CustomerVerification");



                if (htblCustomer["Browserused"] != null) this.BrowserUsed = htblCustomer["Browserused"].ToString();
                if (htblCustomer["IPAddress"] != null) this.IPAddress = htblCustomer["IPAddress"].ToString();
                if (htblCustomer["CustomerID"] != null) this.CustomerID= Convert.ToInt64(htblCustomer["CustomerID"]);
                if (htblCustomer["IsValidAttempt"] != null) this.IsValidAttempt = htblCustomer["IsValidAttempt"].ToString();


                object[] objDBParams = { CustomerID, BrowserUsed, IPAddress, IsValidAttempt };


                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_SET_CUSTOMERVERIFICATION, objDBParams);
               // objectID = Convert.ToInt64(this.CustomerID);

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails - objectID : " + this.CustomerID.ToString());
            

            }
            catch (Exception ex)
            {
                //resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails - Error Message : " + ex.ToString() + " - customerID : " + customerID.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.InsertUpdateCustomerVerificationDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                
                throw ex;

            }
            finally
            {
            }
            return SqlHelper.result.Flag;

        }

        #endregion 
    }
}
