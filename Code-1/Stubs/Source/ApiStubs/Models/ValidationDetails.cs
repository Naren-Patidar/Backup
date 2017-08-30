using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class ValidationDetails
    {
        public string UserId { get; set; }
        public string Status { get; set; }
        public List<Claim> Claims { get; set; }
    }

    public class Claim
    {
        public string claimType { get; set; }
        public string value { get; set; }
    }

    public class TokenDetails
    {
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string clubcard { get; set; }
    }

    public class OAuthTokenDetails
    {
        public string access_token { get; set; }
        public List<Claim> Claims { get; set; }
    }
}