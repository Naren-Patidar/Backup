using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class AddressException 
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }

        public AddressException(string code, string msg)
        {
            this.errorCode = code;
            this.errorMessage = msg;
        }
    }
}