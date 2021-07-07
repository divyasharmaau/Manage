﻿using Manage.Core.Entities;
using Manage.Core.Repository.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository
{
    public interface IAdministrationRepository
    {
        Task<IdentityResult> CreateRole(ApplicationRole role);
        Task<IEnumerable<ApplicationRole>> GetRolesList();
        Task<ApplicationRole> GetRoleById(string id);
        Task<IEnumerable<ApplicationUser>> GetUsersInRole(string name);
        Task<IdentityResult> Update(ApplicationRole role);
        Task<bool> UserInRole(ApplicationUser user, string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName);
    }
}
