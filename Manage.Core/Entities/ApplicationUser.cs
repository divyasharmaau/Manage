using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.Core.Entities
{
   public class ApplicationUser : IdentityUser
   {
        public string Title { get; set; }
        [DisplayName("First Name")]
        [Required, MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{this.FirstName} {this.MiddleName} {this.LastName}"; }
        }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        public DateTime JoiningDate { get; set; }

        [DisplayName("Working Days in Week")]
        [Required]
        public int DaysWorkedInWeek { get; set; }
        [DisplayName("Number of Hours Per Day")]
        [Required]
        public double NumberOfHoursWorkedPerDay { get; set; }
        public string Manager { get; set; }


        //NAVIGATION PROPERTIES
        //n-1 relationship
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        ////1-1 relationship
        public EmployeePersonalDetails EmployeePersonalDetails { get; set; }
        //n-n relationship
        public ICollection<EmployeeLeave> EmployeeLeaves { get; set; }
        //1-n relationship
        public virtual List<Token> Tokens { get; set; }

    }
}
