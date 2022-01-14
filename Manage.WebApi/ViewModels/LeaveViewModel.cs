using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class LeaveViewModel
    {

        public int Id { get; set; }
        [DisplayName("Date Applied")]
        public DateTime CurrentDate { get; set; }
        public LeaveViewModel()
        {
            CurrentDate = DateTime.Now.Date;
        }
        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [DisplayName("Leave Type")]
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }

        [Display(Name = "From Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Display(Name = "Till Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TillDate { get; set; }
        public string Duration { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string FilePath { get; set; }
        public string ExistingFilePath { get; set; }
        [DisplayName("Annual Leave")]
        public double BalanceAnnualLeave { get; set; }
        [DisplayName("Casual/Sick Leave")]
        public double BalanceSickLeave { get; set; }
    }
}
