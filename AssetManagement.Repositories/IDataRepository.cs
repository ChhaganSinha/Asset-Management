using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface IDataRepository
    {
        Task<ApiResponse<List<Allocation>>> GetAllocationByEmail(string email);
        Task<ApiResponse<Employee>> GetEmployeeByEmail(string email);
        Task<ApiResponse<Employee>> UpdateEmployeeFromSP(Employee data);
        Task<ApiResponse<Allocation>> UpdateAllocationFromSP(Allocation data);
    }
}