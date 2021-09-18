using System.Collections.Generic;

namespace AniBand.Video.Web.Models
{
    public class DataRequestVm
    {
        public IEnumerable<string> Props { get; set; }
        
        public IEnumerable<string> Consts { get; set; }

        public string OrderProp { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}