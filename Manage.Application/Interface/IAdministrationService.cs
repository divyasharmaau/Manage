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
    }
}
