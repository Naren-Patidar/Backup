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
namespace Tesco.NGC.BatchConsoleApplication
{
    public class ReIssueRewards
    {
        #region Fields
        // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
        //public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);                
        string OutPutDirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string BCPDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string sBatch = "ReissueRewards";
        private string culture;
        bool retunValue;

        private DateTime previousOfferstartDate;
        private DateTime previousOfferendDate;
        private short previousOfferID;
        private Int64 noOfHighRwardReIssueMailProcessed;
        private Int64 totalVouchersReIssuedProcessed;
        private double totalVouchersValueProcessed;

        #region Output Parameters
        Int64 NoOfHRReissueMailingProcessed = 0;
        Int64 TotalVouchersReissuedProcessed = 0;
        double TotalVocherValueProcessed = 0;


        #endregion

        #endregion

        #region Properties
        public DateTime PreviousCurrentStartDate
        {
            get { return previousOfferstartDate; }
            set { previousOfferstartDate = value; }
        }
        public DateTime PreviousOfferEndDate
        {
            get { return previousOfferendDate; }
            set { previousOfferendDate = value; }
        }
        public short PreviousOfferID
        {
            get { return previousOfferID; }
            set { this.previousOfferID = value; }
        }
        public Int64 NoOfHighRwardReIssueMailProcessed
        {
            get { return noOfHighRwardReIssueMailProcessed; }
            set { this.noOfHighRwardReIssueMailProcessed = value; }
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

        #region Execute Methods
        #region Finding the Next Coolection Period
        public bool ExecuteMethods()
        {
            string message0 = "Reissue High Rewards started.";
            string sFileName = CommonFunctions.CreateLogFile(sBatch);
            CommonFunctions.MessageWriteToLogFile(sFileName, message0);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message0, false);

            string message = "Testing that preconditions are met ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, false);

            string message1 = "Testing that current collection period defined...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message1);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message1, false);
            // Is there a next collection period 
            retunValue = FindCurrentOffer();
            if (retunValue != true) return false;

            string message2 = "Testing last collection period ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message2);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message2, false);
            //Is there a last collection period
            retunValue = FindPreviousOffer();
            if (retunValue != true) return false;

            // ReIssue Rewards Initialize
            retunValue = ReIssueHighRewardInitialize();
            if (retunValue != true) return false;
            //Executing the ReIssue High Rewards 
            retunValue = ReIssueHighReward(PreviousOfferID, out NoOfHRReissueMailingProcessed, out TotalVouchersReissuedProcessed, out TotalVocherValueProcessed);
            if (retunValue != true) return false;

            //Executing Reissue High Reward MailingExport
            retunValue = ReissueHighRewardExport(OutPutDirectory);
            if (retunValue != true) return false;

            string message3 = "Reissue High Rewards completed." + Environment.NewLine + TotalVouchersReIssuedProcessed + " re-issued with total value of " + TotalVouchersValueProcessed + Environment.NewLine + NoOfHighRwardReIssueMailProcessed + " customers have reissued high rewards.";
            CommonFunctions.MessageWriteToLogFile(sFileName, message3);
            CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message3, false);
            Console.WriteLine("Reissue High Rewards completed.");
            return true;
        }
        #endregion
        #endregion

        #region Methods


        #region Find the Current (Next) Offer Period
        public bool FindCurrentOffer()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CalculateRewards.FindCurrentOffer");
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
                    string message = "There is no current collection period defined.";
                    string sFileName = CommonFunctions.CreateLogFile(sBatch);
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, true);
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
                    if ((ds.Tables[0].Rows[0]["OfferID"]) != DBNull.Value)
                        this.PreviousOfferID = Convert.ToInt16(ds.Tables[0].Rows[0]["OfferID"]);
                    rowCount = returnCode;
                    return true;

                }
                else
                {
                    string message = "There is no Previous collection period defined.";
                    string sFileName = CommonFunctions.CreateLogFile(sBatch);
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, true);
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

        #region ReIssue Rewards Initialize
        //Create tables to buffer output 
        public bool ReIssueHighRewardInitialize()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueRewards.ReIssueHighRewardInitialize");
            int returnCode = 0;
            try
            {

                object[] objCreateTable = { };
                // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueHighRewardInitialise, ref objCreateTable);
                return true;

            }

            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in Creating the ReIsssue HighReward Initialization ( Creating the Temporary table ). ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, true);
                return false;
            }
        }
      
        #endregion

        #region ReIssue Rewards

        public bool ReIssueHighReward(short prevOfferId, out Int64 NoOfHRReissueMailingProcessed, out Int64 TotalVouchersReissuedProcessed, out double TotalVocherValueProcessed)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueRewards.ReIssueHighReward");
            int returnCode = 0;
            NoOfHRReissueMailingProcessed = 0;
            TotalVouchersReissuedProcessed = 0;
            TotalVocherValueProcessed = 0;
            //Added to make the Reissue File similar to Mailing File by Syed Amjadulla on 18th Aug 2009 for passing the parameter to ReIssue High Rewards SP for Showing/Hiding Partner Details
            short ShowPartnerDetails = Convert.ToInt16(ConfigurationSettings.AppSettings["ShowPartnerDetailsInMail"]);
            try
            {
                this.Culture = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
                object[] objReIssueRewards = {   
                                                 prevOfferId,   
                                                 ShowPartnerDetails,
                                                 Culture,
                                                 NoOfHRReissueMailingProcessed,
                                                 TotalVouchersReissuedProcessed,
                                                 TotalVocherValueProcessed
                                             };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueHighRewards, ref objReIssueRewards);
                NoOfHighRwardReIssueMailProcessed = Convert.ToInt64(objReIssueRewards[3]);
                TotalVouchersReIssuedProcessed = Convert.ToInt64(objReIssueRewards[4]);
                TotalVouchersValueProcessed = Convert.ToDouble(objReIssueRewards[5]);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in the Processing of ReIssueHighRewards... ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, true);
                return false;
            }
        }
        #endregion


        #region Reissue High Reward Export
        public bool ReissueHighRewardExport(string outputDirectory)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ReIssueRewards.ReissueHighRewardExport");
            int returnCode = 0;
            try
            {
                object[] objRRExport = { outputDirectory };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.ReIssueHighRewardExport, ref objRRExport);
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in the Processing of ReIssue HighReward Export... ";
                string sFileName = CommonFunctions.CreateLogFile(sBatch);
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_REISSUE_REWARD, message, true);
                return false;
            }
        }
        #endregion

        #endregion

    }
}
