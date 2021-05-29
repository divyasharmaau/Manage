using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository , IMapper mapper)
        {
            _departmentRepository =  departmentRepository;
            _mapper = mapper;
        }
        public async Task<DepartmentModel> GetDepartment(int departmentId)
        {
            var dept = await _departmentRepository.GetDepartment(departmentId);
            var department = _mapper.Map<DepartmentModel>(dept);
            return department;
        }

        public async Task<IEnumerable<DepartmentModel>> GetDepartmentList()
        {
            var deptList = await _departmentRepository.GetDepartmentList();
            var departmentList = _mapper.Map<IEnumerable<DepartmentModel>>(deptList);
            return departmentList;
           
        }

        public async Task<DepartmentModel> GetEmployeeDepartment(string employeeId)
        {
            var empDept = await _departmentRepository.GetEmployeeDepartment(employeeId);
            var employeeDept = _mapper.Map<DepartmentModel>(empDept);
            return employeeDept;
           
        }
    }
}
