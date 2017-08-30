/*
 * File   : ImportCustomer.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Import Customer
 * Date   : 21/Aug/2008
 * 
 * 
 */

/** UPDATED HISTORY
 * Update Version No:1
 * Updated By:Sabhareesan O.K
 * Updated Purpose:For NGC V 3.6
 * 
 * 
 * Update Version No:2
 * Updated By:Jenny Joy
 * Updated Purpose:For CR034
 * 
 * **/

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

using System.Configuration;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;
using System.Xml;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using NGCBatchConsoleApplication.CustomerService;
using System.Data;
using NGCBatchConsoleApplication;
//using Fujitsu.eCrm.Generic.LocalizationLibrary;


#endregion


namespace Tesco.NGC.BatchConsoleApplication
{
    public class ImportCustomer
    {

        #region Local Variables for getting the config setting details

        //For SSN
        string sSSNFormat1 = "";
        int iSSNMinValue = 0;
        int iSSNMaxValue = 0;
        bool bIsSSNMandatory = false;
        bool isSSNValid = false;

        //For Passport
        string sPassportFormat1 = "";
        int iPassportMinValue = 0;
        int iPassportMaxValue = 0;
        bool bIsPassportMandatory = false;
        bool isPassportValid = false;

        //Post Code

        string sPostCodeFormat1 = "";
        int iPostCodeMinValue = 0, iPostCodeMaxValue = 0;
        bool bIsPostCodeMandatory = false;
        bool isPostValid = false;

		//Added for CR034 
        //MailingAddressLine1
        int iAddressLine1MinValue = 0;
        int iAddressLine1MaxValue = 0;
        bool bIsAddressLine1Mandatory = false;
        bool isAddressLine1Valid = false;
        string sAddressLine1Format1 = "";

        //MailingAddressLine2
        int iAddressLine2MinValue = 0;
        int iAddressLine2MaxValue = 0;
        bool bIsAddressLine2Mandatory = false;
        bool isAddressLine2Valid = false;
        string sAddressLine2Format1 = "";

        //MailingAddressLine3
        int iAddressLine3MinValue = 0;
        int iAddressLine3MaxValue = 0;
        bool bIsAddressLine3Mandatory = false;
        bool isAddressLine3Valid = false;
        string sAddressLine3Format1 = "";

        //MailingAddressLine4
        int iAddressLine4MinValue = 0;
        int iAddressLine4MaxValue = 0;
        bool bIsAddressLine4Mandatory = false;
        bool isAddressLine4Valid = false;
        string sAddressLine4Format1 = "";

        //MailingAddressLine5
        int iAddressLine5MinValue = 0;
        int iAddressLine5MaxValue = 0;
        bool bIsAddressLine5Mandatory = false;
        bool isAddressLine5Valid = false;
        string sAddressLine5Format1 = "";

        //MailingAddressLine6
        int iAddressLine6MinValue = 0;
        int iAddressLine6MaxValue = 0;
        bool bIsAddressLine6Mandatory = false;
        bool isAddressLine6Valid = false;
        string sAddressLine6Format1 = "";

        //Name1 
        int iName1MinValue = 0;
        int iName1MaxValue = 0;
        bool bIsName1Mandatory = false;
        bool isName1Valid = false;
        string sName1Format = "";

        //Name2
        int iName2MinValue = 0;
        int iName2MaxValue = 0;
        bool bIsName2Mandatory = false;
        bool isName2Valid = false;
        string sName2Format = "";

        //Name3
        int iName3MinValue = 0;
        int iName3MaxValue = 0;
        bool bIsName3Mandatory = false;
        bool isName3Valid = false;
        string sName3Format = "";
        //
		//CR034 Ends
        #endregion

        #region Fields

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int32 customerID;

        /// <summary>
        /// Customer Title
        /// </summary>
        private string title;

        /// <summary>
        /// Customer Name1
        /// </summary>
        private string name1;

        private string nameAsInNRIC;

        private string cardMemberDOB;

        private string cardMemberGender;

        private string businessName;

        private string businessRegistrationNumber;

        /// <summary>
        /// Customer Name2
        /// </summary>
        private string name2;

        /// <summary>
        /// Customer Name3
        /// </summary>
        private string name3;

        /// <summary>
        /// family_member_1_dob
        /// </summary>
        private string dateOfBirth;

        /// <summary>
        /// family_member_2_dob
        /// </summary>
        private string familyMember2Dob;

        /// <summary>
        /// family_member_3_dob
        /// </summary>
        private string familyMember3Dob;

        /// <summary>
        /// family_member_4_dob
        /// </summary>
        private string familyMember4Dob;

        /// <summary>
        /// family_member_5_dob
        /// </summary>
        private string familyMember5Dob;

        /// <summary>
        /// family_member_6_dob
        /// </summary>
        private string familyMember6Dob;

        /// <summary>
        /// FamilyMember1GenderCode
        /// </summary>
        private string sex;

        /// <summary>
        /// FamilyMember2GenderCode
        /// </summary>
        private string familyMember2GenderCode;

        /// <summary>
        /// FamilyMember3GenderCode
        /// </summary>
        private string familyMember3GenderCode;

        /// <summary>
        /// FamilyMember4GenderCode
        /// </summary>
        private string familyMember4GenderCode;

        /// <summary>
        /// FamilyMember5GenderCode
        /// </summary>
        private string familyMember5GenderCode;

        /// <summary>
        /// FamilyMember6GenderCode
        /// </summary>
        private string familyMember6GenderCode;

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
        private string longitude;

        /// <summary>
        /// Customer Latitude
        /// </summary>
        private string latitude;

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
        //private string faxNumber;
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
        /// AllowPromotionsViaMail
        /// </summary>
        private string allowPromotionsViaMail;

        /// <summary>
        /// AllowPromotionsViaPhone
        /// </summary>
        private string allowPromotionsViaPhone;

        /// <summary>
        /// AllowGroupPromotions
        /// </summary>
        private string allowGroupPromotions;

        /// <summary>
        /// AllowThirdPartyPromotions
        /// </summary>
        private string allowThirdPartyPromotions;

        /// <summary>
        /// Diabetic
        /// </summary>
        private string addressInErrorValue;

        /// <summary>
        /// Diabetic
        /// </summary>
        private string diabetic;

        /// <summary>
        /// Vegetarian
        /// </summary>
        private string vegetarian;

        /// <summary>
        /// Teetotal
        /// </summary>
        private string teetotal;

        /// <summary>
        /// Kosher
        /// </summary>
        private string kosher;

        /// <summary>
        /// Halal
        /// </summary>
        private string halal;

        /// <summary>
        /// Celiac
        /// </summary>
        private string celiac;

        /// <summary>
        /// Lactose
        /// </summary>
        private string lactose;

        //Optional Dietary preferences. Added for V3.1.1 by Netra for requirement id 033b

        /// <summary>
        /// optional1
        /// </summary>
        private string optional1;

        /// <summary>
        /// optional2
        /// </summary>
        private string optional2;

        /// <summary>
        /// optional3
        /// </summary>
        private string optional3;

        //Change Cpmplete.

        //Data Protection Prefereces --V3.1 [Req ID:007]
        /// <summary>
        /// TescoGroupMail
        /// </summary>
        private string tescoGroupMail;

        /// <summary>
        /// TescoGroupEmail
        /// </summary>
        private string tescoGroupEmail;

        /// <summary>
        /// TescoGroupPhone
        /// </summary>
        private string tescoGroupPhone;

        /// <summary>
        /// TescoGroupSms
        /// </summary>
        private string tescoGroupSms;

        /// <summary>
        /// PartnerMail
        /// </summary>
        private string partnerMail;

        /// <summary>
        /// PartnerEmail
        /// </summary>
        private string partnerEmail;

        /// <summary>
        /// PartnerPhone
        /// </summary>
        private string partnerPhone;

        /// <summary>
        /// PartnerSms
        /// </summary>
        private string partnerSms;

        /// <summary>
        /// ResearchMail
        /// </summary>
        private string researchMail;

        /// <summary>
        /// ResearchEmail
        /// </summary>
        private string researchEmail;

        /// <summary>
        /// ResearchPhone
        /// </summary>
        private string researchPhone;

        /// <summary>
        /// ResearchSms
        /// </summary>
        private string researchSms;

        /// <summary>
        /// Foreign
        /// </summary>
        private string foreign;

        /// <summary>
        /// Expat
        /// </summary>
        private string expat;

        /// <summary>
        /// PreviousLoyaltySchemeClubcardId
        /// </summary>
        private string previousLoyaltySchemeClubcardId;

        /// <summary>
        /// FormType
        /// </summary>
        private string formType;

        /// <summary>
        /// Customer IncomeBandID
        /// </summary>
        private string incomeBandID;

        /// <summary>
        /// FamilyMember1Age
        /// </summary>
        private string familyMember1Age;

        /// <summary>
        /// FamilyMember2Age
        /// </summary>
        private string familyMember2Age;

        /// <summary>
        /// FamilyMember3Age
        /// </summary>
        private string familyMember3Age;

        /// <summary>
        /// FamilyMember4Age
        /// </summary>
        private string familyMember4Age;

        /// <summary>
        /// FamilyMember5Age
        /// </summary>
        private string familyMember5Age;

        /// <summary>
        /// FamilyMember6Age
        /// </summary>
        private string familyMember6Age;

        /// <summary>
        /// Customer CustomerMailStatus
        /// </summary>
        private string customerMailStatus;

        private string raceID;

        private string preferredMailingAddress;

        private string insertBy;

        /// <summary>
        /// Customer BusinessCustomerInd
        /// </summary>
        private string businessCustomerInd;

        /// <summary>
        /// Customer BusinessType
        /// </summary>
        private string businessType;

        /// <summary>
        /// Customer CustomerSegmentID
        /// </summary>
        private string customerSegmentID;

        /// <summary>
        /// Customer PreferredStoreID
        /// </summary>
        private string preferredStoreID;

        /// <summary>
        /// Customer JoinedDate
        /// </summary>
        private string customerCreatedDate;

        /// <summary>
        /// Customer JoinedStoreID
        /// </summary>
        private string joinedStoreID;

        /// <summary>
        /// Customer InactiveCollectionPeriodQty
        /// </summary>
        private Int16 inactiveCollectionPeriodQty;

        /// <summary>
        /// Customer CustomerUseStatusID
        /// </summary>
        private string customerUseStatusID;

        /// <summary>
        /// Customer PreviousLoyaltySchemeInd
        /// </summary>
        private string previousLoyaltySchemeInd;

        /// <summary>
        /// Customer Primary ClubcardID
        /// </summary>
        private string primaryClubcardID;

        /// <summary>
        /// Customer ClubcardID
        /// </summary>
        private string clubcardID;

        private Int64 previousOfferPrimaryCustomerID;

        /// <summary>
        /// Customer Alternate ID
        /// </summary>
        private string custAlternateID;


        ///<summary>
        ///Number Of House Hold Member
        ///</summary>
        private string noofHouseHoldMember;
        /// <summary>
        /// Customer PreferenceID
        /// </summary>
        private string preferredContactTypeCode;
        private string cardInvalid = "Card is Invalid";
        private string ArchiveRootDirectory;
        private string inputPath;
        //NGCV32 req.No:003
        private string geocodeConfidence;

        //ADDED FOR NGC V 3.6
        /// <summary>
        /// For Join PromotionCode
        /// </summary>
        private string _JoinPromotionCode;

        private string _SSNNumber;
        private string _PassportNumber;
        private string _BuinessLicenseNumber;

        //DECLARE THE CUSTOMER WCF SERVICE FOR VALIDATE THE TELEPHONE NO,POSTAL CODE,VALIDATE SSN -NGC 3.6
        CustomerServiceClient custClient;


        //DECLARE THE DATASET TO STORE THE CONFIG DETAILS
        DataSet dsConfigDetails;

        //Store the Error Code and Validation type For Mobile,SSN, And Passport         

        private string sErrorCode = "";

        private string sValidationFails = "";

        //Variable created for CustomerEMailStatus and CustomerMobilePhoneStatus

        private string CustomerEMailStatus = "";
        private string CustomerMobilePhoneStatus = "";

        //variable to check formtype is set the value or not
        private bool isFormTypeSet = false;

        #endregion

        #region Properties

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int32 CustomerID { get { return this.customerID; } set { this.customerID = value; } }

        /// <summary>
        ///  Customer Title
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                #region Old Code

                //if (value == "Mr" || value == "Mrs" || value == "Ms")
                //{
                //    this.title = value;
                //}
                //else
                //{
                //    this.title = "Unknown";
                //} 

                #endregion

                #region New Code
                //Author : Sabhareesan O.K

                if (FormType.Equals("1"))
                {
                    bool bIsTitleMandatory = false;
                    bool isTitleValid = false;

                    #region FETCH THE CONFIG VALUE FOR Title
                    //retrieve the Title Config Details
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        //For Title Mandantory
                        if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "TitleEnglish")
                        {
                            if (dr["ConfigurationValue1"].ToString().Equals("1"))
                                bIsTitleMandatory = true;
                        }

                    }

                    #endregion

                    #region Set the Value


                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        if (value == "Mr" || value == "Mrs" || value == "Ms")
                        {
                            this.title = value;
                        }
                        else
                        {
                            this.title = "Unknown";
                        }

