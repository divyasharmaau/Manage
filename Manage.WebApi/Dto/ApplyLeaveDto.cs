using Manage.WebApi.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Dto
{
    public class ApplyLeaveDto
    {

        public int Id { get; set; }

        public string UserId { get; set; }
        [DisplayName("Date Applied")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrentDate { get; set; }
        public ApplyLeaveDto()
        {
            CurrentDate = DateTime.Now.Date;
        }

        [DisplayName("Leave Type")]
        public string LeaveType { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Till Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("FromDate", "Till Date should be Greater than or Equal to From Date")]
        public DateTime TillDate { get; set; }
        public string Duration { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Comment { get; set; }
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        [DisplayName("Annual Leave")]
        public double BalanceAnnualLeave { get; set; }
        [DisplayName("Casual/Sick Leave")]
        public double BalanceSickLeave { get; set; }
    }
}
