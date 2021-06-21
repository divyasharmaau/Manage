using AutoMapper;
using Manage.Application.Interface;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly ManageContext _manageContext;

        public LeaveService(ILeaveRepository leaveRepository , IMapper mapper , ManageContext manageContext)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
            _manageContext = manageContext;
        }
        public async Task<LeaveModel> AddNewLeave(LeaveModel leave)
        {
            var leaveMappedWithCore = _mapper.Map<Leave>(leave);
            var newLeave = await _leaveRepository.AddNewLeave(leaveMappedWithCore);
            var leaveMapped = _mapper.Map<LeaveModel>(newLeave);
            return leaveMapped;
            
        }

        public async Task<LeaveModel> GetMyLeaveDetails(int leaveId)
        {
            var leaveDetails = await _manageContext.Leaves.SingleOrDefaultAsync(x => x.Id == leaveId);
            //var leaveDetails2 = await _leaveRepository.GetMyLeaveDetails(leaveId);
            var leaveDetailsMapped = _mapper.Map<LeaveModel>(leaveDetails);
            return leaveDetailsMapped;
        }

        public async Task Update(LeaveModel leaveModel)
        {
            var leaveFromDb = await _leaveRepository.GetMyLeaveDetails(leaveModel.Id);
            var leave = _mapper.Map(leaveModel, leaveFromDb);
            await _leaveRepository.UpdateAsync(leaveFromDb);
        }

    }
}
