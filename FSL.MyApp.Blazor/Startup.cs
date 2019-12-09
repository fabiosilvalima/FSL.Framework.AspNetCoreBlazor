using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FSL.MyApp.Blazor.Data;
using FSL.Framework.Web.Extensions;
using FSL.MyApp.Blazor.Service;
using FSL.MyApp.Blazor.Repository;
using FSL.MyApp.Blazor.Models;

namespace FSL.MyApp.Blazor
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(
            IServiceCollection services)
        {
            services
                .AddBlazorFslFramework(Configuration)
                .Config(opt =>
                {
                    opt.AddDefaultConfiguration();
                    opt.AddConfiguration<MyBlazorConfiguration>();
                    opt.AddCookiesAuthentication();
                    opt.AddAuthorizationService<BlazorAuthorizationService>();    
                    opt.AddAddressRepository<BlazorAddressRepository>();
                    opt.AddFactoryService<BlazorFactoryService>();
                    opt.AddApiClientService<BlazorApiClientService>();
                });

            services.AddSingleton<WeatherForecastService>();
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
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFslFramework();
        }
    }
}
