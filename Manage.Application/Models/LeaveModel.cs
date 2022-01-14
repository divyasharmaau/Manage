using Manage.Application.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.Application.Models
{
   public class LeaveModel 
    {
        public int Id { get; set; }
        [DisplayName("Date Applied")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CurrentDate { get; set; }

        public LeaveModel()
        {
            CurrentDate = DateTime.Now;
        }

        [DisplayName("Leave Type")]
        public string LeaveType { get; set; }

        public string LeaveStatus { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime TillDate { get; set; }

        public string Duration { get; set; }

        public string Reason { get; set; }
        public string Comment { get; set; }


        public string FilePath { get; set; }
        [DisplayName("Annual Leave")]
        public double BalanceAnnualLeave { get; set; }
        [DisplayName("Sick Leave")]
        public double BalanceSickLeave { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime JoiningDate { get; set; }

        //Navigation property
        public ICollection<EmployeeLeaveModel> EmployeeLeaves { get; set; }
    }
}
