using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IAdministrationRepository _administrationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public AdministrationService(IAdministrationRepository administrationRepository , IEmployeeRepository employeeRepository ,IMapper mapper )
        {
            _administrationRepository = administrationRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateRole(ApplicationRoleModel role)
        {
            var mapped = _mapper.Map<ApplicationRole>(role);
            var empRole = await _administrationRepository.CreateRole(mapped);
            return empRole;
        }

        public async Task<IEnumerable<ApplicationRoleModel>> GetRolesList()
        {
            var roleList = await _administrationRepository.GetRolesList();
            var mappedRoleList = _mapper.Map<IEnumerable<ApplicationRoleModel>>(roleList);
            return mappedRoleList;
        }

        public async Task<ApplicationRoleModel> GetRoleById(string id)
        {
            var role = await _administrationRepository.GetRoleById(id);
            var mappedRole = _mapper.Map<ApplicationRoleModel>(role);
            return mappedRole;
        }

        public async Task<IEnumerable<ApplicationUserModel>> GetUsersInRole(string name)
        {
            var users = await _administrationRepository.GetUsersInRole(name);
            var mappedUsers = _mapper.Map<IEnumerable<ApplicationUserModel>>(users);
            return mappedUsers;
        }

        public async Task<bool> UserInRole(ApplicationUserModel user, string roleName)
        {
            var mapped = _mapper.Map<ApplicationUser>(user);
            var result = await _administrationRepository.UserInRole(mapped, roleName);
            return result;

        }

        public async Task<IdentityResult> Update(ApplicationRoleModel role)
        {
            var roleFromDb =  await _administrationRepository.GetRoleById(role.Id);
            var mapped = _mapper.Map(role, roleFromDb);
            var result = await _administrationRepository.Update(roleFromDb);
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUserModel user, string roleName)
        {
            var mappedEmp = _mapper.Map<ApplicationUser>(user);
            var result = await _administrationRepository.AddToRoleAsync(mappedEmp, roleName);
            return result;
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUserModel user, string roleName)
        {
            var mappedEmp = _mapper.Map<ApplicationUser>(user);
            var result = await _administrationRepository.RemoveFromRoleAsync(mappedEmp, roleName);
            return result;
        }
    }
}