                        isTitleValid = true;
                    }
                    else
                    {
                        if (!bIsTitleMandatory)
                        {
                            this.title = "Unknown";
                            isTitleValid = true;
                        }
                        //if mandatory
                        else
                        {
                            this.title = "Unknown";
                            sValidationFails += "TitleCheck,";
                            isTitleValid = false;
                        }
                    }


                    if (!isTitleValid)
                        sErrorCode += ConfigurationManager.AppSettings["TitleEnglishInError"].ToString() + ",";

                    #endregion

                }
                else
                {
                    if (value == "Mr" || value == "Mrs" || value == "Ms")
                    {
                        this.title = value;
                    }
                    else
                    {
                        this.title = "Unknown";
                    }
                }

                #endregion


            }

        }

        /// <summary>
        ///  NoOfHouseHoldMember
        /// </summary>
        public string NoOfHouseHoldMember
        {
            get { return this.noofHouseHoldMember; }
            set
            {
                Regex regex = new Regex("^\\b\\d+\\b$");

                if (!regex.IsMatch(value))
                {
                    this.noofHouseHoldMember = null; // Assignment value changed from "0" to null - by Sakthi
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        //Author:Sabhareesan O.K
                        //Reason: As column is declared as TINYINT ( changed from 999999 to 255)
                        if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 255)
                        {
                            this.noofHouseHoldMember = null; // Assignment value changed from "0" to null - by Sakthi
                        }
                        else
                        {
                            this.noofHouseHoldMember = value;
                        }
                    }
                    else
                    {
                        this.noofHouseHoldMember = null; // Assignment value changed from "0" to null - by Sakthi
                    }
                }

            }
        }

        /// <summary>
        ///  NameAsInNRIC
        /// </summary>
        public string NameAsInNRIC
        {
            get { return this.nameAsInNRIC; }
            set
            {
                if (value != string.Empty)
                {
                    if (value.Length > 99)
                    {
                        this.nameAsInNRIC = value.Substring(0, 99);
                    }
                    else this.nameAsInNRIC = value;
                }
                else
                    this.nameAsInNRIC = "";

            }
        }

        /// <summary>
        ///  CardMemberDOB
        /// </summary>
        public string CardMemberDOB
        {
            get { return this.cardMemberDOB; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.cardMemberDOB = null;
                }
                else
                {
                    this.cardMemberDOB = value;
                    if (Convert.ToDateTime(this.cardMemberDOB) > DateTime.Now)
                        this.cardMemberDOB = null;
                    else
                        this.cardMemberDOB = value;
                }
            }
        }

        public string CardMemberGender
        {
            get { return this.cardMemberGender; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.cardMemberGender = "-1";
                //}
                //else
                //{
                //    //if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    if (Convert.ToString(value) != "0" && Convert.ToString(value) != "1")
                //    {
                //        this.cardMemberGender = "-1";
                //    }
                //    else
                //    {
                //        this.cardMemberGender = value;
                //    }
                //}
                this.cardMemberGender = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  BusinessName
        /// </summary>
        public string BusinessName
        {
            get { return this.businessName; }
            set
            {
                if (value != string.Empty)
                {
                    if (value.Length > 36)
                    {
                        this.businessName = value.Substring(0, 35);
                    }
                    else this.businessName = value;
                }
                else
                    this.businessName = "";

            }
        }



        /// <summary>
        ///  BusinessRegistrationNumber
        /// </summary>
        public string BusinessRegistrationNumber
        {
            get { return this.businessRegistrationNumber; }
            set
            {
                if (value != string.Empty)
                {
                    if (value.Length > 14)
                    {
                        this.businessRegistrationNumber = value.Substring(0, 13);
                    }
                    else this.businessRegistrationNumber = value;
                }
                else
                    this.businessRegistrationNumber = "";

            }
        }



        /// <summary>
        ///  Customer Name1
        /// </summary>
        public string Name1
        {
            #region Code Till CR34
            //// get { return this.name1; }
            ////set
            ////{
            ////    #region Old Code
            ////    //if (value == string.Empty || value.Length > 50)
            ////    //{
            ////    //    this.name1 = "Unknown";
            ////    //}
            ////    //else
            ////    //{
            ////    //    this.name1 = value;
            ////    //} 
            ////    #endregion

            ////    #region New Code
            ////    //Author : Sabhareesan O.K

            ////    if (FormType.Equals("1"))
            ////    {
            ////        bool bIsName1Mandatory = false;
            ////        bool isName1Valid = false;

            ////        int iName1MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MinValue"].ToString().Trim());
            ////        int iName1MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MaxValue"].ToString().Trim());

            ////        #region FETCH THE CONFIG VALUE FOR Name1
            ////        //retrieve the Name1 Config Details
            ////        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            ////        {
            ////            //For Name 1 Mandantory
            ////            if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name1")
            ////            {
            ////                if (dr["ConfigurationValue1"].ToString().Equals("1"))
            ////                    bIsName1Mandatory = true;
            ////            }


            ////        }

            ////        #endregion

            ////        #region Set the Value

            ////        if (!string.IsNullOrEmpty(value.Trim()))
            ////        {

            ////            if (value.Trim() == string.Empty || value.Trim().Length > 50)
            ////            {
            ////                this.name1 = "Unknown";
            ////            }
            ////            else
            ////            {
            ////                this.name1 = value;
            ////            }
            ////            isName1Valid = true;
            ////        }
            ////        else
            ////        {
            ////            if (!bIsName1Mandatory)
            ////            {
            ////                this.name1 = "UnKnown";
            ////                isName1Valid = true;
            ////            }
            ////            //if mandatory
            ////            else
            ////            {
            ////                this.name1 = "";
            ////                sValidationFails += "Name1Check,";
            ////                isName1Valid = false;
            ////            }
            ////        }
            ////        if (!isName1Valid)
            ////            sErrorCode += ConfigurationManager.AppSettings["Name1InError"].ToString() + ",";

            ////        #endregion
            ////    }
            ////    else
            ////    {
            ////        if (value == string.Empty || value.Length > 50)
            ////        {
            ////            this.name1 = "Unknown";
            ////        }
            ////        else
            ////        {
            ////            this.name1 = value;
            ////        }
            ////    }
            ////    #endregion

            ////}
            #endregion

            #region CR34 Changes - Author: Jenny,Date:Mar 14 2013

            get { return this.name1; }
            set
            {
                 iName1MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MinValue"].ToString().Trim());
                 iName1MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MaxValue"].ToString().Trim());
                 bIsName1Mandatory = false;
                 isName1Valid = false;

                    #region FETCH THE CONFIG VALUE FOR Name1
                    //retrieve the Name1 Config Details
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                        {
                            if (dr["ConfigurationValue1"].ToString().Equals("1"))
                                bIsName1Mandatory = true;
                        }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                    {
                        iName1MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MinValue"].ToString().Trim()));
                        iName1MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name1MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                    {
                        sName1Format = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the Name1 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iName1MinValue) && (value.Length <= iName1MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sName1Format.Trim(), false, false))
                            {
                                #region New Code
                                isName1Valid = true;
                                this.name1 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isName1Valid = false;
                                if (bIsName1Mandatory)
                                    sValidationFails += "Name1check,";
                                //capture the value even if validation fails
                                this.name1 = value;  
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isName1Valid = false;
                            //if mandatory
                            if (bIsName1Mandatory)
                                sValidationFails += "Name1check,";
                                //capture the value even if validation fails
                                this.name1 = value;  
                        }
                    }
                    //if value is empty
                    else
                    {
                        ////isName1Valid = false;
                        ////this.name1 = value;

                        //If mandatory
                        if (bIsName1Mandatory)
                        {
                            this.name1 = value;
                            isName1Valid = false;
                            //Validation Fails
                            sValidationFails += "Name1check,";
                        }
                        else
                        {
                            this.name1 = "UNKNOWN";  // currently in Name1 uses UNKNOWN 
                            isName1Valid = true;
                        }
                    }
                    //If any error, Add Name1  in Error.             
                    if (!isName1Valid)
                        sErrorCode += ConfigurationManager.AppSettings["Name1InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Fetch Name1 from config table and check for format
                        if ((value.Length >= iName1MinValue) && (value.Length <= iName1MaxValue))
                        {
                            if (Helper.IsRegexMatch(value.Trim(), sName1Format.Trim(), false, false))
                            {
                                this.name1 = value;
                            }
                            else
                            {
                                this.name1 = "UNKNOWN"; //"";  shd it pass UNKNOWN or "" 
                            }
                        }
                        else
                        {
                            this.name1 = "UNKNOWN"; //"";   shd it pass UNKNOWN or "" 
                        }
                    }
                    else
                    {
                        this.name1 = "UNKNOWN";
                    }
                }
            }

            #endregion
        }

        /// <summary>
        ///  Customer Name2
        /// </summary>
        public string Name2
        {
            #region Code Till CR34
            //get { return this.name2; }
            //set
            //{
            //    #region Old Code
            //    //if (value == string.Empty)
            //    //{
            //    //    this.name2 = "";
            //    //}
            //    //else if (value.Length > 50)
            //    //{
            //    //    this.name2 = value.ToString().Substring(0, 49);
            //    //}
            //    //else
            //    //{
            //    //    this.name2 = value;
            //    //} 
            //    #endregion

            //    #region New Code
            //    //Author : Sabhareesan O.K

            //    if (FormType.Equals("1"))
            //    {

            //        bool bIsName2Mandatory = false;
            //        bool isName2Valid = false;

            //        #region FETCH THE CONFIG VALUE FOR Name2
            //        //retrieve the Name2 Config Details
            //        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            //        {
            //            //For Name 2 Mandantory
            //            if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name2")
            //            {
            //                if (dr["ConfigurationValue1"].ToString().Equals("1"))
            //                    bIsName2Mandatory = true;
            //            }

            //        }

            //        #endregion

            //        #region Set the Value


            //        if (!string.IsNullOrEmpty(value.Trim()))
            //        {

            //            if (value.Trim() == string.Empty)
            //            {
            //                this.name2 = "";
            //            }
            //            else if (value.Trim().Length > 50)
            //            {
            //                this.name2 = value.Trim().ToString().Substring(0, 49);
            //            }
            //            else
            //            {
            //                this.name2 = value;
            //            }

            //            isName2Valid = true;

            //        }
            //        else
            //        {
            //            if (!bIsName2Mandatory)
            //            {
            //                this.name2 = "";
            //                isName2Valid = true;
            //            }
            //            //if mandatory
            //            else
            //            {
            //                this.name2 = "";
            //                sValidationFails += "Name2Check,";
            //                isName2Valid = false;
            //            }
            //        }


            //        if (!isName2Valid)
            //            sErrorCode += ConfigurationManager.AppSettings["Name2InError"].ToString() + ",";

            //        #endregion
            //    }
            //    else
            //    {
            //        if (value == string.Empty)
            //        {
            //            this.name2 = "";
            //        }
            //        else if (value.Length > 50)
            //        {
            //            this.name2 = value.ToString().Substring(0, 49);
            //        }
            //        else
            //        {
            //            this.name2 = value;
            //        }
            //    }


            //    #endregion
            //}
            #endregion

            #region CR34 Changes - Author: Jenny,Date:Mar 14 2013

            get { return this.name2; }
            set
            {
                 iName2MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name2MinValue"].ToString().Trim());
                 iName2MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name2MaxValue"].ToString().Trim());
                 bIsName2Mandatory = false;
                 isName2Valid = false;

                #region FETCH THE CONFIG VALUE FOR Name2
                //retrieve  Name2 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsName2Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        iName2MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name2MinValue"].ToString().Trim()));
                        iName2MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name2MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        sName2Format = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the Name2 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iName2MinValue) && (value.Length <= iName2MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sName2Format.Trim(), false, false))
                            {
                                isName2Valid = true;
                                this.name2 = value;
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isName2Valid = false;
                                if (bIsName2Mandatory)
                                    sValidationFails += "Name2check,";
                                //capture the value even if validation fails
                                this.name2 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isName2Valid = false;
                            //if mandatory
                            if (bIsName2Mandatory)
                                sValidationFails += "Name2check,";
                            //capture the value even if validation fails
                            this.name2 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                          //If mandatory
                        if (bIsName2Mandatory)
                        {
                            this.name2 = value;
                            isName2Valid = false;
                            //Validation Fails
                            sValidationFails += "Name2check,";
                        }
                        else
                        {
                            this.name2 = "";
                            isName2Valid = true;
                        }
                    }
                    //If any error, Add Name1  in Error.             
                    if (!isName2Valid)
                        sErrorCode += ConfigurationManager.AppSettings["Name2InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Fetch Name1 from config table and check for format
                        if ((value.Length >= iName2MinValue) && (value.Length <= iName2MaxValue))
                        {
                            if (Helper.IsRegexMatch(value.Trim(), sName2Format.Trim(), false, false))
                            {
                                this.name2 = value;
                            }
                            else
                            {
                                this.name2 = "";
                            }
                        }
                        else
                        {
                            this.name2 = value.ToString().Substring(0, iName2MaxValue); //current logic returns substring value - confrim if this is correct
                        }
                    }
                    else
                    {
                        this.name2 = "";
                    }
                }
            }

            #endregion
        }

        /// <summary>
        ///  Customer Name3
        /// </summary>
        public string Name3
        {
            #region Code Till CR34
            ////get { return this.name3; }
            ////set
            ////{
            ////    #region Old Code
            ////    //if (value == string.Empty || value.Length > 50)
            ////    //{
            ////    //    this.name3 = "";
            ////    //}
            ////    //else
            ////    //{
            ////    //    this.name3 = value;
            ////    //} 
            ////    #endregion

            ////    #region New Code
            ////    //Author : Sabhareesan O.K

            ////    if (FormType.Equals("1"))
            ////    {

            ////        bool bIsName3Mandatory = false;
            ////        bool isName3Valid = false;

            ////        #region FETCH THE CONFIG VALUE FOR Name3
            ////        //retrieve the Name3 Config Details
            ////        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            ////        {
            ////            //For Name 3 Mandantory
            ////            if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name3")
            ////            {
            ////                if (dr["ConfigurationValue1"].ToString().Equals("1"))
            ////                    bIsName3Mandatory = true;
            ////            }

            ////        }

            ////        #endregion

            ////        #region Set the Value


            ////        if (!string.IsNullOrEmpty(value.Trim()))
            ////        {

            ////            if (value.Trim() == string.Empty || value.Trim().Length > 50)
            ////            {
            ////                this.name3 = "";
            ////            }
            ////            else
            ////            {
            ////                this.name3 = value;
            ////            }
            ////            isName3Valid = true;

            ////        }
            ////        else
            ////        {
            ////            if (!bIsName3Mandatory)
            ////            {
            ////                this.name3 = "";
            ////                isName3Valid = true;
            ////            }
            ////            //if mandatory
            ////            else
            ////            {
            ////                this.name3 = "";
            ////                sValidationFails += "Name3Check,";
            ////                isName3Valid = false;
            ////            }
            ////        }


            ////        if (!isName3Valid)
            ////            sErrorCode += ConfigurationManager.AppSettings["Name3InError"].ToString() + ",";

            ////        #endregion
            ////    }
            ////    else
            ////    {
            ////        if (value == string.Empty || value.Length > 50)
            ////        {
            ////            this.name3 = "";
            ////        }
            ////        else
            ////        {
            ////            this.name3 = value;
            ////        }
            ////    }

            ////    #endregion
            ////}
            #endregion

            #region CR34 Changes - Author: Jenny,Date:Mar 14 2013

            get { return this.name3; }
            set
            {
                 iName3MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name3MinValue"].ToString().Trim());
                 iName3MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["Name3MaxValue"].ToString().Trim());
                 bIsName3Mandatory = false;
                 isName3Valid = false;

                #region FETCH THE CONFIG VALUE FOR Name3
                //retrieve  Name3 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsName3Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        iName3MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name3MinValue"].ToString().Trim()));
                        iName3MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["Name3MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        sName3Format = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the Name3 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iName3MinValue) && (value.Length <= iName3MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sName3Format.Trim(), false, false))
                            {
                                isName3Valid = true;
                                this.name3 = value;
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isName3Valid = false;
                                if (bIsName3Mandatory)
                                    sValidationFails += "Name3check,";
                                //capture the value even if validation fails
                                this.name3 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isName3Valid = false;
                            //if mandatory
                            if (bIsName3Mandatory)
                                sValidationFails += "Name3check,";
                            //capture the value even if validation fails
                            this.name3 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        //If mandatory
                        if (bIsName3Mandatory)
                        {
                            this.name3 = value;
                            isName3Valid = false;
                            //Validation Fails
                            sValidationFails += "Name3check,";
                        }
                        else
                        {
                            this.name3 = "";
                            isName3Valid = true;
                        }
                    }
                    //If any error, Add Name1  in Error.             
                    if (!isName3Valid)
                        sErrorCode += ConfigurationManager.AppSettings["Name3InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Fetch Name1 from config table and check for format
                        if ((value.Length >= iName3MinValue) && (value.Length <= iName3MaxValue))
                        {
                            if (Helper.IsRegexMatch(value.Trim(), sName3Format.Trim(), false, false))
                            {
                                this.name3 = value;
                            }
                            else
                            {
                                this.name3 = "";
                            }
                        }
                        else
                        {
                            this.name3 = "";
                        }
                    }
                    else
                    {
                        this.name3 = "";
                    }
                }
            }

            #endregion
        }

        /// <summary>
        ///  FamilyMember1Dob
        /// </summary>
        public string DateOfBirth
        {
            get { return this.dateOfBirth; }
            set
            {
                #region Old Code
                //Regex regex = new Regex(Constants.DATE_FORMATE);

                //if (!regex.IsMatch(value))
                //{
                //    this.dateOfBirth = null;
                //}
                //else
                //{
                //    this.dateOfBirth = value;
                //    if (Convert.ToDateTime(dateOfBirth) > DateTime.Now)
                //        this.dateOfBirth = null;
                //    else
                //        this.dateOfBirth = value;
                //}

                #endregion

                bool bIsDOBMandatory = false;
                bool isDOBValid = false;

                #region FETCH THE CONFIG VALUE FOR DOB
                //retrieve the DOB Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For DOB Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "DateOfBirth")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsDOBMandatory = true;
                    }

                }

                #endregion

                #region Set the Value


                if (!string.IsNullOrEmpty(value.Trim()))
                {

                    Regex regex = new Regex(Constants.DATE_FORMATE);

                    if (!regex.IsMatch(value))
                    {
                        this.dateOfBirth = null;
                    }
                    else
                    {
                        this.dateOfBirth = value;
                        if (Convert.ToDateTime(dateOfBirth) > DateTime.Now)
                            this.dateOfBirth = null;
                        else
                            this.dateOfBirth = value;
                    }

                    //check for DOB is null or not. 
                    if (this.dateOfBirth != null)
                    {
                        isDOBValid = true;
                    }
                    //if null
                    else
                    {
                        //if mandatory, set the Error flag. 
                        if (bIsDOBMandatory)
                        {
                            sValidationFails += "DOBCheck,";
                        }

                        isDOBValid = false;
                    }
                }
                else
                {
                    if (!bIsDOBMandatory)
                    {
                        this.dateOfBirth = null;
                        isDOBValid = true;
                    }
                    //if mandatory
                    else
                    {
                        this.dateOfBirth = null;
                        sValidationFails += "DOBCheck,";
                        isDOBValid = false;
                    }
                }


                if (!isDOBValid)
                    sErrorCode += ConfigurationManager.AppSettings["DOBInError"].ToString() + ",";

                #endregion


            }
        }

        /// <summary>
        ///  FamilyMember2Dob
        /// </summary>
        public string FamilyMember2DOB
        {
            get { return this.familyMember2Dob; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.familyMember2Dob = null;
                }
                else
                {
                    this.familyMember2Dob = value;
                    if (Convert.ToDateTime(this.familyMember2Dob) > DateTime.Now)
                        this.familyMember2Dob = null;
                    else
                        this.familyMember2Dob = value;
                }
            }
        }


        /// <summary>
        ///  FamilyMember3DOB
        /// </summary>
        public string FamilyMember3DOB
        {


            get { return this.familyMember3Dob; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.familyMember3Dob = null;
                }
                else
                {
                    this.familyMember3Dob = value;
                    if (Convert.ToDateTime(this.familyMember3Dob) > DateTime.Now)
                        this.familyMember3Dob = null;
                    else
                        this.familyMember3Dob = value;
                }
            }
        }


        /// <summary>
        ///  FamilyMember4DOB
        /// </summary>
        public string FamilyMember4DOB
        {
            get { return this.familyMember4Dob; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.familyMember4Dob = null;
                }
                else
                {
                    this.familyMember4Dob = value;
                    if (Convert.ToDateTime(this.familyMember4Dob) > DateTime.Now)
                        this.familyMember4Dob = null;
                    else
                        this.familyMember4Dob = value;
                }
            }
        }


        /// <summary>
        ///  FamilyMember5DOB
        /// </summary>
        public string FamilyMember5DOB
        {
            get { return this.familyMember5Dob; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.familyMember5Dob = null;
                }
                else
                {
                    this.familyMember5Dob = value;
                    if (Convert.ToDateTime(this.familyMember5Dob) > DateTime.Now)
                        this.familyMember5Dob = null;
                    else
                        this.familyMember5Dob = value;
                }
            }
        }


        /// <summary>
        ///  FamilyMember6DOB
        /// </summary>
        public string FamilyMember6DOB
        {
            get { return this.familyMember6Dob; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.familyMember6Dob = null;
                }
                else
                {
                    this.familyMember6Dob = value;
                    if (Convert.ToDateTime(this.familyMember6Dob) > DateTime.Now)
                        this.familyMember6Dob = null;
                    else
                        this.familyMember6Dob = value;
                }
            }
        }


        /// <summary>
        ///  Family_member_1_gender_code
        /// </summary>
        public string Sex
        {
            get { return this.sex; }
            set
            {
                #region Old Old code
                ////Regex regex = new Regex("[0-9]");

                ////if (!regex.IsMatch(value))
                ////{
                ////    this.sex = "-1";
                ////}
                ////else
                ////{
                ////    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                ////    {
                ////        this.sex = "-1";
                ////    }
                ////    else
                ////    {
                ////        this.sex = value;
                ////    }
                ////}

                #endregion


                #region Old Code-Latest

                //this.sex = ArragneGenderCode(value);


                #endregion

                bool bIsSexMandatory = false;
                bool isSexValid = false;

                #region FETCH THE CONFIG VALUE FOR Sex
                //retrieve the Sex Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Sex Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Sex")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsSexMandatory = true;
                    }

                }

                #endregion
                if (FormType.Equals("1"))
                {
                    #region Set the Value


                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        this.sex = ArragneGenderCode(value);
                        isSexValid = true;
                    }
                    else
                    {
                        //if not mandatory
                        if (!bIsSexMandatory)
                        {
                            this.sex = "";
                            isSexValid = true;
                        }
                        //if mandatory
                        else
                        {
                            this.sex = "";
                            isSexValid = false;
                            sValidationFails += "GenderCheck,";
                        }
                    }

                    if (!isSexValid)
                        sErrorCode += ConfigurationManager.AppSettings["GenderInError"].ToString() + ",";

                    #endregion
                }
                //for formtype 2 - Added for the defect - MKTG00009352 - Madhusudhan.
                else
                {
                    //Fetch Gender 
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        this.sex = ArragneGenderCode(value);
                    }
                    else
                    {
                        this.sex = "";
                    }
                   

                }
            }
        }

        /// <summary>
        ///  Family_member_2_gender_code
        /// </summary>
        public string FamilyMember2GenderCode
        {
            get { return this.familyMember2GenderCode; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.familyMember2GenderCode = "-1";
                //}
                //else
                //{
                //    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.familyMember2GenderCode = "-1";
                //    }
                //    else
                //    {
                //        this.familyMember2GenderCode = value;
                //    }
                //}
                this.familyMember2GenderCode = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  Family_member_3_gender_code
        /// </summary>
        public string FamilyMember3GenderCode
        {
            get { return this.familyMember3GenderCode; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.familyMember3GenderCode = "-1";
                //}
                //else
                //{
                //    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.familyMember3GenderCode = "-1";
                //    }
                //    else
                //    {
                //        this.familyMember3GenderCode = value;
                //    }
                //}
                this.familyMember3GenderCode = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  Family_member_4_gender_code
        /// </summary>
        public string FamilyMember4GenderCode
        {
            get { return this.familyMember4GenderCode; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.familyMember4GenderCode = "-1";
                //}
                //else
                //{
                //    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.familyMember4GenderCode = "-1";
                //    }
                //    else
                //    {
                //        this.familyMember4GenderCode = value;
                //    }
                //}
                this.familyMember4GenderCode = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  Family_member_5_gender_code
        /// </summary>
        public string FamilyMember5GenderCode
        {
            get { return this.familyMember5GenderCode; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.familyMember5GenderCode = "-1";
                //}
                //else
                //{
                //    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.familyMember5GenderCode = "-1";
                //    }
                //    else
                //    {
                //        this.familyMember5GenderCode = value;
                //    }
                //}
                this.familyMember5GenderCode = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  Family_member_6_gender_code
        /// </summary>
        public string FamilyMember6GenderCode
        {
            get { return this.familyMember6GenderCode; }
            set
            {
                //Regex regex = new Regex("[0-9]");

                //if (!regex.IsMatch(value))
                //{
                //    this.familyMember6GenderCode = "-1";
                //}
                //else
                //{
                //    if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.familyMember6GenderCode = "-1";
                //    }
                //    else
                //    {
                //        this.familyMember6GenderCode = value;
                //    }
                //}
                this.familyMember6GenderCode = ArragneGenderCode(value);
            }
        }

        /// <summary>
        ///  Customer MailingAddressLine1
        /// </summary>
        public string MailingAddressLine1
        {
            get { return this.mailingAddressLine1; }
            set
            {
                #region CodeTillCR34
                //if (value == string.Empty)
                //{
                //    this.mailingAddressLine1 = "";
                //}
                //else
                //{
                //    if (value.Length > 60)
                //    {
                //        this.mailingAddressLine1 = value.Substring(0, 59);
                //    }
                //    else
                //        this.mailingAddressLine1 = value;
                //}
                #endregion

                #region CR34 Changes - Author: Jenny,Date:Mar 08 2013

                 iAddressLine1MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine1MinValue"].ToString().Trim());
                 iAddressLine1MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine1MaxValue"].ToString().Trim());
                 bIsAddressLine1Mandatory = false;
                 isAddressLine1Valid = false;

                #region FETCH THE CONFIG VALUE FOR AddressLine1
                //retrieve  MailingAddressLine1 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsAddressLine1Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1")
                    {
                        iAddressLine1MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine1MinValue"].ToString().Trim()));
                        iAddressLine1MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine1MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        sAddressLine1Format1 = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the AddressLine1 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iAddressLine1MinValue) && (value.Length <= iAddressLine1MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sAddressLine1Format1.Trim(), false, false))
                            {
                                #region New Code
                                isAddressLine1Valid = true;
                                this.mailingAddressLine1 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isAddressLine1Valid = false;
                                if (bIsAddressLine1Mandatory)
                                    sValidationFails += "AddressLine1check,";
                                //capture the value even if validation fails
                                this.mailingAddressLine1 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isAddressLine1Valid = false;
                            //if mandatory
                            if (bIsAddressLine1Mandatory)
                                sValidationFails += "AddressLine1check,";
                            //capture the value even if validation fails
                            this.mailingAddressLine1 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        isAddressLine1Valid = false;
                        this.mailingAddressLine2 = value;
                        //If mandatory
                        if (bIsAddressLine1Mandatory)
                        {
                            //capture the value even if validation fails
                            sValidationFails += "AddressLine1check,";
                        }
                    }
                    //If any error, Add MailingAddressLine2  in Error.             
                    if (!isAddressLine1Valid)
                        sErrorCode += ConfigurationManager.AppSettings["AddressLine1InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    //Fetch AddressLine1 from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sAddressLine1Format1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found
                        this.mailingAddressLine1 = value;
                    }
                    else
                    {
                        this.mailingAddressLine1 = "";
                    }
                }
                #endregion
            }
        }

        /// <summary>
        ///  Customer MailingAddressLine2
        /// </summary>
        public string MailingAddressLine2
        {
            get { return this.mailingAddressLine2; }
            set
            {
                #region CodeTillCR34
                //if (value == string.Empty)
                //{
                //    this.mailingAddressLine2 = "";
                //}
                //else
                //{
                //    if (value.Length > 80)
                //    {
                //        this.mailingAddressLine2 = value.Substring(0, 79);
                //    }
                //    else
                //        this.mailingAddressLine2 = value;
                //}
                #endregion

                #region CR34 Changes - Author: Jenny,Date:Mar 08 2013

                 iAddressLine2MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine2MinValue"].ToString().Trim());
                 iAddressLine2MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine2MaxValue"].ToString().Trim());
                 bIsAddressLine2Mandatory = false;
                 isAddressLine2Valid = false;

                #region FETCH THE CONFIG VALUE FOR AddressLine2
                //retrieve  MailingAddressLine2 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsAddressLine2Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                    {
                        iAddressLine2MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine2MinValue"].ToString().Trim()));
                        iAddressLine2MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine2MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        sAddressLine2Format1 = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the AddressLine2 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iAddressLine2MinValue) && (value.Length <= iAddressLine2MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sAddressLine2Format1.Trim(), false, false))
                            {
                                #region New Code
                                isAddressLine2Valid = true;
                                this.mailingAddressLine2 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isAddressLine2Valid = false;
                                if (bIsAddressLine2Mandatory)
                                    sValidationFails += "AddressLine2check,";
                                //capture the value even if validation fails
                                this.mailingAddressLine2 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isAddressLine2Valid = false;
                            //if mandatory
                            if (bIsAddressLine2Mandatory)
                                sValidationFails += "AddressLine2check,";
                            //capture the value even if validation fails
                            this.mailingAddressLine2 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        isAddressLine2Valid = false;
                        this.mailingAddressLine2 = value;
                        //If mandatory
                        if (bIsAddressLine2Mandatory)
                        {
                            //capture the value even if validation fails
                            sValidationFails += "AddressLine2check,";
                        }
                    }
                    //If any error, Add MailingAddressLine2  in Error.             
                    if (!isAddressLine2Valid)
                        sErrorCode += ConfigurationManager.AppSettings["AddressLine2InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    //Fetch AddressLine2 from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sAddressLine2Format1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found
                        this.mailingAddressLine2 = value;
                    }
                    else
                    {
                        this.mailingAddressLine2 = "";
                    }
                }
                #endregion
            }
        }

        /// <summary>
        ///  Customer MailingAddressLine3
        /// </summary>
        public string MailingAddressLine3
        {
            get { return this.mailingAddressLine3; }
            set
            {
                #region CodeTill CR34
                //if (value == string.Empty)
                //{
                //    this.mailingAddressLine3 = "";
                //}
                //else
                //{
                //    if (value.Length > 80)
                //    {
                //        this.mailingAddressLine3 = value.Substring(0, 79);
                //    }
                //    else
                //        this.mailingAddressLine3 = value;
                //}
                #endregion

                #region CR34 Changes - Author: Jenny,Date:Mar 08 2013

                 iAddressLine3MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine3MinValue"].ToString().Trim());
                 iAddressLine3MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine3MaxValue"].ToString().Trim());
                 bIsAddressLine3Mandatory = false;
                 isAddressLine3Valid = false;

                #region FETCH THE CONFIG VALUE FOR AddressLine5
                //retrieve  MailingAddressLine3 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsAddressLine3Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                    {
                        iAddressLine3MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine3MinValue"].ToString().Trim()));
                        iAddressLine3MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine3MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        sAddressLine3Format1 = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the AddressLine3 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iAddressLine3MinValue) && (value.Length <= iAddressLine3MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sAddressLine3Format1.Trim(), false, false))
                            {
                                #region New Code
                                isAddressLine3Valid = true;
                                this.mailingAddressLine3 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isAddressLine3Valid = false;
                                if (bIsAddressLine3Mandatory)
                                    sValidationFails += "AddressLine3check,";
                                //capture the value even if validation fails
                                this.mailingAddressLine3 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isAddressLine3Valid = false;
                            //if mandatory
                            if (bIsAddressLine3Mandatory)
                                sValidationFails += "AddressLine3check,";
                            //capture the value even if validation fails
                            this.mailingAddressLine3 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        isAddressLine3Valid = false;
                        this.mailingAddressLine3 = value;
                        //If mandatory
                        if (bIsAddressLine3Mandatory)
                        {
                            //capture the value even if validation fails
                            sValidationFails += "AddressLine3check,";
                        }
                    }
                    //If any error, Add MailingAddressLine3  in Error.             
                    if (!isAddressLine3Valid)
                        sErrorCode += ConfigurationManager.AppSettings["AddressLine3InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    //Fetch AddressLine3 from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sAddressLine3Format1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found
                        this.mailingAddressLine3 = value;
                    }
                    else
                    {
                        this.mailingAddressLine3 = "";
                    }
                }
                #endregion
            }
        }

        /// <summary>
        ///  Customer MailingAddressLine4
        /// </summary>
        public string MailingAddressLine4
        {
            get { return this.mailingAddressLine4; }
            set
            {
                #region CodeTillCR34
                //if (value == string.Empty)
                //{
                //    this.mailingAddressLine4 = "";
                //}
                //else
                //{
                //    if (value.Length > 80)
                //    {
                //        this.mailingAddressLine4 = value.Substring(0, 79);
                //    }
                //    else
                //        this.mailingAddressLine4 = value;
                //}
                #endregion

                #region CR34 Changes - Author: Jenny,Date:Mar 08 2013

                 iAddressLine4MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine4MinValue"].ToString().Trim());
                 iAddressLine4MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine4MaxValue"].ToString().Trim());
                 bIsAddressLine4Mandatory = false;
                 isAddressLine4Valid = false;

                #region FETCH THE CONFIG VALUE FOR AddressLine5
                //retrieve  MailingAddressLine4 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine4")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsAddressLine4Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                    {
                        iAddressLine4MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine4MinValue"].ToString().Trim()));
                        iAddressLine4MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine4MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        sAddressLine4Format1 = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the AddressLine4 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iAddressLine4MinValue) && (value.Length <= iAddressLine4MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sAddressLine4Format1.Trim(), false, false))
                            {
                                #region New Code
                                isAddressLine4Valid = true;
                                this.mailingAddressLine4 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isAddressLine4Valid = false;
                                if (bIsAddressLine4Mandatory)
                                    sValidationFails += "AddressLine4check,";
                                //capture the value even if validation fails
                                this.mailingAddressLine4 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isAddressLine4Valid = false;
                            //if mandatory
                            if (bIsAddressLine4Mandatory)
                                sValidationFails += "AddressLine4check,";
                            //capture the value even if validation fails
                            this.mailingAddressLine4 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        isAddressLine4Valid = false;
                        this.mailingAddressLine4 = value;
                        //If mandatory
                        if (bIsAddressLine4Mandatory)
                        {
                            //capture the value even if validation fails
                            sValidationFails += "AddressLine4check,";
                        }
                    }
                    //If any error, Add MailingAddressLine4  in Error.             
                    if (!isAddressLine4Valid)
                        sErrorCode += ConfigurationManager.AppSettings["AddressLine4InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    //Fetch AddressLine4 from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sAddressLine4Format1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found
                        this.mailingAddressLine4 = value;
                    }
                    else
                    {
                        this.mailingAddressLine4 = "";
                    }
                }
                #endregion
            }
        }
        /// <summary>
        ///  Customer MailingAddressLine6
        /// </summary>
        public string MailingAddressLine6
        {
            get { return this.mailingAddressLine6; }
            set
            {
                #region CodeTill CR34
                if (value == string.Empty)
                {
                    this.mailingAddressLine6 = "";
                }
                else
                {
                    if (value.Length > 80)
                    {
                        this.mailingAddressLine6 = value.Substring(0, 79);
                    }
                    else
                        this.mailingAddressLine6 = value;
                }
                #endregion

                #region CR34 Changes - Author: Jenny,Date:Mar 07 2013 - Use iff required

                // iAddressLine6MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine6MinValue"].ToString().Trim());
                // iAddressLine6MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine6MaxValue"].ToString().Trim());
                // bIsAddressLine6Mandatory = false;
                // isAddressLine6Valid = false;

                //#region FETCH THE CONFIG VALUE FOR AddressLine5
                ////retrieve  MailingAddressLine6 Config Details
                //foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                //{
                //    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine6")
                //    {
                //        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                //            bIsAddressLine6Mandatory = true;
                //    }

                //    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine6")
                //    {
                //        iAddressLine6MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine6MinValue"].ToString().Trim()));
                //        iAddressLine6MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine6MaxValue"].ToString().Trim()));
                //    }

                //    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                //    {
                //        sAddressLine6Format1 = dr["ConfigurationValue1"].ToString().Trim(); 
                //    }
                //}
                //#endregion

                //if (FormType.Equals("1"))
                //{
                //    #region Check the AddressLine6 Wrto Config Details
                //    //If value is not null
                //    if (!string.IsNullOrEmpty(value.Trim()))
                //    {
                //        //Proper max and min length
                //        if ((value.Length >= iAddressLine6MinValue) && (value.Length <= iAddressLine6MaxValue))
                //        {
                //            //check for the Format if exits
                //            //IF ANY FORMAT IS  MATCHING
                //            if (Helper.IsRegexMatch(value.Trim(), sAddressLine6Format1.Trim(), false, false))
                //            {
                //                #region New Code
                //                isAddressLine6Valid = true;
                //                this.mailingAddressLine6 = value;
                //                #endregion
                //            }
                //            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                //            //otherwise don't store
                //            else
                //            {
                //                isAddressLine6Valid = false;
                //                if (bIsAddressLine6Mandatory)
                //                    sValidationFails += "AddressLine6check,";
                //                //capture the value even if validation fails
                //                this.mailingAddressLine6 = value;
                //            }
                //        }
                //        //Not Proper max and min length
                //        else
                //        {
                //            isAddressLine6Valid = false;
                //            //if mandatory
                //            if (bIsAddressLine6Mandatory)
                //                sValidationFails += "AddressLine6check,";
                //            //capture the value even if validation fails
                //            this.mailingAddressLine6 = value;
                //        }
                //    }
                //    //if value is empty
                //    else
                //    {
                //        isAddressLine6Valid = false;
                //        this.mailingAddressLine6 = value;
                //        //If mandatory
                //        if (bIsAddressLine6Mandatory)
                //        {
                //            //capture the value even if validation fails
                //            sValidationFails += "AddressLine6check,";
                //        }
                //    }
                //    //If any error, Add MailingAddressLine6  in Error.             
                //    if (!isAddressLine6Valid)
                //        sErrorCode += ConfigurationManager.AppSettings["AddressLine6InError"].ToString() + ",";
                //    #endregion
                //}
                ////for formtype 2
                //else
                //{
                //    //Fetch AddressLine6 from config table and check for format
                //    if (Helper.IsRegexMatch(value.Trim(), sAddressLine6Format1.Trim(), false, false))
                //    {
                //        ///remove the code once regular expresssion is found
                //        this.mailingAddressLine6 = value;
                //    }
                //    else
                //    {
                //        this.mailingAddressLine6 = "";
                //    }
                //}
                #endregion
            }
        }
        /// <summary>
        ///  Customer MailingAddressLine5 (Province/ State)
        /// </summary>
        public string MailingAddressLine5
        {
            get { return this.mailingAddressLine5; }
            set
            {
                #region CodeTillCR34
                //String ZipRegex = @"^\d+$";

                //Regex regex = new Regex(ZipRegex);
                //if (!regex.IsMatch(value))
                //{
                //    this.mailingAddressLine5 = "-1";
                //}
                //else
                //{
                //    if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                //    {
                //        this.mailingAddressLine5 = "-1";
                //    }
                //    else
                //    {
                //        this.mailingAddressLine5 = value;
                //    }
                //}
                #endregion CodeTillCR34

                #region CR34 Changes - Author: Jenny,Date:Mar 06 2013

                 iAddressLine5MinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine5MinValue"].ToString().Trim());
                 iAddressLine5MaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine5MaxValue"].ToString().Trim());
                 bIsAddressLine5Mandatory = false;
                 isAddressLine5Valid = false;

                #region FETCH THE CONFIG VALUE FOR AddressLine5
                //retrieve  MailingAddressLine5 Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsAddressLine5Mandatory = true;
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                    {
                        iAddressLine5MinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine5MinValue"].ToString().Trim()));
                        iAddressLine5MaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["AddressLine5MaxValue"].ToString().Trim()));
                    }

                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        sAddressLine5Format1 = dr["ConfigurationValue1"].ToString().Trim();
                    }
                }
                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the AddressLine5 Wrto Config Details
                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iAddressLine5MinValue) && (value.Length <= iAddressLine5MaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sAddressLine5Format1.Trim(), false, false))
                            {
                                #region New Code
                                isAddressLine5Valid = true;
                                this.mailingAddressLine5 = value;
                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                isAddressLine5Valid = false;
                                if (bIsAddressLine5Mandatory)
                                    sValidationFails += "AddressLine5check,";
                                //capture the value even if validation fails
                                this.mailingAddressLine5 = value;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isAddressLine5Valid = false;
                            //if mandatory
                            if (bIsAddressLine5Mandatory)
                                sValidationFails += "AddressLine5check,";
                            //capture the value even if validation fails
                            this.mailingAddressLine5 = value;
                        }
                    }
                    //if value is empty
                    else
                    {
                        isAddressLine5Valid = false;
                        this.mailingAddressLine5 = value;
                        //If mandatory
                        if (bIsAddressLine5Mandatory)
                        {
                            //capture the value even if validation fails
                            sValidationFails += "AddressLine5check,";
                        }
                    }
                    //If any error, Add MailingAddressLine5  in Error.             
                    if (!isAddressLine5Valid)
                        sErrorCode += ConfigurationManager.AppSettings["AddressLine5InError"].ToString() + ",";
                    #endregion
                }
                //for formtype 2
                else
                {
                    //Fetch AddressLine5 from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sAddressLine5Format1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found
                        this.mailingAddressLine5 = value;
                    }
                    else
                    {
                        this.mailingAddressLine5 = "";
                    }
                }
                #endregion
            }
        }

        /// <summary>
        ///  Customer MailingAddressPostCode
        /// </summary>
        public string MailingAddressPostCode
        {
            get { return this.mailingAddressPostCode; }
            set
            {

                #region Temp

                //sErrorCode += ConfigurationManager.AppSettings["PostCode"].ToString() + ",";
                //this.mailingAddressPostCode = ""; 

                #endregion

                #region Old Code- For Reference


                //String ZipRegex = @"^\d+$";

                //Regex regex = new Regex(ZipRegex);

                //if (!regex.IsMatch(value))
                //{
                //    this.mailingAddressPostCode = "";
                //}
                //else
                //{
                //    if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                //    {
                //        this.mailingAddressPostCode = "";
                //    }
                //    else
                //    {
                //        this.mailingAddressPostCode = value;
                //    }
                //}

                #endregion

                #region Post Code

                //Author : Sabhareesan O.K                

                int iPostCodeMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PostCodeMinValue"].ToString().Trim());
                int iPostCodeMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PostCodeMaxValue"].ToString().Trim());
                bool bIsPostCodeMandatory = false;
              //  bool isPostValid = false;

                #region FETCH THE CONFIG VALUE FOR Post Code
                //retrieve the Post Code Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Phone Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsPostCodeMandatory = true;
                    }
                    //For mobile max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {

                        iPostCodeMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PostCodeMinValue"].ToString().Trim()));
                        iPostCodeMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PostCodeMaxValue"].ToString().Trim()));
                    }
                    //For MobilePhoneNumber Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {
                        sPostCodeFormat1 = dr["ConfigurationValue1"].ToString().Trim();
                        //sPostCodeFormat2 = dr["ConfigurationValue2"].ToString().Trim();
                    }
                }

                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the Post Code Wrto Config Details

                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iPostCodeMinValue) && (value.Length <= iPostCodeMaxValue))
                        {
                            //check for the Format if exits
                            //IF ANY FORMAT IS  MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sPostCodeFormat1.Trim(), false, false))
                            {

                                #region Old Code - Need to check for 1 to 99999 condition
                                ////remove the code once regular expresssion is found
                                //if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                                //{
                                //    isPostValid = false;
                                //    this.mailingAddressPostCode = "";
                                //}
                                //else
                                //{
                                //    isPostValid = true;
                                //    this.mailingAddressPostCode = value;
                                //} 

                                #endregion

                                #region New Code

                                isPostValid = true;
                                this.mailingAddressPostCode = value;

                                #endregion
                            }
                            //if format is not valid and if mandatory, then should not store into DB , log into customervalidationhistory
                            //otherwise don't store
                            else
                            {
                                if (bIsPostCodeMandatory)
                                    isPostValid = false;
                                    sValidationFails += "PostCheck,";
                                //capture the value even if validation fails
                                this.mailingAddressPostCode = value;
                            }

                        }
                        //Not Proper max and min length
                        else
                        {
                            isPostValid = false;
                            //if mandatory
                            if (bIsPostCodeMandatory)
                            {
                                sValidationFails += "PostCheck,";
                            }
                            //capture the value even if validation fails
                            this.mailingAddressPostCode = value;

                        }
                    }
                    //if value is empty
                    else
                    {
                        this.mailingAddressPostCode = value;

                        //If mandatory
                        if (bIsPostCodeMandatory)
                        {
                            isPostValid = false;
                            //capture the value even if validation fails
                            sValidationFails += "PostCheck,";
                        }

                    }

                    //If any error, Add PostCode in Error.             
                    if (!isPostValid)
                        sErrorCode += ConfigurationManager.AppSettings["PostCodeInError"].ToString() + ",";


                    #endregion
                }
                //for formtype 2
                else
                {
                    //fetch the postcode from config table and check for format
                    if (Helper.IsRegexMatch(value.Trim(), sPostCodeFormat1.Trim(), false, false))
                    {
                        ///remove the code once regular expresssion is found

                        #region Old Code

                        //if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                        //{
                        //    this.mailingAddressPostCode = "";
                        //}
                        //else
                        //{
                        //    this.mailingAddressPostCode = value;
                        //} 
                        #endregion

                        this.mailingAddressPostCode = value;


                    }
                    else
                    {
                        this.mailingAddressPostCode = "";
                    }

                }



                #endregion
            }
        }

        /// <summary>
        ///  Customer BusinessAddressLine1
        /// </summary>
        public string BusinessAddressLine1
        {
            get { return this.businessAddressLine1; }
            set
            {
                if (value == string.Empty)
                {
                    this.businessAddressLine1 = "";
                }
                else
                {
                    if (value.Length > 60)
                    {
                        this.businessAddressLine1 = value.Substring(0, 59);
                    }
                    else
                        this.businessAddressLine1 = value;
                }
            }
        }

        /// <summary>
        ///  Customer BusinessAddressLine2
        /// </summary>
        public string BusinessAddressLine2
        {
            get { return this.businessAddressLine2; }
            set
            {
                if (value == string.Empty)
                {
                    this.businessAddressLine2 = "";
                }
                else
                {
                    if (value.Length > 80)
                    {
                        this.businessAddressLine2 = value.Substring(0, 79);
                    }
                    else
                        this.businessAddressLine2 = value;
                }
            }
        }

        /// <summary>
        ///  Customer BusinessAddressLine3
        /// </summary>
        public string BusinessAddressLine3
        {
            get { return this.businessAddressLine3; }
            set
            {
                if (value == string.Empty)
                {
                    this.businessAddressLine3 = "";
                }
                else
                {
                    if (value.Length > 80)
                    {
                        this.businessAddressLine3 = value.Substring(0, 79);
                    }
                    else
                        this.businessAddressLine3 = value;
                }
            }
        }

        /// <summary>
        ///  Customer BusinessAddressLine4
        /// </summary>
        public string BusinessAddressLine4
        {
            get { return this.businessAddressLine4; }
            set
            {
                if (value == string.Empty)
                {
                    this.businessAddressLine4 = "";
                }
                else
                {
                    if (value.Length > 80)
                    {
                        this.businessAddressLine4 = value.Substring(0, 79);
                    }
                    else
                        this.businessAddressLine4 = value;
                }
            }
        }
        /// <summary>
        ///  Customer BusinessAddressLine5 (Province/ State)
        /// </summary>
        public string BusinessAddressLine5
        {
            get { return this.businessAddressLine5; }
            set
            {
                String ZipRegex = @"^\d+$";

                Regex regex = new Regex(ZipRegex);
                if (!regex.IsMatch(value))
                {
                    this.businessAddressLine5 = "-1";
                }
                else
                {
                    if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                    {
                        this.businessAddressLine5 = "-1";
                    }
                    else
                    {
                        this.businessAddressLine5 = value;
                    }
                }
            }
        }
        /// <summary>
        ///  Customer BusinessAddressLine6
        /// </summary>
        public string BusinessAddressLine6
        {
            get { return this.businessAddressLine6; }
            set
            {
                if (value == string.Empty)
                {
                    this.businessAddressLine6 = "";
                }
                else
                {
                    if (value.Length > 80)
                    {
                        this.businessAddressLine6 = value.Substring(0, 79);
                    }
                    else
                        this.businessAddressLine6 = value;
                }
            }
        }

        /// <summary>
        ///  Customer BusinessAddressPostCode
        /// </summary>
        public string BusinessAddressPostCode
        {
            get { return this.businessAddressPostCode; }
            set
            {
                String ZipRegex = @"^\d+$";

                Regex regex = new Regex(ZipRegex);

                if (!regex.IsMatch(value))
                {
                    this.businessAddressPostCode = "";
                }
                else
                {
                    if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 999999)
                    {
                        this.businessAddressPostCode = "";
                    }
                    else
                    {
                        this.businessAddressPostCode = value;
                    }
                }
            }
        }

        /// <summary>
        ///  Customer Longitude
        /// </summary>
        // public string Longitude { get { return this.longitude; } set { this.longitude = value; } }

        /// <summary>
        ///  Customer Latitude
        /// </summary>
        //  public string Latitude { get { return this.latitude; } set { this.latitude = value; } }

        /// <summary>
        ///  Latitude
        ///   //NGCV32 req.No:003
        /// </summary>
        public string Latitude
        {
            get { return this.latitude; }
            set
            {
                //String LatRegex = @"^[-+]?\d{0,3}(\.\d{0,6})?$";
                String LatRegex = @"[-]?[0-9]{3}\.[0-9]{6}";

                Regex regex = new Regex(LatRegex);

                if (!regex.IsMatch(value))
                {
                    this.latitude = null;
                }
                else
                    if (value == "")
                    {
                        this.latitude = null;
                    }
                    else
                    {
                        if (Convert.ToDouble(value) < -999.999999 || Convert.ToDouble(value) > 999.999999)
                        {
                            this.latitude = null;
                        }
                        else
                        {
                            this.latitude = value;
                        }
                    }
            }
        }
        /// <summary>
        ///  Longitude
        ///  //NGCV32 req.No:003
        /// </summary>
        public string Longitude
        {
            get { return this.longitude; }
            set
            {
                //String LonRegex = @"^[-+]?\d{0,3}(\.\d{0,6})?$";
                String LonRegex = @"[-]?[0-9]{3}\.[0-9]{6}";
                Regex regex = new Regex(LonRegex);

                if (!regex.IsMatch(value))
                {
                    this.longitude = null;
                }
                else
                    if (value == "")
                    {
                        this.longitude = null;
                    }
                    else
                    {
                        if (Convert.ToDouble(value) < -999.999999 || Convert.ToDouble(value) > 999.999999)
                        {
                            this.longitude = null;
                        }
                        else
                        {
                            this.longitude = value;
                        }
                    }
            }
        }

        /// <summary>
        ///  Customer DaytimePhoneNumber
        /// </summary>
        public string DaytimePhoneNumber
        {
            get { return this.daytimePhoneNumber; }
            set
            {

                #region For DayTimePhone Number



                string sDayPhoneNoPrefix = "", sDayPhoneNoFormat1 = "";
                int iDayPhoneNoMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["DayPhoneNumberMinValue"].ToString().Trim());
                int iDayPhoneNoMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["DayPhoneNumberMaxValue"].ToString().Trim());
                bool bIsDayPhoneMandatory = false;
                bool isDayPhoneValid = false;

                #region FETCH THE CONFIG VALUE FOR DAY MOBILE PHONE NUMBER

                //retrieve the phone number Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Phone Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsDayPhoneMandatory = true;
                    }
                    //For mobile max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {

                        iDayPhoneNoMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["DayPhoneNumberMinValue"].ToString().Trim()));
                        iDayPhoneNoMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["DayPhoneNumberMaxValue"].ToString().Trim()));
                    }
                    //For MobilePhoneNumber Prefix 
                    else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        sDayPhoneNoPrefix = dr["ConfigurationValue1"].ToString().Trim();
                    }
                    //For MobilePhoneNumber Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        sDayPhoneNoFormat1 = dr["ConfigurationValue1"].ToString().Trim();

                    }

                }

                #endregion

                //Author : Sabhareesan O.K

                if (FormType.Equals("1"))
                {
                    #region Check the DAY Mobile Number Wrto Config Details

                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iDayPhoneNoMinValue) && (value.Length <= iDayPhoneNoMaxValue))
                        {
                            if (!string.IsNullOrEmpty(sDayPhoneNoPrefix))
                            {
                                //for mutiple prefix (05,8)
                                if (sDayPhoneNoPrefix.Contains(","))
                                {

                                    string[] mobPrefixes = sDayPhoneNoPrefix.Split(',');
                                    bool flgMobPrefix = false;
                                    for (int i = 0; i < mobPrefixes.Length; i++)
                                    {
                                        if (value.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                        {
                                            flgMobPrefix = true;
                                            break;
                                        }
                                    }

                                    if (!flgMobPrefix)
                                    {
                                        isDayPhoneValid = false;
                                        this.daytimePhoneNumber = null;
                                    }
                                    else
                                    {
                                        isDayPhoneValid = false;
                                    }

                                }
                                //if it is single prefix - 5/6
                                else if (value.Trim().Substring(0, sDayPhoneNoPrefix.Trim().Length) != sDayPhoneNoPrefix)
                                {
                                    isDayPhoneValid = false;
                                    this.daytimePhoneNumber = null;

                                }
                                else if (value.Trim().Substring(0, sDayPhoneNoPrefix.Trim().Length) == sDayPhoneNoPrefix)
                                {
                                    isDayPhoneValid = true;

                                }
                            }
                            //IF NO PREFIX IS PASSED
                            else
                            {
                                isDayPhoneValid = true;
                            }

                            bool bchkFormat = Helper.IsRegexMatch(value.Trim(), sDayPhoneNoFormat1.Trim(), false, false);
                            //check for the Format if exits
                            //IF valid Prefix and FORMAT IS MATCHING
                            if (isDayPhoneValid && bchkFormat)
                            {
                                isDayPhoneValid = true;
                                this.daytimePhoneNumber = value;
                            }
                            //valid Prefix but format not matching
                            else if (isDayPhoneValid && !bchkFormat)
                            {
                                isDayPhoneValid = false;
                                if (bIsDayPhoneMandatory)
                                    sValidationFails += "DayCheck,";
                                this.daytimePhoneNumber = null;

                            }
                            //if format is not valid and if mandatory, then should not store into DB
                            else if (!isDayPhoneValid)
                            {
                                isDayPhoneValid = false;
                                if (bIsDayPhoneMandatory)
                                    sValidationFails += "DayCheck,";
                                this.daytimePhoneNumber = null;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isDayPhoneValid = false;
                            //if format is not valid and if mandatory, then should not store into DB
                            if (bIsDayPhoneMandatory)
                                sValidationFails += "DayCheck,";
                            this.daytimePhoneNumber = null;
                        }
                    }
                    //if value is empty
                    else
                    {
                        //If NOT mandatory
                        if (!bIsDayPhoneMandatory)
                        {
                            isDayPhoneValid = true;
                            this.daytimePhoneNumber = null;
                        }
                        //FOR MANDATORY, SKIP THE CUSTOMER
                        else
                        {
                            isDayPhoneValid = false;
                            sValidationFails += "DayCheck,";
                        }
                    }



                    //If any error, Add DAY Mobile in Error value.

                    if (!isDayPhoneValid)
                        sErrorCode += ConfigurationManager.AppSettings["DayMobileError"].ToString() + ",";

                    #endregion

                }
                else
                {
                    if (Helper.IsRegexMatch(value.Trim(), sDayPhoneNoFormat1.Trim(), false, false))
                    {
                        this.daytimePhoneNumber = value;
                    }
                    else
                    {
                        this.daytimePhoneNumber = null;
                    }
                }
                #endregion

                #region Old Code

                //Regex regex = new Regex(@"(\(\d{3}\)|\d{3}-)?\d{8}");

                //if (!regex.IsMatch(value))
                //{
                //    this.daytimePhoneNumber = "";
                //}
                //else
                //{
                //this.daytimePhoneNumber = value;
                //} 
                #endregion
            }
        }

        /// <summary>
        ///  Customer EveningPhoneNumber
        /// </summary>
        public string EveningPhoneNumber
        {
            get { return this.eveningPhoneNumber; }
            set
            {

                #region For Evening Time Phone Number

                //Author : Sabhareesan O.K

                string sEvenPhoneNoPrefix = "", sEvenPhoneNoFormat1 = "";


                int iEvenPhoneNoMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["EvenPhoneNumberMinValue"].ToString().Trim());
                int iEvenPhoneNoMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["EvenPhoneNumberMaxValue"].ToString().Trim());

                bool bIsEvenPhoneMandatory = false;
                bool isEvenPhoneValid = false;

                #region FETCH THE CONFIG VALUE FOR Evening Time MOBILE PHONE NUMBER

                //retrieve the phone number Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Phone Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsEvenPhoneMandatory = true;
                    }
                    //For mobile max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {

                        iEvenPhoneNoMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["EvenPhoneNumberMinValue"].ToString().Trim()));
                        iEvenPhoneNoMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["EvenPhoneNumberMaxValue"].ToString().Trim()));
                    }
                    //For MobilePhoneNumber Prefix 
                    else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {
                        sEvenPhoneNoPrefix = dr["ConfigurationValue1"].ToString().Trim();
                    }
                    //For MobilePhoneNumber Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {
                        sEvenPhoneNoFormat1 = dr["ConfigurationValue1"].ToString().Trim();

                    }

                }

                #endregion

                if (FormType.Equals("1"))
                {

                    #region Check the Evening Time Mobile Number Wrto Config Details

                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iEvenPhoneNoMinValue) && (value.Length <= iEvenPhoneNoMaxValue))
                        {
                            if (!string.IsNullOrEmpty(sEvenPhoneNoPrefix))
                            {
                                //for mutiple prefix (05,8)
                                if (sEvenPhoneNoPrefix.Contains(","))
                                {

                                    string[] mobPrefixes = sEvenPhoneNoPrefix.Split(',');
                                    bool flgMobPrefix = false;
                                    for (int i = 0; i < mobPrefixes.Length; i++)
                                    {
                                        if (value.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                        {
                                            flgMobPrefix = true;
                                            break;
                                        }
                                    }

                                    if (!flgMobPrefix)
                                    {
                                        isEvenPhoneValid = false;
                                        this.eveningPhoneNumber = null;
                                    }
                                    else
                                    {
                                        isEvenPhoneValid = true;
                                    }

                                }
                                //if it is single prefix - 5/6
                                else if (value.Trim().Substring(0, sEvenPhoneNoPrefix.Trim().Length) != sEvenPhoneNoPrefix)
                                {
                                    isEvenPhoneValid = false;
                                    this.eveningPhoneNumber = null;

                                }
                                //if matches happen
                                else if (value.Trim().Substring(0, sEvenPhoneNoPrefix.Trim().Length) == sEvenPhoneNoPrefix)
                                {
                                    isEvenPhoneValid = true;

                                }
                            }
                            //IF NO PREFIX IS PASSED
                            else
                            {
                                isEvenPhoneValid = true;
                            }


                            bool bchkFormat = Helper.IsRegexMatch(value.Trim(), sEvenPhoneNoFormat1.Trim(), false, false);
                            //check for the Format if exits
                            //IF FORMAT IS MATCHING
                            if (isEvenPhoneValid && bchkFormat)
                            {
                                isEvenPhoneValid = true;
                                this.eveningPhoneNumber = value;
                            }
                            //valid Prefix but format not matching
                            else if (isEvenPhoneValid && !bchkFormat)
                            {
                                isEvenPhoneValid = false;
                                if (bIsEvenPhoneMandatory)
                                    sValidationFails += "EvenCheck,";
                                this.eveningPhoneNumber = null;

                            }
                            //if format is not valid and if mandatory, then should not store into DB
                            else if (!isEvenPhoneValid)
                            {
                                isEvenPhoneValid = false;
                                if (bIsEvenPhoneMandatory)
                                    sValidationFails += "EvenCheck,";
                                this.eveningPhoneNumber = null;
                            }


                        }
                        //Not Proper max and min length
                        else
                        {
                            isEvenPhoneValid = false;
                            //if format is not valid and if mandatory, then should not store into DB
                            if (bIsEvenPhoneMandatory)
                                sValidationFails += "EvenCheck,";
                            this.eveningPhoneNumber = null;
                        }
                    }
                    //if value is empty
                    else
                    {
                        //If NOT mandatory
                        if (!bIsEvenPhoneMandatory)
                        {
                            isEvenPhoneValid = true;
                            this.eveningPhoneNumber = null;
                        }
                        //FOR MANDATORY, SKIP THE CUSTOMER
                        else
                        {
                            isEvenPhoneValid = false;
                            sValidationFails += "EvenCheck,";
                        }
                    }

                    //If any error, Add Evening Time Mobile in Error value.
                    if (!isEvenPhoneValid)
                        sErrorCode += ConfigurationManager.AppSettings["EvenMobileError"].ToString() + ",";


                    #endregion

                }
                //for form type 2
                else
                {
                    if (Helper.IsRegexMatch(value.Trim(), sEvenPhoneNoFormat1.Trim(), false, false))
                    {
                        this.eveningPhoneNumber = value;
                    }
                    else
                    {
                        this.eveningPhoneNumber = null;
                    }
                }

                #endregion


                #region Old Code

                //Regex regex = new Regex(@"(\(\d{3}\)|\d{3}-)?\d{8}");

                //if (!regex.IsMatch(value))
                //{
                //    this.eveningPhoneNumber = "";
                //}
                //else
                //{
                //this.eveningPhoneNumber = value;
                //} 
                #endregion
            }
        }

        /// <summary>
        ///  Customer MobilePhoneNumber
        /// </summary>
        public string MobilePhoneNumber
        {
            get { return this.mobilePhoneNumber; }
            set
            {
                #region Temp

                ////Regex regex = new Regex(@"(\(\d{3}\)|\d{3}-)?\d{8}");

                ////if (!regex.IsMatch(value))
                ////{
                ////    this.mobilePhoneNumber = "";
                ////}
                ////else
                ////{

                //only enable this
                //this.mobilePhoneNumber = value;

                ////}
                #endregion

                #region For Mobile Number


                string sPhoneNoPrefix = "", sPhoneNoFormat1 = "";

                int iPhoneNoMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PhoneNumberMinValue"].ToString().Trim());
                int iPhoneNoMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PhoneNumberMaxValue"].ToString().Trim());


                bool bIsPhoneMandatory = false;
                bool isValid = false;

                #region FETCH THE CONFIG VALUE FOR MOBILE PHONE NUMBER

                //retrieve the phone number Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Phone Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsPhoneMandatory = true;
                    }
                    //For mobile max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {

                        iPhoneNoMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PhoneNumberMinValue"].ToString().Trim()));
                        iPhoneNoMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PhoneNumberMaxValue"].ToString().Trim()));
                    }
                    //For MobilePhoneNumber Prefix 
                    else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        sPhoneNoPrefix = dr["ConfigurationValue1"].ToString().Trim();
                    }
                    //For MobilePhoneNumber Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        sPhoneNoFormat1 = dr["ConfigurationValue1"].ToString().Trim();
                    }

                }

                #endregion

                //Author : Sabhareesan O.K

                if (FormType.Equals("1"))
                {
                    #region Check the Mobile Number Wrto Config Details

                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iPhoneNoMinValue) && (value.Length <= iPhoneNoMaxValue))
                        {
                            #region Prefix

                            if (!string.IsNullOrEmpty(sPhoneNoPrefix))
                            {
                                //for mutiple prefix
                                if (sPhoneNoPrefix.Contains(","))
                                {

                                    string[] mobPrefixes = sPhoneNoPrefix.Split(',');
                                    bool flgMobPrefix = false;
                                    for (int i = 0; i < mobPrefixes.Length; i++)
                                    {
                                        if (value.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                        {
                                            flgMobPrefix = true;
                                            break;
                                        }
                                    }

                                    if (!flgMobPrefix)
                                    {
                                        isValid = false;
                                        this.mobilePhoneNumber = null;
                                    }
                                    else
                                    {
                                        isValid = true;
                                    }

                                }
                                //if it is single prefix and not matches
                                else if (value.Trim().Substring(0, sPhoneNoPrefix.Trim().Length) != sPhoneNoPrefix)
                                {
                                    isValid = false;
                                    this.mobilePhoneNumber = null;

                                }
                                //if matches
                                else if (value.Trim().Substring(0, sPhoneNoPrefix.Trim().Length) == sPhoneNoPrefix)
                                {
                                    isValid = true;
                                }
                            }
                            //IF NO PREFIX IS PASSED
                            else
                            {
                                isValid = true;
                            }
                            #endregion

                            //check for the Format if exits
                            //IF valid prfix and FORMAT MATCHING
                            bool bchkFormat = Helper.IsRegexMatch(value.Trim(), sPhoneNoFormat1.Trim(), false, false);
                            if (isValid && bchkFormat)
                            {
                                CustomerMobilePhoneStatus = ConfigurationManager.AppSettings["Deliverable"].ToString();
                                isValid = true;
                                this.mobilePhoneNumber = value;
                            }
                            //valid Prefix but format not matching
                            else if (isValid && !bchkFormat)
                            {
                                isValid = false;
                                if (bIsPhoneMandatory)
                                {
                                    this.mobilePhoneNumber = value;
                                    sValidationFails += "TeleCheck,";
                                }
                                else
                                {
                                    this.mobilePhoneNumber = null;
                                    CustomerMobilePhoneStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                                    //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                                }


                            }
                            //if format is not valid and if mandatory, then should not store into DB
                            else if (!isValid)
                            {
                                isValid = false;
                                if (bIsPhoneMandatory)
                                {
                                    this.mobilePhoneNumber = value;
                                    sValidationFails += "TeleCheck,";
                                }
                                else
                                {
                                    this.mobilePhoneNumber = null;
                                    CustomerMobilePhoneStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                                    //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                                }
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isValid = false;
                            //if format is not valid and if mandatory, then should not store into DB
                            if (bIsPhoneMandatory)
                            {
                                this.mobilePhoneNumber = value;
                                sValidationFails += "TeleCheck,";
                            }
                            else
                            {
                                this.mobilePhoneNumber = null;
                                CustomerMobilePhoneStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                                //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                            }
                        }
                    }
                    //if value is empty
                    else
                    {
                        //If NOT mandatory
                        if (!bIsPhoneMandatory)
                        {
                            isValid = true;
                            CustomerMobilePhoneStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                            //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                            this.mobilePhoneNumber = null;
                        }
                        //FOR MANDATORY, SKIP THE CUSTOMER
                        else
                        {
                            isValid = false;
                            sValidationFails += "TeleCheck,";
                            this.mobilePhoneNumber = null;

                        }
                    }

                    //If any error, Add Mobile in Error value.
                    if (!isValid)
                        sErrorCode += ConfigurationManager.AppSettings["MobileError"].ToString() + ",";

                    #endregion

                }
                //for formtype2
                else
                {
                    if (Helper.IsRegexMatch(value.Trim(), sPhoneNoFormat1.Trim(), false, false))
                    {
                        this.mobilePhoneNumber = value;
                    }
                    else
                    {
                        this.mobilePhoneNumber = null;
                    }
                }

                #endregion

            }
        }
        /// <summary>
        ///  Customer FaxNumber
        /// </summary>
        public string FaxNumber
        {
            get { return this.faxNumber; }
            set
            {
                //Regex regex = new Regex(@"(\(\d{3}\)|\d{3}-)?\d{8}");

                //if (!regex.IsMatch(value))
                //{
                //    this.faxNumber = "";
                //}
                //else
                //{
                this.faxNumber = value;
                //}
            }
        }
        /// <summary>
        ///  Customer EmailAddress
        /// </summary>
        public string EmailAddress
        {
            get { return this.emailAddress; }
            set
            {
                #region Old Code

                //Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                //if (!regex.IsMatch(value))
                //{
                //    this.emailAddress = "";
                //}
                //else
                //{
                //    this.emailAddress = value;
                //} 

                #endregion

                #region For Email Address

                //Author : Sabhareesan O.K


                string sEmailIdFormat1 = "";

                int iEmailIdMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["EmailIdMinValue"].ToString().Trim());
                int iEmailIdMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["EmailIdMaxValue"].ToString().Trim());


                bool bIsEmailIdMandatory = false;
                bool isValid = false;

                #region FETCH THE CONFIG VALUE FOR Email Address

                //retrieve the Email Id Config Details
                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    //For Email Id Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsEmailIdMandatory = true;
                    }
                    //For Email Id max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {

                        iEmailIdMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["EmailIdMinValue"].ToString().Trim()));
                        iEmailIdMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["EmailIdMaxValue"].ToString().Trim()));
                    }

                    //For Email Id Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {
                        sEmailIdFormat1 = dr["ConfigurationValue1"].ToString().Trim();
                    }

                }

                #endregion

                if (FormType.Equals("1"))
                {
                    #region Check the Email Address Wrto Config Details

                    //If value is not null
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //Proper max and min length
                        if ((value.Length >= iEmailIdMinValue) && (value.Length <= iEmailIdMaxValue))
                        {
                            //check for the Format if exits
                            //IF  FORMAT MATCHING
                            if (Helper.IsRegexMatch(value.Trim(), sEmailIdFormat1.Trim(), false, false))
                            {
                                isValid = true;
                                CustomerEMailStatus = ConfigurationManager.AppSettings["Deliverable"].ToString();
                                this.emailAddress = value;
                            }
                            //If mandatory
                            else if (bIsEmailIdMandatory)
                            {
                                isValid = false;
                                sValidationFails += "EmailCheck,";
                                this.emailAddress = value;
                            }
                            //if not mandatory
                            else if (!bIsEmailIdMandatory)
                            {
                                isValid = false;
                                CustomerEMailStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                                //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                                this.emailAddress = null;
                            }
                        }
                        //Not Proper max and min length
                        else
                        {
                            isValid = false;
                            if (bIsEmailIdMandatory)
                            {
                                this.emailAddress = value;
                                sValidationFails += "EmailCheck,";
                            }
                            else
                            {
                                this.emailAddress = null;
                                CustomerEMailStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                                //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();
                            }

                        }
                    }
                    //if value is empty
                    else
                    {
                        this.emailAddress = null;

                        //If NOT mandatory
                        if (!bIsEmailIdMandatory)
                        {
                            isValid = true;
                            CustomerEMailStatus = ConfigurationManager.AppSettings["Missing"].ToString();
                            //CustomerMailStatus = ConfigurationManager.AppSettings["In Error"].ToString();                           
                        }
                        //FOR MANDATORY, SKIP THE CUSTOMER
                        else
                        {
                            isValid = false;
                            sValidationFails += "EmailCheck,";

                        }
                    }

                    //If any error, Add Email Id in Error value.               
                    if (!isValid)
                        sErrorCode += ConfigurationManager.AppSettings["EmailIdInError"].ToString() + ",";


                    #endregion

                }
                else
                {
                    if (Helper.IsRegexMatch(value.Trim(), sEmailIdFormat1.Trim(), false, false))
                    {
                        this.emailAddress = value;
                    }
                    else
                    {
                        this.emailAddress = null;
                    }
                }

                #endregion

            }
        }

        /// <summary>
        ///  Customer ISOLanguageCode
        /// </summary>
        public string ISOLanguageCode
        {
            get { return this.iSOLanguageCode; }
            set
            {

                #region New Code
                //Author : Sabhareesan O.K

                if (FormType.Equals("1"))
                {
                    bool bIsLanguageMandatory = false;
                    bool isLanguageValid = false;

                    #region FETCH THE CONFIG VALUE FOR ISOLanguage
                    //retrieve the Language Config Details
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        //For Language Mandantory
                        if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Language")
                        {
                            if (dr["ConfigurationValue1"].ToString().Equals("1"))
                                bIsLanguageMandatory = true;
                        }

                    }

                    #endregion

                    #region Set the Value

                    this.iSOLanguageCode = null;

                    if (!string.IsNullOrEmpty(value.Trim()))
                    {

                        //check the input value with master/lookup 
                        if (dsConfigDetails.Tables.Count > 0)
                        {
                            if (dsConfigDetails.Tables[2].Rows.Count > 0)
                            {

                                //check the value from datatable
                                DataView dvLanguage = new DataView();
                                dvLanguage = dsConfigDetails.Tables[2].DefaultView;

                                dvLanguage.RowFilter = "ISOLanguageCode = '" + value + "'";

                                //if ISOLanguageCode id  found
                                //Set the ISOLanguageCode id value and isLanguageValid flag
                                if (dvLanguage.Count > 0)
                                {
                                    this.iSOLanguageCode = value;
                                    isLanguageValid = true;
                                }
                                //if language not found, then pass  empty value
                                else
                                {
                                    //if non - mandatory
                                    if (!bIsLanguageMandatory)
                                        isLanguageValid = true;
                                    else
                                    {
                                        sValidationFails += "LangCheck,";
                                        this.iSOLanguageCode = value;
                                        isLanguageValid = false;
                                    }
                                }

                            }
                            else
                            {
                                //if non - mandatory
                                if (!bIsLanguageMandatory)
                                    isLanguageValid = true;
                                else
                                {
                                    sValidationFails += "LangCheck,";
                                    this.iSOLanguageCode = value;
                                    isLanguageValid = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!bIsLanguageMandatory)
                        {
                            isLanguageValid = true;
                        }
                        //if mandatory
                        else
                        {
                            sValidationFails += "LangCheck,";
                            this.iSOLanguageCode = value;
                            isLanguageValid = false;
                        }
                    }


                    if (!isLanguageValid)
                        sErrorCode += ConfigurationManager.AppSettings["LangInError"].ToString() + ",";

                    #endregion

                }
                //for other form type (i.e 2)
                else
                {
                    this.iSOLanguageCode = value;
                }

                #endregion

                #region Old Code
                //this.iSOLanguageCode = value; 
                #endregion
            }
        }

        /// <summary>
        ///  FamilyMember1Age
        /// </summary>
        public string FamilyMember1Age
        {
            get { return this.familyMember1Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember1Age = "";
                }
                else
                {
                    this.familyMember1Age = value;
                }*/

                this.familyMember1Age = ArragneFamilyMemberAge(value);

            }
        }

        /// <summary>
        ///  FamilyMember2Age
        /// </summary>
        public string FamilyMember2Age
        {
            get { return this.familyMember2Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember2Age = "";
                }
                else
                {
                    this.familyMember2Age = value;
                }*/
                this.familyMember2Age = ArragneFamilyMemberAge(value);
            }
        }

        /// <summary>
        ///  FamilyMember3Age
        /// </summary>
        public string FamilyMember3Age
        {
            get { return this.familyMember3Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember3Age = "";
                }
                else
                {
                    this.familyMember3Age = value;
                }*/

                this.familyMember3Age = ArragneFamilyMemberAge(value);
            }
        }

        /// <summary>
        ///  FamilyMember4Age
        /// </summary>
        public string FamilyMember4Age
        {
            get { return this.familyMember4Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember4Age = "";
                }
                else
                {
                    this.familyMember4Age = value;
                }*/
                this.familyMember4Age = ArragneFamilyMemberAge(value);
            }
        }

        /// <summary>
        ///  FamilyMember5Age
        /// </summary>
        public string FamilyMember5Age
        {
            get { return this.familyMember5Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember5Age = "";
                }
                else
                {
                    this.familyMember5Age = value;
                }*/

                this.familyMember5Age = ArragneFamilyMemberAge(value);
            }
        }

        /// <summary>
        ///  FamilyMember6Age
        /// </summary>
        public string FamilyMember6Age
        {
            get { return this.familyMember6Age; }
            set
            {
                /*Regex regex = new Regex("[0-9]");

                if (!regex.IsMatch(value))
                {
                    this.familyMember6Age = "";
                }
                else
                {
                    this.familyMember6Age = value;
                }*/
                this.familyMember6Age = ArragneFamilyMemberAge(value);
            }
        }

        /// <summary>
        ///  Customer IncomeBandID
        /// </summary>
        public string IncomeBandID
        {
            get { return this.incomeBandID; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.incomeBandID = "0";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) < 0 || Convert.ToInt32(value) > 99)
                        {
                            this.incomeBandID = "0";
                        }
                        else
                        {
                            this.incomeBandID = value;
                        }
                    }
                    else
                    {
                        this.incomeBandID = "0";
                    }
                }
            }
        }

        /// <summary>
        ///  AllowPromotionsViaMail
        /// </summary>
        public string AllowPromotionsViaMail
        {
            get { return this.allowPromotionsViaMail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.allowPromotionsViaMail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.allowPromotionsViaMail = Constants.PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_MAIL;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */

                                this.allowPromotionsViaMail = "0";
                            }
                        }
                        else
                        {
                            this.allowPromotionsViaMail = null;
                        }
                    }
                    //if empty, make the fields as null
                    else
                    {
                        this.allowPromotionsViaMail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  AllowPromotionsViaPhone
        /// </summary>
        public string AllowPromotionsViaPhone
        {
            get { return this.allowPromotionsViaPhone; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.allowPromotionsViaPhone = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.allowPromotionsViaPhone = Constants.PREFERENCE_ID_ALLOW_PROMOTIONS_VIA_PHONE;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.allowPromotionsViaPhone = "0";
                            }
                        }
                        else
                        {
                            this.allowPromotionsViaPhone = null;
                        }
                    }
                    else
                    {
                        this.allowPromotionsViaPhone = null;
                    }
                }
            }
        }

        /// <summary>
        ///  AllowGroupPromotions
        /// </summary>
        public string AllowGroupPromotions
        {
            get { return this.allowGroupPromotions; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.allowGroupPromotions = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.allowGroupPromotions = Constants.PREFERENCE_ID_ALLOW_GROUP_PROMOTIONS;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.allowGroupPromotions = "0";
                            }
                        }
                        else
                        {
                            this.allowGroupPromotions = null;
                        }
                    }
                    else
                    {
                        this.allowGroupPromotions = null;
                    }
                }
            }
        }

        /// <summary>
        ///  AllowThirdPartyPromotions
        /// </summary>
        public string AllowThirdPartyPromotions
        {
            get { return this.allowThirdPartyPromotions; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.allowThirdPartyPromotions = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.allowThirdPartyPromotions = Constants.PREFERENCE_ID_ALLOW_THIRD_PARTY_PROMOTIONS;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.allowThirdPartyPromotions = "0";
                            }
                        }
                        else
                        {
                            this.allowThirdPartyPromotions = null;
                        }
                    }
                    else
                    {
                        this.allowThirdPartyPromotions = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Diabetic
        /// </summary>
        public string AddressInErrorValue
        {
            get { return this.addressInErrorValue; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.addressInErrorValue = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            this.addressInErrorValue = value;
                        }
                        else
                        {
                            this.addressInErrorValue = null;
                        }
                    }
                    else
                    {
                        this.addressInErrorValue = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Diabetic
        /// </summary>
        public string Diabetic
        {
            get { return this.diabetic; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.diabetic = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.diabetic = Constants.PREFERENCE_ID_DIABETIC;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.diabetic = "0";
                            }
                        }
                        else
                        {
                            this.diabetic = null;
                        }
                    }
                    else
                    {
                        this.diabetic = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Vegetarian
        /// </summary>
        public string Vegetarian
        {
            get { return this.vegetarian; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.vegetarian = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.vegetarian = Constants.PREFERENCE_ID_VEGETARIAN;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.vegetarian = "0";
                            }
                        }
                        else
                        {
                            this.vegetarian = null;
                        }
                    }
                    else
                    {
                        this.vegetarian = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Teetotal
        /// </summary>
        public string Teetotal
        {
            get { return this.teetotal; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.teetotal = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.teetotal = Constants.PREFERENCE_ID_TEETOTAL;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */
                                this.teetotal = "0";
                            }
                        }
                        else
                        {
                            this.teetotal = null;
                        }
                    }
                    else
                    {
                        this.teetotal = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Kosher
        /// </summary>
        public string Kosher
        {
            get { return this.kosher; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.kosher = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.kosher = Constants.PREFERENCE_ID_KOSHER;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                  Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                  Date : 08-02-2012
                                */
                                this.kosher = "0";
                            }
                        }
                        else
                        {
                            this.kosher = null;
                        }
                    }
                    else
                    {
                        this.kosher = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Halal
        /// </summary>
        public string Halal
        {
            get { return this.halal; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.halal = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.halal = Constants.PREFERENCE_ID_HALAL;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */
                                this.halal = "0";
                            }
                        }
                        else
                        {
                            this.halal = null;
                        }
                    }
                    else
                    {
                        this.halal = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Celiac
        /// </summary>
        public string Celiac
        {
            get { return this.celiac; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.celiac = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.celiac = Constants.PREFERENCE_ID_CELIAC;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */
                                this.celiac = "0";
                            }
                        }
                        else
                        {
                            this.celiac = null;
                        }
                    }
                    else
                    {
                        this.celiac = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Lactose
        /// </summary>
        public string Lactose
        {
            get { return this.lactose; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.lactose = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.lactose = Constants.PREFERENCE_ID_LACTOSE;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */

                                this.lactose = "0";
                            }
                        }
                        else
                        {
                            this.lactose = null;
                        }
                    }
                    else
                    {
                        this.lactose = null;
                    }
                }
            }
        }


        //Optional Dietary preferences. Added for V3.1.1 by Netra for requirement id 033b
        /// <summary>
        ///  Optional1
        /// </summary>
        public string Optional1
        {
            get { return this.optional1; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.optional1 = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.optional1 = Constants.PREFERENCE_ID_OPTIONAL1;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */
                                this.optional1 = "0";
                            }
                        }
                        else
                        {
                            this.optional1 = null;
                        }
                    }
                    else
                    {
                        this.optional1 = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Optional2
        /// </summary>
        public string Optional2
        {
            get { return this.optional2; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.optional2 = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.optional2 = Constants.PREFERENCE_ID_OPTIONAL2;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */
                                this.optional2 = "0";
                            }
                        }
                        else
                        {
                            this.optional2 = null;
                        }
                    }
                    else
                    {
                        this.optional2 = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Optional3
        /// </summary>
        public string Optional3
        {
            get { return this.optional3; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.optional3 = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.optional3 = Constants.PREFERENCE_ID_OPTIONAL3;
                            }
                            else
                            {
                                /*Author:Sabhareesan O.K
                                 Reason:Changed value from null to "0" To delete the entries from the customerpreference table during update(like action:2, etc..)
                                 Date : 08-02-2012
                               */

                                this.optional3 = "0";
                            }
                        }
                        else
                        {
                            this.optional3 = null;
                        }
                    }
                    else
                    {
                        this.optional3 = null;
                    }
                }
            }
        }
        //Change Complete.


        //Data Protection Preferences--V3.1 [Req ID:007]


        /// <summary>
        ///  TescoGroupMail
        /// </summary>
        public string TescoGroupMail
        {
            get { return this.tescoGroupMail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.tescoGroupMail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.tescoGroupMail = Constants.PREFERENCE_ID_TESCOGROUPMAIL;
                            }
                            else
                            {
                                this.tescoGroupMail = "0";
                            }
                        }
                        else
                        {
                            this.tescoGroupMail = null;
                        }
                    }
                    else
                    {
                        this.tescoGroupMail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  TescoGroupEMail
        /// </summary>
        public string TescoGroupEmail
        {
            get { return this.tescoGroupEmail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.tescoGroupEmail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.tescoGroupEmail = Constants.PREFERENCE_ID_TESCOGROUPEMAIL;
                            }
                            else
                            {
                                this.tescoGroupEmail = "0";
                            }
                        }
                        else
                        {
                            this.tescoGroupEmail = null;
                        }
                    }
                    else
                    {
                        this.tescoGroupEmail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  TescoGroupPhone
        /// </summary>
        public string TescoGroupPhone
        {
            get { return this.tescoGroupPhone; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.tescoGroupPhone = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.tescoGroupPhone = Constants.PREFERENCE_ID_TESCOGROUPPHONE;
                            }
                            else
                            {
                                this.tescoGroupPhone = "0";
                            }
                        }
                        else
                        {
                            this.tescoGroupPhone = null;
                        }
                    }
                    else
                    {
                        this.tescoGroupPhone = null;
                    }
                }
            }
        }

        /// <summary>
        ///  TescoGroupSms
        /// </summary>
        public string TescoGroupSms
        {
            get { return this.tescoGroupSms; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.tescoGroupSms = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.tescoGroupSms = Constants.PREFERENCE_ID_TESCOGROUPSMS;
                            }
                            else
                            {
                                this.tescoGroupSms = "0";
                            }
                        }
                        else
                        {
                            this.tescoGroupSms = null;
                        }
                    }
                    else
                    {
                        this.tescoGroupSms = null;
                    }
                }
            }
        }

        /// <summary>
        ///  PartnerMail
        /// </summary>
        public string PartnerMail
        {
            get { return this.partnerMail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.partnerMail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.partnerMail = Constants.PREFERENCE_ID_PARTNERMAIL;
                            }
                            else
                            {
                                this.partnerMail = "0";
                            }
                        }
                        else
                        {
                            this.partnerMail = null;
                        }
                    }
                    else
                    {
                        this.partnerMail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  PartnerEmail
        /// </summary>
        public string PartnerEmail
        {
            get { return this.partnerEmail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.partnerEmail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.partnerEmail = Constants.PREFERENCE_ID_PARTNEREMAIL;
                            }
                            else
                            {
                                this.partnerEmail = "0";
                            }
                        }
                        else
                        {
                            this.partnerEmail = null;
                        }
                    }
                    else
                    {
                        this.partnerEmail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  PartnerPhone
        /// </summary>
        public string PartnerPhone
        {
            get { return this.partnerPhone; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.partnerPhone = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.partnerPhone = Constants.PREFERENCE_ID_PARTNERPHONE;
                            }
                            else
                            {
                                this.partnerPhone = "0";
                            }
                        }
                        else
                        {
                            this.partnerPhone = null;
                        }
                    }
                    else
                    {
                        this.partnerPhone = null;
                    }
                }
            }
        }

        /// <summary>
        ///  PartnerSms
        /// </summary>
        public string PartnerSms
        {
            get { return this.partnerSms; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.partnerSms = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.partnerSms = Constants.PREFERENCE_ID_PARTNERSMS;
                            }
                            else
                            {
                                this.partnerSms = "0";
                            }
                        }
                        else
                        {
                            this.partnerSms = null;
                        }
                    }
                    else
                    {
                        this.partnerSms = null;
                    }
                }
            }
        }

        /// <summary>
        ///  ResearchMail
        /// </summary>
        public string ResearchMail
        {
            get { return this.researchMail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.researchMail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.researchMail = Constants.PREFERENCE_ID_RESEARCHMAIL;
                            }
                            else
                            {
                                this.researchMail = "0";
                            }
                        }
                        else
                        {
                            this.researchMail = null;
                        }
                    }
                    else
                    {
                        this.researchMail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  ResearchEmail
        /// </summary>
        public string ResearchEmail
        {
            get { return this.researchEmail; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.researchEmail = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.researchEmail = Constants.PREFERENCE_ID_RESEARCHEMAIL;
                            }
                            else
                            {
                                this.researchEmail = "0";
                            }
                        }
                        else
                        {
                            this.researchEmail = null;
                        }
                    }
                    else
                    {
                        this.researchEmail = null;
                    }
                }
            }
        }

        /// <summary>
        ///  ResearchPhone
        /// </summary>
        public string ResearchPhone
        {
            get { return this.researchPhone; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.researchPhone = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.researchPhone = Constants.PREFERENCE_ID_RESEARCHPHONE;
                            }
                            else
                            {
                                this.researchPhone = "0";
                            }
                        }
                        else
                        {
                            this.researchPhone = null;
                        }
                    }
                    else
                    {
                        this.researchPhone = null;
                    }
                }
            }
        }

        /// <summary>
        ///  ResearchSms
        /// </summary>
        public string ResearchSms
        {
            get { return this.researchSms; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.researchSms = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.researchSms = Constants.PREFERENCE_ID_RESEARCHSMS;
                            }
                            else
                            {
                                this.researchSms = "0";
                            }
                        }
                        else
                        {
                            this.researchSms = null;
                        }
                    }
                    else
                    {
                        this.researchSms = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Foreign
        /// </summary>
        public string Foreign
        {
            get { return this.foreign; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.foreign = "0";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                        {
                            this.foreign = "0";
                        }
                        else
                        {
                            this.foreign = value;
                        }
                    }
                    else
                    {
                        this.foreign = "0";
                    }
                }
            }
        }

        /// <summary>
        ///  Expat
        /// </summary>
        public string Expat
        {
            get { return this.expat; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.expat = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) == 0 || Convert.ToInt32(value) == 1)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                this.expat = Constants.PREFERENCE_ID_EXPAT;
                            }
                            else
                            {
                                this.expat = "0";
                            }
                        }
                        else
                        {
                            this.expat = null;
                        }
                    }
                    else
                    {
                        this.expat = null;
                    }
                }
            }
        }

        /// <summary>
        ///  PreviousLoyaltySchemeClubcardId
        /// </summary>
        public string PreviousLoyaltySchemeClubcardId
        {
            get { return this.previousLoyaltySchemeClubcardId; }
            set
            {
                if (Convert.ToBoolean(HasCorrectCheckDigit(Convert.ToString(value))))
                {
                    this.previousLoyaltySchemeClubcardId = value;
                }
                else
                {
                    this.previousLoyaltySchemeClubcardId = null;
                }
            }
        }

        /// <summary>
        ///  FormType
        /// </summary>
        public string FormType
        {
            get { return this.formType; }
            set
            { this.formType = value; }

        }

        /// <summary>
        ///  Customer CustomerMailStatus
        /// </summary>
        public string CustomerMailStatus
        {
            get { return this.customerMailStatus; }
            set
            {
                Regex regex = new Regex("^[0-9]*$");
                if (!regex.IsMatch(value))
                {
                    this.customerMailStatus = "-2";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 99999)
                        {
                            this.customerMailStatus = "-2";
                        }
                        else
                        {
                            this.customerMailStatus = value;
                        }
                    }
                    else
                    {
                        this.customerMailStatus = "-2";
                    }
                }
            }
        }

        /// <summary>
        ///  RaceID
        /// </summary>
        public string RaceID
        {
            get { return this.raceID; }
            set
            {
                #region Old Code

                ////Regex regex = new Regex("[0-9]");
                //Regex regex = new Regex("^[0-9]*$");

                //if (!regex.IsMatch(value))
                //{
                //    this.raceID = null; // Assignment value changed from "0" to null - by Sakthi
                //}
                //else
                //{
                //    //Check for empty string before converting
                //    if (!value.Equals(""))
                //    {
                //        this.raceID = value;
                //    }
                //    else
                //    {
                //        this.raceID = null;

                //    }
                //    /*if (Convert.ToInt32(value) != 0 || Convert.ToInt32(value) != 1)
                //    {
                //        this.raceID = "0";
                //    }
                //    else
                //    {
                //        this.raceID = value;
                //    }*/
                //} 
                #endregion

                #region New Code
                //Author : Sabhareesan O.K

                if (FormType.Equals("1"))
                {
                    bool bIsRaceMandatory = false;
                    bool isRaceValid = false;

                    #region FETCH THE CONFIG VALUE FOR Race
                    //retrieve the Race Config Details
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        //For Race Mandantory
                        if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "Race")
                        {
                            if (dr["ConfigurationValue1"].ToString().Equals("1"))
                                bIsRaceMandatory = true;
                        }
                    }

                    #endregion

                    #region Set the Value

                    this.raceID = null;

                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        //check the input value with master/lookup 
                        if (dsConfigDetails.Tables.Count > 0)
                        {
                            if (dsConfigDetails.Tables[1].Rows.Count > 0)
                            {
                                //check the value from datatable
                                DataView dvRace = new DataView();
                                dvRace = dsConfigDetails.Tables[1].DefaultView;

                                dvRace.RowFilter = "raceid = '" + value + "'";

                                //if race id  found
                                //Set the race id value and race flag
                                if (dvRace.Count > 0)
                                {
                                    this.raceID = value;
                                    isRaceValid = true;
                                }
                                //if invalid or no race id found
                                else
                                {
                                    //if not mandatory
                                    if (!bIsRaceMandatory)
                                        isRaceValid = true;
                                    else
                                    {
                                        sValidationFails += "RaceCheck,";
                                        this.raceID = value;
                                        isRaceValid = false;
                                    }

                                }
                            }
                            //if table data not exits
                            else
                            {
                                //if not mandatory
                                if (!bIsRaceMandatory)
                                    isRaceValid = true;
                                else
                                {
                                    sValidationFails += "RaceCheck,";
                                    this.raceID = value;
                                    isRaceValid = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!bIsRaceMandatory)
                        {
                            isRaceValid = true;
                        }
                        //if mandatory
                        else
                        {
                            sValidationFails += "RaceCheck,";
                            this.raceID = value;
                            isRaceValid = false;
                        }
                    }

                    if (!isRaceValid)
                        sErrorCode += ConfigurationManager.AppSettings["RaceInError"].ToString() + ",";

                    #endregion

                }
                //for other form type (i.e 2)
                else
                {
                    //Regex regex = new Regex("[0-9]");
                    Regex regex = new Regex("^[0-9]*$");

                    if (!regex.IsMatch(value))
                    {
                        this.raceID = null; // Assignment value changed from "0" to null - by Sakthi
                    }
                    else
                    {
                        //Check for empty string before converting
                        if (!value.Equals(""))
                        {
                            this.raceID = value;
                        }
                        else
                        {
                            this.raceID = null;

                        }
                    }
                }

                #endregion
            }
        }

        public string PreferredMailingAddress
        {
            get { return this.preferredMailingAddress; }
            set
            {
                Regex regex = new Regex("^[21-22]*$");

                if (!regex.IsMatch(value))
                {
                    this.preferredMailingAddress = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt32(value) < 21 || Convert.ToInt32(value) > 22)
                        {
                            this.preferredMailingAddress = null;
                        }
                        else
                        {
                            this.preferredMailingAddress = value;
                        }
                    }
                    else
                    {
                        this.preferredMailingAddress = null;
                    }
                }
            }
        }

        /// <summary>
        ///  InsertBy 
        /// </summary>
        public string InsertBy { get { return this.insertBy; } set { this.insertBy = value; } }

        /// <summary>
        ///  Customer BusinessCustomerInd
        /// </summary>
        public string BusinessCustomerInd { get { return this.businessCustomerInd; } set { this.businessCustomerInd = value; } }

        /// <summary>
        ///  Customer BusinessType
        /// </summary>
        public string BusinessType
        {
            get { return this.businessType; }
            set
            {
                if (value == "")
                    this.businessType = null;
                else
                    this.businessType = value;
            }
        }

        /// <summary>
        ///  Customer CustomerSegmentID
        /// </summary>
        public string CustomerSegmentID
        {
            get { return this.customerSegmentID; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.customerSegmentID = null;
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        this.customerSegmentID = value;
                    }
                    else
                    {
                        this.customerSegmentID = null;
                    }
                }
            }
        }

        /// <summary>
        ///  Customer PreferredStoreID
        /// </summary>
        public string PreferredStoreID
        {
            get { return this.preferredStoreID; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.preferredStoreID = "-1";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 99999)
                        {
                            this.preferredStoreID = "-1";
                        }
                        else
                        {
                            this.preferredStoreID = value;
                        }
                    }
                    else
                    {
                        this.preferredStoreID = "-1";
                    }
                }
            }
        }
        /// <summary>
        ///  Customer Created Date
        /// </summary>
        public string CustomerCreatedDate
        {
            get { return this.customerCreatedDate; }
            set
            {
                Regex regex = new Regex(Constants.DATE_FORMATE);

                if (!regex.IsMatch(value))
                {
                    this.customerCreatedDate = null;
                }
                else
                {
                    this.customerCreatedDate = value;
                }
            }
        }
        /// <summary>
        ///  Customer JoinedStoreID
        /// </summary>
        public string JoinedStoreID
        {
            get { return this.joinedStoreID; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.joinedStoreID = "-1";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 99999)
                        {
                            this.joinedStoreID = "";
                        }
                        else
                        {
                            this.joinedStoreID = value;
                        }
                    }
                    else
                    {
                        this.joinedStoreID = "-1";
                    }
                }
            }
        }

        /// <summary>
        ///  Customer InactiveCollectionPeriodQty
        /// </summary>
        public Int16 InactiveCollectionPeriodQty { get { return this.inactiveCollectionPeriodQty; } set { this.inactiveCollectionPeriodQty = value; } }

        /// <summary>
        ///  Customer CustomerUseStatusID
        /// </summary>
        public string CustomerUseStatusID
        {
            get { return this.customerUseStatusID; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    this.customerUseStatusID = "-2";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToDecimal(value) < 0 || Convert.ToDecimal(value) > 99999)
                        {
                            this.customerUseStatusID = "-2";
                        }
                        else
                        {
                            this.customerUseStatusID = value;
                        }
                    }
                    else
                    {
                        this.customerUseStatusID = "-2";
                    }
                }
            }
        }

        /// <summary>
        ///  Customer PreviousLoyaltySchemeInd
        /// </summary>
        public string PreviousLoyaltySchemeInd { get { return this.previousLoyaltySchemeInd; } set { this.previousLoyaltySchemeInd = value; } }

        /// <summary>
        ///  CustAlternateID
        /// </summary>
        public string CustAlternateID
        {
            get { return this.custAlternateID; }
            set
            {
                if (value == string.Empty || value.Trim().Length != 13)
                {
                    this.custAlternateID = "";
                }
                else
                {
                    this.custAlternateID = value.Trim();
                }
            }
        }

        /// <summary>
        ///  Customer Primary Clubcard ID
        /// </summary>
        public string PrimaryClubcardID
        {
            get { return this.primaryClubcardID; }
            set
            { this.primaryClubcardID = value; }

        }

        /// <summary>
        ///  Customer Clubcard ID
        /// </summary>
        public string ClubcardID
        {
            get { return this.clubcardID; }
            set
            { this.clubcardID = value; }

        }

        /// <summary>
        ///  Previous Offer Primary CustomerID
        /// </summary>
        public Int64 PreviousOfferPrimaryCustomerID
        {
            get { return this.previousOfferPrimaryCustomerID; }
            set
            { this.previousOfferPrimaryCustomerID = value; }

        }

        /// <summary>
        ///  PreferenceID
        /// </summary>
        public string PreferredContactTypeCode
        {
            get { return this.preferredContactTypeCode; }
            set
            {
                //Alow only 6,44,43,45

                //--Phone call from 10->6
                //--Mobile -> 11 to 44
                //--Email --> 12 to 43
                //--Post ->   13 to 45
                //--SMS ->   14 to 44
                //--Phone ->   15 to 6

                Regex regex = new Regex("^(43|44|45|6)+$");

                if (!regex.IsMatch(value))
                {
                    this.preferredContactTypeCode = "-1";
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if ((Convert.ToInt32(value) < 43 || Convert.ToInt32(value) > 45) && (Convert.ToInt32(value) != 6))
                        {
                            this.preferredContactTypeCode = "-1";
                        }
                        else
                        {
                            this.preferredContactTypeCode = value;
                        }
                    }
                    else
                    {
                        this.preferredContactTypeCode = "-1";
                    }
                }
            }
        }
        //Added to fetch the Age limit --V3.1.1[ReqID:5.3]
        string ageLimit;

        /// <summary>
        ///  GeocodeConfidence
        ///  //NGCV32 req.No:003
        /// </summary>
        public string GeocodeConfidence
        {
            get { return this.geocodeConfidence; }
            set
            {
                //Regex regex = new Regex("[0-9]");
                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(value))
                {
                    if (value == string.Empty)
                    {
                        this.geocodeConfidence = null;
                    }
                    else
                    {
                        this.geocodeConfidence = "-1";
                    }
                }
                else
                {
                    //Check for empty string before converting
                    if (!value.Equals(""))
                    {
                        if (Convert.ToInt16(value) < 0 || Convert.ToInt16(value) > 9)
                        {
                            this.geocodeConfidence = "-1";
                        }
                        else
                        {
                            this.geocodeConfidence = value;
                        }

                    }
                    else
                    {
                        this.geocodeConfidence = null;
                    }

                }
            }
        }

        /// <summary>
        /// Property For holding the Promotional code 
        /// Also it check for validation (Allow Alphanumeric)
        /// </summary>
        public string JoinPromotionCode
        {
            get { return this._JoinPromotionCode; }
            set
            {
                Regex regex = new Regex("^[a-zA-Z0-9]+$");

                if (!regex.IsMatch(value))
                {
                    this._JoinPromotionCode = null;
                }
                else
                {

                    this._JoinPromotionCode = ((value.Length > 10) ? null : value);

                }
            }
        }


        /// <summary>
        /// Property For holding the SSN NUmber
        /// </summary>
        public string SSNNumber
        {
            get { return this._SSNNumber; }
            set
            {

                #region SSN Number

                #region Check the SSN Number Wrto Config Details

                this._SSNNumber = null;

                //If value is not null
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    //Proper max and min length
                    if ((value.Trim().Length >= iSSNMinValue) && (value.Trim().Length <= iSSNMaxValue))
                    {
                        //check for the Format if exits
                        //IF ANY FORMAT IS MATCHING, THEN IT IS VALID FORMAT
                        if (Helper.IsRegexMatch(value.Trim(), sSSNFormat1.Trim(), false, false))
                        {
                            isSSNValid = true;
                            this._SSNNumber = value;
                        }
                        //if mandatory and also validation fails, should not store into DB and shld log into customervalidationhistory table
                        else if (bIsSSNMandatory || bIsPassportMandatory)
                        {
                            isSSNValid = false;
                            this._SSNNumber = value;
                            //sValidationFails += "SSNCheck,";


                        }
                        //if non-mandatory and  validation fails, store into DB and shld log into customervalidationhistory table
                        else if (!(bIsSSNMandatory || bIsPassportMandatory))
                        {
                            isSSNValid = false;

                        }
                    }
                    //Not Proper max and min length
                    else
                    {
                        isSSNValid = false;
                        if (bIsSSNMandatory)
                        {
                            this._SSNNumber = value;
                        }
                        //    sValidationFails += "SSNCheck,";


                    }
                }
                //if value is empty
                else
                {
                    //If not mandatory
                    if (!(bIsSSNMandatory || bIsPassportMandatory))
                    {
                        isSSNValid = true;
                    }
                    //FOR MANDATORY, SKIP/REJECT THE CUSTOMER
                    else
                    {
                        isSSNValid = false;
                        this._SSNNumber = value;
                        //sValidationFails += "SSNCheck,";

                    }
                }

                //If any error, Add SSN in Error.
                //if (!isSSNValid)
                //    sErrorCode += ConfigurationManager.AppSettings["SSNInError"].ToString() + ",";

                #endregion

                #endregion
            }
        }

        /// <summary>
        /// Property For holding the Passport NUmber
        /// </summary>
        public string PassportNumber
        {
            get { return this._PassportNumber; }
            set
            {

                #region Passport Number

                #region Check the Passport Number Wrto Config Details

                this._PassportNumber = null;

                //If value is not null
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    //Proper max and min length
                    if ((value.Length >= iPassportMinValue) && (value.Length <= iPassportMaxValue))
                    {
                        //check for the Format if exits
                        //IF ANY FORMAT IS MATCHING, THEN IT IS VALID FORMAT
                        if (Helper.IsRegexMatch(value.Trim(), sPassportFormat1.Trim(), false, false))
                        {
                            isPassportValid = true;
                            this._PassportNumber = value;
                        }
                        //if mandatory and also validation fails, should not store into DB and shld log into customervalidationhistory table
                        else if (bIsSSNMandatory || bIsPassportMandatory)
                        {
                            isPassportValid = false;
                            this._PassportNumber = value;
                            //sValidationFails += "PassportCheck,";

                        }
                        //if non-mandatory and  validation fails, store into DB and shld log into customervalidationhistory table
                        else if (!(bIsSSNMandatory || bIsPassportMandatory))
                        {
                            isPassportValid = false;

                        }
                    }
                    //Not Proper max and min length
                    else
                    {
                        isPassportValid = false;
                        if (bIsPassportMandatory)
                            this._PassportNumber = value;
                        //    sValidationFails += "PassportCheck,";                     
                    }
                }
                //if value is empty
                else
                {
                    //If not mandatory
                    if (!(bIsSSNMandatory || bIsPassportMandatory))
                    {
                        isPassportValid = true;
                    }
                    //FOR MANDATORY, SKIP THE CUSTOMER
                    else
                    {
                        isPassportValid = false;
                        this._PassportNumber = value;
                        //sValidationFails += "PassportCheck,";
                    }
                }
                ////If any error, Add Passport in Error.
                //if (!isPassportValid)
                //    sErrorCode += ConfigurationManager.AppSettings["PassportInError"].ToString() + ",";


                #endregion

                #endregion
            }
        }


        #endregion

        #region Methods

        #region Batch
        /// <summary>
        /// Execute a Batch Script
        /// </summary>
        /// <param name="sXml">XML data in string format(It contains customer's details to insert into database)</param>
        /// <param name="sTag">Node name of the XML. Based on this Node, it creates Hashtable</param>
        /// <returns>True or False</returns>
        public bool Batch(string sXml, string sTag)//, out string resultXml)
        //private bool Batch(Trace trace, string scriptName, string userName, string password, string[] args, Output standardOutput, Output errorOutput, out string resultXml)
        {
            string sFileName = "";
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ImportCustomer.Batch");
            Result executeResult = new Result();
            Int32 nTotalRecordsRead = 0, nRecordsRejected = 0, nRecordsInserted = 0, nRecordsUpdatedWITHOUTNewCardAccount = 0;
            Int32 nRecordsUpdatedWITHNewCardAccount = 0, nRecordsMerged = 0;
            Int16 nActionCode = 0;
            string sRejectReason = "";
            StringBuilder sb = new StringBuilder("");
            XmlWriter writer = XmlWriter.Create(sb);
            XmlDocument doc = new XmlDocument();
            object[] arr = { 0 };

            try
            {
                /*// Get the User Culture.
                string culture = ConfigurationSettings.AppSettings["UserCultureDefault"];
                // Check that this isn't empty.
                if (StringUtils.IsStringEmpty(culture))
                {
                    // It is empty, so use the Default Culture.
                    culture = ConfigurationSettings.AppSettings["CultureDefault"];
                }*/



                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_CUSTOMER, "Batch Register Customers Started.", false);
                sFileName = CommonFunctions.CreateLogFile(Constants.ACTION_IMPORT_CUSTOMER);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.ACTION_IMPORT_CUSTOMER);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
                CommonFunctions.MessageWriteToLogFile(sFileName, "Batch Register Customers Started.");

                Hashtable htImportCust;
                //Hashtable htOutPut = new Hashtable();
                arr = ConvertXmlHash.XMLToArrayOfHashTable(sXml, sTag);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sXml);

                if (arr.Length > 0)
                {
                    //Start Building XML file where we need to write the rejected customer details.    
                    sb = new StringBuilder();
                    writer = XmlWriter.Create(sb);
                    writer.WriteStartElement("reject_customers");
                }

                string connectionString = "";


                #region TO LOAD THE CONFIG DETAILS INTO DATATABLE

                //Updated by Sabhareesan O.K - NGC 3.6
                //FOR VALIDATING THE TELPEPHONE NUMBER,SSN NUMBER AND PASSPORT NUMBER

                /**  Configuration Type and Description
                 * 
                 * 2	Mandatory fields
                 * 4	Error message
                 * 5	Length of the input fields
                 * 6	Is SSN field Check required
                 * 8	Profanity check fields
                 * 9	Prefix
                 * 10	Format
                 * 11	Join Route Code
                 * 
                 * */


                string conditionXML = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;

                XmlDocument resulDoc = null;
                string resultxml = string.Empty;
                int rowCount = 0;
                string pCulture = "";

                //2,4,5,6,8,9,10,11
                conditionXML = "2,4,5,6,8,9,10,11";

                custClient = new CustomerServiceClient();

                if (custClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, pCulture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsConfigDetails = new DataSet();

                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                }


                #endregion


                #region Fetch the Config Details from Datatable to Respective local variables


                #region Min & Max Length

                //For SSN
                iSSNMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["SSNMinValue"].ToString().Trim());
                iSSNMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["SSNMaxValue"].ToString().Trim());

                //For Passport
                iPassportMinValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PassportMinValue"].ToString().Trim());
                iPassportMaxValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PassportMaxValue"].ToString().Trim());


                #endregion

                #region FETCH THE CONFIG VALUE For Required values


                foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                {
                    #region SSN Config

                    //For SSN Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "PrimaryId")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsSSNMandatory = true;
                    }
                    //For SSN max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "PrimId")
                    {

                        iSSNMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["SSNMinValue"].ToString().Trim()));
                        iSSNMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["SSNMaxValue"].ToString().Trim()));
                    }
                    //For SSN Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "PrimaryId")
                    {
                        sSSNFormat1 = dr["ConfigurationValue1"].ToString().Trim();

                    }

                    #endregion

                    #region Passport Config

                    //For Passport Number Mandantory
                    if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "SecondaryId")
                    {
                        if (dr["ConfigurationValue1"].ToString().Equals("1"))
                            bIsPassportMandatory = true;
                    }
                    //For Passport Number max and min setting
                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "SecId")
                    {

                        iPassportMinValue = (!string.IsNullOrEmpty(dr["ConfigurationValue1"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue1"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PassportMinValue"].ToString().Trim()));
                        iPassportMaxValue = (!string.IsNullOrEmpty(dr["ConfigurationValue2"].ToString().Trim()) ? Convert.ToInt32(dr["ConfigurationValue2"].ToString().Trim()) : Convert.ToInt32(ConfigurationSettings.AppSettings["PassportMaxValue"].ToString().Trim()));
                    }
                    //For Passport Number Format 
                    else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "SecondaryId")
                    {
                        sPassportFormat1 = dr["ConfigurationValue1"].ToString().Trim();

                    }

                    #endregion

                }

                #endregion


                #endregion

                for (int i = 0; i < arr.Length; i++)
                {
                    if (i == 0)
                    {
                        nTotalRecordsRead = arr.Length;
                        connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                    }
                    htImportCust = (Hashtable)arr[i];
                    nActionCode = 0;
                    sRejectReason = "";
                    sErrorCode = "";

                    //Call update DB funstion
                    UpdateDataBase(htImportCust, connectionString, sFileName, ref sRejectReason, ref nActionCode);

                    if (sRejectReason == "" && nActionCode != 0) //If the SP executed successfully (Increment the appropriate variables based on the action)
                    {
                        switch (nActionCode)
                        {
                            case 11:
                            case 4: //?
                            case 5:
                                nRecordsInserted = nRecordsInserted + 1;
                                break;
                            case 2:
                            case 6:
                                nRecordsUpdatedWITHOUTNewCardAccount = nRecordsUpdatedWITHOUTNewCardAccount + 1;
                                break;
                            case 3:
                            case 7:
                            case 8:
                                nRecordsUpdatedWITHNewCardAccount = nRecordsUpdatedWITHNewCardAccount + 1;
                                break;
                            case 12:
                            case 9:
                            case 10:
                                nRecordsMerged = nRecordsMerged + 1;
                                break;
                        }
                    }
                    else //If the SP execution failed (Increment the variable "nRecordsRejected" AND append into "reject_customer" XML)
                    {
                        nRecordsRejected = nRecordsRejected + 1;
                        writer.WriteStartElement("reject_customer");
                        writer.WriteStartElement("reason");

                        //Changed during the development of NGCV32--For writing the reason in rejected file if the primary_card_account_number tag will be missing.
                        string primaryclubcardID = Convert.ToString(htImportCust["primary_card_account_number"]);
                        XmlNode PrimaryCustomerNode;
                        PrimaryCustomerNode = xmlDoc.SelectSingleNode("//customers/customer/primary_card_account_number[text()='" + primaryclubcardID + "']");

                        if (PrimaryCustomerNode == null)
                        {
                            //sRejectReason changed from "Primary_card_account_number tag is missing" -- Ref No(CQ): MKTG00008972
                            sRejectReason = "Primary_card_account_number tag or its value is missing";
                        }

                        // Change completed.
                        writer.WriteValue(sRejectReason);
                        writer.WriteEndElement(); // Closing of TAG "reason"

                        string clubcardID = Convert.ToString(htImportCust["card_account_number"]);
                        XmlNode CustomerNode;
                        CustomerNode = xmlDoc.SelectSingleNode("//customers/customer/card_account_number[text()='" + clubcardID + "']");
                        if (CustomerNode == null)
                        {

                            //sRejectReason = "";
                            writer.WriteStartElement("customer");
                            if (PrimaryCustomerNode != null)
                            {
                                if (PrimaryCustomerNode.ParentNode != null)
                                {
                                    PrimaryCustomerNode = PrimaryCustomerNode.ParentNode;
                                    foreach (XmlElement item in PrimaryCustomerNode)
                                    {
                                        writer.WriteStartElement(item.Name.ToString());
                                        writer.WriteValue(item.InnerText);
                                        writer.WriteEndElement();
                                    }
                                }
                            }
                            else // both primary_card_account_number and card_account_number are missing 
                            {
                                //The below line will give <Customer> node
                                XmlNode customerNode = xmlDoc.DocumentElement.FirstChild;
                                foreach (XmlElement item in customerNode)
                                {
                                    writer.WriteStartElement(item.Name.ToString());
                                    writer.WriteValue(item.InnerText);
                                    writer.WriteEndElement();
                                }
                            }
                        }
                        else
                        //if (CustomerNode.ParentNode != null)
                        {
                            CustomerNode = CustomerNode.ParentNode;

                            //XmlDocument tempCusDoc=new XmlDocument();
                            //tempCusDoc.LoadXml(CustomerNode.OuterXml);

                            writer.WriteStartElement("customer");

                            //writer.WriteString(tempCusDoc.InnerXml);
                            //writer.WriteNode( (tempCusDoc.InnerXml);

                            foreach (XmlElement item in CustomerNode)
                            {
                                writer.WriteStartElement(item.Name.ToString());
                                writer.WriteValue(item.InnerText);
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement(); // Closing of TAG "customer"
                        writer.WriteEndElement();// Closing of TAG "reject_customer"
                    }

                    htImportCust = new Hashtable();//Reset the HashTable
                }

                //Ending Building XML file where we need to write the rejected customer details.           
                if (arr.Length > 0)
                {
                    writer.WriteFullEndElement();// Closing of TAG "reject_customers"
                    //writer.WriteEndElement();
                    writer.Flush();
                    string sXMLDate = Convert.ToString(sb);
                    doc.LoadXml(sXMLDate);

                    string outputRootDirectory = ConfigurationSettings.AppSettings["RejecetdDirectory"];
                    outputRootDirectory = outputRootDirectory.Trim();
                    if (Directory.Exists(outputRootDirectory))
                    {
                        string sRightMostCharacter = outputRootDirectory.Substring(outputRootDirectory.Length - 1);
                        if (sRightMostCharacter != "\\")
                        {
                            outputRootDirectory = outputRootDirectory + "\\";
                        }
                        if (nRecordsRejected > 0)
                            doc.Save(outputRootDirectory + "reject_customer_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xml");
                    }
                }
                executeResult.Flag = true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                executeResult.Add(e);
                //errorOutput(e.Message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_CUSTOMER, e.Message.ToString(), true);
                CommonFunctions.MessageWriteToLogFile(sFileName, "Error : " + e.Message.ToString());
            }
            finally
            {
                trState.EndProc();
                sb = null;
                writer = null;
                doc = null;
            }
            /*resultXml = executeResult.OuterXml;*/
            //to do

            string sMessageSummary = "";
            sMessageSummary = "Read : " + nTotalRecordsRead + " records." + "\n";
            sMessageSummary = sMessageSummary + "Rejected : " + nRecordsRejected + " customers." + "\n";
            sMessageSummary = sMessageSummary + "Inserted : " + nRecordsInserted + " Customer records." + "\n";
            sMessageSummary = sMessageSummary + "Updated : " + nRecordsUpdatedWITHOUTNewCardAccount + " Customer records without new Card Account records." + "\n";
            sMessageSummary = sMessageSummary + "Updated : " + nRecordsUpdatedWITHNewCardAccount + " Customer records with new Card Account records." + "\n";
            sMessageSummary = sMessageSummary + "Merged  : " + nRecordsMerged + " Customer record pairs." + "\n";
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_CUSTOMER, sMessageSummary, false);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_CUSTOMER, "Batch Register Customers Finished.", false);

            CommonFunctions.MessageWriteToLogFile(sFileName, "Batch Register Customers Completed.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Read : " + nTotalRecordsRead + " records");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Rejected : " + nRecordsRejected + " customers.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Inserted : " + nRecordsInserted + " Customer records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Updated : " + nRecordsUpdatedWITHOUTNewCardAccount + " Customer records without new Card Account records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Updated : " + nRecordsUpdatedWITHNewCardAccount + " Customer records with new Card Account records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Merged  : " + nRecordsMerged + " Customer record pairs.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Finished at : " + DateTime.Now);

            //Archive the input file--V3.1.1[ReqID:5.2]            
            ArchiveInputFile();

            return executeResult.Flag;
        }
        #endregion

        #region UpdateDataBase

        private Int32 UpdateDataBase(Hashtable htImportCust, string connectionString, string sFileName, ref string sRejectReason, ref Int16 nActionCode)
        {
            Int32 iResult = -1;
            Trace trace = new Trace();

            TraceState trState = trace.StartProc("ImportCustomer.UpdateDataBase");
            try
            {
                string optional1_flag = ConfigurationSettings.AppSettings["OptinalDietaryPref1"].ToString();
                string optional2_flag = ConfigurationSettings.AppSettings["OptinalDietaryPref2"].ToString();
                string optional3_flag = ConfigurationSettings.AppSettings["OptinalDietaryPref3"].ToString();
                Hashtable htImportCustValues = new Hashtable();
                Hashtable htInputvalues = new Hashtable();
                //Checking validation only for Formtype 1, So, we need get the value first

                htImportCustValues.Add("name_1", "");
                htImportCustValues.Add("name_2", "");
                htImportCustValues.Add("name_3", "");
                htImportCustValues.Add("official_id", "");
                htImportCustValues.Add("primary_card_account_number", "");
                htImportCustValues.Add("family_member_1_gender_code", "");
                htImportCustValues.Add("family_member_1_dob", "");
                htImportCustValues.Add("family_member_2_dob", "");
                htImportCustValues.Add("family_member_3_dob", "");
                htImportCustValues.Add("family_member_4_dob", "");
                htImportCustValues.Add("family_member_5_dob", "");
                htImportCustValues.Add("family_member_6_dob", "");
                htImportCustValues.Add("address_line_1", "");
                htImportCustValues.Add("address_line_2", "");
                htImportCustValues.Add("address_line_3", "");
                htImportCustValues.Add("city", "");
                htImportCustValues.Add("province_code", "");
                htImportCustValues.Add("country", "");
                htImportCustValues.Add("postal_code", "");
                htImportCustValues.Add("daytime_phone_number", "");
                htImportCustValues.Add("evening_phone_number", "");
                htImportCustValues.Add("mobile_phone_number", "");
                htImportCustValues.Add("email_address", "");
                htImportCustValues.Add("preferred_store_code", "");
                htImportCustValues.Add("joined_store_code", "");
                htImportCustValues.Add("preferred_contact_type_code", "");
                htImportCustValues.Add("allow_promotions_via_mail_flag", "");
                htImportCustValues.Add("allow_promotions_via_phone_flag", "");
                htImportCustValues.Add("allow_group_promotions_flag", "");
                htImportCustValues.Add("allow_third_party_promotions_flag", "");
                htImportCustValues.Add("card_account_number", "");
                htImportCustValues.Add("title_code", "");
                htImportCustValues.Add("diabetic_flag", "");
                htImportCustValues.Add("vegetarian_flag", "");
                htImportCustValues.Add("teetotal_flag", "");
                htImportCustValues.Add("kosher_flag", "");
                htImportCustValues.Add("halal_flag", "");
                htImportCustValues.Add("customer_created_by", "");
                htImportCustValues.Add("customer_created_date", "");
                htImportCustValues.Add("business_name", "");
                htImportCustValues.Add("business_registration_number", "");
                htImportCustValues.Add("business_type_code", "");
                htImportCustValues.Add("race_code", "");
                htImportCustValues.Add("previous_loyalty_scheme_card_number", "");
                htImportCustValues.Add("fax_number", "");
                htImportCustValues.Add("form_type", "");
                htImportCustValues.Add("business_address_line_1", "");
                htImportCustValues.Add("business_address_line_2", "");
                htImportCustValues.Add("business_address_line_3", "");
                htImportCustValues.Add("business_address_line_4", "");
                htImportCustValues.Add("business_address_line_5", "");
                htImportCustValues.Add("business_address_line_6", "");
                htImportCustValues.Add("business_postal_code", "");
                htImportCustValues.Add("family_member_2_gender_code", "");
                htImportCustValues.Add("family_member_3_gender_code", "");
                htImportCustValues.Add("family_member_4_gender_code", "");
                htImportCustValues.Add("family_member_5_gender_code", "");
                htImportCustValues.Add("family_member_6_gender_code", "");
                htImportCustValues.Add("income_band_code", "");
                htImportCustValues.Add("family_member_1_age", "");
                htImportCustValues.Add("family_member_2_age", "");
                htImportCustValues.Add("family_member_3_age", "");
                htImportCustValues.Add("family_member_4_age", "");
                htImportCustValues.Add("family_member_5_age", "");
                htImportCustValues.Add("family_member_6_age", "");
                htImportCustValues.Add("language_code", "");
                htImportCustValues.Add("customer_segment", "");
                htImportCustValues.Add("card_member_name_nric", "");
                htImportCustValues.Add("preferred_mailing_address_flag", "");
                htImportCustValues.Add("card_member_dob", "");
                htImportCustValues.Add("card_member_gender_code", "");
                htImportCustValues.Add("expat", "");
                htImportCustValues.Add("tescogroup_mail_flag", "");
                htImportCustValues.Add("tescogroup_email_flag", "");
                htImportCustValues.Add("tescogroup_phone_flag", "");
                htImportCustValues.Add("tescogroup_sms_flag", "");
                htImportCustValues.Add("partner_mail_flag", "");
                htImportCustValues.Add("partner_email_flag", "");
                htImportCustValues.Add("partner_phone_flag", "");
                htImportCustValues.Add("partner_sms_flag", "");
                htImportCustValues.Add("research_mail_flag", "");
                htImportCustValues.Add("research_email_flag", "");
                htImportCustValues.Add("research_phone_flag", "");
                htImportCustValues.Add("research_sms_flag", "");
                htImportCustValues.Add("celiac_flag", "");
                htImportCustValues.Add("lactose_flag", "");
                htImportCustValues.Add("customer_use_status", "");
                htImportCustValues.Add("customer_mail_status", "");
                htImportCustValues.Add("address_in_error", "");
                htImportCustValues.Add("number_of_household_members", "");
                //NGCV32 req.No:003
                htImportCustValues.Add("geocode_confidence", "");
                htImportCustValues.Add("latitude", "");
                htImportCustValues.Add("longitude", "");
                //Added for NGC 3.6
                htImportCustValues.Add("join_promotional_code", "");
                htImportCustValues.Add("ssn_number", "");
                htImportCustValues.Add("passport_number", "");
                htImportCustValues.Add("buiness_license_number", "");
                htImportCustValues.Add("customer_card_type", "");



                foreach (DictionaryEntry item in htImportCustValues)
                {

                    string sKey = item.Key.ToString();
                    string sValue = item.Value.ToString();

                    if (htImportCust.ContainsKey(sKey))
                    {
                        htInputvalues.Add(sKey, htImportCust[sKey].ToString());
                    }
                    else
                    {
                        htInputvalues.Add(sKey, "");
                    }

                }

                foreach (DictionaryEntry item in htInputvalues)
                {

                    string sKey;
                    string sValue;
                    //For First time itself, Value should get set into FormType property
                    //Do validation only if formtype is 1 otherwise skip it
                    if (!isFormTypeSet)
                    {
                        sKey = "form_type";
                        sValue = htInputvalues["form_type"].ToString();
                    }
                    else
                    {
                        sKey = item.Key.ToString();
                        sValue = item.Value.ToString();
                    }

                    //else if (item.Key.ToString().Equals("form_type")) break;



                    switch (sKey.ToLower())
                    {
                        //Added Otional dietary preferences for V3.1.1 req. No-033b By Netra.
                        case "name_1": { Name1 = sValue; sValue = Name1; break; }
                        case "name_2": { Name2 = sValue; sValue = Name2; break; }
                        case "name_3": { Name3 = sValue; sValue = Name3; break; }
                        case "official_id": { CustAlternateID = sValue; sValue = CustAlternateID; break; }
                        case "primary_card_account_number": { PrimaryClubcardID = sValue; sValue = PrimaryClubcardID; break; }
                        case "family_member_1_gender_code": { Sex = sValue; sValue = Sex; break; }
                        case "family_member_1_dob": { DateOfBirth = sValue; sValue = DateOfBirth; break; }
                        case "family_member_2_dob": { FamilyMember2DOB = sValue; sValue = FamilyMember2DOB; break; }
                        case "family_member_3_dob": { FamilyMember3DOB = sValue; sValue = FamilyMember3DOB; break; }
                        case "family_member_4_dob": { FamilyMember4DOB = sValue; sValue = FamilyMember4DOB; break; }
                        case "family_member_5_dob": { FamilyMember5DOB = sValue; sValue = FamilyMember5DOB; break; }
                        case "family_member_6_dob": { FamilyMember6DOB = sValue; sValue = FamilyMember6DOB; break; }
                        case "address_line_1": { MailingAddressLine1 = sValue; sValue = MailingAddressLine1; break; }
                        case "address_line_2": { MailingAddressLine2 = sValue; sValue = MailingAddressLine2; break; }
                        case "address_line_3": { MailingAddressLine3 = sValue; sValue = MailingAddressLine3; break; }
                        case "city": { MailingAddressLine4 = sValue; sValue = MailingAddressLine4; break; }
                        case "province_code": { MailingAddressLine5 = sValue; sValue = MailingAddressLine5; break; }
                        case "country": { MailingAddressLine6 = sValue; sValue = MailingAddressLine6; break; }
                        case "postal_code": { MailingAddressPostCode = sValue; sValue = MailingAddressPostCode; break; }
                        case "daytime_phone_number": { DaytimePhoneNumber = sValue; sValue = DaytimePhoneNumber; break; }
                        case "evening_phone_number": { EveningPhoneNumber = sValue; sValue = EveningPhoneNumber; break; }
                        case "mobile_phone_number": { MobilePhoneNumber = sValue; sValue = MobilePhoneNumber; break; }
                        case "email_address": { EmailAddress = sValue; sValue = EmailAddress; break; }
                        case "preferred_store_code": { PreferredStoreID = sValue; sValue = PreferredStoreID; break; }
                        case "joined_store_code": { JoinedStoreID = sValue; sValue = JoinedStoreID; break; }
                        case "preferred_contact_type_code": { PreferredContactTypeCode = sValue; sValue = PreferredContactTypeCode; break; }
                        case "allow_promotions_via_mail_flag": { AllowPromotionsViaMail = sValue; sValue = AllowPromotionsViaMail; break; }
                        case "allow_promotions_via_phone_flag": { AllowPromotionsViaPhone = sValue; sValue = AllowPromotionsViaPhone; break; }
                        case "allow_group_promotions_flag": { AllowGroupPromotions = sValue; sValue = AllowGroupPromotions; break; }
                        case "allow_third_party_promotions_flag": { AllowThirdPartyPromotions = sValue; sValue = AllowThirdPartyPromotions; break; }
                        //case "foreign_flag": { Foreign = sValue; sValue = Foreign; break; }
                        case "card_account_number": { ClubcardID = sValue; sValue = ClubcardID; break; }
                        //case "address_in_error":
                        case "title_code": { Title = sValue; sValue = Title; break; }
                        case "diabetic_flag": { Diabetic = sValue; sValue = Diabetic; break; }
                        case "vegetarian_flag": { Vegetarian = sValue; sValue = Vegetarian; break; }
                        case "teetotal_flag": { Teetotal = sValue; sValue = Teetotal; break; }
                        case "kosher_flag": { Kosher = sValue; sValue = Kosher; break; }
                        case "halal_flag": { Halal = sValue; sValue = Halal; break; }
                        case "customer_created_by": { InsertBy = sValue; sValue = InsertBy; break; }
                        case "customer_created_date": { CustomerCreatedDate = sValue; sValue = CustomerCreatedDate; break; }
                        case "business_name": { BusinessName = sValue; sValue = BusinessName; break; }
                        case "business_registration_number": { BusinessRegistrationNumber = sValue; sValue = BusinessRegistrationNumber; break; }
                        case "business_type_code": { BusinessType = sValue; sValue = BusinessType; break; }
                        case "race_code": { RaceID = sValue; sValue = RaceID; break; }
                        case "previous_loyalty_scheme_card_number": { PreviousLoyaltySchemeClubcardId = sValue; sValue = PreviousLoyaltySchemeClubcardId; break; }
                        case "fax_number": { FaxNumber = sValue; sValue = FaxNumber; break; }
                        case "form_type": { FormType = sValue; sValue = FormType; break; }

                        case "business_address_line_1": { BusinessAddressLine1 = sValue; sValue = BusinessAddressLine1; break; }
                        case "business_address_line_2": { BusinessAddressLine2 = sValue; sValue = BusinessAddressLine2; break; }
                        case "business_address_line_3": { BusinessAddressLine3 = sValue; sValue = BusinessAddressLine3; break; }
                        case "business_address_line_4": { BusinessAddressLine4 = sValue; sValue = BusinessAddressLine4; break; }
                        case "business_address_line_5": { BusinessAddressLine5 = sValue; sValue = BusinessAddressLine5; break; } // business_province_code
                        case "business_address_line_6": { BusinessAddressLine6 = sValue; sValue = BusinessAddressLine6; break; }
                        case "business_postal_code": { BusinessAddressPostCode = sValue; sValue = BusinessAddressPostCode; break; }

                        case "family_member_2_gender_code": { FamilyMember2GenderCode = sValue; sValue = FamilyMember2GenderCode; break; }
                        case "family_member_3_gender_code": { FamilyMember3GenderCode = sValue; sValue = FamilyMember3GenderCode; break; }
                        case "family_member_4_gender_code": { FamilyMember4GenderCode = sValue; sValue = FamilyMember4GenderCode; break; }
                        case "family_member_5_gender_code": { FamilyMember5GenderCode = sValue; sValue = FamilyMember5GenderCode; break; }
                        case "family_member_6_gender_code": { FamilyMember6GenderCode = sValue; sValue = FamilyMember6GenderCode; break; }
                        case "income_band_code": { IncomeBandID = sValue; sValue = IncomeBandID; break; }
                        case "family_member_1_age": { FamilyMember1Age = sValue; sValue = FamilyMember1Age; break; }
                        case "family_member_2_age": { FamilyMember2Age = sValue; sValue = FamilyMember2Age; break; }
                        case "family_member_3_age": { FamilyMember3Age = sValue; sValue = FamilyMember3Age; break; }
                        case "family_member_4_age": { FamilyMember4Age = sValue; sValue = FamilyMember4Age; break; }
                        case "family_member_5_age": { FamilyMember5Age = sValue; sValue = FamilyMember5Age; break; }
                        case "family_member_6_age": { FamilyMember6Age = sValue; sValue = FamilyMember6Age; break; }
                        case "language_code": { ISOLanguageCode = sValue; sValue = ISOLanguageCode; break; }
                        case "customer_segment": { CustomerSegmentID = sValue; sValue = CustomerSegmentID; break; }
                        case "card_member_name_nric": { NameAsInNRIC = sValue; sValue = NameAsInNRIC; break; }
                        case "preferred_mailing_address_flag": { PreferredMailingAddress = sValue; sValue = PreferredMailingAddress; break; }
                        case "card_member_dob": { CardMemberDOB = sValue; sValue = CardMemberDOB; break; }
                        case "card_member_gender_code": { CardMemberGender = sValue; sValue = CardMemberGender; break; }
                        case "expat": { Expat = sValue; sValue = Expat; break; }

                        //Data Protection Preferences--V3.1 [Req ID:007]
                        case "tescogroup_mail_flag": { TescoGroupMail = sValue; sValue = TescoGroupMail; break; }
                        case "tescogroup_email_flag": { TescoGroupEmail = sValue; sValue = TescoGroupEmail; break; }
                        case "tescogroup_phone_flag": { TescoGroupPhone = sValue; sValue = TescoGroupPhone; break; }
                        case "tescogroup_sms_flag": { TescoGroupSms = sValue; sValue = TescoGroupSms; break; }
                        case "partner_mail_flag": { PartnerMail = sValue; sValue = PartnerMail; break; }
                        case "partner_email_flag": { PartnerEmail = sValue; sValue = PartnerEmail; break; }
                        case "partner_phone_flag": { PartnerPhone = sValue; sValue = PartnerPhone; break; }
                        case "partner_sms_flag": { PartnerSms = sValue; sValue = PartnerSms; break; }
                        case "research_mail_flag": { ResearchMail = sValue; sValue = ResearchMail; break; }
                        case "research_email_flag": { ResearchEmail = sValue; sValue = ResearchEmail; break; }
                        case "research_phone_flag": { ResearchPhone = sValue; sValue = ResearchPhone; break; }
                        case "research_sms_flag": { ResearchSms = sValue; sValue = ResearchSms; break; }

                        //Added for NGCV3.1
                        case "celiac_flag": { Celiac = sValue; sValue = Celiac; break; }
                        case "lactose_flag": { Lactose = sValue; sValue = Lactose; break; }


                        case "number_of_household_members": { NoOfHouseHoldMember = sValue; sValue = NoOfHouseHoldMember; break; }

                        //Fetch the Customer Use Status and Mail Status -- V3.1.1[ReqID:012] --Kavitha

                        case "customer_use_status": { CustomerUseStatusID = sValue; sValue = CustomerUseStatusID; break; }
                        case "customer_mail_status": { CustomerMailStatus = sValue; sValue = CustomerMailStatus; break; }

                        case "address_in_error": { AddressInErrorValue = sValue; sValue = AddressInErrorValue; break; }
                        //NGCV32 req.No:003
                        case "geocode_confidence": { GeocodeConfidence = sValue; sValue = GeocodeConfidence; break; }
                        case "latitude": { Latitude = sValue; sValue = Latitude; break; }
                        case "longitude": { Longitude = sValue; sValue = Longitude; break; }
                        //Added for NGC 3.6
                        case "join_promotional_code": { JoinPromotionCode = sValue; sValue = JoinPromotionCode; break; }
                        case "ssn_number": { SSNNumber = sValue; sValue = SSNNumber; break; }
                        case "passport_number": { PassportNumber = sValue; sValue = PassportNumber; break; }

                    }

                    //if it is false, set it TRUE
                    if (!isFormTypeSet)
                        isFormTypeSet = true;
                }

                //Added for NGCV3.1.1 req.no-033b By Netra
                if (htInputvalues[optional1_flag] != null)
                    Optional1 = htInputvalues[optional1_flag].ToString();

                if (htInputvalues[optional2_flag] != null)
                    Optional2 = htInputvalues[optional2_flag].ToString();

                if (htInputvalues[optional3_flag] != null)
                    Optional3 = htInputvalues[optional3_flag].ToString();
                // Change Complete.

                InsertBy = "1";


                #region Backup Code

                //Validate the Customer status and Mail Status --Kavitha --V3.1.1[ReqID:012]
                //ageLimit = ConfigurationSettings.AppSettings["underage_limit"].ToString();
                //int ageDifference = CalculateAge(Convert.ToDateTime(DateOfBirth));

                //if (AddressInErrorValue == "1")
                //{
                //    CustomerMailStatus = "2";
                //    customerUseStatusID = "0";
                //}
                //if (CustomerUseStatusID == "6")
                //{
                //    if (ageDifference < Convert.ToInt32(ageLimit))
                //    {
                //        CustomerUseStatusID = "7";
                //        CustomerMailStatus = "3";
                //    }
                //    else
                //    {
                //        CustomerMailStatus = "3";
                //    }
                //}
                //else if (ageDifference < Convert.ToInt32(ageLimit))
                //{
                //    CustomerUseStatusID = "5";
                //    CustomerMailStatus = "3";
                //}




                //if ((CustomerUseStatusID == "-1" && CustomerMailStatus == "-1") || (CustomerUseStatusID == null && CustomerMailStatus == null))
                //{
                //    if (MailingAddressLine1 != null && MailingAddressLine2 != null && MailingAddressLine4 != null)
                //    {
                //        if (MailingAddressLine1.Trim() != "" && MailingAddressLine2.Trim() != ""
                //               && MailingAddressLine4.Trim() != "")
                //        {
                //            CustomerUseStatusID = "0";
                //            CustomerMailStatus = "0";
                //        }
                //        else
                //        {
                //            CustomerUseStatusID = "-2";
                //            CustomerMailStatus = "-2";
                //        }
                //    }
                //    else
                //    {
                //        CustomerUseStatusID = "-2";
                //        CustomerMailStatus = "-2";
                //    }

                //}
                #endregion

                #region Updated Code - NGC 3.6

                //updated by sabhari -for NGC 3.6
                //Added into Error code and Assign the Customer Use and Mail Status wrto Priority

                //Validate the Customer status and Mail Status --Kavitha --V3.1.1[ReqID:012]
                ageLimit = ConfigurationSettings.AppSettings["underage_limit"].ToString();
                int ageDifference = CalculateAge(Convert.ToDateTime(DateOfBirth));

                if (AddressInErrorValue == "1")
                {
                    CustomerMailStatus = ConfigurationManager.AppSettings["Address In Error"].ToString();
                    sErrorCode += ConfigurationManager.AppSettings["AddressInError"].ToString() + ",";
                }
                //else
                //{    //set to deliverables
                //    CustomerMailStatus = ConfigurationManager.AppSettings["Deliverable"].ToString();
                //}


                if (CustomerUseStatusID == "6")
                {
                    if (ageDifference < Convert.ToInt32(ageLimit))
                    {
                        //CustomerUseStatusID = "7";
                        CustomerMailStatus = ConfigurationManager.AppSettings["Not Mailable"].ToString();

                        sErrorCode += ConfigurationManager.AppSettings["UnderAgeUnsignedError"].ToString() + ",";
                    }
                    else
                    {
                        CustomerMailStatus = ConfigurationManager.AppSettings["Not Mailable"].ToString();
                        sErrorCode += ConfigurationManager.AppSettings["UnSignedError"].ToString() + ",";
                    }

                }
                else if (ageDifference < Convert.ToInt32(ageLimit))
                {
                    CustomerMailStatus = ConfigurationManager.AppSettings["Not Mailable"].ToString();
                    sErrorCode += ConfigurationManager.AppSettings["UnderAgeError"].ToString() + ",";
                }


                //if ((CustomerMailStatus == "-1") || (CustomerMailStatus == null))
                //{
                //Check if  Address in Error Flag is not equal to 1
                if (AddressInErrorValue != "1")
                {
                    if (MailingAddressLine1 != null && MailingAddressLine2 != null && MailingAddressLine4 != null && MailingAddressPostCode.Trim() != null)
                    {
                        if (MailingAddressLine1.Trim() != "" && MailingAddressLine2.Trim() != ""
                               && MailingAddressLine4.Trim() != "" && MailingAddressPostCode.Trim() != "" && isPostValid)
                        {

                            //set to Mailable
                            CustomerMailStatus = ConfigurationManager.AppSettings["Mailable"].ToString();
                            //sErrorCode += ConfigurationManager.AppSettings["AddressFound"].ToString()+",";
                        }
                        else
                        {
                            CustomerMailStatus = ConfigurationManager.AppSettings["NoAddress"].ToString();
                            //if (!sErrorCode.Contains(ConfigurationManager.AppSettings["PostCodeInError"].ToString()))
                            sErrorCode += ConfigurationManager.AppSettings["AddressInError"].ToString() + ",";
                        }
                    }
                    else
                    {
                        //CustomerUseStatusID = "-2";
                        CustomerMailStatus = ConfigurationManager.AppSettings["NoAddress"].ToString();
                        //if (!sErrorCode.Contains(ConfigurationManager.AppSettings["PostCodeInError"].ToString()))
                        sErrorCode += ConfigurationManager.AppSettings["AddressInError"].ToString() + ",";
                    }
                }

                #region Get the Primary / Secondary Id

                string sFinal_SSNNumber = null;
                string sFinal_PassportNumber = null;


                //Either take Primary(SSN) or Secondary Id 

                //Check Primary Id is valid  and Not Empty(i.e SSN)
                if (isSSNValid && !string.IsNullOrEmpty(SSNNumber))
                {
                    sFinal_SSNNumber = SSNNumber;
                    sFinal_PassportNumber = null;
                }
                //Check Secondary Id is valid  and  Not Empty(i.e Passport)
                else if (isPassportValid && !string.IsNullOrEmpty(PassportNumber))
                {
                    sFinal_PassportNumber = PassportNumber;
                    sFinal_SSNNumber = null;
                }
                //Primary Id is valid  and Empty(i.e SSN) and Secondary Id is valid  and  Empty(i.e Passport)
                //when no value is passed, during non-mandatory
                else if ((isSSNValid && string.IsNullOrEmpty(SSNNumber)) && (isPassportValid && string.IsNullOrEmpty(PassportNumber)))
                {
                    sFinal_SSNNumber = null;
                    sFinal_PassportNumber = null;
                }
                //Set the Error Flag after check the status flag
                //if Primary/Secondary flag may be invalid or may be null 
                //COMES HERE ONLY BOTH VALUE ARE NOT VALID
                else if (!isSSNValid || !isPassportValid)
                {
                    //for both mandatory and non-mandatory
                    sFinal_SSNNumber = null;
                    sFinal_PassportNumber = null;

                    //If both are invalid / any one is invalid - other one entry
                    sErrorCode += ConfigurationManager.AppSettings["PrimarySecondaryError"].ToString() + ",";

                    //check primary id/Secondary id (i.e if any one/both set as mandatory -> Consider as mandatory) otherwise, non-mandatory
                    if ((bIsSSNMandatory || bIsPassportMandatory) && (FormType.Equals("1")))
                    {
                        sValidationFails += "PrimarySecondaryCheck,";

                    }
                }

                #endregion


                //}
                #endregion

                //End of Modification

                if (!CalculateCheckDigit(ClubcardID))
                {
                    if (string.IsNullOrEmpty(ClubcardID))
                    {
                        sRejectReason = "Card_account_number tag is missing";
                    }
                    else
                    {
                        sRejectReason = cardInvalid;
                    }
                    nActionCode = 0;
                    return 0;
                }

                //if Error code found, then append the , at the first
                sErrorCode = sErrorCode.Equals("") ? sErrorCode : sErrorCode.Insert(0, ",");

                //Set the Default Data Protection Preference--V3.1 [Req ID:007]
                string DefaultPreference = ConfigurationSettings.AppSettings["DefaultDataProtectionPreference"].ToString();
                //if (htImportCust["form_type"].ToString() == "1" && !htImportCust["card_account_number"].Equals(htImportCust["primary_card_account_number"]))
                //{

                //    sErrorCode = "";
                //    sValidationFails = "";
                //}

                object[] objDBParams = { Name1, Name2, Name3, Title, CustAlternateID, Sex, FamilyMember2GenderCode,  FamilyMember3GenderCode, 
                                           FamilyMember4GenderCode, FamilyMember5GenderCode, FamilyMember6GenderCode, IncomeBandID, 
                                           DateOfBirth, FamilyMember2DOB, FamilyMember3DOB, 
                                           FamilyMember4DOB, FamilyMember5DOB, FamilyMember6DOB, 
                                           FamilyMember1Age, FamilyMember2Age, FamilyMember3Age,
                                           FamilyMember4Age, FamilyMember5Age, FamilyMember6Age,
                                           MailingAddressLine1, MailingAddressLine2, MailingAddressLine3, MailingAddressLine4, MailingAddressLine5, 
                                           MailingAddressLine6, MailingAddressPostCode, DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber, 
                                           FaxNumber, EmailAddress, PreferredContactTypeCode, CustomerCreatedDate, 
                                           JoinedStoreID, ISOLanguageCode,PreferredStoreID, AllowPromotionsViaMail,
                                           AllowPromotionsViaPhone, AllowGroupPromotions,AllowThirdPartyPromotions,
                                           Diabetic, Vegetarian, Teetotal, Kosher, 
                                           Halal, Celiac, Lactose,Optional1,Optional2,Optional3, ClubcardID, 
                                           PrimaryClubcardID, FormType, CustomerMailStatus, NameAsInNRIC, InsertBy,
                                           CustomerCreatedDate, PreviousLoyaltySchemeClubcardId,RaceID,//Convert.ToInt16(RaceID),
                                           BusinessType, BusinessName, CustomerSegmentID, 
                                           BusinessRegistrationNumber, PreferredMailingAddress, PreviousOfferPrimaryCustomerID
                                           , Expat, BusinessAddressLine1, BusinessAddressLine2, BusinessAddressLine3, BusinessAddressLine4
                                           , BusinessAddressLine5, BusinessAddressLine6, BusinessAddressPostCode,CardMemberDOB,CardMemberGender,
                                           TescoGroupMail,TescoGroupEmail,TescoGroupPhone,TescoGroupSms,
                                           PartnerMail,PartnerEmail,PartnerPhone,PartnerSms,
                                           ResearchMail,ResearchEmail,ResearchPhone,ResearchSms,DefaultPreference,NoOfHouseHoldMember,CustomerUseStatusID,
                                           //NGCV32 req.No:003
                                           GeocodeConfidence,Latitude,Longitude,JoinPromotionCode,sErrorCode,
                                           CustomerEMailStatus,CustomerMobilePhoneStatus,sFinal_SSNNumber,sFinal_PassportNumber
                                          ,sRejectReason , nActionCode};// ,Foreign

                //check for Validation variable, if it got any Error?

                //if main validation not fails
                if (sValidationFails.Trim().Equals(""))
                {
                    iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_DETAILS, ref objDBParams);

                    Int32 i = objDBParams.Length - 1;
                    sRejectReason = Convert.ToString(objDBParams[i - 1]);
                    nActionCode = Convert.ToInt16(objDBParams[i]);
                }
                //Fails due to (values not passed/formar is wrong) when it is mandatory
                //Should log into CustomerValidationHistory table
                else
                {

                    string[] getstr = sValidationFails.TrimEnd(',').Split(',');

                    //assign the value and write into log file
                    for (int sval = 0; sval < getstr.Length; sval++)
                    {

                        sRejectReason += ConfigurationManager.AppSettings[getstr[sval].ToString()].ToString() + ".";
                    }

                    //log the entry of rejected customer into DB (customervalidationhistory table)
                    object[] objDBParams_rej = {sErrorCode, ClubcardID,null,Name1, Name2, Name3,PrimaryClubcardID, MailingAddressLine1, MailingAddressLine2, MailingAddressLine3, MailingAddressLine4,
                                                 MailingAddressPostCode,DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber,EmailAddress,null,SSNNumber,PassportNumber,DateOfBirth,Title,Sex,RaceID,ISOLanguageCode};


                    iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_AUDIT_CUSTOMER_VALIDATION, ref objDBParams_rej);


                    nActionCode = -1;
                }



                //for (i = 0; i < objDBParams.Length - 1; i++)
                // {
                //     if (objDBParams[i] == null)
                //     {
                //         CommonFunctions.MessageWriteToLogFile(sFileName, "null" + ",");
                //     }
                //     else
                //     {
                //         CommonFunctions.MessageWriteToLogFile(sFileName, objDBParams[i].ToString() + ",");
                //     }
                // }                      
                //iResult = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_DETAILS, objDBParams);

            }
            catch (Exception ex)
            {
                sRejectReason += sRejectReason.Equals("") ? ex.Message : " ";
                ExceptionManager.Publish(ex);

            }
            finally
            {


                trState.EndProc();
            }
            return iResult;
        }
        #endregion

        #region HasCorrectCheckDigit
        private string HasCorrectCheckDigit(string parameters)
        {
            string answer = false.ToString();
            try
            {

                /*if (parameters.Length != 1)
                {
                    return answer;
                }*/
                string cardNumber = parameters;
                int cardNumberBound = cardNumber.Length - 1;
                // Must contain at least 2 digits
                if (cardNumberBound < 1)
                {
                    return answer;
                }

                int cardDigit;
                int sum = 0;
                // ignore the last digit, as this is the check digit
                for (int i = 0; i < cardNumberBound; i++)
                {
                    int weight = 2 - (i % 2);
                    cardDigit = int.Parse(cardNumber[i].ToString());
                    int digitTimesWeight = cardDigit * weight;
                    sum += (digitTimesWeight % 10) + (digitTimesWeight / 10);
                }

                int checkDigit = 10 - (sum % 10);
                // If the last digit is 0, the check digit should be zero
                if (checkDigit == 10)
                {
                    checkDigit = 0;
                }

                cardDigit = int.Parse(cardNumber[cardNumberBound].ToString());
                answer = (checkDigit == cardDigit) ? true.ToString() : false.ToString();
            }
            catch
            {
                // Don't propagate
            }
            return answer;
        }
        #endregion

        #region CreateErrorXML File
        private void CreateErrorXMLFile(Hashtable ht, string sErrorMessage)
        {


        }

        #endregion

        #region Check the Card Number Validity //VP
        public bool CalculateCheckDigit(string cardNumber)
        {
            try
            {
                //Handled the 13 digit no also
                //Is 13 Clubcard no is needed
                string Validate13digitClubcard = ConfigurationSettings.AppSettings["Validate13digitClubcard"].ToString();
                //If 13 digit and flag is on
                if (Validate13digitClubcard.Equals("1") && cardNumber.Length.Equals(13))
                {
                    return true;
                }
                else
                {
                int cardNumberBound = cardNumber.Length - 1;
                // Card Number must contain at least 2 digits, including the check digit
                if (cardNumberBound < 1)
                {
                    return false;
                }
                int sum = 0;
                // ignore the last digit, as this is the check digit
                for (int i = 0; i < cardNumberBound; i++)
                {
                    int weight = 2 - (i % 2);
                    int digitTimesWeight = int.Parse(cardNumber[i].ToString()) * weight;
                    sum += (digitTimesWeight % 10) + (digitTimesWeight / 10);
                }
                int correctCheckDigit = 10 - (sum % 10);
                // If the check digit is 10, the check digit should be zero
                if (correctCheckDigit == 10)
                {
                    correctCheckDigit = 0;
                }
                int cardCheckDigit = int.Parse(cardNumber[cardNumberBound].ToString());
                return (correctCheckDigit == cardCheckDigit);
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ArragneGenderCode
        private string ArragneGenderCode(string sValue)
        {
            if (sValue == "M" || sValue == "0")
            {
                return "M";
            }
            else if (sValue == "F" || sValue == "1")
            {
                return "F";
            }
            else if (sValue == "U" || sValue == "2")
            {
                return "U";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region ArragneFamilyMemberAge
        private string ArragneFamilyMemberAge(string sValue)
        {

            //Regex regex = new Regex("[0-9]");
            Regex regex = new Regex("^[0-9]*$");

            if (!regex.IsMatch(sValue))
            {
                return null;
            }
            else
            {
                //Check for empty string before sending
                if (!sValue.Equals(""))
                {
                    return sValue;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ArchiveInputFile
        private void ArchiveInputFile()
        {
            //Back up the customer.xml file after processing the file(move it into Batch\Data\Archive folder)
            string inputPath = ConfigurationSettings.AppSettings["InputRootDirectory"];
            string ArchiveRootDirectory = ConfigurationSettings.AppSettings["ArchiveRootDirectory"];

            string ExactFileName = "Customer";
            if (File.Exists(inputPath + "\\Customer.xml"))
            {
                File.Move(inputPath + "\\Customer.xml", ArchiveRootDirectory + "\\" + ExactFileName + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xml");
            }
        }
        #endregion

        #region CalculateAge
        private int CalculateAge(DateTime birthDate)
        {
            #region Old Code
            //DateTime todayDate = DateTime.Now;
            //int years = DateTime.Now.Year - birthDate.Year;
            //string done = "0";
            //// subtract another year if we're before the
            //// birth day in the current year
            //// date2 = date2.AddYears(years);
            ////if (DateTime.Now.CompareTo(date2) < 0) { years--; }
            //if (years <= Convert.ToInt32(ageLimit))
            //{
            //    if (birthDate.Month < todayDate.Month)
            //    {
            //        years--;
            //        done = "1";
            //    }
            //    if ((birthDate.Day < todayDate.Day) && (done == "0"))
            //    {
            //        years--;
            //    }
            //}
            //return years; 
            #endregion

            #region New Code
                //To Fix MKTG00008762 Issue for group Country
                DateTime now = DateTime.Now;
                int age = now.Year - birthDate.Year;
                if (now < birthDate.AddYears(age)) age--;
                return age;
            #endregion


        }
        #endregion

        #endregion




    }
}