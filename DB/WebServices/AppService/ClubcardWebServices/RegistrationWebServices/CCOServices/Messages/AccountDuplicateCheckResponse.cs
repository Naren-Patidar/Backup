using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class AccountDuplicateCheckResponse : ResponseBase
    {
        private bool _hasDuplicates;

        public AccountDuplicateCheckResponse()
            : this(null, null, null, false)
        {
        }

        public AccountDuplicateCheckResponse(bool hasDuplicates)
            : this(null, null, null, hasDuplicates)
        {
        }

        public AccountDuplicateCheckResponse(string errorLogID, string errorStatusCode, string errorMessage, bool hasDuplicates)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
            this._hasDuplicates = hasDuplicates;
        }

        [DataMember]
        public bool HasDuplicates
        {
            get { return this._hasDuplicates; }
            set { this._hasDuplicates = value; }
        }
    }
}
