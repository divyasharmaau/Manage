using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
   public interface IEmployeePersonalDetailsPageService
   {
        Task<EmployeePersonalDetailsViewModel> AddAsync(EmployeePersonalDetailsViewModel model);
        Task<EmployeePersonalDetailsViewModel> GetEmployeePersonalDetailsById(string id);
   }
}
