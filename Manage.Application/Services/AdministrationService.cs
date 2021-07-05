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
        private readonly IMapper _mapper;

        public AdministrationService(IAdministrationRepository administrationRepository , IMapper mapper)
        {
            _administrationRepository = administrationRepository;
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
    }
}