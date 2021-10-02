using System.Collections.Generic;

namespace AniBand.Query.Services.Abstractions.Models
{
    public class QueryDto
    {
        public IDictionary<string, string> Filter { get; set; }

        public string OrderProp { get; set; }

        public int PageNumber { get; set; } = default;

        public int PageSize { get; set; } = default;
    }
}