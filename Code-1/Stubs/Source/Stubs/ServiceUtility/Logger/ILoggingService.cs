using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using ServiceStack.Text;

namespace ServiceUtility
{
    public interface ILoggingService : IDisposable
    {
        void Submit(LogData logData);
        void ErrorException(Exception exception);
    }
}
