using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity
{
    public class MCAServiceTokenReq
    {
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
    public class MCAServiceTokenRes
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public List<Claim> Claims { get; set; }

    }

}
