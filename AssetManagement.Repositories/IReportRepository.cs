using AssetManagement.Dto;
using AssetManagement.Dto.Dashboard;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<AllocationLog>> GetAllAllocationLog();
        Task<List<Employee>> GetFilteredEmployeeReport(EmployeeFilterModel model);
        Task<List<Asset>> GetFilteredAssetReport(AssetFilterModel model);
        Task<ReportFilters> GetEmployeeReportFileters();
        Task<AssetReportFilters> GetAssetReportFilters();
    }
}
