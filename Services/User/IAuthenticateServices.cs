using Base.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IAuthenticateServices
    {
        LoginResultModel Login(LoginRequestModel model);
    }
}
