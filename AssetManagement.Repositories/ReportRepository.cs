using AssetManagement.Dto;
using AssetManagement.Dto.Dashboard;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IAppRepository _appRepository;
        public ReportRepository(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public Task<IEnumerable<AllocationLog>> GetAllAllocationLog() => _appRepository.GetAllAllocationLog();
        public Task<List<Employee>> GetFilteredEmployeeReport(EmployeeFilterModel model) => _appRepository.GetFilteredEmployeeReport(model);
        public Task<List<Asset>> GetFilteredAssetReport(AssetFilterModel model) => _appRepository.GetFilteredAssetReport(model);
        public Task<ReportFilters> GetEmployeeReportFileters() => _appRepository.GetEmployeeReportFileters();
        public Task<AssetReportFilters> GetAssetReportFilters() => _appRepository.GetAssetReportFilters();
    }
}
