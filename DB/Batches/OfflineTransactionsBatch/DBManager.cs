#region Using
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Data.OleDb;
#endregion

namespace OfflineTransactionsBatch_V3._0
{
    /// <summary>
    /// Summary description for DBManager.
    /// </summary>
    public class DBManager
    {
        #region Class Variables
        private string connString;
        private StringBuilder sbResponseErrors;
        private static int existingRecords;
        string responseFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
        string summaryFolder = ConfigurationSettings.AppSettings["SummaryFileFolderPath"];
        string archiveFolder = ConfigurationSettings.AppSettings["TransactionFileArchivePath"];
        string TransactionFileFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
        int skeletoncount;
        int invalid;
        #endregion

        #region Constructor
        public DBManager()
        {
            this.connString = System.Configuration.ConfigurationSettings.AppSettings["AdminConnectionString"];
            this.sbResponseErrors = new StringBuilder();
            existingRecords = 0;

        }

        #endregion

        #region InsertIntoDB
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsTxns"></param>
        protected internal void InsertIntoDB(string fileName, StringBuilder sbLog, long txnCount, out int noofRecords, out int rejectedRecords, out long insertedRecords, out int existingRecords, out bool rejected)
        {

            noofRecords = 0;
            rejectedRecords = 0;
            insertedRecords = 0;
            existingRecords = 0;
            rejected = false;
            //int  timeOut=0;
            string spName = "sp_Olt_BulkInsert_Txnfile";
            string formatFilePath = ConfigurationSettings.AppSettings["FormatFilePath"];
            SqlConnection sqlConn = new SqlConnection(this.connString);
            try
            {
                //FileStream fs = File.OpenRead(fileName) ;
                sqlConn.Open();

                SqlCommand sqlComm = new SqlCommand(spName);
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Connection = sqlConn;
                //timeOut = int.Parse(ConfigurationSettings.AppSettings["ApplicationTimeOut"]);
                sqlComm.CommandTimeout = int.Parse(ConfigurationSettings.AppSettings["ApplicationTimeOut"]);
                sqlComm.Parameters.Add("@Path", SqlDbType.VarChar, 250);
                sqlComm.Parameters["@Path"].Value = fileName;
                sqlComm.Parameters.Add("@FormatFilePath", SqlDbType.VarChar, 250);
                sqlComm.Parameters["@FormatFilePath"].Value = formatFilePath;
                sqlComm.Parameters.Add("@NoOfTxns", SqlDbType.BigInt, 8);
                sqlComm.Parameters["@NoOfTxns"].Value = txnCount;

                sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UtilityManager.LogError(ex.Message);
                string fileName1 = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                //int n = fileName1.IndexOf('.');
                int len = fileName1.Length;
                int indx = fileName1.IndexOf('_');
                int L = len - indx - 5;
                fileName1 = fileName1.Substring(indx + 1, L);

                string TransactionFileFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
                string ResponseFileFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
                StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName1 + ".dat");
                StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName1 + "R.dat");
                string rec = str.ReadLine();
                while (rec != null)
                {
                    stw.WriteLine(rec);
                    rec = str.ReadLine();
                }
                stw.WriteLine("File Rejected with Error: " + ex.Message);
                stw.Close();
                str.Close();
                sbLog.Append("\r\n" + " The following error occured during Offline Transactions Batch " + ex.Message);
                sqlConn.Close();
                sqlConn.Dispose();
                rejected = true;
            }
            finally
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            if (!rejected)
            {
                string d = "_";
                char[] delm = d.ToCharArray();
                string[] dlm;
                dlm = fileName.Split(delm);
                string fname = dlm[2];
                //string fname = (fileName.Substring(fileName.IndexOf('_',fileName.IndexOf('_')+3) + 1,3)).Trim();
                int storeNo = int.Parse(fname);

                spName = "sp_Olt_Validate_Offlinetxn";
                string workingFolder = ConfigurationSettings.AppSettings["TempTransactionFileFolderPath"];
                try
                {
                    sqlConn.ConnectionString = this.connString;
                    sqlConn.Open();
                    SqlCommand sqlComm = new SqlCommand(spName);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = int.Parse(ConfigurationSettings.AppSettings["ApplicationTimeOut"]);
                    sqlComm.Connection = sqlConn;
                    sqlComm.Parameters.Add("@storeno", SqlDbType.VarChar, 4);
                    sqlComm.Parameters["@storeno"].Value = storeNo;
                    sqlComm.Parameters["@storeno"].Direction = ParameterDirection.Input;
                    sqlComm.Parameters.Add("@errmsg", SqlDbType.VarChar, 2);
                    sqlComm.Parameters["@errmsg"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@errmsg"].Value = "";
                    sqlComm.Parameters.Add("@Count_Skeleton", SqlDbType.BigInt);
                    sqlComm.Parameters["@Count_Skeleton"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@Count_Skeleton"].Value = "";
                    sqlComm.Parameters.Add("@temppath", SqlDbType.VarChar, 200);
                    sqlComm.Parameters["@temppath"].Direction = ParameterDirection.Input;
                    sqlComm.Parameters["@temppath"].Value = workingFolder;
                    sqlComm.ExecuteNonQuery();
                    skeletoncount = Convert.ToInt32(sqlComm.Parameters["@Count_Skeleton"].Value);
                    //Commented For MKTG00008235 fix starts
                    //if (skeletoncount > 0)
                    //{
                    //    int leftout = ValidateCard(skeletoncount, fileName);
                    //    Console.WriteLine("leftout " + leftout);
                    //if (leftout > 0)
                    //{
                    //    bool ret;
                    //    string inputFile = workingFolder + "\\Skeleton.txt";
                    //    ret = UtilityManager.correctFile(inputFile, "Customer.xml");
                    //    string CopyCustomerXml = ConfigurationSettings.AppSettings["MoveCustomerXmlTo"];
                    //    if (File.Exists(CopyCustomerXml + "\\Customer.xml"))
                    //    {
                    //        string from, to, wor;
                    //        from = CopyCustomerXml + "\\Customer.xml";
                    //        to = CopyCustomerXml + "\\Customer" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Year.ToString() + ".xml";
                    //        wor = workingFolder + "\\Customer.xml";
                    //        File.Move(from, to);
                    //        File.Move(wor, from);
                    //    }
                    //    else
                    //    {
                    //        File.Move(workingFolder + "\\Customer.xml", CopyCustomerXml + "\\Customer.xml");
                    //}
                    //if (ret)
                    //{
                    //    Console.WriteLine("olt");
                    //    Process cmd = new Process();
                    //    cmd.EnableRaisingEvents = false;
                    //    cmd.StartInfo.FileName = ConfigurationSettings.AppSettings["OltBatchPath"];
                    //    cmd.Start();
                    //    cmd.WaitForExit();
                    //}
                    //    }
                    //}
                    //Commented For MKTG00008235 fix ends
                }
                catch (Exception ex)
                {
                    UtilityManager.LogError(ex.Message);
                    UtilityManager.LogError("Exception occured in " + fileName);
                    sbLog.Append("\r\n" + " The following error occured during Offline Transactions Batch " + ex.Message);
                    sqlConn.Close();
                    sqlConn.Dispose();
                    rejected = true;
                }
                finally
                {
                    sqlConn.Close();
                    sqlConn.Dispose();

                }
            }
            if (!rejected)
            {
                spName = "sp_Olt_Addtxn";
                try
                {
                    sqlConn.ConnectionString = this.connString;
                    sqlConn.Open();
                    SqlCommand sqlComm = new SqlCommand(spName);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = int.Parse(ConfigurationSettings.AppSettings["ApplicationTimeOut"]);
                    sqlComm.Connection = sqlConn;

                    sqlComm.Parameters.Add("@NoOfInserted", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfInserted"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfInserted"].Value = "";
                    sqlComm.ExecuteNonQuery();
                    insertedRecords = Convert.ToInt64(sqlComm.Parameters["@NoOfInserted"].Value);

                }
                catch (Exception ex)
                {
                    UtilityManager.LogError(ex.Message);
                    UtilityManager.LogError("Exception occured in " + fileName);
                    sbLog.Append("\r\n" + " The following error occured during Offline Transactions Batch " + ex.Message);
                    sqlConn.Close();
                    sqlConn.Dispose();
                    rejected = true;
                }
                finally
                {
                    sqlConn.Close();
                    sqlConn.Dispose();

                }
            }
            if (!rejected)
            {
                spName = "sp_Olt_Process_All_Non_Imported";
                long NoOfDateFuture = 0, NoOfDateFormatWrong = 0, NoOfStoreUnknown = 0, //NoOfInvalidCardStatus = 0, NoOfInvalidUseStatus = 0, 
                        NoOfSkeleton = 0, NoOfDupTxn = 0, NoOfDuplicate = 0;
                long NoOfNegAmt = 0, NoOfNegPtn = 0, NoOfZeroAmt = 0, NoOfNullField = 0, NoOfStoreNoWrong = 0, NoOfOutOfRange = 0, NoOfInvalidPos = 0;
                try
                {
                    sqlConn.ConnectionString = this.connString;
                    sqlConn.Open();
                    SqlCommand sqlComm = new SqlCommand(spName);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = int.Parse(ConfigurationSettings.AppSettings["ApplicationTimeOut"]);
                    sqlComm.Connection = sqlConn;

                    sqlComm.Parameters.Add("@Path", SqlDbType.VarChar, 250);
                    sqlComm.Parameters["@Path"].Direction = ParameterDirection.Input;
                    sqlComm.Parameters["@Path"].Value = responseFolder;

                    sqlComm.Parameters.Add("@FileName", SqlDbType.VarChar, 50);
                    sqlComm.Parameters["@FileName"].Direction = ParameterDirection.Input;
                    sqlComm.Parameters["@FileName"].Value = fileName.Substring(fileName.IndexOf("_") + 1);

                    sqlComm.Parameters.Add("@NoOfDateFuture", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfDateFuture"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfDateFuture"].Value = "";

                    sqlComm.Parameters.Add("@NoOfDateFormatWrong", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfDateFormatWrong"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfDateFormatWrong"].Value = "";


                    sqlComm.Parameters.Add("@NoOfStoreUnknown", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfStoreUnknown"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfStoreUnknown"].Value = "";

                    //sqlComm.Parameters.Add("@NoOfInvalidCardStatus", SqlDbType.BigInt);
                    //sqlComm.Parameters["@NoOfInvalidCardStatus"].Direction = ParameterDirection.Output;
                    //sqlComm.Parameters["@NoOfInvalidCardStatus"].Value = "";

                    //sqlComm.Parameters.Add("@NoOfInvalidUseStatus", SqlDbType.BigInt);
                    //sqlComm.Parameters["@NoOfInvalidUseStatus"].Direction = ParameterDirection.Output;
                    //sqlComm.Parameters["@NoOfInvalidUseStatus"].Value = "";

                    sqlComm.Parameters.Add("@NoOfSkeleton", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfSkeleton"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfSkeleton"].Value = "";

                    sqlComm.Parameters.Add("@NoOfDupTxn", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfDupTxn"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfDupTxn"].Value = "";

                    sqlComm.Parameters.Add("@NoOfNegAmt", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfNegAmt"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfNegAmt"].Value = "";

                    sqlComm.Parameters.Add("@NoOfNegPtn", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfNegPtn"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfNegPtn"].Value = "";

                    sqlComm.Parameters.Add("@NoOfZeroAmt", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfZeroAmt"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfZeroAmt"].Value = "";

                    sqlComm.Parameters.Add("@NoOfNullField", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfNullField"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfNullField"].Value = "";

                    //sqlComm.Parameters.Add("@NoOfHHeadMis", SqlDbType.BigInt);
                    //sqlComm.Parameters["@NoOfHHeadMis"].Direction = ParameterDirection.Output;
                    //sqlComm.Parameters["@NoOfHHeadMis"].Value = "";

                    sqlComm.Parameters.Add("@NoOfOutOfRange", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfOutOfRange"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfOutOfRange"].Value = "";

                    sqlComm.Parameters.Add("@NoOfStoreNoWrong", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfStoreNoWrong"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfStoreNoWrong"].Value = "";

                    sqlComm.Parameters.Add("@NoOfInvalidPos", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfInvalidPos"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfInvalidPos"].Value = "";

                    sqlComm.Parameters.Add("@NoOfDuplicate", SqlDbType.BigInt);
                    sqlComm.Parameters["@NoOfDuplicate"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@NoOfDuplicate"].Value = "";


                    sqlComm.ExecuteNonQuery();
                    NoOfDateFuture = Convert.ToInt64(sqlComm.Parameters["@NoOfDateFuture"].Value);
                    //errmsgfun(04,fileName);
                    NoOfDateFormatWrong = Convert.ToInt64(sqlComm.Parameters["@NoOfDateFormatWrong"].Value);
                    //errmsgfun(03,fileName);
                    NoOfStoreUnknown = Convert.ToInt64(sqlComm.Parameters["@NoOfStoreUnknown"].Value);
                    //errmsgfun(15, fileName);
                    //NoOfInvalidCardStatus = Convert.ToInt64(sqlComm.Parameters["@NoOfInvalidCardStatus"].Value);
                    //    //errmsgfun(16, fileName);
                    //NoOfInvalidUseStatus = Convert.ToInt64(sqlComm.Parameters["@NoOfInvalidUseStatus"].Value);
                    //    //errmsgfun(17, fileName);

                    NoOfSkeleton = Convert.ToInt64(sqlComm.Parameters["@NoOfSkeleton"].Value);

                    NoOfDupTxn = Convert.ToInt64(sqlComm.Parameters["@NoOfDupTxn"].Value);
                    //errmsgfun(10, fileName);
                    NoOfNegAmt = Convert.ToInt64(sqlComm.Parameters["@NoOfNegAmt"].Value);
                    //errmsgfun(06, fileName);
                    NoOfNegPtn = Convert.ToInt64(sqlComm.Parameters["@NoOfNegPtn"].Value);
                    //errmsgfun(07, fileName);
                    NoOfZeroAmt = Convert.ToInt64(sqlComm.Parameters["@NoOfZeroAmt"].Value);
                    //errmsgfun(08, fileName);
                    NoOfNullField = Convert.ToInt64(sqlComm.Parameters["@NoOfNullField"].Value);
                    //errmsgfun(18, fileName);
                    //NoOfHHeadMis = Convert.ToInt64(sqlComm.Parameters["@NoOfHHeadMis"].Value);
                    //    errmsgfun(12, fileName);
                    NoOfOutOfRange = Convert.ToInt64(sqlComm.Parameters["@NoOfOutOfRange"].Value);
                    //errmsgfun(05, fileName);
                    NoOfStoreNoWrong = Convert.ToInt64(sqlComm.Parameters["@NoOfStoreNoWrong"].Value);
                    //errmsgfun(09, fileName);
                    NoOfInvalidPos = Convert.ToInt64(sqlComm.Parameters["@NoOfInvalidPos"].Value);
                    //errmsgfun(09, fileName);
                    NoOfDuplicate = Convert.ToInt64(sqlComm.Parameters["@NoOfDuplicate"].Value);

                    long sum = NoOfDateFuture + NoOfDateFormatWrong + NoOfStoreUnknown +//NoOfInvalidCardStatus+NoOfInvalidUseStatus
                                +NoOfDupTxn
                               + NoOfNegAmt + NoOfNegPtn + NoOfZeroAmt + NoOfNullField + NoOfStoreNoWrong + NoOfOutOfRange + NoOfInvalidPos + invalid + NoOfDuplicate;
                    UtilityManager.LogInformation(sum + " transaction(s) rejected : For more information goto summary folder");

                    string responseString = string.Empty;
                    summaryFolder = summaryFolder + "\\" + "Summary_" + fileName.Substring(fileName.LastIndexOf("\\") + 6);
                    TextWriter tw = new StreamWriter(summaryFolder);
                    responseString = "Summary of TXNs:" +
                    Environment.NewLine + "---------------------------------------------------------------------" +
                    Environment.NewLine + "No Of Inserted TXNs=" + insertedRecords.ToString() +
                    Environment.NewLine + "*********************************************************************" +
                    Environment.NewLine + "Summary Of Rejected Transactions " +
                    Environment.NewLine + "---------------------------------------------------------------------" +
                    Environment.NewLine + "No Of Date in Future TXNs=" + NoOfDateFuture.ToString() +
                    Environment.NewLine + "No Of Date Formate Wrong TXNs=" + NoOfDateFormatWrong.ToString() +
                    Environment.NewLine + "No of TXNs with Unknown Store Code =" + NoOfStoreUnknown.ToString() +
                        //Environment.NewLine + "No of TXNs with Invalid Card Status Code=" + NoOfInvalidCardStatus.ToString() +
                        //Environment.NewLine + "No Of TXNs with Invalid Use Status Code= " + NoOfInvalidUseStatus.ToString() +
                    Environment.NewLine + "No Of TXNs of Skeleton Customers=" + NoOfSkeleton.ToString() +
                    Environment.NewLine + "No Of Duplicate TXNs= " + NoOfDupTxn.ToString() +
                    Environment.NewLine + "No Of Negative Amount Spent= " + NoOfNegAmt.ToString() +
                    Environment.NewLine + "No Of Negative points records= " + NoOfNegPtn.ToString() +
                    Environment.NewLine + "No Of Zero Amount Spent= " + NoOfZeroAmt.ToString() +
                    Environment.NewLine + "No Of transactions with null Field = " + NoOfNullField.ToString() +
                        //Environment.NewLine + "No card account numbers with no household head= " + NoOfHHeadMis.ToString()+
                    Environment.NewLine + "No Of card account numbers out of range= " + NoOfOutOfRange.ToString() +
                    Environment.NewLine + "No Of Transactions with invalid store code= " + NoOfStoreNoWrong.ToString() +
                    Environment.NewLine + "No Of Transactions with invalid till number or cashier Id= " + NoOfInvalidPos.ToString() +
                    Environment.NewLine + "No Of Duplicate Transactions = " + NoOfDuplicate.ToString() +
                    Environment.NewLine + "No Of Transactions with invalid card number= " + invalid.ToString() +
                    Environment.NewLine + "---------------------------------------------------------------------" +
                    Environment.NewLine + "Total no Of TXNs=" + (insertedRecords + sum).ToString() +
                    Environment.NewLine + "---------------------------------------------------------------------";
                    tw.WriteLine(responseString);
                    tw.Close();
                    //Delete all application created files
                    string inputPath = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
                    string ArchivePath = ConfigurationSettings.AppSettings["TransactionFileArchivePath"];
                    string ExactFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    ExactFileName = ExactFileName.Substring(5);
                    if (File.Exists(inputPath + "\\" + ExactFileName))
                    {
                        if (File.Exists(ArchivePath + "\\" + ExactFileName))
                        {
                            ExactFileName = ExactFileName.Substring(0, ExactFileName.IndexOf('.'));
                            string temp = ArchivePath + "\\" + ExactFileName + "_";
                            temp = temp + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".dat";
                            File.Move(ArchivePath + "\\" + ExactFileName + ".dat", temp);
                            ExactFileName = ExactFileName + ".dat";
                        }
                        File.Move(inputPath + "\\" + ExactFileName, ArchivePath + "\\" + ExactFileName);
                    }
                    string workingPath = ConfigurationSettings.AppSettings["TempTransactionFileFolderPath"];
                    string[] txnFiles = Directory.GetFiles(workingPath);
                    for (int fileCount = 0; fileCount < txnFiles.Length; fileCount++)
                    {
                        fileName = txnFiles[fileCount];
                        File.Delete(fileName);
                    }
                }
                catch (Exception ex)
                {
                    UtilityManager.LogError(ex.Message);
                    UtilityManager.LogError("Exception occured in " + fileName);
                    sbLog.Append("\r\n" + " The following error occured during Offline Transactions Batch " + ex.Message);
                    sqlConn.Close();
                    sqlConn.Dispose();
                    rejected = true;
                }
                finally
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        UtilityManager.LogError(ex.Message);
                        UtilityManager.LogError("Exception occured in " + fileName);
                    }

                }
            }
        }

