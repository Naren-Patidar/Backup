using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Logger
{
    public class EventLogger
    {
        static string LogName = "Clubcard";
        static string EventSource = "Clubcard";
        static EventLog _eventLog;
        public static EventLogger Instance = new EventLogger();

        private EventLogger()
        {
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, LogName);
            }
            _eventLog = new EventLog(LogName, Environment.MachineName, EventSource);
        }

        public void WriteLog(string message)
        {
            try
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("IsEventLogEnable") && ConfigurationManager.AppSettings["IsEventLogEnable"].TryParse<bool>())
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Error);
                }
            }
            catch { }
        }


    }
}
