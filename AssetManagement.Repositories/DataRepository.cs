using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ApiResponse<Employee>> GetAllocationByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<Employee>> GetEmployeeByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<Employee>> UpdateEmployee(Employee data)
        {
            throw new NotImplementedException();
        }
    }
}
