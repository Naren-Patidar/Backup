using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IdentityApiStub.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;

namespace IdentityApiStub.Controllers
{
    public class BaseApiController : ApiController
    {
        [NonAction]
        public string GetDataFileName(List<string> parameters)
        {
            string name = string.Empty;
            try
            {
                APIRequest req = new APIRequest();
                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as string;                
                string pars = string.Empty;
                parameters.ForEach(p => pars += p + ".");
                name = string.Format("{0}.{1}txt", controller, pars);
                name = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DataFolder"]), name);
            }
            catch { }
            return name;
        }

        [NonAction]
        public string GetAddressfilename(List<string> parameters)
        {
            string name = string.Empty;
            string parentpath = string.Empty;
            try
            {
                APIRequest req = new APIRequest();
                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as string;
                string pars = string.Empty;
                parameters.ForEach(p => pars += p + ".");
                name = string.Format("{0}.{1}txt", controller, pars);
                name = Path.Combine(ConfigurationManager.AppSettings["AddressDataFolder"], name);

            }
            catch { }
            return name;
        }

    }
}
