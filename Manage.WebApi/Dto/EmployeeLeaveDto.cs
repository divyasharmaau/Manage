using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Dto
{
    public class EmployeeLeaveDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUserDto Employee { get; set; }
        public int LeaveId { get; set; }
        public LeaveDto Leave { get; set; }
    }
}
