using Manage.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class CreateEmployeeViewModel
    {
      
        public string Id { get; set; }
        public string Title { get; set; }
        [DisplayName("First Name")]
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{this.FirstName} {this.MiddleName} {this.LastName}"; }
        }

        [DisplayName("Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime JoiningDate { get; set; }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Status { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public List<SelectListItem> departmentList { get; set; }
        [Required]
        [DisplayName("Working Days in Week")]
        public int DaysWorkedInWeek { get; set; }
        [Required]
        [DisplayName("Working Hours Per Day")]
        public double NumberOfHoursWorkedPerDay { get; set; }
    
        public string Manager { get; set; }
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Remote(action: "IsEmailInUse", controller: "Employee")]
        [ValidEmailDomain(allowedDomain:"gmail.com" , ErrorMessage ="Email Domain  must be gmail.com")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
            ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
