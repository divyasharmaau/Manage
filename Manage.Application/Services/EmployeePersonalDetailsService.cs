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
   public class EmployeePersonalDetailsService : IEmployeePersonalDetailsService
    {
        private readonly IEmployeePersonalDetailsRepository _employeePersonalDetailsRepository;
        private readonly IMapper _mapper;

        public EmployeePersonalDetailsService(IEmployeePersonalDetailsRepository employeePersonalDetailsRepository , IMapper mapper)
        {
            _employeePersonalDetailsRepository = employeePersonalDetailsRepository;
            _mapper = mapper;
        }

        public async Task<EmployeePersonalDetailsModel> AddAsync(EmployeePersonalDetailsModel model)
        {
            var entity = _mapper.Map<EmployeePersonalDetails>(model);
            var empDetails = await _employeePersonalDetailsRepository.AddAsync(entity);
            //var empDetailsModel = _mapper.Map<EmployeePersonalDetailsModel>(empDetails);
            //return empDetailsModel;
            return model;
        }

        public async Task<EmployeePersonalDetailsModel> GetEmployeeById(string employeeId)
        {

           var empEntity = await _employeePersonalDetailsRepository.GetEmployeeById(employeeId);
            var employeeModel = _mapper.Map<EmployeePersonalDetailsModel>(empEntity);
            return employeeModel;
        }

        public async Task UpdateAsync(EmployeePersonalDetailsModel model)
        {
             // var entity = _mapper.Map<EmployeePersonalDetails>(model);
             //await _employeePersonalDetailsRepository.UpdateAsync(entity);
            var emp = await _employeePersonalDetailsRepository.GetEmployeeById(model.Id);
            //var emp = await _manageContext.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
            _mapper.Map(model, emp);
            await _employeePersonalDetailsRepository.UpdateAsync(emp);

        }
    }
}
