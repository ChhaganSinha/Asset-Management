
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DocumentFormat.OpenXml.InkML;
using AssetManagement.Server.Controllers.Api.Abstraction;
using AssetManagement.DataContext.Models;
using AssetManagement.DataContext;
using AssetManagement.Dto.Auth;
using AssetManagement.Dto.RBAC;
using Microsoft.Graph;
using Serilog.Core;
using AssetManagement.Server.EmailService;

namespace AssetManagement.Server.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly AuthDbContext _context;

        public AuthorizeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IMailService emailSender,
            AuthDbContext context,
            ILogger<AuthorizeController> logger)
            : base(logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            var user = await _userManager.FindByEmailAsync(parameters.UserEmail);
            if (user == null) return BadRequest("User does not exist");
            if (!user.IsActive)
            {
                return BadRequest("Your account is deactivated. Login denied.");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
            if (!signInResult.Succeeded) return BadRequest("Invalid password");

            await _signInManager.SignInAsync(user, parameters.RememberMe);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterParameters parameters)
        {
            try
            {
                var userCount = await _userManager.Users.CountAsync();

                var user = new ApplicationUser
                {
                    UserName = parameters.UserName,
                    Email = parameters.Email,
                    PhoneNumber = parameters.MobileNo,
                    Dob = parameters.Dob,
                    Gender = parameters.Gender,
                    Address = parameters.Address,
                    IsActive = parameters.IsActive
                };

                if (userCount == 0)
                {
                    var adminRole = await _roleManager.FindByNameAsync("Admin");
                    if (adminRole == null)
                    {
                        adminRole = new ApplicationRole { Name = "Admin" };
                        var createAdminRoleResult = await _roleManager.CreateAsync(adminRole);
                        if (!createAdminRoleResult.Succeeded)
                            return BadRequest(createAdminRoleResult.Errors.FirstOrDefault()?.Description);
                    }

                    var result = await _userManager.CreateAsync(user, parameters.Password);
                    if (!result.Succeeded)
                        return BadRequest(result.Errors.FirstOrDefault()?.Description);

                    var addAdminRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
                    if (!addAdminRoleResult.Succeeded)
                        return BadRequest(addAdminRoleResult.Errors.FirstOrDefault()?.Description);

                    // Return to login for the first user
                    return await Login(new LoginParameters
                    {
                        UserEmail = parameters.Email,
                        Password = parameters.Password
                    });
                }
                else
                {
                    var userRole = await _roleManager.FindByNameAsync("User");
                    if (userRole == null)
                    {
                        userRole = new ApplicationRole { Name = "User" };
                        var createUserRoleResult = await _roleManager.CreateAsync(userRole);
                        if (!createUserRoleResult.Succeeded)
                            return BadRequest(createUserRoleResult.Errors.FirstOrDefault()?.Description);
                    }

                    var result = await _userManager.CreateAsync(user, parameters.Password);
                    if (!result.Succeeded)
                        return BadRequest(result.Errors.FirstOrDefault()?.Description);

                    var addUserRoleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!addUserRoleResult.Succeeded)
                        return BadRequest(addUserRoleResult.Errors.FirstOrDefault()?.Description);

                    // Return success message for subsequent users
                    return Ok("User registered successfully.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest($"Failed to register user: {ex.Message}");
            }
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userInfo = new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = user.UserName,
                ExposedClaims = User.Claims.ToDictionary(c => c.Type, c => c.Value),
                Roles = userRoles.ToList(),
                Email = user.Email,
                MobileNo = user.PhoneNumber,
                Dob = user.Dob,
                Address = user.Address,
                Gender = user.Gender
            };

            return Ok(userInfo);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                IsActive = u.IsActive,
            }).ToList();

            return Ok(userViewModels);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove all roles from the user
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!removeRolesResult.Succeeded)
            {
                return BadRequest(removeRolesResult.Errors.FirstOrDefault()?.Description);
            }

            // Add the new role to the user
            var role = await _roleManager.FindByNameAsync(userViewModel.Role);
            if (role == null)
            {
                role = new ApplicationRole { Name = userViewModel.Role };
                var createRoleResult = await _roleManager.CreateAsync(role);
                if (!createRoleResult.Succeeded)
                {
                    return BadRequest(createRoleResult.Errors.FirstOrDefault()?.Description);
                }
            }

            // Update IsActive
            user.IsActive = userViewModel.IsActive;

            var addRoleResult = await _userManager.AddToRoleAsync(user, userViewModel.Role);
            if (!addRoleResult.Succeeded)
            {
                return BadRequest(addRoleResult.Errors.FirstOrDefault()?.Description);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UserCount()
        {
            var userCount = await _userManager.Users.CountAsync();
            return Ok(new { Count = userCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            // Fetch roles and ensure properties are correctly set
            var roles = await _roleManager.Roles
                .Select(r => new RoleViewModel
                {
                    Name = r.Name,
                    SelectedPermission = r.HasFullAccess ? "FullAccess" : r.CanEdit ? "Edit" : r.CanView ? "View" : string.Empty
                })
                .ToListAsync();

            // Log roles for debugging
            foreach (var role in roles)
            {
                Console.WriteLine($"Role: {role.Name}, Permission: {role.SelectedPermission}");
            }

            return Ok(roles);
        }




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ResetPassword resetPassword)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                // Check if the current password is correct
                var passwordCheck = await _userManager.CheckPasswordAsync(user, resetPassword.CurrentPassword);
                if (!passwordCheck)
                {
                    return BadRequest("Invalid current password");
                }

                // Change the user's password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, resetPassword.CurrentPassword, resetPassword.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    return Ok("Password changed successfully");
                }
                else
                {
                    return BadRequest("Failed to change password");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest($"Failed to change password: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UserDetailsUpdateParameters updateParameters)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                // Update username if provided
                if (!string.IsNullOrEmpty(updateParameters.NewUserName))
                {
                    user.UserName = updateParameters.NewUserName;
                }

                // Update email if provided
                if (!string.IsNullOrEmpty(updateParameters.NewEmail))
                {
                    // Check if the new email is unique
                    var existingUserWithEmail = await _userManager.FindByEmailAsync(updateParameters.NewEmail);
                    if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                    {
                        return BadRequest("Email is already in use by another user");
                    }

                    user.Email = updateParameters.NewEmail;
                }

                // Update mobile number if provided
                if (!string.IsNullOrEmpty(updateParameters.NewMobileNo))
                {
                    user.PhoneNumber = updateParameters.NewMobileNo;
                }

                // Update gender if provided
                if (!string.IsNullOrEmpty(updateParameters.NewGender.ToString()))
                {
                    user.Gender = updateParameters.NewGender;
                }

                // Update date of birth if provided
                if (updateParameters.NewDob != default)
                {
                    user.Dob = updateParameters.NewDob;
                }

                // Update address if provided
                if (!string.IsNullOrEmpty(updateParameters.NewAddress))
                {
                    user.Address = updateParameters.NewAddress;
                }

                // Update the user in the database
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return Ok("User details updated successfully");
                }
                else
                {
                    return BadRequest(updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to update user details");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest($"Failed to update user details: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RequestPasswordResetByEmail(ResetPasswordByAdmin resetPasswordByAdmin)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordByAdmin.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordByAdmin.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password reset successfully");
            }
            else
            {
                // Failed to reset password, handle errors
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Failed to reset password: {errors}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                Logger.LogError($"{request.Email} - user not found or email is not confirmed");
                return BadRequest("User not found or email is not confirmed");
            }

            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = $"{Request.Scheme}://{Request.Host.Host}:{(Request.Host.Port != 80 ? Request.Host.Port.ToString() : string.Empty)}/resetpassword/{code}";

                if (callbackUrl == null)
                    return BadRequest();

                await _emailSender.SendEmailAsync(
                    request.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                Logger.LogError($"{request.Code} - user not found or email is not confirmed");
                return BadRequest();
            }

            var code = WebEncoders.Base64UrlDecode(request.Code);
            var scode = Encoding.UTF8.GetString(code);

            var result = await _userManager.ResetPasswordAsync(user, scode, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault());

            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset Successfully",
                $"Your password was reset successfully");

            return Ok();
        }




        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> CreateRole([FromBody] RoleViewModel roleViewModel)
        {
            var role = new ApplicationRole { Name = roleViewModel.Name };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpDelete("{roleName}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            // Check if the role exists
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Check if any users are assigned this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole.Any())
            {
                return BadRequest($"Role '{roleName}' is assigned to users. Please unassign it first.");
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' deleted successfully.");
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }



        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> AssignPermission([FromBody] RoleViewModel roleViewModel)
        {
            var role = await _roleManager.Roles
                .Include(r => r.PagePermissions)
                .FirstOrDefaultAsync(r => r.Name == roleViewModel.Name);

            if (role == null)
            {
                return BadRequest("Role not found");
            }

            // Assign general permissions based on the SelectedPermission property
            switch (roleViewModel.SelectedPermission)
            {
                case "View":
                    role.CanView = true;
                    role.CanEdit = false;
                    role.HasFullAccess = false;
                    break;
                case "Edit":
                    role.CanView = true;
                    role.CanEdit = true;
                    role.HasFullAccess = false;
                    break;
                case "FullAccess":
                    role.CanView = true;
                    role.CanEdit = true;
                    role.HasFullAccess = true;
                    break;
                default:
                    return BadRequest("Invalid permission");
            }

            // Update existing page permissions or add new ones if they don't exist
            foreach (var pagePermission in roleViewModel.PagePermissions)
            {
                var existingPermission = role.PagePermissions.FirstOrDefault(p => p.PageName == pagePermission.PageName);
                if (existingPermission != null)
                {
                    // Update existing permission
                    existingPermission.CanView = pagePermission.CanView;
                    existingPermission.CanEdit = pagePermission.CanEdit;
                    existingPermission.HasFullAccess = pagePermission.HasFullAccess;
                }
                else
                {
                    // Add new permission if it doesn't exist
                    role.PagePermissions.Add(new AssetManagement.DataContext.Models.PagePermission
                    {
                        PageName = pagePermission.PageName,
                        CanView = pagePermission.CanView,
                        CanEdit = pagePermission.CanEdit,
                        HasFullAccess = pagePermission.HasFullAccess
                    });
                }
            }

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                // Optionally, persist the role changes to your database here
                // Example: _context.SaveChanges();

                return Ok();
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }





        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserPermissions([FromBody] PermissionRequest request)
        {
            try
            {
                Console.WriteLine($"Received request for permission: {request.Permission}");

                // Get the user ID from claims
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdString == null)
                {
                    return BadRequest("User ID not found in claims.");
                }

                // Convert userIdString to Guid
                var userId = new Guid(userIdString);

                // Retrieve user roles and associated permissions for the requested page
                var permission = await _context.PagePermissions
                    .Where(pp => pp.PageName == request.Permission && _context.UserRoles
                        .Where(ur => ur.UserId == userId)
                        .Any(ur => ur.RoleId == pp.ApplicationRoleId))
                    .Select(pp => new PagePermissionDto
                    {
                        CanView = pp.CanView,
                        CanEdit = pp.CanEdit,
                        HasFullAccess = pp.HasFullAccess
                    })
                    .FirstOrDefaultAsync();

                if (permission == null)
                {
                    // If no specific permissions found, return default permissions
                    permission = new PagePermissionDto { CanView = false, CanEdit = false, HasFullAccess = false };
                }

                Console.WriteLine($"Permission check result: View={permission.CanView}, Edit={permission.CanEdit}, FullAccess={permission.HasFullAccess}");

                return Ok(permission);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest($"Failed to retrieve user permissions: {ex.Message}");
            }
        }




    }
}

public class PermissionRequest
{
    public string Permission { get; set; } = string.Empty;
}
