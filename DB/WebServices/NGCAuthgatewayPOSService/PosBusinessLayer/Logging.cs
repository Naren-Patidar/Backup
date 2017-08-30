using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Reflection;

namespace Tesco.NGC.PosBusinessLayer
{
    public static class Logging
    {

        #region Constants

        public const string APPLICATION_NAME_IN_LOG_FILE = "AuthGatewayPosService";
        public const string END_PROCEDURE_IN_LOG_FILE = "Exited Procedure";
        public const string START_PROCEDURE_IN_LOG_FILE = "Entered Procedure";
        public const string CATEGORY_NAME_WEB_SERVICE_IN_LOG_FILE = "WebService";
        public const string CATEGORY_NAME_BUSINESS_LAYER_IN_LOG_FILE = "BusinessLayer";
        public const int LOW_PRIORITY = 0;
        public const int HIGH_PRIORITY = 1;
        public const int EVENT_ID = 9000;
        public const string NAMESPACE_WEB_SERVICE = "Tesco.NGC.PosNGCWebService.";
        public const string NAMESPACE_BUSINESS_LAYER = "Tesco.NGC.PosBusinessLayer.";

        #endregion

        #region Public Methods

        /// <summary>This method returns Name of a procedure from where it is called</summary>
        /// <param name="">Nothing</param>
        /// <returns>Method Name of a procedure from where it is called</returns>
        /// <remarks></remarks>
        public static string GetMethodName()
        {
            //Decalre an object of StackTrace class.
            StackTrace stackTrace = new StackTrace();

            //Decalre an object of StackFrame class and the Frame.
            StackFrame stackFrame = stackTrace.GetFrame(1);

            //Decalre an object of MethodBase class and get the method.
            MethodBase methodBase = stackFrame.GetMethod();

            //Return the method name.
            return methodBase.Name;
        }

        /// <summary>
        /// Log all the Informations/Warnings into Log file and Event viewer
        /// </summary>
        /// <param name="message">Warning/Information to be written</param>
        /// <param name="title">Application Name/Project Name</param>
        /// <param name="category">Application layer (eg, UI or Business Layer.)</param>
        /// <param name="priority">Any integer value to display / sort based on priority</param>
        /// <param name="eventId">Any integer value</param>
        /// <param name="severity">Error or Information</param>
        /// <param name="methodName">Source of the Warning/Information. Class name and method name(ClassName.MethodName). Eg, 'UserLogin.Page_Load'</param>
        /// <returns> Nothing </returns>
        /// <remarks>This method accepts Warning/Information message, Application Title, Category and Method name with Class name which may write into Log file and Event viewer</remarks>
        public static void WriteToLogFile(string message, string title, string category, int priority, int eventId, TraceEventType severity, string methodName)
        {
            //Declare an object of LogEntry.
            LogEntry log = new LogEntry();

            //Set the message.
            log.Message = message;

            //Set the title.
            log.Title = title;

            //Add the log category.
            log.Categories.Add(category);

            //Set the priority.
            log.Priority = priority;

            //Set the event id.
            log.EventId = eventId;

            //Set the process name.
            log.ProcessName = methodName;

            //Set the time stamp.
            log.TimeStamp = System.DateTime.Now;

            //Set the severity.
            log.Severity = severity;

            //Save the log details.
            Logger.Write(log);
        }

        #endregion

    }
}
