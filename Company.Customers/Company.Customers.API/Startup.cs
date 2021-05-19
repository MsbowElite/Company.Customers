using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Company.Customers.API.Configurations.Swagger;
using Company.Customers.API.Middlewares;
using Company.Customers.Infra.CrossCutting.IoC;

namespace Company.Customers.API
{
    public class Startup
    {
        private readonly bool isDevelopment;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            isDevelopment = env.IsDevelopment();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGlobalExceptionHandlerMiddleware();
            services.AddSwaggerConfiguration();
            services.RegisterServices()
                    .AddUtil(isDevelopment)
                    .AddHealthChecksApiCustomer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalExceptionHandlerMiddleware();

            app.UseSwaggerSetup();

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecksApiCustomer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
