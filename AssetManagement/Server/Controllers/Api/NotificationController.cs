using AssetManagement.Dto.Models;
using AssetManagement.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;

        public NotificationController(NotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Notification>> Get() => await _service.GetNotifications();
    }
}
