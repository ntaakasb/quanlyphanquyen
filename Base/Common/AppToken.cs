using System;

namespace Base.Common
{
    public class AppToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}