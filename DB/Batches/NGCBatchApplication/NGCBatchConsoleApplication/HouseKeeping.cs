using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
//using Tesco.NGC.
using Tesco.NGC.DataAccessLayer;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;

namespace Tesco.NGC.BatchConsoleApplication
{
    public class HouseKeeping
    {
        

        #region Fileds
        int auditRetentionperiod =0;
        int transRetention = 0;
        int offerRetention = 910;
        //int minAuditRet = 0;
        //int minTransRet = 182;
        int minOffRet = 910;
       private string writeLineXml;
       private int partitionCrmID;
       private string tableName;
       private int offerID;
       private int commitSize;
       private string bcpDirectory;
       private string outputDirectory;
       private string filename;
       private int opt_O;
       private DateTime currentstartDate;
       private DateTime currentendDate;
       private int tableID;
       private int ncutomerRecords;
       private int ntxnRecords;
       private int ncustomerOfferRecords;

       //int ncustomerRecords;
       int ntxnVoucherRecords;       
       int offerVoucherRecords;

       int nRemovedVoucherTypeRecords;
       int nRemovedCouponTypeRecords;
       int nRemovedFinancialDayRecords;
       int nRemovedStoreDayRecords;


       string BcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
       string OutPutDirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
       string fileName = ConfigurationSettings.AppSettings["fileName"];
       public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

       string EventLog = "Error in the HouseKeeping";
       string EventLogMsg = "HouseKeeping Message";
       string sBatch = "HouseKeeping";
       string sFileName;
       bool retunValue;
       short flag = 0;

        #endregion 

       #region Current Offer Fields
       string offerName;
       DateTime rewardConversionDate;
       DateTime mainMailDate ;
       DateTime allowReissueDate;
       //DateTime offer_expiry_date;
       DateTime mailingApprovedDate;
       int rewardPointsThreshold;
       decimal amountToPointsConversionRate;
       decimal maximumVoucherValue;
       decimal minimumVoucherValue;
       decimal voucherValueStep;
       int numberOfVouchers;
       decimal pointsConvertedToRewardsBalance;
       int totalVoucherIssued;
       int numberOfBannedCustomers;
       int numberOfInactiveCustomers;
       int numberOfDeceasedCustomers;
       int numberOfLeftSchemeCustomers;
      // int numberOfLeftSchemeCustomers;
       int numberOfAddressInErrorCustomers;
       decimal offerProductPointsBalance;
       int numberOfRewardMailngs;
       int offerWelcomePointsQty;
       int numberOfSkeletonCustomers;
       int numberOfNonRewardMailing;
       short rewardConversionStatus;
       short rewardReconversionStatus;
       Decimal pointsToRewardConversionRate;

       #endregion

       #region Previous offer Details
       short prevOfferId;
       string prevOfferName;                 	
        decimal prevMaximumVoucherValue ;
        decimal prevMinimumVoucherValue;
        int prevNumberOfVouchers;                 
        int prevOfferWelcomePointsQty;
        decimal prevAmountToPointsConversionRate;                                           	                
        DateTime prevAllowReissueDate; 
        //DateTime  prevRewardConversionDate;
        string prevRewardConversionDate;
        //DateTime prevMailingApprovedDate; 
        string prevMailingApprovedDate;
        int prevTotalVoucherIssued ;
        //DateTime prevMainMailDate;
        string prevMainMailDate;
        int prevNumberOfRewardMailngs;
        int prevNumberOfNonRewardMailing;

        int prevNumberOfBannedCustomers;                 
        int prevNumberOfInactiveCustomers;                 
        int prevNumberOfDeceasedCustomers;    
             
        int prevNumberOfLeftSchemeCustomers;                 
        int prevnumberOfSkeletonCustomers;                
        int prevNumberOfAddressInErrorCustomers;
        decimal prevOfferProductPointsBalance;                 
        decimal prevPointsConvertedToRewardsBalance;
        decimal prevVoucherValueStep;
        int prevRewardPointsThreshold;
        DateTime prevStartDateTime;
        DateTime prevEndDateTime;
        decimal prevpointsToRewardConversionRate;
        short prevrewardConversionStatus;

       #endregion
        #region Properties
       public int AuditRetentionPeriod
        {
            get { return auditRetentionperiod; }
            set { auditRetentionperiod = value; }
        }
        public int TransactionRetention
        {
            get { return transRetention; }
            set { transRetention = value; }
        }
        public int OfferRetention
        {
            get { return offerRetention; }
            set { offerRetention = value; }
        }
        public string WriteLineXml
        {
            get { return writeLineXml; }
            set { writeLineXml = value; }
        }
        public int PartionCrmID
        {
            get { return partitionCrmID; }
            set { partitionCrmID = value; }
        }
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        public int OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }
        public int CommitSize
        {
            get { return commitSize; }
            set { commitSize = value; }
        }
        public string BCPDirectory
        {
            get { return bcpDirectory; }
            set { bcpDirectory = value; }
        }
        //public string OutputDirectory
        //{
        //    get { return outputDirectory; }
        //    set { outputDirectory = value; }
        //}
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        public int Opt_O
        {
            get { return opt_O; }
            set { opt_O = value; }
        }
        public DateTime  CurrentSatrtDate
        {
            get { return currentstartDate; }
            set { currentstartDate = value; }
        }
        public DateTime CurrentEndDate
        {
            get { return currentendDate; }
            set { currentendDate = value; }
        }
        public int TableID
        {
            get { return tableID; }
            set { tableID = value; }
        }
        public int nCustomerRecords
        {
            get { return ncutomerRecords; }
            set { ncutomerRecords = value; }
        }
        public int nTxnRecords
        {
            get { return ntxnRecords; }
            set { ntxnRecords = value; }
        }
        public int nCustomerOfferRecords
        {
            get { return ncustomerOfferRecords; }
            set { ncustomerOfferRecords = value; }
        }
        public int nTxnVoucherRecords
        {
            get { return ntxnVoucherRecords; }
            set { ntxnVoucherRecords = value; }
        }
        #endregion

        #region Current Offer Data Properties

