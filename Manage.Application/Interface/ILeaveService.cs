using Manage.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Interface
{
   public interface ILeaveService
    {
        Task<LeaveModel> AddNewLeave(LeaveModel leave);
        Task<LeaveModel> GetMyLeaveDetails(int leaveId);
        Task Update(LeaveModel leaveModel);
        Task Delete(LeaveModel leaveModel);
    }
}
