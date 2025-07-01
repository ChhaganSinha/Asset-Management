
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
    public class EmployeeOnboardingController : ODataController
    {
        public ILogger<EmployeeOnboardingController> Logger { get; }
        public AppDbContext DbContext { get; }
        public EmployeeOnboardingController(ILogger<EmployeeOnboardingController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery]

        public IQueryable<EmployeeOnboardingDto> Get()
        {
            var data = DbContext.EmployeeOnboarding.AsQueryable();
            return data;
        }
    }
}


