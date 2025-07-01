using System;
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
    public class EmployeeOnboardingRepository : BaseRepository, IEmployeeOnboardingRepository
    {
        public AppDbContext AppDbCxt { get; set; }
        public EmployeeOnboardingRepository(ILogger<EmployeeOnboardingRepository> logger, AppDbContext appContext)
            : base(logger)
        {
            AppDbCxt = appContext;
        }

        public async Task<EmployeeOnboardingDto> GetEmployeeOnboardingById(int id)
        {
            try
            {
                return await AppDbCxt.EmployeeOnboarding.FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EmployeeOnboardingDto> GetEmployeeonboardingById(int id)
        {
            try
            {
                return await AppDbCxt.EmployeeOnboarding.FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EmployeeOnboardingDto> UpsertConfirmOnboardingAsync(OnBoardingConfirmationDto data)
        {
            try
            {
                if (data == null || data.Id <= 0)
                    return null;

                var result = await AppDbCxt.EmployeeOnboarding.FirstOrDefaultAsync(o => o.Id == data.Id);
                if (result != null)
                {
                    result.Joinstatus = string.IsNullOrWhiteSpace(data.JoinStatus) ? result.Joinstatus : data.JoinStatus;
                    result.TempDateOfJoin = data.TempDateOfJoin == DateTime.MinValue ? result.TempDateOfJoin : data.TempDateOfJoin;
                    result.Reason = string.IsNullOrWhiteSpace(data.Reason) ? result.Reason : data.Reason;
                    AppDbCxt.EmployeeOnboarding.Update(result);
                    await AppDbCxt.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponse<EmployeeOnboardingDto>> UpsertEmployeeOnboarding(EmployeeOnboardingDto data)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                if (data.Id > 0)
                {
                    data.IsReplied = true;
                    data.ResponceDate = DateTime.Now;
                    AppDbCxt.EmployeeOnboarding.Update(data);
                    await AppDbCxt.SaveChangesAsync();
                }
                else
                {
                    var check = AppDbCxt.EmployeeOnboarding.FirstOrDefault(o => o.ExternalEmailId == data.ExternalEmailId);
                    if (check != null)
                    {
                        result.IsSuccess = false;
                        result.Message = "Email already exist!";
                        return result;
                    }
                    else
                    {
                        var timestamp = DateTime.Now;
                        var key = $"{data.Id}-{data.ExternalEmailId}-{timestamp}".ComputeMd5Hash().ToLower();
                        var returnUrl = $"{data.BaseUrl}/EmployeeOnBoarding/{key}";
                        data.ReturnUrl = returnUrl;
                        data.SecurityStamp = key;
                        data.Date = DateTime.Now;
                        AppDbCxt.EmployeeOnboarding.Add(data);
                        await AppDbCxt.SaveChangesAsync();
                        data.ManagerMobile = AppDbCxt.Employee.Where(o => o.EmailId == data.ReportingTo).Select(o => o.MobileNumber).FirstOrDefault();
                        data.CompanyName = AppDbCxt.Company.Where(o => o.CompanyCode == data.CompanyCode).Select(o => o.Name).FirstOrDefault();
                        result.Result = data;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
            result.Result = data;
            result.IsSuccess = true;
            result.Message = "Successfully Added!";
            return result;
        }

        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByKey(string key)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                var data = AppDbCxt.EmployeeOnboarding.FirstOrDefault(o => o.SecurityStamp == key);
                if (data == null)
                {
                    result.Message = "You have already Updated your details";
                    return result;
                }
                result.Result = data;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByID(string key)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                var data = AppDbCxt.EmployeeOnboarding.FirstOrDefault(o => o.SecurityStamp == key);
                if (data == null)
                {
                    result.Message = "No Data Found in Database!";
                    return result;
                }
                result.Result = data;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ApiResponse<EmployeeFilesMapping>> UpsertEmployeeFilesAsync(EmployeeFilesMapping data)
        {
            var result = new ApiResponse<EmployeeFilesMapping>();
            var existingData = await AppDbCxt.EmployeeFilesMapping.FirstOrDefaultAsync(e => e.EmployeeId == data.EmployeeId);

            if (existingData != null)
            {
                if (!string.IsNullOrEmpty(data.AadhaarFile)) existingData.AadhaarFile = data.AadhaarFile;
                if (!string.IsNullOrEmpty(data.PanFile)) existingData.PanFile = data.PanFile;
                if (!string.IsNullOrEmpty(data.BankPassbookFile)) existingData.BankPassbookFile = data.BankPassbookFile;
                if (!string.IsNullOrEmpty(data.CertificateFile)) existingData.CertificateFile = data.CertificateFile;
                if (!string.IsNullOrEmpty(data.ProfilePhotoFile)) existingData.ProfilePhotoFile = data.ProfilePhotoFile;
                if (!string.IsNullOrEmpty(data.ResumeFile)) existingData.ResumeFile = data.ResumeFile;
                AppDbCxt.EmployeeFilesMapping.Update(existingData);
            }
            else
            {
                AppDbCxt.EmployeeFilesMapping.Add(data);
            }

            await AppDbCxt.SaveChangesAsync();
            return result;
        }

        public async Task<EmployeeFilesMapping> GetEmployeeFilesById(int id)
        {
            return AppDbCxt.EmployeeFilesMapping.FirstOrDefault(o => o.EmployeeId == id);
        }

        public async Task<IEnumerable<EmployeeFilesMapping>> GetAllEmployeeFileMap()
        {
            try
            {
                return AppDbCxt.EmployeeFilesMapping.ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<EmployeeOnboardingDto> ShareFormWithOnboardeeViaEmail(int id)
        {
            EmployeeOnboardingDto result = null;
            result = await AppDbCxt.EmployeeOnboarding.FindAsync(id);
            if (result != null)
            {
                result.IsReplied = false;
                AppDbCxt.EmployeeOnboarding.Update(result);
                AppDbCxt.SaveChanges();
                result.CompanyName = await AppDbCxt.Company.Where(o => o.CompanyCode == result.CompanyCode).Select(o => o.Name).FirstOrDefaultAsync();
            }
            return result;
        }
    }
}
