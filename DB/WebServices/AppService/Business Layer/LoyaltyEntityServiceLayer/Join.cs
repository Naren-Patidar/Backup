/*
 * File   : Join.cs
 * Author : Shishir
 * email  :
 * File   : This file contains methods/properties related to Clubcard Join
 * Date   : 21/12/2011
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
//using LoyaltyEntityServiceLayer.ExactTargetService;
//using LoyaltyEntityServiceLayer.FDRequest;
using USLoyaltySecurityServiceLayer;
using System.Xml;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class Join
    {
        #region Fields


        /// <summary>
        /// Customer Title
        /// </summary>
        public string titleEnglish;

        /// <summary>
        /// Customer Name1
        /// </summary>
        public string name1;

        /// <summary>
        /// Customer Name2
        /// </summary>
        public string name2;

        /// <summary>
        /// Customer Name3
        /// </summary>
        public string name3;

        /// <summary>
        /// Customer DateOfBirth
        /// </summary>
        public string dateOfBirth;

        /// <summary>
        /// Customer DateOfBirth1
        /// </summary>
        public int dateOfBirth1;

        /// <summary>
        /// Customer DateOfBirth2
        /// </summary>
        public int dateOfBirth2;

        /// <summary>
        /// Customer DateOfBirth3
        /// </summary>
        public int dateOfBirth3;

        /// <summary>
        /// Customer DateOfBirth4
        /// </summary>
        public int dateOfBirth4;

        /// <summary>
        /// Customer DateOfBirth5
        /// </summary>
        public int dateOfBirth5;

        /// <summary>
        /// Customer Sex
        /// </summary>
        public string sex;

        /// <summary>
        /// Customer MailingAddressLine1
        /// </summary>
        public string mailingAddressLine1;

        /// <summary>
        /// Customer MailingAddressLine2
        /// </summary>
        public string mailingAddressLine2;

        /// <summary>
        /// Customer MailingAddressLine3
        /// </summary>
        public string mailingAddressLine3;

        /// <summary>
        /// Customer MailingAddressLine4
        /// </summary>
        public string mailingAddressLine4;

        /// <summary>
        /// Customer MailingAddressLine5
        /// </summary>
        public string mailingAddressLine5;

        /// <summary>
        /// Customer MailingAddressLine6
        /// </summary>
        public string mailingAddressLine6;

        /// <summary>
        /// Customer MailingAddressPostCode
        /// </summary>
        public string mailingAddressPostCode;

        /// <summary>
        /// Customer DaytimePhoneNumber
        /// </summary>
        public string daytimePhoneNumber;

        /// <summary>
        /// Customer EveningPhoneNumber
        /// </summary>
        public string eveningPhoneNumber;

        /// <summary>
        /// Customer MobilePhoneNumber
        /// </summary>
        public string mobilePhoneNumber;

        /// <summary>
        /// Customer EmailAddress
        /// </summary>
        public string emailAddress;

        /// <summary>
        /// Customer passport Number
        /// </summary>
        public string passportNumber;

        /// <summary>
        /// Customer Business Line Number
        /// </summary>
        public string businessLineNumber;

        /// <summary>
        /// Customer Culture
        /// </summary>
        public string culture;

        /// <summary>
        /// Customer JoinedDate
        /// </summary>
        public DateTime joinedDate;


        /// <summary>
        /// Customer CustomerMailStatus
        /// </summary>
        public Int16 customerMailStatus;


        /// <summary>
        /// Customer CustomerMobilePhoneStatus
        /// </summary>
        public Int16 customerMobilePhoneStatus;

        /// <summary>
        /// Customer CustomerEmailStatus
        /// </summary>
        public Int16 customerEmailStatus;

        /// <summary>
        /// Customer customerUseStatusID
        /// </summary>
        public Int16 customerUseStatusID;


        /// <summary>
        /// Customer JoinedStoreID
        /// </summary>
        public string joinedStoreID;
        public string language;
        public string race;
        public string dynamicPreferences;
        ///// <summary>
        ///// Call ID
        ///// </summary>
        //public int64 callID;

        public string diabeticFlag;
        public string teetotalFlag;
        public string vegetarianFlag;
        public string halalFlag;
        public string celiacFlag;
        public string lactoseFlag;
        public string expat;
        public string sOption1;
        public string sOption2;
        public string sOption3;
        public string sOption4;
        public string sOption5;
        public string sOption6;
        public string sOption7;
        public string sOption8;
        public string sOption9;
        public string sOption10;
        public string sOption11;
        public string sOption12;
        public string sOption13;
        public string sOption14;
        public string sOption15;
        public int noOfhouseHold;

        ///added as part ofCCO Convergence
        public string kosherFlag;
        public string promotionCode;
        public string sSSN;
        public string passport;

        /// <summary>
        /// Check customer activated

        /// </summary>
        public Int64 dotcomCustomerID;

        public Int64 customerID;

        #endregion

        #region Properties


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
        public int DateOfBirth1 { get { return this.dateOfBirth1; } set { this.dateOfBirth1 = value; } }

        /// <summary>
        ///  Customer DateOfBirth2
        /// </summary>
        public int DateOfBirth2 { get { return this.dateOfBirth2; } set { this.dateOfBirth2 = value; } }

        /// <summary>
        ///  Customer DateOfBirth3
        /// </summary>
        public int DateOfBirth3 { get { return this.dateOfBirth3; } set { this.dateOfBirth3 = value; } }

        /// <summary>
        ///  Customer DateOfBirth4
        /// </summary>
        public int DateOfBirth4 { get { return this.dateOfBirth4; } set { this.dateOfBirth4 = value; } }

        /// <summary>
        ///  Customer DateOfBirth5
        /// </summary>
        public int DateOfBirth5 { get { return this.dateOfBirth5; } set { this.dateOfBirth5 = value; } }

        /// <summary>
        ///  Customer Sex
        /// </summary>
        public string Sex { get { return this.sex; } set { this.sex = value; } }

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
        ///  Customer EmailAddress
        /// </summary>
        public string EmailAddress { get { return this.emailAddress; } set { this.emailAddress = value; } }

        /// <summary>
        ///  Customer JoinedDate
        /// </summary>
        public DateTime JoinedDate { get { return this.joinedDate; } set { this.joinedDate = value; } }

        /// <summary>
        ///  Customer JoinedStoreID
        /// </summary>
        public string JoinedStoreID { get { return this.joinedStoreID; } set { this.joinedStoreID = value; } }
        /// <summary>
        ///  Customer Language
        /// </summary>
        public string Language { get { return this.language; } set { this.language = value; } }
        /// <summary>
        ///  Customer Race
        /// </summary>
        public string Race { get { return this.race; } set { this.race = value; } }
        /// <summary>
        ///  DiabeticFlag
        /// </summary>
        /// 
        public string DynamicPreferences { get { return this.dynamicPreferences; } set { this.dynamicPreferences = value; } }
        /// <summary>
        ///  DiabeticFlag
        /// </summary>
        /// 
        
        public string DiabeticFlag { get { return this.diabeticFlag; } set { this.diabeticFlag = value; } }

        /// <summary>
        ///  TeetotalFlag
        /// </summary>
        public string TeetotalFlag { get { return this.teetotalFlag; } set { this.teetotalFlag = value; } }

        /// <summary>
        ///  VegetarianFlag
        /// </summary>
        public string VegetarianFlag { get { return this.vegetarianFlag; } set { this.vegetarianFlag = value; } }

        /// <summary>
        ///  HalalFlag
        /// </summary>
        public string HalalFlag { get { return this.halalFlag; } set { this.halalFlag = value; } }

        /// <summary>
        ///  CeliacFlag
        /// </summary>
        public string CeliacFlag { get { return this.celiacFlag; } set { this.celiacFlag = value; } }

        /// <summary>
        ///  LactoseFlag
        /// </summary>
        public string LactoseFlag { get { return this.lactoseFlag; } set { this.lactoseFlag = value; } }

        /// <summary>
        ///  Expat
        /// </summary>
        public string Expat { get { return this.expat; } set { this.expat = value; } }

        /// <summary>
        ///  KosherFlag
        /// </summary>
        public string KosherFlag { get { return this.kosherFlag; } set { this.kosherFlag = value; } }
        /*End CCO Properties*/

        /// <summary>
        ///  Number Of House Hold
        /// </summary>
        public int NoOfHouseHold { get { return this.noOfhouseHold; } set { this.noOfhouseHold = value; } }

        public string SSN { get { return this.sSSN; } set { this.sSSN = value; } }
        public string Passport { get { return this.passport; } set { this.passport = value; } }
        /// <summary>
        ///  Customer Name1
        /// </summary>
        public string Option1 { get { return this.sOption1; } set { this.sOption1 = value; } }

        /// <summary>
        ///  Customer Name2
        /// </summary>
        public string Option2 { get { return this.sOption2; } set { this.sOption2 = value; } }

        /// <summary>
        ///  Customer Name3
        /// </summary>
        public string Option3 { get { return this.sOption3; } set { this.sOption3 = value; } }
        #endregion

         /// <summary>
        ///  Customer Name4
        /// </summary>
        public string Option4 { get { return this.sOption4; } set { this.sOption4 = value; } }
        /// <summary>
        ///  Customer Name5
        /// </summary>
        public string Option5 { get { return this.sOption5; } set { this.sOption5 = value; } }
        /// <summary>
        ///  Customer Name6
        /// </summary>
        public string Option6{ get { return this.sOption6; } set { this.sOption6 = value; } }
        /// <summary>
        ///  Customer Name7
        /// </summary>
        public string Option7 { get { return this.sOption7; } set { this.sOption7 = value; } }
        #region Methods
        /// <summary>
        ///  Customer Name8
        /// </summary>
        public string Option8 { get { return this.sOption8; } set { this.sOption8 = value; } }
        /// <summary>
        ///  Customer Name9
        /// </summary>
        public string Option9 { get { return this.sOption9; } set { this.sOption9 = value; } }
        /// <summary>
        ///  Customer Name10
        /// </summary>
        public string Option10 { get { return this.sOption10; } set { this.sOption10 = value; } }
        /// <summary>
        ///  Customer Name11
        /// </summary>
        public string Option11 { get { return this.sOption11; } set { this.sOption11 = value; } }
        /// <summary>
        ///  Customer Name12
        /// </summary>
        public string Option12 { get { return this.sOption12; } set { this.sOption12 = value; } }
        public string Option13 { get { return this.sOption13; } set { this.sOption13 = value; } }
        public string Option14 { get { return this.sOption14; } set { this.sOption14 = value; } }
        public string Option15 { get { return this.sOption15; } set { this.sOption15 = value; } }

        /// <summary>
        ///  Customer CustomerMailStatus
        /// </summary>
        public Int16 CustomerMailStatus { get { return this.customerMailStatus; } set { this.customerMailStatus = value; } }

        /// <summary>
        ///  Customer CustomerUseStatusID
        /// </summary>
        public Int16 CustomerUseStatusID { get { return this.customerUseStatusID; } set { this.customerUseStatusID = value; } }

        /// <summary>
        ///  Customer CustomerUseStatusID
        /// </summary>
        public Int16 CustomerMobilePhoneStatus { get { return this.customerMobilePhoneStatus; } set { this.customerMobilePhoneStatus = value; } }

        /// <summary>
        ///  Customer CustomerUseStatusID
        /// </summary>
        public Int16 CustomerEmailStatus { get { return this.customerEmailStatus; } set { this.customerEmailStatus = value; } }

