using AutoMapper;
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
        private readonly IMapper _mapper;


        public EmployeeRepository(ManageContext manageContext , UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _manageContext = manageContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllEmployeeList()
        {
            var employeeList = await _manageContext.Users
                                    .Include(x => x.Department)
                                    .Include(y => y.EmployeePersonalDetails)
                                    .AsNoTracking()
                                    .ToListAsync();

          
            return employeeList;
        }

        public async Task<IdentityResult> Create(ApplicationUser user, string password)
        {
                var emp = await _userManager.CreateAsync(user, password);
         
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                await _userManager.AddToRoleAsync(user, "Registered User");
                
                _manageContext.SaveChanges();
                return emp;
         
        }

        public async  Task<ApplicationUser> GetEmployeeById(string id)
        {
          
            var employee = await _manageContext.Users
                                    .Include(x => x.Department)
                                    .Include(y => y.EmployeePersonalDetails)      
                                    .SingleOrDefaultAsync(x => x.Id == id);
           // var item =  _manageContext.EmployeePersonalDetails.Find(id);
           // employee.EmployeePersonalDetails = item;
            return employee;
        }

       
        public async Task Update(ApplicationUser user)
        {
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            _manageContext.Users.Update(user);
            await _manageContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindEmail(string email)
        {
           var resultEmail =  await _userManager.FindByEmailAsync(email);
            return resultEmail;
           
        }
    }
}
