using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Model.User
{
    public class UserRequestModel
    {
        public string userName { get; set; }
        public string fullName { get; set; }
        public string branchCode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int status { get; set; }
        public string actionType { get; set; }
        public string userType { get; set; }
        public List<RoleRequestItem> roleList { get; set; }
    }

    public class RoleRequestItem
    {
        public string roleCode { get; set; }
    }
}
