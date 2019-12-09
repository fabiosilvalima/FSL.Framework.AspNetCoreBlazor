using FSL.Framework.Core.Models;
using FSL.Framework.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FSL.Framework.Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public sealed class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Core.Authorization.Service.IAuthorizationService _authorizationService;

        public LoginController(
            IAuthenticationService authenticationService,
            Core.Authorization.Service.IAuthorizationService authorizationService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] LoginUser loginUser)
        {
            var authorization = await _authorizationService.AuthorizeAsync(loginUser);
            if (!authorization.Success)
            {
                return Ok(authorization);
            }

            var authentication = await _authenticationService.AuthenticateAsync(authorization.Data);
            if (!authentication.Success)
            {
                return Ok(authentication);
            }

            return Ok(authentication);
        }
    }
}
