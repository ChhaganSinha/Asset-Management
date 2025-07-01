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
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        public AssetController(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        [HttpGet("{id}")]
        public async Task<Asset> GetAssetById(int id) => await _assetRepository.GetAssetById(id);

        [HttpGet("all-Asset")]
        public async Task<IEnumerable<Asset>> GetAllAsset() => await _assetRepository.GetAllAsset();

        [HttpPost("UpsertAsset")]
        public async Task<ApiResponse<Asset>> UpsertAsset(Asset data) => await _assetRepository.UpsertAssetAsync(data);

        [HttpPost("UpsertImportAssets")]
        public async Task<ApiResponse<List<AssetImport>>> UpsertImportAsset(List<AssetImport> data) => await _assetRepository.UpsertImportAssetAsync(data);

        [HttpPost("DeleteAsset")]
        public async Task<(bool, string)> DeleteAssetById(Asset data) => await _assetRepository.DeleteAssetById(data);

        [HttpGet("AssetType/{id}")]
        public async Task<AssetType> GetAssetTypeById(int id) => await _assetRepository.GetAssetTypeById(id);

        [HttpGet("all-AssetType")]
        public async Task<IEnumerable<AssetType>> GetAllAssetType() => await _assetRepository.GetAllAssetType();

        [HttpPost("UpsertAssetType")]
        public async Task<ApiResponse<AssetType>> UpsertAssetType(AssetType data) => await _assetRepository.UpsertAssetTypeAsync(data);
    }
}
