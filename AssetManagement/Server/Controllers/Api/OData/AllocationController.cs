using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AssetManagement.Server.Controllers.Api.OData
{
   
    public class AllocationController : ODataController
    {
        public ILogger<AllocationController> Logger { get; }
        public AppDbContext DbContext { get; }
        public AllocationController(ILogger<AllocationController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery]
        [Authorize]
        public IQueryable<Allocation> Get()
        {
            var data = DbContext.Allocation.AsQueryable();
            return data;
        }
    }
}

