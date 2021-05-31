using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
   public interface IEmployeePageService
    {
        Task<IdentityResult> CreateEmployee(ApplicationUserViewModel user, string password);
    }
}
