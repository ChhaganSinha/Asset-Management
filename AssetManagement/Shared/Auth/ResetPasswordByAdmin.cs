using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Auth
{
    public class ResetPasswordByAdmin
    {
        public string Username {  get; set; } = string.Empty;
        [Required]
        public string ? Email {  get; set; }

        [Required]
        //[DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        //[DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }

    public class ModifyUserProfileByAdmin
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string? Email { get; set; }

        public string NewUsername { get; set; } = string.Empty;

        public string? NewEmail { get; set; }

    }
}
