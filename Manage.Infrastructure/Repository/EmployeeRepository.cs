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
            var employeeList = await _manageContext.Users.ToListAsync();
            return employeeList;
        }

        public async Task<IdentityResult> Create(ApplicationUser user , string password)
        {
            var emp = await _userManager.CreateAsync(user, password);
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            _manageContext.SaveChanges();
            return emp;
        }

        public async  Task<ApplicationUser> GetEmployeeById(string id)
        {
            var employee = await _userManager.FindByIdAsync(id.ToString());
            return employee;
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var emp = await _userManager.UpdateAsync(user);
            return emp;

        }
    }
}
