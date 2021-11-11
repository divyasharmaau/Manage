using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class EditEmployeePersonalDetailsViewModel
    {
        public string Id { get; set; }
 
        public string FullName { get; set; }

        //Banking Details
        [DisplayName("Bank Name")]
        [Required]
        public string BankName { get; set; }
        [Required]
        public string Branch { get; set; }
        [DisplayName("Account Name")]
        [Required]
        public string AccountName { get; set; }
        [Required]
        public int BSB { get; set; }
        [DisplayName("Account Number")]
        [Required]
        public int AccountNumber { get; set; }

        //Employee Address
        [Required]
        [DisplayName("House/Unit Number")]
        public string HouseNumber { get; set; }
        [Required]
        public string Street { get; set; }
        [DisplayName("City/Suburb")]
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }
        //Personal Details
        //public string PhotoPath { get; set; }
        public string ExistingPhotoPath { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        // public string FilePath { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Nationality { get; set; }
        [DisplayName("Blood Group")]
        [Required]
        public string BloodGroup { get; set; }
        [DisplayName("Marital Status")]
        [Required]
        public string MaritalStatus { get; set; }
        [DisplayName("Gender")]
        [Required]
        public string Gender { get; set; }

        //Emergency Contact Details
        [Required]
        [DisplayName("Emergency Contact")]
        public string EmergencyContact { get; set; }
        [Required]
        public string Relationship { get; set; }
        [Required]
        [DisplayName("Emergency Contact Phone Number")]
        public string EmergencyContactPhoneNumber { get; set; }
        [DisplayName("House/Unit Number")]
        [Required]
        public string EmergencyContactHouseNumber { get; set; }
        [DisplayName("Street")]
        [Required]
        public string EmergencyContactStreet { get; set; }
        [DisplayName("City/Suburb")]
        [Required]
        public string EmergencyContactCity { get; set; }
        [DisplayName("State")]
        [Required]
        public string EmergencyContactState { get; set; }
        [DisplayName("Country")]
        [Required]
        public string EmergencyContactCountry { get; set; }
        [DisplayName("ZipCode")]
        [Required]
        public string EmergencyContactZipCode { get; set; }
        // Navigation Property one to one relationship
        public int EmployeeId { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }


    }
}
