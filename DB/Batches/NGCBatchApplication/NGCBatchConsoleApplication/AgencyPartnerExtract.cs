/*
 * File   : TransactionExtracts.cs
 * Author : Netra V K(HSC) 
 * email  :
 * File   : This file contains methods/properties related to Agency Partner Extracts
 * Date   : 02/sep/2008
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
using Tesco.NGC.BatchConsoleApplication;

namespace NGCBatchConsoleApplication
{
    class AgencyPartnerExtract
    {
        #region Methods

        #region Execute Methods
        public bool ExecuteSPs()
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("AgencyPartnerExtracts.Batch");
            Result executeResult = new Result();

            string sBcpDirectory = "C:\\";//ConfigurationSettings.AppSettings["BcpDirectory"];
            string sOutPutDirectory = "C:\\";//ConfigurationSettings.AppSettings["OutPutDirectory"];
            string sChangeEncodingDirectory = "C:\\";//ConfigurationSettings.AppSettings["ChangeEncodingDirectory"];
            Int32 nAgencyProcessed = 0;
            Int32 nPartnerProcessed = 0;
            string sFileName = "";
            int iResult;

            try
            {
                sFileName = CommonFunctions.CreateLogFile(Constants.BATCH_AGENCY_PARTNER_EXTRACT);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.BATCH_AGENCY_PARTNER_EXTRACT);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);
               
               
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

                #region Execute "sp_migrate_agency_partner_create"

                CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_AGENCY_PARTNER_CREATE + "'");
                object[] objParams = { };
                iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_AGENCY_PARTNER_CREATE, ref objParams);
                CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_AGENCY_PARTNER_CREATE + "'");

                #endregion

                #region Execute "sp_migrate_agency_partner"

                CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_AGENCY_PARTNER + "'");
                object[] objDBParams = { nAgencyProcessed, nPartnerProcessed };
                iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_AGENCY_PARTNER, ref objDBParams);
                CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_AGENCY_PARTNER + "'");

                #endregion

                #region Execute "sp_migrate_agency_partner_export"

                CommonFunctions.MessageWriteToLogFile(sFileName, "Before executing '" + Constants.SP_MIGRATE_AGENCY_PARTNER_EXPORT + "'");
                object[] objDBParams1 = { sBcpDirectory, sChangeEncodingDirectory, sOutPutDirectory };
                iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_AGENCY_PARTNER_EXPORT, ref objDBParams1);
                CommonFunctions.MessageWriteToLogFile(sFileName, "After execution of '" + Constants.SP_MIGRATE_AGENCY_PARTNER_EXPORT + "'");

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


        #endregion
    }
}
