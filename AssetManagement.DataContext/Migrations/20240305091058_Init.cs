using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllocationLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AllocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AllocationType = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueTill = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    AssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    AssetType = table.Column<string>(type: "TEXT", nullable: true),
                    AssetBrand = table.Column<string>(type: "TEXT", nullable: true),
                    AssetModel = table.Column<string>(type: "TEXT", nullable: true),
                    AssetDescription = table.Column<string>(type: "TEXT", nullable: true),
                    AssetAquireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    EmployeeCompanyCode = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeMobileNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeDesignation = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeDateOfJoin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmployeeDateOfleaving = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GSTNNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    PIN = table.Column<string>(type: "TEXT", nullable: false),
                    BankName = table.Column<string>(type: "TEXT", nullable: false),
                    AccNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IFSC = table.Column<string>(type: "TEXT", nullable: false),
                    SwiftCode = table.Column<string>(type: "TEXT", nullable: false),
                    IncorporationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IncorporationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TANNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PanNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PFNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ESINumber = table.Column<string>(type: "TEXT", nullable: false),
                    UAMNumber = table.Column<string>(type: "TEXT", nullable: false),
                    StartupNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BankDetail = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationWith = table.Column<int>(type: "INTEGER", nullable: false),
                    ContactPerson = table.Column<string>(type: "TEXT", nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmailId = table.Column<string>(type: "TEXT", nullable: false),
                    IsEngazed = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmployeeEngazedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetEngazedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOnboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    fatherName = table.Column<string>(type: "TEXT", nullable: false),
                    ExternalEmailId = table.Column<string>(type: "TEXT", nullable: false),
                    MobileNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmergencyContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AadhaarNumber = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmployeeGraduation = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeEducationDetails = table.Column<string>(type: "TEXT", nullable: false),
                    PANNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UANNo = table.Column<string>(type: "TEXT", nullable: false),
                    PermanentAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PCountry = table.Column<string>(type: "TEXT", nullable: false),
                    PState = table.Column<string>(type: "TEXT", nullable: false),
                    PPin = table.Column<string>(type: "TEXT", nullable: false),
                    checkbox = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentAddress = table.Column<string>(type: "TEXT", nullable: false),
                    CCountry = table.Column<string>(type: "TEXT", nullable: false),
                    CState = table.Column<string>(type: "TEXT", nullable: false),
                    CPin = table.Column<string>(type: "TEXT", nullable: false),
                    EmpBankName = table.Column<string>(type: "TEXT", nullable: false),
                    EmpAccountName = table.Column<string>(type: "TEXT", nullable: false),
                    EmpBankAccNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmpBankIfsc = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: false),
                    ReturnUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResponceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsReplied = table.Column<bool>(type: "INTEGER", nullable: false),
                    AadhaarFile = table.Column<string>(type: "TEXT", nullable: false),
                    PanFile = table.Column<string>(type: "TEXT", nullable: false),
                    BankPassbookFile = table.Column<string>(type: "TEXT", nullable: false),
                    CertificateFile = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePhotoFile = table.Column<string>(type: "TEXT", nullable: false),
                    SkillIDs = table.Column<string>(type: "TEXT", nullable: false),
                    OtherSkills = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOnboarding", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkillMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeSkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Skill = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Skill = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubOffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    subName = table.Column<string>(type: "TEXT", nullable: false),
                    subAddress = table.Column<string>(type: "TEXT", nullable: false),
                    subCont = table.Column<string>(type: "TEXT", nullable: false),
                    subContPerson = table.Column<string>(type: "TEXT", nullable: false),
                    SubOficeZone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubOffice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZoneArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZoneName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubAsset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssetTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubAsset_AssetType_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: false),
                    AssetTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    AssetType = table.Column<string>(type: "TEXT", nullable: false),
                    SubAssetType = table.Column<string>(type: "TEXT", nullable: true),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    MacAddress = table.Column<string>(type: "TEXT", nullable: false),
                    AcquireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VendorName = table.Column<string>(type: "TEXT", nullable: false),
                    DiscardDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEngazed = table.Column<bool>(type: "INTEGER", nullable: false),
                    _AssetStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeName = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    EmailId = table.Column<string>(type: "TEXT", nullable: false),
                    ExternalEmailId = table.Column<string>(type: "TEXT", nullable: false),
                    MobileNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PermanentAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PCountry = table.Column<string>(type: "TEXT", nullable: false),
                    PState = table.Column<string>(type: "TEXT", nullable: false),
                    PPin = table.Column<string>(type: "TEXT", nullable: false),
                    checkbox = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentAddress = table.Column<string>(type: "TEXT", nullable: false),
                    CCountry = table.Column<string>(type: "TEXT", nullable: false),
                    CState = table.Column<string>(type: "TEXT", nullable: false),
                    CPin = table.Column<string>(type: "TEXT", nullable: false),
                    AadhaarNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PANNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UANNo = table.Column<string>(type: "TEXT", nullable: false),
                    EmergencyContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeEducation = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeEducationDetails = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeCategory = table.Column<string>(type: "TEXT", nullable: true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfJoin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOfLeaving = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Designation = table.Column<string>(type: "TEXT", nullable: false),
                    ReportingTo = table.Column<string>(type: "TEXT", nullable: false),
                    fatherName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmpBankName = table.Column<string>(type: "TEXT", nullable: false),
                    EmpAccountName = table.Column<string>(type: "TEXT", nullable: false),
                    EmpBankAccNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmpBankIfsc = table.Column<string>(type: "TEXT", nullable: false),
                    IsEngazed = table.Column<bool>(type: "INTEGER", nullable: false),
                    EngazeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: false),
                    ReturnUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsReplied = table.Column<bool>(type: "INTEGER", nullable: false),
                    OtherSkills = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zoffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubOfficeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    zoName = table.Column<string>(type: "TEXT", nullable: false),
                    zoAddress = table.Column<string>(type: "TEXT", nullable: false),
                    zoCont = table.Column<string>(type: "TEXT", nullable: false),
                    zoContPerson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoffice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zoffice_SubOffice_SubOfficeId",
                        column: x => x.SubOfficeId,
                        principalTable: "SubOffice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AllocationType = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueTill = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyCode = table.Column<string>(type: "TEXT", nullable: false),
                    AssetType = table.Column<string>(type: "TEXT", nullable: false),
                    AssetId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetModel = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeName = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeCompanyCode = table.Column<string>(type: "TEXT", nullable: true),
                    Checkbox = table.Column<bool>(type: "INTEGER", nullable: false),
                    Responce = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReturnUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocation_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allocation_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allocation_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFilesMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AadhaarFile = table.Column<string>(type: "TEXT", nullable: false),
                    PanFile = table.Column<string>(type: "TEXT", nullable: false),
                    BankPassbookFile = table.Column<string>(type: "TEXT", nullable: false),
                    CertificateFile = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePhotoFile = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFilesMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFilesMapping_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_AssetId",
                table: "Allocation",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_CompanyId",
                table: "Allocation",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_EmployeeId",
                table: "Allocation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_CompanyId",
                table: "Asset",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFilesMapping_EmployeeId",
                table: "EmployeeFilesMapping",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubAsset_AssetTypeId",
                table: "SubAsset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Zoffice_SubOfficeId",
                table: "Zoffice",
                column: "SubOfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocation");

            migrationBuilder.DropTable(
                name: "AllocationLog");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "EmployeeFilesMapping");

            migrationBuilder.DropTable(
                name: "EmployeeOnboarding");

            migrationBuilder.DropTable(
                name: "EmployeeSkillMapping");

            migrationBuilder.DropTable(
                name: "EmployeeSkills");

            migrationBuilder.DropTable(
                name: "SubAsset");

            migrationBuilder.DropTable(
                name: "Zoffice");

            migrationBuilder.DropTable(
                name: "ZoneArea");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "AssetType");

            migrationBuilder.DropTable(
                name: "SubOffice");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
