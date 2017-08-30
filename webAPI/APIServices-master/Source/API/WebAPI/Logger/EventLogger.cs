using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.API.ServiceManager;

namespace Tesco.ClubcardProducts.MCA.API.Logger
{
    public class EventLogger
    {
        static string LogName = "MCAAPI";
        static string EventSource = "MCAAPI";
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
                if (GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.IsEventLogEnable) == "1")
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Error);
                }
            }
            catch { }
        }

    }
}
