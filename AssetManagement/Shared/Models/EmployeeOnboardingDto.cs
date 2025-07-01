using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models
{
    public class EmployeeOnboardingDto
    {
        public int Id { get; set; }
        public string? employeetype { get; set; }
        public string? Joinstatus { get; set; }
        public string? Reason { get; set; }
        
        [Required]
        public string CompanyCode { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string fatherName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ExternalEmailId { get; set; } = string.Empty;
        //public string ? HR { get; set; }

        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Invalid Mobile Number!")]
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string MobileNumber { get; set; } = string.Empty;

        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Emergency Mobile Number is Invalid!")]
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmergencyContactNumber { get; set; } = string.Empty;

        [RegularExpression(@"[0-9]{12}", ErrorMessage = "Invalid Adhar Number!")]
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string AadhaarNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;
     
        public string? EmployeeGraduation { get; set; }
        //public string? vendorName { get; set; }
        //public string? vendorLocation { get; set; }
        //public string? DeputationCompanyStatus { get; set; }

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmployeeEducationDetails { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]", ErrorMessage = "Pan Number is Invalid")]
        public string PANNumber { get; set; } = string.Empty;

        //[RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string UANNo { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string PermanentAddress { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string PCountry { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string PState { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string PPin { get; set; } = string.Empty;

        public bool checkbox { get; set; }

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string CurrentAddress { get; set; } = string.Empty;
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string CCountry { get; set; } = string.Empty;
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string CState { get; set; } = string.Empty;
        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string CPin { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmpBankName { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmpAccountName { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmpBankAccNumber { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(SecurityStamp))]
        public string EmpBankIfsc { get; set; } = string.Empty;

        public string SecurityStamp { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ResponceDate { get; set; }

        [NotMapped]
        public string BaseUrl { get; set; } = string.Empty;
        public bool IsReplied { get; set; }

     
        public string AadhaarFile { get; set; } = string.Empty;
      
        public string PanFile { get; set; } = string.Empty;
      
        public string BankPassbookFile { get; set; } = string.Empty;
      
        public string CertificateFile { get; set; } = string.Empty;
       
        public string ProfilePhotoFile { get; set; } = string.Empty;
        public string ResumeFile { get; set; } = string.Empty;
        public string SkillIDs { get; set; } = string.Empty;
        
        [NotMapped]
        public DateTime DateOfJoin { get; set; } = DateTime.Now;
        [NotMapped]
        public string CompanyName { get; set; } = string.Empty;
        public string? OtherSkills { get; set; }

        public string Designation { get; set; } = string.Empty;
        
        public string ReportingTo { get; set; } = string.Empty;
        public string ? ManagerEmail { get; set; }
        public string ? Reference { get; set; }

        public string ManagerMobile { get; set; } = string.Empty;

        [Required]
        public DateTime TempDateOfJoin { get; set; } = DateTime.Now.AddDays(4);

         public string ? CreatedBy {  get; set; }
         public string ? UpdatedBy { get; set; } 
    }

    public class RequiredIfNotEmptyAttribute : RequiredAttribute
    {
        private readonly string _propertyName;

        public RequiredIfNotEmptyAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property != null)
            {
                var propertyValue = property.GetValue(validationContext.ObjectInstance);
                if (propertyValue != null && !string.IsNullOrEmpty(propertyValue.ToString()))
                {
                    return base.IsValid(value, validationContext);
                }
            }

            return ValidationResult.Success;
        }
    }
}