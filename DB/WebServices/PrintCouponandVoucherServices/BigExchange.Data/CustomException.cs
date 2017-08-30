using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace BigExchange
{
    [DataContract()]
    public class CustomException
    {
        [DataMember()]
        public string StatusCode;
        [DataMember()]
        public string ErrorMessage;
    }
}
