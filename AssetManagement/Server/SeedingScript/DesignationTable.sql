CREATE TABLE EmployeeInsurance (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId INTEGER NOT NULL,
    InsuranceType TEXT NOT NULL,
    InsuranceNumber TEXT,
    ExpiryDate DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Key Constraint
    FOREIGN KEY (EmployeeId) REFERENCES Employee(Id) ON DELETE CASCADE
);

-- Optional: Add indexes for faster queries if needed
CREATE INDEX idx_employeeid ON EmployeeInsurance(EmployeeId);
