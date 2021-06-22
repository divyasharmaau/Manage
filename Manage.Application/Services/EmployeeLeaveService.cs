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
        private readonly ManageContext _manageContext;
        private readonly IMapper _mapper;

        public EmployeeLeaveService(IEmployeeLeaveRepository employeeLeaveRepository, ManageContext manageContext , IMapper mapper)
        {
            _employeeLeaveRepository = employeeLeaveRepository;
            _manageContext = manageContext;
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
            var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveModel);
            await _employeeLeaveRepository.UpdateAsync(employeeLeave);
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
            var mapped = _mapper.Map<ApplicationUserModel>(emp);
            return mapped;
        }

        public async Task Delete(EmployeeLeaveModel employeeLeaveModel)
        {
           
            var leaveFromDb = await _manageContext.EmployeeLeaves.SingleOrDefaultAsync(x => x.LeaveId == employeeLeaveModel.LeaveId);
            var entity = _mapper.Map(employeeLeaveModel, leaveFromDb);
            await _employeeLeaveRepository.DeleteAsync(entity);
        }

       

    }
}
