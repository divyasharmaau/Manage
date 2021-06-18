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
    }   
}
