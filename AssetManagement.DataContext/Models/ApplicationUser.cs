
using AssetManagement.Dto.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.DataContext.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public GenderEnum Gender { get; set; }
       
        public DateTime Dob { get; set; }

        public string Address { get; set; } = string.Empty;

        [Required]
        //[DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        //[DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        //[DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
