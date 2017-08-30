using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using System.Configuration;
using Tesco.NGC.DataAccessLayer;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Tesco.NGC.BatchConsoleApplication
{
    public class AddressInError
    {
        #region Fields                      
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        string BcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string outputdirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string inputPath = ConfigurationSettings.AppSettings["InputRootDirectory"];      
        string fileFormat = ConfigurationSettings.AppSettings["AddressInErrorFileFormat"];
        string EventLog = "NGC Batch Address In Error";
        string sBatch = "AddressInError";
        
        private string sRecordProcessed;
        private string sRecordRejected;
        private string exportPath;

        #endregion

        #region Property
        public string Path
        {
            get { return inputPath; }
            set { Path = value; }

        }

        public string FileFormat
        {
            get { return fileFormat; }
            set { FileFormat = value; }
        }

        public string OutPutDirectory
        {
            get { return exportPath; }
            set { OutPutDirectory = value; }
        }

        #endregion

        #region Method
        public bool ExecuteSPs()
        {
          
            string message = "Address_In_Error Started.";
            string sFileName = CommonFunctions.CreateLogFile(sBatch);
            CommonFunctions.MessageWriteToLogFile(sFileName, message);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message, false);

            object[] objUpdateStatus = {   Path,
                                           FileFormat,
                                           sRecordProcessed,
                                           sRecordRejected
                                           };

            object[] objExport = { outputdirectory,
                                   BcpDirectory
                                 };

            int returncode = DataAccess.ExecuteNonQuery(connectionString, "USP_UpdateCustomerStatusAddressInError", ref objUpdateStatus);

            Int32 i = objUpdateStatus.Length - 1;
            sRecordProcessed = Convert.ToString(objUpdateStatus[i - 1]);
            sRecordRejected = Convert.ToString(objUpdateStatus[i]);


            int returnexportcode = DataAccess.ExecuteNonQuery(connectionString, "USP_AddressInErrorExport", ref objExport);

            string message1 = "Address In Error Finished. Updated " + "  " + sRecordProcessed + " Record Processed Card Accounts" + ", " + "Rejected  " + sRecordRejected + " Card Accounts";

            string sFileName1 = CommonFunctions.CreateLogFile(sBatch);
            CommonFunctions.MessageWriteToLogFile(sFileName1, message1);
            CommonFunctions.MessageWriteToEventViewer(EventLog, message1, false);
            return true;
        }
        #endregion

    }
}
