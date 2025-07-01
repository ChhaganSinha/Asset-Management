using AssetManagement.Dto.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssetManagement.Dto.Auth
{
    public class RegisterParameters
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime Dob { get; set; }

        public string Address { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
