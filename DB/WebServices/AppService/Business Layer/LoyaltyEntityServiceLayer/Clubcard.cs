/*
 * File   : Clubcard.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Clubcard
 * Date   : 06/Aug/2008
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

//logger for CCO application
using Microsoft.Practices.EnterpriseLibrary.Logging;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    /// <summary>
    /// Tesco Clubcard Details
    /// /// </summary>
    public class Clubcard
    {

        #region Fields

        /// <summary>
        /// ClubcardID
        /// </summary>
        private Int64 clubcardID;

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int64 customerID;

        /// <summary>
        /// ClubcardType
        /// </summary>
        private Int16 clubcardType;

        /// <summary>
        /// ClubcardStatus
        /// </summary>
        private Int32 clubcardStatus;

        /// <summary>
        /// PrimaryClubcardID
        /// </summary>
        private Int32 primaryClubcardID;

        /// <summary>
        /// CardIssuedDate
        /// </summary>
        private DateTime cardIssuedDate;

        private string nameAsInNRIC;

        private string sex;

        private DateTime dateOfBirth;

        #region Transaction Fields

        int transactionReasonID;
        decimal amountSpent;
        int tescoStoreID;
        int sourcePOSID;
       // long clubcardID;
        int partnerOutletID;
        string cashierID;
        int skuPointsQty;
        int welcomePointsQty;
        int manualPointsQty;
        int greenPointsQty;
        int normalPointsQty;
        int sourceTransactionID;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        ///  ClubcardID
        /// </summary>
        public Int64 ClubcardID
        {
            get{return this.clubcardID;}
            set{this.clubcardID = value;}
        }

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int64 CustomerID
        {
            get{return this.customerID;}
            set{this.customerID = value;}
        }

        /// <summary>
        ///  ClubcardType
        /// </summary>
        public Int16 ClubcardType
        {
            get{return this.clubcardType;}
            set{this.clubcardType = value;}
        }

        /// <summary>
        ///  ClubcardStatus
        /// </summary>
        public Int32 ClubcardStatus
        {
            get{return this.clubcardStatus;}
            set{this.clubcardStatus = value;}
        }

        /// <summary>
        ///  PrimaryClubcardID
        /// </summary>
        public Int32 PrimaryClubcardID
        {
            get{return this.primaryClubcardID;}
            set{this.primaryClubcardID = value;}
        }

        /// <summary>
        ///  CardIssuedDate
        /// </summary>
        public DateTime CardIssuedDate
        {
            get{return this.cardIssuedDate;}
            set{this.cardIssuedDate = value;}
        }
        /// <summary>
        ///  Name As In NRIC
        /// </summary>
        public string NameAsInNRIC
        {
            get { return this.nameAsInNRIC; }
            set { this.nameAsInNRIC = value; }
        }
        /// <summary>
        ///  Gender
        /// </summary>
        public string Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }

        /// <summary>
        ///  Date Of Birth
        /// </summary>
        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }
            set { this.dateOfBirth = value; }

        }

        #region Properties
        public int TransactionReasonID
        {
            get { return transactionReasonID; }
            set { transactionReasonID = value; }
        }
        public decimal AmountSpent
        {
            get { return amountSpent; }
            set { amountSpent = value; }
        }
        public int TescoStoreID
        {
            get { return tescoStoreID; }
            set { tescoStoreID = value; }
        }
        public int SourcePOSID
        {
            get { return sourcePOSID; }
            set { sourcePOSID = value; }
        }
        //public long ClubcardID
        //{
        //    get { return clubcardID; }
        //    set { clubcardID = value; }
        //}
        public int PartnerOutletID
        {
            get { return partnerOutletID; }
            set { partnerOutletID = value; }
        }
        public string CashierID
        {
            get { return cashierID; }
            set { cashierID = value; }
        }
        public int SKUPointsQty
        {
            get { return skuPointsQty; }
            set { skuPointsQty = value; }
        }
        public int WelcomePointsQty
        {
            get { return welcomePointsQty; }
            set { welcomePointsQty = value; }
        }
        public int ManualPointsQty
        {
            get { return manualPointsQty; }
            set { manualPointsQty = value; }
        }
        public int GreenPointsQty
        {
            get { return greenPointsQty; }
            set { greenPointsQty = value; }
        }
        public int NormalPointsQty
        {
            get { return normalPointsQty; }
            set { normalPointsQty = value; }
        }
        public int SourceTransactionID
        {
            get { return sourceTransactionID; }
            set { sourceTransactionID = value; }
        }

        #endregion

        #endregion 

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Clubcard()
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

        #region Methods

        #region View
        /// <summary>
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String View(Int64 customerID, string culture)
        {
                     
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.View - customerID :" + customerID);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CLUBCARD_DETAILS, customerID, culture);
                ds.Tables[0].TableName = "ClubcardDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                
            }
            return viewXml;
        }


        #endregion

        #region Get Household Customers
        /// <summary>
        /// Added as part of CCO Convergence- 4/11/2010
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String ViewHouseholdCustomers(Int64 customerID, string culture)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers - customerID :" + customerID);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_HOUSEHOLD_CUSTOMERS, customerID);
                ds.Tables[0].TableName = "HouseholdCustomers";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ViewHouseholdCustomers");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  
            }
            finally
            {
               
            }
            return viewXml;
        }

        #endregion

        #region View Clubcards
        /// <summary>
        /// Added as part of CCO Convergence- 4/11/2010
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String ViewClubcards(Int64 customerID, string culture)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ViewClubcards");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ViewClubcards - customerID :" + customerID);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CLUBCARDS, customerID);
                ds.Tables[0].TableName = "ClubcardDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ViewClubcards");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ViewClubcards - viewXml :" + viewXml.ToString());
               
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ViewClubcards - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ViewClubcards - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ViewClubcards");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                throw ex;
            }
            finally
            {
            }
            return viewXml;
        }

        #endregion

        #region View Clubcards Customer
        /// <summary>
        /// To get (view) the ClubCard details based on the given CustomerID
        /// </summary>
        public String ViewClubcardsCustomer(Int64 customerID, string culture)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer - customerID :" + customerID);
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CLUBCARDS_CUSTOMER, customerID);
                ds.Tables[0].TableName = "ClubcardDetails";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer - viewXml :" + viewXml.ToString());
                

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ViewClubcardsCustomer");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                throw ex;
            }
            finally
            {
                
            }
            return viewXml;
        }

        #endregion


        #region ADD PRIMARY CLUBCARD
        /// <summary>
        /// To Add the Primary ClubCard details to the database
        /// </summary>

        public bool AddPrimaryCard(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.AddPrimaryCard");
                Hashtable htPCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htPCardDetails[Constants.CUSTOMER_NO]);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.AddPrimaryCard - customerID :" + this.CustomerID.ToString());
                this.ClubcardID = Convert.ToInt64(htPCardDetails[Constants.CLUBCARD_ID]);               

               
                object[] objAddClubcard = { userID, CustomerID, ClubcardID };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_PRIMARY_CLUBCARD, objAddClubcard);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.AddPrimaryCard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.AddPrimaryCard - customerID :" + this.CustomerID.ToString());
                
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.AddPrimaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.AddPrimaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.AddPrimaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag; 
            //return true;

        }
        #endregion

        #region CHANGE PRIMARY CLUBCARD
        /// <summary>
        /// To Add the Primary ClubCard details to the database
        /// </summary>

        public bool ChangePrimaryCard(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ChangePrimaryCard");
                
                Hashtable htPCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htPCardDetails[Constants.CUSTOMER_NO]);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ChangePrimaryCard - objectXml :" + objectXml.ToString());

                this.ClubcardID = Convert.ToInt64(htPCardDetails[Constants.CLUBCARD_ID]);

                
                object[] objAddCardType = { userID, CustomerID, ClubcardID };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_CHANGE_PRIMARY_CLUBCARD, objAddCardType);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ChangePrimaryCard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ChangePrimaryCard - customerID :" + this.CustomerID.ToString());
                
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ChangePrimaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ChangePrimaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ChangePrimaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag; 
            //return true;

        }
        #endregion

        #region ADD SUPPLEMENTARY CARD
        /// <summary>
        /// To Add the ClubCard Type details to the database
        /// </summary>

        public bool AddSupplementaryCard(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.AddSupplementaryCard");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.AddSupplementaryCard - objectXml :" + objectXml.ToString());
                Hashtable htPCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htPCardDetails[Constants.CUSTOMER_NO]);
                
                this.ClubcardID = Convert.ToInt64(htPCardDetails["ClubcardNO"]);
                this.NameAsInNRIC = (string)htPCardDetails[Constants.NAME_AS_IN_NRIC];
                this.DateOfBirth = Convert.ToDateTime(htPCardDetails[Constants.CUST_DATE_OF_BIRTH]);
                //string Date = DateOfBirth.ToString("dd/MM/yyyy");
                string gender;
                this.Sex = (string)htPCardDetails[Constants.SEX];
                if (Sex.ToUpper() == "MALE")
                {
                    gender = "M";
                }
                else if (Sex.ToUpper() == "FEMALE")
                {
                    gender = "F";
                }
                else
                {
                    gender = "U";
                }
                
                object[] objAddClubcard = { userID, CustomerID, ClubcardID, NameAsInNRIC, DateOfBirth, gender };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_SUPPLEMENTARY_CLUBCARD, objAddClubcard);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.AddSupplementaryCard");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.AddSupplementaryCard - customerID :" + this.CustomerID.ToString());
               
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.AddSupplementaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.AddSupplementaryCard - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.AddSupplementaryCard");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;

        }
        #endregion

        #region UPDATE EDIT DETAILS
        /// <summary>
        /// To update CardType details 
        /// </summary>
        public bool UpdateEditDetails(string objectXml, int userID, out long objectId, out string resultXml)
        {

            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.UpdateEditDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.UpdateEditDetails - objectXml :" + objectXml.ToString());
                Hashtable htEditCardDetails = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htEditCardDetails[Constants.CUSTOMER_NO]);
                
                this.ClubcardID = Convert.ToInt64(htEditCardDetails[Constants.CLUBCARD_NO]);
                this.NameAsInNRIC = (string)htEditCardDetails[Constants.NAME_AS_IN_NRIC];
                this.DateOfBirth = Convert.ToDateTime(htEditCardDetails[Constants.CUST_DATE_OF_BIRTH]);
                //string Date = DateOfBirth.ToString("dd/MM/yyyy");
                string gender;
                this.Sex = (string)htEditCardDetails[Constants.SEX];
                if (Sex.ToUpper() == "MALE")
                {
                    gender = "M";
                }
                else if (Sex.ToUpper() == "FEMALE")
                {
                    gender = "F";
                }
                else
                {
                    gender = "U";
                }
                
                object[] objEditUpdate = { userID, CustomerID, ClubcardID, NameAsInNRIC, DateOfBirth, gender };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_SUPPLEMENTARY_CLUBCARD, objEditUpdate);
                objectId = this.ClubcardID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.UpdateEditDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.UpdateEditDetails - objectXml :" + objectXml.ToString());

            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.UpdateEditDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.UpdateEditDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.UpdateEditDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
               
            }
            return SqlHelper.result.Flag;
        }

        #endregion

        #region UPDATE EDIT STATUS
        /// <summary>
        /// To update CardType details 
        /// </summary>
        public bool UpdateEditStatus(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.UpdateEditStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.UpdateEditStatus - objectXml :" + objectXml.ToString());
                Hashtable htEditCardStatus = ConvertXmlHash.XMLToHashTable(objectXml, "Clubcard");
                this.CustomerID = Convert.ToInt64(htEditCardStatus[Constants.CUSTOMER_NO]);
               

                this.ClubcardID = Convert.ToInt64(htEditCardStatus[Constants.CLUBCARD_ID]);
                if (htEditCardStatus[Constants.CLUBCARD_STATUS] != null)
                {
                    this.ClubcardStatus = Convert.ToInt16(htEditCardStatus[Constants.CLUBCARD_STATUS]);
                }
                
                /*this.ClubcardStatus = (string)(htEditCardStatus[Constants.CLUBCARD_STATUS]);

                if (ClubcardStatus == "Normal")

                    CardStatus = int.Parse("0");

                else if (ClubcardStatus == "Lost")

                    CardStatus = int.Parse("1");

                else if (ClubcardStatus == "Damaged")

                    CardStatus = int.Parse("2");

                else if (ClubcardStatus == "Withdrawn")

                    CardStatus = int.Parse("3");

                else if (ClubcardStatus == "Expired")

                    CardStatus = int.Parse("4");

                else if (ClubcardStatus == "Stolen")

                    CardStatus = int.Parse("5");*/
                
                object[] objEditStatus = { CustomerID, ClubcardID, ClubcardStatus, userID };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARD_STATUS, objEditStatus);
                objectId = this.ClubcardID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.UpdateEditStatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.UpdateEditStatus");
                
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.UpdateEditStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.UpdateEditStatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.UpdateEditStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
              
            }
            return SqlHelper.result.Flag;
        }

        #endregion

        #endregion

        #region Transaction Methods

        #region View Transaction for Add Transaction of CSD
        /// <summary>
        /// To get the details of Transaction
        /// </summary>       
        /// <returns>Transaction details in xml format</returns>
        public String ViewTransaction(long txnReasonID, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ViewTransaction");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ViewTransaction - txnReasonID :" + txnReasonID.ToString());
                object[] objTxnReasonView = { txnReasonID, culture };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_TRANSACTIONREASON, objTxnReasonView);
                ds.Tables[0].TableName = "Transaction";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ViewTransaction");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ViewTransaction - viewXml :" + viewXml.ToString());
                

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ViewTransaction - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ViewTransaction - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ViewTransaction");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
            }
            finally
            {
               
            }
            return viewXml;
        }
        #endregion

        #region Add Transaction
        #region Add
        /// <summary>
        /// Add a new partner
        /// </summary>
        /// <param name="objectXml">Partner details</param>/// 
        /// <returns>PartnerID of the new Partner</returns>
        public bool AddTransaction(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.AddTransaction");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.AddTransaction - objectXml :" + objectXml.ToString());
                Hashtable htblTransaction = ConvertXmlHash.XMLToHashTable(objectXml, "Transaction");

                this.TransactionReasonID = Convert.ToInt32(htblTransaction[Constants.TXN_REASON]);
               

                if (htblTransaction[Constants.SALES_AMOUNT] != null)
                {
                    if (htblTransaction[Constants.SALES_AMOUNT].ToString() != "")
                    {
                        this.AmountSpent = Convert.ToDecimal(htblTransaction[Constants.SALES_AMOUNT]);
                    }
                }
                this.TescoStoreID = Convert.ToInt32(htblTransaction[Constants.STORE]);
                this.SourcePOSID = Convert.ToInt32(htblTransaction[Constants.POS_ID]);
                this.ClubcardID = Convert.ToInt64(htblTransaction[Constants.CLUBCARD_ID]);
                //this.PartnerOutletID = Convert.ToInt32(htblTransaction[Constants]);
                //this.CashierID = (string)htblTransaction[Constants];
                if (htblTransaction[Constants.PRODUCT_POINTS] != null)
                {
                    if (htblTransaction[Constants.PRODUCT_POINTS].ToString() != "")
                    {
                        this.SKUPointsQty = Convert.ToInt32(htblTransaction[Constants.PRODUCT_POINTS]);
                    }
                }
                //string tempProductPoints = (string)(htblTransaction[Constants.PRODUCT_POINTS]);
                //if (StringUtils.IsStringEmpty(tempProductPoints))
                //{
                //    this.SKUPointsQty = 0;
                //}
                //else                
                //this.SKUPointsQty = Convert.ToInt32(htblTransaction[Constants.PRODUCT_POINTS]);                

                if (htblTransaction[Constants.WELCOME_POINTS] != null)
                {
                    if (htblTransaction[Constants.WELCOME_POINTS].ToString() != "")
                    {
                        this.WelcomePointsQty = Convert.ToInt32(htblTransaction[Constants.WELCOME_POINTS]);
                    }
                }
                //string tempWelcomePoints = (string)htblTransaction[Constants.WELCOME_POINTS];
                //if (StringUtils.IsStringEmpty(tempWelcomePoints))
                //{
                //    this.WelcomePointsQty = 0;
                //}
                //else
                //this.WelcomePointsQty = Convert.ToInt32(htblTransaction[Constants.WELCOME_POINTS]);

                if (htblTransaction[Constants.EXTRA_POINTS1] != null)
                {
                    if (htblTransaction[Constants.EXTRA_POINTS1].ToString() != "")
                    {
                        this.ManualPointsQty = Convert.ToInt32(htblTransaction[Constants.EXTRA_POINTS1]);
                    }
                }
                //string tempExtraPoints1 = (string)htblTransaction[Constants.EXTRA_POINTS1];
                //if (StringUtils.IsStringEmpty(tempExtraPoints1))
                //{
                //    this.ManualPointsQty = 0;
                //}
                //else
                //this.ManualPointsQty = Convert.ToInt32(htblTransaction[Constants.EXTRA_POINTS1]);
                //this.GreenPointsQty = Convert.ToInt32(htblTransaction[Constants]);

                if (htblTransaction[Constants.STANDARD_POINTS] != null)
                {
                    if (htblTransaction[Constants.STANDARD_POINTS].ToString() != "")
                    {
                        this.NormalPointsQty = Convert.ToInt32(htblTransaction[Constants.STANDARD_POINTS]);
                    }
                }
                //this.NormalPointsQty = Convert.ToInt32(htblTransaction[Constants.STANDARD_POINTS]);
                this.SourceTransactionID = Convert.ToInt32(htblTransaction[Constants.SOURCE_SYSTEM_TRANSACTION_ID]);

                object[] objAddTransaction = {
                                                 ClubcardID,
                                                 TescoStoreID,
                                                 DateTime.Now,
                                                 AmountSpent,
                                                 TransactionReasonID,
                                                 WelcomePointsQty,
                                                 SKUPointsQty,
                                                 ManualPointsQty,
                                                 0,
                                                 SourcePOSID,
                                                 SourceTransactionID,
                                                 userID,
                                                 9,
                                                 NormalPointsQty
                                             };
                
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_TRANSACTION, objAddTransaction);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.AddTransaction");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.AddTransaction");
                
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.AddTransaction - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.AddTransaction - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.AddTransaction");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                return false;
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;

        }
        #endregion
        #endregion


        #endregion

        #region Reissue Request
        public bool ReissueRequest(long ClubcardNumber, string userName)
        {
          
            
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.ReissueRequest");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.ReissueRequest - ClubcardNumber :" + ClubcardNumber.ToString());
                
                object[] objDBParams = { ClubcardNumber, userName };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_REISSUEREQUEST, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.ReissueRequest");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.ReissueRequest - ClubcardNumber :" + ClubcardNumber.ToString());
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.ReissueRequest - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.ReissueRequest - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.ReissueRequest");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
                throw ex;
            }
            finally
            {
               
            }

            return SqlHelper.result.Flag;
        }
        #endregion

        #region Rollover Request
        public string RollOverRequest(long ClubcardNumber, Decimal AmountSpent, int CollectionPeriodNumber)
        {
            
            string errorReason = "";
           
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.RollOverRequest");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.RollOverRequest - ClubcardNumber :" + ClubcardNumber.ToString());
                
                object[] objDBParams = { ClubcardNumber, AmountSpent, CollectionPeriodNumber, errorReason };
                int returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_ROLLOVERREQUEST, ref objDBParams);

                Int32 i = objDBParams.Length - 1;
                errorReason = Convert.ToString(objDBParams[i]);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.RollOverRequest");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.RollOverRequest - ClubcardNumber :" + ClubcardNumber.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.RollOverRequest - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.RollOverRequest - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.RollOverRequest");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorReason = ex.Message;
               
                throw ex;
            }
            finally
            {
                
            }

            return errorReason;
        }
        #endregion

        #region GetLatestStatementDetails

        /// <summary>
        /// Added as part of NGC3.6
        /// To get (view) the Customer Statement details based on the given CustomerID
        /// </summary>
        public String GetLatestStatementDetails(Int64 customerID, string culture)
        {
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Clubcard.GetLatestStatementDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Clubcard.GetLatestStatementDetails - customerID :" + customerID);


                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETLATESTSTATEMENT, customerID);
                ds.Tables[0].TableName = "LatestStatement";
                viewXml = ds.GetXml();


                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Clubcard.GetLatestStatementDetails");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Clubcard.GetLatestStatementDetails - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Clubcard.GetLatestStatementDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Clubcard.GetLatestStatementDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Clubcard.GetLatestStatementDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;
        }
        #endregion

    }
}
