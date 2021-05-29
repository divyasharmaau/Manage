using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Core.Repository.Base;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
   public class DepartmentRepository : Repository<Department> , IDepartmentRepository
    {
        private readonly ManageContext _manageContext;

        public DepartmentRepository(ManageContext manageContext) : base(manageContext)
        {
            _manageContext = manageContext;
        }

        public async Task<Department> GetDepartment(int departmentId)
        {
            var department = await _manageContext.Departments.FindAsync(departmentId);
            var employees =  await _manageContext.Users.Where(x => x.DepartmentId == departmentId).ToListAsync();
            department.Employees = employees;
            return department;
        }
        public async Task<IEnumerable<Department>> GetDepartmentList()
        {
           var departmentList =  await _manageContext.Departments.ToListAsync();
            return departmentList;
        }

        public async Task<Department> GetEmployeeDepartment(string userId)
        {
            var employee = await _manageContext.Users.FindAsync(userId);
            var empDepartment =  _manageContext.Departments.Where(x => x.Id == employee.DepartmentId).FirstOrDefault();
            return empDepartment;
        }
    }
}
