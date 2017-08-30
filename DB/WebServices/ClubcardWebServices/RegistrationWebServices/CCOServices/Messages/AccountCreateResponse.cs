using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public sealed class AccountCreateResponse: ResponseBase
    {
        private long _clubcardNumber;

        public AccountCreateResponse()
            : this(null, null, null, 0)
        {
        }

        public AccountCreateResponse(long clubcardNumber)
            : this(null, null, null, clubcardNumber)
        {
        }

        public AccountCreateResponse(string errorLogID, string errorStatusCode, string errorMessage, long clubcardNumber)
            : base(errorLogID, errorStatusCode, errorMessage)
        {
            this._clubcardNumber = clubcardNumber;
        }

        [DataMember]
        public long ClubcardNumber
        {
            get { return this._clubcardNumber; }
            set { this._clubcardNumber = value; }
        }
    }
}