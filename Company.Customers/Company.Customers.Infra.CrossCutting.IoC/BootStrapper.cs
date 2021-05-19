using Microsoft.Extensions.DependencyInjection;
using Stone.Clientes.Aplicacacao.AppService;
using Stone.Clientes.Aplicacacao.AppService.Interfaces;
using Stone.Clientes.Dominio.Repository.Interfaces;
using Stone.Clientes.Dominio.Services;
using Stone.Clientes.Dominio.Services.Interfaces;
using Stone.Clientes.Dominio.Validations;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils.Configuracoes;
using Stone.Clientes.Infra.Data.MongoDb;
using Stone.Clientes.Infra.Data.Repository;
using System;

namespace Stone.Clientes.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>()
                    .AddScoped<IClienteAppService, ClienteAppService>()
                    .RegisterValidations()
                    .AddRepository();     
        }

        private static IServiceCollection AddUtils(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
                VariaveisDeAmbienteLocal.CarregarVariaveis();
            return services;

        }
        private static IServiceCollection RegisterValidations(this IServiceCollection services)
        {
            return services.AddSingleton<IClienteValidation, ClienteValidation>()
                           .AddSingleton<ICpfValidation,CpfValidation>();
        }

        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var db = MongoDatabaseProvider.GetDatabase(Environment.GetEnvironmentVariable("CONNECTION_STRING"),
                                                           Environment.GetEnvironmentVariable("DATABASE_NAME"));
                return db;
            });

            services.AddScoped<IClienteWriterRepository, ClienteWriterRepository>();
            return services;
        }

    }
}
