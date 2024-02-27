using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Exceptions
{
    public class BaseException : Exception
    {
        public override string Message { get; }
        public int StatusCode { get; private set; }

        public BaseException(string message, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            Message = message;
            StatusCode = (int)code;
        }

    }

}
