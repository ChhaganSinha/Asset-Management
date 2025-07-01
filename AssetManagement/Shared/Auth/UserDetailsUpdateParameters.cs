using AssetManagement.Dto.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Auth
{
    public class UserDetailsUpdateParameters
    {
        [Required]
        public string NewUserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string NewEmail { get; set; } = string.Empty;

        public string NewMobileNo { get; set; }
        public GenderEnum NewGender{ get; set; }

        public DateTime NewDob {  get; set; }
        public string NewAddress{ get; set; }
    }

}
