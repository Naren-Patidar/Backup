using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteSheetAutomation
{
    public static class Constants
    {
        public const string strFileSuccesLogs = "SuccessLogs.txt";
        public const string strFileFailureLogs = "FailureLogs.txt";
        public const string strFileApplicationFailureLogs = "ApplicationFailureLogs.txt";
        public const string strFileApplicationSummary = "ApplicationSummary.txt";
        
        //Messages
        public const string strStartInputFileParsing = "Start of Input File Parsing.";
        public const string strEndInputFileParsing = "End of Input File Parsing(File Parsed Successfully).";
        public const string strStartSQLQueryExecution = "Start of SQL Query Execution.";
        public const string strEndSQLQueryExecution = "End of SQL Query Execution(SQL Operations held Successfully).";
        public const string strStartOutputFileCreation = "Start of Output File Creation and load data into PDWS, MDWS and Mailing files.";
        public const string strEndOutputFileCreation = "End of Output File Creation and load data into PDWS, MDWS and Mailing files.";
        public const string strError = "Error in the process.";
        public const string strStartProcess = "==========================START PROCESS==========================";
        public const string strEndProcess = "==========================END PROCESS==========================";

    }
}
