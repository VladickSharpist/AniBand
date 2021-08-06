using System.Collections.Generic;
using System.Net;

namespace AniBand.Core.Abstractions.Infrastructure.Helpers
{
    public interface IHttpResult
    {
        HttpStatusCode StatusCode { get; set; }
        
        List<string> Errors { get; set; }
        
        bool IsSuccessful { get; }
        
        void AddError(string error);
    }
}