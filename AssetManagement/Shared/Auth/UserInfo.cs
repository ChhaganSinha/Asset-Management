using AssetManagement.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManagement.Dto.Auth
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }

        public List<string> Roles { get; set; }

        public string Email {  get; set; } 
        public string MobileNo {  get; set; } 
        public GenderEnum Gender {  get; set; } 

        public DateTime Dob {  get; set; }

        public string Address { get; set; }
        public bool IsActive { get; set; }

    }
}
