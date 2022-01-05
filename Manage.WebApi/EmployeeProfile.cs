using AutoMapper;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.WebApi.Dto;
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
            //EmployeeController
            CreateMap<ApplicationUserModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeListViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, CreateEmployeeViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EditEmployeeOfficialDetailsViewModel>().ReverseMap();
            CreateMap<EditEmployeeOfficialDetailsAdminViewModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            //mapping webApi <=> ViewModels 
            CreateMap<ApplicationUserViewModel, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeOfficialDetailsReadDto>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EditEmployeeOfficialDetailsDto>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, EmployeePersonalDetailsDto>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, CreateEmployeePersonalDetailsDto>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, EditEmployeePersonalDetailsDto>().ReverseMap();
            //dto's
            CreateMap<ApplicationUserDto, CreateEmployeeDto>().ReverseMap();
            CreateMap<EmployeeOfficialDetailsReadDto, CreateEmployeeDto>().ReverseMap();

            CreateMap<DepartmentModel, DepartmentViewModel>().ReverseMap();

            CreateMap<EmployeePersonalDetailsModel, EmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, CreateEmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<EditEmployeePersonalDetailsViewModel, EmployeePersonalDetailsViewModel>().ReverseMap();

            //LeaveController
            CreateMap<LeaveModel, LeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveModel, EmployeeLeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, LeaveViewModel>().ReverseMap();
            CreateMap<LeaveViewModel, ApplyLeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, EditMyLeaveViewModel>().ReverseMap();
            CreateMap<Leave, LeaveViewModel>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, EditLeaveAdminViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, AppUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeLeaveViewModel>().ReverseMap();
           // CreateMap<ApplicationUserViewModel, EmployeeLeaveViewModel>().ReverseMap();

            //ViewModel , dto
            CreateMap<LeaveViewModel, ApplyLeaveDto>().ReverseMap();
            CreateMap<EmployeeLeaveViewModel, EmployeeLeaveDto>().ReverseMap();
            CreateMap<LeaveViewModel, LeaveDto>().ReverseMap();
            CreateMap<ApplicationUserDto, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeLeaveDto>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeeLeaveListDto>().ReverseMap();
            CreateMap<EditMyLeaveDto, LeaveViewModel>().ReverseMap();
            CreateMap<AppUserDto, AppUserViewModel>().ReverseMap();
            //dto<=>dto
           // CreateMap<ApplicationUserDto, EmployeeLeaveListDto>().ReverseMap();
    

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
