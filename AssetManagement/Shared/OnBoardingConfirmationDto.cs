using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto
{
    public class OnBoardingConfirmationDto
    {
        public int Id { get; set; }

        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Desigation { get; set; }

        public string JoinStatus { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime TempDateOfJoin { get; set; } = DateTime.MinValue;
    }
}
