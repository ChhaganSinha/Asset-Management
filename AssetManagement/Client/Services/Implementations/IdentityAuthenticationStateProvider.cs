using AssetManagement.Client.Services.Contracts;
using AssetManagement.Dto.Auth;
using AssetManagement.Dto.RBAC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetManagement.Client.States
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private UserInfo _userInfoCache;
        private readonly IAuthorizeApi _authorizeApi;

        public IdentityAuthenticationStateProvider(IAuthorizeApi authorizeApi)
        {
            this._authorizeApi = authorizeApi;
        }

        public async Task Login(LoginParameters loginParameters)
        {
            await _authorizeApi.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterParameters registerParameters)
        {
            await _authorizeApi.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await _authorizeApi.Logout();
            _userInfoCache = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<UserInfo> GetUserInfo()
        {
            if (_userInfoCache != null && _userInfoCache.IsAuthenticated) return _userInfoCache;
            _userInfoCache = await _authorizeApi.GetUserInfo();
            return _userInfoCache;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetUserInfo();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }.Concat(userInfo.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        public async Task<int> GetUserCount()
        {
            return await _authorizeApi.GetUserCount();
        }
        public async Task<List<RoleViewModel>> GetRoles()
        {
            return await _authorizeApi.GetRoles();
        }
        //////
        ///
        public async Task<(bool, string)> ChangePassword(ResetPassword resetPassword)
        {
            try
            {
                await _authorizeApi.ChangePassword(resetPassword);
                return (true, "Success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> UpdateUserDetails(UserDetailsUpdateParameters updateParameters)
        {
            try
            {
                await _authorizeApi.UpdateUserDetails(updateParameters);
                return (true, "Success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> RequestPasswordResetByEmail(ResetPasswordByAdmin Parameters)
        {
            try
            {
                await _authorizeApi.RequestPasswordResetByEmail(Parameters);
                return (true, "Success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> ForgetPassword(ForgetPasswordRequest request)
        {
            var success = await _authorizeApi.ForgetPassword(request);
            var message = success.Item1 ? "We have sent a password reset link to you respective email address, please check and follow the instructions" : success.Item2;
            return await Task.FromResult((success.Item1, message));
        }

        public async Task<(bool, string)> ResetPassword(ResetPasswordRequest request)
        {
            var success = await _authorizeApi.ResetPassword(request);
            var message = success ? "Your password has been successfully reset." : "There was a problem reseting your password, please try again";
            return await Task.FromResult((success, message));
        }
    }
}
