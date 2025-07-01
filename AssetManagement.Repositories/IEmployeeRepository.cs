using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<PagedResult<Employee>> GetEmployeesAsync(int page, int pageSize);
        Task<ApiResponse<Employee>> UpsertEmployeeAsync(Employee data);
        Task<(bool, string)> DeleteEmployeeByIdAsync(int id);
    }
}
