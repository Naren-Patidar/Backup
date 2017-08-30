using System;
using System.Collections.Generic;
using System.Text;
using Tesco.NGC.BatchConsoleApplication;
using System.Configuration;
using System.IO;
using System.Data.Common;
using System.Data;
using System.Xml;

namespace NGCBatchConsoleApplication
{
    /// <summary>
    ///		Batch to update the customer use status and mail status --  V3.1.1[ReqID:012]
    /// </summary>
    public class UpdateCustomerStatus
    {
        #region Fields
        public string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        string BcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string outputdirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string inputPath = ConfigurationSettings.AppSettings["InputRootDirectory"];
        string rejectedPath = ConfigurationSettings.AppSettings["RejecetdDirectory"];
        string ArchiveRootDirectory = ConfigurationSettings.AppSettings["ArchiveRootDirectory"];
        string EventLog = "NGC Batch UpdateCustomerStatus";
        string sBatch = "CustomerStatus_MailStatus";
        string rejectedFile = "CustomerStatus_MailStatus_Rejected";

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

      

        public string OutPutDirectory
        {
            get { return exportPath; }
            set { outputdirectory = value; }
        }

        #endregion

        #region Method
        public bool ExecuteSPs()
        {
             
            string datetime = DateTime.Now.ToString("s");                                             
            string sFileName = CommonFunctions.CreateLogFile(sBatch);
            string message = "Started at" + datetime;            
            rejectedFile = rejectedFile + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;           
            CommonFunctions.MessageWriteToEventViewer(EventLog, message, false);

            string ExactFileName = "CustomerStatus_MailStatus";
            if (!File.Exists(inputPath + "\\CustomerStatus_MailStatus.xls"))
            {
                string sMessage = "Input file is missing";
                CommonFunctions.MessageWriteToEventViewer(EventLog, sMessage, false);
                CommonFunctions.MessageWriteToLogFile(sFileName, sMessage);
                return true;
            }

            //string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\172.29.3.19\Batch\Data\In\CustomerStatus_MailStatus.xls;Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""";
            string filePath = inputPath + "CustomerStatus_MailStatus.xls";
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + @";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""";

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbDataAdapter adapter = factory.CreateDataAdapter();
            DbCommand selectCommand = factory.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM [Sheet1$]";
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connString;

            selectCommand.Connection = connection;

            adapter.SelectCommand = selectCommand;

            DataSet dsCustomerStatus = new DataSet("customers");
            adapter.Fill(dsCustomerStatus);
            dsCustomerStatus.Tables[0].TableName = "customer";
            string xmlDoc = dsCustomerStatus.GetXml();
            xmlDoc = xmlDoc.Replace("x0020_", "");        

            object[] objUpdateStatus = {   Path,          
                                           xmlDoc,
                                           sRecordProcessed,
                                           sRecordRejected
                                           };

            int returncode = DataAccess.ExecuteNonQuery(connectionString, "USP_UpdateBatchCustomerStatus", ref objUpdateStatus);

            object[] objExport = {BcpDirectory,
                                rejectedPath,
                                rejectedFile,
                               };
            int returnExportCode = DataAccess.ExecuteNonQuery(connectionString, "USP_XmlLogRejectedRecords", ref objExport);

            Int32 i = objUpdateStatus.Length - 1;
            sRecordProcessed = Convert.ToString(objUpdateStatus[i - 1]);
            sRecordRejected = Convert.ToString(objUpdateStatus[i]);

            int readRecords = Int32.Parse(sRecordProcessed) + Int32.Parse(sRecordRejected);

            CommonFunctions.MessageWriteToLogFile(sFileName, "Started at " + datetime);
            CommonFunctions.MessageWriteToLogFile(sFileName, "Read " + readRecords + " records");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Updated " + sRecordProcessed + " records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Rejected " + sRecordRejected + " records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Finished at " + DateTime.Now.ToString("s"));

            string sMessageSummary = "";
            sMessageSummary = "UpdateCustomerStatus Finished." + "\n";
            sMessageSummary = sMessageSummary + "Read : " + readRecords + " records." + "\n";
            sMessageSummary = sMessageSummary + "Rejected : " + sRecordRejected + " records." + "\n";
            sMessageSummary = sMessageSummary + "Updated : " + sRecordProcessed + " records." + "\n";
            CommonFunctions.MessageWriteToEventViewer(EventLog, sMessageSummary, false);      
 
            //Archive the input file
            ArchiveInputFile();
            return true;
        }
        #endregion

        private void ArchiveInputFile()
        {
            //Back up the input file after processing the file(move it into Batch\Data\Archive folder)           
            string inputPath = ConfigurationSettings.AppSettings["InputRootDirectory"];
            string ArchiveRootDirectory = ConfigurationSettings.AppSettings["ArchiveRootDirectory"];
            string ExactFileName = "CustomerStatus_MailStatus";
            if (File.Exists(inputPath + "\\CustomerStatus_MailStatus.xls"))
            {
                File.Move(inputPath + "\\CustomerStatus_MailStatus.xls", ArchiveRootDirectory + "\\" + ExactFileName + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
            }

        }
    } 
}