//join enhancement
        /// <summary>
        /// Creates a Clubcard account within the Tesco Plc Clubcard system.
        /// </summary>
        /// <param name="dotcomCustomerID"></param>
        /// <param name="customer"></param>
        /// <param name="source"></param>
        /// <param name="isDuplicate"></param>
        /// <returns></returns>
        /// 

          //Added as part of ROI conncetion string management
        //begin
        
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Join()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
            }
        }
        //end
        public String AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture)
        {
            string sresultXml = string.Empty;
            try
            {
                string PendingCulture = ConfigurationManager.AppSettings["PendingCustCulture"].ToString();
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Join.AccountCreate");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Join.AccountCreate - objectXml :" + objectXml.ToString());
                DataSet ds = null;
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "customer");

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
                this.DaytimePhoneNumber = (string)htblCustomer["DaytimePhoneNumber"];
                this.EveningPhoneNumber = (string)htblCustomer["EveningPhoneNumber"];
                this.MobilePhoneNumber = (string)htblCustomer["MobilePhoneNumber"];
                this.EmailAddress = (string)htblCustomer["EmailAddress"];

                if (htblCustomer["number_of_household_members"] != null) this.NoOfHouseHold = Convert.ToInt16(htblCustomer["number_of_household_members"].ToString());
                if (htblCustomer["family_member_1_dob"] != null) this.DateOfBirth1 = Convert.ToInt16(htblCustomer["family_member_1_dob"].ToString());
                if (htblCustomer["family_member_2_dob"] != null) this.DateOfBirth2 = Convert.ToInt16(htblCustomer["family_member_2_dob"].ToString());
                if (htblCustomer["family_member_3_dob"] != null) this.DateOfBirth3 = Convert.ToInt16(htblCustomer["family_member_3_dob"].ToString());
                if (htblCustomer["family_member_4_dob"] != null) this.DateOfBirth4 = Convert.ToInt16(htblCustomer["family_member_4_dob"].ToString());
                if (htblCustomer["family_member_5_dob"] != null) this.DateOfBirth5 = Convert.ToInt16(htblCustomer["family_member_5_dob"].ToString());

                if (htblCustomer["IsDiabetic"] != null) this.DiabeticFlag = (string)(htblCustomer["IsDiabetic"].ToString());
                if (htblCustomer["IsTeeTotal"] != null) this.TeetotalFlag = (string)(htblCustomer["IsTeeTotal"].ToString());
                if (htblCustomer["IsVegetarian"] != null) this.VegetarianFlag = (string)(htblCustomer["IsVegetarian"].ToString());
                if (htblCustomer["IsHalal"] != null) this.HalalFlag = (string)(htblCustomer["IsHalal"].ToString());
                if (htblCustomer["IsKosher"] != null) this.KosherFlag = (string)(htblCustomer["IsKosher"].ToString());
                if (htblCustomer["IsCoeliac"] != null) this.CeliacFlag = (string)(htblCustomer["IsCoeliac"].ToString());
                //Dataprotection matrix parameters for UK

                if (htblCustomer["WantTescoInfo"] != null) this.Option13 = (string)(htblCustomer["WantTescoInfo"].ToString());
                if (htblCustomer["WantPartnerInfo"] != null) this.Option14 = (string)(htblCustomer["WantPartnerInfo"].ToString());
                if (htblCustomer["IsResearchContactable"] != null) this.Option15 = (string)(htblCustomer["IsResearchContactable"].ToString());
                
                //Dataprotection matrix parameters for Group

                if (htblCustomer["TescoGroupMail"] != null) this.Option1 = (string)(htblCustomer["TescoGroupMail"].ToString());
                if (htblCustomer["TescoGroupEmail"] != null) this.Option2 = (string)(htblCustomer["TescoGroupEmail"].ToString());
                if (htblCustomer["TescoGroupPhone"] != null) this.Option3 = (string)(htblCustomer["TescoGroupPhone"].ToString());
                if (htblCustomer["TescoGroupSMS"] != null) this.Option4 = (string)(htblCustomer["TescoGroupSMS"].ToString());

                if (htblCustomer["PartnerMail"] != null) this.Option5 = (string)(htblCustomer["PartnerMail"].ToString());
                if (htblCustomer["PartnerEmail"] != null) this.Option6 = (string)(htblCustomer["PartnerEmail"].ToString());
                if (htblCustomer["PartnerPhone"] != null) this.Option7 = (string)(htblCustomer["PartnerPhone"].ToString());
                if (htblCustomer["PartnerSMS"] != null) this.Option8 = (string)(htblCustomer["PartnerSMS"].ToString());

                if (htblCustomer["ResearchMail"] != null) this.Option9 = (string)(htblCustomer["ResearchMail"].ToString());
                if (htblCustomer["ResearchEmail"] != null) this.Option10 = (string)(htblCustomer["ResearchEmail"].ToString());
                if (htblCustomer["ResearchPhone"] != null) this.Option11 = (string)(htblCustomer["ResearchPhone"].ToString());
                if (htblCustomer["ResearchSMS"] != null) this.Option12 = (string)(htblCustomer["ResearchSMS"].ToString());

                if (htblCustomer["PromotionCode"] != null) this.promotionCode = (string)htblCustomer["PromotionCode"];
                if (htblCustomer["Expat"] != null) this.Expat = (string)htblCustomer["Expat"];
                if (htblCustomer["SSN"] != null) this.SSN = (string)htblCustomer["SSN"];
                if (htblCustomer["PassportNumber"] != null) this.Passport = (string)htblCustomer["PassportNumber"];
                if (htblCustomer["JoinStore"] != null) this.JoinedStoreID = (string)((htblCustomer["JoinStore"].ToString()));
                if (htblCustomer["Language"] != null) this.Language = (string)((htblCustomer["Language"].ToString()));
                if (htblCustomer["DynamicPreferences"] != null)
                {
                    this.DynamicPreferences = (string)(htblCustomer["DynamicPreferences"].ToString());
                }
                if (htblCustomer["Race"] != null) this.Race = (string)((htblCustomer["Race"].ToString()));
                if (htblCustomer["CustomerUseStatusMain"] != null) this.CustomerUseStatusID = Convert.ToInt16(htblCustomer["CustomerUseStatusMain"]);
                if (htblCustomer["CustomerMailStatus"] != null) this.CustomerMailStatus = Convert.ToInt16(htblCustomer["CustomerMailStatus"]);
                if (htblCustomer["CustomerMobilePhoneStatus"] != null) this.CustomerMobilePhoneStatus = Convert.ToInt16(htblCustomer["CustomerMobilePhoneStatus"]);
                if (htblCustomer["CustomerEmailStatus"] != null) this.CustomerEmailStatus = Convert.ToInt16(htblCustomer["CustomerEmailStatus"]);
                
                string sProcedure = string.Empty;
                if (culture.ToUpper().ToString().Trim() == PendingCulture.ToUpper().ToString().Trim())
                {
                    
                    object[] objDBParams = {dotcomCustomerID, TitleEnglish, Name1, Name2, Name3,
                                           htblCustomer[Constants.CUSTOMER_DOB] != null ? DateOfBirth :null , Sex,
                                           MailingAddressLine1, MailingAddressLine2,MailingAddressLine3,MailingAddressLine4,
                                           MailingAddressLine5, MailingAddressLine6, MailingAddressPostCode, 
                                           DiabeticFlag, VegetarianFlag, TeetotalFlag, KosherFlag, HalalFlag,CeliacFlag, 
                                           Option13, Option14, Option15, null,
                                           EmailAddress, DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber, 
                                           NoOfHouseHold, DateOfBirth1, DateOfBirth2, DateOfBirth3, DateOfBirth4, DateOfBirth5, source, 'N', 0, Expat, promotionCode, SSN, JoinedStoreID };
                    sProcedure = Constants.SP_CREATE_PENDINGCUSTOMER;
                    ds = SqlHelper.ExecuteDataset(connectionString, sProcedure, objDBParams);
                    ds.Tables[0].TableName = "Clubcard";
                    sresultXml = ds.GetXml();
                }
                else
                {
                    object[] objDBParams = {dotcomCustomerID, TitleEnglish, Name1, Name2, Name3,
                                           htblCustomer[Constants.CUSTOMER_DOB] != null ? DateOfBirth :null , Sex,
                                           MailingAddressLine1, MailingAddressLine2,MailingAddressLine3,MailingAddressLine4,
                                           MailingAddressLine5, MailingAddressLine6, MailingAddressPostCode, 
                                           DiabeticFlag, VegetarianFlag, TeetotalFlag, KosherFlag, HalalFlag,
                                           Option1, Option2, Option3,Option4,Option5,Option6,Option7,Option8,Option9,Option10,Option11,Option12,Race,Language, null,
                                           EmailAddress, DaytimePhoneNumber, EveningPhoneNumber, MobilePhoneNumber, 
                                           NoOfHouseHold, DateOfBirth1, DateOfBirth2, DateOfBirth3, DateOfBirth4, DateOfBirth5, source, 'N', 0, Expat, promotionCode, SSN,Passport, JoinedStoreID,DynamicPreferences,
                                           CustomerUseStatusID,CustomerMailStatus,CustomerMobilePhoneStatus,CustomerEmailStatus };
                    sProcedure = Constants.SP_CREATE_CUSTOMER;
                    ds = SqlHelper.ExecuteDataset(connectionString, sProcedure, objDBParams);
                    ds.Tables[0].TableName = "Clubcard";
                    sresultXml = ds.GetXml();
                }

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Join.AccountCreate");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Join.AccountCreate - sresultXml :" + sresultXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Join.AccountCreate - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Join.AccountCreate - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Join.AccountCreate");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }

            return sresultXml;
        }

