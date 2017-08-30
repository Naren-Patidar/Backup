/*
 * File   : ImportTransaction.cs
 * Author : Sakthi 
 * email  :
 * File   : This file contains methods/properties related to import tr
 * Date   : 28/Aug/2008
 * 
 */
#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Configuration;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;
using System.Xml;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Data;

#endregion


namespace Tesco.NGC.BatchConsoleApplication
{
    public class ImportTransaction
    {
        #region Field
        /// <summary>
        /// CardNumber
        /// </summary>
        private string cardNumber;


        /// <summary>
        /// UserName
        /// </summary>
        private string userName;

        /// <summary>
        /// cashierID
        /// </summary>
        private string cashierID;


        /// <summary>
        /// transactionType
        /// </summary>
        private string transactionType;


        /// <summary>
        /// store
        /// </summary>
        private string store;

        /// <summary>
        /// posNumber
        /// </summary>
        private Int16 posNumber;

        /// <summary>
        /// transactionNumber
        /// </summary>
        private Int16 transactionNumber;

        /// <summary>
        /// transactionReason
        /// </summary>
        private string transactionReason;

        /// <summary>
        /// date
        /// </summary>
        private string date;

        /// <summary>
        /// time
        /// </summary>
        private string time;

        /// <summary>
        /// salesAmount
        /// </summary>
        private Int16 salesAmount;

        /// <summary>
        /// normalPoints
        /// </summary>
        private Int16 normalPoints;

        /// <summary>
        /// productPoints
        /// </summary>
        private Int16 productPoints;

        /// <summary>
        /// totalPoints
        /// </summary>
        private Int16 totalPoints;

        /// <summary>
        /// welcomePoints
        /// </summary>
        private Int16 welcomePoints;

        /// <summary>
        /// stpfPoints
        /// </summary>
        private Int16 stpfPoints;

        #endregion 

        #region Property
        /// <summary>
        ///  CardNumber
        /// </summary>
        public string CardNumber
        {
            get { return this.cardNumber; }
            set
            {                
                this.cardNumber = HasCorrectCheckDigit(value);
            }
        }


        /// <summary>
        ///  username
        /// </summary>
        public string UserName
        {
            get { return this.userName ; }
            set
            {
                if (value == string.Empty || value.Length > 50)
                {
                    this.userName = "";
                }
                else
                {
                    this.userName = value;
                }
            }
        }

        /// <summary>
        ///  cashierID
        /// </summary>
        public string CashierID
        {
            get { return this.cashierID; }
            set
                {
                    this.cashierID = value;
                }
         }

        /// <summary>
        ///  transactionType
        /// </summary>
        public string TransactionType
        {
            get { return this.transactionType ; }
            set
                {

                    this.transactionType = value;
                }
        }

        /// <summary>
        ///  Store
        /// </summary>
        public string Store
        {
            get { return this.store ; }
            set
            {

                this.store = value;
            }
        }

        /// <summary>
        ///  PosNumber
        /// </summary>
        public Int16 PosNumber
        {
            get { return this.posNumber ; }
            set
            {

                this.posNumber = value;
            }
        }

        /// <summary>
        ///  transactionNumber
        /// </summary>
        public Int16 TransactionNumber
        {
            get { return this.transactionNumber ; }
            set
            {

                this.transactionNumber = value;
            }
        }


        /// <summary>
        ///  transactionReason
        /// </summary>
        public string TransactionReason
        {
            get { return this.transactionReason; }
            set
            {

                this.transactionReason = value;
            }
        }



        /// <summary>
        ///  date
        /// </summary>
        public string Date
        {
            get { return this.date; }
            set
            {
                Regex regex = new Regex("[yyyy-mm-dd]");

                if (!regex.IsMatch(value))
                {
                    this.date = null;
                }
                else
                {
                    this.date = value;
                }
            }

        }