        //public void errmsgfun(int err, string filename)
        //{
        //    string responseFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
        //    filename = filename.Substring(filename.IndexOf("_")+1, filename.Length - (filename.IndexOf("_")+1)-(filename.Length-filename.LastIndexOf('.')));            
        //    StreamWriter stw = new StreamWriter(responseFolder + "\\" + filename + "R.dat", true);
        //    stw.WriteLine("Transaction(s) rejected with the error code: " + err);
        //    stw.Close();
        //}

        public int ValidateCard(int skeletoncount, string filename)
        {
            string con = ConfigurationSettings.AppSettings["AdminConnectionString"];
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("Select * from TempOfflineSkeleton", sqlcon);
            da.Fill(ds, "TempOfflineSkeleton");
            filename = filename.Substring(filename.IndexOf('_') + 1);
            foreach (DataRow dr in ds.Tables["TempOfflineSkeleton"].Rows)
            {
                string card = dr["card_account_number"].ToString();
                bool val = CalculateCheckDigit(card);
                if (!val)
                {
                    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + filename);
                    StreamWriter stw = new StreamWriter(responseFolder + "\\InvalidClubCard_" + filename.Substring(0, filename.IndexOf('.')) + "R.dat", true);
                    stw.Write(dr[0].ToString());
                    stw.Write("~");
                    stw.Write(dr[1].ToString());
                    stw.Write("~");
                    stw.Write(dr[2].ToString());
                    stw.Write("~");
                    stw.Write(dr[3].ToString());
                    stw.Write("~");
                    stw.Write(dr[4].ToString());
                    stw.Write("~");
                    stw.Write(dr[5].ToString());
                    stw.Write("~");
                    stw.Write(dr[6].ToString());
                    stw.Write("~");
                    stw.Write(dr[7].ToString());
                    stw.WriteLine();
                    stw.Close();
                    str.Close();
                    SqlCommand sqlcmd = new SqlCommand("delete from TempOfflineSkeleton where card_account_number = " + card, sqlcon);
                    sqlcmd.Connection = sqlcon;
                    sqlcmd.ExecuteNonQuery();
                    skeletoncount--;
                    sqlcmd = new SqlCommand("delete from TempOfflineData where card_account_number = " + card, sqlcon);
                    sqlcmd.Connection = sqlcon;
                    sqlcmd.ExecuteNonQuery();
                    invalid++;
                }
            }
            sqlcon.Close();
            sqlcon.Dispose();
            return skeletoncount;
        }

        public static bool CalculateCheckDigit(string cardNumber)
        {
            try
            {

                int cardNumberBound = cardNumber.Length - 1;
                // Card Number must contain at least 2 digits, including the check digit
                if (cardNumberBound < 1)
                {
                    return false;
                }
                int sum = 0;
                // ignore the last digit, as this is the check digit
                for (int i = 0; i < cardNumberBound; i++)
                {
                    int weight = 2 - (i % 2);
                    int digitTimesWeight = int.Parse(cardNumber[i].ToString()) * weight;
                    sum += (digitTimesWeight % 10) + (digitTimesWeight / 10);
                }

                int correctCheckDigit = 10 - (sum % 10);
                // If the check digit is 10, the check digit should be zero
                if (correctCheckDigit == 10)
                {
                    correctCheckDigit = 0;
                }

                int cardCheckDigit = int.Parse(cardNumber[cardNumberBound].ToString());
                return (correctCheckDigit == cardCheckDigit);

            }
            catch
            {
                //UtilityManager.LogError(ex.Message);
                //UtilityManager.LogError("Exception occured in " + fileName);
                return false;
            }
        }


        #endregion

        #region GetDateTime
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txnDate"></param>
        /// <returns></returns>
        private DateTime GetDateTime(string txnDate)
        {
            int year = int.Parse(txnDate.Substring(0, 4));
            int month = int.Parse(txnDate.Substring(4, 2));
            int day = int.Parse(txnDate.Substring(6, 2));
            int hour = int.Parse(txnDate.Substring(9, 2));
            int minute = int.Parse(txnDate.Substring(12, 2));
            int sec = int.Parse(txnDate.Substring(15, 2));
            DateTime dtTxnDate = new DateTime(year, month, day, hour, minute, sec);
            //DateTime dtTxnDate=new DateTime(year,month,day);
            return dtTxnDate;
        }

        protected internal int GetExistingRecordsCount()
        {
            return existingRecords;
        }
        #endregion
    }
}
