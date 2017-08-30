using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Xml;
using System.Configuration;
using Tesco.NGC.BatchConsoleApplication;

namespace NGCBatchConsoleApplication
{
    class UpdateDataProtectionPreference
    {
        #region Folders

        string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        string BcpDirectory = ConfigurationSettings.AppSettings["BcpDirectory"];
        string outputdirectory = ConfigurationSettings.AppSettings["OutputRootDirectory"];
        string inputPath = ConfigurationSettings.AppSettings["InputRootDirectory"];
        string rejectedPath = ConfigurationSettings.AppSettings["RejecetdDirectory"];
        string ArchiveRootDirectory = ConfigurationSettings.AppSettings["ArchiveRootDirectory"];
        string EventLog = "NGC Batch DataProtectionPreference";
        string sBatch = "Data_Protection_Preference";
        string rejectedFile = "Data_Protection_Preference_Rejected";

        private string sRecordProcessed;
        private string sRecordRejected;

        #endregion

        #region Property
        public string Path
        {
            get { return inputPath; }
            set { Path = value; }
        }

        #endregion

        #region Method

        public bool ExecuteSPs()
        {

            string datetime = DateTime.Now.ToString("s");
            string sFileName = CommonFunctions.CreateLogFile(sBatch);
            string message = "Started at " + datetime;
            rejectedFile = rejectedFile + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            CommonFunctions.MessageWriteToEventViewer(EventLog, message, false);

            string ExactFileName = "Data_Protection_Preference";
            if (!File.Exists(inputPath + "\\Data_Protection_Preference.xls"))
            {
                string sMessage = "Input file is missing";
                CommonFunctions.MessageWriteToEventViewer(EventLog, sMessage, false);
                CommonFunctions.MessageWriteToLogFile(sFileName, sMessage);
                return true;
            }

            //string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\172.29.3.19\Batch\Data\In\CustomerStatus_MailStatus.xls;Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""";
            string filePath = inputPath + "Data_Protection_Preference.xls";
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + @";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""";

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbDataAdapter adapter = factory.CreateDataAdapter();
            DbCommand selectCommand = factory.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM [Sheet1$]";
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connString;

            selectCommand.Connection = connection;

            adapter.SelectCommand = selectCommand;

            DataSet dsCustomerPreference = new DataSet("Prefereces");
            adapter.Fill(dsCustomerPreference);
            dsCustomerPreference.Tables[0].TableName = "Preference";
            string xmlDoc = dsCustomerPreference.GetXml();
            xmlDoc = xmlDoc.Replace("x0020_", "");
            xmlDoc = xmlDoc.Replace("x002F__", "");

            object[] objUpdateStatus = {   Path,          
                                           xmlDoc,
                                           sRecordProcessed,
                                           sRecordRejected
                                           };

            int returncode = DataAccess.ExecuteNonQuery(connectionString, "USP_UpdateBatchDataProtectionPreference", ref objUpdateStatus);

            Int32 i = objUpdateStatus.Length - 1;
            sRecordProcessed = Convert.ToString(objUpdateStatus[i - 1]);
            sRecordRejected = Convert.ToString(objUpdateStatus[i]);

            if (Convert.ToInt32(objUpdateStatus[i]) > 0)
            {
                object[] objExport = {
                                rejectedPath,
                                rejectedFile,
                               };
                int returnExportCode = DataAccess.ExecuteNonQuery(connectionString, "USP_XmlLogRejectedRecords_CP", ref objExport);
            }           

            int readRecords = Int32.Parse(sRecordProcessed) + Int32.Parse(sRecordRejected);

            CommonFunctions.MessageWriteToLogFile(sFileName, "Started at " + datetime);
            CommonFunctions.MessageWriteToLogFile(sFileName, "Read " + readRecords + " records");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Inserted\\Updated " + sRecordProcessed + " records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Rejected " + sRecordRejected + " records.");
            CommonFunctions.MessageWriteToLogFile(sFileName, "Finished at " + DateTime.Now.ToString("s"));

            string sMessageSummary = "";
            sMessageSummary = "UpdateCustomerStatus Finished." + "\n";
            sMessageSummary = sMessageSummary + "Read : " + readRecords + " records." + "\n";
            sMessageSummary = sMessageSummary + "Rejected : " + sRecordRejected + " records." + "\n";
            sMessageSummary = sMessageSummary + "Inserted\\Updated : " + sRecordProcessed + " records." + "\n";
            CommonFunctions.MessageWriteToEventViewer(EventLog, sMessageSummary, false);

            CommonFunctions.MessageWriteToEventViewer(EventLog, "Finished at " + DateTime.Now.ToString("s"), false);
            //Archive the input file
            ArchiveInputFile();
            return true;
        }

        private void ArchiveInputFile()
        {
            //Back up the input file after processing the file(move it into Batch\Data\Archive folder)           
            string ExactFileName = "Data_Protection_Preference";
            if (File.Exists(inputPath + "\\Data_Protection_Preference.xls"))
            {
                File.Move(inputPath + "\\Data_Protection_Preference.xls", ArchiveRootDirectory + "\\" + ExactFileName + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
            }
        }

        #endregion
    }
}
