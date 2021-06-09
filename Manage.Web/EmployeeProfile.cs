using AutoMapper;
using Manage.Application.Models;
using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<ApplicationUserModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<DepartmentModel, DepartmentViewModel>().ReverseMap();
            CreateMap<EmployeePersonalDetailsModel, EmployeePersonalDetailsViewModel>().ReverseMap();


            CreateMap<ApplicationUserViewModel, EmployeeListViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EditEmployeeOfficialDetailsViewModel>().ReverseMap();
            CreateMap<EditEmployeeOfficialDetailsAdminViewModel, ApplicationUserViewModel>().ReverseMap();
            CreateMap<EmployeePersonalDetailsViewModel, CreateEmployeePersonalDetailsViewModel>().ReverseMap();
            CreateMap<ApplicationUserViewModel, EmployeePersonalDetailsViewModel>().ReverseMap(); 
           
        }
    }
}
