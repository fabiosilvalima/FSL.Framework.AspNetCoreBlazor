using FSL.Framework.Core.ApiClient.Provider;
using FSL.Framework.Core.ApiClient.Service;
using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Core.Factory.Service;
using FSL.MyApp.Blazor.Models;
using System.Threading.Tasks;

namespace FSL.MyApp.Blazor.Service
{
    public sealed class BlazorApiClientService : IApiClientService
    {
        private readonly MyBlazorConfiguration _configuration;
        private readonly IFactoryService _factoryService;
        private readonly ILoggedUserService _loggedUserService;

        public BlazorApiClientService(
            MyBlazorConfiguration configuration,
            IFactoryService factoryService,
            ILoggedUserService loggedUserService)
        {
            _configuration = configuration;
            _factoryService = factoryService;
            _loggedUserService = loggedUserService;
        }

        public async Task<IApiClientProvider> CreateInstanceAsync()
        {
            var instance = _factoryService
               .InstanceOf<IApiClientProvider>()
               .UseJsonContentType()
               .UseBaseUrl(_configuration.ApiUrl);

            var user = await _loggedUserService.GetLoggedUserAsync<MyLoggedUser>();

            instance.UseAuthenticationBearer(user?.Data?.AccessToken);

            return instance;
        }
    }
}
