using AutoMapper;
using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Manage.WebApi.Dto;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeavePageService _leavePageService;
        private readonly IEmployeePageService _employeePageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IEmployeeLeavePageService _employeeLeavePageService;
        private readonly ManageContext _manageContext;
        private readonly IFileUploadHelper _fileUploadHelper;

        public LeaveController(ILeavePageService leavePageService, IMapper mapper, IEmployeePageService employeePageService
            , IEmployeeLeavePageService employeeLeavePageService
            , IWebHostEnvironment webHostEnvironment
            , ManageContext manageContext
            , IFileUploadHelper fileUploadHelper
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


        [HttpPost("{id}")]
        public async Task<IActionResult> ApplyLeave([FromForm] ApplyLeaveViewModel model, string id)
        {
            if (model == null)
            {
                return NotFound();
            }

            var user = await _employeePageService.GetEmployeeById(id);
            var leaveApplied = _mapper.Map<LeaveViewModel>(model);
            var newLeave = await _leavePageService.AddNewLeave(leaveApplied);

            //save the newAppliedLeave to the EmployeeLeave
            EmployeeLeaveViewModel employeeLeaveViewModel = new EmployeeLeaveViewModel();
            employeeLeaveViewModel.LeaveId = newLeave.Id;
            employeeLeaveViewModel.EmployeeId = user.Id;
            await _employeeLeavePageService.AddNewLeaveEmployeeLeave(employeeLeaveViewModel);
            return CreatedAtRoute("MyLeaveDetails", new { id = newLeave.Id }, newLeave);
        }


        [HttpGet("ApplyLeaveGet/{id}")]
        //[HttpGet("{id}" , Name ="ApplyLeaveGet")]
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
            return Ok(model);
        }


        //[HttpPost("{id}")]
        //public async Task<IActionResult> ApplyLeave([FromForm] ApplyLeaveDto applyLeaveDto, string id)
        //{
        //    if (applyLeaveDto == null)
        //    {
        //        return NotFound();
        //    }

        //    var mapped = _mapper.Map<LeaveViewModel>(applyLeaveDto);
        //    var newLeave = await _leavePageService.AddNewLeave(mapped);
        //    //add to employeeLeave
        //    var mappedEmployeeLeave = _mapper.Map<EmployeeLeaveViewModel>(newLeave);
        //    mappedEmployeeLeave.LeaveId = newLeave.Id;
        //    mappedEmployeeLeave.EmployeeId = id;

        //    await _employeeLeavePageService.AddNewLeaveEmployeeLeave(mappedEmployeeLeave);
        //    return CreatedAtRoute("MyLeaveDetails", new { id = newLeave.Id }, newLeave);
        //}



        [HttpGet("{id}" , Name = "MyLeaveDetails")]
        public async Task<IActionResult> MyLeaveDetails(int id)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(id);
            return Ok(leave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMyLeave(int id ,EditMyLeaveDto editMyLeaveDto)
        {
            editMyLeaveDto.Id = id;
            var leaveToBeEdited = await _leavePageService.GetMyLeaveDetails(id);
            if(leaveToBeEdited == null)
            {
                return NotFound();
            }
            var mapped = _mapper.Map(editMyLeaveDto, leaveToBeEdited);
            leaveToBeEdited.Id = editMyLeaveDto.Id;
            await _leavePageService.Update(mapped);
            return NoContent();
        }

        //PATCH api/controller/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialLeaveUpdate(int id, JsonPatchDocument<EditMyLeaveDto> patchDoc)
        {
            
            var leaveToBeEdited = await _leavePageService.GetMyLeaveDetails(id);
            if (leaveToBeEdited == null)
            {
                return NotFound();
            }

            var modelToPatch = _mapper.Map<EditMyLeaveDto>(leaveToBeEdited);
            patchDoc.ApplyTo(modelToPatch, ModelState);
            if (!TryValidateModel(modelToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(modelToPatch, leaveToBeEdited);
            await _leavePageService.Update(leaveToBeEdited);
            return NoContent();
        }


        [HttpGet("GetAllMyLeaves/{id}")]
        public async Task<ActionResult<EmployeeLeaveDto>> GetAllMyLeaves(string id)
        {
            var empLeaveList = await  _employeeLeavePageService.GetEmployeeWithLeaveList(id);
            var leaveListmapped = _mapper.Map<EmployeeLeaveListDto>(empLeaveList);
            return Ok(leaveListmapped);
            
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLeaves()
        {
            var allLeaves = await _employeeLeavePageService.GetAllEmployeesWithLeaveList();
            foreach (var item in allLeaves)
            {
                if (item.LeaveType == "Annual Leave")
                {
                    var netLeaveBalance = item.BalanceAnnualLeave - item.NumberOfLeaveDays * 7.6;
                    item.BalanceAnnualLeave = netLeaveBalance;
                }
                else if (item.LeaveType == "Sick Leave")
                {
                    var netSickLeavesBalance = item.BalanceSickLeave - item.NumberOfLeaveDays * 7.6;
                    item.BalanceSickLeave = netSickLeavesBalance;                             
                }
            }
                                                    
      
            return Ok(allLeaves);
        }

       

        //DELETE ap/controller/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int leaveId)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(leaveId);
            if (leave == null)
            {
                return NotFound();
            }
            await _leavePageService.Delete(leave);
            return NoContent();
        }

     
    }
}
