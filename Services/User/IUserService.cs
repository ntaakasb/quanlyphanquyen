using Base.Model;
using Base.Model.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<ResultResponseModel<object>> CreateNewUser(UserRequestModel request);

    }
}
