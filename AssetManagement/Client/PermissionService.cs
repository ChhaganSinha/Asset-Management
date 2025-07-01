using AssetManagement.Client.Services.Contracts;
using AssetManagement.Dto.RBAC;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class PermissionService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthorizeApi _authorizeApi; // Corrected to use interface

    public PermissionService(HttpClient httpClient, IAuthorizeApi authorizeApi)
    {
        _httpClient = httpClient;
        _authorizeApi = authorizeApi;
    }

    public async Task<PagePermissionDto> HasPermission(string permission)
    {
        // Call the GetPagePermissions method to get the detailed permissions
        var permissions = await _authorizeApi.GetPagePermissions(permission);

        // Return the permissions model
        return new PagePermissionDto
        {
            CanView = permissions.CanView,
            CanEdit = permissions.CanEdit,
            HasFullAccess = permissions.HasFullAccess
        };
    }

}
