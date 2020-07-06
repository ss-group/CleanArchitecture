using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Common.Exceptions
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public RestException(HttpStatusCode code, ErrorMessage error = null)
        {
            Code = code;
            Errors = error;
        }

        public RestException(HttpStatusCode code, IEnumerable<ErrorMessage> errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public RestException(HttpStatusCode code, string messageCode, params string[] parameters)
        {
            Code = code;
            Errors = new ErrorMessage(messageCode, parameters);

        }
        public object Errors { get; set; }

        public HttpStatusCode Code { get; }
    }
}
