using AssetManagement.Dto.Auth;
using AssetManagement.Dto.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParameters loginParameters);
        Task Register(RegisterParameters registerParameters);
        Task Logout();
        Task<UserInfo> GetUserInfo();
        Task<List<UserViewModel>> GetUsers();
        Task UpdateUserRole(UserViewModel userViewModel);
        Task<int> GetUserCount();
        Task<List<RoleViewModel>> GetRoles();

        Task ChangePassword(ResetPassword resetPassword);
        Task UpdateUserDetails(UserDetailsUpdateParameters updateParameters);
        Task RequestPasswordResetByEmail(ResetPasswordByAdmin Parameters);
        Task<(bool, string)> ForgetPassword(ForgetPasswordRequest param);
        Task<bool> ResetPassword(ResetPasswordRequest param);
        //Task<bool> HasPermission(string permission);
        Task<PagePermissionDto> GetPagePermissions(string permission);
    }
}
