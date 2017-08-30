using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class APIRequest
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<string> Parameters { get; set; }
    }
}