using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.ServiceManager
{
    public class ValidationDetails
    {
        public string UserId { get; set; }
        public string Status { get; set; }
        public List<Claim> Claims { get; set; }
        
        [JsonIgnore]
        public string dxshid { get; set; }
    }

    public class Claim
    {
        public string claimType { get; set; }
        public string value { get; set; }
    }
}