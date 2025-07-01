using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AssetManagement.DataContext.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool HasFullAccess { get; set; }

        // Navigation property for PagePermissions
        public List<PagePermission> PagePermissions { get; set; } = new List<PagePermission>();
    }
}
