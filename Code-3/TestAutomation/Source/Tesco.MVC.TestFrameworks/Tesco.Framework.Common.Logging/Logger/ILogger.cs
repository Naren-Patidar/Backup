using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tesco.Framework.Common.Logging.Logger
{
    public interface ILogger
    {
        void LogException(Exception Exception);
        void LogInformation(string Message);
        void LogError(Exception Exception);
        void LogWarning(Exception Exception);
        void LogMessage(string Source, TraceEventType TraceType);
        void LogDebug(string Message, object value);
    }
}
