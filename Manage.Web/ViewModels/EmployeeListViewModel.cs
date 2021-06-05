using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class EmployeeListViewModel
    {
        public string Id { get; set; }
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
        public string Manager { get; set; }
        public string Email { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        //NAVIGATION PROPERTIES
        //n-1 relationship
        public int? DepartmentId { get; set; }
        public DepartmentViewModel Department { get; set; }
        ////1-1 relationship
        public EmployeePersonalDetailsViewModel EmployeePersonalDetails { get; set; }
    }
}
