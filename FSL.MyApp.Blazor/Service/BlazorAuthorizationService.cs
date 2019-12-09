using FSL.Framework.Core.ApiClient.Service;
using FSL.Framework.Core.Authorization.Service;
using FSL.Framework.Core.Extensions;
using FSL.Framework.Core.Models;
using FSL.MyApp.Blazor.Models;
using System;
using System.Threading.Tasks;

namespace FSL.MyApp.Blazor.Service
{
    public sealed class BlazorAuthorizationService : IAuthorizationService
    {
        private readonly IApiClientService _apiClientService;

        public BlazorAuthorizationService(
            IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<BaseResult<IUser>> AuthorizeAsync(
            LoginUser loginUser)
        {
            var result = new BaseResult<IUser>();

            try
            {
                var instance = await _apiClientService.CreateInstanceAsync();

                var login = await instance.PostAsync<AuthenticationResult>(
                    "login",
                    loginUser);

                if (login.Success
                    && login.Data.Authenticated)
                {
                    var user = await instance
                        .UseAuthenticationBearer(login.Data.AccessToken)
                        .GetAsync<MyLoggedUser>("user");

                    user.Data.AccessToken = login.Data.AccessToken;

                    result.Data = user.Data;
                    result.Success = result.Data.IsNotNull();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }

            return result;
        }
    }
}
