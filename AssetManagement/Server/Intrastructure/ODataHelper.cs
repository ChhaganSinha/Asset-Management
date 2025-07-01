
namespace AssetManagement.Server.Intrastructure
{
    using Microsoft.AspNetCore.OData;
    using Microsoft.OData.Edm;
    using Microsoft.OData.ModelBuilder;
    using AssetManagement.Dto;
    using AssetManagement.Dto.Models;
    using Microsoft.AspNetCore.Authorization;
    public static class ODataHelper
    {
        public static IMvcBuilder AddODataControllers(this IMvcBuilder builder)
        {
            return builder.AddOData(option =>
            {
                option.Select();
                option.Expand();
                option.Filter();
                option.OrderBy();
                option.Count();
                option.SetMaxTop(100);
                option.SkipToken();
                option.AddRouteComponents("Odata", GetModel());
            });
        }
        [Authorize]
        private static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Details>("Details");
            builder.EntitySet<Company>("Company");
            builder.EntitySet<Employee>("Employee");
            builder.EntitySet<Asset>("Asset");
            builder.EntitySet<Allocation>("Allocation");
            builder.EntitySet<AllocationLog>("Report");
            builder.EntitySet<EmployeeOnboardingDto>("EmployeeOnboarding");
            return builder.GetEdmModel();
        }
    }

    
}
