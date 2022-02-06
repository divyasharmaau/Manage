using AutoMapper;
using Manage.Infrastructure.Data;
using Manage.WebApi.Dto;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;

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


        [HttpPost("ApplyLeave")]
        public async Task<IActionResult> ApplyLeave([FromForm] ApplyLeaveViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            string fileName = "";
            var user = await _employeePageService.GetEmployeeById(model.UserId);
            var leaveApplied = _mapper.Map<LeaveViewModel>(model);
            if (model.File != null)
            {
                var folderName = Path.Combine("Uploads", "files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                fileName = Guid.NewGuid() + "_" + model.File.FileName;
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))

                {

                    await model.File.CopyToAsync(fileStream);
                }
                leaveApplied.FilePath = fileName;
            }



            var newLeave = await _leavePageService.AddNewLeave(leaveApplied);

            //save the newAppliedLeave to the EmployeeLeave
            EmployeeLeaveViewModel employeeLeaveViewModel = new EmployeeLeaveViewModel();
            employeeLeaveViewModel.LeaveId = newLeave.Id;
            employeeLeaveViewModel.EmployeeId = user.Id;
            await _employeeLeavePageService.AddNewLeaveEmployeeLeave(employeeLeaveViewModel);
            return Ok(model);
          
        }


        [HttpGet("ApplyLeaveGet/{id}")]
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

        [HttpGet("MyLeaveDetails/{id}")]
        public async Task<IActionResult> MyLeaveDetails(int id)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(id);
            return Ok(leave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMyLeave(int id, [FromForm] EditMyLeaveDto editMyLeaveDto)
        {
            var leaveToBeEdited = await _leavePageService.GetMyLeaveDetails(id);
            _mapper.Map(editMyLeaveDto, leaveToBeEdited);

            if (editMyLeaveDto.File != null && editMyLeaveDto.File.Length >0)
            {
                var folderName = Path.Combine("Uploads", "files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid() + "_" + editMyLeaveDto.File.FileName;
                var fullPath = Path.Combine(pathToSave, fileName);
                FileStream fileStream = new FileStream(fullPath,FileMode.Create);
                await editMyLeaveDto.File.CopyToAsync(fileStream);
                leaveToBeEdited.FilePath = fileName;

            if (leaveToBeEdited == null)
            {
                return NotFound();
            }

            }

            await _leavePageService.Update(leaveToBeEdited);
            return NoContent();
        }

      

        [HttpGet("GetAllMyLeaves/{id}")]
        public async Task<ActionResult> GetAllMyLeaves(string id, DateTime? fromDate = null )
        {
            bool flag = false;
            var empLeaveList = await _employeeLeavePageService.GetEmployeeWithLeaveList(id);
            List<AppUserViewModel> leaveListWithDate = new List<AppUserViewModel>();

            List<AppUserViewModel> leaveList = new List<AppUserViewModel>();
            if(fromDate == null)
            {
                foreach (var item in empLeaveList.EmployeeLeaves)
                {
                    AppUserViewModel model = new AppUserViewModel();
                    model.FullName = item.Employee.FullName;
                    model.FromDate = item.Leave.FromDate;
                    model.TillDate = item.Leave.TillDate;
                    model.LeaveType = item.Leave.LeaveType;
                    model.BalanceAnnualLeave = item.Leave.BalanceAnnualLeave;
                    model.BalanceSickLeave = item.Leave.BalanceSickLeave;
                    if (item.Leave.Duration == "First Half Day" || item.Leave.Duration == "Second Half Day")
                    {
                        model.NumberOfLeaveDays = (item.Leave.TillDate.Day - item.Leave.FromDate.Day) + 0.5;
                    }
                    else
                    {
                        model.NumberOfLeaveDays = (item.Leave.TillDate.Day - item.Leave.FromDate.Day) + 1;
                    }


                    if (item.Leave.LeaveStatus != "Approved" && item.Leave.LeaveStatus != "Declined")
                    {
                        model.LeaveStatus = "Pending";
                    }
                    else
                    {
                        model.LeaveStatus = item.Leave.LeaveStatus;
                    }

                    model.Reason = item.Leave.Reason;
                    model.LeaveId = item.LeaveId;

                    leaveList.Add(model);
                }

            }
            else if (fromDate != null)
            {
                foreach (var item in empLeaveList.EmployeeLeaves)
                {
                    AppUserViewModel model = new AppUserViewModel();
                    model.FullName = item.Employee.FullName;
                    model.FromDate = item.Leave.FromDate;
                    model.TillDate = item.Leave.TillDate;
                    model.LeaveType = item.Leave.LeaveType;
                    model.BalanceAnnualLeave = item.Leave.BalanceAnnualLeave;
                    model.BalanceSickLeave = item.Leave.BalanceSickLeave;
                    if (item.Leave.Duration == "First Half Day" || item.Leave.Duration == "Second Half Day")
                    {
                        model.NumberOfLeaveDays = (item.Leave.TillDate.Day - item.Leave.FromDate.Day) + 0.5;
                    }
                    else
                    {
                        model.NumberOfLeaveDays = (item.Leave.TillDate.Day - item.Leave.FromDate.Day) + 1;
                    }


                    if (item.Leave.LeaveStatus != "Approved" && item.Leave.LeaveStatus != "Declined")
                    {
                        model.LeaveStatus = "Pending";
                    }
                    else
                    {
                        model.LeaveStatus = item.Leave.LeaveStatus;
                    }

                    model.Reason = item.Leave.Reason;
                    model.LeaveId = item.LeaveId;

                    leaveList.Add(model);
                }
                leaveListWithDate = leaveList.Where(x => x.FromDate >= fromDate).ToList();
                flag = true;
            }

            if(flag == true)
            {
                return Ok(leaveListWithDate);
            }
            return Ok(leaveList);
        }

     
        [HttpGet]
        public async Task<ActionResult> GetAllLeaves(DateTime? fromdate = null)
        {
            List<AppUserViewModel> leaves = new List<AppUserViewModel>();
            bool flag = false; 
            var allLeaves = await _employeeLeavePageService.GetAllEmployeesWithLeaveList();
           
            if (fromdate == null)
            {
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
                    if (item.LeaveStatus == null)
                    {
                        item.LeaveStatus = "Pending";
                    }
                }

            }
            else if (fromdate != null)
            {
                foreach (var item in allLeaves)
                {
                    if(item.FromDate >= fromdate)
                    {
                        leaves.Add(item);
                        
                    }
                }
                flag = true;
            }
           
            if(flag)
            {
                return Ok(leaves);
            }
            return Ok(allLeaves);

        }


        //DELETE ap/controller/{id}
        [HttpDelete("{leaveId}")]
        public async Task<ActionResult> Delete(int leaveId)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(leaveId);
            var empLeave = await _employeeLeavePageService.GetLeaveById(leaveId);
            if (leave == null)
            {
                return NotFound();
            }
            {
                await _leavePageService.Delete(leave);
            }
       
            return NoContent();
        }

       
        [HttpPut("LeaveStatusByAdmin")]
        public async Task<IActionResult> LeaveDetails([FromBody]EmployeeLeaveViewModel model)
        {
            var leave = await _leavePageService.GetMyLeaveDetails(model.LeaveId);
            var empLeave = await _employeeLeavePageService.GetLeaveById(model.LeaveId);
            var employee = await _employeePageService.GetEmployeeById(empLeave.EmployeeId);
            if (model.Approved != null)
            {
                if (employee.Status == "Full-Time" || employee.Status == "Fixed-Term")
                {
                    if (leave.LeaveType == "Annual Leave")
                    {
                        if (leave.Duration == "First Half Day" || leave.Duration == "Second Half Day")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 0.5) * 7.6;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }
                        else if (leave.Duration == "Full Day" || leave.Duration == "Others")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 1) * 7.6;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }

                    }
                    else if (leave.LeaveType == "Sick Leave")
                    {
                        if (leave.Duration == "First Half Day" || leave.Duration == "Second Half Day")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 0.5) * 7.6;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }
                        else if (leave.Duration == "Full Day" || leave.Duration == "Others")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 1) * 7.6;
                            leave.BalanceSickLeave -= numberOfHoursOfAppliedLeave;
                        }
                    }
                }
                else if (employee.Status == "Part-Time" || employee.Status == "Contract")
                {
                    if (leave.LeaveType == "Annual Leave")
                    {
                        if (leave.Duration == "First Half Day" || leave.Duration == "Second Half Day")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 0.5) * employee.NumberOfHoursWorkedPerDay;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }
                        else if (leave.Duration == "Full Day" || leave.Duration == "Others")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 1) * employee.NumberOfHoursWorkedPerDay;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }

                    }
                    else if (leave.LeaveType == "Sick Leave")
                    {
                        if (leave.Duration == "First Half Day" || leave.Duration == "Second Half Day")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 0.5) * employee.NumberOfHoursWorkedPerDay;
                            leave.BalanceAnnualLeave -= numberOfHoursOfAppliedLeave;
                        }
                        else if (leave.Duration == "Full Day" || leave.Duration == "Others")
                        {
                            var numberOfHoursOfAppliedLeave = ((leave.TillDate - leave.FromDate).Days + 1) * employee.NumberOfHoursWorkedPerDay;
                            leave.BalanceSickLeave -= numberOfHoursOfAppliedLeave;
                        }
                    }
                }

                leave.LeaveStatus = "Approved";
            }
            else if (model.Declined != null)
            {
                leave.LeaveStatus = "Declined";
            }

            if (model.Comment!= null)
            {
                leave.Comment = model.Comment;
            }
            await _leavePageService.Update(leave);
          
            return NoContent() ;
        }

    }
}
