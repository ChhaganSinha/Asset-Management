CREATE TABLE EmployeeInsurance (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Relation INTEGER NOT NULL,
    DOB DATE NOT NULL,
    Aadhaar TEXT NOT NULL,
    Age INTEGER NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP,

    -- Foreign Key Constraint
    FOREIGN KEY (EmployeeId) REFERENCES Employee(Id) ON DELETE CASCADE
);

-- Optional: Add indexes for faster queries if needed
CREATE INDEX idx_employeeid ON EmployeeInsurance(EmployeeId);