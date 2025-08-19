using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;

namespace AssetManagement.Repositories
{
    public interface IEmployeeOnboardingRepository
    {
        Task<EmployeeOnboardingDto> GetEmployeeOnboardingByKey(string key);
        Task<EmployeeOnboardingDto> UpsertConfirmOnboardingAsync(OnBoardingConfirmationDto data);
        Task<ApiResponse<EmployeeOnboardingDto>> UpsertEmployeeOnboarding(EmployeeOnboardingDto data);
        Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByKey(string key);
        Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataById(int id);
        Task<EmployeeOnboardingDto> ShareFormWithOnboardeeViaEmail(int id);
        Task<ApiResponse<EmployeeFilesMapping>> UpsertEmployeeFilesAsync(EmployeeFilesMapping data);
        Task<EmployeeFilesMapping> GetEmployeeFilesById(int id);
        Task<IEnumerable<EmployeeFilesMapping>> GetAllEmployeeFileMap();
    }
}
