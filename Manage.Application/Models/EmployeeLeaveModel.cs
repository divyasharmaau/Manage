using Manage.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Application.Models
{
   public class EmployeeLeaveModel : BaseModel
    {
        public int EmployeeId { get; set; }
        public ApplicationUserModel Employee { get; set; }
        public int LeaveId { get; set; }
        public LeaveModel Leave { get; set; }
    }
}
