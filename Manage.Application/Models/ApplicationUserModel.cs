using Manage.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Manage.Application.Models
{
   public class ApplicationUserModel 

    {

        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
   

        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
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
        public DateTime JoiningDate { get; set; }

        [DisplayName("Working Days in Week")]
        public int DaysWorkedInWeek { get; set; }

        [DisplayName("Number of Hours Per Day")]
        public double NumberOfHoursWorkedPerDay { get; set; }
        public string Manager { get; set; }



        //NAVIGATION PROPERTIES
        //n-1 relationship
        public int? DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }
        ////1-1 relationship
        public EmployeePersonalDetailsModel EmployeePersonalDetails { get; set; }
        //n-n relationship
        public ICollection<EmployeeLeaveModel> EmployeeLeaves { get; set; }
    }
}
