using FSL.Framework.Core.Address.Repository;
using FSL.Framework.Core.ApiClient.Provider;
using FSL.Framework.Core.ApiClient.Service;
using FSL.Framework.Core.Authorization.Service;
using FSL.Framework.Core.Factory.Service;
using FSL.Framework.Web.ApiClient;
using FSL.Framework.Web.Configuration.Models;
using FSL.Framework.Web.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FSL.Framework.Web.Models
{
    public sealed class FslFrameworkOptions
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public FslFrameworkOptions(
            IServiceCollection services,
            IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public FslFrameworkOptions Config(
            Action<FslFrameworkOptions> opt)
        {
            opt(this);

            return this;
        }

        public FslFrameworkOptions AddDefaultConfiguration()
        {
            _services.AddConfiguration<DefaultConfiguration>(_configuration);

            return this;
        }

        public FslFrameworkOptions AddConfiguration<T>()
            where T : class
        {
            _services.AddConfiguration<T>(_configuration);

            return this;
        }

        public FslFrameworkOptions AddJwtAuthentication()
        {
            _services.AddJwtAuthentication(_configuration);

            return this;
        }

        public FslFrameworkOptions AddCookiesAuthentication()
        {
            _services.AddCookiesAuthentication(_configuration);

            return this;
        }

        public FslFrameworkOptions AddApiClientService<T>()
            where T : class, IApiClientService
        {
            _services.AddSingleton<IApiClientService, T>();

            return this;
        }

        public FslFrameworkOptions AddApiClientProvider<T>()
            where T : class, IApiClientProvider
        {
            _services.AddTransient<IApiClientProvider, T>();

            return this;
        }

        public FslFrameworkOptions AddApiClient()
        {
            _services.AddApiClient();

            return this;
        }

        public FslFrameworkOptions AddFactoryService<T>()
            where T : class,IFactoryService
        {
            _services.AddSingleton<IFactoryService, T>();

            return this;
        }

        public FslFrameworkOptions AddAuthorizationService<T>()
            where T : class, IAuthorizationService
        {
            _services.AddSingleton<IAuthorizationService, T>();

            return this;
        }

        public FslFrameworkOptions AddAddressRepository<T>()
            where T : class, IAddressRepository
        {
            _services.AddSingleton<IAddressRepository, T>();

            return this;
        }
    }
}
