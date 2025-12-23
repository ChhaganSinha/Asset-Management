using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CertificateController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CertificateController> _logger;

        public CertificateController(
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            ICertificateRepository certificateRepository,
            IWebHostEnvironment environment,
            ILogger<CertificateController> logger)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _certificateRepository = certificateRepository;
            _environment = environment;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<CertificateGenerationResponse>> GenerateCertificate(CertificateGenerationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var company = await _companyRepository.GetCompanyByIdAsync(employee.CompanyId);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            var templateName = ResolveTemplateName(company.CompanyCode, request.CertificateType);
            var templateFile = ResolveTemplateFile(templateName);
            if (templateFile == null)
            {
                return NotFound($"Certificate template not found: {templateName}");
            }

            byte[] fileBytes;
            await using (var templateStream = templateFile.CreateReadStream())
            await using (var memoryStream = new MemoryStream())
            {
                await templateStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var replacements = BuildReplacements(employee, request);
                    ReplacePlaceholders(document, replacements);
                }

                fileBytes = memoryStream.ToArray();
            }

            var fileNamePrefix = request.CertificateType == CertificateType.Relieving ? "RelievingLetter" : "ExperienceCertificate";
            var fileName = $"{fileNamePrefix}_{employee.EmployeeId}_{DateTime.UtcNow:yyyyMMdd}.docx";
            var record = new CertificateGenerationRecord
            {
                CompanyId = company.Id,
                EmployeeId = employee.Id,
                TemplateName = templateName,
                GeneratedFileName = fileName,
                GeneratedOn = DateTime.UtcNow,
                GeneratedBy = User?.Identity?.Name ?? string.Empty
            };

            var saveResult = await _certificateRepository.AddCertificateRecordAsync(record);
            if (!saveResult.IsSuccess)
            {
                _logger.LogError("Failed to save certificate record: {Message}", saveResult.Message);
                return StatusCode(500, "Failed to store certificate record.");
            }

            return new CertificateGenerationResponse
            {
                FileName = fileName,
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileContent = fileBytes
            };
        }

        private static Dictionary<string, string> BuildReplacements(Employee employee, CertificateGenerationRequest request)
        {
            var (addressLine1, addressLine2, city) = BuildAddress(employee);
            var dateOfJoin = employee.DateOfJoin.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var dateOfLeaving = employee.DateOfLeaving.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var duration = FormatEmploymentDuration(employee.DateOfJoin, employee.DateOfLeaving);

            return new Dictionary<string, string>
            {
                ["{LetterDate}"] = request.LetterDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                ["{Title}"] = request.Title,
                ["{EmployeeName}"] = employee.EmployeeName,
                ["{AddressLine1}"] = addressLine1,
                ["{AddressLine2}"] = addressLine2,
                ["{City}"] = city,
                ["{FatherName}"] = employee.fatherName,
                ["{Designation}"] = employee.Designation,
                ["{DateOfJoining}"] = dateOfJoin,
                ["{DateOfLeaving}"] = dateOfLeaving,
                ["{EmploymentDuration}"] = duration,
                ["{HeShe}"] = request.HeShe,
                ["{HisHer}"] = request.HisHer,
                ["{HimHer}"] = request.HimHer
            };
        }

        private static (string line1, string line2, string city) BuildAddress(Employee employee)
        {
            var address = string.IsNullOrWhiteSpace(employee.PermanentAddress)
                ? employee.CurrentAddress
                : employee.PermanentAddress;

            var parts = (address ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            var line1 = parts.Count > 0 ? parts[0] : string.Empty;
            var line2 = parts.Count > 1 ? parts[1] : string.Empty;
            var city = parts.Count > 2
                ? string.Join(", ", parts.Skip(2))
                : (!string.IsNullOrWhiteSpace(employee.PState) ? employee.PState : employee.CState);

            return (line1, line2, city ?? string.Empty);
        }

        private static string FormatEmploymentDuration(DateTime start, DateTime end)
        {
            if (end < start)
            {
                end = DateTime.Today;
            }

            var totalMonths = (end.Year - start.Year) * 12 + end.Month - start.Month;
            if (end.Day < start.Day)
            {
                totalMonths--;
            }

            if (totalMonths < 0)
            {
                totalMonths = 0;
            }

            var years = totalMonths / 12;
            var months = totalMonths % 12;

            var parts = new List<string>();
            if (years > 0)
            {
                parts.Add($"{years} year{(years == 1 ? string.Empty : "s")}");
            }

            if (months > 0)
            {
                parts.Add($"{months} month{(months == 1 ? string.Empty : "s")}");
            }

            return parts.Count > 0 ? string.Join(" ", parts) : "less than a month";
        }

        private string ResolveTemplateName(string? companyCode, CertificateType certificateType)
        {
            var templatePrefix = certificateType == CertificateType.Relieving ? "ReleivingLetter" : "ExpCertificate";
            if (!string.IsNullOrWhiteSpace(companyCode))
            {
                var candidate = $"{templatePrefix}{companyCode.Trim()}.docx";
                var candidateFile = _environment.WebRootFileProvider.GetFileInfo(Path.Combine("Private", candidate));
                if (candidateFile.Exists)
                {
                    return candidate;
                }
            }

            return $"{templatePrefix}.docx";
        }

        private static void ReplacePlaceholders(WordprocessingDocument document, IReadOnlyDictionary<string, string> replacements)
        {
            var body = document.MainDocumentPart?.Document.Body;
            if (body == null)
            {
                return;
            }

            foreach (var replacement in replacements)
            {
                ReplacePlaceholder(body, replacement.Key, replacement.Value);
            }

            document.MainDocumentPart?.Document.Save();
        }

        private static void ReplacePlaceholder(Body body, string placeholder, string replacement)
        {
            var textElements = body.Descendants<Text>().ToList();
            if (textElements.Count == 0)
            {
                return;
            }

            var fullText = string.Concat(textElements.Select(t => t.Text));
            var matchIndex = fullText.IndexOf(placeholder, StringComparison.Ordinal);
            while (matchIndex >= 0)
            {
                var remaining = placeholder.Length;
                var currentIndex = 0;
                var started = false;

                foreach (var textElement in textElements)
                {
                    var text = textElement.Text;
                    var length = text.Length;

                    if (currentIndex + length <= matchIndex)
                    {
                        currentIndex += length;
                        continue;
                    }

                    var startIndex = Math.Max(0, matchIndex - currentIndex);
                    var charactersToConsume = Math.Min(length - startIndex, remaining);

                    if (!started)
                    {
                        textElement.Text = text.Substring(0, startIndex) + replacement + text.Substring(startIndex + charactersToConsume);
                        started = true;
                    }
                    else
                    {
                        textElement.Text = text.Remove(startIndex, charactersToConsume);
                    }

                    remaining -= charactersToConsume;
                    currentIndex += length;

                    if (remaining <= 0)
                    {
                        break;
                    }
                }

                fullText = string.Concat(textElements.Select(t => t.Text));
                matchIndex = fullText.IndexOf(placeholder, StringComparison.Ordinal);
            }
        }

        private IFileInfo? ResolveTemplateFile(string templateName)
        {
            var templateFile = _environment.WebRootFileProvider.GetFileInfo(Path.Combine("Private", templateName));
            if (templateFile.Exists)
            {
                return templateFile;
            }

            var clientTemplatePath = Path.Combine("_content", "AssetManagement.Client", "Private", templateName);
            templateFile = _environment.WebRootFileProvider.GetFileInfo(clientTemplatePath);
            if (templateFile.Exists)
            {
                return templateFile;
            }

            _logger.LogWarning("Certificate template not found. Checked paths: {PrivatePath}, {ClientPath}",
                Path.Combine("Private", templateName),
                clientTemplatePath);
            return null;
        }
    }
}
