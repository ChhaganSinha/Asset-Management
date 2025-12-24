using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models
{
    public class CertificateGenerationRecord : BaseEntity
    {
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string GeneratedFileName { get; set; } = string.Empty;
        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
        public string GeneratedBy { get; set; } = string.Empty;

        [NotMapped]
        public string EmployeeName { get; set; } = string.Empty;

        [NotMapped]
        public string CompanyCode { get; set; } = string.Empty;

        [NotMapped]
        public string CertificateType { get; set; } = string.Empty;
    }
}
