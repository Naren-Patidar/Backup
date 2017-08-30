/*
 * File   : AggregateOffer.cs
 * Author : Netra V K (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Aggregate Transaction
 * Date   : 02/Sep/2008
 * 
 */
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
using Tesco.NGC.BatchConsoleApplication;

#endregion

namespace NGCBatchConsoleApplication
{
    class AggregateTxn
    {
        #region Declerarions
        // Modified by Syed Amjadulla on 19th Mar'2010 to use ReportingDB
        //public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        string EventLog = "Error in the AggregateTxns";
        string EventLogMsg = "AggregateTxns Message";
        string sBatch = "Aggregate_txns";
        string sFileName;
        bool retunValue;
        #endregion
        #region Methods

        #region Batch
        /// <summary>
        /// Execute a Batch Script
        /// </summary>
        /// <param></param>
        /// <returns>True or False</returns>
        public bool Batch()
        {
            string sFileName = "";
            int iResult;

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("AggregateTxn.Batch");
            Result executeResult = new Result();

            try
            {
                sFileName = CommonFunctions.CreateLogFile(Constants.ACTION_AGGREGATE_TRANSACTION);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.ACTION_AGGREGATE_TRANSACTION);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
                // Modified by Syed Amjadulla on 19th Mar'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

                #region Execute "sp_aggregate_txns"

                CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_AGGREGATE_TRANSACTION + "'");
                object[] objParams = { };
                iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_AGGREGATE_TRANSACTION, ref objParams);
                CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_AGGREGATE_TRANSACTION + "'");

                #endregion

                executeResult.Flag = true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                executeResult.Add(e);
                //errorOutput(e.Message);
                CommonFunctions.MessageWriteToLogFile(sFileName, "Error : " + e.Message.ToString());
            }
            finally
            {
                trState.EndProc();
            }
            /*resultXml = executeResult.OuterXml;*/
            //to do

            CommonFunctions.MessageWriteToLogFile(sFileName, "Finished at : " + DateTime.Now);
            return executeResult.Flag;
        }

        #endregion
        public bool ExecuteMethods()
        {
            sFileName = CommonFunctions.CreateLogFile(sBatch);

            string message0 = "Started at :" + DateTime.Now;
            CommonFunctions.MessageWriteToLogFile(sFileName, message0);

            string message = "Daily Summarisation of Transaction Data Started.";
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message, false);
            retunValue = AggregateTxns();
            if (retunValue != true)
                return false;
            string message1 = "Daily Summarisation of Transaction Data Completed Successfully.";
            CommonFunctions.MessageWriteToLogFile(sFileName, message1);
            CommonFunctions.MessageWriteToEventViewer(EventLogMsg, message1, false);

            string message2 = "Finished at :" + DateTime.Now;
            CommonFunctions.MessageWriteToLogFile(sFileName, message2);
            return true;
        }
        #region Aggregate Transactions
        public bool AggregateTxns()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("AggregateTxn.AggregateTxns");
            int returnCode = 0;
            try
            {

                object[] objAgregateTxns = { };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_AGGREGATE_TRANSACTION, ref objAgregateTxns);
                return true;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                string message = "There is a problem in the summerisation of the Aggregate Transactions";
                CommonFunctions.MessageWriteToLogFile(sFileName, message);
                CommonFunctions.MessageWriteToEventViewer(EventLog, message, true);
                return false;
            }
        }
        #endregion

        #endregion
    }
}
