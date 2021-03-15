using System;

namespace Base.Model.Authentication
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}