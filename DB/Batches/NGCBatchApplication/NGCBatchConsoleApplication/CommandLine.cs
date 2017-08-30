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
using System.Data.SqlClient;
using System.Data;
using NGCBatchConsoleApplication;

#endregion


namespace Tesco.NGC.BatchConsoleApplication
{
    public class CommandLine
    {
        Hashtable htscripts = new Hashtable();
        //static string [] filenames;
        static void Main(string[] args)
        {
            try
            {                
                #region Parse Argument List

                BlankLine();
                BlankLine();

                string argument = String.Join("", args);
                Regex argumentExpression = new Regex(@"\-(?<option>[\w|\?])\s*(?<value>[\w|\?]*)");

                string username = String.Empty;
                string password = String.Empty;
                string scriptname = String.Empty;

                // Set-up variables for number of parameters and a hashtable
                // for storing all parameters.
                bool parameterError = false;
                Hashtable parameters = new Hashtable(5);
                // Get each of the parameters. Check for duplication and content
                // on each.
                foreach (Match argumentMatch in argumentExpression.Matches(argument))
                {
                    string option = argumentMatch.Groups["option"].Value;
                    string value = argumentMatch.Groups["value"].Value;
                    switch (option)
                    {
                        case Constants.PARAMETER_SCRIPT:
                            if (!AddToHtAndCheck(ref parameters, option, value))
                                parameterError = true;
                            scriptname = value;
                            break;
                        case Constants.PARAMETER_USER:
                            if (!AddToHtAndCheck(ref parameters, option, value))
                                parameterError = true;
                            username = value;
                            break;
                        case Constants.PARAMETER_PWD:
                            if (!AddToHtAndCheck(ref parameters, option, value))
                                parameterError = true;
                            password = value;
                            break;
                        case Constants.PARAMETER_LIST:
                        case Constants.PARAMETER_HELP:
                            if (!AddToHtAndCheck(ref parameters, option, value, false, false))
                                parameterError = true;
                            break;
                        default:
                            break;
                    }
                    // If there has been an error with one of the parameters then stop.
                    if (parameterError)
                    {
                        Help();
                        return;
                    }
                }
                #endregion

                #region Check Parameters
                // Check that parameters have been supplied.
                if (parameters.Count < 1)
                {
                    NoParameter();
                    Help();
                    return;
                }

                // Check that to many parameters haven't been supplied.
                if (parameters.Count > Constants.MAX_PARAMETERS)
                {
                    ToManyParameter();
                    Help();
                    return;
                }

                // Check if help has been requested.
                if (parameters.ContainsKey(Constants.PARAMETER_HELP))
                {
                    Help();
                    return;
                }

                // Check that the correct combination of parameters have been supplied.
                //
                // Check that if s, p or u have been supplied, then they are all supplied
                // together.
                if (parameters.ContainsKey(Constants.PARAMETER_SCRIPT) || parameters.ContainsKey(Constants.PARAMETER_USER) || parameters.ContainsKey(Constants.PARAMETER_PWD))
                {
                    if (!parameters.ContainsKey(Constants.PARAMETER_SCRIPT))
                    {
                        MissingParameter(Constants.PARAMETER_SCRIPT);
                        Help();
                        return;
                    }

                    if (!parameters.ContainsKey(Constants.PARAMETER_USER))
                    {
                        MissingParameter(Constants.PARAMETER_USER);
                        Help();
                        return;
                    }

                    if (!parameters.ContainsKey(Constants.PARAMETER_PWD))
                    {
                        MissingParameter(Constants.PARAMETER_PWD);
                        Help();
                        return;
                    }
                }
                #endregion

                #region User Validation
                //This section validates whether the given USER ID and PASSWORD are in the Database
                                //To DO
                /*int retval = Convert.ToInt32(SqlHelper.ExecuteScalar(Constants.SP_VALIDATE_USER, username, password ));
                if (retval < 1)  
                {
                    InvalidUserCredentials();
                    return;
                }  */                             
                                
                #endregion

                #region Hashtable for Scripts
                //New Reuirement               
                
                //string 
                //End New Requiurement
                #endregion

                //bool scriptFound = false;
                string[] filenames = {};
                //string resultXml = "";
                string inputRootDirectory = "";
                string ArchiveRootDirectory = "";
                inputRootDirectory = ConfigurationSettings.AppSettings["InputRootDirectory"];
                ArchiveRootDirectory = ConfigurationSettings.AppSettings["ArchiveRootDirectory"];
               //bool success = BatchList(out filenames, out resultXml, out inputRootDirectory);
                //if (!success)
                //{
                //    ServerError(resultXml);
                //    return;
                //}

                // If requested list available scripts
                if (parameters.ContainsKey(Constants.PARAMETER_LIST))
                {
                    ListScripts(filenames);
                    return;
                }

                // Check the script to run is available
                //foreach (string filename in filenames)
                //{
                //    if (filename == scriptname)
                //    {
                //        scriptFound = true;
                //        break;
                //    }
                //}
                //if (!scriptFound)
                //{
                //    UnknownScript(scriptname);
                //    ListScripts(filenames);
                //    return;
                //}

                bool bSuccess;

                switch (scriptname)
                {                    
                    case Constants.ACTION_IMPORT_CUSTOMER:
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(inputRootDirectory + "Customer.xml");
                            //doc.Load(inputRootDirectory + "Customer_New.xml");
                            string inputXML = doc.InnerXml;
                            
                            ImportCustomer objImpCust = new ImportCustomer();
                            bSuccess = objImpCust.Batch(inputXML, "customers/customer");

                                            
                            break;
                        }
                    case Constants.ACTION_TRANSACTION_EXTRACTS:
                        {
                            TransactionExtracts objTrnExtr = new TransactionExtracts();
                            bSuccess = objTrnExtr.Batch(Convert.ToInt32(args[7]));
                            break;
                        }
                    case Constants.ACTION_AGGREGATE_OFFER:
                        {
                            AggregateOffer objAggOffer = new AggregateOffer();
                            bSuccess = objAggOffer.Batch();
                            break;
                        }
                    case Constants.ACTION_AGGREGATE_CUSTOMER:
                        {
                            AggregateCustomer objAggCustomer = new AggregateCustomer();
                            bSuccess = objAggCustomer.Batch();
                            break;
                        }

                    case Constants.ACTION_IMPORT_TRANSACTION:
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(inputRootDirectory + "txn.xml");
                            string intputXML = doc.InnerXml;

                            ImportTransaction objImpTxn = new ImportTransaction();
                            bSuccess = objImpTxn.Batch(intputXML, "txns/txn");
                            break;

                        }

                    case Constants.BATCH_HOUSEKEEPING:
                        {
                            try
                            {
                                HouseKeeping objHousekeep = new HouseKeeping();
                                //objHousekeep.TransactionRetention = Convert.ToInt32(args[7]);
                                //objHousekeep.AuditRetentionPeriod = Convert.ToInt32(args[9]);
                                objHousekeep.ExecuteMethods();
                                
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please Enter the Parameters in Correct Format");
                                Console.WriteLine("Reason : " + e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_CALCULATE_REWARDS:
                        {
                            try
                            {

                                CalculateRewards objCalcRewards = new CalculateRewards();
                                objCalcRewards.ExecuteMethods();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please Enter the Parameters in Correct Format");
                                Console.WriteLine("Reason : " + e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_REISSUE_REWARD:
                        {
                            try
                            {

                                ReIssueReward objReIssueRewards = new ReIssueReward();
                                objReIssueRewards.ExecuteMethods();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please Enter the Parameters in Correct Format");
                                Console.WriteLine("Reason : " + e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_REWARD_MAILING:
                        {
                            try
                            {

                                RewardMailing objRewardMailing = new RewardMailing();                                
                                objRewardMailing.ExecuteSPs();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please Enter the Parameters in Correct Format");
                                Console.WriteLine("Reason : " + e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_CUSTOMER_INSIGHT_EXTRACT:
                        {
                            try
                            {

                                CustomerInsightExtract objCustInsExtarct = new CustomerInsightExtract();
                                objCustInsExtarct.ExecuteSPS();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please Enter the Parameters in Correct Format");
                                Console.WriteLine("Reason : " + e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_POINTS_PARTNER_TRANSACTION_EXTRACT:
                        {
                            try
                            {

                                PointsPartnerTransactionExtract objPointsPartTrans = new PointsPartnerTransactionExtract();
                                objPointsPartTrans.ExecuteSPs();
                                //CommonFunctions.MessageWriteToLogFile(sFileName, "All the Stored Procedures Are Executed SuccessFully");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_CARD_ACCOUNT_EXTRACT:
                        {
                            try
                            {

                                CardAccountExtracts objCardActExt = new CardAccountExtracts();
                                Int32 dateOffset;
                                string updateBy;
                                
                                if (args.Length.ToString() == "6")
                                {
                                    dateOffset = 1;
                                    updateBy = "0";
                                }
                                else
                                {
                                    dateOffset = Convert.ToInt32(args[7]);
                                    updateBy = args[9];
                                }
                                objCardActExt.ExecuteSPs(dateOffset, updateBy);
                                //CommonFunctions.MessageWriteToLogFile(sFileName, "All the Stored Procedures Are Executed SuccessFully");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case Constants.BATCH_AGENCY_PARTNER_EXTRACT:
                        {
                            try
                            {

                                AgencyPartnerExtract objAgenPartExt = new AgencyPartnerExtract();
                                objAgenPartExt.ExecuteSPs();
                                //CommonFunctions.MessageWriteToLogFile(sFileName, "All the Stored Procedures Are Executed SuccessFully");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case Constants.ACTION_AGGREGATE_TRANSACTION:
                        {
                            AggregateTxn objAggTransaction = new AggregateTxn();
                            bSuccess = objAggTransaction.ExecuteMethods();
                            break;
                        }
                    case Constants.BATCH_REISSUE_REWARDS:
                        {
                            ReIssueRewards objRRewards = new ReIssueRewards();
                            bSuccess = objRRewards.ExecuteMethods();
                            break;
                        }
                    case Constants.BATCH_ADDRESS_IN_ERROR:
                        {
                            AddressInError objAddressInError = new AddressInError();
                            bSuccess = objAddressInError.ExecuteSPs();
                            break;
                        }
                    case Constants.BATCH_UPDATECUSTOMERSTATUS_MAILSTATUS:
                        {
                            UpdateCustomerStatus objUpdateCustStatus = new UpdateCustomerStatus();
                            bSuccess = objUpdateCustStatus.ExecuteSPs();
                            break;
                        }
                    case Constants.BATCH_UPDATE_CUSTOMER_PREFERENCE:
                        {
                            UpdateDataProtectionPreference objUpdatePreference = new UpdateDataProtectionPreference();
                            bSuccess = objUpdatePreference.ExecuteSPs();
                            break;
                        }
                    case Constants.ACTION_DELETE_TRACE_FILES:
                        {
                            HouseKeeping objHouseKeeping = new HouseKeeping();
                            bSuccess = objHouseKeeping.Delete_Older_TraceFiles();
                            break;
                        }

                    default:
                        {
                            UnknownScript(scriptname);
                            //ListScripts(filenames);
                            ShowScripts();
                            Console.ReadLine();
                            break;
                        }
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred executing the Batch script");
                Console.WriteLine("Reason: " + e.Message);
            }
            System.Environment.Exit(0);
        }

        

        

        #region Get Files
        /// <summary>
        /// Get a list of files from a directory without the
        /// path.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The pattern of ilfes to look for.</param>
        /// <param name="checkForPath">Check for path before searching.</param>
        /// <returns>An array of just the file names.</returns>
        private static string[] GetFiles( string path, string searchPattern, bool checkForPath )
        {
            if (checkForPath)
            {
                if (!Directory.Exists(path))
                    return new string[0];
            }
            // Get the scripts from the root directory.
            string[] filePaths = Directory.GetFiles(path, searchPattern);
            string[] fileNames = new string[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++)
                fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
            return fileNames;
        }
        #endregion

        #region Merge Arrays
        /// <summary>
        /// Merge a list of arrays into a single array. The order of the
        /// arrays in the object[] determines the order of precedence.
        /// The method will de-duplicate the arrays so that an item only
        /// exists once.
        /// </summary>
        /// <param name="arrays">An object array of arrays.</param>
        /// <returns>A single array with all values.</returns>
        private static string[] MergeArrays(object[] arrays)
        {
            ArrayList all = new ArrayList();
            foreach (object array in arrays)
            {
                // Check that this is an array, otherwise just move
                // onto the next.
                if (array is string[])
                {
                    // Loop round the array and add into the array list
                    // if the entry doesn't already exist.
                    string[] list = (string[])array;
                    for (int i = 0; i < list.Length; i++)
                    {
                        string element = list[i];
                        if (!all.Contains(element))
                        {
                            all.Add(element);
                        }
                    }
                }
            }
            // Sort the final array.
            all.Sort();
            // Convert the arraylist into an array.
            string[] final = new string[all.Count];
            all.CopyTo(final);
            return final;
        }
        #endregion

        #region Messages
        private static void Help()
        {
            Console.WriteLine("BatchApplication can be called with three different sets of options");
            BlankLine();
            Console.WriteLine("(1) BatchApplication -u <username>");
            Console.WriteLine("                     -p <password>");
            Console.WriteLine("                     -s <scriptname> [additional arguments required by the script]");
            Console.WriteLine("      runs the specified scripts");
            BlankLine();
            Console.WriteLine("(2) BatchApplication -l");
            Console.WriteLine("      list available scripts");
            BlankLine();
            Console.WriteLine("(3) BatchApplication -?");
            Console.WriteLine("      displays this message");
            BlankLine();
        }

        private static void ServerError(string resultXml)
        {
            Result serverResult = new Result();
            serverResult.LoadXml(resultXml);
            string uiMessage;
            serverResult.MoveToTopResult();
            serverResult.GetResultElementByName("ui_message", out uiMessage);
            Console.WriteLine(uiMessage);
        }

        private static void ToManyParameter()
        {
            Console.WriteLine("Too many parameters have been supplied");
            BlankLine();
        }

        private static void MissingParameter(string parameter)
        {
            Console.WriteLine("The parameter '" + parameter + "' must be supplied");
            BlankLine();
        }

        private static void DuplicateParameter(string parameter)
        {
            Console.WriteLine("The parameter '" + parameter + "' has been supplied more than once");
            BlankLine();
        }

        private static void NoParameter()
        {
            Console.WriteLine("No parameters have been supplied");
            BlankLine();
        }

        private static void EmptyParameter(string parameter)
        {
            Console.WriteLine("No value has been supplied for the '" + parameter + "' parameter");
            BlankLine();
        }

        private static void UnknownScript(string scriptname)
        {
            Console.WriteLine("The script '" + scriptname + "' is not available");
            BlankLine();
        }

        private static void BlankLine()
        {
            Console.WriteLine("");
        }

        private static void InvalidUserCredentials()
        {
            Console.WriteLine("Invalid User Name or Password");
            BlankLine();
        }
        
        private static void ListScripts(string[] filenames)
        {
            Console.WriteLine("Available scripts:");
            foreach (string filename in filenames)
            {
                Console.WriteLine("    " + filename);
            }
            Console.WriteLine("Total available scripts: " + (filenames.GetUpperBound(0) + 1));
            BlankLine();
        }

        private static void LogDetails(string scriptname, string logFile)
        {
            Console.WriteLine("The log of " + scriptname + " is at " + logFile);
            BlankLine();
        }

        private static bool AddToHtAndCheck(ref Hashtable parameters, string parameter, string parameterValue, bool checkDuplicate, bool checkEmpty)
        {
            if (checkDuplicate)
            {
                if (parameters.ContainsKey(parameter))
                {
                    DuplicateParameter(parameter);
                    return false;
                }
            }
            if (checkEmpty)
            {
                if (StringUtils.IsStringEmpty(parameterValue))
                {
                    EmptyParameter(parameter);
                    return false;
                }
            }
            if (parameters.ContainsKey(parameter))
            {
                parameters[parameter] = parameterValue;
            }
            else
            {
                parameters.Add(parameter, parameterValue);
            }
            return true;
        }

        private static bool AddToHtAndCheck(ref Hashtable parameters, string parameter, string parameterValue)
        {
            return AddToHtAndCheck(ref parameters, parameter, parameterValue, true, true);
        }
        #endregion

        #region Show All Script Names
        private static void ShowScripts()
        {
            Hashtable htscripts = new Hashtable();            
            htscripts.Add("ACTION_IMPORT_CUSTOMER", "import_customer");            
            htscripts.Add("ACTION_TRANSACTION_EXTRACTS", "import_customer_my1");            
            htscripts.Add("ACTION_AGGREGATE_CUSTOMER", "aggregate_customers");           
            htscripts.Add("ACTION_AGGREGATE_OFFER", "aggregate_offers");            
            htscripts.Add("ACTION_AGGREGATE_TRANSACTION", "aggregate_txns");            
            htscripts.Add("ACTION_IMPORT_TRANSACTION", "import_transaction");            
            htscripts.Add("BATCH_HOUSEKEEPING", "housekeeping");
            htscripts.Add("BATCH_REISSUE_REWARD", "reissue_rewards");             
            htscripts.Add("BATCH_REISSUE_REWARDS", "reissue_high_rewards");             
            htscripts.Add("BATCH_REWARD_MAILING", "reward_mailing");             
            htscripts.Add("BATCH_CALCULATE_REWARDS", "calculate_rewards");             
            htscripts.Add("BATCH_CUSTOMER_INSIGHT_EXTRACT", "migrate_customers_reverse");
            htscripts.Add("BATCH_POINTS_PARTNER_TRANSACTION_EXTRACT", "migrate_txn4pp_reverse");             
            htscripts.Add("BATCH_CARD_ACCOUNT_EXTRACT", "migrate_card_accounts_reverse");             
            htscripts.Add("BATCH_AGENCY_PARTNER_EXTRACT", "migrate_partner_agency");
            htscripts.Add("ACTION_DELETE_TRACE_FILES", "delete_trace_files");

            Console.WriteLine("Available scripts:");
            foreach (string i in htscripts.Values)
            {
               Console.WriteLine(i.ToString());
                
            }

        }
        #endregion
    }
}
