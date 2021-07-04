using Manage.Web.ViewModels;
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
    }
}
