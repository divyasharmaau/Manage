using Manage.Core.Entities;
using Manage.Core.Repository.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository
{
   public interface IEmployeeRepository 
    {
        Task<IEnumerable<ApplicationUser>> GetAllEmployeeList();
        Task<IdentityResult> Create(ApplicationUser user);
        Task<ApplicationUser> GetEmployeeById(string id);
        //Task<IdentityResult> Update(ApplicationUser user);
        Task Update(ApplicationUser user);
    }
}
