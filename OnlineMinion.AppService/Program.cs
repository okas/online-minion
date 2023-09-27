using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.AppService.Swagger;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Init;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

const string connectionString = "name=ConnectionStrings:DefaultConnection";

var webAppBuilder = WebApplication.CreateBuilder(args);

#region Container setup

var confManager = webAppBuilder.Configuration;
var services = webAppBuilder.Services;

webAppBuilder.Services.AddDbContext<OnlineMinionDbContext>(
    builder => builder.UseSqlServer(connectionString)
);

services.AddRestApi(webAppBuilder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.Configure<SwaggerOptions>(confManager.GetSection(nameof(SwaggerOptions)));

services
    .Configure<SwaggerGeneratorOptions>(confManager.GetSection(nameof(SwaggerGeneratorOptions)))
    .Configure<SchemaGeneratorOptions>(confManager.GetSection(nameof(SchemaGeneratorOptions)))
    .Configure<SwaggerGenOptions>(confManager.GetSection(nameof(SwaggerGenOptions)))
    .ConfigureOptions<SwaggerGenOptionsConfigurator>()
    .AddSwaggerGen()
    .AddFluentValidationRulesToSwagger(
        options => options.ValidatorSearch = new()
        {
            IsOneValidatorForType = false,
            SearchBaseTypeValidators =
                false, // This aligns with the behavior of default MS DI  Service resolution works!
            // Enforces to use only one validator per type and combine them using 'Include' rules.
        }
    );

if (webAppBuilder.Environment.IsDevelopment())
{
    services.AddDatabaseDeveloperPageExceptionFilter();

    services
        .Configure<SwaggerUIOptions>(confManager.GetSection(nameof(SwaggerUIOptions)))
        .ConfigureOptions<SwaggerUIOptionsConfigurator>();
}

#endregion

var app = webAppBuilder.Build();

#region HTTP request pipeline

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    // Required to serve custom CSS for SwaggerUI.
    app.UseStaticFiles();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRestApi();

#endregion

await app.RunAsync().ConfigureAwait(false);