//join enhancement
        public bool AccountDuplicateCheck(string inputXml, out string resultXml)
        {
            bool result = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Join.AccountDuplicateCheck");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Join.AccountDuplicateCheck - InputXml :" + inputXml.ToString());
                DataSet ds = null;
                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(inputXml, "customer");


                if (htblCustomer["Name1"] != null) this.Name1 = (string)htblCustomer["Name1"];
                if (htblCustomer["Name2"] != null) this.Name2 = (string)htblCustomer["Name2"];
                if (htblCustomer["Name3"] != null) this.Name3 = (string)htblCustomer["Name3"];
                if (htblCustomer["MailingAddressLine1"] != null) this.MailingAddressLine1 = (string)htblCustomer["MailingAddressLine1"];
                if (htblCustomer["MailingAddressPostCode"] != null) this.MailingAddressPostCode = (string)htblCustomer["MailingAddressPostCode"];
                if (htblCustomer["MobilePhoneNumber"] != null) this.MobilePhoneNumber = (string)htblCustomer["MobilePhoneNumber"];
                if (htblCustomer["EmailAddress"] != null) this.EmailAddress = (string)htblCustomer["EmailAddress"];
                if (htblCustomer["PromotionCode"] != null) this.promotionCode = (string)htblCustomer["PromotionCode"];
                if (htblCustomer["SSN"] != null) this.SSN = (string)htblCustomer["SSN"];
                if (htblCustomer["PassportNumber"] != null) this.passportNumber = (string)htblCustomer["PassportNumber"];
                if (htblCustomer["Culture"] != null) this.culture = (string)htblCustomer["Culture"];
                if (htblCustomer["CustomerID"] != null) this.customerID = Convert.ToInt64(htblCustomer["CustomerID"]);


                object[] objDBParams = {promotionCode,SSN,MobilePhoneNumber,EmailAddress,passportNumber,businessLineNumber,Name1, Name2, Name3,
                                           MailingAddressLine1,MailingAddressPostCode,culture,customerID};

                

                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_CheckDuplicate, objDBParams);
                ds.Tables[0].TableName = "Duplicate";
                resultXml = ds.GetXml();

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Join.AccountDuplicateCheck");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Join.AccountDuplicateCheck - resultXml :" + resultXml.ToString());
                result = true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Join.AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Join.AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Join.AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                result = false;
                throw ex;

            }
            return result;

        }

        #endregion
    }
}
