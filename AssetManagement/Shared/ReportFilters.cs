using AssetManagement.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto
{
    public class ReportFilters
    {
        public IEnumerable<string>? Managers { get; set; }
        public IEnumerable<string>? Designations { get; set; }
        public IEnumerable<string>? Locations { get; set; }
        public IEnumerable<string>? Branches { get; set; }
    }

    public class AssetReportFilters
    {
        public IEnumerable<assetType>? AssetType { get; set; }
        public IEnumerable<brand>? Brand { get; set; }
        public IEnumerable<model>? Model { get; set; }
    }
    public class assetType
    {
        public string CompanyCode { get; set; }
        public string AssetTypeName { get; set; }
    }
    public class brand
    {
        public string AssetType { get; set; }
        public string BrandName { get; set; }
    }

    public class model
    {
        public string Brand { get; set; }
        public string ModelName { get; set; }
    }

    public class EmployeeFilterModel
    {
        public string ? Company { get; set; }
        public string ? Designation { get; set; }
        public string ? ManagerName { get; set; }
        public string ? BranchOffice { get; set; }
        public string ? Location { get; set; }
        public string ? EmployeeStatus { get; set; }
        public IEnumerable<string> ? Skills { get; set; }
        public DateTime JoinDateStart { get; set; }
        public DateTime JoinDateEnd { get; set; }
        public DateTime ResignDateStart { get; set; }
        public DateTime ResignDateEnd { get; set; }

        //public int Age { get; set; }
        //public bool MaleFemale { get; set; }
        //public  bool isMarried { get; set; }

    }

    public class AssetFilterModel
    {
        public string? Company { get; set; }
        public string? AssetType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public AllocationStatus ? Status { get; set; }
        public DateTime AquireDateStart { get; set; }
        public DateTime AquireDateEnd { get; set; }
        public DateTime DiscardDateStart { get; set; }
        public DateTime DiscardDateEnd { get; set; }
    }
}
