using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface IDataRepository
    {
        Task<ApiResponse<Employee>> GetAllocationByEmail(string email);
        Task<ApiResponse<Employee>> GetEmployeeByEmail(string email);
        Task<ApiResponse<Employee>> UpdateEmployee(Employee data);
    }
}