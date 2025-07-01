using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Repositories
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(ILogger<CompanyRepository> logger, AppDbContext context) : base(logger)
        {
            _context = context;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Company.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Company>> GetAllCompanyAsync()
        {
            return await _context.Company.AsNoTracking().ToListAsync();
        }

        public async Task<ApiResponse<Company>> UpsertCompanyAsync(Company data)
        {
            var result = new ApiResponse<Company>();
            try
            {
                if (data.Id > 0)
                {
                    _context.Company.Update(data);
                }
                else
                {
                    var companies = await _context.Company.ToListAsync();
                    if (companies.Any(o => o.PanNumber == data.PanNumber && o.CompanyCode != data.CompanyCode) && !string.IsNullOrEmpty(data.PanNumber))
                    {
                        result.IsSuccess = false;
                        result.Message = "Duplicate PAN Number found!";
                        return result;
                    }
                    if (companies.Any(o => o.GSTNNumber == data.GSTNNumber && o.CompanyCode != data.CompanyCode) && !string.IsNullOrEmpty(data.GSTNNumber))
                    {
                        result.IsSuccess = false;
                        result.Message = "Duplicate GST Number found!";
                        return result;
                    }
                    if (companies.Any(o => o.TANNumber == data.TANNumber && o.CompanyCode != data.CompanyCode) && !string.IsNullOrEmpty(data.TANNumber))
                    {
                        result.IsSuccess = false;
                        result.Message = "Duplicate TAN Number found!";
                        return result;
                    }
                    if (companies.Any(o => o.IncorporationNumber == data.IncorporationNumber && o.CompanyCode != data.CompanyCode) && !string.IsNullOrEmpty(data.IncorporationNumber))
                    {
                        result.IsSuccess = false;
                        result.Message = "Duplicate Incorporation Number found!";
                        return result;
                    }
                    var existingCompany = companies.FirstOrDefault(c => c.CompanyCode == data.CompanyCode);
                    if (existingCompany != null)
                    {
                        result.Message = $"Duplicate company code: {data.CompanyCode}";
                        return result;
                    }
                    _context.Company.Add(data);
                }

                await _context.SaveChangesAsync();
                result.Result = data;
                result.IsSuccess = true;
            }
            catch (System.Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<(bool, string)> DeleteCompanyByIdAsync(Company data)
        {
            var existing = await _context.Company.FindAsync(data.Id);
            if (existing != null)
            {
                if (existing.EmployeeEngazedCount > 0 && existing.AssetEngazedCount > 0)
                {
                    return (false, "Company can't be delete, beacuse it has Assets and Employees!");
                }
                else if (existing.AssetEngazedCount > 0)
                {
                    return (false, "Company can't be delete, beacuse it has Assets!");
                }
                else if (existing.EmployeeEngazedCount > 0)
                {
                    return (false, "Company can't be delete, beacuse it has Employees!");
                }
                else
                {
                    _context.Company.Remove(existing);
                    await _context.SaveChangesAsync();
                    return (true, "Company deleted successfully.");
                }
            }
            return (false, "Company not found.");
        }

        public async Task<List<SubOffice>> UpsertSubOfficeAsync(List<SubOffice> dtos)
        {
            if (dtos == null || !dtos.Any())
                throw new System.ArgumentNullException(nameof(dtos), "Invalid SubOffice data");

            var existingQuestions = _context.SubOffice.Include(tq => tq.Zoffice)
                .Where(tq => tq.CompanyId == dtos.First().CompanyId).ToList();
            foreach (var existingQuestion in existingQuestions)
            {
                if (!dtos.Any(dto => dto.Id == existingQuestion.Id))
                {
                    _context.SubOffice.Remove(existingQuestion);
                }
            }
            foreach (var dto in dtos)
            {
                if (dto.Id > 0)
                {
                    var existingSubOffice = await _context.SubOffice.Include(tq => tq.Zoffice)
                        .FirstOrDefaultAsync(tq => tq.Id == dto.Id);
                    if (existingSubOffice == null)
                        throw new System.ArgumentException($"No SubOffice found with ID {dto.Id}", nameof(dtos));

                    existingSubOffice.CompanyId = dto.CompanyId;
                    existingSubOffice.Number = dto.Number;
                    existingSubOffice.subName = dto.subName;
                    existingSubOffice.subAddress = dto.subAddress;
                    existingSubOffice.subCont = dto.subCont;
                    existingSubOffice.subContPerson = dto.subContPerson;
                    existingSubOffice.SubOficeZone = dto.SubOficeZone;

                    foreach (var answer in existingSubOffice.Zoffice.ToList())
                    {
                        if (!dto.Zoffice.Any(a => a.Id == answer.Id))
                        {
                            _context.Zoffice.Remove(answer);
                        }
                    }

                    foreach (var ZofficeDto in dto.Zoffice)
                    {
                        var existingZonalOffice = existingSubOffice.Zoffice
                            .FirstOrDefault(a => a.Id == ZofficeDto.Id);
                        if (existingZonalOffice == null)
                        {
                            var newAnswer = new Zoffice
                            {
                                SubOfficeId = existingSubOffice.Id,
                                Number = ZofficeDto.Number,
                                zoName = ZofficeDto.zoName,
                                zoAddress = ZofficeDto.zoAddress,
                                zoCont = ZofficeDto.zoCont,
                                zoContPerson = ZofficeDto.zoContPerson
                            };
                            _context.Zoffice.Add(newAnswer);
                        }
                        else
                        {
                            existingZonalOffice.Number = ZofficeDto.Number;
                            existingZonalOffice.zoName = ZofficeDto.zoName;
                            existingZonalOffice.zoAddress = ZofficeDto.zoAddress;
                            existingZonalOffice.zoCont = ZofficeDto.zoCont;
                            existingZonalOffice.zoContPerson = ZofficeDto.zoContPerson;
                        }
                    }
                }
                else
                {
                    var newSubOffice = new SubOffice
                    {
                        CompanyId = dto.CompanyId,
                        Number = dto.Number,
                        subName = dto.subName,
                        subAddress = dto.subAddress,
                        subCont = dto.subCont,
                        subContPerson = dto.subContPerson,
                        SubOficeZone = dto.SubOficeZone
                    };
                    _context.SubOffice.Add(newSubOffice);
                    await _context.SaveChangesAsync();

                    foreach (var ZofficeDto in dto.Zoffice)
                    {
                        var newZoffice = new Zoffice
                        {
                            SubOfficeId = newSubOffice.Id,
                            Number = ZofficeDto.Number,
                            zoName = ZofficeDto.zoName,
                            zoAddress = ZofficeDto.zoAddress,
                            zoCont = ZofficeDto.zoCont,
                            zoContPerson = ZofficeDto.zoContPerson
                        };
                        _context.Zoffice.Add(newZoffice);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return dtos;
        }

        public async Task<IEnumerable<SubOffice>> GetSubOfficeByIdAsync(int id)
        {
            return _context.SubOffice.Where(o => o.CompanyId == id).Include(o => o.Zoffice).OrderBy(o => o.Number);
        }
    }
}
