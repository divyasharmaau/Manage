using Manage.Core.Entities;
using Manage.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> GetEmployeeDepartment(int id);
        Task<IEnumerable<Department>> GetDepartmentList();
        Task<Department> GetDepartment(int departmentId);
    }
}
