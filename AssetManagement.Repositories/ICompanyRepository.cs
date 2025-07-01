using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByIdAsync(int id);
        Task<IEnumerable<Company>> GetAllCompanyAsync();
        Task<ApiResponse<Company>> UpsertCompanyAsync(Company data);
        Task<(bool, string)> DeleteCompanyByIdAsync(Company data);
        Task<List<SubOffice>> UpsertSubOfficeAsync(List<SubOffice> data);
        Task<IEnumerable<SubOffice>> GetSubOfficeByIdAsync(int id);
    }
}
