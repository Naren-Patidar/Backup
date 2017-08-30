using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Config;
using NLog;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Tesco.ClubcardProducts.MCA.Web.Utilities.NLogger
{
    public class LoggingService : ILoggingService
    {
        private readonly NLog.ILogger _logger= LogManager.GetCurrentClassLogger();
        private bool _disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);


        public bool IsInfoEnabled
        {
            get
            {
                return _logger.IsInfoEnabled;
            }
        }

        public bool IsTraceEnabled
        {
            get
            {
                return _logger.IsTraceEnabled;
            }
        }
        public bool IsErrorEnabled
        {
            get
            {
                return _logger.IsErrorEnabled;
            }
        }


        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args[0]);
        }


        public void ErrorException(Exception ex, string message, params object[] args)
        {
            _logger.Error(ex,message,args[0]);
        }


        public void Trace(string message, params object[] args)
        {
            _logger.Trace(message, args[0]);
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


                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    handle.Dispose();
                    // Free any other managed objects here.
                    //
                }


                // Note disposing has been done.
                _disposed = true;
            }
            else
            {
                return;
            }
        }

        #endregion

    }
}
