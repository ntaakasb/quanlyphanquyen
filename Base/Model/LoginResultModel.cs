using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Model
{
    public class LoginResultModel
    {
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
        public string jwttoken { get; set; }
        public List<ApplicationModel> application { get; set; }
    }

    public class ApplicationModel
    {
        public string appCode { get; set; }
        public string appName { get; set; }
    }
}
