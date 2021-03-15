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

        public async Task<ResultResponseModel<object>> CreateNewUser(UserRequestModel request)
        {
            string errMsg = string.Empty;
            var result = Post<ResultResponseModel<object>, UserRequestModel>(request, Functions[UserConstant.InsertUser]);
            return result;
        }
    }
}
