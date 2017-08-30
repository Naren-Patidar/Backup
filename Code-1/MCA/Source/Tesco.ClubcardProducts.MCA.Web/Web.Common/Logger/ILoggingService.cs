using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using ServiceStack.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Logger
{
    public interface ILoggingService : IDisposable
    {
        void Submit(LogData logData);
        void ErrorException(Exception exception);
    }
}
