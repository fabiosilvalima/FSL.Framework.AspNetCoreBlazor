using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Core.Models;
using FSL.Framework.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FSL.Framework.Web.Authentication.Service
{
    public sealed class IdentityLoggedUserService : ILoggedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityLoggedUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResult<T>> GetLoggedUserAsync<T>()
            where T : class, IUser
        {
            var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var result = new BaseResult<T>();

            if (identity?.IsAuthenticated ?? false)
            {
                result.Data = identity?.FindFirst("Data")?.Value.FromJson<T>();
                result.Success = result.Data != null;
            }

            return await Task.FromResult(result);
        }
    }
}
