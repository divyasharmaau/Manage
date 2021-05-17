using Manage.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Core.Entities
{
   public class EmployeeLeave : Entity
   {
        public int EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }
        public int LeaveId { get; set; }
        public Leave Leave { get; set; }
   }
}
