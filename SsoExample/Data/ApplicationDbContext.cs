using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microex.All.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SsoExample.Data
{
    public class ApplicationDbContext : IdentityServerDbContext,
        IConfigurationDbContext, IPersistedGrantDbContext
    {
        private IConfiguration _configuration;

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}
        public ApplicationDbContext(DbContextOptions options,
            ConfigurationStoreOptions configurationStoreOptions, OperationalStoreOptions operationalStoreOptions,
            IConfiguration configuration) : base(options, configurationStoreOptions, operationalStoreOptions)
        {
            _configuration = configuration;
        }
    }
}
