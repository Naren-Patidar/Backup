using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class AccountUpdateResponse : ResponseBase
    {
        public AccountUpdateResponse()
            : this(null, null, null)
        {
        }

        public AccountUpdateResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }
    }
}
