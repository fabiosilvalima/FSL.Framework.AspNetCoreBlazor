using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Web.Filters;
using FSL.MyApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FSL.MyApp.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [BearerAuthorize]
    public sealed class UserController : ControllerBase
    {
        private readonly ILoggedUserService _loggedUserService;

        public UserController(
            ILoggedUserService loggedUserService)
        {
            _loggedUserService = loggedUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _loggedUserService.GetLoggedUserAsync<MyLoggedUser>();

            return Ok(user);
        }
    }
}
