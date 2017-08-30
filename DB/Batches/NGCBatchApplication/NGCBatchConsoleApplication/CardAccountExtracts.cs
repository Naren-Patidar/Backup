/*
 * File   : TransactionExtracts.cs
 * Author : Netra V K(HSC) 
 * email  :
 * File   : This file contains methods/properties related to Transaction Extracts
 * Date   : 27/Aug/2008
 * 
*/
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

namespace Tesco.NGC.BatchConsoleApplication
{
    public class CardAccountExtracts
    {

        #region Methods

        #region Execute Methods
        public bool ExecuteSPs(Int32 ndateOffset, string updateBy)
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CardAccountExtracts.Batch");
            Result executeResult = new Result();

            string sBcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
            string sOutPutDirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
            string sChangeEncodingDirectory = ConfigurationSettings.AppSettings["ChangeEncodingDirectory"];
            Int32 nProcessed = 0;
            string sFileName = "";
            int iResult;

             try
            {
                sFileName = CommonFunctions.CreateLogFile(Constants.BATCH_CARD_ACCOUNT_EXTRACT);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.BATCH_CARD_ACCOUNT_EXTRACT);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
                CommonFunctions.MessageWriteToLogFile(sFileName, "Clubcard Migration Reverse Started");
                CommonFunctions.MessageWriteToEventViewer("Clubcards Extract", "Clubcards Extract Started At:" + DateTime.Now.ToString(), false);
                //CommonFunctions.MessageWriteToLogFile(sFileName, "DateOffSet = : " + ndateOffset);
                //CommonFunctions.MessageWriteToLogFile(sFileName, "UpdatedBy = : " + updateBy);
                if (ndateOffset < 1)
                {
                    ndateOffset = 1;
                }
                if (updateBy == null)
                {
                    updateBy = "0";
                }

                // Modified by Syed Amjadulla on 19th Mar'2010 to use ReportingDB
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);


                #region Execute "sp_migrate_reverse_card_account_create"

                /*CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_CREATE + "'");*/
                 object[] objParams = {};
                 iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_CREATE, ref  objParams);
                /*CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_CREATE + "'");*/

                    #endregion

                #region Execute "sp_migrate_reverse_card_account"

                /*CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT + "'");*/
                    object[] objDBParams = { ndateOffset,updateBy, nProcessed};
                    iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT, ref objDBParams);
                    string cardsProcessed = "0";
                    Int32 i = objDBParams.Length - 1;
                    cardsProcessed = Convert.ToString(objDBParams[i]);

                    CommonFunctions.MessageWriteToLogFile(sFileName, "Extracted No of Clubcards :" + cardsProcessed.ToString());
                    CommonFunctions.MessageWriteToEventViewer("Clubcards Extract", "Output " + cardsProcessed.ToString() + " Clubcards", false);
                    /*CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT + "'");*/

                    #endregion

                #region Execute "sp_migrate_reverse_card_account_export"

                    /*CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_EXPORT + "'");*/
                    //object[] objDBParams1 = { sBcpDirectory, sChangeEncodingDirectory, sOutPutDirectory }; 
                    object[] objDBParams1 = { sOutPutDirectory};
                    iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_EXPORT, ref objDBParams1);
                    /*CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_REVERSE_CARD_ACCOUNT_EXPORT + "'");*/

                    #endregion

                    executeResult.Flag = true;

            }
             catch (Exception e)
             {
                 ExceptionManager.Publish(e);
                 executeResult.Add(e);
                 //errorOutput(e.Message);
                 CommonFunctions.MessageWriteToLogFile(sFileName, "Error : " + e.Message.ToString());
                 CommonFunctions.MessageWriteToEventViewer("Clubcards Extract", "Clubcards Extract Failed With Following Error:" + e.Message.ToString(), true);
             }
             finally
             {
                 trState.EndProc();
             }
             /*resultXml = executeResult.OuterXml;*/
             //to do
             CommonFunctions.MessageWriteToLogFile(sFileName, "Clubcard Migration Reverse Finished");
             CommonFunctions.MessageWriteToLogFile(sFileName, "Finished at : " + DateTime.Now);
             CommonFunctions.MessageWriteToEventViewer("Clubcards Extract", "Clubcards Extract Successfully Finished at : " + DateTime.Now.ToString(), false);
             return executeResult.Flag;
        }
        #endregion


        #endregion
    }
}
