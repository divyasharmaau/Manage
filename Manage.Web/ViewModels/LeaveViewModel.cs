﻿using Manage.Web.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class LeaveViewModel
    {

        public int Id { get; set; }
        [DisplayName("Date Applied")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrentDate { get; set; }
        public LeaveViewModel()
        {
            CurrentDate = DateTime.Now.Date;
        }

        [DisplayName("Leave Type")]
        [Required]
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }

        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "From Date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

   

        [Display(Name = "Till Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("FromDate", "Till Date should be Greater than or Equal to From Date")]
        public DateTime TillDate { get; set; }

        [Required]
        public string Duration { get; set; }
        [Required]
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
