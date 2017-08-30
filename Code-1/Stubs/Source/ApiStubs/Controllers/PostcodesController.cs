using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityApiStub.Models;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Net.Http;
using System.Net;
using AttributeRouting.Web.Http;


namespace IdentityApiStub.Controllers
{
    public class PostcodesController : BaseApiController
    {
        //
        // GET: /Postcodes/id
        [HttpGet]
        [GET("v4/{postcodes?}")]
        public HttpResponseMessage Get(string postcode)
        {
            object response = null;
            Type returnType = typeof(object);
            string responseFile = string.Empty;
            if (!string.IsNullOrEmpty(postcode))
            {
                responseFile = GetAddressfilename(new List<string> { string.Format("{0}", postcode.ToString().ToLower()) });
                returnType = typeof(List<Postcode>);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new AddressException("404", "Invelid postcode"));
            }
            if (File.Exists(responseFile))
            {
                string strAddresses = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strAddresses, returnType);
            }
            if (response == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new AddressException("404", "Invelid postcode"));
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public object postcodes(string id, string aid)
        {
            object response = null;
            Type returnType = typeof(object);
            string responseFile = string.Empty;
           
            if (!string.IsNullOrEmpty(aid))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    responseFile = GetAddressfilename(new List<string> { string.Format("{0}", id.ToString().ToLower()) });
                   
                    returnType = typeof(List<PostcodeDetails>);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new AddressException("404", "Invelid postcode"));
                }

                if (File.Exists(responseFile))
                {
                    string strAddresses = File.ReadAllText(responseFile);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    response = serializer.Deserialize(strAddresses, returnType);
                }
                if (response == null)
                {
                    
                    return Request.CreateResponse(HttpStatusCode.NotFound, new AddressException("404", "noresponse"));
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
