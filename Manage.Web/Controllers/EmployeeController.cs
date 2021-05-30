using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeePageService _employeePageService;

        public EmployeeController(IEmployeePageService employeePageService)
        {
            _employeePageService = employeePageService;
        }
        public IActionResult Index()
        {
            return View();
        }

       [HttpGet]
        public async Task<IActionResult>CreateEmployee()
        {
            return View();
        }

    }
}
