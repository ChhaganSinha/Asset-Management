using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public interface IAssetRepository
    {
        Task<Asset> GetAssetById(int id);
        Task<IEnumerable<Asset>> GetAllAsset();
        Task<ApiResponse<Asset>> UpsertAssetAsync(Asset data);
        Task<ApiResponse<List<AssetImport>>> UpsertImportAssetAsync(List<AssetImport> data);
        Task<(bool, string)> DeleteAssetById(Asset data);
        Task<AssetType> GetAssetTypeById(int id);
        Task<IEnumerable<AssetType>> GetAllAssetType();
        Task<ApiResponse<AssetType>> UpsertAssetTypeAsync(AssetType data);
    }
}
