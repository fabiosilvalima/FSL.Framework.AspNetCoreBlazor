using FSL.Framework.Web.Authentication.Models;
using FSL.Framework.Core.Extensions;
using FSL.Framework.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FSL.Framework.Web.Authentication.Service
{
    public sealed class CookiesIdentityAuthenticationService : Core.Service.IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CookiesConfiguration _configuration;

        public CookiesIdentityAuthenticationService(
            IHttpContextAccessor httpContextAccessor,
            CookiesConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(
            IUser user)
        {
            await LogoutAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Data", user.ToJson())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                GetAuthenticationSchema());

            var created = DateTime.UtcNow;
            var expiration = created + TimeSpan.FromSeconds(_configuration.ExpirationInSeconds);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = expiration
            };

            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            var result = new AuthenticationResult
            {
                Success = true,
                Authenticated = true,
                Created = created.ToString(dateFormat),
                Expiration = expiration.ToString(dateFormat),
                Message = "OK",
                Data = new CookiesAuthentication
                {
                    ClaimsIdentity = claimsIdentity,
                    AuthProperties = authProperties
                }
            };

            var cookieAuthentication = result.Data as CookiesAuthentication;
            cookieAuthentication.AuthProperties.RedirectUri = _httpContextAccessor.HttpContext.Request.Host.Value;

            await LogOnAsync(
                cookieAuthentication.ClaimsIdentity,
                cookieAuthentication.AuthProperties);

            return result;
        }

        public string GetAuthenticationSchema()
        {
            return CookieAuthenticationDefaults.AuthenticationScheme;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(GetAuthenticationSchema());
            }
            catch
            {

            }
        }

        private async Task LogOnAsync(
            ClaimsIdentity claimsIdentity,
            AuthenticationProperties authProperties)
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignInAsync(
                    GetAuthenticationSchema(),
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
            catch
            {

            }
        }
    }
}
