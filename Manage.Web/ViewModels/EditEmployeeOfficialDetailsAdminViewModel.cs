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
    public class EditEmployeeOfficialDetailsAdminViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }
        [DisplayName("First Name")]
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{this.FirstName} {this.MiddleName} {this.LastName}"; }
        }

        [DisplayName("Joining Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        public string Status { get; set; }
      
        public List<SelectListItem> departmentList { get; set; }
        [Required]
        [DisplayName("Working Days in Week")]
      
        public int DaysWorkedInWeek { get; set; }
        [DisplayName("Working Hours Per Day")]
        [Required]
        public double NumberOfHoursWorkedPerDay { get; set; }


        public string Manager { get; set; }
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserName { get; set; }

        public int? DepartmentId { get; set; }
        public DepartmentViewModel Department { get; set; }

        public EmployeePersonalDetailsViewModel EmployeePersonalDetails { get; set; }



    }
}
