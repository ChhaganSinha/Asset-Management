using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;


namespace AssetManagement.Server.Controllers.Api.OData
{
    [Authorize]
    public class CompanyController : ODataController
    {
        public ILogger<CompanyController> Logger { get; }
        public AppDbContext DbContext { get; }
        public CompanyController(ILogger<CompanyController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery]

        public IQueryable<Company> Get()
        {
            var data = DbContext.Company.AsQueryable();
            return data;
        }
    }
}
