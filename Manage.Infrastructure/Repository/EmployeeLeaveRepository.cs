using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
    public class EmployeeLeaveRepository : Repository<EmployeeLeave> , IEmployeeLeaveRepository
    {
        private readonly ManageContext _manageContext;
        public EmployeeLeaveRepository(ManageContext manageContext) : base(manageContext)
        {
            _manageContext = manageContext;
        }
        public async Task AddNewLeaveEmployeeLeave(EmployeeLeave employeeLeave)
        {
            await AddAsync(employeeLeave);
            await _manageContext.SaveChangesAsync();
        }

        public async Task<EmployeeLeave> GetLeaveById(int leaveId)
        {
            var leave = await _manageContext.EmployeeLeaves
                .Include(e => e.Employee)
                .Include(l => l.Leave)
                .Where(x => x.LeaveId == leaveId)
                .FirstOrDefaultAsync();
            return leave;
        }


    } 
}
