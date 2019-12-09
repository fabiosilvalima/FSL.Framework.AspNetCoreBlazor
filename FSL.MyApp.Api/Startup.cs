using FSL.Framework.Web.Extensions;
using FSL.MyApp.Api.Repository;
using FSL.MyApp.Api.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FSL.MyApp.Api
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            services
                .AddApiFslFramework(Configuration)
                .Config(opt =>
                {
                    opt.AddDefaultConfiguration();
                    opt.AddJwtAuthentication();
                    opt.AddAuthorizationService<ApiAuthorizationService>();
                    opt.AddAddressRepository<ApiAddressSqlRepository>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApiFslFramework();
        }
    }
}
