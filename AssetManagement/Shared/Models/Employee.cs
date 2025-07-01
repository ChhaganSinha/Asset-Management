using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AssetManagement.Dto.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]

        public string? CompanyCode { get; set; }
        [NotMapped]
        public int Pincode { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        public EmployeeStatus Status { get; set; }

        [Required]
        [EmailAddress]
        public string EmailId { get; set; } = string.Empty;

        public string ExternalEmailId { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number!")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        public string PermanentAddress { get; set; } = string.Empty;

        [Required]
        public string PCountry { get; set; } = string.Empty;

        [Required]
        public string PState { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pin is Invalid")]
        public string PPin { get; set; } = string.Empty;

        public bool checkbox { get; set; }

        [Required]
        public string CurrentAddress { get; set; } = string.Empty;

        [Required]
        public string CCountry { get; set; } = string.Empty;

        [Required]
        public string CState { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pin is Invalid")]
        public string CPin { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Invalid Aadhaar Number!")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Pan Number is Invalid")]
        public string PANNumber { get; set; } = string.Empty;

        public string UANNo { get; set; } = string.Empty;

        public string EmergencyContactNumber { get; set; } = string.Empty;

        public string? EmployeeEducation { get; set; }

        public string EmployeeEducationDetails { get; set; } = string.Empty;

        public string? EmployeeCategory { get; set; }

        public string? Department { get; set; }
        public int ? VendorPincode { get; set; }

        [Required]
        public DateTime DateOfJoin { get; set; }
        
        public DateTime? DeputationStartDate { get; set; }
        public DateTime? DeputationEndDate  { get; set; }

        public DateTime DateOfLeaving { get; set; } = DateTime.Now;

        [Required]
        public string Designation { get; set; } = string.Empty;

        //[Required]
        public string ReportingTo { get; set; } = string.Empty;
        public string? Reference { get; set; } 

        [Required]
        public string fatherName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Required]
        public string EmpBankName { get; set; } = string.Empty;

        [Required]
        public string EmpAccountName { get; set; } = string.Empty;

        [Required]
        public string EmpBankAccNumber { get; set; } = string.Empty;

        [Required]
        public string EmpBankIfsc { get; set; } = string.Empty;

        public bool IsEngazed { get; set; }

        public int EngazeCount { get; set; } = 0;

        public string SecurityStamp { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = string.Empty;

        public bool IsReplied { get; set; }

        [NotMapped]
        public string BaseUrl { get; set; } = string.Empty;

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public string? OtherSkills { get; set; }

        [NotMapped]
        public string Skills { get; set; } = string.Empty;

        [NotMapped]
        public bool EMS { get; set; }

        [NotMapped]
        public string EmsManagerName { get; set; } = string.Empty;

        [NotMapped]
        public string EmsManagerEmail { get; set; } = string.Empty;

        [NotMapped]
        public string EmsManagerEmpId { get; set; } = string.Empty;

        [NotMapped]
        public bool SharepointPortal { get; set; }

        [NotMapped]
        public bool LM { get; set; }

        [NotMapped]
        public string LmManagerName { get; set; } = string.Empty;

        [NotMapped]
        public string LmManagerEmail { get; set; } = string.Empty;

        [NotMapped]
        public string LmManagerEmpId { get; set; } = string.Empty;

        [NotMapped]
        public bool CMS { get; set; }

        public bool SharepointSync { get; set; } = false;
        public string? vendorLocation { get; set; } 
        public string? vendorName { get; set; }
        public string? DeputationCompanyStatus { get; set; }

        public List<EmployeeInsurance>? EmployeeInsurance { get; set; }
    }
    public class EmployeeInsurance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        //[ForeignKey("EmployeeId")]
        //[JsonIgnore]
        //public virtual Employee? Employee { get; set; }

        public string Name { get; set; } = string.Empty;

        public Relation Relation { get; set; }

        public DateTime DOB { get; set; } 


        //[RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Invalid Aadhaar Number!")]
        public string Aadhaar { get; set; } = string.Empty;

        public int Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }

    public enum Relation
    {
        Father,
        Mother,
        Spouse,
        Son,
        Daughter,
        Me
    }
    public class EmployeeImport
    {
        public string CompanyCode { get; set; } = string.Empty;

        public string EmployeeId { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public EmployeeStatus Status { get; set; }

        public string EmailId { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string PermanentAddress { get; set; } = string.Empty;

        public string PCountry { get; set; } = string.Empty;

        public string PState { get; set; } = string.Empty;

        public string PPin { get; set; } = string.Empty;

        public string CurrentAddress { get; set; } = string.Empty;

        public string CCountry { get; set; } = string.Empty;

        public string CState { get; set; } = string.Empty;

        public string CPin { get; set; } = string.Empty;

        public string AadhaarNumber { get; set; } = string.Empty;

        public string PANNumber { get; set; } = string.Empty;

        public string UANNo { get; set; } = string.Empty;

        public string EmergencyContactNumber { get; set; } = string.Empty;

        public DateTime DateOfJoin { get; set; }

        public DateTime DateOfLeaving { get; set; }

        public string Designation { get; set; } = string.Empty;

        public string ReportingTo { get; set; } = string.Empty;

        public string fatherName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string EmpBankName { get; set; } = string.Empty;

        public string EmpAccountName { get; set; } = string.Empty;

        public string EmpBankAccNumber { get; set; } = string.Empty;

        public string EmpBankIfsc { get; set; } = string.Empty;

        public string? EmployeeCategory { get; set; }

        public Departments Department { get; set; }

        public string ValidationErrMsg { get; private set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;

        public EmployeeImport() { }

        public EmployeeImport(Employee o)
        {
            CompanyCode = o.CompanyCode;
            EmployeeId = o.EmployeeId;
            EmployeeName = o.EmployeeName;
            fatherName = o.fatherName;
            Status = o.Status;
            EmailId = o.EmailId;
            MobileNumber = o.MobileNumber;
            PermanentAddress = o.PermanentAddress;
            PState = o.PState;
            PCountry = o.PCountry;
            PPin = o.PPin;
            CurrentAddress = o.CurrentAddress;
            CState = o.CState;
            CCountry = o.CCountry;
            CPin = o.CPin;
            DateOfBirth = o.DateOfBirth;
            AadhaarNumber = o.AadhaarNumber;
            PANNumber = o.PANNumber;
            UANNo = o.UANNo;
            EmergencyContactNumber = o.EmergencyContactNumber;
            DateOfJoin = o.DateOfJoin;
            DateOfLeaving = o.DateOfLeaving;
            Designation = o.Designation;
            ReportingTo = o.ReportingTo;
            EmpBankName = o.EmpBankName;
            EmpAccountName = o.EmpAccountName;
            EmpBankAccNumber = o.EmpBankAccNumber;
            EmpBankIfsc = o.EmpBankIfsc;
            EmployeeCategory = o.EmployeeCategory;
            Department = (Departments)System.Enum.Parse(typeof(Departments), o.Department ?? string.Empty, true);

        }

        public void Validate()
        {
            ValidationErrMsg = string.Empty;

            if (string.IsNullOrEmpty(CompanyCode) || string.IsNullOrEmpty(EmployeeName)
                || string.IsNullOrEmpty(EmailId) || string.IsNullOrEmpty(MobileNumber))
            {
                ValidationErrMsg = "CompanyCode|Name|Email|Mobile are required fields. ";
            }

            bool isEmail = Regex.IsMatch(EmailId, ImportEmployeeConfig.EmailRegex);
            if (!isEmail)
            {
                ValidationErrMsg = "Invalid EmailId. ";
            }

            if (MobileNumber.Length < 10)
            {
                ValidationErrMsg = "Invalid Mobile number specified. ";
            }

            if (string.IsNullOrEmpty(MobileNumber))
            {
                ValidationErrMsg = "Employee Mobile number can't be empty. ";
            }
        }
    }

    
}
