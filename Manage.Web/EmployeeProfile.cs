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
        }
    }
}
