using AssetManagement.DataContext;
using AssetManagement.Dto;
using AssetManagement.Dto.Dashboard;
using AssetManagement.Dto.Models;
using AssetManagement.Repositories;
using AssetManagement.Server.EmailService;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : Controller
    {
        readonly IReportRepository _reportRepository;
        readonly Microsoft.Extensions.Logging.ILogger _logger;
        readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;

        public ReportController(ILogger<ReportController> logger, IConfiguration appConfig, IReportRepository reportRepository, IMailService mailService, IWebHostEnvironment env) : base()
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _configuration = appConfig;
            _mailService = mailService;
            _env = env;
        }

        #region ReportGenerate

        [HttpGet("AllocationlogExcelReport")]
        public async Task<IActionResult> AllocationlogExcelReport([FromQuery] AllocationlogReportGenerate data)
        {
           
            var allocationLogs = await _reportRepository.GetAllAllocationLog();
            if (data.CompanyCode && !(data.CompanyCodeSelected[0] == null))
            {
                var companyCodeString = string.Join(",", data.CompanyCodeSelected);
                var companyCodes = companyCodeString.Split(',');
                allocationLogs = allocationLogs.Where(o => companyCodes.Any(companyCode => companyCode == o.CompanyCode)).ToList();
            }

            if (data.AssetType && !(data.AssetTypeSelected[0] == null))
            {
                var assetTypeString = string.Join(",", data.AssetTypeSelected);
                var assetTypes = assetTypeString.Split(',');
                allocationLogs = allocationLogs.Where(o => assetTypes.Any(assetType => assetType == o.AssetType)).ToList();
            }
            if (data.AllocationType && data.AllocationTypeSelected != null && !data.AllocationTypeSelected.Contains("Select"))
            {
                var allocationTypeString = string.Join(",", data.AllocationTypeSelected);
                var allocationTypes = allocationTypeString.Split(',');
                //allocationLogs = allocationLogs.Where(o => data.AllocationTypeSelected?.Contains(o.AllocationType.ToString()) ?? false).ToList();
                allocationLogs = allocationLogs.Where(o => allocationTypes.Any(allocationType => allocationType == o.AllocationType.ToString())).ToList();
            }
            if (data.IssueDate && data.IssueDateRangeStart != DateTime.MinValue && data.IssueDateRangedEnd != DateTime.MinValue)
            {
                allocationLogs = allocationLogs.Where(o => o.IssueDate >= data.IssueDateRangeStart && o.IssueDate <= data.IssueDateRangedEnd).ToList();
            }
            if (data.IssueTill && data.IssueTillRangeStart != DateTime.MinValue && data.IssueTillRangeEnd != DateTime.MinValue)
            {
                allocationLogs = allocationLogs.Where(o => o.IssueTill >= data.IssueTillRangeStart && o.IssueDate <= data.IssueTillRangeEnd).ToList();
            }
            if (data.ReturnDate && data.ReturnDateRangeStart != DateTime.MinValue && data.ReturnDateRangeEnd != DateTime.MinValue)
            {
                allocationLogs = allocationLogs.Where(o => o.ReturnDate >= data.ReturnDateRangeStart && o.IssueDate <= data.ReturnDateRangeEnd).ToList();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Allocation Log" };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Generate header row
                    Row headerRow = new Row();
                    if (data.CompanyCode == true) { headerRow.Append(ConstructCell("CompanyCode")); }
                    if (data.AssetType == true) { headerRow.Append(ConstructCell("Asset Type")); }
                    if (data.Brand == true) { headerRow.Append(ConstructCell("Asset Brand")); }
                    if (data.Model == true) { headerRow.Append(ConstructCell("Asset Model")); }
                    if (data.Description == true) { headerRow.Append(ConstructCell("Asset Description")); }
                    if (data.EmployeeCompanyCode == true) { headerRow.Append(ConstructCell("Employee CompanyCode")); }
                    if (data.EmployeeName == true) { headerRow.Append(ConstructCell("Employee Name")); }
                    if (data.EmployeeEmail == true) { headerRow.Append(ConstructCell("Employee Email")); }
                    if (data.EmployeeMobileNumber == true) { headerRow.Append(ConstructCell("Mobile Number")); }
                    if (data.EmployeeDesignation == true) { headerRow.Append(ConstructCell("Designation")); }
                    if (data.IssueDate == true) { headerRow.Append(ConstructCell("Issue Date")); }
                    if (data.AllocationType == true) { headerRow.Append(ConstructCell("Allocation Type")); }
                    if (data.IssueTill == true) { headerRow.Append(ConstructCell("Issue Till")); }
                    if (data.ReturnDate == true) { headerRow.Append(ConstructCell("Return Date")); }
                    
                    sheetData.AppendChild(headerRow);

                    // Generate data rows
                    foreach (var log in allocationLogs)
                    {
                        Row dataRow = new Row();
                        if (data.CompanyCode == true) { dataRow.Append(ConstructCell(log.CompanyCode?.ToString())); }
                        if (data.AssetType == true) { dataRow.Append(ConstructCell(log.AssetType?.ToString())); }
                        if (data.Brand == true) { dataRow.Append(ConstructCell(log.AssetBrand.ToString())); }
                        if (data.Model == true) { dataRow.Append(ConstructCell(log.AssetModel.ToString())); }
                        if (data.Description == true) { dataRow.Append(ConstructCell(log.AssetDescription.ToString())); }
                        if (data.EmployeeCompanyCode == true) { dataRow.Append(ConstructCell(log.EmployeeCompanyCode.ToString())); }
                        if (data.EmployeeName == true) { dataRow.Append(ConstructCell(log.EmployeeName.ToString())); }
                        if (data.EmployeeEmail == true) { dataRow.Append(ConstructCell(log.EmployeeEmail.ToString())); }
                        if (data.EmployeeMobileNumber == true) { dataRow.Append(ConstructCell(log.EmployeeMobileNumber.ToString())); }
                        if (data.EmployeeDesignation == true) { dataRow.Append(ConstructCell(log.EmployeeDesignation.ToString())); }
                        if (data.IssueDate == true) { dataRow.Append(ConstructCell(log.IssueDate.ToString("dd/MMM/yyyy"))); }
                        if (data.AllocationType == true) { dataRow.Append(ConstructCell(log.AllocationType.ToString())); }
                        if (data.IssueTill == true) { dataRow.Append(ConstructCell(log.IssueTill?.ToString("dd/MMM/yyyy"))); }
                        if (data.ReturnDate == true) { dataRow.Append(ConstructCell(log.ReturnDate?.ToString("dd/MMM/yyyy"))); }

                        sheetData.AppendChild(dataRow);
                    }


                    workbookPart.Workbook.Save();
                    //document.Close();
                }

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "allocation_log.xlsx");
            }
        }

        private Cell ConstructCell(string value)
        {
            return new Cell(new CellValue(value)) { DataType = CellValues.String };
        }

        [HttpPost("EmployeeExcelReport")]
        public async Task<IActionResult> ExportEmployeeReport(EmployeeFilterModel model)
        {

            var responce = await _reportRepository.GetFilteredEmployeeReport(model);


            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "EmployeeReport" };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Generate header row
                    Row headerRow = new Row();
                    headerRow.Append(ConstructCell("CompanyCode"));
                    headerRow.Append(ConstructCell("Emp. ID"));
                    headerRow.Append(ConstructCell("Name"));
                    headerRow.Append(ConstructCell("Father"));
                    headerRow.Append(ConstructCell("Status"));
                    headerRow.Append(ConstructCell("Email"));
                    headerRow.Append(ConstructCell("ExternalEmail"));
                    headerRow.Append(ConstructCell("Mobile"));
                    headerRow.Append(ConstructCell("Address"));
                    headerRow.Append(ConstructCell("Country"));
                    headerRow.Append(ConstructCell("State"));
                    headerRow.Append(ConstructCell("Pin"));
                    headerRow.Append(ConstructCell("Aadhar"));
                    headerRow.Append(ConstructCell("PAN"));
                    headerRow.Append(ConstructCell("UAN"));
                    headerRow.Append(ConstructCell("EmergencyNumber"));
                    headerRow.Append(ConstructCell("Educations"));
                    headerRow.Append(ConstructCell("Skills"));
                    headerRow.Append(ConstructCell("Category"));
                    headerRow.Append(ConstructCell("Department"));
                    headerRow.Append(ConstructCell("DOB"));
                    headerRow.Append(ConstructCell("DateOfJoin"));
                    headerRow.Append(ConstructCell("DateOfLeaving"));
                    headerRow.Append(ConstructCell("BankName"));
                    headerRow.Append(ConstructCell("Acc Number"));
                    headerRow.Append(ConstructCell("IFSC"));

                    sheetData.AppendChild(headerRow);

                    // Generate data rows
                    foreach (var log in responce)
                    {
                        Row dataRow = new Row();
                        dataRow.Append(ConstructCell(log.CompanyCode));
                        dataRow.Append(ConstructCell(log.EmployeeId));
                        dataRow.Append(ConstructCell(log.EmployeeName));
                        dataRow.Append(ConstructCell(log.fatherName));
                        dataRow.Append(ConstructCell(log.Status.ToString()));
                        dataRow.Append(ConstructCell(log.EmailId));
                        dataRow.Append(ConstructCell(log.ExternalEmailId));
                        dataRow.Append(ConstructCell(log.MobileNumber));
                        dataRow.Append(ConstructCell(log.PermanentAddress));
                        dataRow.Append(ConstructCell(log.PCountry));
                        dataRow.Append(ConstructCell(log.PState));
                        dataRow.Append(ConstructCell(log.PPin));
                        dataRow.Append(ConstructCell(log.AadhaarNumber));
                        dataRow.Append(ConstructCell(log.PANNumber));
                        dataRow.Append(ConstructCell(log.UANNo));
                        dataRow.Append(ConstructCell(log.EmergencyContactNumber));
                        dataRow.Append(ConstructCell(log.EmployeeEducation));
                        dataRow.Append(ConstructCell(log.Skills));
                        dataRow.Append(ConstructCell(log.EmployeeCategory));
                        dataRow.Append(ConstructCell(log.Department));
                        dataRow.Append(ConstructCell(log.DateOfBirth.ToString("dd/MMM/yyyy")));
                        dataRow.Append(ConstructCell(log.DateOfJoin.ToString("dd/MMM/yyyy")));
                        dataRow.Append(ConstructCell(log.DateOfLeaving.ToString("dd/MMM/yyyy")));
                        dataRow.Append(ConstructCell(log.EmpBankName));
                        dataRow.Append(ConstructCell(log.EmpBankAccNumber));
                        dataRow.Append(ConstructCell(log.EmpBankIfsc));


                        sheetData.AppendChild(dataRow);
                    }


                    workbookPart.Workbook.Save();
                    //document.Close();
                }

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"EmployeeReport_{DateTime.Now}.xlsx");
            }         
        }

        [HttpPost("AssetExcelReport")]
        public async Task<IActionResult> AssetEmployeeReport(AssetFilterModel model)
        {

            var responce = await _reportRepository.GetFilteredAssetReport(model);


            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "AssetReport" };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Generate header row
                    Row headerRow = new Row();
                    headerRow.Append(ConstructCell("CompanyCode"));
                    headerRow.Append(ConstructCell("Asset Type"));
                    headerRow.Append(ConstructCell("Brand"));
                    headerRow.Append(ConstructCell("Model"));
                    headerRow.Append(ConstructCell("Status"));
                    headerRow.Append(ConstructCell("EmployeeName"));
                    headerRow.Append(ConstructCell("Employee Email"));
                    headerRow.Append(ConstructCell("Serial Number"));
                    headerRow.Append(ConstructCell("MAC Address"));
                    headerRow.Append(ConstructCell("Aquire Date"));
                    headerRow.Append(ConstructCell("Discard Date"));

                    sheetData.AppendChild(headerRow);

                    // Generate data rows
                    foreach (var log in responce)
                    {
                        Row dataRow = new Row();
                        dataRow.Append(ConstructCell(log.CompanyCode));
                        dataRow.Append(ConstructCell(log.AssetType));
                        dataRow.Append(ConstructCell(log.Brand));
                        dataRow.Append(ConstructCell(log.Model));
                        dataRow.Append(ConstructCell(log.Status.ToString()));
                        dataRow.Append(ConstructCell(log.EmployeeName));
                        dataRow.Append(ConstructCell(log.EmployeeEmail));
                        dataRow.Append(ConstructCell(log.SerialNumber));
                        dataRow.Append(ConstructCell(log.MacAddress));
                        dataRow.Append(ConstructCell(log.AcquireDate.ToString("dd/MMM/yyyy")));
                        dataRow.Append(ConstructCell(log.DiscardDate?.ToString("dd/MMM/yyyy")));


                        sheetData.AppendChild(dataRow);
                    }


                    workbookPart.Workbook.Save();
                    //document.Close();
                }

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"EmployeeReport_{DateTime.Now}.xlsx");
            }
        }
        #endregion
    }
}
