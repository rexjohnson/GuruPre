using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GuruETC.Web.Models
{
    public class PatientModel
    {
        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Full Name*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "UserName*")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "PhoneNumber*")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Date of Birth*")]
        public string DOB { get; set; }

        public string MedicalHistory { get; set; }

        public string PatientHistorical { get; set; }

        public string PersonalMotivator { get; set; }

        public Guid doctorkey { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "Required UserName!")]
        [Display(Name="UserName*")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage="Required Password!")]
        [Display(Name = "Password*")]
        public string Password { get; set; }
    }
    
}