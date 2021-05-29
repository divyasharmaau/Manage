using Manage.Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
   public interface IEmployeePersonalDetailsService
    {
        Task<EmployeePersonalDetailsModel> GetEmployeeById(string employeeId);
        Task<EmployeePersonalDetailsModel> AddAsync(EmployeePersonalDetailsModel model);
        Task UpdateAsync(EmployeePersonalDetailsModel model);
    }
}
