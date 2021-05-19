using Company.Customers.Application.AutoMappers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Company.Customers.API.Configurations.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(
                typeof(DtoToResponseMappingProfile),
                typeof(RequestToDtoMappingProfile),
                typeof(DtoToEntityMappingProfile));
        }
    }
}
