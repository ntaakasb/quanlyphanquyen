using System.Collections.Generic;
using System.Linq;

namespace Services.Common
{
    public abstract class ClientServiceRequestBase : ClientServiceRequest
    {
        protected Dictionary<string, string> Functions { get; set; }
        public override string RestPath { get; set; }
        public override string BaseUri { get; set; }
        public override int Timeout { get; set; }

        public ClientServiceRequestBase(string apiGroupName)
        {
            var api = ApiSetting.Apis.FirstOrDefault(o => o.Key == apiGroupName);
            if (api != null && api.Functions.Any())
            {
                BaseUri = api.Address;
                Functions = api.Functions.ToDictionary(o => o.Code, o => o.Patch);
            }

            Init();
        }

        public virtual string TokenString => string.Empty;
    }
}