using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public interface IAllocationRepository
    {
        Task<Allocation> GetAllocationById(int id);
        Task<Allocation> GetAllocationByAssetId(int id);
        Task<bool> UnAllocation(int id);
        Task<IEnumerable<Allocation>> GetAllAllocation();
        Task<ApiResponse<Allocation>> UpsertAllocationAsync(Allocation data);
        Task<ApiResponse<Allocation>> EmployeeAllocationDetails(string key);
        Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data);
        Task<Allocation> ShareAllocationDetailsToEmployeeViaEmail(int id);
        Task<string> ReadAllocationCommentById(int id);
    }
}
