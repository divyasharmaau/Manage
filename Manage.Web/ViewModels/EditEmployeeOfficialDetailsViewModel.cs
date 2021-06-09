using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class EditEmployeeOfficialDetailsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        [DisplayName("First Name")]
   
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]

        public string LastName { get; set; }

        public string FullName
        {
            get { return $"{this.FirstName} {this.MiddleName} {this.LastName}"; }
        }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Status { get; set; }
        [DisplayName("Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [DisplayName("Working Days in Week")]
        public int DaysWorkedInWeek { get; set; }

        [DisplayName("Number of Working Hours Per Day")]
        public double NumberOfHoursWorkedPerDay { get; set; }
        public string Manager { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        //NAVIGATION PROPERTIES
        //n-1 relationship
        public int? DepartmentId { get; set; }
        public DepartmentViewModel Department { get; set; }
        ////1-1 relationship
        public EmployeePersonalDetailsViewModel EmployeePersonalDetails { get; set; }
        public ICollection<EmployeeLeaveViewModel> EmployeeLeaves { get; set; }
    }
}
