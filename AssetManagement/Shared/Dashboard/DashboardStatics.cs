using AssetManagement.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Dashboard
{
    public class MasterStatics
    {
        public List<CompaniesStatics> CompaniesStatics { get; set; }
        public List<AssetsStatics> AssetsStatics { get; set; }
        public List<EmployeeStatics> EmployeeStatics { get; set; }
        public List<AllocationStatics> AllocationStatics { get; set; }
    }
    public class DashboardStatics
    {
        public List<string> CompanyCodes { get; set; } 
        public int Companies { get; set; }
        public int Employees { get; set; }
        public int Assets { get; set; }
        public int Allocations { get; set; }
    }

    public class CompaniesStatics
    {
        public Company company { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public int TotalAssets { get; set; }
        public int TotalAllocations { get; set; }
    }
    public class AssetsStatics
    {
        public List<Asset> asset { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int TotalAssets { get; set; }
        public int FreeAssets { get; set; }
        public int AllocatedAssets { get; set; }
        public int ActiveAssets { get; set; }
        public int DiscardedAssets { get; set; }
    }
    public class EmployeeStatics
    {
        public List<Employee> employee { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int TotalEmployee { get; set; }
        public List<EmployeeByDepartment> EmployeeByDepartment { get; set; }
        public List<EmployeeByStatus> EmployeeByStatus { get; set; }
    }
    public class EmployeeByDepartment
    {
        public string Department { get; set; }
        public int Count { get; set; }
    }
    public class EmployeeByStatus
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class AllocationStatics
    {
        public List<Allocation> allocation { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int TemporaryAlloction{ get; set; }
        public int PermanentAlloction{ get; set; }
    }
}
