using AutoMapper;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeavePageService _leavePageService;
        private readonly IEmployeePageService _employeePageService;
        private readonly IMapper _mapper;

        public LeaveController(ILeavePageService leavePageService , IMapper mapper ,IEmployeePageService employeePageService  
            )
        {
            _leavePageService = leavePageService;
            _employeePageService = employeePageService;
            _mapper = mapper;
            
        }

        [HttpGet]
        public async Task<IActionResult> ApplyLeave(string id)
        {
            var user = await _employeePageService.GetEmployeeById(id);
            ApplyLeaveViewModel model = new ApplyLeaveViewModel();
            model.JoiningDate = user.JoiningDate;
            return View(model);
        }


        
    }
}
