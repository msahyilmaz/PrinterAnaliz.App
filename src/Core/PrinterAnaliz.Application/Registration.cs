using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PrinterAnaliz.Application.Beheviors;
using PrinterAnaliz.Application.Middlewares;
using System.Globalization;
using System.Reflection;

namespace PrinterAnaliz.Application
{
    public static class Registration
    {

        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddTransient<AuthorizeCorrectlyMiddleware>();
            services.AddTransient<ExeptionMiddleware>();
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr-TR");
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehevior<,>));
        } 
    }
}
