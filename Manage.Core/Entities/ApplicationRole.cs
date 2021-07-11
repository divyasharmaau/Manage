using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Manage.Core.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
