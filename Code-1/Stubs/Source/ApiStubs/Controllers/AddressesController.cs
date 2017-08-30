using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityApiStub.Models;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;

namespace IdentityApiStub.Controllers
{
    public class AddressesController : BaseApiController
    {
        //
        // GET: /Addresses/
        [HttpGet]
        public HttpResponseMessage Index(string id = null, string postcode = null)
        {
            object response = null;            
            Type returnType = null;
            string responseFile = string.Empty;
            if (id.ToUpper().Equals("FIND"))
            {
                responseFile = GetDataFileName(new List<string> { id, string.Format("{0}.{1}", "postcode", postcode) });
                returnType = typeof(List<AddressSummaryResponse>);
            }
            else
            {
                responseFile = GetDataFileName(new List<string> { id }).ToUpper();
                returnType = typeof(Address);
            }

            if (File.Exists(responseFile))
            {
                string strAddresses = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strAddresses, returnType);                
            }
            else
            {                
                return Request.CreateResponse(HttpStatusCode.OK, new  AddressException("ADS001", "Address not found"));
            }
            if (response != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new AddressException("ADS001", "Address not found"));
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }        
    }
}
