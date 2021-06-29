using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class EditLeaveAdminViewModel
    {
        public int Id { get; set; }
        public ApplicationUserViewModel Employee { get; set; }
        public int EmployeeId { get; set; }
        public LeaveViewModel LeaveModel { get; set; }
        public int LeaveId { get; set; }
        public string LeaveStatus { get; set; }
    }
}
