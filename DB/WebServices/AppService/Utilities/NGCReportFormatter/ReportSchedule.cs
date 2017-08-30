/*************************************************************************************************************************
 *  File : ReportSchedule.cs
 *  Part of : NGCReportFormatter
 *  Project : NGC V 3.1.2
 *  Objective : Generate excel formatted report and email it to the recipients based on the input parameters.
 *  Author : Syed Amjadulla
 *  Date : 30th Sep'2009
 *************************************************************************************************************************/
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
using System.Xml.Xsl;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Fujitsu.eCrm.Generic.LocalizationLibrary;

namespace Tesco.NGC.Utilities
{
    class ReportSchedule
    {
        #region Properties
        private static int reportID;
        private static long reportScheduleID;
        private static string userName;
        private static string reportCultureCode;
        private static string defaultCultureCode;
        private static string fileName;
        private static string sqlJobName;
        private static string reportName;
        private static string reportParams;
        private static string reportHeadings;
        private static string localizationPath;
        private static string scheduleTimeHHMMSS;
        private static DateTime insertDateTime;
        private static int insertBy;
        private static DateTime amendDateTime;
        private static int amendBy;
        private static string emailFromAddress;
        private static string emailBody;
        private static string reportOutputDirectory;
        private static string reportOutputFile;
        private static string recurrenceType;
        private static string emailRecepients;
        private static string connectionString;
        private static string startDate;
        private static string endDate;
        private static int week;
        private static string period;
        private static string cashierID;
        private static string previousWeeks;
        private static int storeID;
        private static int formatID;
        private static int regionID;
        private static int cashierRangeFrom;
        private static int cashierRangeTo;
        private static int cardUseThreshold;
        private static int storeGroupID;
        private static int clubcardType;
        private static int userID;
        private static Int64 manualTxnCriteria;
        private static Int64 pointsThreshold;
        private static Int64 numberOfCustomers;
        private static string orderList;
        private static string flag;
        private static int offerID;
        private static int reportType;
        private static decimal thresholdLimit;
        private static string agencyPartnerNumber;
        private static string partnerOutlet;
        private static int lastWeek;
        private static int activeLastWeeks;
        private static int periodFirstWeek;
        private static string sysDate;
        private static Boolean reportNeeded;
        public static int reportTimeout;
        public static string capabilityName;
        static string sFileName = "NGCReportScheduling";
        static string EventLog = "NGCReportScheduling";
        static string message;
        private static string country;
        private static string currentDate;
        private static int totalRows;
        private static int totalCols;

        private static int optinalUseStatus1 = 10;
        private static int optinalUseStatus2 = 11;
        private static int optinalUseStatus3 = 12;
        private static int optinalUseStatus4 = 13;
        private static int optinalUseStatus5 = 14;
        private static string optinalUseStatus1Desc;
        private static string optinalUseStatus2Desc;
        private static string optinalUseStatus3Desc;
        private static string optinalUseStatus4Desc;
        private static string optinalUseStatus5Desc;
    

        public static int TotalCols
        {
            get { return totalCols; }
            set { totalCols = value; }
        }
        public static int TotalRows
        {
            get { return totalRows; }
            set { totalRows = value; }
        }
        public static int ReportID
        {
            get { return reportID; }
            set { reportID = value; }
        }

        public static long ReportScheduleID
        {
            get { return reportScheduleID; }
            set { reportScheduleID = value; }
        }
        public static string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public static string ReportCultureCode
        {
            get { return reportCultureCode; }
            set { reportCultureCode = value; }
        }
        public static string DefaultCultureCode
        {
            get { return defaultCultureCode; }
            set { defaultCultureCode = value; }
        }
        public static string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public static string SqlJobName
        {
            get { return sqlJobName; }
            set { sqlJobName = value; }
        }

        public static string ReportName
        {
            get { return reportName; }
            set { reportName = value; }
        }

        public static string LocalizationPath
        {
            get { return localizationPath; }
            set { localizationPath = value; }
        }
        public static string ReportHeadings
        {
            get { return reportHeadings; }
            set { reportHeadings = value; }
        }
        public static string ReportParams
        {
            get { return reportParams; }
            set { reportParams = value; }
        }

        public static string ScheduleTimeHHMMSS
        {
            get { return scheduleTimeHHMMSS; }
            set { scheduleTimeHHMMSS = value; }
        }

        public static DateTime InsertDateTime
        {
            get { return insertDateTime; }
            set { insertDateTime = value; }
        }
        public static int InsertBy
        {
            get { return insertBy; }
            set { insertBy = value; }
        }

        public static DateTime AmendDateTime
        {
            get { return amendDateTime; }
            set { amendDateTime = value; }
        }
        public static int AmendBy
        {
            get { return amendBy; }
            set { amendBy = value; }
        }

        public static string EmailRecepients
        {
            get { return emailRecepients; }
            set { emailRecepients = value; }
        }
        public static string EmailFromAddress
        {
            get { return emailFromAddress; }
            set { emailFromAddress = value; }
        }
        public static string EmailBody
        {
            get { return emailBody; }
            set { emailBody = value; }
        }
        public static string ReportOutputDirectory
        {
            get { return reportOutputDirectory; }
            set { reportOutputDirectory = value; }
        }
        public static string ReportOutputFile
        {
            get { return reportOutputFile; }
            set { reportOutputFile = value; }
        }
        public static string RecurrenceType
        {
            get { return recurrenceType; }
            set { recurrenceType = value; }
        }

        public static string Country
        {
            get { return country; }
            set { country = value; }
        }

