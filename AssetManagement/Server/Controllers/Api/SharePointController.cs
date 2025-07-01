using AssetManagement.Dto;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SharePointController : ControllerBase
    {
        private readonly SharePointService _sharePointService;

        public SharePointController(SharePointService sharePointService)
        {
            _sharePointService = sharePointService;
        }

        [HttpPost]
        [Route("updateListItem")]
        public async Task<SharepointListUpdate> UpdateListItem(SharepointListUpdate request)
        {

            //return await _sharePointService.UpdateListItem(request);
            return null;
        }

        [HttpPost]
        [Route("insertListItem")]
        public async Task<SharepointListUpdate> InsertListItem(SharepointListUpdate request)
        {
           // return await _sharePointService.GetListItemByEmail("chhagan.sinha@credentinfotech.com");
            return await _sharePointService.InsertOrUpdateListItemAsync(request);
        }       
        
        [HttpPost]
        [Route("getListItemByEmail")]
        public async Task<SharepointListUpdate> GetListItemByEmail(string Email)
        {
            return await _sharePointService.GetListItemByEmail(Email);
        }
    }
}

