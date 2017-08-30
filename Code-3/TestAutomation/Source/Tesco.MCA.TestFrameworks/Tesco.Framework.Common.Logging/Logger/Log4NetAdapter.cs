using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Reflection;
using Tesco.Framework.Common.Logging.Entities;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace Tesco.Framework.Common.Logging.Logger
{
    public sealed class Log4NetAdapter : ILogger
    {
        private static volatile Log4NetAdapter instance;
        private static object syncRoot = new Object();

        private readonly log4net.ILog _log;

        /* Private Constructor */
        private Log4NetAdapter(string appenderName)
        {
            //Set the GlobalContext Property for Log File Name
            log4net.GlobalContext.Properties["LogName"] = appenderName;

            //Configure Log4Net
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(ConfigurationManager.AppSettings["Log4NetConfigPath"]));
            //Gets the object
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static Log4NetAdapter SetInstance(string appenderName)
        {            
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Log4NetAdapter(appenderName);
                }
            }

            return instance;            
        }
        
        /// <summary>
        /// Method to log exception
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex)
        {
            Fault fault = new Fault(ex);
            _log.DebugFormat("FAILED - Debug Details - Message: {0}, Source Class: {1}, Source Method: {2} at Line No. {3}", fault.AdditionalInfo, fault.SourceClass, fault.SourceMethod, fault.LineNumber);
        }

        /// <summary>
        /// Method to log information
        /// </summary>
        /// <param name="ex"></param>
        public void LogInformation(string message)
        {            
            _log.InfoFormat(message);
        }

        /// <summary>
        /// Method to log debug
        /// </summary>
        /// <param name="ex"></param>
        public void LogDebug(string message, object value)
        {
            _log.InfoFormat("{0} :: {1}()" , message, value);
        }


        /// <summary>
        /// Method to log error
        /// </summary>
        /// <param name="ex"></param>
        public void LogError(Exception ex)
        {
            Fault fault = new Fault(ex);
            _log.ErrorFormat("FAILED - Error Occured! Details - Message: {0}, \r\nInnerException: {1}, \r\nSource Class: {2}, \r\nSource Method: {3} at Line No. {4}", fault.AdditionalInfo, fault.InnerException, fault.SourceClass, fault.SourceMethod, fault.LineNumber);
        }

        /// <summary>
        /// Method to log warning
        /// </summary>
        /// <param name="ex"></param>
        public void LogWarning(Exception ex)
        {
            Fault fault = new Fault(ex);
            _log.WarnFormat("FAILED - Warning - Message: {0}, Source Class: {1}, Source Method: {2} at Line No. {3}", fault.AdditionalInfo, fault.SourceClass, fault.SourceMethod, fault.LineNumber);
        }

        /// <summary>
        /// Method to log debug/trace/generic message
        /// </summary>
        /// <param name="source"></param>
        /// <param name="traceType"></param>
        public void LogMessage(string source, TraceEventType traceType)
        {
            _log.InfoFormat("{0} :: {1}()", traceType.ToString(), source);
        }
    }
}



