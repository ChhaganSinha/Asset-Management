using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class EmployeeFilesMapping : BaseEntity
    {

        public int EmployeeId { get; set; } 
        public string AadhaarFile { get; set; } = string.Empty;
        public string PanFile { get; set; } = string.Empty;
        public string BankPassbookFile { get; set; } = string.Empty;
        public string CertificateFile { get; set; } = string.Empty;
        public string ProfilePhotoFile { get; set; } = string.Empty;
        public string ResumeFile { get; set; } = string.Empty;

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set;}
    }
}
