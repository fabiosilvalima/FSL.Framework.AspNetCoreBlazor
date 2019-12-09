using FSL.Framework.Core.Cryptography.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace FSL.Framework.Web.Cryptography
{
    public static class CryptographyExtension
    {
        public static IServiceCollection UseCryptography(
            this IServiceCollection services)
        {
            services.AddSingleton<ICryptographyProvider, DESKeyCryptographyProvider>();

            return services;
        }
    }
}
