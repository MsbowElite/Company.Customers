using HealthChecks.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Company.Customers.Application.AppService;
using Company.Customers.Application.AppService.Interfaces;
using Company.Customers.Domain.Repository.Interfaces;
using Company.Customers.Domain.Services;
using Company.Customers.Domain.Services.Interfaces;
using Company.Customers.Domain.Validations;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils.Configuracoes;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils.Masks;
using Company.Customers.Infra.Data.Constants;
using Company.Customers.Infra.Data.MongoDb;
using Company.Customers.Infra.Data.MongoDb.Configurations;
using Company.Customers.Infra.Data.MongoDb.Configurations.Interfaces;
using Company.Customers.Infra.Data.Query;
using Company.Customers.Infra.Data.Repository;
using System;
using System.Linq;
using System.Net.Mime;

namespace Company.Customers.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>()
                    .AddScoped<ICustomerAppService, CustomerAppService>()
                    .AddValidations()
                    .AddRepository();

            return services;
        }

        public static IServiceCollection AddHealthChecksApiCustomer(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddMongoDb(mongodbConnectionString: Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING), 
                                                         Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME),
                                                         HealthStatus.Unhealthy);
            return services;
        }

        public static void UseHealthChecksApiCustomer(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/hc",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                status = report.Status.ToString(),
                                errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
        }

        private static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerValidation, CustomerValidation>()
                    .AddSingleton<ICpfValidation, CpfValidation>();
            return services;
        }

        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var db = MongoDatabaseProvider.GetDatabase(Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING),
                                                           Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME));
                return db;
            });


            services.AddSingleton<ICustomerQueryRepositoryConfiguration, CustomerQueryRepositoryConfiguration>(x =>
                                    new CustomerQueryRepositoryConfiguration(int.Parse(Environment.GetEnvironmentVariable(DataBaseConstants.TAMANHO_PAGINACAO) ?? "0" )));


            services.AddScoped<ICustomerWriterRepository, CustomerWriterRepository>();
            services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();

            return services;
        }

        public static IServiceCollection AddUtil(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
                EnvironmentVariables.CarregarVariaveis();

            services.AddSingleton<ICpfMask, CpfMask>();

            return services;
        }

    }
}
