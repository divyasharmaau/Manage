﻿using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
    public interface IAdministrationPageService
    {
        Task<IdentityResult> CreateRoleAsync(ApplicationRoleViewModel model);
        Task<IEnumerable<ApplicationRoleViewModel>> GetRolesList();
        Task<ApplicationRoleViewModel> GetRoleById(string id);
        Task<IEnumerable<ApplicationUserViewModel>> GetUsersInRole(string id);
        Task<IdentityResult> Update(ApplicationRoleViewModel role);
        Task<bool> UserInRole(ApplicationUserViewModel user, string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUserViewModel user, string roleName);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUserViewModel user, string roleName);
    }
}
