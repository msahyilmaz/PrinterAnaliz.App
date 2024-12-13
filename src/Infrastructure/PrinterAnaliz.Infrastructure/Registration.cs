using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Infrastructure.Tokens;
using System.Text;

namespace PrinterAnaliz.Infrastructure
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configration)
        {
            services.Configure<TokenSettings>(configration.GetSection("JwtConfig"));
            services.AddTransient<ITokenService, TokenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(opt => { 
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt => { 
                opt.SaveToken = true;
                opt.TokenValidationParameters =  new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{configration["JwtConfig:Secret"]}{DateTime.Now.ToString("yyyy-MM-dd")}")),
                    ValidateLifetime = true,
                    ValidIssuer = configration["JwtConfig:Issuer"],
                    ValidAudience = configration["JwtConfig:Audience"],
                    ClockSkew = TimeSpan.Zero,
                };
              /*  opt.Events = new JwtBearerEvents() {
                    OnChallenge = context =>
                    {
                        // Skip the default logic.
                        context.HandleResponse();
                        var payload = GenericResponseModel<string>.Fail("Bu işlem için yetkiniz bulunmamaktadır.", StatusCodes.Status401Unauthorized);
                        var result = JsonConvert.SerializeObject(payload);
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsync(result);
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var payload = GenericResponseModel<string>.Fail("Bu işlem için yetkiniz bulunmamaktadır.", StatusCodes.Status401Unauthorized);
                        var result = JsonConvert.SerializeObject(payload);
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsync(result);
                    } 

                };*/
              
            });
            return services;
        }
    }
}
