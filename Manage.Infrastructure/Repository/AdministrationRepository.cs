using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Manage.Infrastructure.Repository
{
   public class AdministrationRepository :  IAdministrationRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationRepository(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateRole(ApplicationRole role)
        {
            var empRole = await _roleManager.CreateAsync(role);
            return empRole;
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesList()
        {
            var roleList = _roleManager.Roles;
            return roleList;
        }

        public async Task<ApplicationRole> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return role;
        }

        public async Task<IEnumerable<ApplicationUser>>GetUsersInRole(string name)
        {
            var users = await _userManager.GetUsersInRoleAsync(name);
            return users;
        }

        public async Task<IdentityResult> Update(ApplicationRole role)
        {
            var result =  await _roleManager.UpdateAsync(role);
            return result;
        }
    }
}