        public static string CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; }
        }
        public static string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        public static string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public static string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        public static int Week
        {
            get { return week; }
            set { week = value; }
        }
        public static string Period
        {
            get { return period; }
            set { period = value; }
        }
        public static string CashierID
        {
            get { return cashierID; }
            set { cashierID = value; }
        }
        public static string PreviousWeeks
        {
            get { return previousWeeks; }
            set { previousWeeks = value; }
        }
        public static int StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }
        public static int FormatID
        {
            get { return formatID; }
            set { formatID = value; }
        }
        public static int RegionID
        {
            get { return regionID; }
            set { regionID = value; }
        }
        public static int CashierRangeFrom
        {
            get { return cashierRangeFrom; }
            set { cashierRangeFrom = value; }
        }
        public static int CashierRangeTo
        {
            get { return cashierRangeTo; }
            set { cashierRangeTo = value; }
        }
        public static int CardUseThreshold
        {
            get { return cardUseThreshold; }
            set { cardUseThreshold = value; }
        }
        public static int StoreGroupID
        {
            get { return storeGroupID; }
            set { storeGroupID = value; }
        }
        public static int ClubcardType
        {
            get { return clubcardType; }
            set { clubcardType = value; }
        }
        public static int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public static Int64 ManualTxnCriteria
        {
            get { return manualTxnCriteria; }
            set { manualTxnCriteria = value; }
        }
        public static Int64 PointsThreshold
        {
            get { return pointsThreshold; }
            set { pointsThreshold = value; }
        }

        public static string OrderList
        {
            get { return orderList; }
            set { orderList = value; }
        }

        public static int ReportType
        {
            get { return reportType; }
            set { reportType = value; }
        }

        public static int OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }
        public static decimal ThresholdLimit
        {
            get { return thresholdLimit; }
            set { thresholdLimit = value; }
        }
        public static string AgencyPartnerNumber
        {
            get { return agencyPartnerNumber; }
            set { agencyPartnerNumber = value; }
        }
        public static string PartnerOutlet
        {
            get { return partnerOutlet; }
            set { partnerOutlet = value; }
        }
        public static int LastWeek
        {
            get { return lastWeek; }
            set { lastWeek = value; }
        }

        public static int ActiveLastWeeks
        {
            get { return activeLastWeeks; }
            set { activeLastWeeks = value; }
        }
        public static int PeriodFirstWeek
        {
            get { return periodFirstWeek; }
            set { periodFirstWeek = value; }
        }
        public static string SysDate
        {
            get { return sysDate; }
            set { sysDate = value; }
        }
        public static Boolean ReportNeeded
        {
            get { return reportNeeded; }
            set { reportNeeded = value; }
        }
        public static int ReportTimeout
        {
            get { return reportTimeout; }
            set { reportTimeout = value; }
        }
        public static string CapabilityName
        {
            get { return capabilityName; }
            set { capabilityName = value; }
        }
        public static SqlCommand cmdObj;
        public static SqlConnection connection;
        public static DataSet dsReport;
        public static SqlDataAdapter daReport;
        //public static ApplicationClass excel;
        public static Workbook workbook;
        public static Worksheet worksheet;
        public static int Row, Col;
        public static int inputHeaderCount;
        public static SqlDataReader drStoregroup;
        public static SqlDataReader drDietaryPref;
        public static SqlDataReader drUseStatus;
        public static SqlDataReader drCardType;
        public static Hashtable htHeaders;
        public static Hashtable htExcelProcesses;
        #endregion

        #region hash
        /// <summary>Convert XML to HashTable</summary>
        /// <param name="sXml"> XML data to convert into HashTable </param>
        /// <param name="objName"> Name of the node to search </param>
        /// <returns> Returns HashTable</returns>
        /// <remarks>This method accepts XML data in string format and converts into HashTable and returning the HashTable</remarks>
        public static Hashtable XMLToHashTable(string sXml, string nodeNametoSearch)
        {
            Hashtable ht = new Hashtable();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);

                XmlNodeList nodes = doc.SelectNodes(nodeNametoSearch);
                foreach (XmlNode node in nodes)
                {
                    for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                        {
                            if (node.ChildNodes.Item(i).ChildNodes.Count > 1) //&& node.ChildNodes.Item(i).NodeType != XmlNodeType.Document )
                            {
                                HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                            }
                            else
                            {
                                ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return ht;
        }

        private static void HandleChildNodes(Hashtable ht, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                    {
                        if (node.ChildNodes.Item(i).ChildNodes.Count > 1)
                        {
                            HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                        }
                        else
                        {
                            ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                        }
                    }
                }
            }
        }

        #endregion
        #region SendReport

        /// <summary>
        /// Method used for sending mail across, through SMTPSever
        /// </summary>
        /// <param name="msgFrom">Sender info</param>
        /// <param name="msgTo">Receiver info</param>
        /// <param name="msgCC">CC info</param>
        /// <param name="msgBcc">BCC info</param>
        /// <param name="msgSubject">Subject of the mail</param>
        /// <param name="msgBody">Body(content) of the mail</param>
        /// <param name="fileName">Attachment filename if any</param>
        /// <returns>A bool value indicating whether the mail was sent successfully or not</returns>

        public static bool SendReport(string msgFrom, string msgTo, string msgCC, string msgBcc, string msgSubject, string msgBody, string fileToAttach)
        {
            Attachment fileAttach;
            //MailAttachment fileAttach;
            MailMessage newMsg;
            bool success = false;
            try
            {
                newMsg = new MailMessage();
                if (msgFrom.Trim().Length == 0 || msgTo.Trim().Length == 0)
                {
                    return success;
                }
                //Regular expression that will use a pattern match to validate an e-mail address
                Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
                Match emailMatch = emailRegex.Match(msgTo);
                //checks if the mail id is in valid format and returns if not

                if (!emailMatch.Success)
                {
                    return success;
                }

                newMsg.From = new MailAddress(msgFrom);
                if (msgTo != "")
                {
                    string[] smsg = msgTo.Split(';');
                    foreach (string mailID in smsg)
                        newMsg.To.Add(new MailAddress(mailID));
                }
                if (msgCC != "")
                {
                    string[] smsg = msgCC.Split(';');
                    foreach (string mailID in smsg)
                        newMsg.CC.Add(new MailAddress(mailID));
                }
                if (msgBcc != "")
                {
                    newMsg.Bcc.Add(new MailAddress(msgBcc));
                }
                newMsg.Subject = msgSubject;
                newMsg.Body = msgBody;
                //checks if any valid file is attached 
                if (fileToAttach.Trim().Length != 0)
                {
                    fileAttach = new Attachment(fileToAttach);
                    newMsg.Attachments.Add(fileAttach);
                }
                //Gets the SMTPServer name from Configuration file
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Host = ConfigurationManager.AppSettings["NGCReportSMTPServer"];

                mSmtpClient.Send(newMsg);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        #endregion

        #region ReportSPs
        public static bool ExecuteReportSP()
        {
            try
            {
                CommonFunctions.MessageWriteToEventViewer(EventLog, "1", true);
                cmdObj = new SqlCommand();
                connection = new SqlConnection(ConnectionString);
                dsReport = new DataSet();
                daReport = new SqlDataAdapter();
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.CommandTimeout = ReportTimeout;
                cmdObj.Connection = connection;

                
                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                
                
                string condition = IncreamentDate(Convert.ToString(StartDate), Convert.ToString(EndDate), Convert.ToString(Week), Convert.ToString(Period));




                
                if (StartDate != "" && EndDate != "")
                {
                    string[] dates = condition.Split('_');
                    StartDate = dates[0];
                    EndDate = dates[1];
                }
                else if (Week != 0)
                {
                    Week = Convert.ToInt32(condition);
                }
                else
                {
                    Period = condition;
                }
                
                switch (ReportID)
                {
                    case 0:  //MT5 - Users with transactions for many points

                        cmdObj.CommandText = "USP_UsersTranManyPoints5";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Points", SqlDbType.BigInt, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@Points"].Value = ManualTxnCriteria;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 1:  //MT1 - Customers with many transactions
                        cmdObj.CommandText = "USP_GetCustomersManyTran1";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@NoofTxn", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@NoofTxn"].Value = ManualTxnCriteria;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 2:  //Points and Rewards by Store
                        cmdObj.CommandText = "USP_PointsAndRewardsByStore";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 3:  //Financial Liability
                        cmdObj.CommandText = "USP_FinancialLiability";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        break;

                    case 4:  //MT3 - Transactions for many points
                        cmdObj.CommandText = "USP_TransForManyPoints3";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Points", SqlDbType.BigInt, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@Points"].Value = ManualTxnCriteria;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 5:  //MT2 - Customers with transactions for many points
                        cmdObj.CommandText = "USP_CustomersTranManyPoints2";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Points", SqlDbType.BigInt, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@Points"].Value = ManualTxnCriteria;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 6:  //Collection Period Summary
                        cmdObj.CommandText = "USP_CollectionPeriodSummary";
                        cmdObj.Parameters.Add("@dateFrom", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@dateTo", SqlDbType.DateTime, 20);
                        cmdObj.Parameters["@dateFrom"].Value = StartDate;
                        cmdObj.Parameters["@dateTo"].Value = EndDate;
                        break;

                    case 7:  //Data Protection Opt-Outs
                        cmdObj.CommandText = "USP_DataProtectionOutput";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@StoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@StoreID"].Value = StoreID;
                        break;

                    case 8:  //MT6 - Transactions made by a specific user
                        cmdObj.CommandText = "USP_TransAllUsers6";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@UserID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@UserID"].Value = UserID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 9:  //MT4 - Users who have made many transactions
                        cmdObj.CommandText = "USP_UsersManyTrans4";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@NoofTxn", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@NoofTxn"].Value = ManualTxnCriteria;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 10:  //Membership by week
                        cmdObj.CommandText = "USP_MemberShipByWeek";
                        cmdObj.Parameters.Add("@OfferID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Culture", SqlDbType.Text, 5);
                        //OfferId is passed as ReportType from reportcheduling page
                        cmdObj.Parameters["@OfferID"].Value = ReportType;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
                        break;

                    case 11:  //Points Partner Summary
                        //Modified by Syed Amajdulla on 22nd Jan'2010 for callinng proper sp
                        //cmdObj.CommandText = "USP_PointsPartnerSummary";             
                        cmdObj.CommandText = "USP_RptPointsPartnerSummaryCsv";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 12:  //Billing Report
                        //Modified by Syed Amajdulla on 22nd Jan'2010 for callinng proper sp
                        //cmdObj.CommandText = "USP_BillingReport";
                        cmdObj.CommandText = "USP_RptBillingCsv";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@AgencyPartnerNumber", SqlDbType.BigInt, 15);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@AgencyPartnerNumber"].Value = AgencyPartnerNumber;
                        break;

                    case 13:  //Preview High Reward Customers
                        cmdObj.CommandText = "USP_HighRewardCustomers";
                        cmdObj.Parameters.Add("@PointsThreshold", SqlDbType.BigInt, 2);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@NumberOfCustomers", SqlDbType.Int, 2);

                        cmdObj.Parameters["@PointsThreshold"].Value = PointsThreshold;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@NumberOfCustomers"].Value = 0; //Outpur parameter
                        break;
                        
                    case 14:    //Address In Error
                        
                        cmdObj.CommandText = "USP_AddressInError";
                        cmdObj.Parameters.Add("@StoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters["@StoreID"].Value = StoreID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        
                        break;

                    case 15:    //Reissues Summary
                        cmdObj.CommandText = "USP_ReissueSummaryReport";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Flag", SqlDbType.Char, 1);
                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@Flag"].Value = ReportType.ToString();
                        break;

                    case 16:    //Points Balance Spilt
                        cmdObj.CommandText = "USP_GetPointsBalanceSplit";
                        cmdObj.Parameters.Add("@OrderList", SqlDbType.Text, 500);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Culture", SqlDbType.Text, 5);
                        cmdObj.Parameters["@OrderList"].Value = OrderList;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
                        break;

                    case 17:    //Configuration Management
                        break;

                    case 18:    //New Clubcards Activated
                        cmdObj.CommandText = "USP_NewClubcardsActivated";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 19:    //New Details Uploaded
                        cmdObj.CommandText = "USP_NewDetailsUploaded";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 20:    //Skeleton By Store
                        cmdObj.CommandText = "USP_SkeletonByStore";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        break;

                    case 21:    //Cards with high transaction/value
                        cmdObj.CommandText = "USP_CardsWithHighTransactions";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ReportType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ThresholdLimit", SqlDbType.Int, 2);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@ReportType"].Value = ReportType;
                        cmdObj.Parameters["@ThresholdLimit"].Value = ThresholdLimit;
                        break;

                    case 22:    //Partner Transactions
                        //Modified by Syed Amajdulla on 22nd Jan'2010 for callinng proper sp
                        //cmdObj.CommandText = "USP_PartnerTxnReport";                            
                        cmdObj.CommandText = "USP_RptPartnerTxnCsv";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@AgencyPartnerNumber", SqlDbType.Text, 20);
                        cmdObj.Parameters.Add("@PartnerOutletRef", SqlDbType.Text, 30);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@AgencyPartnerNumber"].Value = agencyPartnerNumber;
                        cmdObj.Parameters["@PartnerOutletRef"].Value = PartnerOutlet;
                        break;

                    case 23:    //Active Accounts
                        cmdObj.CommandText = "USP_ActiveAccounts";
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@LastWeek", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@CardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ActiveLastWeek", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Culture", SqlDbType.Text, 15);

                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@LastWeek"].Value = LastWeek;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@CardType"].Value = ClubcardType;
                        cmdObj.Parameters["@ActiveLastWeek"].Value = ActiveLastWeeks;
                        cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
                        break;

                    case 24:    //Summary Account Information                        
                        break;

                    case 25:    //Business Type                       
                        break;

                    case 26:    //Customer Flags  
                        cmdObj.CommandText = "USP_ReportCustomerFlag";
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@LastWeek", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@StoreGroupID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ClubcardType", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@ActiveLastWeek", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Culture", SqlDbType.Text, 15);

                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@LastWeek"].Value = LastWeek;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@StoreGroupID"].Value = StoreGroupID;
                        cmdObj.Parameters["@ClubcardType"].Value = ClubcardType;
                        cmdObj.Parameters["@ActiveLastWeek"].Value = ActiveLastWeeks;
                        cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
                        break;

                    case 27:    //Helpline Reporting                     
                        cmdObj.CommandText = "Usp_Rpt_GetHelpLineData";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@PreWeeks", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@culture", SqlDbType.Text, 15);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        if (Period == "") Period = "0";
                        if (PreviousWeeks == "") PreviousWeeks = "0";
                        cmdObj.Parameters["@Period"].Value = Convert.ToInt16(Period); ;
                        cmdObj.Parameters["@PreWeeks"].Value = Convert.ToInt16(PreviousWeeks);
                        cmdObj.Parameters["@culture"].Value = ReportCultureCode;
                        break;

                    case 29:  //Cashier Report Added for NGCV32 Req.No 007

                        cmdObj.CommandText = "USP_CashierTransactionReport";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        cmdObj.Parameters.Add("@TescoStoreID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@FormatID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@RegionID", SqlDbType.Int, 2);
                        cmdObj.Parameters.Add("@CashierID", SqlDbType.VarChar, 200);
                        cmdObj.Parameters.Add("@CashierIdFrom", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@CashierIdTo", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@CardUseThreshold", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@Culture", SqlDbType.Text, 15);

                        cmdObj.Parameters["@StartDate"].Value = StartDate;
                        cmdObj.Parameters["@EndDate"].Value = EndDate;
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = PreviousWeeks;
                        cmdObj.Parameters["@TescoStoreID"].Value = StoreID;
                        cmdObj.Parameters["@FormatID"].Value = FormatID;
                        cmdObj.Parameters["@RegionID"].Value = RegionID;
                        cmdObj.Parameters["@CashierID"].Value = CashierID;
                        cmdObj.Parameters["@CashierIdFrom"].Value = CashierRangeFrom;
                        cmdObj.Parameters["@CashierIdTo"].Value = CashierRangeTo;
                        cmdObj.Parameters["@CardUseThreshold"].Value = CardUseThreshold;
                        cmdObj.Parameters["@Culture"].Value = ReportCultureCode;

                        break;

                    case 30:  //Clubcard Registration Report -- V3.6

                        cmdObj.CommandText = "[USP_GetClubcardRegistrationReport]";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);

                        if (StartDate == "" && EndDate == "")
                        {
                            cmdObj.Parameters["@StartDate"].Value = DBNull.Value;
                            cmdObj.Parameters["@EndDate"].Value = DBNull.Value;
                        }
                        else
                        {
                            
                            cmdObj.Parameters["@StartDate"].Value = Convert.ToDateTime(StartDate, enGBCulture);
                            cmdObj.Parameters["@EndDate"].Value = Convert.ToDateTime(EndDate, enGBCulture);
                        };
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = "";
                        

                        break;

                    case 31:  //Promotion Code Report -- V3.6

                        cmdObj.CommandText = "[USP_GetPromotionCodeReport]";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);

                        System.Globalization.CultureInfo enGBCult = new System.Globalization.CultureInfo("en-GB");
                        if (StartDate == "" && EndDate == "")
                        {
                            cmdObj.Parameters["@StartDate"].Value = DBNull.Value;
                            cmdObj.Parameters["@EndDate"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmdObj.Parameters["@StartDate"].Value = Convert.ToDateTime(StartDate, enGBCult);
                            cmdObj.Parameters["@EndDate"].Value = Convert.ToDateTime(EndDate, enGBCult);
                        };
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = "";


                        break;
                    case 32:  //Customer Load Report -- V3.6

                        cmdObj.CommandText = "[USP_GetCustomerLoadReport]";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 20);
                        cmdObj.Parameters.Add("@Week", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 50);
                        cmdObj.Parameters.Add("@PrsWeeks", SqlDbType.VarChar, 10);
                        System.Globalization.CultureInfo enGBCltr = new System.Globalization.CultureInfo("en-GB");
                        if (StartDate == "" && EndDate=="")
                        {
                                 cmdObj.Parameters["@StartDate"].Value = DBNull.Value;
                                 cmdObj.Parameters["@EndDate"].Value = DBNull.Value;
                        }
                         else
                        {
                            cmdObj.Parameters["@StartDate"].Value = Convert.ToDateTime(StartDate, enGBCltr);
                            cmdObj.Parameters["@EndDate"].Value = Convert.ToDateTime(EndDate, enGBCltr);
                        };
                        
                        cmdObj.Parameters["@Week"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        cmdObj.Parameters["@PrsWeeks"].Value = "";


                        break;

                        //Points Earned Report v3.6

                    case 33:
                        cmdObj.CommandText = "[USP_GetEarnedPointsReport]";
                        cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 30);
                        cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 30);
                        cmdObj.Parameters.Add("@TescoCalenderID", SqlDbType.Int, 20);
                        cmdObj.Parameters.Add("@Period", SqlDbType.VarChar, 20);
                        

                        System.Globalization.CultureInfo enGBClt = new System.Globalization.CultureInfo("en-GB");
                        if (StartDate == "" && EndDate == "")
                        {
                            cmdObj.Parameters["@StartDate"].Value = DBNull.Value; 
                            cmdObj.Parameters["@EndDate"].Value = DBNull.Value; 
                        }
                        else
                        {
                            cmdObj.Parameters["@StartDate"].Value = Convert.ToDateTime(StartDate, enGBClt);
                            cmdObj.Parameters["@EndDate"].Value = Convert.ToDateTime(EndDate, enGBClt);
                        };
                        cmdObj.Parameters["@TescoCalenderID"].Value = Week;
                        cmdObj.Parameters["@Period"].Value = Period;
                        break;

                }
                
                daReport.SelectCommand = cmdObj;
                daReport.Fill(dsReport);
                
                if (ReportID == 27)
                {
                    reportName = "Helpline Report";
                }
                if (ReportID == 10)
                {
                    DataSet ds = new DataSet();
                    string Viewxml;
                    XmlDocument customerUseStatusDoc = new XmlDocument();                                    
                    ds = SqlHelper.ExecuteDataset(connectionString, "USP_GetOptionalCustStatus");
                    ds.Tables[0].TableName = "CustomerUseStatus";
                    Viewxml = ds.GetXml();
                    customerUseStatusDoc.LoadXml(Viewxml);
                    customerUseStatusDoc = XmlDocToGridXmlDoc(customerUseStatusDoc, "NewDataSet", "CustomerUseStatus");
                    XmlNode optionalStatus1Node = customerUseStatusDoc.SelectSingleNode("//NewDataSet/CustomerUseStatus[@CustomerUseStatusID='" + optinalUseStatus1 + "']");
                    XmlNode optionalStatus2Node = customerUseStatusDoc.SelectSingleNode("//NewDataSet/CustomerUseStatus[@CustomerUseStatusID='" + optinalUseStatus2 + "']");
                    XmlNode optionalStatus3Node = customerUseStatusDoc.SelectSingleNode("//NewDataSet/CustomerUseStatus[@CustomerUseStatusID='" + optinalUseStatus3 + "']");
                    XmlNode optionalStatus4Node = customerUseStatusDoc.SelectSingleNode("//NewDataSet/CustomerUseStatus[@CustomerUseStatusID='" + optinalUseStatus4 + "']");
                    XmlNode optionalStatus5Node = customerUseStatusDoc.SelectSingleNode("//NewDataSet/CustomerUseStatus[@CustomerUseStatusID='" + optinalUseStatus5 + "']");

                    string localizedDataNoOf = Localization.GetLocalizedAttributeString("NGCMarketing.Reports.NoOf");
                    string localizedData1Acc = Localization.GetLocalizedAttributeString("NGCMarketing.Reports.Accounts");


                    int i = 16;
                    if (ReportCultureCode == "en-GB")
                    {

                        if (optionalStatus1Node != null)
                        {
                            optinalUseStatus1Desc = optionalStatus1Node.SelectSingleNode("CustomerUseStatusDescEnglish").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus1Desc + ' ' + localizedData1Acc;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus2Node != null)
                        {
                            optinalUseStatus2Desc = optionalStatus2Node.SelectSingleNode("CustomerUseStatusDescEnglish").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus2Desc + ' ' + localizedData1Acc;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus3Node != null)
                        {
                            optinalUseStatus3Desc = optionalStatus3Node.SelectSingleNode("CustomerUseStatusDescEnglish").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus3Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus4Node != null)
                        {
                            optinalUseStatus4Desc = optionalStatus4Node.SelectSingleNode("CustomerUseStatusDescEnglish").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus4Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus5Node != null)
                        {
                            optinalUseStatus5Desc = optionalStatus5Node.SelectSingleNode("CustomerUseStatusDescEnglish").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus5Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }

                    }
                    else
                    {
                        if (optionalStatus1Node != null)
                        {
                            optinalUseStatus1Desc = optionalStatus1Node.SelectSingleNode("CustomerUseStatusDescLocal").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus1Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }

                        if (optionalStatus2Node != null)
                        {
                            optinalUseStatus2Desc = optionalStatus2Node.SelectSingleNode("CustomerUseStatusDescLocal").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus2Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus3Node != null)
                        {
                            optinalUseStatus3Desc = optionalStatus3Node.SelectSingleNode("CustomerUseStatusDescLocal").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus3Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus4Node != null)
                        {
                            optinalUseStatus4Desc = optionalStatus4Node.SelectSingleNode("CustomerUseStatusDescLocal").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus4Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                        if (optionalStatus5Node != null)
                        {
                            optinalUseStatus5Desc = optionalStatus5Node.SelectSingleNode("CustomerUseStatusDescLocal").InnerXml.ToString();
                            dsReport.Tables[0].Columns[i].ColumnName = localizedDataNoOf + ' ' + optinalUseStatus5Desc + ' ' + localizedData1Acc; ;
                            htHeaders.Add(dsReport.Tables[0].Columns[i].ColumnName, dsReport.Tables[0].Columns[i].ColumnName);
                            i++;
                        }
                        else
                        {
                            dsReport.Tables[0].Columns.Remove(dsReport.Tables[0].Columns[i]);
                        }
                    }

                }
                
                return true;


            }
            catch (Exception e)
            {
                string message = "There is a problem in executing Stored Procedure of Report. " + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }

        }
        #endregion

        public static string IncreamentDate(string startdate, string enddate, string week, string period)
        {

            if (startdate != "" && enddate != "")
            {
                TimeSpan ts = System.DateTime.Now - insertDateTime;
                int days = ts.Days;
                if (days == 0)
                {
                    return startdate + "_" + enddate;
                }
                if (days > 0)
                {
                    string configValue = ConfigurationSettings.AppSettings["DailyBatchIncrementValue"].ToString();
                    System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                    DateTime dtStart = Convert.ToDateTime(startdate, enGBCulture);
                    DateTime dtEnd = Convert.ToDateTime(enddate, enGBCulture);
                    dtStart = dtStart.AddDays(days * Convert.ToInt32(configValue));
                    dtEnd = dtEnd.AddDays(days * Convert.ToInt32(configValue));

                    return dtStart.ToString("dd/MM/yyyy") + "_" + dtEnd.ToString("dd/MM/yyyy");
                }
            }
            if (week != "0")
            {
                DataSet dsWeek = new DataSet();
                dsWeek = GetWeekData(week);
                TimeSpan ts = System.DateTime.Now - insertDateTime;
                int days = ts.Days;
                int configValue = Convert.ToInt32(ConfigurationSettings.AppSettings["WeeklyBatchIncrementValue"].ToString());
                int diffdays = days / configValue;
                if (diffdays > 0)
                {
                    week = dsWeek.Tables[0].Rows[diffdays][0].ToString();
                }
                return week;
            }

            if (period != "")
            {
                DataSet dsPeriod = new DataSet();
                dsPeriod = GetPeriodData(period);
                TimeSpan ts = System.DateTime.Now - insertDateTime;
                int days = ts.Days;
                int configValue = Convert.ToInt32(ConfigurationSettings.AppSettings["PeriodBatchIncrementValue"].ToString());
                int diffdays = days / configValue;
                if (diffdays > 0)
                {
                    period = dsPeriod.Tables[0].Rows[diffdays][0].ToString();
                }
                return period;
            }
            return "";
        }


        #region WriteReportHeadings
        public static bool WriteReportHeadings()
        {
            try
            {
                Localization.Initialize(DefaultCultureCode, LocalizationPath);
                ReportOutputDirectory = Convert.ToString(ConfigurationManager.AppSettings["NGCReportOutputDirectory"]);
                RegionInfo countryInfo = new RegionInfo(new CultureInfo((string)DefaultCultureCode, false).LCID);
                Country = countryInfo.DisplayName.ToString();
                SysDate = DateTime.Now.ToString("dd/MM/yyyy");
                CurrentDate = DateTime.Now.ToString("dd-MM-yyyy");
                FileName = ReportName + "_" + UserName + "_" + CurrentDate;
                FileName = FileName.Replace(" ", "");
                FileName = FileName.Replace("/", "");
                Row = 1; Col = 1;
                worksheet.Cells[Row, Col] = Country;
                Row = Row + 1;
                worksheet.Cells[Row, Col] = ReportName;
                if (dsReport.Tables.Count == 0)
                {
                    TotalCols = 0;
                }
                else
                {
                    TotalCols = dsReport.Tables[0].Columns.Count;
                }
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[2, 4]).Font.Bold = true;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[2, 4]).Font.Size = 14;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[2, 4]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 4]).Merge(Type.Missing);
                worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[2, 4]).Merge(Type.Missing);
                Row = Row + 2;
                XmlDocument XmlReportHeadings = new XmlDocument();
                Hashtable htReportHeadings = new Hashtable();

                htReportHeadings = XMLToHashTable(ReportHeadings, "InputData");
                inputHeaderCount = htReportHeadings.Count;
                for (int HeadingIndex = 0; HeadingIndex < inputHeaderCount; HeadingIndex++)
                {
                    worksheet.Cells[Row, Col] = htReportHeadings["Input" + HeadingIndex].ToString();
                    HeadingIndex = HeadingIndex + 1;
                    Col = Col + 1;
                    if (StartDate !="" && EndDate != "")
                    {
                        htReportHeadings["Input" + HeadingIndex] = StartDate + "-" + EndDate;
                    }
                    else if (Week != 0)
                    {
                        htReportHeadings["Input" + HeadingIndex] = Week;
                    }
                    else if (Period != "")
                    {
                        htReportHeadings["Input" + HeadingIndex] = Period;
                    } 
                    worksheet.Cells[Row, Col] = htReportHeadings["Input" + HeadingIndex].ToString();
                    if (Col == 2)
                    {
                        Col = Col + 1;
                    }
                    else
                    {
                        Row = Row + 1; Col = 1;
                    }
                }
                if (Col == 2)
                {
                    Col = Col + 1;
                    worksheet.Cells[Row, Col] = "Report Run Date";
                    Col = Col + 1;
                    worksheet.Cells[Row, Col] = "'" + SysDate;
                }
                else
                {
                    worksheet.Cells[Row, Col] = "Report Run Date";
                    Col = Col + 1;
                    worksheet.Cells[Row, Col] = SysDate;
                }
                Row = Row + 2; ; Col = 1;
                if (TotalCols != 0)
                {
                    PopulateStoreGroup();
                    if (ReportID == 10)
                    {
                        PopulateOptionalUseStatus();
                    }
                    if (reportID != 30 && reportID != 31 && reportID != 32 && reportID != 33)
                    {
                        PopulateOptionalDietaryPref();
                        PopulateCardType();
                    }
                    if (ReportColumnHeadings(0) == false) return false;
                }
                return true;
            }
            catch (Exception e)
            {
                string message = "There is a problem in Writing Report Headings. " + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion
        #region ReportColumnHeading
        public static bool ReportColumnHeadings(int TableIndex)
        {
            string localizedHeader="";
            try
            {
                int colCount = dsReport.Tables[TableIndex].Columns.Count -1;
                foreach (DataColumn col in dsReport.Tables[TableIndex].Columns)
                {
                    string colHeader = col.ColumnName;
                    
                    if (inputHeaderCount > 0)
                    {
                        if (ReportID == 30)
                        {
                            if (col.ColumnName == "JoinRoute")
                            {
                                localizedHeader = Fujitsu.eCrm.Generic.LocalizationLibrary.Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);
                            }
                            else
                            {
                                localizedHeader = colHeader;
                            }
                        }
                        if (ReportID == 31)
                        {
                            if (col.ColumnName == "PromotionCode")
                            {
                                localizedHeader = Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);
                               
                            }
                            else if (col.ColumnName == "CodeDescription")
                            {
                                localizedHeader = Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);
                                
                            }
                            else
                            {
                                localizedHeader = colHeader;
                            }
                        }

                        else if (ReportID == 32)
                        {
                            localizedHeader = Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);
                            
                        }

                        if (reportID == 33)
                        {
                            localizedHeader = Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);

                           worksheet.get_Range(worksheet.Cells[Row, Col], worksheet.Cells[Row, Col]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                        }
                        
                    }
                    else
                    {
                        localizedHeader = Localization.GetLocalizedAttributeString("CSC." + CapabilityName + ".ColumnName." + colHeader);
                    }
                   
                     worksheet.Cells[Row, Col] = localizedHeader;                    
                    Col++;

                }
                if (ReportID == 30 || ReportID==31)
                {
                    worksheet.Cells[Row, Col] = "Total";
                    Col++;
                }
                return true;
            }
            catch (Exception e)
            {
                string message = "There is a problem in Writing Column Headings of Excel Sheet. " + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region GetReportScheduleDetailst
        public static bool GetReportScheduleDetails()
        {
            NGC.Utils.Trace trace = new NGC.Utils.Trace();
            TraceState trState = trace.StartProc("GetReportScheduleDetails");
            int returnCode;

            try
            {
                InsertDateTime = DateTime.Now;
                AmendDateTime = DateTime.Now;
                object[] objReportSchedule = 
                        {  
                            ReportID,
                            UserName,  
                            ReportCultureCode,
                            DefaultCultureCode,
                            ReportScheduleID,
                            SqlJobName,
                            ReportName,
                            ReportParams,
                            ReportHeadings,
                            LocalizationPath,
                            RecurrenceType,
                            ScheduleTimeHHMMSS,
                            EmailRecepients,
                            InsertDateTime,
                            InsertBy,
                            AmendDateTime,
                            AmendBy  ,
                            CapabilityName
                         };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, "USP_ViewReportSchedule", ref objReportSchedule);
                ReportCultureCode = Convert.ToString(objReportSchedule[2]);
                DefaultCultureCode = Convert.ToString(objReportSchedule[3]);
                ReportScheduleID = Convert.ToInt16(objReportSchedule[4]);
                SqlJobName = Convert.ToString(objReportSchedule[5]);
                ReportName = Convert.ToString(objReportSchedule[6]);
                ReportParams = Convert.ToString(objReportSchedule[7]);
                ReportHeadings = Convert.ToString(objReportSchedule[8]);
                LocalizationPath = Convert.ToString(objReportSchedule[9]);
                RecurrenceType = Convert.ToString(objReportSchedule[10]);
                ScheduleTimeHHMMSS = Convert.ToString(objReportSchedule[11]);
                EmailRecepients = Convert.ToString(objReportSchedule[12]);
                InsertDateTime = Convert.ToDateTime(objReportSchedule[13]);
                InsertBy = Convert.ToInt16(objReportSchedule[14]);
                AmendDateTime = Convert.ToDateTime(objReportSchedule[15]);
                AmendBy = Convert.ToInt16(objReportSchedule[16]);
                ReportNeeded = true;
                CapabilityName = objReportSchedule[17].ToString();
                if (RecurrenceType == "P")
                {
                    object[] objTescoPeriod = { PeriodFirstWeek };
                    returnCode = DataAccess.ExecuteNonQuery(connectionString, "USP_GetPeriodFirstWeek", ref objTescoPeriod);
                    PeriodFirstWeek = Convert.ToInt16(objTescoPeriod[0]);
                    if (PeriodFirstWeek == 0) ReportNeeded = false;
                }
                return true;
            }
            catch (Exception e)
            {
                string message = "There is a problem in getting Report Schedule Details" + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region GetReportParameterValues
        public static bool GetReportParameterValues()
        {

            NGC.Utils.Trace trace = new NGC.Utils.Trace();
            TraceState trState = trace.StartProc("GetReportParameterValues");

            try
            {
                XmlDocument XmlReportParams = new XmlDocument();
                Hashtable htReport = new Hashtable();
                htReport = XMLToHashTable(ReportParams, "ReportParams");
                string param = "";
                string value = "";
                int inputCount = htReport.Count;

                // set default parameter values - Added by Syed on 20th Jan'2010
                StartDate = null;
                EndDate = null;
                Week = 0;
                LastWeek = 0;
                PreviousWeeks = "";
                Period = "";
                StoreID = -2;
                FormatID = -1;
                RegionID = -2;
                StoreGroupID = -1;
                ClubcardType = -2;
                if (ReportID == 16) // Points Balance Split Report
                {
                } ClubcardType = -3;
                OrderList = "";
                AgencyPartnerNumber = null;
                PartnerOutlet = null;
                ReportType = 1;
                ManualTxnCriteria = 0;
                PointsThreshold = 0;
                ThresholdLimit = 0;
                CardUseThreshold = 0;
                CashierID = null;
                CashierRangeFrom = 0;
                CashierRangeTo = 0;
                #region SetReportInput
                for (int i = 0, j = 1; i < inputCount; i += 2, j += 2)
                {
                    param = htReport["Input" + i].ToString();

                    value = htReport["Input" + j].ToString();
                    switch (param)
                    {
                        case "StartDate":

                            StartDate = value;
                            break;
                        case "EndDate":
                            EndDate = value;
                            break;
                        case "Week":
                            Week = Convert.ToInt16(value);
                            break;
                        case "LastWeek":
                            LastWeek = Convert.ToInt16(value);
                            break;
                        case "PreviousWeeks":
                            PreviousWeeks = value;
                            break;
                        case "ActiveLastWeeks":
                            ActiveLastWeeks = Convert.ToInt16(value);
                            break;

                        case "Period":
                            Period = value;
                            break;
                        case "TescoStoreID":
                            StoreID = Convert.ToInt16(value);
                            break;
                        case "FormatID":
                            FormatID = Convert.ToInt16(value);
                            break;
                        case "RegionID":
                            RegionID = Convert.ToInt16(value);
                            break;
                        case "StoreGroupID":
                            StoreGroupID = Convert.ToInt16(value);
                            break;
                        case "ClubcardType":
                            ClubcardType = Convert.ToInt16(value);
                            break;
                        case "UserID":
                            UserID = Convert.ToInt16(value);
                            break;
                        case "CashierID":
                            CashierID = value;
                            break;
                        case "CashierIDFrom":
                            CashierRangeFrom = Convert.ToInt16(value);
                            break;
                        case "CashierIDTo":
                            CashierRangeTo = Convert.ToInt16(value);
                            break;
                        case "CardUseThreshold":
                            CardUseThreshold = Convert.ToInt16(value);
                            break;
                        case "Culture":
                            if (value.Length != 0)
                            {
                                ReportCultureCode = value;
                            }
                            break;
                        case "OrderList":
                            OrderList = value;
                            break;
                        case "AgencyPartnerNumber":
                            AgencyPartnerNumber = value;
                            break;
                        case "PartnerOutlet":
                            PartnerOutlet = value;
                            break;
                        case "ReportType":
                            ReportType = Convert.ToInt16(value);
                            break;
                        case "ManualTxnCriteria":
                            ManualTxnCriteria = Convert.ToInt64(value);
                            break;
                        case "PointsThreshold":
                            PointsThreshold = Convert.ToInt64(value);
                            break;
                        case "ThresholdLimit":
                            ThresholdLimit = Convert.ToDecimal(value);
                            break;

                    }
                }
                #endregion
                return true;
            }
            catch (Exception e)
            {
                string message = "There is a problem in getting Report Parameter Values" + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #region GenerateReport
        public static void GenerateReport()
        {
            int excelProcessID = 0;
            bool GenerateSucces = false;
            htHeaders = new Hashtable();
            Hashtable processHashtable = new Hashtable();
            int tablesCount  = 0,count = 0;
            try
            {
                if (GetReportScheduleDetails() == true)
                {
                    if (ReportNeeded)
                    {
                        if (GetReportParameterValues() == true)
                        {
                            if (ExecuteReportSP() == true)
                            {
                                //Manipulating excel sheet
                                #region ExcelSheet
                                CheckExcellProcesses();
                                //excel = new ApplicationClass();
                                ////if (excel == null)
                                ////{
                                ////    throw (new Exception("Unable to Start Microsoft Excel."));
                                ////}
                                ////excel.DisplayAlerts = false;
                                workbook = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Type.Missing);
                                //Modified by Syed Amjadulla on 12th Dec'2009 for formatting fo excel sheet
                                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                                //Added by Syed Amajdulla on 24th Jan'2010
                                if (WriteReportHeadings() == true)
                                {
                                 
                                    if (ReportID == 32)
                                    {
                                        string reportData = dsReport.GetXml();
                                        XmlDocument reportXml = new XmlDocument();
                                        reportXml.LoadXml(reportData);
                                        XmlNodeList clubcardNode = reportXml.SelectNodes("//ClubcardAccountNumber");
                                        foreach (XmlNode accountNode in clubcardNode)
                                        {
                                            accountNode.InnerText = "'" + accountNode.InnerText;
                                        }
                                        XmlNodeList primaryclubcardNode = reportXml.SelectNodes("//PrimaryCardAccountNumber");
                                        foreach (XmlNode accountNode in primaryclubcardNode)
                                        {
                                            accountNode.InnerText = "'" + accountNode.InnerText;
                                        }
                                        DataSet dsLoadReport = new DataSet();
                                        dsLoadReport.ReadXml(new XmlNodeReader(reportXml));
                                        tablesCount = dsLoadReport.Tables.Count;
                                        //foreach (System.Data.DataTable Table in dsReport.Tables)
                                        for (int tables = 0; tables < tablesCount; tables++)
                                        {
                                            TotalRows = dsLoadReport.Tables[tables].Rows.Count;
                                            foreach (DataRow row in dsLoadReport.Tables[tables].Rows)
                                            {
                                                Col = 0;
                                                foreach (DataColumn col in dsLoadReport.Tables[tables].Columns)
                                                {
                                                    Col++;
                                                    // Added by Syed Amjadulla on 10th Dec'2009 for setting fromat for Card Numbers                                
                                                    if (tables == 0 && ((Col == 1 && ReportID == 13) || //Preview High Reward Customers
                                                     (Col == 2 && (ReportID == 1 || ReportID == 5 || ReportID == 4 || ReportID == 21)) || //Manual Transactions 1, 2, 3 and Cards with high transaction value
                                                        (Col == 3 && (ReportID == 0 || ReportID == 8 || ReportID == 9) || ReportID == 29) || //Manual Transactions 4, 5 and 6, Casher Report
                                                        (Col == 10 && ReportID == 22) || // Partner Transactions Report
                                                        (Col == 9 && ReportID == 12)))   // Billing Report
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = "'" + row[col.ColumnName];
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = row[col.ColumnName];
                                                    }
                                                }

                                                Row++;
                                            }
                                            
                                        }
                                    }

                                   else if (ReportID == 30)
                                    {
                                        tablesCount = dsReport.Tables.Count;
                                        Row = Row + 1;
                                        for (int m = 0; m < tablesCount; m++)
                                        {
                                            TotalRows = dsReport.Tables[m].Columns.Count;
                                            foreach (DataRow dr in dsReport.Tables[m].Rows)
                                            {
                                                Col = 1;
                                                int total = 0;
                                                for (int k = 0; k <= TotalRows; k++)
                                                {
                                                    if (k < TotalRows)
                                                    {
                                                        worksheet.Cells[Row, Col] = dr[k];
                                                        if (k > 0)
                                                            total = total + Convert.ToInt32(dr[k]);
                                                        Col++;
                                                    }
                                                    else if (k == TotalRows)
                                                    {
                                                        worksheet.Cells[Row, Col] = total.ToString();
                                                    }
                                                }

                                                Row++;
                                            }
                                        }
                                        Col = 2;
                                        int cols = dsReport.Tables[0].Columns.Count;
                                        int rows = dsReport.Tables[0].Rows.Count;
                                        int sum = 0;    
                                        for (int h = 0; h < cols; h++)
                                        {
                                            if (h > 0)
                                            {
                                                sum = 0;
                                                for (int g = 0; g < rows; g++)
                                                {
                                                    sum = sum + Convert.ToInt32(dsReport.Tables[0].Rows[g][h].ToString());
                                                }
                                                worksheet.Cells[Row + 1, Col - 1] = sum;
                                                Col++;
                                            }
                                            else
                                            {
                                                worksheet.Cells[Row + 1, Col - 1] = "Total";
                                                Col++;
                                            }
                                        }

                                        
                                            Range range;
                                            string str = "";
                                            range = worksheet.UsedRange;
                                            int totalRows = range.Rows.Count;
                                            int totalColumn = range.Columns.Count;
                                            int totalCount = 0;
                                            //for (int rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                                            //{
                                            for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                                            {
                                                str = Convert.ToString(((range.Cells[totalRows, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2));
                                                if (str != "" && str != "Total")
                                                {
                                                    totalCount = Convert.ToInt16(str) + totalCount;
                                                }
                                            }


                                            worksheet.Cells[totalRows - 1, totalColumn] = totalCount.ToString();
                                            //}
                                        
                                    }
                                    else  if (reportID == 31)
                                    {
                                        tablesCount = dsReport.Tables.Count;
                                        Row = Row + 1;
                                        for (int m = 0; m < tablesCount; m++)
                                        {
                                            TotalRows = dsReport.Tables[m].Columns.Count;
                                            foreach (DataRow dr in dsReport.Tables[m].Rows)
                                            {
                                                Col = 1;
                                                int total = 0;
                                                for (int k = 0; k <= TotalRows; k++)
                                                {
                                                    if (k < TotalRows)
                                                    {
                                                        worksheet.Cells[Row, Col] = dr[k];
                                                        if (k > 1)
                                                            total = total + Convert.ToInt32(dr[k]);
                                                        Col++;
                                                    }
                                                    else if (k == TotalRows)
                                                    {
                                                        worksheet.Cells[Row, Col] = total.ToString();
                                                    }
                                                }

                                                Row++;
                                            }
                                        }
                                       

                                    }


                                   else if (reportID == 33)
                                    {

                                        string reportData = dsReport.GetXml();
                                        XmlDocument reportXml = new XmlDocument();
                                        reportXml.LoadXml(reportData);
                                        XmlNodeList DateNode = reportXml.SelectNodes("//Date");
                                        foreach (XmlNode pointsdateNode in DateNode)
                                        {
                                            pointsdateNode.InnerText = "'" + pointsdateNode.InnerText;
                                        }
                                        XmlNodeList TotalPointsNode = reportXml.SelectNodes("//PointsEarned");
                                        foreach (XmlNode pointsdateNode in TotalPointsNode)
                                        {
                                            pointsdateNode.InnerText = "'" + pointsdateNode.InnerText;
                                        }
                                        DataSet dsLoadReport = new DataSet();
                                        dsLoadReport.ReadXml(new XmlNodeReader(reportXml));
                                        tablesCount = dsLoadReport.Tables.Count;
                                        //foreach (System.Data.DataTable Table in dsReport.Tables)
                                        for (int tables = 0; tables < tablesCount; tables++)
                                        {
                                            TotalRows = dsLoadReport.Tables[tables].Rows.Count;

                                            foreach (DataRow row in dsLoadReport.Tables[tables].Rows)
                                            {
                                                Col = 0;

                                                foreach (DataColumn col in dsLoadReport.Tables[tables].Columns)
                                                {

                                                    Col++;

                                                    // Added by Syed Amjadulla on 10th Dec'2009 for setting fromat for Card Numbers                                
                                                    if (tables == 0 && ((Col == 1 && ReportID == 13) || //Preview High Reward Customers
                                                     (Col == 2 && (ReportID == 1 || ReportID == 5 || ReportID == 4 || ReportID == 21)) || //Manual Transactions 1, 2, 3 and Cards with high transaction value
                                                        (Col == 3 && (ReportID == 0 || ReportID == 8 || ReportID == 9) || ReportID == 29) || //Manual Transactions 4, 5 and 6, Casher Report
                                                        (Col == 10 && ReportID == 22) || // Partner Transactions Report
                                                        (Col == 9 && ReportID == 12)))   // Billing Report
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = "'" + row[col.ColumnName];
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = row[col.ColumnName];
                                                        worksheet.get_Range(worksheet.Cells[Row+1, Col], worksheet.Cells[Row+1, Col]).HorizontalAlignment = XlHAlign.xlHAlignRight;

                                                    }

                                                }

                                                Row++;

                                            }

                                        }
                                     
                                    }

                                    else
                                    {
                                        tablesCount = dsReport.Tables.Count;
                                        //foreach (System.Data.DataTable Table in dsReport.Tables)
                                        for (int tables = 0; tables < tablesCount; tables++)
                                        {
                                            TotalRows = dsReport.Tables[tables].Rows.Count;
                                            foreach (DataRow row in dsReport.Tables[tables].Rows)
                                            {
                                                Col = 0;
                                                foreach (DataColumn col in dsReport.Tables[tables].Columns)
                                                {
                                                    Col++;
                                                    // Added by Syed Amjadulla on 10th Dec'2009 for setting fromat for Card Numbers                                
                                                    if (tables == 0 && ((Col == 1 && ReportID == 13) || //Preview High Reward Customers
                                                     (Col == 2 && (ReportID == 1 || ReportID == 5 || ReportID == 4 || ReportID == 21)) || //Manual Transactions 1, 2, 3 and Cards with high transaction value
                                                        (Col == 3 && (ReportID == 0 || ReportID == 8 || ReportID == 9) || ReportID == 29) || //Manual Transactions 4, 5 and 6, Casher Report
                                                        (Col == 10 && ReportID == 22) || // Partner Transactions Report
                                                        (Col == 9 && ReportID == 12)))   // Billing Report
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = "'" + row[col.ColumnName];
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[Row + 1, Col] = row[col.ColumnName];
                                                    }
                                                }

                                                Row++;
                                            }
                                            if (ReportID == 12)
                                            {
                                                if (tablesCount > 1 && tables == 0)
                                                {
                                                    string ColHeading = dsReport.Tables[tables + 1].Columns[0].ColumnName.ToUpper();

                                                    if (ColHeading.Equals("TOTAL") || ColHeading.Equals("COLUMN1"))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        Col = 1;
                                                        Row++; Row++;
                                                        ReportColumnHeadings(1);
                                                    }
                                                }
                                            }


                                        }
                                    }
                                }
                                worksheet.Columns.AutoFit();

                                cmdObj.Connection.Close();
                                ReportOutputFile = ReportOutputDirectory + FileName + ".xlsx";
                                if (File.Exists(ReportOutputFile))
                                {
                                    File.Delete(ReportOutputFile);
                                }
                                workbook.Saved = true;
                                workbook.SaveCopyAs(ReportOutputFile);
                                //workbook.SaveAs(ReportOutputFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                message = "There is a problem in generating Report - " + e.Message;
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                GenerateSucces = false;
            }
            finally
            {
                // Prevent an orphaned Excel process by forcibly killing it.
                workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                KillExcel();
                //System.Runtime.InteropServices.Marshal.ReleaseComO bject(oRng);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                workbook = null;
                
                GC.Collect(); // force final cleanup!     
            }
        }

        #endregion

        #region Messages
        private static void Help()
        {
            Console.WriteLine("NGCReportFormatter can be called with three different sets of options");
            BlankLine();
            Console.WriteLine("(1) NGCReportFormatter -r <ReportID>");
            Console.WriteLine("                       -u <UserName>");
            BlankLine();
        }

        private static void ServerError(string resultXml)
        {
            Result serverResult = new Result();
            serverResult.LoadXml(resultXml);
            string uiMessage;
            serverResult.MoveToTopResult();
            serverResult.GetResultElementByName("ui_message", out uiMessage);
            Console.WriteLine(uiMessage);
        }

        private static void ToManyParameter()
        {
            Console.WriteLine("Too many parameters have been supplied");
            BlankLine();
        }

        private static void MissingParameter(string parameter)
        {
            Console.WriteLine("The parameter '" + parameter + "' must be supplied");
            BlankLine();
        }

        private static void DuplicateParameter(string parameter)
        {
            Console.WriteLine("The parameter '" + parameter + "' has been supplied more than once");
            BlankLine();
        }

        private static void NoParameter()
        {
            Console.WriteLine("No parameters have been supplied");
            BlankLine();
        }

        private static void EmptyParameter(string parameter)
        {
            Console.WriteLine("No value has been supplied for the '" + parameter + "' parameter");
            BlankLine();
        }

        private static void UnknownScript(string scriptname)
        {
            Console.WriteLine("The script '" + scriptname + "' is not available");
            BlankLine();
        }

        private static void BlankLine()
        {
            Console.WriteLine("");
        }

        private static void InvalidUserCredentials()
        {
            Console.WriteLine("Invalid User Name or Password");
            BlankLine();
        }

        private static void LogDetails(string scriptname, string logFile)
        {
            Console.WriteLine("The log of " + scriptname + " is at " + logFile);
            BlankLine();
        }

        private static bool AddToHtAndCheck(ref Hashtable parameters, string parameter, string parameterValue, bool checkDuplicate, bool checkEmpty)
        {
            if (checkDuplicate)
            {
                if (parameters.ContainsKey(parameter))
                {
                    DuplicateParameter(parameter);
                    return false;
                }
            }
            if (checkEmpty)
            {
                if (StringUtils.IsStringEmpty(parameterValue))
                {
                    EmptyParameter(parameter);
                    return false;
                }
            }
            if (parameters.ContainsKey(parameter))
            {
                parameters[parameter] = parameterValue;
            }
            else
            {
                parameters.Add(parameter, parameterValue);
            }
            return true;
        }

        private static bool AddToHtAndCheck(ref Hashtable parameters, string parameter, string parameterValue)
        {
            return AddToHtAndCheck(ref parameters, parameter, parameterValue, true, true);
        }
        #endregion
        #region PopulateStoreGroup
        private static void PopulateStoreGroup()
        {
            connection.Open();
            cmdObj.CommandText = "USP_ViewStoreGroups";
            cmdObj.Parameters.Clear();
            cmdObj.Parameters.Add("@Culture", SqlDbType.Char, 15);
            cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
            cmdObj.Connection = connection;
            cmdObj.CommandType = CommandType.StoredProcedure;
            drStoregroup = cmdObj.ExecuteReader(CommandBehavior.Default);
            while (drStoregroup.Read())
            {
                htHeaders.Add(drStoregroup.GetValue(1).ToString(), drStoregroup.GetValue(1).ToString());
            }
            connection.Close();
        }
        #endregion
        #region PopulateOptionalDietaryPreferences
        private static void PopulateOptionalDietaryPref()
        {
            
            connection.Open();
            cmdObj.CommandText = "USP_ViewOptionalDietaryPref";
            cmdObj.Parameters.Clear();
            cmdObj.Parameters.Add("@Culture", SqlDbType.Char, 15);
            cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
            cmdObj.Connection = connection;
            cmdObj.CommandType = CommandType.StoredProcedure;
            drDietaryPref = cmdObj.ExecuteReader(CommandBehavior.Default);

            while (drDietaryPref.Read())
            {
                htHeaders.Add(drDietaryPref.GetValue(0).ToString(), drDietaryPref.GetValue(0).ToString());
            }
            connection.Close();
        }
        #endregion
        #region PopulateOptionalUseStatus
        private static void PopulateOptionalUseStatus()
        {
            connection.Open();
            cmdObj.CommandText = "USP_ViewOptionalUseStatus";
            cmdObj.Parameters.Clear();
            cmdObj.Parameters.Add("@Culture", SqlDbType.Char, 15);
            cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
            cmdObj.Connection = connection;
            cmdObj.CommandType = CommandType.StoredProcedure;
            drUseStatus = cmdObj.ExecuteReader(CommandBehavior.Default);
            int iCol = 16; // membership by week report
            string strUseDesc;
            string localizedDataNoOf = Localization.GetLocalizedAttributeString("NGCMarketing.Reports.NoOf");
            string localizedData1Acc = Localization.GetLocalizedAttributeString("NGCMarketing.Reports.Accounts");
            while (drUseStatus.Read())
            {
                strUseDesc = localizedDataNoOf + ' ' + drUseStatus.GetValue(0).ToString() + ' ' + localizedData1Acc;
                dsReport.Tables[0].Columns[iCol].ColumnName = strUseDesc;
                htHeaders.Add(strUseDesc, strUseDesc);
                iCol++;
            }
            connection.Close();
        }
        #endregion
        #region PopulateCardType
        private static void PopulateCardType()
        {

            
                connection.Open();
            
            cmdObj.CommandText = "USP_ViewCardType";
            cmdObj.Parameters.Clear();
            cmdObj.Parameters.Add("@Culture", SqlDbType.Char, 15);
            cmdObj.Parameters["@Culture"].Value = ReportCultureCode;
            cmdObj.Connection = connection;
            cmdObj.CommandType = CommandType.StoredProcedure;
            drCardType = cmdObj.ExecuteReader(CommandBehavior.Default);
            while (drCardType.Read())
            {
                string ccardType = drCardType.GetValue(1).ToString();
                ccardType = ccardType.Replace(" ", "");
                htHeaders.Add(ccardType, ccardType);
            }
            connection.Close();
        }
        #endregion
        #region ExcelProcesses
        public static void CheckExcellProcesses()
        {
            Process[] AllProcesses = Process.GetProcessesByName("EXCEL");
            htExcelProcesses = new Hashtable();
            int iCount = 0;
            foreach (Process ExcelProcess in AllProcesses)
            {
                htExcelProcesses.Add(ExcelProcess.Id, iCount);
                iCount = iCount + 1;
            }
        }

        public static void KillExcel()
        {
            Process[] AllProcesses = Process.GetProcessesByName("EXCEL");
            // check to kill the right process
            foreach (Process ExcelProcess in AllProcesses)
            {
                if (htExcelProcesses.ContainsKey(ExcelProcess.Id) == false)
                    ExcelProcess.Kill();
            }

            AllProcesses = null;
        }

        #endregion

        #region Get Grid XmlDocument

        public static XmlDocument XmlDocToGridXmlDoc(XmlDocument xmlDoc, string rootElement, string childElement)
        {
            XmlDocument gridXmlDoc = new XmlDocument();
            try
            {
                bool setAttribute;
                XmlNodeList nodes = xmlDoc.SelectNodes(rootElement);

                XmlElement root = gridXmlDoc.CreateElement(rootElement);
                gridXmlDoc.AppendChild(root);

                foreach (XmlNode node in nodes)
                {
                    foreach (XmlElement element in node)
                    {
                        setAttribute = false;
                        XmlElement elem = gridXmlDoc.CreateElement(element.Name);
                        foreach (XmlElement childElem in element)
                        {
                            if (!setAttribute)
                            {
                                elem.SetAttribute(childElem.Name, childElem.InnerText);
                                setAttribute = true;
                                continue;
                            }

                            XmlElement elem2 = gridXmlDoc.CreateElement(childElem.Name);
                            elem2.InnerText = childElem.InnerText;
                            elem.AppendChild(elem2);
                        }
                        root.AppendChild(elem);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return gridXmlDoc;
        }

        #endregion

       
        static void Main(string[] args)
        {

            //string message;
            try
            {
                #region Parse Argument List

                ReportTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
                //Modified by Syed Amjadulla on 5th Feb'2010 for Report DB Connection string as per Req of NGC V 3.2                
                ConnectionString = Convert.ToString(ConfigurationManager.AppSettings["ReportDBNGCConnectionString"]);
                //ConnectionString = Convert.ToString(ConfigurationManager.AppSettings["AdminConnectionString"]);

                string argument = String.Join("", args);
                Regex argumentExpression = new Regex(@"\-(?<option>[\w|\?])\s*(?<value>[\w|\?]*)");

                // Set-up variables for number of parameters and a hashtable
                // for storing all parameters.
                bool parameterError = false;
                Hashtable parameters = new Hashtable(5);
                // Get each of the parameters. Check for duplication and content
                // on each.
                foreach (Match argumentMatch in argumentExpression.Matches(argument))
                {
                    string option = argumentMatch.Groups["option"].Value;
                    string value = argumentMatch.Groups["value"].Value;
                    switch (option)
                    {
                        case Constants.PARAMETER_REPORT:
                            if (!AddToHtAndCheck(ref parameters, option, value))
                                parameterError = true;
                            ReportID = int.Parse(value);
                            break;

                        case Constants.PARAMETER_USER:
                            if (!AddToHtAndCheck(ref parameters, option, value))
                                parameterError = true;
                            UserName = value;
                            break;

                        case Constants.PARAMETER_LIST:
                            if (!AddToHtAndCheck(ref parameters, option, value, false, false))
                                parameterError = true;
                            break;
                        default:
                            break;
                    }
                    // If there has been an error with one of the parameters then stop.
                    if (parameterError)
                    {
                        Help();
                        return;
                    }
                }
                #endregion

                string ebody = string.Empty;
                GenerateReport();

                EmailFromAddress = ConfigurationManager.AppSettings["NGCReportsMailFrom"];

                //Added by santosh as part of know issues fix
                ebody = Localization.GetLocalizedAttributeString("NGCMarketing.Reports.EmailBody");
                ebody += Environment.NewLine;
                RegionInfo countryInfo = new RegionInfo(new CultureInfo((string)ReportCultureCode, false).LCID);
                ebody += ("Country: " + countryInfo.DisplayName.ToString() + Environment.NewLine);
                if (recurrenceType == "D")
                    ebody += ("Recurrence type: " + "Daily Report" + Environment.NewLine);
                else if (recurrenceType == "W")
                    ebody += ("Recurrence type: " + "Weekly Report" + Environment.NewLine);
                else
                    ebody += ("Recurrence type: " + "Periodic Report");

                EmailBody = ebody;
                //Report details : Report type, Country, Cards (all or range), Stores (all or individual store name), Frequency etc
                if (SendReport(EmailFromAddress, EmailRecepients, "", "", FileName, EmailBody, ReportOutputFile) == true)
                {
                    message = "Email with " + ReportOutputFile + " successfully sent.";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLog, message, false);
                }
                else
                {
                    message = "Could not send Email with " + ReportOutputFile;
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer("NGCReportScheduling", message, true);
                }
            }

            catch (Exception ex)
            {
                message = ex.Message;
                CommonFunctions.MessageWriteToEventViewer("NGCReportScheduling", message, true);

            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static DataSet GetWeekData(string week)
        {
            string connectionString = ConfigurationSettings.AppSettings["AdminConnectionString"];
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select TescoCalendarID,'Week ' from dbo.TescoCalendar WHERE TescoCalendarID >= " + week + " ORDER BY Week", connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.SelectCommand = command;

                da.Fill(ds);
            }
            return ds;
        }

        private static DataSet GetPeriodData(string period)
        {
            string connectionString = ConfigurationSettings.AppSettings["AdminConnectionString"];
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Select Distinct Convert(Varchar,TescoYear) + '-' + Convert(Varchar,Period) PeriodYear From TescoCalendar WHERE Convert(Varchar,TescoYear) + '-' + Convert(Varchar,Period) > '2012-10' ORDER BY Convert(Varchar,TescoYear) + '-' + Convert(Varchar,Period)", connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            return ds;
        }
    }


}
