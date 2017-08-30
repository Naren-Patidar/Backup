/*
 * File   : Customer.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains common methods related to Batch Application.. Eg-->Manage Log File
 * Date   : 28/Aug/2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace Tesco.NGC.BatchConsoleApplication
{
    public static class CommonFunctions
    {
        #region CreateLogFile
        public static string CreateLogFile(string sBatch)
        {
            string sFileName = "";
            string sLogRootDirectory = ConfigurationSettings.AppSettings["LogRootDirectory"];
            sLogRootDirectory = sLogRootDirectory.Trim();

            if (sLogRootDirectory.Substring(sLogRootDirectory.Length - 1, 1) != "\\")
            {
                sLogRootDirectory = sLogRootDirectory + "\\";
            }

            if (!Directory.Exists(sLogRootDirectory))
            {
                Directory.CreateDirectory(sLogRootDirectory);
            }

            sFileName = sLogRootDirectory + sBatch + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_"
                + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".log";

            StreamWriter sw = new StreamWriter(sFileName);
            if (!File.Exists(sFileName))
            {
                sFileName = string.Empty;
            }
            sw.Close();
            return sFileName;
        }
        #endregion
               
        #region MessageWriteToLogFile
        public static void MessageWriteToLogFile(string sFileName, string sMessageToWrite)
        {
            if (File.Exists(sFileName))
            {
                StreamWriter sw = new StreamWriter(sFileName, true);
                sw.WriteLine(sMessageToWrite);
                sw.Close();
            }
        }
        #endregion

        #region LogFileCreationError
        public static void LogFileCreationError(string sBatch)
        {
            Console.WriteLine("Failed to create log file for the Batch : " + sBatch);
            Console.WriteLine("");
        }
        #endregion

        #region MessageWriteToEventViewer
        public static void MessageWriteToEventViewer(string messageEventLog, string message, bool isError)
        {
            if (isError)
            {
                System.Diagnostics.EventLog.WriteEntry(messageEventLog, message, System.Diagnostics.EventLogEntryType.Error);
            }
            else
            {
                System.Diagnostics.EventLog.WriteEntry(messageEventLog, message, System.Diagnostics.EventLogEntryType.Information);
            }
        }
        #endregion
    }
}
