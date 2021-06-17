using Manage.Core.Entities;
using Manage.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository
{
   public interface IEmployeeLeaveRepository : IRepository<EmployeeLeave>
    {
        Task AddNewLeaveEmployeeLeave(EmployeeLeave employeeLeave);
        Task<EmployeeLeave> GetLeaveById(int leaveId);
    }
}
