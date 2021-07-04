using AutoMapper;
using Manage.Application.Models;
using Manage.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Application.Mapper
{
   public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<ApplicationUserModel, ApplicationUser>().ReverseMap();
            CreateMap<DepartmentModel, Department>().ReverseMap();
            CreateMap<EmployeePersonalDetailsModel, EmployeePersonalDetails>().ReverseMap();
            CreateMap<LeaveModel, Leave>().ReverseMap();
            CreateMap<EmployeeLeaveModel, EmployeeLeave>().ReverseMap();
            CreateMap<ApplicationRoleModel, ApplicationRole>().ReverseMap();
        }
    }
}
