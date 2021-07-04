using AutoMapper;
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
    }
}
