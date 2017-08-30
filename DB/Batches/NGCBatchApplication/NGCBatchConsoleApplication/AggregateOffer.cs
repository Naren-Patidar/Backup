/*
 * File   : AggregateOffer.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Aggregate Offer
 * Date   : 28/Aug/2008
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

#endregion


namespace Tesco.NGC.BatchConsoleApplication
{
    public class AggregateOffer
    {
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
            TraceState trState = trace.StartProc("AggregateOffer.Batch");
            Result executeResult = new Result();

            try
            {
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_AGGREGATE_OFFER, "Daily Summarisation of Collection Periods Started.", false);
                sFileName = CommonFunctions.CreateLogFile(Constants.ACTION_AGGREGATE_OFFER);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.ACTION_AGGREGATE_OFFER);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
                // Modified by Syed Amjadulla on 19th Mar'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

                #region Execute "sp_aggregate_offers"

                CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_AGGREGATE_OFFERS + "'");
                object[] objParams = { };
                iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_AGGREGATE_OFFERS, ref objParams);
                CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_AGGREGATE_OFFERS + "'");

                #endregion

                executeResult.Flag = true;
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_AGGREGATE_OFFER, "Daily Summarisation of Collection Periods Completed.", false);

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                executeResult.Add(e);
                //errorOutput(e.Message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_AGGREGATE_OFFER, "Daily Summarisation Failed.\r\nERROR: '{0}'\r\nReturn Code: '{1}'", true);
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

        #endregion
    }
}
