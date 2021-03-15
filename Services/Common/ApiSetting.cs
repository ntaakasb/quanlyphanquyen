using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Base.Common;
using Newtonsoft.Json;

namespace Services.Common
{
    public class ApiSetting
    {
        public static List<Api> Apis { get; set; }
        public static List<ApiEndpoint> ApiEndpoints { get; set; }

        public static void LoadApis()
        {
            Apis = new List<Api>();
            var di = new DirectoryInfo(@"./Mapping");
            var reg = new Regex("Endpoints.*.json");
            foreach (var fi in di.GetFiles())
            {
                using (var r = fi.OpenText())
                {
                    var json = r.ReadToEnd();
                    if (fi.Name.Equals(MappingCode.EndpointsName))
                    {
                        ApiEndpoints = JsonConvert.DeserializeObject<List<ApiEndpoint>>(json);
                    }
                    else if (!reg.IsMatch(fi.Name))
                    {
                        Apis.AddRange(JsonConvert.DeserializeObject<List<Api>>(json));
                    }
                }
            }

            if (Apis.Any())
            {
                foreach (var item in Apis)
                {
                    var endpoint = ApiEndpoints.FirstOrDefault(o => o.Code == item.EndpointCode);
                    if (endpoint != null)
                    {
                        item.Address = endpoint.Address;
                    }
                }
            }
        }
    }
}