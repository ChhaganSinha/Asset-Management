using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IAppRepository _appRepository;

        public AssetRepository(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public Task<Asset> GetAssetById(int id) => _appRepository.GetAssetById(id);
        public Task<IEnumerable<Asset>> GetAllAsset() => _appRepository.GetAllAsset();
        public Task<ApiResponse<Asset>> UpsertAssetAsync(Asset data) => _appRepository.UpsertAssetAsync(data);
        public Task<ApiResponse<List<AssetImport>>> UpsertImportAssetAsync(List<AssetImport> data) => _appRepository.UpsertImportAssetAsync(data);
        public Task<(bool, string)> DeleteAssetById(Asset data) => _appRepository.DeleteAssetById(data);
        public Task<AssetType> GetAssetTypeById(int id) => _appRepository.GetAssetTypeById(id);
        public Task<IEnumerable<AssetType>> GetAllAssetType() => _appRepository.GetAllAssetType();
        public Task<ApiResponse<AssetType>> UpsertAssetTypeAsync(AssetType data) => _appRepository.UpsertAssetTypeAsync(data);
    }
}
