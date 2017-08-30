using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Data;
using System.Text;
using System.Data.OleDb;
using Microsoft.SqlServer.Dts.Runtime;

namespace OfflineTransactionsBatch_V3._0
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Processor
    {
        string folderName = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
        string folderNameWorking = ConfigurationSettings.AppSettings["TempTransactionFileFolderPath"];
        string archiveFolder = ConfigurationSettings.AppSettings["TransactionFileArchivePath"];
        string ResponseFileFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
        int rej = 0;
        string NameOfFile;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            //get the folder where the transaction files are stored
            UtilityManager.LogInformation("Offline Transactions Batch started.");
            string inputFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];

            Processor missingTxnProcessor = new Processor();
            missingTxnProcessor.SearchForFiles(inputFolder);

            #region SSISPackage
            //Changes done for NGCV32 Req.No-007

            Application app = new Application();
            //
            // Load package from file system
            //
            string SSISFolder = ConfigurationSettings.AppSettings["SSISFolderPath"];
            Package package = app.LoadPackage(SSISFolder, null);
            string SSISConfigPath = ConfigurationSettings.AppSettings["SSISConfigFilePath"];
            package.ImportConfigurationFile(SSISConfigPath);
            //Package package = app.LoadPackage("E:\\SSISV32\\CashierID\\CashierID\\CashierIDUpdate.dtsx", null);
            //package.ImportConfigurationFile("c:\\ExamplePackage.dtsConfig");
            //Variables vars = package.Variables;
            //vars["MyVariable"].Value = "value from c#";

            DTSExecResult result = package.Execute();
            if (result == DTSExecResult.Failure)
            {
                foreach (DtsError local_DtsError in package.Errors)
                {
                    Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                    Console.WriteLine();
                }
            }

            //change complete

            #endregion
            //Console.WriteLine("Processed Successfully. Please Look in to response Folder for rejected TXNs.");
            //Console.WriteLine("Press Any to Exit.");
            //Console.Read();
        }

        #region SearchForFiles
        /// <summary>
        /// Commented the old method and modified it for removing the validations for Turkish Implementation
        /// Implemented By Rajan
        /// Requirements are:
        /// Bypass checks to ensure that files received from all stores (TH extension going forwards) 
        /// Bypass validation of Store number 
        /// </summary>
        /// <param name="inputFolder"></param>
        private void SearchForFiles(string inputFolder)
        {
            DataSet dsStores = new DataSet();
            string fileName = "";
            //string storeCode="";
            //string storeName="";
            string fileStore = "";
            bool fileFound = false;
            bool fileRejected = false;
            string fileSeqNo = "";

            try
            {
                string[] txnFiles = Directory.GetFiles(inputFolder);

                for (int fileCount = 0; fileCount < txnFiles.Length; fileCount++)
                {
                    fileName = txnFiles[fileCount];
                    //strip out the folder name
                    fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1, fileName.Length - fileName.LastIndexOf(@"\") - 1);
                    NameOfFile = fileName;
                    Console.WriteLine(fileName);
                    int index1 = fileName.IndexOf("_");
                    if (index1 > -1)
                    {
                        int index2 = fileName.IndexOf("_", index1 + 1);
                        if (index2 > -1)
                        {
                            int index3 = fileName.IndexOf("_", index2 + 1);
                            if (index3 > -1)
                            {
                                int index4 = fileName.IndexOf("_", index3 + 1);
                                if (index3 > -1)
                                {
                                    //********File Store validation**************
                                    fileStore = fileName.Substring(index1 + 1, index2 - index1 - 1);
                                    try
                                    {
                                        if (fileStore.Length < 3 || int.Parse(fileStore) < 1)
                                        {
                                            logError(fileName);
                                            continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logError(fileName);
                                        continue;
                                    }
                                    //**********File Sequence Number Validation***********
                                    fileSeqNo = fileName.Substring(index4 + 1, (fileName.IndexOf('.') - (index4 + 1)));
                                    try
                                    {
                                        if (fileSeqNo.Length < 3 || int.Parse(fileSeqNo) < 1 || int.Parse(fileSeqNo) > 999)
                                        {
                                            logError(fileName);
                                            continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        UtilityManager.LogError(ex.Message);
                                        UtilityManager.LogError("Exception occured in " + NameOfFile);
                                        logError(fileName);
                                        continue;
                                    }
                                    fileFound = true;
                                    ProcessFile(fileName, out fileRejected);
                                    if (fileRejected == true)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    logError(fileName);
                                    continue;
                                }
                            }
                            else
                            {
                                logError(fileName);
                                continue;
                            }
                        }
                        else
                        {
                            logError(fileName);
                            continue;
                        }
                    }
                    else
                    {
                        if (fileName.Trim() == "schema.ini")
                        {
                            continue;
                        }
                        logError(fileName);
                        continue;
                    }
                }
                fileFound = false;
            }
            catch (Exception ex)
            {
                UtilityManager.LogError(ex.Message);
                UtilityManager.LogError("Exception occured in " + NameOfFile);
            }
            finally
            {
                if (rej == 0)
                {
                    UtilityManager.LogInformation("Offline Transactions Batch Completed.");

                    ////Changes done for NGCV32 Req.No-007

                    //Application app = new Application();
                    ////
                    //// Load package from file system
                    ////
                    //string SSISFolder = ConfigurationSettings.AppSettings["SSISFolderPath"];
                    //Package package = app.LoadPackage(SSISFolder, null);
                    //string SSISConfigPath = ConfigurationSettings.AppSettings["SSISConfigFilePath"];
                    //package.ImportConfigurationFile(SSISConfigPath);
                    ////Package package = app.LoadPackage("E:\\SSISV32\\CashierID\\CashierID\\CashierIDUpdate.dtsx", null);
                    ////package.ImportConfigurationFile("c:\\ExamplePackage.dtsConfig");
                    ////Variables vars = package.Variables;
                    ////vars["MyVariable"].Value = "value from c#";

                    //DTSExecResult result = package.Execute();
                    //if (result == DTSExecResult.Failure)
                    //{
                    //    foreach (DtsError local_DtsError in package.Errors)
                    //    {
                    //        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                    //        Console.WriteLine();
                    //    }
                    //}

                    ////change complete
                }


                //sbLog.Append("\r\n"+" Ended Offline Transactions Batch run at "+DateTime.Now.ToString("yyyyMMddHHmmss"));
                //UtilityManager.LogMessage(sb.ToString());
            }
        }
        public void logError(string fileName)
        {
            StreamReader str = new StreamReader(folderName + "\\" + fileName);
            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat");
            string rec = str.ReadLine();
            while (rec != null && rec != null)
            {
                stw.WriteLine(rec);
                rec = str.ReadLine();
            }
            stw.WriteLine("File Rejected with ErrCode 08: File name format is Wrong");
            stw.Close();
            str.Close();
            UtilityManager.LogError(fileName + " Format is wrong.File Name Should be in the format CC_SSS_ccul_YYYYMMDD_QQQ.dat ERRORCode: 08");
            string ExactFileName = fileName;
            if (File.Exists(archiveFolder + "\\" + fileName))
            {
                ExactFileName = ExactFileName.Substring(0, ExactFileName.IndexOf('.'));
                string temp = archiveFolder + "\\" + ExactFileName + "_";
                temp = temp + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".dat";
                File.Move(archiveFolder + "\\" + ExactFileName + ".dat", temp);
                ExactFileName = ExactFileName + ".dat";
            }
            File.Move(folderName + "\\" + fileName, archiveFolder + "\\" + fileName);
        }
        #endregion

        #region ProcessFile
        /// <summary>
        /// 
        /// </summary>
        private void ProcessFile(string fileName, out bool rejected)
        {
            //string fileName="";

            string errMsg = "";
            Validator fileValidator = new Validator();
            //DataSet dsTxns=dbMgr.GetTxnDataSet();
            StringBuilder sbIsolatedStorage = new StringBuilder();
            rejected = false;
            long txnCount;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int noOfRecords, rejectedRecords, existingRecords;
            long insertedRecords;
            TextReader tr = null;
            TextWriter tw = null;

            try
            {
                fileValidator.ValidateFile(fileName, folderName, out rejected, out errMsg, out txnCount);

                if (!rejected)
                {
                    if (File.Exists(folderNameWorking + @"\" + "temp_" + fileName))
                    {
                        File.Delete(folderNameWorking + @"\" + "temp_" + fileName);
                    }
                    tr = new StreamReader(folderName + "\\" + fileName);
                    tw = new StreamWriter(folderNameWorking + @"\" + "temp_" + fileName);
                    while (tr.Peek() != -1)
                    {
                        string s = tr.ReadLine();
                        if ((!(s.StartsWith("H"))) && (!(s.StartsWith("T"))))
                        {
                            tw.WriteLine(s, true);
                        }
                    }
                    tw.Close();
                    tr.Close();
                    DBManager dbMan = new DBManager();

                    dbMan.InsertIntoDB(folderNameWorking + "\\" + "temp_" + fileName, sb, txnCount, out noOfRecords, out rejectedRecords, out insertedRecords, out existingRecords, out rejected);
                }
                else
                {
                    sb.Append("File Rejected with ErrCode " + errMsg);
                    int n = fileName.IndexOf('.');
                    string filename = fileName.Substring(1, n - 1);
                    string LogFileFolder = ConfigurationSettings.AppSettings["LogFileFolderPath"];
                    StreamWriter stw = new StreamWriter(LogFileFolder + "\\Error_" + fileName);
                    stw.WriteLine(sb);
                    stw.Close();
                }

            }
            catch (Exception ex)
            {
                UtilityManager.LogError(ex.Message);
                UtilityManager.LogError("Exception occured in " + NameOfFile);
            }
            finally
            {
                //tw.Close();
                //tr.Close();
                try
                {
                    if (errMsg != "" && errMsg != "999")
                    {
                        UtilityManager.LogError(fileName + " rejected with ErrCode " + errMsg);
                        UtilityManager.LogError("Offline Batch Failed");
                        rej = 1;
                    }
                    else
                    {
                        UtilityManager.LogInformation("Offline Transaction Batch Completed successfully");
                        rej = 1;

                        ////Changes done for NGCV32 Req.No-007
                        //// UtilityManager.LogInformation("SSIS Package Started.");
                        //Application app = new Application();
                        ////
                        //// Load package from file system
                        ////
                        //string SSISFolder = ConfigurationSettings.AppSettings["SSISFolderPath"];
                        //Package package = app.LoadPackage(SSISFolder, null);
                        ////Package package = app.LoadPackage("E:\\SSISV32\\CashierID\\CashierID\\CashierIDUpdate.dtsx", null);
                        //string SSISConfigPath = ConfigurationSettings.AppSettings["SSISConfigFilePath"];
                        //package.ImportConfigurationFile(SSISConfigPath);
                        ////Variables vars = package.Variables;
                        ////vars["MyVariable"].Value = "value from c#";

                        //DTSExecResult result = package.Execute();
                        //if (result == DTSExecResult.Failure)
                        //{
                        //    foreach (DtsError local_DtsError in package.Errors)
                        //    {
                        //        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                        //        Console.WriteLine();
                        //    }
                        //}
                        ////UtilityManager.LogInformation("SSIS Package Completed.");
                        ////change complete
                    }
                    if (File.Exists(folderName + "\\" + fileName))
                    {
                        string ExactFileName = fileName;
                        if (File.Exists(archiveFolder + "\\" + fileName))
                        {
                            ExactFileName = ExactFileName.Substring(0, ExactFileName.IndexOf('.'));
                            string temp = archiveFolder + "\\" + ExactFileName + "_";
                            temp = temp + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".dat";
                            File.Move(archiveFolder + "\\" + ExactFileName + ".dat", temp);
                            ExactFileName = ExactFileName + ".dat";
                        }
                        File.Move(folderName + "\\" + fileName, archiveFolder + "\\" + ExactFileName);
                    }
                }
                catch (Exception ex)
                {
                    UtilityManager.LogError(ex.Message);
                    UtilityManager.LogError("Exception occured in " + NameOfFile);
                }
            }
        }
        #endregion


    }
}
