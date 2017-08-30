using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;

namespace Tesco.NGC.BatchConsoleApplication
{
    public class CalculateRewards
    {

        short currentOfferID;
        short prevOfferID;
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirement>
        Int64 currentTotalPointsBalance;
        Int64 currentPointsConvertedToRewardsBalance;
        decimal minimumVoucherValue;
        decimal voucherValueStep;
        decimal amountToPointsConversionRate;
        int CommitSize = 1000;
        decimal pointsToRewardConversionRate;
        int rewardPointsThreshold;
        // Modified by Syed Amjadulla on 24th Feb'2010 to use ReportingDB
        //public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
        //Message modified by Syed Amjadulla on 17th Aug'2009
        string EventLog = "NGC Calculate Rewards";
        string EventogMsg = "NGC Calculate Rewards";
        string sBatch = "CalculateRewards";
        string sFileName;
        bool retunValue;

        #region Previous Offer Data
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirement>
        Int64 previousTotalPointsBalance;
        Int64 previousPointsConvertedToRewardsBalance;
        int previousRewardPointsThreshold;
        decimal previousMinimumVoucherValue;
        decimal previousVoucherValueStep;
        short previousRewardConversionStatus;
        decimal previousPointsToRewardConversionRate;

        #endregion

        //Output Parameters
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment>
        Int64 totalPointsBalance;
        Int64 pointsConvertedToRewardsBalance;
        Int64 pointsCarriedForwardBalance;
        decimal amountRewardedBalance;
        Int64 customersRewarded;
        Int64 customersSkipped;

        #region Prperties

        public short CurrentOfferID { get { return currentOfferID; } set { currentOfferID = value; } }
        public short PrevOfferID { get { return prevOfferID; } set { prevOfferID = value; } }
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment>
        public Int64 CurrentTotalPointsBalance { get { return currentTotalPointsBalance; } set { currentTotalPointsBalance = value; } }
        public Int64 CurrentPointsConvertedToRewardsBalance { get { return currentPointsConvertedToRewardsBalance; } set { currentPointsConvertedToRewardsBalance = value; } }
        public decimal MinimumVoucherValue { get { return minimumVoucherValue; } set { minimumVoucherValue = value; } }
        public decimal VoucherValueStep { get { return voucherValueStep; } set { voucherValueStep = value; } }
        public decimal AmountToPointsConversionRate { get { return amountToPointsConversionRate; } set { amountToPointsConversionRate = value; } }
        public decimal PointsToRewardConversionRate { get { return pointsToRewardConversionRate; } set { pointsToRewardConversionRate = value; } }
        public int RewardPointsThreshold { get { return rewardPointsThreshold; } set { rewardPointsThreshold = value; } }

        #endregion

        #region Previous offer Properties
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment>
        public Int64 PreviousTotalPointsBalance { get { return previousTotalPointsBalance; } set { previousTotalPointsBalance = value; } }
        public Int64 PreviousPointsConvertedToRewardsBalance { get { return previousPointsConvertedToRewardsBalance; } set { previousPointsConvertedToRewardsBalance = value; } }
        public int PreviousRewardPointsThreshold { get { return previousRewardPointsThreshold; } set { previousRewardPointsThreshold = value; } }
        public decimal PreviousMinimumVoucherValue { get { return previousMinimumVoucherValue; } set { previousMinimumVoucherValue = value; } }
        public decimal PreviousVoucherValueStep { get { return previousVoucherValueStep; } set { previousVoucherValueStep = value; } }
        public short PreviousRewardConversionStatus { get { return previousRewardConversionStatus; } set { previousRewardConversionStatus = value; } }
        public decimal PreviousPointsToRewardConversionRate { get { return previousPointsToRewardConversionRate; } set { previousPointsToRewardConversionRate = value; } }
        #endregion

        #region Output Parameters Properties
        //<Added by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment>
        public Int64 TotalPointsBalance { get { return totalPointsBalance; } set { totalPointsBalance = value; } }
        public Int64 PointsConvertedToRewardsBalance { get { return pointsConvertedToRewardsBalance; } set { pointsConvertedToRewardsBalance = value; } }
        public Int64 PointsCarriedForwardBalance { get { return pointsCarriedForwardBalance; } set { pointsCarriedForwardBalance = value; } }
        public decimal AmountRewardedBalance { get { return amountRewardedBalance; } set { amountRewardedBalance = value; } }
        public Int64 CustomersRewarded { get { return customersRewarded; } set { customersRewarded = value; } }
        public Int64 CustomersSkipped { get { return customersSkipped; } set { customersSkipped = value; } }
        #endregion

