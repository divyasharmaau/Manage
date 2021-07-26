using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{

   // assign role to the given user
    public class UserRolesViewModel
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
