using AutoMapper;
using Azure.Messaging.ServiceBus;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly ManageContext _manageContext;
 
        public EmployeeService(IEmployeeRepository employeeRepository ,IMapper mapper, ManageContext manageContext)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _manageContext = manageContext;
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

        //public async Task<IdentityResult> Update(ApplicationUserModel user)
        //{
        //    var emp = _mapper.Map<ApplicationUser>(user);
        //   var result =  await  _employeeRepository.Update(emp);
        //    return result;
        //    //var employee = await  _employeeRepository.Update(emp);
        //    //return employee;
        //}



        public  async Task Update(ApplicationUserModel user)
        {
            //var emp = _mapper.Map<ApplicationUser>(user);
            var emp = await _manageContext.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
            // _mapper.Map<ApplicationUser>(user);
            _mapper.Map(user, emp);
            await _employeeRepository.Update(emp);
        }

        

        public async Task<ApplicationUserModel> FindEmail(string email)
        {
            var resultEmailFromDB = await _employeeRepository.FindEmail(email);
            var resultEmail = _mapper.Map<ApplicationUserModel>(resultEmailFromDB);
            return resultEmail;

        }
    }
}
