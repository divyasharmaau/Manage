using AutoMapper;
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
    }
}
