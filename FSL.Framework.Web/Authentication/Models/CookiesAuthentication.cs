using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FSL.Framework.Web.Authentication.Models
{
    public sealed class CookiesAuthentication
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public AuthenticationProperties AuthProperties { get; set; }
    }
}
