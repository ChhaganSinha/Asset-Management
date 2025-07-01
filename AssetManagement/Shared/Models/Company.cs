using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class Company : BaseEntity
    {
        [Required]
        [RegularExpression(@".{2,}", ErrorMessage = "Company ID is Invalid")]
        public string CompanyCode { get; set; }
        public string EmployeeIdPrefix { get; set; }
        [Required]
        [RegularExpression(@".{2,}", ErrorMessage = "Company Name is Invalid")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"[0-9]{2}[A-Z]{4}.{1}[0-9]{4}[A-Z].{3}", ErrorMessage = "invalid GST Number!")]
        public string GSTNNumber { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string PIN { get; set; } = string.Empty;
        [Required]
        public string BankName { get; set; } = string.Empty;
        [Required]
        public string AccNumber { get; set; } = string.Empty;
        [Required]
        public string IFSC { get; set; } = string.Empty;
        [Required]
        public string SwiftCode { get; set; } = string.Empty;

        [Required]
        public string IncorporationNumber { get; set; } = string.Empty;
        [Required]
        public DateTime IncorporationDate { get; set; } = DateTime.Now;
        [Required]
        public string TANNumber { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]", ErrorMessage = "Pan Number is Invalid")]
        public string PanNumber { get; set; } = string.Empty;

        public string PFNumber { get; set; } = string.Empty;
        public string ESINumber { get; set; } = string.Empty;

        [RequiredIf(nameof(RegistrationWith), RegistrationWith.MSME)]
        public string UAMNumber { get; set; } = string.Empty;
        [RequiredIf(nameof(RegistrationWith), RegistrationWith.StartUp)]
        public string StartupNumber { get; set; } = string.Empty;

        public string BankDetail { get; set; } = string.Empty;
        public RegistrationWith RegistrationWith { get; set; }
        [Required]
        public string ContactPerson { get; set; } = string.Empty;
        [Required]
        public string ContactNumber { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string EmailId { get; set; } = string.Empty;
        public bool IsEngazed { get; set; }
        public int EmployeeEngazedCount { get; set; } = 0;
        public int AssetEngazedCount { get; set; } = 0;


        //public Employee Employee { get; set; }
        //public Asset Asset { get; set; }

    }


    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object[] _propertyValues;

        public RequiredIfAttribute(string propertyName, params object[] propertyValues)
        {
            _propertyName = propertyName;
            _propertyValues = propertyValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_propertyName}", new[] { validationContext.MemberName });
            }

            var propertyValue = property.GetValue(validationContext.ObjectInstance, null);
            if (propertyValue == null || !_propertyValues.Contains(propertyValue))
            {
                return ValidationResult.Success;
            }

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }

}