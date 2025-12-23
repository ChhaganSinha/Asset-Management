using System.Threading.Tasks;
using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Repositories
{
    public class CertificateRepository : BaseRepository, ICertificateRepository
    {
        private readonly AppDbContext _context;

        public CertificateRepository(ILogger<CertificateRepository> logger, AppDbContext context) : base(logger)
        {
            _context = context;
        }

        public async Task<ApiResponse<CertificateGenerationRecord>> AddCertificateRecordAsync(CertificateGenerationRecord record)
        {
            var result = new ApiResponse<CertificateGenerationRecord>();
            try
            {
                _context.CertificateGenerationRecords.Add(record);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Result = record;
                result.Message = "Certificate record saved.";
            }
            catch (System.Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
