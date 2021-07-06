﻿using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Services
{
    public class AdministrationPageService : IAdministrationPageService
    {
        private readonly IAdministrationService _administrationService;
        private readonly IMapper _mapper;

        public AdministrationPageService(IAdministrationService administrationService, IMapper mapper)
        {
            _administrationService = administrationService;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRoleViewModel model)
        {
            var mapped = _mapper.Map<ApplicationRoleModel>(model);
            var role = await _administrationService.CreateRole(mapped);
            return role;
        }

        public async Task<IEnumerable<ApplicationRoleViewModel>> GetRolesList()
        {
            var roleList = await _administrationService.GetRolesList();
            var mappedRoleList = _mapper.Map<IEnumerable<ApplicationRoleViewModel>>(roleList);
            return mappedRoleList;
        }

        public async Task<ApplicationRoleViewModel> GetRoleById(string id)
        {
            var role = await _administrationService.GetRoleById(id);
            var mappedRole = _mapper.Map<ApplicationRoleViewModel>(role);
            return mappedRole;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersInRole(string name)
        {
            
            var users = await _administrationService.GetUsersInRole(name);
            var mappedUsers = _mapper.Map<IEnumerable<ApplicationUserViewModel>>(users);
            return mappedUsers;
        }

        public async Task<IdentityResult> Update(ApplicationRoleViewModel role)
        {
            var roleFromModel = _mapper.Map<ApplicationRoleModel>(role);
            var result = await _administrationService.Update(roleFromModel);
            return result;
        }

    }
}
