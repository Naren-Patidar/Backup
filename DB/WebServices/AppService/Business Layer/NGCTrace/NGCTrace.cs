using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace NGCTrace
{
   
   
    public static class NGCTrace
    {
        
        public static void TraceError(string message)
        {
            Logger.Write(message, "Error", 1, 4500, System.Diagnostics.TraceEventType.Error,  ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

        }
        public static void TraceCritical(string message)
        {
            Logger.Write(message, "Critical", 2, 4500, System.Diagnostics.TraceEventType.Critical, ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

        }
        public static void TraceWarning(string message)
        {
            Logger.Write(message, "Warning", 3, 4500, System.Diagnostics.TraceEventType.Warning, ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

        }
        public static void TraceInfo(string message)
        {
            Logger.Write(message, "Information", 5, 4500, System.Diagnostics.TraceEventType.Information, ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

        }
        public static void TraceDebug(string message)
        {
            Logger.Write(message, "Debug", 4, 4500, System.Diagnostics.TraceEventType.Verbose, ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

        }
        public static void ExeptionHandling(Exception ex)
        {

            if (!EventLog.SourceExists(ConfigurationSettings.AppSettings["ProjectTitle"].ToString()))
            {
                EventLog.CreateEventSource(ConfigurationSettings.AppSettings["ProjectTitle"].ToString(), ConfigurationSettings.AppSettings["ProjectTitle"].ToString());

            }
            EventLog.WriteEntry(ConfigurationSettings.AppSettings["ProjectTitle"].ToString(), ex.ToString(), EventLogEntryType.Error);
          

        }

    }
}
