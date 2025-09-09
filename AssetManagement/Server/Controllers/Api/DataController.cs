using AssetManagement.Dto;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using AssetManagement.Server.EmailService;
using AssetManagement.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        readonly IDataRepository _dataRepository;
        readonly IEmployeeRepository _employeeRepository;
        readonly ICompanyRepository _companyRepository;
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly OnboardingConfirmation _OnboardingConfirmation;
        public DataController(ILogger<DataController> logger, IConfiguration appConfig, IDataRepository dataRepository, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMailService mailService, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, OnboardingConfirmation onboardingConfirmation) : base()
        {
            _dataRepository = (DataRepository?)dataRepository;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _configuration = appConfig;
            _mailService = mailService;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _OnboardingConfirmation = onboardingConfirmation;
        }
        #region Data
        [HttpPost]
        [Route("sp-updateEmployee")]
        public async Task<ApiResponse<Employee>> UpdateEmployee(Employee data)
        {
            return await _dataRepository.UpdateEmployee(data);
        }

        [HttpGet("GetEmployeeByEmail")]
        public async Task<IActionResult> GetEmployeeByEmail([FromQuery] string email)
        {
            var employee =  await _dataRepository.GetEmployeeByEmail(email);
            return Ok(employee);
        }

        [HttpGet("GetAllocationByEmail")]
        public async Task<IActionResult> GetAllocationByEmail([FromQuery] string email)
        {
            var allocation =   await _dataRepository.GetAllocationByEmail(email);
            return Ok(allocation);
        }

        #endregion

    }
}
