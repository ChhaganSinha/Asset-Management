using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models
{
    public class Zoffice : BaseEntity
    {
        [ForeignKey("SubOfficeId")]
        public int SubOfficeId { get; set; }
        public int Number { get; set; }
        public string zoName { get; set; } = string.Empty;
        public string zoAddress { get; set; } = string.Empty;
        public string zoCont { get; set; } = string.Empty;
        public string zoContPerson { get; set; } = string.Empty;
    }
}
