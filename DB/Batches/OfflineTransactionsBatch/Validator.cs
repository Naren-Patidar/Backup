using System;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OfflineTransactionsBatch_V3._0
{
    /// <summary>
    /// Summary description for Validator.
    /// </summary>
    public class Validator
    {
        public Validator()
        {

        }
        string inputFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
        string ResponseFileFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
        string TransactionFileFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
        string archiveFolder = ConfigurationSettings.AppSettings["TransactionFileArchivePath"];

        #region LatestMothodsForOLT
        public void ValidateFile(string fileName, string folderName, out bool rejectFlag, out string errMsg, out long txnCount)
        {
            string connStr = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + folderName + ";Extended Properties='Text;HDR=No;FMT=Delimited'";
            rejectFlag = false;
            string qryStr = string.Empty;
            txnCount = 0;
            double txnCount1;
            //trimming the filename
            if (fileName.Trim().Length != fileName.Length)
            {
                string fileNameCurrent = folderName + "\\" + fileName;
                string tempFileName = folderName + "\\tempFileTrim.dat";
                File.Copy(fileNameCurrent, tempFileName, true);
                string trimmedFileName = folderName + "\\" + fileName.Trim();
                File.Copy(tempFileName, trimmedFileName, true);
                File.Delete(tempFileName);
                File.Delete(fileNameCurrent);
            }

            if (!CheckOfferPeriod())
            {
                errMsg = "99";
                rejectFlag = true;
                int n = fileName.IndexOf('.');
                string filename = fileName.Substring(1, n - 1);

                StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
                StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
                string rec1 = str.ReadLine();
                while (rec1 != null && rec1 != "")
                {
                    stw.WriteLine(rec1);
                    rec1 = str.ReadLine();
                }
                stw.WriteLine("File Rejected with ErrCode: " + errMsg);
                stw.Close();
                str.Close();
                UtilityManager.LogInformation("Offer peroid does not exists");
                return;
            }
            string filenamestring = "[" + fileName + "]";
            errMsg = "";
            try
            {
                if (File.Exists(folderName + @"\schema.ini"))
                {
                    File.Delete(folderName + @"\schema.ini");
                }
                StreamWriter sw = File.CreateText(folderName + @"\schema.ini");
                sw.WriteLine(filenamestring);
                sw.WriteLine("ColNameHeader=False");
                sw.WriteLine("Format=Delimited(~)");
                sw.Close();
            }
            catch (Exception ex)
            {
                UtilityManager.LogError(ex.Message);
                UtilityManager.LogError("Exception occured in " + fileName);
                rejectFlag = true;
                return;
            }


            //code to check the proper date format in the filename
            //====================================================
            string d = "_";
            char[] delm = d.ToCharArray();
            string[] dlm;
            dlm = fileName.Split(delm);
            string fileDate = dlm[3];

            DateTime dt;
            //bool leap = false;
            int year, month, day;
            try
            {
                year = int.Parse(fileDate.Substring(0, 4));
                month = int.Parse(fileDate.Substring(4, 2));
                day = int.Parse(fileDate.Substring(6, 2));
                dt = new DateTime(year, month, day);
            }
            catch (Exception e)
            {
                DateErr(fileName, out errMsg, out rejectFlag);
                return;
            }
            if (year.ToString().Length < 4 || year > DateTime.Now.Year)
            {
                DateErr(fileName, out errMsg, out rejectFlag);
                return;
            }
            if (dt.Subtract(DateTime.Now.Date).Days > 0)
            {
                errMsg = "04";
                rejectFlag = true;
                int n = fileName.IndexOf('.');
                string filename = fileName.Substring(1, n - 1);

                StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
                StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
                string rec1 = str.ReadLine();
                stw.WriteLine(rec1);
                stw.WriteLine("File Rejected with ErrCode " + errMsg);
                stw.Close();
                str.Close();
                UtilityManager.LogInformation("File creation date is future date");
                return;
            }
            //if (month == 2)
            //{
            //    if (year % 4 == 0)
            //    {
            //        if (year % 100 != 0)
            //        {
            //            leap = true;
            //        }
            //        else
            //        {
            //            if (year % 400 == 0)
            //            {
            //                leap = true;
            //            }
            //        }
            //    }
            //    if (leap)
            //    {
            //        if (day > 28)
            //        {
            //            errMsg = "04";
            //            rejectFlag = true;
            //            int n = fileName.IndexOf('.');
            //            string filename = fileName.Substring(1, n - 1);

            //            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            //            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            //            string rec1 = str.ReadLine();
            //            while (rec1 != null && rec1 != null)
            //            {
            //                stw.WriteLine(rec1);
            //                rec1 = str.ReadLine();
            //            }
            //            stw.WriteLine("File Rejected with ErrCode " + errMsg);
            //            stw.Close();
            //            str.Close();
            //            UtilityManager.LogInformation("Leap year's second month cannot have date after 28");
            //            return;
            //        }
            //    }
            //}


            //check for the delimiter is ~ or not
            //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            int track = 0;
            //int n = fileName.IndexOf('.');
            //string filename = fileName.Substring(1, n - 1);
            double sum = 0;
            StreamReader str3 = new StreamReader(inputFolder + "\\" + fileName);
            string frow = str3.ReadLine();
            if (frow == "")
            {
                HeaderErr(fileName, out errMsg, out rejectFlag);
                str3.Close();
                return;
            }
            StreamWriter stwR = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            StreamWriter stw3 = new StreamWriter(inputFolder + "\\temp.dat");
            int cnt = 0;
            while (frow != null)
            {
                if (frow == "")
                {
                    frow = str3.ReadLine();
                    continue;
                }
                string[] delim = frow.Split('~');
                int m, i, len, k;
                string tlr, tlr2, s = "";
                m = 0;
                i = 0;
                k = 0;
                len = 3;

                //if (frow.IndexOf('H') == 0 || frow.IndexOf('T') == 0)
                //{
                //    len = 3;                
                //}
                if (frow.IndexOf('T') == 0 && track > 0)
                {
                    //len = 3;
                    tlr = frow.Substring(0, frow.LastIndexOf('~') + 1);
                    int l = frow.Length - (frow.LastIndexOf('~') + 1);
                    tlr2 = frow.Substring(frow.LastIndexOf('~') + 1, l);
                    s = tlr.Insert(frow.LastIndexOf('~') + 1, (int.Parse(tlr2) - track).ToString());
                    k = 1;
                }
                if (frow.IndexOf('T') != 0 && frow.IndexOf('H') != 0 && frow.Length > 36)
                {
                    cnt++;
                    len = 7;
                }
                try
                {
                    while (delim[i] != "" && i < len)
                    {
                        i++;
                        m++;
                    }
                    if (k == 1)
                    {
                        stw3.WriteLine(s);
                    }
                    else
                    {
                        stw3.WriteLine(frow);
                    }
                }
                catch (Exception e)
                {
                    if (frow.IndexOf('H') == 0 || frow.IndexOf('T') == 0)
                    {
                        if (m != 4)
                        {
                            if (frow.IndexOf('H') == 0)
                                errMsg = "07";
                            else
                                errMsg = "12";
                            rejectFlag = true;
                            stwR.WriteLine(frow);
                            stwR.WriteLine("Transaction(s) Rejected with ErrCode " + errMsg);
                            stwR.Close();
                            stw3.Close();
                            str3.Close();
                            UtilityManager.LogInformation("Delimeter is not present/invalid");
                            if (File.Exists(inputFolder + "\\temp.dat"))
                            {
                                File.Delete(inputFolder + "\\temp.dat");
                            }
                            return;
                        }
                    }
                    else
                    {
                        if (m != 8)
                        {
                            errMsg = "";
                            track++;
                            if (cnt == 1)
                            {
                                stwR.WriteLine("Transaction(s) rejected with ErrCode " + errMsg);
                                stwR.WriteLine("Delimeter is not present/invalid");
                            }
                            try
                            {
                                sum = sum + double.Parse(delim[i - 2]);
                            }
                            catch (Exception ex)
                            {
                                stwR.WriteLine("Error in amount spent field");
                            }
                            stwR.WriteLine(frow);
                            stwR.WriteLine("------------------------------------------------------------------------------");
                        }
                    }
                }
                frow = str3.ReadLine();
            }
            stw3.Close();
            str3.Close();
            stwR.Close();
            if (cnt == 0)
            {
                rejectFlag = true;
                int n = fileName.IndexOf('.');
                string filename = fileName.Substring(1, n - 1);
                StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
                StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
                string rec1 = str.ReadLine();
                while (rec1 != null)
                {
                    stw.WriteLine(rec1);
                    rec1 = str.ReadLine();
                }
                stw.WriteLine("File Rejected with ErrCode " + errMsg);
                stw.Close();
                str.Close();
                UtilityManager.LogInformation("No transactions are present in the file");
                if (File.Exists(inputFolder + "\\temp.dat"))
                {
                    File.Delete(inputFolder + "\\temp.dat");
                }
                return;
            }
            if (File.Exists(inputFolder + "\\temp.dat") && track > 0)
            {
                if (File.Exists(inputFolder + "\\" + fileName))
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
                    File.Move(inputFolder + "\\" + fileName, archiveFolder + "\\" + fileName);
                    File.Move(inputFolder + "\\temp.dat", inputFolder + "\\" + fileName);
                }
                //File.Replace(inputFolder + "\\temp.dat", inputFolder + "\\" + fileName, BackupLoc + "\\" + fileName);
                File.Delete(inputFolder + "\\temp.dat");
            }
            else if (File.Exists(inputFolder + "\\temp.dat"))
                {
                    File.Delete(inputFolder + "\\temp.dat");
                }

            //int n = fileName.IndexOf('.');
            //string filename = fileName.Substring(1, n - 1);

            if (File.ReadAllLines(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat").Length == 0)
            {
                File.Delete(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat");
            }

            //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\///////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\
            ////string obj2 = "";
            ////qryStr = "Select F4 from " + fileName + " where F1 = 'T'";
            ////try
            ////{
            ////    obj2 = GetFileData(qryStr, connStr);
            ////}
            ////catch(Exception e)
            ////{              
            ////        errMsg = "11";
            ////        rejectFlag = true;
            ////        int n = fileName.IndexOf('.');
            ////        string filename = fileName.Substring(1, n - 1);

            ////        StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////        string rec1 = str.ReadLine();
            ////        while (rec1 != "" && rec1 != null)
            ////        {
            ////            stw.WriteLine(rec1);
            ////            rec1 = str.ReadLine();
            ////        }
            ////        stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////        stw.Close();
            ////        str.Close();
            ////        UtilityManager.LogInformation("Trialer indicator T or Trailer does not exists in the file");
            ////        return;                
            ////}

            ////if (obj2 == null)
            ////{
            ////    errMsg = "99";
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("Ineternal error contact support team");
            ////    return;
            ////}
            //////txnCount = Convert.ToInt64(obj);
            ////try
            ////{
            ////    txnCount1 = Convert.ToDouble(obj2);
            ////}
            ////catch (Exception ex)
            ////{
            ////    errMsg = "15";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);

            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();
            ////    while (rec1 != null && rec1 != null)
            ////    {
            ////        stw.WriteLine(rec1);
            ////        rec1 = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Number of txns in file is diffrenet from the count in the trailer");
            ////    return;
            ////}

            ////if(txnCount1 == 0)
            ////{
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("No transactions exists in the file: see the response folder for " + track + " no of rejected transactions");
            ////    return;
            ////}

            //int fyear = int.Parse(fileName.Substring(fileName.IndexOf('_')+ 5,4));
            //int fmonth = int.Parse(fileName.Substring(fileName.IndexOf('_')+9,2));
            //int fday = int.Parse(fileName.Substring(fileName.IndexOf('_')+11,2));
            //int cyear = DateTime.Now.Year;
            //int cmonth = DateTime.Now.Month;
            //int cdate = DateTime.Now.Day;
            //if(fyear>cyear || (fyear==cyear && fmonth>cmonth) || (fyear==cyear && fmonth==cmonth && fday>cdate))
            //{

            //}
            //string archiveFolder = ConfigurationSettings.AppSettings["TransactionFileArchivePath"];


            //Header validation ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            StreamReader sr = new StreamReader(inputFolder + "\\" + fileName);
            string val = sr.ReadLine();
            string present = val.Substring(0,1);
            if (present != "H")
            {
                HeaderErr(fileName, out errMsg, out rejectFlag);
                return;
            }
            if (present == "H" && sum > 0)
            {
                d = "~";
                delm = d.ToCharArray();
                dlm = val.Split(delm);
                string TxnSum = dlm[3];
                TxnSum = (Convert.ToDecimal(TxnSum) - Convert.ToDecimal(sum)).ToString();
                val = dlm[0] + "~" + dlm[1] + "~" + dlm[2] + "~" + TxnSum;
                StreamWriter newFile = new StreamWriter(inputFolder + "\\NewFile.dat");
                while (val != null)
                {
                    newFile.WriteLine(val);
                    val = sr.ReadLine();
                }
                newFile.Close();                
            }
            sr.Close();
            if (present == "H" && sum > 0)
            {
                File.Delete(inputFolder + "\\" + fileName);
                File.Move(inputFolder + "\\NewFile.dat", inputFolder + "\\" + fileName);
            }
            string obj = string.Empty;
            //double totTxnAmount = 0.0;
            //long txnCount = 0;
            string headerDate = string.Empty;
            string trailerDate = string.Empty;


            // set query to count no of headers
            ////qryStr = "Select count(1) from " + fileName + " where F1 = 'H'";

            ////obj = GetFileData(qryStr,connStr);


            ////if(obj != null)
            ////{
            ////    //if(Convert.ToInt16(obj) != 1 && Convert.ToInt16(obj)==0)
            ////    //{
            ////    //    errMsg="21";
            ////    //    rejectFlag = true;
            ////    //    int n = fileName.IndexOf('.');
            ////    //    string filename = fileName.Substring(1, n - 1);
            ////    //    string TransactionFileFolder = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
            ////    //    string ResponseFileFolder = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
            ////    //    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    //    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\temp_" + fileName );
            ////    //    string header = str.ReadLine();
            ////    //    stw.WriteLine(header);
            ////    //    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    //    stw.Close();
            ////    //    str.Close();
            ////    //    UtilityManager.LogInformation("No header exists");
            ////    //    return;
            ////    //}
            ////    if(Convert.ToInt16(obj) != 1 && Convert.ToInt16(obj)>1)
            ////    {
            ////        errMsg="92";
            ////        rejectFlag = true;
            ////        UtilityManager.LogInformation("moer than one header record exists");
            ////        return;
            ////    }

            ////    // set query to get sequence no from header
            ////    // check the sequence no of header.. 00000001 .....................................................................

            ////    qryStr = "Select F2 from " + fileName + " where F1 = 'H'";

            ////    obj = GetFileData(qryStr,connStr);

            ////    if(obj == null)
            ////    {
            ////        errMsg="99";
            ////        rejectFlag = true;
            ////        UtilityManager.LogInformation("internal error contact support");
            ////        return;
            ////    }

            ////    if (Convert.ToString(obj) != "1")
            ////    {
            ////        errMsg = "02";
            ////        rejectFlag = true;
            ////        int n = fileName.IndexOf('.');
            ////        string filename = fileName.Substring(1, n - 1);
            ////        StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////        string header = str.ReadLine();
            ////        stw.WriteLine(header);
            ////        stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////        stw.Close();
            ////        str.Close();
            ////        UtilityManager.LogInformation("Header sequence number is invalid");
            ////        return;
            ////    }

            StreamReader seqno = new StreamReader(inputFolder + "\\" + fileName);
            string seq = seqno.ReadLine();
            if (seq.IndexOf('H') == 0)
            {
                //string sno = seq.Substring(seq.IndexOf('~')+1, seq.IndexOf('~', seq.IndexOf('~')+1));
                string[] arrHdr = seq.Split('~');

                if (arrHdr[1].Length != 8)
                {
                    errMsg = "02";
                    rejectFlag = true;
                    int n = fileName.IndexOf('.');
                    string filename = fileName.Substring(1, n - 1);
                    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
                    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
                    string header = str.ReadLine();
                    stw.WriteLine(header);
                    stw.WriteLine("File Rejected with ErrCode " + errMsg);
                    stw.Close();
                    str.Close();
                    seqno.Close();
                    UtilityManager.LogInformation("Header seequence number is invalid");
                    return;
                }
            }
            seqno.Close();

            // set query to get filedate from header---------------------------------------------------------------------------
            ////qryStr = "Select F3 from " + fileName + " where F1 = 'H'";

            ////obj = GetFileData(qryStr,connStr);

            ////if(obj == null)
            ////{
            ////    errMsg="99";
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("internal error contact support");
            ////    return;
            ////}
            ////headerDate = obj;

            //////validate header date ******************************************************************************************
            ////DateTime dt1;
            ////try
            ////{
            ////    year = int.Parse(headerDate.Substring(0, 4));
            ////    month = int.Parse(headerDate.Substring(4, 2));
            ////    day = int.Parse(headerDate.Substring(6, 2));
            ////    dt1 = new DateTime(year, month, day);
            ////}
            ////catch (Exception e)
            ////{
            ////    errMsg = "03";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);
            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();

            ////        stw.WriteLine(rec1);

            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date format is wrong in header");
            ////    return;
            ////}
            ////if (year.ToString().Length < 4 || month > 12 || day > 31)
            ////{
            ////    errMsg = "03";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);
            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();

            ////    stw.WriteLine(rec1);

            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date format is wrong in header");
            ////    return;   
            ////}
            ////if (dt1.Subtract(DateTime.Now.Date).Days > 0)
            ////{
            ////    errMsg = "04";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);
            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();
            ////    while (rec1 != null && rec1 != null)
            ////    {
            ////        stw.WriteLine(rec1);
            ////        rec1 = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date is future date in header");
            ////    return;
            ////}
            //if (month == 2)
            //{
            //    if (year % 4 == 0)
            //    {
            //        if (year % 100 != 0)
            //        {
            //            leap = true;
            //        }
            //        else
            //        {
            //            if (year % 400 == 0)
            //            {
            //                leap = true;
            //            }
            //        }
            //    }
            //    if (leap)
            //    {
            //        if (day > 28)
            //        {
            //            errMsg = "04";
            //            rejectFlag = true;
            //            int n = fileName.IndexOf('.');
            //            string filename = fileName.Substring(1, n - 1);

            //            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            //            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            //            string rec1 = str.ReadLine();
            //            while (rec1 != null && rec1 != null)
            //            {
            //                stw.WriteLine(rec1);
            //                rec1 = str.ReadLine();
            //            }
            //            stw.WriteLine("File Rejected with ErrCode " + errMsg);
            //            stw.Close();
            //            str.Close();
            //            UtilityManager.LogInformation("File creation date is future date");
            //            return;
            //        }
            //    }
            //}


            //set query to get the txn amount from header                

            ////qryStr = "Select F4 from " + fileName + " where F1 = 'H'";

            ////obj = GetFileData(qryStr,connStr);
            ////if (obj == null)
            ////{
            ////    errMsg = "99";
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("internal error contact support");
            ////    return;
            ////}
            ////else
            ////{
            ////    if (obj == "")
            ////    {
            ////        errMsg = "05";
            ////        rejectFlag = true;
            ////        int n = fileName.IndexOf('.');
            ////        string filename = fileName.Substring(1, n - 1);
            ////        string TransactionFileFolder1 = ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
            ////        string ResponseFileFolder1 = ConfigurationSettings.AppSettings["ResponseFileFolderPath"];
            ////        StreamReader str = new StreamReader(TransactionFileFolder1 + "\\" + fileName);
            ////        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////        string header = str.ReadLine();
            ////        stw.WriteLine(header);
            ////        stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////        stw.Close();
            ////        str.Close();
            ////        UtilityManager.LogInformation("Total Txn value is invalid");
            ////        return;
            ////    }                    
            ////}

            //check the trailer indicator T exists or not
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //StreamReader str2 = new StreamReader(inputFolder + "\\" + fileName);
            //string rec2 = str2.ReadLine();
            //while (rec2 != "" && rec2 != null)
            //{
            //    if (rec2.Length <= 21 && rec2.IndexOf('H') != 0 && rec2.IndexOf('T')!=0)
            //    {
            //        errMsg = "11";
            //        rejectFlag = true;

            //        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            //        stw.WriteLine(rec2);
            //        stw.WriteLine("File Rejected with ErrCode " + errMsg);
            //        stw.Close();
            //        UtilityManager.LogInformation("Trailer indicaor T does not exists");
            //        str2.Close();
            //        return;
            //    }
            //    rec2 = str2.ReadLine();
            //}
            //str2.Close();
            ////try
            ////{
            ////    totTxnAmount = Convert.ToDouble(obj);
            ////    if (sum > 0)
            ////        totTxnAmount = totTxnAmount - sum;
            ////}
            ////catch (Exception e)
            ////{
            ////    errMsg = "06";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);

            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec = str.ReadLine();
            ////    while (rec != null)
            ////    {
            ////        stw.WriteLine(rec);
            ////        rec = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Total txn value diffrent from sum of the txn values in the file");
            ////    return;
            ////}

            ////// set query to count no of trailers
            ////qryStr = "Select count(1) from " + fileName + " where F1 = 'T'";

            ////obj = GetFileData(qryStr,connStr);
            ////if(obj != null)
            ////{
            ////    if(Convert.ToInt16(obj) != 1 && Convert.ToInt16(obj)>1)
            ////    {
            ////        errMsg="82";
            ////        rejectFlag = true;
            ////        UtilityManager.LogInformation("More than one trailer exists in the file");
            ////        return;
            ////    }
            ////    // set query to get sequence no from trailer
            ////    qryStr = "Select F2 from " + fileName + " where F1 = 'T'";

            ////    obj = GetFileData(qryStr,connStr);

            ////    if(obj == null)
            ////    {
            ////        errMsg="99";
            ////        rejectFlag = true;
            ////        UtilityManager.LogInformation("Ineternal error contact support team");
            ////        return;
            ////    }
            ////    // check the sequence no of trailer.. 00000001
            ////    if(Convert.ToString(obj) != "1")
            ////    {
            ////        errMsg="14";
            ////        rejectFlag = true;
            ////        int n = fileName.IndexOf('.');
            ////        string filename = fileName.Substring(1, n - 1);                        
            ////        StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////        string rec1 = str.ReadLine();
            ////        while (rec1 != "" && rec1 != null)
            ////        {
            ////            if (rec1.Length <= 36 && rec1.IndexOf('T') == 0)
            ////            {
            ////                stw.WriteLine(rec1);
            ////            }
            ////            rec1 = str.ReadLine();
            ////        }
            ////        stw.WriteLine("File Rejected with ErrCode "+errMsg);
            ////        stw.Close();
            ////        str.Close();
            ////        UtilityManager.LogInformation("Invalid sequence number in trailer");
            ////        return; 
            ////    }

            //Validate sequence no of trailer,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            StreamReader rdr = new StreamReader(TransactionFileFolder + "\\" + fileName);
            string r = rdr.ReadLine();
            while (r.IndexOf('T') != 0)
            {
                r = rdr.ReadLine();
            }
            //string tno = seq.Substring(seq.IndexOf('~'), seq.IndexOf('~', seq.IndexOf('~') + 1));
            string[] arrTlr = r.Split('~');
            if (arrTlr[1].Length != 8)
            {
                errMsg = "14";
                rejectFlag = true;
                int n = fileName.IndexOf('.');
                string filename = fileName.Substring(1, n - 1);
                StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
                stw.WriteLine(r);
                stw.WriteLine("File Rejected with ErrCode " + errMsg);
                stw.Close();
                rdr.Close();
                return;
            }
            rdr.Close();
            // set query to get filedate from trailer
            ////qryStr = "Select F3 from " + fileName + " where F1 = 'T'";

            ////obj = GetFileData(qryStr,connStr);

            ////if(obj == null)
            ////{
            ////    errMsg="99";
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("Ineternal error contact support team");
            ////    return;
            ////}
            ////trailerDate = obj;

            //////validate trailer date=============================================================================================
            ////DateTime dt2;
            ////try
            ////{
            ////    year = int.Parse(trailerDate.Substring(0, 4));
            ////    month = int.Parse(trailerDate.Substring(4, 2));
            ////    day = int.Parse(trailerDate.Substring(6, 2));
            ////    dt2 = new DateTime(year, month, day);
            ////}
            ////catch (Exception e)
            ////{
            ////    errMsg = "03";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);


            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();
            ////    while (rec1 != null && rec1 != null)
            ////    {
            ////        stw.WriteLine(rec1);
            ////        rec1 = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date format is wrong in trailer");
            ////    return;
            ////}
            ////if (year.ToString().Length < 4 || month > 12 || day > 31)
            ////{
            ////    errMsg = "03";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);


            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();
            ////    while (rec1 != null && rec1 != null)
            ////    {
            ////        stw.WriteLine(rec1);
            ////        rec1 = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date format is wrong in trailer");
            ////    return;
            ////}
            ////if (dt2.Subtract(DateTime.Now.Date).Days > 0)
            ////{
            ////    errMsg = "04";
            ////    rejectFlag = true;
            ////    int n = fileName.IndexOf('.');
            ////    string filename = fileName.Substring(1, n - 1);
            ////    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////    string rec1 = str.ReadLine();
            ////    while (rec1 != null && rec1 != null)
            ////    {
            ////        stw.WriteLine(rec1);
            ////        rec1 = str.ReadLine();
            ////    }
            ////    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////    stw.Close();
            ////    str.Close();
            ////    UtilityManager.LogInformation("Date is future date in trailer");
            ////    return;
            ////}
            //if (month == 2)
            //{
            //    if (year % 4 == 0)
            //    {
            //        if (year % 100 != 0)
            //        {
            //            leap = true;
            //        }
            //        else
            //        {
            //            if (year % 400 == 0)
            //            {
            //                leap = true;
            //            }
            //        }
            //    }
            //    if (leap)
            //    {
            //        if (day > 28)
            //        {
            //            errMsg = "04";
            //            rejectFlag = true;
            //            int n = fileName.IndexOf('.');
            //            string filename = fileName.Substring(1, n - 1);

            //            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            //            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            //            string rec1 = str.ReadLine();
            //            while (rec1 != null && rec1 != null)
            //            {
            //                stw.WriteLine(rec1);
            //                rec1 = str.ReadLine();
            //            }
            //            stw.WriteLine("File Rejected with ErrCode " + errMsg);
            //            stw.Close();
            //            str.Close();
            //            UtilityManager.LogInformation("File creation date is future date");
            //            return;
            //        }
            //    }
            //}


            //check trailer date is same as header date or not
            ////                    if(!(headerDate.Equals(trailerDate)))
            ////                    {
            //////						DateTime dt = new DateTime();
            //////						dt = DateTime.Parse(headerDate);
            //////						if(dt.Subtract(DateTime.Now.Date).Days > 0)
            //////						{
            ////                        errMsg = "13";
            ////                        rejectFlag = true;
            ////                        int n = fileName.IndexOf('.');
            ////                        string filename = fileName.Substring(1, n - 1);

            ////                        StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////                        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////                        string header = str.ReadLine();
            ////                        stw.WriteLine(header);
            ////                        stw.WriteLine("File Rejected with ErrCode "+errMsg);
            ////                        stw.Close();
            ////                        str.Close();
            ////                        UtilityManager.LogInformation("Header and Trailer date are not same");
            ////                        return;

            //////						}
            ////                    }
            // set query to get the txn count from trailer
            //qryStr = "Select F4 from " + fileName + " where F1 = 'T'";
            //obj = GetFileData(qryStr,connStr);
            //if(obj == null)
            //{
            //    errMsg="99";
            //    rejectFlag = true;
            //    UtilityManager.LogInformation("Ineternal error contact support team");
            //    return;
            //}
            //txnCount = Convert.ToInt64(obj);
            //try
            //{
            //    txnCount1 = Convert.ToDouble(obj);
            //}
            //catch (Exception ex)
            //{
            //    errMsg = "15";
            //    rejectFlag = true;
            //    int n = fileName.IndexOf('.');
            //    string filename = fileName.Substring(1, n - 1);

            //    StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            //    StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            //    string rec1 = str.ReadLine();
            //    while (rec1 != null && rec1 != null)
            //    {
            //        if (rec1.Length <= 36 && rec1.IndexOf('H') != 1)
            //        {
            //            stw.WriteLine(rec1);
            //        }
            //        rec1 = str.ReadLine();
            //    }
            //    stw.WriteLine("File Rejected with ErrCode " + errMsg);
            //    stw.Close();
            //    str.Close();
            //    UtilityManager.LogInformation("Number of txns in file is diffrenet from the count in the trailer");
            //    return;
            //}
            ////    }
            ////    else
            ////    {
            ////        errMsg="99";
            ////        rejectFlag=true;
            ////        UtilityManager.LogInformation("Ineternal error contact support team");
            ////        return;
            ////    }
            ////////    // set query to get the txn count from file
            ////    qryStr = "Select count(1) from " + fileName + " where F1 <> 'H' and F1 <> 'T'";

            ////    obj = GetFileData(qryStr,connStr);
            ////    if(obj == null)
            ////    {
            ////        errMsg="99";
            ////        rejectFlag = true;
            ////        UtilityManager.LogInformation("Ineternal error contact support team");
            ////        return;
            ////    }
            ////    if(txnCount1 != Convert.ToDouble(obj))
            ////    {
            ////        errMsg="15";
            ////        rejectFlag = true;
            ////        int n = fileName.IndexOf('.');
            ////        string filename = fileName.Substring(1, n - 1);

            ////        StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////        StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////        string rec1 = str.ReadLine();
            ////        while (rec1 != null && rec1 != null)
            ////        {
            ////            stw.WriteLine(rec1);
            ////            rec1 = str.ReadLine();
            ////        }
            ////        stw.WriteLine("File Rejected with ErrCode " + errMsg);
            ////        stw.Close();
            ////        str.Close();
            ////        UtilityManager.LogInformation("Number of txns in file is diffrenet from the count in the trailer");
            ////        return;
            ////    }

            ////    // set query to find total of txn amount in file
            ////    qryStr = "Select sum(F7) from " + fileName + " where F1 <> 'H' and F1 <> 'T'";

            ////    obj = GetFileData(qryStr,connStr);
            ////    if (obj != null)
            ////    {
            ////        if (!(diff(Convert.ToDouble(obj), totTxnAmount)))
            ////        {
            ////            errMsg = "06";
            ////            rejectFlag = true;
            ////            int n = fileName.IndexOf('.');
            ////            string filename = fileName.Substring(1, n - 1);

            ////            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            ////            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            ////            string rec = str.ReadLine();
            ////            while (rec != null && rec != null)
            ////            {
            ////                stw.WriteLine(rec);
            ////                rec = str.ReadLine();                            
            ////            }
            ////            stw.WriteLine("File Rejected with ErrCode "+errMsg);
            ////            stw.Close();
            ////            str.Close();
            ////            UtilityManager.LogInformation("Total txn value diffrent from sum of the txn values in the file");
            ////            return;
            ////        }
            ////    }
            ////    else
            ////    {
            ////        rejectFlag = true;
            ////        errMsg = "99";
            ////        UtilityManager.LogInformation("Ineternal error contact support team");              
            ////        return;
            ////    }
            ////}
            ////else
            ////{
            ////    errMsg="99";
            ////    rejectFlag = true;
            ////    UtilityManager.LogInformation("Ineternal error contact support team");
            ////    return;
            ////}
        }
        private bool diff(double amt1, double amt2)
        {
            if (amt1 - amt2 < 0.01 && amt1 - amt2 > -0.01)
                return true;
            else
                return false;
        }

        private string GetFileData(string qryStr, string connStr)
        {
            OleDbConnection con = new OleDbConnection(connStr);
            con.Open();
            OleDbDataAdapter da = new OleDbDataAdapter(string.Empty, con);
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand(qryStr, con);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(ds, "InputFile");

            }
            catch (Exception ex)
            {
                //UtilityManager.LogError(ex.Message);
                //UtilityManager.LogError("Exception occured in " + fileName);
                return null;
            }

            con.Close();

            return ds.Tables[0].Rows[0][0].ToString();
        }

        private DataSet GetRows(string qryStr, string connStr)
        {
            OleDbConnection con = new OleDbConnection(connStr);
            con.Open();
            OleDbDataAdapter da = new OleDbDataAdapter(string.Empty, con);
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand(qryStr, con);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(ds, "InputFile");

            }
            catch (Exception ex)
            {
                //UtilityManager.LogError(ex.Message);
                //UtilityManager.LogError("Exception occured in " + fileName);
                return null;
            }

            con.Close();

            return ds;
        }


        public void HeaderErr(string fileName, out string errMsg, out bool rejectFlag)
        {
            errMsg = "10";
            rejectFlag = true;
            int n = fileName.IndexOf('.');
            string filename = fileName.Substring(1, n - 1);
            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            string header = str.ReadLine();
            if (header == "")
            {
                stw.WriteLine("Header is not found in the first line of the file");
            }
            else
            {
                stw.WriteLine(header);
            }
            stw.WriteLine("File Rejected with ErrCode " + errMsg);
            stw.Close();
            str.Close();
            UtilityManager.LogInformation("Header does not have the indicator H");
            return;
        }

        public void DateErr(string fileName, out string errMsg, out bool rejectFlag)
        {
            errMsg = "03";
            rejectFlag = true;
            int n = fileName.IndexOf('.');
            string filename = fileName.Substring(1, n - 1);
            StreamReader str = new StreamReader(TransactionFileFolder + "\\" + fileName);
            StreamWriter stw = new StreamWriter(ResponseFileFolder + "\\" + fileName.Substring(0, fileName.IndexOf('.')) + "R.dat", true);
            string rec1 = str.ReadLine();
            stw.WriteLine(rec1);
            stw.WriteLine("File Rejected with ErrCode " + errMsg);
            stw.Close();
            str.Close();
            UtilityManager.LogInformation("File creation date format is wrong in filename");
        }

        private bool CheckOfferPeriod()
        {
            SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
            SqlCommand cmdObj = new SqlCommand("SELECT count(1) FROM offer WHERE ((offer.StartDateTime <= GETDATE()) AND(offer.EndDateTime > GETDATE()-1) )");
            cmdObj.Connection = connection;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmdObj;
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count < 0 || ds.Tables[0].Rows[0][0].ToString().Trim() == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
