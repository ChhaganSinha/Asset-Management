
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models;

public class AllocationLog
{
    public int Id { get; set; }
    public int AllocationId { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.Now;
    public AllocationType AllocationType { get; set; }
    public DateTime? IssueTill { get; set; }
    public DateTime? ReturnDate { get; set; }
    [NotMapped]
    public CurrentAllocationStatus AllocationStatus { get; set; } = CurrentAllocationStatus.Allocated;

    public int? CompanyId { get; set; }
    public string? CompanyCode { get; set; }
    public string? CompanyName { get; set; }


    public int? AssetId { get; set; }
    public string? AssetType { get; set; }
    public string? AssetBrand { get; set; }
    public string? AssetModel { get; set; }
    public string? AssetDescription { get; set; }
    public DateTime AssetAquireDate { get; set; }


    public int? EmployeeId { get; set; }
    public string? EmployeeCompanyCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeEmail { get; set; }
    public string? EmployeeMobileNumber { get; set; }
    public string? EmployeeDesignation { get; set; }
    public DateTime EmployeeDateOfJoin { get; set; }
    public DateTime EmployeeDateOfleaving { get; set; }


    //[ForeignKey("AssetId")]
    //public virtual Asset? Asset { get; set; }

    //[ForeignKey("NewEmployeeId")]
    //public virtual Employee? Employee { get; set; }

    //[ForeignKey("CompanyId")]
    //public virtual Company? Company { get; set; }
}

public enum CurrentAllocationStatus
{
    Allocated = 1,
    Returned

}