        /// <summary>
        ///  time
        /// </summary>
        public string Time
        {
            get { return this.time ; }
            set
            {
                Regex regex = new Regex("[HH-MM]");

                if (!regex.IsMatch(value))
                {
                    this.time  = null;
                }
                else
                {
                    this.time  = value;
                }
            }
        }


        /// <summary>
        ///  salesAmount
        /// </summary>
        public Int16 SalesAmount
        {
            get { return this.salesAmount; }
            set
            {
                this.salesAmount  = value;
             }
        }

        /// <summary>
        ///  normalPoints
        /// </summary>
        public Int16  NormalPoints
        {
            get { return this.normalPoints ; }
            set
            {      
               this.normalPoints = value;             
            }
        }

        /// <summary>
        ///  ProductPoints
        /// </summary>
        public Int16  ProductPoints
        {
            get { return this.productPoints; }
            set
            {
                this.productPoints = value; 
            }
        }


        /// <summary>
        ///  totalPoints
        /// </summary>
        public Int16 TotalPoints
        {
            get { return this.totalPoints ; }
            set
            {
                this.totalPoints  = value;
            }
        }


        /// <summary>
        ///  WelcomePoints
        /// </summary>
        public Int16 WelcomePoints
        {
            get { return this.welcomePoints; }
            set
            {
                this.welcomePoints = value;
            }
        }

        /// <summary>
        ///  stpfPoints
        /// </summary>
        public Int16 STPFPoints
        {
            get { return this.stpfPoints ; }
            set
            {
                this.stpfPoints = value;
            }
        }

        #endregion

        #region Methods

