using Manage.Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
    public interface IAdministrationService
    {
        Task<IdentityResult> CreateRole(ApplicationRoleModel role);
        Task<IEnumerable<ApplicationRoleModel>> GetRolesList();
        Task<ApplicationRoleModel> GetRoleById(string id);
        Task<IEnumerable<ApplicationUserModel>> GetUsersInRole(string name);
        Task<IdentityResult> Update(ApplicationRoleModel role);
        Task<IdentityResult> DeleteRole(ApplicationRoleModel role);
        Task<bool> UserInRole(ApplicationUserModel user, string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUserModel user, string roleName);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUserModel user, string roleName);
        Task<IEnumerable<ApplicationUserModel>> GetUsers();
        Task<ApplicationUserModel> GetUserById(string id);
        Task<IEnumerable<string>> GetUserRoles(string id);


    }
}
