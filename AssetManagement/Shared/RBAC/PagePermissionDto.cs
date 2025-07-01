namespace AssetManagement.Dto.RBAC
{
    public class PagePermissionDto
    {
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool HasFullAccess { get; set; }
    }
}
