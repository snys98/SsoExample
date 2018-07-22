using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microex.All.IdentityServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace SsoExample.Client2
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration,IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthorization(options =>
                options.DefaultPolicy = new AuthorizationPolicy(
                    new[]
                    {
                        new AssertionRequirement(context => context.User.HasClaim(x => x.Type == JwtClaimTypes.Subject))
                    }, new[] { CookieAuthenticationDefaults.AuthenticationScheme }));
            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                })
                .AddCookie(options =>
                {
                    options.ForwardChallenge = OpenIdConnectDefaults.AuthenticationScheme;
                    options.Cookie.Name = "client2";
                })
                .AddOpenIdConnect(options =>
                {
                    if (_env.IsDevelopment())
                    {
                        IdentityModelEventSource.ShowPII = true;
                    }
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:44300";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "client2";
                    options.ClientSecret = "client2";
                    options.ResponseType = OidcConstants.ResponseTypes.CodeIdTokenToken;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("api");
                    options.SignedOutRedirectUri = "/home/privacy";
                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                    //options.Events = new OpenIdConnectEvents()
                    //{
                    //    on
                    //};
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(options=>options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
