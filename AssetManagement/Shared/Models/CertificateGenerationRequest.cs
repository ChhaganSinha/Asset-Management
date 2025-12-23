using System;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Dto.Models
{
    public class CertificateGenerationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        public CertificateType CertificateType { get; set; } = CertificateType.Experience;

        [Required]
        public DateTime LetterDate { get; set; } = DateTime.Today;

        public string Title { get; set; } = "Mr.";
        public string HeShe { get; set; } = "He";
        public string HisHer { get; set; } = "His";
        public string HimHer { get; set; } = "Him";
    }

    public class CertificateGenerationResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }

    public enum CertificateType
    {
        Experience,
        Relieving
    }
}
