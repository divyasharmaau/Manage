using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
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
        private readonly ManageContext _manageContext;

        public AdministrationRepository(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager ,ManageContext manageContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _manageContext = manageContext;
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

        public async Task<bool>UserInRole(ApplicationUser user , string roleName)
        {
            var result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
           
        }

        public async Task<IdentityResult> Update(ApplicationRole role)
        {
            var result =  await _roleManager.UpdateAsync(role);
            return result;
        }
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user , string roleName)
        {
            var employee =  _manageContext.Users.SingleOrDefault(x => x.UserName == user.UserName);
            var result =  await _userManager.AddToRoleAsync(employee, roleName);
            return result;
        }
        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result;
        }

    }
}
