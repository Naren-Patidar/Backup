using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Config;
using NLog;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using NLog.Targets.Wrappers;
using NLog.Targets;
using System.Collections;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace Tesco.ClubcardProducts.MCA.API.Logger
{
    public class AuditLoggingService : ILoggingService
    {
        private readonly NLog.ILogger _logger = LogManager.GetLogger("auditLogger");
        private bool _disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Submit(LogData logData)
        {
            Task tLog = new Task(() => this.ExecuteLogSubmit(logData));
            tLog.Start();
        }

        public void ErrorException(Exception exception)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLogSubmit(LogData logData)
        {
            try
            {
                this._logger.Info(this.GetFormattedMessage(logData));

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
                                        String.Format("{0}|{1}|{2}", l.DateTime, logData.Source, l.Message
                                    )));
                }
            }
            catch { }
            return sbLogData.ToString();
        }

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
    }
}
