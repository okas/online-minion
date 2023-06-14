using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Swagger;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.HttpRequestPipeline;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var webAppBuilder = WebApplication.CreateBuilder(args);

#region Container setup

var confManager = webAppBuilder.Configuration;

var services = webAppBuilder.Services;

webAppBuilder.Services.AddDbContext<OnlineMinionDbContext>(
    builder => builder.UseSqlServer("name=ConnectionStrings:DefaultConnection")
);

services.AddRestApi(webAppBuilder.Configuration);

if (webAppBuilder.Environment.IsDevelopment())
{
    services.AddDatabaseDeveloperPageExceptionFilter();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.Configure<SwaggerOptions>(confManager.GetSection(nameof(SwaggerOptions)));

    services
        .Configure<SwaggerGeneratorOptions>(confManager.GetSection(nameof(SwaggerGeneratorOptions)))
        .Configure<SchemaGeneratorOptions>(confManager.GetSection(nameof(SchemaGeneratorOptions)))
        .Configure<SwaggerGenOptions>(confManager.GetSection(nameof(SwaggerGenOptions)))
        .ConfigureOptions<SwaggerGenOptionsConfigurator>()
        .AddFluentValidationRulesToSwagger();

    services
        .Configure<SwaggerUIOptions>(confManager.GetSection(nameof(SwaggerUIOptions)))
        .ConfigureOptions<SwaggerUIOptionsConfigurator>();

    services.AddSwaggerGen();
}

#endregion

var app = webAppBuilder.Build();

#region HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRestApi();

#endregion

await app.RunAsync().ConfigureAwait(false);
