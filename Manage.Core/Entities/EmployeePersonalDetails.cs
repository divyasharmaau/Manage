using Manage.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manage.Core.Entities
{
   public class EmployeePersonalDetails 
   {
        //private string pPhotoPath;

        public string Id { get; set; }

        //Personal Details
        //public string PhotoPath
        //{
        //    get {
        //        return "https://localhost:44330/uploads/img/" + pPhotoPath;
        //    }
        //    set {
        //        this.pPhotoPath = value;
        //    }
        //}

        public string PhotoPath { get; set; }
        //public string FilePath { get; set; }
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        [DisplayName("Blood Group")]
        public string BloodGroup { get; set; }
        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }


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



        //Emergency Contact Details
        [DisplayName("Emergency Contact")]
        public string EmergencyContact { get; set; }
        public string Relationship { get; set; }

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
        [DisplayName("Zip Code")]
        public string EmergencyContactZipCode { get; set; }
        [DisplayName("Emergency Contact Phone Number")]
        public string EmergencyContactPhoneNumber { get; set; }

        //one to one relationship
        public int EmployeeId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


    }
}
