ALTER TABLE EmployeeOnboarding ADD COLUMN CreatedBy VARCHAR(255);
ALTER TABLE EmployeeOnboarding ADD COLUMN ModifiedBy VARCHAR(255);

ALTER TABLE EmployeeOnboarding ADD COLUMN ManagerEmail VARCHAR NULL;
