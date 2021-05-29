using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository , IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Create(ApplicationUserModel user, string password)
        {
                var emp =   _mapper.Map<ApplicationUser>(user);
                var employee =   await _employeeRepository.Create(emp, password);
                return employee;
            
        }

        public async Task<ApplicationUserModel> GetEmployeeById(string id)
        {
               var employee = await _employeeRepository.GetEmployeeById(id);
               var empModel =  _mapper.Map<ApplicationUserModel>(employee);
               return empModel;
           
        }

        public async Task<IEnumerable<ApplicationUserModel>> GetListOfAllEmployees()
        {
                var empList = await _employeeRepository.GetAllEmployeeList();
                var empListModel =  _mapper.Map<IEnumerable<ApplicationUserModel>>(empList);
                return empListModel;
        }

        public async Task<IdentityResult> Update(ApplicationUserModel user)
        {
               var emp = _mapper.Map<ApplicationUser>(user);
               var employee = await  _employeeRepository.Update(emp);
               return employee;
        }
    }
}
