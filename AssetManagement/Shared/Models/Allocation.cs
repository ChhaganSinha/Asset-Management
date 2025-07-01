using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models
{
    public class Allocation : IValidatableObject
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required]
        public AllocationType AllocationType { get; set; }

        [IssueTillRequiredIfTempory]
        public DateTime ? IssueTill { get; set; } 

        public DateTime? ReturnDate { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public string? CompanyCode { get; set; }

        [Required]
        public string? AssetType { get; set; }

        [Required]
        public int? AssetId { get; set; }
        public string? AssetModel { get; set; }

        [Required]
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;
        public string ?EmployeeCompanyCode { get; set; }

        public bool Checkbox { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public Responce Responce { get; set; }

        public string Comment { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
        [NotMapped]
        public string BaseUrl { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AllocationType == AllocationType.Temporary && IssueTill == null)
            {
                yield return new ValidationResult("This field is required for temporary allocation", new[] { nameof(IssueTill) });
            }
        }
    }

    public class IssueTillRequiredIfTemporyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var allocation = (Allocation)validationContext.ObjectInstance;
            if (allocation.AllocationType == AllocationType.Temporary && value == null)
            {
                return new ValidationResult("This field is required for temporary allocation", new[] { nameof(allocation.IssueTill) });
            }
            return ValidationResult.Success;
        }
    }

    public enum Responce
    {       
        Approved = 1,
        Reject,
        Pending,
        NotShared
    }
}
