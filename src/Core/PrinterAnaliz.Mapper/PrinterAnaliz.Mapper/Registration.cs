using Microsoft.Extensions.DependencyInjection;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Mapper.GenericAutoMappers;

namespace PrinterAnaliz.Mapper
{
    public static class Registration
    {

        public static IServiceCollection AddGenericMapperRegistration(this IServiceCollection services)
        {
            services.AddSingleton<IGenericAutoMapper, GenericAutoMapper>();
            return services;
        }
    }
}
