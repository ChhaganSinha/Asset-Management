use `asset`;

CREATE TABLE EmployeeOnboarding (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  CompanyCode VARCHAR(25) NOT NULL,
  Name VARCHAR(55) NOT NULL,
  fatherName VARCHAR(55) NOT NULL,
  ExternalEmailId VARCHAR(55) NOT NULL,
  MobileNumber VARCHAR(13) NOT NULL,
  EmergencyContactNumber VARCHAR(13) NOT NULL,
  AadhaarNumber VARCHAR(12) NOT NULL,
  DateOfBirth DATE NOT NULL,
  EmployeeGraduation VARCHAR(55),
  EmployeeEducationDetails VARCHAR(255) NOT NULL,
  PANNumber VARCHAR(25) NOT NULL,
  UANNo VARCHAR(25) NOT NULL,
  PermanentAddress VARCHAR(255) NOT NULL,
  PCountry VARCHAR(25) NOT NULL,
  PState VARCHAR(25) NOT NULL,
  PPin VARCHAR(25) NOT NULL,
  checkbox BOOLEAN NOT NULL,
  CurrentAddress VARCHAR(255) NOT NULL,
  CCountry VARCHAR(25) NOT NULL,
  CState VARCHAR(25) NOT NULL,
  CPin VARCHAR(25) NOT NULL,
  EmpBankName VARCHAR(55) NOT NULL,
  EmpAccountName VARCHAR(55) NOT NULL,
  EmpBankAccNumber VARCHAR(55) NOT NULL,
  EmpBankIfsc VARCHAR(25) NOT NULL,
  SecurityStamp VARCHAR(255) NOT NULL,
  ReturnUrl VARCHAR(255) NOT NULL,
  Date DATE NOT NULL,
  ResponceDate DATE NOT NULL,
  IsReplied BOOLEAN NOT NULL,
  AadhaarFile VARCHAR(255) NOT NULL,
  PanFile VARCHAR(255) NOT NULL,
  BankPassbookFile VARCHAR(255) NOT NULL,
  CertificateFile VARCHAR(255) NOT NULL,
  ProfilePhotoFile VARCHAR(255) NOT NULL,
  SkillIDs LONGTEXT NOT NULL
);


--Date 19 July 2024
alter table Employee add COLUMN SharepointSync bool DEFAULT true;

ALTER TABLE EmployeeOnboarding ADD COLUMN TempDateOfJoin DATE;
-- Update existing rows with a default date or desired value
UPDATE EmployeeOnboarding SET TempDateOfJoin = '1900-01-01' WHERE TempDateOfJoin IS NULL;
-- Alter the column to be NOT NULL
ALTER TABLE EmployeeOnboarding RENAME COLUMN TempDateOfJoin TO TempDateOfJoin_temp;
ALTER TABLE EmployeeOnboarding ADD COLUMN TempDateOfJoin DATE NOT NULL DEFAULT '1900-01-01';
UPDATE EmployeeOnboarding SET TempDateOfJoin = TempDateOfJoin_temp;
ALTER TABLE EmployeeOnboarding DROP COLUMN TempDateOfJoin_temp;

