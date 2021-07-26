using AutoMapper;
using Manage.Application.Interface;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Services
{
    public class DepartmentPageService : IDepartmentPageService
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentPageService( IDepartmentService departmentService , IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentViewModel>> GetDepartmentList()
        {
            var dList = await _departmentService.GetDepartmentList();
            var deptList =   _mapper.Map<IEnumerable<DepartmentViewModel>>(dList);
            return deptList;
        }

    }
}
