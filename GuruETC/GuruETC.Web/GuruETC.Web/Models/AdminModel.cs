using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GuruETC.Web.Models
{
    public class AdminModel
    {
    }

    public class RoleDB
    {
        [Required]
        public string Name { get; set; }
    }


    public class AdminDetail
    {
        [Required(ErrorMessage = "Required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ADOB { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Email { get; set; }
       
    }
}