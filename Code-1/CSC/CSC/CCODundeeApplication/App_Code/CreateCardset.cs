using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCODundeeApplication.App_Code
{
    public class CreateCardsetRequest
    {
        public string customerUuid { get; set; }
        public string stateOfOldCard { get; set; }
        public string cardSetRequestId { get; set; }
    }
    public class CreateCardsetResponse
    {
        public string cardSetRequestId { get; set; }
      //  public string cardNumber { get; set; }         
    }
    public class GetAccessTokenRequest
    {
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class GetAccessTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
    }


    public enum StateOfOldCard
    {
        compromised,
        notCompromised,
        none
    }

   
}