using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class EmployeePortalSPFX
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string fatherName { get; set; } = string.Empty;
        public string ExternalEmailId { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmergencyContactNumber { get; set; } = string.Empty;
        public string AadhaarNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string? EmployeeGraduation { get; set; }
        public string EmployeeEducationDetails { get; set; } = string.Empty;
        public string PANNumber { get; set; } = string.Empty;
        public string UANNo { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string PCountry { get; set; } = string.Empty;
        public string PState { get; set; } = string.Empty;
        public string PPin { get; set; } = string.Empty;
        public bool checkbox { get; set; }
        public string CurrentAddress { get; set; } = string.Empty;
        public string CCountry { get; set; } = string.Empty;
        public string CState { get; set; } = string.Empty;
        public string CPin { get; set; } = string.Empty;
        public string EmpBankName { get; set; } = string.Empty;
        public string EmpAccountName { get; set; } = string.Empty;
        public string EmpBankAccNumber { get; set; } = string.Empty;
        public string EmpBankIfsc { get; set; } = string.Empty;
        public string SecurityStamp { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ResponceDate { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
        public bool IsReplied { get; set; }
        public string AadhaarFile { get; set; } = string.Empty;
        public string PanFile { get; set; } = string.Empty;
        public string BankPassbookFile { get; set; } = string.Empty;
        public string CertificateFile { get; set; } = string.Empty;
        public string ProfilePhotoFile { get; set; } = string.Empty;
        public string SkillIDs { get; set; } = string.Empty;
        public DateTime DateOfJoin { get; set; } = DateTime.Now;
        public string CompanyName { get; set; } = string.Empty;
        public string? OtherSkills { get; set; }
        
        //[NotMapped]
        //public bool? MailToHr { get; set; }
        //[NotMapped]
        //public bool? MailToManager { get; set; }
        //[NotMapped]
        //public bool? MailToAccounts { get; set; }
    }
}
