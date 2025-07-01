using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class AllocationlogReportGenerate
    {
        public bool CompanyCode { get; set; }
        public List<string>? CompanyCodeSelected { get; set; }
        public bool AssetType { get; set; }
        public List<string>? AssetTypeSelected { get; set; }
        public bool Brand { get; set; }
        public bool Model { get; set; }
        public bool Description { get; set; }
        public bool EmployeeName { get; set; }
        public bool EmployeeCompanyCode { get; set; }
        public bool EmployeeEmail { get; set; }
        public bool EmployeeMobileNumber { get; set; }
        public bool EmployeeDesignation { get; set; }
        public bool AllocationType { get; set; }
        public string? AllocationTypeSelected { get; set; }
        public bool IssueDate { get; set; }
        public DateTime IssueDateRangeStart { get; set; }
        public DateTime IssueDateRangedEnd { get; set; }
        public bool IssueTill { get; set; }
        public DateTime IssueTillRangeStart { get; set; }
        public DateTime IssueTillRangeEnd { get; set; }
        public bool ReturnDate { get; set; }
        public DateTime ReturnDateRangeStart { get; set; }
        public DateTime ReturnDateRangeEnd { get; set; }
    }
}
