using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Interface
{
    public interface IEmployeeLeavePageService
    {
        Task AddNewLeaveEmployeeLeave(EmployeeLeaveViewModel employeeLeaveViewModel);
        Task<EmployeeLeaveViewModel> GetLeaveById(int id);
        Task Update(EmployeeLeaveViewModel employeeLeaveViewModel);

        Task<double> TotalAnnualLeaveTaken(string id);
        Task<double> TotalAnnualLeaveAccured(string id);
        Task<double> TotalSickLeaveTaken(string id);
        Task<double> TotalSickLeaveAccured(string id);
        Task<ApplicationUserViewModel> GetEmployeeWithLeaveList(string id);
        Task Delete(EmployeeLeaveViewModel employeeLeaveViewModel);
        Task<IEnumerable<AppUserViewModel>> GetAllEmployeesWithLeaveList();

    }
}