        public string OfferName 
        {
            get { return offerName; }
            set { offerName = value; }
        }
        public DateTime RewardConversionDate 
        {
            get { return rewardConversionDate; }
            set { rewardConversionDate = value; }
        }
        public DateTime MainMailDate
        {
            get { return mainMailDate; }
            set { mainMailDate = value; }
        }
        public DateTime AllowReissueDate
        {
            get { return allowReissueDate; }
            set { allowReissueDate = value; }
        }
        public DateTime MailingApprovedDate 
        {
            get { return mailingApprovedDate; }
            set { mailingApprovedDate = value; }
        }
        public int RewardPointsThreshold
        {
            get { return rewardPointsThreshold; }
            set { rewardPointsThreshold = value; }
        }
        public decimal AmountToPointsConversionRate
        {
            get { return amountToPointsConversionRate; }
            set { amountToPointsConversionRate = value; }
        }
        public decimal MaximumVoucherValue
        {
            get { return maximumVoucherValue; }
            set { maximumVoucherValue = value; }
        }
        public decimal MinimumVoucherValue
        {
            get { return minimumVoucherValue; }
            set { minimumVoucherValue = value; }
        }
        public decimal VoucherValueStep
        {
            get { return voucherValueStep; }
            set { voucherValueStep = value; }
        }
        public int NumberOfVouchers
        {
            get { return numberOfVouchers; }
            set { numberOfVouchers = value; }
        }
        public decimal PointsConvertedToRewardsBalance
        {
            get { return pointsConvertedToRewardsBalance; }
            set { pointsConvertedToRewardsBalance = value; }
        }
        public int TotalVoucherIssued
        {
            get { return totalVoucherIssued; }
            set { totalVoucherIssued = value; }
        }
        public int NumberOfBannedCustomers
        {
            get { return numberOfBannedCustomers; }
            set { numberOfBannedCustomers = value; }
        }
        public int NumberOfInactiveCustomers
        {
            get { return numberOfInactiveCustomers; }
            set { numberOfInactiveCustomers = value; }
        }
        public int NumberOfDeceasedCustomers
        {
            get { return numberOfDeceasedCustomers; }
            set { numberOfDeceasedCustomers = value; }
        }
        public int NumberOfLeftSchemeCustomers
        {
            get { return numberOfLeftSchemeCustomers; }
            set { numberOfLeftSchemeCustomers = value; }
        }
        public int NumberOfAddressInErrorCustomers
        {
            get { return numberOfAddressInErrorCustomers; }
            set { numberOfAddressInErrorCustomers = value; }
        }
        public decimal OfferProductPointsBalance
        {
            get { return offerProductPointsBalance; }
            set { offerProductPointsBalance = value; }
        }
        public int NumberOfRewardMailngs
        {
            get { return numberOfRewardMailngs; }
            set { numberOfRewardMailngs = value; }
        }
         public int OfferWelcomePointsQty
        {
            get { return offerWelcomePointsQty; }
            set { offerWelcomePointsQty = value; }
        }
         public int NumberOfSkeletonCustomers
        {
            get { return numberOfSkeletonCustomers; }
            set { numberOfSkeletonCustomers = value; }
        }
         public int NumberOfNonRewardMailing
        {
            get { return numberOfNonRewardMailing; }
            set { numberOfNonRewardMailing = value; }
        }  
         public short RewardConversionStatus
        {
            get { return rewardConversionStatus; }
            set { rewardConversionStatus = value; }
        }  
         public short RewardReconversionStatus
        {
            get { return rewardReconversionStatus; }
            set { rewardReconversionStatus = value; }
        }
         public Decimal PointsToRewardConversionRate
        {
            get { return pointsToRewardConversionRate; }
            set { pointsToRewardConversionRate = value; }
        }
       
        #endregion

        #region Previous Offer Properties
         public short PreviousOfferID
         {
             get { return prevOfferId; }
             set { prevOfferId = value; }
         }
         public string PrevOfferName
         {
             get { return prevOfferName; }
             set { prevOfferName = value; }
         }
         public decimal PrevMinimumVoucherValue
         {
             get { return prevMinimumVoucherValue; }
             set { prevMinimumVoucherValue = value; }
         }
        public decimal PrevMaximumVoucherValue
         {
             get { return prevMaximumVoucherValue; }
             set { prevMaximumVoucherValue = value; }
         }
         public int PrevNumberOfVouchers
         {
             get { return prevNumberOfVouchers; }
             set { prevNumberOfVouchers = value; }
         }
         public int PrevOfferWelcomePointsQty
         {
             get { return prevOfferWelcomePointsQty; }
             set { prevOfferWelcomePointsQty = value; }
         }
         public decimal PrevAmountToPointsConversionRate
         {
             get { return prevAmountToPointsConversionRate; }
             set { prevAmountToPointsConversionRate = value; }
         }
         public DateTime PrevAllowReissueDate
         {
             get { return prevAllowReissueDate; }
             set { prevAllowReissueDate = value; }
         }
         public string PrevRewardConversionDate
         {
             get { return prevRewardConversionDate; }
             set { prevRewardConversionDate = value; }
         }
         public string PrevMailingApprovedDate
         {
             get { return prevMailingApprovedDate; }
             set { prevMailingApprovedDate = value; }
         }
         public int PrevTotalVoucherIssued
         {
             get { return prevTotalVoucherIssued; }
             set { prevTotalVoucherIssued = value; }
         }
         public string PrevMainMailDate
         {
             get { return prevMainMailDate; }
             set { prevMainMailDate = value; }
         }
         public int PrevNumberOfRewardMailngs
         {
             get { return prevNumberOfRewardMailngs; }
             set { prevNumberOfRewardMailngs = value; }
         }
         public int PrevNumberOfNonRewardMailing
         {
             get { return prevNumberOfNonRewardMailing; }
             set { prevNumberOfNonRewardMailing = value; }
         }
         public int PrevNumberOfBannedCustomers
         {
             get { return prevNumberOfBannedCustomers; }
             set { prevNumberOfBannedCustomers = value; }
         }

         public int PrevNumberOfInactiveCustomers
         {
             get { return prevNumberOfInactiveCustomers; }
             set { prevNumberOfInactiveCustomers = value; }
         }
         public int PrevNumberOfDeceasedCustomers
         {
             get { return prevNumberOfDeceasedCustomers; }
             set { prevNumberOfDeceasedCustomers = value; }
            
         }
         public int PrevNumberOfLeftSchemeCustomers
         {
             get { return prevNumberOfLeftSchemeCustomers; }
             set { prevNumberOfLeftSchemeCustomers = value; }
         }
         public int PrevnumberOfSkeletonCustomers
         {
             get { return prevnumberOfSkeletonCustomers; }
             set { prevnumberOfSkeletonCustomers = value; }
         }
         public int PrevNumberOfAddressInErrorCustomers
         {
             get { return prevNumberOfAddressInErrorCustomers; }
             set { prevNumberOfAddressInErrorCustomers = value; }
         }
         public decimal PrevOfferProductPointsBalance
         {
             get { return prevOfferProductPointsBalance; }
             set { prevOfferProductPointsBalance = value; }
         }
         public decimal PrevPointsConvertedToRewardsBalance
         {
             get { return prevPointsConvertedToRewardsBalance; }
             set { prevPointsConvertedToRewardsBalance = value; }
         }
        
