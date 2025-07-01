using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Models
{
    public class AssetTransferModel
    {
        public int AssetID { get; set; }
        public string SourceCompanyCode { get; set; } = string.Empty;
        public string TargetCompanyCode { get; set; } = string.Empty;
        public int SourceCompanyId { get; set; }
        public int TargetCompanyId { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
    }
}
