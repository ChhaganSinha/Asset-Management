using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Dashboard;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using AssetManagement.Server.EmailService;
using AssetManagement.Server.Service;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static MudBlazor.CategoryTypes;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        readonly IAppRepository _appRepository;
        readonly IEmployeeRepository _employeeRepository;
        readonly ICompanyRepository _companyRepository;
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly OnboardingConfirmation _OnboardingConfirmation;
        public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMailService mailService, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, OnboardingConfirmation onboardingConfirmation ) : base()
        {
            _appRepository = (AppRepository?)appRepository;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _configuration = appConfig;
            _mailService = mailService;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _OnboardingConfirmation = onboardingConfirmation;
        }

        #region Update Profilr images

        [HttpPost]
        [Route("UpsertProfilePic")]
        public async Task<ApiResponse<UserProfilePicUpld>> UpsertProfilePic(UserProfilePicUpld data)
        {


            return await _appRepository.UpsertProfilePic(data);
        }

        [HttpGet]
        [Route("GetProfilePicByEmail/{email}")]
        public async Task<UserProfilePicUpld> GetProfilePicByEmail(string email)
        {


            return await _appRepository.GetProfilePicByEmail(email);
        }

        [HttpPost]
        [Route("DeleteProfBgPic")]
        public async Task<ApiResponse<UserProfilePicUpld>> DeleteProfBgPic(UserProfilePicUpld userProfilePicUpld)
        {
            userProfilePicUpld.Email = User.Identity.Name;
            return await _appRepository.DeleteProfBgPic(userProfilePicUpld);
        }
        #endregion

        #region ZoneArea
        [HttpGet]
        [Route("ZoneArea/{id}")]
        public async Task<ZoneArea> GetZoneAreaById(int id)
        {
            return await _appRepository.GetZoneAreaById(id);
        }

        [HttpGet]
        [Route("all-ZoneArea")]
        public async Task<IEnumerable<ZoneArea>> GetAllZoneArea()
        {
            return await _appRepository.GetAllZoneArea();
        }

        [HttpPost]
        [Route("UpsertZoneArea")]
        public async Task<List<ZoneArea>> UpsertZoneArea(List<ZoneArea> details)
        {

            return await _appRepository.UpsertZoneArea(details);
        }
        #endregion

        #region Company
        [HttpGet]
        [Route("Company/{id}")]
        public async Task<Company> GetCompanyById(int id)
        {
            return await _companyRepository.GetCompanyByIdAsync(id);
        }

        [HttpGet]
        [Route("all-Company")]
        public async Task<IEnumerable<Company>> GetAllCompany()
        {
            return await _companyRepository.GetAllCompanyAsync();
        }

        [HttpPost]
        [Route("UpsertCompany")]
        public async Task<ApiResponse<Company>> UpsertCompany(Company data)
        {
            var result = new ApiResponse<Company>();
            try
            {
                result = await _companyRepository.UpsertCompanyAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = String.Empty;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }


        [HttpPost]
        [Route("DeleteCompany")]
        public async Task<(bool, string)> DeleteCompanyById(Company data)
        {
            return await _companyRepository.DeleteCompanyByIdAsync(data);
        }

        [HttpPost]
        [Route("UpsertSubOffice")]
        public async Task<List<SubOffice>> UpsertTrainingQuiz(List<SubOffice> data)
        {
            return await _companyRepository.UpsertSubOfficeAsync(data);
        }

        [HttpGet]
        [Route("SubOffice-by-id/{id}")]
        public async Task<IEnumerable<SubOffice>> GetTrainingQuizById(int id)
        {
            return await _companyRepository.GetSubOfficeByIdAsync(id);
        }
        #endregion

        #region Sharepoint Portal Access
        [HttpGet]
        [Route("GetEmployeeByEmail/{email}")]
        [AllowAnonymous]
        public async Task<ApiResponse<EmployeePortalSPFX>> GetEmployeeByEmail()
        {
            var email = (string)HttpContext.Request.RouteValues["email"];
            var result = new ApiResponse<EmployeePortalSPFX>();
            result = await _appRepository.GetEmployeeByEmail(email);
            return result;
        }
        [HttpPost]
        [Route("UpdateEmployeeByEmail")]
        [AllowAnonymous]
        public async Task<ApiResponse<EmployeePortalSPFX>> UpdateEmployeeByEmail(EmployeePortalSPFX employeePortalSPFX)
        {
            var result = new ApiResponse<EmployeePortalSPFX>();
            result = await _appRepository.UpdateEmployeeByEmail(employeePortalSPFX);
            return result;
        }
        #endregion

        #region Employee
        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        [HttpGet]
        [Route("EmployeeInsurance/{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<EmployeeInsurance>> GetEmployeeInsuranceById(int id)
        {
            var data =  await _appRepository.GetEmployeeInsuranceById(id);
            return data;
        }

        [HttpGet]
        [Route("all-Employee")]
        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            var result = await _employeeRepository.GetEmployeesAsync(1, int.MaxValue);
            return result.Items;
        }

        [HttpGet]
        [Route("EmployeePaged")]
        public async Task<PagedResult<Employee>> GetEmployeesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            return await _employeeRepository.GetEmployeesAsync(page, pageSize);
        }

        [HttpPost]
        [Route("UpsertEmployee")]
        public async Task<ApiResponse<Employee>> UpsertEmployee(Employee data)
        {
            var result = new ApiResponse<Employee>();

            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                data.BaseUrl = baseUrl;
                result = await _employeeRepository.UpsertEmployeeAsync(data);

                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    //result.Message = string.Empty;

                    string path;
                    //var RawContents = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{path}");
#if DEBUG
                    path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\TableTemplate.txt";
#else
                    path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/TableTemplate.txt");
#endif
                    var RawContents = System.IO.File.ReadAllText($"{path}");
                    var Contents = RawContents.Replace("<ccode>", result.Result.CompanyCode).Replace("<eid>", result.Result.EmployeeId).Replace("<ename>", result.Result.EmployeeName)
                    .Replace("<fname>", result.Result.fatherName).Replace("<email>", result.Result.EmailId)
                    .Replace("<mobile>", result.Result.MobileNumber).Replace("<paddress>", result.Result.PermanentAddress).Replace("<pstate>", result.Result.PState)
                    .Replace("<pcountry>", result.Result.PCountry).Replace("<ppin>", result.Result.PPin).Replace("<caddress>", result.Result.CurrentAddress)
                    .Replace("<cstate>", result.Result.CState).Replace("<ccountry>", result.Result.CCountry).Replace("<cpin>", result.Result.CPin)
                    .Replace("<dob>", result.Result.DateOfBirth.ToString()).Replace("<adhar>", result.Result.AadhaarNumber).Replace("<pan>", result.Result.PANNumber)
                    .Replace("<uan>", result.Result.UANNo).Replace("<bank>", result.Result.EmpBankName).Replace("<account>", result.Result.EmpBankAccNumber)
                    .Replace("<ifsc>", result.Result.EmpBankIfsc).Replace("<doj>", result.Result.DateOfJoin.ToString()).Replace("<emergency>", result.Result.EmergencyContactNumber)
                    .Replace("<designation>", result.Result.Designation).Replace("<report>", result.Result.ReportingTo)
                    .Replace("<###RETURN_URL###>", data.ReturnUrl);

                    string To = result.Result.EmailId;
                    string Subject = "AssetManagement Application Employee Form Details";
                    string Body = Contents;

                    //await _mailService.SendEmailAsync(result.Result.EmailId, Subject, Body);
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }


        [HttpPost]
        [Route("UpsertImportEmployee")]
        public async Task<ApiResponse<List<EmployeeImport>>> UpsertImportEmployee(List<EmployeeImport> data)
        {
            var result = new ApiResponse<List<EmployeeImport>>();
            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                data.ForEach(item => item.BaseUrl = baseUrl);
                result = await _appRepository.UpsertImportEmployeeAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = string.Empty;
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
        [HttpGet("/tracking")]
        public async Task<IActionResult> TrackEmailOpen([FromQuery] string id, [FromQuery] string email)
        {
            return Ok();
        }

        [HttpPost]
        [Route("CreateEmailRequest")]
        public async Task<ApiResponse<CreateEmailRequest>> CreateEmailRequest(CreateEmailRequest createEmailRequest)
        {
            var result = new ApiResponse<CreateEmailRequest>();

            string path;
            //var RawContents = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{path}");
#if DEBUG
            path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\TableTemplate.txt";
#else
                path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/TableTemplate.txt");
#endif
            string mailTo = _configuration.GetValue<string>("AdminEmail");
            string Subject = "Request to Create Email";
            string link = createEmailRequest.LandingUrl.Replace("emp", "emp/mailupdate");
            try
            {
                // Construct the tracking URL
                string trackingUrl = $"https://localhost:7053/api/App/tracking?id=88&email={mailTo}";

                // Modify the email body to include the tracking URL
                string body = $@"Hi, Please create an email for the below user and update from <a href=""{link}"">here</a>.
                <br/><br/>
                CompanyCode: {createEmailRequest.CompanyCode}<br/>
                Name: {createEmailRequest.Name}<br/>
                EmployeeID: {createEmailRequest.EmployeeID}<br/><br/>
                Thank You.
                <br/><br/>
                <img src=""{trackingUrl}"" alt=""tracking pixel"" width=""1"" height=""1"" style=""display:none;"">";
                await _mailService.SendEmailAsync(mailTo, Subject, body);
                result.IsSuccess = true;
                result.Message = $"Sent";


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }


            return result;
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public async Task<(bool, string)> DeleteEmployee(int id)
        {
            return await _employeeRepository.DeleteEmployeeByIdAsync(id);
        }

        [HttpDelete]
        [Route("DeleteOnboardee/{id}")]
        public async Task<(bool, string)> DeleteOnboardeeById(int id)
        {
            return await _appRepository.DeleteOnboardeeById(id);
        }

        [HttpGet]
        [Route("ShareFormWithEmployeeViaEmail/{id}")]
        public async Task<ApiResponse<Employee>> ShareFormWithEmployeeViaEmail(int id)
        {
            var result = new ApiResponse<Employee>();
            var employee = await _appRepository.ShareFormWithEmployeeViaEmail(id);
            if (employee != null)
            {
                string path;
                //var RawContents = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{path}");
#if DEBUG
                path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\TableTemplate.txt";
#else
                path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/TableTemplate.txt");
#endif
                try
                {

                    var RawContents = System.IO.File.ReadAllText($"{path}");
                    var Contents = RawContents.Replace("<ccode>", employee.CompanyCode).Replace("<eid>", employee.EmployeeId).Replace("<ename>", employee.EmployeeName)
                    .Replace("<fname>", employee.fatherName).Replace("<email>", employee.EmailId)
                    .Replace("<mobile>", employee.MobileNumber).Replace("<paddress>", employee.PermanentAddress).Replace("<pstate>", employee.PState)
                    .Replace("<pcountry>", employee.PCountry).Replace("<ppin>", employee.PPin).Replace("<caddress>", employee.CurrentAddress)
                    .Replace("<cstate>", employee.CState).Replace("<ccountry>", employee.CCountry).Replace("<cpin>", employee.CPin)
                    .Replace("<dob>", employee.DateOfBirth.ToString("dd/MM/yyyy")).Replace("<adhar>", employee.AadhaarNumber).Replace("<pan>", employee.PANNumber)
                    .Replace("<uan>", employee.UANNo).Replace("<bank>", employee.EmpBankName).Replace("<accountname>", employee.EmpAccountName).Replace("<account>", employee.EmpBankAccNumber)
                    .Replace("<ifsc>", employee.EmpBankIfsc).Replace("<doj>", employee.DateOfJoin.ToString("dd/MM/yyyy")).Replace("<emergency>", employee.EmergencyContactNumber)
                    .Replace("<designation>", employee.Designation).Replace("<report>", employee.ReportingTo)
                    .Replace("<###RETURN_URL###>", employee.ReturnUrl);

                    string To = employee.EmailId;
                    string Subject = "AssetManagement Application Employee Form Details";
                    string Body = Contents;

                    await _mailService.SendEmailAsync(employee.EmailId, Subject, Body);
                    result.IsSuccess = true;
                    result.Message = $"Successfully Shared with {employee.EmailId}";
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                }

            }
            return result;
        }



        [HttpPost]
        [Route("employee-self-get")]
        [AllowAnonymous]
        public async Task<ApiResponse<Employee>> EmployeeSelfGetDetails(GenericApiRequest<string> request)
        {
            return await _appRepository.EmployeeSelfGetDetails(request.Param);
        }

        [HttpPost]
        [Route("EmployeeSeftUpdate")]
        [AllowAnonymous]
        public async Task<ApiResponse<Employee>> EmployeeSeftUpdate(Employee data)
        {
            var result = new ApiResponse<Employee>();
            try
            {
                result = await _appRepository.EmployeeSeftUpdate(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = string.Empty;
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

        #region EmployeeSkills
        [HttpGet]
        [Route("EmployeeSkills/{id}")]
        public async Task<EmployeeSkills> GetEmployeeSkillsById(int id)
        {
            return await _appRepository.GetEmployeeSkillsById(id);
        }

        [HttpGet]
        [Route("all-EmployeeSkills")]
        [AllowAnonymous]
        public async Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkills()
        {
            return await _appRepository.GetAllEmployeeSkills();
        }

        [HttpPost]
        [Route("UpsertEmployeeSkills")]
        [AllowAnonymous]
        public async Task<List<EmployeeSkills>> UpsertEmployeeSkills(List<EmployeeSkills> details)
        {

            return await _appRepository.UpsertEmployeeSkills(details);
        }

        [HttpPost]
        [Route("UpsertDesignation")]
        public async Task<List<DesignationDTO>> UpsertDesignation(List<DesignationDTO> details)
        {

            return await _appRepository.UpsertDesignation(details);
        }
        [HttpGet]
        [Route("all-designations")]
 
        public async Task<IEnumerable<DesignationDTO>> GetAllDesignations()
        {
            return await _appRepository.GetAllDesignations();
        }

        [HttpPost]
        [Route("EmployeeSkillsIDsMap")]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> EmployeeSkillsIDsMap(Dictionary<int, List<int>> dict)
        {
            var result = new ApiResponse<string>();
            try
            {
                var outcome = await _appRepository.UpsertEmployeeSkillsIDsMap(dict);
                result.Message = "Successful";
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error while uploading data to TrainingCampIdMapping");
            }

            return result;
        }

        [HttpGet]
        [Route("getEmployeeSkillsIDs/{EmployeeId}")]
        [AllowAnonymous]
        public async Task<List<EmployeeSkillMapping>> GetEmployeeSkillsIDs(int EmployeeId)
        {
            var result = await _appRepository.GetEmployeeSkillsIDs(EmployeeId);
            return await Task.FromResult(result);
        }
        #endregion

        #region Asset
        [HttpGet]
        [Route("Asset/{id}")]
        public async Task<Asset> GetAssetById(int id)
        {
            return await _appRepository.GetAssetById(id);
        }

        [HttpGet]
        [Route("all-Asset")]
        public async Task<IEnumerable<Asset>> GetAllAsset()
        {
            return await _appRepository.GetAllAsset();
        }

        [HttpPost]
        [Route("UpsertAsset")]
        public async Task<ApiResponse<Asset>> UpsertAsset(Asset data)
        {
            var result = new ApiResponse<Asset>();
            try
            {
                result = await _appRepository.UpsertAssetAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = String.Empty;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("UpsertImportAssets")]
        public async Task<ApiResponse<List<AssetImport>>> UpsertImportAsset(List<AssetImport> data)
        {
            var result = new ApiResponse<List<AssetImport>>();
            try
            {
                result = await _appRepository.UpsertImportAssetAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("DeleteAsset")]
        public async Task<(bool, string)> DeleteAssetById(Asset data)
        {
            return await _appRepository.DeleteAssetById(data);
        }
        #endregion

        #region Asset&SubAsset
        [HttpGet]
        [Route("AssetType/{id}")]
        public async Task<AssetType> GetAssetTypeById(int id)
        {
            return await _appRepository.GetAssetTypeById(id);
        }

        [HttpGet]
        [Route("all-AssetType")]
        public async Task<IEnumerable<AssetType>> GetAllAssetType()
        {
            return await _appRepository.GetAllAssetType();
        }

        [HttpPost]
        [Route("UpsertAssetType")]
        public async Task<ApiResponse<AssetType>> UpsertAssetType(AssetType data)
        {
            var result = new ApiResponse<AssetType>();
            try
            {
                result = await _appRepository.UpsertAssetTypeAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = String.Empty;
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

        #region Allocation
        [HttpGet]
        [Route("Allocation/{id}")]
        public async Task<Allocation> GetAllocationById(int id)
        {
            return await _appRepository.GetAllocationById(id);
        }

        [HttpGet]
        [Route("GetAllocationByAssetId/{id}")]
        public async Task<Allocation> GetAllocationByAssetId(int id)
        {
            return await _appRepository.GetAllocationByAssetId(id);
        }

        [HttpDelete]
        [Route("UnAllocation/{id}")]
        public async Task<bool> UnAllocation(int id)
        {
            return await _appRepository.UnAllocation(id);
        }

        [HttpGet]
        [Route("all-Allocation")]
        public async Task<IEnumerable<Allocation>> GetAllAllocation()
        {
            return await _appRepository.GetAllAllocation();
        }

        [HttpPost]
        [Route("UpsertAllocation")]
        public async Task<ApiResponse<Allocation>> UpsertAllocation(Allocation data)
        {
            var result = new ApiResponse<Allocation>();
            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                data.BaseUrl = baseUrl;
                result = await _appRepository.UpsertAllocationAsync(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = String.Empty;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("EmployeeAllocationDetails")]
        [AllowAnonymous]
        public async Task<ApiResponse<Allocation>> EmployeeAllocationDetails(GenericApiRequest<string> request)
        {
            return await _appRepository.EmployeeAllocationDetails(request.Param);
        }

        [HttpPost]
        [Route("EmployeeAllocationResponce")]
        [AllowAnonymous]
        public async Task<ApiResponse<Allocation>> EmployeeAllocationResponce(Allocation data)
        {
            var result = new ApiResponse<Allocation>();
            try
            {
                result = await _appRepository.EmployeeAllocationResponce(data);
                if (!result.IsSuccess)
                {
                    result.Message = result.Message;
                }
                else
                {
                    result.Result = data;
                    result.IsSuccess = true;
                    result.Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("ShareAllocationDetailsToEmployeeViaEmail/{id}")]
        public async Task<ApiResponse<Allocation>> ShareAllocationDetailsToEmployeeViaEmail(int id)
        {
            var result = new ApiResponse<Allocation>();
            var allocation = await _appRepository.ShareAllocationDetailsToEmployeeViaEmail(id);
            if (allocation != null)
            {
                string path;
                //var RawContents = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{path}");
#if DEBUG
                path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\AllocationTableTemplate.txt";
#else
                path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/AllocationTableTemplate.txt");
#endif
                try
                {

                    var RawContents = System.IO.File.ReadAllText($"{path}");
                    var Contents = RawContents.Replace("<ccode>", allocation.CompanyCode).Replace("<assettype>", allocation.AssetType).Replace("<asset>", allocation.AssetModel)
                    .Replace("<empname>", allocation.EmployeeName).Replace("<allocationtype>", allocation.AllocationType.ToString())
                    .Replace("<issuedate>", allocation.IssueDate.ToString("dd/MM/yyyy")).Replace("<issuetill>", allocation.IssueTill?.ToString("dd/MM/yyyy"))
                    .Replace("<###RETURN_URL###>", allocation.ReturnUrl);


                    string To = allocation.EmployeeEmail;
                    string Subject = "AssetManagement Application Allocation Details";
                    string Body = Contents;

                    await _mailService.SendEmailAsync(allocation.EmployeeEmail, Subject, Body);
                    result.IsSuccess = true;
                    result.Message = $"Successfully Shared with {allocation.EmployeeEmail}";
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                }

            }
            return result;
        }

        [HttpGet]
        [Route("ReadAllocationCommentById/{id}")]
        public async Task<string> ReadAllocationCommentById(int id)
        {
            return await _appRepository.ReadAllocationCommentById(id);
        }
        #endregion

        #region Dashboard
        [HttpGet]
        [Route("get-latest-statistics")]
        public async Task<ApiResponse<DashboardStatics>> GetLatestStatistics()
        {
            var result = new ApiResponse<DashboardStatics>();
            try
            {
                var data = await _appRepository.GetLastStatics();

                result.IsSuccess = true;
                result.Message = "Success";
                result.Result = data;

              //  _OnboardingConfirmation.OnBoardingquery();
            }
            catch (Exception ex)
            {
                //Logger.LogCritical(ex, ex.Message);
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("GetMasterStaticsEntires")]
        public async Task<ApiResponse<MasterStatics>> GetMasterStaticsEntires(int year)
        {
            var result = new ApiResponse<MasterStatics>();

            try
            {
                var data = await _appRepository.GetMasterStaticsEntires();

                result.IsSuccess = true;
                result.Message = "Success";
                result.Result = data;
            }
            catch (Exception ex)
            {
                //Logger.LogCritical(ex, ex.Message);
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion

        #region AllocationLog
        [HttpGet]
        [Route("FilteredAllocationlogs")]
        public async Task<IEnumerable<AllocationLog>> GetFilteredAllocationlogs([FromQuery] AllocationlogReportGenerate data)
        {
            //return await _appRepository.GetFilteredAllocationLogs(data);
            return await _appRepository.GetAllAllocationLog();
        }

        #endregion

        #region Transfer
        [HttpPost]
        [Route("EmployeeTransfer")]
        public async Task<ApiResponse<Employee>> EmployeeTransfer(EmployeeTransferModel data)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            data.BaseUrl = baseUrl;
            return await _appRepository.EmployeeTransfer(data);
        }
        [HttpPost]
        [Route("AssetTransfer")]
        public async Task<ApiResponse<Asset>> AssetTransfer(AssetTransferModel data)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            data.BaseUrl = baseUrl;
            return await _appRepository.AssetTransfer(data);
        }
        #endregion

        #region Reports
        [HttpGet]
        [Route("GetEmployeeReportFileters")]
        public async Task<ReportFilters> GetEmployeeReportFileters()
        {
            return await _appRepository.GetEmployeeReportFileters();
        }

        [HttpPost]
        [Route("GetFilteredEmployeeReport")]
        public async Task<List<Employee>> GetFilteredEmployeeReport(EmployeeFilterModel model)
        {
            return await _appRepository.GetFilteredEmployeeReport(model);
        }

        //[HttpPost]
        //[Route("GetFilteredAssetReport")]
        //public async Task<List<Employee>> GetFilteredAssetReport(EmployeeFilterModel model)
        //{
        //    return await _appRepository.GetFilteredAssetReport(model);
        //}

        [HttpGet]
        [Route("GetAssetReportFileters")]
        public async Task<AssetReportFilters> GetAssetReportFileters()
        {
            return await _appRepository.GetAssetReportFilters();
        }

        [HttpPost]
        [Route("GetFilteredAssetReport")]
        public async Task<List<Asset>> GetFilteredAssetReport(AssetFilterModel model)
        {
            return await _appRepository.GetFilteredAssetReport(model);
        }
        #endregion

        #region Temporary
        [HttpPost]
        [Route("employee-insurance")]
        [AllowAnonymous]
        public async Task<ApiResponse<Employee>> EmployeeInsuranceByKey(GenericApiRequest<string> request)
        {
            return await _appRepository.EmployeeInsuranceByKey(request.Param);
        }


        [HttpGet]
        [Route("ChangeReturnUrl")]
        public async Task<ApiResponse<string>> ChangeReturnUrl()
        {
            return await _appRepository.ChangeReturnUrl();
        }   
        
        [HttpGet]
        [Route("EmployeeInsuranceformSender")]
        public async Task<ApiResponse<string>> EmployeeInsuranceformSender()
        {
            var result = new ApiResponse<string>();
            var employees = (await _employeeRepository.GetEmployeesAsync(1, int.MaxValue)).Items;
            //var employee = await _appRepository.GetEmployeeById(26);

            List<string> validEmployeeIds = new List<string>
            {
                //"CISLL053", "CISLL064", "IPAI021", "IPAI0016", "STN002",
                //"CISLL058", "CISLL0046", "CISLL0038", "CISLL0008", "CISLL0036",
                //"CISLL059", "CISLL060", "CISLL061", "CISLL055", "CISLL0045",
                //"CISLL0035", "CISLL052", "CISLL0001", "IPAI0015", "CISLL0002",
                //"IPAI033", "IPAI030", "CISLL056", "IPAI020", "STN006",
                //"CISLL054", "CISLL063", "IPAI023", "CISLL0004", "CISLL0041",
                //"CISLL0040", "CISLL0007", "CISLL0033", "CISLL0032", "CISLL0017",
                //"CISLL062", "CISLL0049"
                "CISLL0038"
            };

            foreach (var employee in employees.Where(x => x.Status != EmployeeStatus.Resigned))
            {
                if (validEmployeeIds.Contains(employee.EmployeeId))
                {
                    //string To = "cs.credent@gmail.com";
                    string To = employee.EmailId;
                    string Subject = "Employee Insurance Form";
                    string Body = $@"
<p>Dear {employee.EmployeeName},</p>
<p>We hope this message finds you well. Please fill out the following insurance form to update your insurance details:</p>
<p><a href='{employee.ReturnUrl.Replace("emp", "emp/insurance")}'>Click here to fill out the insurance form</a></p>
<p>Thank you,</p>
<p>{employee.Company?.Name}</p>
<!-- Invisible tracking image -->
<img src='https://localhost:7019/track/{employee.SecurityStamp}' style='display:none;' alt='' />
";

                    try
                    {
                        await _mailService.SendEmailAsync(To, Subject, Body);
                    }
                    catch (Exception ex)
                    {
                        result.IsSuccess = false;
                        result.Message = $"Failed to send email to {To}: {ex.Message}";
                        return result;
                    }

                }
                else
                {
                    // Skip processing for this employee
                    continue;
                }

            }
            result.IsSuccess = true;
            result.Message = "Form successfully shared with employees.";
            return result;
        }

        #endregion

        [HttpGet("track/{key}")]
        public IActionResult TrackEmail(string key)
        {
            // Log the email open event or handle it as needed
            LogEmailOpened(key);

            // Return a transparent 1x1 pixel image
            var transparentImage = new byte[]
            {
            0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0xff, 0x00, 0xff, 0xff, 0xff, 0x00, 0x00,
            0x00, 0x21, 0xf9, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
            0x00, 0x02, 0x02, 0x44, 0x01, 0x00, 0x3b
            };
            return File(transparentImage, "image/gif");
        }

        private void LogEmailOpened(string key)
        {
            // Implement your logic to log or track the email open event
            // For example, store the event in a database
            // _yourDatabaseService.LogEmailOpen(key);
        }
    }
}
