using Base;
using Base.Common;
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
        UserResponseModel CreateNewUser(PushModel model);
    }
}
