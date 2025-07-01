using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Dto.Models
{
    public class SubOffice : BaseEntity
    {
        [Required]
        public int CompanyId { get; set; }
        public int Number { get; set; }
        public string subName { get; set; } = string.Empty;
        public string subAddress { get; set; } = string.Empty;
        public string subCont { get; set; } = string.Empty;
        public string subContPerson { get; set; } = string.Empty;
        public string? SubOficeZone { get; set; }
        public virtual List<Zoffice> Zoffice { get; set; }
        public SubOffice()
        {
            Zoffice = new List<Zoffice>();
        }
    }
}
