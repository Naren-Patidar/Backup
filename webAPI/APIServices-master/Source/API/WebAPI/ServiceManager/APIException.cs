using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace Tesco.ClubcardProducts.MCA.API.ServiceManager
{
    public class APIException : HttpResponseException
    {
        public const string KEY_ERR_DATA = "errordetails";

        public APIException(HttpStatusCode code) : base(code)
        {
        }

        public APIException(HttpResponseMessage message) : base(message)
        {
        }

        public APIException(HttpResponseMessage message, string messageDetails)
            : base(message)
        {
            this.Data.Add(KEY_ERR_DATA, messageDetails);
        }
    }

    public class APIUnAuthorizedException : APIException
    {
        private static HttpStatusCode _code = HttpStatusCode.Unauthorized;

        public APIUnAuthorizedException()
            : base(_code)
        {

        }

        public APIUnAuthorizedException(string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = message })
        {

        }

        public APIUnAuthorizedException(APIErrors errCode)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() })
        {

        }

        public APIUnAuthorizedException(APIErrors errCode, string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() }, message)
        {

        }
    }

    public class APIMethodNotSupportedException : APIException
    {
        private static HttpStatusCode _code = HttpStatusCode.MethodNotAllowed;

        public APIMethodNotSupportedException()
            : base(_code)
        {

        }

        public APIMethodNotSupportedException(string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = message })
        {

        }

        public APIMethodNotSupportedException(APIErrors errCode)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() })
        {

        }

        public APIMethodNotSupportedException(APIErrors errCode, string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() }, message)
        {

        }
    }

    public class APIBadRequestException : APIException
    {
        private static HttpStatusCode _code = HttpStatusCode.BadRequest;

        public APIBadRequestException()
            : base(_code)
        {

        }

        public APIBadRequestException(string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = message })
        {

        }

        public APIBadRequestException(APIErrors errCode)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() })
        {

        }

        public APIBadRequestException(APIErrors errCode, string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() }, message)
        {

        }
    }

    public class APIForbiddenException : APIException
    {
        private static HttpStatusCode _code = HttpStatusCode.Forbidden;

        public APIForbiddenException()
            : base(_code)
        {

        }

        public APIForbiddenException(string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = message })
        {

        }

        public APIForbiddenException(APIErrors errCode)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() })
        {

        }

        public APIForbiddenException(APIErrors errCode, string message)
            : base(new HttpResponseMessage() { StatusCode = _code, ReasonPhrase = errCode.ToString() }, message)
        {

        }
    }
}