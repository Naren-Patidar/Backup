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
using System.IO;

namespace Tesco.NGC.BatchConsoleApplication
{
    public class ReIssueReward
    {
        #region Fields
        // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
        //public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);        
        string OutPutDirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string BCPDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string sBatch = "Reissue-Rewards";
        bool retunValue;
        string FileName;
        private string culture;


        private DateTime previousOfferstartDate;
        private DateTime previousOfferendDate;
        private short previousOfferID;
        private Int64 noOfRewardReIssueMailProcessed;
        private Int64 totalVouchersReIssuedProcessed;
        private double totalVouchersValueProcessed;

        #region Output Parameters
        Int64 noofRewardReIssueMailProcessed = 0;
        Int64 totalVouchersReissuedProcessed = 0;
        double totalVocherValueProcessed = 0.00;


        #endregion

        #endregion

        #region Properties
        public DateTime PreviousCurrentSatrtDate
        {
            get { return previousOfferstartDate; }
            set { previousOfferstartDate = value; }
        }
        public DateTime PreviousCurrentEndDate
        {
            get { return previousOfferendDate; }
            set { previousOfferendDate = value; }
        }
        public short PreviousOfferID
        {
            get { return previousOfferID; }
            set { this.previousOfferID = value; }
        }
        public Int64 NoOfRewardReIssueMailProcessed
        {
            get { return noOfRewardReIssueMailProcessed; }
            set { this.noOfRewardReIssueMailProcessed = value; }
        }
        public Int64 TotalVouchersReIssuedProcessed
        {
            get { return totalVouchersReIssuedProcessed; }
            set { this.totalVouchersReIssuedProcessed = value; }
        }
        public double TotalVouchersValueProcessed
        {
            get { return totalVouchersValueProcessed; }
            set { this.totalVouchersValueProcessed = value; }
        }
        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }
        #endregion

        public bool ExecuteMethods()
        {

            string message0 = "Reissue Rewards started.";
            string sFileName = CommonFunctions.CreateLogFile(sBatch);
            CommonFunctions.MessageWriteToLogFile(sFileName, message0);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message0, false);

            string message = "Testing that preconditions are met ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, false);

            string message1 = "Testing current collection period ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message1);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message1, false);
            retunValue = FindCurrentOffer();
            if (retunValue != true) return false;
            string message2 = "Testing last collection period ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message2);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message2, false);

