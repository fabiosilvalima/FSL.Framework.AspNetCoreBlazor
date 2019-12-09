using FSL.Framework.Web.Authentication.Models;
using Microsoft.AspNetCore.Authorization;

namespace FSL.Framework.Web.Filters
{
    public sealed class BearerAuthorizeAttribute : AuthorizeAttribute
    {
        public BearerAuthorizeAttribute() : base(TokenConfiguration.Policy)
        {

        }
    }
}
