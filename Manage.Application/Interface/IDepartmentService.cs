
using Manage.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
   public interface IDepartmentService
    {
        Task<DepartmentModel> GetEmployeeDepartment(int id);
        Task<IEnumerable<DepartmentModel>> GetDepartmentList();
        Task<DepartmentModel> GetDepartment(int departmentId);


    }
}
