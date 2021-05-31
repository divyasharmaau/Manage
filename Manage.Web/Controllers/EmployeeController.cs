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

    }
}
