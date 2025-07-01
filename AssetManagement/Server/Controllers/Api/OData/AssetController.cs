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
    public class AssetController : ODataController
    {
        public ILogger<AssetController> Logger { get; }
        public AppDbContext DbContext { get; }
        public AssetController(ILogger<AssetController> logger, AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [EnableQuery]
        public IQueryable<Asset> Get()
        {
            var data = from ast in DbContext.Asset
                       join alloc in DbContext.Allocation on ast.Id equals alloc.AssetId into allocGroup
                       from alloc in allocGroup.DefaultIfEmpty() // Left join for Allocation
                       join emp in DbContext.Employee on alloc.EmployeeId equals emp.Id into empGroup
                       from emp in empGroup.DefaultIfEmpty() // Left join for Employee
                       select new Asset
                       {
                           Id = ast.Id,
                           CompanyId = ast.CompanyId,
                           CompanyCode = ast.CompanyCode,
                           AssetTypeId = ast.AssetTypeId,
                           AssetType = ast.AssetType,
                           SubAssetType = ast.SubAssetType,
                           Brand = ast.Brand,
                           Model = ast.Model,
                           SerialNumber = ast.SerialNumber,
                           Description = ast.Description,
                           MacAddress = ast.MacAddress,
                           AcquireDate = ast.AcquireDate,
                           VendorName = ast.VendorName,
                           DiscardDate = ast.DiscardDate,
                           Status = ast.Status,
                           IsEngazed = ast.IsEngazed,
                           _AssetStatus = ast._AssetStatus,
                           // If there's no employee or allocation, show "Unassigned"
                           EmployeeName = alloc != null && emp != null ? $"{emp.EmployeeName} ({emp.EmailId})" : "Unassigned"
                       };

            return data; // Return the IQueryable without calling AsQueryable again
        }



    }
}
