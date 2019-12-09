using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Core.Service;
using FSL.Framework.Web.Authentication.Models;
using FSL.Framework.Web.Authentication.Service;
using FSL.Framework.Web.Models;
using FSL.Framework.Web.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FSL.Framework.Web.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var token = new TokenConfiguration();
            services.AddConfiguration(configuration, token);
            
            var siginingConfiguration = new SigningConfiguration();
            services.AddSingleton(siginingConfiguration);

            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthenticationService, JwtIdentityAuthenticationService>();
            services.AddSingleton<ILoggedUserService, IdentityLoggedUserService>();

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters.IssuerSigningKey = siginingConfiguration.Key;
                opt.TokenValidationParameters.ValidAudience = token.ValidAudience; // dynamic
                opt.TokenValidationParameters.ValidIssuer = token.ValidIssuer;  // dynamic
                opt.TokenValidationParameters.ValidateIssuerSigningKey = token.ValidateIssuerSigningKey;  // dynamic
                opt.TokenValidationParameters.ValidateLifetime = token.ValidateLifetime;  // dynamic
                opt.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(TokenConfiguration.Policy, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }

        public static IServiceCollection AddCookiesAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddConfiguration<CookiesConfiguration>(configuration);
            services.AddSingleton<IAuthenticationService, CookiesIdentityAuthenticationService>();
            services.AddSingleton<ILoggedUserService, IdentityLoggedUserService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services
                .AddAuthentication(options =>
                {
                    var scheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.DefaultAuthenticateScheme = scheme;
                    options.DefaultSignInScheme = scheme;
                    options.DefaultChallengeScheme = scheme;
                })
                .AddCookie(opt =>
                {
                    opt.Cookie.Name = configuration.GetValue<string>($"{typeof(CookiesConfiguration).Name}:CookieName");
                });

            return services;
        }
        
        public static IApplicationBuilder UseCookiesAuthentication(
            this IApplicationBuilder app)
        {
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
            };

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
