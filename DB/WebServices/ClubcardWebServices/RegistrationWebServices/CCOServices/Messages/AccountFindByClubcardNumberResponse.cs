using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class AccountFindByClubcardNumberResponse : ResponseBase
    {
        private bool _matched;
        private string _contactDetailMatchStatus;

        public AccountFindByClubcardNumberResponse()
            : this(null, null, null, false, null)
        {
        }

        public AccountFindByClubcardNumberResponse(bool matched, string contactDetailMatchStatus)
            : this(null, null, null, matched, contactDetailMatchStatus)
        {
        }

        public AccountFindByClubcardNumberResponse(string errorLogID, string errorStatusCode, string errorMessage, bool matched, string contactDetailMatchStatus)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
            this._matched = matched;
            this._contactDetailMatchStatus = contactDetailMatchStatus;
        }

        [DataMember]
        public bool Matched
        {
            get { return this._matched; }
            set { this._matched = value; }
        }

        [DataMember]
        public string ContactDetailMatchStatus
        {
            get { return this._contactDetailMatchStatus; }
            set { this._contactDetailMatchStatus = value; }
        }
    }
}
