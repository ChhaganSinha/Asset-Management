using AssetManagement.Dto;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using AssetManagement.Dto.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.EMMA;
using AssetManagement.Dto.Dashboard;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text.Json;
using ClosedXML;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace AssetManagement.Client.Client
{
    public class AppClient : BaseHttpClient
    {
        public AppClient(ILogger<AppClient> logger, HttpClient httpClient) : base(logger, httpClient)
        {

        }

        #region UserProfile Pic Upload
        public async Task<ApiResponse<UserProfilePicUpld>> UpsertProfilePicAsync(UserProfilePicUpld data)
        {
            var result = new ApiResponse<UserProfilePicUpld>();

            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertProfilePic", data);
                res.EnsureSuccessStatusCode();
                var json = await res.Content.ReadFromJsonAsync<ApiResponse<UserProfilePicUpld>>();
                return json;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }


        }


        public async Task<UserProfilePicUpld> GetProfilePicByEmail(string email)
        {
            UserProfilePicUpld data = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/GetProfilePicByEmail/{email}");
                if (res.IsSuccessStatusCode)
                {
                    data = await res.Content.ReadFromJsonAsync<UserProfilePicUpld>();

                }
                else
                {
                    // Handle the case when user is not found or other errors
                    var errorMessage = await res.Content.ReadAsStringAsync();
                    Logger.LogError($"Error fetching profile pic: {errorMessage}");
                }

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return data;
        }


        public async Task<ApiResponse<UserProfilePicUpld>> DeleteProfBgPic(UserProfilePicUpld profilePic)
        {
            var result = new ApiResponse<UserProfilePicUpld>();
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/DeleteProfBgPic", profilePic);
                res.EnsureSuccessStatusCode();
                var json = await res.Content.ReadFromJsonAsync<ApiResponse<UserProfilePicUpld>>();
                return json;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }

        }
        #endregion

        #region Company
        public async Task<Company> GetCompanyById(int id)
        {
            Company result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/Company/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<Company>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<Company>> GetAllCompany()
        {
            IEnumerable<Company> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-Company");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<Company>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }

        public async Task<ApiResponse<Company>> UpsertCompanyAsync(Company data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertCompany", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Company>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }


        public async Task<(bool, string)> DeleteCompanyById(Company data)
        {
            try
            {

                var res = await HttpClient.PostAsJsonAsync($"api/App/DeleteCompany", data);

                res.EnsureSuccessStatusCode();

                var result = await res.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(result);

                // Access the root element
                var rootElement = jsonDocument.RootElement;

                // Access the values of item1 and item2
                bool item1 = rootElement.GetProperty("item1").GetBoolean();
                string item2 = rootElement.GetProperty("item2").GetString();

                return (item1, item2);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        public async Task<List<SubOffice>> UpsertSubOfficeAsync(List<SubOffice> data)
        {
            List<SubOffice> result = null;
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertSubOffice", data);

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<List<SubOffice>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
            return result;

        }

        public async Task<IEnumerable<SubOffice>> GetSubOfficeByCompanyId(int id)
        {
            IEnumerable<SubOffice> result;

            try
            {
                var res = await HttpClient.GetAsync($"api/App/SubOffice-by-id/{id}");
                res.EnsureSuccessStatusCode();
                result = await res.Content.ReadFromJsonAsync<IEnumerable<SubOffice>>();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        #endregion

        #region ZoneArea
        public async Task<ZoneArea> GetZoneAreaById(int id)
        {
            ZoneArea details = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/ZoneArea/{id}");

                res.EnsureSuccessStatusCode();

                details = await res.Content.ReadFromJsonAsync<ZoneArea>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return details;
        }
        public async Task<IEnumerable<ZoneArea>> GetAllZoneArea()
        {
            IEnumerable<ZoneArea> details = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-ZoneArea");

                res.EnsureSuccessStatusCode();

                details = await res.Content.ReadFromJsonAsync<IEnumerable<ZoneArea>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return details;
        }
        public async Task<List<ZoneArea>> UpsertZoneAreaAsync(List<ZoneArea> data)
        {

            List<ZoneArea> result = null;

            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertZoneArea", data);

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<List<ZoneArea>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;

        }

        #endregion

        #region Employee
        public async Task<ApiResponse<CreateEmailRequest>> CreateEmailRequest(CreateEmailRequest createEmailRequest)
        {
            var result = new ApiResponse<CreateEmailRequest>();
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/CreateEmailRequest", createEmailRequest);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadFromJsonAsync<ApiResponse<CreateEmailRequest>>();
                    result.IsSuccess = true;
                   // result.Result = res.Result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        
        }
        public class LoginResponse
        {
            public string access { get; set; }
            public string refresh { get; set; }
        }

        public async Task<List<EntityVendorDto>> getAllVendordetail()
        {

            string accessToken = null;
            var result = new List<EntityVendorDto>();
            try
            {
                string username = "admin";
                string password = "admin@1234";

                var loginPayload = new
                {
                    username = username,
                    password = password
                };


                var res = await HttpClient.PostAsJsonAsync($"https://apiem.credentinfotech.com/api/login/", loginPayload);

                if (res.IsSuccessStatusCode)
                {
                    var responseContent = await res.Content.ReadFromJsonAsync<LoginResponse>();

                    accessToken = responseContent.access;
                    // result.Result = res.Result;
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        Console.WriteLine("Access token not found.");
                        return null;
                    }
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var entitiesResponse = await HttpClient.GetAsync("https://apiem.credentinfotech.com/api/entities/");

                    if (entitiesResponse.IsSuccessStatusCode)
                    {
                        var apiData = await entitiesResponse.Content.ReadFromJsonAsync<List<EntityVendorDto>>();
                        result = apiData;
                        Console.WriteLine("Entity Data: " + result);
                    }
                    else
                    {
                        Console.WriteLine("Entities API failed with status: " + entitiesResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
            return result;
        
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            Employee result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/Employee/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<Employee>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }  
        public async Task<EmployeeOnboardingDto> GetEmployeeOnboardingById(int id)
        {
            EmployeeOnboardingDto result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/EmployeeOnboarding/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<EmployeeOnboardingDto>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<ApiResponse<EmployeeOnboardingDto>> UpsertConfirmOnboardingAsync(OnBoardingConfirmationDto data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/EmployeeOnboarding/Confirm", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<EmployeeOnboardingDto>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeInsurance>> GetEmployeeInsuranceById(int id)
        {
            IEnumerable<EmployeeInsurance> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/EmployeeInsurance/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<EmployeeInsurance>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                return null;
            }

            return result;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            IEnumerable<Employee> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-Employee");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<Employee>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                return result;
            }

            return result;
        }

        public async Task<ApiResponse<Employee>> UpsertEmployeeAsync(Employee data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync("api/App/UpsertEmployee", data);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    Logger.LogError($"Failed to upsert employee. Status code: {res.StatusCode}, Response: {errorContent}");
                    return new ApiResponse<Employee>
                    {
                        IsSuccess = false,
                        Message = $"Failed to upsert employee. Status code: {res.StatusCode}, Response: {errorContent}"
                    };
                }

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                return new ApiResponse<Employee>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeOnboardingDto>> UpsertEmployeeOnboarding(EmployeeOnboardingDto data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/EmployeeOnboarding", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<EmployeeOnboardingDto>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByKey(string Key)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                var payload = new GenericApiRequest<string>() { Param = Key };
                var responce = await HttpClient.PostAsJsonAsync<GenericApiRequest<string>>($"api/EmployeeOnboarding/ByKey", payload);
                responce.EnsureSuccessStatusCode();
                if (responce.IsSuccessStatusCode)
                {
                    var content = await responce.Content.ReadFromJsonAsync<ApiResponse<EmployeeOnboardingDto>>();
                    result.IsSuccess = true;
                    result.Result = content.Result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataById(string Key)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                var payload = new GenericApiRequest<string>() { Param = Key };
                var responce = await HttpClient.PostAsJsonAsync<GenericApiRequest<string>>($"api/EmployeeOnboarding/ById", payload);
                responce.EnsureSuccessStatusCode();

                if (responce.IsSuccessStatusCode)
                {
                    var content = await responce.Content.ReadFromJsonAsync<ApiResponse<EmployeeOnboardingDto>>();
                    result.IsSuccess = true;
                    result.Result = content.Result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeFilesMapping>> UpsertEmployeeFiles(EmployeeFilesMapping data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/EmployeeOnboarding/Files", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<EmployeeFilesMapping>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

        }

        [AllowAnonymous]
        public async Task<EmployeeFilesMapping> GetEmployeeFilesById(int id)
        {
            EmployeeFilesMapping result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/EmployeeOnboarding/Files/{id}");
                res.EnsureSuccessStatusCode();

                if (res.Content is not null)
                {
                    string responseContent = await res.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        //result = JsonSerializer.Deserialize<EmployeeFilesMapping>(responseContent);
                        result = await res.Content.ReadFromJsonAsync<EmployeeFilesMapping>();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }


        public async Task<IEnumerable<EmployeeFilesMapping>> GetAllEmployeeFileMap()
        {
            IEnumerable<EmployeeFilesMapping> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/EmployeeOnboarding/AllFiles");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<EmployeeFilesMapping>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<ApiResponse<List<EmployeeImport>>> UpsertImportEmployeeAsync(List<EmployeeImport> data)
        {
            var result = new ApiResponse<List<EmployeeImport>>();
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertImportEmployee", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<List<EmployeeImport>>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                result.Message = ex.Message;
                result.IsSuccess = false;
                return result;
            }
        }

        public async Task<(bool, string)> DeleteEmployeeById(int id)
        {
            try
            {
                var res = await HttpClient.DeleteAsync($"api/App/DeleteEmployee/{id}");

                res.EnsureSuccessStatusCode();

                var result = await res.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(result);

                // Access the root element
                var rootElement = jsonDocument.RootElement;

                // Access the values of item1 and item2
                bool item1 = rootElement.GetProperty("item1").GetBoolean();
                string item2 = rootElement.GetProperty("item2").GetString();

                return (item1, item2);

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<(bool, string)> DeleteOnboardeeById(int id)
        {
            try
            {
                var res = await HttpClient.DeleteAsync($"api/App/DeleteOnboardee/{id}");

                res.EnsureSuccessStatusCode();

                var result = await res.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(result);

                // Access the root element
                var rootElement = jsonDocument.RootElement;

                // Access the values of item1 and item2
                bool item1 = rootElement.GetProperty("item1").GetBoolean();
                string item2 = rootElement.GetProperty("item2").GetString();

                return (item1, item2);

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<Employee>> ShareFormWithEmployeeViaEmail(int id)
        {
            var result = new ApiResponse<Employee>();
            try
            {
                var res = await HttpClient.GetAsync($"api/App/ShareFormWithEmployeeViaEmail/{id}");

                res.EnsureSuccessStatusCode();

                return await res.Content.ReadFromJsonAsync<ApiResponse<Employee>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<EmployeeOnboardingDto>> ShareFormWithOnboardeeViaEmail(int id)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            try
            {
                var res = await HttpClient.GetAsync($"api/EmployeeOnboarding/ShareForm/{id}");

                res.EnsureSuccessStatusCode();

                return await res.Content.ReadFromJsonAsync<ApiResponse<EmployeeOnboardingDto>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion

        #region EmployeeSkills
        public async Task<EmployeeSkills> GetEmployeeSkillsById(int id)
        {
            EmployeeSkills details = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/EmployeeSkills/{id}");

                res.EnsureSuccessStatusCode();

                details = await res.Content.ReadFromJsonAsync<EmployeeSkills>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return details;
        }

        [AllowAnonymous]
        public async Task<IEnumerable<EmployeeSkills>> GetAllSkill()
        {
            IEnumerable<EmployeeSkills> details = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-EmployeeSkills");

                res.EnsureSuccessStatusCode();

                details = await res.Content.ReadFromJsonAsync<IEnumerable<EmployeeSkills>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return details;
        }
        public async Task<List<DesignationDTO>> UpsertDesignationAsync(List<DesignationDTO> data)
        {

            List<DesignationDTO> result = null;

            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertDesignation", data);

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<List<DesignationDTO>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;

        }
        public async Task<IEnumerable<DesignationDTO>> GetAllDesignations()
        {
            IEnumerable<DesignationDTO> details = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-designations");

                res.EnsureSuccessStatusCode();

                    details = await res.Content.ReadFromJsonAsync<IEnumerable<DesignationDTO>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return details;
        }
        public async Task<List<EmployeeSkills>> UpsertEmployeeSkillsAsync(List<EmployeeSkills> data)
        {

            List<EmployeeSkills> result = null;

            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertEmployeeSkills", data);

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<List<EmployeeSkills>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;

        }

        [AllowAnonymous]
        public async Task<bool> UpsertEmployeeSkillsIDsMap(Dictionary<int, List<int>> dict)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/EmployeeSkillsIDsMap", dict);
                res.EnsureSuccessStatusCode();

                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            //return false;
        }
        [AllowAnonymous]
        public async Task<List<EmployeeSkillMapping>> GetEmployeeSkillsIDs(int id)
        {
            List<EmployeeSkillMapping> employeeSkillMapping = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/getEmployeeSkillsIDs/{id}");

                res.EnsureSuccessStatusCode();

                employeeSkillMapping = await res.Content.ReadFromJsonAsync<List<EmployeeSkillMapping>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
            return employeeSkillMapping;
        }

        public async Task<ApiResponse<Employee>> EmployeeGetSelfDetail(string Key)
        {
            var result = new ApiResponse<Employee>();
            try
            {
                var payload = new GenericApiRequest<string>() { Param = Key };
                var responce = await HttpClient.PostAsJsonAsync<GenericApiRequest<string>>($"api/App/employee-self-get", payload);
                responce.EnsureSuccessStatusCode();
                if (responce.IsSuccessStatusCode)
                {
                    var content = await responce.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
                    result.IsSuccess = true;
                    result.Result = content.Result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ApiResponse<Employee>> EmployeeDetailSeftUpdate(Employee data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/EmployeeSeftUpdate", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region Asset
        public async Task<Asset> GetAssetById(int id)
        {
            Asset result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/Asset/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<Asset>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<Asset>> GetAllAsset()
        {
            IEnumerable<Asset> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-Asset");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<Asset>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }

        public async Task<ApiResponse<Asset>> UpsertAssetAsync(Asset data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertAsset", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Asset>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<List<AssetImport>>> UpsertImportAssetAsync(List<AssetImport> data)
        {
            var result = new ApiResponse<List<AssetImport>>();
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertImportAssets", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<List<AssetImport>>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                result.Message = ex.Message;
                result.IsSuccess = false;
                return result;
            }
        }
        public async Task<(bool, string)> DeleteAssetById(Asset data)
        {
            try
            {

                var res = await HttpClient.PostAsJsonAsync($"api/App/DeleteAsset", data);

                res.EnsureSuccessStatusCode();

                var result = await res.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(result);

                // Access the root element
                var rootElement = jsonDocument.RootElement;

                // Access the values of item1 and item2
                bool item1 = rootElement.GetProperty("item1").GetBoolean();
                string item2 = rootElement.GetProperty("item2").GetString();

                return (item1, item2);

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region AssetType
        public async Task<AssetType> GetAssetTypeById(int id)
        {
            AssetType result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/AssetType/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<AssetType>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<AssetType>> GetAllAssetType()
        {
            IEnumerable<AssetType> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-AssetType");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<AssetType>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }

        public async Task<ApiResponse<AssetType>> UpsertAssetTypeAsync(AssetType data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertAssetType", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<AssetType>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region Allocation
        public async Task<Allocation> GetAllocationById(int id)
        {
            Allocation result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/Allocation/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<Allocation>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }

        public async Task<Allocation> GetAllocationByAssetId(int id)
        {
            Allocation result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/GetAllocationByAssetId/{id}");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<Allocation>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<Allocation>> GetAllAllocation()
        {
            IEnumerable<Allocation> result = null;
            try
            {
                var res = await HttpClient.GetAsync($"api/App/all-Allocation");

                res.EnsureSuccessStatusCode();

                result = await res.Content.ReadFromJsonAsync<IEnumerable<Allocation>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }
        public async Task<ApiResponse<Allocation>> UpsertAllocationAsync(Allocation data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/UpsertAllocation", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Allocation>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> UnAllocation(int id)
        {
            try
            {
                var res = await HttpClient.DeleteAsync($"api/App/UnAllocation/{id}");
                res.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogCritical(ex, ex.Message);
                return false;
            }
        }


        public async Task<ApiResponse<Allocation>> EmployeeAllocationDetails(string Key)
        {
            var result = new ApiResponse<Allocation>();
            try
            {
                var payload = new GenericApiRequest<string>() { Param = Key };
                var responce = await HttpClient.PostAsJsonAsync<GenericApiRequest<string>>($"api/App/EmployeeAllocationDetails", payload);
                responce.EnsureSuccessStatusCode();
                if (responce.IsSuccessStatusCode)
                {
                    var content = await responce.Content.ReadFromJsonAsync<ApiResponse<Allocation>>();
                    if (content.IsSuccess)
                    {
                        result.IsSuccess = true;
                        result.Result = content.Result;
                    }
                    else
                    {
                        result.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/EmployeeAllocationResponce", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Allocation>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<Allocation>> ShareAllocationDetailsToEmployeeViaEmail(int id)
        {
            var result = new ApiResponse<Allocation>();
            try
            {
                var res = await HttpClient.GetAsync($"api/App/ShareAllocationDetailsToEmployeeViaEmail/{id}");

                res.EnsureSuccessStatusCode();

                return await res.Content.ReadFromJsonAsync<ApiResponse<Allocation>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<string> ReadAllocationCommentById(int id)
        {

            try
            {
                var res = await HttpClient.GetAsync($"api/App/ReadAllocationCommentById/{id}");
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region Dashboard
        public async Task<DashboardStatics> GetLatestStats()
        {
            var result = new DashboardStatics();
            try
            {
                var res = await HttpClient.GetAsync($"api/App/get-latest-statistics");
                res.EnsureSuccessStatusCode();
                var json = await res.Content.ReadFromJsonAsync<ApiResponse<DashboardStatics>>();

                if (json != null)
                    result = json.Result;

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return result;
        }

        public async Task<MasterStatics?> GetMasterStaticsEntires()
        {
            try
            {
                var result = new MasterStatics();
                var res = await HttpClient.GetAsync($"api/App/GetMasterStaticsEntires");
                if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return result;

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<MasterStatics>>();

                if (json != null)
                    return json.Result;

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }

            return null;
        }
        #endregion

        #region ReportGenerate
        public async Task<byte[]> AllocationlogExportToExcel(AllocationlogReportGenerate data)
        {
            var queryString = string.Join("&", data.GetType()
            .GetProperties()
            .Select(prop =>
            {
                var value = prop.GetValue(data);
                if (value is List<string> listValue)
                {
                    var encodedListValue = string.Join(",", listValue.Select(Uri.EscapeDataString));
                    return $"{Uri.EscapeDataString(prop.Name)}={encodedListValue}";
                }
                else
                {
                    return $"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(value?.ToString() ?? "")}";
                }
            }));

            var response = await HttpClient.GetAsync($"/api/Report/AllocationlogExcelReport?{queryString}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }

        public async Task<IEnumerable<AllocationLog>> GetFilteredAllocationlogs(AllocationlogReportGenerate data)
        {
            try
            {
                var queryString = string.Join("&", data.GetType()
                    .GetProperties()
                    .Select(prop => $"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(prop.GetValue(data)?.ToString() ?? "")}"));

                var res = await HttpClient.GetAsync($"api/App/FilteredAllocationlogs?{queryString}");

                res.EnsureSuccessStatusCode();

                return await res.Content.ReadFromJsonAsync<IEnumerable<AllocationLog>>();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion

        #region Tranfer
        public async Task<ApiResponse<Employee>> EmployeeTransfer(EmployeeTransferModel data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/EmployeeTransfer", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        public async Task<ApiResponse<Asset>> AssetTransfer(AssetTransferModel data)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/AssetTransfer", data);

                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadFromJsonAsync<ApiResponse<Asset>>();
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        #region Reports
        public async Task<ReportFilters> GetEmployeeReportFileters()
        {
            try
            {
                var res = await HttpClient.GetAsync($"api/App/GetEmployeeReportFileters");
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<ReportFilters>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<Employee>> GetFilteredEmployeeReport(EmployeeFilterModel model)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/GetFilteredEmployeeReport", model);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<List<Employee>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        public async Task<List<Asset>> GetFilteredAssetReport(AssetFilterModel model)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/App/GetFilteredAssetReport", model);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<List<Asset>>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<byte[]> EmployeeExportToExcel(EmployeeFilterModel model)
        {


            var response = await HttpClient.PostAsJsonAsync($"api/Report/EmployeeExcelReport", model);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }

        public async Task<byte[]> AssetExportToExcel(AssetFilterModel model)
        {


            var response = await HttpClient.PostAsJsonAsync($"api/Report/AssetExcelReport", model);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }

        public async Task<AssetReportFilters> GetAssetReportFileters()
        {
            try
            {
                var res = await HttpClient.GetAsync($"api/App/GetAssetReportFileters");
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<AssetReportFilters>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        #endregion

        public async Task<SharepointListUpdate> UpdateListItems(SharepointListUpdate model)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/SharePoint/updateListItem", model);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<SharepointListUpdate>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        public async Task<SharepointListUpdate> InsertListItem(SharepointListUpdate model)
        {
            try
            {
                var res = await HttpClient.PostAsJsonAsync($"api/SharePoint/insertListItem", model);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<SharepointListUpdate>();

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<SharepointListUpdate> GetListItemByEmail(string Email)
        {
            try
            {
                var res = await HttpClient.PostAsync($"api/SharePoint/getListItemByEmail?Email={Email}", null);
                res.EnsureSuccessStatusCode();
                var data =  await res.Content.ReadFromJsonAsync<SharepointListUpdate>();
                return data;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<PincodeApiResponse>> GetAddrressbyPinCodeValue(int value)
        {
            var result = new List<PincodeApiResponse>();
            try
            {
                // Create a new HttpClient instance (consider using HttpClientFactory in production)
                using var client = new HttpClient();

                // Add user agent header as some APIs require it
                client.DefaultRequestHeaders.Add("User-Agent", "YourApplication/1.0");

                var response = await client.GetAsync($"https://api.postalpincode.in/pincode/{value}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync< List<PincodeApiResponse>>();
                    result = apiResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Consider logging the full exception for debugging
            }
        return result;
        }

        // Add this class to match the API response structure
        //public class PincodeApiResponse
        //{
        //    public List<PincodeApiResponse> PostOffice { get; set; }
        //}

        //public async Task<List<PostOfficeDetails>> GetAddrressbyPinCodeValue(int value)
        //{
        //    var result = new List<PostOfficeDetails>();
        //    try
        //    {
        //        var res = await HttpClient.GetAsync($"http://www.postalpincode.in/api/pincode/{value}");

        //        if (res.IsSuccessStatusCode)
        //        {
        //            var content = await res.Content.ReadFromJsonAsync<List<PostOfficeDetails>>();
        //            result = content;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //    return result;

        //}
        #region Temporary
        public async Task<ApiResponse<Employee>> EmployeeInsuranceByKey(string Key)
        {
            var result = new ApiResponse<Employee>();
            try
            {
                var payload = new GenericApiRequest<string>() { Param = Key };
                var responce = await HttpClient.PostAsJsonAsync<GenericApiRequest<string>>($"api/App/employee-insurance", payload);
                responce.EnsureSuccessStatusCode();
                if (responce.IsSuccessStatusCode)
                {
                    var content = await responce.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
                    result.IsSuccess = true;
                    result.Result = content.Result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ApiResponse<string>> EmployeeInsuranceformSender()
        {
            var result = new ApiResponse<string>();
            try
            {
                var res = await HttpClient.GetAsync($"api/App/EmployeeInsuranceformSender");

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;

        }

        public async Task<ApiResponse<string>> ChangeReturnUrl()
        {
            var result = new ApiResponse<string>();
            try
            {
                var res = await HttpClient.GetAsync($"api/App/ChangeReturnUrl");

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;

        }
        #endregion
    }
}
