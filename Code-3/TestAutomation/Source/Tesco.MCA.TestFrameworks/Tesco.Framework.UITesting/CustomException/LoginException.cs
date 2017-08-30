using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.CustomException
{
        class LoginException : Exception
        {
            public LoginException()
                : base() { }

            public LoginException(string message)
                : base(message) { }

            public LoginException(string format, params object[] args)
                : base(string.Format(format, args)) { }

            public LoginException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    
}
