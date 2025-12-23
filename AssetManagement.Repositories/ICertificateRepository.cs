using System.Threading.Tasks;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface ICertificateRepository
    {
        Task<ApiResponse<CertificateGenerationRecord>> AddCertificateRecordAsync(CertificateGenerationRecord record);
    }
}
