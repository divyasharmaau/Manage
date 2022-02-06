using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Services
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly IMapper _mapper;

        public EmployeeLeaveService(IEmployeeLeaveRepository employeeLeaveRepository, IMapper mapper)
        {
            _employeeLeaveRepository = employeeLeaveRepository;
            _mapper = mapper;
        }

        public async Task AddNewLeaveEmployeeLeave(EmployeeLeaveModel employeeLeave)
        {
            var employeeLeaveRepository = _mapper.Map<EmployeeLeave>(employeeLeave);
            await _employeeLeaveRepository.AddNewLeaveEmployeeLeave(employeeLeaveRepository);
        }

        public async Task<EmployeeLeaveModel> GetLeaveById(int leaveId)
        {
            var leave = await _employeeLeaveRepository.GetLeaveById(leaveId);
            var leaveMapped = _mapper.Map<EmployeeLeaveModel>(leave);
            return leaveMapped;
        }

        public async Task Update(EmployeeLeaveModel employeeLeaveModel)
        {
            var employeeLeaveFromDB = await _employeeLeaveRepository.GetLeaveById(employeeLeaveModel.LeaveId);
            //var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveModel);
            var employeeLeave = _mapper.Map(employeeLeaveModel,employeeLeaveFromDB);
            await _employeeLeaveRepository.UpdateAsync(employeeLeaveFromDB);

        }

        public async Task<double> TotalAnnualLeaveTaken(string id)
        {
            var annualLeaveCount = await _employeeLeaveRepository.TotalAnnualLeaveTaken(id);
            return annualLeaveCount;
        }

        public async Task<double> TotalAnnualLeaveAccured(string id)
        {
            var annualLeaveAccured = await _employeeLeaveRepository.TotalAnnualLeaveAccured(id);
            return annualLeaveAccured;
        }
        public async Task<double> TotalSickLeaveTaken(string id)
        {
            var annualLeaveCount = await _employeeLeaveRepository.TotalSickLeaveTaken(id);
            return annualLeaveCount;
        }

        public async Task<double> TotalSickLeaveAccured(string id)
        {
            var annualLeaveAccured = await _employeeLeaveRepository.TotalSickLeaveAccured(id);
            return annualLeaveAccured;
        }

        public async Task<ApplicationUserModel> GetEmployeeWithLeaveList(string id)
        {
            var emp = await _employeeLeaveRepository.GetEmployeeWithLeaveList(id);
            var mappedEmployee = _mapper.Map<ApplicationUserModel>(emp);
            return mappedEmployee;
        }

        public async Task Delete(EmployeeLeaveModel employeeLeaveModel)
        {
            var leaveFromDb = await _employeeLeaveRepository.GetLeaveById(employeeLeaveModel.LeaveId);
            var entity = _mapper.Map(employeeLeaveModel, leaveFromDb);
            await _employeeLeaveRepository.DeleteAsync(entity);
        }

        //public async Task<IEnumerable<ApplicationUserModel>> GetAllEmployeesWithLeaveList()
        // {
        //     var employeeList = await _employeeLeaveRepository.GetAllEmployeesWithLeaveList();
        //     var mappedEmployeeList = _mapper.Map<IEnumerable<ApplicationUserModel>>(employeeList);
        //     return mappedEmployeeList;
        // }
        public async Task<IEnumerable<AppUserModel>> GetAllEmployeesWithLeaveList()
        {
            var employeeList = await _employeeLeaveRepository.GetAllEmployeesWithLeaveList();
            var mappedEmployeeList = _mapper.Map<IEnumerable<ApplicationUserModel>>(employeeList);
            //return mappedEmployeeList;

            var modelList = new List<AppUserModel>();
            foreach (var item in mappedEmployeeList)
            {
                
                foreach (var emp in item.EmployeeLeaves)
                {
                    AppUserModel model = new AppUserModel();
                    model.FullName = item.FullName;
                    model.FromDate = emp.Leave.FromDate;
                    model.TillDate = emp.Leave.TillDate;
                    model.LeaveType = emp.Leave.LeaveType;
                    model.Reason = emp.Leave.Reason;
                    model.LeaveStatus = emp.Leave.LeaveStatus;
                    model.LeaveId = emp.LeaveId;
                    double numberOfLeaveDays = 0;
                    DateTime end = emp.Leave.TillDate;
                    DateTime start = emp.Leave.FromDate;
                    model.BalanceAnnualLeave = emp.Leave.BalanceAnnualLeave;
                    model.BalanceSickLeave = emp.Leave.BalanceSickLeave;


                    if (emp.Leave.Duration == "First Half Day" || emp.Leave.Duration == "Second Half Day")
                    {
                        numberOfLeaveDays = (end - start).Days + 0.5;
                    }
                    else
                    {
                        numberOfLeaveDays = (end - start).Days + 1;
                    }

                    model.NumberOfLeaveDays = numberOfLeaveDays;
                    modelList.Add(model);
                }
            }
            return modelList;
        }
    }
}
