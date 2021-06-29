﻿using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
   public interface ILeavePageService
    {
        Task<LeaveViewModel> AddNewLeave(LeaveViewModel leaveViewModel);
        Task<LeaveViewModel> GetMyLeaveDetails(int leaveId);
        Task Update(LeaveViewModel leaveViewModel);
        Task Delete(LeaveViewModel leaveViewModel);
       
    }
}
