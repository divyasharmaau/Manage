using Manage.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class EmployeeOfficialDetailsReadDto
    {
        public string Id { get; set; }
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

        [DisplayName("Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public int DepartmentId { get; set; }
        //public string Department { get; set; }
        //public List<SelectListItem> departmentList { get; set; }
        //public IEnumerable<DepartmentViewModel> departmentList2 { get; set; }
        [DisplayName("Working Days in Week")]
        public int DaysWorkedInWeek { get; set; }
        //public int WorkingDaysInWeek { get; set; }
        [DisplayName("Working Hours Per Day")]
        public double NumberOfHoursWorkedPerDay { get; set; }
        // public double WorkingHoursPerDay { get; set; }
        public string Manager { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        [Required]
        [ValidEmailDomain(allowedDomain: "mail.com", ErrorMessage = "Email Domain  must be mail.com")]
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
