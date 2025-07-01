using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Dto.Auth
{
    public class ForgetPasswordRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
