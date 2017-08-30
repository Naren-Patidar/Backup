using System;

namespace Tesco.ClubcardProducts.MCA.API.Logger
{
    public interface ILoggingService : IDisposable
    {
        void Submit(LogData logData);
        void ErrorException(Exception exception);
    }
}
