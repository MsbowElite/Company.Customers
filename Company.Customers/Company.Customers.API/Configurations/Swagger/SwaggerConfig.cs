using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Company.Customers.API.Configurations.Swagger
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Customers API",
                        Version = "v1",
                        Description = "API REST ASP.NET Core 5",
                        Contact = new OpenApiContact
                        {
                            Name = "Lucas Rodrigues Sena",
                            Email = "msbowelite@gmail.com"
                        }
                    });
                c.EnableAnnotations();
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI( options =>options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Customers API"));
        }
    }
}
