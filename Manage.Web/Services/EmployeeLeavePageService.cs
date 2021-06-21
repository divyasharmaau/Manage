﻿using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Services
{
    public class EmployeeLeavePageService : IEmployeeLeavePageService
    {
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IMapper _mapper;

        public EmployeeLeavePageService(IEmployeeLeaveService  employeeLeaveService , IMapper mapper)
        {
            _employeeLeaveService = employeeLeaveService;
            _mapper = mapper;
        }

        public async Task AddNewLeaveEmployeeLeave(EmployeeLeaveViewModel employeeLeaveViewModel)
        {
            var employeeLeaveFromApplication = _mapper.Map<EmployeeLeaveModel>(employeeLeaveViewModel);
            await _employeeLeaveService.AddNewLeaveEmployeeLeave(employeeLeaveFromApplication);
         
        }

        public async Task<EmployeeLeaveViewModel> GetLeaveById(int leaveId)
        {
            var  leaveDetailsFromModel = await _employeeLeaveService.GetLeaveById(leaveId);
            var mappedLeaveDetails = _mapper.Map<EmployeeLeaveViewModel>(leaveDetailsFromModel);
            return mappedLeaveDetails;
        }

        public async Task Update(EmployeeLeaveViewModel employeeLeaveViewModel)
        {
            var employeeLeaveFromModel = _mapper.Map<EmployeeLeaveModel>(employeeLeaveViewModel);
            await _employeeLeaveService.Update(employeeLeaveFromModel);
        }

        public Task<double> TotalAnnualLeaveAccured(string id)
        {
            var totalAnnualLeaveAccured = _employeeLeaveService.TotalAnnualLeaveAccured(id);
            return totalAnnualLeaveAccured;
        }

        public async Task<double> TotalAnnualLeaveTaken(string id)
        {
            var totalAnnualLeaveTaken = await _employeeLeaveService.TotalAnnualLeaveTaken(id);
            return totalAnnualLeaveTaken;
        }

    }
}
