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
        Task<double> TotalAnnualLeaveTaken(string id);
        Task<double> TotalAnnualLeaveAccured(string id);
        Task<double> TotalSickLeaveTaken(string id);
        Task<double> TotalSickLeaveAccured(string id);
        Task<ApplicationUser> GetEmployeeWithLeaveList(string id);
        Task<IEnumerable<ApplicationUser>> GetAllEmployeesWithLeaveList();
    }
}
