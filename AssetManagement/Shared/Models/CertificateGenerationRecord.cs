using System;

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
    }
}
