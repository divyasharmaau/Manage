using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Interface
{
   public interface IDepartmentPageService
    {
        Task<IEnumerable<DepartmentViewModel>> GetDepartmentList();
    }
}
