using System.Collections.Generic;
using System.Linq;
using System.Net;
using AniBand.Auth.Services.Abstractions.Helpers;

namespace AniBand.Auth.Services.Helpers
{
    public class HttpResult : IHttpResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public List<string> Errors { get; set; }

        public bool IsSuccessful => (Errors == null || !Errors.Any()) && StatusCode == HttpStatusCode.OK;

        public HttpResult()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public HttpResult(string error, int statusCode)
        {
            Errors = new List<string>();
            Errors.Add(error);
            StatusCode = (HttpStatusCode)statusCode;
        }
        
        public HttpResult(List<string> errors, int statusCode)
        {
            Errors = new List<string>(errors);
            StatusCode = (HttpStatusCode)statusCode;
        }

        public void AddError(string error)
        {
            if (Errors == null)
            {
                Errors = new List<string>();
            }
            
            Errors.Add(error);
        }
    }
}