using Manage.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
    public interface IEmployeeLeaveService
    {
        Task AddNewLeaveEmployeeLeave(EmployeeLeaveModel employeeLeave);
        Task<EmployeeLeaveModel> GetLeaveById(int leaveId);
        Task Update(EmployeeLeaveModel employeeLeaveModel);
        Task<double> TotalAnnualLeaveTaken(string id);
        Task<double> TotalAnnualLeaveAccured(string id);
        Task<double> TotalSickLeaveTaken(string id);
        Task<double> TotalSickLeaveAccured(string id);
        Task<ApplicationUserModel> GetEmployeeWithLeaveList(string id);
        Task Delete(EmployeeLeaveModel employeeLeaveModel);
        Task<IEnumerable<AppUserModel>> GetAllEmployeesWithLeaveList();

    }   
}
