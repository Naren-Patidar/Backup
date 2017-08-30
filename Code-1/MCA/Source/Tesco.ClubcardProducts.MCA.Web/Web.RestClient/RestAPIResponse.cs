using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Tesco.ClubcardProducts.MCA.Web.RestClient
{
    public class RestAPIResponse<T>
    {
        public RestAPIResponse()
        {
            this.Headers = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Headers { get; set; }

        public string CharacterSet { get; set; }

        public string ContentEncoding { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public DateTime LastModified { get; set; }

        public string Method { get; set; }

        public Version ProtocolVersion { get; set; }

        public Uri ResponseUri { get; set; }

        public string Server { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public T Body { get; set; }
    }
}
