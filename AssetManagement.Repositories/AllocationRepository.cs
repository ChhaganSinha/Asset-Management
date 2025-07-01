using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public class AllocationRepository : IAllocationRepository
    {
        private readonly IAppRepository _appRepository;
        public AllocationRepository(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public Task<Allocation> GetAllocationById(int id) => _appRepository.GetAllocationById(id);
        public Task<Allocation> GetAllocationByAssetId(int id) => _appRepository.GetAllocationByAssetId(id);
        public Task<bool> UnAllocation(int id) => _appRepository.UnAllocation(id);
        public Task<IEnumerable<Allocation>> GetAllAllocation() => _appRepository.GetAllAllocation();
        public Task<ApiResponse<Allocation>> UpsertAllocationAsync(Allocation data) => _appRepository.UpsertAllocationAsync(data);
        public Task<ApiResponse<Allocation>> EmployeeAllocationDetails(string key) => _appRepository.EmployeeAllocationDetails(key);
        public Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data) => _appRepository.EmployeeAllocationResponce(data);
        public Task<Allocation> ShareAllocationDetailsToEmployeeViaEmail(int id) => _appRepository.ShareAllocationDetailsToEmployeeViaEmail(id);
        public Task<string> ReadAllocationCommentById(int id) => _appRepository.ReadAllocationCommentById(id);
    }
}
