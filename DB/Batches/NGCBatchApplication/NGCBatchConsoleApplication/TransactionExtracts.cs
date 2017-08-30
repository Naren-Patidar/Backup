﻿/*
 * File   : TransactionExtracts.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Transaction Extracts
 * Date   : 27/Aug/2008
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
    public class TransactionExtracts
    {
        #region Methods

        #region Batch
        /// <summary>
        /// Execute a Batch Script
        /// </summary>
        /// <param name="dateOffSet">Number of days (this is )</param>
        /// <returns>True or False</returns>
        public bool Batch(Int32 ndateOffset)//, out string resultXml)
        //private bool Batch(Trace trace, string scriptName, string userName, string password, string[] args, Output standardOutput, Output errorOutput, out string resultXml)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("TransactionExtracts.Batch");
            Result executeResult = new Result();

            string sBcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
            string sOutPutDirectory = ConfigurationSettings.AppSettings["OutPutDirectory"];
            string sChangeEncodingDirectory = ConfigurationSettings.AppSettings["ChangeEncodingDirectory"];
            Int32 nTxnProcessed = 0, nVoucherProcessed = 0, nPointProcessed = 0, nOfferCrmID = 0;
            string sMessage = "", sFileName = "";
            int iResult;
                       
            try
            {
                sFileName = CommonFunctions.CreateLogFile(Constants.ACTION_TRANSACTION_EXTRACTS);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.ACTION_TRANSACTION_EXTRACTS);
                    return false;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
                CommonFunctions.MessageWriteToLogFile(sFileName, "DateOffSet = : " + ndateOffset);
                if (ndateOffset < 1)
                {
                    ndateOffset = 1;
                }

                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

                #region Validating the collection Start and End Date
                CommonFunctions.MessageWriteToLogFile(sFileName, "Before validating Collection Start Date and End Date");
                //to do

                CommonFunctions.MessageWriteToLogFile(sFileName, "After validating Collection Start Date and End Date");

                #endregion

                #region Get the Offer Id from Database

                    CommonFunctions.MessageWriteToLogFile(sFileName, "Before retrieving Offer_CRM_ID ");

                    //Note : nOfferCrmID is output parameter in the Stored Procedure, If any OfferCRMId is returning form DB, its value will be none zero
                    iResult = SqlHelper.ExecuteNonQuery(connectionString, "getOfferCRMID", nOfferCrmID);//to do

                    CommonFunctions.MessageWriteToLogFile(sFileName, "After retrieving Offer_CRM_ID .. Offer CRM ID = " + nOfferCrmID);
                    
                    if (nOfferCrmID == 0)
                    {
                        CommonFunctions.MessageWriteToLogFile(sFileName, "Not able to continue since OFFER CRM ID is null");
                    }

                #endregion

                #region Execute "sp_migrate_reverse_txn_create"

                    CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing 'sp_migrate_reverse_txn_create'");
                    iResult = SqlHelper.ExecuteNonQuery(connectionString, "sp_migrate_reverse_txn_create");//to do
                    CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of 'sp_migrate_reverse_txn_create'");

                #endregion

                #region Execute "sp_migrate_reverse_txn"

                    CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing 'sp_migrate_reverse_txn'");
                    object[] objDBParams =  { nOfferCrmID, ndateOffset, nPointProcessed, nTxnProcessed, nVoucherProcessed }; 
                    iResult = SqlHelper.ExecuteNonQuery(connectionString, "sp_migrate_reverse_txn", objDBParams);//to do
                    CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of 'sp_migrate_reverse_txn'");

                #endregion

                #region Execute "sp_migrate_reverse_txn_export"

                    CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing 'sp_migrate_reverse_txn_export'");
                    object[] objDBParams1 = { sBcpDirectory, sChangeEncodingDirectory, sOutPutDirectory };
                    iResult = SqlHelper.ExecuteNonQuery(connectionString, "sp_migrate_reverse_txn_export", objDBParams1);//to do
                    CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of 'sp_migrate_reverse_txn_export'");

                #endregion


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

        #endregion
    }
}
