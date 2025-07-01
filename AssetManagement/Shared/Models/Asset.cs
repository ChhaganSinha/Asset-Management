using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace AssetManagement.Dto.Models;

public class Asset
{
    public int Id { get; set; }
   
    public int CompanyId { get; set; }
    [Required]
    public string CompanyCode { get; set; } = string.Empty;

    //[Required]
    public int? AssetTypeId { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string? SubAssetType { get; set; }
    //[Required]
    public string Brand { get; set; } = string.Empty;
    //[Required]
    public string Model { get; set; } = string.Empty;
    //[Required]
    public string Description { get; set; } = string.Empty;
    //[Required]
    public string SerialNumber { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    //[Required]
    public DateTime AcquireDate { get; set; } = DateTime.Now;
    //[Required]
    public string VendorName { get; set; } = string.Empty;
    public DateTime? DiscardDate { get; set; }
    public AllocationStatus Status { get; set; }
    public bool IsEngazed { get; set; }
    public AssetStatus _AssetStatus { get; set; } = AssetStatus.Active;

    public string EmployeeId {  get; set; } = string.Empty;
    public string EmployeeEmail {  get; set; } = string.Empty;
    public string EmployeeName {  get; set; } = string.Empty;
   
    [ForeignKey("CompanyId")]
    public virtual Company? Company { get; set; }
}
public enum AssetStatus
{
    Active = 0,
    Discarded
}
public class AssetImport
{
    public int CompanyId { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public int AssetTypeId { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string? SubAssetType { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public DateTime AcquireDate { get; set; } = DateTime.Now;
    public string VendorName { get; set; } = string.Empty;
    public DateTime? DiscardDate { get; set; }
    public AllocationStatus Status { get; set; }
}