using Microsoft.AspNetCore.Authorization;

namespace WebApp.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute()
            : base()
        {
        }

        public PermissionAttribute(params string[] roles)
            : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}