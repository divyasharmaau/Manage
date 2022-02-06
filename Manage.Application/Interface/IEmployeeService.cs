using Manage.Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
  public  interface IEmployeeService
    {
        Task<IEnumerable<ApplicationUserModel>> GetListOfAllEmployees();
        Task<IdentityResult> Create(ApplicationUserModel user, string password);
        Task<ApplicationUserModel> GetEmployeeById(string id);
        //Task<IdentityResult> Update(ApplicationUserModel user);
        Task Update(ApplicationUserModel user);
        Task<ApplicationUserModel> FindEmail(string email);
    }
}
