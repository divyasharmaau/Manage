using AutoMapper;
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
            //var employeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(mappedEmpDetails);
            //return employeePersonalDetails;
            return model;
        }
    }
}
