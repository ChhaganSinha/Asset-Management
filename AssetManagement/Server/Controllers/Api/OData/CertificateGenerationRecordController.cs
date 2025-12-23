using System.Linq;
using AssetManagement.DataContext;
using AssetManagement.Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Server.Controllers.Api.OData
{
    [Authorize]
    public class CertificateGenerationRecordController : ODataController
    {
        public CertificateGenerationRecordController(
            ILogger<CertificateGenerationRecordController> logger,
            AppDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        public ILogger<CertificateGenerationRecordController> Logger { get; }
        public AppDbContext DbContext { get; }

        [EnableQuery]
        public IQueryable<CertificateGenerationRecord> Get()
        {
            var data = from record in DbContext.CertificateGenerationRecords
                       join employee in DbContext.Employee on record.EmployeeId equals employee.Id into employeeGroup
                       from employee in employeeGroup.DefaultIfEmpty()
                       join company in DbContext.Company on record.CompanyId equals company.Id into companyGroup
                       from company in companyGroup.DefaultIfEmpty()
                       select new CertificateGenerationRecord
                       {
                           Id = record.Id,
                           CompanyId = record.CompanyId,
                           EmployeeId = record.EmployeeId,
                           TemplateName = record.TemplateName,
                           GeneratedFileName = record.GeneratedFileName,
                           GeneratedOn = record.GeneratedOn,
                           GeneratedBy = record.GeneratedBy,
                           EmployeeName = employee != null ? employee.EmployeeName : string.Empty,
                           CompanyCode = company != null ? company.CompanyCode : string.Empty
                       };

            return data;
        }
    }
}
