using Manage.Core.Entities.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.Core.Entities
{
   public class Leave 
   {
        public int Id { get; set; }
        [DisplayName("Date Applied")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CurrentDate { get; set; }

        public Leave()
        {
            CurrentDate = DateTime.Now;
        }

        [DisplayName("Leave Type")]
        [Required]
        public string LeaveType { get; set; }

        public string LeaveStatus { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        //[TillDateMustBeEqualOrGreaterThanFromDate]
        public DateTime TillDate { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Comment { get; set; }

        //public List<IFormFile> File { get; set; }
        public string FilePath { get; set; }
        [DisplayName("Annual Leave")]
        public double BalanceAnnualLeave { get; set; }
        [DisplayName("Sick Leave")]
        public double BalanceSickLeave { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime JoiningDate { get; set; }

        //collection
        public ICollection<EmployeeLeave> EmployeeLeaves { get; set; }
    }
}
