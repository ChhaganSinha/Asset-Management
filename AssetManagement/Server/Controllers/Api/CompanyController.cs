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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet("{id}")]
        public async Task<Company> GetCompanyById(int id) => await _companyRepository.GetCompanyByIdAsync(id);

        [HttpGet("all-Company")]
        public async Task<IEnumerable<Company>> GetAllCompany() => await _companyRepository.GetAllCompanyAsync();

        [HttpPost("UpsertCompany")]
        public async Task<ApiResponse<Company>> UpsertCompany(Company data) => await _companyRepository.UpsertCompanyAsync(data);

        [HttpPost("DeleteCompany")]
        public async Task<(bool, string)> DeleteCompanyById(Company data) => await _companyRepository.DeleteCompanyByIdAsync(data);

        [HttpPost("UpsertSubOffice")]
        public async Task<List<SubOffice>> UpsertSubOffice(List<SubOffice> data) => await _companyRepository.UpsertSubOfficeAsync(data);

        [HttpGet("SubOffice-by-id/{id}")]
        public async Task<IEnumerable<SubOffice>> GetSubOfficeById(int id) => await _companyRepository.GetSubOfficeByIdAsync(id);
    }
}
