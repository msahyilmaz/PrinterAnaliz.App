using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrinterAnaliz.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Middlewares
{
    public class AuthorizeCorrectlyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            await next(httpContext);
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                var payload = GenericResponseModel<string>.Fail("Bu işlem için oturum açmanız gerekmektedir.", StatusCodes.Status401Unauthorized);
                var result = JsonConvert.SerializeObject(payload);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
               await httpContext.Response.WriteAsync(result);
            }
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                var payload = GenericResponseModel<string>.Fail("Bu işlem için yetkiniz bulunmamaktadır.", StatusCodes.Status403Forbidden);
                var result = JsonConvert.SerializeObject(payload);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync(result);
            }


        }

    }
}
