using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Services
{
    public class LeavePageService : ILeavePageService
    {
        private readonly ILeaveService _leaveService;
        private readonly IMapper _mapper;

        public LeavePageService(ILeaveService leaveService , IMapper mapper)
        {
            _leaveService = leaveService;
            _mapper = mapper;
        }

        public async Task<LeaveViewModel> AddNewLeave(LeaveViewModel leaveViewModel)
        {
            var  newLeaveMappedWithApplicationModel =   _mapper.Map<LeaveModel>(leaveViewModel);
            await _leaveService.AddNewLeave(newLeaveMappedWithApplicationModel);
            return leaveViewModel;
        }
    }
}