         public decimal PrevVoucherValueStep
         {
             get { return prevVoucherValueStep; }
             set { prevVoucherValueStep = value; }
         }
         public int PrevRewardPointsThreshold
         {
             get { return prevRewardPointsThreshold; }
             set { prevRewardPointsThreshold = value; }
         }  
         public DateTime PrevStartDateTime
         {
             get { return prevStartDateTime; }
             set { prevStartDateTime = value; }
         }
         public DateTime PrevEndDateTime
         {
             get { return prevEndDateTime; }
             set { prevEndDateTime = value; }
         }
         public Decimal PrevPointsToRewardConversionRate
         {
             get { return prevpointsToRewardConversionRate; }
             set { prevpointsToRewardConversionRate = value; }
         }
         public short PrevRewardConversionStatus
         {
             get { return prevrewardConversionStatus; }
             set { prevrewardConversionStatus = value; }
         } 
         #endregion

         #region Methods 
         private bool IsNumeric(object Value)
        {
            try
            {
                Convert.ToInt32(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region Execute Methods
        public bool ExecuteMethods()
        {
            try
            {

               // this.OutputDirectory =  "C:\\Program Files\\Common Files\\NGC\\Batch\\Data\\Out\\";
               // this.OutputDirectory = @"\\172.29.3.17\Data\Output\";
                this.CommitSize = 100;
                sFileName = CommonFunctions.CreateLogFile(sBatch);
                //CreateXMLFile();
                string messag = "Housekeeping Started...";
                CommonFunctions.MessageWriteToLogFile(sFileName, messag);
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, messag, false);
                // This is the Sequence of Execution according to the Script file
                
                #region Check Audit Retention Limits
                /*if (AuditRetentionPeriod > 0)
                {
                    if (IsNumeric(AuditRetentionPeriod))
                    {
                        if (!(minAuditRet < AuditRetentionPeriod))
                        {
                            //ArchiveAudit(AuditRetentionPeriod);    This is commented for Temp. Has to get the confirmation              
                            Console.WriteLine("The Audit Retention Period Option [-a] must be equal or greater than minAuditRetentionPeriod days.");
                            string message = "The Audit Retention Period Option [-a] must be equal or greater than minAuditRetentionPeriod days.";                            
                            CommonFunctions.MessageWriteToLogFile(sFileName, message);
                            CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("The Audit Retention Period Option [-a] must be an integer.");
                        string message = "The Audit Retention Period Option [-a] must be an integer.";                        
                        CommonFunctions.MessageWriteToLogFile(sFileName, message);
                        CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Missing Audit Retention Period Option [-a] on the command line");
                    string message = "Missing Audit Retention Period Option [-a] on the command line.";                   
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                    return false;
                }*/
                #endregion

                #region Check Transaction Retention Limits
                /*if (TransactionRetention > 0)
                {
                    if (!IsNumeric(TransactionRetention))
                    {
                        if (!(minTransRet < TransactionRetention))
                        {
                            //ArchiveAudit(AuditRetentionPeriod);  This is commented for Temp. Has to get the confirmation   
                            Console.WriteLine("The Transaction Retention Period Option [-t] must be equal or greater than minAuditRetentionPeriod days.");
                            string message = "The Transaction Retention Period Option [-t] must be equal or greater than minAuditRetentionPeriod days.";
                            CommonFunctions.MessageWriteToLogFile(sFileName, message);
                            CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                            return false;
                        }
                        Console.WriteLine("The Transaction Retention Period Option [-t] must be an integer");
                        string message1 = "The Transaction Retention Period Option [-t] must be an integer";
                        CommonFunctions.MessageWriteToLogFile(sFileName, message1);
                        CommonFunctions.MessageWriteToEventViewer(EventLog, message1, true);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Missing Transaction Retention Period Option [-t] on the command line");
                    string message1 = "Missing Transaction Retention Period Option [-t] on the command line";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message1);
                    CommonFunctions.MessageWriteToEventViewer(EventLog, message1, true);
                    return false;
                }*/
                #endregion

                #region Check Offer Retention Limits
                if (OfferRetention > 0)
                {
                    if (!IsNumeric(OfferRetention))
                    {
                        if (!(minOffRet < OfferRetention))
                        {
                            //ArchiveAudit(AuditRetentionPeriod);   This is commented for Temp. Has to get the confirmation  
                            Console.WriteLine("The Offer Retention Period Option [-o] must be equal or greater than $minCustomerOfferRetentionPeriod days.");
                            string message1 = "The Offer Retention Period Option [-o] must be equal or greater than $minCustomerOfferRetentionPeriod days.";
                            CommonFunctions.MessageWriteToLogFile(sFileName, message1);
                            CommonFunctions.MessageWriteToEventViewer(EventLog, message1, true);
                            return false;
                        }
                        Console.WriteLine("The Offer Retention Period Option [-o] must be an integer");
                        string message2 = "The Offer Retention Period Option [-o] must be an integer";
                        CommonFunctions.MessageWriteToLogFile(sFileName, message2);
                        CommonFunctions.MessageWriteToEventViewer(EventLog, message2, true);
                        return false;
                    }
                }
                else
                {

                }
                
                #endregion

                //Find current offer
                retunValue = FindCurrentOffer();
                if (retunValue!= true)                 
                    return false;
                retunValue = FindPreviousOffer();
                if (retunValue != true)
                    return false;
                // Execute the sp_housekeeping_initialise stored Procedure by calling the CreateTempTable() method
                retunValue = HKInitialise();
                if (retunValue != true)   
                     return false;
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-1-->
                WriteLine("<txn_archive><file_header><date_created>" + DateTime.Now + "</date_created></file_header><data>");
                WriteLine("<offer><collection_period_details><collection_period_start_date>" + PrevStartDateTime + "</collection_period_start_date><collection_period_end_date>" + PrevEndDateTime + "</collection_period_end_date></collection_period_details>");


                string dateTime1 = DateTime.Now.ToString("yyyyMMddHHmmss");
                retunValue = WriteTransdetails(OfferID, OutPutDirectory, "archive_txn_data_" + dateTime1 + ".dat");
                if (retunValue != true)   
                    return false;

                WriteLine("<txn_archive_customer_records>" + nCustomerRecords + "</txn_archive_customer_records>");
                WriteLine("<txn_archive_txn_records>" + nTxnRecords + "</txn_archive_txn_records>");
                WriteLine("<txn_archive_txnvoucher_records>" + nTxnVoucherRecords + "</txn_archive_txnvoucher_records>");
               
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-3-->
                WriteLine("</offer>");
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-4-->
                WriteLine("</data></txn_archive>");
                
                // Execute the sp_housekeeping_export stored Procedure by calling the HouseKeepExport() method                 
                //string dateTime1 = DateTime.Now.ToString("yyyyMMddHHmmss");
                //retunValue = HouseKeepExport(OutPutDirectory, "archive_txn_data_" + dateTime1 + ".xml");
                //if (retunValue != true)   
                //    return false;
                                
                // Execute the sp_housekeeping_initialise stored Procedure by calling the CreateTempTable() method
                retunValue = HKInitialise();
                if (retunValue != true)   
                    return false;
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-5-->
                
                WriteLine("<customer_offer_archive><file_header><date_created>" + DateTime.Now + "</date_created></file_header><data>");
                StringBuilder sbWriteLine = new StringBuilder();
                sbWriteLine.Append("<offer>");
                sbWriteLine.Append("<collection_period_details>");
                sbWriteLine.Append("<collection_period_start_date>" + PrevStartDateTime + "</collection_period_start_date>");                
                sbWriteLine.Append("<collection_period_end_date>" + PrevEndDateTime + "</collection_period_end_date>");
                sbWriteLine.Append("<offer_data>");
                sbWriteLine.Append("<offer_name>"+PrevOfferName+"</offer_name>");
                sbWriteLine.Append("<reward_conversion_date>"+PrevRewardConversionDate+"</reward_conversion_date>");
                sbWriteLine.Append("<main_mail_date>"+PrevMainMailDate+"</main_mail_date>");
                sbWriteLine.Append("<allow_reissue_date>"+PrevAllowReissueDate+"</allow_reissue_date>");
                sbWriteLine.Append("<points_threshold>" + PrevRewardPointsThreshold + "</points_threshold>");
                WriteLine(sbWriteLine.ToString());
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-7-->
                StringBuilder sbWriteLine2 = new StringBuilder();
                sbWriteLine2.Append("<amount_to_points_conversion_rate>" + PrevAmountToPointsConversionRate + "</amount_to_points_conversion_rate>");
                sbWriteLine2.Append("<points_to_reward_conversion_rate>" + PrevPointsToRewardConversionRate + "</points_to_reward_conversion_rate>");
                sbWriteLine2.Append("<maximum_voucher_value>"+PrevMaximumVoucherValue+"</maximum_voucher_value>");
                sbWriteLine2.Append("<minimum_voucher_value>"+PrevMinimumVoucherValue+"</minimum_voucher_value>");
                sbWriteLine2.Append("<voucher_step_size>"+PrevVoucherValueStep+"</voucher_step_size>");
                sbWriteLine2.Append("<number_of_vouchers>"+PrevNumberOfVouchers+"</number_of_vouchers>");
                //sbWriteLine2.Append("<amount_rewarded_balance>$amount_rewarded_balance</amount_rewarded_balance>");
                sbWriteLine2.Append("<points_converted_to_rewards_balance>"+PrevPointsConvertedToRewardsBalance+"</points_converted_to_rewards_balance>");
                sbWriteLine2.Append("<total_vouchers_issued>"+PrevTotalVoucherIssued+"</total_vouchers_issued>");
                sbWriteLine2.Append("<number_of_reward_mailings>"+PrevNumberOfRewardMailngs+"</number_of_reward_mailings>");
                sbWriteLine2.Append("<number_of_non_reward_mailings>" + PrevNumberOfNonRewardMailing + "</number_of_non_reward_mailings>");
                sbWriteLine2.Append("<number_of_banned_customers>"+PrevNumberOfBannedCustomers+"</number_of_banned_customers>");
                WriteLine(sbWriteLine2.ToString());
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-8-->
                StringBuilder sbWriteLine3 = new StringBuilder();
                sbWriteLine3.Append("<number_of_inactive_customers>"+PrevNumberOfInactiveCustomers+"</number_of_inactive_customers>");
                sbWriteLine3.Append("<number_of_deceased_customers>"+PrevNumberOfDeceasedCustomers+"</number_of_deceased_customers>");
                sbWriteLine3.Append("<number_of_left_scheme_customers>"+prevNumberOfLeftSchemeCustomers+"</number_of_left_scheme_customers>");
                sbWriteLine3.Append("<number_of_skeleton_customers>"+PrevnumberOfSkeletonCustomers+"</number_of_skeleton_customers>");
                sbWriteLine3.Append("<number_of_address_in_error_customers>"+PrevNumberOfAddressInErrorCustomers+"</number_of_address_in_error_customers>");
                sbWriteLine3.Append("<offer_welcome_points_balance>"+PrevOfferWelcomePointsQty+"</offer_welcome_points_balance>");
                sbWriteLine3.Append("<offer_product_points_balance>"+PrevOfferProductPointsBalance+"</offer_product_points_balance>");
                sbWriteLine3.Append("<reward_conversion_status_code>" + PrevRewardConversionStatus + "</reward_conversion_status_code>");
                sbWriteLine3.Append("<reward_reconversion_status_code>" + PrevRewardConversionStatus + "</reward_reconversion_status_code>");
                //sbWriteLine3.Append("<view_mailing_report_status_code>$view_mailing_report_status_code</view_mailing_report_status_code>");
                sbWriteLine3.Append("<mailing_approved_date>"+PrevMailingApprovedDate+"</mailing_approved_date>");
                sbWriteLine3.Append("</offer_data>");
                //sbWriteLine3.Append("<voucher_type_details>");
                WriteLine(sbWriteLine3.ToString());
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-9-->
                StringBuilder sbWriteLine4 = new StringBuilder();               
                WriteLine("</collection_period_details>");

                // Execute the sp_housekeeping_write_offer_xml stored Procedure by calling the HouseKeepWriteOffXml() method 
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                retunValue = HouseKeepWriteOffXml(OfferID, OutPutDirectory, "archive_offer_data_" + dateTime + ".dat");
              
                if (retunValue != true)   
                    return false;
                   
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-13-->
                WriteLine("</offer>");
                // Execute the sp_housekeeping_writeline stored Procedure by calling the writeLine() method <--- Write Line-14-->
                WriteLine("</data></customer_offer_archive>");
                //string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                //retunValue = HouseKeepExport(OutPutDirectory, "archive_offer_data_" + dateTime + ".dat");
                flag = 1;
                //if (retunValue != true)   
                //    return false;

                //For Temp output paramter 
                //int rowcount;
                //retunValue = HouseKeepOffer(OfferRetention, PrevStartDateTime, 6, out rowcount);  //Calling sp_housekeeping_offer Stored Procedure. <!--  REMOVE VOUCHER_TYPE RECORDS  -->        
                //if (retunValue != true)   
                //    return false;
                //retunValue = HouseKeepOffer(OfferRetention, PrevStartDateTime, 7, out rowcount); //Calling sp_housekeeping_offer Stored Procedure. <!--  REMOVE COUPON_TYPE RECORDS  --> 
                //if (retunValue != true)   
                //    return false;
                //retunValue = HouseKeepOffer(OfferRetention, PrevStartDateTime, 1, out rowcount); //Calling sp_housekeeping_offer Stored Procedure. <!--  REMOVE FINANCIAL DAY RECORDS   -->
                //if (retunValue != true)   
                //    return false;
                //retunValue = HouseKeepOffer(OfferRetention, PrevStartDateTime, 2, out rowcount); //Calling sp_housekeeping_offer Stored Procedure. <!--  REMOVE STORE DAY RECORDS   -->  
                //if (retunValue != true)   
                //    return false;
                //retunValue = HouseKeepOffer(OfferRetention, PrevStartDateTime, 8, out rowcount);  //Calling sp_housekeeping_offer Stored Procedure.  <!--  REMOVE OFFER RECORDS   --> 
                //if (retunValue != true)   
                //    return false;

                string messages = "Housekeeping Ended Successfully";
                CommonFunctions.MessageWriteToLogFile(sFileName, messages);
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, messages, false);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        
        #region Create Temporary Table
        public bool HKInitialise()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("HouseKeeping.CreateTempTable");
            int returnCode = 0;
            try
            {                
                object[] objHKInitialise = {  };
                returnCode = DataAccess.ExecuteNonQuery(connectionString,Constants.SP_HOUSEKEEPING_INITIALISE, ref objHKInitialise);                
                return true;              
 
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a Problem to execute the HouseKeeping.";
                
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false ;
            }
            
        }
        #endregion

        #region Add the XML Header to the Output Table
        public int WriteLine(string xml)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("HouseKeeping.WriteLine");
            int returnCode = 0;
            try
            {

                object[] objHKWriteLine = {xml};
                returnCode = DataAccess.ExecuteNonQuery(connectionString,Constants.SP_HOUSEKEEPING_WRITELINE, ref objHKWriteLine);
                return returnCode;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                //executeResult.Add(e);
                //errorOutput(e.Message);
                return returnCode;
            }
        }
        #endregion

        
        #region Write the transaction details
        //Write the transaction details to the temp_housekeeping table  
        public bool WriteTransdetails(int offerID, string sExportFilePath, string sExportFileName)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("HouseKeeping.WriteTransDetails");           
            string viewXml = String.Empty;
            int returnCode = 0;
            DataSet ds = new DataSet();
            Int64 nRecords = 0;

            try
            {
                sExportFilePath = sExportFilePath.Trim();
                if (sExportFilePath.Substring(sExportFilePath.Length - 1) == "\\" || sExportFilePath.Substring(sExportFilePath.Length - 1) == "/")
                {
                    sExportFilePath = sExportFilePath.Substring(0, sExportFilePath.Length - 1);
                }

                object[] objWriteTransData = { offerID, sExportFilePath, sExportFileName, nRecords };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_WRITE_TRANSACTION_ALL_IN_ONE, ref objWriteTransData);

                nRecords = (Int64)objWriteTransData[3];
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "Housekeeping Transaction Details Archived : " + nRecords + " records.", false);
                CommonFunctions.MessageWriteToLogFile(sFileName, "WriteTransdetails() Function Finished");           
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a Problem to write the Housekeeping Transaction Details.";                
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
            
        }
        #endregion
         

         #region HouseKeeping Clear Partition Flag
         //Call sp_housekeeping_clear_partition_flag to clear the txn partition requires_archiving flag
        public bool HouseKeepWriteOffXml(int offerID, string sExportFilePath, string sExportFileName)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("HouseKeeping.HouseKeepWriteOffXml");
            int returnCode = 0;
            DataSet ds = new DataSet();
            Int64 nRecords = 0;

            try
            {
                sExportFilePath = sExportFilePath.Trim();
                if (sExportFilePath.Substring(sExportFilePath.Length - 1) == "\\" || sExportFilePath.Substring(sExportFilePath.Length - 1) == "/")
                {
                    sExportFilePath = sExportFilePath.Substring(0, sExportFilePath.Length - 1);
                }

                object[] objHKWriteOffXml = { offerID, sExportFilePath, sExportFileName, nRecords };

                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_WRITE_OFFER_ALL_IN_ONE, ref objHKWriteOffXml);

                nRecords = (Int64)objHKWriteOffXml[3];
                CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "Housekeeping Offer Details Archived : " + nRecords + " records.", false);
                CommonFunctions.MessageWriteToLogFile(sFileName, "HouseKeepWriteOffXml() Function Finished");

                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a Problem to write Housekeeping Offer Details.";                
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
         #endregion

         

         #region House Keeping Offer
         // Offer REMOVE VOUCHER_TYPE RECORDS 
         //public bool HouseKeepOffer(int offRetention, DateTime currentstartdate, int tableid, out int rowCount )
         //{
         //    Trace trace = new Trace();
         //    TraceState trState = trace.StartProc("HouseKeeping.HouseKeepOffer");
         //    int returnCode = 0;
         //    rowCount = 0;
         //    int totalRecords;

         //    try
         //    {
         //        object[] objOfferXml = { offRetention, currentstartdate, tableid, rowCount };
         //        //returnCode = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_OFFER, objOfferXml);
         //        returnCode =  DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_OFFER, ref objOfferXml);
         //        totalRecords = (int)objOfferXml[3];
         //        if (tableid == 6)
         //        {
         //             nRemovedVoucherTypeRecords = nRemovedVoucherTypeRecords + totalRecords;
         //             string message = "Housekeeping Removed : " + nRemovedVoucherTypeRecords + "VouchertypeRecords";                     
         //             CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //             CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
         //        }
         //        else if (tableid == 7)
         //        {
         //            nRemovedCouponTypeRecords = nRemovedCouponTypeRecords + totalRecords;
         //            string message = "Housekeeping Removed : " + nRemovedCouponTypeRecords + " Coupon Type Records";                     
         //            CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
         //        }
         //        else if (tableid == 1)
         //        {
         //            nRemovedFinancialDayRecords = nRemovedFinancialDayRecords + totalRecords;
         //            string message = "Housekeeping Removed : " + nRemovedFinancialDayRecords + " Financial Day Records";                     
         //            CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
         //        }
         //        else if (tableid == 2)
         //        {
         //            nRemovedStoreDayRecords = nRemovedStoreDayRecords + totalRecords;
         //            string message = "Housekeeping Removed : " + nRemovedStoreDayRecords + " Store Day Records";                     
         //            CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
         //        }
                 

         //        return true;
         //    }
         //    catch (Exception e)
         //    {
         //        ExceptionManager.Publish(e);
         //        string message = "There is a Problem in Executing the HouseKeeping Offer.";                 
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
         //        return false;
         //    }
         //}
         #endregion
        #endregion

        #region Find the current offer period
         public bool FindCurrentOffer()
         {
             Trace trace = new Trace();
             TraceState trState = trace.StartProc("HouseKeeping.FindCurrentOffer");
             int returnCode = 0;
             int rowCount = 0;
             DataSet ds = new DataSet();

             try
             {
                 object[] objGetCurrentOffer = {  };
                 ds = SqlHelper.ExecuteDataset(connectionString, Constants.GetCurrentOffer, objGetCurrentOffer);

                 ds.Tables[0].TableName = "CurrentOffer";
                 if ((ds.Tables[0].Rows[0][0]) != DBNull.Value)
                 this.OfferID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                 if ((ds.Tables[0].Rows[0][1]) != DBNull.Value)
                 this.OfferName = (string)ds.Tables[0].Rows[0][1];
                 if ((ds.Tables[0].Rows[0][2]) != DBNull.Value)
                 this.CurrentSatrtDate = Convert.ToDateTime(ds.Tables[0].Rows[0][2]);
                 if ((ds.Tables[0].Rows[0][3]) != DBNull.Value)
                 this.CurrentEndDate = Convert.ToDateTime(ds.Tables[0].Rows[0][3]);
                 if ((ds.Tables[0].Rows[0][4]) != DBNull.Value)
                 this.MinimumVoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[0][4]);
                 if ((ds.Tables[0].Rows[0][5]) != DBNull.Value)
                 this.MaximumVoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[0][5]);
                 if ((ds.Tables[0].Rows[0][6]) != DBNull.Value)
                 this.VoucherValueStep = Convert.ToDecimal(ds.Tables[0].Rows[0][6]);
                 if ((ds.Tables[0].Rows[0][7]) != DBNull.Value)
                     this.NumberOfVouchers = Convert.ToInt32(ds.Tables[0].Rows[0][7]);

                 if ((ds.Tables[0].Rows[0][8]) != DBNull.Value)
                     this.OfferWelcomePointsQty = Convert.ToInt32(ds.Tables[0].Rows[0][8]);

                 if ((ds.Tables[0].Rows[0][9]) != DBNull.Value)
                     this.AmountToPointsConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0][9]);

                 if ((ds.Tables[0].Rows[0][10]) != DBNull.Value)
                     //this.PointsConvertedToRewardsBalance = ds.Tables[0].Rows[0][9];
                     this.RewardPointsThreshold = Convert.ToInt32(ds.Tables[0].Rows[0][10]);

                 if ((ds.Tables[0].Rows[0][11]) != DBNull.Value)
                     this.AllowReissueDate = Convert.ToDateTime(ds.Tables[0].Rows[0][11]);

                 if ((ds.Tables[0].Rows[0][12]) != DBNull.Value)
                     this.RewardConversionDate = Convert.ToDateTime(ds.Tables[0].Rows[0][12]);

                 if ((ds.Tables[0].Rows[0][13]) != DBNull.Value)
                     this.RewardConversionStatus = Convert.ToInt16(ds.Tables[0].Rows[0][13]);

                 if ((ds.Tables[0].Rows[0][14]) != DBNull.Value)
                     this.MailingApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0][14]);

                 if ((ds.Tables[0].Rows[0][15]) != DBNull.Value)
                     this.TotalVoucherIssued = Convert.ToInt32(ds.Tables[0].Rows[0][15]);

                 if ((ds.Tables[0].Rows[0][16]) != DBNull.Value)
                     this.MainMailDate = Convert.ToDateTime(ds.Tables[0].Rows[0][16]);

                 if ((ds.Tables[0].Rows[0][17]) != DBNull.Value)
                     this.NumberOfRewardMailngs = Convert.ToInt32(ds.Tables[0].Rows[0][17]);

                 if ((ds.Tables[0].Rows[0][18]) != DBNull.Value)
                     this.NumberOfNonRewardMailing = Convert.ToInt32(ds.Tables[0].Rows[0][18]);

                 if ((ds.Tables[0].Rows[0][19]) != DBNull.Value)
                     this.NumberOfBannedCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][19]);

                 if ((ds.Tables[0].Rows[0][20]) != DBNull.Value)
                     this.NumberOfInactiveCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][20]);

                 if ((ds.Tables[0].Rows[0][21]) != DBNull.Value)
                     this.NumberOfDeceasedCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][21]);

                 if ((ds.Tables[0].Rows[0][22]) != DBNull.Value)
                     this.NumberOfLeftSchemeCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][22]);

                 if ((ds.Tables[0].Rows[0][23]) != DBNull.Value)
                     this.numberOfSkeletonCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][23]);

                 if ((ds.Tables[0].Rows[0][24]) != DBNull.Value)
                     this.NumberOfAddressInErrorCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][24]);

                 if ((ds.Tables[0].Rows[0][25]) != DBNull.Value)
                     this.OfferProductPointsBalance = Convert.ToDecimal(ds.Tables[0].Rows[0][25]);

                 if ((ds.Tables[0].Rows[0][26]) != DBNull.Value)
                     this.PointsConvertedToRewardsBalance = Convert.ToDecimal(ds.Tables[0].Rows[0][26]);
                 
                     if ((ds.Tables[0].Rows[0][27]) != DBNull.Value)
                         this.PointsToRewardConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0][27]);
                 //this.numberof

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

         #region Find the Previous offer period
         public bool FindPreviousOffer()
         {
             Trace trace = new Trace();
             TraceState trState = trace.StartProc("CalculateRewards.FindPreviousOffer");
             int returnCode = 0;
             int rowCount = 0;
             DataSet ds = new DataSet();

             try
             {
                 object[] objGetPreviousOffer = { };
                 ds = SqlHelper.ExecuteDataset(connectionString, Constants.GetPreviousOffer, objGetPreviousOffer);
                 ds.Tables[0].TableName = "PreviousOffer";
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     if ((ds.Tables[0].Rows[0][0]) != DBNull.Value)
                         this.PreviousOfferID = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                     if ((ds.Tables[0].Rows[0][1]) != DBNull.Value)
                         this.PrevVoucherValueStep = Convert.ToDecimal(ds.Tables[0].Rows[0][1]);
                     if ((ds.Tables[0].Rows[0][2]) != DBNull.Value)
                         this.PrevMinimumVoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[0][2]);
                     if ((ds.Tables[0].Rows[0][3]) != DBNull.Value)
                         this.PrevRewardPointsThreshold = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                     if ((ds.Tables[0].Rows[0][7]) != DBNull.Value)
                         this.PrevRewardConversionStatus = Convert.ToInt16(ds.Tables[0].Rows[0][7]);

                     if ((ds.Tables[0].Rows[0][8]) != DBNull.Value)
                         this.PrevStartDateTime = Convert.ToDateTime(ds.Tables[0].Rows[0][8]);
                     if ((ds.Tables[0].Rows[0][9]) != DBNull.Value)
                         this.PrevEndDateTime = Convert.ToDateTime(ds.Tables[0].Rows[0][9]);

                     if ((ds.Tables[0].Rows[0][10]) != DBNull.Value)
                         this.PrevOfferName = Convert.ToString(ds.Tables[0].Rows[0][10]);

                     if ((ds.Tables[0].Rows[0][11]) != DBNull.Value)
                         this.PrevMaximumVoucherValue= Convert.ToDecimal(ds.Tables[0].Rows[0][11]);
                     if ((ds.Tables[0].Rows[0][12]) != DBNull.Value)
                         this.PrevNumberOfVouchers = Convert.ToInt32(ds.Tables[0].Rows[0][12]);

                     if ((ds.Tables[0].Rows[0][13]) != DBNull.Value)
                         this.PrevOfferWelcomePointsQty = Convert.ToInt32(ds.Tables[0].Rows[0][13]);
                     if ((ds.Tables[0].Rows[0][14]) != DBNull.Value)
                         this.PrevAmountToPointsConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0][14]);

                     if ((ds.Tables[0].Rows[0][15]) != DBNull.Value)
                         this.PrevAllowReissueDate = Convert.ToDateTime(ds.Tables[0].Rows[0][15]);
                     if ((ds.Tables[0].Rows[0][16]) != DBNull.Value)
                         this.PrevRewardConversionDate = Convert.ToString(ds.Tables[0].Rows[0][16]);

                     if ((ds.Tables[0].Rows[0][17]) != DBNull.Value)
                         this.PrevMailingApprovedDate = Convert.ToString(ds.Tables[0].Rows[0][17]);
                     if ((ds.Tables[0].Rows[0][18]) != DBNull.Value)
                         this.PrevTotalVoucherIssued = Convert.ToInt32(ds.Tables[0].Rows[0][18]);

                     if ((ds.Tables[0].Rows[0][19]) != DBNull.Value)
                         this.PrevMainMailDate = Convert.ToString(ds.Tables[0].Rows[0][19]);
                     if ((ds.Tables[0].Rows[0][20]) != DBNull.Value)
                         this.PrevNumberOfRewardMailngs = Convert.ToInt32(ds.Tables[0].Rows[0][20]);
                     if ((ds.Tables[0].Rows[0][21]) != DBNull.Value)
                         this.PrevNumberOfNonRewardMailing = Convert.ToInt32(ds.Tables[0].Rows[0][21]);
                     if ((ds.Tables[0].Rows[0][22]) != DBNull.Value)
                         this.PrevNumberOfBannedCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][22]);
                     if ((ds.Tables[0].Rows[0][23]) != DBNull.Value)
                         this.PrevNumberOfInactiveCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][23]);
                     if ((ds.Tables[0].Rows[0][24]) != DBNull.Value)
                         this.PrevNumberOfDeceasedCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][24]);

                     if ((ds.Tables[0].Rows[0][25]) != DBNull.Value)
                         this.PrevNumberOfLeftSchemeCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][25]);
                     if ((ds.Tables[0].Rows[0][26]) != DBNull.Value)
                         this.PrevnumberOfSkeletonCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][26]);

                     if ((ds.Tables[0].Rows[0][27]) != DBNull.Value)
                         this.PrevNumberOfAddressInErrorCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][27]);

                     if ((ds.Tables[0].Rows[0][28]) != DBNull.Value)
                         this.PrevOfferProductPointsBalance = Convert.ToDecimal(ds.Tables[0].Rows[0][28]);
                     
                     if ((ds.Tables[0].Rows[0][29]) != DBNull.Value)
                         this.PrevPointsConvertedToRewardsBalance= Convert.ToDecimal(ds.Tables[0].Rows[0][29]);

                     
                     
                     //this.numberof
                     rowCount = returnCode;
                     return true;

                 }
                 else
                 {
                     Console.WriteLine("There is no Previous collection period defined");
                     string message = "There is no Previous collection period defined.";
                     CommonFunctions.MessageWriteToLogFile(sFileName, message);
                     CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                     return false;
                 }
                 //return true;
             }
             catch (Exception e)
             {
                 ExceptionManager.Publish(e);
                 //executeResult.Add(e);
                 //errorOutput(e.Message);
                 return false;
             }
         }

         #endregion

         //public bool writeTransdetails(int offerID, int commitsize, out int ncustomerRecords, out int nTxnRecord, out int ntxnVoucherRecords)
         //{
         //    Trace trace = new Trace();
         //    TraceState trState = trace.StartProc("HouseKeeping.WriteTransDetails");
         //    string viewXml = String.Empty;
         //    int returnCode = 0;
         //    ncustomerRecords = 0;
         //    nTxnRecord = 0;
         //    ntxnVoucherRecords = 0;
         //    DataSet ds = new DataSet();
         //    try
         //    {

         //        //object[] objWriteTransData = { offerID, commitsize, ncustomerRecords, nTxnRecord, ntxnVoucherRecords };
         //        /*CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "Array Size : " + objWriteTransData.Length, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "offerID : " + offerID , false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "commitsize : " + commitsize, false);*/


         //        //
         //        SqlConnection con = new SqlConnection(connectionString);
         //        con.Open();
         //        SqlCommand cmd = new SqlCommand(Constants.SP_HOUSEKEEPING_WRITELE_TXN_XML, con);
         //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

         //        SqlParameter Param1;
         //        SqlParameter ParamOut1;
         //        SqlParameter ParamOut2;
         //        SqlParameter ParamOut3;

         //        Param1 = new SqlParameter();
         //        Param1.ParameterName = "@OfferID";
         //        Param1.Direction = System.Data.ParameterDirection.Input;
         //        Param1.Value = offerID;
         //        cmd.Parameters.Add(Param1);

         //        Param1 = new SqlParameter();
         //        Param1.ParameterName = "@commit_size";
         //        Param1.Direction = System.Data.ParameterDirection.Input;
         //        Param1.Value = commitsize;
         //        cmd.Parameters.Add(Param1);

         //        ParamOut1 = new SqlParameter();
         //        ParamOut1.ParameterName = "@nCustomerRecords";
         //        ParamOut1.Direction = System.Data.ParameterDirection.Output;
         //        ParamOut1.DbType = System.Data.DbType.Int32;
         //        cmd.Parameters.Add(ParamOut1);

         //        ParamOut2 = new SqlParameter();
         //        ParamOut2.ParameterName = "@nTxnRecords";
         //        ParamOut2.Direction = System.Data.ParameterDirection.Output;
         //        ParamOut2.DbType = System.Data.DbType.Int32;
         //        cmd.Parameters.Add(ParamOut2);

         //        ParamOut3 = new SqlParameter();
         //        ParamOut3.ParameterName = "@nTxnVoucherRecords";
         //        ParamOut3.Direction = System.Data.ParameterDirection.Output;
         //        ParamOut3.DbType = System.Data.DbType.Int32;
         //        cmd.Parameters.Add(ParamOut3);


         //        /*
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "Default Time out : " + cmd.CommandTimeout, false);
         //        cmd.CommandTimeout = 0; //Timeout set as unlimited since this is a long run process
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "After Setting Time out : " + cmd.CommandTimeout, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "Before Executing : " + Constants.SP_HOUSEKEEPING_WRITELE_TXN_XML, false);
         //        returnCode = cmd.ExecuteNonQuery();
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "After Executing : " + Constants.SP_HOUSEKEEPING_WRITELE_TXN_XML, false);
         //        */


         //        cmd.CommandTimeout = 0; //Timeout set as unlimited since this is a long run process
         //        returnCode = cmd.ExecuteNonQuery();
         //        ncustomerRecords = Convert.ToInt32(ParamOut1.Value);
         //        nTxnRecord = Convert.ToInt32(ParamOut2.Value);
         //        ntxnVoucherRecords = Convert.ToInt32(ParamOut3.Value);

         //        /*CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nCustomerRecords : " + nCustomerRecords, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nTxnRecords : " + nTxnRecords, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nTxnVoucherRecords : " + nTxnVoucherRecords, false);*/

         //        //


         //        //returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_WRITELE_TXN_XML, ref objWriteTransData);



         //        /*nCustomerRecords = (int)objWriteTransData[2];
         //        nTxnRecords = (int)objWriteTransData[3];
         //        nTxnVoucherRecords = (int)objWriteTransData[4];

         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nCustomerRecords : " + nCustomerRecords, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nTxnRecords : " + nTxnRecords, false);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, "nTxnVoucherRecords : " + nTxnVoucherRecords, false);*/


         //        string message = "Housekeeping Archived :" + ncustomerRecords + "  " + "Customer Records" + Environment.NewLine + "Housekeeping Archived :" + nTxnRecord + "  " + "Transaction Records" + Environment.NewLine + "Housekeeping Archived :" + ntxnVoucherRecords + "  " + "Voucher Records";
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
         //        return true;
         //    }
         //    catch (Exception e)
         //    {
         //        ExceptionManager.Publish(e);
         //        string message = "There is a Problem to write the Transaction Details.";
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
         //        return false;
         //    }

         //}

         #region HouseKeeping Export
         //Use BCP to dump the temp_housekeeping table into an xml file
         //public bool HouseKeepExport(string outputDirectory, string filename)
         //{
         //    Trace trace = new Trace();
         //    TraceState trState = trace.StartProc("HouseKeeping.HouseKeepExport");
         //    int returnCode = 0;
         //    try
         //    {

         //        object[] objExport = { outputDirectory, filename };
         //        returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_EXPORT, ref objExport);
         //        return true;

         //    }
         //    catch (Exception e)
         //    {
         //        ExceptionManager.Publish(e);
         //        string message = "There is a Problem to Export the HouseKeeping Transaction Details.";
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
         //        return false;
         //    }
         //}
         #endregion

         #region HouseKeeping Clear Partition Flag
         ////Call sp_housekeeping_clear_partition_flag to clear the txn partition requires_archiving flag
         //public bool HouseKeepWriteOffXml(int offerID, int commitSize, out int ncustomerOfferRecords, out int nofferVoucherRecords)
         //{
         //    Trace trace = new Trace();
         //    TraceState trState = trace.StartProc("HouseKeeping.HouseKeepWriteOffXml");
         //    int returnCode = 0;
         //    //Output Parameters
         //    ncustomerOfferRecords = 0;
         //    nofferVoucherRecords = 0;
         //    int retCustOfferRecords;
         //    int retVoucherRecords;
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        object[] objHKWriteOffXml = { offerID, commitSize, ncustomerOfferRecords, nofferVoucherRecords };
         //        //ds = SqlHelper.ExecuteDataset(connectionString,Constants.SP_HOUSEKEEPING_WRITE_OFFER_XML, objHKWriteOffXml);

         //        returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_HOUSEKEEPING_WRITE_OFFER_XML, ref objHKWriteOffXml);
         //        retCustOfferRecords = (int)objHKWriteOffXml[2];
         //        retVoucherRecords = (int)objHKWriteOffXml[3];
         //        nCustomerOfferRecords = nCustomerOfferRecords + retCustOfferRecords;
         //        nofferVoucherRecords = nofferVoucherRecords + retVoucherRecords;
         //        string message = "Housekeeping Archived :" + nCustomerOfferRecords + "  " + "Customer Offer Records " + Environment.NewLine + "Housekeeping Archived :" + nofferVoucherRecords + "  " + " Voucher Records";
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);

         //        return true;
         //    }
         //    catch (Exception e)
         //    {
         //        ExceptionManager.Publish(e);
         //        string message = "There is a Problem to write Housekeeping offer XML.";
         //        string sFileName = CommonFunctions.CreateLogFile(sBatch);
         //        CommonFunctions.MessageWriteToLogFile(sFileName, message);
         //        CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
         //        return false;
         //    }
         //}
         #endregion
        #region To delete the trace files older than n days
         public bool Delete_Older_TraceFiles()
         {
             String File_Directory_Path;
             int NoOfDays;
             DateTime dtTime;
             try
             {
                 dtTime = DateTime.Now;
                 NoOfDays = Convert.ToInt16(ConfigurationSettings.AppSettings.Get("NoOfDays"));

                 if (NoOfDays > 0)
                 {
                     //Delete NGC NGCMarketing Trace Files
                     Console.WriteLine("Deleting Trace log Files...");
                     //EventLog.WriteEntry("Delete_Files", "Deleting NGCMarketing trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("MarketingTraceFilePath").ToString();//"C:\\Trace\\NGCMarketing\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                     //Delete NGC NGCMarketing Trace Files
                     //EventLog.WriteEntry("Delete_Files", "Deleting NGCWebService trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("NGCWebServiceTraceFilePath").ToString();//"C:\\Trace\\NGCWebService\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                     //Delete NGC NGCMarketing Trace Files
                     //EventLog.WriteEntry("Delete_Files", "Deleting BatchWindowsService trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("BatchWindowsServiceTraceFilePath").ToString();//"C:\\Trace\\BatchWindowsService\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                     //Delete NGC NGCMarketing Trace Files
                     //EventLog.WriteEntry("Delete_Files", "Deleting NGCCSD trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("NGCCSDTraceFilePath").ToString();//"C:\\Trace\\NGCCSD\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                     //Delete NGC NGCMarketing Trace Files
                     // EventLog.WriteEntry("Delete_Files", "Deleting NGCSSC trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("NGCSSCTraceFilePath").ToString();//"C:\\Trace\\NGCSSC\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                     //Delete NGC NGCMarketing Trace Files
                     //EventLog.WriteEntry("Delete_Files", "Deleting PosSocketsService trace files", EventLogEntryType.Information);
                     File_Directory_Path = ConfigurationSettings.AppSettings.Get("PosSocketsServiceTraceFilePath").ToString(); //"C:\\Trace\\PosSocketsService\\";
                     Delete_Files(File_Directory_Path, NoOfDays, dtTime);
                 }

                 return true;
             }

             catch (Exception e)
             {
                 ExceptionManager.Publish(e);
                 string message = "There is a Problem to write the Housekeeping Transaction Details.";
                 CommonFunctions.MessageWriteToLogFile(sFileName, message);
                 CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                 return false;
             }

         }
         private void Delete_Files(String FilePath, int NoOfDays, DateTime dtTime)
         {
             if (Directory.Exists(FilePath))
             {
                 foreach (String SFiles in Directory.GetFiles(FilePath))
                 {
                     TimeSpan Tspan = dtTime.Subtract(File.GetCreationTime(SFiles));
                     if (Tspan.Days > NoOfDays)
                     {
                         File.Delete(SFiles);
                     }
                 }

             }
             else
             {
                 Console.WriteLine("Error: Directory Path " + FilePath + " does not exists");
             }
         }
#endregion

    }
}
