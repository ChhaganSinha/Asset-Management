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
    public class ReportController : ODataController
    {
        public ILogger<ReportController> Logger { get; }
        public AppDbContext DbContext { get; }
        public ReportController(ILogger<ReportController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery]

        public IQueryable<AllocationLog> Get()
        {
            var data = DbContext.AllocationLog.AsQueryable();
            return data;
        }
    }
}
