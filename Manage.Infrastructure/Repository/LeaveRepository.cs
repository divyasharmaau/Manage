using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private readonly ManageContext _manageContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveRepository(ManageContext manageContext , UserManager<ApplicationUser> userManager) : base(manageContext)
        {
            _manageContext = manageContext;
            _userManager = userManager;
            
        }
        public async Task<Leave> AddNewLeave(Leave leave)
        {
            var newLeave = await AddAsync(leave);
            return newLeave;
        }

        public async Task<Leave> GetMyLeaveDetails(int leaveId)
        {
            var leaveDetails = await GetByIdAsync(leaveId);
            return leaveDetails;
        }
    }
}
