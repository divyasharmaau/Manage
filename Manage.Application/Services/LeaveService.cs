using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;

        public LeaveService(ILeaveRepository leaveRepository , IMapper mapper)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
        }
        public async Task<LeaveModel> AddNewLeave(LeaveModel leave)
        {
            var leaveMappedFromCore = _mapper.Map<Leave>(leave);
            await _leaveRepository.AddNewLeave(leaveMappedFromCore);
            return leave;
        }

        public async Task<LeaveModel> GetMyLeaveDetails(int leaveId)
        {
            var leaveDetails = await _leaveRepository.GetMyLeaveDetails(leaveId);
            var leaveDetailsMapped = _mapper.Map<LeaveModel>(leaveDetails);
            return leaveDetailsMapped;
        }
    }
}
