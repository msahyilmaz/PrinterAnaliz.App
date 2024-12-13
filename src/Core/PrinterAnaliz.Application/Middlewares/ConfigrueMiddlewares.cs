using Microsoft.AspNetCore.Builder;

namespace PrinterAnaliz.Application.Middlewares
{
    public static class ConfigrueMiddlewares
    {
        public static void ConfigrueHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExeptionMiddleware>();
            app.UseMiddleware<AuthorizeCorrectlyMiddleware>();
        }
    }
}
