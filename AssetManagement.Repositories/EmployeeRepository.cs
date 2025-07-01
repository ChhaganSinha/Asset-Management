using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, AppDbContext context) : base(logger)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employee.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedResult<Employee>> GetEmployeesAsync(int page, int pageSize)
        {
            var query = _context.Employee.AsNoTracking();
            var total = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Employee>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ApiResponse<Employee>> UpsertEmployeeAsync(Employee data)
        {
            var result = new ApiResponse<Employee>();
            try
            {
                if (data.Id > 0)
                {
                    _context.Employee.Update(data);
                    result.Message = "Data Successfully Updated";
                }
                else
                {
                    _context.Employee.Add(data);
                    result.Message = "Data Successfully Added";
                }
                await _context.SaveChangesAsync();
                result.Result = data;
                result.IsSuccess = true;
            }
            catch (System.Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<(bool, string)> DeleteEmployeeByIdAsync(int id)
        {
            var existing = await _context.Employee.FindAsync(id);
            if (existing == null)
                return (false, "Employee not found.");

            _context.Employee.Remove(existing);
            try
            {
                await _context.SaveChangesAsync();
                return (true, "Employee deleted successfully");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
