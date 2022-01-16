using Manage.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Data
{
  public class IdentitySeeder
  {
        public static async Task SeedAsync(ManageContext manageContext,
            RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (!manageContext.Users.Any())
            {
              await CreateUsers(manageContext, roleManager, userManager);
                    //.GetAwaiter()
                    //.GetResult();
            }
        }

        private static async Task CreateUsers(ManageContext manageContext , 
            RoleManager<ApplicationRole> roleManager ,UserManager<ApplicationUser> userManager)
        {

            if (manageContext.Departments.Any())
            {
                return;
            }
            
            var departments = new List<Department>()
            {
                 new Department(){ Name ="HR"},
                 new Department(){ Name ="IT"},
               
            };

            manageContext.Departments.AddRange(departments);



            string role_Administrator = "Administrator";
            string role_RegisteredUser = "Registered User";

            if(!await roleManager.RoleExistsAsync(role_Administrator))
            {
                await roleManager.CreateAsync(new ApplicationRole(role_Administrator));
            }

            if(!await roleManager.RoleExistsAsync(role_RegisteredUser))
            {
                await roleManager.CreateAsync(new ApplicationRole(role_RegisteredUser));
            }


            var user_Admin = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "Breta",
                LastName = "Collins",
                Email = "demoasadmn@gmail.com",
                UserName = "breta.collins",
                Department = manageContext.Departments.SingleOrDefault(x => x.Name == "HR"),
            };            

            user_Admin.EmailConfirmed = true;
            user_Admin.LockoutEnabled = false;

            await userManager.CreateAsync(user_Admin, "Password1!");
            await userManager.AddToRoleAsync(user_Admin, role_Administrator);

            await manageContext.SaveChangesAsync();

            //var user_Admin = new ApplicationUser()
            //{
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    FirstName = "Breta",
            //    LastName = "Collins",
            //    Email = "bretacollins@mail.com",
            //    UserName = "breta.collins",
            //    DepartmentId = 1,
            //};

            //var user_Admin = new ApplicationUser()
            //{
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    FirstName = "Breta",
            //    LastName = "Collins",
            //    Email = "bretacollins@mail.com",
            //    UserName = "breta.collins",
            //    Department = new Department { Name = "HR" },
            //};
        }
    }
}
