using System.Collections.Generic;
using System.Net;
using AniBand.Auth.Services.Abstractions.Helpers.Generic;

namespace AniBand.Auth.Services.Helpers.Generic
{
    public sealed class HttpResult<T> : HttpResult,IHttpResult<T>
    {
        public T Data { get; set; }

        public bool IsEmpty => Data == null;

        public HttpResult()
        {
            
        }

        public HttpResult(T data)
        {
            Data = data;
        }

        public HttpResult(T data, List<string> errors, HttpStatusCode statusCode) : base(errors, statusCode)
        {
            Data = data;
        }

        public HttpResult(T data, string error, HttpStatusCode statusCode) : base(error, statusCode)
        {
            Data = data;
        }
    }
}