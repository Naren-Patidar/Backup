using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security
{
    public class SecurityDefinition
    {
        [JsonProperty("customerId")]
        public string CustomerID { get; set; }

        [JsonProperty("dotcomId")]
        public string DotcomID { get; set; }

        public string UUID { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        public long exp { get; set; }

        [JsonProperty("customerMailStatus")]
        public string CustomerMailStatus { get; set; }

        [JsonProperty("customerUseStatus")]
        public string CustomerUseStatus { get; set; }

        [JsonProperty("isActivated")]
        public bool IsActivated { get; set; }

        [JsonProperty("isLeftScheme")]
        public bool HasLeftScheme { get; set; }

        [JsonProperty("isBanned")]
        public bool IsBanned { get; set; }

        [JsonProperty("is2LACleared")]
        public bool HasCleared2LA { get; set; }
    }
}
