﻿using AutoMapper;
using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
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
using X.PagedList;

namespace Manage.WebApi.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeavePageService _leavePageService;
        private readonly IEmployeePageService _employeePageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IEmployeeLeavePageService _employeeLeavePageService;
        private readonly ManageContext _manageContext;
        private readonly IFileUploadHelper _fileUploadHelper;

        public LeaveController(ILeavePageService leavePageService , IMapper mapper ,IEmployeePageService employeePageService 
            ,IEmployeeLeavePageService employeeLeavePageService
            ,IWebHostEnvironment webHostEnvironment
            ,ManageContext manageContext
            ,IFileUploadHelper fileUploadHelper
            )
        {
            _leavePageService = leavePageService;
            _employeePageService = employeePageService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _employeeLeavePageService = employeeLeavePageService;
            _manageContext = manageContext;
            _fileUploadHelper = fileUploadHelper;
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
                FileUploadViewModel fileUploadViewModel = new FileUploadViewModel();
                fileUploadViewModel.File = model.File;

                if (model.File != null)
                {

                    var uploadedFile = _fileUploadHelper.UploadFile(fileUploadViewModel);
                    uniqueFileName = uploadedFile.UniqueFileName;
                    
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
     
            return RedirectToAction("GetAllMyLeaves", new { id  = user.Id });
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

                FileUploadViewModel fileUploadViewModel = new FileUploadViewModel();
                fileUploadViewModel.File = model.File;
                string uniqueFileName = "";

                if(model.File != null)
                {
                    if(model.ExistingFilePath != null)
                    {
                        string filePathExisting = Path.Combine(_webHostEnvironment.WebRootPath, "dist/files", model.ExistingFilePath);
                        System.IO.File.Delete(filePathExisting);
                    }
                    var uploadedFile = _fileUploadHelper.UploadFile(fileUploadViewModel);
                       
                }

                uniqueFileName = fileUploadViewModel.UniqueFileName;
                mapped.FilePath = uniqueFileName;
                await _leavePageService.Update(mapped);

                return RedirectToAction("MyLeaveDetails", new { leaveId = mapped.Id });

            }

            return View(model);
           
            
        }

        public async Task<IActionResult> GetAllMyLeaves(IFormCollection obj ,string id , int?page 
            , string searchFromDate 
            ,string searchToDate , string searchLeaveStatus , string searchLeaveType 
            ,string searchAll , string searchApproved , string searchPending , string searchDeclined)
        {

            var emp = await _employeeLeavePageService.GetEmployeeWithLeaveList(id);
            List<AppUserViewModel> leaveList = new List<AppUserViewModel>();
            foreach (var item in emp.EmployeeLeaves)
            {
                AppUserViewModel model = new AppUserViewModel();
                model.FullName = item.Employee.FullName;
                model.FromDate = item.Leave.FromDate;
                model.TillDate = item.Leave.TillDate;
                model.LeaveType = item.Leave.LeaveType;
                model.LeaveStatus = item.Leave.LeaveStatus;
                model.Reason = item.Leave.Reason;
                model.LeaveId = item.LeaveId;

                leaveList.Add(model);
            }


            ViewData["CurrentFilter"] = obj["SearchString"].ToString();
            ViewData["CurrentFilterFromDate"] = obj["searchFromDate"].ToString();
            ViewData["CurrentFilterToDate"] = obj["searchToDate"].ToString();
            ViewData["CurrentFilterLeaveType"] = obj["searchLeaveType"].ToString();
            //ViewData["CurrentFilerLeaveStatus"] = obj["searchLeaveStatus"].ToString();

            Boolean tempValue = obj["searchAll"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSearchAll"] = tempValue;

            Boolean tempValueApproved = obj["searchApproved"].ToString() != "" ? true : false;
            ViewData["CurrentFilterApproved"] = tempValueApproved;

            Boolean tempValueSearchPending = obj["searchPending"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSearchPending"] = tempValueSearchPending;

            Boolean tempValueSearchDeclined = obj["searchDeclined"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSearchDeclined"] = tempValueSearchDeclined;


            if (!String.IsNullOrEmpty(searchFromDate))
            {
                leaveList = leaveList.Where(d => d.FromDate >= DateTime.Parse(searchFromDate)).ToList();
            }
            if (!String.IsNullOrEmpty(searchToDate))
            {
                leaveList = leaveList.Where(d => d.TillDate <= DateTime.Parse(searchToDate)).ToList();
            }

            if (!String.IsNullOrEmpty(searchLeaveType))
            {
                leaveList = leaveList.Where(l => l.LeaveType == searchLeaveType).ToList();
            }


            if (!String.IsNullOrEmpty(searchLeaveStatus))
            {
                leaveList = leaveList.Where(s => s.LeaveStatus == searchLeaveStatus).ToList();
            }

            if (!String.IsNullOrEmpty(searchAll))
            {
                leaveList = leaveList.OrderByDescending(s => s.LeaveStatus).ToList();
            }

            if (!String.IsNullOrEmpty(searchApproved))
            {
                leaveList = leaveList.Where(s => s.LeaveStatus == "Approved").ToList();
            }

            if (!String.IsNullOrEmpty(searchPending))
            {
                leaveList = leaveList.Where(s => s.LeaveStatus == "Pending").ToList();
            }

            if (!String.IsNullOrEmpty(searchDeclined))
            {
                leaveList = leaveList.Where(s => s.LeaveStatus == "Declined").ToList();
            }
          
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var result = leaveList.OrderByDescending(s => s.LeaveStatus).ToPagedList(pageNumber, pageSize);
            return View(result);
        }

        //DELETE ap/controller/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int leaveId)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(leaveId);
            if(leave == null)
            {
                return NotFound();
            }
            await _leavePageService.Delete(leave);
            return NoContent();
        }

        public async Task<IActionResult> DeleteMyLeave(int leaveId)
        {
           var leave = await _leavePageService.GetMyLeaveDetails(leaveId);
           var empLeave = await _employeeLeavePageService.GetLeaveById(leaveId);
            if(leave == null)
            {
                ViewBag.ErrorMessage = "Leave with Id: {leaveId} not found";
            }
            else
            {
                await _leavePageService.Delete(leave);
            }

            return RedirectToAction("GetAllMyLeaves" ,new { id = empLeave.EmployeeId });
        }

        public async Task<IActionResult> GetAllLeaves(IFormCollection obj, int?page  , string currentFilter,
          string employeeName, string searchFromDate, string searchToDate,
         string searchLeaveStatus, string searchLeaveType,
          string searchAll, string searchAprroved, string searchPending, string searchDeclined)
        {
            var employeeLeaveList = await _employeeLeavePageService.GetAllEmployeesWithLeaveList();

            ViewData["CurrentFilter"] = obj["SearchString"].ToString();
            ViewData["CurrentFilterE"] = obj["employeeName"].ToString();
            ViewData["CurrentFilterFD"] = obj["searchFromDate"].ToString();
            ViewData["CurrentFilterTD"] = obj["searchToDate"].ToString();
            ViewData["CurrentFilterLT"] = obj["searchLeaveType"].ToString();
            ViewData["CurrentFilerLS"] = obj["searchLeaveStatus"].ToString();
 
            Boolean tempValue = obj["searchAll"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSA"] = tempValue;

            Boolean tempValueA = obj["searchAprroved"].ToString() != "" ? true : false;
            ViewData["CurrentFilterA"] = tempValueA;

            Boolean tempValueSP = obj["searchPending"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSP"] = tempValueSP;

            Boolean tempValueSD = obj["searchDeclined"].ToString() != "" ? true : false;
            ViewData["CurrentFilterSD"] = tempValueSD;

            if (!String.IsNullOrEmpty(employeeName))
            {
                employeeLeaveList = employeeLeaveList.Where(e => e.FullName.ToLower().Contains(employeeName.ToLower()));

            }
            else
            {
                employeeName = currentFilter;
            }
            ViewBag.CurrentFilter = employeeName;


            if (!String.IsNullOrEmpty(searchFromDate))
            {
      
                employeeLeaveList = employeeLeaveList.Where(d => d.FromDate >= DateTime.Parse(searchFromDate)).ToList();
            }

            if (!String.IsNullOrEmpty(searchToDate))
            {
                employeeLeaveList = employeeLeaveList.Where(d => d.TillDate <= DateTime.Parse(searchToDate)).ToList();
            }


            if (!String.IsNullOrEmpty(searchLeaveType))
            {
                employeeLeaveList = employeeLeaveList.Where(l => l.LeaveType == searchLeaveType);
            }

            //else
            //{
            //    searchLeaveType = currentFilter;
            //}

            if (!String.IsNullOrEmpty(searchLeaveStatus))
            {
                employeeLeaveList = employeeLeaveList.Where(s => s.LeaveStatus == searchLeaveStatus);
            }

            //ViewBag.CurrentFilter = searchLeaveType;

            if (!String.IsNullOrEmpty(searchAll))
            {
                employeeLeaveList = employeeLeaveList.OrderByDescending(s => s.LeaveStatus);
            }


            if (!String.IsNullOrEmpty(searchAprroved))
            {
                employeeLeaveList = employeeLeaveList.Where(s => s.LeaveStatus == "Approved");
            }

            if (!String.IsNullOrEmpty(searchPending))
            {
                employeeLeaveList = employeeLeaveList.Where(s => s.LeaveStatus == "Pending");
            }

            if (!String.IsNullOrEmpty(searchDeclined))
            {
                employeeLeaveList = employeeLeaveList.Where(s => s.LeaveStatus == "Declined");
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var result = employeeLeaveList.OrderByDescending(s => s.LeaveStatus).ToPagedList(pageNumber, pageSize);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> LeaveDetails(int leaveId)
        {
            var leave = await _employeeLeavePageService.GetLeaveById(leaveId);

            if (leave.Leave.FilePath != null)
            {
                var file = leave.Leave.FilePath;
                string docPath = file.Substring(file.IndexOf("_") + 1);
                leave.Leave.FilePath = docPath;
            }
            return View(leave);

        }

        [HttpPost]
        public async Task<IActionResult> LeaveDetails(EmployeeLeaveViewModel model, string approved , string declined , string comment)
        {

            var leave = await _leavePageService.GetMyLeaveDetails(model.LeaveId);
            if (approved != null)
            {
                leave.LeaveStatus = "Approved";
            }
            else if (declined != null)
            {
                leave.LeaveStatus = "Declined";
            }

            if (comment != null)
            {
                leave.Comment = comment;
            }

            await _leavePageService.Update(leave);
            return RedirectToAction("GetAllLeaves");
        }
    }
}