            //Is there a last collection period
            retunValue = FindPreviousOffer();
            if (retunValue != true) return false;
            retunValue = ReIssueNormalInitialize();
            if (retunValue != true) return false;
            retunValue = ReIssueNormalRewards(PreviousOfferID, out noOfRewardReIssueMailProcessed, out totalVouchersReIssuedProcessed, out totalVouchersValueProcessed);
            if (retunValue != true) return false;
            retunValue = ReissueNormalRewardExport(OutPutDirectory);
            if (retunValue != true) return false;
            string message3 = "Reissue Normal Rewards completed." + Environment.NewLine + TotalVouchersReIssuedProcessed + " Vouchers re-issued with total value of " + TotalVouchersValueProcessed + Environment.NewLine + NoOfRewardReIssueMailProcessed + " customers have reissued rewards.";
            CommonFunctions.MessageWriteToLogFile(sFileName, message3);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message3, false);
            Console.WriteLine("Reissue Normal Rewards completed.");
            return true;
        }

        #region Methods

        #region Find the Current (Next) Offer Period
        public bool FindCurrentOffer()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CalculateRewards.FindCurrentOffer");
            //int returnCode = 0;
            //int rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                object[] objGetCurrentOffer = { };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.GetCurrentOffer, objGetCurrentOffer);

                ds.Tables[0].TableName = "CurrentOffer";
                if (ds.Tables[0].Rows.Count > 0)
                {


                    return true;

                }
                else
                {
                    Console.WriteLine("There is no current collection period defined");
                    string message = "There is no current collection period defined.";
                    string sFileName = CommonFunctions.CreateLogFile(sBatch);
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, true);
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
                    //this.numberof
                    rowCount = returnCode;
                    return true;

                }
                else
                {
                    Console.WriteLine("There is no Previous collection period defined");
                    string message = "There is no Previous collection period defined.";
                    string sFileName = CommonFunctions.CreateLogFile(sBatch);
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, true);
                    return false;
                }
                // return true;
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

        #region CreateXMLFile
        public string CreateXMLFile(string sBatch)
        {
            string sFileName = "";
            string sLogRootDirectory = ConfigurationSettings.AppSettings["LogRootDirectory"];
            sLogRootDirectory = sLogRootDirectory.Trim();

            if (sLogRootDirectory.Substring(sLogRootDirectory.Length - 1, 1) != "\\")
            {
                sLogRootDirectory = sLogRootDirectory + "\\";
            }

            if (!Directory.Exists(sLogRootDirectory))
            {
                Directory.CreateDirectory(sLogRootDirectory);
            }

            sFileName = sLogRootDirectory + sBatch + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_"
                + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xml";
            FileName = sFileName;
            StreamWriter sw = new StreamWriter(sFileName);
            if (!File.Exists(sFileName))
            {
                sFileName = string.Empty;
            }
            sw.Close();
            return sFileName;
        }
        #endregion

        #region MessageWriteToLogFile
        public static void MessageWriteXMLFile(string sFileName, string sMessageToWrite)
        {
            if (File.Exists(sFileName))
            {
                StreamWriter sw = new StreamWriter(sFileName, true);
                sw.WriteLine(sMessageToWrite);
                sw.Close();
            }
        }
        #endregion

        #region ReIssue Normal Rewards Initialize
        //Create tables to buffer output 
        public bool ReIssueNormalInitialize()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueReward.ReIssueNormalInitialize");
            int returnCode = 0;
            try
            {

                object[] objCreateTable = { };
                // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueHighRewardInitialise, ref objCreateTable);
                //return returnCode;
                return true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e) ;
                //executeResult.Add ;
                //errorOutput(e.Message);
                //return returnCode;
                string message = "There is a problem in Creating the ReIssue Normal Initialization ( Creating the Temporary table ). ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, true);
                return false;
            }
        }
       
        #endregion

        #region ReIssue Normal Rewards
        public bool ReIssueNormalRewards(short prevOfferId, out Int64 NoOfRewardReIssueMailProcessed, out Int64 TotalVouchersReIssuedProcessed, out double TotalVouchersValueProcessed)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueReward.ReIssueNormalRewards");
            int returnCode = 0;
            NoOfRewardReIssueMailProcessed = 0;
            TotalVouchersReIssuedProcessed = 0;
            TotalVouchersValueProcessed = 0;
            //Added to make the Reissue File similar to Mailing File by Syed Amjadulla on 18th Aug 2009 for passing the parameter to ReIssue Normal Rewards SP for Showing/Hiding Partner Details
            short ShowPartnerDetails = Convert.ToInt16(ConfigurationSettings.AppSettings["ShowPartnerDetailsInMail"]);
            try
            {
                this.Culture = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
                object[] objReIssueRewards = {   
                                                 prevOfferId,    
                                                 ShowPartnerDetails,
                                                 Culture,
                                                 Convert.ToDecimal(ConfigurationManager.AppSettings["VATThresholdValue"].ToString()),
                                                 NoOfRewardReIssueMailProcessed,
                                                 TotalVouchersReIssuedProcessed,
                                                 TotalVouchersValueProcessed
                                             };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueNormalRewards, ref objReIssueRewards);
                NoOfRewardReIssueMailProcessed = Convert.ToInt64(objReIssueRewards[3]);
                TotalVouchersReIssuedProcessed = Convert.ToInt64(objReIssueRewards[4]);
                TotalVouchersValueProcessed = Convert.ToDouble(objReIssueRewards[5]);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in the Processing of ReIssueNormalRewards... ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, true);
                return false;
            }
        }
        #endregion

        #region Reissue Normal Reward Export
        public bool ReissueNormalRewardExport(string outputDirectory)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueRewards.ReissueNormalRewardExport");
            int returnCode = 0;
            try
            {
                object[] objRRExport = { outputDirectory };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueNormalRewardExport, ref objRRExport);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in the Processing of ReIssue Normal Reward Export... ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_NORMAL_REWARD, message, true);
                return false;
            }
        }
        #endregion

        #endregion
    }
}
