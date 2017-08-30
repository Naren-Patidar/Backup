using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using System.Configuration;
using Tesco.NGC.DataAccessLayer;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Tesco.NGC.BatchConsoleApplication
{
    public class RewardMailing
    {
        #region Fields
        short currentOfferID;
        private int commitSize;
        private string culture;
        private short isCurrentOffer;
        private int collPeridNumber;
        private DateTime collStartDate;
        private DateTime collEndDate;
        private DateTime currentDate = DateTime.Now;
        private int prevCollPerNum;
        private short fullMailingStatCode;
        private short voucherDefStatCode;
        private short rewardConvStatCode;
        private short previousOfferID;
        // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
        //public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
        string BcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string OutPutDirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string EventLog = "Error in the RewardMailing";
        string EventLogMsg = "RewardMailingMessage";
        string sBatch = "RewardMailing";
        string sFileName;
        bool retunValue;
        #endregion

        #region Output Parameters
        Int64 numberOfPrimaryCustomers;
        Int64 numberOfRewardMailings;
        Int64 numberOfNonRewardMailings;
        Int64 numberOfHighRewards;
        decimal amountRewardedAndMailedBalance;
        decimal amountRewardedAndHighBalance;
        Int64 numberOfInactiveCustomers;

        Int64 numberOfLeftSchemeCustomers;
        Int64 numberOfDeceasedCustomers;
        Int64 numberOfBannedCustomers;
        Int64 numberOfSkeletonCustomers;
        Int64 numberOfAddressInErrorCustomers;

        //Added to update the Optinal Customer Use Status--V3.1.1[ReqID:012]
        Int64 numberOfUnderageCustomers;
        Int64 numberOfUnsignedCustomers;
        Int64 numberofUnderageAndUnsignedCustomers;
        Int64 numberOfPossibleFraudCustomers;
        Int64 numberOfOtherStatusCustomers;
        Int64 numberOfOptionalStatus1Customers;
        Int64 numberOfOptionalStatus2Customers;
        Int64 numberOfOptionalStatus3Customers;
        Int64 numberOfOptionalStatus4Customers;
        Int64 numberOfOptionalStatus5Customers;
        Int64 numberOfInactiveCustomersInPeriod;
        Int64 totalVouchersIssued;
        decimal totalVoucherValue;
        Int64 totalCouponsIssued;
        Int64 customersSkipped;
        #endregion

        #region Properties

        public short PreviousOfferID
        {
            get { return previousOfferID; }
            set { previousOfferID = value; }
        }

        public int CommitSize
        {
            get { return commitSize; }
            set { commitSize = value; }
        }
        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }
        public short IsCurrOffer
        {
            get { return isCurrentOffer; }
            set { isCurrentOffer = value; }
        }
        public int CollPeriodNumber
        {
            get { return collPeridNumber; }
            set { collPeridNumber = value; }
        }
        public DateTime CollectionStartDate
        {
            get { return collStartDate; }
            set { collStartDate = value; }
        }
        public DateTime CollectionEndDate
        {
            get { return collEndDate; }
            set { collEndDate = value; }
        }
        public int PrevCollPeriodNumber
        {
            get { return prevCollPerNum; }
            set { prevCollPerNum = value; }
        }
        public short FullMailingStatusCode
        {
            get { return fullMailingStatCode; }
            set { fullMailingStatCode = value; }
        }
        public short VoucherDefinitionStatCode
        {
            get { return voucherDefStatCode; }
            set { voucherDefStatCode = value; }
        }
        public short RewardConversionStatCode
        {
            get { return rewardConvStatCode; }
            set { rewardConvStatCode = value; }
        }
        public short CurrentOfferID
        {
            get { return currentOfferID; }
            set { currentOfferID = value; }
        }
        #endregion

        #region Output Parameters Properties
        public Int64 NumberOfPrimaryCustomers
        {
            get { return numberOfPrimaryCustomers; }
            set { this.numberOfPrimaryCustomers = value; }

        }
        public Int64 NumberOfRewardMailngs
        {
            get { return numberOfRewardMailings; }
            set { this.numberOfRewardMailings = value; }

        }
        public Int64 NumberOfNonRewardMailing
        {
            get { return numberOfNonRewardMailings; }
            set { this.numberOfNonRewardMailings = value; }

        }

        public Int64 NumberOfHighRewards
        {
            get { return numberOfHighRewards; }
            set { this.numberOfHighRewards = value; }

        }
        public Int64 NumberOfInactiveCustomers
        {
            get { return numberOfInactiveCustomers; }
            set { this.numberOfInactiveCustomers = value; }

        }
        public Int64 NumberOfLeftSchemeCustomers
        {
            get { return numberOfLeftSchemeCustomers; }
            set { this.numberOfLeftSchemeCustomers = value; }

        }
        public Int64 NumberOfDeceasedCustomers
        {
            get { return numberOfDeceasedCustomers; }
            set { this.numberOfDeceasedCustomers = value; }

        }
        public Int64 NumberOfBannedCustomers
        {
            get { return numberOfBannedCustomers; }
            set { this.numberOfBannedCustomers = value; }

        }

        //Added by Syed Amjadulla on 15th July'2009 for NGC V 3.1.1 Req 009 
        public Int64 NumberOfUnderageCustomers
        {
            get { return numberOfUnderageCustomers; }
            set { this.numberOfUnderageCustomers = value; }

        }

        public Int64 NumberOfUnsignedCustomers
        {
            get { return numberOfUnsignedCustomers; }
            set { this.numberOfUnsignedCustomers = value; }

        }

        public Int64 NumberOfUnderageAndUnsignedCustomers
        {
            get { return numberofUnderageAndUnsignedCustomers; }
            set { this.numberofUnderageAndUnsignedCustomers = value; }

        }

        public Int64 NumberOfPossibleFraudCustomers
        {
            get { return numberOfPossibleFraudCustomers; }
            set { this.numberOfPossibleFraudCustomers = value; }

        }

        public Int64 NumberOfOtherStatusCustomers
        {
            get { return numberOfOtherStatusCustomers; }
            set { this.numberOfOtherStatusCustomers = value; }

        }

        public Int64 NumberOfOptionalStatus1Customers
        {
            get { return numberOfOptionalStatus1Customers; }
            set { this.numberOfOptionalStatus1Customers = value; }

        }

        public Int64 NumberOfOptionalStatus2Customers
        {
            get { return numberOfOptionalStatus2Customers; }
            set { this.numberOfOptionalStatus2Customers = value; }

        }

        public Int64 NumberOfOptionalStatus3Customers
        {
            get { return numberOfOptionalStatus3Customers; }
            set { this.numberOfOptionalStatus3Customers = value; }

        }

        public Int64 NumberOfOptionalStatus4Customers
        {
            get { return numberOfOptionalStatus4Customers; }
            set { this.numberOfOptionalStatus4Customers = value; }

        }


        public Int64 NumberOfOptionalStatus5Customers
        {
            get { return numberOfOptionalStatus5Customers; }
            set { this.numberOfOptionalStatus5Customers = value; }

        }

        public Int64 NumberOfSkeletonCustomers
        {
            get { return numberOfSkeletonCustomers; }
            set { this.numberOfSkeletonCustomers = value; }

        }
        public Int64 NumberOfAddressInErrorCustomers
        {
            get { return numberOfAddressInErrorCustomers; }
            set { this.numberOfAddressInErrorCustomers = value; }

        }

        public Int64 NumberOfInactiveCustomersInPeriod
        {
            get { return numberOfInactiveCustomersInPeriod; }
            set { this.numberOfInactiveCustomersInPeriod = value; }

        }
        public Int64 TotalVoucherIssued
        {
            get { return totalVouchersIssued; }
            set { this.totalVouchersIssued = value; }

        }

        public decimal AmountRewardedAndMailedBalance
        {
            get { return amountRewardedAndMailedBalance; }
            set { this.amountRewardedAndMailedBalance = value; }

        }
        public decimal AmountRewardedAndHighBalance
        {
            get { return amountRewardedAndHighBalance; }
            set { this.amountRewardedAndHighBalance = value; }

        }
        public decimal TotalVoucherValue
        {
            get { return totalVoucherValue; }
            set { this.totalVoucherValue = value; }

        }
        public Int64 TotalCouponsIssued
        {
            get { return totalCouponsIssued; }
            set { this.totalCouponsIssued = value; }

        }
        public Int64 CustomersSkipped
        {
            get { return customersSkipped; }
            set { this.customersSkipped = value; }

        }
        #endregion

        #region Methods
        public bool ExecuteSPs()
        {
            this.CommitSize = 1000;

            string message = "Reward Mailing Started...";
            sFileName = CommonFunctions.CreateLogFile(sBatch);
            Console.WriteLine(message);
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
            //Find Current Offer Period
            retunValue = FindCurrentOffer();
            if (retunValue != true)
                return false;

            //Find Previos Offer Period
            retunValue = FindPreviousOffer();
            if (retunValue != true)
                return false;
            string message0 = "Testing that preconditions are met ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message0);
            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message0, false);
            #region Is mailing already in progress?
            if (this.FullMailingStatusCode == 2)
            {
                string message1 = "Reward Mailing is already in progress.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message1);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message1, true);
                return false;
            }
            #endregion

            #region Is last period's full mailing status requested?
            if ((this.FullMailingStatusCode) != 5)
            {
                string message2 = "Reward Mailing not requested.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message2);
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message2, false);
                return false;
            }
            #endregion

            #region Are last period's vouchers generated and barcodes filled in?

            if ((this.VoucherDefinitionStatCode) != 3)
            {
                string message3 = "The last collection period's voucher types have not been generated, or all barcodes have not been supplied.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message3);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message3, true);
                return false;
            }
            #endregion

            #region Is last period's reward conversion completed?
            if ((this.RewardConversionStatCode) != 3)
            {
                string message4 = "Reward Calculation has not run, or has not successfully completed.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message4);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message4, true);
                return false;
            }
            #endregion

            string messag = "All Preconditions have been met, starting processing ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, messag);
            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, messag, false);

            #region Set last offer mailing status to "in progress"
            // Update SP to set the "in progress"
            UpdatePrevMailStatCode(PreviousOfferID, 2);
            #endregion

            #region Create data in VoucherReference table for High Reward Customers
            CreateHighRewardXml(PreviousOfferID);
            #endregion

            //Executing the USP_RewardMailingInitialize Stored Procedure by calling the CreateTable() method
            retunValue = CreateTable();
            if (retunValue != true) return false;
            //Executing the USP_RewardMailing Stored Procedure by calling the RewardMailings() method
            retunValue = RewardMailings(PreviousOfferID, CommitSize);
            if (retunValue != true) return false;
            //Executing the USP_RewardMailingExport Stored Procedure by calling the RewardMailingExport() method

            retunValue = RewardMailingExport(OutPutDirectory);
            if (retunValue != true) return false;
            retunValue = UpdatePrevOfferDetails(PreviousOfferID, 3, 1);
            if (retunValue != true)
                return false;
            else
            {
                if (CustomersSkipped == 0)
                {
                    object[] objUpdateOffer = 
                    {  
                        PreviousOfferID,
                        NumberOfPrimaryCustomers,
                        NumberOfHighRewards,
                        NumberOfRewardMailngs, 	                    	                     
                        NumberOfNonRewardMailing, 	                    
	                    NumberOfInactiveCustomers, 
	                    NumberOfLeftSchemeCustomers, 
	                    NumberOfDeceasedCustomers, 
	                    NumberOfBannedCustomers, 
                        NumberOfUnderageCustomers,
                        NumberOfUnsignedCustomers,
                        NumberOfUnderageAndUnsignedCustomers, 
	                    NumberOfPossibleFraudCustomers, 
	                    NumberOfOtherStatusCustomers, 
	                    NumberOfOptionalStatus1Customers, 
	                    NumberOfOptionalStatus2Customers, 
	                    NumberOfOptionalStatus3Customers, 
	                    NumberOfOptionalStatus4Customers, 
	                    NumberOfOptionalStatus5Customers, 
	                    NumberOfSkeletonCustomers, 
	                    NumberOfAddressInErrorCustomers, 
                        NumberOfInactiveCustomersInPeriod,
	                    TotalVoucherIssued, 	                  
	                    TotalCouponsIssued	               
                     };
                    int returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.UpdateOffer4Mailing, ref objUpdateOffer);

                }
                string msg = "Mailing files generated.";
                Console.WriteLine(msg);
                string message1 = "Reward Mailing Completed Successfully.";
                Console.WriteLine(message1);
                string sFileName1 = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName1, message1);
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message1, false);

            }
            if (retunValue != true) return false; // Added the condition for Call the second step of CalculateReard- USP_CalculateReawrdProcess2
            retunValue = CalculateRewardProcess2(PreviousOfferID, CommitSize);
            return true;
        }

        #region  Methods

        #region Update Prevous Mail Status Code
        public bool UpdatePrevMailStatCode(short PrevofferID, short fullMailingStatusCode)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.UpdatePrevMailStatCode");
            int retCode = 0;
            try
            {
                object[] objupdatepremailStatCode = { PrevofferID, fullMailingStatusCode };
                retCode = DataAccess.ExecuteNonQuery(connectionString, Constants.UpdatePrevOfferMailStatusCode, ref objupdatepremailStatCode);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in updateting the PreviousMailStatusCode. ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region Find the Previous offer period
        public bool FindPreviousOffer()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.FindPreviousOffer");
            DataSet ds = new DataSet();
            try
            {
                object[] objGetPreviousOffer = { };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.GetPreviousOffer, objGetPreviousOffer);

                ds.Tables[0].TableName = "PreviousOffer";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if ((ds.Tables[0].Rows[0]["OfferID"]) != DBNull.Value)
                        this.PreviousOfferID = Convert.ToInt16(ds.Tables[0].Rows[0]["OfferID"]);

                    if ((ds.Tables[0].Rows[0]["FullMailingStatusCode"]) != DBNull.Value)
                        this.FullMailingStatusCode = Convert.ToInt16(ds.Tables[0].Rows[0]["FullMailingStatusCode"]);
                    if ((ds.Tables[0].Rows[0]["VoucherDefinitionStatusCode"]) != DBNull.Value)
                        this.VoucherDefinitionStatCode = Convert.ToInt16(ds.Tables[0].Rows[0]["VoucherDefinitionStatusCode"]);
                    if ((ds.Tables[0].Rows[0]["RewardConversionStatus"]) != DBNull.Value)
                        this.RewardConversionStatCode = Convert.ToInt16(ds.Tables[0].Rows[0]["RewardConversionStatus"]);
                    return true;

                }
                else
                {
                    string message = "There is a Problem in Finding the Previous Offer Periodor There is no Previous collection period defined.";
                    string sFileName = CommonFunctions.CreateLogFile(sBatch);
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                    return false;
                }
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                return false;
            }
        }

        #endregion

        #region Create Tables
        //Create tables to buffer output 
        public bool CreateTable()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.CreateTable");
            int returnCode = 0;
            try
            {

                object[] objCreateTable = { };
                // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.RewardMailingInitialize, ref objCreateTable);
                return true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in Creating the Temperary table. ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }

        #endregion

        #region Create data in VoucherReference table for High Reward Customers
        public bool CreateHighRewardXml(short PrevofferID)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.CreateHighRewardXml");
            int retCode = 0;
            try
            {
                object[] CreateHighRewardXml = { PrevofferID };
                retCode = DataAccess.ExecuteNonQuery(connectionString, Constants.CreateHighRewardMailingXml, ref CreateHighRewardXml);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in creating data in VoucherReference table for High Reward Customers. ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region Reward RewardMailing
        //Assign vouchers and coupons to customers 
        public bool RewardMailings(int prevOfferID, int commitsize)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.Calc");
            int returnCode;

            //Added by Syed Amjadulla on 21st July 2009 for passing the parameter to Reward Mailing SP for Showing/Hiding Partner Details
            short ShowPartnerDetails = Convert.ToInt16(ConfigurationSettings.AppSettings["ShowPartnerDetailsInMail"]);
            try
            {
                //Modified by Syed Amjadulla to add additional output parameters for SP as per NGC V 3.1.1 Req 009
                object[] objRewardMailing = { 
                                                prevOfferID,
                                                commitsize,
                                                ShowPartnerDetails,
                                                NumberOfHighRewards,
                                                NumberOfRewardMailngs,
                                                NumberOfNonRewardMailing,      
                                                AmountRewardedAndHighBalance,
                                                AmountRewardedAndMailedBalance,                                                
                                                NumberOfInactiveCustomers,
                                                NumberOfLeftSchemeCustomers,
                                                NumberOfDeceasedCustomers,
                                                NumberOfBannedCustomers,
                                                NumberOfUnderageCustomers,
                                                NumberOfUnsignedCustomers,
                                                NumberOfUnderageAndUnsignedCustomers, 
	                                            NumberOfPossibleFraudCustomers, 
	                                            NumberOfOtherStatusCustomers, 
	                                            NumberOfOptionalStatus1Customers, 
	                                            NumberOfOptionalStatus2Customers, 
	                                            NumberOfOptionalStatus3Customers, 
	                                            NumberOfOptionalStatus4Customers, 
	                                            NumberOfOptionalStatus5Customers, 
                                                NumberOfSkeletonCustomers,
                                                NumberOfAddressInErrorCustomers,
                                                NumberOfInactiveCustomersInPeriod,
                                                TotalVoucherIssued,
                                                TotalVoucherValue,
                                                TotalCouponsIssued,
                                                CustomersSkipped,
                                                Convert.ToDecimal(ConfigurationManager.AppSettings["VATThresholdValue"].ToString())
                };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.RewardMailing, ref objRewardMailing);
                NumberOfHighRewards = (Int64)objRewardMailing[3];
                NumberOfRewardMailngs = (Int64)objRewardMailing[4];
                NumberOfNonRewardMailing = (Int64)objRewardMailing[5];
                AmountRewardedAndHighBalance = Convert.ToDecimal(objRewardMailing[6]);
                AmountRewardedAndMailedBalance = Convert.ToDecimal(objRewardMailing[7]);
                NumberOfInactiveCustomers = (Int64)objRewardMailing[8];
                NumberOfLeftSchemeCustomers = (Int64)objRewardMailing[9];
                NumberOfDeceasedCustomers = (Int64)objRewardMailing[10];
                NumberOfBannedCustomers = (Int64)objRewardMailing[11];
                NumberOfUnderageCustomers = (Int64)objRewardMailing[12];
                NumberOfUnsignedCustomers = (Int64)objRewardMailing[13];
                NumberOfUnderageAndUnsignedCustomers = (Int64)objRewardMailing[14];
                NumberOfPossibleFraudCustomers = (Int64)objRewardMailing[15];
                NumberOfOtherStatusCustomers = (Int64)objRewardMailing[16];
                NumberOfOptionalStatus1Customers = (Int64)objRewardMailing[17];
                NumberOfOptionalStatus2Customers = (Int64)objRewardMailing[18];
                NumberOfOptionalStatus3Customers = (Int64)objRewardMailing[19];
                NumberOfOptionalStatus4Customers = (Int64)objRewardMailing[20];
                NumberOfOptionalStatus5Customers = (Int64)objRewardMailing[21];
                NumberOfSkeletonCustomers = (Int64)objRewardMailing[22];
                NumberOfAddressInErrorCustomers = (Int64)objRewardMailing[23];
                NumberOfInactiveCustomersInPeriod = (Int64)objRewardMailing[24];
                TotalVoucherIssued = (Int64)objRewardMailing[25];
                TotalVoucherValue = Convert.ToDecimal(objRewardMailing[26]);
                TotalCouponsIssued = (Int64)objRewardMailing[27];
                CustomersSkipped = (Int64)objRewardMailing[28];
                Int64 TotalCustomers = NumberOfRewardMailngs + NumberOfHighRewards + NumberOfNonRewardMailing;
                NumberOfPrimaryCustomers = TotalCustomers + NumberOfInactiveCustomers + NumberOfLeftSchemeCustomers + NumberOfDeceasedCustomers +
                NumberOfBannedCustomers + NumberOfUnderageCustomers + NumberOfUnsignedCustomers + NumberOfUnderageAndUnsignedCustomers + NumberOfPossibleFraudCustomers +
                NumberOfOtherStatusCustomers + NumberOfOptionalStatus1Customers + NumberOfOptionalStatus2Customers + NumberOfOptionalStatus3Customers +
                NumberOfOptionalStatus4Customers + NumberOfOptionalStatus5Customers + NumberOfSkeletonCustomers + NumberOfAddressInErrorCustomers + NumberOfInactiveCustomersInPeriod;

                if (CustomersSkipped == 0)
                {

                    string message = "Reward Mailing processed " + TotalCustomers + " customers" + Environment.NewLine + TotalVoucherIssued + " vouchers rewarded with total value of " + TotalVoucherValue + Environment.NewLine + TotalCouponsIssued + " coupons rewarded." + Environment.NewLine + NumberOfHighRewards + " customers have high reward mailings." + Environment.NewLine + NumberOfRewardMailngs + " customers have reward mailings." + Environment.NewLine + NumberOfNonRewardMailing + " customers have non-reward mailings.";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
                }
                else
                {
                    string message = "Reward Mailing Completed." + Environment.NewLine + TotalVoucherIssued + " vouchers rewarded with total value of " + TotalVoucherValue + Environment.NewLine + TotalCouponsIssued + " coupons rewarded." + Environment.NewLine + NumberOfHighRewards + " customers have high reward mailings." + Environment.NewLine + NumberOfRewardMailngs + " customers have reward mailings." + Environment.NewLine + NumberOfNonRewardMailing + "customers have non-reward mailings.";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
                }

                return true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in Reward mailing process. ";
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region Export Mailing data to output files
        // Export Mailing data to output files
        public bool RewardMailingExport(string outputDirectory)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.RewardMailingExport");
            int returnCode = 0;
            try
            {
                object[] objCopyTempTable = { outputDirectory };
                // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.RewardMailingExport, ref objCopyTempTable);
                return true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in Exporting the Reward mailing. Reward mailing Export Failed";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }

        #endregion

        #region Update Previous Offer Details
        public bool UpdatePrevOfferDetails(short PrevofferID, short fullMailingStatusCode, short viewMailingReportStatusCode)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.UpdatePrevOffer");
            int retCode = 0;
            try
            {
                object[] objupdatepremailStatCode = { PrevofferID, fullMailingStatusCode, viewMailingReportStatusCode };
                retCode = DataAccess.ExecuteNonQuery(connectionString, Constants.UpdatePrevOfferDetails, ref objupdatepremailStatCode);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in updateting Previous offer details. ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region Find the current offer period
        public bool FindCurrentOffer()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("RewardMailing.FindCurrentOffer");
            int returnCode = 0;
            int rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                object[] objGetCurrentOffer = { };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.GetCurrentOffer, objGetCurrentOffer);
                ds.Tables[0].TableName = "CurrentOffer";
                this.CurrentOfferID = Convert.ToInt16(ds.Tables[0].Rows[0][0]);

                rowCount = returnCode;

                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a Problem in Finding the Current Offer Period.or There is no current collection period defined.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }

        #endregion


        #region Calcualte Reward Process2
        public bool CalculateRewardProcess2(int prevOfferID, int commitsize)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CalculateReward.Process2");
            int returnCode = 0;
            try
            {
                //Modified by Syed Amjadulla to add additional output parameters for SP as per NGC V 3.1.1 Req 009
                object[] objCalculateRewardProcess2 = { 
                                                prevOfferID,
                                                commitsize
                                            };
                Console.WriteLine("Updating brought forward points started ...");
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.CalculareRewardsProcess2, ref objCalculateRewardProcess2);
                Console.WriteLine("Updating brought forward points completed.");
                return true;
            }

            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in CalculateReward synchronization process. ";
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }

        }


        #endregion

        #endregion
        #endregion
    }
}
