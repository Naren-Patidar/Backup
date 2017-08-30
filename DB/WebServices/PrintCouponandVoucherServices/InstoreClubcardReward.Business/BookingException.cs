using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class  BookingException : Exception
    {
        public ErrorTypes ErrorType { get; set; }

        public BookingException()
        {
            
        }
        public BookingException(ErrorTypes errorType, string message)
            : base(message)
        {
            ErrorType = errorType;
        }
        public BookingException(ErrorTypes errorType, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorType = errorType;
        }
        protected BookingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }

    }
}
