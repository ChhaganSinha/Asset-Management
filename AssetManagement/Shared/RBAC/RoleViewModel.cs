using System.Collections.Generic;

namespace AssetManagement.Dto.RBAC
{
    public class RoleViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string SelectedPermission { get; set; } = string.Empty;
        public List<PagePermissionViewModel> PagePermissions { get; set; } = new List<PagePermissionViewModel>();
    }

    public class PagePermissionViewModel
    {
        public string PageName { get; set; } = string.Empty;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool HasFullAccess { get; set; }

        // Property to track manual overrides
        public bool IsManuallySet { get; set; } = false;

    }
}
