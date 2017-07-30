using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class AuthenticationModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "User password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm user password")]
        [Compare("Password", ErrorMessage = "The password and confirmation passowrd do not match.")]
        public string ConfigmPassword { get; set; }
    }
}