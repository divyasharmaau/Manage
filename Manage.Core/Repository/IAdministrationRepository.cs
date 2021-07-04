using Manage.Core.Entities;
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
    }
}
