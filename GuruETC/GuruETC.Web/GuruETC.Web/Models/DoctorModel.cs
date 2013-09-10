using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GuruETC.Web.Models
{
    public class DoctorModel
    {
        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Name*")]
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
        public string DDOB { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "PracticeName*")]
        public string Practice { get; set; }
    }
}