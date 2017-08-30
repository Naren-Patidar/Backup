using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Config;
using NLog;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using ServiceStack.Text;
using NLog.Targets.Wrappers;
using NLog.Targets;
using System.Collections;
using System.Web.Routing;
using System.Web;
using System.Threading.Tasks;
using System.IO;

namespace ServiceUtility
{
    public class LoggingService : ILoggingService
    {
        private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
      
        private bool _disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void ErrorException(Exception exception)
        {
            Task tLog = new Task(() => this.ExecuteLogError(exception));
            tLog.Start();
        }

        public void Submit(LogData logData)
        {
            Task tLog = new Task(() => this.ExecuteLogSubmit(logData));
            tLog.Start();
        }

        private void ExecuteLogSubmit(LogData logData)
        {
            try
            {
                switch (logData.Level)
                {
                    case LoggingLevel.none:
                    case LoggingLevel.error:
                        break;
                    case LoggingLevel.low:
                    case LoggingLevel.high:
                        {
                            logData.RecordStep("Exiting...");
                            _logger.Info(this.GetFormattedMessage(logData));
                            break;
                        }
                }
            }
            catch { }
        }

        private void ExecuteLogError(Exception exception)
        {
            try
            {
                LogData logData = null;

                if (exception.Data.Contains("logdata"))
                {
                    logData = exception.Data["logdata"] as LogData;
                }

                Exception innerException = exception.InnerException;

                while (logData == null && innerException != null)
                {
                    if (innerException.Data.Contains("logdata"))
                    {
                        logData = exception.Data["logdata"] as LogData;
                        break;
                    }

                    innerException = innerException.InnerException;
                }

                LoggingLevel level = logData == null ? LoggingLevel.none : logData.Level;
                string sDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff");
                switch (level)
                {
                    case LoggingLevel.none:
                        {
                            break;
                        }
                    case LoggingLevel.error:
                    case LoggingLevel.low:
                        {
                            string msg = this.GetExceptionEventHistory(exception, level);
                            _logger.Info(msg);
                            EventLogger.Instance.WriteLog(msg);
                            break;
                        }
                    case LoggingLevel.high:
                        {
                            _logger.Error(exception,
                                            String.Format("{0}{1}|error|", this.GetExceptionEventHistory(exception, level), sDateTime),
                                            String.Empty);
                            break;
                        }
                }
            }
            catch { }
        }

        private string GetFormattedMessage(LogData logData)
        {
            StringBuilder sbLogData = new StringBuilder();
            try
            {
                if (logData.LogEntries != null && logData.LogEntries.Count > 0)
                {
                    logData.LogEntries.ForEach(l => sbLogData.AppendLine
                                    (
                                        String.Format("{0}|{1}|{2}|{3}", l.DateTime, l.EntryType.ToString(), logData.Source, l.Message
                                    )));
                }
            }
            catch { }
            return sbLogData.ToString();
        }

        private string GetExceptionEventHistory(Exception ex, LoggingLevel level)
        {            
            StringBuilder sbLogData = new StringBuilder();
            try
            {
                List<LogDataAndEvent> lstAllEvents = new List<LogDataAndEvent>();
                lstAllEvents.AddRange(this.GetLogEventsFromException(ex, level));

                Exception innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    lstAllEvents.AddRange(this.GetLogEventsFromException(innerEx, level));
                    innerEx = innerEx.InnerException;
                }

                lstAllEvents.OrderBy(l => l.LogEntry.DateTime).ToList().ForEach(
                            l => sbLogData.AppendLine
                                (
                                    String.Format("{0}|{1}|{2}|{3}",
                                        l.LogEntry.DateTime,
                                        l.LogEntry.EntryType.ToString(),
                                        l.LogData.Source,
                                        l.LogEntry.Message
                                ))
                    );

                var firsLogDataEvent = lstAllEvents.FirstOrDefault();

                if (level == LoggingLevel.error)
                {
                    if (firsLogDataEvent != null)
                    {
                        sbLogData.AppendLine(String.Format("{0}|{1}|{2}|{3}",
                                                            firsLogDataEvent.LogEntry.DateTime,
                                                            LogEntryType.error.ToString(),
                                                            firsLogDataEvent.LogData.Source,
                                                            ex.ToString()));
                    }
                    else
                    {
                        sbLogData.AppendLine(String.Format("{0}|{1}|{2}|{3}",
                                                            DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-ffff"),
                                                            LogEntryType.error.ToString(),
                                                            "General",
                                                            ex.ToString()));
                    }
                }
            }
            catch { }
            return String.Format("{1}{0}\n", 
                                "--------------------------------------------------------------------------------------------",
                                sbLogData.ToString());
        }

        private List<LogDataAndEvent> GetLogEventsFromException(Exception ex, LoggingLevel level)
        {
            List<LogDataAndEvent> logEvents = new List<LogDataAndEvent>();
            try
            {
                if (ex.Data.Contains(LogConfigProvider.EXCLOGDATAKEY))
                {
                    var logData = ex.Data[LogConfigProvider.EXCLOGDATAKEY] as LogData;

                    if (logData.LogEntries != null && logData.LogEntries.Count > 0)
                    {
                        logData.LogEntries.ForEach(l => logEvents.Add(new LogDataAndEvent() { LogData = logData, LogEntry = l }));
                    }

                    if (level == LoggingLevel.low || level == LoggingLevel.error)
                    {
                        logEvents.Add(new LogDataAndEvent()
                        {
                            LogData = logData,
                            LogEntry = new LogEvent()
                                        {
                                            DateTime = logData.LogEntries.OrderByDescending(l => l.DateTime).FirstOrDefault().DateTime,
                                            EntryType = LogEntryType.error,
                                            Message = ex.Message
                                        }
                        });
                    }
                }
            }
            catch { }
            return logEvents;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!_disposed)
            {
                if (disposing)
                {
                    handle.Dispose();
                }

                // Note disposing has been done.
                _disposed = true;
            }
        }

        #endregion
    }
}
