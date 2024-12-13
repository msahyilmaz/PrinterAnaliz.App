using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Persistence.Repositories;
using PrinterAnaliz.Persistence.SqlContext;


namespace PrinterAnaliz.Persistence
{
    public static class Registration
    {

        public static IServiceCollection AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<SqlDbContext>(opt =>
            {
                var connStr = configuration.GetConnectionString("SqlConnectionString");
                opt.UseSqlServer(connStr, opt => { opt.EnableRetryOnFailure(); });
            });
               // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
               services.AddScoped<IUserCustomerRef, UserCustomerRefRepository>(); 
               services.AddScoped<IUserRepository, UserRepository>(); 
               services.AddScoped<IUserRolesRepository, UserRolesRepository>(); 
               services.AddScoped<IAddressRepository, AddressRepository>(); 
               services.AddScoped<ICustomerRepository, CustomerRepository>(); 
               services.AddScoped<IPrinterRepository, PrinterRepository>(); 
               services.AddScoped<IPrinterLogRepository, PrinterLogRepository>(); 
            return services;
        }
    }
}
