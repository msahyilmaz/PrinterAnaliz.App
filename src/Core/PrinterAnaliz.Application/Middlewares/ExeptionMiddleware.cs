using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrinterAnaliz.Application.Exceptions;
using PrinterAnaliz.Application.Extensions;
using SendGrid.Helpers.Errors.Model;

namespace PrinterAnaliz.Application.Middlewares
{
    public class ExeptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }
        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = GetStatusCode(exception);
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            var errors = new List<string>();
            if (exception.GetType() == typeof(ValidationException))
            {
                var validationExceptionErrors = (ValidationException)exception;
                errors = GetExceptionMessage(validationExceptionErrors);
            }
            else
            {
                errors = GetExceptionMessage(exception);

            }
            var responseModel = GenericResponseModel<List<string>>.Fail(errors, statusCode);
            var result = JsonConvert.SerializeObject(responseModel);
            return httpContext.Response.WriteAsync(result);
        }
        private static List<string> GetExceptionMessage(Exception ex)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var errors = env == "Developments" ? ex.GetAllExceptionAsString() : ex.GetAllExceptionMessages();
            return errors.ToList();
        }
        private static List<string> GetExceptionMessage(ValidationException ex)
        {
            var errors = ex.GetAllExceptionMessages();
            return errors.ToList();
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
