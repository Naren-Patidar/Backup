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
  public  class CustomerInsightExtract
    {
        #region Fields

        private DateTime dateofSet;
        private string cardPrefix;
        private short updatedBy;

        public string bcp_directory = ConfigurationSettings.AppSettings["BCPDirectory"];
        public string changeencoding_directory = ConfigurationSettings.AppSettings["ChangeEncodingDirectory"];
        public string output_directory = ConfigurationSettings.AppSettings["OutputDirectory"];
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);

        #endregion

        #region Properties

        public DateTime DateOfSet { get { return dateofSet; }  set { dateofSet = value; }  }
        public string  CardPrefix { get { return cardPrefix; } set { cardPrefix = value; } }
        public short UpdatedBy { get { return updatedBy; } set { updatedBy = value; } }

        #endregion

        #region Methods
        
        #region Execute SPs
        public bool ExecuteSPS()
        {
            
            CustomerCreate();
            MigrateReverseCustomerExport();           
            return true;
        }

        
        #endregion

        #region CustomerCreate
        //Create the Customers
        public int CustomerCreate()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CustomerInsightExtract.CustomerCreate");
            int returnCode = 0;
            try
            {
                object[] objParams = { };
                returnCode = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_MIGRATE_REVERSE_CUSTOMER_CREATE, ref objParams);
                return returnCode;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                //executeResult.Add(e);
                //errorOutput(e.Message);
                return returnCode;
            }
        }
        #endregion

        #region MigrateReverseCustomer
        //Create the Customers
        public int MigrateReverseCustomerExport()
        {
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("CustomerInsightExtract.MigrateReverseCustomerExport");
            int returnCode = 0;
            try
            {
                object[] objCustomersExport = {bcp_directory,CardPrefix,UpdatedBy };
                returnCode = DataAccess.ExecuteNonQuery(connectionString,Constants.SP_MIGRATE_REVERSE_CUSTOMER_EXPORT, ref objCustomersExport);
                return returnCode;

            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                //executeResult.Add(e);
                //errorOutput(e.Message);
                return returnCode;
            }
        }
        #endregion

        #endregion

    }
}
