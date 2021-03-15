using System.Net;

namespace Base.Common
{
    public class Result
    {
        public ResponseInfo Metadata { get; set; }

        public bool Success => Metadata.Code == HttpStatusCode.OK;
        public bool Error => Metadata.Code != HttpStatusCode.OK;

        public Result()
        {
            if (this.Metadata == null)
            {
                this.Metadata = new ResponseInfo()
                {
                    Code = HttpStatusCode.BadRequest
                };
            }
        }
    }
}