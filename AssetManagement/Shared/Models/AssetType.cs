using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Dto.Models
{
    public class AssetType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public virtual List<SubAsset> SubAssets { get; set; }
        public AssetType()
        {
            SubAssets = new List<SubAsset>();
        }
    }

    public class SubAsset
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AssetTypeId")]
        public int AssetTypeId { get; set; }
        public int Number { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
