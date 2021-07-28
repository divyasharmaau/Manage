using AutoMapper;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<ApplicationUserModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeListViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, CreateEmployeeViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EditEmployeeOfficialDetailsViewModel>().ReverseMap();
            CreateMap<EditEmployeeOfficialDetailsAdminViewModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeOfficialDetailsReadDto>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, EmployeePersonalDetailsReadDto>().ReverseMap();

            CreateMap<DepartmentModel, DepartmentViewModel>().ReverseMap();

            CreateMap<EmployeePersonalDetailsModel, EmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, CreateEmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<EditEmployeePersonalDetailsViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();

            CreateMap<LeaveModel, LeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveModel, EmployeeLeaveViewModel>().ReverseMap();
            CreateMap<LeaveViewModel, ApplyLeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, EditMyLeaveViewModel>().ReverseMap();
            CreateMap<Leave, LeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, EditLeaveAdminViewModel>().ReverseMap();

            CreateMap<AppUserModel, AppUserViewModel>().ReverseMap();
            CreateMap<ApplicationRoleModel, ApplicationRoleViewModel>().ReverseMap();
            CreateMap<ApplicationRoleViewModel, CreateRoleViewModel>().ReverseMap();


            //CreateMap<ApplicationUserViewModel, EmployeeListViewModel>().ReverseMap();
            //CreateMap<ApplicationUserViewModel, CreateEmployeeViewModel>().ReverseMap();
            //CreateMap<ApplicationUserViewModel, EditEmployeeOfficialDetailsViewModel>().ReverseMap();
            //CreateMap<EditEmployeeOfficialDetailsAdminViewModel, ApplicationUserViewModel>().ReverseMap();


            //CreateMap<EmployeePersonalDetailsViewModel, CreateEmployeePersonalDetailsViewModel>().ReverseMap();
            //CreateMap<ApplicationUserViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();
            //CreateMap<EditEmployeePersonalDetailsViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();

            //CreateMap<LeaveViewModel, ApplyLeaveViewModel>().ReverseMap();
            //CreateMap<EmployeeLeaveViewModel, EditMyLeaveViewModel>().ReverseMap();
            //CreateMap<Leave, LeaveViewModel>().ReverseMap();
            //CreateMap<EmployeeLeaveViewModel, EditLeaveAdminViewModel>().ReverseMap();

            //CreateMap<ApplicationRoleViewModel, CreateRoleViewModel>().ReverseMap();


        }
    }
}
