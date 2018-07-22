using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microex.All.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace SsoExample.Client1
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
                //修改默认的authorize特性的policy至需要sub(禁止clientcredentials登陆)
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
                    //因为前面设置了policy,所以不符合policy的需要challenge,不能使用默认的cookie身份验证的challenge
                    options.ForwardChallenge = OpenIdConnectDefaults.AuthenticationScheme;
                    options.Cookie.Name = "client1";
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
                    options.ClientId = "client1";
                    options.ClientSecret = "client1";
                    options.ResponseType = OidcConstants.ResponseTypes.CodeIdTokenToken;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("api");
                    options.Scope.Add("offline_access");
                    options.SignedOutRedirectUri = "/home/privacy";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                    options.SaveTokens = true;
                });
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

