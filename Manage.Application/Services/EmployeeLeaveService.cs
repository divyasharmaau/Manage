﻿using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
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

        public EmployeeLeaveService(IEmployeeLeaveRepository employeeLeaveRepository , IMapper mapper)
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
            var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveModel);
            await _employeeLeaveRepository.UpdateAsync(employeeLeave);
        }

    }
}
