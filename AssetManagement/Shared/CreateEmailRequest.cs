using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto
{
    public class CreateEmailRequest
    {
        public string Name { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LandingUrl { get; set; } = string.Empty;

    }
}
