using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class ValidateClubcardAccountExistsResponse : ResponseBase
    {
        private bool _matched;

        public ValidateClubcardAccountExistsResponse()
            : this(null, null, null, false)
        {
        }

        public ValidateClubcardAccountExistsResponse(bool matched) : this(null, null, null, matched)
        {
        }

        public ValidateClubcardAccountExistsResponse(string errorLogID, string errorStatusCode, string errorMessage, bool matched) : base(errorLogID, errorStatusCode, errorMessage)
        {
            this._matched = matched;
        }

        [DataMember]
        public bool Matched
        {
            get { return this._matched; }
            set { this._matched = value; }
        }
    }
}
