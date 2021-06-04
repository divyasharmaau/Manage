using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeePageService _employeePageService;
        private readonly IDepartmentPageService _departmentPageService;

        public EmployeeController(IEmployeePageService employeePageService , IDepartmentPageService departmentPageService)
        {
            _employeePageService = employeePageService;
            _departmentPageService = departmentPageService;
        }
        public IActionResult Index()
        {
            return View();
        }

       [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var dList = await _departmentPageService.GetDepartmentList();

            var deptList = dList.Select(dept => new SelectListItem()
            {
                Text = dept.Name,
                Value = dept.Id.ToString()
            }).ToList();

            deptList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            CreateEmployeeViewModel model = new CreateEmployeeViewModel();
            model.departmentList = deptList;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {
                ApplicationUserViewModel user = new ApplicationUserViewModel();
                user.Id = Guid.NewGuid().ToString();

                if (user.Status == "Full-Time")
                {
                    user.DaysWorkedInWeek = 5;
                    user.NumberOfHoursOfPerDay = 7.6;
                }
                else
                {
                    user.DaysWorkedInWeek = model.DaysWorkedInWeek;
                    user.NumberOfHoursOfPerDay = model.NumberOfHoursWorkedPerDay;
                }
                
                user.Title = model.Title;
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.ConfirmPassword = model.ConfirmPassword;
                user.JoiningDate = model.JoiningDate;
                user.JobTitle = model.JobTitle;
                user.Status = model.Status;
                user.DepartmentId = model.DepartmentId;
                user.Manager = model.Manager;

                var result = await _employeePageService.CreateEmployee(user);
                if (result.Succeeded)
                {
                    //return RedirectToAction("ListEmployees", "Employee");
                    return View(model);
                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }

            }


            return View(model);
        }
    }
}