        #region Batch
        /// <summary>
        /// Execute a Batch Script
        /// </summary>
        /// <param></param>
        /// <returns>True or False</returns>
        public bool Batch(string sXml, string sTag)
        {
            string sFileName = "";
      
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("ImportTransaction.Batch");
            Result executeResult = new Result();
            Int32 nTotalRecordsRead = 0;

            try
            {
                //CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_TRANSACTION, "Daily Summarisation of Collection Periods Started.", false);

                sFileName = CommonFunctions.CreateLogFile(Constants.ACTION_IMPORT_TRANSACTION);
                if (sFileName == "")
                {
                    CommonFunctions.LogFileCreationError(Constants.ACTION_IMPORT_TRANSACTION);
                    return executeResult.Flag;
                }

                CommonFunctions.MessageWriteToLogFile(sFileName, "Started at : " + DateTime.Now);

                Hashtable htImportTxn;
                Hashtable htOutPut = new Hashtable();

                Object[] arr = ConvertXmlHash.XMLToArrayOfHashTable(sXml, sTag);

                for (int i = 0; i < arr.Length; i++)
                {
                    string connectionString = "";
                    if (i==0)
                    {
                    nTotalRecordsRead = arr.Length;
                    connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                    CommonFunctions.MessageWriteToLogFile(sFileName, "Read : " + nTotalRecordsRead + " records");
                    }
                    htImportTxn = (Hashtable)arr[i];

                    UpdateDataBase(htImportTxn, connectionString);
                    htOutPut = new Hashtable();//Reset the HashTable

                }

                executeResult.Flag = true;     
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                executeResult.Add(e);
                //errorOutput(e.Message);
                CommonFunctions.MessageWriteToEventViewer(Constants.EVENTVIEWER_IMPORT_TRANSACTION, "Daily Summarisation Failed.\r\nERROR: '{0}'\r\nReturn Code: '{1}'", true);
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

        #region UpdateDataBase

        private Int32 UpdateDataBase(Hashtable htImportTxn, string connectionString)
        {

            Int32 iResult = -1;
            Trace trace = new Trace();

            TraceState trState = trace.StartProc("ImportCustomer.UpdateDataBase");

            try
            {

                foreach (DictionaryEntry item in htImportTxn)
                {
                    string sKey = item.Key.ToString();
                    string sValue = item.Value.ToString();

                    if (sValue == "")
                    {sValue ="0"; }

                    switch (sKey)
                    {

                        case "Card_Number": { CardNumber = sValue; sValue = CardNumber; break; }
                        case "User_Name": { UserName = sValue; sValue = UserName; break; }
                        case "Cashier_ID": { CashierID = sValue; sValue = CashierID; break; }
                        case "Transaction_Type": { TransactionType = sValue; sValue = TransactionType; break; }
                        case "Store": { Store = sValue; sValue = Store; break; }
                        case "POS_Number": { PosNumber =Convert.ToInt16(sValue); sValue = Convert.ToString(PosNumber); break; }
                        case "Transaction_Number": { TransactionNumber = Convert.ToInt16(sValue); sValue = Convert.ToString(TransactionNumber); break; }
                        //case "Transaction_Source": { TransactionSource = sValue; sValue = TransactionSource; break; }
                        case "Transaction_Reason": { TransactionReason = sValue; sValue = TransactionReason; break; }
                        case "Date": { Date = sValue; sValue = Date; break; }
                        case "Time": { Time = sValue; sValue = Time; break; }
                        case "Sales_Amount": { SalesAmount = Convert.ToInt16(sValue); sValue = Convert.ToString(SalesAmount); break; }
                        case "Normal_Points": { NormalPoints = Convert.ToInt16(sValue); sValue = Convert.ToString(NormalPoints); break; }
                        case "Product_Points": { ProductPoints = Convert.ToInt16(sValue); sValue = Convert.ToString(ProductPoints); break; }
                        case "Total_Points": { TotalPoints = Convert.ToInt16(sValue); sValue = Convert.ToString(TotalPoints); break; }
                        case "Welcome_Points": { WelcomePoints = Convert.ToInt16(sValue); sValue = Convert.ToString(WelcomePoints); break; }
                        case "STPF_Points": { STPFPoints = Convert.ToInt16(sValue); sValue = Convert.ToString(STPFPoints); break; }
                    }                    
                }
                    object[] objDBParams = {CardNumber,UserName,CashierID,TransactionType,Store,PosNumber,TransactionNumber,
                                             TransactionReason,Date,SalesAmount,NormalPoints,ProductPoints,WelcomePoints,STPFPoints,TotalPoints};
                    iResult = DataAccess.ExecuteNonQuery(connectionString, Constants.SP_IMPORT_TRANSACTION, ref objDBParams);
        

                          
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            finally
            {
                trState.EndProc();
            }
            return iResult;
        }

        #endregion UpdateDataBase

        #region HasCorrectCheckDigit
        private static string HasCorrectCheckDigit(string parameters)
        {
            string answer = false.ToString();
            try
            {

                /*if (parameters.Length != 1)
                {
                    return answer;
                }*/
                string cardNumber = parameters;
                int cardNumberBound = cardNumber.Length - 1;
                // Must contain at least 2 digits
                if (cardNumberBound < 1)
                {
                    return answer;
                }

                int cardDigit;
                int sum = 0;
                // ignore the last digit, as this is the check digit
                for (int i = 0; i < cardNumberBound; i++)
                {
                    int weight = 2 - (i % 2);
                    cardDigit = int.Parse(cardNumber[i].ToString());
                    int digitTimesWeight = cardDigit * weight;
                    sum += (digitTimesWeight % 10) + (digitTimesWeight / 10);
                }

                int checkDigit = 10 - (sum % 10);
                // If the last digit is 0, the check digit should be zero
                if (checkDigit == 10)
                {
                    checkDigit = 0;
                }

                cardDigit = int.Parse(cardNumber[cardNumberBound].ToString());
                answer = (checkDigit == cardDigit) ? true.ToString() : false.ToString();
            }
            catch
            {
                // Don't propagate
            }
            if (answer == "True")
            {
                return parameters;
            }
            else
                return "";
        }
        #endregion

    }
  

}
