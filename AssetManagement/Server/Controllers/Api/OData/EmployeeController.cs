using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Server.Controllers.Api.OData
{
    [Authorize]
    public class EmployeeController : ODataController
    {
        public ILogger<EmployeeController> Logger { get; }
        public AppDbContext DbContext { get; }
        public EmployeeController(ILogger<EmployeeController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery(MaxNodeCount = 1000)]
        public IQueryable<Employee> Get()
        {
            var data = DbContext.Employee.AsQueryable();
            return data;
        }


    }
}
