using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Core.Repository.Base;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
  

    public class EmployeePersonalDetailsRepository : Repository<EmployeePersonalDetails>, IEmployeePersonalDetailsRepository
    {
        private readonly ManageContext _manageContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeePersonalDetailsRepository(ManageContext manageContext , UserManager<ApplicationUser> userManager) : base(manageContext)
        {
            _manageContext = manageContext;
            _userManager = userManager;
          
        }
        public async Task<EmployeePersonalDetails> GetEmployeeById(string id)
        {
            var employee = await _manageContext.EmployeePersonalDetails
                .Include(a => a.ApplicationUser)
                .ThenInclude(d => d.Department)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return employee;
        }

     
    }
}
