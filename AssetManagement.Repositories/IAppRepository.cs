using AssetManagement.Dto;
using AssetManagement.Dto.Dashboard;
using AssetManagement.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Repositories
{
    public interface IAppRepository
    {
        Task<ApiResponse<Asset>> AssetTransfer(AssetTransferModel data);
        Task<ApiResponse<string>> ChangeReturnUrl();
        Task<(bool, string)> DeleteAssetById(Asset data);
        Task<(bool, string)> DeleteEmployeeById(int id);
        Task<(bool, string)> DeleteOnboardeeById(int id);
        Task<ApiResponse<UserProfilePicUpld>> DeleteProfBgPic(UserProfilePicUpld userProfilePicUpld);
        Task<ApiResponse<Allocation>> EmployeeAllocationDetails(string param);
        Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data);
        Task<ApiResponse<Employee>> EmployeeInsuranceByKey(string param);
        Task<ApiResponse<Employee>> EmployeeSeftUpdate(Employee data);
        Task<ApiResponse<Employee>> EmployeeSelfGetDetails(string param);
        Task<ApiResponse<Employee>> EmployeeTransfer(EmployeeTransferModel data);
        Task<IEnumerable<Allocation>> GetAllAllocation();
        Task<IEnumerable<AllocationLog>> GetAllAllocationLog();
        Task<IEnumerable<Asset>> GetAllAsset();
        Task<IEnumerable<AssetType>> GetAllAssetType();
        Task<IEnumerable<DesignationDTO>> GetAllDesignations();
        Task<IEnumerable<Employee>> GetAllEmployee();
        Task<IEnumerable<EmployeeFilesMapping>> GetAllEmployeeFileMap();
        Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkills();
        Task<Allocation> GetAllocationByAssetId(int id);
        Task<Allocation> GetAllocationById(int id);
        Task<IEnumerable<ZoneArea>> GetAllZoneArea();
        Task<Asset> GetAssetById(int id);
        Task<AssetReportFilters> GetAssetReportFilters();
        Task<AssetType> GetAssetTypeById(int id);
        Task<ApiResponse<EmployeePortalSPFX>> GetEmployeeByEmail(string? email);
        Task<Employee> GetEmployeeById(int id);
        Task<EmployeeFilesMapping> GetEmployeeFilesById(int id);
        Task<IEnumerable<EmployeeInsurance>> GetEmployeeInsuranceById(int id);
        Task<EmployeeOnboardingDto> GetEmployeeOnboardingById(int id);
        Task<EmployeeOnboardingDto> GetEmployeeonboardingById(int id);
        Task<ReportFilters> GetEmployeeReportFileters();
        Task<EmployeeSkills> GetEmployeeSkillsById(int id);
        Task<List<EmployeeSkillMapping>> GetEmployeeSkillsIDs(int employeeId);
        Task<List<Asset>> GetFilteredAssetReport(AssetFilterModel model);
        Task<List<Employee>> GetFilteredEmployeeReport(EmployeeFilterModel model);
        Task<DashboardStatics> GetLastStatics();
        Task<MasterStatics> GetMasterStaticsEntires();
        Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByID(string param);
        Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByKey(string param);
        Task<UserProfilePicUpld> GetProfilePicByEmail(string email);
        Task<ZoneArea> GetZoneAreaById(int id);
        Task<string> ReadAllocationCommentById(int id);
        Task<Allocation> ShareAllocationDetailsToEmployeeViaEmail(int id);
        Task<Employee> ShareFormWithEmployeeViaEmail(int id);
        Task<EmployeeOnboardingDto> ShareFormWithOnboardeeViaEmail(int id);
        Task<bool> UnAllocation(int id);
        Task<ApiResponse<EmployeePortalSPFX>> UpdateEmployeeByEmail(EmployeePortalSPFX employeePortalSPFX);
        Task<ApiResponse<Allocation>> UpsertAllocationAsync(Allocation data);
        Task<ApiResponse<Asset>> UpsertAssetAsync(Asset data);
        Task<ApiResponse<AssetType>> UpsertAssetTypeAsync(AssetType data);
        Task<EmployeeOnboardingDto> UpsertConfirmOnboardingAsync(OnBoardingConfirmationDto data);
        Task<List<DesignationDTO>> UpsertDesignation(List<DesignationDTO> details);
        Task<ApiResponse<Employee>> UpsertEmployeeAsync(Employee data);
        Task<ApiResponse<EmployeeFilesMapping>> UpsertEmployeeFilesAsync(EmployeeFilesMapping data);
        Task<ApiResponse<EmployeeOnboardingDto>> UpsertEmployeeOnboarding(EmployeeOnboardingDto data);
        Task<List<EmployeeSkills>> UpsertEmployeeSkills(List<EmployeeSkills> details);
        Task<bool> UpsertEmployeeSkillsIDsMap(Dictionary<int, List<int>> dict);
        Task<ApiResponse<List<AssetImport>>> UpsertImportAssetAsync(List<AssetImport> data);
        Task<ApiResponse<List<EmployeeImport>>> UpsertImportEmployeeAsync(List<EmployeeImport> data);
        Task<ApiResponse<UserProfilePicUpld>> UpsertProfilePic(UserProfilePicUpld data);
        Task<List<ZoneArea>> UpsertZoneArea(List<ZoneArea> details);
    }
}
