﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.WebApi.ViewModels
{
   public class EmployeeLeaveViewModel 
   {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUserViewModel Employee { get; set; }
        public int LeaveId { get; set; }
        public LeaveViewModel Leave { get; set; }
        public string Approved { get; set; }
        public string Declined { get; set; }
        public string Comment { get; set; }
    }
}
