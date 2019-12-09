using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FSL.Framework.Core.Extensions;
using FSL.Framework.Core.Models;
using FSL.Framework.Core.Service;
using FSL.Framework.Web.Authentication.Models;
using FSL.Framework.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FSL.Framework.Web.Service
{
    public sealed class JwtIdentityAuthenticationService : IAuthenticationService
    {
        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public JwtIdentityAuthenticationService(
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration)
        {
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(
            IUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id),
                new Claim("Name", user.Name),
                new Claim("Id", user.Id),
                new Claim("Data", user.ToJson())
            };

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Id, "Login"),
                claims);

            var created = DateTime.UtcNow;
            var expiration = created + TimeSpan.FromSeconds(_tokenConfiguration.ExpirationInSeconds); // dynamic
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.ValidIssuer, // dynamic
                Audience = _tokenConfiguration.ValidAudience, // dynamic
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = created,
                Expires = expiration
            });

            var dateFormat = "yyyy-MM-dd HH:mm:ss";
            var result = new AuthenticationResult
            {
                Success = true,
                Authenticated = true,
                Created = created.ToString(dateFormat),
                Expiration = expiration.ToString(dateFormat),
                AccessToken = handler.WriteToken(securityToken),
                Message = "OK"
            };

            return await Task.FromResult(result);
        }

        public string GetAuthenticationSchema()
        {
            return JwtBearerDefaults.AuthenticationScheme;
        }

        public async Task LogoutAsync()
        {
            await Task.FromResult("");
        }
    }
}
