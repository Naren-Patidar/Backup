using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class AccountLinkResponse : ResponseBase
    {
        public AccountLinkResponse()
            : this(null, null, null)
        {
        }

        public AccountLinkResponse(string errorLogID, string errorStatusCode, string errorMessage)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
        }
    }
}
