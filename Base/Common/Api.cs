using System.Collections.Generic;

namespace Base.Common
{
    public class Api
    {
        public string Key { get; set; }
        public string EndpointCode { get; set; }
        public string Address { get; set; }
        public List<ApiFunction> Functions { get; set; }
    }
}