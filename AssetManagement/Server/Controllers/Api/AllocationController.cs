using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using AssetManagement.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllocationController : ControllerBase
    {
        private readonly IAllocationRepository _allocationRepository;
        private readonly NotificationService _notificationService;
        public AllocationController(IAllocationRepository allocationRepository, NotificationService notificationService)
        {
            _allocationRepository = allocationRepository;
            _notificationService = notificationService;
        }

        [HttpGet("{id}")]
        public async Task<Allocation> GetAllocationById(int id) => await _allocationRepository.GetAllocationById(id);

        [HttpGet("GetAllocationByAssetId/{id}")]
        public async Task<Allocation> GetAllocationByAssetId(int id) => await _allocationRepository.GetAllocationByAssetId(id);

        [HttpDelete("UnAllocation/{id}")]
        public async Task<bool> UnAllocation(int id) => await _allocationRepository.UnAllocation(id);

        [HttpGet("all-Allocation")]
        public async Task<IEnumerable<Allocation>> GetAllAllocation() => await _allocationRepository.GetAllAllocation();

        [HttpPost("UpsertAllocation")]
        public async Task<ApiResponse<Allocation>> UpsertAllocation(Allocation data) => await _allocationRepository.UpsertAllocationAsync(data);

        [HttpPost("EmployeeAllocationDetails")]
        [AllowAnonymous]
        public async Task<ApiResponse<Allocation>> EmployeeAllocationDetails(GenericApiRequest<string> request) => await _allocationRepository.EmployeeAllocationDetails(request.Param);

        [HttpPost("EmployeeAllocationResponce")]
        [AllowAnonymous]
        public async Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data)
        {
            var result = await _allocationRepository.EmployeeAllocationResponce(data);
            if (result.IsSuccess)
            {
                var status = data.Responce switch
                {
                    Responce.Approved => "approved",
                    Responce.Reject => "rejected",
                    _ => "updated"
                };
                await _notificationService.AddNotification($"Allocation {status} for {data.EmployeeName}");
            }
            return result;
        }

        [HttpGet("ShareAllocationDetailsToEmployeeViaEmail/{id}")]
        public async Task<Allocation> ShareAllocationDetailsToEmployeeViaEmail(int id) => await _allocationRepository.ShareAllocationDetailsToEmployeeViaEmail(id);

        [HttpGet("ReadAllocationCommentById/{id}")]
        public async Task<string> ReadAllocationCommentById(int id) => await _allocationRepository.ReadAllocationCommentById(id);
    }
}
