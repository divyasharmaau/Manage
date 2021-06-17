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

            //EmployeeLeaveViewModel model = new EmployeeLeaveViewModel();
            //try
            //{
            //    var leaveModel = await _employeeLeaveService.GetLeaveByIdModel(leaveId);
            //    //var leave = _mapper.Map<EmployeeLeaveViewModel>(leaveModel);
            //    model = _mapper.Map<EmployeeLeaveViewModel>(leaveModel);
            //    //return leave;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return model;
        }
    }
}
