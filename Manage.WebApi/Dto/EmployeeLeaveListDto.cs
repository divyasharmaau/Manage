using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Dto
{
    public class EmployeeLeaveListDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUserDto Employee { get; set; }
        public int LeaveId { get; set; }
        public LeaveDto Leave { get; set; }
        public ICollection<EmployeeLeaveDto> EmployeeLeaves { get; set; }
    }
}
