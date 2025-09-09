using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        public AppDbContext AppDbCxt { get; set; }
        public DataRepository(ILogger<DataRepository> logger, AppDbContext appContext) : base(logger)
        {
            AppDbCxt = appContext;
        }

        public async Task<ApiResponse<Allocation>> GetAllocationByEmail(string email)
        {
            var response = new ApiResponse<Allocation>();
            try
            {
                var allocation = await AppDbCxt.Allocation.AsNoTracking().FirstOrDefaultAsync(o => o.EmployeeEmail.ToLower() == email.ToLower());
                response.IsSuccess = true;
                response.Result = allocation;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<Employee>> GetEmployeeByEmail(string email)
        {
            var response = new ApiResponse<Employee>();
            try
            {
                var employee = await AppDbCxt.Employee.AsNoTracking().FirstOrDefaultAsync(o => o.EmailId.ToLower() == email.ToLower());
                response.IsSuccess = true;
                response.Result = employee;
            }
            catch (Exception ex) 
            { 
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ApiResponse<Employee>> UpdateEmployee(Employee data)
        {
            throw new NotImplementedException();
        }
    }
}
