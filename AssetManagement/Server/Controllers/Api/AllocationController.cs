using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
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
        public AllocationController(IAllocationRepository allocationRepository)
        {
            _allocationRepository = allocationRepository;
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
        public async Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data) => await _allocationRepository.EmployeeAllocationResponce(data);

        [HttpGet("ShareAllocationDetailsToEmployeeViaEmail/{id}")]
        public async Task<Allocation> ShareAllocationDetailsToEmployeeViaEmail(int id) => await _allocationRepository.ShareAllocationDetailsToEmployeeViaEmail(id);

        [HttpGet("ReadAllocationCommentById/{id}")]
        public async Task<string> ReadAllocationCommentById(int id) => await _allocationRepository.ReadAllocationCommentById(id);
    }
}
