﻿using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Services
{
    public class EmployeePageService : IEmployeePageService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeePageService(IEmployeeService employeeService , IMapper mapper )
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateEmployee(ApplicationUserViewModel user , string password)
        {
            var empMapped = _mapper.Map<ApplicationUserModel>(user);
            var newEmployee = await _employeeService.Create(empMapped, password);
            return newEmployee;
        }
    }
}
