using FSL.Framework.Core.Cryptography.Models;
using FSL.Framework.Web.ApiClient;
using FSL.Framework.Web.Caching;
using FSL.Framework.Web.Cryptography;
using FSL.Framework.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSL.Framework.Web.Extensions
{
    public static class FslFrameworkExtension
    {
        public static FslFrameworkOptions AddApiFslFramework(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers();
            services.AddFslFramework(configuration);

            var options = new FslFrameworkOptions(
                services, 
                configuration);
            
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });

            return options;
        }

        public static FslFrameworkOptions AddBlazorFslFramework(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddFslFramework(configuration);

            var options = new FslFrameworkOptions(
                services,
                configuration);

            return options;
        }

        public static IApplicationBuilder UseApiFslFramework(
            this IApplicationBuilder app)
        {
            app.UseCors(option => option.AllowAnyOrigin());
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        public static IApplicationBuilder UseBlazorFslFramework(
            this IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            return app;
        }

        private static IServiceCollection AddFslFramework(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.UseCaching();
            services.AddConfiguration<CryptographyConfiguration>(configuration);
            services.UseCryptography();
            services.AddApiClient();

            return services;
        }
    }
}