        #region Output Parameters

        #endregion

        #region Methods

        #region Execute Methods
        public bool ExecuteMethods()
        {
            sFileName = CommonFunctions.CreateLogFile(sBatch);
            string message = "Reward Calculation Started...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(EventogMsg, message, false);

            //<Modified by Syed Amjadulla on 20th July 2009 for Checking Reward Conversion status before calculating rewards>                       
            string message1 = "Testing that the preconditions are met ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message1);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message1, false);

            string message2 = ("Testing current collection period ...");
            CommonFunctions.MessageWriteToLogFile(sFileName, message2);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message2, false);

            retunValue = FindCurrentOffer();
            if (retunValue != true)
                return false;

            if (PointsToRewardConversionRate <= 0)
            {
                string message4 = "The current collection period's points to reward conversion rate is not a positive number";
                CommonFunctions.MessageWriteToLogFile(sFileName, message4);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message4, true);
                return false;
            }

            string message5 = ("Testing last collection period ...");
            CommonFunctions.MessageWriteToLogFile(sFileName, message5);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message5, false);

            retunValue = FindPreviousOffer();
            if (retunValue != true)
                return false;

            if (PreviousPointsToRewardConversionRate <= 0)
            {
                string message7 = "The last collection period's points to reward conversion rate is not a positive number";
                CommonFunctions.MessageWriteToLogFile(sFileName, message7);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message7, true);
                return false;
            }

            if (PreviousRewardPointsThreshold <= 0)
            {
                string message9 = "The last collection period's points threshold is not a positive number";
                CommonFunctions.MessageWriteToLogFile(sFileName, message9);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message9, true);
                return false;
            }

            if (PreviousVoucherValueStep <= 0)
            {
                string message11 = "The last collection period's voucher step size is not a positive number.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message11);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message11, true);
                return false;
            }

            if (PreviousMinimumVoucherValue <= 0)
            {
                string message13 = "The last collection period's minimum voucher value is not a positive number.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message13);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message13, true);
                return false;
            }
            if (PreviousRewardConversionStatus == 2)
            {
                string message14 = "Reward Calculation is already in progress.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message14);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message14, true);
                return false;
            }
            if (PreviousRewardConversionStatus == 1)
            {
                string message15 = "No Reward Calculation is required.";
                CommonFunctions.MessageWriteToLogFile(sFileName, message15);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message15, true);
                return false;
            }

            string message16 = "All preconditions met, Reward Calculation starting ...";
            CommonFunctions.MessageWriteToLogFile(sFileName, message16);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message16, false);

            retunValue = CalculatRewards();
            if (retunValue != true)
                return false;
            return true;
        }


        #endregion


        #region Find the current offer period
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
                    //<Modified by Syed Amjadulla on 19th Aug'2009 to refer columns by names instead of index>                  
                    this.CurrentOfferID = Convert.ToInt16(ds.Tables[0].Rows[0]["OfferID"]);
                    if ((ds.Tables[0].Rows[0]["MinimumVoucherValue"]) != DBNull.Value) this.MinimumVoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["MinimumVoucherValue"]);
                    if ((ds.Tables[0].Rows[0]["VoucherValueStep"]) != DBNull.Value) this.VoucherValueStep = Convert.ToDecimal(ds.Tables[0].Rows[0]["VoucherValueStep"]);
                    if ((ds.Tables[0].Rows[0]["AmountToPointsConversionRate"]) != DBNull.Value) this.AmountToPointsConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountToPointsConversionRate"]);
                    //<Modified by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment to add TotalPointsBalance parammeter for GetCurrentOffer SP>
                    if ((ds.Tables[0].Rows[0]["TotalPointsBalance"]) != DBNull.Value) this.CurrentTotalPointsBalance = Convert.ToInt64(ds.Tables[0].Rows[0]["TotalPointsBalance"]);
                    if ((ds.Tables[0].Rows[0]["PointsConvertedToRewardsBalance"]) != DBNull.Value) this.CurrentPointsConvertedToRewardsBalance = Convert.ToInt64(ds.Tables[0].Rows[0]["PointsConvertedToRewardsBalance"]);                    
                    if ((ds.Tables[0].Rows[0]["PointsToRewardConversionRate"]) != DBNull.Value) this.PointsToRewardConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["PointsToRewardConversionRate"]);
                    return true;
                }
                else
                {
                    string message = "There is no current collection period defined.";
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
                    //<Modified by Syed Amjadulla on 19th Aug'2009 to refer columns by names instead of index>                   
                    this.PrevOfferID = Convert.ToInt16(ds.Tables[0].Rows[0]["OfferID"]);
                    if ((ds.Tables[0].Rows[0]["VoucherValueStep"]) != DBNull.Value) this.PreviousVoucherValueStep = Convert.ToDecimal(ds.Tables[0].Rows[0]["VoucherValueStep"]);
                    if ((ds.Tables[0].Rows[0]["MinimumVoucherValue"]) != DBNull.Value) this.PreviousMinimumVoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["MinimumVoucherValue"]);
                    if ((ds.Tables[0].Rows[0]["RewardPointsThreshold"]) != DBNull.Value) this.PreviousRewardPointsThreshold = Convert.ToInt16(ds.Tables[0].Rows[0]["RewardPointsThreshold"]);
                    if ((ds.Tables[0].Rows[0]["PointsToRewardConversionRate"]) != DBNull.Value) this.PreviousPointsToRewardConversionRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["PointsToRewardConversionRate"]);
                    //<Added by Syed Amjadulla on 20th July 2009>
                    if ((ds.Tables[0].Rows[0]["RewardConversionStatus"]) != DBNull.Value) this.PreviousRewardConversionStatus = Convert.ToInt16(ds.Tables[0].Rows[0]["RewardConversionStatus"]);
                    rowCount = returnCode;
                    return true;
                }
                else
                {
                    string message = "No Reward Calculation is required.";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventLog, message, false);
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

        #region rewardPoints
        public bool CalculatRewards()
        {
            //<Modified by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment to add TotalPointsBalance parammeter for calculate Reward SP>
            TotalPointsBalance = 0;
            PointsConvertedToRewardsBalance = 0;
            PointsCarriedForwardBalance = 0;
            AmountRewardedBalance = 0;
            CustomersRewarded = 0;
            CustomersSkipped = 0;
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CalculateRewards.Calc");
            int returnCode = 0;
            try
            {               
                object[] objCalculateReward = {  
                                       PrevOfferID,
                                        PointsToRewardConversionRate,
                                        PreviousPointsToRewardConversionRate,
                                        PreviousRewardPointsThreshold,
                                        MinimumVoucherValue,
                                        PreviousVoucherValueStep,
                                        CommitSize,   
                                       TotalPointsBalance,
                                       PointsConvertedToRewardsBalance,                                    
                                       PointsCarriedForwardBalance,
                                       AmountRewardedBalance,
                                       CustomersRewarded,
                                      CustomersSkipped
                                     };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.CalculateRewards, ref objCalculateReward);
                TotalPointsBalance = Convert.ToInt64(objCalculateReward[7]);
                PointsConvertedToRewardsBalance = Convert.ToInt64(objCalculateReward[8]);
                PointsCarriedForwardBalance = Convert.ToInt64(objCalculateReward[9]);
                AmountRewardedBalance = Convert.ToDecimal(objCalculateReward[10]);
                CustomersRewarded = (Int64)objCalculateReward[11];
                CustomersSkipped = (Int64)objCalculateReward[12];

                if (CustomersSkipped == 0)
                {
                    //<Modified by Syed Amjadulla on 16th Sep'2009 as per NGC V 3.1.2 requirment to add TotalPointsBalance parammeter for UpdateOffer4Rewards SP>
                    object[] objUpdateOffer = {  
                        PrevOfferID,
                       PointsConvertedToRewardsBalance,
                       TotalPointsBalance,
                       PointsCarriedForwardBalance,
                       AmountRewardedBalance,
                       CustomersRewarded,
                       CustomersSkipped
                     };
                    returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.UpdateOffer4Rewards, ref objUpdateOffer);
                    Console.WriteLine("Calculate Rewards Completed.");
                    string message = "Calculate Rewards Completed." + Environment.NewLine
                        + CustomersRewarded + " Customers rewarded"
                        + Environment.NewLine + PointsConvertedToRewardsBalance + " points converted to rewards"
                        + Environment.NewLine + PointsCarriedForwardBalance + " points carried forward"
                        + Environment.NewLine + AmountRewardedBalance + " amount rewarded balance";
                    CommonFunctions.MessageWriteToLogFile(sFileName, message);
                    CommonFunctions.MessageWriteToEventViewer(EventogMsg, message, false);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "Calculate Rewards was Failed...";
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #endregion
    }
}
