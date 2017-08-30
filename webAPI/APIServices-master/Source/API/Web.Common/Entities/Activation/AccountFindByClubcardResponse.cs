using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation
{
    public class AccountFindByClubcardResponse : BaseEntity<AccountFindByClubcardResponse>
    {
        public AccountFindByClubcardResponse()
        {

        }

        public string ContactDetailMatchStatus { get; set; }
        public bool Matched { get; set; }
        public string ErrorMessage { get; set; }
    }
}
