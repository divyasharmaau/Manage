using AutoMapper;
using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ManageContext _manageContext;

        public LeaveController(ILeavePageService leavePageService , IMapper mapper ,IEmployeePageService employeePageService 
            ,IEmployeeLeavePageService employeeLeavePageService
            ,IWebHostEnvironment webHostEnvironment
            ,ManageContext manageContext
            )
        {
            _leavePageService = leavePageService;
            _employeePageService = employeePageService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _employeeLeavePageService = employeeLeavePageService;
            _manageContext = manageContext;


        }

        [HttpGet]
        public async Task<IActionResult> ApplyLeave(string id)
        {
            var user = await _employeePageService.GetEmployeeById(id);
            var annualLeaveCount = await _employeeLeavePageService.TotalAnnualLeaveTaken(user.Id);
            var accuredAnnualLeave = await _employeeLeavePageService.TotalAnnualLeaveAccured(user.Id);

            var sickLeaveCount = await _employeeLeavePageService.TotalSickLeaveTaken(user.Id);
            var accuredSickLeave = await _employeeLeavePageService.TotalSickLeaveAccured(user.Id);

            ApplyLeaveViewModel model = new ApplyLeaveViewModel();
            model.JoiningDate = user.JoiningDate;
            model.BalanceAnnualLeave = accuredAnnualLeave - annualLeaveCount;
            model.BalanceSickLeave = accuredSickLeave - sickLeaveCount;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(ApplyLeaveViewModel model , string id)
        {
            var user = await _employeePageService.GetEmployeeById(id);
            var annualLeaveCount = await _employeeLeavePageService.TotalAnnualLeaveTaken(user.Id);
            var accuredAnnualLeave = await _employeeLeavePageService.TotalAnnualLeaveAccured(user.Id);
            model.BalanceAnnualLeave = accuredAnnualLeave - annualLeaveCount;

            var sickLeaveCount = await _employeeLeavePageService.TotalSickLeaveTaken(user.Id);
            var accuredSickLeave = await _employeeLeavePageService.TotalSickLeaveAccured(user.Id);
            model.BalanceSickLeave = accuredSickLeave - sickLeaveCount;
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
                leaveApplied.BalanceAnnualLeave = model.BalanceAnnualLeave;
                leaveApplied.BalanceSickLeave = model.BalanceSickLeave;
                var newLeave =   await _leavePageService.AddNewLeave(leaveApplied);

            
                //save the newAppliedLeave to the EmployeeLeave
                EmployeeLeaveViewModel employeeLeaveViewModel = new EmployeeLeaveViewModel();
                employeeLeaveViewModel.LeaveId = newLeave.Id;
                employeeLeaveViewModel.EmployeeId = user.Id;
                await _employeeLeavePageService.AddNewLeaveEmployeeLeave(employeeLeaveViewModel);
            }
            return View(model);
        }    
        
        public async Task<IActionResult> MyLeaveDetails(int leaveId)
        {
            var leaveDetails = await _employeeLeavePageService.GetLeaveById(leaveId);

            if(leaveDetails.Leave.FilePath == null)
            {
                return View(leaveDetails);
            }
            else
            {
                var file = leaveDetails.Leave.FilePath;
                string docPath = file.Substring(file.IndexOf("_") + 1);
                leaveDetails.Leave.FilePath = docPath;
            }
            return View(leaveDetails);
        }

        [HttpGet]
        public async Task<IActionResult> EditMyLeave(int leaveId)
        {
            var leaveDetails = await _employeeLeavePageService.GetLeaveById(leaveId);
            EditMyLeaveViewModel model = new EditMyLeaveViewModel();
            var mappedModel = _mapper.Map<EditMyLeaveViewModel>(leaveDetails);
            if (leaveDetails.Leave.FilePath == null)
            {
                return View(mappedModel);
            }

            var file = leaveDetails.Leave.FilePath;
            string docPath = file.Substring(file.IndexOf("_") + 1);
            leaveDetails.Leave.FilePath = docPath;
            mappedModel.ExistingFilePath = docPath;
            return View(mappedModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMyLeave(EditMyLeaveViewModel model)
        {  
            if(ModelState.IsValid)
            {
                var mapped = _mapper.Map<LeaveViewModel>(model.Leave);
                mapped.FilePath = model.ExistingFilePath;
                string uniqueFileName = "";
                if(model.File != null)
                {
                    if(model.ExistingFilePath != null)
                    {
                        string filePathExisting = Path.Combine(_webHostEnvironment.WebRootPath, "dist/files", model.ExistingFilePath);
                        System.IO.File.Delete(filePathExisting);
                    }
                  
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "dist/files");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        model.File.CopyTo(fs);  
                }

                mapped.FilePath = uniqueFileName;
                await _leavePageService.Update(mapped);

                return RedirectToAction("MyLeaveDetails", new { leaveId = mapped.Id });

            }

            return View(model);
           
            
        }
    }
}
