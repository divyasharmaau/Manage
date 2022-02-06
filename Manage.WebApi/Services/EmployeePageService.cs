using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Services
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

        public async Task<IdentityResult> CreateEmployee(ApplicationUserViewModel user, string password)
        {
            var empMapped = _mapper.Map<ApplicationUserModel>(user);
            var newEmployee = await _employeeService.Create(empMapped,  password);
            return newEmployee;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetEmployeeList()
        {
           
            var empList =   await _employeeService.GetListOfAllEmployees();
            var employeeList = _mapper.Map<IEnumerable<ApplicationUserViewModel>>(empList);
            return employeeList;
        }

        public async Task<ApplicationUserViewModel> GetEmployeeById(string empId)
        {
            var emp = await _employeeService.GetEmployeeById(empId);
            var employeeDetails = _mapper.Map<ApplicationUserViewModel>(emp);
            return employeeDetails;
        }

        public async Task Update(ApplicationUserViewModel model)
        {
            var emp = _mapper.Map<ApplicationUserModel>(model);
            await _employeeService.Update(emp);
        }

        public async Task<ApplicationUserViewModel> FindEmail(string email)
        {
            var emailFromModel = await _employeeService.FindEmail(email);
            var resultEmail = _mapper.Map<ApplicationUserViewModel>(emailFromModel);
            return resultEmail;

        }
    }
}
