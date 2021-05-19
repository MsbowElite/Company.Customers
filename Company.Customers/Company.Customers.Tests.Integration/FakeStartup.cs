using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Company.Customers.API;
using Company.Customers.Infra.Data.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Customers.Tests.Integration
{
    public class FakeStartup
    {
        public Startup _startup;
        public static Action<IServiceCollection> MockService;
        public FakeStartup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _startup = new Startup(configuration, env);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _startup.Configure(app, env);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Environment.SetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING, "CONNECTION_STRING");
            Environment.SetEnvironmentVariable(DataBaseConstants.DATABASE_NAME, "DATABASE_NAME");
            _startup.ConfigureServices(services);
            services.AddMvc()
                    .AddApplicationPart(typeof(Startup).Assembly);
            services.AddSingleton<IMongoDatabase>(x => null);

            if (MockService is object)
                MockService(services);
            MockService = null;

        }
    }
}
