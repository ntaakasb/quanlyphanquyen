using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Model
{
    public class LoginRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string appCode { get; set; }
    }
}
