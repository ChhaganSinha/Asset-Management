using AssetManagement.DataContext.Models;
using AssetManagement.DataContext;
using AssetManagement.Dto.RBAC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AuthDbContext _context;

        public RolesController(RoleManager<ApplicationRole> roleManager, AuthDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = roles.Select(r => new RoleViewModel { Name = r.Name }).ToList();
            return Ok(roleViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var role = new ApplicationRole { Name = request.Name };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            return Ok();
        }

        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermission([FromBody] AssignPermissionRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Handle permission assignment logic here.

            return Ok();
        }

        [HttpGet("with-permissions")]
        public async Task<IActionResult> GetRolesWithPermissions()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = new List<RoleViewModel>();

            foreach (var role in roles)
            {
                var roleViewModel = new RoleViewModel { Name = role.Name };

                var permissions = await _context.PagePermissions
                    .Where(p => p.ApplicationRoleId == role.Id)
                    .Select(p => new PagePermissionViewModel
                    {
                        PageName = p.PageName,
                        CanView = p.CanView,
                        CanEdit = p.CanEdit,
                        HasFullAccess = p.HasFullAccess
                    })
                    .ToListAsync();

                roleViewModel.PagePermissions.AddRange(permissions);

                // For demonstration, assuming `SelectedPermission` is a string.
                // Adjust accordingly if it involves more complex logic.
                roleViewModel.SelectedPermission = permissions.Any(p => p.HasFullAccess) ? "FullAccess" :
                                                    permissions.Any(p => p.CanEdit) ? "Edit" : "View";

                roleViewModels.Add(roleViewModel);
            }

            // Ensure that all roles have default false permissions if not set
            foreach (var role in roleViewModels)
            {
                foreach (var page in allPages)
                {
                    if (!role.PagePermissions.Any(p => p.PageName == page))
                    {
                        role.PagePermissions.Add(new PagePermissionViewModel { PageName = page, CanView = false, CanEdit = false, HasFullAccess = false });
                    }
                }
            }

            return Ok(roleViewModels);
        }

        public class CreateRoleRequest
        {
            public string Name { get; set; }
        }

        public class AssignPermissionRequest
        {
            public string RoleName { get; set; }
            public string Permission { get; set; }
        }

        private List<string> allPages = new List<string> { };
    }
}
