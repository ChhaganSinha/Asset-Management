namespace AssetManagement.Client.Shared;

public class ClientBreadcrumbItem
{
    public string Text { get; set; } = string.Empty;
    public string? Url { get; set; }
    public bool IsActive { get; set; }
}
