using Base.Common;
using Base.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Base.Constants;
using Services.Common;

namespace Services
{
    public class AuthenticateServices : ClientServiceRequestBase, IAuthenticateServices
    {
        private const string ApiGroupName = "Authentication_Api";
        public AuthenticateServices() :
            base(ApiGroupName)
        {
        }

        public LoginResultModel Login(LoginRequestModel model)
        {
            string errMsg = string.Empty;
            LoginResultModel ret = new LoginResultModel();
            if (Utils.CheckNull(model.username, out errMsg) && Utils.CheckNull(model.password, out errMsg))
            {
                var result = Post<LoginResultModel, LoginRequestModel>(model, Functions[AuthenticationApiCode.Login]);
                return result;
            }

            return ret;
        }
    }
}
