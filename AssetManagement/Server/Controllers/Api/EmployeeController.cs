using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{id}")]
        public async Task<Employee> GetEmployeeById(int id) => await _employeeRepository.GetEmployeeByIdAsync(id);

        [HttpGet("all-Employee")]
        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            var result = await _employeeRepository.GetEmployeesAsync(1, int.MaxValue);
            return result.Items;
        }

        [HttpGet("EmployeePaged")]
        public async Task<PagedResult<Employee>> GetEmployeesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
            => await _employeeRepository.GetEmployeesAsync(page, pageSize);

        [HttpPost("UpsertEmployee")]
        public async Task<ApiResponse<Employee>> UpsertEmployee(Employee data) => await _employeeRepository.UpsertEmployeeAsync(data);

        [HttpPost("DeleteEmployee/{id}")]
        public async Task<(bool, string)> DeleteEmployeeById(int id) => await _employeeRepository.DeleteEmployeeByIdAsync(id);
    }
}
