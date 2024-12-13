using Microsoft.OpenApi.Models;
using PrinterAnaliz.Application;
using PrinterAnaliz.Application.Middlewares;
using PrinterAnaliz.Infrastructure;
using PrinterAnaliz.Mapper;
using PrinterAnaliz.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => {

    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Printer Analiz Web Api", Version = "v1", Description = "Printer Analiz Swagger Clinet" });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "'Bearer' yazýp boþluk býraktýktan sonra Token bilgisini girebilirsiniz. \r\n\r\n Örneðin:  \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""

    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
            },
            Array.Empty<string>()
        }
    }); 
});



var env = builder.Environment;
    builder.Configuration.SetBasePath(env.ContentRootPath).
    AddJsonFile("appsettings.json",optional:false)
   .AddJsonFile($"appsettings.{env.EnvironmentName}.json",optional:true);

builder.Services.AddPersistenceRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddGenericMapperRegistration();
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigrueHandlingMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
