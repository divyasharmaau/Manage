using Manage.Core.Entities;
using Manage.Core.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
   public class AdministrationRepository : IAdministrationRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdministrationRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(ApplicationRole role)
        {
            var empRole = await _roleManager.CreateAsync(role);
            return empRole;
        }

       public async Task<IEnumerable<ApplicationRole>> GetRolesList()
        {
            var roleList =  _roleManager.Roles;
            return roleList;
        }
    }
}
