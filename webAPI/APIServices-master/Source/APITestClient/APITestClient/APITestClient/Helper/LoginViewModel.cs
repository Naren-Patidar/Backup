using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITestClient.Helper
{
    public class LoginViewModel
    {
        public string ClubcardNumber { get; set; }
        public string Password { get; set; }
        public string DotcomCustomerId { get; set; }
    }
}