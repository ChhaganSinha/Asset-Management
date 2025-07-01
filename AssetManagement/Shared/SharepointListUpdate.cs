using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto
{
    public class SharepointListUpdate
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string AadharNumber { get; set; } = string.Empty;
        public bool IsCMSAdmin { get; set; }
        public DateTime DOB { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public string EmployeeID { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ? Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EMSManagerID { get; set; } = string.Empty;
        public string EMSManagerName { get; set; } = string.Empty;
        public string EMSManagerEmail { get; set; } = string.Empty;
        public int MonthNo { get; set; }
        public int LeaveBalanceWF { get; set; }
        public string HOD { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsDualDept { get; set; }
        public string Phoneno { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public bool IsEngineer { get; set; }
        public string AadhaarCard { get; set; } = string.Empty;
        public string PanCard { get; set; } = string.Empty;
        public bool IsFinance { get; set; }
        public bool IsHigherAuthority { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public bool IsTKAdmin { get; set; }
        public string LMManagerID { get; set; } = string.Empty;
        public string LMManagerName { get; set; } = string.Empty;
        public string LMManagerEmail { get; set; } = string.Empty;
        public bool Portal { get; set; } 
        public bool LM { get; set; } 
        public bool EMS { get; set; } 
        public bool CMS { get; set; }
        public string SitePermission { get; set; } = string.Empty;
        public bool RunWF { get; set; } = true;
    }

}

