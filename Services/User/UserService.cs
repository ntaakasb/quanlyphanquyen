using Base;
using Base.Common;
using Base.Constants;
using Base.Model;
using Base.Model.User;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : ClientServiceRequestBase, IUserService
    {
        private const string ApiGroupName = "User_Api";
        public UserService() :
            base(ApiGroupName)
        {
        }


        public UserResponseModel CreateNewUser(PushModel model)
        {
            string errMsg = string.Empty;
            UserResponseModel ret = new UserResponseModel();
            var result = Post<UserResponseModel, PushModel>(model, Functions[UserConstant.InsertUser], token: TokenString);
            return ret;

        }
    }
}
