using System.Collections.Generic;

namespace AniBand.Auth.Services.Helpers
{
    public sealed class HttpResult<T> : HttpResult
    {
        public T Data { get; set; }

        public bool IsEmpty => Data == null;

        public HttpResult()
        {
        }

        public HttpResult(T data) : base()
        {
            Data = data;
        }

        public HttpResult(T data, List<string> errors, int statusCode) : base(errors, statusCode)
        {
            Data = data;
        }

        public HttpResult(T data, string error, int statusCode) : base(error, statusCode)
        {
            Data = data;
        }
    }
}