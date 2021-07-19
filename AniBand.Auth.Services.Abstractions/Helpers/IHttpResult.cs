using System.Collections.Generic;
using System.Net;

namespace AniBand.Auth.Services.Abstractions.Helpers
{
    public interface IHttpResult
    {
        HttpStatusCode StatusCode { get; set; }
        List<string> Errors { get; set; }
        bool IsSuccessful { get; }
        void AddError(string error);
    }
}