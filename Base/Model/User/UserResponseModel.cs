using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Model.User
{
    public class UserResponseModel
    {
        public int  totalRecord { get; set; }
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
    }
}
