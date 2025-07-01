using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class EmployeeSkillMapping : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int EmployeeSkillId { get; set; }
        public string Skill { get; set; } = string.Empty;
    }
}
