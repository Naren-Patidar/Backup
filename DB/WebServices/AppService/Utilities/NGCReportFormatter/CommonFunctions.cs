/*************************************************************************************************************************
 *  File : CommonFunctions.cs
 *  Part of : NGCReportFormatter
 *  Project : NGC V 3.1.2
 *  Objective : Common Functions called from ReportSchedule class
 *  Author : Syed Amjadulla
 *  Date : 30th Sep'2009
 *************************************************************************************************************************/
using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

using System.Configuration;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.Net.Mail;


namespace Tesco.NGC.Utilities
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
