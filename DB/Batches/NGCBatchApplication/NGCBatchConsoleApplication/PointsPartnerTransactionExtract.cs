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
    public class PointsPartnerTransactionExtract
    {
        #region Fields

        private int dateofSet;        
        private int offerCrmID;
        private DateTime collectionStartDate;
        private DateTime collectionEndDate;
        //string sMessage = "";
        string sFileName = "";


        public string bcp_directory = ConfigurationSettings.AppSettings["BCPDirectory"];
        public string changeencoding_directory = ConfigurationSettings.AppSettings["ChangeEncodingDirectory"];
        public string output_directory = ConfigurationSettings.AppSettings["OutputDirectory"];
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        int nTxnProcessed = 0;    
        #endregion

        #region Properties
        public int OfferCrmID { get { return this.offerCrmID; } set { this.offerCrmID = value; } }
        public int DateOfSet { get { return this.dateofSet; } set { this.dateofSet = value; } }
        public DateTime CollectionStartDate { get { return this.collectionStartDate; } set { this.collectionStartDate = value; } }
        public DateTime CollectionEndDate { get { return this.collectionEndDate; } set { this.collectionEndDate = value; } }
        
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

        #region ExecuteSPs
        public bool ExecuteSPs()
        {
            try
            {
                sFileName = CommonFunctions.CreateLogFile(Constants.LOG_POINTS_PARTNER_TRANS);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.LOG_POINTS_PARTNER_TRANS);
                    return false;
                }
                if ((DateOfSet != 0) && (IsNumeric(DateOfSet)))
                {
                    GetOfferId(DateOfSet);
                    TransactionCreate();
                    TransactionforPP();
                    TransactionforPPExport();                   
                    return true;
                    
                }
                return false;
            }
            catch(Exception e)
            {
                ExceptionManager.Publish(e);
                CommonFunctions.MessageWriteToLogFile(sFileName, "There was a problem to execute the Stored Procedures");
                return false;

            }
        }
       
        #endregion
        
        #region Transaction Create
        //Create the Customers
        public int TransactionCreate()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("PointsPartnerTransactionExtract.TransactionCreate");
            int returnCode = 0;
            try
            {
                object[] objParams = { };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_TXN4PP_CREATE, ref objParams);
                return returnCode;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                CommonFunctions.MessageWriteToLogFile(sFileName, " There was a problem to execute the sp_migrate_reverse_txn4pp_create Stored Procedure");
                return returnCode;
            }
        }
        #endregion

        #region Transaction for Points Partner
        // Transaction for Points Partner
        public int TransactionforPP()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("PointsPartnerTransactionExtract.TransactionforPP");
            int returnCode = 0;
            try
            {
                object[] objTransforPP = { OfferCrmID,dateofSet};
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_TXN4PP, ref objTransforPP);
                nTxnProcessed = returnCode;
                return returnCode;
                
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                CommonFunctions.MessageWriteToLogFile(sFileName, " There was a problem to execute the sp_migrate_reverse_txn4pp Stored Procedure");
                return returnCode;
            }
        }
        #endregion

        #region Transaction for Points Partner
        // Transaction for Points Partner
        public int TransactionforPPExport()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("PointsPartnerTransactionExtract.TransactionforPPExport");
            int returnCode = 0;
            try
            {
                object[] objTransPPExport = {bcp_directory,changeencoding_directory,output_directory };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_TXN4PP_EXPORT, ref objTransPPExport);
                return returnCode;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                CommonFunctions.MessageWriteToLogFile(sFileName, " There was a problem to execute the sp_migrate_reverse_txn4pp_export Stored Procedure");
                return returnCode;
            }
        }
        #endregion
        #region GetOfferId
        public int GetOfferId(int dateOfSet)
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("PointsPartnerTransactionExtract.GetOfferId");
            int offerId;
            try
            {
                object [] objGetOfferId = {dateOfSet};
                offerId = (int)SqlHelper.ExecuteScalar(connectionString, Constants.GET_CURRENT_OFFERID, objGetOfferId);
                OfferCrmID = offerId;
                return offerId;
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                CommonFunctions.MessageWriteToLogFile(sFileName, " There was a problem to execute the Get_Current_OfferID Stored Procedure");
                return 0;
                   
            }

        }
        #endregion
        #endregion
    }
}
