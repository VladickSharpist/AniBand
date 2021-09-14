using System.Collections.Generic;

namespace AniBand.Query.Services.Abstractions.Models
{
    public class QueryDto
    {
        public IEnumerable<string> Props { get; set; }
        
        public IEnumerable<string> Consts { get; set; }

        public string OrderProp { get; set; }

        public IEnumerable<string> Includes { get; set; }

        public int PageNumber { get; set; } = default;

        public int PageSize { get; set; } = default;
    }
}