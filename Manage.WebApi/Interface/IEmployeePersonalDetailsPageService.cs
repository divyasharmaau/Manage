using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Interface
{
   public interface IEmployeePersonalDetailsPageService
   {
        Task<EmployeePersonalDetailsViewModel> AddAsync(EmployeePersonalDetailsViewModel model);
        Task<EmployeePersonalDetailsViewModel> GetEmployeePersonalDetailsById(string id);
        Task<EmployeePersonalDetailsViewModel> UpdateAsync(EmployeePersonalDetailsViewModel model);
   }
}
