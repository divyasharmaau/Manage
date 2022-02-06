using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.WebApi.ViewModels
{
   public class EmployeePersonalDetailsViewModel
   {
        public string Id { get; set; }
        public string FullName { get; set; }

        //Banking Details
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        public string Branch { get; set; }
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        [Required]
        public int BSB { get; set; }
        [DisplayName("Account Number")]
        [Required]
        public int AccountNumber { get; set; }

        //Employee Address

        [DisplayName("House/Unit Number")]
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        [DisplayName("City/Suburb")]
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        //Personal Details
        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        [DisplayName("Blood Group")]
        public string BloodGroup { get; set; }
        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }

        //Emergency Contact Details
        [DisplayName("Emergency Contact")]
        public string EmergencyContact { get; set; }
        public string Relationship { get; set; }
        [DisplayName("Emergency Contact Phone Number")]
        public string EmergencyContactPhoneNumber { get; set; }
        [DisplayName("House/Unit Number")]
        public string EmergencyContactHouseNumber { get; set; }
        [DisplayName("Street")]
        public string EmergencyContactStreet { get; set; }
        [DisplayName("City/Suburb")]
        public string EmergencyContactCity { get; set; }
        [DisplayName("State")]
        public string EmergencyContactState { get; set; }
        [DisplayName("Country")]
        public string EmergencyContactCountry { get; set; }
        [DisplayName("ZipCode")]
        public string EmergencyContactZipCode { get; set; }

        // Navigation Property one to one relationship
        public int EmployeeId { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }

    }
}
