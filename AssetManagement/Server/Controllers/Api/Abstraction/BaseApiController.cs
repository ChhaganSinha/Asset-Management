using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Server.Controllers.Api.Abstraction
{
    public class BaseApiController : ControllerBase
    {
        public BaseApiController(ILogger logger) : base()
        {
            Logger = logger;
        }

        public ILogger Logger { get; }
    }

}
