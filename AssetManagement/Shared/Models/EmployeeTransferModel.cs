using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class EmployeeTransferModel
    {
        public int Id { get; set; }
        public string NewEmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
        public string SourceCompanyCode { get; set; } = string.Empty;
        public string TargetCompanyCode { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public int TargetCompanyId { get; set; } 
        public DateTime TransferDate { get; set; } = DateTime.Now;
    }
}
