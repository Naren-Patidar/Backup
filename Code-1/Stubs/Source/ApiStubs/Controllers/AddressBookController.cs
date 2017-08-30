using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IdentityApiStub.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;



namespace IdentityApiStub.Controllers
{
    public class AddressBookController : BaseApiController
    {
        //
        // GET: /AddressBook/

        [HttpGet]
        public object PreferredPhysicalAddresses(string topic)
        {


            object response = null;
            string responseFile = string.Empty;
            Type returnType = null;
            string AuthID = string.Empty;

            IEnumerable<string> headerValues;
            var nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("Authorization", out headerValues))
            {
                AuthID = headerValues.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(AuthID))
            {

                responseFile = GetDataFileName(new List<string> { string.Format("{0}.{1}", "PreferredAddresses", AuthID) });
                returnType = typeof(AddressBook);
            }
            if (File.Exists(responseFile))
            {
                string strAddresses = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strAddresses, returnType);
            }
            return response;
        }
        //put: MCA : save address in Contact
        [HttpPut]
        public object PhysicalAddresses (string id)
        {
            object response = null;
            string AuthID = string.Empty;
            AddressBook contact = new AddressBook();
            HttpContent requestContent = Request.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(jsonContent))
            {
                contact = JsonConvert.DeserializeObject<AddressBook>(jsonContent);
            }

            IEnumerable<string> headerValues;
            if (Request.Headers.TryGetValues("Authorization", out headerValues))
            {
                AuthID = headerValues.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(AuthID) && (!string.IsNullOrEmpty(id)) && (!string.IsNullOrEmpty(contact.addressLookupIdentifier)))
            {
                 return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        //GEt:CEC:/addressbook/
        [HttpGet]
        public object PreferredPhysicalAddresses(string userId,string topic)
        {
            object response = null;
            string responseFile = string.Empty;
            Type returnType = null;
            string AuthID = string.Empty;

            IEnumerable<string> headerValues;
            var nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("Authorization", out headerValues))
            {
                AuthID = headerValues.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(AuthID))
            {

                responseFile = GetDataFileName(new List<string> { string.Format("{0}.{1}", "PreferredAddresses", AuthID) });
                returnType = typeof(AddressBook);
            }
            if (File.Exists(responseFile))
            {
                string strAddresses = File.ReadAllText(responseFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Deserialize(strAddresses, returnType);
            }
            return response;
        }
        //put:CEC: save address in Contact
        [HttpPut]
        public object PhysicalAddresses (string id,string aid)
        {
           
            string AuthID = string.Empty;
            AddressBook contact = new AddressBook();
            HttpContent requestContent = Request.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(jsonContent))
            {
                contact = JsonConvert.DeserializeObject<AddressBook>(jsonContent);
            }

            IEnumerable<string> headerValues;
            if (Request.Headers.TryGetValues("Authorization", out headerValues))
            {
                AuthID = headerValues.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(AuthID) && (!string.IsNullOrEmpty(id)) && (!string.IsNullOrEmpty(aid)) && (!string.IsNullOrEmpty(contact.addressLookupIdentifier)))
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
        }


    }

}

