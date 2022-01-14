using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.Application.Models
{
   public class AppUserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }


        //public string FromDateString { get; set; }
        //public string TillDateString { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime TillDate { get; set; }
        [DisplayName("Leave Type")]

        public string LeaveType { get; set; }
        public int LeaveId { get; set; }
        [DisplayName("Number of Leave Days")]
        public double NumberOfLeaveDays { get; set; }
        public string Reason { get; set; }
        public string LeaveStatus { get; set; }
        public string Duration { get; set; }

        [DisplayName("Annual Leave")]
        public double BalanceAnnualLeave { get; set; }
        [DisplayName("Sick Leave")]
        public double BalanceSickLeave { get; set; }
    }
}
