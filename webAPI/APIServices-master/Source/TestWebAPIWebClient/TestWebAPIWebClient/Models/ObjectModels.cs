using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TestWebAPIWebClient.Models
{
    public class HomeData
    {
        public string APIGatewayURL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CurrentLoggedAccessToken { get; set; }
        public string CurrentLoggedUUID { get; set; }
        public string RequestBody { get; set; }
        public string Response { get; set; }

        [DisplayName("Require Identity Login")]
        public bool RequireIdentityCheck { get; set; }
        public string Hash { get; set; }

        public GatewayMetadata gatewayMetadata { get; set; }
        public string SelectedService { get; set; }
        public string SelectedOperation { get; set; }
    }

    public class LoginClaim
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public List<Claim> Claims { get; set; }
        public string uuid { get; set; }
    }

    public class Claim
    {
        public string claimType { get; set; }
        public string value { get; set; }
    }

    public class GatewayMetadata
    {
        public IEnumerable<KeyValuePair<string, api>> Metadata { get; set; }
    }

    public class api
    {
        public string name { get; set; }
        public List<operation> operations { get; set; }
        public api()
        {
            this.operations = new List<operation>();
        }
    }

    public class operation
    {
        public string name { get; set; }
        public List<parameter> Parameters { get; set; }

        public operation()
        {
            this.Parameters = new List<parameter>();
        }
    }

    public class parameter
    {
        public string name { get; set; }
        public string ptype { get; set; }
    }

    public class APIRequest
    {
        public string service { get; set; }
        public string operation { get; set; }
        public List<KeyValuePair<string, object>> parameters { get; set; }
    }

    public class TescoUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}