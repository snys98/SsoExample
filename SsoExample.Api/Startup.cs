using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace SsoExample.Api
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
            services.AddMvc();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(
                        options =>
                        {
                            if (_env.IsDevelopment())
                            {
                                IdentityModelEventSource.ShowPII = true;
                            }
                            options.Authority = "https://localhost:44300";
                            options.ApiName = "api";
                            options.ApiSecret = "api";
                            options.RequireHttpsMetadata = false;
                            options.JwtBearerEvents = new JwtBearerEvents
                            {
                                OnChallenge = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnTokenValidated = ContextBoundObject =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnMessageReceived = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnAuthenticationFailed = context =>
                                {
                                    var te = context.Exception;
                                    return Task.CompletedTask;
                                }
                            };
                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
