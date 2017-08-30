using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Tesco.ClubcardProducts.MCA.Web.Utilities.NLogger
{
    public interface ILoggingService : IDisposable
    {
        bool IsInfoEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsTraceEnabled { get; }
        void Info(string message, params object[] args);
        void Trace(string message, params object[] args);
        void ErrorException(Exception exception, string message, params object[] args);
    }

}
