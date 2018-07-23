using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
using Microex.All.EntityFramework;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SsoExample.Data;
using Microex.All.Extensions;

namespace SsoExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .EnsurePredefinedIdentityServerConfigs<ApplicationDbContext>(context =>
                {
                    if (context.Clients.Count() == 1)
                    {
                        context.ApiResources.AddRange(SeedData.ApiResources.Select(x => x.ToEntity()));
                        context.Clients.AddRange(SeedData.Clients.Select(x => x.ToEntity()));
                        context.SaveChanges();
                    }
                })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
