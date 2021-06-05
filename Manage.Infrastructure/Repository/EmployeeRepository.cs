using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
   public class EmployeeRepository : IEmployeeRepository
   {
        private readonly ManageContext _manageContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeRepository(ManageContext manageContext , UserManager<ApplicationUser> userManager)
        {
            _manageContext = manageContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllEmployeeList()
        {
            var employeeList = await _manageContext.Users
                                    .Include(x => x.Department)
                                    .Include(y => y.EmployeePersonalDetails)
                                    .ToListAsync();
          
            return employeeList;
        }

        public async Task<IdentityResult> Create(ApplicationUser user)
        {
            var emp = await _userManager.CreateAsync(user);
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            _manageContext.SaveChanges();
            return emp;
        }

        public async  Task<ApplicationUser> GetEmployeeById(string id)
        {
            //var employee = await _userManager.FindByIdAsync(id.ToString());
            var employee = await _manageContext.Users
                                    .Include(x => x.Department)
                                    .Include(y => y.EmployeePersonalDetails)
                                    .SingleOrDefaultAsync(x => x.Id == id);
            return employee;
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var emp = await _userManager.UpdateAsync(user);
            return emp;

        }
    }
}
