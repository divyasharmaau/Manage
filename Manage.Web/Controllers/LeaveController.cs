using AutoMapper;
using Manage.Core.Entities;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeavePageService _leavePageService;
        private readonly IEmployeePageService _employeePageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IEmployeeLeavePageService _employeeLeavePageService;

        public LeaveController(ILeavePageService leavePageService , IMapper mapper ,IEmployeePageService employeePageService 
            ,IEmployeeLeavePageService employeeLeavePageService
            ,IWebHostEnvironment webHostEnvironment
            )
        {
            _leavePageService = leavePageService;
            _employeePageService = employeePageService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _employeeLeavePageService = employeeLeavePageService;


        }

        [HttpGet]
        public async Task<IActionResult> ApplyLeave(string id)
        {
            var user = await _employeePageService.GetEmployeeById(id);
            ApplyLeaveViewModel model = new ApplyLeaveViewModel();
            model.JoiningDate = user.JoiningDate;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(ApplyLeaveViewModel model , string id)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.File != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "dist/files");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.File.CopyTo(fs);
                } 
                var leaveApplied = _mapper.Map<LeaveViewModel>(model);
                leaveApplied.FilePath = uniqueFileName;
                var newLeave =   await _leavePageService.AddNewLeave(leaveApplied);

                var user = await _employeePageService.GetEmployeeById(id);
                //save the newAppliedLeave to the EmployeeLeave
                EmployeeLeaveViewModel employeeLeaveViewModel = new EmployeeLeaveViewModel();
                employeeLeaveViewModel.LeaveId = newLeave.Id;
                employeeLeaveViewModel.EmployeeId = user.Id;
                await _employeeLeavePageService.AddNewLeaveEmployeeLeave(employeeLeaveViewModel);
            }
            return View(model);
        }    
        
        public async Task<IActionResult> MyLeaveDetails(int id)
        {
            var leaveDetails = await _employeeLeavePageService.GetLeaveById(id);

            //return the fileName
            if (leaveDetails.Leave.FilePath != null)
            {
                var file = leaveDetails.Leave.FilePath;
                string docPath = file.Substring(file.IndexOf("_") + 1);
                leaveDetails.Leave.FilePath = docPath;
            }

            return View(leaveDetails);

        }
    }
}
