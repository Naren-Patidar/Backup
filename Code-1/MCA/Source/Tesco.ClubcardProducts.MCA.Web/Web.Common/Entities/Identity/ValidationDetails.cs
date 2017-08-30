using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity
{
    public class ValidationDetails
    {
        public string UserId { get; set; }
        public string Status { get; set; }
        public List<Claim> Claims { get; set; }
        
        [JsonIgnore]
        public string Expiration 
        {
            get 
            {
                string ctype = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";

                if (this.Claims != null && this.Claims.Any(c => c.claimType == ctype))
                {
                    return this.Claims.Where(c => c.claimType == ctype).FirstOrDefault().value;
                }

                return String.Empty;
            }
        }
    }

    public class Claim
    {
        public string claimType { get; set; }
        public string value { get; set; }
    }
}