using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Services
{
    public class EmployeePersonalDetailsPageService : IEmployeePersonalDetailsPageService
    {
        private readonly IEmployeePersonalDetailsService _employeePersonalDetailsService;
        private readonly IMapper _mapper;

        public EmployeePersonalDetailsPageService(IEmployeePersonalDetailsService employeePersonalDetailsService , IMapper mapper)
        {
            _employeePersonalDetailsService = employeePersonalDetailsService;
            _mapper = mapper;
        }

        public async Task<EmployeePersonalDetailsViewModel> AddAsync(EmployeePersonalDetailsViewModel model)
        {
            var empDetailsFromApp = _mapper.Map<EmployeePersonalDetailsModel>(model);
            var mappedEmpDetails =   await _employeePersonalDetailsService.AddAsync(empDetailsFromApp);
            return model;
        }

        public async Task<EmployeePersonalDetailsViewModel>GetEmployeePersonalDetailsById(string id)
        {
           var empDetails =   await  _employeePersonalDetailsService.GetEmployeeById(id);
           var employeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(empDetails);
           return employeePersonalDetails;
        } 

        public async Task<EmployeePersonalDetailsViewModel>UpdateAsync(EmployeePersonalDetailsViewModel model)
        {
            var empPersonalDetailsMapped = _mapper.Map<EmployeePersonalDetailsModel>(model);
            await _employeePersonalDetailsService.UpdateAsync(empPersonalDetailsMapped);
            return model;
        }
    }
}
