using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using AssetManagement.Server.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeOnboardingController : ControllerBase
    {
        private readonly IEmployeeOnboardingRepository _repository;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;
        public EmployeeOnboardingController(IEmployeeOnboardingRepository repository, IMailService mailService, IWebHostEnvironment env)
        {
            _repository = repository;
            _mailService = mailService;
            _env = env;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<EmployeeOnboardingDto> GetEmployeeOnboardingById(int id) => await _repository.GetEmployeeOnboardingById(id);

        [HttpPost("Confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> UpsertConfirmOnboardingAsync(OnBoardingConfirmationDto data)
        {
            if (data == null)
                return BadRequest("Invalid data received.");
            var result = await _repository.UpsertConfirmOnboardingAsync(data);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeOnboardingDto>> UpsertEmployeeOnboarding(EmployeeOnboardingDto data)
        {
            var result = await _repository.UpsertEmployeeOnboarding(data);
            if (result.IsSuccess)
            {
                string path;
#if DEBUG
                path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent!.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\OnboardingForm.txt";
#else
                path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/OnboardingForm.txt");
#endif
                var raw = System.IO.File.ReadAllText($"{path}");
                var contents = raw.Replace("<###RETURN_URL###>", result.Result.ReturnUrl)
                    .Replace("<###CandidateName###>", result.Result.Name)
                    .Replace("<###CompanyName###>", result.Result.CompanyName)
                    .Replace("<###HRName###>", "Pradeep Sharma")
                    .Replace("<###HRMobile###>", "9312291234")
                    .Replace("<###ManagerName###>", result.Result.ReportingTo)
                    .Replace("<###ManagerMobile###>", result.Result.ManagerMobile);
                await _mailService.SendEmailAsync(result.Result.ExternalEmailId, "AssetManagement Application Onboarding Form", contents);
            }
            return result;
        }

        [HttpPost("ByKey")]
        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataByKey(GenericApiRequest<string> request) => await _repository.GetOnboardingDataByKey(request.Param);

        [HttpPost("ById")]
        public async Task<ApiResponse<EmployeeOnboardingDto>> GetOnboardingDataById(GenericApiRequest<string> request) => await _repository.GetOnboardingDataByID(request.Param);

        [HttpGet("form/{id}")]
        public async Task<EmployeeOnboardingDto> GetEmployeeonboardingById(int id) => await _repository.GetEmployeeonboardingById(id);

        [HttpPost("Files")]
        [AllowAnonymous]
        public async Task<ApiResponse<EmployeeFilesMapping>> UpsertEmployeeFiles(EmployeeFilesMapping data) => await _repository.UpsertEmployeeFilesAsync(data);

        [HttpGet("Files/{id}")]
        [AllowAnonymous]
        public async Task<EmployeeFilesMapping> EmployeeFilesById(int id) => await _repository.GetEmployeeFilesById(id);

        [HttpGet("AllFiles")]
        public async Task<IEnumerable<EmployeeFilesMapping>> GetAllEmployeeFileMap() => await _repository.GetAllEmployeeFileMap();

        [HttpGet("ShareForm/{id}")]
        public async Task<ApiResponse<EmployeeOnboardingDto>> ShareFormWithOnboardeeViaEmail(int id)
        {
            var result = new ApiResponse<EmployeeOnboardingDto>();
            var onboardee = await _repository.ShareFormWithOnboardeeViaEmail(id);
            if (onboardee != null)
            {
                string path;
#if DEBUG
                path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent!.FullName + "\\AssetManagement\\Client\\wwwroot\\EmailTemplets\\OnboardingForm.txt";
#else
                path = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplets/OnboardingForm.txt");
#endif
                var raw = System.IO.File.ReadAllText($"{path}");
                var contents = raw.Replace("<###RETURN_URL###>", onboardee.ReturnUrl)
                    .Replace("<CandidateName>", onboardee.Name)
                    .Replace("<CompanyName>", onboardee.CompanyName)
                    .Replace("<HRContact>", "");
                try
                {
                    await _mailService.SendEmailAsync(onboardee.ExternalEmailId, $"Welcome to {onboardee.CompanyName}! Please Complete the Onboarding Form", contents);
                    result.IsSuccess = true;
                    result.Message = "Successfully shared with candidate";
                    result.Result = onboardee;
                }
                catch (Exception ex)
                {
                    result.IsSuccess = true;
                    result.Message = ex.Message;
                }
            }
            return result;
        }
    }
}
